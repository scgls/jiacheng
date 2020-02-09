using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILWeb.InStock;
using BILWeb.Stock;
using BILWeb.Area;

namespace BILWeb.InStockTask
{
    public class T_InStockTaskDetailsInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_InStockTaskDetailsInfo() : base() { }


        //私有变量
        
        
        private string materialno;
        private string materialdesc;
        private decimal? taskqty;
        private decimal? qualityqty;
        private decimal remainqty;
        private decimal shelveqty;
       
        private decimal? isqualitycomp;
        private string keeperuserno;
        private string operatoruserno;
        private DateTime? completedatetime;
        private int taskid;
        private string tmaterialno;
        private string tmaterialdesc;
        private DateTime? operatordatetime;
        private decimal? reviewqty;
        private decimal? packcount;
        private decimal? shelvepackcount;
        private string voucherno;
        private string rowno;        
        private string trackno;
        private string unit;
        private decimal? unqualityqty;
        private decimal? postqty;
        private decimal? poststatus;
       
        private string reservenumber;
        private string reserverowno;
        private decimal? unshelveqty;
        private string requstreason;
        private string remark;
        private string reviewuser;
        private DateTime? reviewdate;
        private decimal? reviewstatus;
        private string postuser;
        private string costcenter;
        private string wbselem;
        private string tostorageloc;
        private string fromstorageloc;
        private decimal? outstockqty;
        private decimal? limitstockqtysap;
        private decimal? remainstockqtysap;
        private decimal? packflag;
        private decimal? currentremainstockqtysap;
        private string movereasoncode;
        private string movereasondesc;
        private string pono;
        private string porowno;        
        private decimal? islock;
        private decimal? issmallbatch;
      
        private string unitname;
      
     



        //公开属性
       

       

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

        public decimal? TaskQty
        {
            get
            {
                return taskqty;
            }
            set
            {
                taskqty = value;
            }
        }

        public decimal? QualityQty
        {
            get
            {
                return qualityqty;
            }
            set
            {
                qualityqty = value;
            }
        }

        public decimal RemainQty
        {
            get
            {
                return remainqty;
            }
            set
            {
                remainqty = value;
            }
        }

        public decimal ShelveQty
        {
            get
            {
                return shelveqty;
            }
            set
            {
                shelveqty = value;
            }
        }

        

        public decimal? IsQualitycomp
        {
            get
            {
                return isqualitycomp;
            }
            set
            {
                isqualitycomp = value;
            }
        }

        public string KeeperUserNo
        {
            get
            {
                return keeperuserno;
            }
            set
            {
                keeperuserno = value;
            }
        }

        public string OperatorUserNo
        {
            get
            {
                return operatoruserno;
            }
            set
            {
                operatoruserno = value;
            }
        }

        public DateTime? CompleteDateTime
        {
            get
            {
                return completedatetime;
            }
            set
            {
                completedatetime = value;
            }
        }

        public int TaskID
        {
            get
            {
                return taskid;
            }
            set
            {
                taskid = value;
            }
        }

        public string TMaterialNo
        {
            get
            {
                return tmaterialno;
            }
            set
            {
                tmaterialno = value;
            }
        }

        public string TMaterialDesc
        {
            get
            {
                return tmaterialdesc;
            }
            set
            {
                tmaterialdesc = value;
            }
        }

        public DateTime? OperatorDateTime
        {
            get
            {
                return operatordatetime;
            }
            set
            {
                operatordatetime = value;
            }
        }

        public decimal? ReviewQty
        {
            get
            {
                return reviewqty;
            }
            set
            {
                reviewqty = value;
            }
        }

        public decimal? PackCount
        {
            get
            {
                return packcount;
            }
            set
            {
                packcount = value;
            }
        }

