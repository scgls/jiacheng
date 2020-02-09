using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILWeb.InStock;
using BILWeb.Stock;
using System.ComponentModel.DataAnnotations;

namespace BILWeb.OutStock
{
    public class T_OutStockDetailInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_OutStockDetailInfo() : base() { }


        //私有变量        
        private decimal outstockid;
        
        private string materialno;
        private string materialdesc;
        private string rowno;
        private string plant;
        private string plantname;
        private string tostorageloc;
        private string unit;
        private string unitname;
        private decimal outstockqty;
        private decimal oldoutstockqty;
        private decimal remainqty;
        private string costcenter;
        private string wbselem;
        private string fromstorageloc;
        private decimal? reviewstatus;        
        private string closeoweuser;
        private DateTime? closeowedate;
        private string closeoweremark;
        private decimal? isoweclose;
        private string oweremark;
        private string oweremarkuser;
        private DateTime? oweremarkdate;
     



        //公开属性
       

        public decimal OutStockID
        {
            get
            {
                return outstockid;
            }
            set
            {
                outstockid = value;
            }
        }

       

        public string MaterialNo
        {
            get
            {
                return materialno;
            }
            set
            {
                materialno = value;
            }
        }

        public string MaterialDesc
        {
            get
            {
                return materialdesc;
            }
            set
            {
                materialdesc = value;
            }
        }

        public string RowNo
        {
            get
            {
                return rowno;
            }
            set
            {
                rowno = value;
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

        public string ToStorageLoc
        {
            get
            {
                return tostorageloc;
            }
            set
            {
                tostorageloc = value;
            }
        }

        public string Unit
        {
            get
            {
                return unit;
            }
            set
            {
                unit = value;
            }
        }

        public string UnitName
        {
            get
            {
                return unitname;
            }
            set
            {
                unitname = value;
            }
        }

        public decimal OutStockQty
        {
            get
            {
                return outstockqty;
            }
            set
            {
                outstockqty = value;
            }
        }

        public decimal OldOutStockQty
        {
            get
            {
                return oldoutstockqty;
            }
            set
            {
                oldoutstockqty = value;
            }
        }

        public decimal RemainQty
        {
            get
            {
                return remainqty;
            }
            set
            {
                remainqty = value;
            }
        }

        public string Costcenter
        {
            get
            {
                return costcenter;
            }
            set
            {
                costcenter = value;
            }
        }

        public string Wbselem
        {
            get
            {
                return wbselem;
            }
            set
            {
                wbselem = value;
            }
        }

        public string FromStorageLoc
        {
            get
            {
                return fromstorageloc;
            }
            set
            {
                fromstorageloc = value;
            }
        }

        public decimal? ReviewStatus
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

       

        public string CloseOweUser
        {
            get
            {
                return closeoweuser;
            }
            set
            {
                closeoweuser = value;
            }
        }

        public DateTime? CloseOweDate
        {
            get
            {
                return closeowedate;
            }
            set
            {
                closeowedate = value;
            }
        }

        public string CloseOweRemark
        {
            get
            {
                return closeoweremark;
            }
            set
            {
                closeoweremark = value;
            }
        }

        public decimal? IsOweClose
        {
            get
            {
                return isoweclose;
            }
            set
            {
                isoweclose = value;
            }
        }

        public string OweRemark
        {
            get
            {
                return oweremark;
            }
            set
            {
                oweremark = value;
            }
        }

        public string OweRemarkUser
        {
            get
            {
                return oweremarkuser;
            }
            set
            {
                oweremarkuser = value;
            }
        }

        public DateTime? OweRemarkDate
        {
            get
            {
                return oweremarkdate;
            }
            set
            {
                oweremarkdate = value;
            }
        }

        public bool OKSelect { get; set; }

        public string VoucherNo { get; set; }

        public List<T_SerialNoInfo> lstSerialNo;

        public string BatchNo { get; set; }

        public int IsSerial { get; set; }

        public string PartNo { get; set; }

        public string RowNoDel { get; set; }

        public decimal StockQty { get; set; }

        public List<T_StockInfo> lstStock { get; set; }

        public decimal ScanQty { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string PalletNo { get; set; }

        public string FromBatchNo { get; set; }

        public string FromErpAreaNo { get; set; }

        public string FromErpWarehouse { get; set; }

        public string FromErpWareHouseName { get; set; }

        public string ToBatchno { get; set; }

        public string ToErpAreaNo { get; set; }

        public string ToErpWarehouse { get; set; }

        public string ToErpWarehouseName { get; set; }

        public string PostUser { get; set; }

        public string SourceVoucherNo { get; set; }

        public string SourceRowNo { get; set; }

        public string IsSpcBatch { get; set; }

        public string StrIsSpcBatch { get; set; }

        public string SouStrongHoldCode { get; set; }

        /// <summary>
        /// 商品金额
        /// </summary>
        public decimal GoodsValue { get; set; }

        /// <summary>
        /// 商品重量
        /// </summary>
        public decimal ItemsWeight { get; set; }

        [Display(Name = "省份")]
        public string Province { get; set; }

        [Display(Name = "市")]
        public string City { get; set; }

        [Display(Name = "区")]
        public string Area { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "物流地址")]
        public string Address1 { get; set; }

        [Display(Name = "联系人")]
        public string Contact { get; set; }

        [Display(Name = "电话")]
        public string Phone { get; set; }

        [Display(Name = "单价")]
        public Decimal Price { get; set; }

        [Display(Name = "总金额")]
        public Decimal Amount { get; set; }

        [Display(Name = "复核数量")]
        public decimal ReviewQty { get; set; }

        [Display(Name = "拣货数量")]
        public decimal PickQty { get;set; }

        /// <summary>
        /// 过账时间
        /// </summary>
        public string StrPostDate { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string ExpNo { get; set; }

        /// <summary>
        /// 物流费用
        /// </summary>
        public string ExpAmount { get; set; }

        [Display(Name = "等通知发货")]
        public string ShipNFlg { get; set; }

        [Display(Name = "是否需要发货清单标记")]
        public string ShipDFlg { get; set; }

        [Display(Name = "打印发货清单是否要价格")]
        public string ShipPFlg { get; set; }
        [Display(Name = "是否等外调标记")]
        public string ShipWFlg { get; set; }

        [Display(Name = "交易条件")]
        public string TradingConditions { get; set; }

        public string EAN { get; set; }

        /// <summary>
        /// 订单过账可分配数量
        /// </summary>
        public decimal CanOutStockQty { get; set; }

        [Display(Name = "任务数量")]
        public decimal TaskQty { get; set; }

        [Display(Name = "拣货数量")]
        public decimal UnShelveQty { get; set; }

        [Display(Name = "复核人")]
        public string ReviewUser { get; set; }

        [Display(Name = "复核时间")]
        public DateTime? ReviewDate { get; set; }

        [Display(Name = "未复核数量")]
        public decimal UnReviewQty { get; set; }
        //库区属性 1-整箱区 2-零散区
        public int HouseProp { get; set; }

        //库区属性 1-整箱区 2-零散区
        [Display(Name = "任务属性")]
        public string StrHouseProp { get; set; }

        public string hmdocno { get; set; }

        public string fydocno { get; set; }

        /// <summary>
        /// 表头据点
        /// </summary>
        public string StrongholdcodeHeader { get; set; }

        /// <summary>
        /// 单身据点
        /// </summary>
        public string StrongholdcodeDetail { get; set; }
        /// <summary>
        /// 拣货车编码
        /// </summary>
        [Display(Name = "拣货车编码")]
        public string CarNo { get; set; }

        public bool isLight { get; set; }

    }
}
