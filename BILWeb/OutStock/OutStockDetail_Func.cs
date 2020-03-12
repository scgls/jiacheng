using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;
using BILBasic.JSONUtil;
using BILWeb.OutBarCode;
using BILWeb.Stock;
using BILWeb.Pallet;
using BILBasic.User;
using BILBasic.Interface;
using BILWeb.InStock;
using BILWeb.Login.User;
using Newtonsoft.Json;
using BILWeb.RuleAll;
using BILWeb.StrategeRuleAll;
using BILWeb.OutStockCreate;
using BILWeb.OutStockTask;


namespace BILWeb.OutStock
{
    public partial class T_OutStockDetail_Func : TBase_Func<T_OutStockDetail_DB, T_OutStockDetailInfo>
     {
        T_OutBarCode_Func outBarCodeFunc = new T_OutBarCode_Func();
        T_Stock_Func stockFunc = new T_Stock_Func();

        protected override bool CheckModelBeforeSave(T_OutStockDetailInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (string.IsNullOrEmpty(model.MaterialNo))
            {
                strError = "物料编号为空，，不能保存！";
                return false;
            }

            if (model.OutStockQty <= 0) 
            {
                strError = "数量为零，不能保存！";
                return false;
            }

            return true;
        }


        protected override bool CheckModelBeforeSave(List<T_OutStockDetailInfo> modelList, ref string strError)
        {
            if (modelList == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            //if (modelList.Where(t => t.ScanQty==0).Count() > 0) 
            //{
            //    strError = "存在未复核扫描物料，不能提交数据！";
            //    return false;
            //}

            //if (CheckReviewData(modelList, ref strError) == false) 
            //{
            //    return false;
            //}

            modelList = modelList.Where(t => t.ScanQty > 0).ToList();

            return true;
        }

        private bool CheckReviewData(List<T_OutStockDetailInfo> modelList,ref string strError) 
        {
            bool bSucc = false;
            foreach (var item in modelList) 
            {
                if (item.ScanQty != item.OutStockQty)
                {
                    strError = "复核数量未扫描完成，不能提交数据！ERP订单号：" + item.ErpVoucherNo + "物料号：" + item.MaterialNo + "项次：" + item.RowNo + "项序：" + item.RowNoDel;
                    bSucc = false;
                    break;
                }
                else 
                {
                    bSucc = true;
                }
            }

            return bSucc;

        }

        protected override string GetModelChineseName()
        {
            return "出库单表体";
        }

        #region PDA复核获取表体数据，只是整箱区的拣货单
        public string GetT_OutStockReviewDetailListByHeaderIDADF(string ModelDetailJson)
        {

            BaseMessage_Model<List<T_OutStockDetailInfo>> messageModel = new BaseMessage_Model<List<T_OutStockDetailInfo>>();

            try
            {
                T_OutStockDetail_DB _db = new T_OutStockDetail_DB();
                T_OutTaskDetails_DB tdb = new T_OutTaskDetails_DB();
                List<T_OutStockTaskDetailsInfo> modelListTaskDetail = new List<T_OutStockTaskDetailsInfo>();
                List<T_OutStockDetailInfo> outStockDetailList = new List<T_OutStockDetailInfo>();
                string strError = string.Empty;

                if (string.IsNullOrEmpty(ModelDetailJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来的数据为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                T_OutStockDetailInfo model = JsonConvert.DeserializeObject<T_OutStockDetailInfo>(ModelDetailJson);

                if (string.IsNullOrEmpty(model.ErpVoucherNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来ERP订单号为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                if (tdb.GetOutTaskDetailByErpVoucherNo(model.ErpVoucherNo, ref modelListTaskDetail, ref strError) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JsonConvert.SerializeObject(messageModel);
                }

                outStockDetailList = _db.CreateOutStockDetailByTaskDetail(modelListTaskDetail.Where(t=>t.HouseProp==1).ToList());

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = outStockDetailList;
                return JsonConvert.SerializeObject(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JsonConvert.SerializeObject(messageModel);
            }
            
            
        } 
        #endregion

        #region 扫描复核条码

        public string ScanOutStockReviewByBarCode(string BarCode)
        {

            BaseMessage_Model<List<T_StockInfo>> model = new BaseMessage_Model<List<T_StockInfo>>();
            
            string strPalletNo = string.Empty;
            string SerialNo = string.Empty;
            string strError = string.Empty;
            string BarCodeType = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(BarCode))
                {
                    model.HeaderStatus = "E";
                    model.Message = "客户端传来的托盘号或者条码为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(model);
                }

                //验证条码正确性
                if (outBarCodeFunc.GetSerialNoByBarCode(BarCode, ref SerialNo, ref BarCodeType, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(model);
                }
                

                List<T_StockInfo> modelList = new List<T_StockInfo>();

                if (GetStockBySerialNo(ref modelList, SerialNo, ref strError, BarCodeType) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(model);
                }

                model.HeaderStatus = "S";
                model.ModelJson = modelList;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(model);

            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(model);
            }
        }


        private bool GetStockBySerialNo(ref List<T_StockInfo> modelList, string SerialNo, ref string strError, string BarCodeType)
        {

            //外箱条码
            if (BarCodeType == "1")
            {
                if (GetStockInfoBySerialNo(SerialNo, ref modelList, ref strError) == false)
                {
                    return false;
                }
            }
            else if (BarCodeType == "2")
            {
                if (GetStockPalletByOutStockReview(SerialNo,  ref strError,ref modelList) == false)
                {
                    return false;
                }
            }
            else if (BarCodeType == "3")//序列号
            {
                //扫描的序列号不是外箱条码，需要查找是否是托盘条码
                if (GetStockInfoBySerialNo(SerialNo, ref modelList, ref strError) == false && GetStockPalletByOutStockReview(SerialNo, ref strError,ref modelList) == false)
                {
                    //strError = "外箱条码或者托盘条码不存在！";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 扫描外箱条码
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <param name="modelList"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        private bool GetStockInfoBySerialNo(string SerialNo, ref List<T_StockInfo> modelList, ref string strError)
        {
            bool bSucc = false;

            T_StockInfo stockModel = new T_StockInfo();

            if (stockFunc.GetStockByBarCode(SerialNo, ref stockModel, ref strError) == false)
            {
                return false;
            }

            if (stockModel.TaskDetailesID == 0) 
            {
                strError = "该条码没有下架，不能复核扫描！";
                return false;
            }

            if (!string.IsNullOrEmpty(stockModel.PalletNo))
            {
                //存在托盘，需要获取库存整托数据
                if (stockFunc.GetStockInfoByPalletNo(stockModel.PalletNo, ref modelList, ref strError) == false)
                {
                    bSucc = false;
                }
                else 
                {
                    bSucc = true;
                    modelList = modelList.Where(t => t.TaskDetailesID > 0).ToList();
                }
            }
            else
            {                
                modelList.Add(stockModel);
                bSucc = true;
            }

            decimal SumQty = modelList.Sum(t1 => t1.Qty).ToDecimal();
            modelList.ForEach(t => t.PalletQty = SumQty);

            return bSucc;
        }


        private bool GetStockPalletByOutStockReview(string BarCode, ref string strError, ref List<T_StockInfo> modelList)
        {
            string Filter = string.Empty;
            T_PalletDetail_Func tpdf = new T_PalletDetail_Func();
            List<T_PalletDetailInfo> pList = new List<T_PalletDetailInfo>();
            //获取托盘表数据，托盘对应的ERP订单号
            if (tpdf.GetPalletByPalletNo("palletno = '" + BarCode + "' ", ref pList, ref strError) == false)
            {
                strError = "扫描的托盘条码不存在！" + BarCode;
                return false;
            }            

            //托盘条码，获取整托明细
            if (stockFunc.GetStockInfoByPalletNo(BarCode, ref modelList, ref strError) == false)
            {
                return false;
            }

            if (modelList.Where(t=>t.TaskDetailesID==0).Count() > 1)
            {
                strError = "该托盘存在未下架条码，不能复核扫描！请确认是否已经拆托！" + BarCode;
                return false;
            }

            modelList.Where(t => t.TaskDetailesID > 0);
            decimal SumQty = modelList.Sum(t1 => t1.Qty).ToDecimal();
            modelList.ForEach(t => t.PalletQty = SumQty);
            return true;
        }

        #endregion

        #region 扫描复核条码支持序列号和ENA

        public string GetReviewStockModelADF(string ModelStockJson)
        {
            BaseMessage_Model<List<T_StockInfo>> messageModel = new BaseMessage_Model<List<T_StockInfo>>();

            string strPalletNo = string.Empty;
            string SerialNo = string.Empty;
            string strError = string.Empty;
            string BarCodeType = string.Empty;

            try
            {
                LogNet.LogInfo("GetReviewStockModelADF:" + ModelStockJson);

                if (string.IsNullOrEmpty(ModelStockJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来的数据为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }
                
                T_StockInfo model = JsonConvert.DeserializeObject<T_StockInfo>(ModelStockJson);

                //if (!model.Barcode.Contains("@")) 
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = "PDA复核，请扫描批次标签";
                //    return JsonConvert.SerializeObject(messageModel);
                //}
                //验证条码正确性
                if (outBarCodeFunc.GetSerialNoByBarCode(model.Barcode, ref SerialNo, ref BarCodeType, ref strError) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JsonConvert.SerializeObject(messageModel);
                }
                
                //if (!model.Barcode.Contains("@")) 
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = "PDA复核，请扫描批次标签";
                //    return JsonConvert.SerializeObject(messageModel);
                //}
                T_OutStockDetail_DB _db = new T_OutStockDetail_DB();
                //string strSerialNo = OutBarCode_DeCode.GetEndSerialNo(model.Barcode);
                int iResult = _db.CheckBarCodeIsReview(SerialNo);
                if (iResult > 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "标签已经复核，不能重复扫描复核！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                List<T_StockInfo> modelList = new List<T_StockInfo>();
                Context<T_StockInfo> context = new Context<T_StockInfo>(RuleAll_Config.SerialItem);
                if (context.GetStockByBarCode(model, ref modelList, ref strError) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JsonConvert.SerializeObject(messageModel);
                }

                if (CheckReviewOutStockStatus(ref strError, modelList[0]) == false) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JsonConvert.SerializeObject(messageModel);
                }

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

        /// <summary>
        /// 提供PC段扫描复核，扫描批次标签或者69码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="modelList"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool GetReviewStockModel(UserModel user,T_StockInfo model,ref List<T_StockInfo> modelList,ref List<T_OutStockDetailInfo> outStockDetailList,ref int ID, ref string strError)
        {
            bool bCheck = false;
            string strErpVoucherNo = string.Empty;
            string strPalletNo = string.Empty;
            string SerialNo = string.Empty;            
            string BarCodeType = string.Empty;
            string Barcode = string.Empty;
            T_StockInfo stockModel = new T_StockInfo();
            T_OutStockDetail_DB _db = new T_OutStockDetail_DB();
            List<T_StockInfo> stockModelList = new List<T_StockInfo>();
            T_OutTaskDetails_DB tdb = new T_OutTaskDetails_DB();
            List<T_OutStockTaskDetailsInfo> modelListTaskDetail = new List<T_OutStockTaskDetailsInfo>();
            try
            {
                
                if (model == null || string.IsNullOrEmpty(model.Barcode)) 
                {
                    strError = "客户端传来的数据或者条码为空！";
                    return false;
                }                

                if (string.IsNullOrEmpty(model.ErpVoucherNo)) 
                {
                    strError = "客户端传来ERP订单号为空！";
                    return false;
                }

                model.ErpVoucherNo = model.ErpVoucherNo.Trim();
                strErpVoucherNo = model.ErpVoucherNo;
                if (model.Barcode.Contains("@"))
                {
                    int iResult = _db.CheckBarCodeIsReview(OutBarCode_DeCode.GetEndSerialNo(model.Barcode));
                    if (iResult > 0)
                    {
                        strError = "批次标签已经复核，不能重复扫描复核！";
                        return false;
                    }
                }

                if (!(model.Barcode.Length == 13 || model.Barcode.Length == 14)) 
                {
                    strError = "复核扫描只能是69码，当前扫描的不是69码！";
                    return false;
                }

                Context<T_StockInfo> context = new Context<T_StockInfo>(RuleAll_Config.SerialItem);
                if (context.GetStockByBarCode(model, ref stockModelList, ref strError) == false)
                {
                    return false;
                }
                
                //modelList = modelList.Where(t => t.TaskDetailesID > 0).ToList();

                if (stockModelList == null || stockModelList.Count == 0) 
                {
                    strError = "该条码未拣货下架！";
                    return false;
                }

                string strFilter = "erpvoucherno = '" + model.ErpVoucherNo + "'";

                T_OutStockInfo outStockModel = new T_OutStockInfo();
                T_OutStock_Func tfunc = new T_OutStock_Func();
                bool bSucc = tfunc.GetModelByFilter(ref outStockModel, strFilter, ref strError);
                if (bSucc == false) 
                {
                    return false;
                }

                if (CheckReviewOutStockStatus(ref strError, outStockModel) == false)
                {
                    return false;
                }

                Barcode = model.Barcode;

                //扫描的是69码，需要将拣货数量汇总，复核不扣库存，只管数量
                if (!model.Barcode.Contains("@"))
                {
                    //model = GroupByStockQtyByEan(stockModelList)[0];
                    modelList = GroupByStockQtyByEan(stockModelList);
                }
                else 
                {
                    //model = stockModelList[0];
                    modelList = stockModelList;
                }
               
                //验证扫描提交的数量 + 已复核的数量是否大于拣货数量
                if (tdb.GetOutTaskDetailByErpVoucherNo(strErpVoucherNo, ref modelListTaskDetail, ref strError) == false)
                {
                    return false;
                }

                foreach (var item in modelList) 
                {
                    var outStockDetailTask = modelListTaskDetail.Find(t => t.MaterialNoID == item.MaterialNoID && t.ID == item.TaskDetailesID);
                    if (outStockDetailTask == null)
                    {
                        bCheck = false;
                    }
                    else 
                    {
                        bCheck = true;
                        break;
                    }
                }

                if (bCheck == false) 
                {
                    strError = "扫描的条码对应物料不在复核订单中！对应的物料编码：" + model.MaterialNo;
                    return false;
                }

                //var outStockDetailTaskModel = modelListTaskDetail.Find(t => t.MaterialNoID == model.MaterialNoID && t.ID == model.TaskDetailesID);

                //if (outStockDetailTaskModel == null) 
                //{
                //    strError = "扫描的条码对应物料不在复核订单中！对应的物料编码：" + model.MaterialNo;
                //    return false;
                //}

                //如果是69码直接返回
                if (!Barcode.Contains("@")) 
                {                   
                    //model.Qty = model.Qty - outStockDetailTaskModel.ReviewQty.ToDecimal();
                    //modelList.Add(model);
                    return true;
                }

                //model.ScanQty = model.Qty;
                model = modelList[0];
                model.ScanQty = modelList[0].Qty;
                //model.ScanQty = modelList[0].Qty;
                //model.TaskDetailesID = modelList[0].TaskDetailesID;
                //model.MaterialNoID = modelList[0].MaterialNoID;

                var outStockDetailTaskModel = modelListTaskDetail.Find(t => t.MaterialNoID == model.MaterialNoID && t.ID == model.TaskDetailesID);

                if (outStockDetailTaskModel == null) 
                {
                    strError = "扫描的条码对应物料不在复核订单中！";
                    return false;
                }

                ID = outStockDetailTaskModel.ID;

                if (CheckReviewQty(model, outStockDetailTaskModel, ref strError) == false) 
                {
                    return false;
                }

                SetOutStockDetailStatusAndQty(model, ref  outStockDetailTaskModel);

                //确定表头状态
                SetOutStockStatus(ref  modelListTaskDetail);
                //更新拣货单复核数量，状态，人员，过滤掉扫描数量为零的行
                modelListTaskDetail = modelListTaskDetail.Where(t => t.ScanQty > 0).ToList();

                //T_OutStockDetail_DB _db = new T_OutStockDetail_DB();
                bSucc = _db.SaveReviewModelListSql(user, modelListTaskDetail, ref strError);
                if (bSucc == false)
                {
                    return false;
                }

                if (tdb.GetOutTaskDetailByErpVoucherNo(strErpVoucherNo, ref modelListTaskDetail, ref strError) == false)
                {
                    return false;
                }

                outStockDetailList = _db.CreateOutStockDetailByTaskDetail(modelListTaskDetail);
                //bSucc = base.GetModelListByFilter(ref outStockDetailList, ref strError, "", strFilter, "*");
                //if (bSucc == false)
                //{
                //    return false;
                //}

                //outStockDetailList = OutStockSameMaterialNoSumQty(outStockDetailList);

                return true;
                
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        
        /// <summary>
        /// 确定拣货单据表头复核状态
        /// </summary>
        /// <param name="newOutStockModelList"></param>
        private static void SetOutStockStatus(ref List<T_OutStockTaskDetailsInfo> newOutStockTaskModelList)
        {
            if (newOutStockTaskModelList.Where(t => t.ReviewStatus == 3).Count() == newOutStockTaskModelList.Count)
            {
                newOutStockTaskModelList.ForEach(t => t.ReviewStatus = 3);
            }
            else
            {
                newOutStockTaskModelList.ForEach(t => t.ReviewStatus = 2);
            }
        }

        /// <summary>
        /// 给当前物料行赋值扫描数量和行状态
        /// </summary>
        /// <param name="model"></param>
        /// <param name="outStockDetailModel"></param>
        private  void SetOutStockDetailStatusAndQty(T_StockInfo model, ref  T_OutStockTaskDetailsInfo outStockTaskDetailModel)
        {
            outStockTaskDetailModel.ScanQty = model.ScanQty;
            outStockTaskDetailModel.lstStockInfo = new List<T_StockInfo>();
            outStockTaskDetailModel.lstStockInfo.Add(model);

            if (outStockTaskDetailModel.ScanQty + outStockTaskDetailModel.ReviewQty >= outStockTaskDetailModel.UnShelveQty)
            {
                outStockTaskDetailModel.ReviewStatus = 3;//全部复核
            }
            else if (outStockTaskDetailModel.ScanQty + outStockTaskDetailModel.ReviewQty < outStockTaskDetailModel.UnShelveQty)
            {
                outStockTaskDetailModel.ReviewStatus = 2;//部分复核
            }
        }

        /// <summary>
        /// 验证69码输入的数量
        /// 验证扫描序列号库存的数量
        /// </summary>
        /// <param name="model"></param>
        /// <param name="outStockDetailModel"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        private  bool CheckReviewQty(T_StockInfo model,   T_OutStockTaskDetailsInfo outStockTaskDetailModel,ref string strError)
        {
            if (outStockTaskDetailModel.ReviewQty + model.ScanQty > outStockTaskDetailModel.UnShelveQty)
            {
                strError = "扫描的数量累加已复核数量超出拣货数量！" + model.MaterialNo;
               return false;
            }

            if (model.ScanQty > model.Qty) 
            {
                strError = "输入复核数量大于69码可复核数量" + model.MaterialNo;
                return false;
            }
            
            return true;
        }
                

        /// <summary>
        /// PC端扫描ENA输入数量保存数据
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool SaveT_OutStockReviewDetailENA(UserModel user, T_StockInfo model, ref List<T_OutStockDetailInfo> outStockDetailList,ref int ID, ref string strError)
        {
            try
            {
                List<T_OutStockTaskDetailsInfo> modelListTaskDetail = new List<T_OutStockTaskDetailsInfo>();
                T_OutTaskDetails_DB tdb = new T_OutTaskDetails_DB();

                if (model == null || string.IsNullOrEmpty(model.Barcode))
                {
                    strError = "客户端传来的数据或者条码为空！";
                    return false;
                }

                if (string.IsNullOrEmpty(model.ErpVoucherNo))
                {
                    strError = "客户端传来ERP订单号为空！";
                    return false;
                }
                if (model.ScanQty < 0) 
                {
                    strError = "复核数量不能小于零！";
                    return false;
                }
                string strFilter = "erpvoucherno = '" + model.ErpVoucherNo.Trim() + "'";

                T_OutStockInfo outStockModel = new T_OutStockInfo();
                T_OutStock_Func tfunc = new T_OutStock_Func();
                bool bSucc = tfunc.GetModelByFilter(ref outStockModel, strFilter, ref strError);
                if (bSucc == false)
                {
                    return false;
                }

                if (CheckReviewOutStockStatus(ref strError, outStockModel) == false) 
                {
                    return false;
                }

                //List<T_OutStockDetailInfo> outStockModelList = new List<T_OutStockDetailInfo>();
                //bool bSucc = base.GetModelListByFilter(ref outStockModelList, ref strError, "", strFilter, "*");
                //if (bSucc == false)
                //{
                //    return false;
                //}

                ////相同物料汇总
                //List<T_OutStockDetailInfo> newOutStockModelListSum = OutStockSameMaterialNoSumQty(outStockModelList);

                //var outStockDetailModel = newOutStockModelListSum.Find(t => t.MaterialNoID == model.MaterialNoID);

                //验证扫描提交的数量 + 已复核的数量是否大于拣货数量
                if (tdb.GetOutTaskDetailByErpVoucherNo(model.ErpVoucherNo, ref modelListTaskDetail, ref strError) == false)
                {
                    return false;
                }

                var outStockDetailTaskModel = modelListTaskDetail.Find(t => t.MaterialNoID == model.MaterialNoID && t.ID == model.TaskDetailesID);

                if (outStockDetailTaskModel == null)
                {
                    strError = "扫描的条码对应物料不在复核订单中！对应的物料编码：" + model.MaterialNo;
                    return false;
                }

                ID = outStockDetailTaskModel.ID;
                if (CheckReviewQty(model, outStockDetailTaskModel, ref strError) == false)
                {
                    return false;
                }

                SetOutStockDetailStatusAndQty(model, ref  outStockDetailTaskModel);

                //确定表头状态
                SetOutStockStatus(ref  modelListTaskDetail);

                //newOutStockModelListSum = newOutStockModelListSum.Where(t => t.ScanQty > 0).ToList();
                modelListTaskDetail = modelListTaskDetail.Where(t => t.ScanQty > 0).ToList();

                T_OutStockDetail_DB _db = new T_OutStockDetail_DB();
                bSucc = _db.SaveReviewModelListSql(user, modelListTaskDetail, ref strError);
                if (bSucc == false)
                {
                    return false;
                }

                //bSucc = base.GetModelListByFilter(ref outStockDetailList, ref strError, "", strFilter, "*");
                //if (bSucc == false)
                //{
                //    return false;
                //}

                //outStockDetailList = OutStockSameMaterialNoSumQty(outStockDetailList);

                if (tdb.GetOutTaskDetailByErpVoucherNo(model.ErpVoucherNo, ref modelListTaskDetail, ref strError) == false)
                {
                    return false;
                }

                outStockDetailList = _db.CreateOutStockDetailByTaskDetail(modelListTaskDetail);

                return true;
            }
            catch (Exception ex) 
            {
                strError = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// PC端一键扫描复核
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool SaveAllT_OutStockReviewDetailENA(string strErpVoucherNo, UserModel user, ref string strError) 
        {
            try
            {
                bool bSucc = false;

                List<T_OutStockTaskDetailsInfo> modelListTaskDetail = new List<T_OutStockTaskDetailsInfo>();
                T_OutTaskDetails_DB tdb = new T_OutTaskDetails_DB();

                if (string.IsNullOrEmpty(strErpVoucherNo))
                {
                     strError = "客户端传来ERP单号为空！";
                    return false;
                }

                if (user == null) 
                {
                    strError = "客户端传来用户为空！";
                    return false;
                }


                string strFilter = "erpvoucherno = '" + strErpVoucherNo.Trim() + "'";

                if (tdb.GetOutTaskDetailByErpVoucherNo(strErpVoucherNo, ref modelListTaskDetail, ref strError) == false)
                {
                    return false;
                }


                if (modelListTaskDetail[0].ReviewStatus == 3) 
                {
                    strError = "订单已经全部复核完成，不能一键复核！";
                    return false;
                }

                //if (modelListTaskDetail.Where(t => t.RemainQty > 0).Count() >= 1) 
                //{
                //    strError = "订单未全部拣货完成，不能一键复核！";
                //    return false;
                //}

                T_Stock_DB stockdb = new T_Stock_DB();
                foreach (var item in modelListTaskDetail) 
                {
                    item.ScanQty = item.UnShelveQty.ToDecimal();
                    item.lstStockInfo =  stockdb.GetStockByTaskDetailID(item.ID);
                    item.ReviewStatus = 3;
                }

                
                T_OutStockDetail_DB _db = new T_OutStockDetail_DB();
                bSucc = _db.SaveYiJianReviewModelListSql(user, modelListTaskDetail, ref strError);
                if (bSucc == false)
                {
                    return false;
                }

                
                return true;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }


        /// <summary>
        /// 复核验证单据表头状态
        /// </summary>
        /// <param name="strError"></param>
        /// <param name="outStockModel"></param>
        /// <returns></returns>
        private bool CheckReviewOutStockStatus(ref string strError, T_OutStockInfo outStockModel)
        {
            if (outStockModel.Status == 5)
            {
                strError = "该单号已经过账，不能再次复核";
                return false;
            }

            if (outStockModel.Status == 4)
            {
                strError = "该单号已经复核完成，不能再次复核";
                return false;
            }

            //if (outStockModel.VoucherType == 24)
            //{
            //    if (!string.IsNullOrEmpty(outStockModel.ShipWFlg) && outStockModel.ShipWFlg == "Y") 
            //    {
            //        strError = "该单号等外调，不能复核！";
            //        return false;
            //    }
            //}

            return true;
        }

        public List<T_OutStockDetailInfo> OutStockSameMaterialNoSumQty(List<T_OutStockDetailInfo> modelList)
        {

            var newModelList = from t in modelList
                               group t by new
                               {
                                   t1 = t.ErpVoucherNo,
                                   t2 = t.MaterialNoID,
                                   t3 = t.StrongHoldCode,
                                   t4 = t.StrongHoldName,
                                   t5 = t.CompanyCode
                               } into m
                               select new T_OutStockDetailInfo
                               {
                                   ID = m.FirstOrDefault().ID,
                                   RowNo = m.FirstOrDefault().RowNo,
                                   ErpVoucherNo = m.Key.t1,
                                   MaterialNoID = m.Key.t2,
                                   StrongHoldCode = m.Key.t3,
                                   StrongHoldName = m.Key.t4,
                                   CompanyCode = m.Key.t5,
                                   HeaderID = m.FirstOrDefault().HeaderID,
                                   DepartmentCode = m.FirstOrDefault().DepartmentCode,
                                   DepartmentName = m.FirstOrDefault().DepartmentName,
                                   VoucherNo = m.FirstOrDefault().VoucherNo,
                                   MaterialNo = m.FirstOrDefault().MaterialNo,
                                   MaterialDesc = m.FirstOrDefault().MaterialDesc,
                                   Unit = m.FirstOrDefault().Unit,
                                   OutStockQty = m.Sum(p => p.OutStockQty),
                                   RemainQty = m.Sum(p => p.RemainQty),
                                   PickQty = m.Sum(p=>p.PickQty),
                                   ReviewQty = m.Sum(p=>p.ReviewQty),
                                   CustomerCode = m.FirstOrDefault().CustomerCode,
                                   CustomerName = m.FirstOrDefault().CustomerName,
                                   VoucherType = m.FirstOrDefault().VoucherType,
                                   MainTypeCode = m.FirstOrDefault().MainTypeCode,                                  
                                   ERPVoucherType = m.FirstOrDefault().ERPVoucherType,
                                   ShipNFlg = m.FirstOrDefault().ShipNFlg,
                                   ShipDFlg = m.FirstOrDefault().ShipDFlg,
                                   ShipPFlg = m.FirstOrDefault().ShipPFlg,
                                   ShipWFlg = m.FirstOrDefault().ShipWFlg,
                                   TradingConditions = m.FirstOrDefault().TradingConditions,
                                   Province = m.FirstOrDefault().Province,
                                   City = m.FirstOrDefault().City,
                                   Area = m.FirstOrDefault().Area,
                                   Address = m.FirstOrDefault().Address,
                                   Address1 = m.FirstOrDefault().Address1,
                                   Contact = m.FirstOrDefault().Contact,
                                   Phone = m.FirstOrDefault().Phone,                                   
                                   ERPNote = m.FirstOrDefault().ERPNote,
                                   LineStatus = m.FirstOrDefault().LineStatus


                               };

            return newModelList.ToList();
        }


        public List<T_StockInfo> GroupByStockQtyByEan(List<T_StockInfo> modelList) 
        {
            var groupList = from t in modelList
                            group t by new { t1 = t.MaterialNoID,t2 = t.TaskDetailesID,t3 = t.BatchNo } into m
                            select new T_StockInfo
                               {
                                   MaterialNoID = m.Key.t1,
                                   Qty = m.Sum(p => p.Qty),
                                   EAN = m.FirstOrDefault().EAN,
                                   MaterialNo = m.FirstOrDefault().MaterialNo,
                                   SaveType = m.FirstOrDefault().SaveType,
                                   SerialNo = m.FirstOrDefault().SerialNo,
                                   HouseProp = m.FirstOrDefault().HouseProp,
                                   TaskDetailesID = m.Key.t2,
                                   Status = m.FirstOrDefault().Status,
                                   MaterialDesc = m.FirstOrDefault().MaterialDesc,
                                   BatchNo = m.Key.t3
                               };

            return groupList.ToList();
        }

        public bool CheckReviewOutStockStatus(ref string strError, T_StockInfo stockModel)
        {
            if (stockModel.TaskDetailesID == 0)
            {
                strError = "该条码未拣货下架！";
                return false;
            }

            if (stockModel.IsAmount == 2) 
            {
                strError = "拆零标签不能在PDA复核，请在PC扫描69码复核！";
                return false;
            }

            return true;
        }


        #endregion

        #region 扫描拣货车条码

        public bool ScanOutStockReviewByCarNo(string CarNo, ref List<T_OutStockDetailInfo> modelList , ref string strError)
        {
            try
            {
                T_OutStockDetail_DB _tdb = new T_OutStockDetail_DB();

                if (string.IsNullOrEmpty(CarNo))
                {
                    strError = "拣货车条码为空!";
                    return false;
                }

                string strErpVoucherNo = _tdb.ScanOutStockReviewByCarNo(CarNo);

                if (string.IsNullOrEmpty(strErpVoucherNo))
                {
                    strError = "拣货车条码未获取到对应的订单信息";
                    return false;
                }

                string strFilter = "ErpVoucherNo = '"+strErpVoucherNo+"'";

                return base.GetModelListByFilter(ref modelList, ref strError, "", strFilter);

            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        #endregion

        #region 复核组托

        public string SaveT_OutStockReviewPalletDetailADF(string UserJson, string ModelJson) 
        {
            BaseMessage_Model<T_OutStockDetailInfo> messageModel = new BaseMessage_Model<T_OutStockDetailInfo>();
            try
            {
                string strError = string.Empty;
                if (string.IsNullOrEmpty(UserJson))
                {
                    messageModel.Message = "客户端传入用户为空！";
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);
                }

                if (string.IsNullOrEmpty(ModelJson))
                {
                    messageModel.Message = "客户端传入复核数据为空！";
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);
                }

                UserModel model = BILBasic.JSONUtil.JSONHelper.JsonToObject<UserModel>(UserJson);
                List<T_OutStockDetailInfo> modelList = BILBasic.JSONUtil.JSONHelper.JsonToObject<List<T_OutStockDetailInfo>>(ModelJson);

                if (modelList == null || modelList.Count == 0) 
                {
                    messageModel.Message = "客户端传入JSON转复核数据为空！";
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);
                }

                //if (modelList.Where(t => t.ScanQty > 0).Count() == 0) 
                //{
                //    messageModel.Message = "复核扫描数量为零！";
                //    messageModel.HeaderStatus = "E";
                //    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);
                //}

                //if(modelList.Where(t=>t.lstStock==null).Count() != modelList.Count)
                //{
                //    messageModel.Message = "复核扫描没有全部完成，不能提交数据！";
                //    messageModel.HeaderStatus = "E";
                //    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);
                //}
                LogNet.LogInfo("SaveT_OutStockReviewPalletDetailADF复核组托:" + ModelJson);
                T_OutStockDetail_DB _db = new T_OutStockDetail_DB();

                string strPalletNo = _db.GetFHPalletNo(GetFirstSerialNo(modelList));

                if (!string.IsNullOrEmpty(strPalletNo)) 
                {
                    messageModel.Message = "该条码已经组托，托盘号：" + strPalletNo;
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);
                }

                if (_db.SaveT_OutStockReviewDetailADF(model, modelList, ref strError) == false)
                {
                    messageModel.Message = strError;
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);
                }
                else 
                {
                    messageModel.Message = "复核组托成功！";
                    messageModel.HeaderStatus = "S";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);
                }
                
            }
            catch (Exception ex) 
            {
                messageModel.Message = ex.Message;
                messageModel.HeaderStatus = "E";
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);
            }
        }

        private string GetFirstSerialNo(List<T_OutStockDetailInfo> modelList) 
        {
            string strSerialNo = string.Empty;

            foreach (var item in modelList)
            {
                if (item.lstStock != null && item.lstStock.Count > 0)
                {
                    foreach (var itemStock in item.lstStock)
                    {
                        strSerialNo = itemStock.SerialNo;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(strSerialNo)) { break; }
            }

            return strSerialNo;

        }
         #endregion

        protected override T_OutStockDetailInfo GetModelByJson(string strJson)
        {
            string errorMsg = string.Empty;
            T_OutStockDetailInfo model = JSONHelper.JsonToObject<T_OutStockDetailInfo>(strJson);

            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
                PFunc.Sync(20, string.Empty, model.ErpVoucherNo, model.VoucherType, ref errorMsg, "ERP", -1, null);
            }
            return JSONHelper.JsonToObject<T_OutStockDetailInfo>(strJson);
        }

        protected override List<T_OutStockDetailInfo> GetModelListByJson(string UserJson, string ModelListJson)
        {
            UserModel userModel = JSONHelper.JsonToObject<UserModel>(UserJson);
            List<T_OutStockDetailInfo> modelList = new List<T_OutStockDetailInfo>();
            modelList = JSONHelper.JsonToObject<List<T_OutStockDetailInfo>>(ModelListJson);
            //modelList = modelList.Where(t => t.OldOutStockQty > 0).ToList();
            //modelList.ForEach(t => t.PostUser = userModel.UserNo);
            //modelList.ForEach(t => t.ToErpAreaNo = userModel.PickAreaNo);
            //modelList.ForEach(t => t.ToErpWarehouse = userModel.PickWareHouseNo);
            return modelList;
        }

        /// <summary>
        /// 生成过账JSON给ERP
        /// </summary>
        /// <param name="user"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        protected override string GetModelListByJsonToERP(UserModel user, List<T_OutStockDetailInfo> modelList) 
        {
            List<T_OutStockDetailInfo> lstDetail = new List<T_OutStockDetailInfo>();
            List<T_StockInfo> lstStock = new List<T_StockInfo>();            
            int VoucherType = 0;
            VoucherType = modelList[0].VoucherType;
            string strUserNo = string.Empty;
            //var GUID = Guid.NewGuid().ToString();
            

            foreach (var item in modelList) 
            {
                //拣货多批次，需要汇总批次给ERP过账，ERP需要拆行
                lstStock = GroupInstockDetailList(VoucherType, item.lstStock);
                foreach(var itemStock in lstStock)
                {
                    T_OutStockDetailInfo ToErpModel = new T_OutStockDetailInfo();
                    ToErpModel.VoucherType = item.VoucherType;
                    ToErpModel.ErpVoucherNo = item.ErpVoucherNo;
                    ToErpModel.RowNo = item.RowNo;
                    ToErpModel.RowNoDel = item.RowNoDel;
                    ToErpModel.CompanyCode = item.CompanyCode;
                    ToErpModel.MaterialNo = item.MaterialNo;
                    //ToErpModel.StrongHoldCode = item.StrongHoldCode;
                    ToErpModel.StrongHoldCode = item.StrongholdcodeHeader;//出库过账取单头
                    ToErpModel.StrongholdcodeDetail = item.StrongHoldCode;//出库过账取单身
                    ToErpModel.Unit = item.Unit;
                    ToErpModel.ToErpWarehouse = item.ToErpWarehouse;//( item.VoucherType == 23 || item.VoucherType==50 ) ? item.ToErpWarehouse: itemStock.ToErpWarehouse; //user.PickWareHouseNo;
                    ToErpModel.ToErpAreaNo = item.ToErpAreaNo;//(item.VoucherType == 23 || item.VoucherType==50 ) ? item.ToErpAreaNo : itemStock.ToErpAreaNo;//user.PickAreaNo;
                    ToErpModel.ToBatchno = itemStock.ToBatchNo;//itemStock.ToBatchNo;
                    ToErpModel.FromBatchNo = itemStock.ToBatchNo;
                    ToErpModel.FromErpWarehouse = item.FromErpWarehouse;
                    ToErpModel.FromErpAreaNo = item.FromErpAreaNo;
                    ToErpModel.PostUser = user.UserNo;// strPostUser;
                    ToErpModel.ScanQty = itemStock.Qty;
                    ToErpModel.SourceVoucherNo = item.SourceVoucherNo;
                    ToErpModel.SourceRowNo = item.SourceRowNo;
                    ToErpModel.StrPostDate = DateTime.Now.ToString("yyyy-MM-dd");
                    ToErpModel.GUID = item.GUID;
                    lstDetail.Add(ToErpModel);
                } 
            }

            return JSONHelper.ObjectToJson<List<T_OutStockDetailInfo>>(lstDetail);
        }

        private List<T_StockInfo> GroupInstockDetailList(int VoucherType, List<T_StockInfo> modelList)
        {
            var newModelList = from t in modelList
                               group t by new { t1 = t.MaterialNo, t2 = t.BatchNo, t3 = t.WarehouseNo, t4 = t.EDate } into m
                               select new T_StockInfo
                               {
                                   MaterialNo = m.Key.t1,
                                   Qty = m.Sum(item => item.Qty),
                                   ToBatchNo = m.Key.t2,
                                   ToErpWarehouse = m.Key.t3,
                                   EDate = m.Key.t4,
                                   FromBatchNo = m.Key.t2,
                                   FromErpWarehouse = m.Key.t3,
                                   FromErpAreaNo = "",
                                   VoucherType = VoucherType,
                                   CompanyCode = m.FirstOrDefault().CompanyCode,
                                   StrongHoldCode = m.FirstOrDefault().StrongHoldCode
                               };

            return newModelList.ToList();
        }


        #region 物料转换

        public bool GetChengeMaterialForStock(ref T_OutStockDetailInfo model, ref string strError) 
        {
            try
            {
                string StrongHoldCode = string.Empty;
                T_Stock_DB stockDb = new T_Stock_DB();
                List<T_StockInfo> stockList = new List<T_StockInfo>();
                stockList = stockDb.GetStockByCheangeMaterial(model);
                int ID = model.ID;
                stockList.ForEach(t => t.OKSelect = t.MaterialChangeID == ID ? true : false);
                model.lstStock = stockList;

                return true;

            }
            catch (Exception ex) 
            {
                strError = ex.Message;
                return false;
            }
        }

        public bool UpdateChangeMaterial(ref T_OutStockDetailInfo model, ref string strError) 
        {
            try
            {
                List<T_OutStockDetailInfo> modelList = new List<T_OutStockDetailInfo>();
                T_OutStockDetail_DB tdb = new T_OutStockDetail_DB();
                if (model.lstStock == null || model.lstStock.Count == 0) 
                {
                    strError = "没有库存数据提交！";
                    return false;
                }

                if (model.lstStock.Where(t => t.OKSelect == true).Count() == 0) 
                {
                    strError = "未选中任何数据！";
                    return false;
                }

                modelList.Add(model);

                if (CheckChangeMaterialQty(modelList, ref strError) == false)
                {
                    return false;
                }

                model.lstStock = model.lstStock.Where(t => t.OKSelect == true).ToList();

                return tdb.UpdateChangeMaterial(model, ref strError);
            }
            catch (Exception ex) 
            {
                strError = ex.Message;
                return false;
            }
        }

        public string SaveT_ChangeMaterialADF(string UserJson, string InStockDetailListJson, string OutStockDetailListJson )
        {
            BaseMessage_Model<T_OutStockDetailInfo> messageModel = new BaseMessage_Model<T_OutStockDetailInfo>();

            try
            {
                bool bSucc = false;
                string strError = string.Empty;
                string strPostInStrError = string.Empty;
                string strPostOutStrError = string.Empty;
                string strWMSError = string.Empty;

                if (string.IsNullOrEmpty(UserJson))
                {
                    messageModel.Message = "客户端传入用户JSON为空！";
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);
                }

                if (string.IsNullOrEmpty(InStockDetailListJson))
                {
                    messageModel.Message = "客户端传入无订单入JSON为空！";
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);
                }

                if (string.IsNullOrEmpty(OutStockDetailListJson))
                {
                    messageModel.Message = "客户端传入无订单出JSON为空！";
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);
                }

                List<T_OutStockDetailInfo> OutStockDetailList = JSONHelper.JsonToObject<List<T_OutStockDetailInfo>>(OutStockDetailListJson);

                if (OutStockDetailList == null || OutStockDetailList.Count == 0)
                {
                    messageModel.Message = "客户端传来无订单出库表体数据为空！";
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);                    
                }                

                if (OutStockDetailList.Where(t => t.lstStock == null).Count() > 0)
                {
                    messageModel.Message = "表体数据存在未扫描的物料！请确认！";
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);   
                }

                if (CheckChangeMaterialQty(OutStockDetailList, ref strError) == false)
                {
                    messageModel.Message = strError;
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);
                }

                UserModel userModel = JSONHelper.JsonToObject<UserModel>(UserJson);

                List<T_InStockDetailInfo> InStockDetailList = JSONHelper.JsonToObject<List<T_InStockDetailInfo>>(InStockDetailListJson);


                //过T100的帐
                if (PostInOtherNull(userModel, InStockDetailList, ref strPostInStrError) == false)
                {
                    messageModel.Message = strPostInStrError;
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);                
                   
                }
                else
                {
                    bSucc = true;
                    strError = strPostInStrError;
                }

                if (PostOutOtherNull(userModel, OutStockDetailList, ref strPostOutStrError) == false)
                {
                    messageModel.Message = strError;
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);
                }
                else
                {
                    bSucc = true;
                    strError += strPostOutStrError;
                }

                T_OutStockDetail_DB tdb = new T_OutStockDetail_DB();
                bSucc = tdb.SaveT_ChangeMaterial(InStockDetailList, OutStockDetailList, ref strWMSError);

                if (bSucc == false)
                {
                    strError += "WMS转料失败：" + strWMSError;
                    messageModel.Message = strError;
                    messageModel.HeaderStatus = "E";                                       
                }
                else
                {
                    strError += "WMS转料成功！";                    
                    messageModel.Message = strError;
                    messageModel.HeaderStatus = "S";                    
                }

                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel); 

            }
            catch (Exception ex)
            {
                messageModel.Message = ex.Message;
                messageModel.HeaderStatus = "E";
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_OutStockDetailInfo>>(messageModel);
            }

        }

        public bool SaveT_ChangeMaterial(UserModel userModel,List<T_InStockDetailInfo> InStockDetailList,List<T_OutStockDetailInfo> OutStockDetailList,ref string strError)        
        {
            try
            {
                bool bSucc = false;
                string strPostInStrError = string.Empty;
                string strPostOutStrError = string.Empty;
                string strWMSError = string.Empty;

                if (OutStockDetailList == null || OutStockDetailList.Count == 0) 
                {
                    strError = "客户端传来无订单出库表体数据为空！";
                    return false;
                }

                if (OutStockDetailList.Where(t => t.lstStock==null).Count() > 0 ) 
                {
                    strError = "表体数据存在未选择库存的物料！请确认！";
                    return false;
                }

                if (CheckChangeMaterialQty(OutStockDetailList, ref strError) == false) 
                {
                    return false;
                }

                //过T100的帐
                if (PostInOtherNull(userModel, InStockDetailList, ref strPostInStrError) == false) 
                {
                    strError = strPostInStrError;
                    return false;
                }
                else
                {
                    bSucc = true;
                    strError = strPostInStrError;
                }

                if (PostOutOtherNull(userModel, OutStockDetailList, ref strPostOutStrError) == false)
                {
                    strError += strPostOutStrError;
                    return false;
                }
                else 
                {
                    bSucc = true;
                    strError += strPostOutStrError;
                }

                T_OutStockDetail_DB tdb = new T_OutStockDetail_DB();
                bSucc = tdb.SaveT_ChangeMaterial(InStockDetailList,OutStockDetailList, ref strWMSError);

                if (bSucc == false)
                {
                    strError += "\r\nWMS转料失败：" + strWMSError;
                }
                else 
                {
                    strError += "WMS转料成功！";
                }

                return bSucc;

            }
            catch (Exception ex) 
            {
                strError = ex.Message;
                return false;
            }
        }

        public bool PostInOtherNull(UserModel userModel, List<T_InStockDetailInfo> modelList,ref string strError) 
        {
            try
            {
                BaseMessage_Model<List<T_InStockDetailInfo>> model = new BaseMessage_Model<List<T_InStockDetailInfo>>();
                bool bSucc = false;
                string strUserNo = string.Empty;
                string strPostUser = string.Empty;

                modelList.ForEach(t => t.ScanQty = t.InStockQty);
                modelList.ForEach(t => t.ReceiveWareHouseNo = t.FromErpWarehouse);
                modelList.ForEach(t => t.ReceiveAreaNo = t.FromErpAreaNo);
                modelList.ForEach(t => t.BatchNo = t.FromBatchNo);
                //if (TOOL.RegexMatch.isExists(userModel.UserNo) == true)
                //{
                //    strUserNo = userModel.UserNo.Substring(0, userModel.UserNo.Length - 1);
                //}
                //else 
                //{
                //    strUserNo = userModel.UserNo;
                //}

                //User_DB _db = new User_DB();
                //strPostUser = _db.GetPostAccountByUserNo(strUserNo, modelList[0].StrongHoldCode);

                modelList.ForEach(t => t.ReceiveUserNo = userModel.UserNo);  //strPostUser              

                T_Interface_Func tfunc = new T_Interface_Func();
                string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<T_InStockDetailInfo>>(modelList);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

                model = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_InStockDetailInfo>>>(interfaceJson);

                LogNet.LogInfo("ERPJsonAfter:" + BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_InStockDetailInfo>>>(model));

                //过账失败直接返回
                if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
                {
                    strError ="无订单入库过账失败！" + model.Message;
                    bSucc = false;
                }
                else if (model.HeaderStatus == "S" && !string.IsNullOrEmpty(model.MaterialDoc)) //过账成功，并且生成了凭证要记录数据库
                {
                    strError = "无订单入库过账成功！凭证号：" + model.MaterialDoc;
                    bSucc = true;
                }

                return bSucc;
            }
            catch (Exception ex) 
            {
                strError = ex.Message;
                return false;
            }
            

        }

        public bool PostOutOtherNull(UserModel userModel, List<T_OutStockDetailInfo> modelList, ref string strError)
        {
            try
            {
                BaseMessage_Model<List<T_OutStockDetailInfo>> model = new BaseMessage_Model<List<T_OutStockDetailInfo>>();
                bool bSucc = false;
                string strUserNo = string.Empty;
                string strPostUser = string.Empty;

                modelList.ForEach(t => t.ScanQty = t.OutStockQty);
                modelList.ForEach(t => t.ToErpWarehouse = t.FromErpWarehouse);
                modelList.ForEach(t => t.ToErpAreaNo = t.FromErpAreaNo);
                modelList.ForEach(t => t.ToBatchno = t.FromBatchNo);
                //if (TOOL.RegexMatch.isExists(userModel.UserNo) == true)
                //{
                //    strUserNo = userModel.UserNo.Substring(0, userModel.UserNo.Length - 1);
                //}
                //else
                //{
                //    strUserNo = userModel.UserNo;
                //}

                //User_DB _db = new User_DB();
                //strPostUser = _db.GetPostAccountByUserNo(strUserNo, modelList[0].StrongHoldCode);
                modelList.ForEach(t => t.PostUser = userModel.UserNo);//strPostUser
                

                T_Interface_Func tfunc = new T_Interface_Func();
                string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<T_OutStockDetailInfo>>(modelList);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

                model = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_OutStockDetailInfo>>>(interfaceJson);

                LogNet.LogInfo("ERPJsonAfter:" + BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutStockDetailInfo>>>(model));

                //过账失败直接返回
                if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
                {
                    strError ="无订单出库过账失败！" + model.Message;
                    bSucc = false;
                }
                else if (model.HeaderStatus == "S" && !string.IsNullOrEmpty(model.MaterialDoc)) //过账成功，并且生成了凭证要记录数据库
                {
                    strError = "无订单出库过账成功！凭证号：" + model.MaterialDoc;
                    bSucc = true;
                }

                return bSucc;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }


        }

        private bool CheckChangeMaterialQty(List<T_OutStockDetailInfo> modelList,ref string strError)
        {
            bool bSucc = false;
            foreach (var item in modelList) 
            {
                if (item.OutStockQty != item.lstStock.Where(t=>t.OKSelect==true).Sum(t => t.Qty))
                {
                    strError = "物料：" + item.MaterialNo + "项次：" + item.RowNo + "项序：" + item.RowNoDel + "订单数量和选择转料库存数量不一致！";
                    bSucc = false;
                    break;
                }
                else { bSucc = true; }
            }

            return bSucc;
        }


       

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="ModelJson"></param>
        /// <returns></returns>
        public string SaveT_OutStockReviewDetailADF(string UserJson, string ModelJson)
        {
            BaseMessage_Model<List<T_OutStockDetailInfo>> messageModel = new BaseMessage_Model<List<T_OutStockDetailInfo>>();


            try
            {
                LogNet.LogInfo("SaveT_OutStockReviewDetailADF:" + ModelJson);

                string strError = string.Empty;

                if (string.IsNullOrEmpty(ModelJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来的业务JSON为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                if (string.IsNullOrEmpty(UserJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来的人员JSON为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                UserModel user = JsonConvert.DeserializeObject<UserModel>(UserJson);//JSONUtil.JSONHelper.JsonToObject<User.UserModel>(UserJson);
                List<T_OutStockDetailInfo> modelList = JsonConvert.DeserializeObject<List<T_OutStockDetailInfo>>(ModelJson);

                if (modelList == null || modelList.Count == 0) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "转换业务对象为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                //if (modelList.Where(t => t.ScanQty == 0).Count() == 0)
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = "扫描数量都为零，不能提交数据";
                //    return JsonConvert.SerializeObject(messageModel);
                //}

                modelList = modelList.Where(t => t.ScanQty > 0).ToList();

                if (modelList == null || modelList.Count == 0) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "扫描数量都为零，不能提交数据";
                    return JsonConvert.SerializeObject(messageModel);
                }

                T_OutTaskDetails_Func tfunc = new T_OutTaskDetails_Func();
                List<T_OutStockTaskDetailsInfo> lstTaskDetail = new List<T_OutStockTaskDetailsInfo>();
                bool bSucc = tfunc.GetModelListByHeaderID(ref lstTaskDetail, modelList[0].HeaderID, ref strError);
                if (bSucc == false) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JsonConvert.SerializeObject(messageModel);
                }

                var taskDetailModel = lstTaskDetail.Find(t => t.ID == modelList[0].ID);
                if (taskDetailModel == null) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "复核提交的数据在拣货单中未找到！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                //if (taskDetailModel.ReviewQty + modelList[0].ScanQty > taskDetailModel.UnShelveQty) 
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = "提交的数量累加已复核数量超出拣货数量！";
                //    return JsonConvert.SerializeObject(messageModel);
                //}

                if (CheckReviewQty(lstTaskDetail, modelList, ref strError) == false) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JsonConvert.SerializeObject(messageModel);
                }

                //taskDetailModel.lstStockInfo = modelList[0].lstStock;
                //taskDetailModel.ScanQty = modelList[0].ScanQty;

                //if (taskDetailModel.ScanQty + taskDetailModel.ReviewQty >= taskDetailModel.UnShelveQty)
                //{
                //    taskDetailModel.ReviewStatus = 3;//全部复核
                //}
                //else if (taskDetailModel.ScanQty + taskDetailModel.ReviewQty < taskDetailModel.UnShelveQty)
                //{
                //    taskDetailModel.ReviewStatus = 2;//部分复核
                //}

                SetDetailStatus(ref lstTaskDetail, modelList);

                //确定表头状态
                SetOutStockStatus(ref  lstTaskDetail);

                lstTaskDetail = lstTaskDetail.Where(t => t.ScanQty > 0).ToList();

                T_OutStockDetail_DB _db = new T_OutStockDetail_DB();
                bSucc = _db.SaveReviewModelListSql(user, lstTaskDetail, ref strError);
                if (bSucc == false)
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

        public bool CheckReviewQty(List<T_OutStockTaskDetailsInfo> lstVoucherTaskDetail, List<T_OutStockDetailInfo> modelList, ref string strError)
        {
            bool bSucc = false;
            foreach (var item in modelList) 
            {
                var taskDetailModel = lstVoucherTaskDetail.Find(t=>t.ID==item.ID);
                if (taskDetailModel.ReviewQty + modelList[0].ScanQty > taskDetailModel.UnShelveQty)
                {
                    strError = "提交的数量累加已复核数量超出拣货数量！物料号："+taskDetailModel.MaterialNo+",行号："+taskDetailModel.RowNo+"" ;
                    bSucc = false;
                    break;
                }
                else { bSucc = true; }
            }

            return bSucc;
        }

        public void SetDetailStatus(ref List<T_OutStockTaskDetailsInfo> lstVoucherTaskDetail, List<T_OutStockDetailInfo> modelList) 
        {
            foreach (var item in modelList) 
            {
                var taskDetailModel = lstVoucherTaskDetail.Find(t => t.ID == item.ID);
                if (taskDetailModel != null) 
                {
                    taskDetailModel.lstStockInfo = item.lstStock;
                    taskDetailModel.ScanQty = item.ScanQty;
                    if (taskDetailModel.ScanQty + taskDetailModel.ReviewQty >= taskDetailModel.UnShelveQty)
                    {
                        taskDetailModel.ReviewStatus = 3;//全部复核
                    }
                    else if (taskDetailModel.ScanQty + taskDetailModel.ReviewQty < taskDetailModel.UnShelveQty)
                    {
                        taskDetailModel.ReviewStatus = 2;//部分复核
                    }
                }                
            }
        }

        #region PDA复核过账

        public string PostT_OutStockReviewDetailADF(string UserJson, string ErpVoucherNo)
        {
            BaseMessage_Model<List<T_OutStockDetailInfo>> messageModel = new BaseMessage_Model<List<T_OutStockDetailInfo>>();

            try
            {
                string strError = string.Empty;

                if (string.IsNullOrEmpty(ErpVoucherNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "传入单号为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                if (string.IsNullOrEmpty(UserJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来的人员JSON为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                UserModel user = JsonConvert.DeserializeObject<UserModel>(UserJson);//JSONUtil.JSONHelper.JsonToObject<User.UserModel>(UserJson);
                List<T_OutStockDetailInfo> modelList = new List<T_OutStockDetailInfo>();//JsonConvert.DeserializeObject<List<T_OutStockDetailInfo>>(ModelJson);

                string strFilter = "erpvoucherno = '" + ErpVoucherNo + "'";

                bool bSucc = base.GetModelListByFilter(ref modelList, ref strError, "", strFilter, "*");
                if (bSucc == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JsonConvert.SerializeObject(messageModel);
                }

                if (CheckPostStatus(ref strError, modelList) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JsonConvert.SerializeObject(messageModel);
                }

                if (CheckReviewAndPickQty(ref strError, modelList) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JsonConvert.SerializeObject(messageModel);
                }

                T_Stock_DB tdb = new T_Stock_DB();
                List<T_StockInfo> lstStock = tdb.GetStockPickByErpNo(ErpVoucherNo);

                GetOutStockDetailIntoStock(ref modelList, lstStock);
                //需要判断拣货数量是否和复核数量一致

                string ModelJson = JsonConvert.SerializeObject(modelList);
                T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
                return tfunc.SaveModelListSqlToDBADF(UserJson, ModelJson);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JsonConvert.SerializeObject(messageModel);
            }
        }


        #endregion

        #region PC复核过账

        public bool PostT_OutStockReviewDetail(UserModel user, string ErpVoucherNo,string GUID,ref string strError)
        {
            BaseMessage_Model<List<T_OutStockDetailInfo>> messageModel = new BaseMessage_Model<List<T_OutStockDetailInfo>>();

            try
            {
                string strError1 = string.Empty;
                if (string.IsNullOrEmpty(ErpVoucherNo))
                {
                    strError = "客户端传入单号为空！";
                    return false;
                }                

                List<T_OutStockDetailInfo> modelList = new List<T_OutStockDetailInfo>();//JsonConvert.DeserializeObject<List<T_OutStockDetailInfo>>(ModelJson);

                string strFilter = "erpvoucherno = '" + ErpVoucherNo + "'";

                bool bSucc = base.GetModelListByFilter(ref modelList, ref strError, "", strFilter, "*");
                if (bSucc == false)
                {
                    return false;
                }

                if (CheckPostStatus(ref strError, modelList) == false) 
                {
                    return false;
                }

                if (CheckReviewAndPickQty(ref strError, modelList) == false) 
                {
                    return false;
                }

                T_Stock_DB tdb = new T_Stock_DB();
                List<T_StockInfo> lstStock = tdb.GetStockPickByErpNo(ErpVoucherNo);

                GetOutStockDetailIntoStock(ref modelList, lstStock);
                //需要判断拣货数量是否和复核数量一致

                modelList.ForEach(t => t.GUID = GUID);
                string ModelJson = JsonConvert.SerializeObject(modelList);
                string UserJson = JsonConvert.SerializeObject(user);

                T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
                string strResult =  tfunc.SaveModelListSqlToDBADF(UserJson, ModelJson);
                messageModel = JsonConvert.DeserializeObject<BaseMessage_Model<List<T_OutStockDetailInfo>>>(strResult);

                strError = messageModel.Message;
                return messageModel.HeaderStatus == "E" ? false : true;               
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        private bool CheckReviewAndPickQty(ref string strError, List<T_OutStockDetailInfo> modelList)
        {
            string strError1 = string.Empty;

            List<T_OutStockDetailInfo> newOutStockModelList = OutStockSameMaterialNoSumQty(modelList);

            foreach (var item in newOutStockModelList)
            {
                if (item.ReviewQty < item.PickQty)
                {
                    strError1 += "物料号：" + item.MaterialNo + "拣货数量：" + item.PickQty + "复核数量：" + item.ReviewQty + "\r\n";
                }
            }

            if (!string.IsNullOrEmpty(strError1))
            {
                strError = "以下物料复核数量小于拣货数量，不能提交过账：" + "\r\n";
                strError += strError1;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 过账状态检查
        /// </summary>
        /// <param name="strError"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        private  bool CheckPostStatus(ref string strError, List<T_OutStockDetailInfo> modelList)
        {
            if (modelList[0].Status == 5)
            {
                strError = "该订单已经过账，不能重复过账！";
                return false;
            }

            if (modelList[0].Status == 1)
            {
                strError = "该订单处于新建状态，不能过账！";
                return false;
            }

            if (modelList[0].Status == 2)
            {
                strError = "该订单处于未复核，不能过账！";
                return false;
            }

            return true;
        }


        #endregion

        #region PC过账和PDA过账，需要将条码拆分到物料，给ERP过账

        public void GetOutStockDetailIntoStock(ref List<T_OutStockDetailInfo> modelList, List<T_StockInfo> lstStock)
        {
            List<T_StockInfo> newListStock = new List<T_StockInfo>();
            //将订单数量分配到订单可分配数量
            modelList.ForEach(t => t.CanOutStockQty = t.OutStockQty);
            modelList = modelList.OrderBy(t => t.Price).ToList();
            foreach (var item in modelList)
            {
                item.lstStock = new List<T_StockInfo>();
                var itemStockList = lstStock.FindAll(t => t.MaterialNoID == item.MaterialNoID && t.StrongHoldCode==item.StrongHoldCode && t.Qty > 0);
                if (itemStockList != null && itemStockList.Count > 0)
                {
                    foreach (var itemStock in itemStockList)
                    {
                        if (item.CanOutStockQty < itemStock.Qty)
                        {
                            //item.ScanQty = item.CanOutStockQty;
                            itemStock.Qty = itemStock.Qty - item.CanOutStockQty;                            
                            item.lstStock.Add(CreateNewStockModel(itemStock,item));
                            break;
                            
                        }
                        else if (item.CanOutStockQty == itemStock.Qty) 
                        {
                            //item.ScanQty =item.ScanQty +  itemStock.Qty;
                            item.lstStock.Add(CreateNewStockModel(itemStock, item));
                            itemStock.Qty = 0;                            
                            break;
                        }
                        else if (item.CanOutStockQty > itemStock.Qty)
                        {                            
                            item.CanOutStockQty = item.CanOutStockQty - itemStock.Qty;
                            item.lstStock.Add(CreateNewStockModel1(itemStock));
                            itemStock.Qty = 0;

                            //T_StockInfo model = new T_StockInfo();
                            //model = itemStock;
                            //model.Qty = item.CanOutStockQty;
                            //item.lstStock.Add(CreateNewStockModel(model));
                            //itemStock.Qty = itemStock.Qty - item.CanOutStockQty;
                            //item.CanOutStockQty = 0;
                            
                        }
                    }
                }

            }
        }



        #endregion

        private T_StockInfo CreateNewStockModel(T_StockInfo stockModel,T_OutStockDetailInfo itemModel)
        {
            T_StockInfo model = new T_StockInfo();
            model.Qty = itemModel.CanOutStockQty;
            model.BatchNo = stockModel.BatchNo;
            model.EDate = stockModel.EDate;
            model.EAN = stockModel.EAN;
            model.Barcode = stockModel.Barcode;
            model.SerialNo = stockModel.SerialNo;
            model.WarehouseNo = stockModel.WarehouseNo;
            model.StrongHoldCode = stockModel.StrongHoldCode;
            model.StrongHoldName = stockModel.StrongHoldName;
            model.CompanyCode = stockModel.CompanyCode;
            model.FromErpWarehouse = stockModel.FromErpWarehouse;            
            model.ToErpWarehouse = stockModel.ToErpWarehouse;            

            return model;
        }

        private T_StockInfo CreateNewStockModel1(T_StockInfo stockModel)
        {
            T_StockInfo model = new T_StockInfo();
            model.Qty = stockModel.Qty;
            model.BatchNo = stockModel.BatchNo;
            model.EDate = stockModel.EDate;
            model.EAN = stockModel.EAN;
            model.Barcode = stockModel.Barcode;
            model.SerialNo = stockModel.SerialNo;
            model.WarehouseNo = stockModel.WarehouseNo;
            model.StrongHoldCode = stockModel.StrongHoldCode;
            model.StrongHoldName = stockModel.StrongHoldName;
            model.CompanyCode = stockModel.CompanyCode;

            return model;
        }

        private T_StockInfo CreateNewStock(T_StockInfo stockModel, T_OutStockDetailInfo itemModel)
        {
            T_StockInfo model = new T_StockInfo();
            model.ID = stockModel.ID;
            model.Barcode = stockModel.Barcode;
            model.SerialNo = stockModel.SerialNo;
            model.MaterialNo = stockModel.MaterialNo;
            model.MaterialDesc = stockModel.MaterialDesc;
            model.WarehouseNo = stockModel.WarehouseNo;
            model.HouseNo = stockModel.HouseNo;
            model.AreaNo = stockModel.AreaNo;            
            model.Qty = itemModel.PickQty;            
            model.Status = stockModel.Status;
            model.IsDel = stockModel.IsDel;
            model.Creater = stockModel.Creater;
            model.CreateTime = stockModel.CreateTime;            
            model.BatchNo = stockModel.BatchNo;            
            model.ReturnSupCode = stockModel.ReturnSupCode;            
            model.ReturnSupName = stockModel.ReturnSupName;            
            model.TaskDetailesID =stockModel.TaskDetailesID;           
            model.Unit = stockModel.Unit;            
            model.WareHouseID = stockModel.WareHouseID;
            model.HouseID = stockModel.HouseID;
            model.AreaID = stockModel.AreaID;            
            model.MaterialNoID = stockModel.MaterialNoID;           
            model.CompanyCode = stockModel.CompanyCode;
            model.StrongHoldCode = stockModel.StrongHoldCode;
            model.StrongHoldName = stockModel.StrongHoldName;            
            model.EDate = stockModel.EDate;
            model.BatchNo = stockModel.BatchNo;

            model.HouseProp = stockModel.HouseProp;

            model.EAN = stockModel.EAN;

            return model;
        }

        #region PC复核页面打印装箱

        /// <summary>
        /// PC复核页面打印装箱
        /// </summary>
        /// <param name="ErpVoucherNo"></param>
        /// <param name="user"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool CreatePalletByTaskTrans(string ErpVoucherNo, UserModel user,ref List<T_PalletDetailInfo> palletModelList, ref string strError)
        {
            try
            {
                if (string.IsNullOrEmpty(ErpVoucherNo))
                {
                    strError = "客户端传来单号为空！";
                    return false;
                }

                T_OutStockDetail_DB _db = new T_OutStockDetail_DB();
                List<T_PalletDetailInfo> modelList = _db.CreatePalletByTaskTrans(ErpVoucherNo, user);

                List<T_PalletDetailInfo> modelListPallet = OutStockPalletSumQty(modelList).ToList();
                modelListPallet = modelListPallet.Where(t => t.PalletNo == string.Empty && t.HouseProp == 2).ToList();

                if (modelList == null || modelList.Count == 0) 
                {
                    strError = "未获取到订单复核数据！请确认是否已经复核69码！";
                    return false;
                }

                string strPalletNo = string.Empty;
                T_PalletDetail_DB _pdb = new T_PalletDetail_DB();
                if (_pdb.CreateOutStockBox(modelList, modelListPallet, user, ref strPalletNo, ref strError) == false) 
                {
                    return false;
                }

                string strSql = "select * from t_Palletdetail where palletno = '"+strPalletNo+"'";
                palletModelList = _pdb.GetModelListBySql(strSql);

                if (modelList == null || modelList.Count == 0) 
                {
                    strError = "未获取到装箱数据！装箱单号：" + strPalletNo;
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        public List<T_PalletDetailInfo> OutStockPalletSumQty(List<T_PalletDetailInfo> modelList)
        {

            var newModelList = from t in modelList
                               group t by new
                               {
                                   t1 = t.ErpVoucherNo,
                                   t2 = t.MaterialNoID,
                                   //t3 = t.StrongHoldCode,
                                   //t4 = t.StrongHoldName,
                                   //t5 = t.CompanyCode,
                                   t6 = t.EAN,
                                   t7 = t.PalletNo
                               } into m
                               select new T_PalletDetailInfo
                               {
                                   ID = m.FirstOrDefault().ID,
                                   RowNo = m.FirstOrDefault().RowNo,
                                   ErpVoucherNo = m.Key.t1,
                                   MaterialNoID = m.Key.t2,
                                   StrongHoldCode = m.FirstOrDefault().StrongHoldCode,
                                   StrongHoldName = m.FirstOrDefault().StrongHoldName,
                                   CompanyCode = m.FirstOrDefault().CompanyCode,
                                   MaterialNo = m.FirstOrDefault().MaterialNo,
                                   MaterialDesc = m.FirstOrDefault().MaterialDesc,
                                   Qty = m.Sum(p => p.Qty),
                                   VoucherType = m.FirstOrDefault().VoucherType,
                                   PalletType = 3,
                                   EAN =m.Key.t6,
                                   SerialNo = m.Key.t6,
                                   SuppliernNo = m.FirstOrDefault().SuppliernNo,
                                   SupplierName = m.FirstOrDefault().SupplierName,
                                   PalletNo = m.Key.t7,
                                   HouseProp = m.FirstOrDefault().HouseProp
                               };

            return newModelList.ToList();
        }

        
        #endregion

        #region 获取汇总装箱清单数据

        /// <summary>
        /// 获取汇总装箱清单数据
        /// </summary>
        /// <param name="ErpVoucherNo"></param>
        /// <param name="modelList"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool GetOutStockPalletForBox(string ErpVoucherNo, ref List<T_PalletDetailInfo> modelList, ref string strError)
        {
            try
            {
                if (string.IsNullOrEmpty(ErpVoucherNo))
                {
                    strError = "客户端传来单号为空！";
                    return false;
                }

                T_PalletDetail_DB _pdb = new T_PalletDetail_DB();
                string strSql = "select b.* from t_Pallet a left join t_Palletdetail b on a.id = b.Headerid where a.Erpvoucherno = '" + ErpVoucherNo + "' and a.Pallettype = '3'";
                modelList = _pdb.GetModelListBySql(strSql);

                if (modelList == null || modelList.Count == 0)
                {
                    strError = "未获取到装箱数据！装箱单号：" + ErpVoucherNo;
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }



        #endregion

        #region PC端打印物流标签

        /// <summary>
        /// 打印物流标签生成托盘数据
        /// </summary>
        /// <param name="ErpVoucherNo"></param>
        /// <param name="PrintQty"></param>
        /// <param name="PalletNo"></param>
        /// <param name="user"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool CreatePalletByEmsLabel(string ErpVoucherNo, int PrintQty, ref string PalletNo,UserModel user, ref string strError) 
        {
            try
            {
                bool bSucc = false;

                if (string.IsNullOrEmpty(ErpVoucherNo))
                {
                    strError = "客户端传来单号为空！";
                    return false;
                }

                if (PrintQty == 0) 
                {
                    strError = "客户端传入打印数量为零！";
                    return false;
                }

                T_OutStock_Func tfunc = new T_OutStock_Func();
                T_OutStockInfo model = new T_OutStockInfo();
                string strFilter1 = "erpvoucherno = '" + ErpVoucherNo + "'";
                bSucc =  tfunc.GetModelByFilter(ref model, strFilter1,ref strError);
                if (bSucc == false) 
                {
                    return false;
                }

                //根据交易条件判断能否打印物流标签
                //if (model.TradingConditions == "MS2") 
                //{
                //    strError = "该订单不能打印物流标签！";
                //    return false;
                //}

                List<T_OutStockDetailInfo> modelList = new List<T_OutStockDetailInfo>();

                bSucc =  base.GetModelListByHeaderID(ref modelList,model.ID,ref strError);
                if (bSucc == false)
                {
                    return false;
                }                

                T_PalletDetail_DB _pdb = new T_PalletDetail_DB();
                return _pdb.CreatePalletByEmsLabel(modelList, PrintQty, user, ref PalletNo, ref strError);

            }
            catch (Exception ex) 
            {
                strError = ex.Message;
                return false;
            }
        }

         #endregion

        #region PC端打印快递面单
        /// <summary>
        /// 
        /// </summary>
        /// <param name="GUID">过账T100用</param>
        /// <param name="ErpVoucherNo">T100发货单号</param>
        /// <param name="BoxCount">快递件数</param>
        /// <param name="Weight">快递重量</param>
        /// <param name="user">数据提交人</param>
        /// <param name="outStockInfo">返回表头，包含快递信息</param>
        /// <param name="strError">返回信息</param>
        /// <returns></returns>
        public bool CreateOutStockByEmsLabel(string GUID, string ErpVoucherNo, decimal BoxCount, decimal Weight, UserModel user,ref T_OutStockInfo outStockInfo, ref string strError)
        {
            try
            {
                bool bSucc = false;

                if (string.IsNullOrEmpty(ErpVoucherNo))
                {
                    strError = "客户端传来单号为空！";
                    return false;
                }

                if (Weight == 0)
                {
                    strError = "客户端传入快递重量为零！";
                    return false;
                }

                if (Weight < 0)
                {
                    strError = "客户端传入快递重量小于零！";
                    return false;
                }

                T_OutStock_Func tfunc = new T_OutStock_Func();
                T_OutStockInfo model = new T_OutStockInfo();
                string strFilter1 = "erpvoucherno = '" + ErpVoucherNo + "'";
                bSucc = tfunc.GetModelByFilter(ref model, strFilter1, ref strError);
                if (bSucc == false)
                {
                    return false;
                }

                EmsCost_Model emsModel = new EmsCost_Model();
                T_OutStockDetail_DB _db = new T_OutStockDetail_DB();
                bSucc =  _db.GetCostByProvince(model.Province, ref emsModel,ref strError);
                if (bSucc == false) 
                {
                    return false;
                }

                model.PostDate = DateTime.Now;

                model.GoodsValue = GetAmountByWeight(model.Province, emsModel, Weight);
                model.ItemsWeight = Weight;

                List<T_OutStockInfo> modelList = new List<T_OutStockInfo>();
                modelList.Add(model);

                string strError1 = string.Empty;
                string EmsNo = string.Empty;
                bSucc = PostOutStockToEMS(ref modelList,ref EmsNo, ref strError1);
                if (bSucc == false)
                {
                    strError = strError + strError1;
                    return false;
                }
                else 
                {
                    strError = strError + strError1;
                }

                string strError2 = string.Empty;
                bSucc = PostCheckToTrans(GUID, modelList, EmsNo, ref strError2);

                if (bSucc == false)
                {
                    strError = strError + "\r\n" + strError2;
                    return false;
                }
                else
                {
                    strError = strError + "\r\n" + strError2;
                }

                string strError3 = string.Empty;
                modelList.ForEach(t => t.BoxCount = BoxCount);
                modelList.ForEach(t => t.Creater = user.UserName);
                _db.SaveTransportOutStock(modelList, ref strError3);

                if (bSucc == false)
                {
                    strError = strError + "\r\n" + strError3;
                    return false;
                }
                else
                {
                    strError = strError + "\r\n" + "WMS操作成功！"  ;
                }

                outStockInfo = modelList[0];

                return bSucc;

            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        public decimal GetAmountByWeight(string strProvince,EmsCost_Model emsModel, decimal Weight) 
        {
            decimal roundWeight = 0;//重量四舍五入
            decimal conWeight = 0;//扣除首重
            decimal newAmount = 0;//计算完成之后的金额

            if (emsModel == null) 
            {
                return 0;
            }

            if (Weight <= 0.5M) 
            {
                return emsModel.FirstHalf;
            }

            if (Weight <= 1) 
            {
                return emsModel.FirstOne;
            }

            roundWeight = Math.Round(Weight,0);

            conWeight = roundWeight - 1;

            if (conWeight <= 0) //按照1公斤首重计算
            {
                return emsModel.FirstOne;
            }

            if (strProvince.Contains("上海") || strProvince.Contains("北京"))
            {
                if (conWeight <= 3)
                {
                    newAmount = emsModel.FirstOne + (conWeight * 1);
                }
                else
                {
                    newAmount = emsModel.FirstOne + (conWeight * 0.3M);
                }
            }
            else 
            {
                newAmount = emsModel.FirstOne + (conWeight * emsModel.ConWeight);
            }

            return newAmount;

        }

        public bool PostOutStockToEMS(ref List<T_OutStockInfo> modelList,ref string EmsNo, ref string strError)
        {
            try
            {
                BaseMessage_Model<List<T_OutStockInfo>> model = new BaseMessage_Model<List<T_OutStockInfo>>();
                bool bSucc = false;
                string strUserNo = string.Empty;
                string strPostUser = string.Empty;
                string StrongHoldCode = string.Empty;

                modelList.ForEach(t => t.VoucherType = 9991);

                T_Interface_Func tfunc = new T_Interface_Func();
                string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<T_OutStockInfo>>(modelList);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

                model = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_OutStockInfo>>>(interfaceJson);

                LogNet.LogInfo("ERPJsonAfter:" + BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutStockInfo>>>(model));

                //过账失败直接返回
                if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
                {
                    strError = "调用快递接口失败！" + model.Message;
                    bSucc = false;
                }
                else if (model.HeaderStatus == "S" && !string.IsNullOrEmpty(model.MaterialDoc)) //过账成功，并且生成了凭证要记录数据库
                {
                    strError = "调用快递接口成功！快递单号：" + model.MaterialDoc.Split('@')[0];
                    EmsNo = model.MaterialDoc.Split('@')[0];
                    modelList.ForEach(t => t.MaterialDoc = model.MaterialDoc.Split('@')[0]);//快递单号
                    modelList.ForEach(t => t.shortAddress = model.MaterialDoc.Split('@')[1]);//三段网点
                    modelList.ForEach(t => t.consigneeBranchCode = model.MaterialDoc.Split('@')[2]);//末端网点
                    modelList.ForEach(t => t.qrCode = model.MaterialDoc.Split('@')[3]);//末端网点
                    modelList.ForEach(t => t.sname = model.MaterialDoc.Split('@')[4]);//寄件人
                    modelList.ForEach(t => t.sphone = model.MaterialDoc.Split('@')[5]);//寄件人电话
                    modelList.ForEach(t => t.spostCode = model.MaterialDoc.Split('@')[6]);//寄件邮编
                    modelList.ForEach(t => t.sprov = model.MaterialDoc.Split('@')[7]);//寄件省份
                    modelList.ForEach(t => t.scity = model.MaterialDoc.Split('@')[8]);//寄件城市
                    modelList.ForEach(t => t.saddress = model.MaterialDoc.Split('@')[9]);//寄件地址
                    bSucc = true;
                }

                return bSucc;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }


        }

        public bool PostCheckToTrans(string GUID,List<T_OutStockInfo> modelList,string EmsNo, ref string strError)
        {
            try
            {
                BaseMessage_Model<List<T_OutStockInfo>> model = new BaseMessage_Model<List<T_OutStockInfo>>();
                bool bSucc = false;
                string strUserNo = string.Empty;
                string strPostUser = string.Empty;
                string StrongHoldCode = string.Empty;

                modelList.ForEach(t => t.Feight = t.GoodsValue);
                modelList.ForEach(t => t.VoucherNo = EmsNo);
                modelList.ForEach(t => t.GUID = GUID);

                if (modelList[0].ErpVoucherNo.Contains("HH2"))
                {
                    modelList.ForEach(t => t.gtype = "A");
                }
                else
                {
                    modelList.ForEach(t => t.gtype = "C");
                }
                StrongHoldCode = modelList[0].ErpVoucherNo.Substring(0, 3);
                modelList.ForEach(t => t.VoucherType = 9992);
                modelList.ForEach(t => t.StrongHoldCode = StrongHoldCode);

                T_Interface_Func tfunc = new T_Interface_Func();
                string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<T_OutStockInfo>>(modelList);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

                model = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_OutStockInfo>>>(interfaceJson);

                LogNet.LogInfo("ERPJsonAfter:" + BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutStockInfo>>>(model));

                //过账失败直接返回
                if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
                {
                    strError = "回传T100快递费用失败！" + model.Message;
                    bSucc = false;
                }
                else if (model.HeaderStatus == "S" && !string.IsNullOrEmpty(model.MaterialDoc)) //过账成功，并且生成了凭证要记录数据库
                {
                    strError = "回传T100快递费用成功！凭证号：" + model.MaterialDoc;
                    bSucc = true;
                }

                return bSucc;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }


        }



        #endregion

        #region 盘点生成差异仓的调拨单

        //public bool SaveT_ChangeMaterial(UserModel userModel,List<CheckDet_Model> modelList,  ref string strError)
        //{
        //    try
        //    {
        //        if (modelList == null || modelList.Count == 0) 
        //        {
        //            strError = "客户端传来过账数据为空！";
        //            return false;
        //        }

        //        //if (modelList.Where(t => t.QTY == 0).Count() == 0) 
        //        //{
        //        //    strError = "客户端提交过账数量都为零！";
        //        //    return false;
        //        //}

        //        return PostCheckToTrans(userModel, modelList, ref strError);

        //    }
        //    catch (Exception ex) 
        //    {
        //        strError = ex.Message;
        //        return false;
        //    }

        //}

        //public bool PostCheckToTrans(UserModel userModel, List<CheckDet_Model> modelList, ref string strError)
        //{
        //    try
        //    {
        //        BaseMessage_Model<List<CheckDet_Model>> model = new BaseMessage_Model<List<CheckDet_Model>>();
        //        bool bSucc = false;
        //        string strUserNo = string.Empty;
        //        string strPostUser = string.Empty;

        //        modelList.ForEach(t => t.VoucherType = 9996);

        //        T_Interface_Func tfunc = new T_Interface_Func();
        //        string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<CheckDet_Model>>(modelList);
        //        string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

        //        model = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<CheckDet_Model>>>(interfaceJson);

        //        LogNet.LogInfo("ERPJsonAfter:" + BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<CheckDet_Model>>>(model));

        //        //过账失败直接返回
        //        if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
        //        {
        //            strError = "盘点生成调拨单过账失败！" + model.Message;
        //            bSucc = false;
        //        }
        //        else if (model.HeaderStatus == "S" && !string.IsNullOrEmpty(model.MaterialDoc)) //过账成功，并且生成了凭证要记录数据库
        //        {
        //            strError = "盘点生成调拨单过账成功！凭证号：" + model.MaterialDoc;
        //            bSucc = true;
        //        }

        //        return bSucc;
        //    }
        //    catch (Exception ex)
        //    {
        //        strError = ex.Message;
        //        return false;
        //    }


        //}


        #endregion





     }
 
}
