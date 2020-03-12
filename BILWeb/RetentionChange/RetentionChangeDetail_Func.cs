using BILBasic.Basing.Factory;
using BILBasic.Interface;
using BILBasic.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.RetentionChange
{
    public partial class T_RetentionDetailChange_Func : TBase_Func<T_RetentionDetailChange_DB, T_RetentionDetailChangeInfo>
    {
        T_RetentionDetailChange_DB _db = new T_RetentionDetailChange_DB();

        protected override bool CheckModelBeforeSave(T_RetentionDetailChangeInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (string.IsNullOrEmpty(model.BatchNo))
            {
                strError = "物料批次为空，请确认是否有库存！";
                return false;
            }

            if (model.ID == 0) 
            {
                if (_db.CheckRetenMaterialNoIsExists(model) > 0)
                {
                    strError = "物料批次已经存在，不能重复新增数据！";
                    return false;
                }
            }


            if (string.IsNullOrEmpty(model.QresoneCode) || model.QresoneCode=="0")
            {
                strError = "请选择留置理由！";
                return false;
            }

            return true;
        }

        protected override string GetModelChineseName()
        {
            return "库存留置单表体";
        }

        protected override T_RetentionDetailChangeInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }


        public bool PostRetentionChange(UserModel user, List<T_RetentionDetailChangeInfo> modelList, ref string strErrMsg)
        {
            try
            {
                bool bSuccErp = false;
                string strErrorERP = string.Empty;

                if (modelList == null || modelList.Count == 0)
                {
                    strErrMsg = "客户端传来实体类为空！";
                    return false;
                }

                bSuccErp = CreateRetentionChangeToERP(user, modelList, ref strErrorERP);

                if (bSuccErp == false)
                {
                    strErrMsg = strErrorERP;
                    return false;
                }

                modelList.ForEach(t => t.ErpVoucherNo = strErrorERP);


                if (_db.UpdateStockRetention(modelList, ref strErrMsg) == false)
                {
                    strErrMsg += "/r/n" + "ERP库存留置单生成成功：" + strErrorERP;
                    return false;
                }

                strErrMsg = "ERP库存留置单生成成功：" + strErrorERP + "/r/n" + "WMS库存留置状态变更成功！";

                return true;

            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }

        public bool CreateRetentionChangeToERP(UserModel userModel, List<T_RetentionDetailChangeInfo> modelList, ref string strError)
        {
            try
            {
                BaseMessage_Model<List<T_RetentionDetailChangeInfo>> model = new BaseMessage_Model<List<T_RetentionDetailChangeInfo>>();
                bool bSucc = false;
                string strUserNo = string.Empty;
                string strPostUser = string.Empty;
                string strErpVoucherNo = string.Empty;
                string strAftDate = string.Empty;

                modelList.ForEach(t => t.PostUser = userModel.UserNo);


                T_Interface_Func tfunc = new T_Interface_Func();
                string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<T_RetentionDetailChangeInfo>>(modelList);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

                model = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_RetentionDetailChangeInfo>>>(interfaceJson);

                //过账失败直接返回
                if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
                {
                    strError = "ERP库存留置单生成失败！" + model.Message;
                    bSucc = false;
                }
                else
                {
                    strError = model.MaterialDoc;
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
