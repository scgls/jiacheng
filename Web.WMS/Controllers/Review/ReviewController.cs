using BILBasic.User;
using BILWeb.LandMark;
using BILWeb.Login.User;
using BILWeb.OutStock;
using BILWeb.Pallet;
using BILWeb.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Review
{
    [RoleActionFilter(Message = "Print/Review")]
    public class ReviewController : Controller
    {
        UserInfo currentUser = Common.Commom.ReadUserInfo();
        // GET: Review
        public ActionResult Review(string strCarNo)
        {
            T_OutStockInfo t_OutStockInfo = new T_OutStockInfo();
            T_OutStock_Func func = new T_OutStock_Func();
            string strError = "";
            func.GetModelListByCar(strCarNo, ref t_OutStockInfo, ref strError);
            ViewData["t_OutStockInfo"] = t_OutStockInfo;
            ViewData["strError"] = strError;
            return View();
        }

        //扫描序列号和EAN
        public JsonResult getbarcode(T_StockInfo model)
        {
            if (currentUser == null)
            {
                return Json(new { state = false, obj = "Cookie失效，重新登陆！" }, JsonRequestBehavior.AllowGet);
            }
            model.ScanType = 3;
            List<T_StockInfo> modelList = new List<T_StockInfo>();
            List<T_OutStockDetailInfo> outStockDetailList = new List<T_OutStockDetailInfo>();
            string strError = "";
            T_OutStockDetail_Func fun = new T_OutStockDetail_Func();
            int ID = 0;
            bool isSuccess = fun.GetReviewStockModel(currentUser, model, ref modelList, ref outStockDetailList, ref ID, ref strError);
            if (isSuccess && model.Barcode.Contains("@") && outStockDetailList.Count > 0)
            {
                for (int i = 0; i < outStockDetailList.Count; i++)
                {
                    if (outStockDetailList[i].ID == ID)
                    {
                        outStockDetailList[i].isLight = true;
                    }
                    else
                    {
                        outStockDetailList[i].isLight = false;
                    }
                }
                return Json(new { state = isSuccess, obj = outStockDetailList, type = "1" }, JsonRequestBehavior.AllowGet);
            }
            if (isSuccess && !model.Barcode.Contains("@") && modelList.Count > 0)
            {
                return Json(new { state = isSuccess, obj = modelList, type = "2" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { state = isSuccess, obj = strError }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult scanEAN(T_StockInfo model)
        {
            if (currentUser == null)
            {
                return Json(new { state = false, obj = "Cookie失效，重新登陆！" }, JsonRequestBehavior.AllowGet);
            }
            if (model.Barcode.Contains("@"))
            {
                return Json(new { state = false, obj = "扫描物料标签不能修改数量！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                model.ScanType = 3;
            }
            List<T_OutStockDetailInfo> outStockDetailList = new List<T_OutStockDetailInfo>();
            string strError = "";
            T_OutStockDetail_Func fun = new T_OutStockDetail_Func();

            //重新构造库存类
            T_StockInfo modelnew = new T_StockInfo();
            modelnew.ErpVoucherNo = model.ErpVoucherNo;
            modelnew.Barcode = model.Barcode;
            List<T_StockInfo> modelList = new List<T_StockInfo>();
            int ID = 0;
            bool isSuccessF = fun.GetReviewStockModel(currentUser, model, ref modelList, ref outStockDetailList, ref ID, ref strError);
            if (isSuccessF && modelList != null && modelList.Count > 0)
            {
                //第二次进来带了materialnoid
                if (model.MaterialNoID != 0)
                {
                    foreach (T_StockInfo item in modelList)
                    {
                        if (item.MaterialNoID == model.MaterialNoID)
                        {
                            modelnew = item;
                            break;
                        }
                    }
                    if (modelnew.MaterialNoID == 0)
                    {
                        return Json(new { state = false, obj = "没有找到对应的条码！" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    modelnew = modelList[0];
                }
            }
            else
            {
                return Json(new { state = isSuccessF, obj = strError }, JsonRequestBehavior.AllowGet);
            }
            if (model.ScanQty > modelnew.Qty)
            {
                return Json(new { state = false, obj = "提交数量不能超过库存数量！" }, JsonRequestBehavior.AllowGet);
            }
            modelnew.Barcode = model.Barcode;
            modelnew.ErpVoucherNo = model.ErpVoucherNo;
            modelnew.ScanQty = model.ScanQty;
            modelnew.ScanQty = model.ScanQty;
            bool isSuccess = fun.SaveT_OutStockReviewDetailENA(currentUser, modelnew, ref outStockDetailList, ref ID, ref strError);
            if (isSuccess)
            {
                for (int i = 0; i < outStockDetailList.Count; i++)
                {
                    if (outStockDetailList[i].ID == ID)
                    {
                        outStockDetailList[i].isLight = true;
                    }
                    else
                    {
                        outStockDetailList[i].isLight = false;
                    }
                }
                return Json(new { state = isSuccess, obj = outStockDetailList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { state = isSuccess, obj = strError }, JsonRequestBehavior.AllowGet);
            }
        }



        //public JsonResult printBox(String ErpVoucherno)
        //{
        //    T_OutStockDetail_Func func = new T_OutStockDetail_Func();
        //    string strError = "";
        //    List<T_PalletDetailInfo> palletModelList = new List<T_PalletDetailInfo>();
        //    bool isSuccess = func.CreatePalletByTaskTrans(ErpVoucherno, currentUser, ref palletModelList, ref strError);
        //    return Json(new { state = isSuccess, obj = strError }, JsonRequestBehavior.AllowGet);
        //}


        public JsonResult CheckPrint(String ErpVoucherno)
        {
            T_OutStock_Func func = new T_OutStock_Func();
            T_OutStockInfo model = new T_OutStockInfo();
            string strError = "";
            bool isSuccess = func.GetOutStockDetailForPrint(ErpVoucherno, ref model, ref strError);
            if (isSuccess)
            {
                string del = "";
                var FYlist = from cust in model.lstDetail
                             where cust.StrongHoldCode == "FY2"
                             select cust;
                var HMlist = from cust in model.lstDetail
                             where cust.StrongHoldCode == "HM1"
                             select cust;
                if (FYlist.Count() == 0)
                {
                    del += "F";
                }
                if (HMlist.Count() == 0)
                {
                    del += "H";
                }
                return Json(new { state = isSuccess, obj = strError, delobj = del }, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(new { state = isSuccess, obj = strError}, JsonRequestBehavior.AllowGet);
            }
            
            

        }

        [HttpPost]
        public JsonResult reviewpost(String ErpVoucherno)
        {
            if (currentUser == null)
            {
                return Json(new { state = false, obj = "Cookie失效，重新登陆！" }, JsonRequestBehavior.AllowGet);
            }
            string strError = "";
            T_OutStockDetail_Func func = new T_OutStockDetail_Func();

            //获取cookie
            string guid = "";
            HttpCookie cookie = Request.Cookies[ErpVoucherno];
            if (cookie == null)
            {
                guid = Guid.NewGuid().ToString();
                HttpCookie hc = new HttpCookie(ErpVoucherno);
                hc["guid"] = guid;
                Response.Cookies.Add(hc);//保存到客户端
            }
            else
            {
                guid = cookie["guid"].ToString();
            }
            bool isSuccess = func.PostT_OutStockReviewDetail(currentUser, ErpVoucherno.Trim(), guid, ref strError);
            //if (isSuccess)
            //{
            //    HttpCookie hc = new HttpCookie(ErpVoucherno);
            //    hc.Expires = DateTime.Now.AddDays(-1);
            //    Response.Cookies.Add(hc);
            //    //复核完成释放地标
            //    T_LandMarkWithTask_DB db = new T_LandMarkWithTask_DB();
            //    string strErrordb = "";
            //    db.DelTaskwithandmark(ErpVoucherno.Trim(),ref strErrordb);
            //    strError += strErrordb;
            //}
            return Json(new { state = isSuccess, obj = strError }, JsonRequestBehavior.AllowGet);
        }

        // 打印物流标签生成托盘数据
        [HttpPost]
        public JsonResult CreatePalletByEmsLabel(String ErpVoucherno, int PrintQty)
        {

            if (currentUser == null)
            {
                return Json(new { state = false, obj = "Cookie失效，重新登陆！" }, JsonRequestBehavior.AllowGet);
            }
            string PalletNo = "";
            string strError = "";
            T_OutStockDetail_Func func = new T_OutStockDetail_Func();
            bool isSuccess = func.CreatePalletByEmsLabel(ErpVoucherno, PrintQty, ref PalletNo, currentUser, ref strError);
            if (isSuccess)
            {
                return Json(new { state = isSuccess, obj = PalletNo }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { state = isSuccess, obj = strError }, JsonRequestBehavior.AllowGet);
            }

        }

        //// 快递单子
        //[HttpPost]
        //public JsonResult CreateOutStockByEmsLabel(String ErpVoucherno, decimal Weight)
        //{

        //    if (currentUser == null)
        //    {
        //        return Json(new { state = false, obj = "Cookie失效，重新登陆！" }, JsonRequestBehavior.AllowGet);
        //    }
        //    T_OutStockDetail_Func func = new T_OutStockDetail_Func();
        //    string GUID = Guid.NewGuid().ToString();
        //    T_OutStockInfo outStockInfo = new T_OutStockInfo();
        //    string strError = "";
        //    bool isSuccess = func.CreateOutStockByEmsLabel(GUID, ErpVoucherno,  1,  Weight, currentUser, ref outStockInfo, ref strError);
        //    if (isSuccess)
        //    {
        //        return Json(new { state = isSuccess, obj = "ok" }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new { state = isSuccess, obj = strError }, JsonRequestBehavior.AllowGet);
        //    }

        //}


    }
}
