//************************************************************
//******************************作者：方颖*********************
//******************************时间：2017/2/18 12:56:34*******

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
using System.Data;

namespace BILWeb.Supplier
{
    public partial class T_SupplierAddress_DB : BILBasic.Basing.Factory.Base_DB<T_SupplierAddressInfo>
    {

        /// <summary>
        /// 添加t_supplieraddress
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_SupplierAddressInfo t_supplieraddress)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            OracleParameter[] param = new OracleParameter[]{
              new OracleParameter("@bResult",OracleDbType.Int32),
               new OracleParameter("@ErrorMsg",OracleDbType.NVarchar2,1000),
               
               new OracleParameter("@v_ID", dbFactory.ToDBValue(t_supplieraddress.ID)),
               new OracleParameter("@v_HeaderID", dbFactory.ToDBValue(t_supplieraddress.HeaderID).ToOracleValue()),               
               new OracleParameter("@v_Contactperson", t_supplieraddress.ContactPerson.ToOracleValue()),
               new OracleParameter("@v_Contacttel", t_supplieraddress.ContactTel.ToOracleValue()),
               new OracleParameter("@v_Mobile", t_supplieraddress.Mobile.ToOracleValue()),
               new OracleParameter("@v_Fax", t_supplieraddress.Fax.ToOracleValue()),
               new OracleParameter("@v_Email", t_supplieraddress.Email.ToOracleValue()),               
               new OracleParameter("@v_Address", t_supplieraddress.Fax.ToOracleValue()),
               new OracleParameter("@v_Isdel", t_supplieraddress.IsDel.ToOracleValue()),
               new OracleParameter("@v_Isdefault", t_supplieraddress.IsDefault.ToOracleValue()),
              new OracleParameter("@v_CreateTime", t_supplieraddress.CreateTime.ToOracleValue()),
               new OracleParameter("@v_Creater", t_supplieraddress.Creater.ToOracleValue()),               
               new OracleParameter("@v_Modifyer", t_supplieraddress.Modifyer.ToOracleValue()),
               new OracleParameter("@v_ModifyTime", t_supplieraddress.ModifyTime.ToOracleValue())
              
              };
            param[0].Direction = System.Data.ParameterDirection.Output;
            param[1].Direction = System.Data.ParameterDirection.Output;
            param[2].Direction = System.Data.ParameterDirection.InputOutput;
            return param;


        }

        protected override List<string> GetSaveSql(BILBasic.User.UserModel user, ref T_SupplierAddressInfo model)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_SupplierAddressInfo ToModel(IDataReader reader)
        {
            T_SupplierAddressInfo t_supplieraddress = new T_SupplierAddressInfo();

            t_supplieraddress.ID =dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_supplieraddress.HeaderID = dbFactory.ToModelValue(reader, "HeaderID").ToInt32();
            t_supplieraddress.ContactPerson = (string)dbFactory.ToModelValue(reader, "CONTACTPERSON");
            t_supplieraddress.ContactTel = (string)dbFactory.ToModelValue(reader, "CONTACTTEL");
            t_supplieraddress.Mobile = (string)dbFactory.ToModelValue(reader, "MOBILE");
            t_supplieraddress.Fax = (string)dbFactory.ToModelValue(reader, "FAX");
            t_supplieraddress.Email = (string)dbFactory.ToModelValue(reader, "EMAIL");
            t_supplieraddress.Address = (string)dbFactory.ToModelValue(reader, "ADDRESS");
            t_supplieraddress.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToDecimal();
            t_supplieraddress.IsDefault = (decimal?)dbFactory.ToModelValue(reader, "ISDEFAULT");
            t_supplieraddress.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_supplieraddress.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_supplieraddress.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_supplieraddress.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            return t_supplieraddress;
        }

        protected override string GetViewName()
        {
            return "V_SUPPLIERADDRESS";
        }

        protected override string GetTableName()
        {
            return "T_SUPPLIERADDRESS";
        }

        protected override string GetSaveProcedureName()
        {
            return "P_SAVE_T_SUPPLIERADDRESS";
        }

        protected override string GetDeleteProcedureName()
        {
            return "P_Delete_T_SUPPLIERADDRESS";
        }

    }
}
