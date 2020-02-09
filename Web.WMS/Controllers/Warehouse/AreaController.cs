using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BILWeb.Area;
using BILBasic.Common;
using Web.WMS.Common;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Warehouse
{
    [RoleActionFilter(Message = "Basic/Area")]
    public class AreaController :   BaseController<T_AreaInfo>
    {
        private IAreaService areaService;
        public AreaController()
        {
            areaService = (IAreaService)ServiceFactory.CreateObject("Area.T_Area_Func");
            baseservice = areaService;
        }


        public JsonResult DeleteBymodel(string ID)
        {
            string strMsg = "";
            areaService.DeleteModelByModel(currentUser, new T_AreaInfo { ID = Convert.ToInt32(ID) }, ref strMsg);
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        //T_Area_Func tfunc = new T_Area_Func();
        //// GET: Area
        //public ActionResult GetModelList(DividPage page, T_AreaInfo model)
        //{

        //    List<T_AreaInfo> modelList = new List<T_AreaInfo>();
        //    string strError = "";
        //    tfunc.GetModelListByPage(ref modelList, Commom.currentUser, model, ref page, ref strError);
        //    ViewData["PageData"] = new PageData<T_AreaInfo> { data = modelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/Area/GetModelList") };
        //    return View("AreaList", model);
        //}

        //[HttpPost]
        //public ActionResult Add(T_AreaInfo model)
        //{
        //    string strMsg = "";
        //    if (tfunc.SaveModelToDB(Common.Commom.currentUser, ref model, ref strMsg))
        //    {
        //        return RedirectToAction("Edit");
        //    }
        //    else
        //    {
        //        return RedirectToAction("GetModelList");
        //    }
        //}


        //[HttpGet]
        //public ActionResult GetModel(T_AreaInfo model)
        //{
        //    string strMsg = "";
        //    tfunc.GetModelByID(ref model, ref strMsg);
        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Edit(T_AreaInfo model)
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
        //        tfunc.DeleteModelByModelSql(Common.Commom.currentUser, new T_AreaInfo { ID = Convert.ToInt32(id) }, ref strMsg);
        //    }
        //    return Json(strMsg, JsonRequestBehavior.AllowGet);
        //}

    }
}