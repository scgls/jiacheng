using BILBasic.DBA;
using BILBasic.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;
using Oracle.ManagedDataAccess.Client;
using BILBasic.JSONUtil;
using BILWeb.Stock;
using BILBasic.XMLUtil;
using System.Data;
using BILWeb.OutStock;

namespace BILWeb.OutStockTask
{
    public partial class T_OutTaskDetails_DB : BILBasic.Basing.Factory.Base_DB<T_OutStockTaskDetailsInfo>
    {

        /// <summary>
        /// 添加t_taskdetails
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_OutStockTaskDetailsInfo t_taskdetails)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();
        }

        protected override List<string> GetSaveSql(UserModel user,ref  T_OutStockTaskDetailsInfo model)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            if (model.ID <= 0)
            {
                return null;
            }
            else
            {
                strSql = "update t_Taskdetails  set  Taskqty = '" + model.TaskQty + "' ,  Remainqty = '" + model.TaskQty + "' - isnull( Unshelveqty,0), Outstockqty = '"+model.TaskQty+"', " +
                        "  Modifyer = '"+user.UserNo+"' , Modifytime = getdate() where id = '"+model.ID+"'";

                lstSql.Add(strSql);
            }
            return lstSql;
        }

        protected override List<string> GetSaveModelListSql(UserModel user, List<T_OutStockTaskDetailsInfo> modelList)
        {
            string strSql1 = string.Empty;
            string strSql2 = string.Empty;
            string strSql3 = string.Empty;
            string strSql4 = string.Empty;
            string strSql5 = string.Empty;
            int Count = 0;
            List<string> lstSql = new List<string>();

            foreach (var item in modelList) 
            {
                strSql1 = string.Format("update t_taskdetails  set  remainqty = (case when isnull( remainqty,0) >= ('{0}') then (isnull( remainqty,0) - '{1}')" +
                                "else 0 end ), unshelveqty = isnull( unshelveqty,0) + '{2}', operatoruserno = '{3}', operatordatetime = getdate() where id  = '{4}'",
                                item.ScanQty,item.ScanQty,item.ScanQty,user.UserNo,item.ID);
                lstSql.Add(strSql1);

                strSql1 = string.Format("update t_Outstockdetail set pickqty = isnull(pickqty,0) + '{0}' where erpvoucherno = '{1}' and materialnoid = '{2}' and rowno = '{3}' and VOUCHERNO='{4}'",
                                item.ScanQty, item.ErpVoucherNo, item.MaterialNoID,item.RowNo,item.VoucherNo);
                lstSql.Add(strSql1);

                strSql2 = string.Format("update t_taskdetails  set  Linestatus =(case when isnull( remainqty,0)< isnull( taskqty,0) and isnull( remainqty,0)<>0 then 2 when isnull( remainqty,0)  = 0  then 3 end )" +
                        "where id ={0}",item.ID);
                lstSql.Add(strSql2);

                strSql3 = "update t_task  set  Status = 2 where id = '" + item.HeaderID + "'";
                lstSql.Add(strSql3);

                strSql4 = string.Format(" update t_task set status = 3 where id in(select HeaderID from t_taskdetails group by HeaderID having(max(isnull(linestatus,1)) = 3 and min(isnull(linestatus,1))=3) and HeaderID = '{0}')" +
                                        "and id = '{1}'", item.HeaderID, item.HeaderID);

                lstSql.Add(strSql4);

                if (item.lstStockInfo != null && item.lstStockInfo.Count > 0)
                {
                    foreach (var itemStock in item.lstStockInfo)
                    {
                        lstSql.Add(GetStockTransSql(user, itemStock));

                        //如果是补货单，需要转移到仓库对应的补货库位
                        if (item.VoucherType == 3)
                        {
                            strSql1 = "update t_stock  set  Areaid = (select id from v_area b where b.warehouseno = '" + item.FromErpWarehouse + "' and b.AREATYPE = '5')," +
                                   "  Houseid = (select c.HOUSEID from v_area c where c.warehouseno = '" + item.FromErpWarehouse + "' and c.AREATYPE = '5'), " +
                                   "   Warehouseid = (select id from t_Warehouse d where d.Warehouseno = '" + item.FromErpWarehouse + "'), TASKDETAILESID=0  where  Serialno = '" + itemStock.SerialNo + "'";
                            lstSql.Add(strSql1);
                        }
                        else
                        {
                            if (itemStock.IsAmount == 1)//不拆零
                            {
                                //扫描到的是外箱，需要拆托盘
                                if (itemStock.IsPalletOrBox == 1)
                                {
                                    //拣货到待发 lstSql.Add("update t_stock  set Taskdetailesid = '" + item.ID + "', Areaid = '" + user.PickAreaID + "', Houseid = '" + user.PickHouseID + "', Warehouseid = '" + user.PickWareHouseID + "',palletno = '' where Serialno = '" + itemStock.SerialNo + "'");
                                    lstSql.Add("delete from t_stock  where Serialno = '" + itemStock.SerialNo + "'");//拣货直接出库

                                    if (!string.IsNullOrEmpty(itemStock.PalletNo))
                                    {
                                        strSql1 = "delete t_Palletdetail where BARCODE = '" + itemStock.Barcode + "'";
                                        lstSql.Add(strSql1);

                                        strSql1 = "delete t_Pallet where palletno = '" + itemStock.PalletNo + "' and (select count(1) from t_Palletdetail where palletno = '" + itemStock.PalletNo + "')=0";
                                        lstSql.Add(strSql1);
                                    }
                                }
                                else //扫描到的是托盘，不需要拆托
                                {
                                    //拣货到待发 lstSql.Add("update t_stock  set Taskdetailesid = '" + item.ID + "', Areaid = '" + user.PickAreaID + "', Houseid = '" + user.PickHouseID + "', Warehouseid = '" + user.PickWareHouseID + "' where Serialno = '" + itemStock.SerialNo + "'");
                                    lstSql.Add("delete from t_stock  where Serialno = '" + itemStock.SerialNo + "'");//拣货直接出库
                                }
                            }
                            else if (itemStock.IsAmount == 2)//拆零
                            {
                                strSql1 = "delete from t_stock where serialno = '" + itemStock.SerialNo + "'";
                                lstSql.Add(strSql1);
                                //lstSql.Add("update t_stock a set Taskdetailesid = '" + item.ID + "', Areaid = '" + user.PickAreaID + "', Houseid = '" + user.PickHouseID + "', Warehouseid = '" + user.PickWareHouseID + "',houseprop='" + item.HouseProp + "' where Serialno = '" + itemStock.SerialNo + "'");

                                //strSql1 = "select count(1) from t_stock a where  Taskdetailesid='" + item.ID + "' and " +
                                //       "  Materialnoid='" + itemStock.MaterialNoID + "' and  Ean='" + itemStock.EAN + "' and  batchno = '" + itemStock.BatchNo + "' and  Warehouseid='" + user.PickWareHouseID + "' " +
                                //       " and  Houseid='" + user.PickHouseID + "' and  Areaid='" + user.PickAreaID + "' " +
                                //       " and  Strongholdcode='" + itemStock.StrongHoldCode + "' and     CONVERT(varchar(100),edate, 111) ='" + itemStock.StrEDate + "' and isnull(IsAmount,0)=2";
                                //Count = base.GetScalarBySql(strSql1).ToInt32();

                                //if (Count == 0)
                                //{
                                //    lstSql.Add("update t_stock  set Taskdetailesid = '" + item.ID + "', Areaid = '" + user.PickAreaID + "', Houseid = '" + user.PickHouseID + "', Warehouseid = '" + user.PickWareHouseID + "',IsAmount=2 where Serialno = '" + itemStock.SerialNo + "'");

                                //}
                                //else
                                //{
                                //    strSql1 = "update t_stock  set qty = qty + '" + itemStock.Qty + "'  where  Taskdetailesid='" + item.ID + "' and " +
                                //           "  Materialnoid='" + item.MaterialNoID + "' and  Ean='" + itemStock.EAN + "' and  batchno = '" + itemStock.BatchNo + "' and  Warehouseid='" + user.PickWareHouseID + "' " +
                                //           " and  Houseid='" + user.PickHouseID + "' and  Areaid='" + user.PickAreaID + "'" +
                                //           " and  Strongholdcode='" + itemStock.StrongHoldCode + "'  and  CONVERT(varchar(100),edate, 111) = '" + itemStock.StrEDate + "'and isnull(IsAmount,0)=2";
                                //    lstSql.Add(strSql1);

                                //    strSql1 = "delete t_stock where serialno = '" + itemStock.SerialNo + "'  and Areaid !='" + user.PickAreaID + "' ";

                                //    lstSql.Add(strSql1);
                                //} 
                            }
                        }

                        //lstSql.Add(GetTaskTransSql(user, itemStock, item));
                        lstSql.AddRange(GetTaskTransSqlList(user, itemStock, item));

                        //if (!string.IsNullOrEmpty(itemStock.PalletNo))
                        //{
                        //    //strSql = _db.GetAmountQtyInsertPalletSql(model, userModel, NewSerialNo, model.PalletNo);
                        //    //lstSql.Add(strSql);

                        //    strSql1 = "update t_Palletdetail  set Qty = qty - '" + itemStock.AmountQty + "' where barcode = '" + itemStock.Barcode + "'";
                        //    lstSql.Add(strSql1);

                        //    strSql1 = "delete t_Palletdetail  where barcode = '" + itemStock.Barcode + "' and Qty = 0";
                        //    lstSql.Add(strSql1);
                        //}

                        //内盒 如果内外不关联的情况 内核条码需记录流向
                        //if (itemStock.lstJBarCode != null && itemStock.lstJBarCode.Count > 0)
                        //{
                        //    foreach (var itemStockJ in itemStock.lstJBarCode)
                        //    {
                        //        lstSql.Add("update T_OUTBARCODE set ToProjectNo='" + itemStockJ.ProjectNo + "',ToTracNo='" + itemStockJ.TracNo + "' ,ToErpVoucherNo='" + item.ErpVoucherNo + "' ,ToTime='" + DateTime.Now.ToString() + "' where serialno='" + itemStockJ.SerialNo + "'");
                        //    }
                        //}

                    }

                }
                
            }

            return lstSql;
        }

        private string GetStockTransSql(UserModel user,T_StockInfo model) 
        {            
            string strSql = "INSERT INTO t_Stocktrans ( Barcode, Serialno, Materialno, Materialdesc, Warehouseid, Houseid, Areaid, Qty,  Status, Isdel, Creater, Createtime, " +
                             "Batchno, Sn,  Oldstockid, Taskdetailesid, Checkid, Transferdetailsid,  Unit, Unitname, Palletno, Receivestatus,  Islimitstock,  " +
                             "Materialnoid, Strongholdcode, Strongholdname, Companycode, Supcode, Supname, Productdate, Supprdbatch, Supprddate, Isquality,ean)" +
                            "SELECT  Barcode, Serialno, Materialno, Materialdesc, Warehouseid, Houseid, Areaid, Qty, " +
                            " Status, Isdel,'" + user.UserName + "', Createtime, Batchno, Sn, Oldstockid, Taskdetailesid, Checkid, Transferdetailsid, Unit, Unitname, Palletno, Receivestatus," +
                            " Islimitstock, Materialnoid, Strongholdcode, Strongholdname, Companycode, Supcode, Supname, Productdate, Supprdbatch," +
                            " Supprddate, Isquality, ean FROM T_STOCK A WHERE  Serialno = '"+model.SerialNo+"'";
            return strSql;
        }

        private string GetTaskTransSql(UserModel user, T_StockInfo model,T_OutStockTaskDetailsInfo detailModel) 
       {
           int id = base.GetTableIDBySqlServer("T_TASKTRANSDETAIL");
            string strSql = "insert into t_tasktrans( Serialno,towarehouseID,TohouseID, ToareaID, Materialno, Materialdesc, Supcuscode, "+
            "Supcusname, Qty, Tasktype, Vouchertype, Creater, Createtime,TaskdetailsId, Unit, Unitname,partno,materialnoid,erpvoucherno,voucherno,"+
            "Strongholdcode,Strongholdname,Companycode,Supprdbatch,Edate,taskno,batchno,Fromareaid,Fromwarehouseid,Fromhouseid,barcode,status,materialdoc,houseprop,ean)" +
            " values ('" + model.SerialNo + "',(select id from t_Warehouse a  where  Warehouseno = '" + model.ToErpWarehouse + "'),(select  HOUSEID from v_Area a where  warehouseno = '" + model.ToErpWarehouse + "' and  AREANO = '" + model.ToErpAreaNo + "'),(select  ID from v_Area a where  warehouseno = '" + model.ToErpWarehouse + "' and  AREANO = '" + model.ToErpAreaNo + "')," +
            " '" + model.MaterialNo + "','" + model.MaterialDesc + "','" + detailModel.SupCusCode + "','" + detailModel.SupCusName + "','" + model.Qty+ "','2'," +
            " (select vouchertype from t_task where id = '" + detailModel.HeaderID + "') ,'" + user.UserName + "',getdate(),'" + model.ID + "', " +
            "'" + detailModel.Unit + "','" + detailModel.UnitName + "','" + detailModel.PartNo + "','" + detailModel.MaterialNoID + "','" + detailModel.ErpVoucherNo + "'," +
            "  '" + detailModel.VoucherNo + "','" + detailModel.StrongHoldCode + "','" + detailModel.StrongHoldName + "','" + detailModel.CompanyCode + "',"+
            "  '" + model.SupPrdBatch + "','" + model.EDate + "' ,'" + detailModel.TaskNo + "'," +
            " '"+model.BatchNo+"', '"+model.AreaID+"','"+model.WareHouseID+"','"+model.HouseID+"' ,'"+model.Barcode+"','"+model.Status+"','"+detailModel.MaterialDoc+"','"+detailModel.HouseProp+"','"+model.EAN+"') ";

            return strSql;
        }

        private List<string> GetTaskTransSqlList(UserModel user, T_StockInfo model, T_OutStockTaskDetailsInfo detailModel)
        {
            int id = base.GetTableIDBySqlServerTaskTrans("t_tasktrans");
            List<string> lstSql = new List<string>();
            string strSql = "SET IDENTITY_INSERT t_tasktrans on ;insert into t_tasktrans(id, Serialno, Materialno, Materialdesc, Supcuscode, " +
            "Supcusname, Qty, Tasktype, Vouchertype, Creater, Createtime,TaskdetailsId, Unit, Unitname,partno,materialnoid,erpvoucherno,voucherno," +
            "Strongholdcode,Strongholdname,Companycode,Supprdbatch,taskno,batchno,barcode,status,materialdoc,houseprop,ean,FromWarehouseNo,FromWarehouseName,FromHouseNo,FromAreaNo,ToWarehouseNo,ToWarehouseName,ToHouseNo,ToAreaNo,PalletNo,IsPalletOrBox)" +
            " values ('"+id+"' , '" + model.SerialNo + "'," +
            " '" + model.MaterialNo + "','" + model.MaterialDesc + "','" + detailModel.SupCusCode + "','" + detailModel.SupCusName + "','" + model.Qty + "','2'," +
            " (select vouchertype from t_task where id = '" + detailModel.HeaderID + "') ,'" + user.UserName + "',getdate(),'" + model.ID + "', " +
            "'" + detailModel.Unit + "','" + detailModel.UnitName + "','" + detailModel.PartNo + "','" + detailModel.MaterialNoID + "','" + detailModel.ErpVoucherNo + "'," +
            "  '" + detailModel.VoucherNo + "','" + detailModel.StrongHoldCode + "','" + detailModel.StrongHoldName + "','" + detailModel.CompanyCode + "'," +
            "  '" + model.SupPrdBatch + "','" + detailModel.TaskNo + "'," +
            " '" + model.BatchNo + "' ,'" + model.Barcode + "','" + model.Status + "','" + detailModel.MaterialDoc + "','" + detailModel.HouseProp + "','" + model.EAN + "',"+
            "  (select WAREHOUSENO from T_WAREHOUSE where id ='"+model.WareHouseID+"'),"+
            " (select WAREHOUSENAME from T_WAREHOUSE where id ='"+model.WareHouseID+"'), "+
            " (select HOUSENO from T_HOUSE where id='"+model.HouseID+"'),"+
            " (select AREANO from T_AREA where id ='"+model.AreaID+"'),"+
            " '',''," +
            " ''," +
            " '','" + model.PalletNo+"','"+model.IsPalletOrBox+"' ) SET IDENTITY_INSERT t_tasktrans off ";//,(select  ID from v_Area a where  warehouseno = '" + model.ToErpWarehouse + "' and  AREANO = '" + model.ToErpAreaNo + "'),'" + model.AreaID + "','" + model.WareHouseID + "','" + model.HouseID + "'

            lstSql.Add(strSql);


//            string strSql = "SET IDENTITY_INSERT t_tasktrans on ;insert into t_tasktrans(id, Serialno, Materialno, Materialdesc, Supcuscode, " +
//"Supcusname, Qty, Tasktype, Vouchertype, Creater, Createtime,TaskdetailsId, Unit, Unitname,partno,materialnoid,erpvoucherno,voucherno," +
//"Strongholdcode,Strongholdname,Companycode,Supprdbatch,Edate,taskno,batchno,barcode,status,materialdoc,houseprop,ean,FromWarehouseNo,FromWarehouseName,FromHouseNo,FromAreaNo,ToWarehouseNo,ToWarehouseName,ToHouseNo,ToAreaNo,PalletNo,IsPalletOrBox)" +
//" values ('" + id + "' , '" + model.SerialNo + "'," +
//" '" + model.MaterialNo + "','" + model.MaterialDesc + "','" + detailModel.SupCusCode + "','" + detailModel.SupCusName + "','" + model.Qty + "','2'," +
//" (select vouchertype from t_task where id = '" + detailModel.HeaderID + "') ,'" + user.UserName + "',getdate(),'" + model.ID + "', " +
//"'" + detailModel.Unit + "','" + detailModel.UnitName + "','" + detailModel.PartNo + "','" + detailModel.MaterialNoID + "','" + detailModel.ErpVoucherNo + "'," +
//"  '" + detailModel.VoucherNo + "','" + detailModel.StrongHoldCode + "','" + detailModel.StrongHoldName + "','" + detailModel.CompanyCode + "'," +
//"  '" + model.SupPrdBatch + "','" + model.StrEDate + "' ,'" + detailModel.TaskNo + "'," +
//" '" + model.BatchNo + "' ,'" + model.Barcode + "','" + model.Status + "','" + detailModel.MaterialDoc + "','" + detailModel.HouseProp + "','" + model.EAN + "'," +
//"  (select WAREHOUSENO from T_WAREHOUSE where id ='" + model.WareHouseID + "')," +
//" (select WAREHOUSENAME from T_WAREHOUSE where id ='" + model.WareHouseID + "'), " +
//" (select HOUSENO from T_HOUSE where id='" + model.HouseID + "')," +
//" (select AREANO from T_AREA where id ='" + model.AreaID + "')," +
//" (select WAREHOUSENO from T_WAREHOUSE where id ='" + user.PickWareHouseID + "'),(select WAREHOUSENAME from T_WAREHOUSE where warehouseno ='" + user.PickWareHouseID + "')," +
//" (select HOUSENO from T_HOUSE where id='" + user.PickHouseID + "')," +
//" (select AREANO from T_AREA where id ='" + user.PickAreaID + "'),'" + model.PalletNo + "','" + model.IsPalletOrBox + "' ) SET IDENTITY_INSERT t_tasktrans off ";//,(select  ID from v_Area a where  warehouseno = '" + model.ToErpWarehouse + "' and  AREANO = '" + model.ToErpAreaNo + "'),'" + model.AreaID + "','" + model.WareHouseID + "','" + model.HouseID + "'



            //if (model.lstJBarCode != null && model.lstJBarCode.Count > 0) 
            //{
            //    foreach (var item in model.lstJBarCode)
            //    {
            //        strSql = "insert into T_TASKTRANSDETAIL( Serialno,towarehouseID,TohouseID, ToareaID, Materialno, Materialdesc, Supcuscode, " +
            //        "Supcusname, Qty, Tasktype, Vouchertype, Creater, Createtime,TaskdetailsId, Unit, Unitname,partno,materialnoid,erpvoucherno,voucherno," +
            //        "Strongholdcode,Strongholdname,Companycode,Supprdbatch,Edate,taskno,batchno,Fromareaid,Fromwarehouseid,Fromhouseid,barcode,status,materialdoc,houseprop,ean,headerid)" +
            //        " values ( '" + item.SerialNo + "',(select id from t_Warehouse a  where  Warehouseno = '" + model.ToErpWarehouse + "'),(select  HOUSEID from v_Area a where  warehouseno = '" + model.ToErpWarehouse + "' and  AREANO = '" + model.ToErpAreaNo + "'),(select  ID from v_Area a where  warehouseno = '" + model.ToErpWarehouse + "' and  AREANO = '" + model.ToErpAreaNo + "')," +
            //        " '" + model.MaterialNo + "','" + model.MaterialDesc + "','" + detailModel.SupCusCode + "','" + detailModel.SupCusName + "','" + item.Qty + "','2'," +
            //        " (select vouchertype from t_task where id = '" + detailModel.HeaderID + "') ,'" + user.UserName + "',getdate(),'" + model.ID + "', " +
            //        "'" + detailModel.Unit + "','" + detailModel.UnitName + "','" + detailModel.PartNo + "','" + detailModel.MaterialNoID + "','" + detailModel.ErpVoucherNo + "'," +
            //        "  '" + detailModel.VoucherNo + "','" + detailModel.StrongHoldCode + "','" + detailModel.StrongHoldName + "','" + detailModel.CompanyCode + "'," +
            //        "  '" + model.SupPrdBatch + "','" + model.StrEDate + "' ,'" + detailModel.TaskNo + "'," +
            //        " '" + model.BatchNo + "', '" + model.AreaID + "','" + model.WareHouseID + "','" + model.HouseID + "' ,'" + item.Barcode + "','" + model.Status + "','" + detailModel.MaterialDoc + "','" + detailModel.HouseProp + "','" + item.EAN + "','" + id + "')  ";
            //        lstSql.Add(strSql);
            //    }
            //}


            return lstSql;
        }

        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_OutStockTaskDetailsInfo ToModel(IDataReader reader)
        {
            T_OutStockTaskDetailsInfo t_taskdetails = new T_OutStockTaskDetailsInfo();

            t_taskdetails.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            //t_taskdetails.ToAreaNo = (string)dbFactory.ToModelValue(reader, "TOAREANO");
            t_taskdetails.MaterialNo = (string)dbFactory.ToModelValue(reader, "MATERIALNO");
            t_taskdetails.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MATERIALDESC");
            t_taskdetails.TaskQty = dbFactory.ToModelValue(reader, "TASKQTY").ToDecimal();
            t_taskdetails.QualityQty = dbFactory.ToModelValue(reader, "QUALITYQTY").ToDecimal();
            t_taskdetails.RemainQty = dbFactory.ToModelValue(reader, "REMAINQTY").ToDecimal();
            t_taskdetails.ShelveQty = dbFactory.ToModelValue(reader, "SHELVEQTY").ToDecimal();
            t_taskdetails.LineStatus = dbFactory.ToModelValue(reader, "LINESTATUS").ToInt32();
            t_taskdetails.IsQualitycomp = dbFactory.ToModelValue(reader, "ISQUALITYCOMP").ToDecimal();
            t_taskdetails.KeeperUserNo = (string)dbFactory.ToModelValue(reader, "KEEPERUSERNO");
            t_taskdetails.OperatorUserNo = (string)dbFactory.ToModelValue(reader, "OPERATORUSERNO");
            t_taskdetails.CompleteDateTime = (DateTime?)dbFactory.ToModelValue(reader, "COMPLETEDATETIME");
            t_taskdetails.HeaderID = dbFactory.ToModelValue(reader, "HeaderID").ToInt32();
            t_taskdetails.TMaterialNo = (string)dbFactory.ToModelValue(reader, "TMATERIALNO");
            t_taskdetails.TMaterialDesc = (string)dbFactory.ToModelValue(reader, "TMATERIALDESC");
            t_taskdetails.OperatorDateTime = (DateTime?)dbFactory.ToModelValue(reader, "OPERATORDATETIME");
            t_taskdetails.ReviewQty = dbFactory.ToModelValue(reader, "REVIEWQTY").ToDecimal();
            t_taskdetails.PackCount = dbFactory.ToModelValue(reader, "PACKCOUNT").ToDecimal();
            t_taskdetails.ShelvePackCount = dbFactory.ToModelValue(reader, "SHELVEPACKCOUNT").ToDecimal();
            t_taskdetails.VoucherNo = (string)dbFactory.ToModelValue(reader, "VOUCHERNO");
            t_taskdetails.RowNo = string.IsNullOrEmpty(dbFactory.ToModelValue(reader, "ROWNO").ToDBString())==true ? "0" : dbFactory.ToModelValue(reader, "ROWNO").ToDBString();
            t_taskdetails.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_taskdetails.TrackNo = (string)dbFactory.ToModelValue(reader, "TRACKNO");
            t_taskdetails.Unit = (string)dbFactory.ToModelValue(reader, "UNIT");
            t_taskdetails.UnQualityQty = dbFactory.ToModelValue(reader, "UNQUALITYQTY").ToDecimal();
            t_taskdetails.PostQty = dbFactory.ToModelValue(reader, "POSTQTY").ToDecimal();
            t_taskdetails.PostStatus = dbFactory.ToModelValue(reader, "POSTSTATUS").ToDecimal();
            t_taskdetails.PostDate = (DateTime?)dbFactory.ToModelValue(reader, "POSTDATE");
            t_taskdetails.ReserveNumber = (string)dbFactory.ToModelValue(reader, "RESERVENUMBER");
            t_taskdetails.ReserveRowNo = (string)dbFactory.ToModelValue(reader, "RESERVEROWNO");
            t_taskdetails.UnShelveQty = dbFactory.ToModelValue(reader, "UNSHELVEQTY").ToDecimal();
            t_taskdetails.Requstreason = (string)dbFactory.ToModelValue(reader, "REQUSTREASON");
            t_taskdetails.Remark = (string)dbFactory.ToModelValue(reader, "REMARK");
            t_taskdetails.ReviewUser = (string)dbFactory.ToModelValue(reader, "REVIEWUSER");
            t_taskdetails.ReviewDate = (DateTime?)dbFactory.ToModelValue(reader, "REVIEWDATE");
            t_taskdetails.ReviewStatus = dbFactory.ToModelValue(reader, "REVIEWSTATUS").ToDecimal();
            t_taskdetails.PostUser = (string)dbFactory.ToModelValue(reader, "POSTUSER");
            t_taskdetails.Costcenter = (string)dbFactory.ToModelValue(reader, "COSTCENTER");
            t_taskdetails.Wbselem = (string)dbFactory.ToModelValue(reader, "WBSELEM");
            t_taskdetails.ToStorageLoc = (string)dbFactory.ToModelValue(reader, "TOSTORAGELOC");
            t_taskdetails.FromStorageLoc = (string)dbFactory.ToModelValue(reader, "FROMSTORAGELOC");
            t_taskdetails.OutStockQty = dbFactory.ToModelValue(reader, "OUTSTOCKQTY").ToDecimal();
            t_taskdetails.LimitStockQtySAP = dbFactory.ToModelValue(reader, "LIMITSTOCKQTYSAP").ToDecimal();
            t_taskdetails.RemainsSockQtySAP = dbFactory.ToModelValue(reader, "REMAINSTOCKQTYSAP").ToDecimal();
            t_taskdetails.PackFlag = dbFactory.ToModelValue(reader, "PACKFLAG").ToDecimal();
            t_taskdetails.CurrentRemainStockQtySAP = dbFactory.ToModelValue(reader, "CURRENTREMAINSTOCKQTYSAP").ToDecimal();
            t_taskdetails.MoveReasonCode = (string)dbFactory.ToModelValue(reader, "MOVEREASONCODE");
            t_taskdetails.MoveReasonDesc = (string)dbFactory.ToModelValue(reader, "MOVEREASONDESC");
            t_taskdetails.PoNo = (string)dbFactory.ToModelValue(reader, "PONO");
            t_taskdetails.PoRowNo = (string)dbFactory.ToModelValue(reader, "POROWNO");
            t_taskdetails.IsLock = dbFactory.ToModelValue(reader, "ISLOCK").ToDecimal();
            t_taskdetails.IsSmallBatch = dbFactory.ToModelValue(reader, "ISSMALLBATCH").ToDecimal();
            t_taskdetails.DepartmentCode = (string)dbFactory.ToModelValue(reader, "DEPARTMENTCODE");
            t_taskdetails.DepartmentName = (string)dbFactory.ToModelValue(reader, "DEPARTMENTNAME");
            t_taskdetails.UnitName = (string)dbFactory.ToModelValue(reader, "UNITNAME");
            t_taskdetails.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_taskdetails.VoucherType = dbFactory.ToModelValue(reader, "VoucherType").ToInt32();
            t_taskdetails.ErpVoucherNo = dbFactory.ToModelValue(reader, "ERPVoucherNo").ToDBString();
            t_taskdetails.TaskNo = (string)dbFactory.ToModelValue(reader, "TaskNo");
            t_taskdetails.OperatorUserName = dbFactory.ToModelValue(reader, "OperatorUserName").ToDBString();
            t_taskdetails.StrLineStatus = dbFactory.ToModelValue(reader, "StrLineStatus").ToDBString();
            t_taskdetails.IsSerial = dbFactory.ToModelValue(reader, "IsSerial").ToInt32();
            t_taskdetails.MoveType = (string)dbFactory.ToModelValue(reader, "MoveType");
            t_taskdetails.MaterialNoID = dbFactory.ToModelValue(reader, "MaterialNoID").ToInt32();
            t_taskdetails.PartNo = dbFactory.ToModelValue(reader, "PartNo").ToDBString();

            t_taskdetails.IsSpcBatch = dbFactory.ToModelValue(reader, "IsSpcBatch").ToDBString();
            t_taskdetails.FromBatchNo = dbFactory.ToModelValue(reader, "FromBatchno").ToDBString();
            t_taskdetails.FromErpAreaNo = dbFactory.ToModelValue(reader, "FromErpAreaNo").ToDBString();
            t_taskdetails.FromErpWarehouse = dbFactory.ToModelValue(reader, "FromErpWareHouse").ToDBString();
            t_taskdetails.ToBatchNo = dbFactory.ToModelValue(reader, "ToBatchno").ToDBString();
            t_taskdetails.ToErpAreaNo = dbFactory.ToModelValue(reader, "ToErpAreaNo").ToDBString();
            //t_taskdetails.ToErpWarehouse = dbFactory.ToModelValue(reader, "ToErpWareHouse").ToDBString();
            t_taskdetails.FloorType = dbFactory.ToModelValue(reader, "FloorType").ToInt32();
            t_taskdetails.HeightArea = dbFactory.ToModelValue(reader, "HeightArea").ToInt32();
            t_taskdetails.StrongHoldCode = dbFactory.ToModelValue(reader, "StrongHoldCode").ToDBString();
            t_taskdetails.StrongHoldName = dbFactory.ToModelValue(reader, "StrongHoldName").ToDBString();
            t_taskdetails.CompanyCode = dbFactory.ToModelValue(reader, "CompanyCode").ToDBString();
            t_taskdetails.RowNoDel = dbFactory.ToModelValue(reader, "RowNoDel").ToDBString();
            t_taskdetails.StrVoucherType = dbFactory.ToModelValue(reader, "StrVoucherType").ToDBString();
            t_taskdetails.OutstockDetailID = dbFactory.ToModelValue(reader, "OutstockDetailID").ToInt32();

            t_taskdetails.StrIsSpcBatch = t_taskdetails.IsSpcBatch == "Y" ? "是" : "否";
            t_taskdetails.ERPVoucherType = dbFactory.ToModelValue(reader, "ErpVoucherType").ToDBString();
            t_taskdetails.StrModifyer = dbFactory.ToModelValue(reader, "StrModifyer").ToDBString();
            t_taskdetails.ModifyTime = dbFactory.ToModelValue(reader, "ModifyTime").ToDateTime();
            t_taskdetails.MainTypeCode = dbFactory.ToModelValue(reader, "MainTypeCode").ToDBString();
            t_taskdetails.HouseProp = dbFactory.ToModelValue(reader, "HouseProp").ToInt32();
            t_taskdetails.StrHouseProp = dbFactory.ToModelValue(reader, "StrHouseProp").ToDBString();
            t_taskdetails.UnReviewQty = t_taskdetails.TaskQty.ToDecimal() - t_taskdetails.ReviewQty.ToDecimal();
            T_OutStockDetail_DB tdb = new T_OutStockDetail_DB();
            t_taskdetails.EAN =  tdb.GetMaterialEAN(t_taskdetails.MaterialNo);

            t_taskdetails.FromErpWareHouseName = dbFactory.ToModelValue(reader, "FromErpWareHouseName").ToDBString();
            t_taskdetails.ToErpWareHouseName = dbFactory.ToModelValue(reader, "ToErpWareHouseName").ToDBString();
            t_taskdetails.SupCusCode = dbFactory.ToModelValue(reader, "SupCusCode").ToDBString();
            t_taskdetails.SupCusName = dbFactory.ToModelValue(reader, "SupCusName").ToDBString();
            t_taskdetails.ProjectNo = dbFactory.ToModelValue(reader, "ProjectNo").ToDBString();
            t_taskdetails.TracNo = dbFactory.ToModelValue(reader, "TracNo").ToDBString();

            string FromErpWarehouse = dbFactory.ToModelValue(reader, "FromErpWarehouse").ToDBString();
            t_taskdetails.FromErpWarehouse = string.IsNullOrEmpty(FromErpWarehouse) ? "" : (FromErpWarehouse.Contains("-") ? FromErpWarehouse : (t_taskdetails.StrongHoldCode + "-" + FromErpWarehouse));

            string ToErpWareHouse = dbFactory.ToModelValue(reader, "ToErpWareHouse").ToDBString();
            t_taskdetails.ToErpWarehouse = string.IsNullOrEmpty(ToErpWareHouse) ? "" : (ToErpWareHouse.Contains("-") ? ToErpWareHouse : (t_taskdetails.StrongHoldCode + "-" + ToErpWareHouse));



            return t_taskdetails;
        }

        protected override string GetViewName()
        {
            return "V_OUTTASKDETAIL";
        }

        protected override string GetTableName()
        {
            return "T_TASKDETAILS";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        

        //public override List<T_OutStockTaskDetailsInfo> GetModelListByHeaderID(int headerID)
        //{
        //    try
        //    {
        //        string strError = string.Empty;

        //        List<T_StockInfo> lstStock = new List<T_StockInfo>();
        //        List<T_OutStockTaskDetailsInfo> list = base.GetModelListByHeaderID(headerID);

        //        if (list == null || list.Count == 0) 
        //        {
        //            return null;
        //        }

        //        if (GetPickRuleAreaNo(list, ref lstStock, ref strError) == false)
        //        {
        //            throw new Exception(strError);
        //        }

        //        //if (lstStock == null || lstStock.Count == 0) 
        //        //{
        //        //    throw new Exception("获取物料对应拣货库位为空！");
        //        //}

        //       return  CreateNewListByPickRuleAreaNo(list, lstStock);
               
        //    }
        //    catch (Exception ex) 
        //    {
        //        throw new Exception(ex.Message);
        //    }

            
        //}


        public bool GetPickRuleAreaNo(List<T_OutStockTaskDetailsInfo> modelList,ref List<T_StockInfo> stockList, ref string strError) 
        {
            try
            {
                int iResult = 0;                
                DataSet ds;

                string strOutStockTaskXml = XmlUtil.Serializer(typeof(List<T_OutStockTaskDetailsInfo>), modelList);
                LogNet.LogInfo("GetPickRuleAreaNo:" + strOutStockTaskXml);
                OracleParameter[] cmdParms = new OracleParameter[] 
                {
                    new OracleParameter("strOutStockTaskXml", OracleDbType.NClob),    
                    new OracleParameter("PickAreaCur", OracleDbType.RefCursor,ParameterDirection.Output),
                    new OracleParameter("bResult", OracleDbType.Int32,ParameterDirection.Output),
                    new OracleParameter("ErrString", OracleDbType.NVarchar2,200,strError,ParameterDirection.Output)
                };

                cmdParms[0].Value = strOutStockTaskXml;

                cmdParms[1].Value = ParameterDirection.Output;
                cmdParms[2].Value = ParameterDirection.Output;
                cmdParms[3].Value = ParameterDirection.Output;

                ds = dbFactory.ExecuteDataSetForCursor(ref iResult, ref strError, dbFactory.ConnectionStringLocalTransaction, CommandType.StoredProcedure, "p_getpickarea", cmdParms);

                if (iResult == 1)
                {
                    stockList = TOOL.DataTableToList.DataSetToList<T_StockInfo>(ds.Tables[0]);
                    strError = string.Empty;
                }

                return iResult == 1 ? true : false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  List<T_OutStockTaskDetailsInfo> CreateNewListByPickRuleAreaNo( List<T_OutStockTaskDetailsInfo> modelList,  List<T_StockInfo> stockList) 
        {
            List<T_OutStockTaskDetailsInfo> NewModelList = new List<T_OutStockTaskDetailsInfo>();

            #region 现有代码暂时注释

            //foreach (var item in stockList)
            //{
            //    List<T_OutStockTaskDetailsInfo> itemModelList = new List<T_OutStockTaskDetailsInfo>();
            //    itemModelList = modelList.FindAll(t => t.ID == item.OutTaskDetID).ToList();
            //    foreach (var itemModel in itemModelList)
            //    {
            //        T_OutStockTaskDetailsInfo model = new T_OutStockTaskDetailsInfo();
            //        model.ID = itemModel.ID;
            //        model.HeaderID = itemModel.HeaderID;
            //        model.CompanyCode = itemModel.CompanyCode;
            //        model.StrongHoldCode = itemModel.StrongHoldCode;
            //        model.StrongHoldName = itemModel.StrongHoldName;
            //        model.MaterialNo = itemModel.MaterialNo;
            //        model.MaterialDesc = itemModel.MaterialDesc;
            //        model.MaterialNoID = itemModel.MaterialNoID;
            //        model.ErpVoucherNo = itemModel.ErpVoucherNo;
            //        model.VoucherNo = itemModel.VoucherNo;
            //        model.TaskNo = itemModel.TaskNo;
            //        model.TaskQty = itemModel.TaskQty;
            //        model.RemainQty = itemModel.RemainQty;
            //        model.RePickQty = itemModel.RePickQty;
            //        model.AreaNo = item.AreaNo;
            //        model.HeightArea = itemModel.HeightArea;
            //        model.FloorType = itemModel.FloorType;
            //        model.IsSpcBatch = itemModel.IsSpcBatch;
            //        model.ScanQty = itemModel.ScanQty;
            //        model.FromBatchNo = itemModel.FromBatchNo;
            //        model.FromErpAreaNo = itemModel.FromErpAreaNo;
            //        model.FromErpWarehouse = itemModel.FromErpWarehouse;
            //        model.ToBatchNo = itemModel.ToBatchNo;
            //        model.ToErpAreaNo = itemModel.ToErpAreaNo;
            //        model.ToErpWarehouse = itemModel.ToErpWarehouse;
            //        model.TaskType = itemModel.TaskType;
            //        model.DepartmentCode = itemModel.DepartmentCode;
            //        model.DepartmentName = itemModel.DepartmentName;
            //        model.EDate = itemModel.EDate;
            //        model.PickGroupNo = itemModel.PickGroupNo;
            //        model.PickLeaderUserNo = itemModel.PickLeaderUserNo;
            //        model.Status = itemModel.Status;
            //        model.StrStatus = itemModel.StrStatus;
            //        model.StrVoucherType = itemModel.StrVoucherType;
            //        model.StockQty = item.Qty;
            //        NewModelList.Add(model);
            //    }               
            //}

            #endregion            

            

            foreach (var item in modelList) 
            {
                List<T_StockInfo> stockModelList = new List<T_StockInfo>();
                //查找物料可分配库存
                stockModelList = stockList.FindAll(t => t.MaterialNo == item.MaterialNo && t.IsSpcBatch == item.IsSpcBatch && t.Qty>0);
                foreach (var stockModel in stockModelList) 
                {
                    if (item.RemainQty < stockModel.Qty)
                    {
                        item.RePickQty = item.RemainQty;
                        stockModel.Qty = stockModel.Qty - item.RemainQty.ToDecimal();
                        NewModelList.Add(CreateNewOutStockModelToADF(stockModel, item));
                        break;
                    }
                    else if (item.RemainQty == stockModel.Qty)
                    {
                        item.RePickQty = stockModel.Qty;
                        stockModel.Qty = 0;
                        NewModelList.Add(CreateNewOutStockModelToADF(stockModel, item));
                        break;
                    }
                    else if (item.RemainQty > stockModel.Qty)
                    {
                        item.RePickQty = stockModel.Qty;
                        item.RemainQty = item.RemainQty - stockModel.Qty;
                        stockModel.Qty = 0;
                        NewModelList.Add(CreateNewOutStockModelToADF(stockModel, item));
                    }
                }
            }
            //for (int i = 0; i < stockList.Count; i++) 
            //{
            //    List<T_OutStockTaskDetailsInfo> itemModelList = new List<T_OutStockTaskDetailsInfo>();
            //    //根据库存查找订单物料
            //    itemModelList = modelList.FindAll(t => t.MaterialNo == stockList[i].MaterialNo && t.IsSpcBatch == stockList[i].IsSpcBatch);
            //    foreach (var itemModel in itemModelList)
            //    {
            //        if (itemModel.RemainQty == 0 || stockList[i].Qty==0) { break; }

            //        if (itemModel.RemainQty < stockList[i].Qty)
            //        {
            //            itemModel.RePickQty = itemModel.RemainQty;
            //            stockList[i].Qty = stockList[i].Qty - itemModel.RemainQty.ToDecimal();
            //            NewModelList.Add(CreateNewOutStockModelToADF(stockList[i], itemModel));
            //        }
            //        else if (itemModel.RemainQty == stockList[i].Qty)
            //        {
            //            itemModel.RePickQty = stockList[i].Qty;
            //            stockList[i].Qty = 0;
            //            NewModelList.Add(CreateNewOutStockModelToADF(stockList[i], itemModel));
            //        }
            //        else if (itemModel.RemainQty > stockList[i].Qty)
            //        {
            //            itemModel.RePickQty = stockList[i].Qty;
            //            itemModel.RemainQty = itemModel.RemainQty - stockList[i].Qty;
            //            stockList[i].Qty = 0;
            //            NewModelList.Add(CreateNewOutStockModelToADF(stockList[i], itemModel));
            //        }
                    
            //    }
            //}

            #region 异常代码
            //foreach (var itemStock in stockList)
            //{
            //    List<T_OutStockTaskDetailsInfo> itemModelList = new List<T_OutStockTaskDetailsInfo>();
            //    //根据库存查找订单物料
            //    itemModelList = modelList.FindAll(t => t.MaterialNo == itemStock.MaterialNo && t.IsSpcBatch == itemStock.IsSpcBatch);
            //    foreach (var itemModel in itemModelList)
            //    {
            //        if (itemModel.RemainQty < itemStock.Qty)
            //        {
            //            itemModel.RePickQty = itemModel.RemainQty;
            //            itemStock.Qty = itemStock.Qty - itemModel.RemainQty.ToDecimal();
            //        }
            //        else if (itemModel.RemainQty == itemStock.Qty)
            //        {
            //            itemModel.RePickQty = itemStock.Qty;
            //            itemStock.Qty = 0;
            //        }
            //        else if (itemModel.RemainQty > itemStock.Qty)
            //        {
            //            itemModel.RePickQty = itemStock.Qty;
            //            itemModel.RemainQty = itemModel.RemainQty - itemStock.Qty;
            //            itemStock.Qty = 0;

            //        }
            //        T_OutStockTaskDetailsInfo model = new T_OutStockTaskDetailsInfo();
            //        model.ID = itemModel.ID;
            //        model.HeaderID = itemModel.HeaderID;
            //        model.CompanyCode = itemModel.CompanyCode;
            //        model.StrongHoldCode = itemModel.StrongHoldCode;
            //        model.StrongHoldName = itemModel.StrongHoldName;
            //        model.MaterialNo = itemModel.MaterialNo;
            //        model.MaterialDesc = itemModel.MaterialDesc;
            //        model.MaterialNoID = itemModel.MaterialNoID;
            //        model.ErpVoucherNo = itemModel.ErpVoucherNo;
            //        model.VoucherNo = itemModel.VoucherNo;
            //        model.TaskNo = itemModel.TaskNo;
            //        model.TaskQty = itemModel.TaskQty;
            //        model.RemainQty = itemModel.RemainQty;
            //        model.RePickQty = itemModel.RePickQty;
            //        model.AreaNo = itemStock.AreaNo;
            //        model.HeightArea = itemModel.HeightArea;
            //        model.FloorType = itemModel.FloorType;
            //        model.IsSpcBatch = itemModel.IsSpcBatch;
            //        model.ScanQty = itemModel.ScanQty;
            //        model.FromBatchNo = itemModel.FromBatchNo;
            //        model.FromErpAreaNo = itemModel.FromErpAreaNo;
            //        model.FromErpWarehouse = itemModel.FromErpWarehouse;
            //        model.ToBatchNo = itemModel.ToBatchNo;
            //        model.ToErpAreaNo = itemModel.ToErpAreaNo;
            //        model.ToErpWarehouse = itemModel.ToErpWarehouse;
            //        model.TaskType = itemModel.TaskType;
            //        model.DepartmentCode = itemModel.DepartmentCode;
            //        model.DepartmentName = itemModel.DepartmentName;
            //        model.EDate = itemModel.EDate;
            //        model.PickGroupNo = itemModel.PickGroupNo;
            //        model.PickLeaderUserNo = itemModel.PickLeaderUserNo;
            //        model.Status = itemModel.Status;
            //        model.StrStatus = itemModel.StrStatus;
            //        model.StrVoucherType = itemModel.StrVoucherType;
            //        model.StockQty = itemStock.Qty;
            //        NewModelList.Add(model);
            //    }
            //}
            #endregion
           
            return NewModelList.OrderBy(t=>t.SortArea).ToList();

        }

        private T_OutStockTaskDetailsInfo CreateNewOutStockModelToADF(T_StockInfo stockModel,  T_OutStockTaskDetailsInfo itemModel)
        {
            T_OutStockTaskDetailsInfo model = new T_OutStockTaskDetailsInfo();
            model.ID = itemModel.ID;
            model.HeaderID = itemModel.HeaderID;
            model.CompanyCode = itemModel.CompanyCode;
            model.StrongHoldCode = itemModel.StrongHoldCode;
            model.StrongHoldName = itemModel.StrongHoldName;
            model.MaterialNo = itemModel.MaterialNo;
            model.MaterialDesc = itemModel.MaterialDesc;
            model.MaterialNoID = itemModel.MaterialNoID;
            model.ErpVoucherNo = itemModel.ErpVoucherNo;
            model.VoucherNo = itemModel.VoucherNo;
            model.TaskNo = itemModel.TaskNo;
            model.TaskQty = itemModel.TaskQty;
            model.RemainQty = itemModel.RemainQty;
            model.RePickQty = itemModel.RePickQty;
            model.AreaNo = stockModel.AreaNo;
            model.HeightArea = itemModel.HeightArea;
            model.FloorType = itemModel.FloorType;
            model.IsSpcBatch = itemModel.IsSpcBatch;
            model.ScanQty = itemModel.ScanQty;
            model.FromBatchNo = itemModel.FromBatchNo;
            model.FromErpAreaNo = itemModel.FromErpAreaNo;
            model.FromErpWarehouse = itemModel.FromErpWarehouse;
            model.ToBatchNo =stockModel.BatchNo ;//itemModel.ToBatchNo
            model.ToErpAreaNo = itemModel.ToErpAreaNo;
            model.ToErpWarehouse = itemModel.ToErpWarehouse;            
            model.TaskType = itemModel.TaskType;
            model.DepartmentCode = itemModel.DepartmentCode;
            model.DepartmentName = itemModel.DepartmentName;
            model.EDate = itemModel.EDate;
            model.PickGroupNo = itemModel.PickGroupNo;
            model.PickLeaderUserNo = itemModel.PickLeaderUserNo;
            model.Status = itemModel.Status;
            model.StrStatus = itemModel.StrStatus;
            model.StrVoucherType = itemModel.StrVoucherType;
            model.StockQty = stockModel.Qty;
            model.SortArea = stockModel.SortArea;
            model.ERPVoucherType = itemModel.ERPVoucherType;
            model.Unit = itemModel.Unit;
            return model;
        }


        public List<T_OutStockTaskDetailsInfo> GetExportTaskDetail(T_OutStockTaskInfo model)
        {
            try
            {
                List<T_OutStockTaskDetailsInfo> modelList = new List<T_OutStockTaskDetailsInfo>();
                string strSql = "select * from V_OUTTASKDETAIL " + GetExportFilterSql(model);
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

        private string GetExportFilterSql(T_OutStockTaskInfo model)
        {
            string strSql = " where isnull(isDel,0) != 2  ";

            string strAnd = " and ";

            strSql += strAnd;
            strSql += "TaskType = 2 ";

            if (model.ID > 0) 
            {
                strSql += strAnd;
                strSql += " id = '"+model.ID+"'";
            }

            if (!string.IsNullOrEmpty(model.SupcusCode))
            {
                strSql += strAnd;
                strSql += " (SupcusCode Like '" + model.SupcusCode + "%'  or SupplierName Like '" + model.SupcusCode + "%' )";
            }

            if (model.DateFrom != null)
            {
                strSql += strAnd;
                strSql += " CreateTime >= " + model.DateFrom.ToDateTime().Date.ToOracleTimeString() + " ";
            }

            if (model.DateTo != null)
            {
                strSql += strAnd;
                strSql += " CreateTime <= " + model.DateTo.ToDateTime().Date.AddDays(1).ToOracleTimeString() + " ";
            }

            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " erpvoucherno Like '" + model.ErpVoucherNo.Trim() + "%'  ";
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

           

            return strSql;
        } 
        

        public List<T_OutStockTaskDetailsInfo> GetOutTaskDetailListByHeaderIDADF(List<T_OutStockTaskInfo> lstModel)
        {
            try
            {
                string strSql = string.Empty;
                string headerID = string.Empty;

                foreach (var item in lstModel) 
                {
                    headerID += item.ID + ",";
                }

                headerID = headerID.TrimEnd(',');

                strSql = "select * from v_Outtaskdetail where headerid in ("+headerID+")";

                return base.GetModelListBySql(strSql);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GetDBVoucherNo(string strSerialNo) 
        {
            string strSql = "select materialdoc from (select materialdoc from t_Tasktrans where serialno  = '"+strSerialNo+"' and tasktype = 2  order by id desc) where rownum = 1";
            return base.GetScalarBySql(strSql).ToDBString();
        }

        #region 拣选小车操作

        public int GetCarNo(string strCarNo)
        {
            string strSql = "select count(1) from t_pickcar where carno = '" + strCarNo + "'";
            return base.GetScalarBySql(strSql).ToInt32();            
        }

        public string PostScanCar(string strCarNo)
        {
            string strSql = "select taskno from t_pickcar where carno = '" + strCarNo + "'";
            return base.GetScalarBySql(strSql).ToDBString();
        }

        public bool PostBindCarTask(string strCarNo, string strTaskNo, string strUserNo, ref string strError)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;
            //int carID = base.GetTableID("SEQ_PICKCAR_ID");

            strSql = "update t_pickcar set taskno = '" + strTaskNo + "' where carno = '" + strCarNo + "'";
            //strSql = "insert into t_Pickcar (ID,Carno,Taskno,Creater,Createtime)" +
            //        " VALUES( '" + carID + "','" + strCarNo + "','" + strTaskNo + "','" + strUserNo + "',getdate() )";
            lstSql.Add(strSql);

            strSql = "update t_task set carno = '" + strCarNo + "' where taskno = '" + strTaskNo + "'";
            lstSql.Add(strSql);
            return base.SaveModelListBySqlToDB(lstSql, ref strError);

        }

        /// <summary>
        /// 手动释放拣选小车
        /// </summary>
        /// <param name="strCarNo"></param>
        /// <param name="strTaskNo"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool DeleteCarModelBySql(string strCarNo, string strTaskNo, ref string strError)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            strSql = "delete t_Pickcar where taskno = '" + strTaskNo + "' and carno = '" + strCarNo + "'";
            lstSql.Add(strSql);

            return base.SaveModelListBySqlToDB(lstSql, ref strError);

        }

        #endregion

        #region 根据ERP订单号获取拣货任务

        public bool GetOutTaskDetailByErpVoucherNo(string strErpVoucherNo, ref List<T_OutStockTaskDetailsInfo> modelListTaskDetail, ref string strError) 
        {
            //获取拣货单表体数据            
            string taskFilter = " erpvoucherno = '" + strErpVoucherNo + "' ";
            modelListTaskDetail = base.GetModelListByFilter("", taskFilter, "*");
            if (modelListTaskDetail == null || modelListTaskDetail.Count == 0)
            {
                strError = "未能获取到拣货数据！";
                return false;
            }
            return true;
        }
        #endregion
    }
}
