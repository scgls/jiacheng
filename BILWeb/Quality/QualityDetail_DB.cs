//************************************************************
//******************************作者：方颖*********************
//******************************时间：2017/6/27 11:36:53*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using BILBasic.Common;
using BILBasic.User;
using BILWeb.Stock;
using System.Data;

namespace BILWeb.Quality
{
    public partial class T_QualityDetail_DB : BILBasic.Basing.Factory.Base_DB<T_QualityDetailInfo>
    {

        /// <summary>
        /// 添加t_qualitydetail
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_QualityDetailInfo t_qualitydetail)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();
        }

        protected override List<string> GetSaveSql(BILBasic.User.UserModel user, ref T_QualityDetailInfo model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_QualityDetailInfo ToModel(IDataReader reader)
        {
            T_QualityDetailInfo t_qualitydetail = new T_QualityDetailInfo();

            t_qualitydetail.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_qualitydetail.ErpInVoucherNo = (string)dbFactory.ToModelValue(reader, "ERPVOUCHERNO");
            t_qualitydetail.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "STRONGHOLDCODE");
            t_qualitydetail.StrongHoldName = (string)dbFactory.ToModelValue(reader, "STRONGHOLDNAME");
            t_qualitydetail.CompanyCode = (string)dbFactory.ToModelValue(reader, "COMPANYCODE");
            t_qualitydetail.ERPCreater = (string)dbFactory.ToModelValue(reader, "ERPCREATER");
            t_qualitydetail.VouDate = (DateTime?)dbFactory.ToModelValue(reader, "VOUDATE");
            t_qualitydetail.VouUser = (string)dbFactory.ToModelValue(reader, "VOUUSER");
            t_qualitydetail.ERPStatus = dbFactory.ToModelValue(reader, "ERPSTATUS").ToDBString();
            t_qualitydetail.ERPNote = (string)dbFactory.ToModelValue(reader, "ERPNOTE");
            t_qualitydetail.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_qualitydetail.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_qualitydetail.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_qualitydetail.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_qualitydetail.Status = dbFactory.ToModelValue(reader, "STATUS").ToInt32();
            t_qualitydetail.TimeStamp = (DateTime?)dbFactory.ToModelValue(reader, "TIMESTAMP");
            t_qualitydetail.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToInt32();
            
            t_qualitydetail.NoticeStatus = dbFactory.ToModelValue(reader, "NOTICESTATUS").ToInt32();
            t_qualitydetail.QualityType = dbFactory.ToModelValue(reader, "QUALITYTYPE").ToInt32();
            t_qualitydetail.MaterialNo = (string)dbFactory.ToModelValue(reader, "MATERIALNO");
            t_qualitydetail.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MATERIALDESC");
            t_qualitydetail.InSQty = (decimal?)dbFactory.ToModelValue(reader, "INSQTY");
            t_qualitydetail.Unit = (string)dbFactory.ToModelValue(reader, "UNIT");
            t_qualitydetail.UnitName = (string)dbFactory.ToModelValue(reader, "UNITNAME");
            t_qualitydetail.QuanQty = (decimal?)dbFactory.ToModelValue(reader, "QUANQTY");
            t_qualitydetail.UnQuanQty = (decimal?)dbFactory.ToModelValue(reader, "UNQUANQTY");
            t_qualitydetail.DesQty = (decimal?)dbFactory.ToModelValue(reader, "DESQTY");
            t_qualitydetail.WarehouseNo = (string)dbFactory.ToModelValue(reader, "WAREHOUSENO");
            t_qualitydetail.BatchNo = (string)dbFactory.ToModelValue(reader, "BATCHNO");
            t_qualitydetail.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ErpVoucherNo");           
            t_qualitydetail.ErpInVoucherNo = (string)dbFactory.ToModelValue(reader, "ErpInVoucherNo");
            t_qualitydetail.SampQty = (decimal)dbFactory.ToModelValue(reader, "SampQty");
            t_qualitydetail.RemainQty = (decimal?)dbFactory.ToModelValue(reader, "RemainQty");
            t_qualitydetail.MaterialNoID = dbFactory.ToModelValue(reader, "MaterialNoID").ToInt32();

            if (Common_Func.readerExists(reader, "Areano")) t_qualitydetail.AreaNo = (string)dbFactory.ToModelValue(reader, "Areano");
            if (Common_Func.readerExists(reader, "AreaType")) t_qualitydetail.AreaType = reader["AreaType"].ToInt32();

            return t_qualitydetail;

        }

        /// <summary>
        /// 获取取样保存SQL
        /// </summary>
        /// <param name="user"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        protected override List<string> GetSaveModelListSql(UserModel user, List<T_QualityDetailInfo> modelList)
        {         
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();
            string NewSerialno = string.Empty;

            foreach (var item in modelList)
            {
                //strSql = "update t_Quality a set a.remainqty = (case when (isnull(a.remainqty,0) - '" + item.ScanQty + "') <= 0 then 0 else isnull(a.remainqty,0) - '" + item.ScanQty + "' end)," +
                //        "a.Scanqty = (isnull(a.Receiveqty,0) + '" + item.ScanQty + "') ," +
                //        "a.Scanuserno = '" + user.UserNo + "',a.Scandate = getdate() where id = '" + item.ID + "'";
                //lstSql.Add(strSql);

                strSql = "update t_Quality a set a.Status = (case when '" + item.ScanQty + "' < isnull(a.Sampqty,0) and isnull(a.Sampqty,0)<>0 then 2" +
                        " when  '" + item.ScanQty + "' = isnull(a.Sampqty,0) and isnull(a.Sampqty,0)<>0 then 3 end ),a.Scanuserno = '" + user.UserNo + "',a.Scandate = getdate(), a.Scanqty =  '" + item.ScanQty + "',a.Remainqty = a.Sampqty - '"+item.ScanQty+"'  where id = '" + item.ID + "'";
                lstSql.Add(strSql);

                strSql = "update t_Taskdetails b set b.Remainqty = b.Remainqty - '"+item.ScanQty+"' where b.Id =  "+
                        "(select b.id from t_task a left join t_Taskdetails b on a.Id =b.Headerid "+
                        "where a.Erpinvoucherno = '"+item.ErpInVoucherNo+"' and b.Materialnoid = '"+item.MaterialNoID+"'"+
                        "and b.Batchno = '"+item.BatchNo+"')";

                lstSql.Add(strSql);

                strSql = "update t_stock a set a.Status = 2 where a.Materialnoid = '" + item.MaterialNoID + "' and a.Batchno = '" + item.BatchNo + "' and a.Strongholdcode = '" + item.StrongHoldCode + "' and a.Warehouseid = (select id from t_Warehouse where warehouseno = '" + item.WarehouseNo + "') and a.Areaid in ( select id from v_Area  a where a.warehouseno = '" + item.WarehouseNo + "' and a.AREANO in " +
                        " (select areano from t_Qualitydetail where headerid = '"+item.ID+"'))";
                lstSql.Add(strSql);

                foreach (var stockItem in item.lstStock)
                {
                    strSql = "update t_stock a set a.Warehouseid = (select warehouseid from v_Area where areano = '" + item.ToErpAreaNo + "' and warehouseno = '" + item.ToErpWarehouse + "')," +
                                " a.Houseid = (select houseid from v_Area where areano = '" + item.ToErpAreaNo + "' and warehouseno = '" + item.ToErpWarehouse + "'),a.Areaid  = (select id from v_Area where areano = '" + item.ToErpAreaNo + "' and warehouseno = '" + item.ToErpWarehouse + "')" +
                                " ,a.Palletno = '' where serialno = '" + stockItem.SerialNo + "'";
                    lstSql.Add(strSql);

                    strSql = "DELETE t_Palletdetail WHERE SERIALNO = '" + stockItem.SerialNo + "'";
                    lstSql.Add(strSql);

                    strSql = "delete t_Pallet where palletno =( select a.Palletno from t_Palletdetail a  where serialno = '" + stockItem.SerialNo + "' )  and (select count(1) from t_Palletdetail where palletno =( select a.Palletno from t_Palletdetail a  where serialno = '" + stockItem.SerialNo + "') )=0";
                    lstSql.Add(strSql);

                    //if (stockItem.PickModel == 2) //整箱取样
                    //{
                    //    //查找取样库，更新库存仓库ID
                    //    strSql = "update t_stock a set a.Warehouseid = (select warehouseid from v_Area where areano = '" + item.ToErpAreaNo+ "')," +
                    //            " a.Houseid = (select houseid from v_Area where areano = '" + item.ToErpAreaNo + "'),a.Areaid  = (select id from v_Area where areano = '" + item.ToErpAreaNo + "')" +
                    //            " where serialno = '"+stockItem.SerialNo+"'";
                    //    lstSql.Add(strSql);
                    //}
                    //else if (stockItem.PickModel == 3)//拆零取样
                    //{
                    //    //strSql = GetAmountQtySql(stockItem, ref NewSerialno);//生成条码表拆零条码
                    //    //lstSql.Add(strSql);

                    //    //strSql = GetAmountQtyInsertStockSql(stockItem, user, NewSerialno);
                    //    //lstSql.Add(strSql);

                    //    //strSql = "update t_stock a set a.Qty = a.Qty - '" + stockItem.AmountQty + "'  where serialno = '" + stockItem.SerialNo + "'";
                    //    //lstSql.Add(strSql);

                    //    //strSql = "delete t_stock where serialno = '" + stockItem.SerialNo + "' and qty =0";
                    //    //lstSql.Add(strSql);
                    //    strSql = "update t_stock a set a.Warehouseid = (select warehouseid from v_Area where areano = '" + item.ToErpAreaNo + "')," +
                    //            " a.Houseid = (select houseid from v_Area where areano = '" + item.ToErpAreaNo + "'),a.Areaid  = (select id from v_Area where areano = '" + item.ToErpAreaNo + "')" +
                    //            " where serialno = '" + stockItem.SerialNo + "'";
                    //    lstSql.Add(strSql);

                    //    if (!string.IsNullOrEmpty(stockItem.PalletNo))
                    //    {
                    //        strSql = GetAmountQtyInsertPalletSql(stockItem, user, NewSerialno, stockItem.PalletNo);
                    //        lstSql.Add(strSql);

                    //        strSql = "update t_Palletdetail a set a.Qty = a.qty - '" + stockItem.AmountQty + "' where serialno = '" + stockItem.SerialNo + "'";
                    //        lstSql.Add(strSql);

                    //        strSql = "delete t_Palletdetail a where a.Serialno = '" + stockItem.AmountQty + "' and a.Qty = 0";
                    //        lstSql.Add(strSql);
                    //    }
                    //}
                }
            }

            return lstSql;
        }


        public List<string> GetAmountQtySql(T_StockInfo model, ref string NewSerialNo)
        {
            //int barcodeID = GetTableID("Seq_Outbarcode_Id");

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

            string BarCode =""+model.BarCodeType+"" + model.MaterialNo.PadLeft(16, '0') + "" + model.BatchNo.PadLeft(11, '0') + ""+NewSerialNo+"";

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
                            "Barcodetype, '" + NewSerialNo + "', Barcodeno, Outcount, Innercount, Mantissaqty, Isrohs,'" + model.ID+ "',Inner_Id, " +
                            "Abatchqty, Isdel, Creater, getdate(), Materialnoid, Strongholdcode, " +
                            "Strongholdname, Companycode, Productdate, Supprdbatch, Supprddate, Productbatch, Edate, Storecondition," +
                            "Specialrequire, Batchno, Barcodemtype, Rownodel, Protectway, Boxweight, Unit, Labelmark, Boxdetail, Matebatch," +
                            "Mixdate, Relaweight,ean from t_Outbarcode where serialno = '" + model.SerialNo + "'";

            lstSql.Add(strSql);

            if (model.lstJBarCode != null && model.lstJBarCode.Count > 0) 
            {
                foreach (var item in model.lstJBarCode) 
                {
                    strSql = "UPDATE T_OUTBARCODE set fserialno = '" + BarCode + "',qty = qty - '"+model.AmountQty+"' WHERE barcode = '" + item.Barcode + "'";
                    lstSql.Add(strSql);
                }
            }

            return lstSql;

        }

        public  string GetAmountQtyInsertStockSql(T_StockInfo model, UserModel user, string NewSerialNo)
        {
            int stockID = 0;
            string strSql = string.Empty;

            //整箱区拣货直接插入库存
            //if (model.HouseProp == 1)
            //{
                

            //    strSql = "insert into t_Stock( Barcode, Serialno, Materialno, Materialdesc, Warehouseid, Houseid, Areaid, Qty, Status, Isdel," +
            //                    "Creater, Createtime, Batchno,  Oldstockid, Unit, Unitname,  " +
            //                    "Receivestatus,  Islimitstock,  Materialnoid, Strongholdcode, Strongholdname, Companycode," +
            //                    "Edate, Supcode, Supname, Productdate, Supprdbatch, Supprddate, Isquality,Stocktype,ean)" +
            //                    "select barcode,serialno,materialno,Materialdesc,'" + model.WareHouseID + "', '" + model.HouseID + "', '" + model.AreaID + "', Qty ,'" + model.Status + "','1'," +
            //                    "'" + user.UserNo + "',getdate(),batchno,'" + model.ID + "',unit,'" + model.UnitName + "','" + model.ReceiveStatus + "','" + model.IsLimitStock + "',Materialnoid," +
            //                    "Strongholdcode, Strongholdname, Companycode,Edate, Supcode, Supname, Productdate, Supprdbatch,Supprddate, '3',1,ean from t_Outbarcode where serialno = '" + NewSerialNo + "'";

            //}
            //else if(model.HouseProp==2)
            //{
            //    stockID = GetTableID("Seq_Stock_Id");

            //    strSql = "insert into t_Stock(Id, Barcode, Serialno, Materialno, Materialdesc, Warehouseid, Houseid, Areaid, Qty, Status, Isdel," +
            //                    "Creater, Createtime, Batchno,  Oldstockid, Unit, Unitname,  " +
            //                    "Receivestatus,  Islimitstock,  Materialnoid, Strongholdcode, Strongholdname, Companycode," +
            //                    "Edate, Supcode, Supname, Productdate, Supprdbatch, Supprddate, Isquality,Stocktype,Taskdetailesid,houseprop,ean)" +
            //                    "select '" + stockID + "',barcode,serialno,materialno,Materialdesc,'" + model.WareHouseID + "', '" + model.HouseID + "', '" + model.AreaID + "', Qty ,'" + model.Status + "','1'," +
            //                    "'" + user.UserNo + "',getdate(),batchno,'" + model.ID + "',unit,'" + model.UnitName + "','" + model.ReceiveStatus + "','" + model.IsLimitStock + "',Materialnoid," +
            //                    "Strongholdcode, Strongholdname, Companycode,Edate, Supcode, Supname, Productdate, Supprdbatch,Supprddate, '" + model.IsQuality + "',1,'" + model.TaskDetailesID + "','" + model.HouseProp + "',ean from t_Outbarcode where serialno = '" + NewSerialNo + "'";
                               
            //}
            strSql = "insert into t_Stock( Barcode, Serialno, Materialno, Materialdesc, Warehouseid, Houseid, Areaid, Qty, Status, Isdel," +
                                "Creater, Createtime, Batchno,  Oldstockid, Unit, Unitname,  " +
                                "Receivestatus,  Islimitstock,  Materialnoid, Strongholdcode, Strongholdname, Companycode," +
                                "Edate, Supcode, Supname, Productdate, Supprdbatch, Supprddate, Isquality,Stocktype,ean,BARCODETYPE)" +
                                "select barcode,serialno,materialno,Materialdesc,'" + model.WareHouseID + "', '" + model.HouseID + "', '" + model.AreaID + "', Qty ,'" + model.Status + "','1'," +
                                "'" + user.UserNo + "',getdate(),batchno,'" + model.ID + "',unit,'" + model.UnitName + "','" + model.ReceiveStatus + "','" + model.IsLimitStock + "','"+model.MaterialNoID+"'," +
                                "Strongholdcode, Strongholdname, Companycode,Edate, Supcode, Supname, Productdate, Supprdbatch,Supprddate, '3',1,ean,BARCODETYPE from t_Outbarcode where serialno = '" + NewSerialNo + "'";

            return strSql;
        }

        public string GetAmountQtyInsertPalletSql(T_StockInfo model, UserModel user, string NewSerialNo,string PalletNo)
        {
            int palletDetailID = GetTableID("Seq_Pallet_Detail_Id");

            string strSql = "insert into t_Palletdetail(Id, Headerid, Palletno, Materialno, Materialdesc, Serialno, Creater, Createtime, " +
                            "Isdel, Rowno, Voucherno, Erpvoucherno, Partno, Materialnoid, Qty, Barcode, Strongholdcode, " +
                            "Strongholdname, Companycode, Batchno,  Rownodel)" +
                            "select '" + palletDetailID + "',(select id from t_Pallet where palletno = '" + PalletNo + "'),'" + PalletNo + "',Materialno, Materialdesc, Serialno,'" + user.UserNo + "', getdate()," +
                            "'1',Rowno, Voucherno, Erpvoucherno, '" + model.PalletNo + "', Materialnoid, Qty, Barcode, Strongholdcode, " +
                            "Strongholdname, Companycode, Batchno,  Rownodel from t_Outbarcode where serialno = '" + NewSerialNo + "'";

            return strSql;
        }

        protected override string GetViewName()
        {
            return "v_Quanitydetail";
        }

        protected override string GetTableName()
        {
            return "T_QUALITYDETAIL";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override string GetHeaderIDFieldName()
        {
            return "ID";
        }

        //根据检验单号获取已经审核并且没有更新过库存的单子
        public List<T_QualityDetailInfo> GetQualityUpadteStock(string ErpVoucherNo) 
        {
            string strSql = "select a.*,b.Areano,(select v_area.AREATYPE from v_area where v_area.warehouseno = a.Warehouseno and v_area.AREANO = b.Areano) as AREATYPE from t_Quality a left join " +
                             "t_Qualitydetail b"+
                            " on a.Id  = b.Headerid where a.Erpstatuscode = 'Y' and "+
                            "isnull(a.Isupdatestock,1) = 1 and a.Erpvoucherno = '" + ErpVoucherNo + "'";
            return base.GetModelListBySql(strSql);
        }

    }
}
