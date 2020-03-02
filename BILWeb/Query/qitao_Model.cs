using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Query
{
    public class qitao_Model
    {
        public int ID { get; set; }
        public decimal QTY { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }
        public int TASKTYPE { get; set; }
        public int TASKDETAILS_ID { get; set; }
        public int VOUCHERTYPE { get; set; }

        public string ERPVOUCHERNO { get; set; }
        public string BARCODE { get; set; }
        public string SERIALNO { get; set; }
        public string FROMWAREHOUSENO { get; set; }
        public string TOWAREHOUSENO { get; set; }
        public string FROMHOUSENO { get; set; }
        public string TOHOUSENO { get; set; }
        public string FROMAREANO { get; set; }
        public string TOAREANO { get; set; }
        public string MATERIALNO { get; set; }
        public string MATERIALDESC { get; set; }
        public string SUPCUSCODE { get; set; }
        public string SUPCUSNAME { get; set; }
        public string TASKNO { get; set; }
        public string CREATER { get; set; }
        public string UNIT { get; set; }
        public string UNITNAME { get; set; }
        public string SALENAME { get; set; }
        public string ROWNO { get; set; }

        public string ROWNODEL { get; set; }
        public DateTime CREATETIME { get; set; }


        public string XH { get; set; }
        public string BATCHNO { get; set; }

        public string begintime { get; set; }
        public string endtime { get; set; }
        public string vouchertypename { get; set; }

        public string tasktypename { get; set; }

        public string partno { get; set; }

        public string StrongHoldCode { get; set; }
        public string StrongHoldName { get; set; }
        public DateTime? ProductDate { get; set; }

        public decimal itemqty { get; set; }

        public string SupPrdBatch { get; set; }

        public DateTime Edate { get; set; }

        public string LINEMANAGENO { get; set; }

        public string ERPNO { get; set; }

        public DateTime? FROMTIME { get; set; }

        public DateTime? TOTIME { get; set; }
        
    }
}
