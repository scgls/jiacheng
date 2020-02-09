using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.BaseInfo
{
    public class T_Machine : BILBasic.Basing.Factory.Base_Model
    {
        public T_Machine() : base(){}

        string sn;
        string machineCode;
        string machineName;
        string machineType;
        string brand;
        string conMode;
        int deviceStatus;
        string iPAddress;
        string port;
        string tVBrand;
        string tVConMode;
        string manufacturer;
        decimal capacity;
        string unit;
        decimal capacity2;
        int eHSStatus;
        int retainPeriod;
        string remark;

        /// <summary>
        /// 二维码
        /// </summary>
        public string Sn { get { return sn; } set { sn = value; } }
        /// <summary>
        /// 设备code
        /// </summary>
        public string MachineCode { get { return machineCode; } set { machineCode = value; } }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string MachineName { get { return machineName; } set { machineName = value; } }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string MachineType { get { return machineType; } set { machineType = value; } }
        /// <summary>
        /// PLC品牌型号
        /// </summary>
        public string Brand { get { return brand; } set { brand = value; } }
        /// <summary>
        /// PLC通讯方式（以太网、RS232等）
        /// </summary>
        public string ConMode { get { return conMode; } set { conMode = value; } }
        /// <summary>
        /// 设备状态
        /// </summary>
        public int DeviceStatus { get { return deviceStatus; } set { deviceStatus = value; } }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get { return iPAddress; } set { iPAddress = value; } }
        /// <summary>
        /// 端口号
        /// </summary>
        public string Port { get { return port; } set { port = value; } }
        /// <summary>
        /// 触屏品牌型号
        /// </summary>
        public string TVBrand { get { return tVBrand; } set { tVBrand = value; } }
        /// <summary>
        /// 触屏通讯端口（以太网、RS232等）
        /// </summary>
        public string TVConMode { get { return tVConMode; } set { tVConMode = value; } }
        /// <summary>
        /// 制造商
        /// </summary>
        public string Manufacturer { get { return manufacturer; } set { manufacturer = value; } }
        /// <summary>
        /// 标准产能
        /// </summary>
        public decimal Capacity { get { return capacity; } set { capacity = value; } }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get { return unit; } set { unit = value; } }
        /// <summary>
        /// 标准产能（副）
        /// </summary>
        public decimal Capacity2 { get { return capacity2; } set { capacity2 = value; } }
        /// <summary>
        /// EHS状态（必须品保和EHS确认过）
        /// </summary>
        public int EHSStatus { get { return eHSStatus; } set { eHSStatus = value; } }
        /// <summary>
        /// 保养时间
        /// </summary>
        public int MaintainTime { get { return retainPeriod; } set { retainPeriod = value; } }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get { return remark; } set { remark = value; } }

        /// <summary>
        /// 属性(组装机、灌装机等)
        /// </summary>
        public string MachineProperty { get; set; }
        /// <summary>
        /// 设备型号（自动:01、半自动:02、手工:03）
        /// </summary>
        public string MachineEversion { get; set; } 
        
        /// <summary>
        /// 放置区域
        /// </summary>
        public string AddressSite { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public int isDel { get; set; }
    }
}
