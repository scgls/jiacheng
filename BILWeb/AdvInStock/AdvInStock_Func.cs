using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILWeb.Login.User;
using BILBasic.JSONUtil;

namespace BILWeb.AdvInStock
{

    public partial class T_AdvInStock_Func : TBase_Func<T_AdvInStock_DB, T_AdvInStockInfo>
    {

        public string GetT_MaterialPackADF(string advInStockmodel)
        {
            T_AdvInStockInfo advInstock = JSONHelper.JsonToObject<T_AdvInStockInfo>(advInStockmodel);

            T_AdvInStock_DB db = new T_AdvInStock_DB();
            UserInfo user = new UserInfo();
            user.UserNo = advInstock.Creater;
            string errMsg = "";
            BaseMessage_Model<List<T_AdvInStockInfo>> model = new BaseMessage_Model<List<T_AdvInStockInfo>>();
            try
            {
                return "";
                //if (db.SaveModelBySqlToDB2(user, advInstock, ref errMsg))
                //{
                //    model.HeaderStatus = "S";
                //    model.Message = "保存成功！";
                //    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_AdvInStockInfo>>>(model);
                //}
                //else
                //{
                //    model.HeaderStatus = "E";
                //    model.Message = errMsg;
                //    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_AdvInStockInfo>>>(model);

                //}
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = "保存异常！" + ex.ToString();
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_AdvInStockInfo>>>(model);
            }
        }
        public string Get_AdvInParameter(string groupname)
        {
            BaseMessage_Model<List<Parameter_Model>> model = new BaseMessage_Model<List<Parameter_Model>>();
            try
            {
                List<Parameter_Model> listparameter = db.GetAdvInParameter(groupname);
                model.ModelJson = listparameter;

                model.HeaderStatus = "S";
                model.Message = "";
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<Parameter_Model>>>(model);
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = "保存异常！" + ex.ToString();
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<Parameter_Model>>>(model);
            }
        }

        protected override List<T_AdvInStockInfo> GetModelListByJson(string UserJson, string ModeJson)
        {
            List<T_AdvInStockInfo> listadv = new List<T_AdvInStockInfo>();
            listadv.Add(JSONHelper.JsonToObject<T_AdvInStockInfo>(ModeJson));
            return listadv;
        }
        //  
        protected override bool CheckModelBeforeSave(T_AdvInStockInfo model, ref string strError)
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
            return "预到货单表头";
        }

        protected override T_AdvInStockInfo GetModelByJson(string ModelJson)
        {
            return JSONHelper.JsonToObject<T_AdvInStockInfo>(ModelJson);
        }

    }
}