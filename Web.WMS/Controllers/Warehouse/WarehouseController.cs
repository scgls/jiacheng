using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BILWeb.Warehouse;
using BILBasic.Common;
using Web.WMS.Common;
using WMS.Factory;
using NPOI.SS.UserModel;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Warehouse
{
    [RoleActionFilter(Message = "Basic/Warehouse")]
    public class WarehouseController : BaseController<T_WareHouseInfo>
    {
        //T_WareHouse_Func tfunc = new T_WareHouse_Func();
        private IWarehouseService warehouseService;
        public WarehouseController()
        {
            warehouseService = (IWarehouseService)ServiceFactory.CreateObject("Warehouse.T_WareHouse_Func");
            baseservice = warehouseService;
        }

        public JsonResult DeleteBymodel(string ID)
        {
            string strMsg = "";
            if (warehouseService.DeleteModelByModel(currentUser, new T_WareHouseInfo { ID = Convert.ToInt32(ID) }, ref strMsg))
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }else
            {
                return Json(strMsg, JsonRequestBehavior.AllowGet);
            }
            
        }


        //internal override void ListWriter(ISheet sheet1)
        //{
        //    throw new NotImplementedException();
        //}

        //public PartialViewResult GetModelList(DividPage page, T_WareHouseInfo model)
        //{
        //    List<T_WareHouseInfo> modelList = new List<T_WareHouseInfo>();
        //    string strError = "";
        //    tfunc.GetModelListByPage(ref modelList, Commom.currentUser, model, ref page, ref strError);
        //    ViewData["PageData"] = new PageData<T_WareHouseInfo> { data = modelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/Warehouse/GetModelList") };
        //    return PartialView("WarehouseList", model);
        //}

        //[HttpGet]
        //public ActionResult GetModel(T_WareHouseInfo model)
        //{
        //    string strMsg = "";
        //    tfunc.GetModelByID(ref model, ref strMsg);
        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Edit(T_WareHouseInfo model)
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
        //        tfunc.DeleteModelByModelSql(Common.Commom.currentUser, new T_WareHouseInfo { ID = Convert.ToInt32(id) }, ref strMsg);
        //    }
        //    return Json(strMsg, JsonRequestBehavior.AllowGet);
        //}



        //// GET: Warehouse
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: Warehouse/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Warehouse/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Warehouse/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Warehouse/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Warehouse/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Warehouse/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Warehouse/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
