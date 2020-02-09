using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.ProductGY
{
    public class Mes_ProductGYLine_GYDInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public Mes_ProductGYLine_GYDInfo() : base() { }


        /// <summary>
        /// 工序段编码
        /// </summary>
        public string gxDuanCode { get; set; }
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public string gyLineID { get; set; }
        /// <summary>
        /// 工序段名称
        /// </summary>
        public string gxDuanName { get; set; }
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
        public decimal Qty { get; set; }

        /// <summary>
        /// 真实的工序段编码
        /// </summary>
        public string gxDCode { get; set; }

        /// <summary>
        /// 工序操作list
        /// </summary>
        public List<Mes_GYLine_GYInfo> lstGY { get; set; }
    }
}
