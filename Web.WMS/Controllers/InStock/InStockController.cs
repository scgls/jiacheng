using BILBasic.Common;
using BILWeb.InStock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers
{
    [RoleActionFilter(Message = "InStock/InStock")]
    public class InStockController : BaseController<T_InStockInfo>
    {

        private IInStockService inStockService;
        public InStockController()
        {
            inStockService = (IInStockService)ServiceFactory.CreateObject("InStock.T_InStock_Func");
            baseservice = inStockService;
        }

        //T_InStock_Func tfunc = new T_InStock_Func();
        T_InStockDetail_Func tfunc_detail = new T_InStockDetail_Func();

        //public ActionResult GetModelList(DividPage page, T_InStockInfo model)
        //{
        //    List<T_InStockInfo> modelList = new List<T_InStockInfo>();
        //    string strError = "";
        //    tfunc.GetModelListByPage(ref modelList, Common.Commom.currentUser, model, ref page, ref strError);
        //    ViewData["PageData"] = new PageData<T_InStockInfo> { data = modelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/OutStock/GetModelList") };
        //    return View("InStockList", model);
        //}

        public JsonResult GetDetail(Int32 ID)
        {
            List<T_InStockDetailInfo> modelList = new List<T_InStockDetailInfo>();
            string strError = "";
            //tfunc.GetModelListByHeaderID(ref modelList,ID,ref strError);
            tfunc_detail.GetModelListByHeaderID(ref modelList, ID, ref strError);
            return Json(modelList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Sync(string ErpVoucherNo)
        {
            string ErrorMsg = ""; int WmsVoucherType = -1; string syncType = "ERP"; int syncExcelVouType = -1; DataSet excelds = null;
            BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
            //10:入库单据
            if (PFunc.Sync(10, string.Empty, ErpVoucherNo, WmsVoucherType, ref ErrorMsg, syncType, syncExcelVouType, excelds))
            {

                return Json(new { state = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { state = false, obj = ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
           
        }
    }
}