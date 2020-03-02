using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.TransportSupplier
{

    public class TransportSupplierDetail 
    {
        public int id { get; set; }
        public string platenumber { get; set; }//车牌号
        public int isdel { get; set; }

        //新增
        public string palletno { get; set; }
        public string boxcount { get; set; }
        public string outboxcount { get; set; }
        public string erpvoucherno { get; set; }
        public string creater { get; set; }
        
        public string customername { get; set; }
        public string FEIGHT { get; set; }
        public string voucherno { get; set; }
        public string type { get; set; }
        public string remark { get; set; }
        public string remark1 { get; set; }
        public string remark2 { get; set; }
        public string remark3 { get; set; }

        public int VoucherType { get; set;  }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string ExpNo { get; set; }

        /// <summary>
        /// 物流费用
        /// </summary>
        public string ExpAmount { get; set; }

        public string guid { get; set; }
        /// <summary>
        /// 仓退单C,销货单A
        /// </summary>
        public string gtype { get; set; }

        public string TradingConditionsCode { get; set; }
    }
}

