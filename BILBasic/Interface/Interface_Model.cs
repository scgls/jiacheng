using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILBasic.Interface
{
    public class T_InterfaceInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_InterfaceInfo() : base() { }


        //私有变量
        
        private string vouchername;
       
        private string function;
        private string dllname;
        private string route;
        private string functionname;
        private string functionnote;
       



        //公开属性
       

        public string VoucherName
        {
            get
            {
                return vouchername;
            }
            set
            {
                vouchername = value;
            }
        }

        

        public string Function
        {
            get
            {
                return function;
            }
            set
            {
                function = value;
            }
        }

        public string DLLName
        {
            get
            {
                return dllname;
            }
            set
            {
                dllname = value;
            }
        }

        public string Route
        {
            get
            {
                return route;
            }
            set
            {
                route = value;
            }
        }

        public string FunctionName
        {
            get
            {
                return functionname;
            }
            set
            {
                functionname = value;
            }
        }

        public string FunctionNote
        {
            get
            {
                return functionnote;
            }
            set
            {
                functionnote = value;
            }
        }

        public string ClassName { get; set; }
    }

   

    public class JsonModel
    {
        public string result { get; set; }

        public string resultValue { get; set; }

        public string ErrMsg { get; set; }

        public string srvver { get; set; }
        public string srvcode { get; set; }
        public PayLoad payload { get; set; }
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
