using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BILBasic.Basing.Factory;

namespace BILWeb.StrategeRuleAll
{
    //定义使用到规则的虚方法类
    public abstract class StrategeRuleAll<T>
    {
        //根据拣货拆分规则得到的数据
        public virtual bool GetOutStockCreateList(ref List<T> modelList, ref string ErrorMsg) 
        {
            return true;
        }

        //根据拣货分配规则得到的数据
        public virtual void GetOutStockSlotList(ref List<T> modelList) { }

        //获取拣货单明细数据
        //启用拣货规则需要按照规则（比如批次先进先出）拆分数据
        //不启用拣货规则原单返回
        public virtual bool GetOutStockTaskDetailPickList(ref List<T> modelList, ref string strError) 
        {
            return true;
        }

        /// <summary>
        ///下架扫描，根据条码获取库存        
        /// </summary>
        /// <param name="T">
        /// T 对应库存类型
        /// 启用序列号，截取条码最后一个字段
        /// 不启用序列号，根据管理维度动态截取
        /// </param>
        /// <returns>库存JSON</returns>
        public virtual bool GetStockByBarCode(T model, ref List<T> modelList,ref string strError)
        {
            return false;
        }

      
    

    }
}
