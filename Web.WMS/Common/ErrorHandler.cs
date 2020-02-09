using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace WMS.Web.Common
{
    /// <summary>
    /// 错误处理日志
    /// </summary>
    public class ErrorHandler
    {
        /// <summary>
        /// 将错误写入日志
        /// </summary>
        /// <param name="e"></param>
        public static void WriteError(Exception e)
        {
            string filepath = "/Log/" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            string serverpath = System.Web.HttpContext.Current.Server.MapPath(filepath);
            if (!File.Exists(serverpath))
            {
                File.Create(serverpath).Close();
            }
            using (StreamWriter sw =File.AppendText(serverpath))
            {
                sw.WriteLine(" Log Entry : ");
                sw.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                string err = "Error in: " + System.Web.HttpContext.Current.Request.Url.ToString() +
                             ". Error Message:" + e.Message + ",详细错误信息：";
                if (e.InnerException != null)
                {
                    if (e.InnerException.InnerException != null)
                    {
                        err += e.InnerException.InnerException.Message;
                    }
                    else
                    {
                        err += e.InnerException.Message;
                    }
                }
                sw.WriteLine(err);
                sw.WriteLine("__________________________");
                sw.Flush();
                sw.Close();
            }
        }
    }
}
