using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BILBasic.Common;
using BILWeb.OutStockCreate;
using System.Reflection;
using BILWeb.RuleAll;
using BILWeb.Stock;
using BILWeb.OutStockTask;

namespace BILWeb.StrategeRuleAll
{
    public class Context<T>    
    {
        private StrategeRuleAll<T> ruleAllCreate;

        //public Context(OutStockCreateRule<TBase_Model> outStockCreateRule) 
        //{
        //    this.outStockCreateRule = outStockCreateRule;
        //}

        public Context(int type)
        {
            //根据传入规则类型判断new对象
            RuleAll.t_RuleAll_DB ruleDB = new RuleAll.t_RuleAll_DB();
            List<RuleAll.T_RuleAllInfo> ruleList = ruleDB.GetRuleListByPage(type);
            int isEnable = ruleList == null ? 0 : ruleList[0].IsEnable.ToInt32();//1-不启用 2-启用 

            switch (type)
            {
                case RuleAll_Config.OutStockSplitItem: //拣货拆分机制
                    if (isEnable == RuleAll_Config.RuleAllUnEnable)
                    {
                        ruleAllCreate = (StrategeRuleAll<T>)(Object)new OutStockCreate_UnEnableRule();
                    }
                    else if (isEnable == RuleAll_Config.RuleAllEnable) { ruleAllCreate = (StrategeRuleAll<T>)(Object)new OutStockCreate_EnableRule(); }

                    break;
                case RuleAll_Config.OutStockSlotItem: //拣货分配机制
                    if (isEnable == RuleAll_Config.RuleAllUnEnable)
                    {
                        ruleAllCreate = (StrategeRuleAll<T>)(Object)new OutStockCreate_UnEnableSlotRule();
                    }
                    else if (isEnable == RuleAll_Config.RuleAllEnable) { ruleAllCreate = (StrategeRuleAll<T>)(Object)new OutStockCreate_EnableSlotRule(); }

                    break;
                case RuleAll_Config.SerialItem://序列号启用
                    if (isEnable == RuleAll_Config.RuleAllUnEnable)
                    {
                        ruleAllCreate = (StrategeRuleAll<T>)(object)new Stock_SerialUnEnableRule();
                    }
                    else if (isEnable == RuleAll_Config.RuleAllEnable) 
                    {
                        ruleAllCreate = (StrategeRuleAll<T>)(object)new Stock_SerialEnableRule();
                    }

                    break;

                case RuleAll_Config.OutStockPickItem://拣货规则启用
                    if (isEnable == RuleAll_Config.RuleAllUnEnable)
                    {
                        ruleAllCreate = (StrategeRuleAll<T>)(object)new OutStockTask_UnEnablePickRule();
                    }
                    else if (isEnable == RuleAll_Config.RuleAllEnable)
                    {
                        ruleAllCreate = (StrategeRuleAll<T>)(object)new OutStockTask_EnablePickRule();
                    }
                    break;

                default:
                    ruleAllCreate = null;
                    break;
            }
        }
        

        public bool GetOutStockCreateList(ref List<T> lstBaseModel, ref string ErrorMsg)
        {
            return ruleAllCreate.GetOutStockCreateList(ref lstBaseModel, ref ErrorMsg);
        }

        public void GetOutStockSlotList(ref List<T> modelList)
        {
            ruleAllCreate.GetOutStockSlotList(ref modelList);
        }

        public bool GetStockByBarCode(T model,ref List<T> modelList,ref string strError)
        {
            return ruleAllCreate.GetStockByBarCode(model,ref modelList,ref strError);
        }

        public bool GetOutStockTaskDetailPickList(ref List<T> modelList, ref string strError) 
        {
            return ruleAllCreate.GetOutStockTaskDetailPickList(ref modelList, ref strError);
        }

    }
}
