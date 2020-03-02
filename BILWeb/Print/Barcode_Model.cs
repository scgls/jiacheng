using BILWeb.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.Print
{
    public class Barcode_Model : DBModel
    {
        

        //私有变量

        private string voucherno;
        private string rowno;
        private string erpvoucherno;

        private string materialno;
        private string materialdesc;
        private string cuscode;
        private string cusname;
        private string supcode;
        private string supname;
        private decimal? outpackqty;
        private decimal? innerpackqty;

        private decimal qty;
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
        private DateTime receivetime;


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

        public DateTime ReceiveTime
        {
            get
            {
                return receivetime;
            }
            set
            {
                receivetime = value;
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

        public string ErpVoucherNo
        {
            get
            {
                return erpvoucherno;
            }
            set
            {
                erpvoucherno = value;
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



        public decimal Qty
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
        //特殊要求
        public string SpecialRequire { get; set; }

        public string BarcodeMType { get; set; }

        //存储条件
        public string StoreCondition { get; set; }

        //防护措施
        public string ProtectWay { get; set; }
        public string RowNoDel { get; set; }

        public string VoucherType { get; set; }

        public string Creater { get; set; }

        public DateTime CreateTime { get; set; }
        public int MaterialNoID { get; set; }

        public string StrongHoldCode { get; set; }
        public string StrongHoldName { get; set; }
        public string CompanyCode { get; set; }


        public DateTime EDate { get; set; }

        //皮重
        public string BoxWeight { get; set; }

        public string LABELMARK { get; set; }
        public string Unit { get; set; }

        //装箱明细
        public string BoxDetail { get; set; }

        //料体批次
        public string MateBatch { get; set; }
        //混料日期
        public DateTime MixDate { get; set; }

        //相对比重
        public string RelaWeight { get; set; }

        //生产班组
        public string ProductClass { get; set; }

        public decimal ItemQty { get; set; }

        //工单
        public string WorkNo { get; set; }

        //识别是否客供品，是填写K不是填写物料类别编码
        public string MTYPEF { get; set; }
        
        public string PROROWNO  { get; set; }

        public string PROROWNODEL { get; set; }
        [DBAttribute(NotDBField = true)]
        public string brandno { get; set; }
        [DBAttribute(NotDBField = true)]
        public string ZXBARCODE { get; set; }

        [DBAttribute(NotDBField = true)]
        public string zyID { get; set; }

        [DBAttribute(NotDBField = true)]
        public bool isCheck { get; set; }

        [DBAttribute(NotDBField = true)]
        public DateTime RecDate { get; set; }

        [DBAttribute(NotDBField = true)]
        public string RecPeo { get; set; }

        
        

        
        //总箱数/第几箱
        [DBAttribute(NotDBField = true)]
        public string AllIn { get; set; }

        [DBAttribute(NotDBField = true)]
        public string begin { get; set; }

        [DBAttribute(NotDBField = true)]
        public string end { get; set; }

        [DBAttribute(NotDBField = true)]
        public string areano { get; set; }

        [DBAttribute(NotDBField = true)]
        public string CHECKNO { get; set; }

        [DBAttribute(NotDBField = true)]
        public int AREAID { get; set; }

        [DBAttribute(NotDBField = true)]
        public string SendPeo { get; set; }

        [DBAttribute(NotDBField = true)]
        public string DelAddress { get; set; }

        [DBAttribute(NotDBField = true)]
        public string PostPeo { get; set; }

        [DBAttribute(NotDBField = true)]
        public string WareAddress { get; set; }

        [DBAttribute(NotDBField = true)]
        public string IP { get; set; }

        [DBAttribute(NotDBField = true)]
        public DateTime PrintCreateTime { get; set; }

        [DBAttribute(NotDBField = true)]

        public string PrintCreater { get; set; }

        //托盘明细
        [DBAttribute(NotDBField = true)]
        public string PalletDetail { get; set; }

        //总箱数

        public decimal BoxCount { get; set; }

        [DBAttribute(NotDBField = true)]
        public string PalletNo { get; set; }

        //检验状态
        [DBAttribute(NotDBField = true)]
        public int STATUS { get; set; }


        [DBAttribute(NotDBField = true)]
        public string warehouseno { get; set; }

        [DBAttribute(NotDBField = true)]
        public string warehousename { get; set; }

        [DBAttribute(NotDBField = true)]
        public decimal sqty { get; set; }

        [DBAttribute(NotDBField = true)]
        public string houseno { get; set; }

        [DBAttribute(NotDBField = true)]
        public int id { get; set; }

        [DBAttribute(NotDBField = true)]
        public string Eds { get; set; }
        [DBAttribute(NotDBField = true)]
        public string ErpBarCode { get; set; }

        [DBAttribute(NotDBField = true)]
        public int standardbox1 { get; set; }

        [DBAttribute(NotDBField = true)]
        public int standardbox2 { get; set; }

        [DBAttribute(NotDBField = true)]
        public int standardbox3 { get; set; }

        [DBAttribute(NotDBField = true)]
        public int year { get; set; }

        [DBAttribute(NotDBField = true)]
        public int month { get; set; }

        [DBAttribute(NotDBField = true)]
        public int day { get; set; }


    }
}
