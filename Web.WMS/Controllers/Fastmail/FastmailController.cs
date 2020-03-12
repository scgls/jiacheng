using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.WMS.Controllers.Fastmail
{
    public class FastmailController : Controller
    {
        // GET: 快递接口
        public ActionResult GetModelList()
        {
            return View();
        }
    }
}