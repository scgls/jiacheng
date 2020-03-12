using BILWeb.AdvInStock;
using BILWeb.Print;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.WMS.Common;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Print
{
    [RoleActionFilter(Message = "Print/PrintIn")]
    public class PrintInController : BaseController<T_AdvInStockDetailInfo>
    {
        public string Userno = Commom.ReadCookie("userinfo");
        private IAdvInStockDetailService advInStockDetailService;
        public PrintInController()
        {
            advInStockDetailService = (IAdvInStockDetailService)ServiceFactory.CreateObject("AdvInStock.T_AdvInStockDetail_Func");
            baseservice = advInStockDetailService;
        }

        [HttpPost]
        public JsonResult PrintList(string IDs)
        {
            if (string.IsNullOrEmpty(Userno))
            {
                return Json(new { state = false, obj = "Cookie失效，重新登陆！" }, JsonRequestBehavior.AllowGet);
            }
            string strError = string.Empty;
            List<T_AdvInStockDetailInfo> lstAdvInStockDetailInfo = new List<T_AdvInStockDetailInfo>();
            string[] strId = IDs.Split(',');
            for (int i = 0; i < strId.Length; i++)
            {
                if (!string.IsNullOrEmpty(strId[i]))
                {
                    T_AdvInStockDetailInfo model = new T_AdvInStockDetailInfo();
                    model.ID = Convert.ToInt32(strId[i]);
                    if (!advInStockDetailService.GetModelByID(ref model, ref strError))
                    {
                        return Json(new { state = false, obj = strError }, JsonRequestBehavior.AllowGet);
                    }
                    lstAdvInStockDetailInfo.Add(model);
                }
            }

            string err = "";
            Print_DB print_DB = new Print_DB();
            List<Barcode_Model> listbarcode = new List<Barcode_Model>();
            //每行打印
            if (lstAdvInStockDetailInfo != null && lstAdvInStockDetailInfo.Count != 0)
            {
                List<string> squence = GetSerialnos(lstAdvInStockDetailInfo.Count, ref err);
                int k = 0;
                for (int i = 0; i < lstAdvInStockDetailInfo.Count; i++)
                {
                    Barcode_Model model = new Barcode_Model();
                    model.CompanyCode = lstAdvInStockDetailInfo[i].CompanyCode;
                    model.MaterialNoID = lstAdvInStockDetailInfo[i].MaterialNoID;
                    model.MaterialNo = lstAdvInStockDetailInfo[i].MaterialNo;
                    model.MaterialDesc = lstAdvInStockDetailInfo[i].MaterialDesc;
                    model.BatchNo = lstAdvInStockDetailInfo[i].SupBatch;
                    model.ErpVoucherNo = lstAdvInStockDetailInfo[i].ErpVoucherNo;
                    model.EDate = Convert.ToDateTime(lstAdvInStockDetailInfo[i].EDate);
                    model.Qty = Convert.ToDecimal(lstAdvInStockDetailInfo[i].AdvQty);
                    model.StrongHoldCode = lstAdvInStockDetailInfo[i].StrongHoldCode;
                    model.SerialNo = squence[k++];
                    model.Creater = Userno;
                    model.EAN = lstAdvInStockDetailInfo[i].EAN;
                    model.ReceiveTime = Convert.ToDateTime(lstAdvInStockDetailInfo[i].CreateTime);
                    model.BarCode = "1@" + model.StrongHoldCode + "@" + model.MaterialNo + "@" + model.EAN + "@" + model.EDate.ToString("yyyy-MM-dd") + "@" + model.BatchNo + "@" + model.Qty + "@" + model.SerialNo;
                    model.RowNo = lstAdvInStockDetailInfo[i].RowNO;
                    model.RowNoDel = lstAdvInStockDetailInfo[i].RowNODel;
                    model.BarcodeType = 1;
                    model.ProductClass = lstAdvInStockDetailInfo[i].Createname;
                    model.WorkNo = lstAdvInStockDetailInfo[i].WarehouseName;
                    listbarcode.Add(model);
                }
            }


            if (print_DB.SubBarcodes(listbarcode, "sup", 1, ref err))
            {
                string serialnos = "";
                for (int i = 0; i < listbarcode.Count; i++)
                {
                    serialnos += listbarcode[i].SerialNo + ",";
                }
                return Json(new { state = true, obj = serialnos }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { state = false, obj = err }, JsonRequestBehavior.AllowGet);
            }

        }


        string sq = "";
        [HttpPost]
        public JsonResult SaveBarcode(string erpvoucherno, string materialno, string materialdesc, string ean, string batch, string edate, string num, string everynum, string receivetime, string RowNO, string RowNODel,string MaterialNoID,string StrongHoldCode,string CompanyCode,string Createname,string WarehouseName)
        {
           
            if (string.IsNullOrEmpty(Userno))
            {
                return Json(new { state = false, obj = "Cookie失效，重新登陆！" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                string err = "";
                //计算外箱数量,和尾箱数量,和尾箱里面的个数
                int outboxnum = 0;
                int inboxnum = 0;
                decimal tailnum = 0;
                GetBoxInfo(ref outboxnum, ref tailnum, ref inboxnum, num, everynum);
                if (outboxnum == 0)
                    return Json(new { state = false, obj = "打印数量为0" }, JsonRequestBehavior.AllowGet);

                Print_DB print_DB = new Print_DB();
                List<string> squence = GetSerialnos(outboxnum + inboxnum, ref err);

                //int matenoid = selectItem.MaterialNoID;
                sq = "";
                //存放打印条码内容
                List<Barcode_Model> listbarcode = new List<Barcode_Model>();
                int k = 0;
                //执行打印外箱命令
                for (int i = 0; i < outboxnum; i++)
                {
                    Barcode_Model model = new Barcode_Model();
                    model.CompanyCode = CompanyCode;
                    model.StrongHoldCode = StrongHoldCode;
                    model.MaterialNoID =Convert.ToInt32(MaterialNoID);
                    model.MaterialNo = materialno;
                    model.MaterialDesc = materialdesc;
                    model.BatchNo = batch;
                    model.ErpVoucherNo = erpvoucherno;
                    model.EDate = Convert.ToDateTime(edate);
                    model.Qty = Convert.ToDecimal(everynum);
                    model.SerialNo = squence[k++];
                    model.Creater = Userno;
                    model.EAN = ean;
                    model.ReceiveTime = string.IsNullOrEmpty(receivetime) ? DateTime.Now : Convert.ToDateTime(receivetime);
                    model.BarCode = "1@" + model.StrongHoldCode + "@" + model.MaterialNo + "@" + model.EAN + "@" + model.EDate.ToString("yyyy-MM-dd") + "@" + model.BatchNo + "@" + model.Qty + "@" + model.SerialNo;
                    model.RowNo = RowNO;
                    model.RowNoDel = RowNODel;
                    model.BarcodeType = 1;
                    model.ProductClass = Createname;
                    model.WorkNo = WarehouseName;
                    listbarcode.Add(model);
                }
                if (inboxnum == 1)
                {
                    Barcode_Model model = new Barcode_Model();
                    model.CompanyCode = CompanyCode;
                    model.StrongHoldCode = StrongHoldCode;
                    model.MaterialNoID = Convert.ToInt32(MaterialNoID);
                    model.MaterialNo = materialno;
                    model.MaterialDesc = materialdesc;
                    model.BatchNo = batch;
                    model.ErpVoucherNo = erpvoucherno;
                    model.EDate = Convert.ToDateTime(edate);
                    model.Qty = Convert.ToDecimal(tailnum);
                    model.SerialNo = squence[k++];
                    model.Creater = Userno;
                    model.EAN = ean;
                    model.ReceiveTime = string.IsNullOrEmpty(receivetime) ? DateTime.Now : Convert.ToDateTime(receivetime);
                    model.BarCode = "1@" + model.StrongHoldCode + "@" + model.MaterialNo + "@" + model.EAN + "@" + model.EDate.ToString("yyyy-MM-dd") + "@" + model.BatchNo + "@" + model.Qty + "@" + model.SerialNo;
                    model.RowNo = RowNO;
                    model.RowNoDel = RowNODel;
                    model.BarcodeType = 1;
                    model.ProductClass = Createname;
                    model.WorkNo = WarehouseName;
                    listbarcode.Add(model);
                }

                if (print_DB.SubBarcodes(listbarcode, "sup", 1, ref err))
                {
                    string serialnos = "";
                    for (int i = 0; i < listbarcode.Count; i++)
                    {
                        serialnos += listbarcode[i].SerialNo + ",";
                    }
                    return Json(new { state = true, obj = serialnos }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { state = false, obj = err }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }


        private List<string> GetSerialnos(int v, ref string err)
        {
            List<string> serialnos = new List<string>();
            for (int i = 0; i < v; i++)
            {
                var seed = Guid.NewGuid().GetHashCode();
                string code = DateTime.Now.ToString("yyMMddHHmm") + new Random(seed).Next(0, 999999).ToString().PadLeft(6, '0');
                if (serialnos.Find(t => t == code) == null)
                {
                    serialnos.Add(code);
                }
                else
                {
                    i--;
                }
            }
            return serialnos;
        }

        private void GetBoxInfo(ref int outboxnum, ref decimal tailnum, ref int inboxnum, string num, string everynum)
        {
            if (decimal.Parse(num) % decimal.Parse(everynum) == 0)
            {
                outboxnum = (int)(decimal.Parse(num) / decimal.Parse(everynum));
                tailnum = decimal.Parse(everynum);
                inboxnum = 0;
            }
            else
            {
                outboxnum = (int)(decimal.Parse(num) / decimal.Parse(everynum));
                tailnum = decimal.Parse(num) % decimal.Parse(everynum);
                inboxnum = 1;
            }

        }


      
        public JsonResult DeleteForAdv(string ID)
        {
            try
            {
                T_AdvInStockDetailInfo model = new T_AdvInStockDetailInfo{ ID = Convert.ToInt32(ID) };
                string strmsg = "";
                advInStockDetailService.GetModelByID(ref model,ref strmsg);
                T_AdvInStockDetail_DB advdb = new T_AdvInStockDetail_DB();
                if (advdb.SaveDeleteAdvDetail(model, out strmsg))
                {
                    return Json(new { state = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { state = false, obj = strmsg }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }


    }
}