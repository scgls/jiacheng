using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.BaseInfo
{
    public class T_ProductLine : BILBasic.Basing.Factory.Base_Model
    {
        /// <summary>
        /// 产线
        /// </summary>
        public T_ProductLine() : base() { }

        int seq;
        string sn;
        string machineLineName;
        string packaging;
        string lineType;
        string fullType;
        string workroomCode;
        string capacityUnit;

        public int Seq { get { return seq; } set { seq = value; } }
        public string Sn { get { return sn; } set { sn = value; } }
        public string MachineLineName { get { return machineLineName; } set { machineLineName = value; } }
        /// <summary>
        /// 灌装/包装
        /// </summary>
        public string Packaging { get { return packaging; } set { packaging = value; } }
        /// <summary>
        /// 产线类型（自动/半自动/人工）
        /// </summary>
        public string LineType { get { return lineType; } set { lineType = value; } }
        /// <summary>
        /// 是否灌包连线
        /// </summary>
        public string FullType { get { return fullType; } set { fullType = value; } }
        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkroomCode { get { return workroomCode; } set { workroomCode = value; } }
        /// <summary>
        /// 标准产能
        /// </summary>
        public string CapacityUnit { get { return capacityUnit; } set { capacityUnit = value; } }

        /// <summary>
        /// 库存报警小时数
        /// </summary>
        public int StockErrHour { get; set; }
        /// <summary>
        /// 首次配料小时数
        /// </summary>
        public int FirstMixHour { get; set; }
        /// <summary>
        /// 单次补料小时数
        /// </summary>
        public int SingleFeedHour { get; set; }
        private readonly double _StartIssueHour = 1;
        private readonly double _TaskIntervalHour = 0.5;
        /// <summary>
        /// 任务超时时间（一张任务超规定时间未完成，直接关闭并可以产生新的任务）
        /// </summary>
        public double TaskIntervalHour
        {
            get { return _TaskIntervalHour; }
        } 

        /// <summary>
        /// 开班前备料提前小时数
        /// </summary>
        public double StartIssueHour
        {
            get { return _StartIssueHour; }
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public int isDel { get; set; }
        /// <summary>
        /// 是否组装（0:否;   1:是）
        /// </summary>
        public string Assemble { get; set; }
    }
}
