using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.AdvInStock
{
    /// <summary>
    /// t_advinstock的实体类
    /// 作者:方颖
    /// 日期：2019/7/17 15:47:19
    /// </summary>

    public class T_AdvInStockInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_AdvInStockInfo() : base() { }


        //私有变量        
        private string supplierno;
        private string suppliername;
        private decimal? vouchertype;
        private decimal? isdel;
        private int warehouseid;


        public string VoucherNo { get; set; }


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



        public int WarehouseID
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
        public List<T_AdvInStockDetailInfo> lstDetail { get; set; }
    }
}

