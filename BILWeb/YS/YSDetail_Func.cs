using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.JSONUtil;
using BILWeb.OutBarCode;
using BILBasic.XMLUtil;
using BILBasic.Common;
using BILWeb.Pallet;
using BILWeb.Stock;

using BILBasic.User;
using BILWeb.Login.User;

namespace BILWeb.YS
{
    public partial class T_YSDetail_Func : TBase_Func<T_YSDetail_DB, T_YSDetailInfo>, IYSDetailService
    {
        T_OutBarCode_Func outBarCodeFunc = new T_OutBarCode_Func();
        T_YSDetail_DB _db = new T_YSDetail_DB();

        protected override bool CheckModelBeforeSave(T_YSDetailInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (string.IsNullOrEmpty(model.MaterialNo))
            {
                strError = "物料编号为空，不能保存！";
                return false;
            }

            if (model.InStockQty == 0)
            {
                strError = "数量为零，不能保存！";
                return false;
            }
            return true;
        }

        /// <summary>
        /// PDA提交数据验证
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        protected override bool CheckModelBeforeSave(List<T_YSDetailInfo> modelList, ref string strError)
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


            if (CheckScanQty(modelList.Where(t => t.ScanQty > 0).ToList(), ref strError) == false)
            {
                return false;
            }

            if (CheckBarCodeIsSame(modelList.Where(t => t.ScanQty > 0).ToList(), ref strError) == false)
            {
                return false;
            }


            //验证单据是否关闭或完成

            return true;
        }

        /// <summary>
        /// 重写过账提交信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        protected override string GetModelListByJsonToERP(UserModel user, List<T_YSDetailInfo> modelList)
        {
            List<T_YSDetailInfo> lstDetail = new List<T_YSDetailInfo>();
            T_YSDetailInfo stockDetailInfo = null;
            if (modelList == null || modelList.Count == 0)
            {
                return "";
            }
            if (modelList[0].VoucherType != 22)
            {

            }
            for (int i = 0; i < modelList.Count; i++)
            {
                if (modelList[i].lstBarCode != null && modelList[i].lstBarCode.Count > 0)
                {
                    foreach (var item in modelList[i].lstBarCode)
                    {
                        stockDetailInfo = lstDetail.Find(p => p.MaterialNo == item.MaterialNo && p.BatchNo == item.BatchNo && p.RowNo == item.RowNo && p.RowNoDel == item.RowNoDel);
                        if (stockDetailInfo == null)
                        {
                            stockDetailInfo = new T_YSDetailInfo();
                            stockDetailInfo.VoucherType = modelList[i].VoucherType;
                            stockDetailInfo.ErpVoucherNo = modelList[i].ErpVoucherNo;
                            stockDetailInfo.RowNo = modelList[i].RowNo;
                            stockDetailInfo.RowNoDel = modelList[i].RowNoDel;
                            stockDetailInfo.MaterialNo = item.MaterialNo;
                            stockDetailInfo.CompanyCode = modelList[i].CompanyCode;
                            stockDetailInfo.StoreCondition = modelList[i].StoreCondition;
                            stockDetailInfo.ScanQty = Convert.ToDecimal(item.Qty);
                            stockDetailInfo.Unit = modelList[i].Unit;
                            stockDetailInfo.SupPrdBatch = modelList[i].SupPrdBatch;
                            stockDetailInfo.ReceiveWareHouseNo = modelList[i].ReceiveWareHouseNo.Replace("SHJC-","").Replace("JSJC-", "").Replace("SHSY-", "");
                            stockDetailInfo.WareHouseNo = stockDetailInfo.ReceiveWareHouseNo;
                            stockDetailInfo.ReceiveAreaNo = modelList[i].ReceiveAreaNo;
                            stockDetailInfo.BatchNo = item.BatchNo;
                            stockDetailInfo.ReceiveUserNo = user.UserNo;
                            stockDetailInfo.PostUser = user.UserName;
                            stockDetailInfo.StrSupPrdDate = modelList[i].StrSupPrdDate;

                            stockDetailInfo.FromErpWarehouse = modelList[i].FromErpWarehouse;
                            stockDetailInfo.ToErpWarehouse = modelList[i].ToErpWarehouse;
                            stockDetailInfo.FromBatchNo = modelList[i].FromBatchNo;
                            stockDetailInfo.ToErpAreaNo = "";
                            stockDetailInfo.ToBatchNo = modelList[i].ToBatchNo;

                            stockDetailInfo.GUID = modelList[i].GUID;
                            if (item.EDate != null)
                            {
                                item.StrEDate = item.EDate.ToString("yyyy/MM/dd");
                            }
                            stockDetailInfo.StrEDate = item.StrEDate;
                            stockDetailInfo.StrongHoldCode = modelList[i].StrongHoldCode;
                            lstDetail.Add(stockDetailInfo);
                        }
                        else
                        {
                            stockDetailInfo.ScanQty = stockDetailInfo.ScanQty + Convert.ToDecimal(item.Qty);
                        }
                    }
                }
            }


            return JSONHelper.ObjectToJson<List<T_YSDetailInfo>>(lstDetail); 
        }

