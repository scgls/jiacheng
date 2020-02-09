//************************************************************
//******************************作者：方颖*********************
//******************************时间：2017/2/18 12:43:50*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
using BILBasic.Common;
using System.Data;

namespace BILWeb.Supplier
{
    public partial class T_Supplier_DB : BILBasic.Basing.Factory.Base_DB<T_SupplierInfo>
    {

        /// <summary>
        /// 添加t_supplier
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_SupplierInfo t_supplier)
        {
            
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            //int iRows = 0;

            OracleParameter[] param = new OracleParameter[]{
              new OracleParameter("@bResult",OracleDbType.Int32),
               new OracleParameter("@ErrorMsg",OracleDbType.NVarchar2,1000),
               
               new OracleParameter("@v_ID", dbFactory.ToDBValue(t_supplier.ID)),
               new OracleParameter("@v_SupplierNo", dbFactory.ToDBValue(t_supplier.SupplierNo).ToOracleValue()),
               new OracleParameter("@v_SupplierName", dbFactory.ToDBValue(t_supplier.SupplierName).ToOracleValue()),
               new OracleParameter("@v_SupplierNameEN", t_supplier.SupplierNameEN.ToOracleValue()),
               new OracleParameter("@v_SupplierAbridge", t_supplier.SupplierAbridge.ToOracleValue()),
               new OracleParameter("@v_Note", t_supplier.Note.ToOracleValue()),
               new OracleParameter("@v_ContactPerson", t_supplier.ContactPerson.ToOracleValue()),
               new OracleParameter("@v_ContactTel", t_supplier.ContactTel.ToOracleValue()),
               new OracleParameter("@v_Mobile", t_supplier.Mobile.ToOracleValue()),
               new OracleParameter("@v_Fax", t_supplier.Fax.ToOracleValue()),
               new OracleParameter("@v_Email", t_supplier.Email.ToOracleValue()),
               new OracleParameter("@v_IsDel", t_supplier.IsDel.ToOracleValue()),
               new OracleParameter("@v_Address", t_supplier.Address.ToOracleValue()),
               new OracleParameter("@v_MailAddress", t_supplier.MailAddress.ToOracleValue()),
               new OracleParameter("@v_Creater", t_supplier.Creater.ToOracleValue()),
               new OracleParameter("@v_CreateTime", t_supplier.CreateTime.ToOracleValue()),
               new OracleParameter("@v_Modifyer", t_supplier.Modifyer.ToOracleValue()),
               new OracleParameter("@v_ModifyTime", t_supplier.ModifyTime.ToOracleValue())
              
              };
            param[0].Direction = System.Data.ParameterDirection.Output;
            param[1].Direction = System.Data.ParameterDirection.Output;
            param[2].Direction = System.Data.ParameterDirection.InputOutput;
            return param;

        }

        protected override List<string> GetSaveSql(BILBasic.User.UserModel user, ref T_SupplierInfo model)
        {
            throw new System.NotImplementedException();
        }
      



        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_SupplierInfo ToModel(IDataReader reader)
        {
            T_SupplierInfo t_supplier = new T_SupplierInfo();

            t_supplier.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_supplier.SupplierNo = (string)dbFactory.ToModelValue(reader, "SUPPLIERNO");
            t_supplier.SupplierName = (string)dbFactory.ToModelValue(reader, "SUPPLIERNAME");
            t_supplier.SupplierNameEN = (string)dbFactory.ToModelValue(reader, "SUPPLIERNAMEEN");
            t_supplier.SupplierAbridge = (string)dbFactory.ToModelValue(reader, "SUPPLIERABRIDGE");
            t_supplier.Note = (string)dbFactory.ToModelValue(reader, "NOTE");
            t_supplier.ContactPerson = (string)dbFactory.ToModelValue(reader, "CONTACTPERSON");
            t_supplier.ContactTel = (string)dbFactory.ToModelValue(reader, "CONTACTTEL");
            t_supplier.Mobile = (string)dbFactory.ToModelValue(reader, "MOBILE");
            t_supplier.Fax = (string)dbFactory.ToModelValue(reader, "FAX");
            t_supplier.Email = (string)dbFactory.ToModelValue(reader, "EMAIL");
            t_supplier.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToDecimal();
            t_supplier.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_supplier.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_supplier.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_supplier.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_supplier.Address = (string)dbFactory.ToModelValue(reader, "ADDRESS");
            t_supplier.MailAddress = (string)dbFactory.ToModelValue(reader, "MAILADRESS");

            return t_supplier;
        }

        protected override string GetViewName()
        {
            return "V_SUPPLIER";
        }

        protected override string GetTableName()
        {
            return "T_SUPPLIER";
        }

       

        protected override string GetFilterSql(BILBasic.User.UserModel user, T_SupplierInfo model)
        {
            string strSql = " where isnull(isDel,0) != 2  ";


            if (model.SupplierNo != null && model.SupplierNo != "")
            {
                strSql += "  and ( SupplierNo Like '%" + model.SupplierNo + "%'  or SupplierName Like '%" + model.SupplierNo + "%' ) ";
            }

            if (model.ContactPerson != null && model.ContactPerson != "")
            {
                strSql += " and ContactPerson like '%" + model.ContactPerson + "%' ";
            }

            if (model.ContactTel != null && model.ContactTel != "")
            {
                strSql += " and ContactTel like '%" + model.ContactTel + "%' ";
            }

            if (model.SupplierName != null && model.SupplierName != "")
            {
                strSql += " and SupplierName like '%" + model.SupplierName + "%' ";
            }

            return strSql;

        }


        protected override string GetSaveProcedureName()
        {
            return "P_Save_T_Supplier";
        }


        protected override string GetDeleteProcedureName()
        {
            return "P_Delete_T_Supplier";
        }


    }
}
