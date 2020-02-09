using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.RuleAll
{
    /// <summary>
    /// t_ruleall的实体类
    /// 作者:方颖
    /// 日期：2019/6/4 15:36:28
    /// </summary>

    public class T_RuleAllInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_RuleAllInfo() : base() { }


        //私有变量               
       
        private int? conitemid;
        private int? isenable;



        //公开属性


        public int? ConItemID
        {
            get
            {
                return conitemid;
            }
            set
            {
                conitemid = value;
            }
        }

        public int? IsEnable
        {
            get
            {
                return isenable;
            }
            set
            {
                isenable = value;
            }
        }

        public int IsDel { get; set; }

        public string strConItemID { get; set; }
        public string strIsEnable { get; set; }

    }
}

