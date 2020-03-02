using BILWeb.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;

namespace BILWeb.OutStockCreate
{
    //楼层
    public class OutStockCreate_SplitFloorRule : OutStockCreate_SplitBaseRule<T_OutStockCreateInfo, Stock.T_StockInfo>
    {
        public override void GetOutStockCreateSplitList(ref List<T_OutStockCreateInfo> modelList, List<Stock.T_StockInfo> stockList)
        {

            List<T_OutStockCreateInfo> newModelFloorList = new List<T_OutStockCreateInfo>();

            List<T_StockInfo> newStockList = new List<T_StockInfo>();
            //根据楼层汇总库存数量
            newStockList = StockGroupBy(stockList);

            List<T_StockInfo> itemFloorList = new List<T_StockInfo>();

            foreach (var item in modelList)
            {
                T_OutStockCreateInfo modelFloor = new T_OutStockCreateInfo();

                //单个物料分布楼层库存数量
                itemFloorList = stockList.FindAll(t => t.MaterialNoID == item.MaterialNoID
                    && t.StrongHoldCode == item.StrongHoldCode && t.Qty >0 ).OrderBy(t=>t.FloorType).ToList();

                foreach (var itemFloor in itemFloorList) 
                {
                    if (item.RemainQty <= itemFloor.Qty) 
                    {
                        itemFloor.Qty = itemFloor.Qty - item.RemainQty.ToDecimal();
                        item.FloorType = itemFloor.FloorType;
                        newModelFloorList.Add(item);
                    }
                    else if (item.RemainQty > itemFloor.Qty) 
                    {
                        itemFloor.Qty = 0;
                        modelFloor = item;
                        modelFloor.FloorType = itemFloor.FloorType;
                        modelFloor.RemainQty = item.RemainQty - itemFloor.Qty;
                        newModelFloorList.Add(modelFloor);
                    }
                }
            }

            modelList = newModelFloorList;

        }


        private List<Stock.T_StockInfo> StockGroupBy(List<Stock.T_StockInfo> stockList)
        {
            var stockGroupList = from t in stockList
                                 group t by new
                                 {
                                     t1 = t.WareHouseID,
                                     t2 = t.FloorType,
                                     t3 = t.MaterialNoID,
                                     t4 = t.StrongHoldCode
                                 } into m
                                 select new
                                 {
                                     WareHouseID = m.Key.t1,
                                     FloorType = m.Key.t2,
                                     MaterialNoID = m.Key.t3,
                                     StrongHoldCode = m.Key.t4,
                                     Qty = m.Sum(p => p.Qty)
                                 };

            return (List<Stock.T_StockInfo>)stockGroupList;


        }

    }
}
