using BILWeb.PickCar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Car
{
    [RoleActionFilter(Message = "Basic/Car")]
    public class CarController : BaseController<T_PickCarInfo>
    {
        // GET: Car
        private IPickCar pickCarService;
        public CarController()
        {
            pickCarService = (IPickCar)ServiceFactory.CreateObject("PickCar.T_PickCar_Func");
            baseservice = pickCarService;
        }


        public JsonResult Shifang(string carno)
        {
            T_PickCar_DB db = new T_PickCar_DB();
            string ErrMsg = "";
            if (db.UpdateCar(carno)>0)
            {
                ErrMsg = "释放成功！";
            }
            else
            {
                ErrMsg = "释放失败！";
            }
            return Json(ErrMsg, JsonRequestBehavior.AllowGet);
        }
    }
}