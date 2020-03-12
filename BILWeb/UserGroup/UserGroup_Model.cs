using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.UserGroup
{
    /// <summary>
    /// T_USERGROUP的实体类
    /// 作者:方颖
    /// 日期：2016/10/24 11:13:16
    /// </summary>
    
    public class T_UserGroupInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_UserGroupInfo() : base() { }


        //私有变量
        
        private string usergroupno;
        private string usergroupname;
        private string usergroupabbname;
        private int usergrouptype;
        private int usergroupstatus;
        private string description;
        private decimal? isdel;




        //公开属性

        [Display(Name = "用户组编号")]
        public string UserGroupNo
        {
            get
            {
                return usergroupno;
            }
            set
            {
                usergroupno = value;
            }
        }
        [Display(Name = "用户组名称")]
        public string UserGroupName
        {
            get
            {
                return usergroupname;
            }
            set
            {
                usergroupname = value;
            }
        }

        public string UserGroupAbbname
        {
            get
            {
                return usergroupabbname;
            }
            set
            {
                usergroupabbname = value;
            }
        }
        [Display(Name = "用户组类型")]
        public int UserGroupType
        {
            get
            {
                return usergrouptype;
            }
            set
            {
                usergrouptype = value;
            }
        }
        [Display(Name = "状态")]
        public int UserGroupStatus
        {
            get
            {
                return usergroupstatus;
            }
            set
            {
                usergroupstatus = value;
            }
        }
        [Display(Name = "描述")]
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

        //辅助字段
        [Display(Name = "类别")]
        public string StrUserGroupType { get; set; }

        public string StrUserGroupStatus { get; set; }

        public bool BIsChecked { get; set; }
        [Display(Name = "物料类型")]
        public string MainTypeCode { get; set; }
        [Display(Name = "拣货组长")]
        public string PickLeaderUserNo { get; set; }

        public string PickGroupNo { get; set; }

        public string WarehouseNo { get; set; }

    }
}

