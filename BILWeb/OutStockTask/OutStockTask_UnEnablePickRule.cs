using BILWeb.StrategeRuleAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILWeb.OutStockTask;

namespace BILWeb.OutStockTask
{
    public class OutStockTask_UnEnablePickRule : StrategeRuleAll<T_OutStockTaskDetailsInfo>
    {
        public override bool GetOutStockTaskDetailPickList(ref List<T_OutStockTaskDetailsInfo> modelList, ref string strError)
        {
            return base.GetOutStockTaskDetailPickList(ref modelList, ref strError);
        }
    }
}
