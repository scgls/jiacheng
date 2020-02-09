using BILWeb.StrategeRuleAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.OutStockCreate
{
    public class OutStockCreate_EnableSlotRule : StrategeRuleAll<T_OutStockCreateInfo>
    {
        public override void GetOutStockSlotList(ref List<T_OutStockCreateInfo> modelList)
        {
            PickRule.T_PickRule_DB pickDB = new PickRule.T_PickRule_DB();
            //分配规则
            List<PickRule.T_PickRuleInfo> pickRuleList = pickDB.GetPearRuleListByPage(3);

            Object obj = WMS.Factory.ServiceFactory.CreateObject(pickRuleList[0].ParameterIDN);

            OutStockCreate_SplitContext<T_OutStockCreateInfo, T_OutStockCreateInfo> context = new OutStockCreate_SplitContext<T_OutStockCreateInfo, T_OutStockCreateInfo>
                ((OutStockCreate_SplitBaseRule<T_OutStockCreateInfo, T_OutStockCreateInfo>)obj);
            context.GetOutStockCreateAutoSlotList(ref modelList);

        }
    }
}
