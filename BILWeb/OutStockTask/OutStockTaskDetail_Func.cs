using BILBasic.Basing.Factory;
using BILBasic.JSONUtil;
using BILWeb.InStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;
using BILWeb.Stock;
using BILBasic.User;
using BILWeb.Login.User;
using BILBasic.XMLUtil;
using Newtonsoft.Json;
using BILBasic.Language;
using BILWeb.StrategeRuleAll;
using BILWeb.RuleAll;
using BILWeb.Boxing;

namespace BILWeb.OutStockTask
{
    public partial class T_OutTaskDetails_Func : TBase_Func<T_OutTaskDetails_DB, T_OutStockTaskDetailsInfo>
    {
        T_OutTaskDetails_DB _db = new T_OutTaskDetails_DB();

        protected override bool CheckModelBeforeSave(T_OutStockTaskDetailsInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (model.LineStatus == 3) 
            {
                strError = "该行物料已经全部下架不能修改拣货数量！";
                return false;
            }

            if (model.RemainQty == 0) 
            {
                strError = "该行物料已经全部下架不能修改拣货数量！";
                return false;
            }

            if (model.UnShelveQty > 0) 
            {
                if (model.TaskQty <= model.UnShelveQty) 
                {
                    strError = "修改行物料拣货数量不能小于或者等于已下架数量！";
                    return false;
                }
            }
            return true;
        }

        protected override bool CheckModelBeforeSave(List<T_OutStockTaskDetailsInfo> modelList, ref string strError)
        {

            if (modelList == null || modelList.Count == 0)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (modelList.Where(t => t.ScanQty == 0).Count() == modelList.Count())
            {
                strError = "所有物料都没有扫描，不能保存！";
                return false;
            }

            modelList = modelList.Where(t => t.ScanQty > 0).ToList();

            List<T_OutStockTaskDetailsInfo> lstTaskDetail = new List<T_OutStockTaskDetailsInfo>();
            bool bSucc = base.GetModelListByHeaderID(ref lstTaskDetail,modelList[0].HeaderID,ref strError);
            if (bSucc == false) 
            {
                return false;
            }

            var model = lstTaskDetail.Find(t => t.ID == modelList[0].ID);
            if (model == null)
            {
                strError = "提交物料未找到数据";
                return false;
            }

            if (modelList[0].VoucherType != 3) 
            {
                if (modelList[0].ScanQty > model.RemainQty)
                {
                    strError = "扫描数量大于剩余数量，不能提交数据！";
                    return false;
                }
            }            

            if (modelList[0].lstStockInfo != null && modelList[0].lstStockInfo.Count > 0) 
            {
                if (modelList[0].FromErpWarehouse != modelList[0].lstStockInfo[0].WarehouseNo) 
                {
                    strError = "扫描条码的条码对应仓库和订单仓库不一致，不能提交数据！";
                    return false;
                }
            }

            //验证提交的条码是否存在质检状态为待检或者送检
            //发起在库检会存在以上情况

            //查找下架条码是否已经生成调拨单
            //List<T_OutStockTaskDetailsInfo> item = modelList.Where(t=>t.lstStockInfo!=null ).ToList();
            //string strDBVoucherNo = _db.GetDBVoucherNo(item[0].lstStockInfo[0].SerialNo);
            //string strSerialNo = string.Empty;

            //foreach (var item in modelList)
            //{
            //    if (item.lstStockInfo != null && item.lstStockInfo.Count > 0)
            //    {
            //        foreach (var itemStock in item.lstStockInfo)
            //        {
            //            strSerialNo = itemStock.SerialNo;
            //            break;
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(strSerialNo)) { break; }
            //}
          
            return true;

        }

