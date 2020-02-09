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
    public partial class ServiceWMS:IService
    {
        #region 自动生成WCF调用T_Machine_Func代码

        public bool SaveT_Machine(UserInfo user, ref T_Machine t_customer, ref string strError)
        {
            T_Machine_Func tfunc = new T_Machine_Func();
            return tfunc.SaveData(t_customer, ref strError);
        }


        public bool DeleteT_MachineByModel(UserInfo user, T_Machine model, ref string strError)
        {
            T_Machine_Func tfunc = new T_Machine_Func();
            return tfunc.DeleteDeviceByID( model, ref strError);
        }


        public bool GetT_MachineByID(ref T_Machine model, ref string strError)
        {
            T_Machine_Func tfunc = new T_Machine_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_MachineListByPage(ref List<T_Machine> modelList, UserInfo user, T_Machine t_customer, ref DividPage page, ref string strError)
        {
            T_Machine_Func tfunc = new T_Machine_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_customer, ref page, ref strError);
        }


        public bool GetAllT_MachineByHeaderID(ref List<T_Machine> modelList, int headerID, ref string strError)
        {
            T_Machine_Func tfunc = new T_Machine_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_MachineStatus(UserInfo user, ref T_Machine t_customer, int NewStatus, ref string strError)
        {
            T_Machine_Func tfunc = new T_Machine_Func();
            return tfunc.UpdateModelStatus(user, ref t_customer, NewStatus, ref strError);
        }


        #endregion

        #region 自动生成WCF调用T_MachineType_Func代码

        public bool SaveT_MachineType(UserInfo user, ref T_MachineType t_customer, ref string strError)
        {
            T_MachineType_Func tfunc = new T_MachineType_Func();
            return tfunc.SaveData(t_customer, ref strError);
        }


        public bool DeleteT_MachineTypeByModel(UserInfo user, T_MachineType model, ref string strError)
        {
            T_MachineType_Func tfunc = new T_MachineType_Func();
            return tfunc.DeleteDeviceByID(model, ref strError);
        }


        public bool GetT_MachineTypeByID(ref T_MachineType model, ref string strError)
        {
            T_MachineType_Func tfunc = new T_MachineType_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_MachineTypeListByPage(ref List<T_MachineType> modelList, UserInfo user, T_MachineType t_customer, ref DividPage page, ref string strError)
        {
            T_MachineType_Func tfunc = new T_MachineType_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_customer, ref page, ref strError);
        }


        public bool GetAllT_MachineTypeByHeaderID(ref List<T_MachineType> modelList, int headerID, ref string strError)
        {
            T_MachineType_Func tfunc = new T_MachineType_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_MachineTypeStatus(UserInfo user, ref T_MachineType t_customer, int NewStatus, ref string strError)
        {
            T_MachineType_Func tfunc = new T_MachineType_Func();
            return tfunc.UpdateModelStatus(user, ref t_customer, NewStatus, ref strError);
        }


        #endregion
    }
}