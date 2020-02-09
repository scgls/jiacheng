using BILBasic.Common;
using BILWeb.Check;
using BILWeb.Login.User;
using BILWeb.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WMS.Web.Filter;

namespace Web.WMS.Controllers
{
    [RoleActionFilter(Message = "Check/Check")]
    public class CheckController : Controller
    {
         UserInfo currentUser = Common.Commom.ReadUserInfo();
        Check_DB tfun = new Check_DB();

        public ActionResult PageView(PageData<Check_Model> model)
        {
            ViewData["dividPage"] = model.dividPage;
            ViewData["url"] = model.link;
            return View("_ViewPage");
        }

        public ActionResult GetModelList(DividPage page, Check_Model model)
        {
            List<Check_Model> modelList = new List<Check_Model>();
            string strError = "";
            tfun.GetCheckInfo(model, ref page, ref modelList, ref strError);
            ViewData["PageData"] = new PageData<Check_Model> { data = modelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/Check/GetModelList") };
            return View("GetModelList", model);
        }


        [HttpPost]
        public ActionResult Add(Check_Model model,string strAll)
        {
            model.CHECKSTATUS = "新建";
            model.CHECKTYPE = "货位";
            model.CREATER = currentUser.UserName;
            List<CheckArea_Model> list = new List<CheckArea_Model>();
            string[] strsp = strAll.Split(';');
            foreach (string item in strsp)
            {
                if (item.Contains(','))
                {
                    CheckArea_Model m = new CheckArea_Model();
                    string[] strsp2 = item.Split(',');
                    m.AREANO = strsp2[1];
                    m.ID =Convert.ToInt32(strsp2[0]);
                    list.Add(m);
                }
            }
            string ErrMsg = "";
            bool bSucc = tfun.SaveCheck(model, list, ref ErrMsg);
            if (!bSucc)
            {
                ErrMsg="保存失败！" + ErrMsg;
            }
            else
            {
                ErrMsg = "保存成功！";
            }
            return Json(ErrMsg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult shanchu(string CHECKNO)
        {
            string ErrMsg = "";
            if (tfun.DelCloCheck(CHECKNO, 0, ref ErrMsg, ""))
            {
                ErrMsg = "删除成功！";
            }
            else
            {
                ErrMsg = "删除失败！" + ErrMsg;
            }
            return Json(ErrMsg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult zhongzhi(string CHECKNO)
        {
            string ErrMsg = "";
            if (tfun.DelCloCheck(CHECKNO, 1, ref ErrMsg, ""))
            {
                ErrMsg="终止成功！";
            }
            else
            {
                ErrMsg = "终止失败！"+ErrMsg;
            }
            return Json(ErrMsg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Delete(Check_Model model)
        {
            return RedirectToAction("GetModelList");
        }

        [HttpGet]
        public ActionResult GetModel (Check_Model model)
        {
            Check_Func db = new Check_Func();
            ViewData["Taskno"] = db.GetTableID("SEQ_CHECK_NO").ToString();
            return View();
        }

        
        public JsonResult GetDetail(int Svalue,string areano,string houseno,string warehouseno) {
            Thread.Sleep(5000);
            List<CheckArea_Model> lsttask = new List<CheckArea_Model>();
            tfun.GetCheckArea(Svalue, areano, houseno, warehouseno, ref lsttask);
            return Json(lsttask, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckAnalyze(DividPage page, CheckAnalyze model)
        {
            List<CheckAnalyze> modelList = new List<CheckAnalyze>();
            string strError = "";
            tfun.GetCheckAnalyze(model, ref page, ref modelList, ref strError);
            ViewData["PageData"] = new PageData<CheckAnalyze> { data = modelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/Check/CheckAnalyze") };
            return View("CheckAnalyze", model);
        }
        public ActionResult PageView1(PageData<CheckAnalyze> model)
        {
            ViewData["dividPage"] = model.dividPage;
            ViewData["url"] = model.link;
            return View("_ViewPage");
        }

        public JsonResult tiaozheng(string CHECKNO) {
            if (currentUser == null)
            {
                return Json("Cookie失效，重新登陆！" , JsonRequestBehavior.AllowGet);
            }
            string ErrMsg = "";
            Check_DB db = new Check_DB();
            if (db.DelCloCheck(CHECKNO, 2, ref ErrMsg, currentUser.UserNo))
            {
                return Json("调整成功！", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ErrMsg, JsonRequestBehavior.AllowGet);
            }
        }


        
        public JsonResult tiaozheng_ms(string CHECKNO)
        {
            if (currentUser == null)
            {
                return Json("Cookie失效，重新登陆！", JsonRequestBehavior.AllowGet);
            }
            string ErrMsg = "";
            Check_DB db = new Check_DB();
            if (db.DelCloCheck_MsTest(CHECKNO, 2, ref ErrMsg, currentUser.UserNo))
            {
                return Json("调整成功！"+ ErrMsg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ErrMsg, JsonRequestBehavior.AllowGet);
            }
        }
    }
}