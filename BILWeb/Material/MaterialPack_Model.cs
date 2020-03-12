using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Material
{
    public class MaterialPack_Model : BILBasic.Basing.Factory.Base_Model
    {

        //public int ID { get; set; }
        //public int HEADERID { get; set; }

        public string MATERIALNO { get; set; }
        public string MATERIALDESC { get; set; }

        public int ISDEL { get; set; }
        /// <summary>
        ///// 据点编号
        ///// </summary>
        //public string STRONGHOLDCODE { get; set; }

        ///// <summary>
        ///// 据点名称
        ///// </summary>
        //public string STRONGHOLDNAME { get; set; }
        ///// <summary>
        ///// 企业编号
        ///// </summary>
        //public string COMPANYCODE { get; set; }

        /// <summary>
        /// 保质期天
        /// </summary>
        public int QUALITYDAY { get; set; }
        /// <summary>
        /// 保质期月
        /// </summary>
        public int QUALITYMON { get; set; }

        /// <summary>
        /// 包装量
        /// </summary>
        public int QTY { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string UNIT { get; set; }
        /// <summary>
        /// 整箱包装量
        /// </summary>
        public decimal UNITNUM { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string WATERCODE { get; set; }


    }
}
