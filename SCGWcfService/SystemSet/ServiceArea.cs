using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.Area;
using BILWeb.Login.User;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_AREA_Func代码

        public bool SaveT_Area(UserInfo user, ref T_AreaInfo t_area, ref string strError)
        {
            T_Area_Func tfunc = new T_Area_Func();
            return tfunc.SaveModelToDB(user, ref t_area, ref strError);
        }


        public bool DeleteT_AreaByModel(UserInfo user, T_AreaInfo model, ref string strError)
        {
            T_Area_Func tfunc = new T_Area_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_AreaByID(ref T_AreaInfo model, ref string strError)
        {
            T_Area_Func tfunc = new T_Area_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_AreaListByPage(ref List<T_AreaInfo> modelList, UserInfo user, T_AreaInfo t_area, ref DividPage page, ref string strError)
        {
            T_Area_Func tfunc = new T_Area_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_area, ref page, ref strError);
        }


        public bool GetAllT_AreaByHeaderID(ref List<T_AreaInfo> modelList, int headerID, ref string strError)
        {
            T_Area_Func tfunc = new T_Area_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_AreaStatus(UserInfo user, ref T_AreaInfo t_area, int NewStatus, ref string strError)
        {
            T_Area_Func tfunc = new T_Area_Func();
            return tfunc.UpdateModelStatus(user, ref t_area, NewStatus, ref strError);
        }

        //public string GetAreaModelADF(string AreaNo) 
        //{
        //    T_Area_Func tfunc = new T_Area_Func();
        //    return tfunc.GetAreaModelBySqlADF(AreaNo);
        //}

        #endregion
    }
}