        /// <summary>
        /// 验证扫描数量和剩余数量
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        private bool CheckScanQty(List<T_YSDetailInfo> modelList, ref string strErrMsg)
        {
            bool bSucc = false;

            foreach (var item in modelList)
            {
                if (item.ScanQty > item.RemainQty)
                {
                    bSucc = false;
                    strErrMsg = "扫描数量大于收货剩余数量！物料：" + item.MaterialNo + "行号：" + item.RowNo + "扫描数量：" + item.ScanQty + "收货剩余：" + item.RemainQty;
                    break;
                }
                else
                {
                    bSucc = true;
                }
            }

            return bSucc;
        }

        /// <summary>
        /// 验证条码是否有相同数据
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        private bool CheckBarCodeIsSame(List<T_YSDetailInfo> modelList, ref string strErrMsg)
        {
            bool bSucc = false;

            List<T_OutBarCodeInfo> lstBarCode = new List<T_OutBarCodeInfo>();

            foreach (var item in modelList)
            {
                lstBarCode.AddRange(item.lstBarCode);
            }

            var groupByList = from t in lstBarCode
                              group t by new { t1 = t.SerialNo } into m
                              select new
                              {
                                  SerialNo = m.Key.t1
                              };

            if (groupByList.Count() != modelList.Count)
            {
                strErrMsg = "收货条码存在相同数据，不能提交！";
                bSucc = false;
            }
            else { bSucc = true; }

            T_OutBarcode_DB _db = new T_OutBarcode_DB();
            bSucc = _db.CheckBarCodeIsExists(XmlUtil.Serializer(typeof(List<T_OutBarCodeInfo>), lstBarCode), ref strErrMsg);

            return bSucc;
        }

        /// <summary>
        /// PDA提交JSON转换成List对象
        /// </summary>
        /// <param name="ModelListJson"></param>
        /// <returns></returns>
        protected override List<T_YSDetailInfo> GetModelListByJson(string UserJson, string ModelListJson)
        {
            int IsQuality = 0;
            string strUserNo = string.Empty;

            List<T_YSDetailInfo> modelList = JSONHelper.JsonToObject<List<T_YSDetailInfo>>(ModelListJson);

            UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);

            modelList.ForEach(t => t.ReceiveQty = t.ScanQty);
            modelList = modelList.Where(t => t.ReceiveQty > 0).ToList();
            //IsQuality =  GetIsQuality(modelList[0]);
            IsQuality = GetIsQualityByVoucherType(modelList[0]);
            modelList.ForEach(t => t.IsQuality = IsQuality);

            //if (TOOL.RegexMatch.isExists(user.UserNo) == true)
            //{
            //    strUserNo = user.UserNo.Substring(0, user.UserNo.Length - 1);
            //}
            //else
            //{
            //    strUserNo = user.UserNo;
            //}

            ////确定过账人，根据登录账户以及操作的订单据点来确定
            //User_DB _db = new User_DB();
            //string strPostUser = _db.GetPostAccountByUserNo(strUserNo, modelList[0].StrongHoldCode);

