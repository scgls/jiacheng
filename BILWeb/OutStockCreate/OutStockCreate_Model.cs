using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.OutStockCreate
{
    public class T_OutStockCreateInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_OutStockCreateInfo() : base() { }


        //私有变量                
        private string materialno;
        private string materialdesc;
        private string rowno;
        private string plant;
        private string plantname;
        private string tostorageloc;
        private string unit;
        private string unitname;
        private decimal? outstockqty;
        private decimal? oldoutstockqty;
        private decimal? remainqty;
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
        [Display(Name ="物料号")]
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
        [Display(Name = "物料名")]
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
        [Display(Name = "行号")]
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
        [Display(Name = "单位")]
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
        [Display(Name = "单位")]
        public string Unitname
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
        [Display(Name = "出库数")]
        public decimal? OutStockQty
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
        [Display(Name = "已出库数")]
        public decimal? OldOutStockQty
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
        [Display(Name = "剩余数")]
        public decimal? RemainQty
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
        [Display(Name = "客户")]
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
        [Display(Name = "任务号")]
        public string VoucherNo { get; set; }

       

        public decimal CurrentOutStockQty { get; set; }

        [Display(Name = "客户编号")]
        public string CustomerCode { get; set; }

        [Display(Name = "客户名")]
        public string CustomerName { get; set; }

        public string PartNo { get; set; }

        public string ErpDocNo { get; set; }

        public decimal StockQty { get; set; }

        /// <summary>
        /// 物料库存状态，1-库存为零 2-库存不足 3-库存满足
        /// </summary>
        public int MStockStatus { get; set; }

        public int StrongHoldType { get; set; }
         [Display(Name = "发料日期")]
        /// <summary>
        /// 发料日期
        /// </summary>
        public DateTime? FromShipmentDate { get; set; }
        [Display(Name = "发料日期")]
        /// <summary>
        /// 发料日期
        /// </summary>
        public DateTime? ToShipmentDate { get; set; }

        /// <summary>
        /// 主分群编码
        /// </summary>
        public string MainTypeCode { get; set; }

        /// <summary>
        /// 主分群名称
        /// </summary>
        public string MainTypeName { get; set; }
        [Display(Name = "是否指定批次")]
        /// <summary>
        /// 是否指定批次
        /// </summary>
        public string IsSpcBatch { get; set; }
        [Display(Name = "是否指定批次")]
        public string StrIsSpcBatch { get; set; }
        [Display(Name = "发货批次")]
        /// <summary>
        /// ERP指定的发货批次
        /// </summary>
        public string FromBatchno { get; set; }
        [Display(Name = "发货储位")]
        /// <summary>
        /// ERP指定发货储位
        /// </summary>
        public string FromErpAreaNo { get; set;  }
        [Display(Name = "发货仓库")]
        /// <summary>
        /// ERP指定发货仓库
        /// </summary>
        public string FromErpWareHouse { get; set; }
        

        /// <summary>
        /// 给ERP指定的发货批次
        /// </summary>
        public string ToBatchno { get; set; }


        [Display(Name = "到达货位")]
        /// <summary>
        /// 给ERP指定发货储位
        /// </summary>
        public string ToErpAreaNo { get; set; }
        [Display(Name = "到达仓库")]
        /// <summary>
        /// 给ERP指定发货仓库
        /// </summary>
        public string ToErpWareHouse { get; set; }


        public int FloorType { get; set; }

        public string BatchNo { get; set; }

        public string RowNoDel { get; set; }

        public string PickLeaderUserNo { get; set; }

        public string PickGroupNo { get; set; }

        public int HeightArea { get; set; }

        public string SelectHeaderID { get; set; }

        public string ProductNo { get; set; }

        public int TaskType { get; set; }

        //存放outstock表头ID
        public int InStockID { get; set; }

        //客户编码
        public string SupCusCode { get; set; }

        //客户名称
        public string SupCusName { get; set; }

        public int IsDel { get; set; }

        public int IssueType { get; set; }

        public List<T_OutStockCreateInfo> lstOutStockCreateInfo { get; set; }

        public int OutstockDetailid { get; set; }

        /// <summary>
        /// 库区属性 1-整箱区 2-零拣区
        /// </summary>
        public int HouseProp { get; set; }

        /// <summary>
        /// 物料箱包装量
        /// </summary>
        public decimal PackQty { get; set; }

        /// <summary>
        /// 箱子个数(整箱)
        /// </summary>
        public decimal BoxQty { get; set; }

        /// <summary>
        /// 不足一箱包装量，零数
        /// </summary>
        public decimal ScatQty { get; set; }

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

        [Display(Name = "省份")]
        public string Province { get; set; }

        [Display(Name = "市")]
        public string City { get; set; }

        [Display(Name = "区")]
        public string Area { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name="物流地址")]
        public string Address1 { get; set; }

        [Display(Name = "联系人")]
        public string Contact { get; set; }

        [Display(Name = "电话")]
        public string Phone { get; set; }
        [Display(Name = "交易条件")]
        public string TradingConditionsName { get; set; }
    }
}
