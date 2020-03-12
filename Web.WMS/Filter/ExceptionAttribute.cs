using System.Web.Mvc;

namespace WMS.Web.Filter
{
    public class ExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                filterContext.Result = new RedirectResult("~/Views/Shared/Error.cshtml");
            }
            base.OnException(filterContext);
        }
    }
}