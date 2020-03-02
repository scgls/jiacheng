using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.User;
using Oracle.ManagedDataAccess.Client;
using BILBasic.DBA;
using BILBasic.Common;
using Newtonsoft.Json.Linq;
using System.Data;

namespace BILWeb.SyncService
{
    public class ParamaterFiled_DB : BILBasic.Basing.Factory.Base_DB<ParamaterField_Model>
    {
        protected override IDataParameter[] GetSaveModelIDataParameter(ParamaterField_Model model)
        {
            throw new NotImplementedException();
        }

        protected override string GetSaveProcedureName()
        {
            throw new NotImplementedException();
        }

        protected override List<string> GetSaveSql(UserModel user, ref ParamaterField_Model model)
        {
            throw new NotImplementedException();
        }


        protected override string GetTableName()
        {
            return "V_PARAMETERTABLE";
        }

        protected override string GetViewName()
        {
            return "V_PARAMETERTABLE";
        }

        protected override ParamaterField_Model ToModel(IDataReader reader)
        {
            ParamaterField_Model pmodel = new ParamaterField_Model();
            pmodel.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            pmodel.InStockType = dbFactory.ToModelValue(reader, "InStockType").ToInt32();
            pmodel.VoucherType = dbFactory.ToModelValue(reader, "VoucherType").ToInt32();
            pmodel.ErpVouType = dbFactory.ToModelValue(reader, "ErpVouType").ToInt32();
            pmodel.WMSTableNameH = dbFactory.ToModelValue(reader, "WMSTableNameH").ToDBString();
            pmodel.WMSTableNameD = dbFactory.ToModelValue(reader, "WMSTableNameD").ToDBString();
            pmodel.HEADID = dbFactory.ToModelValue(reader, "HEADID").ToDBString();
            pmodel.DETAILID = dbFactory.ToModelValue(reader, "DETAILID").ToDBString();
            pmodel.VOUCHERNO = dbFactory.ToModelValue(reader, "VOUCHERNO").ToDBString();
            pmodel.ExcelTitleLangrage = dbFactory.ToModelValue(reader, "ExcelTitleLangrage").ToDBString();
            pmodel.FieldHD = dbFactory.ToModelValue(reader, "FieldHD").ToString();
            pmodel.XlsNameEN = dbFactory.ToModelValue(reader, "XlsNameEN").ToDBString();
            pmodel.XlsNameCN = dbFactory.ToModelValue(reader, "XlsNameCN").ToDBString();
            pmodel.WMSField = dbFactory.ToModelValue(reader, "WMSField").ToDBString();
            pmodel.ERPField = dbFactory.ToModelValue(reader, "ERPField").ToDBString();
            pmodel.DefaultValue = dbFactory.ToModelValue(reader, "DefaultValue").ToDBString();
            pmodel.HEADKEYS = dbFactory.ToModelValue(reader, "HEADKEYS").ToDBString();
            pmodel.DETAILKEYS = dbFactory.ToModelValue(reader, "DETAILKEYS").ToDBString();
            pmodel.DefaultType = dbFactory.ToModelValue(reader, "DefaultType").ToInt32();
            pmodel.ErpVourcherType = dbFactory.ToModelValue(reader, "ErpVourcherType").ToDBString();
            pmodel.CompanyNo = dbFactory.ToModelValue(reader, "CompanyNo").ToDBString();
            pmodel.MATERIALNOKEYS = dbFactory.ToModelValue(reader, "MATERIALNOKEYS").ToDBString();
            return pmodel;
        }


        /// <summary>
        /// 根据stockType查询出入库类型（T_PARAMETER表-出入库类型）
        /// </summary>
        /// <param name="type">出入库类型</param>
        /// <returns></returns>
        public int GetStockType(int type)
        {
            string sql = "SELECT PARAMETERNAME FROM T_PARAMETER WHERE GROUPNAME = 'Sync_Type' AND PARAMETERID = {0}";
            return base.GetScalarBySql(String.Format(sql, type)).ToInt32();
        }

        /// <summary>
        /// 获取ERP单据最后同步时间
        /// </summary>
        /// <param name="stockType">同步类型 10 ：入库  20：出库</param>
        /// <param name="ErpVoucherNo"></param>
        /// <returns></returns>
        public string GetLastSyncTime(int stockType, string ErpVoucherNo, string companyNo, string erpvouType)
        {
            string tableName = stockType == 10 ? "T_INSTOCK" : "T_OUTSTOCK";
            string sql = "SELECT LastSyncTIme FROM " + tableName + " WHERE ErpVoucherNo = '" + ErpVoucherNo + "' and STRONGHOLDCODE='"+ companyNo + "' and ERPVOUCHERTYPE='"+ erpvouType + "'";
            string time = base.GetScalarBySql(sql).ToDBString();
            return String.IsNullOrEmpty(time) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : time;
        }

        internal string getLastSyncErpVoucherNo(int stockType, string wmsVoucherType, string companyNo, string erpvouType)
        {
            string tableName = stockType == 10 ? 
                (erpvouType.ToUpper().Contains("QC")? "T_QUALITY_SYNC_VIEW" : "T_INTSTOCK_SYNC_VIEW" ): (erpvouType.ToUpper().Contains("GD") ? "T_WOINFO_SYNC_VIEW" : "T_OUTTSTOCK_SYNC_VIEW");
            string sql = "SELECT ErpVoucherNo FROM " + tableName + " WHERE VourcherType = " + wmsVoucherType + " and CompanyNo='" + companyNo + "' and ErpVourcherType='" + erpvouType + "'";
            string ErpVoucherNo = base.GetScalarBySql(sql).ToDBString();
            return String.IsNullOrEmpty(ErpVoucherNo) ? "0" : ErpVoucherNo;
        }


