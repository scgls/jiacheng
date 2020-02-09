using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BILWeb.BaseInfo
{
    public class T_System
    {
        public T_System()
        {}
        public int id { get; set; }
        [Display(Name ="Logo路径")]
        public string filepath { get; set; }
        [Display(Name = "公司名称")]
        public string companyname { get; set; }
        public string remark { get; set; }
        public string remark1 { get; set; }
        public string remark2 { get; set; }
        public string remark3 { get; set; }
        public string remark4 { get; set; }
        public string remark5 { get; set; }
        public string remark6 { get; set; }
        public string remark7 { get; set; }
        public string remark8 { get; set; }
        public string remark9 { get; set; }
    }
}
