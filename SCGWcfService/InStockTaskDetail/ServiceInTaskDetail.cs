using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.InStockTask;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_InTaskDetails_Func代码

        public bool SaveT_InTaskDetails(UserInfo user, ref T_InStockTaskDetailsInfo t_taskdetails, ref string strError)
        {
            T_InTaskDetails_Func tfunc = new T_InTaskDetails_Func();
            return tfunc.SaveModelToDB(user, ref t_taskdetails, ref strError);
        }


        public bool DeleteT_InTaskDetailsByModel(UserInfo user, T_InStockTaskDetailsInfo model, ref string strError)
        {
            T_InTaskDetails_Func tfunc = new T_InTaskDetails_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_InTaskDetailsByID(ref T_InStockTaskDetailsInfo model, ref string strError)
        {
            T_InTaskDetails_Func tfunc = new T_InTaskDetails_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_InTaskDetailsListByPage(ref List<T_InStockTaskDetailsInfo> modelList, UserInfo user, T_InStockTaskDetailsInfo t_taskdetails, ref DividPage page, ref string strError)
        {
            T_InTaskDetails_Func tfunc = new T_InTaskDetails_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_taskdetails, ref page, ref strError);
        }


        public bool GetAllT_InTaskDetailsByHeaderID(ref List<T_InStockTaskDetailsInfo> modelList, int headerID, ref string strError)
        {
            T_InTaskDetails_Func tfunc = new T_InTaskDetails_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_InTaskDetailsStatus(UserInfo user, ref T_InStockTaskDetailsInfo t_taskdetails, int NewStatus, ref string strError)
        {
            T_InTaskDetails_Func tfunc = new T_InTaskDetails_Func();
            return tfunc.UpdateModelStatus(user, ref t_taskdetails, NewStatus, ref strError);
        }

        public string GetT_InTaskDetailListByHeaderIDADF(string ModelDetailJson)
        {
            T_InTaskDetails_Func tfunc = new T_InTaskDetails_Func();
            return tfunc.GetModelListByHeaderIDADF(ModelDetailJson);
        }

        public string LockTaskOperUserADF(string TaskDetailsJson, string UserJson) 
        {
            T_InTaskDetails_Func tfunc = new T_InTaskDetails_Func();
            return tfunc.LockTaskOperUser(TaskDetailsJson, UserJson);
        }

        public string UnLockTaskOperUserADF(string TaskDetailsJson, string UserJson) 
        {
            T_InTaskDetails_Func tfunc = new T_InTaskDetails_Func();
            return tfunc.UnLockTaskOperUser(TaskDetailsJson, UserJson);
        }

        #endregion

        public bool GetExportTaskDetail(T_InStockTaskInfo model, ref List<T_InStockTaskDetailsInfo> modelList, ref string strErrMsg) 
        {
            T_InTaskDetails_Func tfunc = new T_InTaskDetails_Func();
            return tfunc.GetExportTaskDetail(model, ref modelList, ref strErrMsg);
        }
    }
}