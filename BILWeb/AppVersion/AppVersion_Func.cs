using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BILBasic.Common;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BILWeb.AppVersion
{
    public class AppVersion_Func
    {
        private AppVersion_DB _db = new AppVersion_DB();

        public List<string> GetIPListByHostAddresses()
        {
            List<string> lstIP = new List<string>();
            string hostName = Dns.GetHostName();//本机名   
            IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6   
            string strIP;
            foreach (IPAddress ip in addressList)
            {
                strIP = IPAddressToString(ip);
                if (string.IsNullOrEmpty(strIP)) continue;
                lstIP.Add(strIP);
            }
            return lstIP;
        }
        private string IPAddressToString(IPAddress ip)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                if (ip.ToString().IndexOf('.') < 0) return string.Empty;
                string[] arrIP = ip.ToString().Split('.');
                long p1 = long.Parse(arrIP[0]);
                long p2 = long.Parse(arrIP[1]);
                long p3 = long.Parse(arrIP[2]);
                long p4 = arrIP.Length >= 4 ? long.Parse(arrIP[3]) : 0;

                return string.Format("{0}.{1}.{2}.{3}", p1, p2, p3, p4);
            }
            else
            {
                return "";
            }
        }
        private void DownloadFile(string FileName, string strUrl)
        {
            HttpWebRequest webRequest;
            HttpWebResponse webResponse = null;
            FileWebRequest fileRequest;
            FileWebResponse fileResponse = null;
            bool isFile = false;
            try
            {
                System.Globalization.DateTimeFormatInfo dfi = null;
                System.Globalization.CultureInfo ci = null;
                ci = new System.Globalization.CultureInfo("zh-CN");
                dfi = new System.Globalization.DateTimeFormatInfo();

                //WebRequest wr = WebRequest.Create("");

                //System.Net.WebResponse w=wr.
                DateTime fileDate;
                long totalBytes;
                DirectoryInfo theFolder = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                string fileName = Path.Combine(theFolder.FullName, FileName);

                isFile = (HttpWebRequest.Create(strUrl) is FileWebRequest);

                if (isFile)
                {
                    fileRequest = (FileWebRequest)FileWebRequest.Create(strUrl);
                    fileResponse = (FileWebResponse)fileRequest.GetResponse();
                    if (fileResponse == null)
                        return;
                    fileDate = DateTime.Now;
                    totalBytes = fileResponse.ContentLength;
                }
                else
                {
                    webRequest = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                    webResponse = (HttpWebResponse)webRequest.GetResponse();
                    if (webResponse == null)
                        return;
                    fileDate = webResponse.LastModified;
                    totalBytes = webResponse.ContentLength;
                }

                //pbUpdate.Maximum = Convert.ToInt32(totalBytes);

                Stream stream;
                if (isFile)
                {
                    stream = fileResponse.GetResponseStream();
                }
                else
                {
                    stream = webResponse.GetResponseStream();
                }
                FileStream sw = new FileStream(fileName, FileMode.Create);
                int totalDownloadedByte = 0;
                Byte[] @by = new byte[1024];
                int osize = stream.Read(@by, 0, @by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    sw.Write(@by, 0, osize);
                    //pbUpdate.Value = totalDownloadedByte;
                    osize = stream.Read(@by, 0, @by.Length);
                }
                sw.Close();
                stream.Close();

                File.SetLastWriteTime(FileName, fileDate);

            }
            catch //(Exception ex)
            {
                if (fileResponse != null)
                    fileResponse.Close();

                if (webResponse != null)
                    webResponse.Close();
            }
        }

        /// <summary>
        /// 检查版本更新
        /// </summary>
        /// <param name="appversion">版本信息</param>
        /// <returns>是否需要更新</returns>
        public bool VerifyVersion(ref AppVersionInfo appversion, ref string strError)
        {
            try
            {
                string UpdateIP = string.Empty;
                string UpdateDir = string.Empty;
                Match m = Regex.Match(appversion.UpdateUrl, @"\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}");
                if (m.Success)
                {
                    UpdateIP = m.Value;
                    UpdateDir = appversion.UpdateUrl.Substring(appversion.UpdateUrl.IndexOf('/', appversion.UpdateUrl.IndexOf(UpdateIP) + UpdateIP.Length) + 1);
                }
                else
                {
                    //strError = "更新地址请使用IP域名";
                    //return false;
                }

                string NewAppPath = string.Empty;
                if (!string.IsNullOrEmpty(UpdateIP) && GetIPListByHostAddresses().Contains(UpdateIP))
                {
                    NewAppPath = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, Path.Combine(UpdateDir, appversion.FileName));

                    if (!File.Exists(NewAppPath))
                    {
                        //strError = "没有找到可对比的程序";
                        return false;
                    }

                    FileVersionInfo fv = FileVersionInfo.GetVersionInfo(NewAppPath);
                    appversion.AppVersion = fv.FileVersion;
                }
                else
                {
                    string LocalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, appversion.FileName);
                    NewAppPath = Path.Combine(appversion.UpdateUrl, appversion.FileName);
                    DownloadFile(appversion.FileName, NewAppPath);

                    if (!File.Exists(LocalPath))
                    {
                        //strError = "没有找到可对比的程序";
                        return false;
                    }

                    FileVersionInfo fv = FileVersionInfo.GetVersionInfo(LocalPath);
                    appversion.AppVersion = fv.FileVersion;
                    File.Delete(LocalPath);
                }

                if (CompareVersion(appversion))
                {
                    AppVersionInfo model = new AppVersionInfo();
                    model.AppVersion = appversion.AppVersion;
                    model.AppName = appversion.AppName;
                    if (GetAppVersionByVersion(ref model, ref strError))
                    {
                        model.LocalVersion = appversion.LocalVersion;
                        model.UpdateUrl = appversion.UpdateUrl;
                        model.UpdateAppName = appversion.UpdateAppName;
                        model.UpdateAppPath = appversion.UpdateAppPath;
                        model.FileName = appversion.FileName;
                        appversion = model;
                    }
                    return true;
                }
                else
                {
                    //strError = "当前程序已经是最新版本";
                    return false;
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        private bool CompareVersion(AppVersionInfo appversion)
        {
            if (string.IsNullOrEmpty(appversion.AppVersion)) return false;
            if (string.IsNullOrEmpty(appversion.LocalVersion)) return true;

            string[] arrService = appversion.AppVersion.Split('.');
            string[] arrLocal = appversion.LocalVersion.Split('.');

            if (arrService.Length != 4) return false;
            if (arrLocal.Length != 4) return true;

            long lService = arrService[0].ToLong() * 1000000000L + arrService[1].ToLong() * 1000000L + arrService[2].ToLong() * 1000L + arrService[3].ToLong();
            long lLocal = arrLocal[0].ToLong() * 1000000000L + arrLocal[1].ToLong() * 1000000L + arrLocal[2].ToLong() * 1000L + arrLocal[3].ToLong();

            return lService > lLocal;
        }


        public bool GetAppVersionByVersion(ref AppVersionInfo model, ref string strError)
        {
            try
            {
                using (IDataReader dr = _db.GetAppVersionByVersion(model))
                {
                    if (dr.Read())
                    {
                        model = (GetModelFromDataReader(dr));
                        return true;
                    }
                    else
                    {
                        strError = "找不到任何数据";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
            finally
            {
            }
        }

        internal AppVersionInfo GetModelFromDataReader(IDataReader dr)
        {
            AppVersionInfo model = new AppVersionInfo();
            model.ID = dr["ID"].ToInt32();
            model.AppName = dr["AppName"].ToDBString();
            model.AppVersion = dr["AppVersion"].ToDBString();
            model.VersionType = dr["VersionType"].ToInt32();
            model.VersionLevel = dr["VersionLevel"].ToInt32();
            model.VersionTitle = dr["VersionTitle"].ToDBString();
            model.VersionDesc = dr["VersionDesc"].ToDBString();
            model.Creater = dr["Creater"].ToDBString();
            model.CreateTime = dr["CreateTime"].ToDateTime();

            if (Common_Func.readerExists(dr, "StrVersionType")) model.StrVersionType = dr["StrVersionType"].ToDBString();
            if (Common_Func.readerExists(dr, "StrVersionLevel")) model.StrVersionLevel = dr["StrVersionLevel"].ToDBString();

            return model;
        }

        /// <summary>
        /// 检查版本
        /// </summary>
        /// <param name="FileVersion">文件版本</param>
        /// <param name="FileName">文件名</param>
        /// <param name="path">更新地址</param>
        /// <returns>是否需要更新</returns>
        public bool VerifyVersion(string FileVersion, string FileName, string path)
        {
            try
            {
                string ServiceVersion = null;
                string strPath = AppDomain.CurrentDomain.BaseDirectory + path + "\\" + FileName;
                if (!File.Exists(strPath))
                {
                    return false;
                }

                FileVersionInfo fv = FileVersionInfo.GetVersionInfo(strPath);
                ServiceVersion = fv.FileVersion;

                if (ServiceVersion != FileVersion)
                {
                    return CompareVersion(ServiceVersion, FileVersion);
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private bool CompareVersion(string ServiceVersion, string FileVersion)
        {
            if (string.IsNullOrEmpty(ServiceVersion)) return false;
            if (string.IsNullOrEmpty(FileVersion)) return true;

            string[] arrService = ServiceVersion.Split('.');
            string[] arrFile = FileVersion.Split('.');

            if (arrService.Length != 4) return false;
            if (arrFile.Length != 4) return true;

            long lService = arrService[0].ToInt32() * 1000000000 + arrService[1].ToInt32() * 1000000 + arrService[2].ToInt32() * 1000 + arrService[3].ToInt32();
            long lFile = arrFile[0].ToInt32() * 1000000000 + arrFile[1].ToInt32() * 1000000 + arrFile[2].ToInt32() * 1000 + arrFile[3].ToInt32();

            return lService > lFile;
        }
    }
}
