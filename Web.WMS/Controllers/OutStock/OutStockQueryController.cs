using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.OutStock;
using BILWeb.OutStockCreate;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers
{
    [RoleActionFilter(Message = "OutStockQuery/OutStockQuery")]
    public class OutStockQueryController :  BaseController<T_OutStockInfo>
    {
        private IOutStockService outStockService;
        //UserInfo currentUser = Common.Commom.ReadUserInfo();
        public OutStockQueryController()
        {
            outStockService = (IOutStockService)ServiceFactory.CreateObject("OutStock.T_OutStock_Func");
            baseservice = outStockService;
        }

        T_OutStockDetail_Func tfunc_detail = new T_OutStockDetail_Func();
        
        public JsonResult GetDetail(Int32 ID)
        {
            List<T_OutStockDetailInfo> modelList = new List<T_OutStockDetailInfo>();
            string strError = "";
            tfunc_detail.GetModelListByHeaderID(ref modelList, ID, ref strError);
            return Json(modelList, JsonRequestBehavior.AllowGet);
        }



        public JsonResult Sync(string ErpVoucherNo)
        {
            T_OutStock_Func func = new T_OutStock_Func();
            if (func.GetOutStockNoIsExists(ErpVoucherNo) > 0)
            {
                return Json(new { state = false, obj = "该ERP单号已经存在" }, JsonRequestBehavior.AllowGet);
            }

            string ErrorMsg = ""; int WmsVoucherType = -1; string syncType = "ERP"; int syncExcelVouType = -1; DataSet excelds = null;
            BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
            //20:出库单据
            if (PFunc.Sync(20, string.Empty, ErpVoucherNo, WmsVoucherType, ref ErrorMsg, syncType, syncExcelVouType, excelds))
            {

                return Json(new { state = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { state = false, obj = ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddBySqlForXT(T_OutStockInfo modeladd)
        {
            string strMsg = "";
            if (baseservice.SaveModelBySqlToDB(currentUser, ref modeladd, ref strMsg))
            {
                //return RedirectToRoute("/outstockquery/getmodellist?VoucherType=24");
                Response.Redirect("/outstockquery/getmodellist?VoucherType=24");
                return View();
            }
            else
            {
                this.TempData["modeladd"] = modeladd;
                return RedirectToAction("GetModel", new { model = modeladd, strMsg = strMsg });
            }
        }

        public FileResult Excel(T_OutStockInfo model)
        {
            string strErrMsg = string.Empty;
            List<T_OutStockInfo> lstExport = new List<T_OutStockInfo>();
            string strError = "";
            DividPage page = new DividPage
            {
                CurrentPageShowCounts = 1000000
            };
            outStockService.GetModelListByPage(ref lstExport, currentUser, model, ref page, ref strError);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("单据号");
            row1.CreateCell(1).SetCellValue("据点名");
            row1.CreateCell(2).SetCellValue("效期");
            row1.CreateCell(3).SetCellValue("ERP单号");
            row1.CreateCell(4).SetCellValue("单据类型");
            row1.CreateCell(5).SetCellValue("状态");
            row1.CreateCell(6).SetCellValue("部门");
            row1.CreateCell(7).SetCellValue("创建人");
            row1.CreateCell(8).SetCellValue("创建时间");
            row1.CreateCell(9).SetCellValue("提交时间");
            row1.CreateCell(10).SetCellValue("供应商");
            row1.CreateCell(11).SetCellValue("电话");
            row1.CreateCell(12).SetCellValue("联系人");
            row1.CreateCell(13).SetCellValue("发货地址");
            row1.CreateCell(14).SetCellValue("物流地址");
            row1.CreateCell(15).SetCellValue("等通知发货");
            row1.CreateCell(16).SetCellValue("等外调/等单"); 
            row1.CreateCell(17).SetCellValue("交易条件");
            row1.CreateCell(18).SetCellValue("省");
            row1.CreateCell(19).SetCellValue("市");
            row1.CreateCell(20).SetCellValue("区");
            row1.CreateCell(21).SetCellValue("ERP备注");
            row1.CreateCell(22).SetCellValue("备注");
            row1.CreateCell(23).SetCellValue("客户");
            //将数据逐步写入seet1各个行
            for (int i = 0; i < lstExport.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);

                rowtemp.CreateCell(0).SetCellValue(lstExport[i].VoucherNo == null ? "" : lstExport[i].VoucherNo.ToString());
                rowtemp.CreateCell(1).SetCellValue(lstExport[i].StrongHoldName == null ? "" : lstExport[i].StrongHoldName.ToString());
                rowtemp.CreateCell(2).SetCellValue(lstExport[i].EDate == null ? "" : lstExport[i].EDate.ToString());
                rowtemp.CreateCell(3).SetCellValue(lstExport[i].ErpVoucherNo == null ? "" : lstExport[i].ErpVoucherNo.ToString());
                rowtemp.CreateCell(4).SetCellValue(lstExport[i].StrVoucherType == null ? "" : lstExport[i].StrVoucherType.ToString());
                rowtemp.CreateCell(5).SetCellValue(lstExport[i].StrStatus == null ? "" : lstExport[i].StrStatus.ToString());
                rowtemp.CreateCell(6).SetCellValue(lstExport[i].DepartmentName == null ? "" : lstExport[i].DepartmentName.ToString());
                rowtemp.CreateCell(7).SetCellValue(lstExport[i].StrCreater == null ? "" : lstExport[i].StrCreater.ToString());
                rowtemp.CreateCell(8).SetCellValue(lstExport[i].CreateTime.ToString());
                rowtemp.CreateCell(9).SetCellValue(lstExport[i].PostDate.ToString());
                rowtemp.CreateCell(10).SetCellValue(lstExport[i].SupName == null ? "" : lstExport[i].SupName.ToString()); 
                rowtemp.CreateCell(11).SetCellValue(lstExport[i].Phone == null ? "" : lstExport[i].Contact.ToString()); 
                rowtemp.CreateCell(12).SetCellValue(lstExport[i].Contact == null ? "" : lstExport[i].Phone.ToString()); 
                rowtemp.CreateCell(13).SetCellValue(lstExport[i].Address == null ? "" : lstExport[i].Address.ToString()); 
                rowtemp.CreateCell(14).SetCellValue(lstExport[i].Address1 == null ? "" : lstExport[i].Address1.ToString()); 
                rowtemp.CreateCell(15).SetCellValue(lstExport[i].ShipNFlg == null ? "" : lstExport[i].ShipNFlg.ToString()); 
                rowtemp.CreateCell(16).SetCellValue(lstExport[i].ShipWFlg == null ? "" : lstExport[i].ShipWFlg.ToString()); 
                rowtemp.CreateCell(17).SetCellValue(lstExport[i].TradingConditionsName == null ? "" : lstExport[i].TradingConditionsName.ToString());
                rowtemp.CreateCell(18).SetCellValue(lstExport[i].Province == null ? "" : lstExport[i].Province.ToString());
                rowtemp.CreateCell(19).SetCellValue(lstExport[i].City == null ? "" : lstExport[i].City.ToString());
                rowtemp.CreateCell(20).SetCellValue(lstExport[i].Area == null ? "" : lstExport[i].Area.ToString());
                rowtemp.CreateCell(21).SetCellValue(lstExport[i].ERPNote == null ? "" : lstExport[i].ERPNote.ToString());
                rowtemp.CreateCell(22).SetCellValue(lstExport[i].Note == null ? "" : lstExport[i].Note.ToString());
                rowtemp.CreateCell(23).SetCellValue(lstExport[i].CustomerName == null ? "" : lstExport[i].CustomerName.ToString());
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }


        //关闭单据
        [HttpPost]
        public JsonResult CloseOutstock(string ID)
        {
            try
            {
                string strError = "";
                T_OutStock_Func tfunc = new T_OutStock_Func();
                if (tfunc.CloseOutStockVoucherNo(Convert.ToInt32(ID),currentUser, ref strError))
                {
                    return Json(new { state = true, obj = strError }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { state = false, obj = strError }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}