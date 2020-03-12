using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILWeb.Login.User;
using BILBasic.JSONUtil;

namespace BILWeb.OutBarCode
{

    public partial class T_OutBarCode_Func : TBase_Func<T_OutBarcode_DB, T_OutBarCodeInfo>
    {

        protected override bool CheckModelBeforeSave(T_OutBarCodeInfo model, ref string strError)
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
            return "条码";
        }

        protected override T_OutBarCodeInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

        
        /// <summary>
        /// 获取条码信息(包材接收)
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public string GetOutBarCodeInfobyymh(string BarCode)
        {
            //return "";
            BaseMessage_Model<T_OutBarCodeInfo> model = new BaseMessage_Model<T_OutBarCodeInfo>();
            try
            {
                bool bSucc = false;

                string strError = string.Empty;
                string SerialNo = string.Empty;
                string BarCodeType = string.Empty;

                //验证条码正确性
                if (GetSerialNoByBarCode(BarCode, ref SerialNo, ref BarCodeType, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(model);
                }

                if (BarCodeType == "2")
                {
                    model.HeaderStatus = "E";
                    model.Message = "该条码是托盘条码，必须是外箱条码，请重新扫描！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(model);
                }

                T_OutBarcode_DB _db = new T_OutBarcode_DB();
                bSucc = _db.CheckSerialNo(SerialNo, ref strError);

                if (bSucc == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(model);
                }
               
                T_OutBarCodeInfo BarCodeInfo = new T_OutBarCodeInfo();
                BarCodeInfo.SerialNo = SerialNo;

                BarCodeInfo = _db.GetModelBySql(BarCodeInfo);
                if (BarCodeInfo == null)
                {
                    model.HeaderStatus = "E";
                    model.Message = "您扫描的条码不存在！请确认是否已经打印！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(model);
                }

                if (_db.CheckSerialNobyymh(SerialNo, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(model);
                }

                model.HeaderStatus = "S";
                model.ModelJson = BarCodeInfo;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(model);
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(model);
            }
        }



        /// <summary>
        /// 获取条码信息，并验证是否已经收货
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public string GetOutBarCodeInfo(string BarCode)
        {
            //return "";
            BaseMessage_Model<T_OutBarCodeInfo> model = new BaseMessage_Model<T_OutBarCodeInfo>();
            try
            {
                bool bSucc = false;

                string strError = string.Empty;
                string SerialNo = string.Empty;
                string BarCodeType = string.Empty;

                //add by cym 2018-10-23 齐套扫描的时候，可以扫描内箱条码复核！！！ 
                //验证条码正确性
                if (GetSerialNoByBarCodeQiTao(BarCode, ref SerialNo, ref BarCodeType, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(model);
                }

                if (BarCodeType == "2")
                {
                    model.HeaderStatus = "E";
                    model.Message = "该条码是托盘条码，必须是外箱条码，请重新扫描！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(model);
                }

                T_OutBarcode_DB _db = new T_OutBarcode_DB();
                bSucc = _db.CheckSerialNo(SerialNo, ref strError);

                if (bSucc == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(model);
                }

                T_OutBarCodeInfo BarCodeInfo = new T_OutBarCodeInfo();
                BarCodeInfo.SerialNo = SerialNo;

                BarCodeInfo = _db.GetModelBySql(BarCodeInfo);
                if (BarCodeInfo == null)
                {
                    model.HeaderStatus = "E";
                    model.Message = "您扫描的条码不存在！请确认是否已经打印！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(model);
                }

                model.HeaderStatus = "S";
                model.ModelJson = BarCodeInfo;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(model);
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(model);
            }
        }

        /// <summary>
        /// 验证是否是外箱条码或者托盘条码
        /// </summary>
        /// <param name="BarCode"></param>
        /// <param name="SerialNo"></param>
        /// <param name="strError"></param>
        /// <returns>返回序列号和条码类型</returns>
        public   bool GetSerialNoByBarCode(string BarCode,   ref string SerialNo,ref string BarCodeType,ref string strError)
        {
            bool deCodeResult = false;

            //如果没有@分隔符，就认为是序列号
            if (OutBarCode_DeCode.InvalidBarcode(BarCode) == false)
            {
                BarCodeType = OutBarCode_DeCode.GetSubBarcodeType(BarCode);

                if (BarCodeType=="3" ) 
                {
                    BarCodeType = "1";
                    SerialNo = OutBarCode_DeCode.GetSubBarcodeSerialNo(BarCode);
                    deCodeResult = true;
                }
                else if (BarCodeType == "4")
                {
                    BarCodeType = "2";
                    SerialNo = BarCode;
                    deCodeResult = true;
                }
                else 
                {
                    deCodeResult = false;
                    strError = "您扫描的不是外箱条码或者托盘条码，请确认！";
                }
                //if (BarCode.Contains("P"))
                //{
                //    BarCodeType = "2";
                //}
                //else 
                //{
                //    BarCodeType = "1";
                //}                
                //SerialNo = BarCode;
                //deCodeResult = true;
            }
            else
            {
                if (OutBarCode_DeCode.GetBarcodeType(BarCode) == "0")
                {
                    deCodeResult = false;
                    strError = "您扫描的不是外箱条码或者托盘条码，请确认！";
                }
                else
                {
                    BarCodeType = OutBarCode_DeCode.GetBarcodeType(BarCode);
                    deCodeResult = true;
                    SerialNo = OutBarCode_DeCode.GetSerialNo(BarCode);
                }
            }

            return deCodeResult;
        }

        /// <summary>
        /// 验证是否是外箱条码或者内箱条码
        /// </summary>
        /// <param name="BarCode"></param>
        /// <param name="SerialNo"></param>
        /// <param name="strError"></param>
        /// <returns>返回序列号和条码类型</returns>
        public bool GetSerialNoByBarCodeQiTao(string BarCode, ref string SerialNo, ref string BarCodeType, ref string strError)
        {
            bool deCodeResult = false;

            //如果没有@分隔符，就认为是序列号
            if (OutBarCode_DeCode.InvalidBarcode(BarCode) == false)
            {
                if (BarCode.Contains("P"))
                {
                    BarCodeType = "2";
                }
                else
                {
                    BarCodeType = "1";
                }
                SerialNo = BarCode;
                deCodeResult = true;
            }
            else
            {
                BarCodeType = OutBarCode_DeCode.GetBarcodeType(BarCode);
                deCodeResult = true;
                SerialNo = OutBarCode_DeCode.GetSerialNo(BarCode);
            }

            return deCodeResult;
        }

        public   bool GetMaterialNoByBarCode(string BarCode,   ref string MaterialNo,ref string strError)
        {
            bool deCodeResult = false;

            //如果没有@分隔符，就认为是物料号
            if (OutBarCode_DeCode.InvalidBarcode(BarCode) == false)
            {
                MaterialNo = BarCode;
                deCodeResult = true;
            }
            else
            {
                if (OutBarCode_DeCode.GetBarcodeType(BarCode) == "0")
                {
                    deCodeResult = false;
                    strError = "您扫描的不是外箱条码，请确认！";
                }
                else
                { 
                    deCodeResult = true;
                    MaterialNo = OutBarCode_DeCode.GetMaterialNo(BarCode);
                }
            }

            return deCodeResult;
        }

        public bool GetEANByBarCode(string BarCode, ref string EAN, ref string strError)
        {
            bool deCodeResult = false;
            if (EAN.Contains("@"))
            {
                if (OutBarCode_DeCode.GetBarcodeType(BarCode) == "0")
                {
                    deCodeResult = false;
                    strError = "您扫描的不是外箱条码，请确认！";
                }
                else
                {
                    deCodeResult = true;
                    EAN = OutBarCode_DeCode.GeEAN(BarCode);
                }
            }
            else
            {
                deCodeResult = true;
                EAN = BarCode;
            }
            return deCodeResult;
        }

        /// <summary>
        /// barcodetype 1-批次 2-序列号
        /// </summary>
        /// <param name="BarCode"></param>
        /// <param name="BatchNo"></param>
        /// <param name="BarCodeType"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool GetBatchNoByBarCode(string BarCode, ref string BatchNo,ref string BarCodeType, ref string strError)
        {
            bool deCodeResult = false;

            //如果没有@分隔符，就认为是厂内批次号
            if (OutBarCode_DeCode.InvalidBarcode(BarCode) == false)
            {
                BatchNo = BarCode;
                BarCodeType = "1";
                deCodeResult = true;
            }
            else
            {
                if (OutBarCode_DeCode.GetBarcodeType(BarCode) == "0")
                {
                    deCodeResult = false;
                    strError = "您扫描的不是外箱条码，请确认！";
                }
                else
                {
                    deCodeResult = true;
                    BarCodeType = "2";
                    BatchNo = OutBarCode_DeCode.GetSerialNo(BarCode);
                }
            }

            return deCodeResult;
        }

        /// <summary>
        /// 验证条码是否已经收货
        /// </summary>
        /// <param name="BarCode"></param>
        /// <param name="model"></param>
        /// <param name="strErrMsg"></param>
        /// <returns>如果已经收货返回FALSE,没有收货返回TRUE和SERIALNO</returns>
        public bool CheckBaeCodeIsRecive( string SerialNo, ref string strErrMsg)
        {
            try
            {
                bool bSucc = false;

                //string strError = string.Empty;

                T_OutBarcode_DB _db = new T_OutBarcode_DB();
                bSucc = _db.CheckSerialNo(SerialNo, ref strErrMsg);

                if (bSucc == false)
                {
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
        /// 根据序列号获取条码信息
        /// </summary>
        /// <param name="BarCode"></param>
        /// <param name="model"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        public bool GetOutBarCodeInfoBySerialNo(string SerialNo, ref T_OutBarCodeInfo model, ref string strErrMsg)
        {
            try
            {
                string strError = string.Empty;                             

                T_OutBarcode_DB _db = new T_OutBarcode_DB();                
                model.SerialNo = SerialNo;
                model = _db.GetModelBySql(model);
                if (model == null)
                {
                    strErrMsg = "您扫描的条码不存在！请确认是否已经打印！";
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

        

        /// <summary>
        /// 根据托盘号查找条码
        /// </summary>
        /// <param name="PalletNo"></param>
        /// <returns></returns>
        public bool GetOutBarCodeByPalletNo(string PalletNo,ref List<T_OutBarCodeInfo> modelList,ref string strError)
        {
            T_OutBarcode_DB _db = new T_OutBarcode_DB();

            //modelList = _db.GetOutBarCodeByPalletNoforCar(PalletNo);
            modelList = _db.GetOutBarCodeByPalletNo(PalletNo);

            if (modelList == null || modelList.Count == 0) 
            {
                strError = "根据托盘号获取条码列表为空！";
                return false;
            }

            return true;
        }

        public bool GetOutBarCodeByPalletNoforCar(string PalletNo, ref List<T_OutBarCodeInfo> modelList, ref string strError)
        {
            T_OutBarcode_DB _db = new T_OutBarcode_DB();

            modelList = _db.GetOutBarCodeByPalletNoforCar(PalletNo);

            if (modelList == null || modelList.Count == 0)
            {
                strError = "根据托盘号获取条码列表为空！";
                return false;
            }

            return true;
        }

        public string GetErpBarCode(string strBarCode) 
        {
            BaseMessage_Model<T_OutBarCodeInfo> messageModel = new BaseMessage_Model<T_OutBarCodeInfo>();
            try
            {
                string strError = string.Empty;
                string SerialNo = string.Empty;
                string BarCodeType = string.Empty;

                if (string.IsNullOrEmpty(strBarCode))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "传入条码参数为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(messageModel);
                }

                //验证条码正确性
                if (GetSerialNoByBarCode(strBarCode, ref SerialNo, ref BarCodeType, ref strError) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(messageModel);
                }

                T_OutBarcode_DB _db = new T_OutBarcode_DB();
                T_OutBarCodeInfo model = _db.GetErpBarCode(SerialNo);

                if (model == null) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "您扫描的条码不存在！请确认是否已经打印！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = model;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(messageModel);

            }
            catch (Exception ex) 
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_OutBarCodeInfo>>(messageModel);
            }

        }
    }
}