        internal bool ComparerListAndCreateSQL(List<ParamaterField_Model> pmList, int stockType, string dataJson, string syncType, ref string errMsg)
        {
            JArray jarray = JArray.Parse(dataJson);
            string sql = String.Empty;
            List<string> sqlList = new List<string>(); //执行sql语句
            List<deleteModel> deleteModels = new List<deleteModel>();
            for (int i = 0; i < jarray.Count; i++)
            {
                var jObject = JObject.Parse(jarray[i].ToString());
                int VoucherType =Convert.ToInt32(jObject.GetValue("WmsVoucherType").ToString());
                List<ParamaterField_Model> pmListbyType = pmList.FindAll(p => p.VoucherType == VoucherType);
                if (pmListbyType.Count != 0)
                {
                    CreateSqlByJson(pmListbyType, syncType, ref sql, sqlList, ref deleteModels, jObject, VoucherType, pmListbyType);
                }
                createDeleteSql(pmListbyType, sqlList, deleteModels);
                deleteModels = new List<deleteModel>();
            }
            return base.SaveModelListBySqlToDB(sqlList, ref errMsg);
        }

        public bool WOComparerListAndCreateSQL(List<ParamaterField_Model> pmList, string dataJson, string ErpvouType,int VoucherType,ref List<WOReturnModel> WOReturns, ref string errMsg, string syncType)
        {
            JArray jarray = JArray.Parse(dataJson);
            string sql = String.Empty;
            List<string> sqlList = new List<string>(); //执行sql语句
            List<deleteModel> deleteModels = new List<deleteModel>();

            for (int i = 0; i < jarray.Count; i++)
            {
                var Head = JObject.Parse(jarray[i]["Head"].ToString());
                //0:未同步  3：有变更
                if (!(Head["mes_stus"].ToString() == "0" || Head["mes_stus"].ToString() == "3"))
                    continue;
                WOReturnModel woreturn = new WOReturnModel();
                woreturn.ErpVoucherNo = Head["wo_no"].ToString();
                woreturn.ERPVoucherType =  ErpvouType;
                woreturn.StrongHoldCode = woreturn.ErpVoucherNo.Split('-')[0];
                woreturn.WmsStatus = 1;
                woreturn.VoucherType = 88;
                WOReturns.Add(woreturn);

                var detail = Head["Detail"];
                Head.Remove("Detail");
                JObject j = new JObject();
                j.Add(new JProperty("Head", Head));
                j.Add(new JProperty("Detail", detail));
                CreateSqlByJson(pmList, syncType, ref sql, sqlList, ref deleteModels, j, VoucherType, pmList);
            }

            createDeleteSql(pmList, sqlList, deleteModels);

            return base.SaveModelListBySqlToDB(sqlList, ref errMsg);
        }


        public bool ComparerListAndCreateSQL(List<ParamaterField_Model> pmList, string dataJson, string ErpvouType, int VoucherType, ref List<OrderReturnModel> orderReturns , ref string errMsg, string syncType)
        {
            JArray jarray = JArray.Parse(dataJson);
            string sql = String.Empty;
            List<string> sqlList = new List<string>(); //执行sql语句
            List<deleteModel> deleteModels = new List<deleteModel>();

            for (int i = 0; i < jarray.Count; i++)
            {
                var Head = JObject.Parse(jarray[i]["head"].ToString());

                //if (VoucherType == 23 || VoucherType == 24)
                //{
                //    OrderReturnModel orderReturn = new OrderReturnModel();
                //    orderReturn.ErpVoucherNo = VoucherType == 23?Head["trans_no"].ToString(): Head["ship_no"].ToString();
                //    orderReturn.ERPVoucherType = ErpvouType;
                //    orderReturn.WmsStatus = "S";
                //    orderReturn.VoucherType = 9993;
                //    orderReturn.StrongHoldCode = orderReturn.ErpVoucherNo.Split('-')[0];
                //    orderReturns.Add(orderReturn);
                //}

                // jObject["Head"]["Detail"].Remove();
                var detail = Head["Detail"];
                Head.Remove("Detail");
                JObject j = new JObject();
                j.Add(new JProperty("head", Head));
                j.Add(new JProperty("Detail", detail));
                CreateSqlByJson(pmList, syncType, ref sql, sqlList, ref deleteModels, j, VoucherType, pmList);
            }

            createDeleteSql(pmList, sqlList, deleteModels);

             return base.SaveModelListBySqlToDB(sqlList, ref errMsg);
        }

        private void CreateSqlByJson(List<ParamaterField_Model> pmList, string syncType, ref string sql, List<string> sqlList, ref List<deleteModel> deleteModels, JObject jObject, int VoucherType, List<ParamaterField_Model> pmListbyType)
        {
            string WmsVourcherNoSQL = pmListbyType[0].VOUCHERNO;  //wms单据号
            int HeadtaskID = base.GetTableIDBySqlServer(pmListbyType[0].HEADID); //表头ID序号
            string WmsVourcherNo = WmsVourcherNoSQL.Equals("")?"" :base.GetScalarBySql(WmsVourcherNoSQL).ToString();
            foreach (KeyValuePair<string, JToken> keyValuePair in jObject)
            {
                string key = keyValuePair.Key;
                JToken jtokenArr = keyValuePair.Value;

                if (key.ToUpper() == "STATUS")
                {
                    continue;
                }

                bool isHead = key.ToUpper() == "HEAD";
                if (jtokenArr.GetType().Name.ToUpper() == "JOBJECT")
                {
                    sql = CreateSQL(pmList, isHead, VoucherType, syncType, HeadtaskID, WmsVourcherNo,
                        jtokenArr, ref deleteModels);
                    sqlList.Add(sql);
         
                }
                else
                {
                    foreach (JToken jtoken in jtokenArr)
                    {
                        sql = CreateSQL(pmList, isHead, VoucherType, syncType, HeadtaskID, WmsVourcherNo,
                            jtoken, ref deleteModels);
                        sqlList.Add(sql);

                      
                    }
                }
            }
        }


