using BILBasic.Common;
using BILWeb.ContactCompany;
using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_ContactCompany_Address_Func代码

        public bool SaveT_ContactCompany_Address(UserInfo user, ref T_ContactCompany_AddressInfo t_contactcompany_address, ref string strError)
        {
            T_ContactCompany_Address_Func tfunc = new T_ContactCompany_Address_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_contactcompany_address, ref strError);
        }


        public bool DeleteT_ContactCompany_AddressByModel(UserInfo user, T_ContactCompany_AddressInfo model, ref string strError)
        {
            T_ContactCompany_Address_Func tfunc = new T_ContactCompany_Address_Func();
            return tfunc.DeleteModelByModelSql(user, model, ref strError);
        }




        public bool GetT_ContactCompany_AddressByID(ref T_ContactCompany_AddressInfo model, ref string strError)
        {
            T_ContactCompany_Address_Func tfunc = new T_ContactCompany_Address_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_ContactCompany_AddressListByPage(ref List<T_ContactCompany_AddressInfo> modelList, UserInfo user, T_ContactCompany_AddressInfo t_contactcompany_address, ref DividPage page, ref string strError)
        {
            T_ContactCompany_Address_Func tfunc = new T_ContactCompany_Address_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_contactcompany_address, ref page, ref strError);
        }


        public bool GetAllT_ContactCompany_AddressByHeaderID(ref List<T_ContactCompany_AddressInfo> modelList, int headerID, ref string strError)
        {
            T_ContactCompany_Address_Func tfunc = new T_ContactCompany_Address_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_ContactCompany_AddressStatus(UserInfo user, ref T_ContactCompany_AddressInfo t_contactcompany_address, int NewStatus, ref string strError)
        {
            T_ContactCompany_Address_Func tfunc = new T_ContactCompany_Address_Func();
            return tfunc.UpdateModelStatus(user, ref t_contactcompany_address, NewStatus, ref strError);
        }


        #endregion
    }
}