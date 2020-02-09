using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using Newtonsoft.Json;

namespace BILBasic.Interface
{
    public class T_Interface_Func
    {
        /// <summary>
        /// 获取二次开发配置数据
        /// </summary>
        /// <returns></returns>
        public   List<T_InterfaceInfo> GetInterface(string VoucherType)
        {
            try
            {
                T_Interface_DB db = new T_Interface_DB();

                List<T_InterfaceInfo> lstModel = db.GetInterface();

                if (lstModel == null || lstModel.Count == 0) return null;

                return lstModel.Where(t => t.StrVoucherType == VoucherType).ToList();
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
            

        }

        /// <summary>
        /// 获取二开接口数据
        /// </summary>
        /// <param name="VoucherJson"></param>
        /// <returns></returns>
        public string GetModelListByInterface(string VoucherJson)
        {
            JsonModel jsonModel = new JsonModel();
            jsonModel.payload = new PayLoad();
            jsonModel.payload.std_data = new Std();
            jsonModel.payload.std_data.execution = new Exe();

            try
            {
                if (string.IsNullOrEmpty(VoucherJson))
                {
                    //jsonModel.payload.std_data.execution.code = "1";
                    //jsonModel.payload.std_data.execution.description = "解析JSON为空";
                    jsonModel.result = "0";
                    jsonModel.resultValue = "解析JSON为空";
                    return JSONUtil.JSONHelper.ObjectToJson<JsonModel>(jsonModel);
                }

                JToken jtoken = JToken.Parse(VoucherJson);
                string VoucherType =jtoken["VoucherType"].ToString();

                if (string.IsNullOrEmpty(VoucherType)) 
                {
                    //jsonModel.payload.std_data.execution.code = "1";
                    //jsonModel.payload.std_data.execution.description = "单据类型为空！";
                    jsonModel.result = "0";
                    jsonModel.resultValue = "单据类型为空！";
                    return JSONUtil.JSONHelper.ObjectToJson<JsonModel>(jsonModel);
                }

                List<T_InterfaceInfo> lstModel = GetInterface(VoucherType);
                if (lstModel == null || lstModel.Count == 0) 
                {
                    //jsonModel.payload.std_data.execution.code = "1";
                    //jsonModel.payload.std_data.execution.description = "没有配置获取数据二开接口！";
                    jsonModel.result = "0";
                    jsonModel.resultValue = "没有配置获取数据二开接口！";
                    return JSONUtil.JSONHelper.ObjectToJson<JsonModel>(jsonModel);
                }

                T_InterfaceInfo model = lstModel.Find(t => t.Function == "3");

                if (model == null) 
                {
                    //jsonModel.payload.std_data.execution.code = "1";
                    //jsonModel.payload.std_data.execution.description = "没有配置获取数据二开接口！";
                    jsonModel.result = "0";
                    jsonModel.resultValue = "没有配置获取数据二开接口！";
                    return JSONUtil.JSONHelper.ObjectToJson<JsonModel>(jsonModel);
                }

                return CreateInterfaceGet(model, VoucherJson);

            }
            catch (Exception ex) 
            {
                //jsonModel.payload.std_data.execution.code = "1";
                //jsonModel.payload.std_data.execution.description = ex.Message;
                jsonModel.result = "0";
                jsonModel.resultValue = ex.Message;
                return JSONUtil.JSONHelper.ObjectToJson<JsonModel>(jsonModel);
            }
        }

        /// <summary>
        /// 过账二开接口
        /// </summary>
        /// <param name="VoucherJson"></param>
        /// <returns></returns>
        public string PostModelListToInterface(string VoucherJson)
        {
            BaseMessage_Model<List<T_InterfaceInfo>> messageModel = new BaseMessage_Model<List<T_InterfaceInfo>>();
            try
            {
                string VoucherType = "";
                string ErpJson = string.Empty;
                string Result = string.Empty;

                JToken jtoken = JToken.Parse(VoucherJson);
                

                if (jtoken.GetType().Name == "JObject")
                {
                    VoucherType = jtoken["VoucherType"].ToString();
                }
                else
                {
                    VoucherType = jtoken[0]["VoucherType"].ToString();
                }

                if (string.IsNullOrEmpty(VoucherType)) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.MaterialDoc = string.Empty;
                    messageModel.ModelJson = null;
                    messageModel.Message = "过账检查单据类型为空！";
                    return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_InterfaceInfo>>>(messageModel);
                }

                if(!string.IsNullOrEmpty(VoucherType))
                {
                    if (Convert.ToInt32(VoucherType) == 0) 
                    {
                        messageModel.HeaderStatus = "E";
                        messageModel.MaterialDoc = string.Empty;
                        messageModel.ModelJson = null;
                        messageModel.Message = "过账检查单据类型为零！";
                        return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_InterfaceInfo>>>(messageModel);
                    }
                }

                List<T_InterfaceInfo> lstModel = GetInterface(VoucherType);

                if (lstModel == null || lstModel.Count == 0)
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.MaterialDoc = string.Empty;
                    messageModel.ModelJson = null;
                    return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_InterfaceInfo>>>(messageModel);
                }

                //配置了过账前检查
                T_InterfaceInfo model = GetInterface(VoucherType).Find(t => t.Function == "1");

                if (model != null)
                {
                    CreateInterfacePost(model, VoucherJson);
                }

                T_InterfaceInfo modelPost = GetInterface(VoucherType).Find(t => t.Function == "2");

                if (modelPost == null) 
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.MaterialDoc = string.Empty;
                    messageModel.ModelJson = null;
                    messageModel.Message = string.Empty;
                    return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_InterfaceInfo>>>(messageModel);
                }
                

