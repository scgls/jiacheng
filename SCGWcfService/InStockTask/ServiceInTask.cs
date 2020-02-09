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
        #region 自动生成WCF调用T_InStockTask_Func代码

        public bool SaveT_InTask(UserInfo user, ref T_InStockTaskInfo t_task, ref string strError)
        {
            T_InStockTask_Func tfunc = new T_InStockTask_Func();
            return tfunc.SaveModelToDB(user, ref t_task, ref strError);
        }


        public bool DeleteT_InTaskByModel(UserInfo user, T_InStockTaskInfo model, ref string strError)
        {
            T_InStockTask_Func tfunc = new T_InStockTask_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_InTaskByID(ref T_InStockTaskInfo model, ref string strError)
        {
            T_InStockTask_Func tfunc = new T_InStockTask_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_InTaskListByPage(ref List<T_InStockTaskInfo> modelList, UserInfo user, T_InStockTaskInfo t_task, ref DividPage page, ref string strError)
        {
            T_InStockTask_Func tfunc = new T_InStockTask_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_task, ref page, ref strError);
        }


        public bool GetAllT_InTaskByHeaderID(ref List<T_InStockTaskInfo> modelList, int headerID, ref string strError)
        {
            T_InStockTask_Func tfunc = new T_InStockTask_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_InTaskStatus(UserInfo user, ref T_InStockTaskInfo t_task, ref string strError)
        {
            T_InStockTask_Func tfunc = new T_InStockTask_Func();
            return tfunc.UpadteModelByModelSql(user,t_task, ref strError);
        }

        public string GetT_InTaskListADF(string UserJosn, string ModelJson)
        {
            T_InStockTask_Func tfunc = new T_InStockTask_Func();
            return tfunc.GetModelListADF(UserJosn, ModelJson);
        }

        
        #endregion


        
    }
}