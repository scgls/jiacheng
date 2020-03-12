using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MESDLL.OutStock
{
    public class item
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string itemName { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public decimal number { get; set; }

        /// <summary>
        /// 商品单价（两位小数）
        /// </summary>
        public decimal itemValue { get; set; }
    }
}
