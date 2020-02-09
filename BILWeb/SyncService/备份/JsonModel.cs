using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.SyncService
{
    //public class JsonModel
    //{
    //    public string Result { get; set; }

    //    public string ErrMsg { get; set; }

    //    public JArray RecordList { get; set; }
    //}

    public class JsonModel
    {
        public string result { get; set; }

        public string resultValue { get; set; }

        public JArray data { get; set; }
        //public string srvver { get; set; }
        //public string srvcode { get; set; }
        //public PayLoad payload { get; set; }
    }

    public class PayLoad
    {
        public Std std_data { get; set; }
    }

    public class Std
    {
        public Exe execution { get; set; }
        public Parameter parameter { get; set; }
    }


    public class Exe
    {
        public string code { get; set; }
        public string sqlcode { get; set; }

        public string description { get; set; }
    }

    public class Parameter
    {
        public JArray data { get; set; }
    }




}
