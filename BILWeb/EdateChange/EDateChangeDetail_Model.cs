using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.EdateChange
{
    /// <summary>
    /// t_edatechangedetail的实体类
    /// 作者:方颖
    /// 日期：2017/9/6 14:50:47
    /// </summary>

    public class T_EDateChangeDetailInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_EDateChangeDetailInfo() : base() { }


        //私有变量
        private string rowno;
        private string materialno;
        private string batchno;
        private DateTime? aftedate;
        private DateTime befedate;
        private int resonecode;
        private string resonename;
        private string note;        
        private decimal? isdel;



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

        public string MaterialDesc { get; set; }
        

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

        public DateTime? AftEDate
        {
            get
            {
                return aftedate;
            }
            set
            {
                aftedate = value;
            }
        }

        public DateTime BefEDate
        {
            get
            {
                return befedate;
            }
            set
            {
                befedate = value;
            }
        }

        public int ResoneCode
        {
            get
            {
                return resonecode;
            }
            set
            {
                resonecode = value;
            }
        }

        /// <summary>
        /// 传给ERP过账
        /// </summary>
        public string StrResoneCode { get; set; }
        

        public string ResoneName
        {
            get
            {
                return resonename;
            }
            set
            {
                resonename = value;
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

        public string VoucherNo { get; set; }

        public string PostUser { get; set; }

        public string StrAftEDate { get; set; }

        public Decimal StockQty { get; set; }

    }
}

