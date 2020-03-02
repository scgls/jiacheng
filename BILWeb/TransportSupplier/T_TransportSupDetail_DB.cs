//************************************************************
//******************************作者：方颖*********************
//******************************时间：2019/9/9 16:47:18*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using BILBasic.User;
using BILBasic.Common;
using BILWeb.Login.User;
using System.Data;

namespace BILWeb.TransportSupplier
{
    public partial class T_TransportSupdetail_DB : BILBasic.Basing.Factory.Base_DB<T_TransportSupDetailInfo>
    {

        /// <summary>
        /// 添加t_transportsupplierdetail
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_TransportSupDetailInfo t_transportsupplierdetail)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_TransportSupDetailInfo t_transportsupplierdetail)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_TransportSupDetailInfo ToModel(IDataReader reader)
        {
            T_TransportSupDetailInfo t_transportsupplierdetail = new T_TransportSupDetailInfo();

            t_transportsupplierdetail.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_transportsupplierdetail.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ERPVOUCHERNO");
            t_transportsupplierdetail.PlateNumber = (string)dbFactory.ToModelValue(reader, "PLATENUMBER");
            t_transportsupplierdetail.Feight = (string)dbFactory.ToModelValue(reader, "FEIGHT");
            t_transportsupplierdetail.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_transportsupplierdetail.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_transportsupplierdetail.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToDecimal();
            t_transportsupplierdetail.PalletNo = (string)dbFactory.ToModelValue(reader, "PALLETNO");
            t_transportsupplierdetail.BoxCount = (string)dbFactory.ToModelValue(reader, "BOXCOUNT");
            t_transportsupplierdetail.OutBoxCount = (string)dbFactory.ToModelValue(reader, "OUTBOXCOUNT");
            t_transportsupplierdetail.CustomerName = (string)dbFactory.ToModelValue(reader, "CUSTOMERNAME");
            t_transportsupplierdetail.Remark = (string)dbFactory.ToModelValue(reader, "REMARK");
            t_transportsupplierdetail.Remark1 = (string)dbFactory.ToModelValue(reader, "REMARK1");
            t_transportsupplierdetail.Remark2 = (string)dbFactory.ToModelValue(reader, "REMARK2");
            t_transportsupplierdetail.Remark3 = (string)dbFactory.ToModelValue(reader, "REMARK3");
            t_transportsupplierdetail.VoucherNo = (string)dbFactory.ToModelValue(reader, "VOUCHERNO");
            t_transportsupplierdetail.Type = (string)dbFactory.ToModelValue(reader, "TYPE");
            t_transportsupplierdetail.TradingConditionsCode = (string)dbFactory.ToModelValue(reader, "tradingconditions");
            t_transportsupplierdetail.strType = (string)dbFactory.ToModelValue(reader, "strtype");
            return t_transportsupplierdetail;
        }

        protected override string GetViewName()
        {
            return "V_Transportsupplierdetail";
        }

        protected override string GetTableName()
        {
            return "T_TRANSPORTSUPPLIERDETAIL";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }
        protected override string GetFilterSql(UserModel user, T_TransportSupDetailInfo model)
        {
            string strSql = " where isnull(isDel,0) != 2";
            string strAnd = " and ";

            if (!Common_Func.IsNullOrEmpty(model.PlateNumber))
            {
                strSql += strAnd;
                strSql += " PlateNumber LIKE '%" + model.PlateNumber + "%'  ";
            }

            if (!Common_Func.IsNullOrEmpty(model.PalletNo))
            {
                strSql += strAnd;
                //strSql += " ID In (Select WarehouseID From T_House Where HouseNo LIKE '%" + model.HouseNo + "%' OR HouseName Like '%" + model.HouseNo + "%') ";
                strSql += " PalletNo LIKE '%" + model.PalletNo + "%' ";
            }

            if (!Common_Func.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " ErpVoucherNo LIKE '%" + model.ErpVoucherNo + "%' ";
            }

            if (!Common_Func.IsNullOrEmpty(model.strType))
            {
                strSql += strAnd;
                strSql += " strType = '" + model.strType + "' ";
            }

            if (!Common_Func.IsNullOrEmpty(model.VoucherNo))
            {
                strSql += strAnd;
                strSql += " VoucherNo = '" + model.VoucherNo + "' ";
            }
            

            if (!Common_Func.IsNullOrEmpty(model.Creater))
            {
                strSql += strAnd;
                strSql += " Creater Like '%" + model.Creater + "%' ";
            }

            if (model.DateFrom != null)
            {
                strSql += strAnd;
                strSql += " CreateTime >= " + model.DateFrom.ToDateTime().Date.ToOracleTimeString() + "  ";
            }

            if (model.DateTo != null)
            {
                strSql += strAnd;
                strSql += " CreateTime <= " + model.DateTo.ToDateTime().AddDays(1).Date.ToOracleTimeString() + " ";
            }

            return strSql;
        }

        //internal List<T_TransportSupplier> GetModelListBySql(UserInfo user, bool IncludNoCheck)
        //{
        //    string strSql = string.Empty;
        //    strSql = "select * from t_Transportsupplier T order by ID";
        //    return GetModelListBySql(strSql);
        //}

        //internal List<T_TransportSupDetailInfo> GetTransportSupplierList()
        //{
        //    List<T_TransportSupDetailInfo> modelList = new List<T_TransportSupDetailInfo>();
        //    // string strSql = "SELECT t.transportsupplierid,(t.transportsupplierid  || ':'  ||t.transportsuppliername) as  transportsuppliername  from T_TRANSPORTSUPPLIER  t";
        //    string strSql = "SELECT t.transportsupplierid,t.transportsuppliername from T_TRANSPORTSUPPLIER  t";

        //    using (IDataReader reader = dbFactory.ExecuteReader(strSql))
        //    {
        //        while (reader.Read())
        //        {
        //            T_TransportSupDetailInfo model = new T_TransportSupDetailInfo();
        //            model.TransportSupplierID = reader["transportsupplierid"].ToInt32();
        //            model.TransportSupplierName = reader["transportsuppliername"].ToDBString();
        //            modelList.Add(model);
        //        }
        //    }
        //    return modelList;
        //}




        internal List<T_TransportSupDetailInfo> GetTransportSupplierDetailList(string Palletno)
        {
            List<T_TransportSupDetailInfo> modelList = new List<T_TransportSupDetailInfo>();
            string strSql = " select pa.*,t_outstock.contact,t_outstock.address,t_outstock.address1,t_outstock.phone from V_TRANSPORTSUPPLIERDETAIL  pa left join t_outstock on pa.erpvoucherno=t_outstock.erpvoucherno where pa.palletno='" + Palletno + "' and pa.type =1 ";
            using (IDataReader reader = dbFactory.ExecuteReader(strSql))
            {
                while (reader.Read())
                {
                    T_TransportSupDetailInfo model = new T_TransportSupDetailInfo();
                    model.ID = reader["id"].ToInt32();
                    model.PalletNo = reader["palletno"].ToDBString();
                    model.ErpVoucherNo = reader["erpvoucherno"].ToDBString();
                    model.PlateNumber = reader["platenumber"].ToDBString();
                    model.Feight = reader["FEIGHT"].ToDBString();
                    model.Creater = reader["creater"].ToDBString();
                    model.BoxCount = reader["boxcount"].ToDBString();
                    model.VoucherNo = reader["voucherno"].ToDBString();
                    model.strType = reader["strType"].ToDBString();
                    model.CustomerName = reader["CustomerName"].ToDBString();
                    model.TradingConditionsCode = reader["tradingconditions"].ToDBString();

                    model.Contact = reader["Contact"].ToDBString();
                    model.Phone = reader["Phone"].ToDBString();
                    model.Address = reader["Address"].ToDBString();
                    model.Address1 = reader["Address1"].ToDBString();
                    modelList.Add(model);
                }
            }
            return modelList;
        }


        internal bool SaveTransportSupplierADF(List<T_TransportSupDetailInfo> modeliist, ref string strError)
        {
            try
            {
                string type = modeliist[0].Type;//1:装2：卸
                string strSql = string.Empty;
                List<string> lstSql = new List<string>();

                List<TransportSupplierDetail> delmodeliist = (from st in modeliist
                                                              group st by new
                                                              {
                                                                  st.PalletNo,
                                                                  st.PlateNumber
                                                              }
                                                              into temp
                                                              select new TransportSupplierDetail()
                                                              {
                                                                  palletno = temp.Key.PalletNo,
                                                                  platenumber = temp.Key.PlateNumber
                                                              }).ToList();
                for (int i = 0; i < delmodeliist.Count; i++)
                {
                    if (type == "1")
                    {
                        strSql = @"delete from T_TRANSPORTSUPPLIERDETAIL where type=1 and palletno='" + delmodeliist[i].palletno + "' and PLATENUMBER='" + delmodeliist[i].platenumber + "'";
                        lstSql.Add(strSql);
                    }
                    if (type == "2")
                    {
                        strSql = @"delete from T_TRANSPORTSUPPLIERDETAIL where type=2 and palletno='" + delmodeliist[i].palletno + "'";
                        lstSql.Add(strSql);
                    }
                }

                for (int i = 0; i < modeliist.Count; i++)
                {


                    int ID = base.GetTableID("SEQ_TRANSPORTSUPPLIERDETAIL");
                    strSql = @"INSERT into T_TRANSPORTSUPPLIERDETAIL(ID,ERPVOUCHERNO,PLATENUMBER,FEIGHT,CREATETIME,isdel,palletno,boxcount,outboxcount,customername,voucherno,type,remark,remark1,remark2,remark3,creater)
                    VALUES(" + ID + ",'{0}', '{1}', '{2}',SYSDATE, '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}')";
                    strSql = string.Format(strSql, modeliist[i].ErpVoucherNo, modeliist[i].PlateNumber, modeliist[i].Feight,
                       modeliist[i].IsDel, modeliist[i].PalletNo, modeliist[i].BoxCount, modeliist[i].OutBoxCount,
                       modeliist[i].CustomerName, modeliist[i].VoucherNo, modeliist[i].Type, modeliist[i].Remark, modeliist[i].Remark1, modeliist[i].Remark2, modeliist[i].Remark3, modeliist[i].Creater);
                    lstSql.Add(strSql);
                }
                return base.SaveModelListBySqlToDB(lstSql, ref strError);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
