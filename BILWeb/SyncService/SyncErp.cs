using BILWeb.SyncService.Model;
using BILWeb.SyncService.SqlSugar;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BILWeb.SyncService
{
    class SyncErp
    {

        internal static bool SyncSAPJsonFromErp(int stockType, string lastSyncTime, string erpVoucherNo, int wmsVourcherType, string InJsonData, ref string errMsg)
        {
            //InJsonData = "{\"result\":\"1\",\"resultValue\":\"\",\"data\":[{\"head\":{\"cDLCode\":\"MD20190104183726000007\",\"caddcode\":\"JC10\",\"cDefine14\":\"电工成品库\",\"cShipAddress\":\"详细地址\",\"cCusCode\":\"1002000\",\"cCusName\":\"winwen回归经销商\",\"ccusperson\":\"13122360008\",\"contactName\":\"winwen\",\"dDate\":\"2019-01-04 06:37:26\",\"cMaker\":\"正泰民用\",\"lightingType\":0,\"options\":\" \",\"cancelType\":\"0\",\"province\":\"北京\",\"city\":\"北京市\",\"region\":\"东城区\",\"extOrderType\":\"销售出库\",\"body\":[{\"cSoCode\":\"17\",\"cOrderCode\":\"SO20190104143851000027\",\"cInvCode\":\"100000019585\",\"itemInventoryCode\":\"def\",\"cInvName\":\"sap民用商品 - A253\",\"iQuantity\":3,\"iSum\":768.71,\"iQuotedPrice\":76.871,\"iTaxUnitPrice\":76.871,\"caddcode\":\"JC10\",\"cDLCode\":\"MD20190104183726000007\",\"cDefine14\":\"电工成品库\",\"sapOrderCode\":\"0100000079\"}]}}]}";
            bool result = false;
            try
            {
                int autoSync = String.IsNullOrEmpty(InJsonData) ? 1 : 0;
                 //根据单据类型和出库入类型获取同步字段
                List<Sync_Model> syncModelList = SyncManager.GetInstance().GetSyncModelList(stockType,erpVoucherNo, wmsVourcherType, autoSync); //同步字段
                if (syncModelList == null || syncModelList.Count == 0)
                {
                    errMsg += wmsVourcherType + "|单据类型未配置！\r\n";
                    LogNet.SyncError(errMsg);
                    return false;
                }
                //  var ErpTypeList = syncModelList.Where(p => p.ErpType != null).DistinctBy(s => new { s.ErpType }); //获取同步单据类型
                var WmsTypeList = syncModelList.Where(p => p.ErpType != null).DistinctBy(s => new { s.WmsType, s.MainSubject });

                //  foreach (var erpType in ErpTypeList) //ERP单据类型
                foreach (var wmsType in WmsTypeList)
                {
                    //获取单据同步数据Json
                    string dataJson = String.Empty;
                    string SAPServerTime = "";
                    if (String.IsNullOrEmpty(erpVoucherNo) && String.IsNullOrEmpty(InJsonData))
                    {
                        SAPServerTime = Sync_Erp_Func.GetSAPServerTime(ConfigurationManager.ConnectionStrings["ServerTimeType"].ConnectionString); //获取SAP服务器时间
                        JsonModel jsmodel = JsonConvert.DeserializeObject<JsonModel>(SAPServerTime);
                        if (!jsmodel.result.Equals("1"))
                        {
                            errMsg += wmsType.WmsName + "|" + jsmodel.resultValue + "\r\n";
                            continue;
                        }
                        SAPServerTime = JArray.Parse(jsmodel.data.ToString())[0]["head"]["ZDATUM"].ToString();

                    }
                    SyncTime_Model syncTime = new SyncTime_Model();

                    string Json = String.IsNullOrEmpty(InJsonData) ? Sync_Erp_Func.GetSAPJson(stockType, erpVoucherNo, wmsType, ref syncTime) : InJsonData; //获取JSON
                    if (String.IsNullOrEmpty(Json))
                    {
                        errMsg += wmsType.WmsName + "|" + erpVoucherNo + "|接口没有返回数据\r\n";
                        continue;
                    }
                    result = GetDataJson(Json, ref dataJson, wmsType.WmsName, ref errMsg);
                    //result = true;
                    //dataJson = "[{\"head\":{\"EBELN\":\"4500000767\",\"BSART\":\"ZNB\",\"EKGRP\":\"J20\",\"EKNAM\":\"建筑-中山LED成品\",\"LIFNR\":\"100076\",\"NAME1\":\"江门市睿点照明科技有限公司\",\"ZCDATE\":\"2019-01-14\",\"ZCTIME\":\"09:38:31\",\"ZUDATE\":\"0000-00-00\",\"ZUTIME\":\"00:00:00\",\"ERNAM\":\"MM5001001\",\"body\":[{\"EBELN\":\"4500000767\",\"EBELP\":\"00010\",\"WERKS\":\"7000\",\"LGORT\":null,\"BWART\":null,\"MATNR\":\"100000029402\",\"ZZMAKTX\":\"LED蜡烛泡08 尖泡 3W 6500K 香槟金\",\"GROES\":\"NEP-QP0800362\",\"BISMT\":\"205260\",\"PLIFZ\":\"1\",\"MENGE\":1400.0,\"MEINS\":\"PC\",\"LOEKZ\":\"\",\"TXZ01\":\"NEP-QP0800362蜡烛泡08尖泡3W6500K香槟金\",\"BUDAT\":\"0001-01-01T00:00:00Z\",\"BLDAT\":\"0001-01-01T00:00:00Z\",\"USNAM\":null,\"GUID\":null}]}}]";
                    if (!result)
                    {
                        if (String.IsNullOrEmpty(erpVoucherNo) && String.IsNullOrEmpty(InJsonData))
                        {

                            if (string.Compare(syncTime.SyncServerTime.Split(',')[0], SAPServerTime.Split(',')[0]) < 0)
                            {
                                syncTime.SyncServerTime = SAPServerTime.Split(',')[0] + "," + "000000";
                                //同步成功，更新同步时间
                                SyncTimeManager.GetInstance().InsertOrUpdateSyncTime(syncTime);
                            }
                        }
                        continue;
                    }
                    if(wmsVourcherType != 21)
                        LogNet.SyncInfo(wmsType.WmsName + ":\r\n" + dataJson);
                    else
                        LogNet.SyncWarn(wmsType.WmsName + ":\r\n" + dataJson);
                    // string ErpvouType = wmsType.ErpType.ToString();
                    //根据ERP类型查询WMS类型
                    // var WmsTypeList = syncModelList.Where(p => p.ErpType != null && p.ErpType == ErpvouType).DistinctBy(s => new { s.WmsType, s.MainSubject });

                    List<string> sqlList = new List<string>();

                    List<string> headInsertKeys = new List<string>();
                    List<string> detailInsertKeys = new List<string>();
                    List<string> headUpdateKeys = new List<string>();
                    List<string> detailUpdateKeys = new List<string>();

                    List<Sync_Model> pmListbyType = syncModelList.FindAll(p => p.WmsType.ToString() == wmsType.WmsType && p.MainSubject == wmsType.MainSubject);

                    foreach (Sync_Model sync in pmListbyType)
                    {
                        if (sync.FieldHD.Equals("H"))
                        {
                            headInsertKeys.Add(sync.WmsField);
                            if (!(sync.FUNCTIONTYPE == 2 || sync.FUNCTIONTYPE == 1))
                                headUpdateKeys.Add(sync.WmsField);
                        }

                        else
                        {
                            detailInsertKeys.Add(sync.WmsField);
                            if (!(sync.FUNCTIONTYPE == 2 || sync.FUNCTIONTYPE == 1))
                                detailUpdateKeys.Add(sync.WmsField);
                        }

                    }

                    List<string> detailWhereStringList;
                    string[] headwherekeys = pmListbyType[0].WmsHeadKeys.Split(',');
                    string[] detailwherekeys = null;
                    if (pmListbyType[0].WmsDetailKeys != null)
                        detailwherekeys = pmListbyType[0].WmsDetailKeys.Split(',');
                    JArray jarray = JArray.Parse(dataJson);
                    ParamaterFiled_DB db = new ParamaterFiled_DB();
                    List<string> headValues = new List<string>();
                    List<List<string>> detailValues = new List<List<string>>();
                    List<List<string>> detailInsertOrUpdateValues = new List<List<string>>();
                    JToken Head;
                    JArray Detail;




                    //生成SQL语句（insert\update\delete）
                    string insertHeadSQL = String.Empty;
                    string insertDetailSQL = String.Empty;
                    string updateHeadSQL = String.Empty;
                    string updateDetailSQL = String.Empty;

                    if (syncModelList.Count > 0 && !String.IsNullOrEmpty(dataJson.TrimStart('[').TrimEnd(']')))
                    {
                        string WmsvouType = wmsType.WmsType.ToString();
                        string MainProject = wmsType.MainSubject.ToString();
                        //单据类型对应字段
                        pmListbyType = syncModelList.FindAll(p => p.WmsType.ToString() == WmsvouType && p.ErpType == wmsType.ErpType.ToString() && p.MainSubject == MainProject);
                        string[] headTableName = pmListbyType[0].WmsTableH.Split(',');
                        string[] detailTableName = pmListbyType[0].WmsTableD.Split(',');

                        for (int i = 0; i < jarray.Count; i++) //记录数
                        {
                            headValues = new List<string>();
                            detailValues = new List<List<string>>();
                            detailInsertOrUpdateValues = new List<List<string>>();
                            Head = JObject.Parse(jarray[i]["head"].ToString());
                            string headWhereString = SyncManager.GetInstance().GetSAPwhereString(true, headwherekeys, pmListbyType, Head);
                            int ID = SyncManager.GetInstance().CheckVoucherNoExit(stockType, headTableName[0], headWhereString);
                            if (ID == -99) continue; //不需要执行update

                            //同一种单据类型中，没有生成过SQL的，第一次需要生成SQL语句
                            if (ID == 0) //insert
                            {
                                if (String.IsNullOrEmpty(insertHeadSQL))
                                {
                                    insertHeadSQL = SqlModel.InsertSAPTitleSql;
                                    insertDetailSQL =(WmsvouType=="47" ?SqlModel.InsertU8DetailSql: SqlModel.InsertSAPDetailSql);
                                    //  string WmsVourcherNo = db.GetWmsWoucherNo(pmListbyType[0].WmsVoucherNoRual, headTableName, null);
                                    insertHeadSQL = String.Format(insertHeadSQL, "{4}", "{2}", "{1}", "{0}", "{3}", "{5}", wmsType.ErpType.ToString(), WmsvouType);
                                }
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(updateHeadSQL))
                                {
                                    updateHeadSQL = SqlModel.UpdateSAPSql;
                                    updateDetailSQL = SqlModel.UpdateSAPSql;
                                }
                            }


                            Detail = null;
                            detailWhereStringList = new List<string>();
                            if (!String.IsNullOrEmpty(Head.ToString()) && !String.IsNullOrEmpty(detailTableName[0]))
                            {
                                Detail = JArray.Parse(Head["body"].ToString());
                                for (int j = 0; j < Detail.Count; j++)
                                {
                                    string detailWhereString = SyncManager.GetInstance().GetSAPwhereString(false, detailwherekeys, pmListbyType, JObject.Parse(Detail[j].ToString()));
                                    detailWhereStringList.Add(detailWhereString);
                                }
                            }

                            //判断表体行数是否变化，发生变化需要执行insert语句
                            int detauLCount = 0;
                            string OMDWmsVourcherNo = String.Empty;
                            bool isNoChangeDetail = false;
                            if (ID != 0)
                            {
                                int index = pmListbyType.FindIndex(p => p.WmsField.ToUpper() == "ERPVOUCHERNO" && p.FieldHD.ToUpper() == "D");
                                string erpvoucherno = String.Empty;
                                if (index != -1)
                                {
                                    erpvoucherno = Detail[0][pmListbyType[index].ErpField].ToString();
                                    SyncManager.GetInstance().GetDetailCountByErpVoucherNo(detailTableName[0], erpvoucherno, ref detauLCount, ref OMDWmsVourcherNo);
                                    isNoChangeDetail = detauLCount == Detail.Count;

                                }
                            }else
                            {
                                isNoChangeDetail = true;
                            }
          

                            bool isFormart = true;
                            foreach (Sync_Model sync in pmListbyType)
                            {
                                try
                                {
                                    if ((ID != 0 && (sync.FUNCTIONTYPE == 2 || sync.FUNCTIONTYPE == 1)) && (sync.FieldHD.Equals("H") || (sync.FieldHD.Equals("D") && isNoChangeDetail))) continue; //如果是update，FUNCTIONTYPE=2不需要赋值
                                    if (sync.FieldHD.Equals("H"))
                                    {
                                        string value = sync.FUNCTIONTYPE == 1 || sync.FUNCTIONTYPE == 3 ? sync.DefaultValue : Head[sync.ErpField].ToString();
                                        if (sync.DefaultType == 0)
                                        {
                                            value = sync.ErpField.Trim().Equals("") ? value : "'" + value + "'";
                                        }
                                        else
                                        if (sync.DefaultType == 1)
                                        {
                                            value = "CONVERT(varchar(100),'" + value + "', 20)";
                                        }
                                        else
                                        if (sync.DefaultType == 2)
                                        {
                                            string materialNo = Head[sync.ErpField].ToString();
                                            string materialSubSql = String.Format(SqlModel.GetsAPmaterialIdsql, materialNo);
                                            value = "(" + materialSubSql + ")";
                                        }
                                        //else
                                        //{
                                        //   value = sync.DefaultType == 1 ? "CONVERT(varchar(100),'" + value + "', 20)" : ;
                                        //}
                                        headValues.Add(value.Trim());
                                    }
                                    else
                                    {
                                        if (Detail != null)
                                        {
                                            for (int j = 0; j < Detail.Count; j++)
                                            {
                                                JToken detailJToken = JObject.Parse(Detail[j].ToString());
                                                string value = sync.FUNCTIONTYPE == 1 || sync.FUNCTIONTYPE == 3 ? sync.DefaultValue : detailJToken[sync.ErpField].ToString();
                                                // value = sync.DefaultType == 1 ? "CONVERT(varchar(100),'" + value + "', 20)" : (sync.ErpField.Trim().Equals("") ? value : "'" + value + "'");

                                                if (sync.DefaultType == 0)
                                                {
                                                    value = sync.ErpField.Trim().Equals("") ? value : "'" + value + "'";
                                                }
                                                else
                                                    if (sync.DefaultType == 1)
                                                {
                                                    value = "CONVERT(varchar(100),'" + value + "', 20)";
                                                }
                                                else
                                                if (sync.DefaultType == 2)
                                                {
                                                    string materialNo = detailJToken[sync.ErpField].ToString();
                                                    string materialSubSql = String.Format(SqlModel.GetsAPmaterialIdsql, materialNo);
                                                    value = "(" + materialSubSql + ")";
                                                }
                                                else              //出库单自动分配客户对应发货仓库 只有发货单需要处理
                                                if (sync.DefaultType == 3 || sync.DefaultType == 4)
                                                {
                                                    string customerno = Head[sync.ErpField].ToString();
                                                    string warehouseIDSubSql = String.Format(sync.DefaultType == 3 ? SqlModel.GetWhareHouseID : SqlModel.GetWhareHouseNo, customerno);
                                                    value = "(" + warehouseIDSubSql + ")";
                                                }
                                                else
                                                //调拨单获取warehouseid
                                                if (sync.DefaultType == 5)
                                                {

                                                    string warehouseNo = detailJToken[sync.ErpField].ToString();
                                                    string warehouseIDSubSql = String.Format(SqlModel.GetWhareHouseIDByNo, warehouseNo);
                                                    value = "(" + warehouseIDSubSql + ")";
                                                }

                                                if (!(sync.FUNCTIONTYPE == 2 || sync.FUNCTIONTYPE == 1) || isNoChangeDetail)
                                                {
                                                    if (detailValues.Count < (j + 1))
                                                    {
                                                        List<string> valueD = new List<string>();
                                                        valueD.Add(value.Trim());
                                                        detailValues.Add(valueD);
                                                    }
                                                    else
                                                    {
                                                        detailValues[j].Add(value.Trim());

                                                    }
                                                }
                                                if (!isNoChangeDetail)
                                                {
                                                    if (detailInsertOrUpdateValues.Count < (j + 1))
                                                    {
                                                        List<string> valueD = new List<string>();
                                                        valueD.Add(value.Trim());
                                                        detailInsertOrUpdateValues.Add(valueD);
                                                    }
                                                    else
                                                    {
                                                        detailInsertOrUpdateValues[j].Add(value.Trim());

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    result = false;
                                    isFormart = false;
                                    errMsg += wmsVourcherType + "|" + sync.FieldHD + "|" + sync.ErpField + "|" + sync.WmsType + ex.Message + "\r\n";
                                 
                                }
                            }

                            if (!isFormart) continue;
                            if (ID == 0)
                            {
                                int NewTableID = db.GetHeadID(pmListbyType[0].WmsHeadID.Split(',')[0]);
                                string TableName = headTableName[0];
                                string WmsVourcherNo = db.GetWmsWoucherNo(pmListbyType[0].WmsVoucherNoRual, TableName, null);
                                string insertHead = String.Format(insertHeadSQL, String.Join(",", headValues.ToArray()), String.Join(",", headInsertKeys.ToArray()).ToString(), headWhereString, NewTableID, TableName, WmsVourcherNo);
                                LogNet.SyncInfo(insertHead);
                                sqlList.Add(insertHead);
                                if (detailValues.Count != 0)
                                {
                                    for (int k = 0; k < detailWhereStringList.Count; k++)
                                    {
                                        //"IF NOT EXISTS (SELECT id FROM {0} WHERE {1}) INSERT INTO {0} ({2},HEADERID,VOUCHERNO,SUBIARRSID) SELECT {3},'{4}','{5}','{6}'";
                                        //   string insertDetail = WmsvouType == "47" ? 
                                        //       String.Format(insertDetailSQL, detailTableName[0], detailWhereStringList[k], String.Join(",", detailInsertKeys.ToArray()).ToString(), String.Join(",", detailValues[k].ToArray()), NewTableID, WmsVourcherNo):
                                        string insertDetail = String.Format(insertDetailSQL, detailTableName[0], detailWhereStringList[k],String.Join(",", detailInsertKeys.ToArray()).ToString(), String.Join(",", detailValues[k].ToArray()), NewTableID, WmsVourcherNo, NewTableID + "-" + k);
                                        LogNet.SyncInfo(insertDetail);
                                        sqlList.Add(insertDetail);

                                    }
                                }
                                if (headTableName.Length > 1)
                                {
                                    int OMSNewTableID = db.GetHeadID(pmListbyType[0].WmsHeadID.Split(',')[1]);
                                    TableName = headTableName[1];
                                    WmsVourcherNo = db.GetWmsWoucherNo(pmListbyType[0].WmsVoucherNoRual + "OMS", TableName, null);
                                    insertHead = String.Format(insertHeadSQL, String.Join(",", headValues.ToArray()), String.Join(",", headInsertKeys.ToArray()).ToString(), headWhereString, OMSNewTableID, TableName, WmsVourcherNo);
                                    sqlList.Add(insertHead);
                                    if (detailValues.Count != 0)
                                    {
                                        for (int k = 0; k < detailWhereStringList.Count; k++)
                                        {
                                            //string insertDetail = WmsvouType == "47" ?
                                            //String.Format(insertDetailSQL, detailTableName[0], detailWhereStringList[k], String.Join(",", detailInsertKeys.ToArray()).ToString(), String.Join(",", detailValues[k].ToArray()), NewTableID, WmsVourcherNo) :
                                            string insertDetail = String.Format(insertDetailSQL, detailTableName[1], detailWhereStringList[k], String.Join(",", detailInsertKeys.ToArray()).ToString(), String.Join(",", detailValues[k].ToArray()), OMSNewTableID, WmsVourcherNo, NewTableID + "-" + k);
                                            sqlList.Add(insertDetail);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                List<string> updateHeadValues = new List<string>();

                                for (int k = 0; k < headUpdateKeys.Count; k++)
                                {
                                    updateHeadValues.Add(headUpdateKeys[k] + "=" + headValues[k]);
                                }

                                //"update {0} set {1} where {2}"
                                string updateHSQL = String.Format(updateHeadSQL, headTableName[0], String.Join(",", updateHeadValues.ToArray()), headWhereString);
                                LogNet.SyncInfo(updateHSQL);
                                sqlList.Add(updateHSQL);
                                if (headTableName.Length > 1)
                                {
                                    updateHSQL = String.Format(updateHeadSQL, headTableName[1], String.Join(",", updateHeadValues.ToArray()), headWhereString);
                                    sqlList.Add(updateHSQL);
                                }



                                for (int l = 0; l < detailWhereStringList.Count; l++)
                                {
                                    List<string> updateDetailValues = new List<string>();
                                    for (int m = 0; m < detailUpdateKeys.Count; m++)
                                    {
                                        updateDetailValues.Add(detailUpdateKeys[m] + "=" + detailValues[l][m]);

                                    }
                                   
                                    if (isNoChangeDetail)
                                    {
                                        string updateDSQL = String.Format(updateDetailSQL, detailTableName[0], String.Join(",", updateDetailValues.ToArray()), detailWhereStringList[l]);
                                        LogNet.SyncInfo(updateDSQL);
                                        sqlList.Add(updateDSQL);
                                        if (headTableName.Length > 1)
                                        {
                                            updateDSQL = String.Format(updateDetailSQL, detailTableName[1], String.Join(",", updateDetailValues.ToArray()), detailWhereStringList[l]);
                                            sqlList.Add(updateDSQL);
                                        }
                                    }
                                    else
                                    {
                                           insertDetailSQL =(WmsvouType=="47" ?SqlModel.InsertU8DetailSql: SqlModel.InsertSAPDetailSql);
                                        string insertDSQL = String.Format(insertDetailSQL, detailTableName[0], detailWhereStringList[l], String.Join(",", detailInsertKeys.ToArray()).ToString(), String.Join(",", detailInsertOrUpdateValues[l].ToArray()), ID, OMDWmsVourcherNo, ID + "-" + (detauLCount+l));
                                        string updateDSQL = String.Format(updateDetailSQL, detailTableName[0], String.Join(",", updateDetailValues.ToArray()), detailWhereStringList[l]);
                                        updateDSQL = insertDSQL+" ELSE "+updateDSQL;
                                        LogNet.SyncInfo(updateDSQL);
                                        sqlList.Add(updateDSQL);
                                        if (headTableName.Length > 1)
                                        {
                                            int OMSID = SyncManager.GetInstance().CheckVoucherNoExit(stockType, headTableName[1], headWhereString);
                                            insertDSQL = String.Format(insertDetailSQL, detailTableName[1], detailWhereStringList[l], String.Join(",", detailInsertKeys.ToArray()).ToString(), String.Join(",", detailInsertOrUpdateValues[l].ToArray()), OMSID, OMDWmsVourcherNo, ID + "-" + (detauLCount + l));
                                            updateDSQL = String.Format(updateDetailSQL, detailTableName[1], String.Join(",", updateDetailValues.ToArray()), detailWhereStringList[l]);
                                            updateDSQL = insertDSQL + " ELSE " + updateDSQL; sqlList.Add(updateDSQL);
                                        }
                                    }
                                }
                               

                            }

                            if (sqlList.Count >= 100)
                            {
                                result = SyncManager.GetInstance().Executrans(sqlList, ref errMsg);
                                sqlList = new List<string>();
                                GC.Collect();
                            }

                        }

                    }
                    if (sqlList.Count != 0)
                    {
                        result = SyncManager.GetInstance().Executrans(sqlList, ref errMsg);
                    }
                    if (result)
                    {
                        if (String.IsNullOrEmpty(erpVoucherNo) && String.IsNullOrEmpty(InJsonData))
                        {
                            syncTime.SyncServerTime = SAPServerTime;
                            //同步成功，更新同步时间
                            SyncTimeManager.GetInstance().InsertOrUpdateSyncTime(syncTime);
                        }
                    }


                }
  
                GC.Collect();
                
            

            }
            catch (Exception ex)
            {
                result = false;
                errMsg += wmsVourcherType + "|" + ex.Message + "\r\n";
            }
            if (wmsVourcherType != 21) LogNet.SyncError(errMsg);
            else LogNet.SyncWarn(errMsg);
            LogNet.SyncInfo(wmsVourcherType + "|"+result + "\r\n");
            return result;
        }


     

















        #region U*同步



        private static string headContent = "HEAD";
        private static string detailContent = "DETAIL";
        private static string customKey = "CUSTOMERCODE";
        private static string warehouseNoKey = "WAREHOUSENO";

     

        /// <summary>
        /// 获取单据同步数据Json
        /// </summary>
        /// <param name="StockType">类型 10：入库 20:出库  99:基础资料</param>
        /// <param name="LastSyncTime">最后同步时间</param>
        /// <param name="ErpVoucherNo">ERP单号</param>
        /// <param name="wmsVourcherType">wms单据类型</param>
        /// <param name="db"></param>
        /// <returns></returns>
        private static string GetErpJson(int StockType, string LastSyncTime, string ErpVoucherNo, ParamaterField_Model Type, ParamaterFiled_DB db)
        {
            //取最后同步时间或者最大ERP单号单号
            //ERP单据号不为空的情况下，查询该单据的最后同步时间
            string lastSyncErpVoucherNo = String.Empty;
            if (!String.IsNullOrEmpty(ErpVoucherNo))
            {
                LastSyncTime = db.GetLastSyncTime(StockType, ErpVoucherNo);
            }
            else
            {
                if (StockType != 99) //99：基础资料同步
                {
                    //获取最大单号特殊处理
                    lastSyncErpVoucherNo = db.getLastSyncErpVoucherNo(StockType, Type.VoucherType.ToString(), Type.ErpVouType,Type.ErpVourcherType);
                }
                    
            }
            BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
            string json = "{\"data_no\":\"" + ErpVoucherNo + "\",\"VoucherType\":\"" + Type.VoucherType.ToString() + "\",\"max_code\":\"" + lastSyncErpVoucherNo + "\",\"edit_time\":\"";
            json += !String.IsNullOrEmpty(LastSyncTime)?DateTime.Parse(LastSyncTime).ToString("yyyy-MM-dd"):"";
            json += "\",\"erp_vourcher_type\":\"" + Type.ErpVourcherType + "\"}";
            
            return TIF.GetModelListByInterface(json);
        }

        /// <summary>
        /// 同步ERP单据
        /// </summary>
        /// <param name="StockType">类型 10：入库 20:出库  99:基础资料</param>
        /// <param name="LastSyncTime">最后同步时间</param>
        /// <param name="ErpVoucherNo">ERP单号</param>
        /// <param name="wmsVourcherType">wms单据类型</param>
        /// <param name="ErrMsg">返回错误信息</param>
        /// <param name="syncType">同步数据来源 ERP或者 EXCEL</param>
        /// <returns></returns>
        public static bool SyncJsonFromErp(int StockType, string LastSyncTime, string ErpVoucherNo, int wmsVourcherType, ref string ErrMsg)
        {
            bool result = false;
            int iresult = 0;

            try
            {
                //根据stockType查询出入库类型（T_PARAMETER表-出入库类型）
                ParamaterFiled_DB db = new ParamaterFiled_DB();
                int type = db.GetRealStockType(StockType);
                //根据单据类型和出库入类型获取同步字段
                List<ParamaterField_Model> pmList = db.GetPmList(type, wmsVourcherType, ref ErrMsg); //同步字段
                if (pmList == null || pmList.Count == 0)
                {
                    ErrMsg += "单据类型未配置！";
                    return false;
                }

                var TypeList = pmList.Where(p=>p.ErpVourcherType != null).DistinctBy(s=> new { s.ErpVourcherType, s.VoucherType }); //获取同步单据类型
               //按照单据类型循环
                foreach (var Type in TypeList)
                {
                    //获取单据同步数据Json
                    string dataJson = String.Empty;
                    string ErpvouType = Type.ErpVourcherType.ToString();
                    string WmsvouType = Type.VoucherType.ToString();

                 //   string time = "";
                 //   System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                //    stopwatch.Start();

                    string Json = //"{\"result\":\"1\",\"resultValue\":\"\",\"data\":[{\"Head\":{\"darvdate\":\"2018-01-02\",\"cvenname\":\"供应商名称1\",\"cvencode\":\"供应商编码1\",\"carvcode\":\"\",\"cbustype\":\"业务类型1\",\"ipurarriveid\":\"到货单ID2\",\"itaxrate\":\"121\",\"iexchrate\":\"131\",\"cexch_name\":\"币种\",\"body\":[{\"iarrsid\":\"到货单子表ID\",\"POID\":\"到货单ID2\",\"cinvcode\":\"存货编码1\",\"cinvname\":\"存货名称1\",\"cinvstd\":\"规格型号1\",\"cvencode\":\"供应商编码1\",\"cbatch\":\"\",\"iNQuantity\":\"2500\"},{\"iarrsid\":\"到货单子表ID3\",\"POID\":\"到货单ID2\",\"cinvcode\":\"存货编码3\",\"cinvname\":\"存货名称3\",\"cinvstd\":\"规格型号3\",\"cvencode\":\"供应商编码3\",\"cbatch\":\"\",\"iNQuantity\":\"900\"}]}},{\"Head\":{\"darvdate\":\"2018-01-03\",\"cvenname\":\"供应商名称2\",\"cvencode\":\"供应商编码2\",\"carvcode\":\"\",\"cbustype\":\"业务类型2\",\"ipurarriveid\":\"到货单ID3\",\"itaxrate\":\"132\",\"iexchrate\":\"142\",\"cexch_name\":\"币种\",\"body\":[{\"iarrsid\":\"到货单子表ID\",\"POID\":\"到货单ID3\",\"cinvcode\":\"存货编码2\",\"cinvname\":\"存货名称2\",\"cinvstd\":\"规格型号2\",\"cvencode\":\"供应商编码\",\"cbatch\":\"\",\"iNQuantity\":\"2500\"}]}}]}";
                        GetErpJson(StockType, LastSyncTime, ErpVoucherNo, Type, db); //获取JSON
                    //LogNet.LogInfo("SyncJsonFromErp:" + Json);
                    //string Json = "{\"result\":\"1\",\"resultValue\":\"\",\"data\":[{\"head\":{\"standard_box2\":null,\"standard_box3\":null,\"sto_condition\":null,\"spc_require\":null,\"protect_way\":null,\"EntId\":null,\"item_spec\":\"个\",\"item_unit\":null,\"group_code\":null,\"group_name\":null,\"classfiy_code\":null,\"classfiy_name\":null,\"purchase_group_code\":null,\"purchase_group_name\":null,\"main_supplier\":null,\"quality_month\":null,\"quality_day\":0,\"item_brand\":null,\"origin_place\":null,\"life_cycle\":null,\"pack_quantity\":0,\"item_size\":null,\"pallet_size\":null,\"pallet_amount\":null,\"all_size\":null,\"item_weight\":null,\"status\":null,\"standard_box1\":null,\"customer\":null,\"standard_box\":null,\"brand_intro\":null,\"bar_code\":\"\",\"Companyid\":null,\"item_name_us\":null,\"item_no\":\"14H22Q1\",\"item_name\":\"ALBION产品托盘\",\"Detail\":[{\"base_num\":0,\"base_unit\":null,\"unit_num\":0,\"Companyid\":\"ABH\",\"from_unit\":null,\"water_code\":\"\",\"item_no\":\"14H22Q1\",\"unit\":null,\"pack_amount\":0,\"EntId\":10,\"USNAM\":null,\"GUID\":null}]}}],\"MaterialDoc\":null,\"MaterialYear\":null,\"QualityNo\":null,\"GUID\":null,\"DeliveryNo\":null}";
                //    time += "获取json：" + stopwatch.Elapsed.TotalSeconds;
                  //  stopwatch.Reset(); stopwatch.Start();
                    
                    //解析JSON格式
                    result = GetDataJson(Json, ref dataJson, WmsvouType, ref ErrMsg);
                    
                    if (result)
                    {
                        ErrMsg += " \r\nerp类型：" + ErpvouType + "\r\n" + dataJson + "\r\n";
                        if (pmList.Count > 0 && !String.IsNullOrEmpty(dataJson.TrimStart('[').TrimEnd(']')))
                        {
                            //单据类型对应字段
                            List<ParamaterField_Model> pmListbyType = pmList.FindAll(p => p.VoucherType.ToString() == WmsvouType && p.ErpVourcherType == ErpvouType);
                            //生成SQL语句
                            List<string> SQList = CreateSqlList(db,pmListbyType, dataJson, wmsVourcherType, ref ErrMsg);
                       //     time += "生成SQL：" + stopwatch.Elapsed.TotalSeconds;
                       //     stopwatch.Reset(); stopwatch.Start();
                            result = db.SaveSqlList(SQList,ref ErrMsg);
                            iresult += 1;
                        //    time += "插入SQL：" + stopwatch.Elapsed.TotalSeconds;
                         //   stopwatch.Reset(); stopwatch.Start();
                            //if (!result)
                            //    break;
                        }
                    }

                  //  time += "解析SQL：" + stopwatch.Elapsed.TotalSeconds;
                  //  stopwatch.Stop();
                }

                if (iresult > 0) { result = true; }

            }
            catch (Exception ex)
            {
                result = false;
                ErrMsg += ex.Message + "|";
                LogNet.LogInfo("SyncJsonFromErp:" + ErrMsg);
            }
            return result;
        }

        /// <summary>
        /// 按照ERP数据生产对应SQL
        /// </summary>
        /// <param name="pmListbyType">同步字段</param>
        /// <param name="dataJson">ERP数据</param>
        /// <param name="wmsVourcherType">WMS单据类型</param>
        /// <param name="errMsg">错误消息</param>
        /// <param name="syncType">同步类型 10：入库 20:出库  99:基础资料</param>
        /// <returns></returns>
        private static List<string> CreateSqlList(ParamaterFiled_DB db,List<ParamaterField_Model> pmListbyType,
            string dataJson, int wmsVourcherType, ref string errMsg)
        {
            List<string> SQLIST = new List<string>();
            string[] Headkeys = pmListbyType[0].HEADKEYS.ToString().Split(','); //表头主键
            string[] Detailkeys = pmListbyType[0].DETAILKEYS.ToString().Split(',');//表体主键
            string[] MatrtialKeys = pmListbyType[0].MATERIALNOKEYS.ToString().Split(',');
            string headTableName = pmListbyType[0].WMSTableNameH.ToString(); //表头名称
            string detailTableName = pmListbyType[0].WMSTableNameD.ToString();//表体名称

            JArray headJarray = JArray.Parse(dataJson);
            for (int i = 0; i < headJarray.Count; i++)
            {
                
                ////------------测试用
                //if (i == 2)
                //    break;
                ////--------------
                JToken jToken = JObject.Parse(headJarray[i].ToString());

                foreach (JProperty jp in jToken)
                {
                    if (jp.Name.ToUpper().Equals(headContent))
                    {
                        string WmsVourcherNo = String.Empty;
                        JToken headJToken = JObject.Parse(jp.Value.ToString());
                        //表头SQL
                        int headID = 0;
                        List<int> BeforeIDs = new List<int>();  //同步前表体ID集合
                        List<int> AfterIDs = new List<int>();  //同步后表体ID集合
                        string headsql = GetSql(headJToken,db, pmListbyType, Headkeys, MatrtialKeys, headTableName,true,ref headID,ref WmsVourcherNo,ref BeforeIDs,ref AfterIDs);
                        SQLIST.Add(headsql);
                        foreach (JProperty hjp in headJToken)
                        {
                            if (hjp.Name.ToUpper().Equals(detailContent))
                            {
                                if (!String.IsNullOrEmpty(hjp.Value.ToString()))
                                {
                                    //表体SQL
                                    JArray detailJarray = JArray.Parse(hjp.Value.ToString());
                                    for (int j = 0; j < detailJarray.Count; j++)
                                    {
                                        JToken detailJToken = JObject.Parse(detailJarray[j].ToString());
                                        string detailsql = GetSql(detailJToken, db, pmListbyType, Detailkeys, MatrtialKeys, detailTableName, false, ref headID, ref WmsVourcherNo, ref BeforeIDs, ref AfterIDs);
                                        SQLIST.Add(detailsql);
                                    }
                                }
                            }
                        }
                        //删除sql
                        //List<int> diffID = BeforeIDs.Where(a => !AfterIDs.Exists(t => a == t)).ToList();
                        //if (diffID.Count != 0)
                        //{
                        //    string ids = String.Join(",", diffID.ToArray());
                        //    string sql = String.Format(SqlModel.DeleteSql, detailTableName, ids);
                        //    //零时修改，等老余修复问题之后，需要恢复2018-08-06
                        //    SQLIST.Add(sql);
                        //}

                    }
                }
            }
            return SQLIST;
        }

        /// <summary>
        /// 获取SQL
        /// </summary>
        private static string  GetSql(JToken JToken,ParamaterFiled_DB db, List<ParamaterField_Model> pmListbyType, 
            string[] keys, string[] MatrtialKeys, string TableName,bool isHead,ref int headID,
            ref string WmsHeadVourcherNo, ref List<int> BeforeIDs, ref List<int> AfterIDs)
        {
            string sql = String.Empty;
            string whereString = GetwhereString(isHead, keys, pmListbyType, JToken);
          

            int TableID = db.checkRecode(whereString, pmListbyType, JToken, TableName);
            if (TableID == 0) //新增
            {
                sql = GetInsertSql(JToken, db, pmListbyType, MatrtialKeys, TableName, isHead, ref headID, ref WmsHeadVourcherNo, whereString);
            }
            else //修改
            {
                if (isHead)//获取修改前的表体ID集合
                {
                    headID = TableID;
                    WmsHeadVourcherNo = db.GetWmsWoucherNo(pmListbyType[0].WMSTableNameH, TableID);
                    if(!String.IsNullOrEmpty(pmListbyType[0].WMSTableNameD))
                        BeforeIDs = db.getDetailIDs(pmListbyType[0].WMSTableNameD, TableID);
                }
                else //获取修改表体ID
                {
                    AfterIDs.Add(TableID);
                }
                sql = GetUpdateSql(JToken, db, pmListbyType, MatrtialKeys, TableName, isHead, whereString);
            }

            return sql;
        }

    

        /// <summary>
        /// 获取updateSql
        /// </summary>
        /// <returns></returns>
        private static string GetUpdateSql(JToken JToken, ParamaterFiled_DB db, List<ParamaterField_Model> pmListbyType, 
            string[] MatrtialKeys, string TableName, bool isHead, string whereString)
        {
            string sql;
            StringBuilder valueList = new StringBuilder();
            foreach (JProperty jp in JToken)
            {
                int index = pmListbyType.FindIndex(p => p.ERPField.ToUpper() == jp.Name.ToUpper() && p.FieldHD.ToUpper() == (isHead ? "H" : "D"));
                if (index != -1)
                {
                    string value = FilteSQLStr(jp.Value.ToString());
                    value = pmListbyType[index].DefaultType==0? "'" + pmListbyType[index].DefaultValue + "',":pmListbyType[index].DefaultType == 1 ? "CONVERT(varchar(100),'" + value + "', 20)," : "'" + value + "',";
                    valueList.Append(pmListbyType[index].WMSField + "=" + value);
                }
            }
            var defaultValueList = pmListbyType.FindAll(p => !String.IsNullOrEmpty(p.DefaultValue) && (p.DefaultType == 2  || p.DefaultType==3 || p.DefaultType == 4 || p.DefaultType == 5)
            && p.FieldHD.ToUpper() == (isHead ? "H" : "D")
              && String.IsNullOrEmpty(p.ERPField)).DistinctBy(s => new { s.WMSField, s.DefaultValue });
            foreach (var pmodel in defaultValueList)
            {
                if (pmodel.DefaultType == 2)
                {
                    string materialSubSql = String.Format(SqlModel.GetmaterialIdsql, GetwhereString(isHead, MatrtialKeys, pmListbyType, JToken));
                    materialSubSql = "(" + materialSubSql + ")";
                    valueList.Append(pmodel.WMSField + "=" + materialSubSql + ",");
                }
                //出库单自动分配客户对应发货仓库 只有发货单需要处理
                if (pmodel.DefaultType == 3 || pmodel.DefaultType==4)
                {
                    int index = pmListbyType.FindIndex(p => p.WMSField.ToUpper() == customKey && p.FieldHD.ToUpper() == "D");
                    if (index != -1)
                    {
                        string customerno = ((JObject)JToken).GetValue(pmListbyType[index].ERPField.ToString()).ToString();
                        string warehouseIDSubSql = String.Format(pmodel.DefaultType == 3?SqlModel.GetWhareHouseID: SqlModel.GetWhareHouseNo, customerno);
                        valueList.Append(pmodel.WMSField + "=(" + warehouseIDSubSql + "),");
                    }
                }
                //调拨单获取warehouseid
                if (pmodel.DefaultType == 5)
                {
                    int index = pmListbyType.FindIndex(p => p.WMSField.ToUpper() == warehouseNoKey && p.FieldHD.ToUpper() == "D");
                    if (index != -1)
                    {
                        string warehouseNo = ((JObject)JToken).GetValue(pmListbyType[index].ERPField.ToString()).ToString();
                        string warehouseIDSubSql = String.Format(SqlModel.GetWhareHouseIDByNo, warehouseNo);
                        valueList.Append(pmodel.WMSField + "=(" + warehouseIDSubSql + "),");
                    }
                }
            }
            // "update {0} set {1} where 1=1 and {2}";
            sql = String.Format(SqlModel.UpdateSql, TableName, (isHead ? "LASTSYNCTIME=GETDATE()," : "") + valueList.Remove(valueList.Length - 1, 1), whereString);
            return sql;
        }

        /// <summary>
        /// 获取insertsql
        /// </summary>
        /// <returns></returns>
        private static string GetInsertSql(JToken JToken, ParamaterFiled_DB db, List<ParamaterField_Model> pmListbyType,
            string[] MatrtialKeys, string TableName, bool isHead, ref int headID, ref string WmsHeadVourcherNo, string whereString)
        {
            string sql;
            string WmsVourcherNo = isHead ? db.GetWmsWoucherNo(pmListbyType[0].VOUCHERNO, pmListbyType[0].WMSTableNameH, WmsHeadVourcherNo) : "";
            int NewTableID = isHead ? db.GetHeadID(pmListbyType[0].HEADID) : 0;
            StringBuilder contextList = new StringBuilder();
            StringBuilder valueList = new StringBuilder();
            foreach (JProperty jp in JToken)
            {
                int index = pmListbyType.FindIndex(p => p.ERPField.ToUpper() == jp.Name.ToUpper() && p.FieldHD.ToUpper() == (isHead ? "H" : "D"));
                if (index != -1)
                {
                    string value =  FilteSQLStr(jp.Value.ToString());
                    value = pmListbyType[index].DefaultType==0? "'" + pmListbyType[index].DefaultValue + "',":pmListbyType[index].DefaultType == 1 ? "CONVERT(varchar(100),'" + value + "', 20)," : "'" + value + "',";

                    contextList.Append(pmListbyType[index].WMSField + ",");
                    valueList.Append(value.Trim());
                }
            }

            var defaultValueList = pmListbyType.FindAll(p => !String.IsNullOrEmpty(p.DefaultValue) && p.FieldHD.ToUpper() == (isHead ? "H" : "D")
            && String.IsNullOrEmpty(p.ERPField)).DistinctBy(s => new { s.WMSField, s.DefaultValue });

            foreach (var pmodel in defaultValueList)
            {
                string value = pmodel.DefaultValue;
                if (pmodel.DefaultType == 2)
                {
                    string materialSubSql = String.Format(SqlModel.GetmaterialIdsql, GetwhereString(isHead, MatrtialKeys, pmListbyType, JToken));
                    value = "(" + materialSubSql + ")";
                }
                //出库单自动分配客户对应发货仓库 只有发货单需要处理
                if (pmodel.DefaultType == 3 || pmodel.DefaultType == 4 )
                {
                    int index = pmListbyType.FindIndex(p => p.WMSField.ToUpper() == customKey && p.FieldHD.ToUpper() == "D");
                    if (index != -1)
                    {
                        string customerno = ((JObject)JToken).GetValue(pmListbyType[index].ERPField.ToString()).ToString();
                        string warehouseIDSubSql = String.Format(pmodel.DefaultType == 3 ? SqlModel.GetWhareHouseID : SqlModel.GetWhareHouseNo, customerno);
                        value = "(" + warehouseIDSubSql + ")";
                    }
                }
                //调拨单获取warehouseid
                if (pmodel.DefaultType == 5)
                {
                    int index = pmListbyType.FindIndex(p => p.WMSField.ToUpper() == warehouseNoKey && p.FieldHD.ToUpper() == "D");
                    if (index != -1)
                    {
                        string warehouseNo = ((JObject)JToken).GetValue(pmListbyType[index].ERPField.ToString()).ToString();
                        string warehouseIDSubSql = String.Format(SqlModel.GetWhareHouseIDByNo, warehouseNo);
                        value = "(" + warehouseIDSubSql + ")";
                    }
                }
                contextList.Append(pmodel.WMSField + ",");
                valueList.Append(value + ",");
            }
            //"IF NOT EXISTS (SELECT id FROM {0} WHERE {1}) INSERT INTO {0} ({2},ID,VOUCHERNO,VOUCHERTYPE) SELECT {3},{4},'{5}','{6}'"
            if (isHead)
            {
                headID = NewTableID;
                WmsHeadVourcherNo = WmsVourcherNo;
                sql =
                    String.Format(SqlModel.InsertTitleSql, TableName, whereString,
                    contextList.Remove(contextList.Length - 1, 1), valueList.Remove(valueList.Length - 1, 1),
                    NewTableID, WmsVourcherNo, pmListbyType[0].ErpVourcherType, CheckVoucherType(pmListbyType[0].VoucherType));
            }
            else
                //IF NOT EXISTS (SELECT id FROM {0} WHERE {1} 1=1) INSERT INTO {0} ({2},HEADERID,VOUCHERNO) SELECT {3},'{4}','{5}'
                sql = String.Format(SqlModel.InsertDetailSql, TableName, whereString,
                    contextList.Remove(contextList.Length - 1, 1), valueList.Remove(valueList.Length - 1, 1), headID, WmsHeadVourcherNo);
            return sql;
        }


        private static string GetwhereString(bool isHead, string[] keys, List<ParamaterField_Model> pmListbyType, JToken JToken)
        {
            string whereString = String.Empty;
            foreach (string key in keys)
            {
                int index = pmListbyType.FindIndex(p => p.WMSField.ToUpper() == key.ToUpper() && p.FieldHD.ToUpper() == (isHead ? "H" : "D"));
                if (index != -1)
                {
                    whereString += pmListbyType[index].WMSField + "='" + ((JObject)JToken).GetValue(pmListbyType[index].ERPField.ToString()) + "' and ";
                }
            }

            return whereString;
        }

        /// <summary>
        /// 解析JSON
        /// </summary>
        /// <param name="json">ERP-json</param>
        /// <param name="dataJson">返回数据JSON</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private static bool GetDataJson(string json, ref string dataJson,string wmstypeName, ref string errMsg)
        {
            bool result = false;
            JsonModel jsmodel = JsonConvert.DeserializeObject<JsonModel>(json);
            if (jsmodel.result.ToString()=="1")
            {
                result = true;
                if (jsmodel.data != null)
                {
                    dataJson = jsmodel.data.ToString().Trim();
                }
            }
            else
            {
                errMsg += wmstypeName+"|"+jsmodel.resultValue+ "\r\n";
            }
            return result;
        }


        /// <summary>
        /// 特殊处理  将29转换成22 34 转 31
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static int CheckVoucherType(int type)
        {

            switch (type)
            {
                case 29:
                    type = 22;
                    break;
                case 34:
                    type = 31;
                    break;
                case 37:
                    type = 21;
                    break;
                case 38:
                    type = 35;
                    break;
            }
          
            return type;
        }
        #endregion


        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        private static string FilteSQLStr(string Str)
        {

            Str = Str.Replace("'", "");
            Str = Str.Replace("\"", "");
            Str = Str.Replace("&", "&amp");
            Str = Str.Replace("<", "&lt");
            Str = Str.Replace(">", "&gt");

            Str = Str.Replace("delete", "");
            Str = Str.Replace("update", "");
            Str = Str.Replace("insert", "");

            return Str;
        }
    }
}
