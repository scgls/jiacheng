using BILBasic.Basing.Factory;
using BILBasic.Interface;
using BILBasic.JSONUtil;
using BILBasic.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.OutStock
{
    public partial class T_OutStock_Func : TBase_Func<T_OutStock_DB, T_OutStockInfo>,IOutStockService
    {

        protected override bool CheckModelBeforeSave(T_OutStockInfo model, ref string strError)
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
            return "出库单";
        }

        protected override T_OutStockInfo GetModelByJson(string strJson)
        {
            return JSONHelper.JsonToObject<T_OutStockInfo>(strJson);
        }

        public bool GetOutStockAndDetailsModelByNo(string erpNo, ref BILWeb.OutStockTask.T_OutStockTaskInfo head, ref List<BILWeb.OutStockTask.T_OutStockTaskDetailsInfo> lstDetail, ref string ErrMsg)
        {
            T_OutStock_DB os = new T_OutStock_DB();
            return os.GetOutStockAndDetailsModelByNo(erpNo, ref head, ref lstDetail, ref ErrMsg);
        }

        protected override bool Sync(T_OutStockInfo model, ref string strErrMsg)
        {
            BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
            return PFunc.Sync(20, string.Empty, model.ErpVoucherNo, model.VoucherType, ref strErrMsg, "ERP", -1, null);
        }

        /// <summary>
        /// 扫描小车或者ERP订单号复核，返回的是表头+表体的数据
        /// 表体需要根据相同物料汇总
        /// </summary>
        /// <param name="strCarNo"></param>
        /// <param name="model"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool GetModelListByCar(string strCarNo, ref T_OutStockInfo model ,ref string strError)
        {
            try
            {
                if (string.IsNullOrEmpty(strCarNo))
                {
                    strError = "扫描或输入拣货车编号为空！";
                    return false;
                }
                T_OutStock_DB _db = new T_OutStock_DB();
                return _db.GetModelListByCar(strCarNo,ref  model,ref  strError);

            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        #region PC打印发货清单
        public bool GetOutStockDetailForPrint(string strErpVoucherNo, ref T_OutStockInfo model, ref string strError)
        {
            try
            {
                if (string.IsNullOrEmpty(strErpVoucherNo))
                {
                    strError = "传入的ERP单号为空！";
                    return false;
                }

                T_OutStock_DB tdb = new T_OutStock_DB();
                model = tdb.GetOutStockDetailForPrint(strErpVoucherNo);

                if (model == null) 
                {
                    strError = "单号不存在！" +strErpVoucherNo;
                    return false;
                }

                if (model.VoucherType == 24 ) 
                {
                    if (string.IsNullOrEmpty(model.ShipDFlg) || model.ShipDFlg == "N")
                    {
                        strError = "单据不需要打印发货清单！" + strErpVoucherNo;
                        return false;
                    }
                }       

                List<T_OutStockDetailInfo> modelList = new List<T_OutStockDetailInfo>();
                T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
                bool bSucc = tfunc.GetModelListByHeaderID(ref modelList, model.ID, ref strError);
                if (bSucc == false)
                {                    
                    return false;
                }

                if (string.IsNullOrEmpty(model.ShipPFlg) || model.ShipPFlg == "N") 
                {
                     modelList.ForEach(t => t.Price = 0);
                     modelList.ForEach(t => t.Amount = 0);
                }

                model.lstDetail = modelList;

                return true;

            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }
        #endregion

        #region 验证发货单据是否已经在WMS存在

        public int GetOutStockNoIsExists(string strErpVoucherNo) 
        {
            T_OutStock_DB _db = new T_OutStock_DB();
            return _db.GetOutStockNoIsExists(strErpVoucherNo);
        }

        #endregion

        #region 关闭发货单

        public bool CloseOutStockVoucherNo(int ID, UserModel user,  ref string strError)
        {
            try
            {
                bool bSucc = false;
                if (ID == 0)
                {
                    strError = "客户端传入关闭ID为0！";
                    return false;
                }
                T_OutStockInfo model = new T_OutStockInfo();
                model.ID = ID;
                bSucc = base.GetModelByID(ref model, ref strError);
                if (bSucc == false) 
                {
                    return false;
                }

                if (model.Status == 5) 
                {
                    strError = "单据已经过账，不能关闭！";
                    return false;
                }

                if (model.Status == 6) 
                {
                    strError = "单据已经关闭，不能重复关闭！";
                    return false;
                }


                //发货单需要调用T100状态接口
                //if (model.VoucherType == 24) 
                //{
                //    List<T_OutStockInfo> modelList = new List<T_OutStockInfo>();
                //    modelList.Add(model);
                //    bSucc = PostCloseOutStockVoucherNo(modelList, ref strError);
                //    if (bSucc == false) 
                //    {
                //        return false;
                //    }
                //}
                string strError1 = string.Empty;

                //关闭WMS单据状态
                bSucc = base.UpadteModelByModelSql(user, model, ref strError1);

                if (bSucc == false)
                {
                    strError = strError + "\r\n" + strError1;
                }
                else 
                {
                    strError = strError + "\r\n" + "WMS单据关闭成功！";
                }

                return bSucc;

            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        public bool PostCloseOutStockVoucherNo(List<T_OutStockInfo> modelList, ref string strError)
        {
            try
            {
                BaseMessage_Model<List<T_OutStockInfo>> model = new BaseMessage_Model<List<T_OutStockInfo>>();
                bool bSucc = false;
                string strUserNo = string.Empty;
                string strPostUser = string.Empty;
                string StrongHoldCode = string.Empty;

                //StrongHoldCode = modelList[0].ErpVoucherNo.Substring(0, 3);
                modelList.ForEach(t => t.VoucherType = 9993);
                modelList.ForEach(t => t.WmsStatus = "E");
                //modelList.ForEach(t => t.StrongHoldCode = StrongHoldCode);

                T_Interface_Func tfunc = new T_Interface_Func();
                string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<T_OutStockInfo>>(modelList);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

                model = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_OutStockInfo>>>(interfaceJson);

                LogNet.LogInfo("ERPJsonAfter:" + BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutStockInfo>>>(model));

                //过账失败直接返回
                if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
                {
                    strError = "回传T100关闭状态失败！" + model.Message;
                    bSucc = false;
                }
                else if (model.HeaderStatus == "S" && !string.IsNullOrEmpty(model.MaterialDoc)) //过账成功，并且生成了凭证要记录数据库
                {
                    strError = "回传T100关闭状态成功！凭证号：" + model.MaterialDoc;
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

    }
}
