using BILBasic.XMLUtil;
using BILWeb.RuleAll;
using BILWeb.Stock;
using BILWeb.StrategeRuleAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;

namespace BILWeb.OutStockTask
{
    public class OutStockTask_EnablePickRule : StrategeRuleAll<T_OutStockTaskDetailsInfo>
    {
        public override bool GetOutStockTaskDetailPickList(ref List<T_OutStockTaskDetailsInfo> modelList, ref string strError)
        {
            PickRule.T_PickRule_DB pickDB = new PickRule.T_PickRule_DB();
            //拣货拆分规则
            List<PickRule.T_PickRuleInfo> pickRuleSplitList = pickDB.GetPearRuleListByPage(RuleAll_Config.RuleTypePick).OrderBy(t => t.SortLevel).ToList();

            if (pickRuleSplitList == null || pickRuleSplitList.Count == 0)
            {
                strError = "拣货规则未配置";
                return false;
            }

            //获取生单物料库存
            Stock.T_Stock_DB stockDB = new Stock.T_Stock_DB();
            List<Stock.T_StockInfo> stockList = new List<Stock.T_StockInfo>();

            //汇总本次生单的物料 
            List<T_OutStockTaskDetailsInfo> lstMgp = MaterialGroupBy(modelList);
            //var newModelList = from t in modelList
            //                   group t by new { t1 = t.MaterialNoID } into m
            //                   select new T_OutStockTaskDetailsInfo
            //                   {
            //                       MaterialNoID = m.Key.t1
            //                   };

            stockList = stockDB.GetCanStockListByMaterialNoIDToSql(lstMgp);

            

            //所有物料都没有库存，不拆分，直接返回
            if (stockList == null || stockList.Count == 0)
            {
                modelList.ForEach(t => t.RePickQty = 0);
                //strError = "获取订单明细所有物料可用库存为零！";
                //return false;
            }
            else 
            {
                int houseProp = modelList[0].HouseProp;
                //物料有库存，先按照整箱区和零拣区过滤
                stockList = stockList.Where(t => t.HouseProp == houseProp).ToList();

                if (stockList == null || stockList.Count == 0)
                {
                    modelList.ForEach(t => t.RePickQty = 0);
                }
                else 
                {
                    //需要按照库位拆分库存
                    modelList = CreateNewListByPickRuleAreaNo(modelList, stockList);
                }                
            }
                       

            
            return true;
            
        }


        public List<T_OutStockTaskDetailsInfo> CreateNewListByPickRuleAreaNo( List<T_OutStockTaskDetailsInfo> modelList, List<T_StockInfo> stockList)
        {
            List<T_OutStockTaskDetailsInfo> NewModelList = new List<T_OutStockTaskDetailsInfo>();
            List<T_StockInfo> stockModelList = new List<T_StockInfo>();
            List<T_StockInfo> stockModelListSum = new List<T_StockInfo>();
            string strAreaNo = string.Empty;

            foreach (var item in modelList)
            {                
                //查找物料可分配库存
                stockModelList = stockList.FindAll(t => t.MaterialNoID == item.MaterialNoID && t.StrongHoldCode==item.StrongHoldCode && t.WarehouseNo==item.FromErpWarehouse && t.HouseProp==item.HouseProp && t.Qty>0 && t.BatchNo==item.FromBatchNo ).OrderBy(t=>t.EDate).OrderBy(t=>t.SortArea).ToList();
                stockModelListSum = CreateNewStockInfoSum(stockModelList);

                if (stockModelListSum != null && stockModelListSum.Count > 0) 
                {
                    strAreaNo = string.Empty;
                    foreach (var itemArea in stockModelListSum) 
                    {
                        strAreaNo += itemArea.AreaNo + "|";
                    }
                    item.AreaNo = strAreaNo.TrimEnd('|');
                }
                //暂时屏蔽，拆分推荐库位
                //foreach (var stockModel in stockModelListSum)
                //{
                //    if (item.RemainQty < stockModel.Qty)
                //    {
                //        item.RePickQty = item.RemainQty;
                //        stockModel.Qty = stockModel.Qty - item.RemainQty.ToDecimal();
                //        NewModelList.Add(CreateNewOutStockModelToADF(stockModel, item));
                //        break;
                //    }
                //    else if (item.RemainQty == stockModel.Qty)
                //    {
                //        item.RePickQty = stockModel.Qty;
                //        stockModel.Qty = 0;
                //        NewModelList.Add(CreateNewOutStockModelToADF(stockModel, item));
                //        break;
                //    }
                //    else if (item.RemainQty > stockModel.Qty)
                //    {
                //        item.RePickQty = stockModel.Qty;
                //        item.RemainQty = item.RemainQty - stockModel.Qty;
                //        stockModel.Qty = 0;
                //        NewModelList.Add(CreateNewOutStockModelToADF(stockModel, item));
                //    }
                //}

            }

            ///暂时屏蔽，代码有用，拆分推荐库位有用
            //if (NewModelList == null || NewModelList.Count == 0)
            //{
            //    modelList.ForEach(t => t.RePickQty=0);
            //    return modelList;
            //}
            //else 
            //{
            //    return NewModelList.OrderBy(t => t.FloorType).OrderBy(t => t.SortArea).ToList();
            //}
            modelList.ForEach(t => t.ToBatchNo = t.FromBatchNo);
            modelList.ForEach(t => t.BatchNo = t.FromBatchNo);
            return modelList;

        }


