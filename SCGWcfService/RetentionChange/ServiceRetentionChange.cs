using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.RetentionChange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_RetentionChange_Func代码

        public bool SaveT_RetentionChange(UserInfo user, ref T_RetentionChangeInfo t_retentionchange, ref string strError)
        {
            T_RetentionChange_Func tfunc = new T_RetentionChange_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_retentionchange, ref strError);
        }


        public bool DeleteT_RetentionChangeByModel(UserInfo user, T_RetentionChangeInfo model, ref string strError)
        {
            T_RetentionChange_Func tfunc = new T_RetentionChange_Func();
            return tfunc.DeleteModelByModelSql(user, model, ref strError);
        }




        public bool GetT_RetentionChangeByID(ref T_RetentionChangeInfo model, ref string strError)
        {
            T_RetentionChange_Func tfunc = new T_RetentionChange_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_RetentionChangeListByPage(ref List<T_RetentionChangeInfo> modelList, UserInfo user, T_RetentionChangeInfo t_retentionchange, ref DividPage page, ref string strError)
        {
            T_RetentionChange_Func tfunc = new T_RetentionChange_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_retentionchange, ref page, ref strError);
        }


        public bool GetAllT_RetentionChangeByHeaderID(ref List<T_RetentionChangeInfo> modelList, int headerID, ref string strError)
        {
            T_RetentionChange_Func tfunc = new T_RetentionChange_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_RetentionChangeStatus(UserInfo user, ref T_RetentionChangeInfo t_retentionchange, int NewStatus, ref string strError)
        {
            T_RetentionChange_Func tfunc = new T_RetentionChange_Func();
            return tfunc.UpdateModelStatus(user, ref t_retentionchange, NewStatus, ref strError);
        }


        #endregion

        #region 自动生成WCF调用T_RetentionDetailChange_Func代码

        public bool SaveT_RetentionDetailChange(UserInfo user, ref T_RetentionDetailChangeInfo t_retentiondetailchange, ref string strError)
        {
            T_RetentionDetailChange_Func tfunc = new T_RetentionDetailChange_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_retentiondetailchange, ref strError);
        }


        public bool DeleteT_RetentionDetailChangeByModel(UserInfo user, T_RetentionDetailChangeInfo model, ref string strError)
        {
            T_RetentionDetailChange_Func tfunc = new T_RetentionDetailChange_Func();
            return tfunc.DeleteModelByModelSql(user, model, ref strError);
        }




        public bool GetT_RetentionDetailChangeByID(ref T_RetentionDetailChangeInfo model, ref string strError)
        {
            T_RetentionDetailChange_Func tfunc = new T_RetentionDetailChange_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_RetentionDetailChangeListByPage(ref List<T_RetentionDetailChangeInfo> modelList, UserInfo user, T_RetentionDetailChangeInfo t_retentiondetailchange, ref DividPage page, ref string strError)
        {
            T_RetentionDetailChange_Func tfunc = new T_RetentionDetailChange_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_retentiondetailchange, ref page, ref strError);
        }


        public bool GetAllT_RetentionDetailChangeByHeaderID(ref List<T_RetentionDetailChangeInfo> modelList, int headerID, ref string strError)
        {
            T_RetentionDetailChange_Func tfunc = new T_RetentionDetailChange_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_RetentionDetailChangeStatus(UserInfo user, ref T_RetentionDetailChangeInfo t_retentiondetailchange, int NewStatus, ref string strError)
        {
            T_RetentionDetailChange_Func tfunc = new T_RetentionDetailChange_Func();
            return tfunc.UpdateModelStatus(user, ref t_retentiondetailchange, NewStatus, ref strError);
        }


        #endregion

        public bool PostRetentionChange(UserInfo user, List<T_RetentionDetailChangeInfo> modelList, ref string strErrMsg) 
        {
            T_RetentionDetailChange_Func tfunc = new T_RetentionDetailChange_Func();
            return tfunc.PostRetentionChange(user,modelList,ref strErrMsg);
        }

    }
}