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
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_SUPPLIERADDRESS_Func代码
        [OperationContract]
        bool SaveT_SupplierAddress(UserInfo user, ref T_SupplierAddressInfo t_supplieraddress, ref string strError);


        [OperationContract]
        bool DeleteT_SupplierAddressByModel(UserInfo user, T_SupplierAddressInfo model, ref string strError);

        [OperationContract]
        bool GetT_SupplierAddressByID(ref T_SupplierAddressInfo model, ref string strError);


        [OperationContract]
        bool GetT_SupplierAddressListByPage(ref List<T_SupplierAddressInfo> modelList, UserInfo user, T_SupplierAddressInfo t_supplieraddress, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_SupplierAddressByHeaderID(ref List<T_SupplierAddressInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_SupplierAddressStatus(UserInfo user, ref T_SupplierAddressInfo t_supplieraddress, int NewStatus, ref string strError);



        #endregion
    }
}