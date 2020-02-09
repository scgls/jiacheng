using BILBasic.Common;
using BILWeb.Box;
using BILWeb.OutStock;
using BILWeb.OutStockCreate;
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
    [RoleActionFilter(Message = "OutStock/OutStock")]
    public class OutStockController : BaseController<T_OutStockInfo>
    {
        private IOutStockService outStockService;
        public OutStockController()
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

        T_OutStockCreate_Func tfunc_OutStockCreate = new T_OutStockCreate_Func();
        [HttpPost]
        public JsonResult Shengdan(string ID)
        {
            try
            {
                if (currentUser == null)
                {
                    return Json(new { state = false, obj = "Cookie失效，重新登陆！" }, JsonRequestBehavior.AllowGet);
                }
                //获取全部的生单明细
                T_OutStockCreateInfo createModel = new T_OutStockCreateInfo();
                string strErrMsg = "";
                string cHeaderID = string.Empty;
                List<T_OutStockCreateInfo> lstCreate = new List<T_OutStockCreateInfo>();
                ID = ID.TrimEnd(',');
                createModel.SelectHeaderID = ID;
                DividPage _serverPage = new DividPage();
                _serverPage.CurrentPageShowCounts = 100000;
                if (!tfunc_OutStockCreate.GetModelListByPage(ref lstCreate, currentUser, createModel, ref _serverPage, ref strErrMsg))
                {
                    return Json(new { state = false, obj = strErrMsg }, JsonRequestBehavior.AllowGet);
                }


                lstCreate.ForEach(t => t.OKSelect = true);
                if (!tfunc_OutStockCreate.SaveModelListBySqlToDB(currentUser, lstCreate, ref strErrMsg))
                {
                    return Json(new { state = false, obj = strErrMsg }, JsonRequestBehavior.AllowGet);
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


        [HttpPost]
        public JsonResult Shengxiang(string ID,string HeaderName)
        {
            try
            {
                if (currentUser == null)
                {
                    return Json(new { state = false, obj = "Cookie失效，重新登陆！" }, JsonRequestBehavior.AllowGet);
                }
                string strErrMsg = "";
                T_Box_Func boxFunc = new T_Box_Func();
                ID = ID.TrimEnd(',');
                string[] strid = ID.Split(',');
                List<string> sArray =new List<string>(strid);
                if (!boxFunc.CreatePrintBoxInfo(currentUser, sArray, HeaderName, ref strErrMsg))
                {
                    return Json(new { state = false, obj = strErrMsg }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { state = true, obj = "箱号生成成功！" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult Print(string ID, string num)
        {
            T_OutStockInfo t_OutStockInfo = new T_OutStockInfo();
            t_OutStockInfo.ID = Convert.ToInt32(ID);
            string strMsg = "";
            if (outStockService.GetModelByID(ref t_OutStockInfo, ref strMsg)) {
                //
                return Json(new { state = false, obj = strMsg }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { state = false, obj = strMsg }, JsonRequestBehavior.AllowGet);
            }

            //string err = "";

            //每行打印
            //if (lstAdvInStockDetailInfo != null && lstAdvInStockDetailInfo.Count != 0)
            //{
            //    List<string> squence = GetSerialnos(lstAdvInStockDetailInfo.Count, ref err);
            //    int k = 0;
            //    for (int i = 0; i < lstAdvInStockDetailInfo.Count; i++)
            //    {
            //        Barcode_Model model = new Barcode_Model();
            //        model.MaterialNo = lstAdvInStockDetailInfo[i].MaterialNo;
            //        model.MaterialDesc = lstAdvInStockDetailInfo[i].MaterialDesc;
            //        model.BatchNo = lstAdvInStockDetailInfo[i].SupBatch;
            //        model.ErpVoucherNo = lstAdvInStockDetailInfo[i].ErpVoucherNo;
            //        model.EDate = Convert.ToDateTime(lstAdvInStockDetailInfo[i].EDate);
            //        model.Qty = Convert.ToDecimal(lstAdvInStockDetailInfo[i].AdvQty);
            //        model.StrongHoldCode = lstAdvInStockDetailInfo[i].StrongHoldCode;
            //        model.SerialNo = squence[k++];
            //        model.Creater = Common.Commom.currentUser.UserNo;
            //        model.EAN = lstAdvInStockDetailInfo[i].EAN;
            //        model.ReceiveTime = Convert.ToDateTime(lstAdvInStockDetailInfo[i].CreateTime);
            //        model.BarCode = "1@" + model.StrongHoldCode + "@" + model.MaterialNo + "@" + model.EAN + "@" + model.EDate.ToString("yyyy-MM-dd") + "@" + model.BatchNo + "@" + model.Qty + "@" + model.SerialNo;
            //        model.RowNo = lstAdvInStockDetailInfo[i].RowNO;
            //        model.RowNoDel = lstAdvInStockDetailInfo[i].RowNODel;
            //        listbarcode.Add(model);
            //    }
            //}


            //if (print_DB.SubBarcodes(listbarcode, "sup", 1, ref err))
            //{
            //    string serialnos = "";
            //    for (int i = 0; i < listbarcode.Count; i++)
            //    {
            //        serialnos += listbarcode[i].SerialNo + ",";
            //    }
            //    return Json(new { state = true, obj = serialnos }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(new { state = false, obj = err }, JsonRequestBehavior.AllowGet);
            //}
        }

        public JsonResult Sync(string ErpVoucherNo)
        {
            T_OutStock_Func func = new T_OutStock_Func();
            if (func.GetOutStockNoIsExists(ErpVoucherNo) > 0) {
                return Json(new { state = false, obj = "该ERP单号已经存在" }, JsonRequestBehavior.AllowGet);
            }

            string ErrorMsg=""; int WmsVoucherType = -1; string syncType = "ERP"; int syncExcelVouType = -1; DataSet excelds = null;
            BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
             //20:出库单据
            if (PFunc.Sync(20, string.Empty, ErpVoucherNo, WmsVoucherType, ref ErrorMsg, syncType, syncExcelVouType, excelds)) {

                return Json(new { state = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //T_OutStock_DB db = new T_OutStock_DB();
                //db.insertLog(ErrorMsg);
                return Json(new { state = false, obj = ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }

        //关闭单据
        [HttpPost]
        public JsonResult CloseOutstock(string ID)
        {
            try
            {
                string strError = "";
                T_OutStock_Func tfunc = new T_OutStock_Func();
                if (tfunc.CloseOutStockVoucherNo(Convert.ToInt32(ID), currentUser, ref strError))
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