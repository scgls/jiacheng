using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.OutStockCreate
{
    public class OutStockCreate_ModelData
    {

        public List<T_OutStockCreateInfo> GetNewOutStockModelList(List<T_OutStockCreateInfo> modelList)
        {
            List<T_OutStockCreateInfo> newModelList = new List<T_OutStockCreateInfo>();
            List<T_OutStockCreateInfo> itemList = new List<T_OutStockCreateInfo>();
            //没有启用拣货分配规则,按照订单，组织来生成
            var groupList = from t in modelList
                            group t by new { t1 = t.StrongHoldCode, t2 = t.ErpVoucherNo,t3 = t.HouseProp,t4 = t.FromErpWareHouse } into m
                            select new
                            {
                                StrongHoldCode = m.Key.t1,
                                ErpVoucherNo = m.Key.t2,
                                HouseProp = m.Key.t3,
                                FromErpWareHouse =m.Key.t4
                            };

            foreach (var item in groupList) 
            {
                itemList = modelList.FindAll(t => t.StrongHoldCode == item.StrongHoldCode && t.ErpVoucherNo == item.ErpVoucherNo && 
                    t.HouseProp==item.HouseProp && t.FromErpWareHouse == item.FromErpWareHouse);
                newModelList.Add(CreateNewOutStockModelList(itemList));
            }

            return newModelList;
            
        }

        public T_OutStockCreateInfo CreateNewOutStockModelList(List<T_OutStockCreateInfo> itemList)
        {
            T_OutStockCreateInfo model = new T_OutStockCreateInfo();
            model.lstOutStockCreateInfo = new List<T_OutStockCreateInfo>();
            model.VoucherType = itemList[0].VoucherType;
            model.TaskType = 2;
            model.Status = 1;
            model.InStockID = itemList[0].HeaderID;
            model.ErpVoucherNo = itemList[0].ErpVoucherNo;
            model.SupCusCode = itemList[0].CustomerCode;
            model.SupCusName = itemList[0].CustomerName;
            model.StrongHoldCode = itemList[0].StrongHoldCode;
            model.StrongHoldName = itemList[0].StrongHoldName;
            model.CompanyCode = itemList[0].CompanyCode;
            model.DepartmentCode = itemList[0].DepartmentCode;
            model.DepartmentName = itemList[0].DepartmentName;
            model.ERPStatus = itemList[0].ERPStatus;
            model.VouDate = itemList[0].VouDate;
            model.VouUser = itemList[0].VouUser;
            model.IsDel = itemList[0].IsDel;
            model.ERPVoucherType = itemList[0].ERPVoucherType;
            model.FloorType = itemList[0].FloorType;
            model.HouseProp = itemList[0].HouseProp;
            model.ShipDFlg = itemList[0].ShipDFlg;
            model.ShipNFlg = itemList[0].ShipNFlg;
            model.ShipPFlg = itemList[0].ShipPFlg;
            model.ShipWFlg = itemList[0].ShipWFlg;
            model.Address = itemList[0].Address;
            model.TradingConditions = itemList[0].TradingConditions;
            model.Contact = itemList[0].Contact;
            model.Phone = itemList[0].Phone;
            model.Address1 = itemList[0].Address1;
            model.Province = itemList[0].Province;
            model.City = itemList[0].City;
            model.Area = itemList[0].Area;
            model.HeaderID = itemList[0].HeaderID;
            model.VoucherNo = itemList[0].VoucherNo;
            model.ERPNote = itemList[0].ERPNote;

            foreach (var item1 in itemList)
            {
                T_OutStockCreateInfo detail = new T_OutStockCreateInfo();

                detail.MaterialNo = item1.MaterialNo;
                detail.MaterialDesc = item1.MaterialDesc;
                detail.RemainQty = item1.RemainQty;
                detail.OutStockQty = item1.OutStockQty;
                detail.LineStatus = 1;
                detail.RowNo = item1.RowNo;
                detail.RowNoDel = item1.RowNoDel;
                detail.Unit = item1.Unit;
                detail.ErpVoucherNo = item1.ErpVoucherNo;
                detail.MaterialNoID = item1.MaterialNoID;
                detail.StrongHoldCode = item1.StrongHoldCode;
                detail.StrongHoldName = item1.StrongHoldName;
                detail.CompanyCode = item1.CompanyCode;                
                detail.IsDel = item1.IsDel;
                detail.IsSpcBatch = item1.IsSpcBatch;
                detail.FromBatchno = item1.FromBatchno;
                detail.FromErpAreaNo = item1.FromErpAreaNo;
                detail.FromErpWareHouse = item1.FromErpWareHouse;
                detail.ToBatchno = item1.ToBatchno;
                detail.ToErpAreaNo = item1.ToErpAreaNo;
                detail.ToErpWareHouse = item1.ToErpWareHouse;
                detail.OutstockDetailid = item1.ID;
                detail.ProductNo = item1.ProductNo;                
                detail.ID = item1.ID;
                model.lstOutStockCreateInfo.Add(detail);
            }

            return model;
        }
    }
}
