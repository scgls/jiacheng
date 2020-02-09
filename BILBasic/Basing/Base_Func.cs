using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Interface;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BILBasic.Basing
{
    public abstract class TBase_Func<TBase_DB, TBase_Model>
        where TBase_DB : Base_DB<TBase_Model>, new()
        where TBase_Model : Base_Model
    {
        protected TBase_DB db = new TBase_DB();

        public bool GetModelByID(ref TBase_Model info, ref string strError)
        {
            try
            {
                TBase_Model model = db.GetModelByID(info.ID);
                if (model == null)
                {
                    strError = "该" + GetModelChineseName() + "已被删除！";
                    return false;
                }
                else
                {
                    info = model;
                    return true;
                }
            }
            catch (Exception ex)
            {
                strError = "获取" + GetModelChineseName() + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }


        public bool GetModelBySql(ref TBase_Model model, ref string strError)
        {
            try
            {
                model = db.GetModelBySql(model);
                if (model == null)
                {
                    strError = "该" + GetModelChineseName() + "不存在或已被删除！请确认仓库是否正确！";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                strError = "获取" + GetModelChineseName() + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }

        


        public bool GetModelListByHeaderID(ref List<TBase_Model> modelList, int headerID, ref string strError)
        {
            try
            {
                modelList = db.GetModelListByHeaderID(headerID);
                return true;
            }
            catch (Exception ex)
            {
                strError = "获取" + GetModelChineseName() + "列表失败！" + ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }

        public bool GetModelListByPage(ref List<TBase_Model> modelList, User.UserModel user, TBase_Model model, ref Common.DividPage page, ref string strError)
        {
            try
            {
                modelList = db.GetModelListByPage(user, model, ref page);
                return true;
            }
            catch (Exception ex)
            {
                strError = "获取" + GetModelChineseName() + "列表失败！" + ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }


        /// <summary>
        /// 通过过滤条件得到model
        /// </summary>
        /// <param name="strFilter"></param>
        /// <returns></returns>
        public bool GetModelByFilter(ref TBase_Model model, string strFilter, ref string strError)
        {
            try
            {
                model = db.GetModelByFilter(strFilter);
                if (model == null)
                {
                    strError = "未能获取" + GetModelChineseName() + "数据！";
                    return false;
                }                
                return true;                
            }
            catch (Exception ex)
            {
                strError = "获取" + GetModelChineseName() + "数据失败！" + ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }

        public bool GetModelListByFilter(ref List<TBase_Model> modelList, ref string strError, string OrderBy = "", string Filter = "", string Fields = "*")
        {
            try
            {
                modelList = db.GetModelListByFilter(OrderBy, Filter, Fields);
                if (modelList == null || modelList.Count==0) 
                {
                    strError = "获取" + GetModelChineseName() + "数据为空！";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                strError = "获取" + GetModelChineseName() + "列表失败！" + ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }
               

        public bool SaveModelToDB(User.UserModel user, ref TBase_Model model, ref string strError)
        {
            try
            {
                if (CheckModelBeforeSave(model, ref strError) == false)
                {
                    return false;
                }
                return db.SaveModelToDB(user, ref model, ref strError);
            }
            catch (Exception ex)
            {
                strError = "保存" + GetModelChineseName() + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }

        public bool SaveModelBySqlToDB(User.UserModel user, ref TBase_Model model, ref string strError)
        {
            try
            {                
                if (CheckModelBeforeSave(model, ref strError) == false)
                {
                    return false;
                }

                return db.SaveModelBySqlToDB(user, ref model, ref strError);
            }
            catch (Exception ex)
            {
                strError = "保存" + GetModelChineseName() + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }

        public bool SaveModelListBySqlToDB(User.UserModel user, List<TBase_Model> modelList, ref string strError)
        {
            try
            {
                if (CheckModelBeforeSave(modelList, ref strError) == false)
                {
                    return false;
                }

                if (GetRuleModelBeforeSave(ref modelList, ref strError) == false)
                {
                    return false;
                }

                return db.SaveModelListBySqlToDB(user, ref modelList, ref strError);
            }
            catch (Exception ex)
            {
                strError = "保存" + GetModelChineseName() + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }

        public bool SaveModelListBySqlToDB(List<string> lstSql, ref string strError)
        {
            try
            {
                return db.SaveModelListBySqlToDB(lstSql, ref strError);
            }
            catch (Exception ex)
            {
                strError = "保存" + GetModelChineseName() + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }

        

        public bool UpdateModelStatus(User.UserModel user, ref  TBase_Model model, int NewStatus, ref  string strError, bool NeedReturnModel = true)
        {
            try
            {
                if (!CheckModelBeforeUpdateStatus(user, model, NewStatus, ref strError))
                {
                    return false;
                }
                return db.UpdateModelStatus(user, ref  model, NewStatus, ref strError, NeedReturnModel);
            }
            catch (Exception ex)
            {
                strError = ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }

        public bool DeleteModelByID(User.UserModel user, int iD, ref string strError)
        {
            try
            {
                if (!db.CanDelModel(user, iD, ref strError))
                {
                    return false;
                }

                if (db.DeleteModelByID(user, iD) == false)
                {
                    strError = "删除" + GetModelChineseName() + "失败！";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                strError = "删除" + GetModelChineseName() + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }

        public bool DeleteModelByModel(User.UserModel user, TBase_Model model, ref string strError)
        {
            try
            {
                if (!CanDelModel(user, model, ref strError))
                {
                    return false;
                }

                if (db.DeleteModelByModel(user, model, ref strError) == false)
                {
                    strError = "删除" + GetModelChineseName() + "失败！" + strError;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                strError = "删除" + GetModelChineseName() + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }

        public bool DeleteModelByModelSql(User.UserModel user, TBase_Model model, ref string strError)
        {
            try
            {
                if (!CanDelModel(user, model, ref strError))
                {
                    return false;
                }

                if (db.DeleteModelByModelSql(user,  model, ref strError) == false)
                {
                    strError = "删除" + GetModelChineseName() + "失败！" + strError;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                strError = "删除" + GetModelChineseName() + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }

        public bool UpadteModelByModelSql(User.UserModel user, TBase_Model model, ref string strError)
        {
            try
            {                

                if (db.UpdateModelByModelSql(user, model, ref strError) == false)
                {
                    strError = "更新" + GetModelChineseName() + "失败！" + strError;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                strError = "删除" + GetModelChineseName() + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }


        #region Acdroid使用框架方法


        public string GetModelListADF(string UserJson, string ModelJson)
        {
            BaseMessage_Model<List<TBase_Model>> messageModel = new BaseMessage_Model<List<TBase_Model>>();
            try
            {
                string strError = string.Empty;

                List<TBase_Model> modelList = new List<TBase_Model>();
                if (string.IsNullOrEmpty(UserJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "用户JSON为空！";
                    //return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(messageModel);
                    return JsonConvert.SerializeObject(messageModel);
                }

                User.UserModel user = JSONUtil.JSONHelper.JsonToObject<User.UserModel>(UserJson);

                if (string.IsNullOrEmpty(ModelJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "业务JSON为空！";
                    //return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(messageModel);
                    return JsonConvert.SerializeObject(messageModel);
                }

                TBase_Model model = GetModelByJson(ModelJson);

                //if (!string.IsNullOrEmpty(model.ErpVoucherNo))
                //{
                //    //请求ERP数据
                //    if (Sync(model, ref strError) == false)
                //    {
                //        messageModel.HeaderStatus = "E";
                //        messageModel.Message = "获取" + GetModelChineseName() + "ERP数据失败！" + strError;
                //        //return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(messageModel);
                //        return JsonConvert.SerializeObject(messageModel);
                //    }
                //}


                modelList = db.GetModelListADF(user, model);

                if (modelList == null || modelList.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "获取" + GetModelChineseName() + "数据列表为空！";
                }
                else
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.ModelJson = modelList;
                }
                //return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(messageModel);
                return JsonConvert.SerializeObject(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                messageModel.ModelJson = null;
                return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(messageModel);
            }
        }

        public string GetModelListADF(User.UserModel user, TBase_Model baseModel)
        {
            BaseMessage_Model<List<TBase_Model>> messageModel = new BaseMessage_Model<List<TBase_Model>>();
            try
            {
                string strError = string.Empty;

                List<TBase_Model> modelList = new List<TBase_Model>();
                if (user == null)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "用户对象为空！";
                    //return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(messageModel);
                    return JsonConvert.SerializeObject(messageModel);
                }


                if (baseModel ==null)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "业务对象为空！";
                    //return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(messageModel);
                    return JsonConvert.SerializeObject(messageModel);
                }

                //TBase_Model model = GetModelByJson(ModelJson);

                if (!string.IsNullOrEmpty(baseModel.ErpVoucherNo))
                {
                    //请求ERP数据
                    if (Sync(baseModel, ref strError) == false)
                    {
                        messageModel.HeaderStatus = "E";
                        messageModel.Message = "获取" + GetModelChineseName() + "ERP数据失败！" + strError;
                        //return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(messageModel);
                        return JsonConvert.SerializeObject(messageModel);
                    }
                }


                modelList = db.GetModelListADF(user, baseModel);

                if (modelList == null || modelList.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "获取" + GetModelChineseName() + "数据列表为空！";
                }
                else
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.ModelJson = modelList;
                }
                //return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(messageModel);
                return JsonConvert.SerializeObject(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                messageModel.ModelJson = null;
                //return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(messageModel);
                return JsonConvert.SerializeObject(messageModel);
            }
        }

        public string GetModelListByHeaderIDADF(string ModelDetailJson)
        {

            BaseMessage_Model<List<TBase_Model>> messageModel = new BaseMessage_Model<List<TBase_Model>>();
            try
            {
                if (string.IsNullOrEmpty(ModelDetailJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "业务JSON为空！";
                    return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(messageModel);
                }

                TBase_Model model = GetModelByJson(ModelDetailJson);

                List<TBase_Model> modelList = db.GetModelListByHeaderID(model.HeaderID);

                if (modelList == null || modelList.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "获取" + GetModelChineseName() + "数据列表为空！";
                }
                else
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.ModelJson = modelList;
                }
                return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                messageModel.ModelJson = null;
                return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(messageModel);

            }
        }

        public string GetModelListByHeaderIDADF(int headerID)
        {

            BaseMessage_Model<List<TBase_Model>> messageModel = new BaseMessage_Model<List<TBase_Model>>();
            try
            {

                List<TBase_Model> modelList = db.GetModelListByHeaderID(headerID);

                if (modelList == null || modelList.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "获取" + GetModelChineseName() + "数据列表为空！";
                }
                else
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.ModelJson = modelList;
                }
                return  JsonConvert.SerializeObject(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                messageModel.ModelJson = null;
                return JsonConvert.SerializeObject(messageModel);
            }
        }


        public string SaveModelListSqlToDBADF(string UserJson, string ModeJson)
        {
            BaseMessage_Model<List<TBase_Model>> model = new BaseMessage_Model<List<TBase_Model>>();
            try
            {
                bool bSucc = false;

                string strError = "";
                

                if (string.IsNullOrEmpty(UserJson))
                {
                    model.HeaderStatus = "E";
                    model.Message = "传入用户信息为空！";
                    return JsonConvert.SerializeObject(model);
                    //return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(model);
                }

                User.UserModel user = JsonConvert.DeserializeObject<User.UserModel>(UserJson);//JSONUtil.JSONHelper.JsonToObject<User.UserModel>(UserJson);

                
                List<TBase_Model> modelList = GetModelListByJson(UserJson, ModeJson);

                if (CheckModelBeforeSave(modelList, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JsonConvert.SerializeObject(model);
                    //return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(model);
                }

                T_Interface_Func tfunc = new T_Interface_Func();
                string ERPJson = GetModelListByJsonToERP(user, modelList);//JSONUtil.JSONHelper.ObjectToJson<List<TBase_Model>>(modelList);

                LogNet.LogInfo("ERPJsonBefore:" + ERPJson);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

                model = JsonConvert.DeserializeObject<BaseMessage_Model<List<TBase_Model>>>(interfaceJson) ;//JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<TBase_Model>>>(interfaceJson);

                LogNet.LogInfo("ERPJsonAfter:" + JsonConvert.SerializeObject(model));//JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(model));

                //过账失败直接返回
                if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
                {
                    return interfaceJson;
                }
                else if (model.HeaderStatus == "S" && !string.IsNullOrEmpty(model.MaterialDoc)) //过账成功，并且生成了凭证要记录数据库
                {
                    modelList.ForEach(t => t.MaterialDoc = model.MaterialDoc);
                }

                //LogNet.LogInfo("ERPJson:" + JSONUtil.JSONHelper.ObjectToJson<List<TBase_Model>>(modelList));

                bSucc = db.SaveModelListBySqlToDB(user, ref modelList, ref strError);

                if (bSucc == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                }
                else
                {
                    model.HeaderStatus = "S";
                    model.TaskNo = modelList[0].TaskNo;
                    model.Message = GetSuccessMessage(model.MaterialDoc, modelList[0].TaskNo);                  
                }

                return JsonConvert.SerializeObject(model);//JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(model);
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = "保存" + GetModelChineseName() + "失败！" + ex.Message + ex.TargetSite;

                return JsonConvert.SerializeObject(model);//JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(model);
                             
            }
        }

        public string SaveModelListSqlToDBADF(User.UserModel user, List<TBase_Model> modelList )
        {
            BaseMessage_Model<List<TBase_Model>> model = new BaseMessage_Model<List<TBase_Model>>();
            try
            {
                bool bSucc = false;

                string strError = "";


                if (user == null)
                {
                    model.HeaderStatus = "E";
                    model.Message = "传入用户信息为空！";
                    JsonConvert.SerializeObject(model);
                }


                if (CheckModelBeforeSave(modelList, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    JsonConvert.SerializeObject(model);
                }

                T_Interface_Func tfunc = new T_Interface_Func();
                string ERPJson = GetModelListByJsonToERP(user, modelList);//JSONUtil.JSONHelper.ObjectToJson<List<TBase_Model>>(modelList);

                LogNet.LogInfo("ERPJsonBefore:" + ERPJson);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

                model = JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<TBase_Model>>>(interfaceJson);

                LogNet.LogInfo("ERPJsonAfter:" + JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(model));

                //过账失败直接返回
                if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
                {
                    return interfaceJson;
                }
                else if (model.HeaderStatus == "S" && !string.IsNullOrEmpty(model.MaterialDoc)) //过账成功，并且生成了凭证要记录数据库
                {
                    modelList.ForEach(t => t.MaterialDoc = model.MaterialDoc);
                }

                //LogNet.LogInfo("ERPJson:" + JSONUtil.JSONHelper.ObjectToJson<List<TBase_Model>>(modelList));

                bSucc = db.SaveModelListBySqlToDB(user, ref modelList, ref strError);

                if (bSucc == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                }
                else
                {
                    model.HeaderStatus = "S";
                    model.TaskNo = modelList[0].TaskNo;
                    model.Message = GetSuccessMessage(model.MaterialDoc, modelList[0].TaskNo);
                }

                return JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = "保存" + GetModelChineseName() + "失败！" + ex.Message + ex.TargetSite;

                return JsonConvert.SerializeObject(model);
            }
        }
        
        
        private string GetSuccessMessage(string ERPDoc,string TaskNo) 
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("操作结果：{0}{1}", "数据提交成功！", Environment.NewLine);
            if (!string.IsNullOrEmpty(ERPDoc)) 
            {
                sb.AppendFormat("ERP过账凭证:{0}{1}", ERPDoc, Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(TaskNo)) 
            {
                sb.AppendFormat("WMS任务号:{0}{1}", TaskNo, Environment.NewLine);            }
            
            return sb.ToString();
            //return "操作结果：数据提交成功！\n" + "ERP过账凭证:" + ERPDoc + "\n" + "WMS任务号:" + TaskNo;
        }

        

        public string UpdateModelListSqlToDBADF(string UserJson, string ModelJson) 
        {
            BaseMessage_Model<List<TBase_Model>> model = new BaseMessage_Model<List<TBase_Model>>();
            try
            {
                bool bSucc = false;

                string strError = "";


                if (string.IsNullOrEmpty(UserJson))
                {
                    model.HeaderStatus = "E";
                    model.Message = "传入用户信息为空！";
                    return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(model);
                }

                User.UserModel user = JSONUtil.JSONHelper.JsonToObject<User.UserModel>(UserJson);

                List<TBase_Model> modelList = GetModelListByJson( UserJson,ModelJson);

                bSucc = db.UpdateModelListBySqlToDB(user, ref modelList, ref strError);

                if (bSucc == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                }
                else
                {
                    model.HeaderStatus = "S";
                    model.TaskNo = modelList[0].TaskNo;
                    model.Message = GetSuccessMessage(model.MaterialDoc, modelList[0].TaskNo);
                }

                return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(model);
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = "更新" + GetModelChineseName() + "失败！" + ex.Message + ex.TargetSite;

                return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(model);

            }
        }

        #endregion


        

        public virtual bool CanDelModel(User.UserModel user, TBase_Model model, ref string strError)
        {
            return true;
        }

        public virtual bool CheckModelBeforeUpdateStatus(User.UserModel user, TBase_Model model, int NewStatus, ref string strError)
        {
            return true;
        }

        protected virtual string GetJsonByListModel(List<TBase_Model> modelList)
        {
            return string.Empty;
        }

        protected virtual TBase_Model GetModelByJson(string ModelJson) 
        {
            return null;
        }

        protected virtual List<TBase_Model> GetModelListByJson(string UserJson,string ModelListJson) 
        {
            return null;
        }

        /// <summary>
        /// add by cym 2017-10-06
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="ModelListJson"></param>
        /// <returns></returns>
        protected virtual List<TBase_Model> GetModelListByJson_ERP_Product(string UserJson, string ModelListJson)
        {
            return null;
        }

        protected virtual bool CheckModelBeforeSave(List<TBase_Model> modelList, ref string strError) 
        {
            return true;
        }

        //数据保存前有规则设置读取规则重新组织数据
        protected virtual bool GetRuleModelBeforeSave(ref List<TBase_Model> modelList, ref string strError)
        {
            return true;
        }

        protected virtual bool Sync(TBase_Model model, ref string strErrMsg)
        {
            return true; 
        }

        protected virtual string GetModelListByJsonToERP(User.UserModel user, List<TBase_Model> modelList)
        {
            return JSONUtil.JSONHelper.ObjectToJson<List<TBase_Model>>(modelList); 
        }

        #region 纯虚方法

        /// <summary>
        /// 保存前检查实体类的值
        /// </summary>
        /// <param name="model"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        protected abstract bool CheckModelBeforeSave(TBase_Model model, ref string strError);

        /// <summary>
        /// 得到该实体类的中文描述
        /// </summary>
        /// <returns></returns>
        protected abstract string GetModelChineseName();        

        #endregion
    }
}
