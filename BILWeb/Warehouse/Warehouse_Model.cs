using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.Warehouse
{
    /// <summary>
    /// T_WAREHOUSE的实体类
    /// 作者:方颖
    /// 日期：2016/10/20 11:00:31
    /// </summary>
    
    public class T_WareHouseInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_WareHouseInfo() : base() { }


        //私有变量
        
        private string warehouseno;
        private string warehousename;
        private int warehousetype;
        private string contactuser;
        private string contactphone;
        private int housecount;
        private int houseusingcount;
        private string address;
        private string locationdesc;
        private int warehousestatus;




        //公开属性

        [Display(Name = "仓库编号")]
        public string WareHouseNo
        {
            get
            {
                return warehouseno;
            }
            set
            {
                warehouseno = value;
            }
        }
        [Display(Name = "仓库名")]
        public string WareHouseName
        {
            get
            {
                return warehousename;
            }
            set
            {
                warehousename = value;
            }
        }

        [Display(Name = "类别")]
        public int WareHouseType
        {
            get
            {
                return warehousetype;
            }
            set
            {
                warehousetype = value;
            }
        }

        [Display(Name = "联系人")]
        public string ContactUser
        {
            get
            {
                return contactuser;
            }
            set
            {
                contactuser = value;
            }
        }
        [Display(Name = "联系电话")]
        public string ContactPhone
        {
            get
            {
                return contactphone;
            }
            set
            {
                contactphone = value;
            }
        }

        [Display(Name = "库区数量")]
        public int HouseCount
        {
            get
            {
                return housecount;
            }
            set
            {
                housecount = value;
            }
        }

        [Display(Name = "使用库区数")]
        public int HouseUsingCount
        {
            get
            {
                return houseusingcount;
            }
            set
            {
                houseusingcount = value;
            }
        }
        [Display(Name = "地址")]
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
            }
        }
        [Display(Name = "描述")]
        public string LocationDesc
        {
            get
            {
                return locationdesc;
            }
            set
            {
                locationdesc = value;
            }
        }
        [Display(Name = "状态")]
        public int WareHouseStatus
        {
            get
            {
                return warehousestatus;
            }
            set
            {
                warehousestatus = value;
            }
        }


        public decimal? IsDel { get; set; }

        //辅助字段
        [Display(Name = "库区编号")]
        public string HouseNo { get; set; }
        [Display(Name = "库区名")]
        public string HouseName { get; set; }

        public bool BIsLock { get; set; }

        public bool BIsChecked { get; set; }
        [Display(Name = "类别")]
        public string StrWarehouseType { get; set; }
        [Display(Name = "状态")]
        public string StrWarehouseStatus { get; set; }
        [Display(Name = "库区数量")]
        public int AreaCount { get; set; }
        [Display(Name = "使用库区数")]
        public int AreaUsingCount { get; set; }

        public decimal HouseRate { get; set; }
        [Display(Name = "库区使用率")]
        public decimal AreaRate { get; set; }

        /// <summary>
        /// 拣货规则
        /// </summary>
        public int PickRule { get; set; }

        /// <summary>
        /// 取样人编号
        /// </summary>
        public string SamplerCode { get; set; }

        /// <summary>
        /// 取样人姓名
        /// </summary>
        public string SamplerName { get; set; }

        /// <summary>
        /// 楼层
        /// </summary>
        public string Floor { get; set; }

    }
}