        private static void createDeleteSql(List<ParamaterField_Model> pmList, List<string> sqlList, List<deleteModel> deleteModels)
        {
            if (deleteModels.Count != 0)
            {
                //string sqlDelete = "delete from {0} where ERPVOUCHERNO ='{1}'  and ROWNO not in ('{2}')";
                string sqlDelete = "DELETE from {0} where ERPVOUCHERNO ='{1}' AND \"ID\" NOT IN( SELECT \"ID\"  from {0} where ERPVOUCHERNO = '{1}'  and ";
                //(ROWNO = '20' AND ROWNODEL  IN('1', '2') or (ROWNO = '31' AND ROWNODEL  IN('1', '2'))))

                foreach (deleteModel del in deleteModels)
                {
                    List<RowDelModel> RowModels = del.ROW;
                    string sqldel = String.Format(sqlDelete, pmList[0].WMSTableNameD, del.ERPVOUCHERNO);
                    string delsql1 = "(ROWNO = '{0}' AND ROWNODEL  IN('{1}'))";
                    string delsql2 = "ROWNO = '{0}'";
                    foreach (RowDelModel rowdel in RowModels)
                    {
                        string ROWNODEL = String.Join("','", rowdel.RowDel.ToArray());
                        string delrowsql = String.IsNullOrEmpty(ROWNODEL)? String.Format(delsql2, rowdel.ROWNO):  String.Format(delsql1, rowdel.ROWNO, ROWNODEL);
                        sqldel += delrowsql + " or ";
                    }
                    sqlList.Add(sqldel.Substring(0,sqldel.Length-4)+")");
                }
            }
        }

        private string CreateSQL(List<ParamaterField_Model> pmList, bool isHead, int VoucherType, string syncType,
            int HeadtaskID, string WmsVourcherNo, JToken jtoken,
           ref List<deleteModel> deleteModels)
        {
            string tableName = isHead ? pmList[0].WMSTableNameH : pmList[0].WMSTableNameD; //表名
            string sqlInsertTitle = "set identity_insert {0} ON;IF NOT EXISTS (SELECT id FROM {0} WHERE {1}) INSERT INTO {0} ({2},ID,VOUCHERNO,VOUCHERTYPE,STRONGHOLDCODE) SELECT {3},'{4}','{5}';set identity_insert {0} off;";//"insert when (not exists (@)) then into {0}(ID,VOUCHERNO,VOUCHERTYPE,{4}) values({1},{2},{3},{5}) select * from  dual";
            string sqlInsertDetail = "IF NOT EXISTS (SELECT id FROM {0} WHERE {1}) INSERT INTO {0} ({2},HEADERID,VOUCHERNO,SUBIARRSID) SELECT {3},'{4}','{5}'";//"insert when (not exists (@)) then into {0}(ID,HEADERID,VOUCHERNO,{4}) values({1},{2},{3},{5}) select * from  dual";
            string sqlUpdate = "update {0} set {1} where 1=1 and {2}";
            string checkSQL = "select count(*) from {0} WHere 1=1 and {1}";
            string SubSQL = "select ID from {0} WHere 1=1 and {1}";
            string sql = "";
            string whereHeadString = String.Empty;
            string whereDetailString = String.Empty;
            string[] Headkeys = pmList[0].HEADKEYS.ToString().Split(',') ;
            string[] Detailkeys = pmList[0].DETAILKEYS.ToString().Split(',');
            //foreach (string key in Headkeys)
            //{
            //    int index = pmList.FindIndex(p => (syncType.ToUpper() == "ERP" ? p.WMSField.ToUpper() :
            //   (p.ExcelTitleLangrage.ToUpper() == "E" ? p.XlsNameEN : p.XlsNameCN)) == key.ToUpper());
            //    if (index != -1)
            //    {
            //        whereHeadString += pmList[index].WMSField + "='" + ((JObject)jtoken).GetValue(pmList[index].ERPField.ToString()) + "' and ";
            //     }
                  
            //}
            //if (!isHead)
            //{
            //    foreach (string key in Detailkeys)
            //    {
            //        int index = pmList.FindIndex(p => (syncType.ToUpper() == "ERP" ? p.WMSField.ToUpper() :
            //       (p.ExcelTitleLangrage.ToUpper() == "E" ? p.XlsNameEN : p.XlsNameCN)) == key.ToUpper());
            //        if (index != -1)
            //        {
            //            whereDetailString += pmList[index].WMSField + "='" + ((JObject)jtoken).GetValue(pmList[index].ERPField.ToString()) + "' and ";
            //        }

            //    }

            //}

            foreach (string key in Headkeys)
            {
                int index = pmList.FindIndex(p => (syncType.ToUpper() == "ERP" ? p.WMSField.ToUpper() :
               (p.ExcelTitleLangrage.ToUpper() == "E" ? p.XlsNameEN : p.XlsNameCN)) == key.ToUpper() && p.FieldHD == "H");
                if (index != -1)
                {
                    whereHeadString += pmList[index].WMSField + "='" + ((JObject)jtoken).GetValue(pmList[index].ERPField.ToString()) + "' and ";
                }

            }
            if (!isHead)
            {
                foreach (string key in Detailkeys)
                {
                    int index = pmList.FindIndex(p => (syncType.ToUpper() == "ERP" ? p.WMSField.ToUpper() :
                   (p.ExcelTitleLangrage.ToUpper() == "E" ? p.XlsNameEN : p.XlsNameCN)) == key.ToUpper() && p.FieldHD == "D");
                    if (index != -1)
                    {
                        whereDetailString += pmList[index].WMSField + "='" + ((JObject)jtoken).GetValue(pmList[index].ERPField.ToString()) + "' and ";
                    }

                }

            }

            if (pmList[0].ErpVourcherType.ToUpper().Contains("QC") && !isHead)
            {
                string delsql = "delete from " + tableName + " where " + whereHeadString+" 1=1";
                base.GetExecuteNonQuery(delsql);
            }


            string whereString = (isHead ? whereHeadString+ "VOUCHERTYPE=" + VoucherType +" and " : whereDetailString)+" 1=1";

            string[] MATERIALNOKEYS = pmList[0].MATERIALNOKEYS.ToString().Split(',');
            string whereMATERIALNOString = String.Empty;
            foreach (string key in MATERIALNOKEYS)
            {
                int index = pmList.FindIndex(p => (syncType.ToUpper() == "ERP" ? p.WMSField.ToUpper() :
               (p.ExcelTitleLangrage.ToUpper() == "E" ? p.XlsNameEN : p.XlsNameCN)) == key.ToUpper());
                if (index != -1)
                {
                    whereMATERIALNOString += pmList[index].WMSField + "='" + ((JObject)jtoken).GetValue(pmList[index].ERPField.ToString()) + "' and ";
                }

            }
            string GetmaterialNoIdsql = "select id from T_MATERIAL where " + whereMATERIALNOString + " 1=1";

            if (!String.IsNullOrEmpty(whereString))
            {
                SubSQL = String.Format(SubSQL, tableName, whereString);
                checkSQL = String.Format(checkSQL, tableName, whereString);
                if (Convert.ToInt32(base.GetScalarBySql(checkSQL).ToString()) == 0)
                {
                    string checkheadidsql = "select id from " + pmList[0].WMSTableNameH + " where " + whereHeadString + " 1=1";
                    object temp = base.GetScalarBySql(checkheadidsql);
                    int headid = temp == null ? 0 : Convert.ToInt32(temp.ToString());
                    if (headid != 0) HeadtaskID = headid;
                    string insertsql = isHead ? sqlInsertTitle : sqlInsertDetail;
                    insertsql = insertsql.Replace("@", SubSQL);
                    sql = CreateInsertSql(pmList, insertsql, WmsVourcherNo,
                                isHead, tableName, HeadtaskID, VoucherType, jtoken, syncType, GetmaterialNoIdsql);
                }
                else
                {
                    //表体需要记录有的数据，最后删除不在此范围内数据
                    sql = CreateUpdateSql(pmList,sqlUpdate,isHead,tableName, jtoken, syncType,whereString, GetmaterialNoIdsql);
                }
                GetDeletePara(pmList, isHead, jtoken, syncType, ref deleteModels);
            }
            return sql;
        }

