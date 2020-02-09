using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.QualityChange
{
    /// <summary>
    /// t_qualitychangedetail的实体类
    /// 作者:方颖
    /// 日期：2017/9/20 16:08:47
    /// </summary>

    public class T_QualityChangeDetailInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_QualityChangeDetailInfo() : base() { }


        //私有变量        
        private string rowno;
        private string materialno;        
        private string materialdesc;
        private string batchno;
        private int qresonecode;
        private string qresonename;
        private string note;        
        private decimal? isdel;
        private string voucherno;       
        private string warehouseno;
        private string areano;
        private decimal? stockqty;

        //公开属性
        

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

        public int QResoneCode
        {
            get
            {
                return qresonecode;
            }
            set
            {
                qresonecode = value;
            }
        }

        public string QResoneName
        {
            get
            {
                return qresonename;
            }
            set
            {
                qresonename = value;
            }
        }

        public string Note
        {
            get
            {
                return note;
            }
            set
            {
                note = value;
            }
        }       

       

       
        public decimal? IsDel
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

        public string VoucherNo
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

        

        public string WareHouseNo
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

        public decimal? StockQty
        {
            get
            {
                return stockqty;
            }
            set
            {
                stockqty = value;
            }
        }

        public string PostUser { get; set; }

        public int WareHouseID { get; set; }

        public int AreaID { get; set; }
    }
}

