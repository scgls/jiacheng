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
        #region 自动生成WCF接口方法T_CONTACTCOMPANY_ADDRESS_Func代码
        [OperationContract]
        bool SaveT_ContactCompany_Address(UserInfo user, ref T_ContactCompany_AddressInfo t_contactcompany_address, ref string strError);


        [OperationContract]
        bool DeleteT_ContactCompany_AddressByModel(UserInfo user, T_ContactCompany_AddressInfo model, ref string strError);

        [OperationContract]
        bool GetT_ContactCompany_AddressByID(ref T_ContactCompany_AddressInfo model, ref string strError);


        [OperationContract]
        bool GetT_ContactCompany_AddressListByPage(ref List<T_ContactCompany_AddressInfo> modelList, UserInfo user, T_ContactCompany_AddressInfo t_contactcompany_address, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_ContactCompany_AddressByHeaderID(ref List<T_ContactCompany_AddressInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_ContactCompany_AddressStatus(UserInfo user, ref T_ContactCompany_AddressInfo t_contactcompany_address, int NewStatus, ref string strError);



        #endregion
    }
}