        private string CreateUpdateSql(List<ParamaterField_Model> pmList, string sql, bool isHead, string tableName, JToken jtoken,string syncType,string whereString,
            string GetmaterialNoIdsql)
        {
            StringBuilder valueList = new StringBuilder();
       
            int index = -1;
            foreach (JProperty jp in jtoken)
            {
                index = pmList.FindIndex(p => (syncType.ToUpper() == "ERP" ? p.ERPField.ToUpper() :
                  (p.ExcelTitleLangrage.ToUpper() == "E" ? p.XlsNameEN : p.XlsNameCN)) == jp.Name.ToUpper() && p.FieldHD.ToUpper() == (isHead ? "H" : "D"));
                if (index != -1)
                {
                    string value = jp.Value.ToString().Replace("'", "''");
                    value = pmList[index].DefaultType == 1 ? "to_date('" + value + "','yyyy-mm-dd hh24:mi:ss')," : "'" + value + "',";
                    valueList.Append(pmList[index].WMSField + "="+value.Trim().Replace("；", ";").Replace(" ", " "));
                }
            }
            var defaultValueList = pmList.FindAll(
              p => !String.IsNullOrEmpty(p.DefaultValue) && p.DefaultType==2 && p.FieldHD.ToUpper() == (isHead ? "H" : "D") && String.IsNullOrEmpty(p.ERPField)).DistinctBy(s => new { s.WMSField, s.DefaultValue });
            foreach (var pmodel in defaultValueList)
            {
                string value = "(" + GetmaterialNoIdsql + ")";
                valueList.Append(pmodel.WMSField + "=" + value + ",");
            }

            sql = String.Format(sql, tableName, (isHead ? "LASTSYNCTIME=SYSDATE," : "") + valueList.Remove(valueList.Length - 1, 1), whereString);
            return sql;
        }