                ErpJson = CreateInterfacePost(modelPost, VoucherJson);
                              

                jtoken = JToken.Parse(ErpJson);

                if (jtoken.GetType().Name == "JObject")
                {
                    Result = jtoken["Result"].ToString();
                }
                else
                {
                    Result = jtoken[0]["Result"].ToString();
                }

                if (Result == "false" )
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.MaterialDoc = string.Empty;
                    messageModel.ModelJson = null;
                    messageModel.Message ="ERP接口"+ jtoken["ErrMsg"].ToString();
                    return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_InterfaceInfo>>>(messageModel);
                }
                else 
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.MaterialDoc = jtoken["ErrMsg"].ToString();
                    messageModel.ModelJson = null;
                    messageModel.Message = string.Empty;
                    return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_InterfaceInfo>>>(messageModel);
                }
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.MaterialDoc = string.Empty;
                messageModel.Message = ex.Message;
                messageModel.ModelJson = null;
                return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_InterfaceInfo>>>(messageModel);
            }
        }
        


        public string CreateInterfaceGet(T_InterfaceInfo model,string VoucherJson) 
        {
            JsonModel jsonModel = new JsonModel();
            jsonModel.payload = new PayLoad();
            jsonModel.payload.std_data = new Std();
            jsonModel.payload.std_data.execution = new Exe();
            try
            {
                LogNet.LogInfo("VoucherJson:" + VoucherJson);
                
                var assembly = Assembly.LoadFile(@model.Route);

                var type = assembly.GetType(model.ClassName);

                var instance = assembly.CreateInstance(model.ClassName);

                //type.GetProperty("strJson").SetValue(instance, modeJson, null);
                //strError =  type.GetProperty("strErrMsg").GetValue(instance).ToString();

                object[] obj = new object[1];
                obj[0] = VoucherJson;
                var method = type.GetMethod(model.FunctionName);
                return method.Invoke(instance, obj).ToString();
            }
            catch (Exception ex)
            {
                LogNet.LogInfo("CreateInterface:" + ex.InnerException + ex.Message);
                jsonModel.payload.std_data.execution.code = "1";
                jsonModel.payload.std_data.execution.description = "调用ERP接口异常：" + ex.InnerException + ex.Message;
                return JSONUtil.JSONHelper.ObjectToJson<JsonModel>(jsonModel);
            }
            
        }

        public string CreateInterfacePost(T_InterfaceInfo model, string VoucherJson)
        {
            JsonModel jsonModel = new JsonModel();
            try
            {
                var assembly = Assembly.LoadFile(@model.Route);

                var type = assembly.GetType(model.ClassName);

                var instance = assembly.CreateInstance(model.ClassName);

                //type.GetProperty("strJson").SetValue(instance, modeJson, null);
                //strError =  type.GetProperty("strErrMsg").GetValue(instance).ToString();
               
                object[] obj = new object[1];
                obj[0] = VoucherJson;
                var method = type.GetMethod(model.FunctionName);
                return method.Invoke(instance, obj).ToString();
            }
            catch (Exception ex)
            {
                jsonModel.result = "false";
                jsonModel.ErrMsg = "调用ERP接口异常：" + ex.Message + ex.InnerException;
                LogNet.LogInfo("CreateInterfacePost----" + jsonModel.ErrMsg);
                return JSONUtil.JSONHelper.ObjectToJson<JsonModel>(jsonModel);
            }

        }


    }
}
