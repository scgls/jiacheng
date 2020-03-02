using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Material
{
    /// <summary>
    /// t_material的实体类
    /// 作者:方颖
    /// 日期：2016/11/22 17:16:36
    /// </summary>

    public class T_MaterialInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_MaterialInfo() : base() { }


        //私有变量
       
        private string materialno;
        private string materialdesc;
        private string materialdescen;
        private int? stackwarehouse;
        private int? stackhouse;
        private int? stackarea;
        private decimal? length;
        private decimal? wide;
        private decimal? hight;
        private decimal? volume;
        private decimal? weight;
        private decimal? netweight;
        private decimal? packrule;
        private decimal? stackrule;
        private decimal? disrule;
        private string supplierno;
        private string suppliername;
        private string unit;
        private string unitname;
        private string keeperno;
        private string keepername;
        private decimal? isdangerous;
        private decimal? isactivate;
        private decimal? isbond;
        private decimal? isquality;

        /// <summary>
        /// 包装ENA
        /// </summary>
        public string WaterCode { get; set; }

        //公开属性
        [Display(Name ="物料编号")]
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
        public string ErpBarCode { get; set; }
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
        [Display(Name = "物料英文描述")]
        public string MaterialDescEN
        {
            get
            {
                return materialdescen;
            }
            set
            {
                materialdescen = value;
            }
        }

        public int? StackWareHouse
        {
            get
            {
                return stackwarehouse;
            }
            set
            {
                stackwarehouse = value;
            }
        }

        public int? StackHouse
        {
            get
            {
                return stackhouse;
            }
            set
            {
                stackhouse = value;
            }
        }

        public int? StackArea
        {
            get
            {
                return stackarea;
            }
            set
            {
                stackarea = value;
            }
        }
        [Display(Name = "长")]
        public decimal? Length
        {
            get
            {
                return length;
            }
            set
            {
                length = value;
            }
        }
        [Display(Name = "宽")]
        public decimal? Wide
        {
            get
            {
                return wide;
            }
            set
            {
                wide = value;
            }
        }
        [Display(Name = "高")]
        public decimal? Hight
        {
            get
            {
                return hight;
            }
            set
            {
                hight = value;
            }
        }
        [Display(Name = "体积")]
        public decimal? Volume
        {
            get
            {
                return volume;
            }
            set
            {
                volume = value;
            }
        }
        [Display(Name = "重量")]
        public decimal? Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }
        [Display(Name = "净重")]
        public decimal? NetWeight
        {
            get
            {
                return netweight;
            }
            set
            {
                netweight = value;
            }
        }
        [Display(Name = "拣货规则")]
        public decimal? PackRule
        {
            get
            {
                return packrule;
            }
            set
            {
                packrule = value;
            }
        }

        public decimal? StackRule
        {
            get
            {
                return stackrule;
            }
            set
            {
                stackrule = value;
            }
        }
        [Display(Name = "分配规则")]
        public decimal? DisRule
        {
            get
            {
                return disrule;
            }
            set
            {
                disrule = value;
            }
        }
        [Display(Name = "供应商编号")]
        public string SupplierNo
        {
            get
            {
                return supplierno;
            }
            set
            {
                supplierno = value;
            }
        }
        [Display(Name = "供应商")]
        public string SupplierName
        {
            get
            {
                return suppliername;
            }
            set
            {
                suppliername = value;
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
        [Display(Name = "单位名称")]
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
        [Display(Name = "保管员编号")]
        public string KeeperNo
        {
            get
            {
                return keeperno;
            }
            set
            {
                keeperno = value;
            }
        }
        [Display(Name = "保管员")]
        public string KeeperName
        {
            get
            {
                return keepername;
            }
            set
            {
                keepername = value;
            }
        }

        public decimal? IsDangerous
        {
            get
            {
                return isdangerous;
            }
            set
            {
                isdangerous = value;
            }
        }

        public decimal? IsActivate
        {
            get
            {
                return isactivate;
            }
            set
            {
                isactivate = value;
            }
        }

        public decimal? IsBond
        {
            get
            {
                return isbond;
            }
            set
            {
                isbond = value;
            }
        }

        public decimal? IsQuality
        {
            get
            {
                return isquality;
            }
            set
            {
                isquality = value;
            }
        }

        public int IsSerial { get; set; }

        public string PartNo { get; set; }

        public string MainTypeCode { get; set; }

        public string MainTypeName { get; set; }

        public string PurchaseTypeCode { get; set; }

        public string PurchaseTypeName { get; set; }

        public string ProductTypeCode { get; set; }

        public string ProductTypeName { get; set; }

        public decimal QualityDay { get; set; }

        public decimal QualityMon { get; set; }

        public string Brand { get; set; }

        public string PlaceArea { get; set; }

        public string LifeCycle { get; set; }

        public decimal PackQty { get; set; }

        public decimal PalletVolume { get; set; }

        public decimal PalletPackQty { get; set; }

        public decimal PackVolume { get; set; }


        public string ProtectWay { get; set; }
        public string SpecialRequire { get; set; }
        public string StoreCondition { get; set; }

        /// <summary>
        /// 2-只查询物料表 1-查询库存表和物料表
        /// </summary>
        public int QueryStock { get; set; }

        public string BatchNo { get; set; }

        public string WareHouseNo { get; set; }

        public string AreaNo { get; set; }

        public int WareHouseID { get; set; }

        public int AreaID { get; set; }

        public decimal StockQty { get; set; }

        //add by cym 2018-7-16
        public string BrandIntRO { get; set; }
        public int standardbox1 { get; set; }
        public int standardbox2 { get; set; }
        public int standardbox3 { get; set; }
    }
}