        private static void GetDeletePara(List<ParamaterField_Model> pmList, bool isHead, JToken jtoken, string syncType,ref List<deleteModel> deleteModels)
        {
            int index = -1;
            if (!isHead)
            {
                index = pmList.FindIndex(p => (syncType.ToUpper() == "ERP" ? p.WMSField.ToUpper() :
                 (p.ExcelTitleLangrage.ToUpper() == "E" ? p.XlsNameEN : p.XlsNameCN)) == "ERPVOUCHERNO" && p.FieldHD.ToUpper() == "D");
                string ErpVoucherNo = index != -1 ? ((JObject)jtoken).GetValue(pmList[index].ERPField).ToString() : "";

                index = pmList.FindIndex(p => (syncType.ToUpper() == "ERP" ? p.WMSField.ToUpper() :
                    (p.ExcelTitleLangrage.ToUpper() == "E" ? p.XlsNameEN : p.XlsNameCN)) == "ROWNO" && p.FieldHD.ToUpper() == "D");
                string ROWNO = index != -1 ? ((JObject)jtoken).GetValue(pmList[index].ERPField).ToString() : "";

                index = pmList.FindIndex(p => (syncType.ToUpper() == "ERP" ? p.WMSField.ToUpper() :
                    (p.ExcelTitleLangrage.ToUpper() == "E" ? p.XlsNameEN : p.XlsNameCN)) == "ROWNODEL" && p.FieldHD.ToUpper() == "D");
                string ROWNODEL = index != -1 ? ((JObject)jtoken).GetValue(pmList[index].ERPField).ToString() : "";

                if (!String.IsNullOrEmpty(ErpVoucherNo) && !String.IsNullOrEmpty(ROWNO))
                {
                    index = deleteModels.FindIndex(p => p.ERPVOUCHERNO == ErpVoucherNo);
                    if (index != -1)
                    {
                        int rowindex= deleteModels[index].ROW.FindIndex(p => p.ROWNO == ROWNO);
                        if (rowindex != -1)
                        {
                            if(!String.IsNullOrEmpty(ROWNODEL))
                                deleteModels[index].ROW[rowindex].RowDel.Add(ROWNODEL);
                        }else
                        {
                            RowDelModel RowModel = new RowDelModel();
                            RowModel.ROWNO = ROWNO;
                            RowModel.RowDel = new List<string>();
                            RowModel.RowDel.Add(ROWNODEL);
                            deleteModels[index].ROW.Add(RowModel);
                        }
                    }
                    else
                    {
                        deleteModel deleteModel = new deleteModel();
                        deleteModel.ERPVOUCHERNO = ErpVoucherNo;
                        deleteModel.ROW = new List<RowDelModel>();
                        RowDelModel RowModel = new RowDelModel();
                        RowModel.ROWNO = ROWNO;
                        RowModel.RowDel = new List<string>();
                        RowModel.RowDel.Add(ROWNODEL);
                        deleteModel.ROW.Add(RowModel);
                        deleteModels.Add(deleteModel);
                    }
                }
            }
        }




        /// <summary>
        /// 拼接SQL
        /// </summary>
        /// <param name="pmList">配置表</param>
        /// <param name="sql">SQL</param>
        /// <param name="WmsVourcherNo">WMS单据号</param>
        /// <param name="isHead">是否表头数据</param>
        /// <param name="tableName">表明</param>
        /// <param name="HeadtaskID">表头ID</param>
        /// <param name="taskID">表体ID</param>
        /// <param name="jtoken">数据</param>
        /// <param name="syncType">导入方式  ERP  EXCEL</param>
        /// <returns></returns>
        private string CreateInsertSql(List<ParamaterField_Model> pmList, string sql,
            string WmsVourcherNo, bool isHead, string tableName, int HeadtaskID, int VoucherType, JToken jtoken, string syncType,string GetmaterialNoIdsql)
        {
            StringBuilder contextList = new StringBuilder();
            StringBuilder valueList = new StringBuilder();

            
            foreach (JProperty jp in jtoken)
            {
                int index = pmList.FindIndex(p => (syncType.ToUpper() == "ERP" ? p.ERPField.ToUpper() :
                (p.ExcelTitleLangrage.ToUpper() == "E" ? p.XlsNameEN : p.XlsNameCN)) == jp.Name.ToUpper() && p.FieldHD.ToUpper() == (isHead ? "H" : "D"));
                if (index != -1)
                {
                    string value = jp.Value.ToString().Replace("'", "''");
                    value = pmList[index].DefaultType == 1 ? "to_date('" + value + "','yyyy-mm-dd hh24:mi:ss')," : "'" + value + "',";
                   
                    contextList.Append(pmList[index].WMSField + ",");
                    valueList.Append( value.Trim().Replace("；",";").Replace(" "," ") );
                }
            }

           var defaultValueList =pmList.FindAll(
                p => !String.IsNullOrEmpty(p.DefaultValue) && p.FieldHD.ToUpper() == (isHead ? "H" : "D") && String.IsNullOrEmpty(p.ERPField)).DistinctBy(s => new { s.WMSField, s.DefaultValue });
            foreach (var pmodel in defaultValueList)
            {
                string value = pmodel.DefaultValue ;
                if (pmodel.DefaultType == 2)
                    value = "(" + GetmaterialNoIdsql + ")";
                contextList.Append(pmodel.WMSField + ",");
                valueList.Append(value + ",");
            }
            sql =                       
                isHead ?
                String.Format(sql, tableName, HeadtaskID, (WmsVourcherNo.Equals("")? HeadtaskID+"":WmsVourcherNo), VoucherType, contextList.Remove(contextList.Length - 1, 1),
                valueList.Remove(valueList.Length - 1, 1)) :
                 String.Format(sql, tableName, base.GetTableIDBySqlServer(pmList[0].DETAILID), HeadtaskID, (WmsVourcherNo.Equals("") ? HeadtaskID + "" : WmsVourcherNo), contextList.Remove(contextList.Length - 1, 1),
                valueList.Remove(valueList.Length - 1, 1));
            return sql;
        }


