//************************************************************
//******************************作者：方颖*********************
//******************************时间：2017/2/21 10:24:07*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using BILBasic.User;
using BILBasic.Common;
using System.Data;

namespace BILWeb.Customer
{
    public partial class T_Customer_DB : BILBasic.Basing.Factory.Base_DB<T_CustomerInfo>
    {

        /// <summary>
        /// 添加t_customer
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_CustomerInfo t_customer)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            OracleParameter[] param = new OracleParameter[]{
              new OracleParameter("@bResult",OracleDbType.Int32),
               new OracleParameter("@ErrorMsg",OracleDbType.NVarchar2,1000),
               
               new OracleParameter("@v_ID", dbFactory.ToDBValue(t_customer.ID)),
               new OracleParameter("@v_Customerno", dbFactory.ToDBValue(t_customer.CustomerNo).ToOracleValue()),
               new OracleParameter("@v_Customername", dbFactory.ToDBValue(t_customer.CustomerName).ToOracleValue()),
               new OracleParameter("@v_Englishname", t_customer.EnglishName.ToOracleValue()),
               new OracleParameter("@v_Customerabridge", t_customer.CustomerAbridge.ToOracleValue()),
               new OracleParameter("@v_Customerstyle", t_customer.CustomerStyle.ToOracleValue()),
               new OracleParameter("@v_Companyid", t_customer.CompanyID.ToOracleValue()),
               new OracleParameter("@v_Note", t_customer.Note.ToOracleValue()),
               new OracleParameter("@v_Contactperson", t_customer.ContactPerson.ToOracleValue()),
               new OracleParameter("@v_Contacttel", t_customer.ContactTel.ToOracleValue()),
               new OracleParameter("@v_Mobile", t_customer.Mobile.ToOracleValue()),
               new OracleParameter("@v_Fax", t_customer.Fax.ToOracleValue()),
               new OracleParameter("@v_Email", t_customer.Email.ToOracleValue()),
               new OracleParameter("@v_Isdel", t_customer.IsDel.ToOracleValue()),
               new OracleParameter("@v_Mailadress", t_customer.MailAdress.ToOracleValue()),
               new OracleParameter("@v_Address", t_customer.Address.ToOracleValue()),
               new OracleParameter("@v_Creater", t_customer.Creater.ToOracleValue()),
               new OracleParameter("@v_CreateTime", t_customer.CreateTime.ToOracleValue()),
               new OracleParameter("@v_Modifyer", t_customer.Modifyer.ToOracleValue()),
               new OracleParameter("@v_ModifyTime", t_customer.ModifyTime.ToOracleValue())
              
              };
            param[0].Direction = System.Data.ParameterDirection.Output;
            param[1].Direction = System.Data.ParameterDirection.Output;
            param[2].Direction = System.Data.ParameterDirection.InputOutput;
            return param;


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_CustomerInfo model)
        {
 	        throw new System.NotImplementedException();
        }
        




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_CustomerInfo ToModel(IDataReader reader)
        {
            T_CustomerInfo t_customer = new T_CustomerInfo();

            t_customer.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_customer.CustomerNo = (string)dbFactory.ToModelValue(reader, "CUSTOMERNO");
            t_customer.CustomerName = (string)dbFactory.ToModelValue(reader, "CUSTOMERNAME");
            t_customer.EnglishName = (string)dbFactory.ToModelValue(reader, "ENGLISHNAME");
            t_customer.CustomerAbridge = (string)dbFactory.ToModelValue(reader, "CUSTOMERABRIDGE");
            t_customer.CustomerStyle = (decimal?)dbFactory.ToModelValue(reader, "CUSTOMERSTYLE");
            t_customer.CompanyID = dbFactory.ToModelValue(reader, "COMPANYID").ToInt32();
            t_customer.Note = (string)dbFactory.ToModelValue(reader, "NOTE");
            t_customer.ContactPerson = (string)dbFactory.ToModelValue(reader, "CONTACTPERSON");
            t_customer.ContactTel = (string)dbFactory.ToModelValue(reader, "CONTACTTEL");
            t_customer.Mobile = (string)dbFactory.ToModelValue(reader, "MOBILE");
            t_customer.Fax = (string)dbFactory.ToModelValue(reader, "FAX");
            t_customer.Email = (string)dbFactory.ToModelValue(reader, "EMAIL");
            t_customer.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToDecimal();
            t_customer.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_customer.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_customer.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_customer.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_customer.MailAdress = (string)dbFactory.ToModelValue(reader, "MAILADRESS");
            t_customer.Address = (string)dbFactory.ToModelValue(reader, "ADDRESS");
            return t_customer;
        }

        protected override string GetViewName()
        {
            return "v_customer";
        }

        protected override string GetTableName()
        {
            return "T_CUSTOMER";
        }

        protected override string GetSaveProcedureName()
        {
            return "P_SAVE_T_CUSTOMER";
        }


        protected override string GetFilterSql(UserModel user, T_CustomerInfo customer)
        {
            string strSql = " where isnull(isDel,0) != 2  ";
            string strAnd = " and ";

            if (!Common_Func.IsNullOrEmpty(customer.CustomerNo))
            {
                strSql += strAnd;
                strSql += " (CustomerNo like '%" + customer.CustomerNo + "%' or CustomerName like '%" + customer.CustomerNo + "%')  ";
            }


            if (!string.IsNullOrEmpty(customer.ContactPerson))
            {
                strSql += strAnd;
                strSql += " ContactPerson like '%" + customer.ContactPerson + "%'";
            }

            if (!string.IsNullOrEmpty(customer.ContactTel))
            {
                strSql += strAnd;
                strSql += " ContactTel like '%" + customer.ContactTel + "%'";
            }
            if (!string.IsNullOrEmpty(customer.CustomerName))
            {
                strSql += strAnd;
                strSql += " CustomerName like '%" + customer.CustomerName + "%'";
            }
            



            return strSql;
        }

    }
}
