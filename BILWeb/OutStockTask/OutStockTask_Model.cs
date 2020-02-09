using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.OutStockTask
{
    public class T_OutStockTaskInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_OutStockTaskInfo() : base() { }


        //私有变量

        
        
        private decimal? tasktype;
        private string supcusname;        
        private string audituserno;
        private DateTime? auditdatetime;
        private DateTime? taskissued;
        private string receiveuserno;
        private string remark;
        private string reason;
        private string supcuscode;
        private decimal? isshelvepost;        
       
        private decimal? isquality;
        private decimal? isreceivepost;
        private string plant;
        private string plantname;
        private decimal? poststatus;
        private string movetype;
        private decimal? isoutstockpost;
        private decimal? isundershelvepost;      
        private decimal? reviewstatus;
        private string movereasoncode;
        private string movereasondesc;
        private decimal? printqty;
        private DateTime? printtime;
        private DateTime? closedatetime;
        private string closeuserno;
        private string closereason;
        private decimal? isowe;
        private decimal? isurgent;
        private DateTime? outstockdate;
        private string taskissueduser;



        //公开属性


        

        public decimal? TaskType
        {
            get
            {
                return tasktype;
            }
            set
            {
                tasktype = value;
            }
        }

       
        [Display(Name ="供应商")]
        public string SupcusName
        {
            get
            {
                return supcusname;
            }
            set
            {
                supcusname = value;
            }
        }

        

        public string AuditUserNo
        {
            get
            {
                return audituserno;
            }
            set
            {
                audituserno = value;
            }
        }

        public DateTime? AuditDateTime
        {
            get
            {
                return auditdatetime;
            }
            set
            {
                auditdatetime = value;
            }
        }
        [Display(Name = "任务下发时间")]
        public DateTime? TaskIssued
        {
            get
            {
                return taskissued;
            }
            set
            {
                taskissued = value;
            }
        }

        public string ReceiveUserNo
        {
            get
            {
                return receiveuserno;
            }
            set
            {
                receiveuserno = value;
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

        public string Reason
        {
            get
            {
                return reason;
            }
            set
            {
                reason = value;
            }
        }
        [Display(Name = "供应商编号")]
        public string SupcusCode
        {
            get
            {
                return supcuscode;
            }
            set
            {
                supcuscode = value;
            }
        }



        public decimal? IsShelvePost
        {
            get
            {
                return isshelvepost;
            }
            set
            {
                isshelvepost = value;
            }
        }

        
       

        public decimal? IsQuality
        {
            get
            {
                return isquality;
            }
            set
            {
                isquality = value;
            }
        }

        public decimal? IsReceivePost
        {
            get
            {
                return isreceivepost;
            }
            set
            {
                isreceivepost = value;
            }
        }

        public string Plant
        {
            get
            {
                return plant;
            }
            set
            {
                plant = value;
            }
        }

        public string PlanName
        {
            get
            {
                return plantname;
            }
            set
            {
                plantname = value;
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

        public string MoveType
        {
            get
            {
                return movetype;
            }
            set
            {
                movetype = value;
            }
        }

        public decimal? IsOutStockPost
        {
            get
            {
                return isoutstockpost;
            }
            set
            {
                isoutstockpost = value;
            }
        }

        public decimal? IsUnderShelvePost
        {
            get
            {
                return isundershelvepost;
            }
            set
            {
                isundershelvepost = value;
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

        public DateTime? PrintTime
        {
            get
            {
                return printtime;
            }
            set
            {
                printtime = value;
            }
        }

        public DateTime? CloseDateTime
        {
            get
            {
                return closedatetime;
            }
            set
            {
                closedatetime = value;
            }
        }

        public string CloseUserNo
        {
            get
            {
                return closeuserno;
            }
            set
            {
                closeuserno = value;
            }
        }

        public string CloseReason
        {
            get
            {
                return closereason;
            }
            set
            {
                closereason = value;
            }
        }

        public decimal? IsOwe
        {
            get
            {
                return isowe;
            }
            set
            {
                isowe = value;
            }
        }

        public decimal? IsUrgent
        {
            get
            {
                return isurgent;
            }
            set
            {
                isurgent = value;
            }
        }

        public DateTime? OutStockDate
        {
            get
            {
                return outstockdate;
            }
            set
            {
                outstockdate = value;
            }
        }

        public string TaskIsSueduser
        {
            get
            {
                return taskissueduser;
            }
            set
            {
                taskissueduser = value;
            }
        }

        [Display(Name = "物料编号")]
        public string MaterialNo { get; set; }

        [Display(Name = "是否指定批次")]
        /// <summary>
        /// 是否指定批次
        /// </summary>
        public string IsSpcBatch { get; set; }
   
        public int FloorType { get; set; }
        [Display(Name = "楼层")]
        public string FloorName { get; set; }
        [Display(Name = "批次")]
        public string BatchNo { get; set; }
        [Display(Name = "项序")]
        public string RowNoDel { get; set; }
        [Display(Name = "拣货组长编号")]
        public string PickLeaderUserNo { get; set; }

        public string PickGroupNo { get; set; }

        public string StrPickLeaderUserNo { get; set; }
        [Display(Name = "拣货人编号")]
        public string PickUserNo { get; set; }
        [Display(Name = "拣货人")]
        public string PickUserName { get; set; }

        public int WareHouseID { get; set; }
        [Display(Name = "仓库")]
        public string WareHouseName { get; set; }
        [Display(Name = "仓库编号")]
        public string WareHouseNo { get; set; }

        /// <summary>
        /// 1-不需要做有效期控制，2-需要做有效期控制
        /// </summary>
        public string IsEdate { get; set; }

        public string HeightArea { get; set; }
        [Display(Name = "高低库位")]
        public string HeightAreaName { get; set; }

        public string IssueType { get; set; }

        public int TaskCount { get; set; }

        /// <summary>
        /// 拣货车编码
        /// </summary>
        [Display(Name = "拣货车")]
        public string CarNo { get; set; }

        /// <summary>
        /// 查询出库任务总览
        /// PC=1 其他情况为PDA查询
        /// </summary>
        public int PcOrPda { get; set; }

        //库区属性 1-整箱区 2-零散区
        [Display(Name = "任务属性")]
        public string StrHouseProp { get; set; }

        public string BarCode { get; set; }

        
    }
}
