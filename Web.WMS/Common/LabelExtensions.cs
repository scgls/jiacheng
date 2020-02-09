using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WMS.Web.Common
{
    public static class LabelExtensions
    {
        public static MvcHtmlString ToDate(this HtmlHelper helper, object date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date.ToString());
                string str = String.Format("<label>{0}</label>", dt.ToString("yyyy/MM/dd"));
                return new MvcHtmlString(str);
            }
            catch
            {
                string str = String.Format("<label>{0}</label>", "");
                return new MvcHtmlString(str);
            }
        }

        public static MvcHtmlString ToDateTime(this HtmlHelper helper, object datetime)
        {
            try
            {
                DateTime dt = DateTime.Parse(datetime.ToString());
                string str = String.Format("<label>{0}</label>", dt.ToString("yyyy/MM/dd HH:mm:ss"));
                return new MvcHtmlString(str);
            }
            catch
            {
                string str = String.Format("<label>{0}</label>", "");
                return new MvcHtmlString(str);
            }
        }
    }
}