            foreach (var item in modelList)
            {
                if (item.lstBarCode != null && item.lstBarCode.Count() > 0) 
                {
                    item.SupPrdBatch = item.lstBarCode.FirstOrDefault().SupPrdBatch;
                    item.SupPrdDate = item.lstBarCode.FirstOrDefault().SupPrdDate;
                    item.StrSupPrdDate = item.lstBarCode.FirstOrDefault().SupPrdDate.ToShortDateString();
                    item.BatchNo = item.lstBarCode.FirstOrDefault().BatchNo; //item.IsSpcBatch=="Y"?item.FromBatchNo: item.lstBarCode.FirstOrDefault().BatchNo;
                    item.ReceiveWareHouseNo = user.ReceiveWareHouseNo;
                    item.ReceiveAreaNo = string.Empty;
                    item.ReceiveUserNo = user.UserNo;// strPostUser;
                    item.ToErpWarehouse = user.ReceiveWareHouseNo;
                    item.ToErpAreaNo = string.Empty;
                    item.ToBatchNo = item.BatchNo;
                    item.PostUser = user.UserNo;
                    item.StrEDate = item.lstBarCode.FirstOrDefault().EDate.ToString("yyyy/MM/dd");
                }
                
                //item..PostDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            LogNet.LogInfo("SaveT_InStockDetailADF---" + JSONHelper.ObjectToJson<List<T_YSDetailInfo>>(modelList));

            return modelList;
        }


        private int GetIsQuality(T_YSDetailInfo model)
        {
            int iIsQuality = 0;
            string strError = string.Empty;
            List<ComboBoxItem> items = new List<ComboBoxItem>();
            if (Common_Func.GetComboBoxItem("cbxIsQuality", ref items, ref strError) == false)
            {
                iIsQuality = 0;
            }
            else
            {
                var item = items.Find(t => t.ID == model.VoucherType);
                if (item != null)
                {
                    iIsQuality = Convert.ToInt32(item.Name);
                }
            }
            return iIsQuality;
        }

        private int GetIsQualityByVoucherType(T_YSDetailInfo model)
        {
            int iIsQuality = 0;

            if (model.VoucherType == 22)
            {
                iIsQuality = 1;
            }
            else
            {
                switch (model.QcCode)
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
            }


            return iIsQuality;
        }


        protected override string GetModelChineseName()
        {
            return "收货表体";
        }

        protected override T_YSDetailInfo GetModelByJson(string strJson)
        {
            //string errorMsg = string.Empty;
            //T_YSDetailInfo model = JSONHelper.JsonToObject<T_YSDetailInfo>(strJson);

            //if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            //{
            //    BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
            //    PFunc.Sync(10, string.Empty, model.ErpVoucherNo, model.VoucherType, ref errorMsg, "ERP", -1, null);

            //}
            return JSONHelper.JsonToObject<T_YSDetailInfo>(strJson);
        }

        #region  扫描条码收货
        /// <summary>
        /// 根据条码或者托盘号获取托盘明细,不需要需要获取库存数据的，收货用
        /// 条码有组托返回托盘明细，没有组托返回单个条码托盘明细
        /// 托盘条码返回托盘明细
        /// 该接口判断了条码或者托盘是否已经入库
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public string GetPalletDetailByBarCode(string UserJson, string BarCode)
        {

            BaseMessage_Model<List<T_OutBarCodeInfo>> model = new BaseMessage_Model<List<T_OutBarCodeInfo>>();
            List<T_OutBarCodeInfo> modelList = new List<T_OutBarCodeInfo>();
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
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(model);
                }

                if (string.IsNullOrEmpty(UserJson))
                {
                    model.HeaderStatus = "E";
                    model.Message = "客户端传来的用户JSON为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(model);
                }

                UserModel userModel = JSONHelper.JsonToObject<UserModel>(UserJson);
                if (userModel == null || string.IsNullOrEmpty(userModel.UserNo))
                {
                    model.HeaderStatus = "E";
                    model.Message = "解析用户JSON为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(model);
                }

