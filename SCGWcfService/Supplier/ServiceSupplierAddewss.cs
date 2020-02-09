using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_SupplierAddress_Func代码

        public bool SaveT_SupplierAddress(UserInfo user, ref T_SupplierAddressInfo t_supplieraddress, ref string strError)
        {
            T_SupplierAddress_Func tfunc = new T_SupplierAddress_Func();
            return tfunc.SaveModelToDB(user, ref t_supplieraddress, ref strError);
        }


        public bool DeleteT_SupplierAddressByModel(UserInfo user, T_SupplierAddressInfo model, ref string strError)
        {
            T_SupplierAddress_Func tfunc = new T_SupplierAddress_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_SupplierAddressByID(ref T_SupplierAddressInfo model, ref string strError)
        {
            T_SupplierAddress_Func tfunc = new T_SupplierAddress_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_SupplierAddressListByPage(ref List<T_SupplierAddressInfo> modelList, UserInfo user, T_SupplierAddressInfo t_supplieraddress, ref DividPage page, ref string strError)
        {
            T_SupplierAddress_Func tfunc = new T_SupplierAddress_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_supplieraddress, ref page, ref strError);
        }


        public bool GetAllT_SupplierAddressByHeaderID(ref List<T_SupplierAddressInfo> modelList, int headerID, ref string strError)
        {
            T_SupplierAddress_Func tfunc = new T_SupplierAddress_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_SupplierAddressStatus(UserInfo user, ref T_SupplierAddressInfo t_supplieraddress, int NewStatus, ref string strError)
        {
            T_SupplierAddress_Func tfunc = new T_SupplierAddress_Func();
            return tfunc.UpdateModelStatus(user, ref t_supplieraddress, NewStatus, ref strError);
        }


        #endregion
    }
}