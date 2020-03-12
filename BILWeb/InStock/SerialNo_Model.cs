using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.InStock
{
    public class T_SerialNoInfo : BILBasic.Basing.Factory.Base_Model
    {
        public string FacMaterialNo { get; set; }

        public string SerialNo { get; set; }

        public string ERPVoucherNo { get; set; }

        public decimal SerialQty { get; set; }

        /// <summary>
        /// 客户端提交的SerialNo包含@信息，先赋值给SerialNoWMS，再对SerialNo通过@符号解析，提交给ERP
        /// </summary>
        public string SerialNoWMS { get; set; }

        public string MaterialDesc { get; set; }

        public string VoucherNo { get; set; }

        /// <summary>
        /// 是否自动分配，EXCEL导入1-未分配 2-已分配
        /// </summary>
        public int IsDis { get; set; }

        public string RowNo { get; set; }

        public string AreaNo { get; set; }
    }
}
