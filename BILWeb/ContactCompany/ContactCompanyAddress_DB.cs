
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

namespace BILWeb.ContactCompany
{
    public partial class T_ContactCompany_Address_DB : BILBasic.Basing.Factory.Base_DB<T_ContactCompany_AddressInfo>
    {

        /// <summary>
        /// 添加t_contactcompany_address
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_ContactCompany_AddressInfo t_contactcompany_address)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user,ref  T_ContactCompany_AddressInfo model)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            if (model.ID <= 0)
            {
                strSql = "insert into T_CONTACTCOMPANY_ADDRESS (id, contactcompanyid, contactuser, tel, mobile, fax, netaddress, email, address, creater, createtime)" +
                        "values (seq_contactaddress.Nextval, '" + model.ContactCompanyID + "', '" + model.ContactUser + "', '" + model.Tel + "', '" + model.Mobile + "', "+
                        "'" + model.Fax + "', '" + model.NetAddress + "', '" + model.Email + "', '" + model.Address + "', '" + user.Creater + "', Sysdate)";

                lstSql.Add(strSql);
            }
            else
            {
                strSql = "update t_Contactcompany_Address a set a.Contactuser='" + model.ContactUser + "',a.Tel='" + model.Tel + "',a.Fax='" + model.Fax + "',a.Netaddress='" + model.NetAddress + "',a.Email='" + model.Email + "',a.Address='" + model.Address + "'," +
                        "a.Mobile='" + model.Mobile + "',a.Modifyer='" + user.Modifyer + "',a.Modifytime=Sysdate";
                lstSql.Add(strSql);
            }

            return lstSql;
        }

        protected override List<string> GetDeleteSql(UserModel user, T_ContactCompany_AddressInfo model)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            strSql = "delete t_Contactcompany_Address where id = '" + model.ID + "'";

            lstSql.Add(strSql);

            return lstSql;
        }


        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_ContactCompany_AddressInfo ToModel(IDataReader reader)
        {
            T_ContactCompany_AddressInfo t_contactcompany_address = new T_ContactCompany_AddressInfo();

            t_contactcompany_address.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_contactcompany_address.ContactCompanyID = (decimal?)dbFactory.ToModelValue(reader, "CONTACTCOMPANYID");
            t_contactcompany_address.ContactUser = (string)dbFactory.ToModelValue(reader, "CONTACTUSER");
            t_contactcompany_address.Tel = (string)dbFactory.ToModelValue(reader, "TEL");
            t_contactcompany_address.Mobile = (string)dbFactory.ToModelValue(reader, "MOBILE");
            t_contactcompany_address.Fax = (string)dbFactory.ToModelValue(reader, "FAX");
            t_contactcompany_address.NetAddress = (string)dbFactory.ToModelValue(reader, "NETADDRESS");
            t_contactcompany_address.Email = (string)dbFactory.ToModelValue(reader, "EMAIL");
            t_contactcompany_address.Address = (string)dbFactory.ToModelValue(reader, "ADDRESS");
            t_contactcompany_address.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_contactcompany_address.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_contactcompany_address.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_contactcompany_address.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            return t_contactcompany_address;
        }

        protected override string GetViewName()
        {
            return "t_contactcompany_address";
        }

        protected override string GetTableName()
        {
            return "T_CONTACTCOMPANY_ADDRESS";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }


    }
}
