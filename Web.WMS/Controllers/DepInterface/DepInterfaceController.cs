using BILWeb.DepInterface;
using BILWeb.PickRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.DepInterface
{
    [RoleActionFilter(Message = "Basic/DepInterface")]
    public class DepInterfaceController : BaseController<T_DepInterfaceInfo>
    {
        // GET: PickRule
        private IDepInterfaceService depInterfaceService;
        public DepInterfaceController()
        {
            depInterfaceService = (IDepInterfaceService)ServiceFactory.CreateObject("DepInterface.T_DepInterface_Func");
            baseservice = depInterfaceService;
        }
    }
}