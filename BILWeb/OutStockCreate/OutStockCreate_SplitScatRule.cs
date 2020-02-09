using BILWeb.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;

namespace BILWeb.OutStockCreate
{
    //零散区
    public class OutStockCreate_SplitScatRule : OutStockCreate_SplitBaseRule<T_OutStockCreateInfo,Stock.T_StockInfo>
    {
        public override void GetOutStockCreateSplitList(ref List<T_OutStockCreateInfo> modelList,List<Stock.T_StockInfo> stockList)
        {           
            List<T_OutStockCreateInfo> newModelScatList = new List<T_OutStockCreateInfo>();

            List<T_StockInfo> newStockList = new List<T_StockInfo>();
            //根据零拣区汇总库存数量
            newStockList = StockGroupBy(stockList);


            T_StockInfo itemScatModel = new T_StockInfo();            

            foreach (var item in modelList)
            {
                T_OutStockCreateInfo modelScat = new T_OutStockCreateInfo();
                
                //单个物料零散区总库存数量
                itemScatModel = stockList.Find(t => t.MaterialNoID == item.MaterialNoID
                    && t.StrongHoldCode == item.StrongHoldCode && t.HouseProp == 2 && t.Qty>0);

                //零拣区没有库存，直接到整箱区
                if (itemScatModel == null)
                {
                    item.HouseProp = 1;
                    newModelScatList.Add(item);
                    continue;
                }

                if (item.RemainQty.ToDecimal() <= itemScatModel.Qty)
                {
                    itemScatModel.Qty = itemScatModel.Qty - item.RemainQty.ToDecimal();
                    item.HouseProp = itemScatModel.HouseProp;
                    newModelScatList.Add(item);
                }
                else if (item.RemainQty > itemScatModel.Qty)
                {
                    itemScatModel.Qty = 0;
                    modelScat = item;
                    modelScat.HouseProp = itemScatModel.HouseProp;
                    modelScat.RemainQty = item.RemainQty - itemScatModel.Qty;
                    newModelScatList.Add(modelScat);
                    //剩余的拣货数量拆分到整箱区
                    T_OutStockCreateInfo modelPack = new T_OutStockCreateInfo();
                    modelPack = item;
                    modelPack.HouseProp = 1;
                    modelPack.RemainQty = item.RemainQty - modelScat.RemainQty;
                    newModelScatList.Add(modelPack);
                }
            }

            modelList = newModelScatList;
        }

      
        //按照零拣区和整箱区汇总库存数量
        private List<Stock.T_StockInfo> StockGroupBy( List<Stock.T_StockInfo> stockList)
        {

                var stockGroupList = from t in stockList
                                     group t by new
                                     {
                                         t1 = t.WareHouseID,
                                         t2 = t.HouseProp,                                         
                                         t3 = t.MaterialNoID,
                                         t4 = t.StrongHoldCode                                         
                                     } into m
                                     select new
                                     {
                                         WareHouseID = m.Key.t1,
                                         HouseProp = m.Key.t2,                                         
                                         MaterialNoID = m.Key.t3,
                                         StrongHoldCode = m.Key.t4,
                                         Qty = m.Sum(p => p.Qty)                                                                                
                                     };

                return (List<Stock.T_StockInfo>)stockGroupList;
            

        }
    }
}
