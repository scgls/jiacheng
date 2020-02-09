using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsService1.ServiceReference1;
using WMSDocumentSynchronizationService;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        string basePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        string syncReceiptTimeFile = ""; //收货同步最后执行时间记录文件
        string syncDeliveryTimeFile = ""; //发货同步最后执行时间记录文件
        string syncBaseTimeFile = ""; //基础数据同步最后执行时间记录文件
        string syncQualityStatuceTimeFile = ""; //质检状态同步最后执行时间记录文件
        string syncReceiptFileName = "LastReceiptTime.ini";
        string syncDeliveryFileName = "LastDeliveryTime.ini";
        string syncBaseFileName = "LastBaseTime.ini";
        string synQualityStatuceFileName = "LastQualityStatucTime.ini";
        public static WindowsService1.ServiceReference1.ServiceClient server = null;


        public Service1()
        {
            InitializeComponent();
        }

        Thread DocumentSyncReceipt;
        Thread DocumentSyncDelivery;
        Thread DocumentSyncBase;
        Thread QualitySyncStatus;
        bool StopSync = false;

        //  protected override void OnStart(string[] args)
        public void OnStart()
        {
            StopSync = false;
            server = WCF.GetWCF();
            syncReceiptTimeFile = Path.Combine(basePath, syncReceiptFileName);
            syncDeliveryTimeFile = Path.Combine(basePath, syncDeliveryFileName);
            syncBaseTimeFile = Path.Combine(basePath, syncBaseFileName);
            syncQualityStatuceTimeFile = Path.Combine(basePath, syncQualityStatuceTimeFile);
            int FirstSyncTime = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["FirstSyncTime"]);
            //同步时间文件不存在，自动创建
            createSyncTimeFile(syncReceiptTimeFile, FirstSyncTime);
            createSyncTimeFile(syncDeliveryTimeFile,FirstSyncTime);
            createSyncTimeFile(syncBaseTimeFile, FirstSyncTime);
            createSyncTimeFile(syncQualityStatuceTimeFile, FirstSyncTime);
            LogNet.LogInfo("Server Start:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //DocumentSyncReceipt = new Thread(new ThreadStart(SyncReceipt));
            //DocumentSyncReceipt.Start();
            //DocumentSyncDelivery = new Thread(new ThreadStart(SyncDelivery));
            //DocumentSyncDelivery.Start();
            DocumentSyncBase = new Thread(new ThreadStart(SyncBase));
            DocumentSyncBase.Start();
            //QualitySyncStatus = new Thread(new ThreadStart(QualityStatus));
            //QualitySyncStatus.Start();
        }

        private void createSyncTimeFile(string syncTimeFile, int FirstSyncTime)
        {
            if (!File.Exists(syncTimeFile))
            {
                File.Create(syncTimeFile).Close();
                using (StreamWriter sw = new StreamWriter(syncTimeFile))
                {
                    sw.WriteLine(DateTime.Now.AddDays(FirstSyncTime).ToString("yyyy-MM-dd HH:mm:ss"));
                    sw.Close();
                }
            }
        }

        protected override void OnStop()
        {
            StopSync = true;
            //while (true)
            //{
            //    if (DocumentSyncReceipt != null && DocumentSyncReceipt.ThreadState != System.Threading.ThreadState.Running)
            //    {
            //        DocumentSyncReceipt.Abort();
            //        DocumentSyncReceipt = null;
            //        break;
            //    }

            //    Thread.Sleep(500);
            //}

            //while (true)
            //{
            //    if (DocumentSyncDelivery != null && DocumentSyncDelivery.ThreadState != System.Threading.ThreadState.Running)
            //    {
            //        DocumentSyncDelivery.Abort();
            //        DocumentSyncDelivery = null;
            //        break;
            //    }

            //    Thread.Sleep(500);
            //}

            //while (true)
            //{
            //    if (QualitySyncStatus != null && QualitySyncStatus.ThreadState != System.Threading.ThreadState.Running)
            //    {
            //        QualitySyncStatus.Abort();
            //        QualitySyncStatus = null;
            //        break;
            //    }

            //    Thread.Sleep(500);
            //}
            while (true)
            {
                //if (DocumentSyncBase != null && DocumentSyncBase.ThreadState != System.Threading.ThreadState.Running)
                //{
                    DocumentSyncBase.Abort();
                    DocumentSyncBase = null;
                    break;
               // }

                Thread.Sleep(500);
            }
            LogNet.LogInfo("Server Stop:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        /// <summary>
        /// 收货数据同步
        /// </summary>
        void SyncReceipt()
        {
            int SyncReceiptTime = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SyncReceiptTime"])*1000;
            while (!StopSync)
            {
                try
                {
                    string lastSyncReceiptTime = string.Empty; //初始时间
                    if (File.Exists(syncReceiptTimeFile))
                    {
                        using (StreamReader sr = new StreamReader(syncReceiptTimeFile))
                        {
                            lastSyncReceiptTime = sr.ReadLine(); //最后执行日期
                            sr.Close();
                        }
                    }
                    LogNet.LogInfo("同步收货数据开始:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    string errMsg = "";
                    bool result = server.DocumentSyncReceipt(lastSyncReceiptTime,String.Empty, ref errMsg, -1, "ERP", -1, null);
                    LogNet.LogInfo("同步收货数据结束:" + result+"|"+ errMsg + "\t同步时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (result)
                    {
                        using (StreamWriter sw = new StreamWriter(syncReceiptTimeFile))
                        {
                            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            sw.Close();
                        }
                    }
                   
                }
                catch (Exception ex)
                {
                    LogNet.LogInfo("同步收货数据错误:" + ex.Message + "\t同步时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                Thread.Sleep(SyncReceiptTime);
            }

        }

        /// <summary>
        /// 发货数据同步
        /// </summary>
        void SyncDelivery()
        {
            int SyncDeliverytTime = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SyncDeliverytTime"])*1000;
            while (!StopSync)
            {
                try
                {
                    string lastSyncDeliveryTime = string.Empty; //初始时间
                    if (File.Exists(syncDeliveryTimeFile))
                    {
                        using (StreamReader sr = new StreamReader(syncDeliveryTimeFile))
                        {
                            lastSyncDeliveryTime = sr.ReadLine(); //最后执行日期
                            sr.Close();
                        }
                    }
                    LogNet.LogInfo("同步发货数据开始:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    string errMsg = "";
                    bool result = server.DocumentSyncDelivery(lastSyncDeliveryTime, String.Empty, ref errMsg, -1, "ERP",-1,null);
                    LogNet.LogInfo("同步发货数据结束:" + result + "|" + errMsg + "\t同步时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (result)
                    {
                        using (StreamWriter sw = new StreamWriter(syncDeliveryTimeFile))
                        {
                            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            sw.Close();
                        }
                    }
                    Thread.Sleep(SyncDeliverytTime);
                }catch(Exception ex)
                {
                    LogNet.LogInfo("同步发货数据错误:" + ex.Message + "\t同步时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Thread.Sleep(SyncDeliverytTime);
                }
               
            }
        }

        /// <summary>
        /// 基础资料同步
        /// </summary>
        void SyncBase()
        {
            int SyncBaseTime = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SyncBaseTime"]) * 1000;
            while (!StopSync)
            {
                try
                {
                    string lastSyncBaseTime = string.Empty; //初始时间
                    if (File.Exists(syncBaseTimeFile))
                    {
                        using (StreamReader sr = new StreamReader(syncBaseTimeFile))
                        {
                            lastSyncBaseTime = sr.ReadLine(); //最后执行日期
                            sr.Close();
                        }
                    }
                    LogNet.LogInfo("同步基础数据开始:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    string errMsg = "";
                    bool result = server.DocumentSyncBase(lastSyncBaseTime, String.Empty, ref errMsg, -1,"ERP", -1, null);
                    LogNet.LogInfo("同步基础数据结束:" + result + "|" + errMsg + "\t同步时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (result)
                    {
                        using (StreamWriter sw = new StreamWriter(syncBaseTimeFile))
                        {
                            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd"));
                            sw.Close();
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogNet.LogInfo("同步基础数据错误:" + ex.Message + "\t同步时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                Thread.Sleep(SyncBaseTime);
            }

        }
        
        /// <summary>
        ///质检同步
        /// </summary>
        //void QualityStatus()
        //{
        //    int SyncQualityStatusTime = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["QualitySyncStatus"]) * 1000;
        //    while (!StopSync)
        //    {
        //        try
        //        {
        //            string lastQualityStatusTime = string.Empty; //初始时间
        //            if (File.Exists(syncBaseTimeFile))
        //            {
        //                using (StreamReader sr = new StreamReader(syncQualityStatuceTimeFile))
        //                {
        //                    lastQualityStatusTime = sr.ReadLine(); //最后执行日期
        //                    sr.Close();
        //                }
        //            }
        //            LogNet.LogInfo("同步质检数据开始:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //            string errMsg = "";
        //            List<T_QualityInfo> modelList = new List<T_QualityInfo>();
        //            string strError = String.Empty;
        //            bool result = server.GetT_AllQualityList(ref modelList, ref strError);
        //            if(result && modelList.Count != 0)
        //            {
        //                foreach (T_QualityInfo model in modelList)
        //                {
        //                    result = server.DocumentSyncReceipt(lastQualityStatusTime, model.ErpVoucherNo, ref errMsg, 20, "ERP", -1, null);
        //                    if (!result)
        //                        break;
        //                }
                           
        //            }
        //            LogNet.LogInfo("同步质检数据结束:" + result + "|" + strError +"|"+ errMsg + "\t同步时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //            if (result)
        //            {
        //                using (StreamWriter sw = new StreamWriter(syncBaseTimeFile))
        //                {
        //                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd"));
        //                    sw.Close();
        //                }
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            LogNet.LogInfo("同步质检数据错误:" + ex.Message + "\t同步时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //        }
        //        Thread.Sleep(SyncQualityStatusTime);
        //    }

        //}
    }
}
