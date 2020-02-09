using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.Quality;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_Quality_Func代码

        public bool SaveT_Quality(UserInfo user, ref T_QualityInfo t_quality, ref string strError)
        {
            T_Quality_Func tfunc = new T_Quality_Func();
            return tfunc.SaveModelToDB(user, ref t_quality, ref strError);
        }


        public bool DeleteT_QualityByModel(UserInfo user, T_QualityInfo model, ref string strError)
        {
            T_Quality_Func tfunc = new T_Quality_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_QualityByID(ref T_QualityInfo model, ref string strError)
        {
            T_Quality_Func tfunc = new T_Quality_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_QualityListByPage(ref List<T_QualityInfo> modelList, UserInfo user, T_QualityInfo t_quality, ref DividPage page, ref string strError)
        {
            T_Quality_Func tfunc = new T_Quality_Func();            
            return tfunc.GetModelListByPage(ref modelList, user, t_quality, ref page, ref strError);
        }


        public bool GetAllT_QualityByHeaderID(ref List<T_QualityInfo> modelList, int headerID, ref string strError)
        {
            T_Quality_Func tfunc = new T_Quality_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_QualityStatus(UserInfo user, ref T_QualityInfo t_quality, int NewStatus, ref string strError)
        {
            T_Quality_Func tfunc = new T_Quality_Func();
            return tfunc.UpdateModelStatus(user, ref t_quality, NewStatus, ref strError);
        }

        public bool GetT_AllQualityList(ref List<T_QualityInfo> modelList, ref string strError) 
        {
            T_Quality_Func tfunc = new T_Quality_Func();
            return tfunc.GetT_AllQualityList(ref modelList, ref strError);
        }

        #endregion
    }
}