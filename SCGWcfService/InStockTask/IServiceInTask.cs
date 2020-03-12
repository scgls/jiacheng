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
        #region 自动生成WCF接口方法T_TASK_Func代码
        [OperationContract]
        bool SaveT_InTask(UserInfo user, ref T_InStockTaskInfo t_task, ref string strError);


        [OperationContract]
        bool DeleteT_InTaskByModel(UserInfo user, T_InStockTaskInfo model, ref string strError);

        [OperationContract]
        bool GetT_InTaskByID(ref T_InStockTaskInfo model, ref string strError);


        [OperationContract]
        bool GetT_InTaskListByPage(ref List<T_InStockTaskInfo> modelList, UserInfo user, T_InStockTaskInfo t_task, ref DividPage page, ref string strError);
               


        [OperationContract]
        bool GetAllT_InTaskByHeaderID(ref List<T_InStockTaskInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_InTaskStatus(UserInfo user, ref T_InStockTaskInfo t_task,  ref string strError);


        [OperationContract]
        string GetT_InTaskListADF(string UserJosn, string ModelJson);

        

        #endregion

      
    }
}