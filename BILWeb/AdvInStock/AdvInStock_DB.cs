//************************************************************
//******************************作者：方颖*********************
//******************************时间：2019/7/17 15:50:21*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using BILBasic.User;
using System.Data;
using BILBasic.Common;

namespace BILWeb.AdvInStock
{
    public partial class T_AdvInStock_DB : BILBasic.Basing.Factory.Base_DB<T_AdvInStockInfo>
    {

        /// <summary>
        /// 添加t_advinstock
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_AdvInStockInfo t_advinstock)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }
        protected override List<string> GetSaveModelListSql(BILBasic.User.UserModel user, List<T_AdvInStockInfo> listadvinstock)
        {
            T_AdvInStockInfo t_advinstock = listadvinstock[0];
            return GetSaveSql(user, ref  t_advinstock);
        }
        protected override List<string> GetSaveSql(BILBasic.User.UserModel user, ref T_AdvInStockInfo t_advinstock)
        {
            List<string> lstSql = new List<string>();
            string sql = "";
            // SEQ_ADVINSTOCK_ID
            //SEQ_ADVINSTOCKDETAIL_ID
            //SEQ_ADVINSTOCK_NO
            t_advinstock.ID = GetTableID("SEQ_ADVINSTOCK_ID");
            t_advinstock.VoucherNo = DateTime.Now.ToString("yyMMdd") + GetTableID("SEQ_ADVINSTOCK_NO").ToString().PadLeft(4, '0');
            t_advinstock.CreateTime = DateTime.Now;
            t_advinstock.Modifyer = t_advinstock.Creater;
            t_advinstock.ModifyTime = DateTime.Now;

            sql = "insert into t_advinstock(id, voucherno, erpvoucherno, supplierno, suppliername, vouchertype, createtime, creater, modifyer, modifytime, status, isdel, strongholdcode, strongholdname, companycode, warehouseid)" +
    "values (" + t_advinstock.ID + ", '" + t_advinstock.VoucherNo + "', '" + t_advinstock.ErpVoucherNo + "', '" + t_advinstock.SupplierNo + "','" + t_advinstock.SupplierName + "'," + t_advinstock.VoucherType + ", sysdate, '" + t_advinstock.Creater + "', '" + t_advinstock.Modifyer + "',sysdate, " + t_advinstock.Status + ", 1, '" + t_advinstock.StrongHoldCode + "', '" + t_advinstock.StrongHoldName + "', '" + t_advinstock.CompanyCode + "', '" + t_advinstock.WarehouseID + "')";

            lstSql.Add(sql);
            Dictionary<string, decimal> receiveQty = new Dictionary<string, decimal>();
            int advInState = 3;
            foreach (T_AdvInStockDetailInfo item in t_advinstock.lstDetail)
            {

                item.ID = GetTableID("SEQ_ADVINSTOCKDETAIL_ID");
                item.Creater = t_advinstock.Creater;
                item.CreateTime = DateTime.Now;
                item.HeaderID = t_advinstock.ID;
                item.VOUCHERNO = t_advinstock.VoucherNo;
                item.ErpVoucherNo = t_advinstock.ErpVoucherNo;
                sql = " insert into  T_ADVINSTOCKDETAIL (id, headerid, materialno, materialdesc, advqty, unit, linestatus, creater, createtime, isdel, ean, edate, supbatch, qualitytype, voucherno, erpvoucherno,ERPROWID,RowNO,RowNODel,remark,materialnoid)"//
                    + " values (" + item.ID + "," + item.HeaderID + ",'" + item.MaterialNo + "','" + item.MaterialDesc + "'," + item.AdvQty + ",'" + item.Unit + "',1,'" + item.Creater + "',sysdate,1,'" + item.EAN + "',to_date('" + item.EDate.ToString("yyyy-MM-dd") + "','yyyy-MM-dd'),'" + item.SupBatch + "'," + item.QualityType + ",'" + item.VOUCHERNO + "','" + item.ErpVoucherNo + "'," + item.ERPNote + ",'" + item.RowNO + "','" + item.RowNODel + "','" + item.remark + "'," + item.MaterialNoID + ")";
                lstSql.Add(sql);
                if (receiveQty.ContainsKey(item.ERPNote))
                {
                    receiveQty[item.ERPNote] = receiveQty[item.ERPNote] + Convert.ToDecimal(item.AdvQty);
                }
                else
                {
                    receiveQty.Add(item.ERPNote, Convert.ToDecimal(item.AdvQty));
                }

            }

            foreach (var item in receiveQty.Keys)
            {
                lstSql.Add("update t_instockdetail set ADVRECEIVEQTY=isnull(ADVRECEIVEQTY,0)+" + receiveQty[item] + " where id=" + item);
            }
            lstSql.Add("update t_Instock a set advinstatus =3 where (select count(*) from t_Instock a left join t_instockdetail d on a.id =d.headerid where d.instockqty >isnull(d.advreceiveqty,0) and a.erpvoucherno ='" + t_advinstock.ErpVoucherNo + "')=0 and a.erpvoucherno ='" + t_advinstock.ErpVoucherNo + "' ");
            return lstSql;
        }
        /// <summary>
        /// 获取质检类型
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public List<Parameter_Model> GetAdvInParameter(string groupname)
        {
            List<Parameter_Model> listParameter = new List<Parameter_Model>();
            using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, "select parameterid,parametername from t_parameter where groupname ='" + groupname + "'"))
            {
                while (dr.Read())
                {
                    Parameter_Model parameter = new Parameter_Model();
                    parameter.parameterid = dr["parameterid"].ToString();
                    parameter.ParameterName = dr["parametername"].ToString();
                    listParameter.Add(parameter);
                }
            }

            return listParameter;
        }

        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_AdvInStockInfo ToModel(IDataReader reader)
        {
            T_AdvInStockInfo t_advinstock = new T_AdvInStockInfo();

            t_advinstock.ID = (int)dbFactory.ToModelValue(reader, "ID");
            t_advinstock.VoucherNo = (string)dbFactory.ToModelValue(reader, "VOUCHERNO");
            t_advinstock.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ERPVOUCHERNO");
            t_advinstock.SupplierNo = (string)dbFactory.ToModelValue(reader, "SUPPLIERNO");
            t_advinstock.SupplierName = (string)dbFactory.ToModelValue(reader, "SUPPLIERNAME");
            t_advinstock.VoucherType = (int)dbFactory.ToModelValue(reader, "VOUCHERTYPE");
            t_advinstock.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_advinstock.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_advinstock.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_advinstock.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_advinstock.Status = (int)dbFactory.ToModelValue(reader, "STATUS");
            t_advinstock.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToDecimal();
            t_advinstock.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "STRONGHOLDCODE");
            t_advinstock.StrongHoldName = (string)dbFactory.ToModelValue(reader, "STRONGHOLDNAME");
            t_advinstock.CompanyCode = (string)dbFactory.ToModelValue(reader, "COMPANYCODE");
            t_advinstock.WarehouseID = (int)dbFactory.ToModelValue(reader, "WAREHOUSEID");
            return t_advinstock;
        }

        protected override string GetViewName()
        {
            return "V_AdvInStock";
        }

        protected override string GetTableName()
        {
            return "T_ADVINSTOCK";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }



        protected override string GetFilterSql(UserModel user, T_AdvInStockInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";


            return strSql + "order by id desc";
        }

    }
}