                //验证条码正确性
                if (outBarCodeFunc.GetSerialNoByBarCode(BarCode, ref SerialNo, ref BarCodeType, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(model);
                }

                //验证外箱条码或者托盘条码是否已经收货
                if (outBarCodeFunc.CheckBaeCodeIsRecive(SerialNo, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(model);
                }

                if (GetPalletDetailBySerialNo(ref modelList, SerialNo, ref strError, BarCodeType) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(model);
                }

               

                ///如果物料，批次在DJ存在已经判定的质量状态。必须先移走
                //T_Stock_DB _db = new T_Stock_DB();
                //string strStatus = _db.GetMaterialBatchStatus(modelList[0].MaterialNo, modelList[0].BatchNo, userModel.ReceiveAreaID.ToString());
                //if (strStatus == "3")
                //{
                //    model.HeaderStatus = "E";
                //    model.ModelJson = null;
                //    model.Message = "物料：" + modelList[0].MaterialNo + "批次：" + modelList[0].BatchNo + "在收货库位检验合格，请先移至其他库位后再进行收货！";
                //    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(model);
                //}

                //if (strStatus == "4")
                //{
                //    model.HeaderStatus = "E";
                //    model.ModelJson = null;
                //    model.Message = "物料：" + modelList[0].MaterialNo + "批次：" + modelList[0].BatchNo + "在收货库位检验不合格，请先移至其他库位后再进行收货！";
                //    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(model);
                //}

                //截取物料第一位
                //string IsKeGong = modelList[0].MaterialNo.Substring(0, 1);
                //string UserWareHouse = userModel.ReceiveWareHouseNo;

                //if (IsKeGong.ToUpper().Equals("K")) //是客供料
                //{
                //    if (!"AD05,AD09,AD08,AD10,AD11".Contains(UserWareHouse))
                //    {
                //        model.HeaderStatus = "E";
                //        model.ModelJson = null;
                //        model.Message = "客供料收货仓库错误！应录入客供品仓库！当前登录仓库为：" + UserWareHouse;
                //        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(model);
                //    }
                //}
                //else
                //{
                //    if ("AD05".Contains(UserWareHouse))
                //    {
                //        model.HeaderStatus = "E";
                //        model.ModelJson = null;
                //        model.Message = "非客供料不能录入客供品仓库！当前登录仓库为：" + UserWareHouse;
                //        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(model);
                //    }
                //}

                model.HeaderStatus = "S";
                model.ModelJson = modelList;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(model);
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(model);
            }
        }





