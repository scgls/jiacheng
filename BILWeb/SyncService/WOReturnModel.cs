using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.SyncService
{
    public class WOReturnModel
    {

        public int VoucherType { get; set; }

        public string StrongHoldCode { get; set; }
        
        /// <summary>
        /// 工单单号
        /// </summary>
        public string ErpVoucherNo { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public string ERPVoucherType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int WmsStatus { get; set; }

    }

    public class OrderReturnModel
    {

        public int VoucherType { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string ErpVoucherNo { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string ERPVoucherType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string WmsStatus { get; set; }

        public string StrongHoldCode { get; set; }

    }
}
