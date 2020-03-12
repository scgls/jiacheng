using BILBasic.Basing.Factory;
using BILBasic.JSONUtil;
using BILWeb.InStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILWeb.OutBarCode;
using BILWeb.Stock;
using BILBasic.User;

using Newtonsoft.Json;
using BILWeb.Print;

namespace BILWeb.Pallet
{
    public partial class T_PalletDetail_Func : TBase_Func<T_PalletDetail_DB, T_PalletDetailInfo>
    {
        T_OutBarCode_Func outBarCodeFunc = new T_OutBarCode_Func();
        T_PalletDetail_DB _db = new T_PalletDetail_DB();

        protected override bool CheckModelBeforeSave(T_PalletDetailInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            return true;
        }

        protected override bool CheckModelBeforeSave(List<T_PalletDetailInfo> modelList, ref string strError)
        {


            if (modelList == null || modelList.Count == 0)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (String.IsNullOrEmpty(modelList[0].PrintIPAdress))
            {
                strError = "请设置打印机IP地址！";
                return false;
            }

            //if (!Print_DB.isConnected(modelList[0].PrintIPAdress))
            //{
            //    strError = "打印机连接失败！";
            //    return false;
            //}


            return true;
        }

        protected override List<T_PalletDetailInfo> GetModelListByJson(string UserJson, string ModelListJson)
        {
            LogNet.LogInfo("SaveT_PalletDetailADF:" + ModelListJson);

            return JSONHelper.JsonToObject<List<T_PalletDetailInfo>>(ModelListJson);
        }


        protected override string GetModelChineseName()
        {
            return "托盘表体";
        }

