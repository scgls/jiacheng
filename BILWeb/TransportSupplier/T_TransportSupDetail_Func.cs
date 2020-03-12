using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILBasic.Interface;
using BILWeb.Login.User;

namespace BILWeb.TransportSupplier
{

    public partial class T_TransportSupDetail_Func : TBase_Func<T_TransportSupdetail_DB, T_TransportSupDetailInfo>, IT_TransportSupDetailService
    {

        protected override bool CheckModelBeforeSave(T_TransportSupDetailInfo model, ref string strError)
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
            return "装车，卸车";
        }

        protected override T_TransportSupDetailInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

        public bool GetModelListBySql(UserInfo user, ref List<T_TransportSupplier> lstTransportSupplier)
        {
            try
            {
                T_TransportSupplier_DB thd = new T_TransportSupplier_DB();
                lstTransportSupplier = thd.GetModelListBySql(user, false);
                if (lstTransportSupplier == null || lstTransportSupplier.Count <= 0) { return false; }
                else { return true; }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public string GetTransportSupplierDetailList(string Palletno)
        {
            BaseMessage_Model<List<T_TransportSupDetailInfo>> messageModel = new BaseMessage_Model<List<T_TransportSupDetailInfo>>();
            try
            {

                T_TransportSupdetail_DB _db = new T_TransportSupdetail_DB();
                List<T_TransportSupDetailInfo> modelList = _db.GetTransportSupplierDetailList(Palletno);

                if (modelList == null || modelList.Count == 0)
                {
                    messageModel.Message = "没有获取到装车信息！";
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_TransportSupDetailInfo>>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = modelList;
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_TransportSupDetailInfo>>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.Message = ex.Message;
                messageModel.HeaderStatus = "E";
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_TransportSupDetailInfo>>>(messageModel);
            }
        }


        public string SaveTransportSupplierListADF(string ModelJson)
        {
            BaseMessage_Model<string> messageModel = new BaseMessage_Model<string>();

            try
            {
                string strError = string.Empty;
                string strError1 = string.Empty;
                string TradingConditionsCode = string.Empty;

                if (string.IsNullOrEmpty(ModelJson))
                {
                    messageModel.Message = "客户端传入物流数据为空！";
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                }

                List<T_TransportSupDetailInfo> modellist = BILBasic.JSONUtil.JSONHelper.JsonToObject<List<T_TransportSupDetailInfo>>(ModelJson);

                if (modellist == null)
                {
                    messageModel.Message = "客户端传入JSON转物流数据为空！";
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                }

                //卸车，回传T100费用
                TradingConditionsCode = modellist[0].TradingConditionsCode;
                if (!string.IsNullOrEmpty(TradingConditionsCode)) 
                {
                    TradingConditionsCode = TradingConditionsCode.Substring(0, 3);
                    if (modellist[0].Type == "2")
                    {
                        if (PostCheckToTrans(modellist, ref strError1) == false)
                        {
                            messageModel.Message = strError;
                            messageModel.HeaderStatus = "E";
                            return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                        }
                    }
                    //if (TradingConditionsCode != "MS0") //不为MS0，需要回传物流费用
                    //{
                    //    if (modellist[0].Type == "2")
                    //    {
                    //        if (PostCheckToTrans(modellist, ref strError1) == false)
                    //        {
                    //            messageModel.Message = strError;
                    //            messageModel.HeaderStatus = "E";
                    //            return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                    //        }
                    //    }
                    //}
                }
                

                T_TransportSupdetail_DB _db = new T_TransportSupdetail_DB();
                if (_db.SaveTransportSupplierADF(modellist, ref strError) == false)
                {
                    messageModel.Message = strError;
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                }
                else
                {
                    messageModel.Message = "物流数据保存成功！" + "\r\n" + strError1;
                    messageModel.HeaderStatus = "S";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                }

            }
            catch (Exception ex)
            {
                messageModel.Message = ex.Message;
                messageModel.HeaderStatus = "E";
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
            }

        }

        public bool PostCheckToTrans(List<T_TransportSupDetailInfo> modelList, ref string strError)
        {
            try
            {
                BaseMessage_Model<List<T_TransportSupDetailInfo>> model = new BaseMessage_Model<List<T_TransportSupDetailInfo>>();
                bool bSucc = false;
                string strUserNo = string.Empty;
                string strPostUser = string.Empty;
                string StrongHoldCode = string.Empty;

                if (modelList[0].ErpVoucherNo.Contains("HH2"))
                {
                    modelList.ForEach(t => t.gtype = "C");
                }
                else
                {
                    modelList.ForEach(t => t.gtype = "A");
                }
                StrongHoldCode = modelList[0].ErpVoucherNo.Substring(0,3);
                modelList.ForEach(t => t.VoucherType = 9992);
                modelList.ForEach(t => t.StrongHoldCode = StrongHoldCode);

                T_Interface_Func tfunc = new T_Interface_Func();
                string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<T_TransportSupDetailInfo>>(modelList);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

                model = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_TransportSupDetailInfo>>>(interfaceJson);

                LogNet.LogInfo("ERPJsonAfter:" + BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_TransportSupDetailInfo>>>(model));

                //过账失败直接返回
                if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
                {
                    strError = "回传T100费用失败！" + model.Message;
                    bSucc = false;
                }
                else if (model.HeaderStatus == "S" && !string.IsNullOrEmpty(model.MaterialDoc)) //过账成功，并且生成了凭证要记录数据库
                {
                    strError = "回传T100费用成功！凭证号：" + model.MaterialDoc;
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

    }
}