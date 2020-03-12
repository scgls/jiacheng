using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MESDLL.OutStock
{
    public class Sender
    {
        public string name { get; set; }

        /// <summary>
        /// 用户邮编
        /// </summary>
        public string postCode { get; set; }

        /// <summary>
        /// 用户电话，包括区号、电话号码及分机号，中间用“-”分隔；
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 用户移动电话， 手机和电话至少填一项
        /// </summary>
        public string mobile { get; set; }

        public string prov { get; set; }

        public string city { get; set; }


        public string address { get; set; }
    }
}