        public decimal? ShelvePackCount
        {
            get
            {
                return shelvepackcount;
            }
            set
            {
                shelvepackcount = value;
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

        

        public string TrackNo
        {
            get
            {
                return trackno;
            }
            set
            {
                trackno = value;
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

        public decimal? UnQualityQty
        {
            get
            {
                return unqualityqty;
            }
            set
            {
                unqualityqty = value;
            }
        }

        public decimal? PostQty
        {
            get
            {
                return postqty;
            }
            set
            {
                postqty = value;
            }
        }

        public decimal? PostStatus
        {
            get
            {
                return poststatus;
            }
            set
            {
                poststatus = value;
            }
        }

        

        public string ReserveNumber
        {
            get
            {
                return reservenumber;
            }
            set
            {
                reservenumber = value;
            }
        }

        public string ReserveRowNo
        {
            get
            {
                return reserverowno;
            }
            set
            {
                reserverowno = value;
            }
        }

        public decimal? UnShelveQty
        {
            get
            {
                return unshelveqty;
            }
            set
            {
                unshelveqty = value;
            }
        }

        public string Requstreason
        {
            get
            {
                return requstreason;
            }
            set
            {
                requstreason = value;
            }
        }

        public string Remark
        {
            get
            {
                return remark;
            }
            set
            {
                remark = value;
            }
        }

        public string ReviewUser
        {
            get
            {
                return reviewuser;
            }
            set
            {
                reviewuser = value;
            }
        }

        public DateTime? ReviewDate
        {
            get
            {
                return reviewdate;
            }
            set
            {
                reviewdate = value;
            }
        }

        public decimal? ReviewStatus
        {
            get
            {
                return reviewstatus;
            }
            set
            {
                reviewstatus = value;
            }
        }

        public string PostUser
        {
            get
            {
                return postuser;
            }
            set
            {
                postuser = value;
            }
        }

        public string Costcenter
        {
            get
            {
                return costcenter;
            }
            set
            {
                costcenter = value;
            }
        }

        public string Wbselem
        {
            get
            {
                return wbselem;
            }
            set
            {
                wbselem = value;
            }
        }

        public string ToStorageLoc
        {
            get
            {
                return tostorageloc;
            }
            set
            {
                tostorageloc = value;
            }
        }

        public string FromStorageLoc
        {
            get
            {
                return fromstorageloc;
            }
            set
            {
                fromstorageloc = value;
            }
        }

        public decimal? OutStockQty
        {
            get
            {
                return outstockqty;
            }
            set
            {
                outstockqty = value;
            }
        }

        public decimal? LimitStockQtySAP
        {
            get
            {
                return limitstockqtysap;
            }
            set
            {
                limitstockqtysap = value;
            }
        }

        public decimal? RemainsSockQtySAP
        {
            get
            {
                return remainstockqtysap;
            }
            set
            {
                remainstockqtysap = value;
            }
        }

        public decimal? PackFlag
        {
            get
            {
                return packflag;
            }
            set
            {
                packflag = value;
            }
        }

        public decimal? CurrentRemainStockQtySAP
        {
            get
            {
                return currentremainstockqtysap;
            }
            set
            {
                currentremainstockqtysap = value;
            }
        }

        public string MoveReasonCode
        {
            get
            {
                return movereasoncode;
            }
            set
            {
                movereasoncode = value;
            }
        }

        public string MoveReasonDesc
        {
            get
            {
                return movereasondesc;
            }
            set
            {
                movereasondesc = value;
            }
        }

        public string PoNo
        {
            get
            {
                return pono;
            }
            set
            {
                pono = value;
            }
        }

        public string PoRowNo
        {
            get
            {
                return porowno;
            }
            set
            {
                porowno = value;
            }
        }

        

        

        public decimal? IsLock
        {
            get
            {
                return islock;
            }
            set
            {
                islock = value;
            }
        }

        public decimal? IsSmallBatch
        {
            get
            {
                return issmallbatch;
            }
            set
            {
                issmallbatch = value;
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

        public decimal ScanQty { get; set; }

        public string AreaNo { get; set; }

        public string HouseNo { get; set; }

        public string WareHouseNo { get; set; }

        public string WareHouseName { get; set; }

        public List<T_SerialNoInfo> lstSerialNo { get; set; }

        public string SupCusCode { get; set; }

        public string SupCusName { get; set; }

        public string SaleName { get; set; }

        public int TaskType { get; set; }


        public List<T_StockInfo> lstStockInfo { get; set; }

        public string OperatorUserName { get; set; }

        public string PartNo { get; set; }

        public int WarehouseID { get; set; }

        public int HouseID { get; set; }

        public int AreaID { get; set; }

        public string BatchNo { get; set; }

        public List<T_AreaInfo> lstArea { get; set; }

        public string FromBatchNo { get; set; }

        public string FromErpAreaNo { get; set; }

        public string FromErpWarehouse { get; set; }

        public string ToBatchNo { get; set; }

        public string ToErpAreaNo { get; set; }

        public string ToErpWarehouse { get; set; }

        public string UserNo { get; set; }
    }
}
