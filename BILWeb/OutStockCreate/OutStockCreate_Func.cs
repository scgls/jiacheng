using BILBasic.Basing.Factory;
using BILWeb.PickRule;
using BILWeb.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILWeb.UserGroup;
using BILWeb.StrategeRuleAll;
using BILWeb.RuleAll;


namespace BILWeb.OutStockCreate
{
    public partial class T_OutStockCreate_Func : TBase_Func<T_OutStockCreate_DB, T_OutStockCreateInfo>, IOutStockCreateService
    {
        T_OutStockCreate_DB _db = new T_OutStockCreate_DB();
        protected override bool CheckModelBeforeSave(T_OutStockCreateInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }
            return true;
        }


        protected override bool CheckModelBeforeSave(List<T_OutStockCreateInfo> modelList, ref string strError)
        {
            if (modelList == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (modelList == null || modelList.Count == 0) 
            {
                strError = "提交生单数据为空，请确认是否已经生成拣货单！";
                return false;
            }

            //modelList = modelList.Where(t => t.OKSelect == true).ToList();

            //if (modelList.Where(t => t.OKSelect == true).Count() == 0)
            //{
            //    strError = "请先勾选生成拣货单行！";
            //    return false;
            //}

            ////库存数量为零的不能生成下架任务
            //if (modelList.Where(t => t.StockQty > 0).Count() == 0)
            //{
            //    strError = "勾选的单据行库存数量都为零，不能生成拣货单！";
            //    return false;
            //}

            //if (modelList.Where(t => t.StockQty == 0).Count() > 0)
            //{
            //    strError = "勾选的单据行存在库存为零的物料，不能生成拣货单！";
            //    return false;
            //}

            ////modelList = modelList.Where(t => t.StockQty > 0).ToList();

            //if (CheckStrongholdIsSame(modelList, ref strError) == false)
            //{
            //    return false;
            //}

            if (CheckVoucherTypeIsSame(modelList, ref strError) == false)
            {
                return false;
            }

            //查看单据行是否已经生成了下架任务
            if (_db.CheckCreatePickBefore(modelList, ref strError) == false)
            {
                return false;
            }

            //查看库存数量
            //if (_db.CheckOutStockCreate(modelList, ref strError) == false)
            //{
            //    return false;
            //}


            //if (GetMaterialFloorForPickRule(ref modelList, ref strError) == false)
            //{
            //    return false;
            //}


            
            return true;
        }

        protected override bool GetRuleModelBeforeSave(ref List<T_OutStockCreateInfo> modelList, ref string strError)
        {
            //拣货拆分规则
            Context<T_OutStockCreateInfo> context = new Context<T_OutStockCreateInfo>(RuleAll_Config.OutStockSplitItem);
            if (context.GetOutStockCreateList(ref modelList, ref strError) == false)
            {
                return false;
            }

            //拣货任务分配规则
            //Context<T_OutStockCreateInfo> contextSlot = new Context<T_OutStockCreateInfo>(RuleAll_Config.OutStockSlotItem);
            //contextSlot.GetOutStockSlotList(ref modelList);

            return true;

        }


        /// <summary>
        /// 判断据点是否相同
        /// </summary>
        /// <returns></returns>
        private bool CheckStrongholdIsSame(List<T_OutStockCreateInfo> modelList, ref string strError) 
        {
            var groupList = from t in modelList
                            group t by new { t1 = t.StrongHoldCode } into m
                            select new
                            {
                                StrongHoldCode = m.Key.t1
                            };

            if (groupList.Count() > 1)
            {
                strError = "输入的据点不同，不能同时合并生成下架任务！";
                return false;
            }

            return true;

        }

        /// <summary>
        /// 判断单据类型是否相同
        /// </summary>
        /// <returns></returns>
        private bool CheckVoucherTypeIsSame(List<T_OutStockCreateInfo> modelList, ref string strError)
        {
            var groupList = from t in modelList
                            group t by new { t1 = t.VoucherType } into m
                            select new
                            {
                                VoucherType = m.Key.t1
                            };

            if (groupList.Count() > 1)
            {
                strError = "输入的单据类型不同，不能同时合并生成下架任务！";
                return false;
            }

            return true;

        }

        #region 根据WMS拣货规则以及ERP指定批次获取楼层


        private bool GetMaterialFloorForPickRule(ref List<T_OutStockCreateInfo> modelList,ref string strError)
        {           
            List<T_PickRuleInfo> _pickRuleList = new List<T_PickRuleInfo>();
            List<T_UserGroupInfo> _userGroupList = new List<T_UserGroupInfo>();

            //获取拣货规则
            GetAllPickRule(ref _pickRuleList, ref strError);            

            //获取拣货主管
            if (GetPickLaderForMaintypeCode(ref _userGroupList, ref strError) == false) 
            {
                return false;
            }

            return GetFloorForModelList(ref modelList, _pickRuleList,_userGroupList, ref strError);
        }


        /// <summary>
        /// 获取物料分类对应主管的用户组编码
        /// </summary>
        /// <returns></returns>
        public bool GetPickLaderForMaintypeCode(ref List<T_UserGroupInfo> modelList,ref string strError) 
        {
            try
            {
                T_UserGroup_DB _db = new T_UserGroup_DB();
                modelList = _db.GetPickLaderForMaintypeCode();

                if (modelList == null || modelList.Count == 0) 
                {
                    strError = "请先配置拣货主管！";
                    return false;
                }

                return true;

            }
            catch (Exception ex) 
            {
                strError = ex.Message;
                return false;
            }
        }


        //获取拣货规则
        private bool GetAllPickRule(ref List<T_PickRuleInfo> modelList, ref string strError)
        {
            try
            {
                T_PickRule_DB _db = new T_PickRule_DB();
                modelList = _db.GetAllPickRule();

                if (modelList == null || modelList.Count == 0)
                {
                    strError = "未配置拣货规则！";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }


        }


        //获取楼层编号
        private bool GetFloorByMaterialNoID(int MaterialNoID, string BatchNo, int PickRule, ref T_StockInfo model, ref string strError, string StrongholdCode,string WareHouseNo)
        {
            try
            {                
                T_Stock_DB _db = new T_Stock_DB();
                model = _db.GetFloorForStock(MaterialNoID, BatchNo, PickRule, StrongholdCode, WareHouseNo);

                if (model.FloorType == 0)
                {
                    strError = "请先设置货位" + model.AreaNo + "对应的楼层编号！";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }

        }

        /// <summary>
        /// 获取物料根据拣货规则以及批次对应的楼层，以及物料分类对应的主管
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="pickRuleModelList"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        private bool GetFloorForModelList(ref List<T_OutStockCreateInfo> modelList, List<T_PickRuleInfo> pickRuleModelList,List<T_UserGroupInfo> userGroupList, ref string strError)
        {
            bool bSucc = false;
            string strBatchNo = string.Empty;
            int pickRule = 0;            
            string PickLeaderUserNo = string.Empty;
            string PickGroupNo = string.Empty;
            T_PickRuleInfo _pickModel = new T_PickRuleInfo();
            T_UserGroupInfo _userGroupModel = new T_UserGroupInfo();
            T_StockInfo _stockModel = new T_StockInfo();

            foreach (var item in modelList)
            {
                strBatchNo = item.IsSpcBatch == "Y" ? item.FromBatchno : string.Empty;

                _pickModel = pickRuleModelList == null ? null : pickRuleModelList.Find(t => t.MaterialClassCode == item.MainTypeCode);

                if (_pickModel != null) 
                {
                    pickRule = _pickModel.PickRuleCode;
                }

                _userGroupModel = userGroupList == null ? null: userGroupList.FindLast(t => t.MainTypeCode == item.MainTypeCode && t.WarehouseNo.Contains(item.FromErpWareHouse));

                if (_userGroupModel == null)
                {
                    strError = "请先设置物料所属分类拣货组长！" + "订单号：" + item.ErpVoucherNo + " 物料编码：" + item.MaterialNo + " 项次：" + item.RowNo + " 项序:" + item.RowNoDel + "";
                    bSucc = false;
                    break;
                }
                else 
                {
                    PickLeaderUserNo = _userGroupModel.PickLeaderUserNo;//拣货组长
                    PickGroupNo = _userGroupModel.PickGroupNo;//拣货组
                }

                if (GetFloorByMaterialNoID(item.MaterialNoID, strBatchNo, pickRule, ref _stockModel, ref strError,item.StrongHoldCode,item.FromErpWareHouse) == false)
                {
                    strError = strError + "订单号：" + item.ErpVoucherNo + " 物料编码：" + item.MaterialNo + " 项次：" + item.RowNo + " 项序:" + item.RowNoDel + "";
                    bSucc = false;
                    break;
                }
                else
                {
                    item.PickLeaderUserNo = PickLeaderUserNo;
                    item.PickGroupNo = PickGroupNo;
                    item.FloorType = _stockModel.FloorType;
                    item.HeightArea = _stockModel.HeightArea;
                    bSucc = true;
                }
            }
            return bSucc;
        }

        #endregion

        protected override string GetModelChineseName()
        {
            return "生成拣货单";
        }

        protected override T_OutStockCreateInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

    }
}
