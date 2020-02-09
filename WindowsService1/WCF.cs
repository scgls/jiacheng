using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace WMSDocumentSynchronizationService
{
    /// <summary>
    /// 重定向WCF地址
    /// </summary>
    class WCF
    {
        

        public static WindowsService1.ServiceReference1.ServiceClient GetWCF()
        {
            WindowsService1.ServiceReference1.ServiceClient service = null;
            try
            {
                service = new WindowsService1.ServiceReference1.ServiceClient();
                service.Endpoint.Address = new System.ServiceModel.EndpointAddress(System.Configuration.ConfigurationManager.AppSettings["WCFIP"]);
             
            }
            catch(Exception ex) { string e = ex.Message; }
            return service;
        }
    }
}
