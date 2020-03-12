using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILWeb.Stock;

namespace BILWeb.Quality
{
    /// <summary>
    /// t_qualitydetail的实体类
    /// 作者:方颖
    /// 日期：2017/6/27 11:33:12
    /// </summary>

    public class T_QualityDetailInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_QualityDetailInfo() : base() { }


        //私有变量
        
           
        private string areano;        
        private int isdel;

        //公开属性
          
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
                

        public int NoticeStatus
        {
            get;
            set;
        }

        public int QualityType
        {
            get;
            set;
        }

        public string MaterialNo
        {
            get;
            set;
        }

        public string MaterialDesc
        {
            get;
            set;
        }

        public decimal? InSQty
        {
            get;
            set;
        }

        public string Unit
        {
            get;
            set;
        }

        public string UnitName
        {
            get;
            set;
        }     

        public string WarehouseNo
        {
            get;
            set;
        }

        public string BatchNo
        {
            get;
            set;
        }

        /// <summary>
        /// ERP收货入库单
        /// </summary>
        public string ErpInVoucherNo { get; set; }

        /// <summary>
        /// 取样人编号
        /// </summary>
        public string QuanUserNo { get; set; }

        public decimal SampQty { get; set; }

        public decimal ScanQty { get; set; }

        public decimal? RemainQty { get; set; }

        public decimal? QuanQty
        {
            get;
            set;
        }

        public decimal? UnQuanQty
        {
            get;
            set;
        }

        public decimal? DesQty
        {
            get;
            set;
        }

        public List<T_StockInfo> lstStock { get; set; }

        public string FromBatchNo { get; set; }

        public string FromErpAreaNo { get; set; }

        public string FromErpWarehouse { get; set; }

        public string ToBatchNo { get; set; }

        public string ToErpAreaNo { get; set; }

        public string ToErpWarehouse { get; set; }

        public string PostUser { get; set; }

        public int SampWareHouseID { get; set; }

        public int SampHouseID { get; set; }

        public int SampAreaID { get; set; }

        public int AreaType { get; set; }

    }
}

