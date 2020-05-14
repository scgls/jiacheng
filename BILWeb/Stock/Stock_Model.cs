using BILWeb.OutBarCode;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Stock
{
    /// <summary>
    /// t_stock的实体类
    /// 作者:方颖
    /// 日期：2016/12/12 13:22:14
    /// </summary>

    public class T_StockInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_StockInfo() : base() { }


        //私有变量
        
        private string barcode;
        private string serialno;
        private string materialno;
        private string materialdesc;
        private string warehouseno;
        private string houseno;
        private string areano;
        private decimal qty;
        private string tmaterialno;
        private string tmaterialdesc;
        private string pickareano;
        private string celareano;        
        private int isdel;
        private string batchno;
        private string sn;
        private string returnsupcode;
        private string returnreson;
        private string returnsupname;
        private int oldstockid;
        private int taskdetailesid;
        private int checkid;
        private int transferdetailsid;
        private int returntype;
        private string returntypedesc;
        private string unit;
        private string salename;
        private string unitname;
        private string palletno;
        private int receivestatus;



        //公开属性

        [Display(Name = "条码")]
        public string Barcode
        {
            get
            {
                return barcode;
            }
            set
            {
                barcode = value;
            }
        }
        [Display(Name = "序列号")]
        public string SerialNo
        {
            get
            {
                return serialno;
            }
            set
            {
                serialno = value;
            }
        }
        [Display(Name = "物料号")]
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
        [Display(Name = "仓库")]
        public string WarehouseNo
        {
            get
            {
                return warehouseno;
            }
            set
            {
                warehouseno = value;
            }
        }
        [Display(Name = "库区")]
        public string HouseNo
        {
            get
            {
                return houseno;
            }
            set
            {
                houseno = value;
            }
        }
        [Display(Name = "库位")]
        public string AreaNo
        {
            get
            {
                return areano;
            }
            set
            {
                areano = value;
            }
        }
        [Display(Name = "数量")]
        public decimal Qty
        {
            get
            {
                return qty;
            }
            set
            {
                qty = value;
            }
        }

        public string TMaterialNo
        {
            get
            {
                return tmaterialno;
            }
            set
            {
                tmaterialno = value;
            }
        }

        public string TMaterialDesc
        {
            get
            {
                return tmaterialdesc;
            }
            set
            {
                tmaterialdesc = value;
            }
        }
        [Display(Name = "拣货库位")]
        public string PickAreaNo
        {
            get
            {
                return pickareano;
            }
            set
            {
                pickareano = value;
            }
        }

        public string CelareaNo
        {
            get
            {
                return celareano;
            }
            set
            {
                celareano = value;
            }
        }

        

        public int IsDel
        {
            get
            {
                return isdel;
            }
            set
            {
                isdel = value;
            }
        }


        [Display(Name = "批次")]
        public string BatchNo
        {
            get
            {
                return batchno;
            }
            set
            {
                batchno = value;
            }
        }

        public string SN
        {
            get
            {
                return sn;
            }
            set
            {
                sn = value;
            }
        }

        public string ReturnSupCode
        {
            get
            {
                return returnsupcode;
            }
            set
            {
                returnsupcode = value;
            }
        }

        public string ReturnReson
        {
            get
            {
                return returnreson;
            }
            set
            {
                returnreson = value;
            }
        }

        public string ReturnSupName
        {
            get
            {
                return returnsupname;
            }
            set
            {
                returnsupname = value;
            }
        }

        public int OldStockID
        {
            get
            {
                return oldstockid;
            }
            set
            {
                oldstockid = value;
            }
        }

        public int TaskDetailesID
        {
            get
            {
                return taskdetailesid;
            }
            set
            {
                taskdetailesid = value;
            }
        }

        public int CheckID
        {
            get
            {
                return checkid;
            }
            set
            {
                checkid = value;
            }
        }

        public int TransferDetailsID
        {
            get
            {
                return transferdetailsid;
            }
            set
            {
                transferdetailsid = value;
            }
        }

        public int ReturnType
        {
            get
            {
                return returntype;
            }
            set
            {
                returntype = value;
            }
        }

        public string ReturnTypeDesc
        {
            get
            {
                return returntypedesc;
            }
            set
            {
                returntypedesc = value;
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

        public string SaleName
        {
            get
            {
                return salename;
            }
            set
            {
                salename = value;
            }
        }
        [Display(Name = "单位")]
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
        [Display(Name = "托盘号")]
        public string PalletNo
        {
            get
            {
                return palletno;
            }
            set
            {
                palletno = value;
            }
        }

        public int ReceiveStatus
        {
            get
            {
                return receivestatus;
            }
            set
            {
                receivestatus = value;
            }
        }



        public string FromAreaNo { get; set; }

        public string FromHouseNo { get; set; }

        public string FromWareHouseNo { get; set; }

        public decimal PalletQty { get; set; }

        public int WareHouseID { get; set; }

        public int HouseID { get; set; }

        public int AreaID { get; set; }

        public int FromAreaID { get; set; }

        public int FromHouseID { get; set; }

        public int FromWareHouseID { get; set; }

        public string PartNo { get; set; }

        public int IsLimitStock { get; set; }

        public int IsQuality { get; set; }
        [Display(Name = "供应商号")]
        public string SupCode { get; set; }
        [Display(Name = "供应商名")]
        public string SupName { get; set; }

        public DateTime ProductDate{ get; set; }

        public string SupPrdBatch { get; set; }

        public DateTime SupPrdDate { get; set; }
        /// <summary>
        /// 下架方式 1-整托 2-整箱 3-零数
        /// </summary>
        public int PickModel { get; set; }



        /// <summary>
        /// 零数
        /// </summary>
        public decimal AmountQty { get; set; }

        /// <summary>
        /// 楼层编码
        /// </summary>
        public int FloorType { get; set; }

        /// <summary>
        /// 高低库位
        /// </summary>
        public int HeightArea { get; set; }

        /// <summary>
        /// 库位类型
        /// </summary>
        public int AreaType { get; set; }
        

        public decimal ScanQty { get; set; }

        public string FromBatchNo { get; set; }

        public string FromErpAreaNo { get; set; }

        public string FromErpWarehouse { get; set; }

        public string ToBatchNo { get; set; }

        public string ToErpAreaNo { get; set; }

        public string ToErpWarehouse { get; set; }

        public string PostUser { get; set; }

        public int OutstockDetailID { get; set; }


        public bool OKSelect { get; set; }

        public int MaterialChangeID { get; set; }


        public string UnitTypeCode { get; set; }

        public string DecimalLngth { get; set; }

        public string IsSpcBatch { get; set; }

        public string SortArea { get; set; }

        public string IsRetention { get; set; }
       

        //库区属性 1-整箱区 2-零散区
        public int HouseProp { get;set; }
        public string EAN { get; set; }

        /// <summary>
        /// 扫描类型 
        /// 1-整托发货 2-整箱发货 3-零数发货
        /// </summary>
        public int ScanType { get; set; }

        /// <summary>
        /// 1-69码扫描 2-69码保存
        /// 序列号直接保存，不看这个字段值
        /// </summary>
        public int SaveType { get; set; }

        /// <summary>
        /// 是否带J箱,1-是 2-否
        /// </summary>
        public string ISJ { get; set; }

        /// <summary>
        /// 条码类型
        /// </summary>
        public int BarCodeType { get; set; }

        /// <summary>
        /// J箱条码
        /// </summary>
        public string JBarCode { get; set; }

        /// <summary>
        /// 存放混箱条码(不用，可以删除)
        /// </summary>
        public List<T_OutBarCodeInfo> lstBarCode { get; set; }

        /// <summary>
        /// 存放混箱条码
        /// </summary>
        public List<T_StockInfo> lstHBarCode { get; set; }

        /// <summary>
        /// 存放拣货拆零J箱条码
        /// </summary>
        public List<T_StockInfo> lstJBarCode { get; set; }

        /// <summary>
        /// 拆零标记 1-否 2-是
        /// </summary>
        public int IsAmount { get; set; }

        public string fserialno { get; set; }

        /// <summary>
        /// 区分扫描到的是整箱还是整托
        /// 1-整箱 2-整托
        /// </summary>
        public int IsPalletOrBox { get; set; }

        public string WarehouseName { get; set; }

        public string ProjectNo { get; set; }
        [Display(Name = "跟踪号")]
        public string TracNo { get; set; }

        public string Spec { get; set; }
        
    }
}
