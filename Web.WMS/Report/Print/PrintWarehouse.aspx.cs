using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.WMS.Report.Print
{
    public partial class PrintWarehouse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.LocalReport.EnableExternalImages = true;
                List<PrintModel> lista = new List<PrintModel>();
                if (Request["area"] != null)
                {
                    string areas = Request["area"].ToString();
                    string[] Area = areas.Split(',') ;
                    for (int i = 0; i < Area.Length; i++)
                    {
                        lista.Add(new PrintModel
                        {
                            DataColumn1 = Area[i],
                            DataColumn2 = Common.PrintRdlc.ConvertImageToString(Common.PrintRdlc.GetQRImg(Area[i]))
                        });
                    }

                }

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource rds = new ReportDataSource("PrintWarehouseDs", lista);
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.LocalReport.Refresh();
            }

        }
    }
}