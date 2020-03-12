using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Query
{
    public class TaskTrans_Model
    {
        public int ID { get; set; }
        [Display(Name ="数量")]
        public decimal QTY { get; set; }
        [Display(Name = "状态")]
        public int Status { get; set; }
        [Display(Name = "状态")]
        public string StatusName { get; set; }
        [Display(Name = "类型")]
        public int TASKTYPE { get; set; }
        public int TASKDETAILS_ID { get; set; }
        [Display(Name = "类型")]
        public int VOUCHERTYPE { get; set; }
        [Display(Name = "ERP单号")]
        public string ERPVOUCHERNO { get; set; }
        [Display(Name = "条码")]
        public string BARCODE { get; set; }
        [Display(Name = "序列号")]
        public string SERIALNO { get; set; }
        [Display(Name = "来源仓库")]
        public string FROMWAREHOUSENO { get; set; }
        [Display(Name = "目标仓库")]
        public string TOWAREHOUSENO { get; set; }
        [Display(Name = "来源库区")]
        public string FROMHOUSENO { get; set; }
        [Display(Name = "目标库区")]
        public string TOHOUSENO { get; set; }
        [Display(Name = "来源库位")]
        public string FROMAREANO { get; set; }
        [Display(Name = "目标库位")]
        public string TOAREANO { get; set; }
        [Display(Name = "物料号")]
        public string MATERIALNO { get; set; }
        [Display(Name = "物料名")]
        public string MATERIALDESC { get; set; }
        [Display(Name = "供应商号")]
        public string SUPCUSCODE { get; set; }
        [Display(Name = "供应商")]
        public string SUPCUSNAME { get; set; }
        [Display(Name = "任务号")]
        public string TASKNO { get; set; }
        [Display(Name = "创建人")]
        public string CREATER { get; set; }
        [Display(Name = "单位")]
        public string UNIT { get; set; }
        [Display(Name = "单位")]
        public string UNITNAME { get; set; }

        public string SALENAME { get; set; }
        [Display(Name = "项次")]
        public string ROWNO { get; set; }
        [Display(Name = "项序")]
        public string ROWNODEL { get; set; }
        [Display(Name = "创建时间")]
        public DateTime CREATETIME { get; set; }

        [Display(Name = "序列号")]
        public string XH { get; set; }
        [Display(Name = "批次")]
        public string BATCHNO { get; set; }
        [Display(Name = "开始时间")]
        public string begintime { get; set; }
        [Display(Name = "结束时间")]
        public string endtime { get; set; }
        [Display(Name = "类型")]
        public string vouchertypename { get; set; }
        [Display(Name = "类型")]
        public string tasktypename { get; set; }

        public string partno { get; set; }
        [Display(Name = "据点号")]
        public string StrongHoldCode { get; set; }
        [Display(Name = "据点名")]
        public string StrongHoldName { get; set; }
        public DateTime? ProductDate { get; set; }
        [Display(Name = "称重")]
        public decimal itemqty { get; set; }
        [Display(Name = "供应商批次")]
        public string SupPrdBatch { get; set; }

        public DateTime Edate { get; set; }

    }
}
