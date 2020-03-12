using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTOBDLL.Model
{
    public class JsonReturn
    {
        public string Result { get; set; }

        public string resultValue { get; set; }

        public string ErrMsg { get; set; }

        //public List<data> data { get; set; }

        public List<string> data { get; set; }

        public string MaterialDoc { get; set; }

        public string MaterialYear { get; set; }


        public string GUID { get; set; }

        /// <summary>
        /// 三段码
        /// </summary>
        public string shortAddress { get; set; }

        /// <summary>
        /// 末端网点代码
        /// </summary>
        public string consigneeBranchCode { get; set; }

        public string qrCode { get; set; }

        /// <summary>
        /// 物流订单号
        /// </summary>
        public string txLogisticID { get; set; }

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

    public class data
    {
        public header head { get; set; }
    }


    public class header 
    {
        public List<body> body { get; set; }
    }

    public class body 
    {
        /// <summary>
        /// 记账日期(当天)
        /// </summary>
        public DateTime BUDAT { get; set; }
        /// <summary>
        /// 凭证日期(当天)
        /// </summary>
        public DateTime BLDAT { get; set; }
        /// <summary>
        /// 过账人
        /// </summary>
        public string USNAM { get; set; }
        /// <summary>
        /// GUID
        /// </summary>
        public string GUID { get; set; }
        

    }
}
