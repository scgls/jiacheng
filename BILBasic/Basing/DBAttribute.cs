using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILBasic.Basing
{

    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public class DBAttribute : System.Attribute
    {
        public bool NotDBField { get; set; }
    }
}
