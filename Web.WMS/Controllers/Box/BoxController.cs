using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BILWeb.Box;
using BILBasic.Common;
using Web.WMS.Common;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Box
{
    [RoleActionFilter(Message = "Basic/Box")]
    public class BoxController :   BaseController<T_BoxInfo>
    {
        private IBoxService boxService;
        public BoxController()
        {
            boxService = (IBoxService)ServiceFactory.CreateObject("Box.T_Box_Func");
            baseservice = boxService;
        }

    }
}