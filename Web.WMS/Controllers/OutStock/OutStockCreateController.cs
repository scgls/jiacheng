using BILWeb.OutStockCreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.OutStock
{
    [RoleActionFilter(Message = "OutStockCreate/OutStockCreate")]
    public class OutStockCreateController : BaseController<T_OutStockCreateInfo>
    {
        private IOutStockCreateService outStockCreateService;
        // GET: OutStockCreate
        public OutStockCreateController()
        {
            outStockCreateService = (IOutStockCreateService)ServiceFactory.CreateObject("OutStockCreate.T_OutStockCreate_Func");
            baseservice = outStockCreateService;
        }

        T_OutStockCreate_Func tfunc_OutStockCreate = new T_OutStockCreate_Func();

        [HttpPost]
        public JsonResult Shengdan(string ID)
        {
            try
            {
                string strError = string.Empty;
                List<T_OutStockCreateInfo> lstCreate = new List<T_OutStockCreateInfo>();
                string[] strId = ID.Split(',');
                for (int i = 0; i < strId.Length; i++)
                {
                    if (!string.IsNullOrEmpty(strId[i]))
                    {
                        T_OutStockCreateInfo model = new T_OutStockCreateInfo();
                        model.ID =Convert.ToInt32(strId[i]);
                        if (!tfunc_OutStockCreate.GetModelByID(ref model, ref strError)) {
                            return Json(new { state = false, obj = strError }, JsonRequestBehavior.AllowGet);
                        }
                        lstCreate.Add(model);
                    }
                }
                lstCreate.ForEach(t => t.OKSelect = true);
                if (!tfunc_OutStockCreate.SaveModelListBySqlToDB(currentUser, lstCreate, ref strError))
                {
                    return Json(new { state = false, obj = strError }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { state = true, obj = "拣货单生成成功！" }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}