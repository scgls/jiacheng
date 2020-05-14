﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.User;
using BILBasic.Common;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using BILBasic.Basing;

namespace BILWeb.YS
{
    public partial class T_YS_DB : BILBasic.Basing.Factory.Base_DB<T_YS>
    {

        /// <summary>
        /// 添加t_instock
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_YS t_instock)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_YS model)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            return lstSql;

        }

        


        protected override List<string> GetUpdateSql(UserModel user, T_YS model)
        {
            List<string> lstSql = new List<string>();

            string strSql = "update t_ys  set Status = '" + model.Status + "' where id = '" + model.ID + "'";

            lstSql.Add(strSql);

            return lstSql;
        }

        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_YS ToModel(IDataReader reader)
        {
            T_YS t_instock = new T_YS();

            t_instock.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_instock.VoucherNo = (string)dbFactory.ToModelValue(reader, "VOUCHERNO");
            t_instock.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ERPVOUCHERNO");
            t_instock.SupplierNo = (string)dbFactory.ToModelValue(reader, "SUPPLIERNO");
            t_instock.SupplierName = (string)dbFactory.ToModelValue(reader, "SUPPLIERNAME");
            t_instock.IsQuality = (decimal?)dbFactory.ToModelValue(reader, "ISQUALITY");
            t_instock.IsReceivePost = (decimal?)dbFactory.ToModelValue(reader, "ISRECEIVEPOST");
            t_instock.IsShelvePost = (decimal?)dbFactory.ToModelValue(reader, "ISSHELVEPOST");
            t_instock.VoucherType = dbFactory.ToModelValue(reader, "VOUCHERTYPE").ToInt32();
            t_instock.Plant = (string)dbFactory.ToModelValue(reader, "PLANT");
            t_instock.PlantName = (string)dbFactory.ToModelValue(reader, "PLANTNAME");
            t_instock.MoveType = (string)dbFactory.ToModelValue(reader, "MOVETYPE");
            t_instock.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_instock.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_instock.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_instock.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_instock.Status = dbFactory.ToModelValue(reader, "STATUS").ToInt32();
            t_instock.StrVoucherType = (string)dbFactory.ToModelValue(reader, "StrVoucherType");
            t_instock.StrStatus = (string)dbFactory.ToModelValue(reader, "StrStatus");
            t_instock.StrCreater = (string)dbFactory.ToModelValue(reader, "StrCreater");

            t_instock.StrongHoldCode = dbFactory.ToModelValue(reader, "StrongHoldCode").ToDBString();
            t_instock.StrongHoldName = dbFactory.ToModelValue(reader, "StrongHoldName").ToDBString();
            t_instock.CompanyCode = dbFactory.ToModelValue(reader, "CompanyCode").ToDBString();
            t_instock.ERPCreater = dbFactory.ToModelValue(reader, "ERPCreater").ToDBString();
            t_instock.VouDate = dbFactory.ToModelValue(reader, "VouDate").ToDateTime();
            t_instock.VouUser = dbFactory.ToModelValue(reader, "VouUser").ToDBString();
            t_instock.DepartmentCode = dbFactory.ToModelValue(reader, "DepartmentCode").ToDBString();
            t_instock.DepartmentName = dbFactory.ToModelValue(reader, "DepartmentName").ToDBString();
            t_instock.ERPStatus = dbFactory.ToModelValue(reader, "ERPStatus").ToDBString();
            t_instock.ERPNote = dbFactory.ToModelValue(reader, "ERPNote").ToDBString();
            t_instock.QcCode = dbFactory.ToModelValue(reader, "QcCode").ToDBString();
            t_instock.QcDesc = dbFactory.ToModelValue(reader, "QcDesc").ToDBString();
            t_instock.AdvInStatus = dbFactory.ToModelValue(reader, "AdvInStatus").ToInt32(); // 预到货状态

            t_instock.Note = (string)dbFactory.ToModelValue(reader, "Note");
            
            return t_instock;
        }

        protected override string GetViewName()
        {
            return "V_YS";
        }

        protected override string GetTableName()
        {
            return "T_YS";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override string GetFilterSql(UserModel user, T_YS model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";

            if (model.Status > 0)
            {
                //strSql += strAnd;
                //strSql += "isnull(status,1)= '" + model.Status + "'";

                if (model.Status == 1 || model.Status == 2)
                {
                    strSql += strAnd;
                    strSql += "(isnull(status,1)=1 or isnull(status,1)=2)";

                }
                else
                {
                    strSql += strAnd;
                    strSql += "isnull(status,1)= '" + model.Status + "'";
                }
            }
            if (!string.IsNullOrEmpty(model.StrVoucherType) && model.StrVoucherType.Equals("预到货"))
            {
                strSql += strAnd;
                strSql += "(isnull(AdvInStatus,1)=1 or isnull(AdvInStatus,1)=2)";
            }

            if (!string.IsNullOrEmpty(model.SupplierNo))
            {
                strSql += strAnd;
                strSql += " (SupplierNo Like '" + model.SupplierNo + "%'  or SupplierName Like '" + model.SupplierNo + "%' )";
            }

            if (model.DateFrom != null)
            {
                strSql += strAnd;
                strSql += " CreateTime >= " + model.DateFrom.ToDateTime().Date.ToOracleTimeString() + " ";
            }

            if (model.DateTo != null)
            {
                strSql += strAnd;
                strSql += " CreateTime <= " + model.DateTo.ToDateTime().Date.AddDays(1).ToOracleTimeString() + " ";
            }

            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " erpvoucherno like  '%" + model.ErpVoucherNo.Trim() + "%'  ";
            }

            if (model.VoucherType > 0)
            {
                strSql += strAnd;
                strSql += " vouchertype ='" + model.VoucherType + "'  ";
            }

            if (!user.UserNo.Equals("admin"))
            {
                strSql += strAnd;
                strSql += " strongholdcode = '"+user.StrongHoldCode+"' ";
            }

            return strSql; //+ "order by id desc";
        }

        /// <summary>
        /// 导入序列号，验证订单是否存在
        /// </summary>
        /// <param name="VoucherNo"></param>
        /// <returns></returns>
        public int CheckVoucherNo(string VoucherNo,int VoucherType) 
        {
            string strSql = "select count(1) from t_Instock   where Erpvoucherno ='" + VoucherNo + "' and vouchertype = '"+VoucherType+"' ";

           return base.GetScalarBySql(strSql).ToInt32();
        }

        public override List<T_YS> GetModelListADF(UserModel user, T_YS model)
        {
            List <T_YS> modelList= base.GetModelListADF(user, model);
            return modelList;

        }

    }
}
