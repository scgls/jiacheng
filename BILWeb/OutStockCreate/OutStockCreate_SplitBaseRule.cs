using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.OutStockCreate
{
    //具体拆分规则抽象类
    public abstract class OutStockCreate_SplitBaseRule<T,T1>
    {
        public virtual void GetOutStockCreateSplitList(ref List<T> modelList, List<T1> modelList1) { }

        public virtual void GetOutStockCreateAutoSlotList(ref List<T> modelList) { }

    }
}
