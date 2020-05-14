
using BILWeb.YS;
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
    [RoleActionFilter(Message = "YS/YS")]
    public class YSController : BaseController<T_YS>
    {

        private IYSService ysService;
        public YSController()
        {
            ysService = (IYSService)ServiceFactory.CreateObject("YS.T_YS_Func");
            baseservice = ysService;
        }

        T_YSDetail_Func tfunc_detail = new T_YSDetail_Func();


        public JsonResult GetDetail(Int32 ID)
        {
            List<T_YSDetailInfo> modelList = new List<T_YSDetailInfo>();
            string strError = "";
            tfunc_detail.GetModelListByHeaderID(ref modelList, ID, ref strError);
            return Json(modelList, JsonRequestBehavior.AllowGet);
        }

    }
}