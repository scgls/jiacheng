using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILBasic.User
{
    public class UserModel : Basing.Factory.Base_Model
    {

        //私有变量
        

        private string username;
        private string password;
        private int usertype;
        private string pinyin;
        private string duty;
        private string tel;
        private string mobile;
        private string email;
        private int sex;
        private int ispick;
        private int isreceive;
        private int isquality;
        private int userstatus;
        private string address;
        private string groupcode;
        private string warehousecode;
        private string description;
        private string loginip;
        private DateTime? logintime;
        private decimal? isdel;

        private string logindevice;
        private string deviceversion;



        //公开属性

        [Display(Name = "用户编号")]
        public string UserNo { get; set; }
        [Display(Name = "用户名称")]
        public string UserName
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }
        [Display(Name = "登陆密码")]
        public string PassWord
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }
        [Display(Name = "确认密码")]
        public string RePassword { get; set; }

        public int UserType
        {
            get
            {
                return usertype;
            }
            set
            {
                usertype = value;
            }
        }

        public string PinYin
        {
            get
            {
                return pinyin;
            }
            set
            {
                pinyin = value;
            }
        }

        public string Duty
        {
            get
            {
                return duty;
            }
            set
            {
                duty = value;
            }
        }
        [Display(Name = "联系电话")]
        public string Tel
        {
            get
            {
                return tel;
            }
            set
            {
                tel = value;
            }
        }
        [Display(Name = "手机号")]
        public string Mobile
        {
            get
            {
                return mobile;
            }
            set
            {
                mobile = value;
            }
        }
        [Display(Name = "邮箱")]
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }
        [Display(Name = "性别")]
        public int Sex
        {
            get
            {
                return sex;
            }
            set
            {
                sex = value;
            }
        }

        public int IsPick
        {
            get
            {
                return ispick;
            }
            set
            {
                ispick = value;
            }
        }

        public int IsReceive
        {
            get
            {
                return isreceive;
            }
            set
            {
                isreceive = value;
            }
        }

        public int IsQuality
        {
            get
            {
                return isquality;
            }
            set
            {
                isquality = value;
            }
        }

        [Display(Name = "用户状态")]
        public int UserStatus
        {
            get
            {
                return userstatus;
            }
            set
            {
                userstatus = value;
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

        public string GroupCode
        {
            get
            {
                return groupcode;
            }
            set
            {
                groupcode = value;
            }
        }

        /// <summary>
        /// 登录选择的仓库ID，用于查找收货库位和发货库位
        /// </summary>
        public int WarehouseID
        {
            get;
            set;
        }

        public string ReceiveWareHouseNo { get; set; }

        /// <summary>
        /// 仓库对应的收货临时库区，只能对应一个库区
        /// </summary>
        public int ReceiveHouseID { get; set; }

        public string ReceiveHouseNo { get; set; }

        /// <summary>
        /// 仓库对应的收货临时库位，只能对应一个库位
        /// </summary>
        public int ReceiveAreaID { get; set; }

        public string ReceiveAreaNo { get; set; }

        public string ReceiveWareHouseName { get; set; }

        /// <summary>
        /// 仓库对应的发货临时库区，只能对应一个库区
        /// </summary>
        public int PickHouseID { get; set; }

        public string PickHouseNo { get; set; }


        /// <summary>
        /// 仓库对应的发货临时库位，只能对应一个库位
        /// </summary>
        public int PickAreaID { get; set; }

        public int PickWareHouseID { get; set; }

        public string WarehouseCode
        {
            get
            {
                return warehousecode;
            }
            set
            {
                warehousecode = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        public string LoginIP
        {
            get
            {
                return loginip;
            }
            set
            {
                loginip = value;
            }
        }

        public DateTime? LoginTime
        {
            get
            {
                return logintime;
            }
            set
            {
                logintime = value;
            }
        }

        public decimal? IsDel
        {
            get
            {
                return isdel;
            }
            set
            {
                isdel = value;
            }
        }



        public string LoginDevice
        {
            get
            {
                return logindevice;
            }
            set
            {
                logindevice = value;
            }
        }

        public string Deviceversion
        {
            get
            {
                return deviceversion;
            }
            set
            {
                deviceversion = value;
            }
        }

        public string StrIsPick { get; set; }

        public string StrIsReceive { get; set; }

        public string GroupName { get; set; }
       
        public int IsPickLeader { get; set; }
        [Display(Name = "拣货组长")]
        public string  StrIsPickLeader { get; set; }

        public bool PickLeader { get; set; }

        public string CYAccount { get; set; }

        public string CXAccount { get; set; }

        public string FCAccount { get; set; }

        public string PostAccount { get; set; }

        public string QuanUserNo { get; set; }

        public string QuanUserName { get; set; }

        public string PDAPrintIP { get; set; }

        public string ToSampWareHouseNo { get; set; }

        public string ToSampAreaNo { get; set; }

        public string PickWareHouseNo { get; set; }

        public string PickWareHouseName { get; set; }

        public string PickAreaNo { get; set; }

        public string LoginPassword { get; set; }

        public string LoginType { get; set; }

        public bool IsChangePwd { get; set; }
    }
}
