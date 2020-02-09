using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILBasic.User;
using BILBasic.Interface;

namespace BILWeb.QualityChange
{

    public partial class T_QualityChangeDetail_Func : TBase_Func<T_QualityChangeDetail_DB, T_QualityChangeDetailInfo>
    {
        T_QualityChangeDetail_DB _db = new T_QualityChangeDetail_DB();

        protected override bool CheckModelBeforeSave(T_QualityChangeDetailInfo model, ref string strError)
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

            if (_db.CheckQualityMaterialNoIsExists(model) > 0)
            {
                strError = "物料批次已经存在，不能重复新增数据！";
                return false;
            }
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "质量变更单表体";
        }

        protected override T_QualityChangeDetailInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }


        public bool PostQualityChange(UserModel user, List<T_QualityChangeDetailInfo> modelList, ref string strErrMsg)
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

                bSuccErp = CreateQualityChangeToERP(user, modelList, ref strErrorERP);

                if (bSuccErp == false)
                {
                    strErrMsg = strErrorERP;
                    return false;
                }

                modelList.ForEach(t => t.ErpVoucherNo = strErrorERP);

                T_QualityChangeDetail_DB _db = new T_QualityChangeDetail_DB();

                if (_db.UpdateStockQuality(modelList, ref strErrMsg) == false)
                {
                    strErrMsg += "/r/n" + "ERP质量状态变更单生成成功：" + strErrorERP;
                    return false;
                }

                strErrMsg = "ERP质量状态变更单生成成功：" + strErrorERP + "/r/n" + "WMS库存质量状态变更成功！";

                return true;

            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }

        public bool CreateQualityChangeToERP(UserModel userModel, List<T_QualityChangeDetailInfo> modelList, ref string strError)
        {
            try
            {
                BaseMessage_Model<List<T_QualityChangeDetailInfo>> model = new BaseMessage_Model<List<T_QualityChangeDetailInfo>>();
                bool bSucc = false;
                string strUserNo = string.Empty;
                string strPostUser = string.Empty;
                string strErpVoucherNo = string.Empty;
                string strAftDate = string.Empty;
                
                modelList.ForEach(t => t.PostUser = userModel.UserNo);
                if (modelList[0].WareHouseNo.Contains("AD07") || modelList[0].WareHouseNo.Contains("AD10")
                    || modelList[0].WareHouseNo.Contains("BH02") || modelList[0].WareHouseNo.Contains("BH03")
                    || modelList[0].WareHouseNo.Contains("BH04") || modelList[0].WareHouseNo.Contains("BH07")
                    || modelList[0].WareHouseNo.Contains("BH08")) 
                {
                    modelList.ForEach(t => t.AreaNo = " "); 
                }
                               

                T_Interface_Func tfunc = new T_Interface_Func();
                string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<T_QualityChangeDetailInfo>>(modelList);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

                model = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_QualityChangeDetailInfo>>>(interfaceJson);

                //过账失败直接返回
                if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
                {
                    strError = "ERP质量状态变更单生成失败！" + model.Message;
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
