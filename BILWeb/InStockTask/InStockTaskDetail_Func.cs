using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.JSONUtil;
using BILBasic.User;
using BILWeb.OutBarCode;
using BILWeb.Stock;
using BILWeb.Login.User;

namespace BILWeb.InStockTask
{
    public partial class T_InTaskDetails_Func : TBase_Func<T_InTaskDetails_DB, T_InStockTaskDetailsInfo>
    {

        protected override bool CheckModelBeforeSave(T_InStockTaskDetailsInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            return true;
        }

        protected override bool CheckModelBeforeSave(List<T_InStockTaskDetailsInfo> modelList, ref string strError)
        {

            if (modelList == null || modelList.Count == 0)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }


            if (modelList.Where(t => t.ScanQty == 0).Count() == modelList.Count())
            {
                strError = "所有物料都没有扫描，不能保存！";
                return false;
            }

            if (CheckScanQty(modelList.Where(t => t.ScanQty > 0).ToList(), ref strError) == false)
            {
                return false;
            }

            if (CheckBarCodeIsSame(modelList.Where(t => t.ScanQty > 0).ToList(), ref strError) == false)
            {
                return false;
            }
            if (checkStronHoldCode(modelList.Where(t => t.ScanQty > 0).ToList(), ref strError) == false)
            {
                return false;
            }

            foreach (var item in modelList)
            {
                if (item.lstStockInfo != null && item.lstStockInfo.Count > 0)
                {
                    foreach (var itemStock in item.lstStockInfo)
                    {
                        var count = db.GetScalarBySql("select count(*)  from t_stock where Areaid =" + itemStock.AreaID + " and  serialno ='" + itemStock.SerialNo + "' and qty =" + itemStock.Qty + "");
                        if (Convert.ToInt16(count) == 0)
                        {
                            strError = "扫描条码已发生变更,请重新扫描";
                            return false;
                        }
                        //var value = db.GetScalarBySql("select count(*)  from t_stock where Areaid =" + item.AreaID + " and materialno ='" + itemStock.MaterialNo + "' and batchno !='" + itemStock.BatchNo + "' and  strongholdcode='" + itemStock.StrongHoldCode + "' ");
                        //if (Convert.ToInt16(value) > 0)
                        //{
                        //    strError = "该货位已存在同物料不同批次的库存，请更换其它货位保存";
                        //    return false;
                        //}
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 上架校验同库区是否存在不同组织物料信息
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        private bool checkStronHoldCode(List<T_InStockTaskDetailsInfo> modelList, ref string strErrMsg)
        {
            string sql = "";
            foreach (var item in modelList)
            {
                if (item.ScanQty > 0)
                {
                    sql = "select count(*) from t_stock where materialno='" + item.MaterialNo + "' and HOUSEID=" + item.HouseID + " and strongholdcode !='" + item.StrongHoldCode + "'";
                    if (Convert.ToInt16(db.GetScalarBySql(sql)) > 0)
                    {
                        strErrMsg = "上架库区 物料" + item.MaterialNo + " 存在非" + item.StrongHoldCode + " 组织库存";
                        return false;
                    }

                }
            }
            return true;
        }

        /// <summary>
        /// 验证扫描数量和剩余数量
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        private bool CheckScanQty(List<T_InStockTaskDetailsInfo> modelList, ref string strErrMsg)
        {
            bool bSucc = false;

            foreach (var item in modelList)
            {
                if (item.ScanQty > item.RemainQty)
                {
                    bSucc = false;
                    strErrMsg = "扫描数量大于上架剩余数量！物料：" + item.MaterialNo + "行号：" + item.RowNo + "扫描数量：" + item.ScanQty + "上架剩余：" + item.RemainQty;
                    break;
                }
                else
                {
                    bSucc = true;
                }
            }

            return bSucc;
        }


        /// <summary>
        /// 验证条码是否有相同数据
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        private bool CheckBarCodeIsSame(List<T_InStockTaskDetailsInfo> modelList, ref string strErrMsg)
        {
            bool bSucc = false;

            List<T_StockInfo> lstBarCode = new List<T_StockInfo>();

            foreach (var item in modelList)
            {
                lstBarCode.AddRange(item.lstStockInfo);
            }

            var groupByList = from t in lstBarCode
                              group t by new { t1 = t.SerialNo } into m
                              select new
                              {
                                  SerialNo = m.Key.t1
                              };

            if (groupByList.Count() != lstBarCode.Count)
            {
                strErrMsg = "上架条码存在相同数据，不能提交！";
                bSucc = false;
            }
            else { bSucc = true; }


            return bSucc;
        }


        protected override string GetModelChineseName()
        {
            return "上架任务表体";
        }

        protected override T_InStockTaskDetailsInfo GetModelByJson(string ModelJson)
        {
            return JSONHelper.JsonToObject<T_InStockTaskDetailsInfo>(ModelJson);
        }

        protected override List<T_InStockTaskDetailsInfo> GetModelListByJson(string UserJson, string ModelListJson)
        {
            List<T_InStockTaskDetailsInfo> modelList = JSONHelper.JsonToObject<List<T_InStockTaskDetailsInfo>>(ModelListJson);
            UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);
            modelList = modelList.Where(t => t.ScanQty > 0).ToList();

            string strUserNo = string.Empty;
            string strPostUser = string.Empty;

            //if (TOOL.RegexMatch.isExists(user.UserNo) == true)
            //{
            //    strUserNo = user.UserNo.Substring(0, user.UserNo.Length - 1);
            //}
            //else
            //{
            //    strUserNo = user.UserNo;
            //}

            //User_DB _db = new User_DB();
            //strPostUser = _db.GetPostAccountByUserNo(strUserNo, modelList[0].StrongHoldCode);
            modelList.ForEach(t => t.PostUser = user.UserNo);//strPostUser
            return modelList;
        }

        public string LockTaskOperUser(string TaskDetailsJson, string UserJson)
        {
            BaseMessage_Model<T_InStockTaskDetailsInfo> messageModel = new BaseMessage_Model<T_InStockTaskDetailsInfo>();

            try
            {
                int iLock = 0;
                string strUserName = string.Empty;

                T_InStockTaskDetailsInfo taskDetailsModel = BILBasic.JSONUtil.JSONHelper.JsonToObject<T_InStockTaskDetailsInfo>(TaskDetailsJson);

                if (taskDetailsModel == null || (string.IsNullOrEmpty(taskDetailsModel.MaterialNo) && string.IsNullOrEmpty(taskDetailsModel.TMaterialNo)))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "没有物料信息";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);
                }

                UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);

                if (user == null || string.IsNullOrEmpty(user.UserNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "没有用户信息";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);
                }
                T_InTaskDetails_DB _db = new T_InTaskDetails_DB();

                strUserName = _db.QueryUserNameByTaskDetails(taskDetailsModel, user);
                if (!string.IsNullOrEmpty(strUserName))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "物料：" + taskDetailsModel.MaterialNo + taskDetailsModel.TMaterialNo + "\r\n" + "被：" + strUserName + "锁定！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);
                }

