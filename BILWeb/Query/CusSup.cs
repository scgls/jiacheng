using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.Query
{
    public class CusSup
    {
        public string Mobile { get; set; }
        public string SendNo { get; set; }
        public string ErpVoucherNo{ get; set; }

        public string strongholdcode { get; set; }
        public int VoucherType{ get; set; }

        public string customercode { get; set; }
        public string customername { get; set; }

        public string suppliername { get; set; }

        public string supplierno { get; set; }

        public string departmentcode { get; set; }

        public string departmentname { get; set; }

        public string vouuser { get; set; }

        public string toerpwarehouse { get; set; }

        public List<Address> addresses { get; set; }

        public string address { get; set; }
    }

    public class Address
    {
        public string address { get; set; }
    }
}