        private bool CheckBarCodeIsSame(List<T_OutStockTaskDetailsInfo> modelList, ref string strErrMsg)
        {
            bool bSucc = false;

            List<T_StockInfo> lstBarCode = new List<T_StockInfo>();

            foreach (var item in modelList)
            {
                lstBarCode.AddRange(item.lstStockInfo);
            }

            var groupByList = from t in lstBarCode
                              group t by new { t1 = t.SerialNo } into m
                              select new
                              {
                                  SerialNo = m.Key.t1
                              };


            T_Stock_DB _db = new T_Stock_DB();
            bSucc = _db.CheckBarCodeQualityStatus(XmlUtil.Serializer(typeof(List<T_StockInfo>), lstBarCode), ref strErrMsg);

            return bSucc;
        }


        protected override string GetModelChineseName()
        {
            return "下架任务表体";
        }

        

        protected override List<T_OutStockTaskDetailsInfo> GetModelListByJson(string UserJson,string ModelListJson)
        {
            UserModel userModel = JSONHelper.JsonToObject<UserModel>(UserJson);
            List<T_OutStockTaskDetailsInfo> NewModelList = new List<T_OutStockTaskDetailsInfo>();
            List<T_OutStockTaskDetailsInfo> modelList = new List<T_OutStockTaskDetailsInfo>();
            modelList = JSONHelper.JsonToObject<List<T_OutStockTaskDetailsInfo>>(ModelListJson);
            modelList = modelList.Where(t => t.ScanQty > 0).ToList();
            modelList.ForEach(t => t.VoucherType = 9996);
            LogNet.LogInfo("SaveT_OutStockTaskDetailADF---" + JSONHelper.ObjectToJson<List<T_OutStockTaskDetailsInfo>>(modelList));
            //modelList.ForEach(t => t.PostUser = userModel.UserNo);
            //modelList.ForEach(t => t.ToErpAreaNo = userModel.PickAreaNo);
            //modelList.ForEach(t => t.ToErpWarehouse = userModel.PickWareHouseNo);
            return modelList;
        }

        /// <summary>
        /// 生成T_OutStockTaskDetailsInfo提交ERP调拨过账
        /// </summary>
        /// <param name="user"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        protected override string GetModelListByJsonToERP(UserModel user,List<T_OutStockTaskDetailsInfo> modelList)
        {
            List<T_StockInfo> lstStock = new List<T_StockInfo>();
            List<T_StockInfo> NewLstStock = new List<T_StockInfo>();
            List<T_OutStockTaskDetailsInfo> lstDetail = new List<T_OutStockTaskDetailsInfo>();
            string strUserNo = string.Empty;

            foreach (var item in modelList) 
            {
                if (item.lstStockInfo != null) 
                {
                    lstStock.AddRange(item.lstStockInfo);
                }
                
            }

            NewLstStock = GroupInstockDetailList(modelList[0].VoucherType,lstStock);

            foreach (var item in NewLstStock) 
            {
                T_OutStockTaskDetailsInfo model = new T_OutStockTaskDetailsInfo();
                model.CompanyCode = item.CompanyCode;
                model.StrongHoldCode = item.StrongHoldCode;
                model.MaterialNo = item.MaterialNo;
                model.FromBatchNo = item.FromBatchNo;
                model.FromErpWarehouse = item.FromErpWarehouse;
                model.FromErpAreaNo = item.FromErpAreaNo;
                model.ToErpWarehouse = user.PickWareHouseNo;
                model.ToErpAreaNo = user.PickAreaNo;
                model.PostUser = user.UserNo;// strPostUser;
                model.VoucherType = item.VoucherType;
                model.ScanQty = item.Qty;
                model.ERPVoucherType = modelList[0].ERPVoucherType;
                lstDetail.Add(model);
            }

            return JSONHelper.ObjectToJson<List<T_OutStockTaskDetailsInfo>>(lstDetail);

        }


