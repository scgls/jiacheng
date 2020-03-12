using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.OutBarCode
{
    /// <summary>
    /// t_outbarcode的实体类
    /// 作者:方颖
    /// 日期：2017/3/23 15:58:10
    /// </summary>

    public class T_OutBarCodeInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_OutBarCodeInfo() : base() { }


        //私有变量

        private string voucherno;
        private string rowno;
        
        
        private string materialno;
        private string materialdesc;
        private string cuscode;
        private string cusname;
        private string supcode;
        private string supname;
        private decimal? outpackqty;
        private decimal? innerpackqty;
        
        private decimal? qty;
        private int nopack;
        private decimal? printqty;
        private string barcode;
        private int barcodetype;
        private string serialno;
        private int barcodeno;
        private int outcount;
        private int innercount;
        private decimal? mantissaqty;
        private int isrohs;
        private int outbox_id;
        private int inner_id;        
        private string batchno;
        private decimal? abatchqty;
        private int isdel;
        private string ean;
        public DateTime receivetime { get; set; }


        //公开属性

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

        public string CusCode
        {
            get
            {
                return cuscode;
            }
            set
            {
                cuscode = value;
            }
        }

        public string CusName
        {
            get
            {
                return cusname;
            }
            set
            {
                cusname = value;
            }
        }

        public string SupCode
        {
            get
            {
                return supcode;
            }
            set
            {
                supcode = value;
            }
        }

        public string SupName
        {
            get
            {
                return supname;
            }
            set
            {
                supname = value;
            }
        }

        public decimal? OutPackQty
        {
            get
            {
                return outpackqty;
            }
            set
            {
                outpackqty = value;
            }
        }

        public decimal? InnerPackQty
        {
            get
            {
                return innerpackqty;
            }
            set
            {
                innerpackqty = value;
            }
        }



        public decimal? Qty
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

        public int NoPack
        {
            get
            {
                return nopack;
            }
            set
            {
                nopack = value;
            }
        }

        public decimal? PrintQty
        {
            get
            {
                return printqty;
            }
            set
            {
                printqty = value;
            }
        }

        public string BarCode
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

        public int BarcodeType
        {
            get
            {
                return barcodetype;
            }
            set
            {
                barcodetype = value;
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

        public int BarcodeNo
        {
            get
            {
                return barcodeno;
            }
            set
            {
                barcodeno = value;
            }
        }





        public int OutCount
        {
            get
            {
                return outcount;
            }
            set
            {
                outcount = value;
            }
        }

        public int InnerCount
        {
            get
            {
                return innercount;
            }
            set
            {
                innercount = value;
            }
        }

        public decimal? MantissaQty
        {
            get
            {
                return mantissaqty;
            }
            set
            {
                mantissaqty = value;
            }
        }

        public int IsRohs
        {
            get
            {
                return isrohs;
            }
            set
            {
                isrohs = value;
            }
        }

        public int OutBox_ID
        {
            get
            {
                return outbox_id;
            }
            set
            {
                outbox_id = value;
            }
        }

        public int Inner_ID
        {
            get
            {
                return inner_id;
            }
            set
            {
                inner_id = value;
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

        public decimal? ABatchQty
        {
            get
            {
                return abatchqty;
            }
            set
            {
                abatchqty = value;
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

       

        public decimal? VoucherQty { get; set; }

        public DateTime SupPrdDate { get; set; }

        public string SupPrdBatch { get; set; }

        public DateTime ProductDate { get; set; }

        public string ProductBatch { get; set; }

        public string SpecialRequire { get; set; }

        public string BarcodeMType { get; set; }

        public int WareHouseID { get; set; }

        public int HouseID { get; set; }

        public int AreaID { get; set; }

        public string PalletNo { get; set; }

        public decimal PalletQty { get; set; }

        public string SN { get; set; }

        public int PalletType { get; set; }


        //存储条件
        public string StoreCondition { get; set; }




        public DateTime RecDate { get; set; }


        public string RecPeo { get; set; }

        //装箱明细
        public string BoxDetail { get; set; }

        //总箱数/第几箱
        public string AllIn { get; set; }

        public string RowNoDel { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        public string LabelMark { get; set; }

        /// <summary>
        /// ERP成品条码
        /// </summary>
        public string ErpBarCode { get; set; }

        //add by cym 2018-12-25 MES Using-----------------------
        /// <summary>
        /// 兆信条码
        /// </summary>
        public string ZXBarcode { get; set; }
        /// <summary>
        /// 小工单号（即作业指示书ID）
        /// </summary>
        public string zyID { get; set; }
        /// <summary>
        /// 是否需要倒扣（Y：是/N：否）
        /// </summary>
        public string UPSIDEDOWN { get; set; }
        /// <summary>
        /// 产线编号
        /// </summary>
        public string Lineno { get; set; }
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseNo { get; set; }
        /// <summary>
        /// 库位编码
        /// </summary>
        public string AreaNo { get; set; }


        //收货地址
        public string Address { get; set; }

        //物流地址
        public string Address1 { get; set; }

        //联系人
        public string Contact { get; set; }

        //电话
        public string Phone { get; set; }

        public string ProductClass { get; set; }
        public string WorkNo { get; set; }

        public string InvoiceNo { get; set; }

        public string fserialno { get; set; }

        //public int BarCodeType { get; set; }

        public List<T_OutBarCodeInfo> lstBarCode { get; set; }
    }
}

