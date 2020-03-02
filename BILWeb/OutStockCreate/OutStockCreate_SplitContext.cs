using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.OutStockCreate
{
    public class OutStockCreate_SplitContext<T,T1>
    {
        private OutStockCreate_SplitBaseRule<T,T1> splitBase;

        public OutStockCreate_SplitContext(OutStockCreate_SplitBaseRule<T, T1> splitBase) 
        {
            this.splitBase = splitBase;
        }

        public void GetOutStockCreateSplitList(ref List<T> modelList,List<T1> modelList1)
        {
            splitBase.GetOutStockCreateSplitList(ref modelList,modelList1);
        }

        public void GetOutStockCreateAutoSlotList(ref List<T> modelList) 
        {
            splitBase.GetOutStockCreateAutoSlotList(ref modelList);
        }

    }
}
