using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.Warehouse;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_WareHouse_Func代码

        public bool SaveT_WareHouse(UserInfo user, ref T_WareHouseInfo t_warehouse, ref string strError)
        {
            T_WareHouse_Func tfunc = new T_WareHouse_Func();
            return tfunc.SaveModelToDB(user, ref t_warehouse, ref strError);
        }


        public bool DeleteT_WareHouseByModel(UserInfo user, T_WareHouseInfo model, ref string strError)
        {
            T_WareHouse_Func tfunc = new T_WareHouse_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_WareHouseByID(ref T_WareHouseInfo model, ref string strError)
        {
            T_WareHouse_Func tfunc = new T_WareHouse_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_WareHouseListByPage(ref List<T_WareHouseInfo> modelList, UserInfo user, T_WareHouseInfo t_warehouse, ref DividPage page, ref string strError)
        {
            T_WareHouse_Func tfunc = new T_WareHouse_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_warehouse, ref page, ref strError);
        }


        public bool GetAllT_WareHouseByHeaderID(ref List<T_WareHouseInfo> modelList, int headerID, ref string strError)
        {
            T_WareHouse_Func tfunc = new T_WareHouse_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_WareHouseStatus(UserInfo user, ref T_WareHouseInfo t_warehouse, int NewStatus, ref string strError)
        {
            T_WareHouse_Func tfunc = new T_WareHouse_Func();
            return tfunc.UpdateModelStatus(user, ref t_warehouse, NewStatus, ref strError);
        }


        #endregion
    }
}