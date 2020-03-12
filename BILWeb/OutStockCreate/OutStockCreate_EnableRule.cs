using BILBasic.Basing.Factory;
using BILBasic.XMLUtil;
using BILWeb.PickRule;
using BILWeb.RuleAll;
using BILWeb.StrategeRuleAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Factory;
using BILBasic.Common;
using BILWeb.Stock;

namespace BILWeb.OutStockCreate
{
    //拣货拆分规则
    public class OutStockCreate_EnableRule : StrategeRuleAll<T_OutStockCreateInfo>        
    {
        //启用拣货拆分规则
        public override bool GetOutStockCreateList(ref List<T_OutStockCreateInfo> lstBaseModel,ref string ErrorMsg)
        {
            OutStockCreate_ModelData modelData = new OutStockCreate_ModelData();
            List<T_OutStockCreateInfo> newModelList = new List<T_OutStockCreateInfo>();
            List<T_OutStockCreateInfo> itemList = new List<T_OutStockCreateInfo>();
            List<T_PickRuleInfo> pickRuleMainType = new List<T_PickRuleInfo>();//具体的物料类别规则集合

            PickRule.T_PickRule_DB pickDB = new PickRule.T_PickRule_DB();
            //拣货拆分规则
            List<PickRule.T_PickRuleInfo> pickRuleSplitList = pickDB.GetPearRuleListByPage(RuleAll_Config.RuleTypeSplit).OrderBy(t => t.SortLevel).ToList();

            if (pickRuleSplitList == null || pickRuleSplitList.Count == 0) 
            {
                ErrorMsg = "拣货拆分规则未配置";
                return false;
            }


            //获取生单物料库存
            Stock.T_Stock_DB stockDB = new Stock.T_Stock_DB();
            List<Stock.T_StockInfo> stockList = new List<Stock.T_StockInfo>();

            //汇总本次生单的物料 
            List<T_OutStockCreateInfo> lstMgp = MaterialGroupBy(lstBaseModel);

            //string strMaterialXml = XmlUtil.Serializer(typeof(List<T_OutStockCreateInfo>), lstMgp);

            stockList = stockDB.GetCanStockListByMaterialNoIDToSql(lstMgp);
            //if (stockDB.GetCanStockListByMaterialNoID(strMaterialXml, ref stockList, ref ErrorMsg) == false)
            //{
            //    return false;
            //}

            //根据传入规则类型判断new对象
            RuleAll.t_RuleAll_DB ruleDB = new RuleAll.t_RuleAll_DB();
            List<RuleAll.T_RuleAllInfo> ruleList = ruleDB.GetRuleListByPage(RuleAll_Config.OutStockSumQty);
            int isEnable = ruleList == null ? 0 : ruleList[0].IsEnable.ToInt32();//1-不启用 2-启用

            //启用单据相同物料数量汇总
            if (isEnable == 2)
            {
                lstBaseModel = OutStockSameMaterialNoSumQty(lstBaseModel);
            }
            

            //把要生单的数据根据物料类别分组
            var mainTypeCodeList =  from t in lstBaseModel group t by new { t1 = t.MainTypeCode} into m 
                                    select new {MainTypeCode = m.Key.t1};

            foreach (var item in mainTypeCodeList)
            {
                //获取具体的物料类别规则
                pickRuleMainType = pickRuleSplitList.FindAll(t => t.MaterialClassCode == item.MainTypeCode).OrderBy(t => t.SortLevel).ToList();

                foreach (var item1 in pickRuleMainType)
                {
                    //循环开始，默认按照优先级升序
                    //如果第一个优先级是零散区，先按照零散区拆分
                    //循环第二个优先级，假设是楼层，根据零散区的结果按照楼层拆分
                    //循环结束

                    //循环开始，默认按照优先级升序
                    //如果第一个优先级是楼层，先按照楼层拆分
                    //循环第二个优先级，假设是零散区，根据楼层的结果按照零散区拆分
                    //循环结束
                    Object obj = WMS.Factory.ServiceFactory.CreateObject(item1.ParameterIDN);

                    OutStockCreate_SplitContext<T_OutStockCreateInfo, Stock.T_StockInfo> context
                        = new OutStockCreate_SplitContext<T_OutStockCreateInfo, Stock.T_StockInfo>
                        ((OutStockCreate_SplitBaseRule<T_OutStockCreateInfo, Stock.T_StockInfo>)obj);
                    context.GetOutStockCreateSplitList(ref lstBaseModel, stockList);
                }
            }

            lstBaseModel = modelData.GetNewOutStockModelList(lstBaseModel);

            return true;

        }

