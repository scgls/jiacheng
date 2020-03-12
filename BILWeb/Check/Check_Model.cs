using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Query
{
    public class Check_Model
    {
        public   int     ID	{get;set;}
        [Display(Name ="盘点单号")]
        public string CHECKNO { get; set; }
        [Display(Name = "盘点单类别")]
        public string CHECKTYPE { get; set; }
        [Display(Name = "盘点单描述")]
        public string CHECKDESC { get; set; }
        [Display(Name = "状态")]
        public string CHECKSTATUS { get; set; }
        public DateTime? CBEGINTIME { get; set; }
        [Display(Name = "结束时间")]
        public DateTime? CDONETIME { get; set; }
        [Display(Name = "备注")]
        public string REMARKS { get; set; }
        public int ISDEL { get; set; }
        [Display(Name = "创建人")]
        public string CREATER { get; set; }
        [Display(Name = "创建时间")]
        public DateTime? CREATETIME { get; set; }
        [Display(Name = "开始时间")]
        public string begintime { get; set; }
        [Display(Name = "结束时间")]
        public string endtime { get; set; }
        public bool ischeck { get; set; }  

    }
}
