using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.ProductGY
{
    public class Mes_ProductGYInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public Mes_ProductGYInfo() : base() { }


        /// <summary>
        /// 工序操作ID
        /// </summary>
        public string gxID { get; set; }
        /// <summary>
        /// 产品分类
        /// </summary>
        public string cptype { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public int isDel { get; set; }
        /// <summary>
        /// 设备类型（自动、半自动、手工）
        /// </summary>
        public string machineType { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int gxSeq { get; set; }
        /// <summary>
        /// 工序名称
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
        /// 版本号
        /// </summary>
        public string bbNo { get; set; }
        /// <summary>
        /// 工序段编码
        /// </summary>
        public string gxDuanCode { get; set; }
        /// <summary>
        /// 工序段名称
        /// </summary>
        public string gxDuanName { get; set; }

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
