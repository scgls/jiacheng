using BILBasic.Basing.Factory;
using BILBasic.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.RuleAll
{

    public partial class T_RuleAll_Func : TBase_Func<t_RuleAll_DB, T_RuleAllInfo>,IRuleAllService
    {

        protected override bool CheckModelBeforeSave(T_RuleAllInfo model, ref string strError)
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
            return "配置总规则";
        }

        protected override T_RuleAllInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

        public bool GetRuleModelByItemID(int ConItemID,ref T_RuleAllInfo model,ref string strError) 
        {
            t_RuleAll_DB rdb = new t_RuleAll_DB();
            List<T_RuleAllInfo> modelList = new List<T_RuleAllInfo>();
            modelList = rdb.GetRuleListByPage(ConItemID);

            if (modelList == null || modelList.Count == 0) 
            {
                strError = Language_CHS.RuleAllIsEmpty;
                return false;
            }

            model = modelList.Find(t => t.ConItemID == ConItemID);
            return true;

        }

       

    }
}
