using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILWeb.Pallet;
using BILWeb.OutBarCode;
using BILWeb.Stock;
using System.ComponentModel.DataAnnotations;

namespace BILWeb.InStock
{
    public class T_InStockDetailInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_InStockDetailInfo() : base() { }


        //私有变量       
        private int instockid;
        private string rowno;
        private string materialno;
        private string materialdesc;
        private decimal instockqty;
        private decimal receiveqty;
        private string unit;
        private string storageloc;
        private string plant;
        private string plantname;
        private decimal qualityqty;
        private decimal unqualityqty;
        private string qualitytype;
        private string qualityuserno;
        private DateTime? qualitydate;
        private string unitname;





        //公开属性
       

        public int InStockID
        {
            get
            {
                return instockid;
            }
            set
            {
                instockid = value;
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
        [Display(Name = "物料编号")]
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
        [Display(Name = "物料描述")]
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
        [Display(Name = "入库数量")]
        /// <summary>
        /// 入库数量，ERP单据同步过来的订单数量
        /// </summary>
        public decimal InStockQty
        {
            get
            {
                return instockqty;
            }
            set
            {
                instockqty = value;
            }
        }
        [Display(Name = "收货数量")]
        /// <summary>
        ///实际收货数量，多次收货数量累加
        /// </summary>
        public decimal ReceiveQty
        {
            get
            {
                return receiveqty;
            }
            set
            {
                receiveqty = value;
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

        public string StorageLoc
        {
            get
            {
                return storageloc;
            }
            set
            {
                storageloc = value;
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

        public decimal QualityQty
        {
            get
            {
                return qualityqty;
            }
            set
            {
                qualityqty = value;
            }
        }

        public decimal UnQualityQty
        {
            get
            {
                return unqualityqty;
            }
            set
            {
                unqualityqty = value;
            }
        }

        public string QualityType
        {
            get
            {
                return qualitytype;
            }
            set
            {
                qualitytype = value;
            }
        }

        public string QualityUserNo
        {
            get
            {
                return qualityuserno;
            }
            set
            {
                qualityuserno = value;
            }
        }

        public DateTime? QualityDate
        {
            get
            {
                return qualitydate;
            }
            set
            {
                qualitydate = value;
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
        [Display(Name = "剩余收货数量")]
        /// <summary>
        /// 剩余收货数量
        /// </summary>
        public decimal RemainQty { get; set; }

        public decimal ScanQty { get; set; }

        public List<T_SerialNoInfo> lstSerialNo { get; set; }

        public string SaleName { get; set; }

        public string VoucherNo { get; set; }

        public DateTime? ArrivalDate { get; set; }

        public List<T_PalletInfo> lstPallet { get; set; }        

        public string SaleCode { get; set; }

        public string SupplierNo { get; set; }

        public string SupplierName { get; set; }

        public string BatchNo { get; set; }

       

        /// <summary>
        /// 1-批次 2-序列号
        /// </summary>
        public int IsSerial { get; set; }

        public string PartNo { get; set; }

        public List<T_OutBarCodeInfo> lstBarCode { get; set; }

        public string DeliverySup { get; set; }

        public DateTime ShipmentDate { get; set; }

        public DateTime ArrStockDate { get; set; }

        public string Stationname { get; set; }

        //已经打印数量
        public decimal Hasprint { get; set; }


        public decimal OutPackNum { get; set; }

        public decimal CenterPackNum { get; set; }
        public decimal InnerPackNum { get; set; }

        //存储条件
        public string StoreCondition { get; set; }

        //特殊要求
        public string SpecialRequire { get; set; }

        public string ProtectWay { get; set; }

        //采购子分类名称
        public string ERPVoucherDesc { get; set; }

        //采购子分类
        public string ERPVoucherType { get; set; }

        public int lasttime { get; set; }

        public string RowNoDel { get; set; }

        public string MainTypeCode { get; set; }

        public DateTime SupPrdDate { get; set; }

        public string StrSupPrdDate { get; set; }

        public string SupPrdBatch { get; set; }

        /// <summary>
        /// 收货仓库
        /// </summary>
        public string ReceiveWareHouseNo { get; set; }

        /// <summary>
        /// 收货库位
        /// </summary>
        public string ReceiveAreaNo { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string ReceiveUserNo { get; set; }        

        public DateTime ProductDate { get; set; }

        public string ProductBatch { get; set; }

        public string ReasonCode { get; set; }

        public int IsQuality { get; set; }

        public string IsSpcBatch { get; set; }

        public string FromBatchNo { get; set; }

        public string FromErpAreaNo { get; set; }

        public string FromErpWarehouse { get; set; }

        public string ToBatchNo { get; set; }

        public string ToErpAreaNo { get; set; }

        public string ToErpWarehouse { get; set; }

        public string QcCode { get; set; }
        public string QcDesc { get; set; }

        public string PostUser { get; set; }

        public string productno { get; set; }

        public string ProRowNo{ get; set; }

        public string ProRowNoDel { get; set; }
        /// <summary>
        /// 预收货数量
        /// </summary>
        public decimal ADVRECEIVEQTY { get; set; }

        public bool ischeck { get; set; }

        public string InvoiceNo { get; set; }
        
    }
}
