using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_Supplier_Func代码

        public bool SaveT_Supplier(UserInfo user, ref T_SupplierInfo t_supplier, ref string strError)
        {
            T_Supplier_Func tfunc = new T_Supplier_Func();
            return tfunc.SaveModelToDB(user, ref t_supplier, ref strError);
        }


        public bool DeleteT_SupplierByModel(UserInfo user, T_SupplierInfo model, ref string strError)
        {
            T_Supplier_Func tfunc = new T_Supplier_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_SupplierByID(ref T_SupplierInfo model, ref string strError)
        {
            T_Supplier_Func tfunc = new T_Supplier_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_SupplierListByPage(ref List<T_SupplierInfo> modelList, UserInfo user, T_SupplierInfo t_supplier, ref DividPage page, ref string strError)
        {
            T_Supplier_Func tfunc = new T_Supplier_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_supplier, ref page, ref strError);
        }


        public bool GetAllT_SupplierByHeaderID(ref List<T_SupplierInfo> modelList, int headerID, ref string strError)
        {
            T_Supplier_Func tfunc = new T_Supplier_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_SupplierStatus(UserInfo user, ref T_SupplierInfo t_supplier, int NewStatus, ref string strError)
        {
            T_Supplier_Func tfunc = new T_Supplier_Func();
            return tfunc.UpdateModelStatus(user, ref t_supplier, NewStatus, ref strError);
        }


        #endregion
    }
}