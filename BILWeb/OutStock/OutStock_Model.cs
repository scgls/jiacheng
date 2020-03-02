using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.OutStock
{
    public class T_OutStockInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_OutStockInfo() : base() { }


        //私有变量
        private string customercode;
        private string customername;
        private decimal? isoutstockpost;
        private decimal? isundershelvepost;
        private string plant;
        private string plantname;
        private string movetype;
        private string supcode;
        private string supname;       
        private string movereasoncode;
        private string movereasondesc;
        private int reviewstatus;
        private DateTime? outstockdate;
       



        //公开属性
        [Display(Name="客户编号")]
        public string CustomerCode
        {
            get
            {
                return customercode;
            }
            set
            {
                customercode = value;
            }
        }
        [Display(Name = "客户名")]
        public string CustomerName
        {
            get
            {
                return customername;
            }
            set
            {
                customername = value;
            }
        }

        public decimal? IsOutStockPost
        {
            get
            {
                return isoutstockpost;
            }
            set
            {
                isoutstockpost = value;
            }
        }

        public decimal? IsUnderShelvePost
        {
            get
            {
                return isundershelvepost;
            }
            set
            {
                isundershelvepost = value;
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
        [Display(Name = "供应商编号")]
        public string SupCode
        {
            get
            {
                return supcode;
            }
            set
            {
                supcode = value;
            }
        }
        [Display(Name = "供应商")]
        public string SupName
        {
            get
            {
                return supname;
            }
            set
            {
                supname = value;
            }
        }

        

        public string MoveReasonCode
        {
            get
            {
                return movereasoncode;
            }
            set
            {
                movereasoncode = value;
            }
        }

        public string MoveReasonDesc
        {
            get
            {
                return movereasondesc;
            }
            set
            {
                movereasondesc = value;
            }
        }

        public int ReviewStatus
        {
            get
            {
                return reviewstatus;
            }
            set
            {
                reviewstatus = value;
            }
        }

        public DateTime? OutStockDate
        {
            get
            {
                return outstockdate;
            }
            set
            {
                outstockdate = value;
            }
        }
        [Display(Name = "单据号")]
        public string VoucherNo { get; set; }

        [Display(Name = "备注")]
        public string Note { get; set; }

        /// <summary>
        /// 发料日期
        /// </summary>
        public DateTime? FromShipmentDate { get; set; }

        /// <summary>
        /// 发料日期
        /// </summary>
        public DateTime? ToShipmentDate { get; set; }

        public bool OKSelect { get; set; }

        public int StrongHoldType { get; set; }

        public int MStockStatus { get; set; }

        public decimal? StockQty { get; set; }

        public decimal? OutStockQty { get; set; }

        public List<T_OutStockDetailInfo> lstDetail { get; set; }

        [Display(Name = "省份")]
        public string Province { get; set; }

        [Display(Name = "市")]
        public string City { get; set; }

        [Display(Name = "区")]
        public string Area { get; set; }

        [Display(Name = "收货地址")]
        public string Address { get; set; }

        [Display(Name = "物流地址")]
        public string Address1 { get; set; }

        [Display(Name = "联系人")]
        public string Contact { get; set; }

        [Display(Name = "电话")]
        public string Phone { get; set; }
        

        [Display(Name = "等通知发货")]
        public string ShipNFlg { get; set; }

        [Display(Name = "是否要发货清单")]
        public string ShipDFlg { get; set; }

        [Display(Name = "是否要价格")]
        public string ShipPFlg { get; set; }
        [Display(Name = "等外调/等单")]
        public string ShipWFlg { get; set; }

        [Display(Name = "交易条件")]
        public string TradingConditions { get; set; }

        [Display(Name = "交易条件")]
        public string TradingConditionsName { get; set; }

        public string ExpAmount { get; set; }

        /// <summary>
        /// 拣货车编码
        /// </summary>
        [Display(Name = "拣货车编码")]
        public string CarNo { get; set; }

        public string strReviewUserNo { get; set; }

        public string hmdocno { get; set; }

        public string fydocno { get; set; }
        [Display(Name = "调入仓")]
        public string ToErpWarehouse { get; set; }

        /// <summary>
        /// 查询出库任务总览
        /// PC=1 其他情况为PDA查询
        /// </summary>
        public int PcOrPda { get; set; }

        public string WmsStatus { get; set; }

        /// <summary>
        /// 快递金额
        /// </summary>

        public decimal GoodsValue { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal ItemsWeight { get; set; }

        public string gtype { get; set; }

        /// <summary>
        /// 快递费用
        /// </summary>
        public decimal Feight { get; set; }

        /// <summary>
        /// 快递数量
        /// </summary>
        public decimal BoxCount { get; set; }

        /// <summary>
        /// 三段码
        /// </summary>
        public string shortAddress { get; set; }

        /// <summary>
        /// 末端网点代码
        /// </summary>
        public string consigneeBranchCode { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>

        public string qrCode { get; set; }

        /// <summary>
        /// 寄件人
        /// </summary>
        public string sname { get; set; }

        /// <summary>
        /// 寄件邮编
        /// </summary>
        public string spostCode { get; set; }

        /// <summary>
        /// 寄件用户电话，包括区号、电话号码及分机号，中间用“-”分隔；
        /// </summary>
        public string sphone { get; set; }

        /// <summary>
        /// 寄件用户移动电话， 手机和电话至少填一项
        /// </summary>
        public string smobile { get; set; }

        /// <summary>
        /// 寄件省份
        /// </summary>
        public string sprov { get; set; }

        /// <summary>
        /// 寄件城市
        /// </summary>
        public string scity { get; set; }

        /// <summary>
        /// 寄件地址
        /// </summary>
        public string saddress { get; set; }

        [Display(Name = "仓库")]
        public string FromErpWarehouse { get; set; }

        [Display(Name = "调出仓库")]
        public string FromErpWarehouseName { get; set; }
        [Display(Name = "调入仓库")]
        public string ToErpWarehouseName { get; set; }

        public string TOTALNUM { get; set; }

        public string TOTALAMT { get; set; }

        public string TOTALNUM1 { get; set; }

        public string TOTALAMT1 { get; set; }

        public string DISPRICE { get; set; }

        public string DISPRICE1 { get; set; }

        public decimal Weight { get; set; }

        public decimal Volume { get; set; }

        public int CountBox { get; set; }
    }
}
