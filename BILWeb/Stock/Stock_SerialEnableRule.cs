using BILBasic.Language;
using BILWeb.OutBarCode;
using BILWeb.StrategeRuleAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;


namespace BILWeb.Stock
{
    public class Stock_SerialEnableRule : StrategeRuleAll<T_StockInfo>
    {

        T_OutBarCode_Func outBarCodeFunc = new T_OutBarCode_Func();
        //启用序列号管理
        public override bool GetStockByBarCode(T_StockInfo model, ref List<T_StockInfo> modelList, ref string strError)
        {
            string strSerialNo = string.Empty;
            string BarCodeType = string.Empty;
            int iWareHouseID = 0;
            T_Stock_Func sfunc = new T_Stock_Func();
            T_StockInfo newModel = new T_StockInfo();
            List<T_StockInfo> newModelList = new List<T_StockInfo>();
            T_Stock_DB db = new T_Stock_DB();
            T_OutBarCode_Func toc = new T_OutBarCode_Func();
            T_OutBarCodeInfo outBarCodeModel = new T_OutBarCodeInfo();
            T_OutBarCodeInfo JBarCodeModel = new T_OutBarCodeInfo();
            T_OutBarcode_DB odb = new T_OutBarcode_DB();

            //if (model.Barcode.Contains("@") == true)
            //{
            //    strSerialNo = OutBarCode_DeCode.GetEndSerialNo(model.Barcode);
            //    //根据序列号查库存
            //    if (sfunc.GetStockByBarCode(strSerialNo, ref newModel, ref strError) == false)
            //    {
            //        return false;
            //    }
                
            //}
            //else 
            //{
            //    iWareHouseID = model.WareHouseID;

            //    newModelList = db.GetStockByWHBarCode(model);
            //    if (newModelList == null || newModelList.Count==0)
            //    {
            //        strError = Language_CHS.StockIsEmpty;
            //        return false;
            //    }
            //    if (string.IsNullOrEmpty(model.ErpVoucherNo))
            //    {
            //        newModelList = newModelList.Where(t => t.TaskDetailesID == 0).ToList();
            //    }
            //    else 
            //    {
            //        newModelList = newModelList.Where(t => t.TaskDetailesID > 0).ToList();
            //    }
                
            //}
            
            if ((model.Barcode.Length == 13 || model.Barcode.Length == 14) && model.ScanType != 2) //69码，复核的时候用
            {
                iWareHouseID = model.WareHouseID;

                newModelList = db.GetStockByWHBarCode(model);
                if (newModelList == null || newModelList.Count == 0)
                {
                    strError = Language_CHS.StockIsEmpty;
                    return false;
                }
                if (string.IsNullOrEmpty(model.ErpVoucherNo))
                {
                    newModelList = newModelList.Where(t => t.TaskDetailesID == 0).ToList();
                }
                else
                {
                    newModelList = newModelList.Where(t => t.TaskDetailesID > 0).ToList();
                }

                modelList = newModelList;

            }
            else 
            {
                if (outBarCodeFunc.GetSerialNoByBarCode(model.Barcode, ref strSerialNo, ref BarCodeType, ref strError) == false)
                {
                    return false;
                }

                if (BarCodeType == "1")
                {
                    //根据序列号查库存
                    if (sfunc.GetStockByBarCode(strSerialNo, ref newModel, ref strError) == false)
                    {
                        return false;
                    }
                    //扫描到的是外箱
                    newModel.IsPalletOrBox = 1;
                }
                else if (BarCodeType == "2")
                {
                    if (sfunc.GetStockInfoByPalletNo(strSerialNo, ref modelList, ref strError) == false)
                    {
                        return false;
                    }

                    

                    //扫描到的是托盘
                    modelList.ForEach(t => t.IsPalletOrBox = 2);
                    List<T_StockInfo> HStockList = modelList.Where(t => t.BarCodeType == 5).ToList();

                    if (HStockList != null && HStockList.Count > 0) 
                    {
                        foreach (var item in HStockList)
                        {
                            item.lstHBarCode = GetNewListStock(item.Barcode,item);//odb.GetBarCodeOutAll(model.Barcode);
                            item.lstHBarCode.ForEach(t => t.WareHouseID = item.WareHouseID);
                            item.lstHBarCode.ForEach(t => t.HouseID = item.HouseID);
                            item.lstHBarCode.ForEach(t => t.AreaID = item.AreaID);
                        }
                    }

                }

                if (!string.IsNullOrEmpty(model.JBarCode))
                {
                    string strJSerialNo = string.Empty;
                    if (OutBarCode_DeCode.GetSubBarcodeType(model.JBarCode) != "1")
                    {
                        strError = "您扫描的不是J箱条码！";
                        return false;
                    }
                    strJSerialNo = OutBarCode_DeCode.GetSubBarcodeSerialNo(model.JBarCode);
                    if (toc.GetOutBarCodeInfoBySerialNo(strJSerialNo, ref JBarCodeModel, ref strError) == false)
                    {
                        return false;
                    }

                    if (odb.GetJBarCodeIsScan(strJSerialNo) > 0) 
                    {
                        strError = "J箱条码已经扫描，不能重复扫描！";
                        return false;
                    }

                    if (JBarCodeModel.fserialno.Substring(0, 1) == "2") //J箱对应中盒
                    {
                        //根据中盒找外箱
                        if (toc.GetOutBarCodeInfoBySerialNo(JBarCodeModel.fserialno, ref JBarCodeModel, ref strError) == false)
                        {
                            return false;
                        }
                    }

                    if (model.Barcode.CompareTo(JBarCodeModel.fserialno) != 0)
                    {
                        strError = "外箱条码不包含J箱码！";
                        return false;
                    }
                    else
                    {
                        //J箱码转换库存类
                        modelList.Add(GetAmoutInnerStock(newModel, JBarCodeModel));
                    }
                }

                //外箱条码需要查看条码是否是混箱
                if (BarCodeType == "1" && string.IsNullOrEmpty(model.JBarCode))
                {
                    if (toc.GetOutBarCodeInfoBySerialNo(strSerialNo, ref outBarCodeModel, ref strError) == false)
                    {
                        return false;
                    }

                    if (outBarCodeModel.BarcodeType == 5) //混箱
                    {
                        newModel.BarCodeType = 5;

                        newModel.lstHBarCode = GetNewListStock(model.Barcode,newModel);//odb.GetBarCodeOutAll(model.Barcode);
                        newModel.lstHBarCode.ForEach(t => t.WareHouseID = newModel.WareHouseID);
                        newModel.lstHBarCode.ForEach(t => t.HouseID = newModel.HouseID);
                        newModel.lstHBarCode.ForEach(t => t.AreaID = newModel.AreaID);
                    }
                    else//不是混箱
                    {
                        newModel.BarCodeType = outBarCodeModel.BarcodeType;
                        //查看是否J箱
                        newModel.lstBarCode = odb.GetBarCodeOutAll(model.Barcode);
                        //newModel.lstBarCode.ForEach(t => t.WareHouseID = newModel.WareHouseID);
                        //newModel.lstBarCode.ForEach(t => t.HouseID = newModel.HouseID);
                        //newModel.lstBarCode.ForEach(t => t.AreaID = newModel.AreaID);

                        if (newModel.lstBarCode == null || newModel.lstBarCode.Count == 0)
                        {
                            newModel.ISJ = "2";//不是J箱
                        }
                        else if (newModel.lstBarCode[0].BarcodeType == 1)
                        {
                            newModel.ISJ = "1";//是J想
                        }
                        else if (newModel.lstBarCode[0].BarcodeType == 2) //找到中盒
                        {
                            //根据中盒找J箱
                            newModel.lstBarCode = odb.GetBarCodeOutAll(newModel.lstBarCode[0].BarCode);
                            //newModel.lstBarCode.ForEach(t => t.WareHouseID = newModel.WareHouseID);
                            //newModel.lstBarCode.ForEach(t => t.HouseID = newModel.HouseID);
                            //newModel.lstBarCode.ForEach(t => t.AreaID = newModel.AreaID);

                            if (newModel.lstBarCode == null || newModel.lstBarCode.Count == 0)
                            {
                                newModel.ISJ = "2";//不是J箱
                            }
                            else if (newModel.lstBarCode[0].BarcodeType == 1)
                            {
                                newModel.ISJ = "1";//是J箱
                            }
                        }
                        newModel.lstBarCode = null;
                    }
                    modelList.Add(newModel);
                }
            }

            

            

            ////整箱或者零数发货
            //if (model.ScanType == 2 || model.ScanType == 3)
            //{
            //    if (model.Barcode.Contains("@") == true)
            //    {
            //        modelList.Add(newModel);
            //    }
            //    else 
            //    {
            //        modelList.AddRange(newModelList);
            //    }                
            //}

            ////整托发货
            //if (model.ScanType == 1 && model.Barcode.Contains("@") == true)
            //{
            //    if (string.IsNullOrEmpty(newModel.PalletNo))
            //    {
            //        strError = Language_CHS.StockPEmpty;
            //        return false;
            //    }

            //    if (sfunc.GetStockInfoByPalletNo(newModel.PalletNo, ref modelList, ref strError) == false)
            //    {
            //        return false;
            //    }
            //}

            decimal SumQty = modelList.Sum(t1 => t1.Qty).ToDecimal();
            modelList.ForEach(t => t.PalletQty = SumQty);

            return true;

        }

        public List<T_StockInfo> GetNewListStock(string strBarCode, T_StockInfo newModel) //, T_StockInfo stockModel
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
                model.BatchNo = item.BatchNo;
                model.WarehouseNo = newModel.WarehouseNo;
                model.ErpVoucherNo = newModel.ErpVoucherNo;
                model.IsPalletOrBox = newModel.IsPalletOrBox;
                model.PalletNo = newModel.PalletNo;
                //model.AreaID = stockModel.AreaID;
                lstStock.Add(model);
            }
            return lstStock;
        }

        private static T_StockInfo GetAmoutInnerStock(T_StockInfo stockModel, T_OutBarCodeInfo innerBarCodeModel)
        {
            T_StockInfo t_stock = new T_StockInfo();
            t_stock = stockModel;
            t_stock.Barcode = innerBarCodeModel.BarCode;
            t_stock.SerialNo = innerBarCodeModel.SerialNo;
            t_stock.BarCodeType = innerBarCodeModel.BarcodeType;
            t_stock.Qty = innerBarCodeModel.Qty.ToDecimal();            
            t_stock.WareHouseID = stockModel.WareHouseID;
            t_stock.Status = 3;
            return t_stock;
        }

    }
}
