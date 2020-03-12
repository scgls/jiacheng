using BILBasic.Common;
using BILWeb.Material;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using Web.WMS.Common;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Material
{
    [RoleActionFilter(Message = "Basic/Material")]
    public class MaterialController : BaseController<T_MaterialInfo>
    {
        private IMaterialService materialService;
        public MaterialController()
        {
            materialService = (IMaterialService)ServiceFactory.CreateObject("Material.T_Material_Func");
            baseservice = materialService;
        }



        public JsonResult Sync(string MaterialNo)
        {
            string ErrorMsg = ""; int WmsVoucherType = -1; string syncType = "ERP"; int syncExcelVouType = -1; DataSet excelds = null;
            BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
           
            if (PFunc.Sync(99, string.Empty, MaterialNo, WmsVoucherType, ref ErrorMsg, syncType, syncExcelVouType, excelds))
            {

                return Json(new { state = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { state = false, obj = ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }


        //T_Material_Func tfunc = new T_Material_Func();

        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult GetModelList(DividPage page, T_MaterialInfo model)
        //{
        //    string strMsg = "";
        //    List<T_MaterialInfo> gmodelList = new List<T_MaterialInfo>();
        //    tfunc.GetModelListByPage(ref gmodelList, Commom.currentUser, model, ref page, ref strMsg);
        //    ViewData["PageData"] = new PageData<T_MaterialInfo> { data = gmodelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/Material/GetModelList") };
        //    return View("MaterialList");
        //}

        //[HttpGet]
        //public ActionResult GetModel(T_MaterialInfo model)
        //{
        //    string strMsg = "";
        //    tfunc.GetModelByID(ref model, ref strMsg);
        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Add(T_MaterialInfo model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string strMsg = "";
        //        if (tfunc.SaveModelBySqlToDB(Common.Commom.currentUser, ref model, ref strMsg))
        //        {
        //            return RedirectToAction("GetModelList");
        //        }
        //    }
        //    return RedirectToAction("GetModel", model);

        //}

        //[HttpPost]
        //public JsonResult Delect(string ID)
        //{
        //    string strMsg = "";
        //    string[] Ids = ID.TrimEnd(',').Split(',');
        //    foreach (string id in Ids)
        //    {
        //        tfunc.DeleteModelByModelSql(Common.Commom.currentUser, new T_MaterialInfo { ID = Convert.ToInt32(id) }, ref strMsg);
        //    }
        //    return Json(strMsg, JsonRequestBehavior.AllowGet);
        //}
    }
}