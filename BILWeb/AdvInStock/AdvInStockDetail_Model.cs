using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.AdvInStock
{
    /// <summary>
    /// t_advinstockdetail的实体类
    /// 作者:方颖
    /// 日期：2019/7/17 15:58:40
    /// </summary>

    public class T_AdvInStockDetailInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_AdvInStockDetailInfo() : base() { }


        //私有变量
        private string materialno;
        private string materialdesc;
        private decimal? advqty;
        private string unit;
        private int linestatus;        
        private int isdel;
        private string ean;
   
        private string supbatch;
        private string voucherno;


        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        

        /// <summary>
        /// 项次
        /// </summary>
        public string RowNO{get;set;}
        /// <summary>
        /// 项序
        /// </summary>
        public string RowNODel { get; set; }

        [Display(Name = "单号")]
        public string VOUCHERNO
        {
            get
            {
                return voucherno;
            }
            set
            {
                voucherno = value;
            }
        }


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
        [Display(Name = "收货数量")]
        public decimal? AdvQty
        {
            get
            {
                return advqty;
            }
            set
            {
                advqty = value;
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
        [Display(Name = "EAN")]
        public string EAN
        {
            get
            {
                return ean;
            }
            set
            {
                ean = value;
            }
        }


        [Display(Name = "供应商批次")]
        public string SupBatch
        {
            get
            {
                return supbatch;
            }
            set
            {
                supbatch = value;
            }
        }
        
        [Display(Name = "质检类型")]
        public int QualityType { get; set; }

        [Display(Name = "质检类型")]
        public string strqualitytype { get; set; }

        [Display(Name = "操作人")]
        public string Createname { get; set; }

        [Display(Name = "是否打印")]
        public string IsPrint { get; set; }

        [Display(Name = "仓库")]
        public string WarehouseName { get; set; }
        
    }
}

