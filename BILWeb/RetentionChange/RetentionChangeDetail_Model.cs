using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.RetentionChange
{
    /// <summary>
    /// t_retentiondetailchange的实体类
    /// 作者:方颖
    /// 日期：2017/10/19 15:36:28
    /// </summary>

    public class T_RetentionDetailChangeInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_RetentionDetailChangeInfo() : base() { }


        //私有变量
        
        private string rowno;
        private string materialno;        
        private string materialdesc;
        private string batchno;
        private string qresonecode;
        private string qresonename;
        private string note;        
        private decimal? isdel;       
        private string warehouseno;
        private string areano;
        private decimal? warehouseid;
        private decimal? areaid;



        //公开属性
       

        public string Rowno
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

        public string QresoneCode
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

        public string QresoneName
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

        public decimal? WareHouseID
        {
            get
            {
                return warehouseid;
            }
            set
            {
                warehouseid = value;
            }
        }

        public decimal? AreaID
        {
            get
            {
                return areaid;
            }
            set
            {
                areaid = value;
            }
        }

        public string VoucherNo { get; set; }

        public string RetainType { get; set; }

        public string PostUser { get; set; }
    }
}

