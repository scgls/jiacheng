
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.User;
using BILBasic.DBA;
using BILBasic.Common;
using Oracle.ManagedDataAccess.Client;
using BILWeb.Stock;
using BILWeb.InStock;
using BILWeb.Pallet;
using BILWeb.OutStockTask;
using System.Data;

namespace BILWeb.OutStock
{
    public partial class T_OutStockDetail_DB : BILBasic.Basing.Factory.Base_DB<T_OutStockDetailInfo>
    {
        T_Stock_DB _db = new T_Stock_DB();

        /// <summary>
        /// 添加t_outstockdetails
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_OutStockDetailInfo t_outstockdetails)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user,ref  T_OutStockDetailInfo model)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            if (model.ID <= 0)
            {
                int detailID = base.GetTableID("seq_outstockdetail_id");
                model.ID = detailID;
                strSql = string.Format("insert into t_Outstockdetail (Id, Headerid,  Materialno, Materialdesc,Unit, Unitname, Outstockqty,  Creater, Createtime, Isdel,voucherno,partno) " +
                        "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',getdate(),'1','{8}','{9}')", detailID, model.HeaderID, model.MaterialNo, model.MaterialDesc, model.Unit, model.UnitName, model.OutStockQty, user.UserNo, model.VoucherNo, model.PartNo);

                lstSql.Add(strSql);
            }
            else 
            {
                strSql = "update t_Outstockdetail a set  Materialno = '" + model.MaterialNo + "', Partno='" + model.PartNo + "', Materialdesc='" + model.MaterialDesc + "', Outstockqty = '" + model.OutStockQty + "' where  Id='" + model.ID + "'";
                lstSql.Add(strSql);
            }

            
            return lstSql;
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_OutStockDetailInfo ToModel(IDataReader reader)
        {
            T_OutStockDetailInfo t_outstockdetails = new T_OutStockDetailInfo();

            t_outstockdetails.ID = dbFactory.ToModelValue(reader, "ID").ToInt32()
                ;
            t_outstockdetails.HeaderID = dbFactory.ToModelValue(reader, "HeaderID").ToInt32();
            t_outstockdetails.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ERPVOUCHERNO");
            t_outstockdetails.MaterialNo = (string)dbFactory.ToModelValue(reader, "MATERIALNO");
            t_outstockdetails.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MATERIALDESC");
            t_outstockdetails.RowNo = dbFactory.ToModelValue(reader, "ROWNO").ToDBString();
            t_outstockdetails.Plant = (string)dbFactory.ToModelValue(reader, "PLANT");
            t_outstockdetails.PlantName = (string)dbFactory.ToModelValue(reader, "PLANTNAME");
            t_outstockdetails.ToStorageLoc = (string)dbFactory.ToModelValue(reader, "TOSTORAGELOC");
            t_outstockdetails.Unit = (string)dbFactory.ToModelValue(reader, "UNIT");
            t_outstockdetails.UnitName = (string)dbFactory.ToModelValue(reader, "UNITNAME");
            t_outstockdetails.OutStockQty = (decimal)dbFactory.ToModelValue(reader, "OUTSTOCKQTY");
            t_outstockdetails.OldOutStockQty = (decimal)dbFactory.ToModelValue(reader, "OLDOUTSTOCKQTY");
            t_outstockdetails.RemainQty = (decimal)dbFactory.ToModelValue(reader, "REMAINQTY");
            t_outstockdetails.Costcenter = (string)dbFactory.ToModelValue(reader, "COSTCENTER");
            t_outstockdetails.Wbselem = (string)dbFactory.ToModelValue(reader, "WBSELEM");
            t_outstockdetails.FromStorageLoc = (string)dbFactory.ToModelValue(reader, "FROMSTORAGELOC");
            t_outstockdetails.ReviewStatus = (decimal?)dbFactory.ToModelValue(reader, "REVIEWSTATUS");
            t_outstockdetails.DepartmentCode = (string)dbFactory.ToModelValue(reader, "DEPARTMENTCODE");
            t_outstockdetails.DepartmentName = (string)dbFactory.ToModelValue(reader, "DEPARTMENTNAME");
            t_outstockdetails.CloseOweUser = (string)dbFactory.ToModelValue(reader, "CLOSEOWEUSER");
            t_outstockdetails.CloseOweDate = (DateTime?)dbFactory.ToModelValue(reader, "CLOSEOWEDATE");
            t_outstockdetails.CloseOweRemark = (string)dbFactory.ToModelValue(reader, "CLOSEOWEREMARK");
            t_outstockdetails.IsOweClose = (decimal?)dbFactory.ToModelValue(reader, "ISOWECLOSE");
            t_outstockdetails.OweRemark = (string)dbFactory.ToModelValue(reader, "OWEREMARK");
            t_outstockdetails.OweRemarkUser = (string)dbFactory.ToModelValue(reader, "OWEREMARKUSER");
            t_outstockdetails.OweRemarkDate = (DateTime?)dbFactory.ToModelValue(reader, "OWEREMARKDATE");
            t_outstockdetails.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_outstockdetails.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_outstockdetails.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_outstockdetails.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");

            t_outstockdetails.PartNo = (string)dbFactory.ToModelValue(reader, "PartNo");
            t_outstockdetails.RowNoDel = dbFactory.ToModelValue(reader, "RowNoDel").ToDBString();           

            t_outstockdetails.CompanyCode = (string)dbFactory.ToModelValue(reader, "CompanyCode");
            t_outstockdetails.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "StrongHoldCode");
            t_outstockdetails.StrongHoldName = (string)dbFactory.ToModelValue(reader, "StrongHoldName");
            t_outstockdetails.VoucherType = dbFactory.ToModelValue(reader, "VoucherType").ToInt32();
            t_outstockdetails.SourceVoucherNo = dbFactory.ToModelValue(reader, "SourceVoucherNo").ToDBString();
            t_outstockdetails.SourceRowNo = dbFactory.ToModelValue(reader, "SourceRowNo").ToDBString();
            t_outstockdetails.MaterialNoID = dbFactory.ToModelValue(reader, "MaterialNoID").ToInt32();
            t_outstockdetails.IsSpcBatch = dbFactory.ToModelValue(reader, "IsSpcBatch").ToDBString();
            t_outstockdetails.StrIsSpcBatch = t_outstockdetails.IsSpcBatch == "Y" ? "是" : "否";
            t_outstockdetails.FromBatchNo = dbFactory.ToModelValue(reader, "FromBatchNo").ToDBString();            
            t_outstockdetails.FromBatchNo = dbFactory.ToModelValue(reader, "FromBatchNo").ToDBString();
            t_outstockdetails.FromErpAreaNo = dbFactory.ToModelValue(reader, "FromErpAreaNo").ToDBString();
            t_outstockdetails.FromErpWarehouse = dbFactory.ToModelValue(reader, "FromErpWarehouse").ToDBString();
            t_outstockdetails.ToBatchno = dbFactory.ToModelValue(reader, "ToBatchno").ToDBString();
            t_outstockdetails.ToErpAreaNo = dbFactory.ToModelValue(reader, "ToErpAreaNo").ToDBString();
            t_outstockdetails.ToErpWarehouse = dbFactory.ToModelValue(reader, "ToErpWarehouse").ToDBString();
            t_outstockdetails.VoucherNo = dbFactory.ToModelValue(reader, "VoucherNo").ToDBString();
            t_outstockdetails.ERPVoucherType = dbFactory.ToModelValue(reader, "ERPVoucherType").ToDBString();

            t_outstockdetails.StockQty = _db.GetSumStockQtyByMaterialIDForOutDetail(t_outstockdetails.MaterialNoID, t_outstockdetails.IsSpcBatch, t_outstockdetails.FromBatchNo, t_outstockdetails.FromErpWarehouse, t_outstockdetails.StrongHoldCode);
            
            t_outstockdetails.Address = dbFactory.ToModelValue(reader, "Address").ToDBString();
            t_outstockdetails.Province = dbFactory.ToModelValue(reader, "Province").ToDBString();
            t_outstockdetails.City = dbFactory.ToModelValue(reader, "City").ToDBString();
            t_outstockdetails.Area = dbFactory.ToModelValue(reader, "Area").ToDBString();
            t_outstockdetails.Phone = dbFactory.ToModelValue(reader, "Phone").ToDBString();
            t_outstockdetails.Contact = dbFactory.ToModelValue(reader, "Contact").ToDBString();
            t_outstockdetails.Address1 = dbFactory.ToModelValue(reader, "Address1").ToDBString();
            t_outstockdetails.Status = dbFactory.ToModelValue(reader, "Status").ToInt32();
            t_outstockdetails.ReviewQty = dbFactory.ToModelValue(reader, "ReviewQty").ToDecimal();
            t_outstockdetails.PickQty = dbFactory.ToModelValue(reader, "PickQty").ToDecimal();
            t_outstockdetails.StrongholdcodeHeader = dbFactory.ToModelValue(reader, "StrongholdcodeHeader").ToDBString();
            //t_outstockdetails.Price = dbFactory.ToModelValue(reader, "Price").ToDecimal();
            //t_outstockdetails.Amount = dbFactory.ToModelValue(reader, "Amount").ToDecimal();
            t_outstockdetails.EAN = GetMaterialEAN(t_outstockdetails.MaterialNo);
            t_outstockdetails.CustomerCode = dbFactory.ToModelValue(reader, "CustomerCode").ToDBString();
            t_outstockdetails.CustomerName = dbFactory.ToModelValue(reader, "CustomerName").ToDBString();
            t_outstockdetails.FromErpWareHouseName = dbFactory.ToModelValue(reader, "FromErpWareHouseName").ToDBString();
            t_outstockdetails.ToErpWarehouseName = dbFactory.ToModelValue(reader, "ToErpWareHouseName").ToDBString();
            return t_outstockdetails;
        }

        public string GetMaterialEAN(string MaterialNo) 
        {
            string strSql = "select top 1  Watercode from t_Material_Pack a where  Mateno = '" + MaterialNo + "'  order by id desc";
            return base.GetScalarBySql(strSql).ToDBString();
        }

        //public override List<T_OutStockDetailInfo> GetModelListByHeaderID(int headerID)
        //{
        //    List<T_OutStockDetailInfo> list = base.GetModelListByHeaderID(headerID);
        //    if (list.Count > 0)
        //    {
        //        T_OutStockDetailInfo model = new T_OutStockDetailInfo();
        //        model.EditText = "";
        //        model.OrderNumber = "合计";
        //        model.OutStockQty = list.Sum(p => p.OutStockQty);

        //        list.Add(model);
        //    }

        //    return list;
        //}


        protected override string GetViewName()
        {
            return "V_OUTSTOCKDETAIL";
        }

        protected override string GetTableName()
        {
            return "T_OUTSTOCKDETAIL";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override List<string> GetDeleteSql(UserModel user, T_OutStockDetailInfo model)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            strSql = "delete t_Outstockdetail where id = '" + model.ID + "'";

            lstSql.Add(strSql);

            return lstSql;
        }

        public bool SaveT_OutStockReviewDetailADF(UserModel userModel, List<T_OutStockDetailInfo> modelList,ref string strError) 
        {
            try
            {
                string strSql = string.Empty;
                List<string> lstSql = new List<string>();
                int ID = base.GetTableID("Seq_Pallet_Id");

                int detailID = 0;

                string VoucherNoID = base.GetTableID("Seq_Pallet_No").ToString();

                string VoucherNo = "P" + System.DateTime.Now.ToString("yyyyMMdd") + VoucherNoID.PadLeft(4, '0');

                strSql = string.Format("insert into t_Pallet(Id, Palletno, Creater, Createtime,Strongholdcode,Strongholdname,Companycode,Pallettype,Supplierno,Suppliername,ERPVOUCHERNO)" +
                                " values ('{0}','{1}','{2}',getdate(),'{3}','{4}','{5}','2','{6}','{7}','{8}')", ID, VoucherNo, userModel.UserNo, modelList[0].StrongHoldCode, modelList[0].StrongHoldName, modelList[0].CompanyCode, modelList[0].CustomerCode, modelList[0].CustomerName, modelList[0].ErpVoucherNo);

                lstSql.Add(strSql);

                foreach (var item in modelList)
                {
                    item.PalletNo = VoucherNo;

                    foreach (var itemSerial in item.lstStock)
                    {
                        detailID = base.GetTableID("SEQ_PALLET_DETAIL_ID");
                        strSql = string.Format("insert into t_Palletdetail(Id, Headerid, Palletno, Materialno, Materialdesc, Serialno,Creater," +
                        "Createtime,RowNo,VOUCHERNO,ERPVOUCHERNO,materialnoid,qty,BARCODE,StrongHoldCode,StrongHoldName,CompanyCode,pallettype," +
                        "batchno,rownodel,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,PRODUCTBATCH,EDATE,Supplierno,Suppliername)" +
                        "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',getdate(),'{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','2'," +
                        "'{16}','{17}',to_date('{18}','yyyy-mm-dd hh24:mi:ss'),'{19}',to_date('{20}','yyyy-mm-dd hh24:mi:ss'),'{21}',to_date('{22}','yyyy-mm-dd hh24:mi:ss'),'{23}','{24}')", detailID, ID, VoucherNo, itemSerial.MaterialNo, itemSerial.MaterialDesc, itemSerial.SerialNo, userModel.UserNo,
                        string.Empty, item.VoucherNo, item.ErpVoucherNo, item.MaterialNoID, itemSerial.Qty, itemSerial.Barcode,
                        itemSerial.StrongHoldCode, itemSerial.StrongHoldName, itemSerial.CompanyCode, itemSerial.BatchNo, string.Empty,
                        itemSerial.ProductDate, itemSerial.SupPrdBatch, itemSerial.SupPrdDate, string.Empty, itemSerial.EDate, itemSerial.SupCode, itemSerial.SupName);

                        lstSql.Add(strSql);                        
                    }
                }
                return base.SaveModelListBySqlToDB(lstSql, ref strError);
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        //提交复核扣库存SQL
        protected override List<string> GetSaveModelListSql(UserModel user, List<T_OutStockDetailInfo> modelList)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();
            string ToErpWarehouse = string.Empty;
            //int iVoucherType = modelList[0].VoucherType;

            //foreach (var item in modelList) 
            //{
            //    strSql = string.Format("update t_Outstockdetail a set  Oldoutstockqty = isnull( Oldoutstockqty,0)+{0}," +
            //            " Reviewuserno = '{1}', Reviewdate = getdate() where  id = '{2}'", item.ScanQty, user.UserNo, item.ID);
            //    lstSql.Add(strSql);

            //    strSql = string.Format("update t_Outstockdetail a set  linestatus =(case when isnull( Oldoutstockqty,0)< isnull( Outstockqty,0) and isnull( Oldoutstockqty,0)<>0 then 2 when isnull( Oldoutstockqty,0)  = isnull( Outstockqty,0)  then 3 end )," +
            //                            " Toerpareano ='{0}',Toerpwarehouse='{1}' where id = '{2}'", item.ToErpAreaNo, item.ToErpWarehouse, item.ID);
            //    lstSql.Add(strSql);

            //    foreach (var itemStock in item.lstStock) 
            //    {
            //        lstSql.Add(GetStockTransSql(user, itemStock));
            //        lstSql.Add(GetTaskTransSql(user, itemStock, item));
            //        //只有调拨单,并且是调入BH仓库的 才能在生产接受扫描
            //        //if (iVoucherType == 23 && !item.ToErpWarehouse.Contains("BH")) 
            //        //{
            //        //    lstSql.Add(GetStockPRDSql(user, itemStock));
            //        //}

            //        //if (item.ERPVoucherType == "BF2")//ERP单据类型BF2不能扣账，需要转移到报废仓库
            //        //{
            //        //    lstSql.Add("update t_stock a set  Warehouseid = '1080',  Houseid='1262', Areaid='32177', Taskdetailesid='' where  Serialno='" + itemStock.SerialNo + "'");
            //        //}
            //        //else if (iVoucherType == 23 && item.FromErpWarehouse.Contains("VMA") && item.ToErpWarehouse.Contains("AD"))//调拨单并且是从供应商调出的需要转移到调入对应的待收区
            //        //{
            //        //    strSql = "update t_stock a set  Warehouseid = (select id from t_Warehouse b where b.Warehouseno='"+item.ToErpWarehouse+"'), " +
            //        //            "  Houseid=( select id from v_house where housetype = 2 and Warehouseno = '" + item.ToErpWarehouse + "' )," +
            //        //            "  Areaid=(select id from v_area a where  warehouseno = '" + item.ToErpWarehouse + "' and  AREATYPE = '2'), Taskdetailesid='' where  Serialno='" + itemStock.SerialNo + "'";
            //        //    lstSql.Add(strSql);
            //        //}
            //        //else 
            //        //{
            //        //    lstSql.Add("delete t_Stock a where  Serialno = '" + itemStock.SerialNo + "'");//其他单据类型需要扣账
            //        //}

            //        //strSql = "delete t_Palletdetail where serialno = '" + itemStock.SerialNo + "' and isnull(Pallettype,1) = 1";
            //        //lstSql.Add(strSql);

            //        //strSql = "delete t_Pallet where palletno = '" + itemStock.PalletNo + "' and (select count(1) from t_Palletdetail where palletno = '" + itemStock.PalletNo + "')=0";
            //        //lstSql.Add(strSql);
            //    }

            //}

            //strSql = "update t_Outstock a set  Status = 3 where id = '" + modelList[0].HeaderID + "'";
            //lstSql.Add(strSql);

            //strSql = " update t_Outstock a set  Status = 4 where " +
            //          " id in(select b.Headerid from t_Outstockdetail b  group by b.Headerid having(max(isnull(linestatus,1)) = 3 and min(isnull(linestatus,1))=3) and b.Headerid = '" + modelList[0].HeaderID + "')" +
            //          "and id = '" + modelList[0].HeaderID + "'";

            //lstSql.Add(strSql);

            //strSql = "update t_task a set  Status = '4' where  Instockid = '" + modelList[0].HeaderID + "'";
            //lstSql.Add(strSql);
            foreach (var item in modelList) 
            {
                strSql = "update t_Outstockdetail  set  Postqty = '" + item.ScanQty + "', Postdate = getdate(), Poststatus = '" + item.LineStatus + "' where id = '" + item.ID + "'";
                lstSql.Add(strSql);
            }

            strSql = "insert into t_Stocktrans (Serialno,Materialno,Materialdesc,Warehouseid,Houseid,areaid,Qty,tmaterialno," +
                      "  pickareano,status,isdel,Creater,Createtime,Modifyer,Modifytime,Batchno,Oldstockid,Taskdetailesid," +
                      "  checkid,Transferdetailsid,Unit,receivestatus,Islimitstock,materialnoid,STRONGHOLDCODE,Strongholdname," +
                      "  COMPANYCODE,EDATE,BARCODE,EAN,HOUSEPROP,ISAMOUNT,BARCODETYPE) " +
                      "  select Serialno,Materialno,Materialdesc,Warehouseid,Houseid,areaid,Qty,tmaterialno," +
                      "  pickareano,status,isdel,Creater,Createtime,Modifyer,Modifytime,Batchno,Oldstockid,Taskdetailesid," +
                      "  checkid,Transferdetailsid,Unit,receivestatus,Islimitstock,materialnoid,STRONGHOLDCODE,Strongholdname," +
                      "  COMPANYCODE,EDATE,BARCODE,EAN,HOUSEPROP,ISAMOUNT,BARCODETYPE FROM t_stock a  WHERE EXISTS" +
                      "  ( select * from (select  a.Id from t_stock a left join t_Taskdetails b on  Taskdetailesid = b.Id " +
                      "  where b.ErpVoucherNo = '" + modelList[0].ErpVoucherNo + "') t where   a.Id = t.Id)";
            lstSql.Add(strSql);


            //ToErpWarehouse = modelList[0].ToErpWarehouse;
            //if (!string.IsNullOrEmpty(ToErpWarehouse)) 
            //{
            //    ToErpWarehouse = ToErpWarehouse.Substring(0, 3);
            //}

            //if ((modelList[0].FromErpWarehouse.Substring(0, 3) == "MS0" && modelList[0].ToErpWarehouse == "MS0") ||
            //    (modelList[0].FromErpWarehouse.Substring(0, 3) == "MS0" && ToErpWarehouse == "MS5") ||
            //    (modelList[0].FromErpWarehouse.Substring(0, 3) == "MS0" && ToErpWarehouse == "MS7") ||
            //    (modelList[0].FromErpWarehouse.Substring(0, 3) == "MS7" && ToErpWarehouse == "MS0") ||
            //    (modelList[0].FromErpWarehouse.Substring(0, 3) == "MS5" && ToErpWarehouse == "MS0") ||
            //    (modelList[0].FromErpWarehouse.Substring(0, 3) == "MS7" && ToErpWarehouse == "MS5") ||
            //    (modelList[0].FromErpWarehouse.Substring(0, 3) == "MS5" && ToErpWarehouse == "MS7") ||
            //    (modelList[0].FromErpWarehouse.Substring(0, 3) == "MS5" && ToErpWarehouse == "MS5") ||
            //    (modelList[0].FromErpWarehouse.Substring(0, 3) == "MS7" && ToErpWarehouse == "MS7"))
            //{
            //    strSql = "update t_stock a set  Areaid = (select id from v_area b where b.warehouseno = '"+modelList[0].ToErpWarehouse+"' and b.AREATYPE = '2')," +
            //           "  Houseid = (select c.HOUSEID from v_area c where c.warehouseno = '" + modelList[0].ToErpWarehouse + "' and c.AREATYPE = '2'), " +
            //           "   Warehouseid = (select id from t_Warehouse d where d.Warehouseno = '" + modelList[0].ToErpWarehouse + "'), TASKDETAILESID=0 where  EXISTS " +
            //           " ( select * from (select  Id from t_stock a left join t_Taskdetails b on  Taskdetailesid = b.Id " +
            //           " where b.Erpvoucherno = '" + modelList[0].ErpVoucherNo + "') t where   Id = t.Id)";
            //    lstSql.Add(strSql);
            //}
            //else 
            //{
            //    strSql = "delete FROM t_stock a  WHERE EXISTS" +
            //        "( select * from (select  Id from t_stock a left join t_Taskdetails b on  Taskdetailesid = b.Id " +
            //        "where b.Erpvoucherno = '" + modelList[0].ErpVoucherNo + "') t where   Id = t.Id)";

            //    lstSql.Add(strSql);
            //}

            //strSql = "delete FROM t_stock a  WHERE EXISTS" +
            //        "( select * from (select  Id from t_stock a left join t_Taskdetails b on  Taskdetailesid = b.Id " +
            //        "where b.Erpvoucherno = '" + modelList[0].ErpVoucherNo + "') t where   Id = t.Id)";

            strSql = "delete FROM t_stock   WHERE EXISTS " +
                    " ( select * from (select  t_stock.Id from t_stock  left join t_Taskdetails  on  t_stock.Taskdetailesid = t_Taskdetails.Id " +
                    "  where t_Taskdetails.ErpVoucherNo = '" + modelList[0].ErpVoucherNo + "') t where   t_stock.Id = t.Id)";

            lstSql.Add(strSql);

            strSql = "update t_Outstock  set  Status = 5,Postdate = getdate(),Postuser = '"+user.UserName+"' where id = '" + modelList[0].HeaderID + "'";
            lstSql.Add(strSql);

            //strSql = "update t_Pickcar  set taskno = ''" +
            //        " where exists( select 1 from (select taskno from t_task a where  Erpvoucherno = '" + modelList[0].ErpVoucherNo + "') b " +
            //        " where  Taskno=b.taskno)";
            
            //lstSql.Add(strSql);

            return lstSql;
        }


        private string GetStockTransSql(UserModel user, T_StockInfo model)
        {
            string strSql = "INSERT INTO t_Stocktrans ( Barcode, Serialno, Materialno, Materialdesc, Warehouseid, Houseid, Areaid, Qty,  Status, Isdel, Creater, Createtime, " +
                             "Batchno, Sn,  Oldstockid, Taskdetailesid, Checkid, Transferdetailsid,  Unit, Unitname, Palletno, Receivestatus,  Islimitstock,  " +
                             "Materialnoid, Strongholdcode, Strongholdname, Companycode, Edate, Supcode, Supname, Productdate, Supprdbatch, Supprddate, Isquality)" +
                            "SELECT  Barcode, Serialno, Materialno, Materialdesc, Warehouseid, Houseid, Areaid, Qty, " +
                            " Status, Isdel,'" + user.UserName + "', Createtime, Batchno, Sn, Oldstockid, Taskdetailesid, Checkid, Transferdetailsid, Unit, Unitname, Palletno, Receivestatus," +
                            " Islimitstock, Materialnoid, Strongholdcode, Strongholdname, Companycode, Edate, Supcode, Supname, Productdate, Supprdbatch," +
                            " Supprddate, Isquality FROM T_STOCK A WHERE  Serialno = '" + model.SerialNo + "'";
            return strSql;
        }

        private string GetStockPRDSql(UserModel user, T_StockInfo model)
        {
            string strSql = "INSERT INTO t_stockprd (Id, Barcode, Serialno, Materialno, Materialdesc, Warehouseid, Houseid, Areaid, Qty,  Status, Isdel, Creater, Createtime, " +
                             "Batchno, Sn,  Oldstockid, Taskdetailesid, Checkid, Transferdetailsid,  Unit, Unitname, Palletno, Receivestatus,  Islimitstock,  " +
                             "Materialnoid, Strongholdcode, Strongholdname, Companycode, Edate, Supcode, Supname, Productdate, Supprdbatch, Supprddate, Isquality)" +
                            "SELECT SEQ_STOCKPRD_ID.Nextval, Barcode, Serialno, Materialno, Materialdesc, Warehouseid, Houseid, Areaid, Qty, " +
                            " Status, Isdel,'" + user.UserName + "', Createtime, Batchno, Sn, Oldstockid, Taskdetailesid, Checkid, Transferdetailsid, Unit, Unitname, Palletno, Receivestatus," +
                            " Islimitstock, Materialnoid, Strongholdcode, Strongholdname, Companycode, Edate, Supcode, Supname, Productdate, Supprdbatch," +
                            " Supprddate, Isquality FROM T_STOCK A WHERE  Serialno = '" + model.SerialNo + "'";
            return strSql;
        }

        private string GetTaskTransSql(UserModel user, T_StockInfo model, T_OutStockTaskDetailsInfo detailModel)
        {
            string strSql = string.Empty;
            
                strSql = "insert into t_tasktrans( Serialno, Materialno, Materialdesc, Supcuscode, " +
               "Supcusname, Qty, Tasktype, Vouchertype, Creater, Createtime,TaskdetailsId, Unit, Unitname,partno,materialnoid,erpvoucherno,voucherno," +
               "Strongholdcode,Strongholdname,Companycode,Supprdbatch,Edate,taskno,status,batchno,barcode,houseprop,ean,Fromerpwarehouse,ISAMOUNT,ToWarehouseNo,ToWarehouseName,ToHouseNo,ToAreaNo,PalletNo)" +
               " values ('" + model.SerialNo + "'," +
               " '" + model.MaterialNo + "','" + model.MaterialDesc + "','" + detailModel.CustomerCode + "','" + detailModel.CustomerName + "','" + model.Qty + "','12'," +
               " (select vouchertype from t_outstock where id = '" + detailModel.HeaderID + "') ,'" + user.UserName + "',getdate(),'" + model.TaskDetailesID + "', " +
               "'" + detailModel.Unit + "','" + detailModel.UnitName + "','" + detailModel.PartNo + "','" + model.MaterialNoID + "','" + detailModel.ErpVoucherNo + "'," +
               "  '" + detailModel.VoucherNo + "','" + detailModel.StrongHoldCode + "','" + detailModel.StrongHoldName + "','" + detailModel.CompanyCode + "'," +
               "  '" + model.SupPrdBatch + "','" + model.StrEDate + "' ,'" + detailModel.TaskNo + "','" + model.Status + "','" + model.FromBatchNo + "','" + model.Barcode + "','" + model.HouseProp+ "','"+model.EAN+"','"+detailModel.FromErpWarehouse+"','"+model.IsAmount+"',"+
               " (select WAREHOUSENO from T_WAREHOUSE where id ='" + model.WareHouseID + "') ,"+
               " (select WAREHOUSENAME from T_WAREHOUSE where id ='" + model.WareHouseID + "'),"+
               " (select HOUSENO from T_HOUSE where id='" + model.HouseID + "'),"+
               " (select AREANO from T_AREA where id ='" + model.AreaID + "'),'"+model.PalletNo+"' ) ";

            
            return strSql;
        }


        /// <summary>
        /// 保存复核扫描的数据
        /// </summary>
        /// <param name="user"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public bool SaveReviewModelListSql(UserModel user, List<T_OutStockTaskDetailsInfo> modelList,ref string strError)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            int iVoucherType = modelList[0].VoucherType;

            foreach (var item in modelList)
            {
                strSql = string.Format("update t_Outstockdetail  set  Oldoutstockqty = isnull( Oldoutstockqty,0)+{0}, reviewqty=isnull( reviewqty,0)+{0}," +
                        " Reviewuserno = '{1}', Reviewdate = getdate() where  erpvoucherno='{2}' and  materialnoid='{3}' and  rowno = '" + item.RowNo + "'  ", item.ScanQty, user.UserNo, item.ErpVoucherNo, item.MaterialNoID);
                lstSql.Add(strSql);


                strSql = string.Format("update t_Outstockdetail  set  linestatus =(case when isnull( reviewqty,0)< isnull( Outstockqty,0) and isnull( reviewqty,0)<>0 then 3 when isnull( reviewqty,0)  = isnull( Outstockqty,0)  then 4 end ) " +
                                        "  where erpvoucherno='{0}' and materialnoid='{1}' and rowno = '{2}'  ", item.ErpVoucherNo, item.MaterialNoID, item.RowNo);
                lstSql.Add(strSql);

                strSql = "update t_Taskdetails  set  Reviewqty = isnull( Reviewqty,0) + '"+item.ScanQty+"', Reviewstatus = '"+item.ReviewStatus+"' , Reviewdate = getdate(), Reviewuser='"+user.UserNo+"' where id = '"+item.ID+"'";
                lstSql.Add(strSql);
                

                foreach (var itemStock in item.lstStockInfo)
                {
                    lstSql.Add(GetStockTransSql(user, itemStock));
                    lstSql.Add(GetTaskTransSql(user, itemStock, item));                    
                }
            }



            strSql = "update t_Outstock set status = 3 , Reviewuserno = '" + user.UserNo + "' where erpvoucherno = '" + modelList[0].ErpVoucherNo + "' ";
            lstSql.Add(strSql);

            strSql = " update t_Outstock  set  Status = 4 where " +
                      " ErpVoucherNo in(select b.ErpVoucherNo from t_Outstockdetail b  group by b.ErpVoucherNo having(max(isnull(linestatus,1)) = 4 and min(isnull(linestatus,1))=4) and b.ErpVoucherNo = '" + modelList[0].ErpVoucherNo + "')" +
                      "and ErpVoucherNo = '" + modelList[0].ErpVoucherNo + "'";

            lstSql.Add(strSql);

            strSql = "update t_task  set  Reviewstatus = '" + modelList[0].ReviewStatus + "' where id = '" + modelList[0].HeaderID + "'";
            lstSql.Add(strSql);

            return base.SaveModelListBySqlToDB(lstSql, ref strError);            
        }

        public bool SaveYiJianReviewModelListSql(UserModel user, List<T_OutStockTaskDetailsInfo> modelList, ref string strError)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            int iVoucherType = modelList[0].VoucherType;

            foreach (var item in modelList)
            {
                strSql = string.Format("update t_Outstockdetail  set  Oldoutstockqty = isnull( Oldoutstockqty,0)+{0}, reviewqty={0}," +
                        " Reviewuserno = '{1}', Reviewdate = getdate() where  erpvoucherno='{2}' and  materialnoid='{3}' and  rowno = '" + item.RowNo + "'  ", item.ScanQty, user.UserNo, item.ErpVoucherNo, item.MaterialNoID);
                lstSql.Add(strSql);


                strSql = string.Format("update t_Outstockdetail  set  linestatus =(case when isnull( reviewqty,0)< isnull( Outstockqty,0) and isnull( reviewqty,0)<>0 then 3 when isnull( reviewqty,0)  = isnull( Outstockqty,0)  then 4 end ) " +
                                        "  where erpvoucherno='{0}' and materialnoid='{1}' and rowno = '{2}'  ", item.ErpVoucherNo, item.MaterialNoID, item.RowNo);
                lstSql.Add(strSql);

                strSql = "update t_Taskdetails  set  Reviewqty =  '" + item.ScanQty + "', Reviewstatus = '" + item.ReviewStatus + "' , Reviewdate = getdate(), Reviewuser='" + user.UserNo + "' where id = '" + item.ID + "'";
                lstSql.Add(strSql);


                foreach (var itemStock in item.lstStockInfo)
                {
                    lstSql.Add(GetStockTransSql(user, itemStock));
                    lstSql.Add(GetTaskTransSql(user, itemStock, item));
                }
            }



            strSql = "update t_Outstock set status = 3 , Reviewuserno = '" + user.UserNo + "' where erpvoucherno = '" + modelList[0].ErpVoucherNo + "' ";
            lstSql.Add(strSql);

            strSql = " update t_Outstock  set  Status = 4 where " +
                      " ErpVoucherNo in(select b.ErpVoucherNo from t_Outstockdetail b  group by b.ErpVoucherNo having(max(isnull(linestatus,1)) = 4 and min(isnull(linestatus,1))=4) and b.ErpVoucherNo = '" + modelList[0].ErpVoucherNo + "')" +
                      "and ErpVoucherNo = '" + modelList[0].ErpVoucherNo + "'";

            lstSql.Add(strSql);

            strSql = "update t_task  set  Reviewstatus = '" + modelList[0].ReviewStatus + "' where id = '" + modelList[0].HeaderID + "'";
            lstSql.Add(strSql);

            return base.SaveModelListBySqlToDB(lstSql, ref strError);
        }


        public bool UpdateChangeMaterial(T_OutStockDetailInfo model,ref string strError) 
        {
            try
            {
                string strSql = string.Empty;
                List<string> lstSql = new List<string>();

                foreach (var item in model.lstStock) 
                {
                    strSql = "update t_stock a set  MaterialChangeID = '" + item.MaterialChangeID + "' where  Serialno = '"+item.SerialNo+"'";
                    lstSql.Add(strSql);
                }

                return base.UpdateModelListStatusBySql(lstSql,ref strError);
            }
            catch (Exception ex) 
            {
                strError = ex.Message;
                return false;
            }
        }

        public bool SaveT_ChangeMaterial(List<T_InStockDetailInfo> inStockList, List<T_OutStockDetailInfo> modelList, ref string strError) 
        {
            try
            {
                string strSql = string.Empty;
                List<string> lstSql = new List<string>();

                strSql = "update t_Instockdetail a set  Linestatus = 3 where  Headerid = '"+inStockList[0].HeaderID+"'";
                lstSql.Add(strSql);

                strSql = "update t_Instock a set  Status = 3 where id = '" + inStockList[0].HeaderID + "'";
                lstSql.Add(strSql);

                strSql = "update t_Outstockdetail a set  Linestatus = 3 where  Headerid = '" + modelList[0].HeaderID + "'";
                lstSql.Add(strSql);

                strSql = "update t_Outstock a set  Status = 3 where id = '" + modelList[0].HeaderID + "'";
                lstSql.Add(strSql);

                foreach (var item in modelList)
                {
                    foreach (var itemSerial in item.lstStock)
                    {
                        strSql = "update t_stock a set  Strongholdcode = '" + inStockList[0].StrongHoldCode + "',  Strongholdname = '" + inStockList[0].StrongHoldName + "', Materialchangeid = 0 where serialno = '" + itemSerial.SerialNo + "'";
                        lstSql.Add(strSql);

                        strSql = "update t_stock a set  Materialnoid = (select max(id) from t_Material where materialno = '" + item.MaterialNo + "' and strongholdcode = '" + inStockList[0].StrongHoldCode + "') " +
                                " where  Serialno = '" + itemSerial.SerialNo+ "'";
                        lstSql.Add(strSql);

                        strSql = "update t_Outbarcode a set  Strongholdcode = '" + inStockList[0].StrongHoldCode + "', Strongholdname = '" + inStockList[0].StrongHoldName + "' where  Serialno = '" + itemSerial.SerialNo + "'";
                        lstSql.Add(strSql);

                        strSql = "update t_Outbarcode a set  Materialnoid = (select max(id) from t_Material where materialno = '" + item.MaterialNo + "' and strongholdcode = '" + inStockList[0].StrongHoldCode + "') " +
                                " where  Serialno = '" + itemSerial.SerialNo + "'";
                        lstSql.Add(strSql);

                    }
                }

                return base.UpdateModelListStatusBySql(lstSql, ref strError);
            }
            catch (Exception ex) 
            {
                strError = ex.Message;
                return false;
            }
            
        }

        public string GetFHPalletNo(string strSerialNo)
        {
            string strSql = "select palletno from t_Palletdetail  where serialno = '" + strSerialNo + "' and isnull(pallettype,1) =2";
            return base.GetScalarBySql(strSql).ToDBString();
        }

        public string ScanOutStockReviewByCarNo(string CarNo) 
        {
            string strSql = "select b.Erpinvoucherno from t_Pickcar a left join t_task b on  Taskno = b.Taskno where  Carno='" + CarNo + "'";
            return base.GetScalarBySql(strSql).ToDBString();

        }

        public List<T_OutStockDetailInfo> GetModelListByHeaderIDForCar(int HeaderID)
        {
            return base.GetModelListByHeaderID(HeaderID);
        }

        #region PC复核页面打印装箱

        /// <summary>
        /// PC复核页面打印装箱
        /// </summary>
        /// <param name="ErpVoucherNo"></param>
        /// <param name="user"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public List<T_PalletDetailInfo> CreatePalletByTaskTrans(string ErpVoucherNo, UserModel user)
        {
            try
            {
                List<T_PalletDetailInfo> modelList = new List<T_PalletDetailInfo>();
                string strSql = "select  palletno, id, Erpvoucherno, Materialno, Materialnoid, Materialdesc, Ean, Qty, Strongholdcode, Strongholdname, Companycode, houseprop, Supcuscode, Supcusname from t_Tasktrans a where  Erpvoucherno = '" + ErpVoucherNo + "'  and  Creater='" + user.UserName + "' and  Tasktype = 12";
                using (IDataReader dr = dbFactory.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        T_PalletDetailInfo model = new T_PalletDetailInfo();
                        model.ID = Convert.ToInt32(dr["id"]);
                        model.ErpVoucherNo = dr["Erpvoucherno"].ToString();
                        model.MaterialNo = dr["Materialno"].ToString();
                        model.MaterialNoID = dr["MaterialNoID"].ToInt32();
                        model.MaterialDesc = dr["MaterialDesc"].ToDBString();
                        model.EAN = dr["EAN"].ToDBString();
                        model.Qty = dr["Qty"].ToDecimal();
                        model.StrongHoldCode = dr["StrongHoldCode"].ToDBString();
                        model.StrongHoldName = dr["StrongHoldName"].ToDBString();
                        model.CompanyCode = dr["CompanyCode"].ToDBString();
                        model.HouseProp = dr["HouseProp"].ToInt32();
                        model.SuppliernNo = dr["Supcuscode"].ToDBString();
                        model.SupplierName = dr["Supcusname"].ToDBString();
                        model.PalletNo = dr["PalletNo"].ToDBString();

                        modelList.Add(model);
                    }
                }

                return modelList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 验证批次条码是否已经复核扫描

        public int CheckBarCodeIsReview(string strSerialNo) 
        {
            try
            {
                string strSql = "select count(1) from t_Tasktrans where (serialno = '" + strSerialNo + "' OR PalletNo = '"+strSerialNo+"' ) AND tasktype = '12'";
                return base.GetScalarBySql(strSql).ToInt32();

            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
        #endregion

        #region 根据ERP订单号获取到的拣货明细构造出库表明细

        public List<T_OutStockDetailInfo> CreateOutStockDetailByTaskDetail(List<T_OutStockTaskDetailsInfo> modelList) 
        {
            List<T_OutStockDetailInfo> outStockDetailList = new List<T_OutStockDetailInfo>();
            foreach (var item in modelList) 
            {
                T_OutStockDetailInfo model = new T_OutStockDetailInfo();
                model.ID = item.ID;
                model.HeaderID = item.HeaderID;
                model.VoucherType = item.VoucherType;
                model.ErpVoucherNo = item.ErpVoucherNo;
                model.MaterialNo = item.MaterialNo;
                model.MaterialDesc = item.MaterialDesc;
                model.TaskQty = item.TaskQty.ToDecimal();
                model.OutStockQty = item.TaskQty.ToDecimal();
                model.StrLineStatus = item.StrLineStatus;
                model.HeaderID = item.HeaderID;
                model.TaskNo = item.TaskNo;
                model.VoucherNo = item.VoucherNo;
                model.RowNo = item.RowNo;
                model.UnShelveQty = item.UnShelveQty.ToDecimal();
                model.MaterialNoID = item.MaterialNoID;
                model.StrongHoldCode = item.StrongHoldCode;
                model.StrongHoldName = item.StrongHoldName;
                model.CompanyCode = item.CompanyCode;
                model.FromErpWarehouse = item.FromErpWarehouse;
                model.ReviewQty = item.ReviewQty.ToDecimal();
                model.ReviewStatus = item.ReviewStatus;
                model.ReviewUser = item.ReviewUser;
                model.ReviewDate = item.ReviewDate;
                model.CustomerCode = item.CustomerCode;
                model.CustomerName = item.CustomerName;
                model.Unit = item.Unit;
                model.RowNo = item.RowNo;
                model.HouseProp = item.HouseProp;
                model.StrHouseProp = item.StrHouseProp;
                model.UnReviewQty = item.UnReviewQty;
                model.PickQty = item.UnShelveQty.ToDecimal();
                model.CarNo = item.CarNo;
                model.EAN = item.EAN;
                model.ToErpWarehouse = item.ToErpWarehouse;
                model.BatchNo = item.FromBatchNo;
                model.FromErpWareHouseName = item.FromErpWareHouseName;
                model.ToErpWarehouseName = item.ToErpWareHouseName;

                outStockDetailList.Add(model);
            }
            return outStockDetailList;
        }

        #endregion

        #region 根据省份获取费用

        public bool GetCostByProvince(string Province,ref EmsCost_Model model,ref string strError) 
        {
            try
            {
                string sql = "select * from t_emscost where province like '%" + Province + "%'";
                using (IDataReader dr = dbFactory.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        model.Province = dr["Province"].ToString();
                        model.FirstHalf = dr["FirstHalf"].ToDecimal();
                        model.FirstOne = dr["FirstOne"].ToDecimal();
                        model.FirstOne1 = dr["FirstOne1"].ToDecimal();
                        model.ConWeight = dr["ConWeight"].ToDecimal();
                    }
                }
                return true;
            }
            catch (Exception ex) 
            {
                strError = ex.Message;
                return false;
            }
        }

        #endregion


        internal bool SaveTransportOutStock(List<T_OutStockInfo> modeliist, ref string strError)
        {
            try
            {
                
                string strSql = string.Empty;
                List<string> lstSql = new List<string>();

                
                for (int i = 0; i < modeliist.Count; i++)
                {


                    int ID = base.GetTableID("SEQ_TRANSPORTSUPPLIERDETAIL");
                    strSql = @"INSERT into T_TRANSPORTSUPPLIERDETAIL(ID,ERPVOUCHERNO,PLATENUMBER,FEIGHT,CREATETIME,isdel,palletno,boxcount,outboxcount,customername,voucherno,type,remark,remark1,remark2,remark3,creater)
                    VALUES(" + ID + ",'{0}', '{1}', '{2}',getdate(), '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}')";
                    strSql = string.Format(strSql, modeliist[i].ErpVoucherNo, '0', modeliist[i].Feight,
                       '1', string.Empty, modeliist[i].BoxCount, '0',
                       modeliist[i].CustomerName, modeliist[i].VoucherNo, '3', string.Empty, string.Empty, string.Empty, string.Empty, modeliist[i].Creater);
                    lstSql.Add(strSql);
                }
                return base.SaveModelListBySqlToDB(lstSql, ref strError);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
