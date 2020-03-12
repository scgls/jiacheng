using BILWeb.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Supplier
{
    [RoleActionFilter(Message = "Basic/Supplier")]
    public class SupplierController : BaseController<T_SupplierInfo>
    {
        private ISupplierService supplierService;
        public SupplierController()
        {
            supplierService = (ISupplierService)ServiceFactory.CreateObject("Supplier.T_Supplier_Func");
            baseservice = supplierService;
        }


    }
}