using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.Quality
{
    /// <summary>
    /// t_quality的实体类
    /// 作者:方颖
    /// 日期：2017/6/27 10:41:07
    /// </summary>

    public class T_QualityInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_QualityInfo() : base() { }


        //私有变量
            
        private int isdel;        
        private int noticestatus;
        private int qualitytype;
        private string materialno;
        private string materialdesc;
        private decimal? insqty;
        private string unit;
        private string unitname;
        private decimal? quanqty;
        private decimal? unquanqty;
        private decimal? desqty;
        private string warehouseno;
        private string batchno;



        //公开属性

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
            get
            {
                return noticestatus;
            }
            set
            {
                noticestatus = value;
            }
        }

        public int QualityType
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

        public decimal? InSQty
        {
            get
            {
                return insqty;
            }
            set
            {
                insqty = value;
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

        public decimal? QuanQty
        {
            get
            {
                return quanqty;
            }
            set
            {
                quanqty = value;
            }
        }

        public decimal? UnQuanQty
        {
            get
            {
                return unquanqty;
            }
            set
            {
                unquanqty = value;
            }
        }

        public decimal? DesQty
        {
            get
            {
                return desqty;
            }
            set
            {
                desqty = value;
            }
        }

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

        /// <summary>
        /// ERP收货入库单
        /// </summary>
        public string ErpInVoucherNo { get; set; }

        /// <summary>
        /// 取样人编号
        /// </summary>
        public string QuanUserNo { get; set; }

        /// <summary>
        /// 取样人编号，用于客户端选择检验任务更新的取样人
        /// </summary>
        public string StrQuanUserNo { get; set; }

        public decimal? SampQty { get; set; }
       

    }
}

