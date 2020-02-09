using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Box
{
    public class T_BoxInfo : BILBasic.Basing.Factory.Base_Model
    {
        [Display(Name = "序号")]
        public string DelNo { get; set; }

        [Display(Name = "箱号")]
        public string SerialNo { get; set; }

        [Display(Name = "组织")]
        public string HeaderName { get; set; }

        [Display(Name = "客户名称")]
        public string CustomerName { get; set; }

        [Display(Name = "标记")]
        public string Flag { get; set; }//整|零

        [Display(Name = "备注")]
        public string Remark { get; set; }//备注

        public int IsDel { get; set; }
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }

        public int IsAmount { get; set; }
        public string FserialNo { get; set; }
        public int IsPin { get; set; }

        public decimal Weight { get; set; }

        public decimal Volume { get; set; }

        public int CountBox { get; set; }
    }
}
