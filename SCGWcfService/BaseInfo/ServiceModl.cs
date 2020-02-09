using BILBasic.Common;
using BILWeb.BaseInfo;
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
        #region 自动生成WCF调用T_Mould_Func代码

        public bool SaveT_Modl(UserInfo user, ref T_Modl t_customer, ref string strError)
        {
            T_Modl_Func tfunc = new T_Modl_Func();
            return tfunc.SaveData(t_customer, ref strError);
        }


        public bool DeleteT_ModlByModel(UserInfo user, T_Modl model, ref string strError)
        {
            T_Modl_Func tfunc = new T_Modl_Func();
            return tfunc.DeletModlByID( model, ref strError);
        }




        public bool GetT_ModlByID(ref T_Modl model, ref string strError)
        {
            T_Modl_Func tfunc = new T_Modl_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_ModlListByPage(ref List<T_Modl> modelList, UserInfo user, T_Modl t_customer, ref DividPage page, ref string strError)
        {
            T_Modl_Func tfunc = new T_Modl_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_customer, ref page, ref strError);
        }


        public bool GetAllT_ModlByHeaderID(ref List<T_Modl> modelList, int headerID, ref string strError)
        {
            T_Modl_Func tfunc = new T_Modl_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_ModlStatus(UserInfo user, ref T_Modl t_customer, int NewStatus, ref string strError)
        {
            T_Modl_Func tfunc = new T_Modl_Func();
            return tfunc.UpdateModelStatus(user, ref t_customer, NewStatus, ref strError);
        }


        #endregion

        #region 自动生成WCF调用T_MouldType_Func代码

        public bool SaveT_MouldType(UserInfo user, ref T_MouldType t_customer, ref string strError)
        {
            T_MouldType_Func tfunc = new T_MouldType_Func();
            return tfunc.SaveData(t_customer, ref strError);
        }


        public bool DeleteT_MouldTypeByModel(UserInfo user, T_MouldType model, ref string strError)
        {
            T_MouldType_Func tfunc = new T_MouldType_Func();
            return tfunc.DeleteMouldTypeByID(model, ref strError);
        }


        public bool GetT_MouldTypeByID(ref T_MouldType model, ref string strError)
        {
            T_MouldType_Func tfunc = new T_MouldType_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_MouldTypeListByPage(ref List<T_MouldType> modelList, UserInfo user, T_MouldType t_customer, ref DividPage page, ref string strError)
        {
            T_MouldType_Func tfunc = new T_MouldType_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_customer, ref page, ref strError);
        }


        public bool GetAllT_MouldTypeByHeaderID(ref List<T_MouldType> modelList, int headerID, ref string strError)
        {
            T_MouldType_Func tfunc = new T_MouldType_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_MouldTypeStatus(UserInfo user, ref T_MouldType t_customer, int NewStatus, ref string strError)
        {
            T_MouldType_Func tfunc = new T_MouldType_Func();
            return tfunc.UpdateModelStatus(user, ref t_customer, NewStatus, ref strError);
        }


        #endregion
    }
}