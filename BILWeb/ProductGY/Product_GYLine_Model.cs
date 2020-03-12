using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.ProductGY
{
    public class Mes_ProductGYLineInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public Mes_ProductGYLineInfo() : base() { }


        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public string gyLineID { get; set; }
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
        public int Seq { get; set; }
        /// <summary>
        /// 工艺分类预留
        /// </summary>
        public string bbID { get; set; }
        /// <summary>
        /// 工艺版本号
        /// </summary>
        public string gybbID { get; set; }
        /// <summary>
        /// 产品号
        /// </summary>
        public string cpCode { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string cpName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string gyLineName { get; set; }
        /// <summary>
        /// 是否新品
        /// </summary>
        public int isNew { get; set; }
        /// <summary>
        /// 是否模板(0: 是;  1:否)
        /// </summary>
        public int isMoban { get; set; }
        /// <summary>
        /// 总人数
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// 产线标准产能
        /// </summary>
        public decimal? capacity { get; set; }

        /// <summary>
        /// 加工方式
        /// </summary>
        public string processMode { get; set; }
        /// <summary>
        /// 产线类型（0:灌装/1:包装/2:灌包连线/3:组装）
        /// </summary>
        public string lineType { get; set; }
        /// <summary>
        /// 生技审批者
        /// </summary>
        public string ApprovalSOP { get; set; }
        /// <summary>
        /// 生产审批者
        /// </summary>
        public string ApprovalProduct { get; set; }
        /// <summary>
        /// 品保审批者
        /// </summary>
        public string ApprovalQC { get; set; }
        /// <summary>
        /// 驳回原因
        /// </summary>
        public string returnRemark { get; set; }

        /// <summary>
        /// 工序段list
        /// </summary>
        public List<Mes_ProductGYLine_GYDInfo> lstGYLine_GYD { get; set; }
    }
}
