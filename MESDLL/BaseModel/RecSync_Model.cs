using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTOBDLL.BaseModel
{

    //传入接口参数格式
    //{"code":"FH1805130002-1","VoucherType":"29","max_code":"","sync_time":"0001-01-01","erp_vourcher_type":"FH"}

    public class RecSync_Model
    {

        public string code { get; set; }

        public string VoucherType { get; set; }

        public string max_code { get; set; }

        public string sync_time { get; set; }//传入参数日期和时间用,分隔

        public string erp_vourcher_type { get; set; }

        public string cDate { get; set; } //根据sync_time解析后得到的创建日期
        public string cTime { get; set; } //根据sync_time解析后得到的创建时间
        public string uDate { get; set; } //根据sync_time解析后得到的修改日期
        public string uTime { get; set; } //根据sync_time解析后得到的修改时间
        /// <summary>
        /// 工厂
        /// </summary>
        public string IS_WERKS { get; set; }
        /// <summary>
        /// 库存地点
        /// </summary>
        public string IS_LGORT { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string IS_MATNR { get; set; }
        /// <summary>
        /// 物料长文本
        /// </summary>
        public string IS_MAKTX { get; set; }
        /// <summary>
        /// 大小量纲
        /// </summary>
        public string IS_GROES { get; set; }

    }
}
