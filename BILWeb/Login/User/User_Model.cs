using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BILWeb.Menu;

using BILWeb.Warehouse;
using System.ComponentModel.DataAnnotations;

namespace BILWeb.Login.User
{
    
    public class UserInfo : BILBasic.User.UserModel
    {

        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool BIsAdmin { get; set; }
        [Display(Name = "是否是管理员")]
        public string StrIsAdmin { get; set; }

        public DateTime CurrentTime { get; set; }

        public int IsOnline { get; set; }

        public bool BIsOnline { get; set; }

        

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public string StrUserType { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        [Display(Name = "用户状态")]
        public string StrUserStatus { get; set; }
        [Display(Name = "性别")]
        /// <summary>
        /// 用户性别
        /// </summary>
        public string StrSex { get; set; }

        public List<UserGroup.T_UserGroupInfo> lstUserGroup { get; set; }

        public List<T_MenuInfo> lstMenu { get; set; }

        public List<T_WareHouseInfo> lstWarehouse { get; set; }

        public List<T_QuanUser_Model> lstQuanUser { get; set; }

        public int TaskCount { get; set; }

        
    }
}
