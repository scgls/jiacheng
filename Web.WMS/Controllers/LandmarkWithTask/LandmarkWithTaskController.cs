using BILWeb.LandMark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.LandmarkWithTask
{
    [RoleActionFilter(Message = "Print/LandmarkWithTask")]
    public class LandmarkWithTaskController : BaseController<T_LandMarkWithTaskInfo>
    {

        private ILandMarkDerailService iLandMarkDerailService;
        public LandmarkWithTaskController()
        {
            iLandMarkDerailService = (ILandMarkDerailService)ServiceFactory.CreateObject("LandMark.T_LandMarkWithTask_Func");
            baseservice = iLandMarkDerailService;
        }
    }
}