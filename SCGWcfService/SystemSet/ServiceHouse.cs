using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.House;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_House_Func代码

        public bool SaveT_House(UserInfo user, ref T_HouseInfo t_house, ref string strError)
        {
            T_House_Func tfunc = new T_House_Func();
            return tfunc.SaveModelToDB(user, ref t_house, ref strError);
        }


        public bool DeleteT_HouseByModel(UserInfo user, T_HouseInfo model, ref string strError)
        {
            T_House_Func tfunc = new T_House_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_HouseByID(ref T_HouseInfo model, ref string strError)
        {
            T_House_Func tfunc = new T_House_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_HouseListByPage(ref List<T_HouseInfo> modelList, UserInfo user, T_HouseInfo t_house, ref DividPage page, ref string strError)
        {
            T_House_Func tfunc = new T_House_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_house, ref page, ref strError);
        }


        public bool GetAllT_HouseByHeaderID(ref List<T_HouseInfo> modelList, int headerID, ref string strError)
        {
            T_House_Func tfunc = new T_House_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_HouseStatus(UserInfo user, ref T_HouseInfo t_house, int NewStatus, ref string strError)
        {
            T_House_Func tfunc = new T_House_Func();
            return tfunc.UpdateModelStatus(user, ref t_house, NewStatus, ref strError);
        }


        #endregion
    }
}