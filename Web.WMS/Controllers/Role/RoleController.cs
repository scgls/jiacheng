using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.RuleAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers
{
    [RoleActionFilter(Message = "Basic/Role")]
    public class RoleController : BaseController<T_RuleAllInfo>
    {
        private IRuleAllService ruleAllService;
        public RoleController()
        {
            ruleAllService = (IRuleAllService)ServiceFactory.CreateObject("RuleAll.T_RuleAll_Func");
            baseservice = ruleAllService;
        }

        //T_OutStockDetail_Func tfunc_detail = new T_OutStockDetail_Func();

        //public JsonResult GetDetail(Int32 ID)
        //{
        //    List<T_OutStockDetailInfo> modelList = new List<T_OutStockDetailInfo>();
        //    string strError = "";
        //    tfunc_detail.GetModelListByHeaderID(ref modelList, ID, ref strError);
        //    return Json(modelList, JsonRequestBehavior.AllowGet);
        //}



        //T_RuleAll_Func func = new T_RuleAll_Func();
        //// GET: Role
        //public ActionResult Index()
        //{
        //    return View();
        //}


        //public ActionResult RoleList(DividPage page,T_RuleAllInfo ruleAllInfo)
        //{
        //    List<T_RuleAllInfo> modelList = new List<T_RuleAllInfo>();
        //    string strError = "";
        //    func.GetModelListByPage(ref modelList, Common.Commom.currentUser, ruleAllInfo, ref page, ref strError);
        //    ViewData["modelList"] = modelList;
        //    return View();
        //}


        public JsonResult update(T_RuleAllInfo t_RuleAllInfo)
        {
            string strError = "";
             bool isSuccess = ruleAllService.UpadteModelByModelSql(currentUser, t_RuleAllInfo,ref strError);
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }
    }
}