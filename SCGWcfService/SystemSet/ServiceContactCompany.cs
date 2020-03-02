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
        #region 自动生成WCF调用T_ContactCompany_Func代码

        public bool SaveT_ContactCompany(UserInfo user, ref T_ContactCompanyInfo t_contactcompany, ref string strError)
        {
            T_ContactCompany_Func tfunc = new T_ContactCompany_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_contactcompany, ref strError);
        }


        public bool DeleteT_ContactCompanyByModel(UserInfo user, T_ContactCompanyInfo model, ref string strError)
        {
            T_ContactCompany_Func tfunc = new T_ContactCompany_Func();
            return tfunc.DeleteModelByModelSql(user, model, ref strError);
        }




        public bool GetT_ContactCompanyByID(ref T_ContactCompanyInfo model, ref string strError)
        {
            T_ContactCompany_Func tfunc = new T_ContactCompany_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_ContactCompanyListByPage(ref List<T_ContactCompanyInfo> modelList, UserInfo user, T_ContactCompanyInfo t_contactcompany, ref DividPage page, ref string strError)
        {
            T_ContactCompany_Func tfunc = new T_ContactCompany_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_contactcompany, ref page, ref strError);
        }


        public bool GetAllT_ContactCompanyByHeaderID(ref List<T_ContactCompanyInfo> modelList, int headerID, ref string strError)
        {
            T_ContactCompany_Func tfunc = new T_ContactCompany_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_ContactCompanyStatus(UserInfo user, ref T_ContactCompanyInfo t_contactcompany, int NewStatus, ref string strError)
        {
            T_ContactCompany_Func tfunc = new T_ContactCompany_Func();
            return tfunc.UpdateModelStatus(user, ref t_contactcompany, NewStatus, ref strError);
        }


        #endregion
    }
}