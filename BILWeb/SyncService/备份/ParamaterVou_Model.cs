using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.SyncService
{
    class ParamaterVou_Model : BILBasic.Basing.Factory.Base_Model
    {
        /// <summary>
        /// 单据名称
        /// </summary>
        public string VoucherName { get; set; }

        /// <summary>
        /// erp单据类型
        /// </summary>
        public int EerVouType { get; set; }

        /// <summary>
        /// ERP单据名称
        /// </summary>
        public string ErpVouName { get; set; }
    }
}
