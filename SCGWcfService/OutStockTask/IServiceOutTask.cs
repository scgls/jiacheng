using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.OutStockTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace SCGWcfService
{
    public partial interface IService
    {
        [OperationContract]
        string GetT_OutTaskListADF(string UserJosn, string ModelJson);

        #region 自动生成WCF接口方法T_TASK_Func代码
        [OperationContract]
        bool SaveT_OutTask(UserInfo user, ref T_OutStockTaskInfo t_task, ref string strError);


        [OperationContract]
        bool DeleteT_OutTaskByModel(UserInfo user, T_OutStockTaskInfo model, ref string strError);

        [OperationContract]
        bool GetT_OutTaskByID(ref T_OutStockTaskInfo model, ref string strError);


        [OperationContract]
        bool GetT_OutTaskListByPage(ref List<T_OutStockTaskInfo> modelList, UserInfo user, T_OutStockTaskInfo t_task, ref DividPage page, ref string strError);



        [OperationContract]
        bool GetAllT_OutTaskByHeaderID(ref List<T_OutStockTaskInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_OutTaskStatus(UserInfo user, ref T_OutStockTaskInfo t_task,  ref string strError);


        



        #endregion

        #region 自动生成WCF接口方法T_TASKDETAILS_Func代码
        [OperationContract]
        bool SaveT_OutTaskDetails(UserInfo user, ref T_OutStockTaskDetailsInfo t_taskdetails, ref string strError);


        [OperationContract]
        bool DeleteT_OutTaskDetailsByModel(UserInfo user, T_OutStockTaskDetailsInfo model, ref string strError);

        [OperationContract]
        bool GetT_OutTaskDetailsByID(ref T_OutStockTaskDetailsInfo model, ref string strError);


        [OperationContract]
        bool GetT_OutTaskDetailsListByPage(ref List<T_OutStockTaskDetailsInfo> modelList, UserInfo user, T_OutStockTaskDetailsInfo t_taskdetails, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_OutTaskDetailsByHeaderID(ref List<T_OutStockTaskDetailsInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_OutTaskDetailsStatus(UserInfo user, ref T_OutStockTaskDetailsInfo t_taskdetails, int NewStatus, ref string strError);

     

      
        #endregion

        [OperationContract]
        bool GetExportOutTaskDetail(T_OutStockTaskInfo model, ref List<T_OutStockTaskDetailsInfo> modelList, ref string strErrMsg);
    }
}