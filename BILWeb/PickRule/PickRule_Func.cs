using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILWeb.Login.User;

namespace BILWeb.PickRule
{

    public partial class T_PickRule_Func : TBase_Func<T_PickRule_DB, T_PickRuleInfo>,IPickRuleService
    {
        T_PickRule_DB _db = new T_PickRule_DB();
        protected override bool CheckModelBeforeSave(T_PickRuleInfo model, ref string strError)
        {
            T_PickRuleInfo pr = new T_PickRuleInfo();

            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (string.IsNullOrEmpty(model.MaterialClassCode)||model.MaterialClassCode == "0") 
            {
                strError = "请选择物料分类！";
                return false;
            }

            if (model.PickRuleCode == 0)
            {
                strError = "请选择拣货规则！";
                return false;
            }

            if (model.Status == 0)
            {
                strError = "请选择拣货规则状态！";
                return false;
            }

            //if (base.GetModelByFilter(ref pr, "Materialclasscode='" + model.MaterialClassCode + "' and id <> '"+model.ID+"'", ref strError) == true) 
            //{
            //    strError = "该物料分类已经设置拣货规则，请确认！";
            //    return false;
            //}

            return true;
        }

        protected override string GetModelChineseName()
        {
            return "拣货规则";
        }

        protected override T_PickRuleInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

        public bool GetAllPickRule(ref List<T_PickRuleInfo> modelList,ref string strError) 
        {
            try
            {
                modelList = _db.GetAllPickRule();
                if (modelList == null || modelList.Count == 0) 
                {
                    strError = "拣货规则未配置！";
                    return false;
                }

                return true;

            }
            catch(Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

    }
}