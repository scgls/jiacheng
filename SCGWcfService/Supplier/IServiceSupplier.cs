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
        #region 自动生成WCF接口方法T_SUPPLIER_Func代码
        [OperationContract]
        bool SaveT_Supplier(UserInfo user, ref T_SupplierInfo t_supplier, ref string strError);


        [OperationContract]
        bool DeleteT_SupplierByModel(UserInfo user, T_SupplierInfo model, ref string strError);

        [OperationContract]
        bool GetT_SupplierByID(ref T_SupplierInfo model, ref string strError);


        [OperationContract]
        bool GetT_SupplierListByPage(ref List<T_SupplierInfo> modelList, UserInfo user, T_SupplierInfo t_supplier, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_SupplierByHeaderID(ref List<T_SupplierInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_SupplierStatus(UserInfo user, ref T_SupplierInfo t_supplier, int NewStatus, ref string strError);



        #endregion
    }
}