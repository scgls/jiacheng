using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ThoughtWorks.QRCode.Codec;

namespace Web.WMS.Report
{
    public partial class Test : System.Web.UI.Page
    {
        public static string ConvertImageToString(System.Drawing.Image imgOrderNo)
        {
            byte[] BImage = ImageToBytes(imgOrderNo, System.Drawing.Imaging.ImageFormat.Bmp);
            return Convert.ToBase64String(BImage);
        }

        public static byte[] ImageToBytes(System.Drawing.Image Image, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            if (Image == null) { return null; }
            byte[] data;
            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap Bitmap = new Bitmap(Image))
                {
                    Bitmap.Save(ms, imageFormat);
                    ms.Position = 0;
                    data = new byte[ms.Length];
                    ms.Read(data, 0, Convert.ToInt32(ms.Length));
                    ms.Flush();
                }
            }
            return data;
        }

        public static System.Drawing.Image GetQRImg(string barcode)
        {
            QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//编码方式(注意：BYTE能支持中文，ALPHA_NUMERIC扫描出来的都是数字)
            encoder.QRCodeScale = 4;//大小(值越大生成的二维码图片像素越高)
            encoder.QRCodeVersion = 0;//版本(注意：设置为0主要是防止编码的字符串太长时发生错误)
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;//错误效验、错误更正(有4个等级)
            String qrdata = barcode;
            System.Drawing.Bitmap bp = encoder.Encode(qrdata.ToString(), Encoding.Default);
            return bp;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DataTable dt = new DataTable();
                //string sql = "";
                //try
                //{
                //    if (Request["PkgCode"] != null && Request["Bnumber"] != null && Request["PrintNownumber"] != null)
                //    {
                //        string PkgCode = Request["PkgCode"].ToString();
                //        sql = "select * from tBarCode where PkgCode='" + PkgCode + "'";
                //    }
                //    else
                //    {
                //        //打印标品
                //        if (Request["vid"] != null && Request["mateno"] != null && Request["Bnumber"] != null && Request["SerialNo"] != null && Request["BatchNo"] != null && Request["ColorNo"] != null)
                //        {
                //            string vid = Request["vid"].ToString();
                //            string mateno = Request["mateno"].ToString();
                //            string SerialNo = Request["SerialNo"].ToString();
                //            string BatchNo = System.Web.HttpUtility.UrlDecode(Request["BatchNo"].ToString());
                //            string ColorNo = System.Web.HttpUtility.UrlDecode(Request["ColorNo"].ToString());

                //            sql = "select * from tBarCode where PurchaseCode='" + vid + "' and ProductCode='" + mateno + "' and MainOrder='" + SerialNo + "' and BatchNo='" + BatchNo + "' and ColorNo='" + ColorNo + "'";
                //        }
                //        else
                //        {
                //            return;
                //        }
                //    }

                //    string msg = "";
                //    Common com = new Common.Common();
                //    dt = com.GetTable(sql, out msg);
                //    if (msg != "")
                //    {
                //        return;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    return;
                //}

                //ReportViewer1.LocalReport.EnableExternalImages = true;
                //List<A> lista = new List<A>();

                //double qty = Int16.Parse(Request["PrintNownumber"].ToString());
                //int Bnumber = Int16.Parse(Request["Bnumber"].ToString());
                //double BycodeNumber = qty / Bnumber;
                //double NumberL = qty % Bnumber;

                //string outboxbarcode = "02" + "@" + dt.Rows[0]["PurchaseCode"].ToString().PadLeft(15, '0') + "@" +
                //    dt.Rows[0]["ProductCode"].ToString().PadLeft(20, '0') + "@" +
                //    dt.Rows[0]["SupplierBarcode"].ToString().PadLeft(15, '0') + "@" +
                //    dt.Rows[0]["BatchNo"].ToString().PadLeft(20, '0') + "@" +
                //    dt.Rows[0]["ColorNo"].ToString().PadLeft(20, '0');

                //if (NumberL != 0)
                //{
                //    string outboxbarcodeL = outboxbarcode + "@" + NumberL.ToString().PadLeft(4, '0');
                //    A b = new A()
                //    {
                //        aa = dt.Rows[0]["ProductCode"].ToString(),
                //        bb = dt.Rows[0]["ProductName"].ToString(),
                //        cc = dt.Rows[0]["BatchNo"].ToString(),
                //        dd = Convert.ToDateTime(dt.Rows[0]["OperTime"]).ToString("yyyy/MM/dd"),
                //        ee = NumberL.ToString(),
                //        ff = ConvertImageToString(GetQRImg(outboxbarcodeL)),
                //        gg = dt.Rows[0]["PkgCode"].ToString(),
                //        hh = (dt.Rows[0]["compontentID"].ToString() == "" ? "" : ConvertImageToString(GetQRImg("03@" + dt.Rows[0]["compontentID"].ToString()))),
                //        ii = (dt.Rows[0]["ColorNo"] == null ? "" : dt.Rows[0]["ColorNo"].ToString()),
                //        jj = (dt.Rows[0]["compontentID"].ToString() == "" ? "" : dt.Rows[0]["compontentID"].ToString())
                //    };
                //    lista.Add(b);
                //}

                //for (int i = 0; i < Math.Floor(BycodeNumber); i++)
                //{
                //    string outboxbarcodeR = outboxbarcode + "@" + Bnumber.ToString().PadLeft(4, '0');
                //    A b = new A()
                //    {
                //        aa = dt.Rows[0]["ProductCode"].ToString(),
                //        bb = dt.Rows[0]["ProductName"].ToString(),
                //        cc = dt.Rows[0]["BatchNo"].ToString(),
                //        dd = Convert.ToDateTime(dt.Rows[0]["OperTime"]).ToString("yyyy/MM/dd"),
                //        ee = Bnumber.ToString(),
                //        ff = ConvertImageToString(GetQRImg(outboxbarcodeR)),
                //        gg = dt.Rows[0]["PkgCode"].ToString(),
                //        hh = (dt.Rows[0]["compontentID"].ToString() == "" ? "" : ConvertImageToString(GetQRImg("03@" + dt.Rows[0]["compontentID"].ToString()))),
                //        ii = (dt.Rows[0]["ColorNo"] == null ? "" : dt.Rows[0]["ColorNo"].ToString()),
                //        jj = (dt.Rows[0]["compontentID"].ToString() == "" ? "" : dt.Rows[0]["compontentID"].ToString())
                //    };
                //    lista.Add(b);
                //}
                List<A> lista = new List<A>();
                A a = new A();
                a.aa = "111";
                a.bb = ConvertImageToString(GetQRImg("222"));


                ReportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource rds = new ReportDataSource("DataSet1", lista);

                ReportViewer1.LocalReport.DataSources.Add(rds);

                ReportViewer1.LocalReport.Refresh();
            }





        }

    }
}