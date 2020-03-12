using BILWeb.AdvInStock;
using BILWeb.InStock;
using BILWeb.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Print
{
    [RoleActionFilter(Message = "Print/PrintInstock")]
    public class PrintInstockController :  BaseController<T_InStockDetailInfo>
    {
        private IInStockDetailService iInStockDetailService;
        public PrintInstockController()
        {
            iInStockDetailService = (IInStockDetailService)ServiceFactory.CreateObject("InStock.T_InStockDetail_Func");
            baseservice = iInStockDetailService;
        }


        public JsonResult GetEAN(string materialno) {
            try
            {
                T_Material_DB t_Material_DB = new T_Material_DB();
                string EAN = t_Material_DB.getEAN(materialno);
                if (!string.IsNullOrEmpty(EAN))
                {
                    return Json(new { state = true, obj = EAN }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { state = false}, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception)
            {
                return Json(new { state = false }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetT_MaterialPackADF(string edate, string materialno)
        {
            try
            {
                MaterialPack_Func func = new MaterialPack_Func();
                string strError = "";
                string QUALITYDAY = func.GetQUALITYDAY(materialno, ref strError);
                if (!string.IsNullOrEmpty(QUALITYDAY))
                {
                    string batchno = Convert.ToDateTime(edate).AddDays(0 - Convert.ToInt32(QUALITYDAY)).ToString("yyyyMMdd");
                    return Json(new { state = true, obj = batchno }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { state = false ,obj= strError }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { state = false ,obj= ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }

        
    }
}