        protected override T_PalletDetailInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
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
        public string GetPalletDetailByBarCode(string BarCode)
        {

            BaseMessage_Model<List<T_PalletDetailInfo>> model = new BaseMessage_Model<List<T_PalletDetailInfo>>();
            List<T_PalletDetailInfo> modelList = new List<T_PalletDetailInfo>();
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
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                //验证条码正确性
                if (outBarCodeFunc.GetSerialNoByBarCode(BarCode, ref SerialNo, ref BarCodeType, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                //验证外箱条码或者托盘条码是否已经收货
                if (outBarCodeFunc.CheckBaeCodeIsRecive(SerialNo, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                if (GetPalletDetailBySerialNo(ref modelList, SerialNo, ref strError, BarCodeType) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                model.HeaderStatus = "S";
                model.ModelJson = modelList;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
            }
        }

        /// <summary>
        /// 根据序列号获取托盘明细,托盘表的明细
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="SerialNo"></param>
        /// <param name="strError"></param>
        /// <param name="BarCodeType"></param>
        /// <returns></returns>
        private bool GetPalletDetailBySerialNo(ref List<T_PalletDetailInfo> modelList, string SerialNo, ref string strError, string BarCodeType)
        {

            //外箱条码
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
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 外箱条码没有收货，需要查询是否已经拼托，如果拼托要返回整托信息,如果没有拼托则返回单个条码，用托盘类封装
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        private bool GetPalletInfoBySerialNo(string SerialNo, ref List<T_PalletDetailInfo> modelList, ref string strError)
        {
            string strFilter = string.Empty;
            T_OutBarCodeInfo model = new T_OutBarCodeInfo();

            //外箱条码不存在
            if (outBarCodeFunc.GetOutBarCodeInfoBySerialNo(SerialNo, ref model, ref strError) == false)
            {
                return false;
            }

            strFilter = "palletno = (select palletno from t_Palletdetail where serialno = '" + SerialNo + "')";

            //外箱条码存在，但是没有组托，需要生成新的托盘类，返回客户端
            if (base.GetModelListByFilter(ref modelList, ref strError, "", strFilter) == false)
            {
                return false;
                //modelList = CreatePalletByBarcode(model);
            }

            return true;
        }

        /// <summary>
        /// 根据条码生成新的托盘类
        /// </summary>
        /// <param name="barcodeModel"></param>
        /// <returns></returns>
        private List<T_PalletDetailInfo> CreatePalletByBarcode(T_OutBarCodeInfo barcodeModel)
        {
            List<T_PalletDetailInfo> lstP = new List<T_PalletDetailInfo>();
            T_PalletDetailInfo pModel = new T_PalletDetailInfo();
            pModel.lstBarCode = new List<T_OutBarCodeInfo>();
            pModel.lstBarCode.Add(barcodeModel);
            lstP.Add(pModel);
            return lstP;
        }

        /// <summary>
        /// 根据托盘条码获取托盘明细
        /// </summary>
        /// <returns></returns>
        private bool GetPalletInfoByPalletSerialNo(string SerialNo, ref List<T_PalletDetailInfo> modelList, ref string strError)
        {
            string strFilter = string.Empty;

            //根据托盘号获取托盘明细
            strFilter = "palletno =  '" + SerialNo + "'";
            if (base.GetModelListByFilter(ref modelList, ref strError, "", strFilter) == false)
            {
                strError = "该托盘号不存在，请确认是否组托！" + strError;
                return false;
            }

            return true;
        }

        #endregion

        #region 组托扫描接口
        /// <summary>
        /// 根据条码或者托盘号获取托盘明细,如果托盘表没有，需要获取库存数据的
        /// 新建组托或者添加组托都是这个接口
        /// 此接口支持扫描外箱条码或者托盘条码或者序列号
        /// palletModel 1-新建组托 2-添加组托
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public string GetPalletDetailByPalletNo(string BarCode, string PalletModel)
        {
            BaseMessage_Model<List<T_PalletDetailInfo>> model = new BaseMessage_Model<List<T_PalletDetailInfo>>();

            string SerialNo = string.Empty;
            string strError = string.Empty;
            string BarCodeType = string.Empty;
            T_OutBarCodeInfo outBarCodeModel = new T_OutBarCodeInfo();
            List<T_PalletDetailInfo> modelList = new List<T_PalletDetailInfo>();
            List<T_PalletDetailInfo> NewModelList = new List<T_PalletDetailInfo>();
            List<T_StockInfo> lstStock = new List<T_StockInfo>();

            try
            {
                if (string.IsNullOrEmpty(BarCode))
                {
                    model.HeaderStatus = "E";
                    model.Message = "客户端传来的托盘号或者条码为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                //验证条码正确性
                if (!outBarCodeFunc.GetSerialNoByBarCode(BarCode, ref SerialNo, ref BarCodeType, ref strError))
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                //新建组托
                if (PalletModel == "1")
                {
                    //验证外箱条码或者托盘条码是否已经收货
                    //if (outBarCodeFunc.CheckBaeCodeIsRecive(SerialNo, ref strError) == false)
                    //{
                    //    model.HeaderStatus = "E";
                    //    model.Message = strError;
                    //    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                    //}

                    //验证外箱条码是否已经组托
                    if (_db.CheckSerialNoIsInPallet(SerialNo) > 0)
                    {
                        model.HeaderStatus = "E";
                        model.Message = "该条码已经拼托！";
                        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                    }

                    if (outBarCodeFunc.GetOutBarCodeInfoBySerialNo(SerialNo, ref outBarCodeModel, ref strError) == false)
                    {
                        model.HeaderStatus = "E";
                        model.Message = strError;
                        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                    }

                    modelList = CreatePalletForBarCode(outBarCodeModel);
                }
                else if (PalletModel == "2") //添加组托
                {
                    //验证外箱条码是否已经组托
                    if (_db.CheckSerialNoIsInPallet(SerialNo) == 0)
                    {
                        model.HeaderStatus = "E";
                        model.Message = "该条码没有拼托，不能添加组托！";
                        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                    }

                    //验证外箱条码或者托盘条码是否已经收货，如果收货，则认为是库存拼托
                    //没有收货则是收货添加组托
                    //if (outBarCodeFunc.CheckBaeCodeIsRecive(SerialNo, ref strError) == true)
                    //{
                        //外箱条码或者托盘条码没有收货，查找托盘表，返回托盘LIST
                        if (GetPalletDetailBySerialNo(ref modelList, SerialNo, ref strError, BarCodeType) == false)
                        {
                            model.HeaderStatus = "E";
                            model.Message = strError;
                            return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                        }
                        else
                        {
                            //根据托盘LIST查找条码表，返回新的托盘detailmodel+listbarcode返回客户端
                            //modelList = CreatePalletListBarCodeForPallet(modelList);
                            if (CreatePalletListBarCodeForPallet(modelList, ref NewModelList, ref strError) == false)
                            {
                                model.HeaderStatus = "E";
                                model.Message = strError;
                                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                            }

                            modelList = NewModelList;
                        }
                    //}
                    //else //条码已经收货,查找库存数据
                    //{
                        //model.HeaderStatus = "E";
                        //model.Message = "条码已经收货，不能添加组托！";
                        //return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                        //if (CheckPalletIsStock(SerialNo, ref lstStock, ref strError) == false)
                        //{
                        //    model.HeaderStatus = "E";
                        //    model.Message = strError;
                        //    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                        //}
                        //else
                        //{
                        //    GetStockPalletToPallet(ref modelList, lstStock);
                        //}

                    //}
                }

                model.HeaderStatus = "S";
                model.ModelJson = modelList;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
            }
        }


        public string GetPalletDetailByPalletNoYmh(string BarCode, string PalletModel)
        {
            BaseMessage_Model<List<T_PalletDetailInfo>> model = new BaseMessage_Model<List<T_PalletDetailInfo>>();

            string SerialNo = string.Empty;
            string strError = string.Empty;
            string BarCodeType = string.Empty;
            T_OutBarCodeInfo outBarCodeModel = new T_OutBarCodeInfo();
            List<T_PalletDetailInfo> modelList = new List<T_PalletDetailInfo>();
            List<T_PalletDetailInfo> NewModelList = new List<T_PalletDetailInfo>();
            List<T_StockInfo> lstStock = new List<T_StockInfo>();

            try
            {
                if (string.IsNullOrEmpty(BarCode))
                {
                    model.HeaderStatus = "E";
                    model.Message = "客户端传来的托盘号或者条码为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                //验证条码正确性
                if (!outBarCodeFunc.GetSerialNoByBarCode(BarCode, ref SerialNo, ref BarCodeType, ref strError))
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                //新建组托
                if (PalletModel == "1")
                {
                    //验证外箱条码或者托盘条码是否已经收货
                    if (outBarCodeFunc.CheckBaeCodeIsRecive(SerialNo, ref strError) == false)
                    {
                        model.HeaderStatus = "E";
                        model.Message = strError;
                        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                    }

                    //验证外箱条码是否已经组托
                    if (_db.CheckSerialNoIsInPallet(SerialNo) > 0)
                    {
                        model.HeaderStatus = "E";
                        model.Message = "该条码已经拼托！";
                        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                    }

                    if (outBarCodeFunc.GetOutBarCodeInfoBySerialNo(SerialNo, ref outBarCodeModel, ref strError) == false)
                    {
                        model.HeaderStatus = "E";
                        model.Message = strError;
                        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                    }

                    modelList = CreatePalletForBarCode(outBarCodeModel);
                }
                else if (PalletModel == "2") //添加组托
                {
                    //验证外箱条码是否已经组托
                    if (_db.CheckSerialNoIsInPallet(SerialNo) == 0)
                    {
                        model.HeaderStatus = "E";
                        model.Message = "该条码没有拼托，不能添加组托！";
                        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                    }

                    //验证外箱条码或者托盘条码是否已经收货，如果收货，则认为是库存拼托
                    //没有收货则是收货添加组托
                    if (outBarCodeFunc.CheckBaeCodeIsRecive(SerialNo, ref strError) == true)
                    {
                        //外箱条码或者托盘条码没有收货，查找托盘表，返回托盘LIST
                        if (GetPalletDetailBySerialNo(ref modelList, SerialNo, ref strError, BarCodeType) == false)
                        {
                            model.HeaderStatus = "E";
                            model.Message = strError;
                            return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                        }
                        else
                        {
                            //根据托盘LIST查找条码表，返回新的托盘detailmodel+listbarcode返回客户端
                            //modelList = CreatePalletListBarCodeForPallet(modelList);
                            if (CreatePalletListBarCodeForPallet(modelList, ref NewModelList, ref strError) == false)
                            {
                                model.HeaderStatus = "E";
                                model.Message = strError;
                                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                            }

                            modelList = NewModelList;
                        }
                    }
                    else //条码已经收货,查找库存数据
                    {
                        model.HeaderStatus = "E";
                        model.Message = "条码已经收货，不能添加组托！";
                        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                        //if (CheckPalletIsStock(SerialNo, ref lstStock, ref strError) == false)
                        //{
                        //    model.HeaderStatus = "E";
                        //    model.Message = strError;
                        //    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                        //}
                        //else
                        //{
                        //    GetStockPalletToPallet(ref modelList, lstStock);
                        //}

                    }
                }

                model.HeaderStatus = "S";
                model.ModelJson = modelList;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
            }
        }

        /// <summary>
        /// 根据条码或者托盘号获取托盘明细,如果托盘表没有，需要获取库存数据的
        /// 新建组托或者添加组托都是这个接口
        /// 此接口支持扫描外箱条码或者托盘条码或者序列号
        /// palletModel 1-新建组托 2-添加组托
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public string GetPalletDetailByPalletNoByCym(string BarCode, string PalletModel)
        {
            BaseMessage_Model<List<T_PalletDetailInfo>> model = new BaseMessage_Model<List<T_PalletDetailInfo>>();

            string SerialNo = string.Empty;
            string strError = string.Empty;
            string BarCodeType = string.Empty;
            T_OutBarCodeInfo outBarCodeModel = new T_OutBarCodeInfo();
            List<T_PalletDetailInfo> modelList = new List<T_PalletDetailInfo>();
            List<T_PalletDetailInfo> NewModelList = new List<T_PalletDetailInfo>();
            List<T_StockInfo> lstStock = new List<T_StockInfo>();

            try
            {
                if (string.IsNullOrEmpty(BarCode))
                {
                    model.HeaderStatus = "E";
                    model.Message = "客户端传来的托盘号或者条码为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                //验证条码正确性
                if (outBarCodeFunc.GetSerialNoByBarCode(BarCode, ref SerialNo, ref BarCodeType, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                //新建组托
                if (PalletModel == "1")
                {
                    //验证外箱条码或者托盘条码是否已经收货
                    if (outBarCodeFunc.CheckBaeCodeIsRecive(SerialNo, ref strError) == false)
                    {
                        model.HeaderStatus = "E";
                        model.Message = strError;
                        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                    }

                    //验证外箱条码是否已经组托
                    //if (_db.CheckSerialNoIsInPallet(SerialNo) > 0)
                    //{
                    //    model.HeaderStatus = "E";
                    //    model.Message = "该条码已经拼托！";
                    //    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                    //}

                    if (outBarCodeFunc.GetOutBarCodeInfoBySerialNo(SerialNo, ref outBarCodeModel, ref strError) == false)
                    {
                        model.HeaderStatus = "E";
                        model.Message = strError;
                        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                    }

                    modelList = CreatePalletForBarCode(outBarCodeModel);
                }

                model.HeaderStatus = "S";
                model.ModelJson = modelList;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
            }
        }


        /// <summary>
        /// 将库存托盘属性赋值托盘属性
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="stockList"></param>
        private void GetStockPalletToPallet(ref List<T_PalletDetailInfo> modelList, List<T_StockInfo> stockList)
        {
            foreach (var item in modelList)
            {
                var itemModel = stockList.Find(t => t.SerialNo == item.SerialNo);
                if (itemModel != null)
                {
                    item.BatchNo = itemModel.BatchNo;
                    item.Qty = itemModel.Qty;
                    item.WareHouseID = itemModel.WareHouseID;
                    item.HouseID = itemModel.HouseID;
                    item.AreaID = itemModel.AreaID;
                    item.PalletNo = itemModel.PalletNo;
                }
            }
        }

        /// <summary>
        /// 获取库存组托明细
        /// </summary>
        /// <param name="strPalletNo"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        private bool CheckPalletIsStock(string strPalletNo, ref List<T_StockInfo> modelList, ref string strErrMsg)
        {
            T_Stock_Func tfunc = new T_Stock_Func();

            return tfunc.GetStockInfoByPalletNo(strPalletNo, ref modelList, ref strErrMsg);
        }



        private void StockPalletToBarCodeInfo(List<T_PalletDetailInfo> modelList, ref List<T_PalletDetailInfo> NewModelList)
        {
            bool bSucc = false;
            string strError = string.Empty;
            T_OutBarCodeInfo BarCodeInfo = new T_OutBarCodeInfo();
            T_PalletDetailInfo model = new T_PalletDetailInfo();
            model.lstBarCode = new List<T_OutBarCodeInfo>();

            T_OutBarCode_Func _db = new T_OutBarCode_Func();

            foreach (var item in modelList)
            {
                bSucc = _db.GetModelBySql(ref BarCodeInfo, ref strError);
                if (bSucc == true)
                {
                    BarCodeInfo.BatchNo = item.BatchNo;
                    BarCodeInfo.Qty = item.Qty;
                    BarCodeInfo.WareHouseID = item.WareHouseID;
                    BarCodeInfo.HouseID = item.HouseID;
                    BarCodeInfo.AreaID = item.AreaID;
                    BarCodeInfo.PalletNo = item.PalletNo;
                }
                model.lstBarCode.Add(BarCodeInfo);
            }

            NewModelList.Add(model);

        }

        /// <summary>
        /// 根据条码类生成托盘类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private List<T_PalletDetailInfo> CreatePalletForBarCode(T_OutBarCodeInfo model)
        {
            T_PalletDetailInfo palletModel = new T_PalletDetailInfo();
            List<T_PalletDetailInfo> modelList = new List<T_PalletDetailInfo>();
            List<T_OutBarCodeInfo> lstBarCode = new List<T_OutBarCodeInfo>();
            palletModel.lstBarCode = new List<T_OutBarCodeInfo>();
            lstBarCode.Add(model);
            palletModel.lstBarCode = lstBarCode;
            modelList.Add(palletModel);
            return modelList;
        }

        /// <summary>
        /// 根据托盘条码，重新组织新的托盘类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool CreatePalletListBarCodeForPallet(List<T_PalletDetailInfo> modelList, ref List<T_PalletDetailInfo> NewModelList, ref string strError)
        {
            List<T_OutBarCodeInfo> lstBarCode = new List<T_OutBarCodeInfo>();
            T_PalletDetailInfo palletModel = new T_PalletDetailInfo();

            palletModel.lstBarCode = new List<T_OutBarCodeInfo>();

            if (_db.GetBarCodeByPalletDetailnewByCym(modelList, ref lstBarCode, ref strError) == false)
            {
                return false;
            }

            if (lstBarCode == null || lstBarCode.Count == 0)
            {
                strError = "根据托盘获取条码列表为空！";
                return false;
            }

            palletModel = modelList[0];
            palletModel.lstBarCode = lstBarCode;
            NewModelList.Add(palletModel);
            return true;
            //foreach (var item in modelList) 
            //{
            //    T_OutBarCodeInfo outBarCodeModel = new T_OutBarCodeInfo();
            //    outBarCodeModel.BatchNo = item.BatchNo;
            //    outBarCodeModel.Qty = item.Qty;                
            //    outBarCodeModel.PalletNo = item.PalletNo;
            //    outBarCodeModel.SerialNo = item.SerialNo;
            //    outBarCodeModel.BarCode = item.BarCode;
            //    outBarCodeModel.MaterialNo = item.MaterialNo;
            //    outBarCodeModel.MaterialDesc = item.MaterialDesc;
            //    outBarCodeModel.BatchNo = item.BatchNo;
            //    outBarCodeModel.SupPrdDate = item.SupPrdDate;
            //    outBarCodeModel.SupPrdBatch = item.SupPrdBatch;
            //    outBarCodeModel.ProductBatch = item.ProductBatch;
            //    outBarCodeModel.ProductDate = item.ProductDate;
            //    outBarCodeModel.EDate = item.EDate;
            //    outBarCodeModel.ErpVoucherNo = item.ErpVoucherNo;
            //    outBarCodeModel.StrongHoldCode = item.StrongHoldCode;
            //    outBarCodeModel.StrongHoldName = item.StrongHoldName;
            //    outBarCodeModel.CompanyCode = item.CompanyCode;
            //    outBarCodeModel.SupCode = item.SuppliernNo;
            //    outBarCodeModel.SupName = item.SupplierName;

            //    palletModel.MaterialNo = item.MaterialNo;
            //    palletModel.MaterialDoc = item.MaterialDesc;
            //    palletModel.PalletNo = item.PalletNo;
            //    palletModel.BatchNo = item.BatchNo;
            //    palletModel.SupPrdDate = item.SupPrdDate;
            //    palletModel.SupPrdBatch = item.SupPrdBatch;
            //    palletModel.ProductBatch = item.ProductBatch;
            //    palletModel.ProductDate = item.ProductDate;
            //    palletModel.EDate = item.EDate;
            //    palletModel.ErpVoucherNo = item.ErpVoucherNo;
            //    palletModel.StrongHoldCode = item.StrongHoldCode;
            //    palletModel.StrongHoldName = item.StrongHoldName;
            //    palletModel.CompanyCode = item.CompanyCode;
            //    palletModel.SuppliernNo = item.SuppliernNo;
            //    palletModel.SupplierName = item.SupplierName;

            //    palletModel.lstBarCode.Add(outBarCodeModel);
            //}


            //NewModelList.Add(palletModel);

            //return NewModelList;
        }
        #endregion

        #region 删除托盘，拆托
        /// <summary>
        /// 删除托盘或者托盘中的条码
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="PalletDetailJson"></param>
        /// <returns></returns>
        public string DeletePalletORBarCode(string UserJson, string PalletDetailJson)
        {
            BaseMessage_Model<T_PalletDetailInfo> messageModel = new BaseMessage_Model<T_PalletDetailInfo>();
            string strError = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(UserJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来用户JSON为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_PalletDetailInfo>>(messageModel);
                }

                if (string.IsNullOrEmpty(PalletDetailJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来托盘JSON为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_PalletDetailInfo>>(messageModel);
                }

                List<T_PalletDetailInfo> modelList = JSONHelper.JsonToObject<List<T_PalletDetailInfo>>(PalletDetailJson);

                //if (_db.CheckDeletePalletDetailBefore(modelList, ref strError) == false)
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = strError;
                //    return JSONHelper.ObjectToJson<BaseMessage_Model<T_PalletDetailInfo>>(messageModel);
                //}

                bool bSucc = false;
                bSucc = _db.DeletePalletORBarCode(modelList, ref strError);

                if (bSucc == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_PalletDetailInfo>>(messageModel);
                }
                else
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.Message = "删除成功！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_PalletDetailInfo>>(messageModel);
                }
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_PalletDetailInfo>>(messageModel);
            }
        }
        #endregion

        #region 拼箱和拆箱
        /// <summary>
        /// 拼箱获取库存数据
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public string GetOutBarCodeInfoByBox(string BarCode)
        {

            BaseMessage_Model<T_StockInfo> model = new BaseMessage_Model<T_StockInfo>();

            try
            {
                string strError = string.Empty;
                string SerialNo = string.Empty;
                string BarCodeType = string.Empty;

                T_StockInfo stockModel = new T_StockInfo();
                T_Stock_Func tfunc = new T_Stock_Func();
                //验证条码正确性
                if (outBarCodeFunc.GetSerialNoByBarCode(BarCode, ref SerialNo, ref BarCodeType, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                if (BarCodeType == "2")
                {
                    model.HeaderStatus = "E";
                    model.Message = "托盘条码不支持拼箱操作！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                //读取条码库存数据
                if (GetBarCodeIsStock(SerialNo, ref stockModel, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                if (tfunc.CheckOutStockStatus(ref strError, stockModel, "", "") == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }



                model.HeaderStatus = "S";
                model.ModelJson = stockModel;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
            }
        }


        /// <summary>
        /// 获取条码库存数据
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <param name="model"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        private bool GetBarCodeIsStock(string SerialNo, ref T_StockInfo model, ref string strErrMsg)
        {

            T_Stock_Func tfunc = new T_Stock_Func();

            return tfunc.GetStockByBarCode(SerialNo, ref model, ref strErrMsg);

        }


        /// <summary>
        /// 保存装箱或者拆箱数据,strOldBarCode拆零 ，strNewBarCode装的
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="OldBarCode"></param>
        /// <param name="NewBarCode"></param>
        /// <returns></returns>
        public string SaveBarCodeToStock(string UserJson, string strOldBarCode, string strNewBarCode, string PrintFlag)
        {
            BaseMessage_Model<T_StockInfo> messageModel = new BaseMessage_Model<T_StockInfo>();
            string NewSerialNo = string.Empty;
            T_StockInfo NewStockModel = new T_StockInfo();
            string strError = string.Empty;

            try
            {
                LogNet.LogInfo("SaveBarCodeToStock---" + strOldBarCode + "----" + strNewBarCode + "---" + PrintFlag);
                string strErrMsg = string.Empty;

                if (string.IsNullOrEmpty(UserJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来用户JSON为空！";
                    //return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                    return JsonConvert.SerializeObject(messageModel);
                }

                if (string.IsNullOrEmpty(strOldBarCode))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来原箱号条码JSON为空！";
                    //return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                    return JsonConvert.SerializeObject(messageModel);
                }

                //if (string.IsNullOrEmpty(strNewBarCode))
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = "客户端传来新箱号条码JSON为空！";
                //    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                //}

                UserModel userModel = JSONHelper.JsonToObject<UserModel>(UserJson);
                T_StockInfo OldModel = JSONHelper.JsonToObject<T_StockInfo>(strOldBarCode);
                T_StockInfo NewModel = JSONHelper.JsonToObject<T_StockInfo>(strNewBarCode);

                //if (!Print_DB.isConnected(userModel.PDAPrintIP))
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = "打印机连接失败！";
                //    //return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                //    return JsonConvert.SerializeObject(messageModel);
                //}

                if (OldModel == null)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来转换原箱号条码实体类为空！";
                    //return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                    return JsonConvert.SerializeObject(messageModel);
                }

                if (NewModel == null && OldModel.AmountQty == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "请输入拆零数量！";
                    //return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                    return JsonConvert.SerializeObject(messageModel);
                }


                if (NewModel == null && OldModel.AmountQty > OldModel.Qty)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "输入的拆零数量大于原箱数量！";
                    //return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                    return JsonConvert.SerializeObject(messageModel);
                }

                //if (NewModel == null && OldModel.AmountQty == OldModel.Qty) 
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = "输入的拆零数量等于原箱数量，不能拆零！";
                //    //return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                //    return JsonConvert.SerializeObject(messageModel);
                //}


                //if (OldModel.Barcode.Contains("@") == true)
                //{
                    T_Stock_Func sfunc = new T_Stock_Func();
                    T_StockInfo cmodel = new T_StockInfo();
                    //根据序列号查库存
                    if (sfunc.GetStockByBarCode(OutBarCode_DeCode.GetSubBarcodeSerialNo(OldModel.Barcode), ref cmodel, ref strError) == false)
                    {
                        messageModel.HeaderStatus = "E";
                        messageModel.Message = strError;                        
                        return JsonConvert.SerializeObject(messageModel);
                    }

                    if (cmodel.Qty < OldModel.AmountQty)
                    {
                        messageModel.HeaderStatus = "E";
                        messageModel.Message = "输入的拆零数量大于库存数量！";
                        //return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                        return JsonConvert.SerializeObject(messageModel);
                    }

                //}

                if (_db.SaveBarCodeToStock(userModel, OldModel, NewModel, ref NewSerialNo, ref strErrMsg) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strErrMsg;
                    //return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                    return JsonConvert.SerializeObject(messageModel);
                }
                else
                {
                    if (NewModel != null) //装箱，不需要打印和获取条码
                    {
                        messageModel.HeaderStatus = "S";
                        messageModel.Message = NewModel == null ? "拆箱操作成功！" : "装箱操作成功！";
                        messageModel.ModelJson = NewStockModel;
                        //return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                        return JsonConvert.SerializeObject(messageModel);
                    }

                    if (GetBarCodeIsStock(NewSerialNo, ref NewStockModel, ref strError) == false)
                    {
                        messageModel.HeaderStatus = "E";
                        messageModel.Message = "获取拆零库存条码失败！";
                    }
                    else
                    {
                        NewStockModel.Qty = OldModel.AmountQty;
                        NewStockModel.IsAmount = 2;
                        NewStockModel.lstJBarCode = OldModel.lstJBarCode;
                        if (PrintFlag == "1")//需要打印的
                        {
                            if (PDAPrintBarCodeForUnBoxAmount(NewStockModel.SerialNo, userModel.PDAPrintIP, ref strErrMsg) == false)
                            {
                                messageModel.HeaderStatus = "S";
                                messageModel.Message = NewModel == null ? "拆箱操作成功！" : "装箱操作成功！" + "打印拆零条码失败！" + strErrMsg;
                                messageModel.ModelJson = NewStockModel;
                            }
                            else
                            {
                                messageModel.HeaderStatus = "S";
                                messageModel.Message = NewModel == null ? "拆箱操作成功！" : "装箱操作成功！" + "打印拆零条码成功！";
                                messageModel.ModelJson = NewStockModel;
                            }
                        }
                        else
                        {
                            messageModel.HeaderStatus = "S";
                            messageModel.Message = NewModel == null ? "拆箱操作成功！" : "装箱操作成功！ ";
                            messageModel.ModelJson = NewStockModel;
                        }


                    }

                }
                //return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                return JsonConvert.SerializeObject(messageModel);
            }
            catch (Exception ex)
            {
                LogNet.LogInfo("SaveBarCodeToStock异常：" + ex.Message);
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                //return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(messageModel);
                return JsonConvert.SerializeObject(messageModel);
            }
        }


        #endregion

        #region 公司WMS组托用
        public string GetPalletDetailByVoucherNo(string VoucherNo)
        {
            BaseMessage_Model<List<T_PalletDetailInfo>> messageModel = new BaseMessage_Model<List<T_PalletDetailInfo>>();
            try
            {
                string strError = string.Empty;
                T_PalletDetailInfo model = new T_PalletDetailInfo();

                T_PalletDetail_DB tdb = new T_PalletDetail_DB();

                if (string.IsNullOrEmpty(VoucherNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来WMS单据编号为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(messageModel);
                }

                List<T_PalletDetailInfo> lstModel = new List<T_PalletDetailInfo>();
                lstModel = tdb.GetPalletDetailByVoucherNo(VoucherNo);

                if (lstModel == null || lstModel.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "该单据没有拼托数据！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(messageModel);
                }

                var newListModel = lstModel.GroupBy(t => t.PalletNo, t => t);
                List<T_PalletDetailInfo> returnlstModel = new List<T_PalletDetailInfo>();
                foreach (var item in newListModel.ToList())
                {
                    List<T_PalletDetailInfo> newSerialModel = item.ToList<T_PalletDetailInfo>();
                    T_PalletDetailInfo templstModel = new T_PalletDetailInfo();
                    templstModel.ID = newSerialModel[0].ID;
                    templstModel.HeaderID = newSerialModel[0].HeaderID;
                    templstModel.PalletNo = item.Key.ToString();
                    templstModel.ErpVoucherNo = newSerialModel[0].ErpVoucherNo;
                    List<T_OutBarCodeInfo> serialNoList = new List<T_OutBarCodeInfo>();

                    templstModel.lstBarCode = new List<T_OutBarCodeInfo>();
                    foreach (T_PalletDetailInfo Serials in newSerialModel)
                    {
                        T_OutBarCodeInfo tempSerialNo = new T_OutBarCodeInfo();
                        tempSerialNo.SerialNo = Serials.SerialNo;
                        tempSerialNo.RowNo = Serials.RowNo;
                        tempSerialNo.BarCode = Serials.BarCode;
                        tempSerialNo.MaterialDesc = Serials.MaterialDesc;
                        tempSerialNo.MaterialNoID = Serials.MaterialNoID;
                        tempSerialNo.MaterialNo = Serials.MaterialNo;
                        //add by cym 2017-9-19
                        tempSerialNo.Unit = Serials.Unit;

                        templstModel.ErpVoucherNo = Serials.ErpVoucherNo;

                        templstModel.lstBarCode.Add(tempSerialNo);
                    }

                    returnlstModel.Add(templstModel);
                }
                if (returnlstModel.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "该单据没有拼托数据！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(messageModel);
                }
                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = returnlstModel;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(messageModel);
                //return "";
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(messageModel);
            }
        }

        public string Del_PalletOrSerialNo(string palletNo, string serialNo)
        {
            BaseMessage_Model<Base_Model> messageModel = new BaseMessage_Model<Base_Model>();
            try
            {
                if (string.IsNullOrEmpty(palletNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "参数错误：托盘号为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<Base_Model>>(messageModel);
                }
                string errMsg = String.Empty;
                T_PalletDetail_DB db = new T_PalletDetail_DB();
                if (db.Del_PalletOrSerialNo(palletNo, serialNo, ref errMsg))
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.Message = "删除成功！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<Base_Model>>(messageModel);
                }
                else
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = errMsg;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<Base_Model>>(messageModel);
                }
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<Base_Model>>(messageModel);

            }
        }
        #endregion

        /// <summary>
        /// 根据filter获取托盘数据
        /// </summary>
        /// <param name="Filter"></param>
        /// <param name="modelList"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool GetPalletByPalletNo(string Filter, ref List<T_PalletDetailInfo> modelList, ref string strError)
        {
            return base.GetModelListByFilter(ref modelList, ref strError, "", Filter);
        }

        public bool PDAPrintBarCodeForUnBoxAmount(string serialno, string ip, ref string ErrMsg)
        {
            Print_DB pd = new Print_DB();
            return pd.PrintLpkApart(serialno, ip, ref ErrMsg);
        }

        public string DeletePalletByErpVoucherNo(string ErpVoucherNo, string PalletType)
        {
            BaseMessage_Model<T_PalletDetailInfo> messageModel = new BaseMessage_Model<T_PalletDetailInfo>();
            try
            {
                string strError = string.Empty;

                if (string.IsNullOrEmpty(ErpVoucherNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来ERP单据号为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_PalletDetailInfo>>(messageModel);
                }

                if (string.IsNullOrEmpty(PalletType))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来托盘类型为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_PalletDetailInfo>>(messageModel);
                }

                T_PalletDetail_DB tfunc = new T_PalletDetail_DB();
                bool bSucc = tfunc.DeletePalletByErpVoucherNo(ErpVoucherNo, PalletType, ref strError);

                messageModel.HeaderStatus = "S";
                messageModel.Message = strError;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_PalletDetailInfo>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_PalletDetailInfo>>(messageModel);

            }

        }

        public string GetPalletDetailMessageADF(string ErpVoucherNo)
        {
            BaseMessage_Model<List<T_PalletDetailInfo>> messageModel = new BaseMessage_Model<List<T_PalletDetailInfo>>();
            try
            {
                string strError = string.Empty;

                if (string.IsNullOrEmpty(ErpVoucherNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来ERP单据号为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(messageModel);
                }


                T_PalletDetail_DB tfunc = new T_PalletDetail_DB();
                List<T_PalletDetailInfo> modelList = tfunc.GetPalletDetailMessage(ErpVoucherNo);
                if (modelList == null || modelList.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "该单号还没有组托";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(messageModel);
                }


                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = modelList;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(messageModel);

            }

        }


        public string CheckSerialnoInPallet(string serialNo)
        {
            BaseMessage_Model<Base_Model> messageModel = new BaseMessage_Model<Base_Model>();
            try
            {
                if (string.IsNullOrEmpty(serialNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "参数错误：参数为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<Base_Model>>(messageModel);
                }
                string errMsg = String.Empty;
                T_PalletDetail_DB db = new T_PalletDetail_DB();
                string palletno = db.CheckSerialnoInPallet(serialNo);
                if (palletno != null)
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.Message = palletno;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<Base_Model>>(messageModel);
                }
                else
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = errMsg;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<Base_Model>>(messageModel);
                }
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<Base_Model>>(messageModel);

            }
        }





        /// <summary>
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string GetCPInstockMes(string json)
        {
            BaseMessage_Model<List<T_PalletDetailInfo>> model = new BaseMessage_Model<List<T_PalletDetailInfo>>();
            T_PalletDetail_DB db = new T_PalletDetail_DB();
            List<T_PalletDetailInfo> modelList = new List<T_PalletDetailInfo>();

            T_PalletDetailInfo palletModel = new T_PalletDetailInfo();
            List<T_OutBarCodeInfo> lstBarCode = new List<T_OutBarCodeInfo>();
            palletModel.lstBarCode = new List<T_OutBarCodeInfo>();

            try
            {
                if (string.IsNullOrEmpty(json))
                {
                    model.HeaderStatus = "E";
                    model.Message = "客户端传来的托盘号或者条码为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                //验证外箱条码是否重复组托
                if (!db.CheckSerialNoIsMore(json))
                {
                    model.HeaderStatus = "E";
                    model.Message = "该条码重复组托，请先去拆托！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                //验证外箱条码是否已经入库
                if (!db.CheckSerialNoIsInStock(json))
                {
                    model.HeaderStatus = "E";
                    model.Message = "该条码已经入库！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                //add by cym 2018-8-3 验证外箱条码是否已经完工入库
                if (db.CheckSerialNoIsTasktransForWGRK(json) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = "该条码已经完工入库！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                modelList = db.GetCPInstockMes(json);
                if (modelList == null || modelList.Count == 0)
                {
                    model.HeaderStatus = "E";
                    model.Message = "该成品还没有组托";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }
                else
                {
                    string strError = "";
                    if (_db.GetBarCodeByPalletDetailnewByCym(modelList, ref lstBarCode, ref strError) == false)
                    {
                        model.HeaderStatus = "E";
                        model.Message = "获取条码失败！";
                        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                    }

                    if (lstBarCode == null || lstBarCode.Count == 0)
                    {
                        model.HeaderStatus = "E";
                        model.Message = "根据托盘获取条码列表为空！";
                        return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                    }

                    palletModel = modelList[0];
                    palletModel.lstBarCode = lstBarCode;
                    List<T_PalletDetailInfo> modelListnew = new List<T_PalletDetailInfo>();
                    modelListnew.Add(palletModel);

                    model.HeaderStatus = "S";
                    model.ModelJson = modelListnew;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
            }
        }

        #region ymh装车卸车
        /// <summary>
        /// 根据托盘条码获取托盘明细
        /// </summary>
        public string GetPalletInfoByPalletNo(string PalletNo)
        {
            BaseMessage_Model<List<T_OutBarCodeInfo>> model = new BaseMessage_Model<List<T_OutBarCodeInfo>>();
            try
            {
                T_OutBarCode_Func outBarCodeFunc = new T_OutBarCode_Func();
                //List<T_OutBarCodeInfo> modelList = new List<T_OutBarCodeInfo>();
                List<T_OutBarCodeInfo> newmodelList = new List<T_OutBarCodeInfo>();
                string strError = "";
                if (outBarCodeFunc.GetOutBarCodeByPalletNoforCar(PalletNo, ref newmodelList, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(model);
                }
                else
                {
                    //modelList = (from st in newmodelList
                    //             group st by new
                    //             {
                    //                 st.ErpVoucherNo
                    //             }
                    //        into temp
                    //             select new T_OutBarCodeInfo()
                    //             {
                    //                 ErpVoucherNo = temp.Key.ErpVoucherNo,
                    //                 CusName = temp.Key.CusName,
                    //                 Qty = temp.Sum(a => a.Qty),
                    //                 OutCount = temp.Count()
                    //             }).ToList();
                    model.HeaderStatus = "S";
                    model.ModelJson = newmodelList;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(model);
                }
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(model);
            }
        }




        #endregion
    }
}
