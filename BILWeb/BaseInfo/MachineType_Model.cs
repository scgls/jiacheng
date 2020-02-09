using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.BaseInfo
{
    public class T_MachineType : BILBasic.Basing.Factory.Base_Model
    {
        public T_MachineType() : base() { }

        /// <summary>
        /// id
        /// </summary>
        public string MachineTypeCode { get; set; }
        /// <summary>
        /// name
        /// </summary>
        public string MachineTypeName { get; set; } 
        

    }
}
