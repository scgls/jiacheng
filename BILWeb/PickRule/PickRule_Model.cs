using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.PickRule
{
    /// <summary>
    /// t_pickrule的实体类
    /// 作者:方颖
    /// 日期：2017/6/21 16:34:14
    /// </summary>

    public class T_PickRuleInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_PickRuleInfo() : base() { }


        //私有变量
        
        private string materialclasscode;
        private string materialclassname;
        private int pickrulecode;
        private string pickrulename;


        [Display(Name = "类别编号")]
        
        //公开属性


        public string MaterialClassCode
        {
            get
            {
                return materialclasscode;
            }
            set
            {
                materialclasscode = value;
            }
        }
        [Display(Name = "物料类别")]
        public string MaterialClassName
        {
            get
            {
                return materialclassname;
            }
            set
            {
                materialclassname = value;
            }
        }
        [Display(Name = "规则编号")]
        public int PickRuleCode
        {
            get
            {
                return pickrulecode;
            }
            set
            {
                pickrulecode = value;
            }
        }
        [Display(Name = "规则名称")]
        public string PickRuleName
        {
            get
            {
                return pickrulename;
            }
            set
            {
                pickrulename = value;
            }
        }
        [Display(Name = "是否删除")]
        public int IsDel { get; set; }
        [Display(Name = "规则描述")]
        public string Note { get; set; }

        [Display(Name = "规则类型")]
        public int RuleType { get; set; }

        [Display(Name = "优先级")]
        public int SortLevel { get; set; }

        [Display(Name = "字段")]
        public string ParameterIDN { get; set; }

    }
}

