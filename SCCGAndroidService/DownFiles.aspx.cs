using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

public partial class update : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string url= HttpContext.Current.Request.Url.Query;
        string fileName = Request.QueryString["fileName"];
        if (fileName != null)
        {
            string pcPath = Server.MapPath("App_Data\\DownFiles\\");
            string filePath = Path.Combine(pcPath, fileName);
            DownLoadFile(filePath, fileName);
        }
        Response.Write("<script language='javascript'>window.opener=null;window.close();</script>");
    }

    /// <summary>
    /// 文件下载
    /// </summary>
    /// <param name="filePath">路径</param>
    /// <param name="fileName">文件名</param>
    void DownLoadFile(string filePath, string fileName)
    {

        //指定块大小 
        long chunkSize = 102400;
        byte[] buffer = new byte[chunkSize];
        //已读的字节数 
        long dataToRead = 0;
        FileStream stream = null;
        try
        {
            //打开文件 
            stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            dataToRead = stream.Length;

            //添加Http头 
            //HttpUtility.UrlEncode(fileName, Encoding.GetEncoding("GB2312"))
            //HttpContext.Current.Response.Write("<meta   http-equiv=Content-Type   content=text/html;charset=GB2312>");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.UTF8;//注意编码
            Response.AddHeader("Content-Disposition", "attachement;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
            Response.AddHeader("Content-Length", dataToRead.ToString());

            while (dataToRead > 0)
            {
                if (Response.IsClientConnected)
                {
                    int length = stream.Read(buffer, 0, Convert.ToInt32(chunkSize));
                    Response.OutputStream.Write(buffer, 0, length);
                    Response.Flush();
                    Response.Clear();
                    dataToRead -= length;
                }
                else
                {
                    //防止client失去连接 
                    dataToRead = -1;
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Error:" + ex.Message);
        }
        finally
        {
            if (stream != null)
            {
                stream.Close();
            }
            Response.Close();
        }
    }
}
