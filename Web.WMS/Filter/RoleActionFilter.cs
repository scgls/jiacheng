using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace WMS.Web.Filter
{
    public class RoleActionFilter : ActionFilterAttribute
    {
        public string Message { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (Message.Contains("/"))
                {
                    string[] strs = Message.Split('/');
                    HttpContext.Current.Session["Menu"] = strs[0];
                    HttpContext.Current.Session["Message"] = strs[1];
                }
                //HttpContext.Current.Session["printIP"] = "http://10.1.254.172:7030/";
                //HttpContext.Current.Session["printIP"] = "http://10.1.254.71:8882/";
                HttpContext.Current.Session["printIP"] = ConfigurationManager.ConnectionStrings["printIP"].ConnectionString;
                string UserNo = HttpContext.Current.Request.Cookies["userinfo"]["UserNo"];
                if (string.IsNullOrEmpty(UserNo))
                {
                    filterContext.Result = new RedirectResult("/Login/Login");
                    return;
                }
            }
            catch (Exception ex)
            {
                filterContext.Result = new RedirectResult("/Login/Login");
                return;
            }

            //HttpContext.Current.Session["printIP"] = "http://10.1.254.172:7030/";


            base.OnActionExecuting(filterContext);
            //filterContext.HttpContext.Response.Write("Action执行之前" + Message + "<br />");
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            //filterContext.HttpContext.Response.Write("Action执行之后" + Message + "<br />");
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            //filterContext.HttpContext.Response.Write("返回Result之前" + Message + "<br />");
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            //filterContext.HttpContext.Response.Write("返回Result之后" + Message + "<br />");
        }
    }
}