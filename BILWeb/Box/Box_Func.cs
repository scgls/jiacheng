using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.JSONUtil;
using BILWeb.Stock;
using BILWeb.Login.User;
using BILBasic.User;
using BILWeb.Boxing;
using BILWeb.Box;
using BILWeb.OutStock;
using BILWeb.OutStockTask;
using Newtonsoft.Json;


namespace BILWeb.Box
{
    public partial class T_Box_Func : TBase_Func<T_Box_DB, T_BoxInfo>,IBoxService
     {
   
        protected override bool CheckModelBeforeSave(T_BoxInfo model, ref string strError)
        {
            T_Box_DB _db = new T_Box_DB();
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            return true;
        }

        protected override string GetModelChineseName()
        {
            return "箱号";
        }
        
        protected override T_BoxInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 生成物流箱码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="lstErpVoucherNo"></param>
        /// <param name="HeadName"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool CreatePrintBoxInfo(UserModel user,List<string> lstID,string HeadName,ref string strError) 
        {
            try
            {
                List<T_BoxInfo> lstBox = new List<T_BoxInfo>();//存放需要提交的数据
                List<T_BoxInfo> CreateLstBox = new List<T_BoxInfo>();
                T_Box_DB _tdb = new T_Box_DB();
                string strErpNote = string.Empty;
                string strCustomerName = string.Empty;
                string strErpNoteCat = string.Empty;
                string strErpVoucherNoCat = string.Empty;
                int iDelNo = 0;
                bool bSucc = true;
                List<T_OutStockInfo> outNewStockList = new List<T_OutStockInfo>();

                if (lstID == null || lstID.Count == 0)
                {
                    strError = "提交的生成箱子数据为空！";
                    return false;
                }

                string strFilter = string.Empty;//"erpvoucherno = '" + ErpVoucherNo + "'";

                T_OutStock_Func tfunc = new T_OutStock_Func();
                List<T_OutStockInfo> outStockList = new List<T_OutStockInfo>();
                foreach (var item in lstID) 
                {
                    T_OutStockInfo model = new T_OutStockInfo();
                    strFilter = "id = '" + item + "'";
                    tfunc.GetModelByFilter(ref model,strFilter,ref strError);
                    outStockList.Add(model);
                }

                if (outStockList == null || outStockList.Count == 0) 
                {
                    strError = "未能获取订单数据";
                    return false;
                }



                GetPinErpvoucherNo(ref  outNewStockList,CreateNewOutStock(outStockList) );

                if (outNewStockList != null && outNewStockList.Count > 0) 
                {
                    foreach (var item in outNewStockList)
                    {
                        T_OutStockInfo model1 = new T_OutStockInfo();
                        strFilter = "erpvoucherno = '" + item.ErpVoucherNo + "'";
                        tfunc.GetModelByFilter(ref model1, strFilter, ref strError);
                        outStockList.Add(model1);
                    }
                }


                if (CheckVoucherTypeIsSame(outStockList, ref strError) == false) 
                {
                    return false;
                }

                foreach (var item in outStockList)
                {
                    if (item.Status == 1)
                    {
                        strError += "订单号：" + item.ErpVoucherNo + "处于新建状态，不能生成物流标签！" + "\r\n";
                        bSucc = false;
                    }
                }

                if (bSucc == false)
                {
                    return false;
                }

                T_OutTaskDetails_DB tdb = new T_OutTaskDetails_DB();
                List<T_OutStockTaskDetailsInfo> modelListTaskDetail = new List<T_OutStockTaskDetailsInfo>();

                foreach (var item in outStockList)
                {
                    if (tdb.GetOutTaskDetailByErpVoucherNo(item.ErpVoucherNo, ref modelListTaskDetail, ref strError) == false)
                    {
                        return false;
                    }

                    if (modelListTaskDetail.Where(t => t.UnReviewQty > 0).Count() > 0)
                    {
                        strError = "订单：" + item.ErpVoucherNo + "没有全部复核完成，不能生成物流标签！";
                        bSucc = false;
                        break;
                    }
                }

                if (bSucc == false)
                {
                    return false;
                }

                T_Box_DB boxdb = new T_Box_DB();
                foreach (var item in outStockList)
                {
                    if (boxdb.GetErpVoucherNoIsPrint(item.ErpVoucherNo, ref strError) == false)
                    {
                        bSucc = false;
                        break;
                    }
                }

                if (bSucc == false) { return false; }


                //其他出库单，需要判断客户是否相同
                if (outStockList[0].VoucherType == 24)
                {
                    if (CheckCustomerIsSame(outStockList, ref strError) == false)
                    {
                        return false;
                    }
                    strCustomerName = outStockList[0].CustomerName;
                }
                else ///调拨出库单，验证调入仓库是否相同
                {
                    T_OutStockDetail_Func tfuncd = new T_OutStockDetail_Func();
                    List<T_OutStockDetailInfo> lstOutStockDetail = new List<T_OutStockDetailInfo>();
                    List<T_OutStockDetailInfo> lstOutStockDetailSum = new List<T_OutStockDetailInfo>();
                    foreach (var item in lstID)
                    {
                        strFilter = "headerid = '" + item + "'";
                        tfuncd.GetModelListByFilter(ref lstOutStockDetail, ref strError, "", strFilter, "*");
                        lstOutStockDetailSum.AddRange(lstOutStockDetail);
                    }

                    if (CheckWarehouseIsSame(lstOutStockDetailSum, ref strError) == false)
                    {
                        return false;
                    }
                    strCustomerName = lstOutStockDetailSum[0].ToErpWarehouseName;
                }

                foreach (var item in outStockList)
                {
                    CreateLstBox.AddRange(GetNeBox(_tdb.GetPrintBoxInfo(item.ErpVoucherNo),item.ErpVoucherNo ,strCustomerName, item.ERPNote, HeadName));
                }

                if (CreateLstBox == null || CreateLstBox.Count == 0) 
                {
                    strError = "可以生成箱子的数据为空！";
                    return false;
                }
                                
                List<T_BoxInfo> lstNewCreateBox =  ModelListGroupBy(CreateLstBox);
                List<T_BoxInfo> lstFserialNo = new List<T_BoxInfo>();
                string strFserialNoCat = string.Empty;

                foreach (var item in lstNewCreateBox) 
                {
                   lstFserialNo =  _tdb.GetSerialNoByFserialNo(item.SerialNo);
                   if (lstFserialNo != null && lstFserialNo.Count > 0) 
                   {
                       foreach (var item1 in lstFserialNo)
                       {
                           strFserialNoCat += item1.SerialNo + ",";
                       }
                       item.SerialNo = strFserialNoCat.TrimEnd(',');
                   }
                }

                foreach (var item in lstNewCreateBox)
                {
                    iDelNo += 1;
                    item.DelNo = iDelNo.ToString().PadLeft(4, '0');
                    //item.Remark = strErpNoteCat;
                    //item.ErpVoucherNo = strErpVoucherNoCat;
                }

                //lstNewCreateBox.OrderBy(t => t.IsAmount);

                return _tdb.SaveBoxByModelList(user, lstNewCreateBox.OrderBy(t => t.IsAmount).ToList(), ref strError);

            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        private List<T_OutStockInfo> CreateNewOutStock(List<T_OutStockInfo> modelList) 
        {
            List<T_OutStockInfo> lstModel = new List<T_OutStockInfo>();
            foreach (var item in modelList) 
            {
                T_OutStockInfo model = new T_OutStockInfo();
                model.ErpVoucherNo = item.ErpVoucherNo;
                lstModel.Add(model);
            }
            return lstModel;
        }

        private void GetPinErpvoucherNo( ref List<T_OutStockInfo> outNewStockList, List<T_OutStockInfo> outStockList)
        {
            int bSucc = 0;
            T_Box_DB _tdb = new T_Box_DB();

            List<T_OutStockInfo> outStockListAdd = new List<T_OutStockInfo>();


            List<T_BoxingInfo> lstFserialNo = _tdb.GetFserialNoByErpvoucherNo(outStockList);
            //有父级序列号说明是拼箱，需要再次查找订单
            if (lstFserialNo != null && lstFserialNo.Count > 0)
            {
                List<T_OutStockInfo> lstNewOutStock = _tdb.GetErpvoucherNoByFSerialNo(lstFserialNo);
                foreach (var item in lstNewOutStock)
                {
                    var lst = outStockList.FindAll(t => t.ErpVoucherNo == item.ErpVoucherNo);
                    if (lst == null || lst.Count == 0)
                    {
                        T_OutStockInfo outStock = new T_OutStockInfo();
                        outStock.ErpVoucherNo = item.ErpVoucherNo;
                        outNewStockList.Add(outStock);
                        bSucc += 1;
                    }                    
                }

                if (bSucc > 0 )
                {
                    outStockList.AddRange(outNewStockList);
                    GetPinErpvoucherNo(ref outNewStockList, outStockList);
                }
            }
        }

        private List<T_BoxInfo> ModelListGroupBy(List<T_BoxInfo> modelList)
        {
            string strErpNoteCat = string.Empty;
            string strErpVoucherNoCat = string.Empty;

            List<T_BoxInfo> lstNewBox = new List<T_BoxInfo>();
            
            var lstGroupModelList = from t in modelList
                                    group t by new { t1 = t.SerialNo} into m
                                    select new
                                    {
                                        SerialNo = m.Key.t1
                                    };

            foreach (var item in lstGroupModelList)
            {
                var lst =  modelList.FindAll(t => t.SerialNo == item.SerialNo);
                if (lst != null && lst.Count >= 2)
                {
                    var groupList = from t in lst
                                    group t by new { t1 = t.ErpVoucherNo, t2 = t.Remark } into m
                                    select new
                                    {
                                        ErpVoucherNo = m.Key.t1,
                                        Remark = m.Key.t2,
                                        SerialNo = m.FirstOrDefault().SerialNo,
                                        HeaderName = m.FirstOrDefault().HeaderName,
                                        CustomerName = m.FirstOrDefault().CustomerName,
                                        Flag = m.FirstOrDefault().Flag,
                                        IsAmount = m.FirstOrDefault().IsAmount
                                    };

                    foreach (var item1 in groupList)
                    {
                        strErpNoteCat += item1.Remark + ",";
                        strErpVoucherNoCat += item1.ErpVoucherNo + ",";
                    }

                    T_BoxInfo newModel = new T_BoxInfo();
                    newModel.SerialNo = groupList.FirstOrDefault().SerialNo;
                    newModel.HeaderName = groupList.FirstOrDefault().HeaderName;
                    newModel.CustomerName = groupList.FirstOrDefault().CustomerName;
                    newModel.Flag = groupList.FirstOrDefault().Flag;
                    newModel.Remark = strErpNoteCat.TrimEnd(',');
                    newModel.ErpVoucherNo = strErpVoucherNoCat.TrimEnd(',');
                    newModel.IsAmount = groupList.FirstOrDefault().IsAmount;
                    lstNewBox.Add(newModel);
                }
                else 
                {
                    lstNewBox.AddRange(lst);
                }
            }

            return lstNewBox;
        }

        

        private bool CheckVoucherTypeIsSame(List<T_OutStockInfo> modelList, ref string strError)
        {
            var groupList = from t in modelList
                            group t by new { t1 = t.VoucherType } into m
                            select new
                            {
                                VoucherType = m.Key.t1
                            };

            if (groupList.Count() > 1)
            {
                strError = "生成箱码的单据类型不同，不能同时合并生成箱码！";
                return false;
            }

            return true;

        }

        private bool CheckCustomerIsSame(List<T_OutStockInfo> modelList, ref string strError)
        {
            var groupList = from t in modelList
                            group t by new { t1 = t.CustomerCode } into m
                            select new
                            {
                                CustomerCode = m.Key.t1
                            };

            if (groupList.Count() > 1)
            {
                strError = "生成箱码的客户不同，不能同时合并生成箱码！";
                return false;
            }

            return true;

        }

        private bool CheckWarehouseIsSame(List<T_OutStockDetailInfo> modelList, ref string strError)
        {
            var groupList = from t in modelList
                            group t by new { t1 = t.ToErpWarehouse } into m
                            select new
                            {
                                ToErpWarehouse = m.Key.t1
                            };

            if (groupList.Count() > 1)
            {
                strError = "调拨出库单生成箱码的仓库不同，不能同时合并生成箱码！";
                return false;
            }

            return true;

        }

        public List<T_BoxInfo> GetNeBox(List<T_BoxInfo> lstBox,string strErpVoucherNo ,string strCustomerName, string strErpNote, string HeadName) 
        {
            List<T_BoxInfo> _lstBox = new List<T_BoxInfo>();
            foreach (var item in lstBox) 
            {
                T_BoxInfo model = new T_BoxInfo();
                model.SerialNo = item.SerialNo;
                model.HeaderName = HeadName;
                model.CustomerName = strCustomerName;
                model.Flag = item.Flag;
                model.Remark = strErpNote;
                model.ErpVoucherNo = strErpVoucherNo;
                model.FserialNo = item.FserialNo;
                model.IsPin = item.IsPin;
                model.IsAmount = item.IsAmount;
                _lstBox.Add(model);
            }
            return _lstBox;
        }

        public string GetMessageForPrint(string filter, string flag)
        {
            BaseMessage_Model<List<T_BoxingInfo>> model = new BaseMessage_Model<List<T_BoxingInfo>>();
            List<T_BoxingInfo> modelList = new List<T_BoxingInfo>();
            try
            {
                if (string.IsNullOrEmpty(filter))
                {
                    model.HeaderStatus = "E";
                    model.Message = "客户端传来的条码为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(model);
                }


                T_Box_DB db = new Box.T_Box_DB();
                modelList = db.GetMessageForPrint(filter,  flag);
                model.HeaderStatus = "S";
                model.ModelJson = modelList;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(model);
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(model);
            }
        }


        public string SaveCobBoxListADF(string UserJson, string ModelJson)
        {
            T_Box_DB _db = new T_Box_DB();
            BaseMessage_Model<List<T_BoxingInfo>> messageModel = new BaseMessage_Model<List<T_BoxingInfo>>();

            try
            {
                string strError = string.Empty;

                if (string.IsNullOrEmpty(UserJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "用户端传来用户JSON为空！";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                }

                if (string.IsNullOrEmpty(ModelJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来拼箱条码列表为空！";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                }

                UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);

                if (user == null)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "用户JSON转换用户列表为空！";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                }

                List<T_BoxingInfo> modelList= JSONHelper.JsonToObject<List<T_BoxingInfo>>(ModelJson);

                if(modelList==null || modelList.Count ==0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "拼箱条码列表转换为空！";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                }

                List<T_BoxingInfo> modelNewList = new List<T_BoxingInfo>();

                foreach (var item in modelList) 
                {
                    modelNewList.AddRange(_db.GetModelBySerial(item.SerialNo));
                }

                string VoucherNo = string.Empty;

                T_OutStockTask_DB _db1 = new T_OutStockTask_DB();
                if (_db1.SaveBoxPinList(user, modelList, ref VoucherNo, ref strError) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                }
                else
                {
                    modelList.ForEach(t => t.SerialNo = VoucherNo);
                    messageModel.HeaderStatus = "S";
                    messageModel.MaterialDoc = VoucherNo;
                    messageModel.ModelJson = modelList;
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                }
                //string strError = string.Empty;

                //if (string.IsNullOrEmpty(UserJson))
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = "用户端传来用户JSON为空！";
                //    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                //}

                ////if (string.IsNullOrEmpty(strSerialNew))
                ////{
                ////    messageModel.HeaderStatus = "E";
                ////    messageModel.Message = "客户端传来拼箱条码为空！";
                ////    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                ////}

                ////if (string.IsNullOrEmpty(strSerialOld))
                ////{
                ////    messageModel.HeaderStatus = "E";
                ////    messageModel.Message = "客户端传来拼箱条码为空！";
                ////    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                ////}

                //UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);

                //if (user == null)
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = "用户JSON转换用户列表为空！";
                //    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                //}

                //if (string.IsNullOrEmpty(ModelJson))
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = "客户端传来拼箱条码为空！";
                //    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                //}

                ////List<T_BoxingInfo> modelNew = new List<T_BoxingInfo>();
                ////modelNew = _db.GetModelBySerial(strSerialNew);

                ////if (modelNew == null || modelNew.Count==0) 
                ////{
                ////    messageModel.HeaderStatus = "E";
                ////    messageModel.Message = "未能获取散装箱子数据！箱码：" + strSerialNew;
                ////    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                ////}

                ////if (!string.IsNullOrEmpty(modelNew[0].FserialNo)) 
                ////{
                ////    messageModel.HeaderStatus = "E";
                ////    messageModel.Message = "箱码已经拼箱，不能重复拼箱！箱码：" + strSerialNew;
                ////    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                ////}

                ////List<T_BoxingInfo> modelOld = new List<T_BoxingInfo>();
                ////modelOld = _db.GetModelBySerial(strSerialOld);

                ////if (modelOld == null || modelOld.Count==0)
                ////{
                ////    messageModel.HeaderStatus = "E";
                ////    messageModel.Message = "未能获取散装箱子数据！箱码：" + strSerialOld;
                ////    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                ////}

                ////if (!string.IsNullOrEmpty(modelOld[0].FserialNo))
                ////{
                ////    messageModel.HeaderStatus = "E";
                ////    messageModel.Message = "箱码已经拼箱，不能重复拼箱！箱码：" + strSerialOld;
                ////    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                ////}

                ////if (modelNew[0].CustomerNo.CompareTo(modelOld[0].CustomerNo) != 0) 
                ////{
                ////    messageModel.HeaderStatus = "E";
                ////    messageModel.Message = "拼箱码对应的客户不同，不能拼成一箱！客户分别为："+ modelNew[0].CustomerName + "---" + modelOld[0].CustomerName;
                ////    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                ////}

                ////List<T_BoxingInfo> modelList = new List<T_BoxingInfo>();
                ////modelList.AddRange(modelNew);
                ////modelList.AddRange(modelOld);

                //string VoucherNo = string.Empty;

                //T_OutStockTask_DB _db1 = new T_OutStockTask_DB();
                //if (_db1.SaveBoxPinList(user, strSerialNew, strSerialOld, modelList, ref VoucherNo, ref strError) == false)
                //{
                //    messageModel.HeaderStatus = "E";
                //    messageModel.Message = strError;
                //    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                //}
                //else
                //{
                //    modelList.ForEach(t => t.SerialNo = VoucherNo);
                //    messageModel.HeaderStatus = "S";
                //    messageModel.MaterialDoc = VoucherNo;
                //    messageModel.ModelJson = modelList;
                //    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
                //}
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_BoxingInfo>>>(messageModel);
            }
        }

        public string ScanBoxSerial(string strSerialNo) 
        {
            T_Box_DB _db = new T_Box_DB();
            BaseMessage_Model<T_BoxingInfo> messageModel = new BaseMessage_Model<T_BoxingInfo>();

            try
            {
                string strError = string.Empty;

                if (string.IsNullOrEmpty(strSerialNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来拼箱条码为空！";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_BoxingInfo>>(messageModel);
                }

                List<T_BoxingInfo> modelNew = new List<T_BoxingInfo>();
                modelNew = _db.GetModelBySerial(strSerialNo);

                if (modelNew == null || modelNew.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "未能获取散装箱子数据！箱码：" + strSerialNo;
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_BoxingInfo>>(messageModel);
                }

                if (!string.IsNullOrEmpty(modelNew[0].FserialNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "箱码已经拼箱，不能重复拼箱！箱码：" + strSerialNo;
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_BoxingInfo>>(messageModel);
                }



                messageModel.HeaderStatus = "S";
                messageModel.MaterialDoc = string.Empty;
                messageModel.ModelJson = modelNew[0];
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_BoxingInfo>>(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<T_BoxingInfo>>(messageModel);
            }
        }
    }
   
}
