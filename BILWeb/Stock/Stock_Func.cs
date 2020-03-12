using BILBasic.Basing.Factory;
using BILBasic.JSONUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILWeb.Area;
using BILBasic.Common;
using BILWeb.InStock;
using BILWeb.Material;
using BILWeb.OutBarCode;
using BILWeb.Pallet;
using BILWeb.Quality;
using BILWeb.InStockTask;
using BILBasic.User;
using BILWeb.Login.User;
using BILBasic.Interface;
using Newtonsoft.Json;
using BILBasic.Language;
using BILWeb.RuleAll;
using BILWeb.StrategeRuleAll;
using BILWeb.Print;
using BILWeb.Query;


namespace BILWeb.Stock
{
    public partial class T_Stock_Func : TBase_Func<T_Stock_DB, T_StockInfo>
    {
        T_Stock_DB tdb = new T_Stock_DB();
        T_OutBarCode_Func outBarCodeFunc = new T_OutBarCode_Func();
        T_RuleAll_Func rfunc = new T_RuleAll_Func();

        protected override bool CheckModelBeforeSave(T_StockInfo model, ref string strError)
        {
            return true;
        }

        #region 下架扫描条码新

        /// <summary>
        /// PDA扫描条码拣货
        /// </summary>
        /// <param name="ModelStockJson"></param>
        /// <returns></returns>
        public string GetStockModelBySql(string ModelStockJson)//string BarCode, string ScanType, string MoveType, string IsEdate
        {
            BaseMessage_Model<List<T_StockInfo>> messageModel = new BaseMessage_Model<List<T_StockInfo>>();
            try
            {
                string strError = string.Empty;

                LogNet.LogInfo("GetStockModelADF:" + ModelStockJson);


                if (string.IsNullOrEmpty(ModelStockJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = Language_CHS.DataIsEmpty;
                    return JsonConvert.SerializeObject(messageModel);
                }

                T_StockInfo model = JsonConvert.DeserializeObject<T_StockInfo>(ModelStockJson);

                if (string.IsNullOrEmpty(model.Barcode))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端提交条码数据为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                //if (model.HouseProp == 0)
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = "客户端提交拣货单属性为空（整箱区、零拣区属性）！";
                //    return JsonConvert.SerializeObject(messageModel);
                //}

                //if (model.Barcode.Contains("@") && model.HouseProp == 2)
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = "零拣区的拣货单不能扫描批次标签，请扫描69码！";
                //    return JsonConvert.SerializeObject(messageModel);
                //}

                //if (!model.Barcode.Contains("@") && model.HouseProp == 1)
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = "整箱区的拣货单不能扫描69码，请扫描批次标签！";
                //    return JsonConvert.SerializeObject(messageModel);
                //}

                //根据序列号和非序列号查询库存
                //序列号根据序列号直接查找出一条记录
                //非序列号根据库位+条码查找出一条记录
                List<T_StockInfo> modelList = new List<T_StockInfo>();
                Context<T_StockInfo> context = new Context<T_StockInfo>(RuleAll_Config.SerialItem);
                if (context.GetStockByBarCode(model, ref modelList, ref strError) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JsonConvert.SerializeObject(messageModel);
                }

                //modelList = modelList.Where(t => t.TaskDetailesID == 0).ToList();

                modelList = modelList.Where(t => t.TaskDetailesID == 0).ToList();

                if (modelList == null || modelList.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "该托盘已经全部下架！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                if (CheckOutStockStatus(ref strError, modelList[0], "", "") == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JsonConvert.SerializeObject(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = modelList;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);//JsonConvert.SerializeObject(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message; ;
                return JsonConvert.SerializeObject(messageModel);
            }
        }

        /// <summary>
        /// PC扫描条码拣货
        /// </summary>
        /// <param name="ModelStockJson"></param>
        /// <returns></returns>
        public bool GetStockModelBySql(T_StockInfo model, ref List<T_StockInfo> modelList, ref string strError)//string BarCode, string ScanType, string MoveType, string IsEdate
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.Barcode))
                {
                    strError = Language_CHS.DataIsEmpty;
                    return false;
                }
               
                //根据序列号和非序列号查询库存
                //序列号根据序列号直接查找出一条记录
                //非序列号根据库位+条码查找出一条记录
                //List<T_StockInfo> modelList = new List<T_StockInfo>();
                Context<T_StockInfo> context = new Context<T_StockInfo>(RuleAll_Config.SerialItem);
                if (context.GetStockByBarCode(model, ref modelList, ref strError) == false)
                {
                    return false;
                    //messageModel.HeaderStatus = "E";
                    //messageModel.Message = strError;
                    //return JsonConvert.SerializeObject(messageModel);
                }

                if (CheckOutStockStatus(ref strError, modelList[0], "", "") == false)
                {
                    //messageModel.HeaderStatus = "E";
                    //messageModel.Message = strError;
                    //return JsonConvert.SerializeObject(messageModel);
                    return false;
                }

