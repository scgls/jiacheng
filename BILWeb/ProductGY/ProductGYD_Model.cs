using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.ProductGY
{
    public class Mes_ProductGYDInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public Mes_ProductGYDInfo() : base() { }


        /// <summary>
        /// 工序段编码
        /// </summary>
        public string gxDuanCode { get; set; }
        /// <summary>
        /// 工序段名称
        /// </summary>
        public string gxDuanName { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string bbNo { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int gxSeq { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string MachineName { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        public string MachineType { get; set; }
        /// <summary>
        /// 模具/治具编号
        /// </summary>
        public string MouldCode { get; set; }
        /// <summary>
        /// 人数限定
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int isDel { get; set; }
        /// <summary>
        /// 产品分类
        /// </summary>
        public string cptype { get; set; }

        /// <summary>
        /// 标准工时
        /// </summary>
        public decimal workHours { get; set; }

        /// <summary>
        /// 工序操作名称list
        /// </summary>
        public List<Mes_ProductGYInfo> lstGY { get; set; }
    }
}
