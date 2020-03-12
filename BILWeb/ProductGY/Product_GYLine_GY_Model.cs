using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.ProductGY
{
    public class Mes_GYLine_GYInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public Mes_GYLine_GYInfo() : base() { }


        /// <summary>
        /// 工艺明细ID
        /// </summary>
        public string gyDetailID { get; set; }
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public string gyLineID { get; set; }
        /// <summary>
        /// 工序操作ID
        /// </summary>
        public string gxID { get; set; }
        /// <summary>
        /// 产品号
        /// </summary>
        public string cpCode { get; set; }
        /// <summary>
        /// 工序段编码
        /// </summary>
        public string gxDuanCode { get; set; }
        /// <summary>
        /// 排列顺序
        /// </summary>
        public int Seq { get; set; }
        /// <summary>
        /// 工序操作名称
        /// </summary>
        public string gxName { get; set; }
        /// <summary>
        /// 操作描述
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 照片说明
        /// </summary>
        public byte[] imagePic { get; set; }
        /// <summary>
        /// Video显示
        /// </summary>
        public string videoUrl { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 照片说明2
        /// </summary>
        public byte[] imagePic2 { get; set; }
        /// <summary>
        /// 照片说明3
        /// </summary>
        public byte[] imagePic3 { get; set; }


        /// <summary>
        /// 限定参数list
        /// </summary>
        public List<Mes_ProductGY_LimitInfo> lstLimit { get; set; }
    }
}