        public bool CreateSQLByExcel(List<ParamaterField_Model> pmList, string dataJson, int VoucherType, ref string errMsg, string syncType)
        {
            pmList = pmList.FindAll(p => p.VoucherType == VoucherType);
    
            
            List<string> sqlList = new List<string>(); //执行sql语句
            JArray jarray = JArray.Parse(dataJson);
 
            for (int i = 0; i < jarray.Count; i++)
            {
                string sqlInsert = "insert into {0}(ID,{1}) values({2},{3})";
                string sqlUpdate = "update {0} set {1} where {2}";
                string checkSQL = "select count(*) from {0} WHere 1=1 and {1}";
                var jObject = JObject.Parse(jarray[i].ToString());

                string[] keys = pmList[0].HEADKEYS.ToString().Split(',');
                string whereString = String.Empty;
                foreach (string key in keys)
                {
                    // int index = pmList.FindIndex(p => (syncType.ToUpper() == "ERP" ? p.WMSField.ToUpper() :
                    //(p.ExcelTitleLangrage.ToUpper() == "E" ? p.XlsNameEN : p.XlsNameCN)) == key.ToUpper());
                    int index = pmList.FindIndex(p =>  p.WMSField.ToUpper()  == key.ToUpper());
                    if (index != -1)
                    {
                        whereString += pmList[index].WMSField + "='" + jObject.GetValue(pmList[index].ERPField) + "' and ";

                        //whereString += pmList[index].WMSField + "='" + jObject.GetValue(pmList[index].ExcelTitleLangrage.ToUpper() == "E" ? pmList[index].XlsNameEN : pmList[index].XlsNameCN) + "' and ";
                    }
                }
                string[] MATERIALNOKEYS = pmList[0].MATERIALNOKEYS.ToString().Split(',');
                string whereMATERIALNOString = String.Empty;
                foreach (string key in MATERIALNOKEYS)
                {
                    int index = pmList.FindIndex(p => (syncType.ToUpper() == "ERP" ? p.WMSField.ToUpper() :
                   (p.ExcelTitleLangrage.ToUpper() == "E" ? p.XlsNameEN : p.XlsNameCN)) == key.ToUpper());
                    if (index != -1)
                    {
                        whereMATERIALNOString += pmList[index].WMSField + "='" + jObject.GetValue(pmList[index].ERPField.ToString()) + "' and ";
                    }

                }
                string GetmaterialNoIdsql = "select id from T_MATERIAL where " + whereMATERIALNOString + " 1=1";

                whereString += " 1=1";
                string sql = String.Empty;
                bool isupdate = false;
                string tableName = pmList[0].WMSTableNameH;//表名
                if (!String.IsNullOrEmpty(whereString))
                {
                    checkSQL = String.Format(checkSQL, tableName, whereString);
                    isupdate = !(Convert.ToInt32(base.GetScalarBySql(checkSQL).ToString()) == 0);
                }                
                int HeadtaskID = isupdate ? 0 : base.GetTableID(pmList[0].HEADID); //表头ID序号

                StringBuilder contextList = new StringBuilder();
                StringBuilder valueList = new StringBuilder();
                foreach (KeyValuePair<string, JToken> keyValuePair in jObject)
                {
                    string Name = keyValuePair.Key;
                    string Value = keyValuePair.Value.ToString().Replace("'", "''");//.Replace(",", "\\,")
                    int index = pmList.FindIndex(p => (syncType.ToUpper() == "ERP" ? p.ERPField.ToUpper() :
                       (p.ExcelTitleLangrage.ToUpper() == "E" ? p.XlsNameEN : p.XlsNameCN)) == Name.ToUpper() && p.FieldHD.ToUpper() == "H");
                    if (index != -1)
                    {
                        string value = Value.ToString().Replace("'", "''");
                        value = pmList[index].DefaultType == 1 ? "to_date('" + value + "','yyyy-mm-dd hh24:mi:ss')," : "'" + value + "',";                    
                        contextList.Append(isupdate ? pmList[index].WMSField + "='" + Value.ToString() + "'," :
                            pmList[index].WMSField + ",");
                        valueList.Append( value );          
                    }

                }
               var defaultValueList =pmList.FindAll(
                      p => !String.IsNullOrEmpty(p.DefaultValue) && p.FieldHD.ToUpper() == "H" && String.IsNullOrEmpty(p.ERPField)).DistinctBy(s => new { s.WMSField, s.DefaultValue });
           
                foreach (var pmodel in defaultValueList)
                {
                    string value = pmodel.DefaultValue;
                    if (pmodel.DefaultType == 2)
                        value = "(" + GetmaterialNoIdsql + ")";
                    contextList.Append(isupdate ? pmodel.WMSField + "=" + pmodel.DefaultValue + "," :
                               pmodel.WMSField + ",");
                    valueList.Append(value + ",");
                }

                sql = isupdate ? String.Format(sqlUpdate, tableName, contextList.Remove(contextList.Length - 1, 1), whereString) :
                    String.Format(sqlInsert, tableName, contextList.Remove(contextList.Length - 1, 1), HeadtaskID, valueList.Remove(valueList.Length - 1, 1));
                sqlList.Add(sql);
                if (sqlList.Count > 1000)
                {
                    if (!base.SaveModelListBySqlToDB(sqlList, ref errMsg))
                        return false;
                   sqlList = new List<string>();
                }
            }

            //调用数据库插入语句
            return base.SaveModelListBySqlToDB(sqlList, ref errMsg);
        }


