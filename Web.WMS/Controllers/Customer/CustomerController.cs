using BILWeb.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Customer
{
    [RoleActionFilter(Message = "Basic/Customer")]
    public class CustomerController : BaseController<T_CustomerInfo>
    {

        private ICustomerService customerService;
        public CustomerController()
        {
            customerService = (ICustomerService)ServiceFactory.CreateObject("Customer.T_Customer_Func");
            baseservice = customerService;
        }

    }
}