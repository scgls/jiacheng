using BILWeb.TransportSupplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Query
{
    [RoleActionFilter(Message = "Query/T_TransportSupDetail")]
    public class T_TransportSupDetailController : BaseController<T_TransportSupDetailInfo>
    {
        private IT_TransportSupDetailService transportSupDetailService;
        public T_TransportSupDetailController()
        {
            transportSupDetailService = (IT_TransportSupDetailService)ServiceFactory.CreateObject("TransportSupplier.T_TransportSupDetail_Func");
            baseservice = transportSupDetailService;
        }



    }
}