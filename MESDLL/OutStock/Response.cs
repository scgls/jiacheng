using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MESDLL.OutStock
{
    public class Response
    {
        /// <summary>
        /// 客户编码
        /// </summary>
        public string clientID { get; set; }

        /// <summary>
        /// 成功或失败编码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 时效类型
        /// </summary>
        public string effectType { get; set; }

        /// <summary>
        /// 承诺送达时间
        /// </summary>
        public string estimatedTrrivalTime { get; set; }

        /// <summary>
        /// 物流公司ID
        /// </summary>
        public string logisticProviderID { get; set; }

        /// <summary>
        /// 成功绑定的面单号
        /// </summary>
        public string mailNo { get; set; }

        /// <summary>
        /// 始发网点
        /// </summary>
        public string originateOrgCode { get; set; }

        public string pickUpTime { get; set; }

        public string qrCode { get; set; }

        public string success { get; set; }

        /// <summary>
        /// 物流订单号
        /// </summary>
        public string txLogisticID { get; set; }

        public string reason { get; set; }

        ///// <summary>
        ///// 末端网点代码
        ///// </summary>
        //public string consigneeBranchCode { get; set; }

        ///// <summary>
        ///// 集包地中心代码
        ///// </summary>
        //public string packageCenterCode { get; set; }

        ///// <summary>
        ///// 集包地中心名称
        ///// </summary>
        //public string packageCenterName { get; set; }

        ///// <summary>
        ///// 五级地址
        ///// </summary>
        //public string printKeyWord { get; set; }

        ///// <summary>
        ///// 三段码
        ///// </summary>
        //public string shortAddress { get; set; }


        public distributeInfos distributeInfo { get; set; }

    }
}
