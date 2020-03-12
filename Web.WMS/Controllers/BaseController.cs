using BILBasic.Basing;
using BILBasic.Common;
using BILWeb;
using BILWeb.Login.User;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.WMS.Controllers
{
    public abstract class BaseController<Tmodel> : Controller 
    {
        protected IBaseService<Tmodel> baseservice;
        protected UserInfo currentUser = Common.Commom.ReadUserInfo();
        public ActionResult PageView(PageData<Tmodel> model)
        {
            ViewData["dividPage"] = model.dividPage;
            ViewData["url"] = model.link;
            return View("_ViewPage");
        }

        public ActionResult GetModelList(DividPage page, Tmodel model)
        {
            List<Tmodel> modelList = new List<Tmodel>();
            string strError = "";
            baseservice.GetModelListByPage(ref modelList, currentUser, model, ref page, ref strError);
            ViewData["PageData"] = new PageData<Tmodel> { data = modelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/" + System.Web.HttpContext.Current.Session["Message"].ToString() + "/GetModelList") };
            return View("GetModelList", model);
        }

        [HttpGet]
        public ActionResult GetModel(Tmodel model, string strMsg)
        {
            if (model == null)
            {
                model = (Tmodel)this.TempData["modeladd"];
            }

            string Msg = "";
            baseservice.GetModelByID(ref model, ref Msg);
            ViewData["Msg"] = Msg;
            return View(model);
        }


        [HttpGet]
        public ActionResult ErrorMsg(string strMsg)
        {
            ViewData["Msg"] = strMsg;
            return View("_ViewErrorMsg");
        }

       // [HttpPost]
        public JsonResult Delete(string ID)
        {
            string strMsg = "";
            string[] Ids = ID.TrimEnd(',').Split(',');
            foreach (string id in Ids)
            {
                baseservice.DeleteModelByID(currentUser, Convert.ToInt32(id), ref strMsg);
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult Add(Tmodel modeladd)
        {
            string strMsg = "";
            if (baseservice.SaveModelToDB(currentUser, ref modeladd, ref strMsg))
            {
                return RedirectToAction("GetModelList");
            }
            else
            {
                this.TempData["modeladd"] = modeladd;
                return RedirectToAction("GetModel", new { model = modeladd, strMsg = strMsg });
            }
        }

        [HttpPost]
        public ActionResult AddBySql(Tmodel modeladd)
        {
            string strMsg = "";
            if (baseservice.SaveModelBySqlToDB(currentUser, ref modeladd, ref strMsg))
            {
                return RedirectToAction("GetModelList");
            }
            else
            {
                this.TempData["modeladd"] = modeladd;
                return RedirectToAction("GetModel", new { model = modeladd, strMsg = strMsg });
            }
        }

        [HttpPost]
        public ActionResult Addsql(Tmodel modeladd)
        {
            string strMsg = "";
            if (baseservice.SaveModelBySqlToDB(currentUser, ref modeladd, ref strMsg))
            {
                return RedirectToAction("GetModelList");
            }
            else
            {
                this.TempData["modeladd"] = modeladd;
                return RedirectToAction("GetModel", new { model = modeladd, strMsg = strMsg });
            }
        }

        public string ErrorController(object obj)
        {
            return "<script>alert('" + obj.ToString() + "')</script>";
        }





        //public ActionResult Modelist(Tmodel tmodel)
        //{
        //    DividPage page = new DividPage();
        //    List<Tmodel> modelList = new List<Tmodel>();
        //    string strError = "";
        //    baseservice.GetModelListByPage(ref modelList, Common.Commom.currentUser, tmodel, ref page, ref strError);
        //    ViewData["modelList"] = modelList;
        //    return View();
        //}




    }
}