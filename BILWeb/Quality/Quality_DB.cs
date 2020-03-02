//************************************************************
//******************************作者：方颖*********************
//******************************时间：2017/6/27 11:06:15*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using BILBasic.Common;
using BILBasic.User;
using BILWeb.Stock;
using System.Data;

namespace BILWeb.Quality
{
    public partial class T_Quality_DB : BILBasic.Basing.Factory.Base_DB<T_QualityInfo>
    {

        /// <summary>
        /// 添加t_quality
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_QualityInfo t_quality)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();
        }

        protected override List<string> GetSaveSql(BILBasic.User.UserModel user, ref T_QualityInfo model)
        {
            throw new NotImplementedException();
        }

        

        protected override List<string> GetUpdateModelListSql(UserModel user, List<T_QualityInfo> modelList)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;
            foreach (var item in modelList) 
            {
                strSql = string.Format("update t_Quality a set a.Quanuserno = '{0}' where id = '{1}'", item.StrQuanUserNo, item.ID);
                lstSql.Add(strSql);
            }

            return lstSql;
        }

        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_QualityInfo ToModel(IDataReader reader)
        {
            T_QualityInfo t_quality = new T_QualityInfo();

            t_quality.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_quality.ErpInVoucherNo = (string)dbFactory.ToModelValue(reader, "ERPVOUCHERNO");
            t_quality.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "STRONGHOLDCODE");
            t_quality.StrongHoldName = (string)dbFactory.ToModelValue(reader, "STRONGHOLDNAME");
            t_quality.CompanyCode = (string)dbFactory.ToModelValue(reader, "COMPANYCODE");
            t_quality.ERPCreater = (string)dbFactory.ToModelValue(reader, "ERPCREATER");
            t_quality.VouDate = (DateTime?)dbFactory.ToModelValue(reader, "VOUDATE");
            t_quality.VouUser = (string)dbFactory.ToModelValue(reader, "VOUUSER");
            t_quality.ERPStatus = dbFactory.ToModelValue(reader, "ERPSTATUS").ToDBString();
            t_quality.ERPNote = (string)dbFactory.ToModelValue(reader, "ERPNOTE");
            t_quality.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_quality.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_quality.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_quality.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_quality.Status = dbFactory.ToModelValue(reader, "STATUS").ToInt32();
            t_quality.TimeStamp = (DateTime?)dbFactory.ToModelValue(reader, "TIMESTAMP");
            t_quality.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToInt32();
            
            t_quality.NoticeStatus = dbFactory.ToModelValue(reader, "NOTICESTATUS").ToInt32();
            t_quality.QualityType = dbFactory.ToModelValue(reader, "QUALITYTYPE").ToInt32();
            t_quality.MaterialNo = (string)dbFactory.ToModelValue(reader, "MATERIALNO");
            t_quality.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MATERIALDESC");
            t_quality.InSQty = (decimal?)dbFactory.ToModelValue(reader, "INSQTY");
            t_quality.Unit = (string)dbFactory.ToModelValue(reader, "UNIT");
            t_quality.UnitName = (string)dbFactory.ToModelValue(reader, "UNITNAME");
            t_quality.QuanQty = (decimal?)dbFactory.ToModelValue(reader, "QUANQTY");
            t_quality.UnQuanQty = (decimal?)dbFactory.ToModelValue(reader, "UNQUANQTY");
            t_quality.DesQty = (decimal?)dbFactory.ToModelValue(reader, "DESQTY");
            t_quality.WarehouseNo = (string)dbFactory.ToModelValue(reader, "WAREHOUSENO");
            t_quality.BatchNo = (string)dbFactory.ToModelValue(reader, "BATCHNO");
            t_quality.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ErpVoucherNo");

            t_quality.ErpInVoucherNo = (string)dbFactory.ToModelValue(reader, "ErpInVoucherNo");
            t_quality.SampQty = (decimal?)dbFactory.ToModelValue(reader, "SampQty");
            t_quality.QuanUserNo = (string)dbFactory.ToModelValue(reader, "QuanUserNo");
            t_quality.StrQuanUserNo = (string)dbFactory.ToModelValue(reader, "StrQuanUserNo");

            //add by cym 
            t_quality.ERPStatusCode = (string)dbFactory.ToModelValue(reader, "ERPStatusCode");
            
            return t_quality;

        }

        protected override string GetViewName()
        {
            return "v_quality";
        }

        protected override string GetTableName()
        {
            return "T_QUALITY";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }


        protected override string GetFilterSql(UserModel user, T_QualityInfo model)
        {
            string strUserNo = string.Empty;

            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";

            if (model.Status > 0)
            {
                if (model.Status == 1 || model.Status == 2)
                {
                    strSql += strAnd;
                    strSql += "( isnull(status,1)=1 or isnull(status,1)=2 )";
                }
                else
                {
                    strSql += strAnd;
                    strSql += "isnull(status,1)= '" + model.Status + "'";
                }
            }

            if (!string.IsNullOrEmpty(model.ErpInVoucherNo))
            {
                strSql += strAnd;
                strSql += " (ERPInVoucherNo ='" + model.ErpInVoucherNo + "'  and isnull(sampqty,0) != 0 )";
            }
            

            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                if (string.IsNullOrEmpty(model.ErpInVoucherNo)) 
                {
                    strSql += strAnd;
                    strSql += " ErpVoucherNo  = '" + model.ErpVoucherNo + "'  ";
                }                
            }

            //delete by cym 2017-12-14
            if (!string.IsNullOrEmpty(model.QuanUserNo))
            {
                strUserNo = GetUserNo(user);
                strSql += strAnd;
                strSql += " QuanUserNo in (SELECT ID FROM T_USER WHERE USERNO LIKE '" + strUserNo + "%') ";
            }
            //if (!string.IsNullOrEmpty(user.QuanUserNo))
            //{
            //    //strUserNo = GetUserNo(user);
            //    strSql += strAnd;
            //    strSql += " QuanUserNo in (" + user.QuanUserNo + ") ";
            //}

            if (!string.IsNullOrEmpty(model.ERPStatus)) 
            {
                strSql += strAnd;
                strSql += "Erpstatuscode = '" + model.ERPStatusCode  + "' ";
            }

            if (!string.IsNullOrEmpty(model.MaterialNo))
            {
                strSql += strAnd;
                strSql += "MaterialNo = '" + model.MaterialNo + "' ";
            }

            strSql += strAnd;
            strSql += "Sampqty > 0 ";

            

            return strSql + " order by id desc ";
        }

        private string GetUserNo(UserModel user) 
        {
            string strUserNo = string.Empty;

            if (TOOL.RegexMatch.isExists(user.UserNo) == true)
            {
                strUserNo = user.UserNo.Substring(0, user.UserNo.Length - 1);
            }
            else
            {
                strUserNo = user.UserNo;
            }
            return strUserNo;
        }

        /// <summary>
        /// 上架扫描物料条码，获取该物料的质检状态
        /// 用来判断能不能上架
        /// </summary>
        /// <param name="strTaskNo"></param>
        /// <param name="MaterialNoID"></param>
        /// <param name="BatchNo"></param>
        /// <returns></returns>
        public int GetQualityStatusByTaskNo(string strTaskNo, int MaterialNoID, string BatchNo) 
        {
            string strSql = "select ( case when isnull(c.Quanqty,0)> 0 then 3  when isnull(c.Unquanqty,0) > 0 then 4 "+
                            "when isnull(c.Erpvoucherno,' ') = ' ' then 3 else 1 end) as IsQuality from t_task a left join t_Parameter b "+
                            "on a.Vouchertype = b.Id and b.Groupname = 'Voucher_Quality' left join t_Quality c "+
                            "on a.Erpinvoucherno = c.Erpinvoucherno where a.Taskno ='" + strTaskNo + "' and c.Materialnoid = '" + MaterialNoID + "' and c.Batchno = '" + BatchNo + "'";
            return base.GetScalarBySql(strSql).ToInt32();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTaskNo"></param>
        /// <param name="MaterialNoID"></param>
        /// <param name="BatchNo"></param>
        /// <returns></returns>
        public int GetQualitySampQtyByTaskNo(string strTaskNo, int MaterialNoID, string BatchNo)
        {
            string strSql = "select Sampqty from t_task a left join t_Parameter b " +
                            "on a.Vouchertype = b.Id and b.Groupname = 'Voucher_Quality' left join t_Quality c " +
                            "on a.Erpinvoucherno = c.Erpinvoucherno where a.Taskno ='" + strTaskNo + "' and c.Materialnoid = '" + MaterialNoID + "' and c.Batchno = '" + BatchNo + "'";
            return base.GetScalarBySql(strSql).ToInt32();
        }

        /// <summary>
        /// 在库检，更新库存仓储批物料为待检状态
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool UpadteQualityForStock(List<T_StockInfo> modelList, ref string strError) 
        {
            List<string> lstSql = new List<string>();

            foreach (var item in modelList) 
            {
                string strSql = "update t_stock a set a.Status = 1 where a.Materialnoid = '" + item.MaterialNoID + "' and a.Batchno = '" + item.BatchNo + "' and a.Warehouseid = '" + item.WareHouseID + "' and a.areaid = '"+item.AreaID+"' ";
                lstSql.Add(strSql);
            }

            return base.UpdateModelListStatusBySql(lstSql, ref strError);
        }

        /// <summary>
        /// 在库检，提交之前，看看有没有待下架的物料
        /// </summary>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public bool CheckQualityTaskDetailsID(List<T_StockInfo> modelList,ref string strError) 
        {
            string strSql = string.Empty;
            int Count = 0;
            bool bSucc = false;

            foreach (var item in modelList) 
            {
                strSql = "select count(1) from t_stock a where a.Materialnoid = '" + item.MaterialNoID + "' and a.Batchno = '" + item.BatchNo + "' and isnull(a.Taskdetailesid,0) > 0 and warehouseid = '" + item.WareHouseID + "' and Areaid = '"+item.AreaID+"' ";
                Count = base.GetScalarBySql(strSql).ToInt32();
                if (Count > 0)
                {
                    bSucc = false;
                    strError = "物料：" + item.MaterialNo + "仓库：" + item.WarehouseNo + "储位：" + item.AreaNo + "存在待下架库存！";
                    break;
                }
                else 
                {
                    bSucc = true;
                }
            }

            return bSucc;
            
        }

        public override List<T_QualityInfo> GetModelListADF(UserModel user, T_QualityInfo model)
        {
            List<T_QualityInfo> modelList = base.GetModelListADF(user, model);
            if (modelList != null && modelList.Count > 0)
            {
                if (string.IsNullOrEmpty(model.MaterialNo)) 
                {
                    modelList = modelList.Take(20).ToList();
                }
                
            }

            return modelList;

        }

    }
}
