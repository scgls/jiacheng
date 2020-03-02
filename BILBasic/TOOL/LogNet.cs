using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

    public class LogNet
    {
        public static bool debugeKey=true;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("WebLogger");
        private static log4net.ILog logInfo = log4net.LogManager.GetLogger("syncinfo");
        private static log4net.ILog logWarn = log4net.LogManager.GetLogger("syncwarn");
        private static log4net.ILog logError = log4net.LogManager.GetLogger("syncerror");

        public LogNet()
        {
            debugeKey = true;
        }

        public static void SyncError(string Message)
        {
            if (!logError.IsErrorEnabled)
                SetConfig();
            logError.Error(Message);
        }

        private static void SetConfig()
        {
            object o = ConfigurationManager.GetSection("log4net");
            log4net.Config.XmlConfigurator.Configure(o as System.Xml.XmlElement);
        }

        public static void SyncWarn(string Message)
        {
            if (!logWarn.IsWarnEnabled)
                SetConfig();
            logWarn.Warn(Message);
        }

        public static void LogInfo(string Message)
        {
            if (!log.IsInfoEnabled)
                SetConfig();
            if(debugeKey)
                log.Info(Message);
        }

        public static void SyncInfo(string Message)
        {
            if (!logInfo.IsInfoEnabled)
                SetConfig();
            logInfo.Info(Message);
        }

        public static void LogInfo(string Message, Exception ex)
        {
            if (!log.IsInfoEnabled)
                SetConfig();
        if (debugeKey)
            log.Info(Message, ex);
        }
        public static void ErrorInfo(string Message)
        {
            if (!log.IsInfoEnabled)
                SetConfig();
            log.Error(Message);
        }

        public static void DebugInfo(string Message)
        {
            if (!log.IsInfoEnabled)
                SetConfig();
        if (debugeKey)
            log.Debug(Message);
        }

    }