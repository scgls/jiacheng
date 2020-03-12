using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Boxing
{
    public class T_BoxingInfo : BILBasic.Basing.Factory.Base_Model
    {
        //public int ID;
        [Display(Name = "清单号")]
        public string SerialNo;
        [Display(Name = "物料号")]
        public string MaterialNo;
        [Display(Name = "物料名")]
        public string MaterialName;
        [Display(Name = "数量")]
        public decimal Qty;
        //public string TaskNo;
        //public string Creater;
        //public DateTime CreateTime;
        //public string Modifyer;
        //public DateTime ModifyTime;
        public int IsDel;
        public string Remark;
        public string Remark1;
        public string Remark2;
        public string FserialNo { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }

        public int IsPin { get; set; }
    }
}
