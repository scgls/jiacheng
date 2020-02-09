using BILBasic.Common;
using BILWeb.House;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.WMS.Common;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Warehouse
{
    [RoleActionFilter(Message = "Basic/House")]
    public class HouseController :   BaseController<T_HouseInfo>
    {
        private IHouseService houseService;
        public HouseController()
        {
            houseService = (IHouseService)ServiceFactory.CreateObject("House.T_House_Func");
            baseservice = houseService;
        }

        public JsonResult DeleteBymodel(string ID)
        {
            string strMsg = "";
            houseService.DeleteModelByModel(currentUser, new T_HouseInfo { ID = Convert.ToInt32(ID) }, ref strMsg);
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        //T_House_Func tfunc = new T_House_Func();

        //// GET: House
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult GetModelList(DividPage page, T_HouseInfo model)
        //{
        //    List<T_HouseInfo> modelList = new List<T_HouseInfo>();
        //    string strError = "";
        //    tfunc.GetModelListByPage(ref modelList, Commom.currentUser, model, ref page, ref strError);
        //    ViewData["PageData"] = new PageData<T_HouseInfo> { data = modelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/House/GetModelList") };
        //    return View("HouseList", model);
        //}

        //[HttpGet]
        //public ActionResult GetModel(T_HouseInfo model)
        //{
        //    string strMsg = "";
        //    tfunc.GetModelByID(ref model, ref strMsg);
        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Edit(T_HouseInfo model)
        //{
        //    string strMsg = "";
        //    tfunc.GetModelByID(ref model, ref strMsg);
        //    return View(model);
        //}

        //[HttpPost]
        //public JsonResult Delect(string ID)
        //{
        //    string strMsg = "";
        //    string[] Ids = ID.TrimEnd(',').Split(',');
        //    foreach (string id in Ids)
        //    {
        //        tfunc.DeleteModelByModelSql(Common.Commom.currentUser, new T_HouseInfo { ID = Convert.ToInt32(id) }, ref strMsg);
        //    }
        //    return Json(strMsg, JsonRequestBehavior.AllowGet);
        //}
    }
    }