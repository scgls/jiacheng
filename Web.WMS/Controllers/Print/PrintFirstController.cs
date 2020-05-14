using BILWeb.Login.User;
using BILWeb.Material;
using BILWeb.Print;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.WMS.Report.Print;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Print
{
    [RoleActionFilter(Message = "Print/PrintFirst")]
    public class PrintFirstController : Controller
    {
        protected UserInfo currentUser = Common.Commom.ReadUserInfo();
        // GET: PrintFirst
        public ActionResult Index()
        {
            List<Barcode_Model> list = TempData["list"] as List<Barcode_Model>;
            string Path = TempData["Path"] as string;
            ViewData["Data"] = list;
            ViewData["Path"] = Path;
            return View();
        }


        public ActionResult ImportExcel()
        {
            List<Barcode_Model> barcodelist = new List<Barcode_Model>();
            HttpPostedFileBase File = Request.Files["file"];
            string path = "";
            if (File.ContentLength > 0)
            {
                var Isxls = System.IO.Path.GetExtension(File.FileName).ToString().ToLower();
                if (Isxls != ".xls" && Isxls != ".xlsx")
                {
                    Content("请上传Excel文件");
                }
                var FileName = File.FileName;//获取文件夹名称
                path = Server.MapPath("~/Upload/" +DateTime.Now.ToString("yyyyMMddHHmmss")) + FileName;
                File.SaveAs(path);//将文件保存到服务器
                DataTable dt = new DataTable();
                if (Isxls == ".xls")
                {
                    dt = ImportExcelFile(path);
                }
                if (Isxls == ".xlsx")
                {
                    dt = ImportExcelFilexlsx(path);
                }
                barcodelist = Print_DB.ConvertToModel<Barcode_Model>(dt);
            }
            TempData["list"] = barcodelist;
            TempData["Path"] = path;
            return RedirectToAction("Index", "PrintFirst");
        }

        public JsonResult Print(string IDs, string Path)
        {
            try
            {
                List<Barcode_Model> barcodelist = new List<Barcode_Model>();
                var Isxls = System.IO.Path.GetExtension(Path).ToString().ToLower();
                DataTable dt = new DataTable();
                if (Isxls == ".xls")
                {
                    dt = ImportExcelFile(Path);
                }
                if (Isxls == ".xlsx")
                {
                    dt = ImportExcelFilexlsx(Path);
                }
                barcodelist = Print_DB.ConvertToModel<Barcode_Model>(dt);
                List<Barcode_Model> barcodelistnew = new List<Barcode_Model>();
                List<Barcode_Model> barcodelistnewsub = new List<Barcode_Model>();
                List<Barcode_Model> barcodelistnewAll = new List<Barcode_Model>();
                string[] ids = IDs.Split(',');
                for (int i = 0; i < ids.Length; i++)
                {
                    if (ids[i] != "") {
                        List<Barcode_Model> barcodes = new List<Barcode_Model>();
                        barcodes.AddRange(barcodelist.Where(P => P.RowNo == ids[i]));
                        //单张条码
                        //Barcode_Model model = (Barcode_Model)DeepCopy(barcodes[0]);
                        //barcodelistnew.Add(model);
                        //多张条码
                        if (barcodes.Count == 1 && barcodes[0].BoxCount >= 1)
                        {
                            for (int j = 0; j < barcodes[0].BoxCount; j++)
                            {
                                Barcode_Model model = (Barcode_Model)DeepCopy(barcodes[0]);
                                barcodelistnew.Add(model);
                            }
                        }

                    }
                }

                if (barcodelistnew != null && barcodelistnew.Count > 0)
                {
                    string strMsg = "";
                    PrintInController printIn = new PrintInController();
                    List<string> squence = printIn.GetSerialnos(barcodelistnew.Count,"外",ref strMsg);
                    int k = 0;
                    DateTime time = DateTime.Now;
                    for (int i = 0; i < barcodelistnew.Count; i++)
                    {
                        barcodelistnew[i].CompanyCode = "SHJC";
                        barcodelistnew[i].BarcodeType = 1;
                        barcodelistnew[i].SerialNo = squence[k++];
                        barcodelistnew[i].Creater = currentUser.UserNo;
                        barcodelistnew[i].ReceiveTime = time;
                        barcodelistnew[i].BarCode = "1@" + barcodelistnew[i].StrongHoldCode + "@" + barcodelistnew[i].MaterialNo + "@" + barcodelistnew[i].BatchNo + "@" + barcodelistnew[i].Qty + "@" + barcodelistnew[i].SerialNo;

                        //查物料
                        T_Material_Func funM = new T_Material_Func();
                        string strErrMsg = "";
                        List<T_MaterialInfo> modelList = funM.GetMaterialModelBySql(barcodelistnew[i].MaterialNo, ref strErrMsg);
                        if (modelList == null || modelList.Count == 0)
                        {
                            //失败
                            return Json(new { state = false, obj = "没有该物料号" + barcodelistnew[i].MaterialNo }, JsonRequestBehavior.AllowGet);
                        }
                        if (modelList[0].sku == "是")
                        {
                            for (int kk = 0; kk < barcodelistnew[i].Qty; kk++)
                            {
                                Barcode_Model modelsub = (Barcode_Model)DeepCopy(barcodelistnew[i]);
                                modelsub.fserialno = barcodelistnew[i].SerialNo;
                                barcodelistnewsub.Add(modelsub);
                            }

                        }


                    }

                    //本体打印
                    DateTime timesub = DateTime.Now.AddSeconds(5);
                    if (barcodelistnewsub != null && barcodelistnewsub.Count > 0)
                    {
                        List<string> squencesub = printIn.GetSerialnos(barcodelistnewsub.Count, "内", ref strMsg);
                        int ksub = 0;
                       
                        for (int isub = 0; isub < barcodelistnewsub.Count; isub++)
                        {
                            barcodelistnewsub[isub].CompanyCode = "SHJC";
                            barcodelistnewsub[isub].BarcodeType = 2;
                            barcodelistnewsub[isub].SerialNo = squencesub[ksub++];
                            barcodelistnewsub[isub].Creater = currentUser.UserNo;
                            barcodelistnewsub[isub].ReceiveTime = timesub;
                            barcodelistnewsub[isub].Qty = 1;

                            barcodelistnewsub[isub].BarCode = "2@" + barcodelistnewsub[isub].StrongHoldCode + "@" + barcodelistnewsub[isub].MaterialNo + "@" + barcodelistnewsub[isub].BatchNo + "@1@" + barcodelistnewsub[isub].SerialNo;

                        }
                    }
                    barcodelistnewAll.AddRange(barcodelistnew);
                    barcodelistnewAll.AddRange(barcodelistnewsub);
                    Print_DB func = new Print_DB();
                    if (func.SubBarcodesNoPrint(barcodelistnewAll, ref strMsg))
                    {
                        if (barcodelistnewsub != null && barcodelistnewsub.Count > 0)
                        {
                            return Json(new { state = true, obj = time.ToString("yyyy/MM/dd HH:mm:ss"), obj1 = timesub.ToString("yyyy/MM/dd HH:mm:ss") }, JsonRequestBehavior.AllowGet);
                        }
                        else {
                            return Json(new { state = true, obj = time.ToString("yyyy/MM/dd HH:mm:ss"), obj1 = "" }, JsonRequestBehavior.AllowGet);
                        }
                            
                    }
                    else
                    {
                        //失败
                        return Json(new { state = false, obj = strMsg }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //失败
                    return Json(new { state = false, obj = "数据为空" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
            

        }

        public object DeepCopy(object _object)
        {
            Type T = _object.GetType();
            object o = Activator.CreateInstance(T);
            PropertyInfo[] PI = T.GetProperties();
            for (int i = 0; i < PI.Length; i++)
            {
                PropertyInfo P = PI[i];
                P.SetValue(o, P.GetValue(_object));
            }
            return o;
        }
    

    public static DataTable ImportExcelFile(string filePath)

        {

            HSSFWorkbook hssfworkbook;
            #region//初始化信息

            try

            {

                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))

                {

                    hssfworkbook = new HSSFWorkbook(file);

                }

            }

            catch (Exception e)

            {

                throw e;

            }

            #endregion
            NPOI.SS.UserModel.ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            DataTable dt = new DataTable();
            for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
            {
                dt.Columns.Add(sheet.GetRow(0).GetCell(j).ToString());
            }
            //sheet.cu
            while (rows.MoveNext())
            {
                HSSFRow row = (HSSFRow)rows.Current;
                if (row.RowNum != 0)
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        NPOI.SS.UserModel.ICell cell = row.GetCell(i);
                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            dr[i] = cell.ToString();
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public static DataTable ImportExcelFilexlsx(string filePath)
        {
            XSSFWorkbook hssfworkbook;
            #region//初始化信息

            try

            {

                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))

                {

                    hssfworkbook = new XSSFWorkbook(file);

                }

            }

            catch (Exception e)

            {

                throw e;

            }

            #endregion
            NPOI.SS.UserModel.ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            DataTable dt = new DataTable();
            for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
            {
                dt.Columns.Add(sheet.GetRow(0).GetCell(j).ToString());
            }
            //sheet.cu
            while (rows.MoveNext())
            {
                XSSFRow row = (XSSFRow)rows.Current;
                if (row.RowNum != 0)
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        NPOI.SS.UserModel.ICell cell = row.GetCell(i);
                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            dr[i] = cell.ToString();
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public static IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<DataRow> rows = new List<DataRow>();
            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }
            return ConvertTo<T>(rows);
        }

        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        prop.SetValue(obj, value, null);
                    }
                    catch
                    {  //You can log something here     
                       //throw;    
                    }
                }
            }

            return obj;
        }




        //string sq = "";
        //[HttpPost]
        //public void SaveBarcode(string erpvoucherno, string materialno, string materialdesc, string ean, string batch, string edate, string num, string everynum, string receivetime, string RowNO, string RowNODel)
        //{
        //    Print_DB func = new Print_DB();
        //    func.FirstPrint()


        //    try
        //    {
        //        string err = "";
        //        //计算外箱数量,和尾箱数量,和尾箱里面的个数
        //        int outboxnum = 0;
        //        int inboxnum = 0;
        //        decimal tailnum = 0;
        //        GetBoxInfo(ref outboxnum, ref tailnum, ref inboxnum, num, everynum);
        //        if (outboxnum == 0)
        //            return Json(new { state = false, obj = "打印数量为0" }, JsonRequestBehavior.AllowGet);

        //        Print_DB print_DB = new Print_DB();
        //        List<string> squence = GetSerialnos(outboxnum + inboxnum, ref err);

        //        //int matenoid = selectItem.MaterialNoID;
        //        sq = "";
        //        //存放打印条码内容
        //        List<Barcode_Model> listbarcode = new List<Barcode_Model>();
        //        int k = 0;
        //        //执行打印外箱命令
        //        for (int i = 0; i < outboxnum; i++)
        //        {
        //            Barcode_Model model = new Barcode_Model();
        //            model.MaterialNo = materialno;
        //            model.MaterialDesc = materialdesc;
        //            model.BatchNo = batch;
        //            model.ErpVoucherNo = erpvoucherno;
        //            model.EDate = Convert.ToDateTime(edate);
        //            model.Qty = Convert.ToDecimal(everynum);
        //            model.SerialNo = squence[k++];
        //            model.Creater = Common.Commom.currentUser.UserNo;
        //            model.EAN = ean;
        //            model.ReceiveTime = string.IsNullOrEmpty(receivetime) ? DateTime.Now : Convert.ToDateTime(receivetime);
        //            model.BarCode = "1@" + model.StrongHoldCode + "@" + model.MaterialNo + "@" + model.EAN + "@" + model.EDate.ToString("yyyy-MM-dd") + "@" + model.BatchNo + "@" + model.Qty + "@" + model.SerialNo;
        //            model.RowNo = RowNO;
        //            model.RowNoDel = RowNODel;
        //            listbarcode.Add(model);
        //        }
        //        if (inboxnum == 1)
        //        {
        //            Barcode_Model model = new Barcode_Model();
        //            model.MaterialNo = materialno;
        //            model.MaterialDesc = materialdesc;
        //            model.BatchNo = batch;
        //            model.ErpVoucherNo = erpvoucherno;
        //            model.EDate = Convert.ToDateTime(edate);
        //            model.Qty = Convert.ToDecimal(tailnum);
        //            model.SerialNo = squence[k++];
        //            model.Creater = Common.Commom.currentUser.UserNo;
        //            model.EAN = ean;
        //            model.ReceiveTime = string.IsNullOrEmpty(receivetime) ? DateTime.Now : Convert.ToDateTime(receivetime);
        //            model.BarCode = "1@" + model.StrongHoldCode + "@" + model.MaterialNo + "@" + model.EAN + "@" + model.EDate.ToString("yyyy-MM-dd") + "@" + model.BatchNo + "@" + model.Qty + "@" + model.SerialNo;
        //            model.RowNo = RowNO;
        //            model.RowNoDel = RowNODel;
        //            listbarcode.Add(model);
        //        }

        //        if (print_DB.SubBarcodes(listbarcode, "sup", 1, ref err))
        //        {
        //            string serialnos = "";
        //            for (int i = 0; i < listbarcode.Count; i++)
        //            {
        //                serialnos += listbarcode[i].SerialNo + ",";
        //            }
        //            return Json(new { state = true, obj = serialnos }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(new { state = false, obj = err }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        private List<string> GetSerialnos(int v, ref string err)
        {
            List<string> serialnos = new List<string>();
            for (int i = 0; i < v; i++)
            {
                var seed = Guid.NewGuid().GetHashCode();
                //string code = DateTime.Now.ToString("yyMMddHHmm") + new Random(seed).Next(0, 999999).ToString().PadLeft(6, '0');
                string code = DateTime.Now.ToString("yyMMdd") +"77"+ new Random(seed).Next(0, 999999).ToString().PadLeft(6, '0');//奥碧虹
                if (serialnos.Find(t=>t==code)==null)
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





    }
}