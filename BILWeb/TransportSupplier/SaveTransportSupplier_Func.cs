using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILWeb.Login.User;
using BILWeb.TransportSupplier;
using BILBasic.User;
using BILBasic.Interface;

namespace BILWeb.TransportSupplier
{

    public partial class T_SaveTransportSupplier_Func : TBase_Func<T_SaveTransportSupplier_DB, T_TransportSupplier>
    {
        protected override bool CheckModelBeforeSave(T_TransportSupplier model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (BILBasic.Common.Common_Func.IsNullOrEmpty(model.TransportSupplierID.ToString()))
            {
                strError = "承运商编号不能为空！";
                return false;
            }

            if (BILBasic.Common.Common_Func.IsNullOrEmpty(model.TransportSupplierName))
            {
                strError = "承运商名称不能为空！";
                return false;
            }

            return true;
        }

        protected override string GetModelChineseName()
        {
            return "承运商";
        }

        //public string SaveTransportSupplierListADF(string ModelJson)
        //{
        //    BaseMessage_Model<string> messageModel = new BaseMessage_Model<string>();

        //    try
        //    {
        //        string strError = string.Empty;
        //        string strError1 = string.Empty;

        //        if (string.IsNullOrEmpty(ModelJson))
        //        {
        //            messageModel.Message = "客户端传入装车数据为空！";
        //            messageModel.HeaderStatus = "E";
        //            return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
        //        }

        //        List<TransportSupplierDetail> modellist = BILBasic.JSONUtil.JSONHelper.JsonToObject<List<TransportSupplierDetail>>(ModelJson);

        //        if (modellist == null)
        //        {
        //            messageModel.Message = "客户端传入JSON转装车数据为空！";
        //            messageModel.HeaderStatus = "E";
        //            return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
        //        }

        //        //卸车，回传T100费用
        //        if (modellist[0].type == "2") 
        //        {
        //            if (PostCheckToTrans(modellist, ref strError) == false) 
        //            {
        //                messageModel.Message = strError;
        //                messageModel.HeaderStatus = "E";
        //                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
        //            }
        //        }

        //        T_SaveTransportSupplier_DB _db = new T_SaveTransportSupplier_DB();
        //        if (_db.SaveTransportSupplierADF(modellist, ref strError) == false)
        //        {
        //            messageModel.Message = strError;
        //            messageModel.HeaderStatus = "E";
        //            return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
        //        }
        //        else
        //        {
        //            messageModel.Message = "装车数据保存成功！" +"\r\n" + strError1;
        //            messageModel.HeaderStatus = "S";
        //            return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        messageModel.Message = ex.Message;
        //        messageModel.HeaderStatus = "E";
        //        return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
        //    }

        //}

        //public bool PostCheckToTrans( List<TransportSupplierDetail> modelList, ref string strError)
        //{
        //    try
        //    {
        //        BaseMessage_Model<List<TransportSupplierDetail>> model = new BaseMessage_Model<List<TransportSupplierDetail>>();
        //        bool bSucc = false;
        //        string strUserNo = string.Empty;
        //        string strPostUser = string.Empty;

        //        if (modelList[0].erpvoucherno.Contains("HH2"))
        //        {
        //            modelList.ForEach(t => t.gtype = "A");
        //        }
        //        else 
        //        {
        //            modelList.ForEach(t => t.gtype = "C");
        //        }

        //        modelList.ForEach(t => t.VoucherType = 9992);

        //        T_Interface_Func tfunc = new T_Interface_Func();
        //        string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<TransportSupplierDetail>>(modelList);
        //        string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

        //        model = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<TransportSupplierDetail>>>(interfaceJson);

        //        LogNet.LogInfo("ERPJsonAfter:" + BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<TransportSupplierDetail>>>(model));

        //        //过账失败直接返回
        //        if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
        //        {
        //            strError = "回传T100费用失败！" + model.Message;
        //            bSucc = false;
        //        }
        //        else if (model.HeaderStatus == "S" && !string.IsNullOrEmpty(model.MaterialDoc)) //过账成功，并且生成了凭证要记录数据库
        //        {
        //            strError = "回传T100费用成功！凭证号：" + model.MaterialDoc;
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

    }
}