using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILBasic.JSONUtil;
using BILBasic.User;
using Newtonsoft.Json;

namespace BILWeb.LandMark
{

    public partial class T_LandMarkWithTask_Func : TBase_Func<T_LandMarkWithTask_DB, T_LandMarkWithTaskInfo>, ILandMarkDerailService
    {

        protected override bool CheckModelBeforeSave(T_LandMarkWithTaskInfo model, ref string strError)
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
            return "复核地标表体";
        }

        protected override T_LandMarkWithTaskInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

        public string GetTaskForLandmark(string barcode, string UserJson)
        {

            BaseMessage_Model<T_LandMarkWithTaskInfo> messageModel = new BaseMessage_Model<T_LandMarkWithTaskInfo>();
            try
            {
                if (string.IsNullOrEmpty(barcode))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "传入的条码为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_LandMarkWithTaskInfo>>(messageModel);
                }

                T_LandMarkWithTask_DB tdb = new T_LandMarkWithTask_DB();
                T_LandMarkWithTaskInfo model = new T_LandMarkWithTaskInfo();
                string strmsg = "";
                if(!tdb.GetTaskForLandmark(barcode, ref model, ref strmsg))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strmsg;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_LandMarkWithTaskInfo>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = model;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_LandMarkWithTaskInfo>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_LandMarkWithTaskInfo>>(messageModel);
            }
        }


        public string SaveTaskwithandmark(string ModelJson, string UserJson)
        {

            BaseMessage_Model<string> messageModel = new BaseMessage_Model<string>();
            try
            {
                if (string.IsNullOrEmpty(ModelJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "传入的信息为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                }
                UserModel user = JsonConvert.DeserializeObject<UserModel>(UserJson);//JSONUtil.JSONHelper.JsonToObject<User.UserModel>(UserJson);
                T_LandMarkWithTaskInfo landMarkWithTaskInfo = JsonConvert.DeserializeObject<T_LandMarkWithTaskInfo>(ModelJson);//JSONUtil.JSONHelper.JsonToObject<User.UserModel>(UserJson);

                T_LandMarkWithTask_DB tdb = new T_LandMarkWithTask_DB();
                string strmsg = "";
                if (!tdb.SaveTaskwithandmark(landMarkWithTaskInfo, user, ref strmsg))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strmsg;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                return JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
            }
        }
    }
}