using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Query
{
    public class CheckDet_Model
    {
       public string CHECKNO{get;set;}
       public string AREANO{get;set;}

       public int AREAID { get; set; }
       public int MATERIALID { get; set; }
       public string MATERIALNO{get;set;}
       
       public string MATERIALDESC{get;set;}
       public string SERIALNO{get;set;}
       public string BATCHNO{get;set;}
       public decimal QTY { get; set; }

       public int VoucherType { get; set; }



    }
}
