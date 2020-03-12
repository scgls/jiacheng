using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.BaseInfo
{
    public class T_Team : BILBasic.Basing.Factory.Base_Model
    {
        /// <summary>
        /// 班组
        /// </summary>
        public T_Team() : base() { }


        /// <summary>
        /// 序号
        /// </summary>
        public string Seq { get; set; }
        /// <summary>
        /// 班组code
        /// </summary>
        public string teamCode { get; set; }
        /// <summary>
        /// 班组名称
        /// </summary>
        public string teamName { get; set; }
        /// <summary>
        /// 班组长工号
        /// </summary>
        public string LeaderCode { get; set; }
        /// <summary>
        /// 翻班主管Code
        /// </summary>
        public string ShiftCode { get; set; }
        /// <summary>
        /// 翻班主管名称
        /// </summary>
        public string ShiftName { get; set; }
        /// <summary>
        /// 物理位置
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public string Attribute { get; set; }
        /// <summary>
        /// 类型（灌/包）
        /// </summary>
        public string LineType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public int isDel { get; set; }
        
    }
}
