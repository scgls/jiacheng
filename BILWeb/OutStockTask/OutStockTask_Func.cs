using BILBasic.Basing.Factory;
using BILBasic.JSONUtil;
using BILBasic.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.OutStockTask
{
    public partial class T_OutStockTask_Func : TBase_Func<T_OutStockTask_DB, T_OutStockTaskInfo>, IOutStockTaskService
    {

        protected override bool CheckModelBeforeSave(T_OutStockTaskInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            return true;
        }

        protected override string GetModelChineseName()
        {
            return "拣货单列表";
        }


        protected override T_OutStockTaskInfo GetModelByJson(string ModelJson)
        {
            return JSONHelper.JsonToObject<T_OutStockTaskInfo>(ModelJson);
        }

        /// <summary>
        ///分配拣货任务对应拣货人
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="ModelJson"></param>
        /// <returns></returns>
        public string SavePickUserListADF(string UserJson, string ModelJson) 
        {
            BaseMessage_Model<List<T_OutStockTaskInfo>> messageModel = new BaseMessage_Model<List<T_OutStockTaskInfo>>();

            try
            {
                string strError = string.Empty;                

                if (string.IsNullOrEmpty(UserJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "用户端传来用户JSON为空！";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutStockTaskInfo>>>(messageModel);
                }

                if (string.IsNullOrEmpty(ModelJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来拣货单JSON为空！";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutStockTaskInfo>>>(messageModel);
                }

                List<UserModel> UserList = JSONHelper.JsonToObject< List<UserModel>> (UserJson);

                if (UserList == null || UserList.Count == 0) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "用户JSON转换用户列表为空！";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutStockTaskInfo>>>(messageModel);
                }

                List<T_OutStockTaskInfo> modelList = JSONHelper.JsonToObject<List<T_OutStockTaskInfo>>(ModelJson);
                if (modelList == null || modelList.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "拣货单JSON转换拣货单列表为空！";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutStockTaskInfo>>>(messageModel);
                }

                T_OutStockTask_DB _db = new T_OutStockTask_DB();
                if (_db.SavePickUserList(UserList, modelList, ref strError) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutStockTaskInfo>>>(messageModel);
                }
                else 
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.Message = "拣货任务分配成功！";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutStockTaskInfo>>>(messageModel);
                }

            }
            catch (Exception ex) 
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutStockTaskInfo>>>(messageModel);
            }
        }

        #region 锁定拣货任务

        /// <summary>
        /// 锁定拣货单号
        /// </summary>
        /// <param name="TaskOutStockDetailsJson"></param>
        /// <param name="UserJson"></param>
        /// <returns></returns>
        public string LockTaskOperUser(string TaskOutStockModelJson, string UserJson)
        {
            BaseMessage_Model<T_OutStockTaskInfo> messageModel = new BaseMessage_Model<T_OutStockTaskInfo>();

            try
            {
                int iLock = 0;
                string strUserName = string.Empty;

                if (string.IsNullOrEmpty(TaskOutStockModelJson)) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来业务JSON为空";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockTaskInfo>>(messageModel);
                }

                T_OutStockTaskInfo taskDetailsModel = BILBasic.JSONUtil.JSONHelper.JsonToObject<T_OutStockTaskInfo>(TaskOutStockModelJson);

                if (taskDetailsModel == null)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "转换实体类为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockTaskInfo>>(messageModel);
                }

                UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);

                if (user == null || string.IsNullOrEmpty(user.UserNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "没有用户信息";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockTaskInfo>>(messageModel);
                }
                T_OutStockTask_DB _db = new T_OutStockTask_DB();

                strUserName = _db.QueryUserNameByTaskOutDetails(taskDetailsModel, user);
                if (!string.IsNullOrEmpty(strUserName))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "拣货任务：" + taskDetailsModel.TaskNo  + "\r\n" + "被：" + strUserName + "锁定！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockTaskInfo>>(messageModel);
                }

                iLock = _db.LockTaskOperUser(user, taskDetailsModel);

                if (iLock == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "任务：" + taskDetailsModel.TaskNo + "锁定失败！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockTaskInfo>>(messageModel);
                }
                messageModel.HeaderStatus = "S";
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockTaskInfo>>(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockTaskInfo>>(messageModel);
            }
        }

        public string UnLockTaskOperUser(string TaskOutStockModelJson, string UserJson) 
        {
            BaseMessage_Model<T_OutStockTaskInfo> messageModel = new BaseMessage_Model<T_OutStockTaskInfo>();

            try
            {
                int iLock = 0;
                string strUserName = string.Empty;

                if (string.IsNullOrEmpty(TaskOutStockModelJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来业务JSON为空";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockTaskInfo>>(messageModel);
                }

                T_OutStockTaskInfo taskDetailsModel = BILBasic.JSONUtil.JSONHelper.JsonToObject<T_OutStockTaskInfo>(TaskOutStockModelJson);

                if (taskDetailsModel == null)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "转换实体类为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockTaskInfo>>(messageModel);
                }

                UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);

                if (user == null || string.IsNullOrEmpty(user.UserNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "没有用户信息";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockTaskInfo>>(messageModel);
                }
                T_OutStockTask_DB _db = new T_OutStockTask_DB();



                iLock = _db.UnLockTaskOperUser(user, taskDetailsModel);

                //if (iLock == 0)
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = "任务：" + taskDetailsModel.TaskNo + "解锁失败！";
                //    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockTaskInfo>>(messageModel);
                //}
                messageModel.HeaderStatus = "S";
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockTaskInfo>>(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockTaskInfo>>(messageModel);
            }
        }

        #endregion
    }
}
