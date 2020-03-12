using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Stock
{
    /// <summary>
    /// t_stock的实体类
    /// 作者:方颖
    /// 日期：2016/12/12 13:22:14
    /// </summary>

    public class T_StockInfoEX : T_StockInfo
    {
        //无参构造函数
        public T_StockInfoEX() : base() { }

        public string ManagerModel { get; set; }
        [Display(Name ="序号")]
        public string XH { get; set; }

        [Display(Name = "检验状态")]
        public string StatusName { get; set; }
        [Display(Name = "序号")]
        public string ReceiveStatusName { get; set; }

        public DateTime PrintCreateTime { get; set; }

        public string PrintCreater { get; set; }
        public int BarcodeNo { get; set; }
        [Display(Name = "批次")]
        public string productbatch { get; set; }

        public string storecondition { get; set; }


        public bool IsCheck { get; set; }

        public string specialrequire { get; set; }
        public string protectway { get; set; }
        public string boxweight { get; set; }
        public string boxdetail { get; set; }

        public string labelmark { get; set; }
        public string matebatch { get; set; }
        public DateTime mixdate { get; set; }


        public string relaweight { get; set; }
        public string productclass { get; set; }

        public decimal PrintQty { get; set; }

        public decimal ItemQty { get; set; }

        public string WorkNo { get; set; }


        public string SupplierNo { get; set; }

        public string SupplierName { get; set; }

        public string RecPeo { get; set; }

        public string DelAddress { get; set; }
        public string SendPeo { get; set; }
        public string PostPeo { get; set; }
        public string WareAddress { get; set; }

        public decimal SQTY { get; set; }

        public string ERPBarcode { get; set; }

        public int IsAmount { get; set; }
        [Display(Name = "拆/原")]
        public string StrIsAmount { get; set; }
    }
}
