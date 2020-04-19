
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.User;
using BILBasic.DBA;
using BILBasic.Common;
using BILWeb.Area;
using Oracle.ManagedDataAccess.Client;
using BILWeb.Stock;

using System.Data;
using BILBasic.XMLUtil;

namespace BILWeb.InStockTask
{

    public partial class T_InTaskDetails_DB : BILBasic.Basing.Factory.Base_DB<T_InStockTaskDetailsInfo>
    {

        /// <summary>
        /// 添加t_taskdetails
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_InStockTaskDetailsInfo t_taskdetails)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();
        }

        protected override List<string> GetSaveSql(UserModel user, ref T_InStockTaskDetailsInfo t_taskdetails)
        {
            throw new NotImplementedException();
        }

        protected override List<string> GetSaveModelListSql(UserModel user, List<T_InStockTaskDetailsInfo> modelList)
        {
            string strSql1 = string.Empty;
            string strSql2 = string.Empty;
            string strSql3 = string.Empty;
            string strSql4 = string.Empty;
            string strSql5 = string.Empty;

            List<string> lstSql = new List<string>();
            T_Stock_DB stockDB = new T_Stock_DB();
            Area. T_Area_DB areaDB = new Area. T_Area_DB();
            int transID = 0;

            foreach (var item in modelList)
            {
                strSql1 = string.Format("update t_taskdetails  set  remainqty =  remainqty-{0}, shelveqty = isnull( shelveqty,0)+{1}," +
                        " toareano = '{2}', operatoruserno = '{3}', operatordatetime = getdate() where  id = '{4}'", item.ScanQty, item.ScanQty, item.AreaID, user.UserNo, item.ID);
                lstSql.Add(strSql1);

                strSql2 = string.Format("update t_taskdetails  set  linestatus =(case when isnull( remainqty,0)< isnull( taskqty,0) and isnull( remainqty,0)<>0 then 2 when isnull( remainqty,0)  = 0  then 3 end )," +
                                        " Toerpareano ='{1}',Toerpwarehouse='{2}' where id = '{0}'", item.ID, item.AreaNo, item.ToErpWarehouse);
                lstSql.Add(strSql2);

                if (item.lstStockInfo != null && item.lstStockInfo.Count > 0)
                {
                    foreach (var itemStock in item.lstStockInfo)
                    {

                        Area.T_AreaInfo areaInfo = new T_AreaInfo();
                        string NewSerialNo = "";
                        areaInfo.AreaNo = item.AreaNo;
                        areaInfo.WarehouseID = item.WarehouseID;
                        areaInfo = areaDB.GetModelBySql(areaInfo);
                        if (itemStock.BarCodeType != 5) 
                        {
                            itemStock.AmountQty = itemStock.Qty;
                        }
                        itemStock.Status = 3;
                        itemStock.ReceiveStatus = 2;//是否已上架
                        itemStock.IsLimitStock = 2;
                        itemStock.IsQuality = 3;
                        lstSql.AddRange(stockDB.saveSplitBarCode(user, itemStock, areaInfo, out NewSerialNo));

                        //itemStock.AmountQty = itemStock.Qty;
                        //if (item.ScanQty != itemStock.Qty)
                        //{
                        //    itemStock.AmountQty = item.ScanQty;
                        //    string NewSerialNo = "";
                        //    var value = base.GetScalarBySql("select serialno  from t_stock where Areaid =" + item.AreaID + " and materialno ='" + itemStock.MaterialNo + "' and edate=to_date('" + itemStock.EDate.ToString("yyyy-MM-dd") + "','yyyy-mm-dd') and batchno ='" + itemStock.BatchNo + "' and  strongholdcode='" + itemStock.StrongHoldCode + "' ");
                        //    if (value == null || value.ToString() == "")
                        //    {
                        //        lstSql.Add(GetAmountQtySql(itemStock, ref NewSerialNo));
                        //        itemStock.AreaID = item.AreaID;
                        //        itemStock.HouseID = item.HouseID;
                        //        itemStock.WareHouseID = item.WarehouseID;
                        //        itemStock.Status = 3;
                        //        itemStock.ReceiveStatus = 2;
                        //        itemStock.IsLimitStock = 2;
                        //        itemStock.IsQuality = 3;
                        //        lstSql.Add(GetAmountQtyInsertStockSql(itemStock, user, NewSerialNo));
                        //        lstSql.Add("update t_stock a set  Qty =  Qty - '" + itemStock.AmountQty + "'  where serialno = '" + itemStock.SerialNo + "'");
                        //    }
                        //    else
                        //    {
                        //        lstSql.Add("update t_stock a set  Qty =  Qty - '" + itemStock.AmountQty + "'  where serialno = '" + itemStock.SerialNo + "'");
                        //        lstSql.Add("update t_stock a set  Qty =  Qty + '" + itemStock.AmountQty + "'  where serialno = '" + value + "'");
                        //    }

                        //}
                        //else
                        //{
                        //    strSql3 = string.Format("update t_Stock a set  Receivestatus = '2' ,  islimitstock = '2', Warehouseid = '{0}', Houseid = '{1}', Areaid = '{2}' where    serialno = '{3}'", item.WarehouseID, item.HouseID, item.AreaID, itemStock.SerialNo);
                        //    lstSql.Add(strSql3);
                        //}

                        //transID = base.GetTableID("seq_tasktrans_id");
                        transID = base.GetTableIDBySqlServerTaskTrans("t_tasktrans");
                        strSql5 = "SET IDENTITY_INSERT t_tasktrans on ;insert into t_tasktrans(id, Serialno, Materialno, Materialdesc, Supcuscode, " +
                                "Supcusname, Qty, Tasktype, Vouchertype, Creater, Createtime,TaskdetailsId, Unit, Unitname,partno,materialnoid,erpvoucherno,voucherno," +
                                "Strongholdcode,Strongholdname,Companycode,Supprdbatch,Edate,taskno,Batchno,Barcode,FromWarehouseNo,FromWarehouseName,FromHouseNo,FromAreaNo,ToWarehouseNo,ToWarehouseName,ToHouseNo,ToAreaNo,PalletNo,IsPalletOrBox)" +
                                " values ('" + transID + "','" + itemStock.SerialNo + "'," +
                                " '" + item.MaterialNo + "','" + item.MaterialDesc + "','" + item.SupCusCode + "','" + item.SupCusName + "','" + itemStock.AmountQty + "','1'," +
                                " (select vouchertype from t_task where id = '" + item.HeaderID + "') ,'" + user.UserName + "',getdate(),'" + item.ID + "', " +
                                "'" + item.Unit + "','" + item.UnitName + "','" + item.PartNo + "','" + item.MaterialNoID + "','" + item.ErpVoucherNo + "'," +
                                "  '" + item.VoucherNo + "','" + itemStock.StrongHoldCode + "','" + itemStock.StrongHoldName + "','" + itemStock.CompanyCode + "'," +
                                "  '" + itemStock.SupPrdBatch + "','" + itemStock.EDate + "' ,'" + item.TaskNo + "','" + itemStock.BatchNo + "','" + itemStock.Barcode + "',"+
                                " (select WAREHOUSENO from T_WAREHOUSE where id = (select WAREHOUSEID from T_STOCK where SERIALNO = '" + itemStock.SerialNo + "')),"+
                                " (select WAREHOUSENAME from T_WAREHOUSE where id = (select WAREHOUSEID from T_STOCK where SERIALNO = '" + itemStock.SerialNo + "')),"+
                                " (select HOUSENO from T_HOUSE where id = (select HOUSEID from T_STOCK where SERIALNO = '" + itemStock.SerialNo + "')),"+
                                " (select AREANO from T_AREA where id = (select AREAID from T_STOCK where SERIALNO = '" + itemStock.SerialNo + "')),"+
                                " '" + item.WareHouseNo + "',(select WAREHOUSENAME from T_WAREHOUSE where warehouseno = '" + item.WareHouseNo + "'),(SELECT HOUSENO from v_area where WAREHOUSENO = '" + item.WareHouseNo + "' AND AREANO = '" + item.AreaNo + "'),'" + item.AreaNo + "','" + itemStock.PalletNo + "','" + itemStock.IsPalletOrBox + "') SET IDENTITY_INSERT t_tasktrans off ";

                        lstSql.Add(strSql5);


                    }
                }

            }

            strSql3 = "update t_task  set  Status = 2 where id = '" + modelList[0].HeaderID + "'";
            lstSql.Add(strSql3);

            strSql4 = string.Format(" update t_task set status = 3 where id in(select HeaderID from t_taskdetails group by HeaderID having(max(isnull(linestatus,1)) = 3 and min(isnull(linestatus,1))=3) and HeaderID = '{0}')" +
                                    "and id = '{1}'", modelList[0].HeaderID, modelList[0].HeaderID);

            lstSql.Add(strSql4);

            return lstSql;
        }

        /// <summary>
        /// 写入条码表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="NewSerialNo"></param>
        /// <returns></returns>
        public string GetAmountQtySql(T_StockInfo model, ref string NewSerialNo)
        {
            int barcodeID = GetTableID("Seq_Outbarcode_Id");

            string SeqSerialNo = base.GetTableID("SEQ_SERIAL_NO").ToString();

            NewSerialNo = System.DateTime.Now.ToString("yyyyMMdd") + SeqSerialNo.PadLeft(6, '0');

            string strSql = "insert into t_Outbarcode (Id, Voucherno, Rowno, Erpvoucherno, Vouchertype, Materialno, Materialdesc, Cuscode," +
                            "Cusname, Supcode, Supname, Outpackqty, Innerpackqty, Voucherqty, Qty, Nopack, Printqty, Barcode, " +
                            "Barcodetype, Serialno, Barcodeno, Outcount, Innercount, Mantissaqty, Isrohs, Outbox_Id, " +
                            "Inner_Id, Abatchqty, Isdel, Creater, Createtime, Materialnoid, Strongholdcode, " +
                            "Strongholdname, Companycode, Productdate, Supprdbatch, Supprddate, Productbatch, Edate, Storecondition," +
                            "Specialrequire, Batchno, Barcodemtype, Rownodel, Protectway, Boxweight, Unit, Labelmark, Boxdetail, Matebatch," +
                            "Mixdate, Relaweight,Ean)" +
                            "select '" + barcodeID + "',voucherno,rowno,erpvoucherno,vouchertype, Materialno, Materialdesc, Cuscode," +
                            "Cusname, Supcode, Supname, Outpackqty, Innerpackqty, Voucherqty, '" + model.AmountQty + "',Nopack, Printqty," +
                            "Barcodetype || '@' || Strongholdcode || '@' || materialno || '@' || ean || '@' || to_char(edate,'yyyy/MM/dd') ||  '@'|| Batchno ||'@' || " + model.AmountQty + " || '@' || '" + NewSerialNo + "'," +
                            "Barcodetype, '" + NewSerialNo + "', Barcodeno, Outcount, Innercount, Mantissaqty, Isrohs,'" + model.ID + "',Inner_Id, " +
                            "Abatchqty, Isdel, Creater, getdate(), Materialnoid, Strongholdcode, " +
                            "Strongholdname, Companycode, Productdate, Supprdbatch, Supprddate, Productbatch, Edate, Storecondition," +
                            "Specialrequire, Batchno, Barcodemtype, Rownodel, Protectway, Boxweight, Unit, Labelmark, Boxdetail, Matebatch," +
                            "Mixdate, Relaweight,ean from t_Outbarcode where serialno = '" + model.SerialNo + "'";
            return strSql;

        }
        /// <summary>
        /// 写入库存新条码信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <param name="NewSerialNo"></param>
        /// <returns></returns>
        public string GetAmountQtyInsertStockSql(T_StockInfo model, UserModel user, string NewSerialNo)
        {
            int stockID = GetTableID("Seq_Stock_Id");

            string strSql = "insert into t_Stock(Id, Barcode, Serialno, Materialno, Materialdesc, Warehouseid, Houseid, Areaid, Qty, Status, Isdel," +
                            "Creater, Createtime, Batchno,  Oldstockid, Unit, Unitname,  " +
                            "Receivestatus,  Islimitstock,  Materialnoid, Strongholdcode, Strongholdname, Companycode," +
                            "Edate, Supcode, Supname, Productdate, Supprdbatch, Supprddate, Isquality,Stocktype,ean)" +
                            "select '" + stockID + "',barcode,serialno,materialno,Materialdesc,'" + model.WareHouseID + "', '" + model.HouseID + "', '" + model.AreaID + "', Qty ,'" + model.Status + "','1'," +
                            "'" + user.UserNo + "',getdate(),batchno,'" + model.ID + "',unit,'" + model.UnitName + "','" + model.ReceiveStatus + "','" + model.IsLimitStock + "',Materialnoid," +
                            "Strongholdcode, Strongholdname, Companycode,Edate, Supcode, Supname, Productdate, Supprdbatch,Supprddate, '" + model.IsQuality + "',1,ean from t_Outbarcode where serialno = '" + NewSerialNo + "'";

            return strSql;
        }

        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_InStockTaskDetailsInfo ToModel(IDataReader reader)
        {
            T_InStockTaskDetailsInfo t_taskdetails = new T_InStockTaskDetailsInfo();

            t_taskdetails.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            //t_taskdetails.ToAreaNo = (string)dbFactory.ToModelValue(reader, "TOAREANO");
            t_taskdetails.MaterialNo = (string)dbFactory.ToModelValue(reader, "MATERIALNO");
            t_taskdetails.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MATERIALDESC");
            t_taskdetails.TaskQty = ((decimal?)dbFactory.ToModelValue(reader, "TASKQTY1") == null || (decimal?)dbFactory.ToModelValue(reader, "TASKQTY1") == 0) ? (decimal?)dbFactory.ToModelValue(reader, "TASKQTY") : (decimal?)dbFactory.ToModelValue(reader, "TASKQTY1");
            t_taskdetails.QualityQty = (decimal?)dbFactory.ToModelValue(reader, "QUALITYQTY");
            t_taskdetails.RemainQty = dbFactory.ToModelValue(reader, "REMAINQTY").ToDecimal();
            t_taskdetails.ShelveQty = (decimal)dbFactory.ToModelValue(reader, "SHELVEQTY");
            t_taskdetails.LineStatus = dbFactory.ToModelValue(reader, "LineStatus").ToInt32();
            t_taskdetails.IsQualitycomp = (decimal?)dbFactory.ToModelValue(reader, "ISQUALITYCOMP");
            t_taskdetails.KeeperUserNo = (string)dbFactory.ToModelValue(reader, "KEEPERUSERNO");
            t_taskdetails.OperatorUserNo = (string)dbFactory.ToModelValue(reader, "OPERATORUSERNO");
            t_taskdetails.CompleteDateTime = (DateTime?)dbFactory.ToModelValue(reader, "COMPLETEDATETIME");
            t_taskdetails.HeaderID = dbFactory.ToModelValue(reader, "HEADERID").ToInt32();
            t_taskdetails.TMaterialNo = (string)dbFactory.ToModelValue(reader, "TMATERIALNO");
            t_taskdetails.TMaterialDesc = (string)dbFactory.ToModelValue(reader, "TMATERIALDESC");
            t_taskdetails.OperatorDateTime = (DateTime?)dbFactory.ToModelValue(reader, "OPERATORDATETIME");
            t_taskdetails.ReviewQty = (decimal?)dbFactory.ToModelValue(reader, "REVIEWQTY");
            t_taskdetails.PackCount = (decimal?)dbFactory.ToModelValue(reader, "PACKCOUNT");
            t_taskdetails.ShelvePackCount = (decimal?)dbFactory.ToModelValue(reader, "SHELVEPACKCOUNT");
            t_taskdetails.VoucherNo = (string)dbFactory.ToModelValue(reader, "VOUCHERNO");
            t_taskdetails.RowNo = (string)dbFactory.ToModelValue(reader, "ROWNO");
            t_taskdetails.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_taskdetails.TrackNo = (string)dbFactory.ToModelValue(reader, "TRACKNO");
            t_taskdetails.Unit = (string)dbFactory.ToModelValue(reader, "UNIT");
            t_taskdetails.UnQualityQty = (decimal?)dbFactory.ToModelValue(reader, "UNQUALITYQTY");
            t_taskdetails.PostQty = (decimal?)dbFactory.ToModelValue(reader, "POSTQTY");
            t_taskdetails.PostStatus = (decimal?)dbFactory.ToModelValue(reader, "POSTSTATUS");
            t_taskdetails.PostDate = (DateTime?)dbFactory.ToModelValue(reader, "POSTDATE");
            t_taskdetails.ReserveNumber = (string)dbFactory.ToModelValue(reader, "RESERVENUMBER");
            t_taskdetails.ReserveRowNo = (string)dbFactory.ToModelValue(reader, "RESERVEROWNO");
            t_taskdetails.UnShelveQty = (decimal?)dbFactory.ToModelValue(reader, "UNSHELVEQTY");
            t_taskdetails.Requstreason = (string)dbFactory.ToModelValue(reader, "REQUSTREASON");
            t_taskdetails.Remark = (string)dbFactory.ToModelValue(reader, "REMARK");
            t_taskdetails.ReviewUser = (string)dbFactory.ToModelValue(reader, "REVIEWUSER");
            t_taskdetails.ReviewDate = (DateTime?)dbFactory.ToModelValue(reader, "REVIEWDATE");
            t_taskdetails.ReviewStatus = (decimal?)dbFactory.ToModelValue(reader, "REVIEWSTATUS");
            t_taskdetails.PostUser = (string)dbFactory.ToModelValue(reader, "POSTUSER");
            t_taskdetails.Costcenter = (string)dbFactory.ToModelValue(reader, "COSTCENTER");
            t_taskdetails.Wbselem = (string)dbFactory.ToModelValue(reader, "WBSELEM");
            t_taskdetails.ToStorageLoc = (string)dbFactory.ToModelValue(reader, "TOSTORAGELOC");
            t_taskdetails.FromStorageLoc = (string)dbFactory.ToModelValue(reader, "FROMSTORAGELOC");
            t_taskdetails.OutStockQty = (decimal?)dbFactory.ToModelValue(reader, "OUTSTOCKQTY");
            t_taskdetails.LimitStockQtySAP = (decimal?)dbFactory.ToModelValue(reader, "LIMITSTOCKQTYSAP");
            t_taskdetails.RemainsSockQtySAP = (decimal?)dbFactory.ToModelValue(reader, "REMAINSTOCKQTYSAP");
            t_taskdetails.PackFlag = (decimal?)dbFactory.ToModelValue(reader, "PACKFLAG");
            t_taskdetails.CurrentRemainStockQtySAP = (decimal?)dbFactory.ToModelValue(reader, "CURRENTREMAINSTOCKQTYSAP");
            t_taskdetails.MoveReasonCode = (string)dbFactory.ToModelValue(reader, "MOVEREASONCODE");
            t_taskdetails.MoveReasonDesc = (string)dbFactory.ToModelValue(reader, "MOVEREASONDESC");
            t_taskdetails.PoNo = (string)dbFactory.ToModelValue(reader, "PONO");
            t_taskdetails.PoRowNo = (string)dbFactory.ToModelValue(reader, "POROWNO");
            t_taskdetails.IsLock = (decimal?)dbFactory.ToModelValue(reader, "ISLOCK");
            t_taskdetails.IsSmallBatch = (decimal?)dbFactory.ToModelValue(reader, "ISSMALLBATCH");
            t_taskdetails.DepartmentCode = (string)dbFactory.ToModelValue(reader, "DEPARTMENTCODE");
            t_taskdetails.DepartmentName = (string)dbFactory.ToModelValue(reader, "DEPARTMENTNAME");
            t_taskdetails.UnitName = (string)dbFactory.ToModelValue(reader, "UNITNAME");
            t_taskdetails.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");

            t_taskdetails.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ErpVoucherNo");
            t_taskdetails.TaskNo = (string)dbFactory.ToModelValue(reader, "TaskNo");
            t_taskdetails.AreaNo = dbFactory.ToModelValue(reader, "toareano").ToDBString();
            t_taskdetails.OperatorUserName = dbFactory.ToModelValue(reader, "OperatorUserName").ToDBString();
            t_taskdetails.StrLineStatus = dbFactory.ToModelValue(reader, "StrLineStatus").ToDBString();
            t_taskdetails.PartNo = dbFactory.ToModelValue(reader, "PartNo").ToDBString();
            t_taskdetails.MaterialNoID = dbFactory.ToModelValue(reader, "MaterialNoID").ToInt32();
            t_taskdetails.SupCusCode = dbFactory.ToModelValue(reader, "SupCusCode").ToDBString();
            t_taskdetails.SupCusName = dbFactory.ToModelValue(reader, "SupCusName").ToDBString();
            t_taskdetails.BatchNo = dbFactory.ToModelValue(reader, "BatchNo").ToDBString();
            t_taskdetails.FromBatchNo = dbFactory.ToModelValue(reader, "FromBatchNo").ToDBString();
            t_taskdetails.FromErpAreaNo = dbFactory.ToModelValue(reader, "FromErpAreaNo").ToDBString();
            t_taskdetails.FromErpWarehouse = dbFactory.ToModelValue(reader, "FromErpWarehouse").ToDBString();
            t_taskdetails.ToBatchNo = dbFactory.ToModelValue(reader, "ToBatchNo").ToDBString();
            t_taskdetails.ToErpAreaNo = dbFactory.ToModelValue(reader, "ToErpAreaNo").ToDBString();
            t_taskdetails.ToErpWarehouse = dbFactory.ToModelValue(reader, "ToErpWarehouse").ToDBString();
            t_taskdetails.StrongHoldCode = dbFactory.ToModelValue(reader, "StrongHoldCode").ToDBString();
            t_taskdetails.StrongHoldName = dbFactory.ToModelValue(reader, "StrongHoldName").ToDBString();
            t_taskdetails.CompanyCode = dbFactory.ToModelValue(reader, "CompanyCode").ToDBString();
            t_taskdetails.StrVoucherType = dbFactory.ToModelValue(reader, "strVoucherType").ToDBString();
            t_taskdetails.ERPVoucherType = dbFactory.ToModelValue(reader, "ErpVoucherType").ToDBString();
            t_taskdetails.iarrsid = dbFactory.ToModelValue(reader, "iarrsid").ToDBString();
            t_taskdetails.TaskQty1 = (dbFactory.ToModelValue(reader, "TaskQty1")==null?0: (decimal)dbFactory.ToModelValue(reader, "TaskQty1"));
            
            return t_taskdetails;
        }

        protected override string GetViewName()
        {
            return "V_TASKDETAIL";
        }

        protected override string GetTableName()
        {
            return "T_TASKDETAILS";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }



        /// <summary>
        /// 锁定物料操作人
        /// </summary>
        /// <param name="taskDetailsMdl"></param>
        /// <param name="userMdl"></param>
        /// <returns></returns>
        public int LockTaskOperUser(UserModel user, T_InStockTaskDetailsInfo detailModel)
        {
            try
            {
                string strSql = "update t_taskdetails set operatoruserno='{0}' where id = '{1}'";
                strSql = string.Format(strSql, user.UserNo, detailModel.ID);
                return GetExecuteNonQuery(strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.TargetSite);
            }
        }

        /// <summary>
        /// 查看物料被哪个用户锁定
        /// </summary>
        /// <param name="taskDetailsMdl"></param>
        /// <param name="userMdl"></param>
        /// <returns></returns>
        public string QueryUserNameByTaskDetails(T_InStockTaskDetailsInfo taskDetailsMdl, UserModel user)
        {
            try
            {
                string strUserName = string.Empty;
                string strSql = "select b.UserName from t_taskdetails a left join t_user b on  operatoruserno = b.userno where  id = {0} " +
                                " and   operatoruserno!='{1}'";
                strSql = string.Format(strSql, taskDetailsMdl.ID, user.UserNo);
                return GetScalarBySql(strSql).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.TargetSite);
            }
        }

        /// <summary>
        /// 批量解锁物料
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="user"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool UnLockTaskOperUser(List<T_InStockTaskDetailsInfo> modelList, UserModel user, ref string strError)
        {
            List<string> lstSql = new List<string>();

            string strSql = string.Empty;

            foreach (var item in modelList)
            {
                strSql = string.Format("update t_taskdetails a set  operatoruserno = '' where  id = {0} and  operatoruserno = {1} and  status !=3", item.ID, user.UserNo);
                lstSql.Add(strSql);
            }

            return UpdateModelListStatusBySql(lstSql, ref strError);
        }

        public List<T_InStockTaskDetailsInfo> GetExportTaskDetail(T_InStockTaskInfo model)
        {
            try
            {
                List<T_InStockTaskDetailsInfo> modelList = new List<T_InStockTaskDetailsInfo>();
                string strSql = "select * from v_Taskdetail " + GetExportFilterSql(model);
                using (IDataReader reader = dbFactory.ExecuteReader(strSql))
                {
                    modelList = base.ToModels(reader);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetExportFilterSql(T_InStockTaskInfo model)
        {
            string strSql = " where isnull(isDel,0) != 2  ";

            string strAnd = " and ";

            strSql += strAnd;
            strSql += "TaskType = 1 ";

            if (!string.IsNullOrEmpty(model.SupcusCode))
            {
                strSql += strAnd;
                strSql += " (SupcusCode Like '" + model.SupcusCode + "%'  or SupplierName Like '" + model.SupcusCode + "%' )";
            }

            if (model.DateFrom != null)
            {
                strSql += strAnd;
                strSql += " CreateTime >= " + model.DateFrom.ToDateTime().Date.AddDays(-1).ToOracleTimeString() + " ";
            }

            if (model.DateTo != null)
            {
                strSql += strAnd;
                strSql += " CreateTime <= " + model.DateTo.ToDateTime().Date.AddDays(1).ToOracleTimeString() + " ";
            }

            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " erpvoucherno Like '" + model.ErpVoucherNo + "%'  ";
            }

            if (model.VoucherType > 0)
            {
                strSql += strAnd;
                strSql += " vouchertype ='" + model.VoucherType + "'  ";
            }

            if (!string.IsNullOrEmpty(model.TaskNo))
            {
                strSql += strAnd;
                strSql += " TaskNo ='" + model.TaskNo + "'  ";
            }

            if (!string.IsNullOrEmpty(model.ReceiveUserNo))
            {
                strSql += strAnd;
                strSql += " Creater ='" + model.ReceiveUserNo + "'  ";
            }

            //if (!string.IsNullOrEmpty(model.StrStatus))
            //{
            //    string strStatus = string.Empty;
            //    string[] strSplit = model.StrStatus.Split('&');
            //    int sLen = strSplit.Length;

            //    foreach (var item in strSplit)
            //    {
            //        switch (item.Trim())
            //        {
            //            case "新建":
            //                strStatus += "1" + (sLen > 1 ? "," : "");
            //                break;
            //            case "部分上架":
            //                strStatus += "2" + (sLen > 1 ? "," : "");
            //                break;
            //            case "全部上架":
            //                strStatus += "3" + (sLen > 1 ? "," : "");
            //                break;
            //            case "已关闭":
            //                strStatus += "5" + (sLen > 1 ? "," : "");
            //                break;
            //            default:
            //                strStatus += "0" + (sLen > 1 ? "," : "");
            //                break;
            //        }
            //    }
            //    strStatus = strStatus.TrimEnd(',');
            //    strSql += strAnd;
            //    strSql += "isnull(linestatus,1) in (" + strStatus + ")";

            //}

            return strSql;
        }


        public int GetIDByTaskNo(string TaskNo)
        {
            string strSql = "select id from t_task where taskno = '" + TaskNo + "'";

            return base.GetScalarBySql(strSql).ToInt32();
        }

        /// <summary>
        /// 根据获取到上架任务明细后，再获取库存信息
        /// </summary>
        /// <param name="headerID"></param>
        /// <returns></returns>
        public override List<T_InStockTaskDetailsInfo> GetModelListByHeaderID(int headerID)
        {
            string strErrMsg = string.Empty;
            List<T_AreaInfo> areaList = new List<T_AreaInfo>();
            T_Stock_Func tfunc = new T_Stock_Func();
            List<T_InStockTaskDetailsInfo> list = base.GetModelListByHeaderID(headerID);

            foreach (T_InStockTaskDetailsInfo item in list)
            {
                item.RemainQty = item.TaskQty1;
            }

            //获取推荐库位
            if (GetRecommendAreaNo(list, ref areaList, ref strErrMsg) == false)
            {
                throw new Exception(strErrMsg);
            }

            foreach (var item in list)
            {
                item.lstArea = areaList.FindAll(t => t.MaterialNo == item.MaterialNo);
                if (!(item.lstArea == null || item.lstArea. Count == 0))
                {
                    item.AreaNo = item.lstArea[0].AreaNo;
                }
                else
                {
                    item.AreaNo = "";
                }

            }

            return list;
        }

        public  List<T_InStockTaskDetailsInfo> GetModelListByHeaderIDForPc(int headerID)
        {
            return base.GetModelListByHeaderID(headerID); 
        }

        public bool GetRecommendAreaNo(List<T_InStockTaskDetailsInfo> modelList, ref List<T_AreaInfo> areaList, ref string strError)
        {
            try
            {

                int iResult = 0;
                DataSet ds;

                string strInStockTaskXml = XmlUtil.Serializer(typeof(List<T_InStockTaskDetailsInfo>), modelList);

                dbFactory.dbF.CreateParameters(3);
                dbFactory.dbF.AddParameters(0, "@strInStockTaskXml", SqlDbType.Xml);
                dbFactory.dbF.AddParameters(1, "@bResult", SqlDbType.Int, 0);
                dbFactory.dbF.AddParameters(2, "@ErrString", SqlDbType.NVarChar, 200);

                dbFactory.dbF.Parameters[0].Value = strInStockTaskXml;
                dbFactory.dbF.Parameters[1].Direction = System.Data.ParameterDirection.Output;
                dbFactory.dbF.Parameters[2].Direction = System.Data.ParameterDirection.Output;

                ds = dbFactory.ExecuteDataSetForCursor2(ref iResult, ref strError, dbFactory.ConnectionStringLocalTransaction, CommandType.StoredProcedure, "p_getRecommendArea", dbFactory.dbF.Parameters);//ExecuteNonQuery2(dbFactory.ConnectionStringLocalTransaction, CommandType.StoredProcedure, "p_getRecommendArea", dbFactory.dbF.Parameters);
                iResult = Convert.ToInt32(dbFactory.dbF.Parameters[1].Value);
                strError = dbFactory.dbF.Parameters[2].Value.ToString();

                if (iResult == 1)
                {
                    areaList = TOOL.DataTableToList.DataSetToList<T_AreaInfo>(ds.Tables[0]);
                    strError = string.Empty;
                }

                return iResult == 1 ? true : false;

            //    int iResult = 0;
            //    DataSet ds;

            //    string strInStockTaskXml = XmlUtil.Serializer(typeof(List<T_InStockTaskDetailsInfo>), modelList);
            //    LogNet.LogInfo("GetRecommendAreaNo---" + strInStockTaskXml);
            //    OracleParameter[] cmdParms = new OracleParameter[] 
            //{
            //    new OracleParameter("strInStockTaskXml", OracleDbType.NClob),    
            //    new OracleParameter("RecommendAreaCur", OracleDbType.RefCursor,ParameterDirection.Output),
            //    new OracleParameter("bResult", OracleDbType.Int32,ParameterDirection.Output),
            //    new OracleParameter("ErrString", OracleDbType.NVarchar2,200,strError,ParameterDirection.Output)
            //};

            //    cmdParms[0].Value = strInStockTaskXml;

            //    cmdParms[1].Value = ParameterDirection.Output;
            //    cmdParms[2].Value = ParameterDirection.Output;
            //    cmdParms[3].Value = ParameterDirection.Output;

            //    ds = dbFactory.ExecuteDataSetForCursor(ref iResult, ref strError, dbFactory.ConnectionStringLocalTransaction, CommandType.StoredProcedure, "p_getRecommendArea", cmdParms);

            //    if (iResult == 1)
            //    {
            //        areaList = TOOL.DataTableToList.DataSetToList<T_AreaInfo>(ds.Tables[0]);
            //        strError = string.Empty;
            //    }

            //    return iResult == 1 ? true : false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

