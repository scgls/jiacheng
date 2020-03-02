using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.AppVersion
{
    public class AppVersionInfo
    {
        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _AppName;

        public string AppName
        {
            get { return _AppName; }
            set { _AppName = value; }
        }
        private string _AppVersion;

        public string AppVersion
        {
            get { return _AppVersion; }
            set { _AppVersion = value; }
        }
        private int _VersionType;

        public int VersionType
        {
            get { return _VersionType; }
            set { _VersionType = value; }
        }
        private int _VersionLevel;

        public int VersionLevel
        {
            get { return _VersionLevel; }
            set { _VersionLevel = value; }
        }
        private string _VersionTitle;

        public string VersionTitle
        {
            get { return _VersionTitle; }
            set { _VersionTitle = value; }
        }
        private string _VersionDesc;

        public string VersionDesc
        {
            get { return _VersionDesc; }
            set { _VersionDesc = value; }
        }
        private string _Creater;

        public string Creater
        {
            get { return _Creater; }
            set { _Creater = value; }
        }
        private DateTime _CreateTime;

        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }



        public string LocalVersion { get; set; }
        public string UpdateAppName { get; set; }
        public string UpdateAppPath { get; set; }
        public string FileName { get; set; }
        public string UpdateUrl { get; set; }

        public string StrVersionType { get; set; }
        public string StrVersionLevel { get; set; }
    }
}
