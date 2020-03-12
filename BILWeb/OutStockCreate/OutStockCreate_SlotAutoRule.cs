using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILWeb.Login.User;
using BILWeb.OutStockTask;

namespace BILWeb.OutStockCreate
{
    public class OutStockCreate_SlotAutoRule : OutStockCreate_SplitBaseRule<T_OutStockCreateInfo, T_OutStockCreateInfo>
    {
        public override void GetOutStockCreateAutoSlotList(ref List<T_OutStockCreateInfo> modelList)
        {
            base.GetOutStockCreateAutoSlotList(ref modelList);
            ////先查询所有已经登录的拣货人
            //List<UserInfo> userList = new List<UserInfo>();
            //User_Func userFunc = new User_Func();
            //UserInfo userInfo = new UserInfo();
            //BILBasic.User.UserModel userModel = new BILBasic.User.UserModel();
            ////userInfo.LoginTime = DateTime.Now;
            //userInfo.IsPick = 2;
            //BILBasic.Common.DividPage userPage = new BILBasic.Common.DividPage();
            //string strError = "";

            //userFunc.GetModelListByPage(ref userList,userModel, userInfo, ref userPage, ref strError);

            ////查找所有未完成的拣货单
            //T_OutStockTask_Func taskFunc = new T_OutStockTask_Func();
            //List<T_OutStockTaskInfo> taskList = new List<T_OutStockTaskInfo>();
            //T_OutStockTaskInfo taskModel = new T_OutStockTaskInfo();
            //taskModel.Status=1;
            //BILBasic.Common.DividPage taskPage = new BILBasic.Common.DividPage();
            //taskFunc.GetModelListByPage(ref taskList, userModel, taskModel, ref taskPage, ref strError);

            //var taskCountList = from t in taskList
            //                group t by new { t1 = t.AuditUserNo } into m
            //                select new
            //                {
            //                    AuditUserNo = m.Key.t1,
            //                    TaskCount = m.Count()                                
            //                };

            //userList.ForEach(m =>
            //       {
            //           var s = taskCountList.FirstOrDefault(n => n.AuditUserNo == m.UserNo);
            //           if (s != null)
            //           {
            //               m.TaskCount = s.TaskCount;
            //           }
            //       });

            //userList.OrderByDescending(t => t.TaskCount);

            //int maxCount = taskCountList.Max(t => t.TaskCount);
            //int minCount = taskCountList.Min(t => t.TaskCount);


            //if (maxCount == minCount)
            //{
            //    foreach (var itemUserTask in userList)
            //    {
            //        var newModelList = modelList.Where(t => t.Auditor != "");

            //        if (newModelList == null || newModelList.Count() == 0)
            //        {
            //            break;
            //        }
            //        else
            //        {
            //            modelList.Where(t => t.Auditor != "").ToList().First().Auditor = itemUserTask.UserNo;
            //        }
            //    }
            //}
            //else 
            //{

            //}
            
            
            //先分配给拣货任务最少的拣货人

        }
    }
}
