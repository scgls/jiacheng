using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SCCGAndroidService
{
    /// <summary>
    /// UpLoad 的摘要说明
    /// </summary>
    public class UpLoad : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
           try
            {
                byte[] byts = new byte[HttpContext.Current.Request.InputStream.Length];
                HttpContext.Current.Request.InputStream.Read(byts, 0, byts.Length);
                string req = System.Text.Encoding.UTF8.GetString(byts);
                req = HttpContext.Current.Server.UrlDecode(req);
                string filename = "log_" + DateTime.Now.ToString("yyyyMMdd") + "_" + System.Guid.NewGuid().ToString("N") + ".txt";
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(context.Server.MapPath("App_Data\\UploadFiles\\" + filename), false, Encoding.UTF8))
                {
                    sw.Write(req);
                    sw.Flush();
                    sw.Close();
                }
                context.Response.Write("上传成功！");
            }catch(Exception ex)
            {
                context.Response.Write("上传失败！"+ex.Message);
            }
          

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}