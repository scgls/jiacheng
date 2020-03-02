using BILWeb.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;

namespace BILWeb.OutStockCreate
{
    public class OutStockCreate_SplitFclRule:OutStockCreate_SplitBaseRule<T_OutStockCreateInfo, T_StockInfo>
    {
        public override void GetOutStockCreateSplitList(ref List<T_OutStockCreateInfo> modelList, List<T_StockInfo> stockList)
        {
            //List<T_StockInfo> materialFclStock = new List<T_StockInfo>();//整箱区库存
            //List<T_StockInfo> materialScatStock = new List<T_StockInfo>();//零拣区库存
            //List<T_OutStockCreateInfo> newModelFclList = new List<T_OutStockCreateInfo>();//数据拆分后新的对象

            //decimal flcQtySum = 0;
            //decimal scatQtySum = 0;
            //decimal boxQty = 0;//库存整箱区箱子个数


            //foreach (var item in modelList)
            //{
            //    T_OutStockCreateInfo modelFcl = new T_OutStockCreateInfo();

            //    //物料零拣区库存
            //    materialScatStock = stockList.FindAll(t => t.MaterialNoID == item.MaterialNoID && t.HouseProp == 2 && t.StrongHoldCode == item.StrongHoldCode && t.WarehouseNo == item.FromErpWareHouse);
            //    //物料整箱区库存
            //    materialFclStock = stockList.FindAll(t => t.MaterialNoID == item.MaterialNoID && t.HouseProp == 1 && t.StrongHoldCode == item.StrongHoldCode && t.WarehouseNo == item.FromErpWareHouse && t.BatchNo == item.FromBatchno);



            //    //整箱区和零拣区都没有库存，优先分配到整箱区
            //    if ((materialFclStock == null || materialFclStock.Count == 0) && (materialScatStock == null || materialScatStock.Count == 0))
            //    {
            //        item.RemainQty = item.OutStockQty;
            //        item.HouseProp = 1;
            //        newModelFclList.Add(item);
            //        continue;
            //    }

            //    //如果物料整箱区没有库存但是零拣区有库存，分配到零拣区 && (materialScatStock != null || materialScatStock.Count >0)
            //    if ((materialFclStock == null || materialFclStock.Count == 0))
            //    {
            //        item.RemainQty = item.OutStockQty;
            //        item.HouseProp = 2;
            //        newModelFclList.Add(item);
            //        continue;
            //    }

            //    //物料包装量为零，直接分配到整箱区
            //    if (item.PackQty == 0)
            //    {
            //        item.RemainQty = item.OutStockQty;
            //        item.HouseProp = 1;
            //        newModelFclList.Add(item);
            //        continue;
            //    }

            //    //物料整箱区总库存
            //    flcQtySum = materialFclStock.Sum(t => t.Qty);
            //    //物料零拣区总库存
            //    scatQtySum = materialScatStock.Sum(t => t.Qty);
            //    //库存整箱区箱子个数，取地板
            //    boxQty = Math.Floor(flcQtySum / item.PackQty);
            //    //订单箱子个数，取地板
            //    item.BoxQty = item.PackQty == 0 || item.RemainQty.ToDecimal() < item.PackQty
            //                    ? 0 : Math.Floor(item.RemainQty.ToDecimal() / item.PackQty);
            //    //订单零拣数量
            //    item.ScatQty = item.PackQty == 0 ? 0 : (item.RemainQty.ToDecimal() % item.PackQty);

            //    //整箱区总库存不能满足订单总数量，拆分2张单子
            //    //整箱区库存数量，订单剩余数量都到零拣区
            //    if (flcQtySum < item.OutStockQty)
            //    {
            //        modelFcl.HouseProp = 1;
            //        modelFcl.RemainQty = flcQtySum;
            //        newModelFclList.Add(GetNewOutStockCreate(item, modelFcl));

            //        T_OutStockCreateInfo modelScat = new T_OutStockCreateInfo();
            //        modelScat.HouseProp = 2;
            //        modelScat.RemainQty = item.OutStockQty - flcQtySum;
            //        newModelFclList.Add(GetNewOutStockCreate(item, modelScat));
            //        continue;
            //    }

            //    //订单整箱数量 <=库存整箱数量，将订单整箱数量分配到整箱区拣货
            //    if (item.BoxQty <= boxQty && item.BoxQty > 0)
            //    {
            //        modelFcl.HouseProp = 1;
            //        modelFcl.RemainQty = item.BoxQty * item.PackQty;
            //        modelFcl.BoxQty = item.BoxQty;
            //        newModelFclList.Add(GetNewOutStockCreate(item, modelFcl));
            //    }
            //    else if (item.BoxQty > boxQty)
            //    {
            //        modelFcl.HouseProp = 1;
            //        modelFcl.RemainQty = boxQty * item.PackQty;
            //        modelFcl.BoxQty = boxQty;
            //        item.ScatQty = item.ScatQty + (item.BoxQty - boxQty) * item.PackQty;
            //        newModelFclList.Add(GetNewOutStockCreate(item, modelFcl));
            //    }

            //    //订单零拣数量 > 0 分配到零拣区
            //    if (item.ScatQty > 0)
            //    {
            //        item.HouseProp = 2;
            //        item.OutStockQty = item.ScatQty;
            //        item.RemainQty = item.ScatQty;
            //        newModelFclList.Add(item);
            //    }
            //}

            //modelList = newModelFclList;
            modelList.ForEach(t => t.RemainQty = t.OutStockQty);
            modelList.ForEach(t => t.HouseProp = 1);

        }

