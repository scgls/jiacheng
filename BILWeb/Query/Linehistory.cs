using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Query
{
    public class Linehistory
    {
        public Int32 rownum { get; set; } 
        public string ERPVOUCHERNO { get; set; }

        public string PRODUCTTEAMNO { get; set; }

        public decimal SUMTIME { get; set; }

        public decimal POSTNUM { get; set; }

        public decimal PRODUCTQTY { get; set; }

        public string PERC { get; set; }

        public decimal NUMUSER { get; set; }

        public string MATERIALDESC { get; set; }

    }
}
