using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;
using BILBasic.JSONUtil;
using Newtonsoft.Json;

namespace BILWeb.Material
{
    public partial class T_Material_Func : TBase_Func<T_Material_DB, T_MaterialInfo>, IMaterialService
    {

        protected override bool CheckModelBeforeSave(T_MaterialInfo model, ref string strError)
        {
            T_Material_DB mdb = new T_Material_DB();
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.MaterialNo))
            {
                strError = "物料编号不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.MaterialDesc))
            {
                strError = "物料名称不能为空！";
                return false;
            }

            //新增的情况需要验证物料编号是否存在
            if (model.ID <= 0)
            {
                if (mdb.CheckMaterialExist(model) > 0)
                {
                    strError = "物料编码已经存在！";
                    return false;
                }
            }


            return true;
        }

        protected override string GetModelChineseName()
        {
            return "物料";
        }

        protected override T_MaterialInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

        public string getMaterialsADF(T_MaterialInfo material)
        {
            BaseMessage_Model<List<T_MaterialInfo>> messageModel = new BaseMessage_Model<List<T_MaterialInfo>>();
            T_Material_DB mdb = new T_Material_DB();
            try
            {
                List<T_MaterialInfo> modelList = mdb.getListMaterial(material);


                if (modelList == null || modelList.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "获取物料数据列表为空！";
                }
                else
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.ModelJson = modelList;
                }
                return JsonConvert.SerializeObject(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                messageModel.ModelJson = null;
                return JsonConvert.SerializeObject(messageModel);
            }
        }
        public string GetMaterialModelBySqlADF(string MaterialNo)
        {
            BaseMessage_Model<T_MaterialInfo> messageModel = new BaseMessage_Model<T_MaterialInfo>();
            try
            {
                string strError = string.Empty;
                T_MaterialInfo model = new T_MaterialInfo();
                model.MaterialNo = MaterialNo;
                bool bSucc = base.GetModelBySql(ref model, ref strError);

                if (bSucc == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_MaterialInfo>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = model;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_MaterialInfo>>(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_MaterialInfo>>(messageModel);
            }

        }

        public List<T_MaterialInfo> GetMaterialModelBySql(string MaterialNo, ref string strErrMsg)
        {
            try
            {

                List<T_MaterialInfo> modelList = new List<T_MaterialInfo>();

                string Filter = " materialno like '" + MaterialNo + "%'";

                bool bSucc = base.GetModelListByFilter(ref modelList, ref strErrMsg, "", Filter);

                if (bSucc == false)
                {
                    return null;
                }
                else
                {
                    if (modelList == null)
                    {
                        strErrMsg = "物料不存在！";
                        return null;
                    }
                }
                return modelList;

            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return null;
            }

        }

    }
}
