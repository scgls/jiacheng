using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.House
{
    public class T_HouseInfo : BILBasic.Basing.Factory.Base_Model
    {
        private string _HouseNo;
        [Display(Name="库区号")]
        public string HouseNo
        {
            get { return _HouseNo; }
            set { _HouseNo = value; }
        }
        private string _HouseName;
        [Display(Name = "库区名")]
        public string HouseName
        {
            get { return _HouseName; }
            set { _HouseName = value; }
        }
        private int _HouseType;
        [Display(Name = "库区类型")]
        public int HouseType
        {
            get { return _HouseType; }
            set { _HouseType = value; }
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
        private int _AreaCount;
        [Display(Name = "货位数量")]
        public int AreaCount
        {
            get { return _AreaCount; }
            set { _AreaCount = value; }
        }
        
        private int _AreaUsingCount;
        [Display(Name = "使用货位")]
        public int AreaUsingCount
        {
            get { return _AreaUsingCount; }
            set { _AreaUsingCount = value; }
        }
        private string _LocationDesc;
        [Display(Name = "描述")]
        public string LocationDesc
        {
            get { return _LocationDesc; }
            set { _LocationDesc = value; }
        }
        private int _HouseStatus;
        [Display(Name = "状态")]
        public int HouseStatus
        {
            get { return _HouseStatus; }
            set { _HouseStatus = value; }
        }
        private int _WarehouseID;
        [Display(Name = "仓库")]
        public int WarehouseID
        {
            get { return _WarehouseID; }
            set { _WarehouseID = value; }
        }
        private string _Address;
        [Display(Name = "地址")]
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }


        //辅助字段


        public string AreaNo { get; set; }

        public string AreaName { get; set; }

        public bool BIsLock { get; set; }
        [Display(Name = "仓库号")]
        public string WarehouseNo { get; set; }
        [Display(Name = "仓库名")]
        public string WarehouseName { get; set; }
        [Display(Name = "状态")]
        public string StrHouseStatus { get; set; }

        public decimal AreaRate { get; set; }

        public int IsDel { get; set; }
        [Display(Name = "类别")]
        public string StrHouseType { get; set; }

        [Display(Name = "库区楼层")]
        /// <summary>
        /// 库区对应楼层
        /// </summary>
        public int FloorType { get; set; }

        public string StrFloorType { get; set; }

        public string MaterialClassCode { get; set; }

        public string MaterialClassName { get; set; }

        [Display(Name = "库区分类")]
        /// <summary>
        /// 1-整箱库区 2-零拣库区
        /// </summary>
        public int HouseProp { get; set; }

        [Display(Name = "库区分类")]
        /// <summary>
        /// 1-整箱库区 2-零拣库区
        /// </summary>
        public string StrHouseProp { get; set; }

    }
}
