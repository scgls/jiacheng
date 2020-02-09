using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.EdateChange;
using BILBasic.Common;
using BILBasic.User;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_EDateChange_Func代码

        public bool SaveT_EDateChange(UserInfo user, ref T_EDateChangeInfo t_edatechange, ref string strError)
        {
            T_EDateChange_Func tfunc = new T_EDateChange_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_edatechange, ref strError);
        }


        public bool DeleteT_EDateChangeByModel(UserInfo user, T_EDateChangeInfo model, ref string strError)
        {
            T_EDateChange_Func tfunc = new T_EDateChange_Func();
            return tfunc.DeleteModelByModelSql(user, model, ref strError);
        }




        public bool GetT_EDateChangeByID(ref T_EDateChangeInfo model, ref string strError)
        {
            T_EDateChange_Func tfunc = new T_EDateChange_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_EDateChangeListByPage(ref List<T_EDateChangeInfo> modelList, UserInfo user, T_EDateChangeInfo t_edatechange, ref DividPage page, ref string strError)
        {
            T_EDateChange_Func tfunc = new T_EDateChange_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_edatechange, ref page, ref strError);
        }


        public bool GetAllT_EDateChangeByHeaderID(ref List<T_EDateChangeInfo> modelList, int headerID, ref string strError)
        {
            T_EDateChange_Func tfunc = new T_EDateChange_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_EDateChangeStatus(UserInfo user, ref T_EDateChangeInfo t_edatechange, int NewStatus, ref string strError)
        {
            T_EDateChange_Func tfunc = new T_EDateChange_Func();
            return tfunc.UpdateModelStatus(user, ref t_edatechange, NewStatus, ref strError);
        }


        #endregion

        #region 自动生成WCF调用T_EDateChangeDetail_Func代码

        public bool SaveT_EDateChangeDetail(UserInfo user, ref T_EDateChangeDetailInfo t_edatechangedetail, ref string strError)
        {
            T_EDateChangeDetail_Func tfunc = new T_EDateChangeDetail_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_edatechangedetail, ref strError);
        }


        public bool DeleteT_EDateChangeDetailByModel(UserInfo user, T_EDateChangeDetailInfo model, ref string strError)
        {
            T_EDateChangeDetail_Func tfunc = new T_EDateChangeDetail_Func();
            return tfunc.DeleteModelByModelSql(user, model, ref strError);
        }




        public bool GetT_EDateChangeDetailByID(ref T_EDateChangeDetailInfo model, ref string strError)
        {
            T_EDateChangeDetail_Func tfunc = new T_EDateChangeDetail_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_EDateChangeDetailListByPage(ref List<T_EDateChangeDetailInfo> modelList, UserInfo user, T_EDateChangeDetailInfo t_edatechangedetail, ref DividPage page, ref string strError)
        {
            T_EDateChangeDetail_Func tfunc = new T_EDateChangeDetail_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_edatechangedetail, ref page, ref strError);
        }


        public bool GetAllT_EDateChangeDetailByHeaderID(ref List<T_EDateChangeDetailInfo> modelList, int headerID, ref string strError)
        {
            T_EDateChangeDetail_Func tfunc = new T_EDateChangeDetail_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_EDateChangeDetailStatus(UserInfo user, ref T_EDateChangeDetailInfo t_edatechangedetail, int NewStatus, ref string strError)
        {
            T_EDateChangeDetail_Func tfunc = new T_EDateChangeDetail_Func();
            return tfunc.UpdateModelStatus(user, ref t_edatechangedetail, NewStatus, ref strError);
        }


        #endregion

        public bool PostEDateChange(UserInfo user, List<T_EDateChangeDetailInfo> modelList, ref string strErrMsg) 
        {
            T_EDateChangeDetail_Func tfunc = new T_EDateChangeDetail_Func();
            return tfunc.PostEDateChange(user, modelList, ref strErrMsg);
        }
 
    }
}