using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WMS.Web.Filter
{
    public class YmhAuthorizeAttribute : AuthorizeAttribute
    {
        public YmhAuthorizeAttribute(string roles) {
            Roles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string roles = httpContext.Request["role"];
            if (!string.IsNullOrEmpty(roles))
            {
                return Roles.Contains(roles);
            }
            return base.AuthorizeCore(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Login/Login");
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}