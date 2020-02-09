using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.DepInterface
{
    public class T_DepInterfaceInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_DepInterfaceInfo() : base() { }


        //私有变量
        
        private int vouchername;
        
        private int function;
        private string dllname;
        private string route;
        private string functionname;
        private string functionnote;
       



        //公开属性

        [Display(Name ="单据名称")]
        public int VoucherName
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


        [Display(Name = "功能")]
        public int Function
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
        [Display(Name = "DLL名称")]
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
        [Display(Name = "路径")]
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
        [Display(Name = "函数名称")]
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
        [Display(Name = "功能说明")]
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
        [Display(Name = "类名")]
        public string ClassName { get; set; }

        public string StrVoucherName { get; set; }
        [Display(Name = "功能")]
        public string StrFunction { get; set; }

        
    }
}
