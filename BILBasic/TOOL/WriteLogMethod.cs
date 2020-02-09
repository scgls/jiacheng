using System.IO;
using System.Web;
using System.Configuration;


namespace BILBasic.TOOL
{
   public class WriteLogMethod
    {

        private static readonly string strFlag = ConfigurationManager.ConnectionStrings["logflag"].ConnectionString;

        public static bool bLog = strFlag == "true" ? true : false;

       public static void  WriteLog(string logtxt)
       {
           //如果bLog为false，则不打印Log
           if (!bLog) return;

           string strLog = GetDateTimeNow() + logtxt;
           Log(strLog);
       }

       public static void LogNoTimeStamp(string logtxt)
       {
           //如果bLog为false，则不打印Log
           if (!bLog) return;

           Log(logtxt);
       }

       private static string GetDateTimeNow()
       {
           return System.DateTime.Now.ToString("yyyy年MM月dd日  HH时 mm分 ss秒  fff毫秒");

       }

      

       private static void Log(string logtxt)
       {
           if (!bLog) return;

           string LogPath = HttpContext.Current.Server.MapPath("LogFIle.txt");
           using (StreamWriter sw = new StreamWriter(LogPath, true, System.Text.Encoding.Default))
           {
               sw.WriteLine(logtxt);
           }
       }

    }
}