        private T_OutStockTaskDetailsInfo CreateNewOutStockModelToADF(T_StockInfo stockModel, T_OutStockTaskDetailsInfo itemModel)
        {
            T_OutStockTaskDetailsInfo model = new T_OutStockTaskDetailsInfo();
            model.ID = itemModel.ID;
            model.HeaderID = itemModel.HeaderID;
            model.CompanyCode = itemModel.CompanyCode;
            model.StrongHoldCode = itemModel.StrongHoldCode;
            model.StrongHoldName = itemModel.StrongHoldName;
            model.MaterialNo = itemModel.MaterialNo;
            model.MaterialDesc = itemModel.MaterialDesc;
            model.MaterialNoID = itemModel.MaterialNoID;
            model.ErpVoucherNo = itemModel.ErpVoucherNo;
            model.VoucherNo = itemModel.VoucherNo;
            model.TaskNo = itemModel.TaskNo;
            model.TaskQty = itemModel.TaskQty;
            model.RemainQty = itemModel.RemainQty;
            model.RePickQty = itemModel.RePickQty;
            model.AreaNo = stockModel.AreaNo;
            model.HeightArea = itemModel.HeightArea;
            model.FloorType = stockModel.FloorType;
            model.IsSpcBatch = itemModel.IsSpcBatch;
            model.ScanQty = itemModel.ScanQty;
            model.FromBatchNo = itemModel.FromBatchNo;
            model.FromErpAreaNo = itemModel.FromErpAreaNo;
            model.FromErpWarehouse = itemModel.FromErpWarehouse;
            model.ToBatchNo = stockModel.BatchNo;//itemModel.ToBatchNo
            model.ToErpAreaNo = itemModel.ToErpAreaNo;
            model.ToErpWarehouse = itemModel.ToErpWarehouse;
            model.TaskType = itemModel.TaskType;
            model.DepartmentCode = itemModel.DepartmentCode;
            model.DepartmentName = itemModel.DepartmentName;
            model.EDate = itemModel.EDate;
            model.PickGroupNo = itemModel.PickGroupNo;
            model.PickLeaderUserNo = itemModel.PickLeaderUserNo;
            model.Status = itemModel.Status;
            model.StrStatus = itemModel.StrStatus;
            model.StrVoucherType = itemModel.StrVoucherType;
            model.StockQty = stockModel.Qty;
            model.SortArea = stockModel.SortArea;
            model.ERPVoucherType = itemModel.ERPVoucherType;
            model.Unit = itemModel.Unit;
            model.BatchNo = stockModel.BatchNo;
            model.HouseProp = itemModel.HouseProp;
            model.RowNo = itemModel.RowNo;
                       
            return model;
        }

        private List<T_OutStockTaskDetailsInfo> MaterialGroupBy(List<T_OutStockTaskDetailsInfo> modelList)
        {
            //汇总本次生单的物料
            var newModelList = from t in modelList
                               group t by new { t1 = t.MaterialNoID } into m
                               select new T_OutStockTaskDetailsInfo
                               {
                                   MaterialNoID = m.Key.t1
                               };


            return newModelList.ToList();

        }

        /// <summary>
        /// 在库位推荐的时候需要把相同库位，物料，批次，效期，组织，汇总库存数量
        /// </summary>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public List<T_StockInfo> CreateNewStockInfoSum(List<T_StockInfo> modelList) 
        {
            var newModelList = from t in modelList
                               group t by new
                               {
                                   t1 = t.MaterialNoID,
                                   t2 = t.AreaNo,
                                   t3 = t.StrongHoldCode,
                                   t4 = t.StrongHoldName,
                                   t5 = t.CompanyCode,
                                   t6 = t.BatchNo,
                                   t7 = t.EDate
                               } into m
                               select new T_StockInfo
                               {
                                   MaterialNoID = m.Key.t1,
                                   AreaNo = m.Key.t2,
                                   StrongHoldCode = m.Key.t3,
                                   StrongHoldName = m.Key.t4,
                                   CompanyCode = m.Key.t5,
                                   BatchNo = m.Key.t6,
                                   EDate = m.Key.t7,
                                   Qty = m.Sum(p => p.Qty),
                                   FloorType = m.FirstOrDefault().FloorType,
                                   SortArea = m.FirstOrDefault().SortArea
                               };

            return newModelList.ToList();

        }
    }
}
