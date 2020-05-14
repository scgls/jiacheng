using BILWeb.AdvInStock;
using BILWeb.Login.User;
using BILWeb.Material;
using BILWeb.OutBarCode;
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
        UserInfo currentUser = Common.Commom.ReadUserInfo();
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
                List<string> squence = GetSerialnos(lstAdvInStockDetailInfo.Count, "外", ref err);
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
        public JsonResult SaveBarcode(string erpvoucherno, string materialno, string materialdesc, string ean, string batch, string edate, string num, string everynum, string receivetime, string RowNO, string RowNODel, string MaterialNoID, string StrongHoldCode, string CompanyCode, string Createname, string WarehouseName, string TracNo, string ProjectNo,string flag="")//flag=1 是预留释放打印
        {
            //查物料
            T_Material_Func funM = new T_Material_Func();
            string strErrMsg = "";
            List<T_MaterialInfo> modelList = funM.GetMaterialModelBySql(materialno, ref strErrMsg);
            if (modelList == null || modelList.Count == 0)
            {
                //失败
                return Json(new { state = false, obj = "没有该物料号" + materialno }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(Userno))
            {
                return Json(new { state = false, obj = "Cookie失效，重新登陆！" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                DateTime time1 = DateTime.Now;
                DateTime time2 = DateTime.Now.AddSeconds(1);
                string err = "";
                //计算外箱数量,和尾箱数量,和尾箱里面的个数
                int outboxnum = 0;
                int inboxnum = 0;
                decimal tailnum = 0;
                GetBoxInfo(ref outboxnum, ref tailnum, ref inboxnum, num, everynum);
                if (outboxnum == 0)
                    return Json(new { state = false, obj = "打印数量为0" }, JsonRequestBehavior.AllowGet);

                Print_DB print_DB = new Print_DB();
                List<string> squence = GetSerialnos(outboxnum + inboxnum, "外", ref err);//外箱码序列号
                List<string> squenceforin = GetSerialnos(Int16.Parse(num), "内", ref err);//本体序列号

                //int matenoid = selectItem.MaterialNoID;
                sq = "";
                //存放打印条码内容
                List<Barcode_Model> listbarcode = new List<Barcode_Model>();
                int k = 0;
                int kIn = 0;
                //执行打印外箱命令
                for (int i = 0; i < outboxnum; i++)
                {
                    Barcode_Model model = new Barcode_Model();
                    model.CompanyCode = CompanyCode;
                    model.StrongHoldCode = StrongHoldCode;
                    model.MaterialNoID = Convert.ToInt32(MaterialNoID);
                    model.MaterialNo = materialno;
                    model.MaterialDesc = materialdesc;
                    model.BatchNo = DateTime.Now.ToString("yyyyMMdd");
                    model.ErpVoucherNo = erpvoucherno;
                    //model.EDate = Convert.ToDateTime(edate);
                    model.Qty = Convert.ToDecimal(everynum);
                    model.SerialNo = squence[k++];
                    model.Creater = Userno;
                    //model.EAN = ean;
                    //model.ReceiveTime = string.IsNullOrEmpty(receivetime) ? DateTime.Now : Convert.ToDateTime(receivetime);
                    model.ReceiveTime = time1;
                    model.BarCode = "1@" + model.StrongHoldCode + "@" + model.MaterialNo + "@" + model.BatchNo + "@" + model.Qty + "@" + model.SerialNo;
                    model.RowNo = RowNO;
                    model.RowNoDel = RowNODel;
                    model.BarcodeType = 1;
                    model.ProductClass = Createname;
                    model.WorkNo = WarehouseName;
                    model.TracNo = TracNo;
                    model.ProjectNo = ProjectNo;
                    model.originalCode = flag;
                    listbarcode.Add(model);
                    if (modelList[0].sku == "是")
                    {
                        //本体打印
                        for (int ii = 0; ii < Convert.ToDecimal(everynum); ii++)
                        {
                            Barcode_Model modelIn1 = new Barcode_Model();
                            modelIn1.CompanyCode = CompanyCode;
                            modelIn1.StrongHoldCode = StrongHoldCode;
                            modelIn1.MaterialNoID = Convert.ToInt32(MaterialNoID);
                            modelIn1.MaterialNo = materialno;
                            modelIn1.MaterialDesc = materialdesc;
                            modelIn1.BatchNo = DateTime.Now.ToString("yyyyMMdd");
                            modelIn1.ErpVoucherNo = erpvoucherno;
                            modelIn1.Qty = 1;
                            modelIn1.SerialNo = squenceforin[kIn++];
                            modelIn1.Creater = Userno;
                            modelIn1.ReceiveTime = time2;
                            //modelIn1.ReceiveTime = string.IsNullOrEmpty(receivetime) ? DateTime.Now : Convert.ToDateTime(receivetime);
                            modelIn1.BarCode = "2@" + modelIn1.StrongHoldCode + "@" + modelIn1.MaterialNo + "@" + modelIn1.BatchNo + "@" + modelIn1.Qty + "@" + modelIn1.SerialNo;
                            modelIn1.RowNo = RowNO;
                            modelIn1.RowNoDel = RowNODel;
                            modelIn1.BarcodeType = 2;
                            modelIn1.ProductClass = Createname;
                            modelIn1.WorkNo = WarehouseName;
                            modelIn1.TracNo = TracNo;
                            modelIn1.ProjectNo = ProjectNo;
                            modelIn1.fserialno = model.SerialNo;
                            modelIn1.originalCode = flag;
                            listbarcode.Add(modelIn1);
                        }
                    }
                }
                if (inboxnum == 1)
                {
                    Barcode_Model model = new Barcode_Model();
                    model.CompanyCode = CompanyCode;
                    model.StrongHoldCode = StrongHoldCode;
                    model.MaterialNoID = Convert.ToInt32(MaterialNoID);
                    model.MaterialNo = materialno;
                    model.MaterialDesc = materialdesc;
                    model.BatchNo = DateTime.Now.ToString("yyyyMMdd");
                    model.ErpVoucherNo = erpvoucherno;
                    //model.EDate = Convert.ToDateTime(edate);
                    model.Qty = Convert.ToDecimal(tailnum);
                    model.SerialNo = squence[k++];
                    model.Creater = Userno;
                    //model.EAN = ean;
                    //model.ReceiveTime = string.IsNullOrEmpty(receivetime) ? DateTime.Now : Convert.ToDateTime(receivetime);
                    model.ReceiveTime = time1;
                    model.BarCode = "1@" + model.StrongHoldCode + "@" + model.MaterialNo + "@" + model.BatchNo + "@" + model.Qty + "@" + model.SerialNo;
                    model.RowNo = RowNO;
                    model.RowNoDel = RowNODel;
                    model.BarcodeType = 1;
                    model.ProductClass = Createname;
                    model.WorkNo = WarehouseName;
                    model.TracNo = TracNo;
                    model.ProjectNo = ProjectNo;
                    model.originalCode = flag;
                    listbarcode.Add(model);
                    if (modelList[0].sku == "是")
                    {
                        //本体打印
                        for (int ii = 0; ii < Convert.ToDecimal(tailnum); ii++)
                        {
                            Barcode_Model modelIn2 = new Barcode_Model();
                            modelIn2.CompanyCode = CompanyCode;
                            modelIn2.StrongHoldCode = StrongHoldCode;
                            modelIn2.MaterialNoID = Convert.ToInt32(MaterialNoID);
                            modelIn2.MaterialNo = materialno;
                            modelIn2.MaterialDesc = materialdesc;
                            modelIn2.BatchNo = DateTime.Now.ToString("yyyyMMdd");
                            modelIn2.ErpVoucherNo = erpvoucherno;
                            modelIn2.Qty = 1;
                            modelIn2.SerialNo = squenceforin[kIn++];
                            modelIn2.Creater = Userno;
                            modelIn2.ReceiveTime = time2;
                            //modelIn2.ReceiveTime = string.IsNullOrEmpty(receivetime) ? DateTime.Now : Convert.ToDateTime(receivetime);
                            modelIn2.BarCode = "2@" + modelIn2.StrongHoldCode + "@" + modelIn2.MaterialNo + "@" + modelIn2.BatchNo + "@" + modelIn2.Qty + "@" + modelIn2.SerialNo;
                            modelIn2.RowNo = RowNO;
                            modelIn2.RowNoDel = RowNODel;
                            modelIn2.BarcodeType = 2;
                            modelIn2.ProductClass = Createname;
                            modelIn2.WorkNo = WarehouseName;
                            modelIn2.TracNo = TracNo;
                            modelIn2.ProjectNo = ProjectNo;
                            modelIn2.fserialno = model.SerialNo;
                            modelIn2.originalCode = flag;
                            listbarcode.Add(modelIn2);
                        }
                    }
                }
                if (print_DB.SubBarcodes(listbarcode, "sup", 1, ref err))
                {
                    string serialnosB = "";
                    string serialnosS = "";
                    for (int i = 0; i < listbarcode.Count; i++)
                    {
                        if (listbarcode[i].BarcodeType == 1)
                        {
                            serialnosB += listbarcode[i].SerialNo + ",";
                        }
                        else
                        {
                            serialnosS += listbarcode[i].SerialNo + ",";
                        }

                    }
                    if (serialnosS == "")
                    {
                        return Json(new { state = true, obj = time1.ToString("yyyy/MM/dd HH:mm:ss") }, JsonRequestBehavior.AllowGet);
                    }
                    else {
                        return Json(new { state = true, obj = time1.ToString("yyyy/MM/dd HH:mm:ss"), objS = time2.ToString("yyyy/MM/dd HH:mm:ss") }, JsonRequestBehavior.AllowGet);
                    }
                    
                    //return Json(new { state = true, obj = serialnosB, objS = serialnosS }, JsonRequestBehavior.AllowGet);
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


        public List<string> GetSerialnos(int v, string flag, ref string err)
        {
            List<string> serialnos = new List<string>();
            for (int i = 0; i < v; i++)
            {
                var seed = Guid.NewGuid().GetHashCode();
                string code = DateTime.Now.ToString("yyMMddHHmmss") + new Random(seed).Next(0, 99999999).ToString().PadLeft(8, '0') + (flag == "外" ? "01" : "02");
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
                T_AdvInStockDetailInfo model = new T_AdvInStockDetailInfo { ID = Convert.ToInt32(ID) };
                string strmsg = "";
                advInStockDetailService.GetModelByID(ref model, ref strmsg);
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