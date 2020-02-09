//************************************************************
//******************************作者：方颖*********************
//******************************时间：2019/7/17 16:01:41*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using BILBasic.Common;
using Oracle.ManagedDataAccess.Client;
using BILBasic.User;
using System.Data;

namespace BILWeb.AdvInStock
{
    public partial class T_AdvInStockDetail_DB : BILBasic.Basing.Factory.Base_DB<T_AdvInStockDetailInfo>
    {

        /// <summary>
        /// 添加t_advinstockdetail
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_AdvInStockDetailInfo t_advinstockdetail)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(BILBasic.User.UserModel user, ref T_AdvInStockDetailInfo t_advinstockdetail)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_AdvInStockDetailInfo ToModel(IDataReader reader)
        {
            T_AdvInStockDetailInfo t_advinstockdetail = new T_AdvInStockDetailInfo();

            t_advinstockdetail.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_advinstockdetail.HeaderID = dbFactory.ToModelValue(reader, "HEADERID").ToInt32();
            t_advinstockdetail.MaterialNo = (string)dbFactory.ToModelValue(reader, "MATERIALNO");
            t_advinstockdetail.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MATERIALDESC");
            t_advinstockdetail.MaterialNoID = dbFactory.ToModelValue(reader, "MaterialNoID").ToInt32();
            t_advinstockdetail.AdvQty = dbFactory.ToModelValue(reader, "ADVQTY").ToDecimalNull();
            t_advinstockdetail.Unit = (string)dbFactory.ToModelValue(reader, "UNIT");
            t_advinstockdetail.LineStatus = dbFactory.ToModelValue(reader, "LINESTATUS").ToInt32();
            t_advinstockdetail.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_advinstockdetail.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_advinstockdetail.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_advinstockdetail.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_advinstockdetail.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToInt32();
            t_advinstockdetail.EAN = (string)dbFactory.ToModelValue(reader, "EAN");
            t_advinstockdetail.EDate = (DateTime)dbFactory.ToModelValue(reader, "EDATE");
            t_advinstockdetail.SupBatch = (string)dbFactory.ToModelValue(reader, "SUPBATCH");
            t_advinstockdetail.QualityType = dbFactory.ToModelValue(reader, "QualityType").ToInt32(); ;
            t_advinstockdetail.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ErpVoucherNo");
            t_advinstockdetail.VOUCHERNO = (string)dbFactory.ToModelValue(reader, "VOUCHERNO");
            t_advinstockdetail.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "StrongHoldCode");
            t_advinstockdetail.RowNO = (string)dbFactory.ToModelValue(reader, "RowNO");
            t_advinstockdetail.RowNODel = (string)dbFactory.ToModelValue(reader, "RowNODel");
            t_advinstockdetail.strqualitytype = (string)dbFactory.ToModelValue(reader, "strqualitytype");
            t_advinstockdetail.CompanyCode = (string)dbFactory.ToModelValue(reader, "CompanyCode");
            t_advinstockdetail.Createname = (string)dbFactory.ToModelValue(reader, "createname");
            t_advinstockdetail.IsPrint = dbFactory.ToModelValue(reader, "sumqty")==null||dbFactory.ToModelValue(reader, "sumqty").ToInt32()==0?"未打印":"已打印";
            t_advinstockdetail.WarehouseName = (string)dbFactory.ToModelValue(reader, "warehousename"); 

            return t_advinstockdetail;
        }
        public bool SaveDeleteAdvDetail(T_AdvInStockDetailInfo advDetail, out string strError)
        {
            strError = "";

            List<string> lstSql = new List<string>();
            string sql = "";
            try
            {
                sql = " select count(*) from t_instockdetail where erpvoucherno ='" + advDetail.ErpVoucherNo + "' and rowno='" + advDetail.RowNO + "' and rownodel ='" + advDetail.RowNODel + "' and  (isnull(advreceiveqty,0)-isnull(receiveqty,0))>=" + advDetail.AdvQty + "";

                int count = Convert.ToInt16(GetScalarBySql(sql));
                if (count == 0)
                {
                    strError = "该预收货不允许删除，请检查是否已收货";
                    return false;
                }

                sql = "delete T_ADVINSTOCKDETAIL where id =" + advDetail.ID;

                lstSql.Add(sql);
                sql = "update t_instockdetail set ADVRECEIVEQTY=isnull(ADVRECEIVEQTY,0)-" + advDetail.AdvQty + " where  erpvoucherno ='" + advDetail.ErpVoucherNo + "' and rowno='" + advDetail.RowNO + "' and rownodel ='" + advDetail.RowNODel + "'";
                lstSql.Add(sql);

                sql = "update  t_instock set advinstatus =2 where erpvoucherno = '" + advDetail.ErpVoucherNo + "' ";
                lstSql.Add(sql);
                int i = dbFactory.ExecuteNonQueryList(lstSql, ref strError);

                if (i > 0)
                {
                    return true;
                }
                else
                {
                    strError = "删除动作未执行成功";
                    return false;
                }
            }
            catch (Exception ex)
            {
                strError = ex.ToString();
                return false;
            }
        }
        protected override string GetViewName()
        {
            return "V_AdvInStockDetail";
        }

        protected override string GetTableName()
        {
            return "T_ADVINSTOCKDETAIL";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override string GetFilterSql(UserModel user, T_AdvInStockDetailInfo model)
        {
            string strSql = " where isnull(isDel,0) != 2  ";
            string strAnd = " and ";

            if (!Common_Func.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " ErpVoucherNo like '%" + model.ErpVoucherNo.Trim() + "%' ";
            }

            if (!Common_Func.IsNullOrEmpty(model.IsPrint))
            {
                if (model.IsPrint=="是")
                {
                    strSql += strAnd;
                    strSql += " sumqty > 0";
                }
                if (model.IsPrint == "否")
                {
                    strSql += strAnd;
                    strSql += " sumqty = 0";
                }

            }

            if (!Common_Func.IsNullOrEmpty(model.MaterialNo))
            {
                strSql += strAnd;
                strSql += " MaterialNo like '%" + model.MaterialNo.Trim() + "%' ";
            }

            if (!Common_Func.IsNullOrEmpty(model.Createname))
            {
                strSql += strAnd;
                strSql += " Createname like '%" + model.Createname.Trim() + "%' ";
            }
            if (model.DateFrom != null)
            {
                strSql += strAnd;
                strSql += " CREATETIME>=to_date('" + model.DateFrom.ToDateTime().ToString("yyyy/MM/dd") + "','YYYY/MM/DD') ";
            }
            if (model.DateTo != null)
            {
                strSql += strAnd;
                strSql += " CREATETIME<=to_date('" + model.DateTo.ToDateTime().ToString("yyyy/MM/dd") + "','YYYY/MM/DD')+1 ";
            }


            return strSql;
        }
    }
}
