using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILBasic.JSONUtil;
using BILWeb.Login.User;

namespace BILWeb.LandMark
{

    public partial class T_LandMark_Func : TBase_Func<T_LandMark_DB, T_LandMarkInfo>, ILandMarkService
    {

        protected override bool CheckModelBeforeSave(T_LandMarkInfo model, ref string strError)
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
            return "复核地标";
        }

        protected override T_LandMarkInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }


        public string GetLandmark(string landmarkno, string UserJson)
        {

            BaseMessage_Model<T_LandMarkInfo> messageModel = new BaseMessage_Model<T_LandMarkInfo>();
            try
            {
                if (string.IsNullOrEmpty(landmarkno))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "传入的地标号为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_LandMarkInfo>>(messageModel);
                }

                T_LandMark_DB tdb = new T_LandMark_DB();
                T_LandMarkInfo model = tdb.Getlandmark(landmarkno);
                if (model == null)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "获取信息失败！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_LandMarkInfo>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = model;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_LandMarkInfo>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_LandMarkInfo>>(messageModel);
            }
        }



        


    }
}