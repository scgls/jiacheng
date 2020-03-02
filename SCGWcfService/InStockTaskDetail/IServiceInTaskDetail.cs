using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using BILWeb.InStockTask;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_TASKDETAILS_Func代码
        [OperationContract]
        bool SaveT_InTaskDetails(UserInfo user, ref T_InStockTaskDetailsInfo t_taskdetails, ref string strError);


        [OperationContract]
        bool DeleteT_InTaskDetailsByModel(UserInfo user, T_InStockTaskDetailsInfo model, ref string strError);

        [OperationContract]
        bool GetT_InTaskDetailsByID(ref T_InStockTaskDetailsInfo model, ref string strError);


        [OperationContract]
        bool GetT_InTaskDetailsListByPage(ref List<T_InStockTaskDetailsInfo> modelList, UserInfo user, T_InStockTaskDetailsInfo t_taskdetails, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_InTaskDetailsByHeaderID(ref List<T_InStockTaskDetailsInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_InTaskDetailsStatus(UserInfo user, ref T_InStockTaskDetailsInfo t_taskdetails, int NewStatus, ref string strError);

        [OperationContract]
        string GetT_InTaskDetailListByHeaderIDADF(string ModelDetailJson);

        [OperationContract]
        string LockTaskOperUserADF(string TaskDetailsJson, string UserJson);

        [OperationContract]
        string UnLockTaskOperUserADF(string TaskDetailsJson, string UserJson);
        #endregion

        [OperationContract]
        bool GetExportTaskDetail(T_InStockTaskInfo model, ref List<T_InStockTaskDetailsInfo> modelList, ref string strErrMsg);
    }
}