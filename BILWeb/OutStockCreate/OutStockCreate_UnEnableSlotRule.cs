using BILWeb.StrategeRuleAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.OutStockCreate
{
    //拣货任务分配规则
    public class OutStockCreate_UnEnableSlotRule : StrategeRuleAll<T_OutStockCreateInfo>
    {
        public override void GetOutStockSlotList(ref List<T_OutStockCreateInfo> modelList)
        {
            base.GetOutStockSlotList(ref modelList);
        }
    }
}