        private void StockGroupBy(List<PickRule.T_PickRuleInfo> pickRuleList, ref List<Stock.T_StockInfo> stockList)
        {
            
                var stockGroupList = from t in stockList
                                     group t by new
                                     {
                                         t1 = t.WareHouseID,
                                         t2 = t.HouseID,                                         
                                         t4 = t.MaterialNoID,
                                         t5 = t.StrongHoldCode
                                     } into m
                                     select new
                                     {
                                         WareHouseID = m.Key.t1,
                                         HouseID = m.Key.t2,                                        
                                         MaterialNoID = m.Key.t4,
                                         StrongHoldCode = m.Key.t5,
                                         Qty = m.Sum(p => p.Qty),
                                         HouseProp = m.FirstOrDefault().HouseProp,
                                         FloorType = m.FirstOrDefault().FloorType
                                     };
                stockList = (List<Stock.T_StockInfo>)stockGroupList;
            

        }

        /// <summary>
        /// 生成拣货单，启用单据相同物料数量汇总
        /// </summary>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public  List<T_OutStockCreateInfo> OutStockSameMaterialNoSumQty( List<T_OutStockCreateInfo> modelList)
        {

            var newModelList = from t in modelList
                                 group t by new
                                 {
                                     t1 = t.ErpVoucherNo,                                     
                                     t2 = t.MaterialNoID,
                                     t3 = t.StrongHoldCode,
                                     t4 = t.StrongHoldName,
                                     t5 = t.CompanyCode,
                                     t6 = t.FromErpWareHouse,
                                     t7 = t.HeaderID,
                                     t8 = t.FromBatchno,

                                 } into m
                               select new T_OutStockCreateInfo
                                 {
                                     ErpVoucherNo = m.Key.t1,
                                     MaterialNoID = m.Key.t2,
                                     StrongHoldCode = m.Key.t3,     
                                     StrongHoldName = m.Key.t4,
                                     CompanyCode = m.Key.t5,
                                     FromErpWareHouse = m.Key.t6,
                                     ToErpWareHouse = m.FirstOrDefault().ToErpWareHouse,
                                     HeaderID = m.Key.t7,
                                     DepartmentCode = m.FirstOrDefault().DepartmentCode,
                                     DepartmentName = m.FirstOrDefault().DepartmentName,
                                     VoucherNo = m.FirstOrDefault().VoucherNo,
                                     MaterialNo = m.FirstOrDefault().MaterialNo,
                                     MaterialDesc = m.FirstOrDefault().MaterialDesc,
                                     Unit = m.FirstOrDefault().Unit,
                                     OutStockQty = m.Sum(p => p.OutStockQty),
                                     RemainQty = m.Sum(p => p.RemainQty),
                                     CustomerCode = m.FirstOrDefault().CustomerCode,
                                     CustomerName = m.FirstOrDefault().CustomerName,
                                     VoucherType = m.FirstOrDefault().VoucherType,
                                     MainTypeCode = m.FirstOrDefault().MainTypeCode,
                                     FromShipmentDate = m.FirstOrDefault().FromShipmentDate,                                     
                                     ERPVoucherType = m.FirstOrDefault().ERPVoucherType,
                                     ShipNFlg = m.FirstOrDefault().ShipNFlg,
                                     ShipDFlg = m.FirstOrDefault().ShipDFlg,
                                     ShipPFlg = m.FirstOrDefault().ShipPFlg,
                                     ShipWFlg = m.FirstOrDefault().ShipWFlg,
                                     TradingConditions = m.FirstOrDefault().TradingConditions,
                                     Province = m.FirstOrDefault().Province,
                                     City = m.FirstOrDefault().City,
                                     Area = m.FirstOrDefault().Area,
                                     Address = m.FirstOrDefault().Address,
                                     Address1 = m.FirstOrDefault().Address1,
                                     Contact = m.FirstOrDefault().Contact,
                                     Phone = m.FirstOrDefault().Phone,
                                     PackQty = m.FirstOrDefault().PackQty,
                                     ERPNote = m.FirstOrDefault().ERPNote,
                                     RowNo = m.FirstOrDefault().RowNo,
                                     RowNoDel = m.FirstOrDefault().RowNoDel,
                                     VouUser = m.FirstOrDefault().VouUser,
                                     FromBatchno = m.Key.t8,
                                     IsSpcBatch = m.FirstOrDefault().IsSpcBatch
                                 };

            return newModelList.ToList();
        }

        private List<T_OutStockCreateInfo> MaterialGroupBy(List<T_OutStockCreateInfo> modelList) 
        {            
            //汇总本次生单的物料
            var newModelList = from t in modelList
                               group t by new { t1 = t.MaterialNoID  } into m
                               select new T_OutStockCreateInfo
                               {
                                   MaterialNoID = m.Key.t1
                                   
                               };


            return newModelList.ToList();

        }
        
    }
}
