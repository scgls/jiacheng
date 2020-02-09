using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.BaseInfo
{
    public class T_MouldType : BILBasic.Basing.Factory.Base_Model
    {
        public T_MouldType() : base() { }

        /// <summary>
        /// id
        /// </summary>
        public string MouldTypeCode { get; set; }
        /// <summary>
        /// name
        /// </summary>
        public string MouldTypeName { get; set; }

        public string MachineType { get; set; }

        public int Qty { get; set; }
    }
}
