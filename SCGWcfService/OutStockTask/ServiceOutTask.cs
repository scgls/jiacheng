using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.OutStockTask;
using BILWeb.Login.User;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        public string GetT_OutTaskListADF(string UserJosn, string ModelJson)
        {
            T_OutStockTask_Func tfunc = new T_OutStockTask_Func();
            return tfunc.GetModelListADF(UserJosn, ModelJson);
        }

        #region 自动生成WCF调用T_OutStockTask_Func代码

        public bool SaveT_OutTask(UserInfo user, ref T_OutStockTaskInfo t_task, ref string strError)
        {
            T_OutStockTask_Func tfunc = new T_OutStockTask_Func();
            return tfunc.SaveModelToDB(user, ref t_task, ref strError);
        }


        public bool DeleteT_OutTaskByModel(UserInfo user, T_OutStockTaskInfo model, ref string strError)
        {
            T_OutStockTask_Func tfunc = new T_OutStockTask_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_OutTaskByID(ref T_OutStockTaskInfo model, ref string strError)
        {
            T_OutStockTask_Func tfunc = new T_OutStockTask_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_OutTaskListByPage(ref List<T_OutStockTaskInfo> modelList, UserInfo user, T_OutStockTaskInfo t_task, ref DividPage page, ref string strError)
        {
            T_OutStockTask_Func tfunc = new T_OutStockTask_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_task, ref page, ref strError);
        }


        public bool GetAllT_OutTaskByHeaderID(ref List<T_OutStockTaskInfo> modelList, int headerID, ref string strError)
        {
            T_OutStockTask_Func tfunc = new T_OutStockTask_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_OutTaskStatus(UserInfo user, ref T_OutStockTaskInfo t_task, ref string strError)
        {
            T_OutStockTask_Func tfunc = new T_OutStockTask_Func();
            return tfunc.UpadteModelByModelSql(user, t_task, ref strError);
        }

     


        #endregion

        #region 自动生成WCF调用T_OutTaskDetails_Func代码

        public bool SaveT_OutTaskDetails(UserInfo user, ref T_OutStockTaskDetailsInfo t_taskdetails, ref string strError)
        {
            T_OutTaskDetails_Func tfunc = new T_OutTaskDetails_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_taskdetails, ref strError);
        }


        public bool DeleteT_OutTaskDetailsByModel(UserInfo user, T_OutStockTaskDetailsInfo model, ref string strError)
        {
            T_OutTaskDetails_Func tfunc = new T_OutTaskDetails_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_OutTaskDetailsByID(ref T_OutStockTaskDetailsInfo model, ref string strError)
        {
            T_OutTaskDetails_Func tfunc = new T_OutTaskDetails_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_OutTaskDetailsListByPage(ref List<T_OutStockTaskDetailsInfo> modelList, UserInfo user, T_OutStockTaskDetailsInfo t_taskdetails, ref DividPage page, ref string strError)
        {
            T_OutTaskDetails_Func tfunc = new T_OutTaskDetails_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_taskdetails, ref page, ref strError);
        }


        public bool GetAllT_OutTaskDetailsByHeaderID(ref List<T_OutStockTaskDetailsInfo> modelList, int headerID, ref string strError)
        {
            T_OutTaskDetails_Func tfunc = new T_OutTaskDetails_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_OutTaskDetailsStatus(UserInfo user, ref T_OutStockTaskDetailsInfo t_taskdetails, int NewStatus, ref string strError)
        {
            T_OutTaskDetails_Func tfunc = new T_OutTaskDetails_Func();
            return tfunc.UpdateModelStatus(user, ref t_taskdetails, NewStatus, ref strError);
        }

       

        #endregion

        public bool GetExportOutTaskDetail(T_OutStockTaskInfo model, ref List<T_OutStockTaskDetailsInfo> modelList, ref string strErrMsg)
        {
            T_OutTaskDetails_Func tfunc = new T_OutTaskDetails_Func();
            return tfunc.GetExportTaskDetail(model, ref modelList, ref strErrMsg);
        }
    }
}