                return true;
                //messageModel.HeaderStatus = "S";
                //messageModel.ModelJson = modelList;
                //return JsonConvert.SerializeObject(messageModel);
            }
            catch (Exception ex) 
            {
                strError = ex.Message;
                return false;
            }

        }

        #endregion

        /// <summary>
        /// 移库操作
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        protected override bool CheckModelBeforeSave(List<T_StockInfo> modelList, ref string strError)
        {
            T_Area_Func tfunc = new T_Area_Func();
            T_AreaInfo model = new T_AreaInfo();
            if (modelList == null || modelList.Count == 0)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }


            if (modelList[0].AreaID <= 0)
            {
                strError = "移入库位为空！";
                return false;
            }


            if (!tfunc.GetAreaModelBySql(modelList[0].WareHouseID, modelList[0].AreaNo, ref model, ref strError))
            {
                strError = "移入库位" + strError;
                return false;
            }


            return true;
        }


        protected override string GetModelChineseName()
        {
            return "库存";
        }

        protected override T_StockInfo GetModelByJson(string strJson)
        {
            return JSONHelper.JsonToObject<T_StockInfo>(strJson);
        }


        #region 下架扫描条码



        /// <summary>
        /// 移库，下架用的序列号扫描
        /// 1-整托扫描 2-整箱扫描 3-零数
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <returns></returns>
        public string GetStockModelBySqlADF(string BarCode, string ScanType, string MoveType, string IsEdate)
        {
            BaseMessage_Model<List<T_StockInfo>> messageModel = new BaseMessage_Model<List<T_StockInfo>>();
            try
            {
                string strError = string.Empty;
                string SerialNo = string.Empty;
                string BarCodeType = string.Empty;
                List<T_StockInfo> modelList = new List<T_StockInfo>();

                T_OutBarCodeInfo BarCodeInfo = new T_OutBarCodeInfo();
                T_OutBarCode_Func tfunc = new T_OutBarCode_Func();

                //验证条码正确性
                if (tfunc.GetSerialNoByBarCode(BarCode, ref SerialNo, ref BarCodeType, ref strError) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                if (GetOutStockListBySerialNo(SerialNo, BarCodeType, ScanType, ref strError, ref modelList, MoveType, IsEdate) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = modelList;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
            }

        }


        private bool GetOutStockListBySerialNo(string SeriaNo, string BarCodeType, string ScanType, ref string strError, ref List<T_StockInfo> modelList, string MoveType, string IsEdate)
        {
            //外箱条码
            if (BarCodeType == "1")
            {
                if (GetOutBarCodeByOutStockTask(SeriaNo, ScanType, ref strError, ref modelList, MoveType, IsEdate) == false)
                {
                    return false;
                }
            }
            else if (BarCodeType == "2")
            {
                if (GetPalletByOutStockTask(SeriaNo, ScanType, ref strError, ref modelList, MoveType, IsEdate) == false)
                {
                    return false;
                }
            }
            else if (BarCodeType == "3")//序列号
            {
                //扫描的序列号不是外箱条码，需要查找是否是托盘条码
                if (GetOutBarCodeByOutStockTask(SeriaNo, ScanType, ref strError, ref modelList, MoveType, IsEdate) == false && GetPalletByOutStockTask(SeriaNo, ScanType, ref strError, ref modelList, MoveType, IsEdate) == false)
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 扫描的是外箱
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <param name="ScanType"></param>
        /// <param name="strError"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        private bool GetOutBarCodeByOutStockTask(string SerialNo, string ScanType, ref string strError, ref List<T_StockInfo> modelList, string MoveType, string IsEdate)
        {
            T_StockInfo stockModel = new T_StockInfo();
            T_OutBarCode_Func toc = new T_OutBarCode_Func();
            T_OutBarCodeInfo outBarCodeModel = new T_OutBarCodeInfo();
            //查询外箱条码是否已经打印
            if (toc.GetOutBarCodeInfoBySerialNo(SerialNo, ref outBarCodeModel, ref strError) == false)
            {
                return false;
            }


            //查询外箱条码库存是否存在
            if (GetStockByBarCode(SerialNo, ref stockModel, ref strError) == false)
            {
                return false;
            }

            if (CheckOutStockStatus(ref strError, stockModel, MoveType, IsEdate) == false)
            {
                return false;
            }

            //整箱或者零数发货
            if (ScanType == "2" || ScanType == "3")
            {
                modelList.Add(stockModel);
            }

            //整托发货
            if (ScanType == "1")
            {
                if (string.IsNullOrEmpty(stockModel.PalletNo))
                {
                    strError = "该条码没有组托，不能整托拣货！";
                    return false;
                }

                if (GetStockInfoByPalletNo(stockModel.PalletNo, ref modelList, ref strError) == false)
                {
                    return false;
                }
            }

            decimal SumQty = modelList.Sum(t1 => t1.Qty).ToDecimal();
            modelList.ForEach(t => t.PalletQty = SumQty);

            return true;
        }

        /// <summary>
        /// 验证条码发货状态
        /// 1-下架 2-移库
        /// </summary>
        /// <param name="strError"></param>
        /// <param name="stockModel"></param>
        /// <returns></returns>
        public bool CheckOutStockStatus(ref string strError, T_StockInfo stockModel, string MoveType, string IsEdate)
        {
            string SysNowDate = System.DateTime.Now.ToString("yyyy/MM/dd");

            if (stockModel.TaskDetailesID > 0)
            {
                strError = "该条码处于待发货状态！";
                return false;
            }

            if (stockModel.CheckID > 0)
            {
                strError = "该条码处于盘点状态！";
                return false;
            }

            if (stockModel.TransferDetailsID > 0)
            {
                strError = "该条码处于调拨状态！";
                return false;
            }
            //2018-2-7修改移库去掉限制
            //if (MoveType == "2")
            //{
            //    if (stockModel.AreaType != 4)
            //    {
            //        if (stockModel.Status == 1)
            //        {
            //            strError = "该条码处于待检状态！";
            //            return false;
            //        }
            //    }
            //}

            //if (MoveType == "1") 
            //{
            //    if (stockModel.Status == 1)
            //    {
            //        strError = "该条码处于待检状态！";
            //        return false;
            //    }

            //    if (stockModel.Status == 2)
            //    {
            //        strError = "该条码处于送检状态！";
            //        return false;
            //    }
            //}

            //杂出单，仓退单不需要管控效期
            //其他单子需要管控效期
            //2-其他单子
            //if (IsEdate == "2") 
            //{
            //    if (stockModel.EDate.ToString("yyyy/MM/dd").CompareTo(SysNowDate) < 0)
            //    {
            //        strError = "该条码超过有效期，不能下架！";
            //        return false;
            //    }
            //}

            if (stockModel.EDate.ToString("yyyy/MM/dd").CompareTo(SysNowDate) < 0)
            {
                strError = "该条码超过有效期，不能下架！";
                return false;
            }

            if (stockModel.IsRetention == "1")
            {
                strError = "该条码已经被库存留置，不能下架！";
                return false;
            }
            //if (stockModel.Status == 4)
            //{
            //    strError = "该条码检验不合格！";
            //    return false;
            //}

            return true;
        }


        private bool GetPalletByOutStockTask(string BarCode, string ScanType, ref string strError, ref List<T_StockInfo> modelList, string MoveType, string IsEdate)
        {
            string Filter = string.Empty;
            T_PalletDetail_Func tpdf = new T_PalletDetail_Func();
            List<T_PalletDetailInfo> pList = new List<T_PalletDetailInfo>();

            if (ScanType == "2" || ScanType == "3")
            {
                strError = "您扫描的是托盘条码，不能按整箱或者零数出库！";
                return false;
            }

            //托盘条码，获取整托明细
            if (GetStockInfoByPalletNo(BarCode, ref modelList, ref strError) == false)
            {
                strError += "请检查扫描或输入的条码是否正确！";
                return false;
            }

            if (CheckOutStockStatus(ref strError, modelList[0], MoveType, IsEdate) == false)
            {
                return false;
            }


            decimal SumQty = modelList.Sum(t1 => t1.Qty).ToDecimal();
            modelList.ForEach(t => t.PalletQty = SumQty);
            return true;
        }

        #endregion

        /// <summary>
        /// 上架扫描，不带货位
        /// 没有解析条码类型
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <param name="VoucherNo"></param>
        /// <param name="TaskNo"></param>
        /// <param name="AreaNo"></param>
        /// <returns></returns>
        public string GetInStockModelADF(string SerialNo, string VoucherNo, string TaskNo)
        {
            BaseMessage_Model<List<T_StockInfo>> messageModel = new BaseMessage_Model<List<T_StockInfo>>();
            try
            {
                string DeSerialNo = string.Empty;
                string strError = string.Empty;
                string BarCodeType = string.Empty;

                T_StockInfo stockModel = new T_StockInfo();
                List<T_StockInfo> modelList = new List<T_StockInfo>();

                if (string.IsNullOrEmpty(SerialNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "传入条码参数为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                if (string.IsNullOrEmpty(VoucherNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "传入订单号参数为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                bool bSucc = new T_OutBarCode_Func().GetSerialNoByBarCode(SerialNo, ref DeSerialNo, ref BarCodeType, ref strError);

                if (bSucc == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                T_Stock_DB tdb = new T_Stock_DB();

                int IsLimitStock = tdb.GetLimitStockBySerialNo(DeSerialNo);

                if (IsLimitStock == 2)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "该条码已经上架！" + SerialNo;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                modelList = tdb.GetStockModelList(DeSerialNo);

                if (modelList == null || modelList.Count == 0)
                {
                    //没有拼托，查找单个库存对象
                    stockModel.SerialNo = DeSerialNo;
                    if (base.GetModelBySql(ref stockModel, ref strError) == false)
                    {
                        messageModel.HeaderStatus = "E";
                        messageModel.Message = strError;
                        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                    }
                    else
                    {
                        modelList = new List<T_StockInfo>();
                        modelList.Add(stockModel);
                    }
                }

                T_SerialNo_DB serialdb = new T_SerialNo_DB();
                T_SerialNoInfo serialModel = new T_SerialNoInfo();
                serialModel = serialdb.GetSerialModel(DeSerialNo);

                if (serialModel == null)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "扫描的条码在流水中未找到！请确认是否收货！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                if (VoucherNo.CompareTo(serialModel.VoucherNo) != 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "扫描的条码对应订单号不符！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                if (TaskNo.CompareTo(serialModel.TaskNo) != 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "扫描的条码对应任务号不符！条码对应的任务号为：" + serialModel.TaskNo;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                messageModel.HeaderStatus = "S";

                modelList.ForEach(t => t.PalletQty = modelList.Sum(t1 => t1.Qty).ToDecimal());

                messageModel.ModelJson = modelList;

                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
            }

        }





        #region 上架扫描方法


        /// <summary>
        /// 上架扫描，带货位号
        /// 根据条码类型获取不同数据
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <param name="VoucherNo"></param>
        /// <param name="TaskNo"></param>
        /// <param name="AreaNo"></param>
        /// <returns></returns>
        public string GetScanInStockModelADF(string BarCode, string ERPVoucherNo, string TaskNo, string AreaNo, int WareHouseID)
        {
            BaseMessage_Model<List<T_StockInfo>> messageModel = new BaseMessage_Model<List<T_StockInfo>>();
            try
            {
                string strError = string.Empty;
                T_AreaInfo areaModel = new T_AreaInfo();
                List<T_StockInfo> modelList = new List<T_StockInfo>();
                string BarCodeType = string.Empty;
                string SerialNo = string.Empty;

                if (string.IsNullOrEmpty(BarCode))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "传入条码参数为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                //if (string.IsNullOrEmpty(ERPVoucherNo))
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = "传入订单号参数为空！";
                //    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                //}

                //货位编码不为空，需要验证货位是否存在
                if (!string.IsNullOrEmpty(AreaNo))
                {
                    if (CheckAreaNoIsExist(WareHouseID, AreaNo, ref areaModel, ref strError) == false)
                    {
                        messageModel.HeaderStatus = "E";
                        messageModel.Message = strError;
                        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                    }
                }

                //验证条码正确性
                if (outBarCodeFunc.GetSerialNoByBarCode(BarCode, ref SerialNo, ref BarCodeType, ref strError) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }


                if (GetStockListBySerialNo(SerialNo, BarCodeType, AreaNo, ERPVoucherNo, ref strError, ref modelList, TaskNo, WareHouseID) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = modelList;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
            }
        }


        //根据扫描的内容获取库存数据
        //可能是外箱条码，托盘条码，或者外箱序列号和托盘序列号
        private bool GetStockListBySerialNo(string SeriaNo, string BarCodeType, string AreaNo, string ERPVoucherNo, ref string strError, ref List<T_StockInfo> modelList, string strTaskNo, int WareHouseID)
        {

            //外箱条码
            if (BarCodeType == "1")
            {
                if (GetOutBarCodeByInStockTask(SeriaNo, AreaNo, ERPVoucherNo, ref strError, ref modelList, strTaskNo, WareHouseID) == false)
                {
                    return false;
                }
            }
            else if (BarCodeType == "2")
            {
                if (GetPalletByInStockTask(SeriaNo, ERPVoucherNo, AreaNo, ref strError, ref modelList, strTaskNo, WareHouseID) == false)
                {
                    return false;
                }
            }
            else if (BarCodeType == "3")//序列号
            {
                //扫描的序列号不是外箱条码，需要查找是否是托盘条码
                if (GetOutBarCodeByInStockTask(SeriaNo, AreaNo, ERPVoucherNo, ref strError, ref modelList, strTaskNo, WareHouseID) == false && GetPalletByInStockTask(SeriaNo, ERPVoucherNo, AreaNo, ref strError, ref modelList, strTaskNo, WareHouseID) == false)
                {
                    return false;
                }
            }
            string TaskNo = string.Empty;
            T_InStockTask_Func _tfunc = new T_InStockTask_Func();
            if (_tfunc.GetReceiveTaskNoBySerialNo(modelList[0].SerialNo, ref TaskNo, ref strError) == false)
            {
                return false;
            }
            else
            {
                modelList.ForEach(t => t.TaskNo = TaskNo);
            }


            return true;
        }

        /// <summary>
        /// 上架扫描条码，扫描的是外箱条码的情况
        /// </summary>
        /// <param name="BarCode"></param>
        /// <param name="messageModel"></param>
        /// <param name="strError"></param>
        /// <param name="stockModel"></param>
        /// <param name="modelList"></param>
        /// <param name="pList"></param>
        private bool GetOutBarCodeByInStockTask(string SerialNo, string AreaNo, string ERPVoucherNo, ref string strError, ref List<T_StockInfo> modelList, string strTaskNo, int WareHouseID)
        {
            T_StockInfo stockModel = new T_StockInfo();
            T_OutBarCode_Func toc = new T_OutBarCode_Func();
            T_OutBarCodeInfo outBarCodeModel = new T_OutBarCodeInfo();
            List<T_OutBarCodeInfo> lstBarCode = new List<T_OutBarCodeInfo>();
            List<T_StockInfo> HModelList = new List<T_StockInfo>();
            
            //查询外箱条码是否已经打印
            if (toc.GetOutBarCodeInfoBySerialNo(SerialNo, ref outBarCodeModel, ref strError) == false)
            {
                return false;
            }

            //if (!string.IsNullOrEmpty(ERPVoucherNo))
            //{
            //    if (ERPVoucherNo.CompareTo(outBarCodeModel.ErpVoucherNo) != 0)
            //    {
            //        strError = "扫描的条码对应订单号不符！应上架订单号为：" + outBarCodeModel.ErpVoucherNo;
            //        return false;
            //    }
            //}


            //查询外箱条码是否已经收货
            if (GetStockByBarCode(SerialNo, ref stockModel, ref strError) == false)
            {
                return false;
            }

            if (stockModel.ReceiveStatus == 2)
            {
                strError = "该条码已经上架！" + SerialNo;
                return false;
            }

            stockModel.IsPalletOrBox = 1;

            //上架，扫描到整箱需要保存的时候拆托库存，加个标记区分是不是扫描到的整箱
            if (outBarCodeModel.BarcodeType == 5)
            {
                stockModel.lstHBarCode = GetNewListStock(outBarCodeModel.BarCode, stockModel);                
            }
            //整箱扫描
                        
            modelList.Add(stockModel);

            //T_Quality_DB qdb = new T_Quality_DB();
            //decimal sampQty =  qdb.GetQualitySampQtyByTaskNo(strTaskNo, stockModel.MaterialNoID, stockModel.BatchNo);

            //if (sampQty > 0 && stockModel.Status == 1) 
            //{
            //    strError = "该条码处在待检状态，请确认是否已经取样！" + SerialNo;
            //    return false;
            //}



            //库位不为空，需要判断该库位中的物料以及批次和扫描条码的物料以及批次的质检状态是否一致
            //if (!string.IsNullOrEmpty(AreaNo)) 
            //{
            //    if (ChechkMaterialIsCanShelve(WareHouseID,strTaskNo, stockModel.MaterialNoID, stockModel.BatchNo, AreaNo, ref strError) == false) 
            //    {
            //        return false;
            //    }
            //}

            //if (!string.IsNullOrEmpty(stockModel.PalletNo))
            //{
            //    //GetOutBarCodeByPalletNo
            //    //存在托盘，需要获取库存整托数据
            //    if (GetStockInfoByPalletNo(stockModel.PalletNo, ref modelList, ref strError) == false)
            //    {
            //        return false;
            //    }
            //    else 
            //    {
            //        HModelList = modelList.Where(t => t.BarCodeType == 5).ToList();

            //        if (HModelList != null && HModelList.Count > 0)
            //        {
            //            foreach (var item in HModelList)
            //            {
            //                item.lstHBarCode = GetNewListStock(item.Barcode);
            //                //modelList.Find(t => t.ID == item.ID).lstBarCode = odb.GetBarCodeOutAll(model.BarCode);
            //            }
            //        }
            //    }                
            //}
            //else
            //{
            //    if (outBarCodeModel.BarcodeType == 5) 
            //    {
            //        stockModel.lstHBarCode = GetNewListStock(outBarCodeModel.BarCode);
            //    }
            //    modelList.Add(stockModel);
            //}

            decimal SumQty = modelList.Sum(t1 => t1.Qty).ToDecimal();
            modelList.ForEach(t => t.PalletQty = SumQty);

            return true;
        }

        public List<T_StockInfo> GetNewListStock(string strBarCode, T_StockInfo stockModel) //, T_StockInfo stockModel
        {
            List<T_StockInfo> lstStock = new List<T_StockInfo>();
            T_OutBarcode_DB odb = new T_OutBarcode_DB();
            List<T_OutBarCodeInfo> lstHbarCode = odb.GetBarCodeOutAll(strBarCode);

            foreach (var item in lstHbarCode) 
            {
                T_StockInfo model = new T_StockInfo();
                
                model.Barcode = item.BarCode;
                model.SerialNo = item.SerialNo;
                model.BarCodeType = item.BarcodeType;
                model.MaterialNo = item.MaterialNo;
                model.MaterialDesc = item.MaterialDesc;
                model.Qty = item.Qty.ToDecimal();
                model.CompanyCode = item.CompanyCode;
                model.StrongHoldCode = item.StrongHoldCode;
                model.StrongHoldName = item.StrongHoldName;
                model.fserialno = item.fserialno;
                model.IsPalletOrBox = stockModel.IsPalletOrBox;
                model.PalletNo = stockModel.PalletNo;
                //model.AreaID = stockModel.AreaID;
                lstStock.Add(model);
            }
            return lstStock;
        }

        /// <summary>
        /// 库位部位空，判断上架扫描的条码能否上到该库位
        /// </summary>
        /// <param name="strTaskNo"></param>
        /// <param name="MaterialNoID"></param>
        /// <param name="BatchNo"></param>
        /// <param name="AreaNo"></param>
        /// <returns></returns>
        private bool ChechkMaterialIsCanShelve(int WareHouseID, string strTaskNo, int MaterialNoID, string BatchNo, string AreaNo, ref string strError)
        {

            int IsQuality = 0;
            int StockStatus = 0;

            int VoucherType = 0;

            T_InStock_Func tfunc = new T_InStock_Func();
            VoucherType = tfunc.GetInStockVoucherType(strTaskNo);

            IsQuality = GetIsQuality(VoucherType, strTaskNo, MaterialNoID, BatchNo, WareHouseID);

            StockStatus = tdb.GetMaterialStatusByAreaID(WareHouseID, AreaNo, MaterialNoID, BatchNo);

            if (StockStatus == 2)
            {
                StockStatus = 1;
            }

            if (IsQuality == 2)
            {
                IsQuality = 1;
            }

            if (StockStatus > 0)
            {
                if (IsQuality != StockStatus)
                {
                    strError = "上架条码当前库位存在不同质检状态，不能上架！";
                    return false;
                }
            }

            return true;
        }


        private int GetIsQuality(int VoucherType, string strTaskNo, int MaterialNoID, string BatchNo, int WareHouseID)
        {
            int iIsQuality = 0;

            switch (VoucherType)
            {
                case 22:
                    iIsQuality = GetPurQualityStatus(strTaskNo, MaterialNoID, BatchNo);
                    break;
                case 26:
                case 27:
                case 28:
                case 30:
                    iIsQuality = GetOtherQualityStatus(strTaskNo);
                    break;
                case 32:
                case 9996:
                case 33:
                    iIsQuality = GetStockStatusByQualityAreaNo(MaterialNoID, BatchNo, WareHouseID);
                    break;
                default:
                    break;
            }
            return iIsQuality;
        }

        private int GetStockStatusByQualityAreaNo(int MaterialNoID, string BatchNo, int WareHouseID)
        {
            return tdb.GetStockStatusByQualityAreaNo(MaterialNoID, BatchNo, WareHouseID);
        }

        /// <summary>
        /// 获取采购订单质检状态
        /// </summary>
        /// <param name="strTaskNo"></param>
        /// <param name="MaterialNoID"></param>
        /// <param name="BatchNo"></param>
        /// <returns></returns>
        private int GetPurQualityStatus(string strTaskNo, int MaterialNoID, string BatchNo)
        {
            T_Quality_Func tfunc = new Quality.T_Quality_Func();
            return tfunc.GetQualityStatusByTaskNo(strTaskNo, MaterialNoID, BatchNo);
        }

        /// <summary>
        /// 获取其他订单的检验状态
        /// </summary>
        /// <param name="strTaskNo"></param>
        /// <returns></returns>
        private int GetOtherQualityStatus(string strTaskNo)
        {
            int iIsQuality = 0;
            string ERPQualityStatus = string.Empty;
            T_InStock_Func tfunc = new T_InStock_Func();
            ERPQualityStatus = tfunc.GetInStockStatusByTaskNo(strTaskNo);

            switch (ERPQualityStatus)
            {
                case "0":
                    iIsQuality = 1;
                    break;
                case "1":
                    iIsQuality = 3;
                    break;
                case "2":
                    iIsQuality = 4;
                    break;
                default:
                    break;
            }

            return iIsQuality;

        }


        /// <summary>
        /// 上架扫描托盘条码，获取库存托盘信息
        /// </summary>
        /// <param name="BarCode"></param>
        /// <param name="strError"></param>
        /// <param name="pList"></param>
        /// <returns></returns>
        private bool GetPalletByInStockTask(string BarCode, string ERPVoucherNo, string AreaNo, ref string strError, ref List<T_StockInfo> modelList, string strTaskNo, int WareHouseID)
        {
            string Filter = string.Empty;
            T_PalletDetail_Func tpdf = new T_PalletDetail_Func();
            List<T_PalletDetailInfo> pList = new List<T_PalletDetailInfo>();
            List<T_StockInfo> HModelList = new List<T_StockInfo>();

            //获取托盘表数据，托盘对应的ERP订单号
            if (tpdf.GetPalletByPalletNo("palletno = '" + BarCode + "' ", ref pList, ref strError) == false)
            {
                //strError = "扫描的托盘条码对应订单号不符！应上架订单号为：" + ERPVoucherNo;
                strError = "扫描的托盘条码不存在！" + ERPVoucherNo;
                return false;
            }

            //if (!string.IsNullOrEmpty(ERPVoucherNo))
            //{
            //    if (pList[0].ErpVoucherNo.CompareTo(ERPVoucherNo) != 0)
            //    {
            //        strError = "扫描的托盘条码对应订单号不符！上架订单号为：" + pList[0].ErpVoucherNo;
            //        return false;
            //    }
            //}

            //托盘条码，获取整托明细
            if (GetStockInfoByPalletNo(BarCode, ref modelList, ref strError) == false)
            {
                return false;
            }

            if (modelList[0].ReceiveStatus == 2)
            {
                strError = "该托盘已经上架！" + BarCode;
                return false;
            }

            //T_Quality_DB qdb = new T_Quality_DB();
            //decimal sampQty = qdb.GetQualitySampQtyByTaskNo(strTaskNo, modelList[0].MaterialNoID, modelList[0].BatchNo);

            //if (sampQty > 0 && modelList[0].Status == 1)
            //{
            //    strError = "该条码处在待检状态，请确认是否已经取样！" + BarCode;
            //    return false;
            //}

            //库位不为空，需要判断该库位中的物料以及批次和扫描条码的物料以及批次的质检状态是否一致
            //if (!string.IsNullOrEmpty(AreaNo))
            //{
            //    if (ChechkMaterialIsCanShelve(WareHouseID, strTaskNo, modelList[0].MaterialNoID, modelList[0].BatchNo, AreaNo, ref strError) == false)
            //    {
            //        return false;
            //    }
            //}

            //整托扫描
            modelList.ForEach(t => t.IsPalletOrBox = 2);

            HModelList = modelList.Where(t => t.BarCodeType == 5).ToList();

            if (HModelList != null && HModelList.Count > 0)
            {
                foreach (var item in HModelList)
                {
                    item.lstHBarCode = GetNewListStock(item.Barcode,item);
                    //modelList.Find(t => t.ID == item.ID).lstBarCode = odb.GetBarCodeOutAll(model.BarCode);
                }
            }
            
            decimal SumQty = modelList.Sum(t1 => t1.Qty).ToDecimal();
            modelList.ForEach(t => t.PalletQty = SumQty);
            return true;
        }


        #endregion


        private bool CheckAreaNoIsExist(int WareHouseID, string AreaNo, ref T_AreaInfo model, ref string strError)
        {
            T_Area_Func tfunc = new Area.T_Area_Func();

            return tfunc.GetAreaModelBySql(WareHouseID, AreaNo, ref model, ref strError);
        }

        /// <summary>
        /// 根据scantype获取不同的库存数据
        /// </summary>
        /// <param name="MaterialNo"></param>
        /// <param name="ScanType"></param>
        /// <returns></returns>
        public string GetStockInfoByScanType(string UserJson,string MaterialNo, string ScanType)
        {
            BaseMessage_Model<List<T_StockInfo>> messageModel = new BaseMessage_Model<List<T_StockInfo>>();
            string ReturnJson = string.Empty;

            try
            {
                switch (ScanType)
                {
                    case "1"://物料查询
                        ReturnJson = GetStockByMaterialNoADF(MaterialNo);
                        break;
                    case "2"://库位查询
                        ReturnJson = GetStockByAreaNoADF( MaterialNo);
                        break;
                    case "3"://批次查询
                        ReturnJson = GetStockByBatchNoADF( MaterialNo);
                        break;
                    case "4"://供应商查询
                        ReturnJson = GetStockBySupplierNoADF(MaterialNo);
                        break;
                    case "5"://EAN查询
                        ReturnJson = GetStockByEANADF(MaterialNo);
                        break;
                }

                return ReturnJson;

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
            }
        }
        /// <summary>
        /// 移库保存
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="StockJson"></param>
        /// <param name="AreaJson"></param>
        /// <returns></returns>
        public string saveMoveBarcode(string UserJson, string StockJson, string AreaJson)
        {
            BaseMessage_Model<List<T_StockInfo>> messageModel = new BaseMessage_Model<List<T_StockInfo>>();
            List<T_StockInfo> listStock = new List<T_StockInfo>();
            string errMsg = "";
            string newSerialNO = "";
            if (string.IsNullOrEmpty(UserJson))
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = "用户JSON为空！";
                //return JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TBase_Model>>>(messageModel);
                return JsonConvert.SerializeObject(messageModel);
            }
            try
            {
                UserModel user = BILBasic.JSONUtil.JSONHelper.JsonToObject<UserModel>(UserJson);
                T_StockInfo stockInfo = BILBasic.JSONUtil.JSONHelper.JsonToObject<T_StockInfo>(StockJson);
                T_AreaInfo areaInfo = BILBasic.JSONUtil.JSONHelper.JsonToObject<T_AreaInfo>(AreaJson);
                if (stockInfo.WarehouseNo != areaInfo.WarehouseNo)
                {

                    T_StockInfo stockERP = new T_StockInfo();
                    stockERP.CompanyCode = stockInfo.CompanyCode;
                    stockERP.StrongHoldCode = stockInfo.StrongHoldCode;
                    stockERP.ERPVoucherType = "DB6"; 
                    stockERP.MaterialNo = stockInfo.MaterialNo;
                    stockERP.VoucherType = 9996;
                    stockERP.ScanQty = stockInfo.AmountQty;
                    stockERP.FromErpWarehouse = stockInfo.WarehouseNo;
                    stockERP.FromAreaNo = "";
                    stockERP.FromBatchNo = stockInfo.BatchNo;
                    stockERP.ToErpWarehouse = areaInfo.WarehouseNo;
                    stockERP.ToErpAreaNo = "";
                 
                    stockERP.PostUser = user.UserNo;
                    listStock.Add(stockERP);
                    messageModel = SaveMoveErpDB(user, listStock);
                    if (messageModel.HeaderStatus == "E")
                    {
                        return JsonConvert.SerializeObject(messageModel);
                    }
                }
        

                if (!db.saveMoveBarcode(user, stockInfo, areaInfo, out errMsg, out newSerialNO))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = errMsg;
                }
                else
                {
                    listStock = new List<T_StockInfo>();
                    T_StockInfo stock = new T_StockInfo();
                    if (newSerialNO != "")
                    {
                        T_Stock_Func func = new T_Stock_Func();
                        stock.SerialNo = newSerialNO;
                        func.GetModelBySql(ref stock, ref errMsg);
                        listStock.Add(stock);
                    }
                    messageModel.HeaderStatus = "S";
                    messageModel.Message = newSerialNO;
                    messageModel.ModelJson = listStock;
                }

            }
            catch (Exception ex)
            {

                messageModel.HeaderStatus = "E";
                messageModel.Message = "执行异常" + ex.ToString();
            }

            return JsonConvert.SerializeObject(messageModel);
        }


        public BaseMessage_Model<List<T_StockInfo>> SaveMoveErpDB(UserModel user, List<T_StockInfo> stockInfo)
        {
            BaseMessage_Model<List<T_StockInfo>> model = new BaseMessage_Model<List<T_StockInfo>>();
            try
            {
                if (user == null)
                {
                    model.HeaderStatus = "E";
                    model.Message = "传入用户信息为空！";
                    JsonConvert.SerializeObject(model);
                }
                T_Interface_Func tfunc = new T_Interface_Func();

                string ERPJson = JSONHelper.ObjectToJson<List<T_StockInfo>>(stockInfo);
                LogNet.LogInfo("ERPJsonBefore:" + ERPJson);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);
                model = JSONHelper.JsonToObject<BaseMessage_Model<List<T_StockInfo>>>(interfaceJson);
                LogNet.LogInfo("ERPJsonAfter:" + interfaceJson);
                return model;

            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = "过账失败" + GetModelChineseName() + "失败！" + ex.Message + ex.TargetSite;
                return model;
            }
        }

        /// <summary>
        /// 根据物料编号查询库存
        /// </summary>
        /// <param name="MaterialNo"></param>
        /// <returns></returns>
        public string GetStockByMaterialNoADF( string MaterialNo)
        {
            BaseMessage_Model<List<T_StockInfo>> messageModel = new BaseMessage_Model<List<T_StockInfo>>();
            try
            {
                string strError = string.Empty;
                T_StockInfo model = new T_StockInfo();
                string MaterialNoIDIN = string.Empty;
                string SerialNo = string.Empty;
                string DeMaterialNo = string.Empty;
                string BarCodeType = string.Empty;

                if (string.IsNullOrEmpty(MaterialNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来物料号为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }


                bool bSucc = new T_OutBarCode_Func().GetMaterialNoByBarCode(MaterialNo, ref DeMaterialNo, ref strError);

                if (bSucc == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }


                T_Material_Func tfunc = new T_Material_Func();
                List<T_MaterialInfo> modelList = new List<T_MaterialInfo>();
                modelList = tfunc.GetMaterialModelBySql(DeMaterialNo, ref strError);
                if (modelList == null)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                foreach (var item in modelList)
                {
                    MaterialNoIDIN += item.ID + ",";
                }
                MaterialNoIDIN = MaterialNoIDIN.TrimEnd(',');

                List<T_StockInfo> lstModel = new List<T_StockInfo>();
                lstModel = tdb.GetStockByMaterialNo(MaterialNoIDIN);

                if (lstModel == null || lstModel.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "该物料没有库存数据！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = lstModel;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
            }
        }



        /// <summary>
        /// 根据EAN查询库存
        /// </summary>
        /// <param name="EAN"></param>
        /// <returns></returns>
        public string GetStockByEANADF(string EAN)
        {
            BaseMessage_Model<List<T_StockInfo>> messageModel = new BaseMessage_Model<List<T_StockInfo>>();
            try
            {
                string strError = string.Empty;
                T_StockInfo model = new T_StockInfo();
                string SerialNo = string.Empty;
                string DeEAN = string.Empty;
                string BarCodeType = string.Empty;

                if (string.IsNullOrEmpty(EAN))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来EAN号为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                bool bSucc = new T_OutBarCode_Func().GetEANByBarCode(EAN, ref DeEAN, ref strError);

                if (bSucc == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                List<T_StockInfo> lstModel = new List<T_StockInfo>();
                lstModel = tdb.GetStockByEAN(DeEAN);

                if (lstModel == null || lstModel.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "该EAN没有库存数据！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = lstModel;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
            }
        }


        /// <summary>
        /// 根据货位编码获取库存
        /// </summary>
        /// <param name="AreaNo"></param>
        /// <returns></returns>
        public string GetStockByAreaNoADF(string AreaNo)
        {
            BaseMessage_Model<List<T_StockInfo>> messageModel = new BaseMessage_Model<List<T_StockInfo>>();
            try
            {
                string strError = string.Empty;
                T_StockInfo model = new T_StockInfo();

                if (string.IsNullOrEmpty(AreaNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来货位编码为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                List<T_StockInfo> lstModel = new List<T_StockInfo>();
                lstModel = tdb.GetStockByAreaNo(AreaNo);

                if (lstModel == null || lstModel.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "该货位没有库存数据！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = lstModel;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
            }
        }

        /// <summary>
        /// 根据批次获取库存数据
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public string GetStockByBatchNoADF(string BarCode)
        {
            BaseMessage_Model<List<T_StockInfo>> messageModel = new BaseMessage_Model<List<T_StockInfo>>();
            try
            {
                string strError = string.Empty;
                T_StockInfo model = new T_StockInfo();

                string StockBatchNo = string.Empty;
                string BarCodeType = string.Empty;

                if (string.IsNullOrEmpty(BarCode))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来批次编号为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                bool bSucc = new T_OutBarCode_Func().GetBatchNoByBarCode(BarCode, ref StockBatchNo, ref BarCodeType, ref strError);

                if (bSucc == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                //2表示是扫描条码，需要查找库存
                if (BarCodeType == "2")
                {
                    if (GetStockBySerialNo(StockBatchNo, ref model, ref strError) == false)
                    {
                        messageModel.HeaderStatus = "E";
                        messageModel.Message = strError;
                        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                    }
                    else
                    {
                        StockBatchNo = model.BatchNo;
                    }
                }

                List<T_StockInfo> lstModel = new List<T_StockInfo>();
                lstModel = tdb.GetStockByBatchNo(StockBatchNo);

                if (lstModel == null || lstModel.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "该批次没有库存数据！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = lstModel;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
            }
        }

        public string GetStockBySupplierNoADF(string BarCode)
        {
            BaseMessage_Model<List<T_StockInfo>> messageModel = new BaseMessage_Model<List<T_StockInfo>>();
            try
            {
                string strError = string.Empty;
                T_StockInfo model = new T_StockInfo();

                string StockSupplierNo = string.Empty;
                string BarCodeType = string.Empty;

                if (string.IsNullOrEmpty(BarCode))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来供应商编号为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                bool bSucc = new T_OutBarCode_Func().GetBatchNoByBarCode(BarCode, ref StockSupplierNo, ref BarCodeType, ref strError);

                if (bSucc == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                //2表示是扫描条码，需要查找库存
                if (BarCodeType == "2")
                {
                    if (GetStockBySerialNo(StockSupplierNo, ref model, ref strError) == false)
                    {
                        messageModel.HeaderStatus = "E";
                        messageModel.Message = strError;
                        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                    }
                    else
                    {
                        StockSupplierNo = model.SupCode;
                    }
                }

                List<T_StockInfo> lstModel = new List<T_StockInfo>();
                lstModel = tdb.GetStockBySupplierNo(StockSupplierNo);

                if (lstModel == null || lstModel.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "该批次供应商没有库存数据！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = lstModel;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
            }
        }


        /// <summary>
        /// 根据货位编码获取库存，并判断扫描物料和库位库存物料检验状态以及是否合格是否一致
        /// </summary>
        /// <param name="AreaNo"></param>
        /// <returns></returns>
        public bool GetStockByAreaNo(string AreaNo, T_StockInfo model, ref string strError)
        {
            try
            {

                List<T_StockInfo> lstModel = new List<T_StockInfo>();
                lstModel = tdb.GetStockByAreaNo(AreaNo);

                if (lstModel == null || lstModel.Count == 0)
                {
                    return true;
                }

                //相同物料，不同库存质检状态不同质检判定结果不能放在一个库位
                var itemModel = lstModel.FindAll(t => t.MaterialNoID == model.MaterialNoID);

                return true;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据货位编码获取库存
        /// </summary>
        /// <param name="AreaNo"></param>
        /// <returns></returns>
        public bool GetStockBySerialNo(string SerialNo, ref T_StockInfo model, ref string strErrMsg)
        {

            try
            {
                string strError = string.Empty;



                if (string.IsNullOrEmpty(SerialNo))
                {

                    strErrMsg = "传入条码为空！";
                    return false;
                }

                model = tdb.GetStockBySerialNo(SerialNo);

                if (model == null || string.IsNullOrEmpty(model.SerialNo))
                {
                    strErrMsg = "该条码没有库存数据！";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据托盘号或者序列号获取库存托盘信息
        /// </summary>
        /// <param name="strPalletNo"></param>
        /// <param name="modelList"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        public bool GetStockInfoByPalletNo(string strPalletNo, ref List<T_StockInfo> modelList, ref string strErrMsg)
        {
            string Filter = " (palletno = '" + strPalletNo + "' or serialno = '" + strPalletNo + "' ) ";

            return base.GetModelListByFilter(ref modelList, ref strErrMsg, "", Filter);
        }


        /// <summary>
        /// 根据序列号获取库存信息
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <param name="model"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        public bool GetStockByBarCode(string SerialNo, ref T_StockInfo model, ref string strErrMsg)
        {

            try
            {
                string strError = string.Empty;

                if (string.IsNullOrEmpty(SerialNo))
                {
                    strErrMsg = "传入条码为空！";
                    return false;
                }

                return base.GetModelByFilter(ref model, "serialno='" + SerialNo + "'", ref strErrMsg);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据检验结果更新库存状态
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool UpdateStockByQuality(List<T_QualityDetailInfo> modelList, ref string strError)
        {
            return tdb.UpdateStockByQuality(modelList, ref strError);
        }

        protected override List<T_StockInfo> GetModelListByJson(string UserJson, string ModelListJson)
        {
            string strUserNo = string.Empty;
            UserModel userModel = JSONHelper.JsonToObject<UserModel>(UserJson);
            List<T_StockInfo> modelList = new List<T_StockInfo>();
            modelList = JSONHelper.JsonToObject<List<T_StockInfo>>(ModelListJson);

            //if (TOOL.RegexMatch.isExists(userModel.UserNo) == true)
            //{
            //    strUserNo = userModel.UserNo.Substring(0, userModel.UserNo.Length - 1);
            //}
            //else
            //{
            //    strUserNo = userModel.UserNo;
            //}

            ////确定过账人，根据登录账户以及操作的订单据点来确定
            //User_DB _db = new User_DB();
            //string strPostUser = _db.GetPostAccountByUserNo(strUserNo, modelList[0].StrongHoldCode);

            modelList.ForEach(t => t.ScanQty = t.Qty);
            modelList.ForEach(t => t.PostUser = userModel.UserNo);//strPostUser

            string ErpVoucherType = (userModel.ReceiveWareHouseNo.Contains("AD09") || userModel.ReceiveWareHouseNo.Contains("AD04")) ? "ZF1" : string.Empty;

            modelList.ForEach(t => t.ERPVoucherType = ErpVoucherType);

            LogNet.LogInfo("SaveT_StockADF---" + JSONHelper.ObjectToJson<List<T_StockInfo>>(modelList));

            return modelList;
        }

        /// <summary>
        /// add by cym 2017-10-06
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="ModelListJson"></param>
        /// <returns></returns>
        protected override List<T_StockInfo> GetModelListByJson_ERP_Product(string UserJson, string ModelListJson)
        {
            string strUserNo = string.Empty;
            UserModel userModel = JSONHelper.JsonToObject<UserModel>(UserJson);
            List<T_StockInfo> modelList = new List<T_StockInfo>();
            modelList = JSONHelper.JsonToObject<List<T_StockInfo>>(ModelListJson);

            //if (BILWeb.TOOL.RegexMatch.isExists(userModel.UserNo) == true)
            //{
            //    strUserNo = userModel.UserNo.Substring(0, userModel.UserNo.Length - 1);
            //}
            //else
            //{
            //    strUserNo = userModel.UserNo;
            //}

            ////确定过账人，根据登录账户以及操作的订单据点来确定
            //BILWeb.Login.User.User_DB _db = new BILWeb.Login.User.User_DB();
            //string strPostUser = _db.GetPostAccountByUserNo(strUserNo, modelList[0].StrongHoldCode);

            modelList.ForEach(t => t.FromErpAreaNo = "");
            //add by cym 2018-04-25
            modelList.ForEach(t => t.FromAreaNo = "");
            modelList.ForEach(t => t.ToErpAreaNo = "");

            modelList.ForEach(t => t.ScanQty = t.Qty);
            modelList.ForEach(t => t.PostUser = userModel.UserNo);//strPostUser

            LogNet.LogInfo("GetModelListByJson_ERP_Product---" + JSONHelper.ObjectToJson<List<T_StockInfo>>(modelList));

            return modelList;
        }

        /// <summary>
        /// 根据物料ID获取可用总库存
        /// </summary>
        /// <param name="MaterialNoID"></param>
        /// <returns></returns>
        public decimal GetSumStockQtyByMaterialIDForOutDetail(int MaterialNoID, string IsSpcBatchNo, string BatchNo, string WareHouseNo, string StrongHoldCode)
        {
            T_Stock_DB _db = new T_Stock_DB();
            return _db.GetSumStockQtyByMaterialIDForOutDetail(MaterialNoID, IsSpcBatchNo, BatchNo, WareHouseNo, StrongHoldCode);
        }

        #region 在库检扫描

        public string ScanQualityStockADF(string BarCode)
        {
            BaseMessage_Model<T_StockInfo> messageModel = new BaseMessage_Model<T_StockInfo>();
            try
            {
                string strError = string.Empty;
                string SerialNo = string.Empty;
                string BarCodeType = string.Empty;
                List<T_StockInfo> modelList = new List<T_StockInfo>();

                T_OutBarCodeInfo BarCodeInfo = new T_OutBarCodeInfo();
                T_OutBarCode_Func tfunc = new T_OutBarCode_Func();

                //验证条码正确性
                if (tfunc.GetSerialNoByBarCode(BarCode, ref SerialNo, ref BarCodeType, ref strError) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                }

                T_Stock_DB _db = new T_Stock_DB();
                T_StockInfo model = _db.ScanQualityStockADF(SerialNo);

                if (model == null)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "您扫描的条码或者托盘条码不存在！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                }

                if (model.Status == 1)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "物料待检中，不能发起在库检！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                }

                if (model.Status == 2)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "物料送检中，不能发起在库检！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                }

                if (model.TaskDetailesID > 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "物料已经拣货，不能发起在库检！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                }

                if (model.CheckID > 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "物料正在盘点，不能发起在库检！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = model;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
            }
        }
        #endregion



        public string GetStockByOutStockReviewByID(string ID)
        {
            BaseMessage_Model<List<T_StockInfo>> messageModel = new BaseMessage_Model<List<T_StockInfo>>();
            try
            {
                string strError = string.Empty;
                string SerialNo = string.Empty;
                string BarCodeType = string.Empty;
                List<T_StockInfo> modelList = new List<T_StockInfo>();



                T_Stock_DB _db = new T_Stock_DB();
                modelList = _db.GetStockByOutStockReviewByID(ID);

                if (modelList == null || modelList.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "该物料没有下架，不能复核！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                modelList.ForEach(t => t.PalletQty = modelList.Sum(t1 => t1.Qty));

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = modelList;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
            }

        }

        public string SaveMoveStockToOutADF(string UserJson, string ModelJson)
        {
            BaseMessage_Model<List<T_StockInfo>> messageModel = new BaseMessage_Model<List<T_StockInfo>>();

            try
            {
                string strUserNo = string.Empty;
                T_Area_Func tfunc = new T_Area_Func();
                T_AreaInfo model = new T_AreaInfo();
                string strError = string.Empty;
                string strWMSError = string.Empty;
                bool bSucc = false;

                if (string.IsNullOrEmpty(UserJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来用户JSON为空!";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                if (string.IsNullOrEmpty(ModelJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来业务JSON为空!";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                UserModel userModel = JSONHelper.JsonToObject<UserModel>(UserJson);

                List<T_StockInfo> modelList = JSONHelper.JsonToObject<List<T_StockInfo>>(ModelJson);

                LogNet.LogInfo("SaveMoveStockToOutADF---" + ModelJson);

                if (string.IsNullOrEmpty(modelList[0].AreaNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "移入货位为空!";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                bSucc = PostMoveStock(userModel, modelList, ref strError);
                if (bSucc == false)
                {
                    //strError += "生成调拨单失败：" + strError;
                    messageModel.Message = strError;
                    messageModel.HeaderStatus = "E";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
                }

                T_Stock_DB _db = new T_Stock_DB();
                bSucc = tdb.SaveMoveStockToOutADF(userModel, modelList, ref strWMSError);

                if (bSucc == false)
                {
                    strError += "   WMS库存扣减失败！" + strWMSError;
                    messageModel.Message = strError;
                    messageModel.HeaderStatus = "E";
                }
                else
                {
                    strError += "   WMS库存扣减成功！" + strWMSError;
                    messageModel.Message = strError;
                    messageModel.HeaderStatus = "S";
                }

                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_StockInfo>>>(messageModel);
            }
        }

        public bool PostMoveStock(UserModel userModel, List<T_StockInfo> modelList, ref string strError)
        {
            try
            {
                BaseMessage_Model<List<T_StockInfo>> model = new BaseMessage_Model<List<T_StockInfo>>();
                bool bSucc = false;
                string strUserNo = string.Empty;
                string strPostUser = string.Empty;
                List<T_StockInfo> NewLstStock = new List<T_StockInfo>();


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

                NewLstStock = GroupInstockDetailList(modelList[0].VoucherType, modelList);

                NewLstStock.ForEach(t => t.ScanQty = t.Qty);
                NewLstStock.ForEach(t => t.PostUser = userModel.UserNo);//strPostUser
                NewLstStock.ForEach(t => t.ToErpWarehouse = modelList[0].ToErpWarehouse);
                NewLstStock.ForEach(t => t.ToErpAreaNo = modelList[0].ToErpAreaNo);

                T_Interface_Func tfunc = new T_Interface_Func();
                string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<T_StockInfo>>(NewLstStock);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

                model = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_StockInfo>>>(interfaceJson);

                //过账失败直接返回
                if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
                {
                    strError = "生成调拨单失败！" + model.Message;
                    bSucc = false;
                }
                else if (model.HeaderStatus == "S" && !string.IsNullOrEmpty(model.MaterialDoc)) //过账成功，并且生成了凭证要记录数据库
                {
                    strError = "生成调拨单成功！凭证号：" + model.MaterialDoc;
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

        private List<T_StockInfo> GroupInstockDetailList(int VoucherType, List<T_StockInfo> modelList)
        {
            var newModelList = from t in modelList
                               group t by new { t1 = t.MaterialNo, t2 = t.BatchNo, t3 = t.FromWareHouseNo, t4 = t.FromAreaNo } into m
                               select new T_StockInfo
                               {
                                   MaterialNo = m.Key.t1,
                                   Qty = m.Sum(item => item.Qty),
                                   FromBatchNo = m.Key.t2,
                                   FromErpWarehouse = m.Key.t3,
                                   FromErpAreaNo = m.Key.t4,
                                   VoucherType = VoucherType,
                                   CompanyCode = m.FirstOrDefault().CompanyCode,
                                   StrongHoldCode = m.FirstOrDefault().StrongHoldCode

                               };

            return newModelList.ToList();
        }


        /// <summary>
        /// 生成T_OutStockTaskDetailsInfo提交ERP调拨过账
        /// </summary>
        /// <param name="user"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        protected override string GetModelListByJsonToERP(UserModel user, List<T_StockInfo> modelList)
        {
            List<T_StockInfo> NewLstStock = new List<T_StockInfo>();

            NewLstStock = GroupMoveStockDetailList(modelList[0].VoucherType, modelList);

            return JSONHelper.ObjectToJson<List<T_StockInfo>>(NewLstStock);

        }

        private List<T_StockInfo> GroupMoveStockDetailList(int VoucherType, List<T_StockInfo> modelList)
        {
            var newModelList = from t in modelList
                               group t by new { t1 = t.MaterialNo, t2 = t.BatchNo, t3 = t.FromWareHouseNo, t4 = t.FromAreaNo } into m
                               select new T_StockInfo
                               {
                                   MaterialNo = m.Key.t1,
                                   ScanQty = m.Sum(item => item.ScanQty),
                                   FromBatchNo = m.Key.t2,
                                   FromErpWarehouse = m.Key.t3,
                                   FromErpAreaNo = m.Key.t4,
                                   VoucherType = VoucherType,
                                   CompanyCode = m.FirstOrDefault().CompanyCode,
                                   StrongHoldCode = m.FirstOrDefault().StrongHoldCode,
                                   ToErpWarehouse = m.FirstOrDefault().ToErpWarehouse,
                                   ToErpAreaNo = m.FirstOrDefault().ToErpAreaNo,
                                   PostUser = m.FirstOrDefault().PostUser,
                                   ERPVoucherType = m.FirstOrDefault().ERPVoucherType
                               };

            return newModelList.ToList();
        }

        #region 玛莎盘点
        public string CheckGetBatchnoAndMaterialno(string EAN, string areaid)
        {
            BaseMessage_Model<List<string>> messageModel = new BaseMessage_Model<List<string>>();
            try
            {
                if (string.IsNullOrEmpty(EAN) || string.IsNullOrEmpty(areaid))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "EAN和库位ID为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<string>>>(messageModel);
                }

                List<string> list = new T_Stock_DB().CheckGetBatchnoAndMaterialno(EAN, areaid);

                if (list == null || list.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "获取失败";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<string>>>(messageModel);
                }
                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = list;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<string>>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<string>>>(messageModel);
            }
        }


        public string CheckSerialno(string EAN, string areaid, string batchno, string materialno)
        {
            BaseMessage_Model<List<Barcode_Model>> messageModel = new BaseMessage_Model<List<Barcode_Model>>();
            try
            {
                if (string.IsNullOrEmpty(EAN) || string.IsNullOrEmpty(areaid) || string.IsNullOrEmpty(batchno) || string.IsNullOrEmpty(materialno))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "参数为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<Barcode_Model>>>(messageModel);
                }

                string serialno = new T_Stock_DB().CheckSerialno(EAN, areaid, batchno, materialno);
                if (string.IsNullOrEmpty(serialno))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "获取库存数据失败";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<Barcode_Model>>>(messageModel);
                }
                //根据序列号获取条码信息
                return new Check_DB().GetScanInfo(serialno);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<Barcode_Model>>>(messageModel);
            }
        }


        public string OffSerialno(string EAN, string areaid, string batchno, string materialno)
        {
            BaseMessage_Model<List<Barcode_Model>> messageModel = new BaseMessage_Model<List<Barcode_Model>>();
            try
            {
                if (string.IsNullOrEmpty(EAN) || string.IsNullOrEmpty(areaid) || string.IsNullOrEmpty(batchno) || string.IsNullOrEmpty(materialno))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "参数为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<Barcode_Model>>>(messageModel);
                }

                string serialno = new T_Stock_DB().CheckSerialno(EAN, areaid, batchno, materialno);
                if (string.IsNullOrEmpty(serialno))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "获取库存数据失败";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<Barcode_Model>>>(messageModel);
                }
                //根据序列号获取条码信息
                return GetStockModelBySqlADF(serialno, "2", "1", "1");
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<Barcode_Model>>>(messageModel);
            }
        }
        #endregion

    }
}