                iLock = _db.LockTaskOperUser(user, taskDetailsModel);

                if (iLock == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "物料：" + taskDetailsModel.MaterialNo + "锁定失败！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);
                }
                messageModel.HeaderStatus = "S";
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);
            }
        }

        public string UnLockTaskOperUser(string TaskDetailsJson, string UserJson)
        {
            BaseMessage_Model<T_InStockTaskDetailsInfo> messageModel = new BaseMessage_Model<T_InStockTaskDetailsInfo>();

            try
            {
                bool bSucc = false;

                string strErrMsg = string.Empty;

                List<T_InStockTaskDetailsInfo> modelList = JSONHelper.JsonToObject<List<T_InStockTaskDetailsInfo>>(TaskDetailsJson);

                if (modelList == null || modelList.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "表体数据为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);
                }

                UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);

                if (user == null || string.IsNullOrEmpty(user.UserNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "用户信息为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);
                }

                T_InTaskDetails_DB _db = new T_InTaskDetails_DB();
                bSucc = _db.UnLockTaskOperUser(modelList, user, ref strErrMsg);
                if (bSucc == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strErrMsg;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);

                }
                messageModel.HeaderStatus = "S";
                messageModel.Message = "上架数据解锁成功！";
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_InStockTaskDetailsInfo>>(messageModel);
            }
        }

        public bool GetExportTaskDetail(T_InStockTaskInfo model, ref List<T_InStockTaskDetailsInfo> modelList, ref string strErrMsg)
        {
            try
            {
                T_InTaskDetails_DB tdb = new T_InTaskDetails_DB();
                modelList = tdb.GetExportTaskDetail(model);
                if (modelList == null || modelList.Count == 0)
                {
                    strErrMsg = "获取导出数据为空！";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }

        public bool GetModelListByHeaderIDForPc(ref List<T_InStockTaskDetailsInfo> modelList, int headerID, ref string strError) 
        {
            try
            {
                T_InTaskDetails_DB tdb = new T_InTaskDetails_DB();
                
                modelList = tdb.GetModelListByHeaderIDForPc(headerID);
                if (modelList == null || modelList.Count == 0) 
                {
                    strError = "获取明细列表为空！";
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

    }
}
