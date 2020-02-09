using BILWeb.BaseInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.WMS.Controllers.Manage
{
    public class ManageController : Controller
    {
        T_System_Func funcsystem = new T_System_Func();
        // GET: Manage
        public ActionResult GetModel(bool isTrue=false,bool isadd=false)
        {
            T_System model = new T_System();
            List<T_System> tsystem =funcsystem.GetModel();
            if (tsystem != null && tsystem.Count>0)
            {
                model = tsystem[0];
            }
            string isSuccess = "";
            if (isadd) {
                if (isTrue)
                {
                    isSuccess = "true";
                }
                else {
                    isSuccess = "false";
                }
            }
            ViewData["isSuccess"] = isSuccess;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(T_System model, HttpPostedFileBase file)
        {
            string fileOne = "";
            if (file != null && file.ContentLength != 0)
            {
                fileOne = DateTime.Now.ToString("yyyyMMddHHmmssff") + "1" + System.IO.Path.GetExtension(file.FileName);
                var fileName = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(fileOne));
                file.SaveAs(fileName);
            }
            string strMsg = "";
            model.filepath = fileOne;
            bool isSuccess = funcsystem.SaveData(model, ref strMsg);
            //return Json(new { state = isSuccess, obj = model }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("GetModel", new { isTrue=isSuccess ,isadd=true});
        }

    }
}