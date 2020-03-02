using BILBasic.JSONUtil;
using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILWeb.SyncService;
using BILWeb.InStock;
using BILBasic.XMLUtil;
using BILBasic.Common;
using BILBasic.Basing.Factory;
using BILBasic.User;
using BILWeb.InStockTask;
using BILWeb.Stock;
using System.Configuration;
using BILWeb.Area;

namespace BILWeb.Excel
{
    public class Excel_Func 
    {
        public bool ImportExcel(int StockType, UserInfo user, DataSet HeadDS, DataSet DetailDS, ref string strError)
        {            
            
            string strKey = string.Empty;
            string HeadJson = string.Empty;
            string DetailJson = string.Empty;

            StringBuilder jsonString = new StringBuilder();

            jsonString.Append("{\"Result\":\"TRUE\",\"ErrMsg\":null,\"RecordList\": [");

            for (int i = 0; i < HeadDS.Tables["Sheet1"].Rows.Count; i++)
            {
                DataSet ImportDS = new DataSet();
                DataTable TableHead = new DataTable();                

                TableHead = HeadDS.Tables["Sheet1"].Clone();
                                

                //添加行到新的dataset
                TableHead.Rows.Add(HeadDS.Tables["Sheet1"].Rows[i].ItemArray);
                ImportDS.Tables.Add(TableHead);

                if (i > 0) 
                {
                    jsonString.Append(",");
                }
                jsonString.Append("{");
                jsonString.Append("\"WmsVoucherType\":");
                jsonString.Append("\"" + HeadDS.Tables["Sheet1"].Rows[i]["WmsVoucherType"].ToString() + "\"");
                jsonString.Append(",");
                HeadJson = JSONHelper.DateSetObjectToJson(ImportDS).Replace("Sheet1", "Head");
                jsonString.Append(HeadJson.Replace("Head", "\"Head\""));
                jsonString.Append(",");

                strKey = HeadDS.Tables["Sheet1"].Rows[i][0].ToString();

                DataTable TableDetail = new DataTable();
                TableDetail = DetailDS.Tables["Sheet2"].Clone();

                for (int j = 0; j < DetailDS.Tables["Sheet2"].Rows.Count; j++)
                {
                    if (strKey.CompareTo(DetailDS.Tables["Sheet2"].Rows[j][0].ToString()) == 0)
                    {
                        TableDetail.Rows.Add(DetailDS.Tables["Sheet2"].Rows[j].ItemArray);
                    }
                }
                DataSet ImportDetailDS = new DataSet();
                ImportDetailDS.Tables.Add(TableDetail);
                DetailJson = JSONHelper.DateSetListToJson(ImportDetailDS).Replace("Sheet2", "Detail");

                jsonString.Append(DetailJson.Replace("Detail", "\"Detail\""));

                jsonString.Append("}");
            }

            jsonString.Append("]}");

            ParamaterField_Func pfunc = new ParamaterField_Func();
            return pfunc.GetExcelmport(StockType, jsonString.ToString(), "ERP", ref strError);            
        }

