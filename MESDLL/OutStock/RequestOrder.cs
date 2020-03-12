using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MESDLL.OutStock
{
    public class RequestOrder
    {
        /// <summary>
        /// 商家代码（必须与customerId一致）
        /// </summary>
        public string clientID { get; set; }

        /// <summary>
        /// 物流公司ID 默认YTO
        /// </summary>
        public string logisticProviderID { get; set; }

        /// <summary>
        /// 商家代码 (由商家设置， 必须与clientID一致)
        /// </summary>
        public string customerId { get; set; }

        /// <summary>
        /// 物流订单号
        /// </summary>
        public string txLogisticID { get; set; }

        /// <summary>
        /// 渠道名称 同customerId一致
        /// </summary>
        public string tradeNo { get; set; }

        /// <summary>
        /// 保值金额=insuranceValue*货品数量(默认为0.0）
        /// </summary>
        public string totalServiceFee { get; set; }

        /// <summary>
        /// 物流公司分润[COD] （暂时没有使用，默认为0.0）
        /// </summary>
        public string codSplitFee { get; set; }

        /// <summary>
        /// 订单类型(0-COD,1-普通订单,2-便携式订单,3-退货单,4-到付)
        /// </summary>
        public string orderType { get; set; }

        /// <summary>
        /// 服务类型(1-上门揽收, 0-自己联系)。默认为0
        /// </summary>
        public string serviceType { get; set; }

        /// <summary>
        /// 产品类型 1
        /// </summary>
        public string flag { get; set; }

        /// <summary>
        /// 物流公司上门取货时间段，通过”yyyy-MM-dd HH:mm:ss”格式化，本文中所有时间格式相同。
        /// </summary>
        public string sendStartTime { get; set; }

        /// <summary>
        /// 物流公司上门取货时间段，通过”yyyy-MM-dd HH:mm:ss”格式化，本文中所有时间格式相同。
        /// </summary>
        public string sendEndTime { get; set; }

        /// <summary>
        /// 商品金额，包括优惠和运费，但无服务费
        /// </summary>
        public decimal goodsValue { get; set; }

        /// <summary>
        /// 货物价值
        /// </summary>
        public decimal itemsValue { get; set; }

        /// <summary>
        /// 保值金额 （保价金额为货品价值（大于等于100少于3w），默认为0.0）
        /// </summary>
        public decimal insuranceValue { get; set; }

        /// <summary>
        /// 商品类型（保留字段，暂时不用）
        /// </summary>
        public string special { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        public string deliverNo { get; set; }

        /// <summary>
        /// 订单类型（该字段是为向下兼容预留）
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// goodsValue+总服务费
        /// </summary>
        public string totalValue { get; set; }

        /// <summary>
        /// 货物总重量
        /// </summary>
        public decimal itemsWeight { get; set; }

        public string packageOrNot { get; set; }

        public string orderSource { get; set; }

        public string parternId { get; set; }

        #region sender

        public string name { get; set; }

        /// <summary>
        /// 用户邮编
        /// </summary>
        public string postCode { get; set; }

        /// <summary>
        /// 用户电话，包括区号、电话号码及分机号，中间用“-”分隔；
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 用户移动电话， 手机和电话至少填一项
        /// </summary>
        public string mobile { get; set; }

        public string prov { get; set; }

        public string city { get; set; }


        public string address { get; set; }

        #endregion

        #region receiver
        public string namer { get; set; }

        /// <summary>
        /// 用户邮编
        /// </summary>
        public string postCoder { get; set; }

        /// <summary>
        /// 用户电话，包括区号、电话号码及分机号，中间用“-”分隔；
        /// </summary>
        public string phoner { get; set; }

        /// <summary>
        /// 用户移动电话， 手机和电话至少填一项
        /// </summary>
        public string mobiler { get; set; }

        public string provr { get; set; }

        public string arear { get; set; }

        public string cityr { get; set; }


        public string addressr { get; set; }

        #endregion

        #region item

        /// <summary>
        /// 商品名称
        /// </summary>
        public string itemName { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public string number { get; set; }

        /// <summary>
        /// 商品单价（两位小数）
        /// </summary>
        public string itemValue { get; set; }

        #endregion

        public Sender sender { get; set; }

        public Receiver receiver { get; set; }

        public List<item> items { get; set; }

    }
}