        internal void insertStock(string dataJson, ref string errMsg)
        {
            string sqlInsert = "insert into T_stock(ID,{0}) values({1},{2})";
            List<string> sqlList = new List<string>(); //执行sql语句

            JArray jarray = JArray.Parse(dataJson);

            for (int i = 0; i < jarray.Count; i++)
            {
                var jObject = JObject.Parse(jarray[i].ToString());
                string PartNO = jObject.GetValue("MaterialNO").ToString();
                string SerialNO = jObject.GetValue("SerialNO").ToString();
                int HeadtaskID = base.GetTableID("seq_stock_id"); //表头ID序号
                string title = "SERIALNO,MATERIALNO,WAREHOUSEID,HOUSEID,AREAID,QTY,STATUS,ISDEL,CREATER,CREATETIME,RECEIVESTATUS,ISLIMITSTOCK,PARTNO,MATERIALNOID";
                String sqlselet = "select * from T_Material where PartNO='" + PartNO + "'";

                
                DataTable dr= dbFactory.ExecuteDataSet(CommandType.Text,sqlselet).Tables[0];
                string MaterialNO = dr.Rows[0]["MATERIALNO"].ToString();
                SerialNO = MaterialNO + SerialNO;
                string content = "'" + SerialNO + "'," + "'" + MaterialNO + "',02,106,230,1,1,1,'admin',SYSDATE,2,2,'" + PartNO + "'," + dr.Rows[0]["ID"].ToString();
                string sql = String.Format(sqlInsert, title, HeadtaskID, content);
                sqlList.Add(sql);
            }
            base.SaveModelListBySqlToDB(sqlList, ref errMsg);

        }

        ///// <summary>
        ///// 生成sql
        ///// </summary>
        ///// <param name="pmList">配置表信息</param>
        ///// <param name="dataJson">数据JSON</param>
        ///// <returns></returns>
        //public bool ComparerListAndCreateSQL(List<ParamaterField_Model> pmList, string dataJson, int VoucherType, ref string errMsg, string syncType)
        //{
        //    JArray jarray = JArray.Parse(dataJson);
        //    List<string> sqlInsertList = new List<string>();
        //    List<string> sqlUpdateList = new List<string>();
        //    string sqlInsertTitle = "insert into {0}(ID,VOUCHERNO,VOUCHERTYPE,{4}) values({1},{2},{3},{5})";
        //    string sqlInsertDetail = "insert into {0}(ID,HEADERID,VOUCHERNO,{4}) values({1},{2},{3},{5})";
        //    string sqlUpdate = "update {0} set {1} where {2}";
        //    string sqlDelete = "delete from {0} where ERPVOUCHERNO in ('{1}') and MATERIALNO not in ('{2}') and ROWNO not in ('{3}')";
        //    string sql = String.Empty;

        //    string WmsVourcherNoSQL = pmList[0].VOUCHERNO;  //wms单据号
        //    bool isUpdate = false;//是否更新单据信息
        //    List<string> ErpVoucherNoList = new List<string>();//更新是检查是否存在需要删除数据
        //    List<string> MATERIALNOList = new List<string>();//更新是检查是否存在需要删除数据
        //    List<string> ROWNOList = new List<string>();//更新是检查是否存在需要删除数据
        //    List<string> sqlList = new List<string>(); //执行sql语句

        //    for (int i = 0; i < jarray.Count; i++)
        //    {
        //        int HeadtaskID = base.GetTableID(pmList[0].HEADID); //表头ID序号
        //        string WmsVourcherNo = base.GetScalarBySql(WmsVourcherNoSQL).ToString();
        //        var jObject = JObject.Parse(jarray[i].ToString());
        //        //isUpdate = jObject.GetValue("STATUS").ToString() == "1";
        //        foreach (KeyValuePair<string, JToken> keyValuePair in jObject)
        //        {
        //            string key = keyValuePair.Key;
        //            JToken jtokenArr = keyValuePair.Value;

        //            if (key.ToUpper() == "STATUS" && keyValuePair.Value.ToString() == "1")
        //            {
        //                isUpdate = true;
        //                continue;
        //            }

        //            bool isHead = key.ToUpper() == "HEAD";
        //            string tableName = isHead ? pmList[0].WMSTableNameH : pmList[0].WMSTableNameD; //表名

        //            if (jtokenArr.GetType().Name.ToUpper() == "JOBJECT")
        //            {
        //                sql = CreateSQL(pmList, VoucherType, syncType, sqlInsertTitle, sqlInsertDetail,
        //                    sqlUpdate, isUpdate, sqlList, HeadtaskID, WmsVourcherNo, jtokenArr, isHead, tableName,
        //                    ref ErpVoucherNoList, ref MATERIALNOList, ref ROWNOList);
        //                sqlList.Add(sql);
        //            }
        //            else
        //            {
        //                foreach (JToken jtoken in jtokenArr)
        //                {
        //                    sql = CreateSQL(pmList, VoucherType, syncType, sqlInsertTitle, sqlInsertDetail,
        //                     sqlUpdate, isUpdate, sqlList, HeadtaskID, WmsVourcherNo, jtoken, isHead, tableName,
        //                     ref ErpVoucherNoList, ref MATERIALNOList, ref ROWNOList);
        //                    sqlList.Add(sql);
        //                }
        //            }

        //        }
        //        //更新状态下，删除原有表体数据
        //        if (isUpdate)
        //        {
        //            string ErpVoucherNo = String.Join("','", ErpVoucherNoList.ToArray());
        //            string MATERIALNO = String.Join("','", MATERIALNOList.ToArray());
        //            string ROWNO = String.Join("','", ROWNOList.ToArray());
        //            sqlDelete = String.Format(sqlDelete, pmList[0].WMSTableNameD, ErpVoucherNo, MATERIALNO, ROWNO);
        //            sqlList.Add(sqlDelete);
        //        }
        //    }
        //    //调用数据库插入语句
        //    return base.SaveModelListBySqlToDB(sqlList, ref errMsg);
        //    //return true;
        //}

