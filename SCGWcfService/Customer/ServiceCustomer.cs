using BILBasic.Common;
using BILWeb.Customer;
using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_Customer_Func代码

        public bool SaveT_Customer(UserInfo user, ref T_CustomerInfo t_customer, ref string strError)
        {
            T_Customer_Func tfunc = new T_Customer_Func();
            return tfunc.SaveModelToDB(user, ref t_customer, ref strError);
        }


        public bool DeleteT_CustomerByModel(UserInfo user, T_CustomerInfo model, ref string strError)
        {
            T_Customer_Func tfunc = new T_Customer_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_CustomerByID(ref T_CustomerInfo model, ref string strError)
        {
            T_Customer_Func tfunc = new T_Customer_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_CustomerListByPage(ref List<T_CustomerInfo> modelList, UserInfo user, T_CustomerInfo t_customer, ref DividPage page, ref string strError)
        {
            T_Customer_Func tfunc = new T_Customer_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_customer, ref page, ref strError);
        }


        public bool GetAllT_CustomerByHeaderID(ref List<T_CustomerInfo> modelList, int headerID, ref string strError)
        {
            T_Customer_Func tfunc = new T_Customer_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_CustomerStatus(UserInfo user, ref T_CustomerInfo t_customer, int NewStatus, ref string strError)
        {
            T_Customer_Func tfunc = new T_Customer_Func();
            return tfunc.UpdateModelStatus(user, ref t_customer, NewStatus, ref strError);
        }


        #endregion

        #region 自动生成WCF调用T_CustomerAddress_Func代码

        public bool SaveT_CustomerAddress(UserInfo user, ref T_CustomerAddressInfo t_customeraddress, ref string strError)
        {
            T_CustomerAddress_Func tfunc = new T_CustomerAddress_Func();
            return tfunc.SaveModelToDB(user, ref t_customeraddress, ref strError);
        }


        public bool DeleteT_CustomerAddressByModel(UserInfo user, T_CustomerAddressInfo model, ref string strError)
        {
            T_CustomerAddress_Func tfunc = new T_CustomerAddress_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_CustomerAddressByID(ref T_CustomerAddressInfo model, ref string strError)
        {
            T_CustomerAddress_Func tfunc = new T_CustomerAddress_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_CustomerAddressListByPage(ref List<T_CustomerAddressInfo> modelList, UserInfo user, T_CustomerAddressInfo t_customeraddress, ref DividPage page, ref string strError)
        {
            T_CustomerAddress_Func tfunc = new T_CustomerAddress_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_customeraddress, ref page, ref strError);
        }


        public bool GetAllT_CustomerAddressByHeaderID(ref List<T_CustomerAddressInfo> modelList, int headerID, ref string strError)
        {
            T_CustomerAddress_Func tfunc = new T_CustomerAddress_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_CustomerAddressStatus(UserInfo user, ref T_CustomerAddressInfo t_customeraddress, int NewStatus, ref string strError)
        {
            T_CustomerAddress_Func tfunc = new T_CustomerAddress_Func();
            return tfunc.UpdateModelStatus(user, ref t_customeraddress, NewStatus, ref strError);
        }


        #endregion
    }
}