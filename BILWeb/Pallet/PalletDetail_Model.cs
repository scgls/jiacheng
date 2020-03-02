using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILWeb.InStock;
using BILWeb.OutBarCode;
using BILWeb.Stock;


namespace BILWeb.Pallet
{
    public class T_PalletDetailInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_PalletDetailInfo() : base() { }


        //私有变量
        
        
        private string palletno;
        private string materialno;
        private string materialdesc;
        private string serialno;



        //公开属性
        
       
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

        public string RowNo { get; set; }

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

        public List<T_SerialNoInfo> lstSerialNo { get; set; }

        public int IsSerial { get; set; }

        public string VoucherNo { get; set; }


        public string PartNo { get; set; }

        public List<T_OutBarCodeInfo> lstBarCode { get; set; }

        public string BarCode { get; set; }

        public int PalletType { get; set; }

        public int WareHouseID { get; set; }

        public int HouseID { get; set; }

        public int AreaID { get; set; }

        public string BatchNo { get; set; }

        public decimal? Qty { get; set; }

        public List<T_StockInfo> lstStock { get; set; }

        public string RowNoDel { get; set; }

        public DateTime SupPrdDate { get; set; }

        public string SupPrdBatch { get; set; }

        public DateTime ProductDate { get; set; }

        public string ProductBatch { get; set; }

        public string SuppliernNo { get; set; }

        public string SupplierName { get; set; }

        public string Unit { get; set; }

        public string EAN { get; set; }

        public int HouseProp { get; set; }

        public int BarCodeType { get; set; }
    }
}
