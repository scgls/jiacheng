using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.InStockTask
{
    public class T_InStockTaskInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_InStockTaskInfo() : base() { }


        //私有变量


        private decimal? tasktype;

        private string supcusname;
        private decimal? taskstatus;
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


        [Display(Name = "供应商")]
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
        [Display(Name = "状态")]
        public decimal? TaskStatus
        {
            get
            {
                return taskstatus;
            }
            set
            {
                taskstatus = value;
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
        [Display(Name ="任务下发时间")]
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


        [Display(Name = "备注")]
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
        [Display(Name = "供应商编码")]
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

        public string TaskIsSuedUser
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


        public string MaterialNo { get; set; }

        public int InStockID { get; set; }

        public string StrTaskType { get; set; }
        [Display(Name = "任务下发人")]
        public string StrTaskIsSuedUser { get; set; }
        [Display(Name = "收货入库单")]
        /// <summary>
        /// 采购收货入库单
        /// </summary>
        public string ErpInVoucherNo { get; set; }

        public int StrongHoldType { get; set; }


        public string WareHouseNo { get; set; }

        public string WareHouseName { get; set; }

        public int WareHouseID { get; set; }
    }
}