        private List<T_StockInfo> GroupInstockDetailList(int VoucherType,List<T_StockInfo> modelList)
        {
            var newModelList = from t in modelList
                               group t by new { t1 = t.MaterialNo, t2 = t.BatchNo ,t3 = t.WarehouseNo,t4 = t.AreaNo} into m
                               select new T_StockInfo
                               {
                                  MaterialNo = m.Key.t1,
                                  Qty = m.Sum(item => item.Qty),                                   
                                  FromBatchNo =m.Key.t2,
                                  FromErpWarehouse = m.Key.t3,
                                  FromErpAreaNo = m.Key.t4,
                                  VoucherType = VoucherType,
                                  CompanyCode = m.FirstOrDefault().CompanyCode,
                                  StrongHoldCode = m.FirstOrDefault().StrongHoldCode                                  

                               };

            return newModelList.ToList();
        }


        public bool GetExportTaskDetail(T_OutStockTaskInfo model, ref List<T_OutStockTaskDetailsInfo> modelList, ref string strErrMsg)
        {
            try
            {
                T_OutTaskDetails_DB tdb = new T_OutTaskDetails_DB();
                modelList = tdb.GetExportTaskDetail(model);
                if (modelList == null || modelList.Count == 0)
                {
                    strErrMsg = "获取导出数据为空！";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }

        public string GetOutTaskDetailListByHeaderIDADF(string ModelDetailJson) 
        {
            BaseMessage_Model<List<T_OutStockTaskDetailsInfo>> messageModel = new BaseMessage_Model<List<T_OutStockTaskDetailsInfo>>();
            
            try
            {
                string strError = string.Empty;

                if (string.IsNullOrEmpty(ModelDetailJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来拣货单JSON为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                LogNet.LogInfo("GetOutTaskDetailListByHeaderIDADF--" + ModelDetailJson);

                List<T_OutStockTaskInfo> lstModel = JsonConvert.DeserializeObject<List<T_OutStockTaskInfo>>(ModelDetailJson);//JSONHelper.JsonToObject<List<T_OutStockTaskInfo>>(ModelDetailJson);
                if (lstModel == null || lstModel.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "拣货单JSON转换拣货单列表为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                if (CheckERPVoucherNoIsSame(lstModel,ref strError) == false) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JsonConvert.SerializeObject(messageModel);
                }

                T_OutTaskDetails_DB _db = new T_OutTaskDetails_DB();
                List<T_OutStockTaskDetailsInfo> modelList = _db.GetOutTaskDetailListByHeaderIDADF(lstModel);

                if (modelList == null || modelList.Count == 0) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "获取拣货单表体列表为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                modelList = modelList.Where(t => t.RemainQty > 0).ToList();

                if (modelList == null || modelList.Count == 0) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "该拣货单已经全部完成拣货！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                //拣货分配规则
                Context<T_OutStockTaskDetailsInfo> context = new Context<T_OutStockTaskDetailsInfo>(RuleAll_Config.OutStockPickItem);
                if (context.GetOutStockTaskDetailPickList(ref modelList, ref strError) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JsonConvert.SerializeObject(messageModel);
                }

                modelList.ForEach(t => t.ScanQty = 0);
                LogNet.LogInfo("GetT_OutTaskDetailListByHeaderIDADF--" + JsonConvert.SerializeObject(modelList));
                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = modelList;
                return JsonConvert.SerializeObject(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JsonConvert.SerializeObject(messageModel);
            }
        }

        public string GetOutTaskDetailListByHeaderIDADF(List<T_OutStockTaskInfo> modelList)
        {
            BaseMessage_Model<List<T_OutStockTaskDetailsInfo>> messageModel = new BaseMessage_Model<List<T_OutStockTaskDetailsInfo>>();

            try
            {
                string strError = string.Empty;

                if (modelList == null || modelList.Count==0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = Language_CHS.DataIsEmpty;
                    return  JsonConvert.SerializeObject(messageModel);
                }

                //if (CheckERPVoucherNoIsSame(lstModel, ref strError) == false)
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = strError;
                //    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutStockTaskDetailsInfo>>>(messageModel);
                //}

                T_OutTaskDetails_DB _db = new T_OutTaskDetails_DB();
                List<T_OutStockTaskDetailsInfo> modelListDetail = _db.GetOutTaskDetailListByHeaderIDADF(modelList);

                modelListDetail = modelListDetail.Where(t => t.RemainQty > 0).ToList();

                if (modelListDetail == null || modelListDetail.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "获取拣货单表体列表为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                //拣货分配规则
                Context<T_OutStockTaskDetailsInfo> context = new Context<T_OutStockTaskDetailsInfo>(RuleAll_Config.OutStockPickItem);
                if (context.GetOutStockTaskDetailPickList(ref modelListDetail, ref strError) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JsonConvert.SerializeObject(messageModel);
                }

                //List<T_StockInfo> lstStock = new List<T_StockInfo>();                

                //List<T_OutStockTaskDetailsInfo> modelListGroup = GroupInstockDetailList(modelListDetail);

                //if (_db.GetPickRuleAreaNo(modelListGroup, ref lstStock, ref strError) == false)
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = strError;
                //    return JsonConvert.SerializeObject(messageModel);
                //}

                //if (lstStock != null && lstStock.Count > 0)
                //{
                //    lstStock = lstStock.Where(t => t.AreaNo != "NOSALES").ToList();
                //}

                //if (lstStock == null || lstStock.Count == 0)
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = "获取推荐库位库存数量为零！";
                //    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutStockTaskDetailsInfo>>>(messageModel);
                //}

                //List<T_OutStockTaskDetailsInfo> NewModelList = _db.CreateNewListByPickRuleAreaNo(modelList, lstStock);

                LogNet.LogInfo("GetT_OutTaskDetailListByHeaderIDADF--" + JsonConvert.SerializeObject(modelListDetail));
                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = modelListDetail;
                return JsonConvert.SerializeObject(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JsonConvert.SerializeObject(messageModel);
            }
        }

        private bool CheckERPVoucherNoIsSame(List<T_OutStockTaskInfo> modelList, ref string strErrMsg)
        {
            bool bSucc = false;


            var groupByList = from t in modelList
                              group t by new { t1 = t.ErpVoucherNo } into m
                              select new
                              {
                                  ErpVoucherNo = m.Key.t1
                              };

            if (groupByList.Count() > 1)
            {
                strErrMsg = "不同ERP发料单不能合并拣货！";
                bSucc = false;
            }
            else { bSucc = true; }
                        
            return bSucc;
        }

        private List<T_OutStockTaskDetailsInfo> GroupInstockDetailList(List<T_OutStockTaskDetailsInfo> modelList)
        {
            var newModelList = from t in modelList
                               group t by new { t1 = t.MaterialNoID,t2 = t.FloorType,
                                   t3 = t.IsSpcBatch,t4 = t.FromErpWarehouse } into m
                               select new T_OutStockTaskDetailsInfo
                               {
                                   MaterialNoID = m.Key.t1,
                                   //HeightArea = m.Key.t2,
                                   //IsSpcBatch  = m.Key.t3,
                                   //FloorType = m.Key.t2,
                                   FromErpWarehouse = m.Key.t4,
                                   RemainQty = m.Sum(item => item.RemainQty),
                                   MaterialDesc = m.FirstOrDefault().MaterialDesc,                                   
                                   MaterialNo = m.FirstOrDefault().MaterialNo,                                   
                                   StrongHoldCode = m.FirstOrDefault().StrongHoldCode,
                                   StrongHoldName = m.FirstOrDefault().StrongHoldName,
                                   CompanyCode = m.FirstOrDefault().CompanyCode,
                                   FromBatchNo = m.FirstOrDefault().FromBatchNo                            
                               };

            return newModelList.ToList();
        }

        #region 拣选小车操作

        /// <summary>
        /// android扫描并提交绑定拣选小车
        /// </summary>
        /// <param name="strCarNo"></param>
        /// <param name="strTaskNo"></param>
        /// <param name="strUserNo"></param>
        /// <returns></returns>
        public string GetCarModelADF(string strCarNo, string strTaskNo, string strUserNo)
        {
            BaseMessage_Model<T_OutStockTaskDetailsInfo> messageModel = new BaseMessage_Model<T_OutStockTaskDetailsInfo>();

            try
            {
                string strError = string.Empty;

                if (string.IsNullOrEmpty(strCarNo)) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来小车编号为空！" ;
                    return JsonConvert.SerializeObject(messageModel);
                }

                if (string.IsNullOrEmpty(strTaskNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来任务号为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                T_OutTaskDetails_DB db = new T_OutTaskDetails_DB();

                if (db.GetCarNo(strCarNo) == 0) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "小车编号不存在！";
                    return JsonConvert.SerializeObject(messageModel);
                }
                
                string taskno = db.PostScanCar(strCarNo);
                if (!string.IsNullOrEmpty(taskno) && taskno != strTaskNo)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "拣货车被拣货单占用,拣货单号" + taskno;
                    return JsonConvert.SerializeObject(messageModel);
                }

                if (db.PostBindCarTask(strCarNo, strTaskNo, strUserNo, ref strError) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JsonConvert.SerializeObject(messageModel);
                }

                messageModel.HeaderStatus = "S";
                return JsonConvert.SerializeObject(messageModel);
            }
            catch (Exception ex) 
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JsonConvert.SerializeObject(messageModel);
            }


        }


        /// <summary>
        /// 手动释放拣选小车
        /// </summary>
        /// <param name="strCarNo"></param>
        /// <param name="strTaskNo"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool DeleteCarModelBySql(string strCarNo, string strTaskNo, ref string strError)
        {
            T_OutTaskDetails_DB tdb = new T_OutTaskDetails_DB();
            return tdb.DeleteCarModelBySql(strCarNo, strTaskNo, ref strError);
        }

        #endregion

        public string LockOutTaskOperUser(string TaskJson, string UserJson)
        {
            return string.Empty;
            //BaseMessage_Model<T_OutStockTaskInfo> messageModel = new BaseMessage_Model<T_OutStockTaskInfo>();

            //try
            //{
            //    int iLock = 0;
            //    string strUserName = string.Empty;

            //    T_OutStockTaskInfo taskDetailsModel = BILBasic.JSONUtil.JSONHelper.JsonToObject<T_OutStockTaskInfo>(TaskJson);

            //    if (taskDetailsModel == null || (string.IsNullOrEmpty(taskDetailsModel.MaterialNo) && string.IsNullOrEmpty(taskDetailsModel.TMaterialNo)))
            //    {
            //        messageModel.HeaderStatus = "E";
            //        messageModel.Message = "没有物料信息";
            //        return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);
            //    }

            //    UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);

            //    if (user == null || string.IsNullOrEmpty(user.UserNo))
            //    {
            //        messageModel.HeaderStatus = "E";
            //        messageModel.Message = "没有用户信息";
            //        return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);
            //    }
            //    T_InTaskDetails_DB _db = new T_InTaskDetails_DB();

            //    strUserName = _db.QueryUserNameByTaskDetails(taskDetailsModel, user);
            //    if (!string.IsNullOrEmpty(strUserName))
            //    {
            //        messageModel.HeaderStatus = "E";
            //        messageModel.Message = "物料：" + taskDetailsModel.MaterialNo + taskDetailsModel.TMaterialNo + "\r\n" + "被：" + strUserName + "锁定！";
            //        return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);
            //    }

            //    iLock = _db.LockTaskOperUser(user, taskDetailsModel);

            //    if (iLock == 0)
            //    {
            //        messageModel.HeaderStatus = "E";
            //        messageModel.Message = "物料：" + taskDetailsModel.MaterialNo + "锁定失败！";
            //        return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);
            //    }
            //    messageModel.HeaderStatus = "S";
            //    return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);
            //}
            //catch (Exception ex)
            //{
            //    messageModel.HeaderStatus = "E";
            //    messageModel.Message = ex.Message;
            //    return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);
            //}
        }

        /// <summary>
        /// 保存拣货散件箱码
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="ModelJson"></param>
        /// <returns></returns>
        public string SaveBoxListADF(string UserJson, string ModelJson)
        {
            BaseMessage_Model<List<T_BoxingInfo>> messageModel = new BaseMessage_Model<List<T_BoxingInfo>>();

            try
            {
                LogNet.LogInfo("SaveBoxListADF--" + JsonConvert.SerializeObject(ModelJson));
                string strError = string.Empty;

                if (string.IsNullOrEmpty(UserJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "用户端传来用户JSON为空！";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                }

                if (string.IsNullOrEmpty(ModelJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来拣货单JSON为空！";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                }

                UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);

                if (user == null )
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "用户JSON转换用户列表为空！";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                }

                List<T_BoxingInfo> modelList = JSONHelper.JsonToObject<List<T_BoxingInfo>>(ModelJson);
                if (modelList == null || modelList.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "装箱清单JSON转换列表为空！";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                }

                var NewModelList = GroupBoxDetailList(modelList);

                List<T_OutStockTaskDetailsInfo> outTaskDetail = new List<T_OutStockTaskDetailsInfo>();
                string strFilter = "erpvoucherno = '" + NewModelList[0].ErpVoucherNo + "'";

                bool bSucc = base.GetModelListByFilter(ref outTaskDetail, ref strError, "", strFilter, "*");
                if (bSucc == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JsonConvert.SerializeObject(messageModel);
                }

                if (outTaskDetail[0].VoucherType == 31)//调拨出
                {
                    NewModelList.ForEach(t => t.CustomerNo = outTaskDetail[0].ToErpWarehouse);
                    NewModelList.ForEach(t => t.CustomerName = outTaskDetail[0].ToErpWarehouse);
                }
                else 
                {
                    NewModelList.ForEach(t => t.CustomerNo = outTaskDetail[0].SupCusCode);
                    NewModelList.ForEach(t => t.CustomerName = outTaskDetail[0].SupCusName);
                }

                string VoucherNo = string.Empty;
            
                T_OutStockTask_DB _db = new T_OutStockTask_DB();
                if (_db.SaveBoxList(user, NewModelList, ref VoucherNo, ref strError) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                }
                else 
                {
                    NewModelList.ForEach(t => t.SerialNo = VoucherNo);
                    messageModel.HeaderStatus = "S";
                    messageModel.MaterialDoc = VoucherNo;
                    messageModel.ModelJson = NewModelList;
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                }

                //T_OutStockTask_DB _db = new T_OutStockTask_DB();
                //if (_db.SavePickUserList(UserList, modelList, ref strError) == false)
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = strError;
                //    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutStockTaskInfo>>>(messageModel);
                //}
                //else
                //{
                //    messageModel.HeaderStatus = "S";
                //    messageModel.Message = "拣货任务分配成功！";
                //    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutStockTaskInfo>>>(messageModel);
                //}

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
            }
        }

        private List<T_BoxingInfo> GroupBoxDetailList(List<T_BoxingInfo> modelList)
        {
            var newModelList = from t in modelList
                               group t by new { t1 = t.MaterialNo,t2=t.TaskNo} into m//, t2 = t.BatchNo 
                               select new T_BoxingInfo
                               {
                                   MaterialNo = m.Key.t1,
                                   Qty = m.Sum(item => item.Qty),
                                   MaterialName = m.FirstOrDefault().MaterialName,
                                   TaskNo = m.Key.t2,
                                   ErpVoucherNo = m.FirstOrDefault().ErpVoucherNo
                               };

            return newModelList.ToList();
        }

    }
}
