using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;
using BILBasic.User;
using BILWeb.Quality;
using BILWeb.OutStockTask;
using BILWeb.OutStock;
using System.Data;
using System.Reflection;
using BILBasic.Basing.Factory;
using BILWeb.OutStockCreate;

namespace BILWeb.Stock
{
    public partial class T_Stock_DB : BILBasic.Basing.Factory.Base_DB<T_StockInfo>
    {

        string strError = string.Empty;

        /// <summary>
        /// 添加t_stock
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_StockInfo t_stock)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        /// <summary>
        /// 仓库内移库操作
        /// </summary>
        /// <param name="user"></param>
        /// <param name="t_stock"></param>
        /// <returns></returns>
        protected override List<string> GetSaveSql(UserModel user, ref  T_StockInfo t_stock)
        {
            return null;
            //string strSql1 = string.Empty;
            //string strSql2 = string.Empty;

            //List<string> lstSql = new List<string>();

            //strSql1 = string.Format("update t_stock a set a.warehouseid = '{0}',a.houseid = '{1}',a.areaid = '{2}' where serialno ='{3}'",
            //                        t_stock.WareHouseID, t_stock.HouseID, t_stock.AreaID, t_stock.SerialNo);

            //lstSql.Add(strSql1);

            ////strSql2 = string.Format("select * from t_Stocktrans(Id, Barcode, Serialno, Materialno, Materialdesc, Warehouseid, Houseid, Areaid, Qty, Tmaterialno, Tmaterialdesc, Pickareano, Celareano, Status, Isdel, Creater, Createtime, Modifyer, Modifytime, Batchno, Sn, Returnsupcode, Returnreson, Returnsupname, Oldstockid, Taskdetailesid, Checkid, Transferdetailsid, Returntype, Returntypedesc, Unit, Salename, Unitname, Palletno, Receivestatus)" +
            ////                    "select seq_stocktrans_id.Nextval,barcode,Serialno, Materialno, Materialdesc, Warehouseno, Houseno, Areano, Qty, Tmaterialno, Tmaterialdesc, Pickareano, Celareano, Status, Isdel, '{0}', getdate(), Modifyer, Modifytime, Batchno, Sn, Returnsupcode, Returnreson, Returnsupname, Oldstockid, Taskdetailesid, Checkid, Transferdetailsid, Returntype, Returntypedesc, Unit, Salename, Unitname, Palletno, Receivestatus from t_stock where serialno = '{1}'", user.UserNo, t_stock.SerialNo);

            //strSql2 = "insert into t_tasktrans(Id, Serialno,towarehouseid,Tohouseid, Toareaid, fromwarehouseid,fromhouseid, fromareaid, Materialno, Materialdesc," +
            //          " Qty, Tasktype,  Creater, Createtime, Unitcode, Unitname,partno,materialnoid)" +
            //                " values (seq_stocktrans_id.Nextval,'" + t_stock.SerialNo + "','" + t_stock.WareHouseID + "','" + t_stock.HouseID + "','" + t_stock.AreaID + "',"+
            //                " '" + t_stock.FromWareHouseID + "', '" + t_stock.FromHouseID + "', '" + t_stock.FromAreaID + "',  '" + t_stock.MaterialNo + "','" + t_stock.MaterialDesc + "' " +
            //                ",'" + t_stock.Qty + "', '3' , '" + user.UserName + "',getdate(), '"+t_stock.Unit+"','"+t_stock.UnitName+"','"+t_stock.PartNo+"','"+t_stock.MaterialNoID+"')";
            //lstSql.Add(strSql2);

            //BILBasic.TOOL.WriteLogMethod.WriteLog(strSql2);

            //return lstSql;
        }

        protected override List<string> GetSaveModelListSql(UserModel user, List<T_StockInfo> modelList)
        {
            string strSql1 = string.Empty;
            string strSql2 = string.Empty;
            int taskTransID = 0;
            List<string> lstSql = new List<string>();

            foreach (var item in modelList)
            {
                taskTransID = base.GetTableIDBySqlServerTaskTrans("t_tasktrans");
                item.Status = 3;
                //扫描到的是外箱，需要拆托盘
                if (item.IsPalletOrBox == 1)
                {
                    strSql1 = string.Format("update t_stock  set warehouseid = (select id from t_Warehouse where warehouseno = '{0}'  ),houseid = (select HOUSEID from v_area where warehouseno = '{1}' and areano = '{2}' ),areaid = (select id from v_area where warehouseno = '{3}' and areano = '{4}'),status = '{5}',qty = '{6}',palletno = '' where serialno ='{7}'",
                                    item.ToErpWarehouse, item.ToErpWarehouse, item.ToErpAreaNo, item.ToErpWarehouse, item.ToErpAreaNo, item.Status, item.Qty, item.SerialNo);
                    lstSql.Add(strSql1);

                    if (!string.IsNullOrEmpty(item.PalletNo))
                    {
                        strSql1 = "delete t_Palletdetail where BARCODE = '" + item.Barcode + "'";
                        lstSql.Add(strSql1);

                        strSql1 = "delete t_Pallet where palletno = '" + item.PalletNo + "' and (select count(1) from t_Palletdetail where palletno = '" + item.PalletNo + "')=0";
                        lstSql.Add(strSql1);
                    }                    
                }
                else 
                {
                    strSql1 = string.Format("update t_stock  set warehouseid = (select id from t_Warehouse where warehouseno = '{0}'  ),houseid = (select HOUSEID from v_area where warehouseno = '{1}' and areano = '{2}' ),areaid = (select id from v_area where warehouseno = '{3}' and areano = '{4}'),status = '{5}',qty = '{6}' where serialno ='{7}'",
                                    item.ToErpWarehouse, item.ToErpWarehouse, item.ToErpAreaNo, item.ToErpWarehouse, item.ToErpAreaNo, item.Status, item.Qty, item.SerialNo);
                    lstSql.Add(strSql1);
                }

                strSql2 = "SET IDENTITY_INSERT t_tasktrans on ;insert into t_tasktrans(id, Serialno,towarehouseid,Tohouseid, Toareaid, Materialno, Materialdesc, Supcuscode, " +
                                "Supcusname, Qty, Tasktype, Creater, Createtime,TaskdetailsId, Unit, Unitname,materialnoid," +
                                "barcode,STRONGHOLDCODE,STRONGHOLDNAME,COMPANYCODE,SUPPRDBATCH,EDATE,batchno,Fromareaid,Fromwarehouseid,Fromhouseid,STATUS,houseprop,ean,FromWarehouseNo,FromWarehouseName,FromHouseNo,FromAreaNo,ToWarehouseNo,ToWarehouseName,ToHouseNo,ToAreaNo,PalletNo,IsPalletOrBox)" +
                            " values ('"+taskTransID+"','" + item.SerialNo + "',(select id from t_Warehouse a  where a.Warehouseno = '" + item.ToErpWarehouse + "'),(select a.HOUSEID from v_Area a where a.warehouseno = '" + item.ToErpWarehouse + "' and a.AREANO = '" + item.ToErpAreaNo + "')," +
                            "(select a.ID from v_Area a where a.warehouseno = '" + item.ToErpWarehouse + "' and a.AREANO = '" + item.ToErpAreaNo + "'),'" + item.MaterialNo + "','" + item.MaterialDesc + "','" + item.SupCode + "','" + item.SupName + "'," +
                            " '" + item.Qty + "','3' ,'" + user.UserName + "',getdate(),'" + item.ID + "'," +
                            "'" + item.Unit + "','" + item.UnitName + "','" + item.MaterialNoID + "','" + item.Barcode + "'," +
                            "'" + item.StrongHoldCode + "','" + item.StrongHoldName + "','" + item.CompanyCode + "','" + item.SupPrdBatch + "'" +
                            " ,'" + item.EDate + "','" + item.BatchNo + "','" + item.AreaID + "','" + item.WareHouseID + "','" + item.HouseID + "','" + item.Status + "',"+
                            " '"+item.HouseProp+"','"+item.EAN+"',"+
                            " '"+item.WarehouseNo+"', "+
                            " (select WAREHOUSENAME from T_WAREHOUSE where warehouseno = '" + item.WarehouseNo + "')," +
                            " '"+item.HouseNo+"',"+
                            " '"+item.AreaNo+"',"+
                            " '"+item.ToErpWarehouse+"',"+
                            " (select WAREHOUSENAME from T_WAREHOUSE where warehouseno = '" + item.ToErpWarehouse + "'),"+
                            " (SELECT HOUSENO from v_area where WAREHOUSENO = '" + item.WarehouseNo + "' AND AREANO = '" + item.AreaNo + "'),"+
                            " '" + item.ToErpAreaNo + "','" + item.PalletNo + "','" + item.IsPalletOrBox + "' ) SET IDENTITY_INSERT t_tasktrans off";

                lstSql.Add(strSql2);

                
            }


            //BILBasic.TOOL.WriteLogMethod.WriteLog(strSql2);

            return lstSql;
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_StockInfo ToModel(IDataReader reader)
        {
            T_StockInfo t_stock = new T_StockInfo();

            t_stock.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_stock.Barcode = (string)dbFactory.ToModelValue(reader, "BARCODE");
            t_stock.SerialNo = (string)dbFactory.ToModelValue(reader, "SERIALNO");
            t_stock.MaterialNo = (string)dbFactory.ToModelValue(reader, "MATERIALNO");
            t_stock.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MATERIALDESC");
            //t_stock.WarehouseNo = (string)dbFactory.ToModelValue(reader, "WAREHOUSENO");
            //t_stock.HouseNo = (string)dbFactory.ToModelValue(reader, "HOUSENO");
            //t_stock.AreaNo = (string)dbFactory.ToModelValue(reader, "AREANO");
            //t_stock.FromWareHouseNo = (string)dbFactory.ToModelValue(reader, "WAREHOUSENO");
            //t_stock.FromAreaNo = (string)dbFactory.ToModelValue(reader, "AREANO");
            if (Common_Func.readerExists(reader, "WAREHOUSENO")) t_stock.WarehouseNo = reader["WAREHOUSENO"].ToDBString();
            if (Common_Func.readerExists(reader, "HouseNo")) t_stock.HouseNo = reader["HouseNo"].ToDBString();
            if (Common_Func.readerExists(reader, "AreaNo")) t_stock.AreaNo = reader["AreaNo"].ToDBString();
            if (Common_Func.readerExists(reader, "WAREHOUSENO")) t_stock.FromWareHouseNo = reader["WAREHOUSENO"].ToDBString();
            if (Common_Func.readerExists(reader, "AREANO")) t_stock.FromAreaNo = reader["AREANO"].ToDBString();

            t_stock.Qty =  dbFactory.ToModelValue(reader, "QTY").ToInt32();
            t_stock.TMaterialNo = (string)dbFactory.ToModelValue(reader, "TMATERIALNO");
            t_stock.TMaterialDesc = (string)dbFactory.ToModelValue(reader, "TMATERIALDESC");
            t_stock.PickAreaNo = (string)dbFactory.ToModelValue(reader, "PICKAREANO");
            t_stock.CelareaNo = (string)dbFactory.ToModelValue(reader, "CELAREANO");
            t_stock.Status = dbFactory.ToModelValue(reader, "STATUS").ToInt32();
            t_stock.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToInt32();
            t_stock.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_stock.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_stock.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_stock.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_stock.BatchNo = (string)dbFactory.ToModelValue(reader, "BATCHNO");
            t_stock.SN = (string)dbFactory.ToModelValue(reader, "SN");
            t_stock.ReturnSupCode = (string)dbFactory.ToModelValue(reader, "RETURNSUPCODE");
            t_stock.ReturnReson = (string)dbFactory.ToModelValue(reader, "RETURNRESON");
            t_stock.ReturnSupName = (string)dbFactory.ToModelValue(reader, "RETURNSUPNAME");
            t_stock.OldStockID = dbFactory.ToModelValue(reader, "OLDSTOCKID").ToInt32();
            t_stock.TaskDetailesID = dbFactory.ToModelValue(reader, "TASKDETAILESID").ToInt32();
            t_stock.CheckID = dbFactory.ToModelValue(reader, "CHECKID").ToInt32();
            t_stock.TransferDetailsID = dbFactory.ToModelValue(reader, "TRANSFERDETAILSID").ToInt32();
            t_stock.ReturnType = dbFactory.ToModelValue(reader, "RETURNTYPE").ToInt32();
            t_stock.ReturnTypeDesc = (string)dbFactory.ToModelValue(reader, "RETURNTYPEDESC");
            t_stock.Unit = (string)dbFactory.ToModelValue(reader, "UNIT");
            t_stock.SaleName = (string)dbFactory.ToModelValue(reader, "SALENAME");
            t_stock.UnitName = (string)dbFactory.ToModelValue(reader, "UNITNAME");
            t_stock.PalletNo = (string)dbFactory.ToModelValue(reader, "PALLETNO");
            t_stock.ReceiveStatus = dbFactory.ToModelValue(reader, "RECEIVESTATUS").ToInt32();

            t_stock.WareHouseID = dbFactory.ToModelValue(reader, "WareHouseID").ToInt32();
            t_stock.HouseID = dbFactory.ToModelValue(reader, "HouseID").ToInt32();
            t_stock.AreaID = dbFactory.ToModelValue(reader, "AreaID").ToInt32();

            t_stock.PartNo = (string)dbFactory.ToModelValue(reader, "PartNo");
            t_stock.MaterialNoID = dbFactory.ToModelValue(reader, "MaterialNoID").ToInt32();

            t_stock.IsLimitStock = dbFactory.ToModelValue(reader, "IsLimitStock").ToInt32();
            t_stock.IsQuality = dbFactory.ToModelValue(reader, "IsQuality").ToInt32();

            t_stock.CompanyCode = (string)dbFactory.ToModelValue(reader, "CompanyCode");
            t_stock.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "StrongHoldCode");
            t_stock.StrongHoldName = (string)dbFactory.ToModelValue(reader, "StrongHoldName");
            //t_stock.AreaType = dbFactory.ToModelValue(reader, "AreaType").ToInt32();
            if (Common_Func.readerExists(reader, "AreaType")) t_stock.AreaType = reader["AreaType"].ToInt32();

            t_stock.EDate = dbFactory.ToModelValue(reader, "EDate").ToDateTime();
            t_stock.StrEDate = t_stock.EDate.ToString("yyyy/MM/dd");
            t_stock.BatchNo = dbFactory.ToModelValue(reader, "BatchNo").ToDBString();
            t_stock.MaterialChangeID = dbFactory.ToModelValue(reader, "MaterialChangeID").ToInt32();

            if (Common_Func.readerExists(reader, "WAREHOUSENO")) t_stock.FromErpWarehouse = reader["WAREHOUSENO"].ToDBString();
            if (Common_Func.readerExists(reader, "AREANO")) t_stock.FromErpAreaNo = reader["AREANO"].ToDBString();

            //t_stock.FromErpWarehouse = (string)dbFactory.ToModelValue(reader, "WAREHOUSENO");
            //t_stock.FromErpAreaNo = (string)dbFactory.ToModelValue(reader, "AREANO");
            t_stock.FromBatchNo = (string)dbFactory.ToModelValue(reader, "BatchNo");
            //t_stock.UnitTypeCode = dbFactory.ToModelValue(reader, "UnitTypeCode").ToDBString();
            //t_stock.DecimalLngth = dbFactory.ToModelValue(reader, "DecimalLngth").ToDBString();

            T_OutTaskDetails_Func tfunc = new T_OutTaskDetails_Func();
            T_OutStockTaskDetailsInfo model = new T_OutStockTaskDetailsInfo();
            model.ID = t_stock.TaskDetailesID.ToInt32();
            if (model.ID > 0)
            {
                tfunc.GetModelByID(ref model, ref strError);
                t_stock.OutstockDetailID = model.OutstockDetailID;
                t_stock.ErpVoucherNo = model.ErpVoucherNo;
            }



            t_stock.ProductDate = dbFactory.ToModelValue(reader, "ProductDate").ToDateTime();
            t_stock.SupPrdBatch = dbFactory.ToModelValue(reader, "SupPrdBatch").ToDBString();
            t_stock.SupPrdDate = dbFactory.ToModelValue(reader, "SupPrdDate").ToDateTime();
            t_stock.IsRetention = dbFactory.ToModelValue(reader, "IsRetention").ToDBString();
            //t_stock.StrStatus = dbFactory.ToModelValue(reader, "StrStatus").ToDBString();
            if (Common_Func.readerExists(reader, "StrStatus")) t_stock.StrStatus = reader["StrStatus"].ToDBString();

            t_stock.HouseProp = dbFactory.ToModelValue(reader, "HouseProp").ToInt32();
            if (Common_Func.readerExists(reader, "FloorType")) t_stock.FloorType = reader["FloorType"].ToInt32();

            //t_stock.FloorType = dbFactory.ToModelValue(reader, "FloorType").ToInt32();
            t_stock.EAN = dbFactory.ToModelValue(reader, "EAN").ToDBString();
            if (Common_Func.readerExists(reader, "SortArea")) t_stock.SortArea = reader["SortArea"].ToDBString();
            //t_stock.SortArea = dbFactory.ToModelValue(reader, "SortArea").ToDBString();
            t_stock.IsAmount = dbFactory.ToModelValue(reader, "IsAmount").ToInt32();
            t_stock.BarCodeType = dbFactory.ToModelValue(reader, "BarCodeType").ToInt32();//GetBarCodeType(t_stock.Barcode);
            t_stock.Spec = dbFactory.ToModelValue(reader, "Spec").ToDBString();
            t_stock.TracNo = dbFactory.ToModelValue(reader, "TracNo").ToDBString();
            

            return t_stock;
        }

