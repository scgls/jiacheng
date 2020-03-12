using BILWeb.LandMark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Landmark
{
    [RoleActionFilter(Message = "Basic/Landmark")]
    public class LandmarkController : BaseController<T_LandMarkInfo>
    {
        // GET: Landmark

        private ILandMarkService iLandMarkService;
        public LandmarkController()
        {
            iLandMarkService = (ILandMarkService)ServiceFactory.CreateObject("LandMark.T_LandMark_Func");
            baseservice = iLandMarkService;
        }
    }
}