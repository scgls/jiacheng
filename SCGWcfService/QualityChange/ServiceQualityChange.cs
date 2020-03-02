using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.QualityChange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_QualityChange_Func代码

        public bool SaveT_QualityChange(UserInfo user, ref T_QualityChangeInfo t_qualitychange, ref string strError)
        {
            T_QualityChange_Func tfunc = new T_QualityChange_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_qualitychange, ref strError);
        }


        public bool DeleteT_QualityChangeByModel(UserInfo user, T_QualityChangeInfo model, ref string strError)
        {
            T_QualityChange_Func tfunc = new T_QualityChange_Func();
            return tfunc.DeleteModelByModelSql(user, model, ref strError);
        }




        public bool GetT_QualityChangeByID(ref T_QualityChangeInfo model, ref string strError)
        {
            T_QualityChange_Func tfunc = new T_QualityChange_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_QualityChangeListByPage(ref List<T_QualityChangeInfo> modelList, UserInfo user, T_QualityChangeInfo t_qualitychange, ref DividPage page, ref string strError)
        {
            T_QualityChange_Func tfunc = new T_QualityChange_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_qualitychange, ref page, ref strError);
        }


        public bool GetAllT_QualityChangeByHeaderID(ref List<T_QualityChangeInfo> modelList, int headerID, ref string strError)
        {
            T_QualityChange_Func tfunc = new T_QualityChange_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_QualityChangeStatus(UserInfo user, ref T_QualityChangeInfo t_qualitychange, int NewStatus, ref string strError)
        {
            T_QualityChange_Func tfunc = new T_QualityChange_Func();
            return tfunc.UpdateModelStatus(user, ref t_qualitychange, NewStatus, ref strError);
        }


        #endregion

        #region 自动生成WCF调用T_QualityChangeDetail_Func代码

        public bool SaveT_QualityChangeDetail(UserInfo user, ref T_QualityChangeDetailInfo t_qualitychangedetail, ref string strError)
        {
            T_QualityChangeDetail_Func tfunc = new T_QualityChangeDetail_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_qualitychangedetail, ref strError);
        }


        public bool DeleteT_QualityChangeDetailByModel(UserInfo user, T_QualityChangeDetailInfo model, ref string strError)
        {
            T_QualityChangeDetail_Func tfunc = new T_QualityChangeDetail_Func();
            return tfunc.DeleteModelByModelSql(user, model, ref strError);
        }




        public bool GetT_QualityChangeDetailByID(ref T_QualityChangeDetailInfo model, ref string strError)
        {
            T_QualityChangeDetail_Func tfunc = new T_QualityChangeDetail_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_QualityChangeDetailListByPage(ref List<T_QualityChangeDetailInfo> modelList, UserInfo user, T_QualityChangeDetailInfo t_qualitychangedetail, ref DividPage page, ref string strError)
        {
            T_QualityChangeDetail_Func tfunc = new T_QualityChangeDetail_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_qualitychangedetail, ref page, ref strError);
        }


        public bool GetAllT_QualityChangeDetailByHeaderID(ref List<T_QualityChangeDetailInfo> modelList, int headerID, ref string strError)
        {
            T_QualityChangeDetail_Func tfunc = new T_QualityChangeDetail_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_QualityChangeDetailStatus(UserInfo user, ref T_QualityChangeDetailInfo t_qualitychangedetail, int NewStatus, ref string strError)
        {
            T_QualityChangeDetail_Func tfunc = new T_QualityChangeDetail_Func();
            return tfunc.UpdateModelStatus(user, ref t_qualitychangedetail, NewStatus, ref strError);
        }


        #endregion

        public bool PostQualityChange(UserInfo user, List<T_QualityChangeDetailInfo> modelList, ref string strErrMsg)
        {
            T_QualityChangeDetail_Func tfunc = new T_QualityChangeDetail_Func();
            return tfunc.PostQualityChange(user, modelList, ref strErrMsg);
        }
    }
}