using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.RuleAll
{
    public class RuleAll_Config
    {
        #region 规则是否启用配置

        /// <summary>
        /// 不启用配置项规则
        /// </summary>
        public const int RuleAllUnEnable = 1;

        /// <summary>
        /// 启用配置项规则
        /// </summary>
        public const int RuleAllEnable = 2;

        #endregion

        #region 查找拣货规则配置项对应表t_Ruleall中CONITEMID字段

        /// <summary>
        /// 序列号配置项指定常量
        /// </summary>
        public const int SerialItem = 3;

        /// <summary>
        /// 拣货拆分 配置项指定常量
        /// </summary>
        public const int OutStockSplitItem = 4;

        /// <summary>
        /// 拣货分配 配置项指定常量
        /// </summary>
        public const int OutStockSlotItem = 6;

        /// <summary>
        /// 拣货规则 配置项指定常量
        /// </summary>
        public const int OutStockPickItem = 5;

        /// <summary>
        /// 单据相同物料数量汇总
        /// </summary>
        public const int OutStockSumQty = 15;

        #endregion

        #region 查找某个拣货规则配置项对应表t_Pickrule中RULETYPE字段

        /// <summary>
        /// 具体拣货规则类型-拣货拆分类型
        /// </summary>
        public const int RuleTypeSplit = 1;

        /// <summary>
        /// 具体拣货规则类型-拣货类型
        /// </summary>
        public const int RuleTypePick = 2;


        #endregion


    }
}