        /// <summary>
        /// 根据序列号获取托盘明细,托盘表的明细，托盘用
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="SerialNo"></param>
        /// <param name="strError"></param>
        /// <param name="BarCodeType"></param>
        /// <returns></returns>
        private bool GetPalletDetailBySerialNo(ref List<T_OutBarCodeInfo> modelList, string SerialNo, ref string strError, string BarCodeType)
        {

            //外箱条码序列号
            if (BarCodeType == "1")
            {
                if (GetPalletInfoBySerialNo(SerialNo, ref modelList, ref strError) == false)
                {
                    return false;
                }
            }
            else if (BarCodeType == "2")
            {
                if (GetPalletInfoByPalletSerialNo(SerialNo, ref modelList, ref strError) == false)
                {
                    return false;
                }
            }
            else if (BarCodeType == "3")//序列号
            {
                //扫描的序列号不是外箱条码，需要查找是否是托盘条码
                if (GetPalletInfoBySerialNo(SerialNo, ref modelList, ref strError) == false && GetPalletInfoByPalletSerialNo(SerialNo, ref modelList, ref strError) == false)
                {
                    strError = "外箱条码或者托盘条码不存在！";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 外箱条码没有收货，需要查询是否已经拼托，如果拼托要返回整托信息,如果没有拼托则返回单个条码，用条码类封装
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        private bool GetPalletInfoBySerialNo(string SerialNo, ref List<T_OutBarCodeInfo> modelList, ref string strError)
        {
            bool bSucc = false;
            string strFilter = string.Empty;
            T_OutBarCodeInfo model = new T_OutBarCodeInfo();
            T_OutBarCode_Func toc = new T_OutBarCode_Func();
            T_PalletDetail_Func palletFunc = new T_PalletDetail_Func();
            List<T_PalletDetailInfo> lstPallet = new List<T_PalletDetailInfo>();
            T_OutBarCodeInfo outBarCodeModel = new T_OutBarCodeInfo();
            List<T_OutBarCodeInfo> HModelList = new List<T_OutBarCodeInfo>();
            T_OutBarcode_DB odb = new T_OutBarcode_DB();
            decimal SumPalletQty = 0;

            //外箱条码不存在
            if (outBarCodeFunc.GetOutBarCodeInfoBySerialNo(SerialNo, ref model, ref strError) == false)
            {
                return false;
            }


            strFilter = "palletno = (select palletno from t_Palletdetail where barcode = '" + model.BarCode + "')";

            //外箱条码存在，但是没有组托，需要生成新的托盘类，返回客户端
            if (palletFunc.GetPalletByPalletNo(strFilter, ref lstPallet, ref strError) == false)
            {
                if (toc.GetOutBarCodeInfoBySerialNo(SerialNo, ref outBarCodeModel, ref strError) == false)
                {
                    return false;
                }

                if (outBarCodeModel.BarcodeType == 5) //混箱
                {
                    model.BarcodeType = 5;

                    model.lstBarCode = odb.GetBarCodeOutAll(model.BarCode);
                }

                modelList.Add(model);
                bSucc = true;
            }
            else //已经组托，根据组托条码获取条码类
            {
                if (outBarCodeFunc.GetOutBarCodeByPalletNo(lstPallet[0].PalletNo, ref modelList, ref strError) == false)
                {
                    bSucc = false;
                }
                else
                {
                    HModelList = modelList.Where(t => t.BarcodeType == 5).ToList();

                    if (HModelList != null && HModelList.Count > 0) 
                    {
                        foreach (var item in HModelList) 
                        {
                            modelList.Find(t=>t.ID==item.ID).lstBarCode =  odb.GetBarCodeOutAll(item.BarCode);
                        }
                    }

                    modelList.ForEach(t => t.PalletNo = lstPallet[0].PalletNo);
                    SumPalletQty = modelList.Sum(t1 => t1.Qty).ToDecimal();
                    modelList.ForEach(t => t.PalletQty = SumPalletQty);
                    bSucc = true;
                }
            }

            return bSucc;
        }



        /// <summary>
        /// 根据托盘条码获取托盘明细
        /// </summary>
        /// <returns></returns>
        private bool GetPalletInfoByPalletSerialNo(string SerialNo, ref List<T_OutBarCodeInfo> modelList, ref string strError)
        {
            bool bSucc = false;
            T_PalletDetail_Func palletFunc = new T_PalletDetail_Func();
            string strFilter = string.Empty;
            decimal SumPalletQty = 0;
            List<T_OutBarCodeInfo> HModelList = new List<T_OutBarCodeInfo>();
            T_OutBarcode_DB odb = new T_OutBarcode_DB();

            //根据托盘号获取托盘明细
            strFilter = "palletno =  '" + SerialNo + "'";

            if (outBarCodeFunc.GetOutBarCodeByPalletNo(SerialNo, ref modelList, ref strError) == false)
            {
                bSucc = false;
            }
            else
            {
                HModelList = modelList.Where(t => t.BarcodeType == 5).ToList();

                if (HModelList != null && HModelList.Count > 0)
                {
                    foreach (var item in HModelList)
                    {
                        modelList.Find(t => t.ID == item.ID).lstBarCode = odb.GetBarCodeOutAll(item.BarCode);
                    }
                }
                modelList.ForEach(t => t.PalletNo = SerialNo);
                SumPalletQty = modelList.Sum(t1 => t1.Qty).ToDecimal();
                modelList.ForEach(t => t.PalletQty = SumPalletQty);
                bSucc = true;
            }

            return bSucc;

        }

        #endregion


        public string YSPost(string UserJson, string ModelJson)
        {
            BaseMessage_Model<string> messageModel = new BaseMessage_Model<string>();
            try
            {
                List<T_YSDetailInfo> modelList = JSONHelper.JsonToObject<List<T_YSDetailInfo>>(ModelJson);
                UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);
                string strError = "";
                T_YSDetail_DB db = new T_YSDetail_DB();
                if (db.YSPost(user, modelList, ref strError) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "原因：" + strError + "。保存失败！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                }
                messageModel.HeaderStatus = "S";
                messageModel.Message = "保存成功！";
                return JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = "原因：" + ex.ToString() + "。保存失败！";
                return JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
            }
        }

  

    }

}