        public int GetBarCodeType(string strBarCode) 
        {
            string strSql = "SELECT barcodetype from T_OUTBARCODE where barcode = '" + strBarCode + "'";
            return base.GetScalarBySql(strSql).ToInt32();
        }

        protected override string GetViewName()
        {
            return "v_stock";
        }

        protected override string GetTableName()
        {
            return "T_STOCK";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override string GetFilterSql(UserModel user, T_StockInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";
            if (!string.IsNullOrEmpty(model.AreaNo))
            {
                strSql += strAnd;
                strSql += " areano = '" + model.AreaNo + "' ";
            }
            if (!string.IsNullOrEmpty(model.EAN))
            {
                strSql += strAnd;
                strSql += " EAN = '" + model.EAN + "' ";
            }
            if (!string.IsNullOrEmpty(model.Barcode))
            {
                strSql += strAnd;
                strSql += " Barcode = '" + model.Barcode + "' ";
            }
            if (!string.IsNullOrEmpty(model.BatchNo))
            {
                strSql += strAnd;
                strSql += " BatchNo = '" + model.BatchNo + "' ";
            }

            return strSql + " order by id ";
        }


        public bool saveMoveBarcode(UserModel user, T_StockInfo model, Area.T_AreaInfo areaInfo, out string errMsg, out string NewSerialNo)
        {
            NewSerialNo = "";
            errMsg = "";
            List<string> listSql = new List<string>();
            string strSql = "";
            try
            {
                if (model.SerialNo == null || model.SerialNo == "")
                {
                    listSql.Add("update t_stock set Areaid =" + areaInfo.ID + ",houseid=" + areaInfo.HouseID + " where houseid =" + model.HouseID + " and Areaid =" + model.AreaID + "");

                    strSql = "insert into t_tasktrans( Serialno,towarehouseID,TohouseID, ToareaID, Materialno, Materialdesc, Supcuscode, " +
                                 "Supcusname, Qty, Tasktype, Vouchertype, Creater, Createtime,TaskdetailsId, Unit, Unitname,partno,materialnoid,erpvoucherno," +
                                 "Strongholdcode,Strongholdname,Companycode,Supprdbatch,Edate,Batchno,Barcode, fromwarehouseid,fromhouseid, fromareaid)" +
                                 " values ('" + model.SerialNo + "','" + areaInfo.WarehouseID + "','" + areaInfo.HouseID + "','" + areaInfo.ID + "'," +
                                 " '" + model.MaterialNo + "','" + model.MaterialDesc + "','" + model.SupCode + "','" + model.SupName + "','" + model.AmountQty + "','3'," +
                                 " 3 ,'" + user.UserName + "',getdate(),'" + model.ID + "', " +
                                 "'" + model.Unit + "','" + model.UnitName + "','" + model.PartNo + "','" + model.MaterialNoID + "','" + model.ErpVoucherNo + "'," +
                                 "'" + model.StrongHoldCode + "','" + model.StrongHoldName + "','" + model.CompanyCode + "'," +
                                 "  '" + model.SupPrdBatch + "','" + model.EDate + "' ,'" + model.BatchNo + "','" + model.Barcode + "'," + model.WareHouseID + "," + model.HouseID + "," + model.AreaID + ") ";

                    listSql.Add(strSql);
                }
                else
                {

                    var count = base.GetScalarBySql("select count(*)  from t_stock where Areaid =" + model.AreaID + " and  serialno ='" + model.SerialNo + "' and qty =" + model.Qty + "");
                    if (Convert.ToInt16(count) == 0)
                    {
                        errMsg = "扫描条码已发生变更,请重新扫描";
                        return false;
                    }

                    listSql = saveSplitBarCode(user, model, areaInfo, out NewSerialNo);

                    strSql = "insert into t_tasktrans( Serialno,towarehouseID,TohouseID, ToareaID, Materialno, Materialdesc, Supcuscode, " +
                                   "Supcusname, Qty, Tasktype, Vouchertype, Creater, Createtime,TaskdetailsId, Unit, Unitname,partno,materialnoid,erpvoucherno," +
                                   "Strongholdcode,Strongholdname,Companycode,Supprdbatch,Edate,Batchno,Barcode, fromwarehouseid,fromhouseid, fromareaid)" +
                                   " values ('" + model.SerialNo + "','" + areaInfo.WarehouseID + "','" + areaInfo.HouseID + "','" + areaInfo.ID + "'," +
                                   " '" + model.MaterialNo + "','" + model.MaterialDesc + "','" + model.SupCode + "','" + model.SupName + "','" + model.AmountQty + "','3'," +
                                   " 3 ,'" + user.UserName + "',getdate(),'" + model.ID + "', " +
                                   "'" + model.Unit + "','" + model.UnitName + "','" + model.PartNo + "','" + model.MaterialNoID + "','" + model.ErpVoucherNo + "'," +
                                   "'" + model.StrongHoldCode + "','" + model.StrongHoldName + "','" + model.CompanyCode + "'," +
                                   "  '" + model.SupPrdBatch + "','" + model.EDate + "' ,'" + model.BatchNo + "','" + model.Barcode + "'," + model.WareHouseID + "," + model.HouseID + "," + model.AreaID + ") ";

                    listSql.Add(strSql);
                    for (int i = 0; i < listSql.Count; i++)
                    {
                        LogNet.LogInfo("MoveSQL:" + listSql[i].ToString());
                    }
                }



                if (SaveModelListBySqlToDB(listSql, ref errMsg))
                {

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                errMsg = ex.ToString();
                return false;
            }

        }

        /// <summary>
        /// 拆零信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <param name="areaInfo"></param>
        /// <param name="NewSerialNo"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public List<string> saveSplitBarCode(UserModel user, T_StockInfo model, Area.T_AreaInfo areaInfo, out string NewSerialNo)
        {
            NewSerialNo = "";
            List<string> listSql = new List<string>();

            var value = base.GetScalarBySql("select serialno  from t_stock where Areaid =" + areaInfo.ID + " and materialno ='" + model.MaterialNo + "' and edate=CONVERT(datetime,'"+model.StrEDate+"',101) and batchno ='" + model.BatchNo + "' and  strongholdcode='" + model.StrongHoldCode + "' ");
            string areaSerialNO = "";
            if (!(value == null || value.ToString() == ""))
            {
                areaSerialNO = value.ToString();
            }
            if (model.Qty != model.AmountQty)
            {
                listSql.Add("update t_stock  set Qty = Qty - '" + model.AmountQty + "'  where serialno = '" + model.SerialNo + "'");
                if (areaSerialNO == "" || (areaSerialNO != "" && areaInfo.HouseProp != "2"))//目的库位不存在 或为整箱库存
                {
                    listSql.Add(GetAmountQtySql(model, ref NewSerialNo));
                    model.AreaID = areaInfo.ID;
                    model.HouseID = areaInfo.HouseID;
                    model.WareHouseID = areaInfo.WarehouseID;
                    model.Status = 3;
                    model.ReceiveStatus = 2;
                    model.IsLimitStock = 2;
                    model.IsQuality = 3;
                    listSql.Add(GetAmountQtyInsertStockSql(model, user, NewSerialNo));
                }
                else
                {
                    listSql.Add("update t_stock  set Qty = Qty + '" + model.AmountQty + "'  where serialno = '" + areaSerialNO + "'");
                }
            }
            else
            {
                if (value == null || value.ToString() == "")
                {
                    listSql.AddRange(GetSheveSql(model, areaInfo));
                }
                else
                {
                    if (areaInfo.HouseProp == "2")//零拣区
                    {
                        listSql.Add("delete t_stock  where serialno = '" + model.SerialNo + "'");
                        listSql.Add("update t_stock  set Qty = Qty + '" + model.AmountQty + "'  where serialno = '" + value + "'");
                    }
                    else
                    {
                        listSql.AddRange(GetSheveSql(model, areaInfo));
                        //string strSql3 = string.Format("update t_Stock  set Receivestatus = " + model.ReceiveStatus + " , islimitstock =" + model.IsLimitStock + ",Warehouseid = '{0}',Houseid = '{1}',Areaid = '{2}' where   serialno = '{3}'", areaInfo.WarehouseID, areaInfo.HouseID, areaInfo.ID, model.SerialNo);
                        //listSql.Add(strSql3);
                    }
                }
            }

            return listSql;
        }

        private List<string> GetSheveSql(T_StockInfo model, Area.T_AreaInfo areaInfo)
        {
            List<string> listSql = new List<string>();
            string strSql3 = string.Empty;
            //扫描的是整箱，需要更新库存托盘为空，同时删除托盘表记录
            if (model.IsPalletOrBox == 1)
            {
                strSql3 = string.Format("update t_Stock  set Receivestatus = " + model.ReceiveStatus + " , islimitstock =" + model.IsLimitStock + ",palletno = '',Warehouseid = '{0}',Houseid = '{1}',Areaid = '{2}' where   serialno = '{3}'", areaInfo.WarehouseID, areaInfo.HouseID, areaInfo.ID, model.SerialNo);
                listSql.Add(strSql3);

                if (!string.IsNullOrEmpty(model.PalletNo))
                {
                    strSql3 = "delete t_Palletdetail  where barcode = '" + model.Barcode + "'";
                    listSql.Add(strSql3);

                    strSql3 = "delete t_Pallet where palletno = '" + model.PalletNo + "' and (select count(1) from t_Palletdetail where palletno = '" + model.PalletNo + "')=0";
                    listSql.Add(strSql3);
                }
            }
            else //整托上架，不更新托盘号和删除托盘表记录
            {
                strSql3 = string.Format("update t_Stock  set Receivestatus = " + model.ReceiveStatus + " , islimitstock =" + model.IsLimitStock + ",Warehouseid = '{0}',Houseid = '{1}',Areaid = '{2}' where   serialno = '{3}'", areaInfo.WarehouseID, areaInfo.HouseID, areaInfo.ID, model.SerialNo);
                listSql.Add(strSql3);
            }

            return listSql;
        }

        /// <summary>
        /// 新条码写入条码表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="NewSerialNo"></param>
        /// <returns></returns>
        public string GetAmountQtySql(T_StockInfo model, ref string NewSerialNo)
        {
            //int barcodeID = GetTableIDBySqlServer("t_Outbarcode");//GetTableID("Seq_Outbarcode_Id");

            //string SeqSerialNo = base.GetTableID("SEQ_SERIAL_NO").ToString();

            //NewSerialNo = System.DateTime.Now.ToString("yyyyMMdd") + SeqSerialNo.PadLeft(6, '0');

            List<string> lstSql = new List<string>();

            var seed = Guid.NewGuid().GetHashCode();

            NewSerialNo = DateTime.Now.ToString("yyMMdd") + "77" + new Random(seed).Next(0, 999999).ToString().PadLeft(6, '0');//奥碧虹

            string strSqlCount = "SELECT COUNT(1) from T_OUTBARCODE where serialno = '" + NewSerialNo + "'";

            int i = base.GetScalarBySql(strSqlCount).ToInt32();

            if (i > 0)
            {
                NewSerialNo = DateTime.Now.ToString("yyMMdd") + "77" + new Random(seed).Next(0, 999999).ToString().PadLeft(6, '0');//奥碧虹
            }

            string BarCode = "" + model.BarCodeType + "" + model.MaterialNo.PadLeft(16, '0') + "" + model.BatchNo.PadLeft(11, '0') + "" + NewSerialNo + "";


            string strSql = "insert into t_Outbarcode ( Voucherno, Rowno, Erpvoucherno, Vouchertype, Materialno, Materialdesc, Cuscode," +
                            "Cusname, Supcode, Supname, Outpackqty, Innerpackqty, Voucherqty, Qty, Nopack, Printqty, Barcode, " +
                            "Barcodetype, Serialno, Barcodeno, Outcount, Innercount, Mantissaqty, Isrohs, Outbox_Id, " +
                            "Inner_Id, Abatchqty, Isdel, Creater, Createtime, Materialnoid, Strongholdcode, " +
                            "Strongholdname, Companycode, Productdate, Supprdbatch, Supprddate, Productbatch, Edate, Storecondition," +
                            "Specialrequire, Batchno, Barcodemtype, Rownodel, Protectway, Boxweight, Unit, Labelmark, Boxdetail, Matebatch," +
                            "Mixdate, Relaweight,Ean)" +
                            "select voucherno,rowno,erpvoucherno,vouchertype, Materialno, Materialdesc, Cuscode," +
                            "Cusname, Supcode, Supname, Outpackqty, Innerpackqty, Voucherqty, '" + model.AmountQty + "',Nopack, Printqty," +
                             "'" + BarCode + "'," +
                              "Barcodetype, '" + NewSerialNo + "', Barcodeno, Outcount, Innercount, Mantissaqty, Isrohs,'" + model.ID + "',Inner_Id, " +
                            "Abatchqty, Isdel, Creater, getdate(), Materialnoid, Strongholdcode, " +
                            "Strongholdname, Companycode, Productdate, Supprdbatch, Supprddate, Productbatch, Edate, Storecondition," +
                            "Specialrequire, Batchno, Barcodemtype, Rownodel, Protectway, Boxweight, Unit, Labelmark, Boxdetail, Matebatch," +
                            "Mixdate, Relaweight,ean from t_Outbarcode where serialno = '" + model.SerialNo + "'";
            return strSql;

        }
        /// <summary>
        /// 新条码写入库存新条码信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <param name="NewSerialNo"></param>
        /// <returns></returns>
        public string GetAmountQtyInsertStockSql(T_StockInfo model, UserModel user, string NewSerialNo)
        {
            //int stockID = GetTableID("Seq_Stock_Id");

            string strSql = "insert into t_Stock( Barcode, Serialno, Materialno, Materialdesc, Warehouseid, Houseid, Areaid, Qty, Status, Isdel," +
                            "Creater, Createtime, Batchno,  Oldstockid, Unit, Unitname,  " +
                            "Receivestatus,  Islimitstock,  Materialnoid, Strongholdcode, Strongholdname, Companycode," +
                            "Edate, Supcode, Supname, Productdate, Supprdbatch, Supprddate, Isquality,Stocktype,ean)" +
                            "select barcode,serialno,materialno,Materialdesc,'" + model.WareHouseID + "', '" + model.HouseID + "', '" + model.AreaID + "', Qty ,'" + model.Status + "','1'," +
                            "'" + user.UserNo + "',getdate(),batchno,'" + model.ID + "',unit,'" + model.UnitName + "','" + model.ReceiveStatus + "','" + model.IsLimitStock + "',Materialnoid," +
                            "Strongholdcode, Strongholdname, Companycode,Edate, Supcode, Supname, Productdate, Supprdbatch,Supprddate, '" + model.IsQuality + "',1,ean from t_Outbarcode where serialno = '" + NewSerialNo + "'";

            return strSql;
        }

        /// <summary>
        /// 移库用
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override string GetModelSql(T_StockInfo model)
        {
            string strSql = string.Format("select a.Id, Barcode, Serialno, Materialno, Materialdesc,a.warehouseid,a.houseid,  a.Areaid,b.warehouseno,b.houseno,b.AREANO, Qty, Tmaterialno, Tmaterialdesc, Pickareano, Celareano, Status, a.Isdel, a.Creater, " +
                                "a.Createtime, a.Modifyer, a.Modifytime, Batchno, Sn, Returnsupcode, Returnreson, Returnsupname, Oldstockid," +
                                "Taskdetailesid, a.Checkid, Transferdetailsid, Returntype, Returntypedesc, Unit, Salename, Unitname," +
                                "Palletno, Receivestatus, Salecode, Islimitstock, Partno, Materialnoid from t_stock a left join " +
                                "v_Area b on a.Areaid = b.ID where a.Serialno = '{0}'", model.SerialNo);
            return strSql;
        }


        public List<T_StockInfo> GetStockModelList(string SerialNo)
        {
            try
            {
                //先根据序列号汇总托盘数据
                string strSql = "select a.Id, Barcode, Serialno, Materialno, Materialdesc, a.Warehouseid, a.Houseid, a.Areaid, Qty, Tmaterialno," +
                            "Tmaterialdesc, Pickareano, Celareano, Status, a.Isdel, a.Creater, a.Createtime, a.Modifyer, a.Modifytime, Batchno," +
                            "Sn, Returnsupcode, Returnreson, Returnsupname, Oldstockid, Taskdetailesid,a.Checkid, Transferdetailsid," +
                            "Returntype, Returntypedesc, Unit, Salename, Unitname, Palletno, Receivestatus, Salecode, Islimitstock, " +
                            "Partno, Materialnoid,b.warehouseno,b.houseno,b.AREANO from t_stock a  left join v_area b on a.Areaid = b.ID" +
                            " where a.palletno = (select palletno from t_stock where serialno = '" + SerialNo + "' and isnull(isdel,0)=1 ) and isnull(Taskdetailesid,0) = 0 and isnull(a.Checkid,0) = 0 and isnull(Transferdetailsid,0) = 0 and isnull(islimitstock,1) = 1";

                List<T_StockInfo> lstStock = base.GetModelListBySql(strSql);

                if (lstStock != null && lstStock.Count > 0)
                {
                    return lstStock;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public string GetERPVoucherNoBySerialNo(string SerialNo)
        {
            try
            {
                string strSql = "select erpvoucherno from t_serialno where serialno = '" + SerialNo + "'";

                return base.GetScalarBySql(strSql).ToDBString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int GetLimitStockBySerialNo(string SerialNo)
        {
            try
            {
                string strSql = "select isnull(islimitstock,1) as islimitstock from t_stock where serialno  = '" + SerialNo + "'";

                return base.GetScalarBySql(strSql).ToInt32();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据批次先进先出获取推荐库位
        /// </summary>
        /// <param name="MaterialNo"></param>
        /// <returns></returns>
        public List<T_StockInfo> GetFIFOAreaNoByMaterial(string MaterialNo)
        {
            try
            {
                List<T_StockInfo> lstStock = new List<T_StockInfo>();

                string strSql = "select a.*,(select isnull(sum(qty),0)  from t_stock where  materialno = '" + MaterialNo + "'  and isnull(taskdetailesid,0) = 0 and isnull(checkid,0) = 0 " +
                                "and isnull(transferdetailsid,0) = 0 ) as qty from (select  distinct  (select t_area.Areano from t_area where t_area.id = t_stock.Areaid) AS Areano,batchno  from t_stock where   materialno = " +
                                "'" + MaterialNo + "' and isnull(qty,0) >0 and isnull(taskdetailesid,0) = 0 and isnull(checkid,0) = 0 " +
                                "and isnull(transferdetailsid,0) = 0   order by batchno asc) a  where rownum < 3";

                using (IDataReader reader = dbFactory.ExecuteReader(strSql))
                {
                    while (reader.Read())
                    {
                        T_StockInfo stock = new T_StockInfo();
                        stock.Qty = reader["qty"].ToDecimal();
                        stock.AreaNo = reader["AreaNo"].ToDBString();
                        lstStock.Add(stock);
                    }
                }

                if (lstStock == null || lstStock.Count == 0)
                {
                    return null;
                }
                else
                {
                    return lstStock;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 上架推荐库位
        /// </summary>
        /// <param name="MaterialNoID"></param>
        /// <returns></returns>
        //public List<T_StockInfo> GetInStockAreaNoByMaterialNoID(int MaterialNoID)
        //{
        //    try
        //    {
        //        List<T_StockInfo> lstStock = new List<T_StockInfo>();

        //        string strSql = "select a.* from (select  distinct  (select t_area.Areano from t_area where t_area.id = t_stock.Areaid and t_area.Areatype=1) AS Areano"+
        //                        " from t_stock where   materialnoid = '" + MaterialNoID + "' and isnull(qty,0) >0 and isnull(taskdetailesid,0) = 0 and isnull(checkid,0) = 0 " +
        //                        " and isnull(transferdetailsid,0) = 0  ) a";

        //        using (IDataReader reader = dbFactory.ExecuteReader(strSql))
        //        {
        //            while (reader.Read())
        //            {
        //                T_StockInfo stock = new T_StockInfo();                       
        //                stock.AreaNo = reader["AreaNo"].ToDBString();
        //                lstStock.Add(stock);
        //            }
        //        }

        //        if (lstStock == null || lstStock.Count == 0)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            return lstStock;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public List<T_StockInfo> GetStockByMaterialNo(string MaterialNoID)
        {
            try
            {
                List<T_StockInfo> lstStock = new List<T_StockInfo>();

                string strSql = "select b.Materialno,b.Materialdesc,isnull(c.Areano,'') as areano,isnull(sum(a.QTY),0) as qty,a.Strongholdname,a.Batchno,a.warehouseid,(case a.Status when 1 then '待检' when 2 then '送检' when 3 then '合格' when 4 then '不合格' end) as StrStatus,a.ean from t_Stock a left join t_Material b on a.Materialnoid = b.Id" +
                                " left join t_Area c on a.Areaid = c.Id where a.Materialnoid in (" + MaterialNoID + ") and isnull(a.Taskdetailesid,0)  = 0 and isnull(a.Checkid,0) = 0  " +
                                " group by b.Materialno,b.Materialdesc,c.Areano,a.Strongholdname,a.Batchno,a.Status,a.ean,a.warehouseid ORDER BY BATCHNO";

                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        T_StockInfo stockModel = new T_StockInfo();
                        stockModel.MaterialNo = dr["materialno"].ToDBString();
                        stockModel.AreaNo = dr["areano"].ToDBString();
                        stockModel.MaterialDesc = dr["materialdesc"].ToDBString();
                        stockModel.Qty = dr["qty"].ToInt32();
                        stockModel.StrongHoldName = dr["StrongHoldName"].ToDBString();
                        stockModel.BatchNo = dr["BatchNo"].ToDBString();
                        stockModel.StrStatus = dr["StrStatus"].ToDBString();
                        stockModel.EAN = dr["ean"].ToDBString();
                        stockModel.WareHouseID =dr["warehouseid"].ToInt32();
                        lstStock.Add(stockModel);
                    }
                }
                return lstStock;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T_StockInfo> GetStockByEAN(string EAN)
        {
            try
            {
                List<T_StockInfo> lstStock = new List<T_StockInfo>();

                string strSql = "select b.Materialno,b.Materialdesc,isnull(c.Areano,'') as areano,isnull(sum(a.QTY),0) as qty,a.Strongholdname,a.Batchno,a.warehouseid,(case a.Status when 1 then '待检' when 2 then '送检' when 3 then '合格' when 4 then '不合格' end) as StrStatus,a.ean from t_Stock a left join t_Material b on a.Materialnoid = b.Id" +
                                " left join t_Area c on a.Areaid = c.Id where a.ean = '" + EAN + "' and isnull(a.Taskdetailesid,0)  = 0 and isnull(a.Checkid,0) = 0  " +
                                " group by b.Materialno,b.Materialdesc,c.Areano,a.Strongholdname,a.Batchno,a.Status,a.ean,a.warehouseid ORDER BY BATCHNO";

                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        T_StockInfo stockModel = new T_StockInfo();
                        stockModel.MaterialNo = dr["materialno"].ToDBString();
                        stockModel.AreaNo = dr["areano"].ToDBString();
                        stockModel.MaterialDesc = dr["materialdesc"].ToDBString();
                        stockModel.Qty = dr["qty"].ToInt32();
                        stockModel.StrongHoldName = dr["StrongHoldName"].ToDBString();
                        stockModel.BatchNo = dr["BatchNo"].ToDBString();
                        stockModel.StrStatus = dr["StrStatus"].ToDBString();
                        stockModel.EAN = dr["ean"].ToDBString();
                        stockModel.WareHouseID = dr["warehouseid"].ToInt32();
                        lstStock.Add(stockModel);
                    }
                }
                return lstStock;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<T_StockInfo> GetStockByBatchNo(string BatchNo)
        {
            try
            {
                List<T_StockInfo> lstStock = new List<T_StockInfo>();

                string strSql = "select b.Materialno,b.Materialdesc,sum(a.Qty) as qty,a.Strongholdname,a.Batchno,c.areano , (case a.Status when 1 then '待检' when 2 then '送检' when 3 then '合格' when 4 then '不合格' end) as StrStatus,a.ean,a.warehouseid from t_Stock a left join t_Material b on a.Materialnoid = b.Id" +
                                " left join t_Area c on a.Areaid = c.Id where a.batchno like '" + BatchNo + "%' and isnull(a.Taskdetailesid,0)  = 0 and isnull(a.Checkid,0) = 0 " +
                                " group by b.Materialno,b.Materialdesc,a.Strongholdname,a.Batchno,a.Status,c.areano,a.ean,a.warehouseid";

                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        T_StockInfo stockModel = new T_StockInfo();
                        stockModel.MaterialNo = dr["materialno"].ToDBString();
                        stockModel.AreaNo = dr["areano"].ToDBString();
                        stockModel.MaterialDesc = dr["materialdesc"].ToDBString();
                        stockModel.Qty = dr["qty"].ToInt32();
                        stockModel.StrongHoldName = dr["StrongHoldName"].ToDBString();
                        stockModel.BatchNo = dr["BatchNo"].ToDBString();
                        stockModel.StrStatus = dr["StrStatus"].ToDBString();
                        stockModel.EAN = dr["ean"].ToDBString();
                        stockModel.WareHouseID = dr["warehouseid"].ToInt32();
                        lstStock.Add(stockModel);
                    }
                }
                return lstStock;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T_StockInfo> GetStockBySupplierNo(string SupplierNo)
        {
            try
            {
                List<T_StockInfo> lstStock = new List<T_StockInfo>();

                string strSql = "select b.Materialno,b.Materialdesc,sum(a.Qty) as qty,a.Strongholdname,a.Batchno,c.areano,(case a.Status when 1 then '待检' when 2 then '送检' when 3 then '合格' when 4 then '不合格' end) as StrStatus from t_Stock a left join t_Material b on a.Materialnoid = b.Id" +
                                " left join t_Area c on a.Areaid = c.Id where a.Supcode like '" + SupplierNo + "%' and isnull(a.Taskdetailesid,0)  = 0 and isnull(a.Checkid,0) = 0 " +
                                " group by b.Materialno,b.Materialdesc,a.Strongholdname,a.Batchno,a.Status,c.areano";

                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        T_StockInfo stockModel = new T_StockInfo();
                        stockModel.MaterialNo = dr["materialno"].ToDBString();
                        stockModel.AreaNo = dr["areano"].ToDBString();
                        stockModel.MaterialDesc = dr["materialdesc"].ToDBString();
                        stockModel.Qty = dr["qty"].ToInt32();
                        stockModel.StrongHoldName = dr["StrongHoldName"].ToDBString();
                        stockModel.BatchNo = dr["BatchNo"].ToDBString();
                        stockModel.StrStatus = dr["StrStatus"].ToDBString();

                        lstStock.Add(stockModel);
                    }
                }
                return lstStock;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T_StockInfo> GetStockByAreaNo(string AreaNo)
        {
            try
            {
                List<T_StockInfo> lstStock = new List<T_StockInfo>();

                string strSql = "select b.Materialno,b.Materialdesc,sum(a.Qty) as qty,a.Strongholdname,a.Batchno,c.areano,(case a.Status when 1 then '待检' when 2 then '送检' when 3 then '合格' when 4 then '不合格' end) as StrStatus,a.ean,a.warehouseid from t_Stock a left join t_Material b on a.Materialnoid = b.Id" +
                                " left join t_Area c on a.Areaid = c.Id where c.Areano = '" + AreaNo + "' and isnull(a.Taskdetailesid,0)  = 0 and isnull(a.Checkid,0) = 0 " +
                                " group by b.Materialno,b.Materialdesc,a.Strongholdname,a.Batchno,a.Status,c.areano,a.ean,a.warehouseid";

                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        T_StockInfo stockModel = new T_StockInfo();
                        stockModel.MaterialNo = dr["materialno"].ToDBString();
                        stockModel.MaterialDesc = dr["materialdesc"].ToDBString();
                        stockModel.Qty = dr["qty"].ToInt32();
                        stockModel.StrongHoldName = dr["StrongHoldName"].ToDBString();
                        stockModel.BatchNo = dr["BatchNo"].ToDBString();
                        stockModel.AreaNo = dr["AreaNo"].ToDBString();
                        stockModel.StrStatus = dr["StrStatus"].ToDBString();
                        stockModel.EAN = dr["ean"].ToDBString();
                        stockModel.WareHouseID = dr["warehouseid"].ToInt32();
                        lstStock.Add(stockModel);
                    }
                }
                return lstStock;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T_StockInfo> GetStockByBarcode(string SerialNo)
        {
            try
            {
                List<T_StockInfo> models = new List<T_StockInfo>();
                T_StockInfo model = new T_StockInfo();
                string strSql = "select A.Palletno,A.Areaid,a.Warehouseid,a.Houseid,a.Qty,a.Barcode,a.Serialno,a.Strongholdcode,(case a.Status when 1 then '待检' when 2 then '送检' when 3 then '合格' when 4 then '不合格' end) as StrStatus,c.areano," +
                                "a.Strongholdname,a.Companycode,a.Batchno,a.Materialno,a.Materialdesc from t_Stock a left join t_Material b on a.Materialnoid = b.Id left join t_Area c on a.Areaid = c.Id" +
                                " where a.Serialno = '" + SerialNo + "' or a.barcode='" + SerialNo + "'";

                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
                {
                    if (dr.Read())
                    {
       
                        model.MaterialNo = dr["materialno"].ToDBString();
                        model.MaterialDesc = dr["materialdesc"].ToDBString();
                        model.Qty = dr["Qty"].ToInt32();
                        model.StrongHoldName = dr["StrongHoldName"].ToDBString();
                        model.BatchNo = dr["BatchNo"].ToDBString();
                        model.AreaNo = dr["AreaNo"].ToDBString();
                        model.StrStatus = dr["StrStatus"].ToDBString();
                        //model.EAN = dr["ean"].ToDBString();//
                        model.WareHouseID = dr["warehouseid"].ToInt32();

                    }
                }
                models.Add(model);
                return models;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T_StockInfo GetStockBySerialNo(string SerialNo)
        {
            try
            {
                T_StockInfo model = new T_StockInfo();

                string strSql = "select A.Palletno,A.Areaid,a.Warehouseid,a.Houseid,a.Qty,a.Barcode,a.Serialno,a.Strongholdcode," +
                                "a.Strongholdname,a.Companycode,a.Batchno,a.Materialno,a.Materialdesc from t_Stock a left join t_Material b on a.Materialnoid = b.Id" +
                                " where a.Serialno = '" + SerialNo + "'";

                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
                {
                    if (dr.Read())
                    {
                        model.MaterialNo = dr["materialno"].ToDBString();
                        model.MaterialDesc = dr["materialdesc"].ToDBString();
                        model.PalletNo = dr["Palletno"].ToDBString();
                        model.AreaID = dr["Areaid"].ToInt32();
                        model.WareHouseID = dr["Warehouseid"].ToInt32();
                        model.HouseID = dr["Houseid"].ToInt32();
                        model.Barcode = dr["Barcode"].ToDBString();
                        model.SerialNo = dr["Serialno"].ToDBString();
                        model.StrongHoldCode = dr["Strongholdcode"].ToDBString();
                        model.StrongHoldName = dr["Strongholdname"].ToDBString();
                        model.CompanyCode = dr["Companycode"].ToDBString();
                        model.BatchNo = dr["Batchno"].ToDBString();
                        model.MaterialNo = dr["MaterialNo"].ToDBString();
                        model.MaterialDesc = dr["MaterialDesc"].ToDBString();
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据检验结果更新库存
        /// </summary>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public bool UpdateStockByQuality(List<T_QualityDetailInfo> modelList, ref string strError)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;
            int Status = 0;

            foreach (var item in modelList)
            {
                if (item.QuanQty > 0 && item.QuanQty == item.InSQty)
                {
                    Status = 3;
                }
                else if (item.UnQuanQty > 0 && item.UnQuanQty == item.InSQty)
                {
                    Status = 4;
                }

                if (Status > 0)
                {
                    if (!string.IsNullOrEmpty(item.AreaNo.Trim()))
                    {
                        strSql = "update t_stock a  set a.Status = '" + Status + "' where a.Batchno = '" + item.BatchNo + "' and a.Areaid = (select id from v_area where areano = '" + item.AreaNo + "' and warehouseno = '" + item.WarehouseNo + "' ) and a.Materialnoid = '" + item.MaterialNoID + "' and a.Strongholdcode = '" + item.StrongHoldCode + "' and Warehouseid = (select id from t_Warehouse a where a.Warehouseno =  '" + item.WarehouseNo + "')";
                        lstSql.Add(strSql);
                    }
                    else
                    {
                        strSql = "update t_stock a  set a.Status = '" + Status + "' where a.Batchno = '" + item.BatchNo + "'  and a.Materialnoid = '" + item.MaterialNoID + "' and a.Strongholdcode = '" + item.StrongHoldCode + "' and Warehouseid = (select id from t_Warehouse a where a.Warehouseno =  '" + item.WarehouseNo + "')";
                        lstSql.Add(strSql);
                    }


                    strSql = "update t_Quality a set a.Isupdatestock  = 2 where a.id = '" + item.ID + "'";
                    lstSql.Add(strSql);
                }
            }

            if (modelList[0].DesQty > 0 && modelList[0].DesQty == modelList[0].SampQty)
            {
                lstSql.AddRange(GetInsertStockByBatchNo(modelList.FindAll(t => t.AreaType == 4)));
            }

            return base.SaveModelListBySqlToDB(lstSql, ref strError);

        }


        public List<string> GetInsertStockByBatchNo(List<T_QualityDetailInfo> modelList)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            foreach (var model in modelList)
            {
                strSql = "insert into t_Stocktrans (Id, Barcode, Serialno, Materialno, Materialdesc, Warehouseid, " +
                        "Houseid, Areaid, Qty, Tmaterialno, Tmaterialdesc, Pickareano, Celareano, Status, Isdel, " +
                        "Creater, Createtime, Batchno, Sn, Returnsupcode, Returnreson, " +
                        "Returnsupname, Oldstockid, Taskdetailesid, Checkid, Transferdetailsid, Returntype, " +
                        "Returntypedesc, Unit, Salename, Unitname, Palletno, Receivestatus, Partno, Materialnoid," +
                         "Strongholdcode, Strongholdname, Companycode)" +
                        "select Seq_Stocktrans_Id.Nextval,barcode,serialno,Materialno, Materialdesc, Warehouseid, " +
                        "Houseid, Areaid, Qty, Tmaterialno, Tmaterialdesc, Pickareano, Celareano, Status, Isdel, " +
                        "Creater, Createtime, Batchno, Sn, Returnsupcode, Returnreson, " +
                        "Returnsupname, Oldstockid, Taskdetailesid, Checkid, Transferdetailsid, Returntype, " +
                        "Returntypedesc, Unit, Salename, Unitname, Palletno, Receivestatus, Partno, Materialnoid," +
                         "Strongholdcode, Strongholdname, Companycode from t_Stock a where a.Batchno = '" + model.BatchNo + "' " +
                        "and a.Areaid = (select id from v_area where areano = '" + model.AreaNo + "' and Warehouseno =  '" + model.WarehouseNo + "')" +
                        "and a.Materialnoid = '" + model.MaterialNoID + "' and a.Strongholdcode = '" + model.StrongHoldCode + "' and a.Warehouseid = (select id from t_Warehouse where warehouseno = '" + model.WarehouseNo + "')";

                lstSql.Add(strSql);


                strSql = "Delete t_Stock a where a.Batchno = '" + model.BatchNo + "' and a.Areaid = (select id from v_area where areano = '" + model.AreaNo + "' and Warehouseno =  '" + model.WarehouseNo + "')and a.Materialnoid = '" + model.MaterialNoID + "' and a.Strongholdcode = '" + model.StrongHoldCode + "' and a.Warehouseid = (select id from t_Warehouse where warehouseno = '" + model.WarehouseNo + "')";
                lstSql.Add(strSql);

            }


            return lstSql;



            //strSql = "select * from t_stock a where a.Materialnoid = '"+model.MaterialNoID+"' and a.Batchno = '"+model.BatchNo+"' and a.Strongholdcode = '"+model.StrongHoldCode+"' " +
            //        "and a.Areaid = (select a.ID from v_Area a where a.warehouseno = '"+model.WarehouseNo+"' and a.AREANO = '"+model.AreaNo+"' and a.AREATYPE = '4')";
            //List<T_StockInfo> modelList =  base.GetModelListBySql(strSql);

            //if (modelList == null)
            //{
            //    return null;
            //}

            //T_StockInfo item = modelList.Find(t => t.Qty == model.DesQty);
            //if (item != null)
            //{
            //    lstSql = GetStockSqlByDeQty(item);
            //}
            //else 
            //{
            //    foreach (var itemModel in modelList) 
            //    {
            //        if (model.DesQty < itemModel.Qty) 
            //        {

            //        }


            //    }

            //}




        }

        //private List<string> GetStockSqlByDeQty(T_StockInfo item)
        //{
        //    string strSql = string.Empty;
        //    List<string> lstSql = new List<string>();

        //    strSql = "insert into t_Stocktrans (Id, Barcode, Serialno, Materialno, Materialdesc, Warehouseid, " +
        //                "Houseid, Areaid, Qty, Tmaterialno, Tmaterialdesc, Pickareano, Celareano, Status, Isdel, " +
        //                "Creater, Createtime, Batchno, Sn, Returnsupcode, Returnreson, " +
        //                "Returnsupname, Oldstockid, Taskdetailesid, Checkid, Transferdetailsid, Returntype, " +
        //                "Returntypedesc, Unit, Salename, Unitname, Palletno, Receivestatus, Partno, Materialnoid," +
        //                 "Strongholdcode, Strongholdname, Companycode)" +
        //                "select Seq_Stocktrans_Id.Nextval,barcode,serialno,Materialno, Materialdesc, Warehouseid, " +
        //                "Houseid, Areaid, Qty, Tmaterialno, Tmaterialdesc, Pickareano, Celareano, Status, Isdel, " +
        //                "Creater, Createtime, Batchno, Sn, Returnsupcode, Returnreson, " +
        //                "Returnsupname, Oldstockid, Taskdetailesid, Checkid, Transferdetailsid, Returntype, " +
        //                "Returntypedesc, Unit, Salename, Unitname, Palletno, Receivestatus, Partno, Materialnoid," +
        //                 "Strongholdcode, Strongholdname, Companycode from t_Stock a where a.serialno = '" + item.SerialNo + "'";

        //    lstSql.Add(strSql);

        //    strSql = "Delete t_Stock where serialno = '" + item.SerialNo + "'";
        //    lstSql.Add(strSql);
        //    return lstSql;
        //}

        /// <summary>
        /// 拣货规则获取楼层
        /// </summary>
        /// <param name="MaterialNoID"></param>
        /// <param name="BatchNo"></param>
        /// <param name="PickRule"></param>
        /// <returns></returns>
        public T_StockInfo GetFloorForStock(int MaterialNoID, string BatchNo, int PickRule, string StrongholdCode, string WareHouseNo)
        {
            T_StockInfo model = new T_StockInfo();

            //string strSql = "select c.Floortype ,b.Areano,b.heightarea from t_stock a " +
            //                "left join t_Area b on a.Areaid = b.Id left join t_House c on b.Houseid = c.Id";
            //string Filter = " where Materialnoid = '" + MaterialNoID + "' and ( a.Status = 3 or a.status = 4 )  and a.Qty > 0 and isnull(a.Taskdetailesid, 0) = 0 and a.warehouseid = (select id from t_warehouse where warehouseno = '" + WareHouseNo + "') and a.Strongholdcode = '" + StrongholdCode + "' and Rownum=1 ";

            string strSql = "select Floortype ,Areano,heightarea from (select c.Floortype ,b.Areano,b.heightarea from t_stock a " +
                            "left join t_Area b on a.Areaid = b.Id left join t_House c on b.Houseid = c.Id ";

            string Filter = " where Materialnoid = '" + MaterialNoID + "' and ( a.Status = 3 or a.status = 4 ) and isnull(a.Isretention,2) = 2 and a.Qty > 0 and isnull(a.Taskdetailesid, 0) = 0 " +
                            "and a.warehouseid = (select id from t_warehouse where warehouseno = '" + WareHouseNo + "') and a.Strongholdcode = '" + StrongholdCode + "' ";

            strSql = strSql + Filter;


            if (!string.IsNullOrEmpty(BatchNo))
            {
                //strSql = strSql + Filter;
                strSql = strSql + " and a.Batchno = '" + BatchNo + "' ";
            }

            if (PickRule == 1) //批次先进先出
            {
                strSql = strSql + "  order by  substr(batchno,2) asc " + ")";
            }

            if (PickRule == 2)
            {
                strSql = strSql + " order by to_char(a.Edate,'yyyy-mm-dd') asc" + ")";
            }

            if (PickRule == 3)
            {
                strSql = strSql + " order by to_char(a.Productdate,'yyyy-mm-dd') asc" + ")";
            }

            strSql = strSql + " where rownum = 1";

            using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
            {
                if (dr.Read())
                {
                    model.FloorType = dr["FloorType"].ToInt32();
                    model.AreaNo = dr["AreaNo"].ToDBString();
                    model.HeightArea = dr["HeightArea"].ToInt32();
                }
            }

            return model;

        }

        /// <summary>
        /// 获取物料在库位的状态
        /// 用于上架扫描判断
        /// </summary>
        /// <param name="AreaNo"></param>
        /// <param name="MaterialNoID"></param>
        /// <param name="BatchNo"></param>
        /// <returns></returns>
        public int GetMaterialStatusByAreaID(int WareHouseID, string AreaNo, int MaterialNoID, string BatchNo)
        {
            string strSql = "select status from t_stock a where a.Areaid = ( select id from v_area where areano = '" + AreaNo + "' and warehouseid = '" + WareHouseID + "' ) " +
                            " and a.warehouseid = '" + WareHouseID + "' and a.Materialnoid = '" + MaterialNoID + "' and a.Batchno = '" + BatchNo + "' group by status";
            return base.GetScalarBySql(strSql).ToInt32();
        }

        public T_StockInfo GetMaterialStatusByAreaIDForMove(string AreaNo, int MaterialNoID, string BatchNo)
        {
            T_StockInfo model = new T_StockInfo();
            string strSql = "select status,(select areatype from t_area where areano = '" + AreaNo + "' ) as areatype from t_stock a where a.Areaid = ( select id from t_area where areano = '" + AreaNo + "' ) " +
                            " and a.Materialnoid = '" + MaterialNoID + "' and a.Batchno = '" + BatchNo + "' group by status";

            using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
            {
                if (dr.Read())
                {
                    model.Status = dr["status"].ToInt32();
                    model.AreaType = dr["areatype"].ToInt32();
                }
            }
            return model;
        }


        public decimal GetSumStockQtyByMaterialIDForOutDetail(int MaterialNoID, string IsSpcBatch, string BatchNo, string WareHouseNo, string StrongHoldCode)
        {
            //and isnull(Stocktype,1) = 1
            string strSql = "select isnull(sum(qty),0) from t_stock  where  (status =3 or status = 4) and warehouseid = (select id from t_warehouse where warehouseno = '" + WareHouseNo + "') and isnull(Taskdetailesid,0)=0    and materialnoid = '" + MaterialNoID + "' and Strongholdcode ='" + StrongHoldCode + "' and isnull(Isretention,2) = 2 ";

            if (IsSpcBatch == "Y")
            {
                strSql += " and batchno = '" + BatchNo + "'";
            }

            return base.GetScalarBySql(strSql).ToDecimal();
        }

        /// <summary>
        /// 获取物料转换库存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<T_StockInfo> GetStockByCheangeMaterial(T_OutStockDetailInfo model)
        {
            List<T_StockInfo> lstStock = new List<T_StockInfo>();

            string strSql = string.Empty;

            if (model.FromErpWarehouse.Contains("BH") && string.IsNullOrEmpty(model.FromErpAreaNo.Trim()))
            {
                strSql = "select a.id,a.Serialno,a.Batchno,a.Qty,a.Barcode,a.Companycode,a.Strongholdcode,a.Strongholdname,b.Materialno,b.Materialdesc,c.warehouseno,c.houseno,c.AREANO,a.Materialnoid,a.Materialchangeid from t_stock a left join t_Material b on  a.Materialnoid = b.Id " +
                           " left join v_area c on a.Areaid = c.Id where a.Materialnoid = '" + model.MaterialNoID + "' and isnull(a.Taskdetailesid,0)  = 0 and isnull(a.Checkid,0) = 0 " +
                           " and a.Strongholdcode = '" + model.StrongHoldCode + "' and a.Warehouseid = (select id from t_Warehouse where   warehouseno = '" + model.FromErpWarehouse + "') and a.Batchno = '" + model.FromBatchNo + "' ";

            }
            else
            {
                strSql = "select a.id,a.Serialno,a.Batchno,a.Qty,a.Barcode,a.Companycode,a.Strongholdcode,a.Strongholdname,b.Materialno,b.Materialdesc,c.warehouseno,c.houseno,c.AREANO,a.Materialnoid,a.Materialchangeid from t_stock a left join t_Material b on  a.Materialnoid = b.Id " +
                           " left join v_area c on a.Areaid = c.Id where a.Materialnoid = '" + model.MaterialNoID + "' and isnull(a.Taskdetailesid,0)  = 0 and isnull(a.Checkid,0) = 0 " +
                           " and a.Strongholdcode = '" + model.StrongHoldCode + "' and a.Areaid = (select id from v_Area where v_area.AREANO = '" + model.FromErpAreaNo + "' and v_area.warehouseno = '" + model.FromErpWarehouse + "' and a.Batchno = '" + model.FromBatchNo + "' )";

            }


            using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
            {
                while (dr.Read())
                {
                    T_StockInfo stockModel = new T_StockInfo();
                    stockModel.ID = dr["id"].ToInt32();
                    stockModel.SerialNo = dr["SerialNo"].ToDBString();
                    stockModel.SerialNo = dr["SerialNo"].ToDBString();
                    stockModel.Barcode = dr["Barcode"].ToDBString();
                    stockModel.CompanyCode = dr["CompanyCode"].ToDBString();
                    stockModel.StrongHoldCode = dr["StrongHoldCode"].ToDBString();
                    stockModel.StrongHoldName = dr["StrongHoldName"].ToDBString();
                    stockModel.MaterialNo = dr["MaterialNo"].ToDBString();
                    stockModel.MaterialNoID = dr["MaterialNoID"].ToInt32();
                    stockModel.MaterialDesc = dr["MaterialDesc"].ToDBString();
                    stockModel.WarehouseNo = dr["WarehouseNo"].ToDBString();
                    stockModel.HouseNo = dr["HouseNo"].ToDBString();
                    stockModel.AreaNo = dr["AreaNo"].ToDBString();
                    stockModel.MaterialChangeID = dr["MaterialChangeID"].ToInt32();
                    stockModel.BatchNo = dr["BatchNo"].ToDBString();
                    stockModel.Qty = dr["Qty"].ToDecimal();

                    lstStock.Add(stockModel);
                }
            }
            return lstStock;

        }

        /// <summary>
        /// 在库检扫描
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public T_StockInfo ScanQualityStockADF(string BarCode)
        {
            try
            {
                T_StockInfo stockModel = new T_StockInfo();

                string strSql = "select a.Companycode,a.Strongholdcode,a.Strongholdname,a.Edate,a.Warehouseid,a.Areaid,a.warehouseno,a.AREANO,a.MATERIALNOID,a.MATERIALNO,a.MATERIALDESC, " +
                                "a.BATCHNO,a.Status,a.Taskdetailesid,a.Checkid,(select sum(a.QTY) as qty  from t_stock a  " +
                                "where serialno = '" + BarCode + "' or palletno = '" + BarCode + "'" +
                                "group by a.AREAID,a.Materialnoid,a.Warehouseid,a.Batchno) as qty from v_stock a  " +
                                "where serialno = '" + BarCode + "' or palletno = '" + BarCode + "'";

                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
                {
                    if (dr.Read())
                    {
                        stockModel.CompanyCode = dr["CompanyCode"].ToDBString();
                        stockModel.StrongHoldCode = dr["StrongHoldCode"].ToDBString();
                        stockModel.StrongHoldName = dr["StrongHoldName"].ToDBString();
                        stockModel.WareHouseID = dr["Warehouseid"].ToInt32();
                        stockModel.AreaID = dr["areaid"].ToInt32();
                        stockModel.WarehouseNo = dr["WarehouseNo"].ToDBString();
                        stockModel.AreaNo = dr["AreaNo"].ToDBString();
                        stockModel.MaterialNoID = dr["MaterialNoID"].ToInt32();
                        stockModel.MaterialNo = dr["MaterialNo"].ToDBString();
                        stockModel.MaterialDesc = dr["MaterialDesc"].ToDBString();
                        stockModel.BatchNo = dr["BatchNo"].ToDBString();
                        stockModel.Status = dr["Status"].ToInt32();
                        stockModel.Qty = dr["Qty"].ToDecimal();
                        stockModel.TaskDetailesID = dr["Taskdetailesid"].ToInt32();
                        stockModel.CheckID = dr["CheckID"].ToInt32();
                        stockModel.EDate = dr["EDate"].ToDateTime();
                    }
                }

                return stockModel;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 批量验证下架数据提交是否存在检验状态为待检的
        /// </summary>
        /// <param name="BarCodeXml"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        public bool CheckBarCodeQualityStatus(string BarCodeXml, ref string strErrMsg)
        {
            try
            {
                int iResult = 0;

                OracleParameter[] cmdParms = new OracleParameter[] 
            {
                new OracleParameter("OutBarCodeXml", OracleDbType.NClob),
                new OracleParameter("bResult", OracleDbType.Int32,ParameterDirection.Output),
                new OracleParameter("strErrMsg", OracleDbType.NVarchar2,200,strErrMsg,ParameterDirection.Output)
            };

                cmdParms[0].Value = BarCodeXml;

                dbFactory.ExecuteNonQuery3(dbFactory.ConnectionStringLocalTransaction, CommandType.StoredProcedure, "P_Check_BarCodeQualityStatus", cmdParms);
                iResult = Convert.ToInt32(cmdParms[1].Value.ToString());
                strErrMsg = cmdParms[2].Value.ToString();

                return iResult == 1 ? true : false;
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据复核表体ID获取已经下架的库存
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<T_StockInfo> GetStockByOutStockReviewByID(string ID)
        {
            List<T_StockInfo> lstStock = new List<T_StockInfo>();

            string strSql = "select * from V_stock a where a.Taskdetailesid =( select a.Id from t_Taskdetails a where a.Outstockdetailid = '" + ID + "')";

            return base.GetModelListBySql(strSql);

        }

        public int GetStockStatusByQualityAreaNo(int MaterialNoID, string BatchNo, int WareHouseID)
        {
            string strSql = "select a.Status from t_stock a where a.Materialnoid = '" + MaterialNoID + "' and a.Batchno = '" + BatchNo + "' and a.Warehouseid = '" + WareHouseID + "' and " +
                            " a.Areaid = (select id from v_area b where b.WAREHOUSEID = '" + WareHouseID + "' and b.AREATYPE = 2)";
            return base.GetScalarBySql(strSql).ToInt32();
        }

        public bool SaveMoveStockToOutADF(UserModel user, List<T_StockInfo> modelList, ref string strError)
        {
            try
            {
                string strSql = string.Empty;
                List<string> lstSql = new List<string>();

                foreach (var itemStock in modelList)
                {
                    lstSql.Add(GetStockTransSql(user, itemStock));
                    lstSql.Add(GetTaskTransSql(user, itemStock));

                    lstSql.Add("delete t_Stock a where a.Serialno = '" + itemStock.SerialNo + "'");

                    strSql = "delete t_Palletdetail where serialno = '" + itemStock.SerialNo + "' and isnull(Pallettype,1) = 1";
                    lstSql.Add(strSql);

                    strSql = "delete t_Pallet where palletno = '" + itemStock.PalletNo + "' and (select count(1) from t_Palletdetail where palletno = '" + itemStock.PalletNo + "')=0";
                    lstSql.Add(strSql);
                }

                return base.UpdateModelListStatusBySql(lstSql, ref strError);
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }

        }

        private string GetStockTransSql(UserModel user, T_StockInfo model)
        {
            string strSql = "INSERT INTO t_Stocktrans (Id, Barcode, Serialno, Materialno, Materialdesc, Warehouseid, Houseid, Areaid, Qty,  Status, Isdel, Creater, Createtime, " +
                             "Batchno, Sn,  Oldstockid, Taskdetailesid, Checkid, Transferdetailsid,  Unit, Unitname, Palletno, Receivestatus,  Islimitstock,  " +
                             "Materialnoid, Strongholdcode, Strongholdname, Companycode, Edate, Supcode, Supname, Productdate, Supprdbatch, Supprddate, Isquality)" +
                            "SELECT Seq_Stocktrans_Id.Nextval,A.Barcode,A.Serialno,A.Materialno,A.Materialdesc,A.Warehouseid,A.Houseid,A.Areaid,A.Qty, " +
                            "A.Status,A.Isdel,'" + user.UserName + "',A.Createtime,A.Batchno,A.Sn,A.Oldstockid,A.Taskdetailesid,A.Checkid,A.Transferdetailsid,A.Unit,A.Unitname,A.Palletno,A.Receivestatus," +
                            "A.Islimitstock,A.Materialnoid,A.Strongholdcode,A.Strongholdname,A.Companycode,A.Edate,A.Supcode,A.Supname,A.Productdate,A.Supprdbatch," +
                            "A.Supprddate,A.Isquality FROM T_STOCK A WHERE A.Serialno = '" + model.SerialNo + "'";
            return strSql;
        }

        private string GetTaskTransSql(UserModel user, T_StockInfo model)
        {
            string strSql = "insert into t_tasktrans( Serialno,towarehouseID,TohouseID, ToareaID, Materialno, Materialdesc, Supcuscode, " +
            "Supcusname, Qty, Tasktype, Vouchertype, Creater, Createtime,TaskdetailsId, Unit, Unitname,partno,materialnoid,erpvoucherno,voucherno," +
            "Strongholdcode,Strongholdname,Companycode,Supprdbatch,Edate,taskno,status,batchno,barcode)" +
            " values ('" + model.SerialNo + "','" + model.WareHouseID + "','" + model.HouseID + "','" + model.AreaID + "'," +
            " '" + model.MaterialNo + "','" + model.MaterialDesc + "','','','" + model.Qty + "','12'," +
            " (select vouchertype from t_outstock where id = '') ,'" + user.UserName + "',getdate(),'" + model.ID + "', " +
            "'" + model.Unit + "','" + model.UnitName + "','','" + model.MaterialNoID + "',''," +
            "  '','" + model.StrongHoldCode + "','" + model.StrongHoldName + "','" + model.CompanyCode + "'," +
            "  '" + model.SupPrdBatch + "','" + model.EDate + "' ,'','" + model.Status + "','" + model.FromBatchNo + "','" + model.Barcode + "') ";

            return strSql;
        }

        /// <summary>
        /// 根据物料，批次，查询待检库位质量状态
        /// </summary>
        /// <param name="strMaterialNo"></param>
        /// <param name="strBatchNo"></param>
        /// <param name="strAreaNo"></param>
        /// <returns></returns>
        public string GetMaterialBatchStatus(string strMaterialNo, string strBatchNo, string strAreaID)
        {
            string strSql = "select status from t_stock where materialno = '" + strMaterialNo + "' and batchno  = '" + strBatchNo + "' and areaid = '" + strAreaID + "'";
            return GetScalarBySql(strSql).ToDBString();
        }

        /// <summary>
        /// 获取物料汇总库存
        /// </summary>
        /// <param name="strArrayGroupby">分组字段，比如批号，仓库，库区，货位等,可设多个，以“，”开始和分隔</param>
        /// <param name="strArrayMaterialNo">物料号，以“，”分隔</param>
        /// <param name="list"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public List<T_StockInfo> QueryStockSum(string strGroupBy, string strArrayMaterialNo, string where, ref string msg)
        {
            msg = "";
            List<T_StockInfo> lstStock = new List<T_StockInfo>();

            string strMaterialNo = "";
            string[] ArrayMaterialNo = strArrayMaterialNo.Split(',');
            foreach (var item in ArrayMaterialNo)
            {
                if (string.IsNullOrEmpty(strMaterialNo))
                {
                    strMaterialNo = "N'" + item + "'";
                }
                else
                    strMaterialNo += (",N'" + item + "'");
            }
            if (!strGroupBy.StartsWith(",")) strGroupBy = "," + strGroupBy;
            StringBuilder sb = new StringBuilder();
            sb.Append("select materialno,materialdesc,unit,sum(qty) qty,strongholdcode,strongholdname,companycode");
            sb.Append(strGroupBy);
            sb.Append(" from v_stock where 1=1 ");
            if (!strMaterialNo.Equals("N''"))
            {
                sb.Append(" AND  MATERIALNO IN (");
                sb.Append(strMaterialNo);
                sb.Append(")");
            }
            sb.Append(where);
            sb.Append(" group by materialno,materialdesc,unit,strongholdcode,strongholdname,companycode ");
            sb.Append(strGroupBy);
            //sb.Append(" having SUM(iquantity-ISNULL(fUnQualityQuantity,0))>0");
            string sql = sb.ToString();
            T_StockInfo stockModel;
            using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, sql))
            {
                while (dr.Read())
                {
                    stockModel = new T_StockInfo();
                    stockModel.MaterialNo = dr["MaterialNo"].ToDBString();
                    stockModel.MaterialDesc = dr["MaterialDesc"].ToDBString();
                    stockModel.Unit = dr["Unit"].ToDBString();
                    stockModel.StrongHoldCode = dr["StrongHoldCode"].ToDBString();
                    stockModel.StrongHoldName = dr["StrongHoldName"].ToDBString();
                    stockModel.CompanyCode = dr["CompanyCode"].ToDBString();
                    stockModel.Qty = dr["Qty"].ToDecimal();
                    string[] arrayColumn = strGroupBy.Split(',');

                    #region 通过反射给实体对象赋值

                    PropertyInfo[] piArray = stockModel.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    for (int i = 0; i < piArray.Count(); i++)
                    {
                        foreach (var item in arrayColumn)
                        {
                            if (string.IsNullOrEmpty(item))
                                continue;
                            if (piArray[i].Name.ToLower().Equals(item))
                            {
                                piArray[i].SetValue(stockModel, ObjectExtend.GetPropertyValue(piArray[i], dr[piArray[i].Name]), null);//为特定对象的指定属性赋值
                            }
                        }
                    }
                    #endregion

                    lstStock.Add(stockModel);
                }
            }
            return lstStock;
        }





        /// <summary>
        /// 非序列号扫描
        /// 库位+条码查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<T_StockInfo> GetStockByWHBarCode(T_StockInfo model)
        {
            List<T_StockInfo> modelList = new List<T_StockInfo>();
            string strFilter = string.Empty;
            string EAN = string.Empty;

            if (model.Barcode.Length == 14)
            {
                EAN = model.Barcode.Substring(1, model.Barcode.Length - 1);
            }
            else
            {
                EAN = model.Barcode;
            }
            

            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strFilter = "select a.* from t_stock a left join t_Taskdetails b on a.Taskdetailesid = b.Id where b.Erpvoucherno = '" + model.ErpVoucherNo.Trim() + "' and ean = '" + EAN + "' and ISAMOUNT = '2'";
                modelList = base.GetModelListBySql(strFilter);
            }
            else
            {
                
                //需要加个据点
                strFilter = "EAN='" + EAN + "' and Warehouseid ='" + model.WareHouseID + "' and areaid = '" + model.AreaID + "' ";
                modelList = base.GetModelListByFilter(string.Empty, strFilter, "*");
            }

            return modelList;
        }

        public List<T_StockInfo> GetStockByTaskDetailID(int ID) 
        {
            List<T_StockInfo> modelList = new List<T_StockInfo>();

            string strFilter = "select * from T_STOCK where ISNULL(TASKDETAILESID,0) ='"+ID+"'";
            modelList = base.GetModelListBySql(strFilter);

            return modelList;
        }


        #region 根据物料ID批量获取可用库存（存储过程）

        /// <summary>
        /// 根据物料批量获取可用库存
        /// 使用存储过程的方式
        /// </summary>
        /// <param name="MaterialXml"></param>
        /// <param name="modelList"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        public bool GetCanStockListByMaterialNoID(string MaterialXml, ref List<T_StockInfo> modelList, ref string strErrMsg)
        {
            try
            {
                int iResult = 0;
                DataSet ds;

                OracleParameter[] cmdParms = new OracleParameter[] 
            {
                new OracleParameter("strMaterialXml", OracleDbType.NClob),    
                new OracleParameter("StockCur", OracleDbType.RefCursor,ParameterDirection.Output),
                new OracleParameter("bResult", OracleDbType.Int32,ParameterDirection.Output),
                new OracleParameter("ErrString", OracleDbType.NVarchar2,200,strError,ParameterDirection.Output)
            };

                cmdParms[0].Value = MaterialXml;

                cmdParms[1].Value = ParameterDirection.Output;
                cmdParms[2].Value = ParameterDirection.Output;
                cmdParms[3].Value = ParameterDirection.Output;

                ds = dbFactory.ExecuteDataSetForCursor(ref iResult, ref strError, dbFactory.ConnectionStringLocalTransaction, CommandType.StoredProcedure, "p_getstockbymateril", cmdParms);

                if (iResult == 1)
                {
                    modelList = TOOL.DataTableToList.DataSetToList<T_StockInfo>(ds.Tables[0]);
                    modelList = modelList.Where(t => t.TaskDetailesID == 0 && t.TransferDetailsID == 0 && t.CheckID == 0 && t.Qty > 0).ToList();
                    strError = string.Empty;
                }

                return iResult == 1 ? true : false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取可用库存
        /// 生成拣货单调用
        /// </summary>
        /// <param name="baseModelList"></param>
        /// <returns></returns>
        public List<T_StockInfo> GetCanStockListByMaterialNoIDToSql(List<T_OutStockCreateInfo> baseModelList)
        {
            try
            {
                string strSql = "";
                string materialID = string.Empty;
                List<T_StockInfo> stockList = new List<T_StockInfo>();

                foreach (var item in baseModelList)
                {
                    materialID += item.MaterialNoID + ",";
                }

                materialID = materialID.TrimEnd(',');

                strSql = "select * from v_stock where materialnoid in (" + materialID + ")";

                stockList = base.GetModelListBySql(strSql);

                if (stockList != null)
                {
                    stockList = stockList.Where(t => t.TaskDetailesID == 0 && t.TransferDetailsID == 0 && t.CheckID == 0 && t.Qty > 0 ).ToList();
                }

                return stockList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region 根据物料ID批量获取可用库存（SQL），拣货单用
        public List<T_StockInfo> GetCanStockListByMaterialNoIDToSql(List<T_OutStockTaskDetailsInfo> baseModelList)
        {
            try
            {
                string strSql = "";
                string materialID = string.Empty;
                List<T_StockInfo> stockList = new List<T_StockInfo>();

                foreach (var item in baseModelList)
                {
                    materialID += item.MaterialNoID + ",";
                }

                materialID = materialID.TrimEnd(',');

                //strSql = "select * from v_stock where materialnoid in (" + materialID + ") union all "+
                //        " select * from (select a.id,b.barcode,b.serialno,b.materialno,b.materialdesc,a.WAREHOUSEID,a.HOUSEID,a.AREAID,"+
                //        " b.qty,a.TMATERIALNO,a.TMATERIALDESC,a.PICKAREANO,a.CELAREANO,a.STATUS,a.ISDEL,a.CREATER,a.CREATETIME,"+
                //        " a.MODIFYER,a.MODIFYTIME,b.batchno,a.SN,a.RETURNSUPCODE,a.RETURNRESON,a.RETURNSUPNAME,a.OLDSTOCKID,"+
                //        " a.TASKDETAILESID,a.CHECKID,a.TRANSFERDETAILSID,a.RETURNTYPE,a.RETURNTYPEDESC,b.unit,a.SALENAME,a.UNITNAME,a.PALLETNO,a.RECEIVESTATUS,"+
                //        " a.SALECODE,a.ISLIMITSTOCK,a.PARTNO,b.materialnoid,b.strongholdcode,b.strongholdname,b.companycode,b.edate,"+
                //        " a.SUPCODE,a.SUPNAME,a.PRODUCTDATE,a.SUPPRDBATCH,a.SUPPRDDATE,a.ISQUALITY,a.WAREHOUSENO,a.HOUSENO,a.AREANO,"+
                //        " a.AREATYPE,a.MATERIALCHANGEID,a.Expr1,a.Isretention,a.SPEC,b.ean,a.HOUSEPROP,a.ISAMOUNT,a.BARCODETYPE from v_stock a left join T_OUTBARCODE b on a.BARCODE = b.fserialno where a.BARCODETYPE = 5 "+
                //        " ) as c";

                strSql = "select * from v_stock where materialnoid in (" + materialID + ") ";

                stockList = base.GetModelListBySql(strSql);

                if (stockList != null)
                {
                    stockList = stockList.Where(t => t.TaskDetailesID == 0 && t.TransferDetailsID == 0 && t.CheckID == 0 && t.Qty > 0).ToList();
                }

                return stockList;
                //string strSql = "";
                //string materialID = string.Empty;
                //List<T_StockInfo> stockList = new List<T_StockInfo>();

                //foreach (var item in baseModelList)
                //{
                //    materialID += item.MaterialNoID + ",";
                //}

                //materialID = materialID.TrimEnd(',');

                //strSql = "select * from t_stock where materialnoid in (" + materialID + ")";

                //stockList =  base.GetModelListBySql(strSql);

                //if (stockList != null) 
                //{
                //    stockList = stockList.Where(t => t.TaskDetailesID == 0 && t.TransferDetailsID == 0 && t.CheckID == 0 && t.Qty > 0).ToList();
                //}

                //return stockList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 玛莎盘点
        //ymh盘点 根据EAN，areaid获取batch
        public List<string> CheckGetBatchnoAndMaterialno(string EAN, string areaid)
        {
            List<string> batch = new List<string>();
            string Sql = "select batchno,materialno from t_stock where EAN='" + EAN + "' and areaid='" + areaid + "'";
            using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, Sql))
            {
                while (dr.Read())
                {
                    batch.Add(dr["batchno"].ToDBString() + "," + dr["materialno"].ToDBString());
                }
            }
            return batch;
        }

        public string CheckSerialno(string EAN, string areaid, string batchno, string materialno)
        {
            string Sql = "select serialno from t_stock where EAN='" + EAN + "' and areaid='" + areaid + "' and materialno='" + materialno + "' and batchno='" + batchno + "'";
            using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, Sql))
            {
                while (dr.Read())
                {
                    return dr["serialno"].ToDBString();
                }
            }
            return "";

        }
        #endregion

        /// <summary>
        /// 复核根据订单号获取拣货库存
        /// </summary>
        public List<T_StockInfo> GetStockPickByErpNo(string strErpVoucherNo)
        {
            try
            {
                List<T_StockInfo> modelList = new List<T_StockInfo>();
                string strFilter = "select a.* from v_stock a left join t_Taskdetails b on a.Taskdetailesid = b.Id where b.Erpvoucherno = '" + strErpVoucherNo + "'";
                modelList = base.GetModelListBySql(strFilter);

                return modelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
