using BILBasic.Basing.Factory;
using BILWeb.StrategeRuleAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.OutStockCreate
{
    public class OutStockCreate_UnEnableRule : StrategeRuleAll<T_OutStockCreateInfo>
    {
        //不启用生单规则或者使用默认规则
        //按照订单号和组织拆分订单
        public override bool GetOutStockCreateList(ref List<T_OutStockCreateInfo> lstBaseModel, ref string ErrorMsg)
        {
            OutStockCreate_ModelData modelData = new OutStockCreate_ModelData();
            

            lstBaseModel = modelData.GetNewOutStockModelList(lstBaseModel);

            return true;
        }

        
    }
    
}
