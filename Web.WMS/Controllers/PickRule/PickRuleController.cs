using BILWeb.PickRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.PickRule
{
    [RoleActionFilter(Message = "Basic/PickRule")]
    public class PickRuleController : BaseController<T_PickRuleInfo>
    {
        // GET: PickRule
        private IPickRuleService pickRuleService;
        public PickRuleController()
        {
            pickRuleService = (IPickRuleService)ServiceFactory.CreateObject("PickRule.T_PickRule_Func");
            baseservice = pickRuleService;
        }

    }
}