        public T_OutStockCreateInfo GetNewOutStockCreate(T_OutStockCreateInfo model,T_OutStockCreateInfo modelFcl) 
        {
            T_OutStockCreateInfo newModel = new T_OutStockCreateInfo();

            newModel.ErpVoucherNo = model.ErpVoucherNo;
            newModel.MaterialNoID = model.MaterialNoID;
            newModel.StrongHoldCode = model.StrongHoldCode;
            newModel.HeaderID = model.HeaderID;
            newModel.DepartmentCode = model.DepartmentCode;
            newModel.DepartmentName = model.DepartmentName;
            newModel.StrongHoldName = model.StrongHoldName;
            newModel.CompanyCode = model.CompanyCode;
            newModel.VoucherNo = model.VoucherNo;
            newModel.MaterialNo = model.MaterialNo;
            newModel.MaterialDesc = model.MaterialDesc;
            newModel.Unit = model.Unit;
            newModel.OutStockQty = modelFcl.RemainQty;
            newModel.RemainQty = modelFcl.RemainQty;
            newModel.CustomerCode = model.CustomerCode;
            newModel.CustomerName = model.CustomerName;
            newModel.VoucherType = model.VoucherType;
            newModel.MainTypeCode = model.MainTypeCode;
            newModel.FromShipmentDate = model.FromShipmentDate;
            newModel.FromErpWareHouse = model.FromErpWareHouse;
            newModel.ToErpWareHouse = model.ToErpWareHouse;
            newModel.ERPVoucherType = model.ERPVoucherType;
            newModel.ShipNFlg = model.ShipNFlg;
            newModel.ShipDFlg = model.ShipDFlg;
            newModel.ShipPFlg = model.ShipPFlg;
            newModel.ShipWFlg = model.ShipWFlg;
            newModel.TradingConditions = model.TradingConditions;
            newModel.Province = model.Province;
            newModel.City = model.City;
            newModel.Area = model.Area;
            newModel.Address = model.Address;
            newModel.Address1 = model.Address1;
            newModel.Contact = model.Contact;
            newModel.Phone = model.Phone;
            newModel.PackQty = model.PackQty;
            newModel.HouseProp = modelFcl.HouseProp;
            newModel.ERPNote = model.ERPNote;
            newModel.VoucherNo = model.VoucherNo;
            newModel.RowNo = model.RowNo;
            

            return newModel;
        }
    }
}
