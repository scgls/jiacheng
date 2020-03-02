using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILBasic.User;
using BILWeb.Login.User;
using BILBasic.Interface;

namespace BILWeb.EdateChange
{

    public partial class T_EDateChangeDetail_Func : TBase_Func<T_EDateChangeDetail_DB, T_EDateChangeDetailInfo>
    {
        T_EDateChangeDetail_DB _db = new T_EDateChangeDetail_DB();
        protected override bool CheckModelBeforeSave(T_EDateChangeDetailInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (model.AftEDate == null) 
            {
                strError = "请选择变更后效期！";
                return false;
            }

            if (model.ResoneCode == 0)
            {
                strError = "请选择理由码！";
                return false;
            }

            if (string.IsNullOrEmpty(model.BatchNo))
            {
                strError = "物料批次为空，请确认是否有库存！";
                return false;
            }

            if (_db.CheckBatchIsExists(model) > 0) 
            {
                strError = "物料批次已经存在，不能重复新增数据！";
                return false;
            }
            return true;
        }

        public override bool CanDelModel(UserModel user, T_EDateChangeDetailInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (!string.IsNullOrEmpty(model.ErpVoucherNo)) 
            {
                strError = "已经生成ERP效期调整单，不能删除数据！";
                return false;
            }

            return true;
        }

        protected override string GetModelChineseName()
        {
            return "效期变更单表体行";
        }

        protected override T_EDateChangeDetailInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

        public bool PostEDateChange(UserModel user, List<T_EDateChangeDetailInfo> modelList, ref string strErrMsg)
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

                bSuccErp = CreateEDateChangeToERP(user, modelList, ref strErrorERP);

                if (bSuccErp == false)
                {
                    strErrMsg = strErrorERP;
                    return false;
                }

                modelList.ForEach(t => t.ErpVoucherNo = strErrorERP);

                T_EDateChangeDetail_DB _db = new T_EDateChangeDetail_DB();

                if (_db.UpdateStockEdate(modelList, ref strErrMsg) == false) 
                {
                    strErrMsg += "/r/n" + "ERP效期变更单生成成功：" + strErrorERP;
                    return false;
                }

                strErrMsg = "ERP效期变更单生成成功：" + strErrorERP + "/r/n" + "WMS库存效期变更成功！";

                return true;

            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }

        public bool CreateEDateChangeToERP(UserModel userModel, List<T_EDateChangeDetailInfo> modelList, ref string strError)
        {
            try
            {
                BaseMessage_Model<List<T_EDateChangeDetailInfo>> model = new BaseMessage_Model<List<T_EDateChangeDetailInfo>>();
                bool bSucc = false;
                string strUserNo = string.Empty;
                string strPostUser = string.Empty;
                string strErpVoucherNo = string.Empty;
                string strAftDate = string.Empty;

                modelList.ForEach(t => t.StrResoneCode=t.ResoneCode.ToString().PadLeft(3,'0'));
                modelList.ForEach(t => t.PostUser = userModel.UserNo);
                modelList.ForEach(t => t.StrAftEDate = t.AftEDate.ToDateTime().Date.ToString("yyyy-MM-dd"));                

                T_Interface_Func tfunc = new T_Interface_Func();
                string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<T_EDateChangeDetailInfo>>(modelList);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

                model = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_EDateChangeDetailInfo>>>(interfaceJson);

                //过账失败直接返回
                if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
                {
                    strError = "ERP效期变更单生成失败！" + model.Message;
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
