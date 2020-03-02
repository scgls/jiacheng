using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.Stock;
using BILWeb.OutBarCode;
using BILBasic.JSONUtil;
using BILWeb.SyncService;
using BILBasic.User;
using BILBasic.Interface;

namespace BILWeb.Quality
{
    public partial class T_Quality_Func : TBase_Func<T_Quality_DB, T_QualityInfo>
    {
        T_Quality_DB _db = new T_Quality_DB();
        protected override bool CheckModelBeforeSave(T_QualityInfo model, ref string strError)
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
            return "检验单";
        }

        
        protected override T_QualityInfo GetModelByJson(string strJson)
        {

            T_QualityInfo model = JSONHelper.JsonToObject<T_QualityInfo>(strJson);


            return JSONHelper.JsonToObject<T_QualityInfo>(strJson);
        }

        /// <summary>
        /// 质检扫描
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public string GetOutBarCodeInfoForQuanADF(string BarCode) 
        {
            BaseMessage_Model<T_StockInfo> model = new BaseMessage_Model<T_StockInfo>();

            try
            {
                string strError = string.Empty;
                string SerialNo = string.Empty;
                string BarCodeType = string.Empty;


                T_OutBarCodeInfo BarCodeInfo = new T_OutBarCodeInfo();               
                T_OutBarCode_Func tfunc = new T_OutBarCode_Func();

                //验证条码正确性
                if (tfunc.GetSerialNoByBarCode(BarCode, ref SerialNo, ref BarCodeType, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                if (BarCodeType == "2")
                {
                    model.HeaderStatus = "E";
                    model.Message = "托盘条码不支持整托操作！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                T_StockInfo stockModel = new T_StockInfo();
                BarCodeInfo.SerialNo = SerialNo;
                //读取条码库存数据
                if (GetBarCodeIsStock(BarCodeInfo.SerialNo, ref stockModel, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                if (stockModel.Status != 1) 
                {
                    model.HeaderStatus = "E";
                    model.Message = "该条码不是待检状态，不能取样！";
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
        /// 质检扫描--------------add by cym 2017-12-14
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public string GetOutBarCodeInfoForQuanADF_Product(string BarCode)
        {
            BaseMessage_Model<T_StockInfo> model = new BaseMessage_Model<T_StockInfo>();

            try
            {
                string strError = string.Empty;
                string SerialNo = string.Empty;
                string BarCodeType = string.Empty;


                T_OutBarCodeInfo BarCodeInfo = new T_OutBarCodeInfo();
                T_OutBarCode_Func tfunc = new T_OutBarCode_Func();

                //验证条码正确性
                if (tfunc.GetSerialNoByBarCode(BarCode, ref SerialNo, ref BarCodeType, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                if (BarCodeType == "2")
                {
                    model.HeaderStatus = "E";
                    model.Message = "托盘条码不支持整托操作！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                T_StockInfo stockModel = new T_StockInfo();
                BarCodeInfo.SerialNo = SerialNo;
                //读取条码库存数据
                if (GetBarCodeIsStock(BarCodeInfo.SerialNo, ref stockModel, ref strError) == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                //update by cym 2018-1-4
                if (stockModel.Status != 1)
                {
                    model.HeaderStatus = "E";
                    model.Message = "该条码不是待检状态，不能取样！";
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


        protected override bool Sync(T_QualityInfo model,ref string strErrMsg)
        {
            ParamaterField_Func tfunc = new ParamaterField_Func();
            return tfunc.Sync(model.StockType, "", model.ErpVoucherNo, model.VoucherType, ref strErrMsg, "ERP", -1, null);
        }

        /// <summary>
        /// 请求未审核的检验结果
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool GetT_AllQualityList(ref List<T_QualityInfo> modelList, ref string strError)
        {
            string strFilter = "isnull(Erpstatuscode,'N') = 'N'";

            return base.GetModelListByFilter(ref modelList,ref strError,"",strFilter);

        }

        protected override List<T_QualityInfo> GetModelListByJson(string UserJson, string ModelListJson)
        {
            List<T_QualityInfo> modelList = JSONHelper.JsonToObject<List<T_QualityInfo>>(ModelListJson);
           

            return modelList;
        }

        public int GetQualityStatusByTaskNo(string strTaskNo, int MaterialNoID, string BatchNo) 
        {
            T_Quality_DB _db = new T_Quality_DB();
            return _db.GetQualityStatusByTaskNo(strTaskNo, MaterialNoID, BatchNo);
        }

        //在库检，生成检验单
        public string CreateQualityForStock(string UserJson, string ModelJson) 
        {
            BaseMessage_Model<T_StockInfo> model = new BaseMessage_Model<T_StockInfo>();
            try
            {
                string strError = string.Empty;
                string strErrorWMS = string.Empty;
                string QualityNo = string.Empty;

                if (string.IsNullOrEmpty(UserJson)) 
                {
                    model.HeaderStatus = "E";
                    model.Message = "客户端传来用户JSON为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                if (string.IsNullOrEmpty(ModelJson))
                {
                    model.HeaderStatus = "E";
                    model.Message = "客户端传来业务JSON为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                UserModel userModel = JSONHelper.JsonToObject<UserModel>(UserJson);

                List<T_StockInfo> modelList = JSONHelper.JsonToObject<List<T_StockInfo>>(ModelJson);

                if (modelList == null || modelList.Count == 0) 
                {
                    model.HeaderStatus = "E";
                    model.Message = "客户端传来业务JSON转换为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                if (CheckMaterialIsSame(modelList, ref strError) == false) 
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                if (CheckStrongHoldCodeIsSame(modelList, ref strError) == false) 
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                if (CheckBatchNoIsSame(modelList, ref strError) == false) 
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                if (CheckWareHouseIsSame(modelList, ref strError) == false) 
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                if (_db.CheckQualityTaskDetailsID(modelList,ref strError) ==false) 
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;//"该物料："+ modelList[0].MaterialNo+"批次：" + modelList[0].BatchNo+"存在拣货数据，不能发起在库检！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                //生成ERP检验单
                if (PostQuality(userModel, modelList, ref strError, ref QualityNo) == false) 
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                //检验单生成成功，更新仓储批库位为待检状态
                if (_db.UpadteQualityForStock(modelList, ref strErrorWMS) == false) 
                {
                    model.HeaderStatus = "E";
                    model.Message = strError + "WMS数据更新失败！" + strErrorWMS;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                }

                model.HeaderStatus = "S";
                model.MaterialDoc = QualityNo;
                model.Message = strError + "WMS数据更新成功！";
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
                

            }
            catch (Exception ex) 
            {
                model.HeaderStatus = "E";
                model.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_StockInfo>>(model);
            }
        }

        private bool CheckMaterialIsSame(List<T_StockInfo> modelList, ref string strErrMsg)
        {
            bool bSucc = false;

            var groupByList = from t in modelList
                              group t by new { t1 = t.MaterialNo } into m
                              select new
                              {
                                  MaterialNo = m.Key.t1
                              };

            if (groupByList.Count() > 1 )
            {
                strErrMsg = "在库检存在不同物料，不能送检！";
                bSucc = false;
            }
            else { bSucc = true; }
            
            return bSucc;
        }

        private bool CheckStrongHoldCodeIsSame(List<T_StockInfo> modelList, ref string strErrMsg)
        {
            bool bSucc = false;

            var groupByList = from t in modelList
                              group t by new { t1 = t.StrongHoldCode } into m
                              select new
                              {
                                  StrongHoldCode = m.Key.t1
                              };

            if (groupByList.Count() > 1)
            {
                strErrMsg = "在库检存在不同据点，不能送检！";
                bSucc = false;
            }
            else { bSucc = true; }

            return bSucc;
        }

        private bool CheckBatchNoIsSame(List<T_StockInfo> modelList, ref string strErrMsg)
        {
            bool bSucc = false;

            var groupByList = from t in modelList
                              group t by new { t1 = t.BatchNo } into m
                              select new
                              {
                                  BatchNo = m.Key.t1
                              };

            if (groupByList.Count() > 1)
            {
                strErrMsg = "在库检存在不同批次，不能送检！";
                bSucc = false;
            }
            else { bSucc = true; }

            return bSucc;
        }

        private bool CheckWareHouseIsSame(List<T_StockInfo> modelList, ref string strErrMsg)
        {
            bool bSucc = false;

            var groupByList = from t in modelList
                              group t by new { t1 = t.WareHouseID } into m
                              select new
                              {
                                  WareHouseID = m.Key.t1
                              };

            if (groupByList.Count() > 1)
            {
                strErrMsg = "在库检存在不同仓库，不能送检！";
                bSucc = false;
            }
            else { bSucc = true; }

            return bSucc;
        }

        public bool PostQuality(UserModel userModel, List<T_StockInfo> modelList, ref string strError,ref string QualityNo)
        {
            try
            {
                BaseMessage_Model<List<T_StockInfo>> model = new BaseMessage_Model<List<T_StockInfo>>();
                bool bSucc = false;
                string strUserNo = string.Empty;                

                modelList = GroupInstockDetailList(modelList);

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

                modelList.ForEach(t => t.VoucherType = 20);
                modelList.ForEach(t => t.PostUser = userModel.UserNo);//strPostUser
               
                T_Interface_Func tfunc = new T_Interface_Func();
                string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<T_StockInfo>>(modelList);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

                model = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_StockInfo>>>(interfaceJson);               

                //过账失败直接返回
                if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
                {
                    strError = "生成检验单失败！" + model.Message;
                    bSucc = false;
                }
                else if (model.HeaderStatus == "S" && !string.IsNullOrEmpty(model.MaterialDoc)) 
                {
                    strError = "检验单生成成功！检验单号：" + model.MaterialDoc;
                    QualityNo = model.MaterialDoc;
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

        private List<T_StockInfo> GroupInstockDetailList(List<T_StockInfo> modelList)
        {
            var newModelList = from t in modelList
                               group t by new { t1 = t.MaterialNo, t2 = t.AreaNo} into m
                               select new T_StockInfo
                               {
                                   MaterialNo = m.Key.t1,
                                   Qty = m.Sum(item => item.Qty),
                                   BatchNo = m.FirstOrDefault().BatchNo,
                                   WarehouseNo = m.FirstOrDefault().WarehouseNo,
                                   AreaNo = m.Key.t2,                                 
                                   CompanyCode = m.FirstOrDefault().CompanyCode,
                                   StrongHoldCode = m.FirstOrDefault().StrongHoldCode
                               };

            return newModelList.ToList();
        }

    }
}