        public bool ImportSerialNoByExcel(UserInfo user, DataSet ds, ref string strErrMsg)
        {
            try
            {
                bool bResult = false;
                string MateiralDoc = string.Empty;
                string TaskNo = string.Empty; 

                int ImportCount = 0;
                int VoucherType = 0;
                ImportCount = ConfigurationManager.ConnectionStrings["ImportCount"].ConnectionString.ToInt32();
                VoucherType = ConfigurationManager.ConnectionStrings["ImportVoucherType"].ConnectionString.ToInt32();

                List<T_SerialNoInfo> lstSerialNo = new List<T_SerialNoInfo>();
                BaseMessage_Model<T_InStockDetailInfo> baseMessage = new BaseMessage_Model<T_InStockDetailInfo>();
                //BaseMessage_Model<T_InStockTaskDetailsInfo> taskMessage = new BaseMessage_Model<T_InStockTaskDetailsInfo>();
                Excel_DB edb = new Excel_DB();

                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    strErrMsg = "没有导入数据！";
                    return false;
                }

                if (ds.Tables[0].Rows.Count < ImportCount) 
                {
                    strErrMsg = "导入的序列号不足上限值，请扫描输入！上限为：" + ImportCount.ToDBString();
                    return false;
                }

                //获取第一行第一列的值，只有采购订单号
                string VoucherNo = ds.Tables[0].Rows[0][0].ToString();

                //验证订单号是否存在
                T_InStock_DB tdb = new T_InStock_DB();
                if (tdb.CheckVoucherNo(VoucherNo,VoucherType) <= 0)
                {
                    strErrMsg = "ERP订单号不存在！不能导入数据";
                    return false;
                }

                //DataSet转List
                List<T_SerialNoInfo> lstModel = DataSetToList(ds);
                if (CheckSerialIsSame(ref strErrMsg, lstModel) == false)
                {
                    return false;
                }

                if (CheckVoucherNoIsSame(ref strErrMsg, lstModel) == false)
                {
                    return false;
                }

                //验证序列号库存是否存在
                string strSerialXml = XmlUtil.Serializer(typeof(List<T_SerialNoInfo>), lstModel);
                T_InStockDetail_DB indetail = new T_InStockDetail_DB();
                bResult = indetail.CheckSerialListIsExist(strSerialXml, ref strErrMsg);
                if (bResult == false)
                {
                    return false;
                }

                //根据订单号获取订单明细
                List<T_InStockDetailInfo> lstDetail = indetail.GetDetailByVoucherNo(VoucherNo,VoucherType);

                //分配到订单明细行
                foreach (var item in lstDetail)
                {
                    item.lstSerialNo = lstModel.FindAll(t => t.FacMaterialNo == item.MaterialNo && t.RowNo==item.RowNo);
                    item.ReceiveQty = item.lstSerialNo.Count();
                    item.ScanQty = item.ReceiveQty;
                    item.IsSerial = 2;
                }

                //过账ERP
                baseMessage = PostReceiveToERP(user, lstDetail.Where(t=>t.ScanQty>0).ToList());
                if (baseMessage.HeaderStatus == "E")
                {
                    strErrMsg = baseMessage.Message;
                    return false;
                }

                TaskNo = baseMessage.TaskNo;

                bResult = edb.ImportSerialNo(strSerialXml, TaskNo, user, ref strErrMsg);
                if (bResult == false) 
                {
                    strErrMsg = baseMessage.Message + "\r\n上架任务处理失败：" + strErrMsg;
                    return false;
                }

                strErrMsg = baseMessage.Message + "\r\n上架任务处理成功！";

                //taskMessage = GetInTaskDetailListByHeaderID(TaskNo, user, lstDetail);

                //if (taskMessage.HeaderStatus == "E")
                //{
                //    strErrMsg = baseMessage.Message;
                //    return false;
                //}

                //strErrMsg = taskMessage.Message;

                return true;
                
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据任务ID获取任务数据
        /// </summary>
        /// <param name="TaskNo"></param>
        /// <returns></returns>
        public BaseMessage_Model<T_InStockTaskDetailsInfo> GetInTaskDetailListByHeaderID(string TaskNo,UserModel user, List<T_InStockDetailInfo> lstModel) 
        {
            int HeadID = 0;
            bool bSucc = false;
            string strErrMsg = string.Empty;
            BaseMessage_Model<T_InStockTaskDetailsInfo> baseMessage = new BaseMessage_Model<T_InStockTaskDetailsInfo>();
            T_InTaskDetails_DB tdb = new T_InTaskDetails_DB();
            T_InTaskDetails_Func tfunc = new T_InTaskDetails_Func();
            T_AreaInfo areaModel = new T_AreaInfo();
            T_Area_Func areaFunc = new T_Area_Func();

            HeadID = tdb.GetIDByTaskNo(TaskNo);
            List<T_InStockTaskDetailsInfo> lstTaskDetail = new List<T_InStockTaskDetailsInfo>();
            bSucc = tfunc.GetModelListByHeaderID(ref lstTaskDetail, HeadID, ref strErrMsg);
            if (bSucc == false) 
            {
                baseMessage.HeaderStatus = "E";
                baseMessage.Message = strErrMsg;
                return baseMessage;
            }

            

            //任务表体数据
            foreach (var item in lstTaskDetail) 
            {
                item.lstStockInfo = new List<T_StockInfo>();
                //根据任务表体数据查找收货数据，存在多行
                 var lstInDetail = lstModel.FindAll(t => t.MaterialNo == item.MaterialNo);
               
                 foreach (var itemInDetail in lstInDetail) 
                 {
                     areaFunc.GetAreaModelBySql(0,itemInDetail.lstSerialNo.FirstOrDefault().AreaNo, ref areaModel, ref strErrMsg);
                     foreach (var itemSerialNo in itemInDetail.lstSerialNo) 
                     {
                         T_StockInfo stock = new T_StockInfo();
                         stock.SerialNo = itemSerialNo.SerialNo;
                         item.lstStockInfo.Add(stock);
                     }                     
                 }
                 item.AreaID = areaModel.ID;
                 item.WarehouseID = areaModel.WarehouseID;
                 item.HouseID = areaModel.HouseID;
                 item.ScanQty = item.lstStockInfo.Count;
            }

            string Result =  tfunc.SaveModelListSqlToDBADF(JSONHelper.ObjectToJson<UserModel>(user), JSONHelper.ObjectToJson<List<T_InStockTaskDetailsInfo>>(lstTaskDetail));

            return JSONHelper.JsonToObject<BaseMessage_Model<T_InStockTaskDetailsInfo>>(Result);
        }

        private BaseMessage_Model<T_InStockDetailInfo> PostReceiveToERP(UserModel user, List<T_InStockDetailInfo> lstDetail)
        {
            T_InStockDetail_Func tfunc = new T_InStockDetail_Func();
            string UserJson = JSONHelper.ObjectToJson<UserModel>(user);
            string ModelJson = JSONHelper.ObjectToJson<List<T_InStockDetailInfo>>(lstDetail);            

            string Result = tfunc.SaveModelListSqlToDBADF(UserJson, ModelJson);

            BaseMessage_Model<T_InStockDetailInfo> baseMessage = JSONHelper.JsonToObject<BaseMessage_Model<T_InStockDetailInfo>>(Result);

            return baseMessage;            
        }


        private bool CheckVoucherNoIsSame(ref string strErrMsg, List<T_SerialNoInfo> lstModel)
        {
            var lstVoucherNo = from t in lstModel
                            group t by new { t1 = t.VoucherNo } into m
                            select new
                            {
                                VoucherNo = m.Key.t1
                            };

            if (lstVoucherNo.Count() > 1)
            {
                strErrMsg = "存在不同订单号，不能导入数据！";
                return false;
            }

            return true;
        }

        private  bool CheckSerialIsSame(ref string strErrMsg, List<T_SerialNoInfo> lstModel)
        {
            var lstSerial = from t in lstModel
                            group t by new { t1 = t.SerialNo } into m
                            select new
                            {
                                SerialNo = m.Key.t1
                            };
            
            if (lstSerial.Count() < lstModel.Count)
            {
                strErrMsg = "存在相同序列号，不能导入数据！";
               return false;
            }

            return true;
        }

        public List<T_SerialNoInfo> DataSetToList(DataSet ds) 
        {
            string VoucherNo = string.Empty;
            string MaterialNo = string.Empty;

            List<T_SerialNoInfo> lstModel = new List<T_SerialNoInfo>();

            VoucherNo = ds.Tables[0].Rows[0][0].ToString();
            MaterialNo = ds.Tables[0].Rows[0][1].ToString();

            for (int i = 0; i < ds.Tables[0].Rows.Count;i++ )
            {
                T_SerialNoInfo model = new T_SerialNoInfo();

                model.VoucherNo = ds.Tables[0].Rows[i][0].ToString();
                model.RowNo = ds.Tables[0].Rows[i][1].ToString();
                model.FacMaterialNo = ds.Tables[0].Rows[i][2].ToString();
                model.SerialNo = model.FacMaterialNo + "@" + ds.Tables[0].Rows[i][3].ToString();
                model.AreaNo = ds.Tables[0].Rows[i][4].ToString();
                lstModel.Add(model);
            }

            return lstModel;
        }

        /// <summary>
        /// 读取EXCEL数据，根据物料号分组序列号
        /// </summary>
        /// <param name="lstModel"></param>
        /// <returns></returns>
        //public List<T_SerialNoInfo> GetSerialGroupByMaterialNo(List<T_SerialNoInfo> lstModel) 
        //{
        //    var lstSerial = from t in lstModel
        //                    group t by new { t1 = t.FacMaterialNo } into m
        //                    select new
        //                    {
        //                        FacMaterialNo = m.Key.t1,
        //                    };
        //}

    }
}
