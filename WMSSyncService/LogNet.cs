using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

    public class LogNet
    {
        public static bool debugeKey=true;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("WebLogger");

        public LogNet()
        {
            debugeKey = true;
        }

        private static void SetConfig()
        {
            object o = ConfigurationManager.GetSection("log4net");
            log4net.Config.XmlConfigurator.Configure(o as System.Xml.XmlElement);
        }

    public static void LogInfo(string Message)
    {

        if (!log.IsInfoEnabled)
            SetConfig();
        if (debugeKey)
            log.Info(Message);
        Console.WriteLine(Message);
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