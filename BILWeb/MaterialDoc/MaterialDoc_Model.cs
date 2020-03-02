using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.MaterialDoc
{
    public class T_MaterialDoc_Info : BILBasic.Basing.Factory.Base_Model
    {
        

        public int InOutStockID { get; set; }

        public int TaskID { get; set; }

        public int MaterialDocType { get; set; }


        public int TaskType { get; set; }

        public string TimeStampPost { get; set; }

        public int IsDel { get; set; }
    }
}
