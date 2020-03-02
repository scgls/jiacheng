using BILBasic.Common;
using BILWeb.ContactCompany;
using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_CONTACTCOMPANY_Func代码
        [OperationContract]
        bool SaveT_ContactCompany(UserInfo user, ref T_ContactCompanyInfo t_contactcompany, ref string strError);


        [OperationContract]
        bool DeleteT_ContactCompanyByModel(UserInfo user, T_ContactCompanyInfo model, ref string strError);

        [OperationContract]
        bool GetT_ContactCompanyByID(ref T_ContactCompanyInfo model, ref string strError);


        [OperationContract]
        bool GetT_ContactCompanyListByPage(ref List<T_ContactCompanyInfo> modelList, UserInfo user, T_ContactCompanyInfo t_contactcompany, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_ContactCompanyByHeaderID(ref List<T_ContactCompanyInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_ContactCompanyStatus(UserInfo user, ref T_ContactCompanyInfo t_contactcompany, int NewStatus, ref string strError);



        #endregion
    }
}