        //private string CreateSQL(List<ParamaterField_Model> pmList, int VoucherType, string syncType, string sqlInsertTitle, string sqlInsertDetail,
        //    string sqlUpdate, bool isUpdate, List<string> sqlList, int HeadtaskID, string WmsVourcherNo, JToken jtoken, bool isHead, string tableName,
        //    ref List<string> ErpVoucherNoList, ref List<string> MATERIALNOList, ref List<string> ROWNOList)
        //{
        //    string sql = "";
        //    if (isUpdate)
        //    {
        //        sql = CreateUpdateSql(pmList, sqlUpdate, WmsVourcherNo, isHead, tableName, HeadtaskID, VoucherType, jtoken, syncType, ref ErpVoucherNoList, ref MATERIALNOList, ref ROWNOList);
        //    }
        //    else
        //    {
        //        sql = CreateInsertSql(pmList, isHead ? sqlInsertTitle : sqlInsertDetail, WmsVourcherNo,
        //        isHead, tableName, HeadtaskID, VoucherType, jtoken, syncType);
        //    }
        //    return sql;
        //}

        //private string CreateUpdateSql(List<ParamaterField_Model> pmList, string sql,
        //    string WmsVourcherNo, bool isHead, string tableName, int HeadtaskID, int VoucherType, JToken jtoken, string syncType,
        //     ref List<string> ErpVoucherNoList, ref List<string> MATERIALNOList, ref List<string> ROWNOList)
        //{
        //    StringBuilder whereList = new StringBuilder();
        //    StringBuilder valueList = new StringBuilder();
        //    string ErpVoucherNo = "";
        //    string MATERIALNO = "";
        //    string ROWNO = "";


        //    foreach (JProperty jp in jtoken)
        //    {
        //        int index = pmList.FindIndex(p => (syncType.ToUpper() == "ERP" ? p.ERPField.ToUpper() :
        //        (p.ExcelTitleLangrage.ToUpper() == "E" ? p.XlsNameEN : p.XlsNameCN)) == jp.Name.ToUpper() && p.FieldHD.ToUpper() == (isHead ? "H" : "D"));
        //        if (index != -1)
        //        {
        //            if (pmList[index].WMSField.ToUpper() == "ERPVOUCHERNO" || pmList[index].WMSField.ToUpper() == "MATERIALNO" || pmList[index].WMSField.ToUpper() == "ROWNO")
        //            {
        //                if (pmList[index].WMSField.ToUpper() == "ERPVOUCHERNO")
        //                {
        //                    ErpVoucherNo = jtoken.Value<string>(jp.Name.ToString());
        //                    if (!ErpVoucherNoList.Contains(ErpVoucherNo)) ErpVoucherNoList.Add(ErpVoucherNo);
        //                }
        //                if (pmList[index].WMSField.ToUpper() == "MATERIALNO")
        //                {
        //                    MATERIALNO = jtoken.Value<string>(jp.Name.ToString());
        //                    if (!MATERIALNOList.Contains(MATERIALNO)) MATERIALNOList.Add(MATERIALNO);
        //                }
        //                if (pmList[index].WMSField.ToUpper() == "ROWNO")
        //                {
        //                    ROWNO = jtoken.Value<string>(jp.Name.ToString());
        //                    if (!ROWNOList.Contains(ROWNO)) ROWNOList.Add(ROWNO);
        //                }
        //                whereList.Append(pmList[index].WMSField + "='" + jp.Value.ToString() + "' and ");
        //            }
        //            else
        //            {
        //                valueList.Append(pmList[index].WMSField + "='" + jp.Value.ToString() + "',");
        //            }
        //        }
        //    }
        //    string checkSQL = "select count(*) from {0} WHere ErpVoucherNo='{1}'" + (isHead ? "" : "and MATERIALNO='{2}' and ROWNO='{3}'");

        //    checkSQL = String.Format(checkSQL, tableName, ErpVoucherNo, MATERIALNO, ROWNO);
        //    if (Convert.ToInt32(base.GetScalarBySql(checkSQL).ToString()) == 0)
        //    {

        //        //根据ErpVoucherNo 查询headID,WmsVourcherNo
        //        string getInsertInfo = String.Format("select headerID,VoucherNo from {0} where Rownum<=1 and ErpVoucherNo='{1}'", tableName, ErpVoucherNo);
        //        IDataReader reader = base.GetRowBySql(getInsertInfo);
        //        if (reader.Read())
        //        {
        //            WmsVourcherNo = reader["VoucherNo"].ToString();
        //            HeadtaskID = Convert.ToInt32(reader["headerID"]);
        //        }
        //        reader.Close();
        //        reader.Dispose();
        //        string sqlInsertDetail = "insert into {0}(ID,HEADERID,VOUCHERNO,{4}) values({1},{2},{3},{5})";
        //        sql = CreateInsertSql(pmList, sqlInsertDetail, WmsVourcherNo, isHead, tableName, HeadtaskID, VoucherType, jtoken, syncType);
        //    }
        //    else
        //    {
        //        sql = String.Format(sql, tableName, (isHead ? "LASTSYNCTIME=SYSDATE," : "") + valueList.Remove(valueList.Length - 1, 1), whereList.Remove(whereList.Length - 4, 4));
        //    }
        //    return sql;
        //}


    }
}
