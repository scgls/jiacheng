using BILWeb.OutBarCode;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.WMS.Report.Print
{
    public partial class PrintIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.LocalReport.EnableExternalImages = true;
                List<PrintModel> lista = new List<PrintModel>();
                if (Request["serialnos"] != null)
                {
                    string serialnos = Request["serialnos"].ToString();
                    string[] serialno = serialnos.Split(',');
                    for (int i = 0; i < serialno.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(serialno[i]))
                        {
                            T_OutBarCode_Func t_OutBarCode_Func = new T_OutBarCode_Func();
                            string strMsg = "";
                            T_OutBarCodeInfo t_OutBarCodeInfo = new T_OutBarCodeInfo();
                            t_OutBarCode_Func.GetOutBarCodeInfoBySerialNo(serialno[i], ref t_OutBarCodeInfo, ref strMsg);
                            lista.Add(new PrintModel
                            {
                                DataColumn1 = t_OutBarCodeInfo.MaterialNo,
                                DataColumn2 = t_OutBarCodeInfo.MaterialDesc,
                                DataColumn3 = t_OutBarCodeInfo.EAN,
                                DataColumn4 = t_OutBarCodeInfo.ErpVoucherNo,
                                DataColumn5 = t_OutBarCodeInfo.EDate.ToString(),
                                DataColumn6 = t_OutBarCodeInfo.BatchNo,
                                DataColumn7 = t_OutBarCodeInfo.Qty.ToString(),
                                DataColumn8 = t_OutBarCodeInfo.receivetime.ToString(),
                                DataColumn9 = Common.PrintRdlc.ConvertImageToString(Common.PrintRdlc.GetQRImg(t_OutBarCodeInfo.BarCode)),
                            });
                        }

                    }

                }

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource rds = new ReportDataSource("PrintInDs", lista);
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.LocalReport.Refresh();
            }

        }
    }
}