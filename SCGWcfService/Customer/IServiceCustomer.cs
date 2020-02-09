using BILBasic.Common;
using BILWeb.Customer;
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
        #region 自动生成WCF接口方法T_CUSTOMER_Func代码
        [OperationContract]
        bool SaveT_Customer(UserInfo user, ref T_CustomerInfo t_customer, ref string strError);


        [OperationContract]
        bool DeleteT_CustomerByModel(UserInfo user, T_CustomerInfo model, ref string strError);

        [OperationContract]
        bool GetT_CustomerByID(ref T_CustomerInfo model, ref string strError);


        [OperationContract]
        bool GetT_CustomerListByPage(ref List<T_CustomerInfo> modelList, UserInfo user, T_CustomerInfo t_customer, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_CustomerByHeaderID(ref List<T_CustomerInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_CustomerStatus(UserInfo user, ref T_CustomerInfo t_customer, int NewStatus, ref string strError);



        #endregion

        #region 自动生成WCF接口方法T_CUSTOMERADDRESS_Func代码
        [OperationContract]
        bool SaveT_CustomerAddress(UserInfo user, ref T_CustomerAddressInfo t_customeraddress, ref string strError);


        [OperationContract]
        bool DeleteT_CustomerAddressByModel(UserInfo user, T_CustomerAddressInfo model, ref string strError);

        [OperationContract]
        bool GetT_CustomerAddressByID(ref T_CustomerAddressInfo model, ref string strError);


        [OperationContract]
        bool GetT_CustomerAddressListByPage(ref List<T_CustomerAddressInfo> modelList, UserInfo user, T_CustomerAddressInfo t_customeraddress, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_CustomerAddressByHeaderID(ref List<T_CustomerAddressInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_CustomerAddressStatus(UserInfo user, ref T_CustomerAddressInfo t_customeraddress, int NewStatus, ref string strError);



        #endregion
    }
}