using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Area
{
    public class T_AreaInfo : BILBasic.Basing.Factory.Base_Model
    {
        private string _AreaNo;

        [Display(Name ="货位号")]
        public string AreaNo
        {
            get { return _AreaNo; }
            set { _AreaNo = value; }
        }
        private string _AreaName;
        [Display(Name = "货位名")]
        public string AreaName
        {
            get { return _AreaName; }
            set { _AreaName = value; }
        }
        private int _AreaType;
        [Display(Name = "货位类型")]
        public int AreaType
        {
            get { return _AreaType; }
            set { _AreaType = value; }
        }
        private string _ContactUser;
        [Display(Name = "联系人")]
        public string ContactUser
        {
            get { return _ContactUser; }
            set { _ContactUser = value; }
        }
        private string _ContactPhone;
        [Display(Name = "联系电话")]
        public string ContactPhone
        {
            get { return _ContactPhone; }
            set { _ContactPhone = value; }
        }
        private string _LocationDesc;
        [Display(Name = "描述")]
        public string LocationDesc
        {
            get { return _LocationDesc; }
            set { _LocationDesc = value; }
        }
        private int _AreaStatus;
        [Display(Name = "状态")]
        public int AreaStatus
        {
            get { return _AreaStatus; }
            set { _AreaStatus = value; }
        }
        private int _HouseID;
        [Display(Name = "库区")]
        public int HouseID
        {
            get { return _HouseID; }
            set { _HouseID = value; }
        }
        private string _Address;
        [Display(Name = "地址")]
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        public int IsDel { get; set; }

        //辅助字段

        public bool BIsLock { get; set; }
        [Display(Name = "仓库号")]
        public string WarehouseNo { get; set; }

        public string WarehouseName { get; set; }
        [Display(Name = "库区号")]
        public string HouseNo { get; set; }

        public string HouseName { get; set; }
        [Display(Name = "状态")]
        public string StrAreaStatus { get; set; }

        public int CheckID { get; set; }
        [Display(Name = "长cm")]
        /// <summary>
        /// 长cm
        /// </summary>
        public decimal Length { get; set; }
        [Display(Name = "宽")]
        /// <summary>
        /// 宽
        /// </summary>
        public decimal Wide { get; set; }
        [Display(Name = "高")]
        /// <summary>
        /// 高
        /// </summary>
        public decimal Hight { get; set; }

        /// <summary>
        /// 重量限制kg
        /// </summary>
        public decimal WeightLimit { get; set; }

        /// <summary>
        /// 体积限制立方
        /// </summary>
        public decimal VolumeLimit { get; set; }

        /// <summary>
        /// 数量限制
        /// </summary>
        public decimal QuantityLimit { get; set; }

        /// <summary>
        /// 托盘限制
        /// </summary>
        public decimal PalletLimit { get; set; }

        /// <summary>
        /// 温度属性
        /// </summary>
        public string TemperaturePry { get; set; }
        [Display(Name = "客户编号")]
        /// <summary>
        /// 客户编号
        /// </summary>
        public string CustomerNo { get; set; }
        [Display(Name = "客户名称")]
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 代管ID
        /// </summary>
        public int IEscrow { get; set; }

        /// <summary>
        /// 代管名称
        /// </summary>
        public string StrEscrowName { get; set; }

        public int WarehouseID { get; set; }
        [Display(Name = "高低货位")]
        public int HeightArea { get; set; }

        public string StrHeightArea { get; set; }

        public string QuanUserNo { get; set; }

        public string QuanUserName { get; set; }

        public string MaterialNo { get; set; }

        public int IsQuality { get; set; }
        [Display(Name = "货位排序")]
        public string SortArea { get; set; }
        [Display(Name = "类别")]
        public string StrAreaType{ get; set; }

        /// <summary>
        /// 库区类型 1 整箱 2 拆零
        /// </summary>
        public string HouseProp { get; set; }
    }
}
