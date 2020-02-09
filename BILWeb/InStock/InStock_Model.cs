using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.InStock
{
    public class T_InStockInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_InStockInfo() : base() { }


        //私有变量
       
        private string voucherno;
        
        private string supplierno;
        private string suppliername;
        private decimal? isquality;
        private decimal? isreceivepost;
        private decimal? isshelvepost;
        
        private string plant;
        private string plantname;
        private string movetype;
        private int? advInStatus;

        public int? AdvInStatus
        {
            get { return advInStatus; }
            set { advInStatus = value; }
        }

        //公开属性

        [Display(Name = "单据号")]
        public string VoucherNo
        {
            get
            {
                return voucherno;
            }
            set
            {
                voucherno = value;
            }
        }


        [Display(Name = "供应商编号")]
        public string SupplierNo
        {
            get
            {
                return supplierno;
            }
            set
            {
                supplierno = value;
            }
        }
        [Display(Name ="供应商")]
        public string SupplierName
        {
            get
            {
                return suppliername;
            }
            set
            {
                suppliername = value;
            }
        }

        public decimal? IsQuality
        {
            get
            {
                return isquality;
            }
            set
            {
                isquality = value;
            }
        }

        public decimal? IsReceivePost
        {
            get
            {
                return isreceivepost;
            }
            set
            {
                isreceivepost = value;
            }
        }

        public decimal? IsShelvePost
        {
            get
            {
                return isshelvepost;
            }
            set
            {
                isshelvepost = value;
            }
        }

       

        public string Plant
        {
            get
            {
                return plant;
            }
            set
            {
                plant = value;
            }
        }

        public string PlantName
        {
            get
            {
                return plantname;
            }
            set
            {
                plantname = value;
            }
        }

        public string MoveType
        {
            get
            {
                return movetype;
            }
            set
            {
                movetype = value;
            }
        }

        public List<T_InStockDetailInfo> lstDetail { get; set; }

        public string Note { get; set; }

        public string QcCode { get; set; }
        public string QcDesc { get; set; }

    }
}
