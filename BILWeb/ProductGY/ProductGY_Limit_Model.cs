using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.ProductGY
{
    public class Mes_ProductGY_LimitInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public Mes_ProductGY_LimitInfo() : base() { }


        /// <summary>
        /// Key（工艺明细ID + 5位流水号）
        /// </summary>
        public string xzID { get; set; }
        /// <summary>
        /// 工艺明细ID
        /// </summary>
        public string gyDetailID { get; set; }
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public string gyLineID { get; set; }
        /// <summary>
        /// 5位流水号
        /// </summary>
        public int seqno { get; set; }
        /// <summary>
        /// 赋值类型（decimal、int、）
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 限制名称
        /// </summary>
        public string xzName { get; set; }
        /// <summary>
        /// 上限值（<=）
        /// </summary>
        public string xzRoleMax { get; set; }
        /// <summary>
        /// 下限值（>=）
        /// </summary>
        public string xzRoleMin { get; set; }
        /// <summary>
        /// 数值
        /// </summary>
        public string QTY { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }


    }
}
