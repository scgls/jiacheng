
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.User;
using BILBasic.DBA;
using BILBasic.Common;
using Oracle.ManagedDataAccess.Client;
using BILWeb.Area;
using System.Data;
using BILWeb.OutBarCode;

namespace BILWeb.YS
{
    public partial class T_YSDetail_DB : BILBasic.Basing.Factory.Base_DB<T_YSDetailInfo>
    {

        /// <summary>
        /// 添加t_instockdetail
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_YSDetailInfo t_instockdetail)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_YSDetailInfo model)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();
            return lstSql;
        }

        protected override List<string> GetSaveModelListSql(UserModel user, List<T_YSDetailInfo> modelList)
        {
            List<string> lstSql = new List<string>();


            return lstSql;
        }


        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_YSDetailInfo ToModel(IDataReader reader)
        {
            T_YSDetailInfo t_ysdetail = new T_YSDetailInfo();

            t_ysdetail.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_ysdetail.HeaderID = dbFactory.ToModelValue(reader, "HeaderID").ToInt32();
            t_ysdetail.RowNo = dbFactory.ToModelValue(reader, "ROWNO").ToDBString();
            t_ysdetail.MaterialNo = (string)dbFactory.ToModelValue(reader, "MATERIALNO");
            t_ysdetail.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MATERIALDESC");
            t_ysdetail.InStockQty = dbFactory.ToModelValue(reader, "INSTOCKQTY").ToDecimal();
            t_ysdetail.ReceiveQty = dbFactory.ToModelValue(reader, "RECEIVEQTY").ToDecimal();
            t_ysdetail.Unit = (string)dbFactory.ToModelValue(reader, "UNIT");
            t_ysdetail.StorageLoc = (string)dbFactory.ToModelValue(reader, "STORAGELOC");
            t_ysdetail.Plant = (string)dbFactory.ToModelValue(reader, "PLANT");
            t_ysdetail.PlantName = (string)dbFactory.ToModelValue(reader, "PLANTNAME");
            t_ysdetail.QualityQty = dbFactory.ToModelValue(reader, "QUALITYQTY").ToDecimal();
            t_ysdetail.UnQualityQty = dbFactory.ToModelValue(reader, "UNQUALITYQTY").ToDecimal();
            t_ysdetail.QualityType = (string)dbFactory.ToModelValue(reader, "QUALITYTYPE");
            t_ysdetail.QualityUserNo = (string)dbFactory.ToModelValue(reader, "QUALITYUSERNO");
            t_ysdetail.QualityDate = (DateTime?)dbFactory.ToModelValue(reader, "QUALITYDATE");
            t_ysdetail.UnitName = (string)dbFactory.ToModelValue(reader, "UNITNAME");
            t_ysdetail.LineStatus = dbFactory.ToModelValue(reader, "LINESTATUS").ToInt32();
            t_ysdetail.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_ysdetail.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_ysdetail.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_ysdetail.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_ysdetail.TimeStamp = (DateTime?)dbFactory.ToModelValue(reader, "TIMESTAMP");
            t_ysdetail.RemainQty = dbFactory.ToModelValue(reader, "RemainQty").ToDecimal();
            t_ysdetail.ArrivalDate = (DateTime?)dbFactory.ToModelValue(reader, "ArrivalDate");
            t_ysdetail.VoucherType = dbFactory.ToModelValue(reader, "VoucherType").ToInt32();
            t_ysdetail.SupplierNo = dbFactory.ToModelValue(reader, "SupplierNo").ToDBString();
            t_ysdetail.SupplierName = dbFactory.ToModelValue(reader, "SupplierName").ToDBString();
            t_ysdetail.SaleCode = dbFactory.ToModelValue(reader, "SaleCode").ToDBString();
            t_ysdetail.SaleName = dbFactory.ToModelValue(reader, "SaleName").ToDBString();
            t_ysdetail.ErpVoucherNo = dbFactory.ToModelValue(reader, "ErpVoucherNo").ToDBString();

            t_ysdetail.MaterialNoID = dbFactory.ToModelValue(reader, "MateiralNoID").ToInt32();
            t_ysdetail.PartNo = dbFactory.ToModelValue(reader, "PartNo").ToDBString();

            t_ysdetail.VoucherNo = dbFactory.ToModelValue(reader, "VoucherNo").ToDBString();
            t_ysdetail.IsSerial = dbFactory.ToModelValue(reader, "IsSerial").ToInt32();


            t_ysdetail.StrongHoldCode = dbFactory.ToModelValue(reader, "StrongHoldCode").ToDBString();
            t_ysdetail.StrongHoldName = dbFactory.ToModelValue(reader, "StrongHoldName").ToDBString();
            t_ysdetail.CompanyCode = dbFactory.ToModelValue(reader, "CompanyCode").ToDBString();
            t_ysdetail.RowNoDel = dbFactory.ToModelValue(reader, "RowNoDel").ToDBString();
            t_ysdetail.DeliverySup = dbFactory.ToModelValue(reader, "DeliverySup").ToDBString();
            t_ysdetail.ShipmentDate = dbFactory.ToModelValue(reader, "ShipmentDate").ToDateTime();
            t_ysdetail.ArrStockDate = dbFactory.ToModelValue(reader, "ArrStockDate").ToDateTime();
            t_ysdetail.ErpLineStatus = dbFactory.ToModelValue(reader, "ErpLineStatus").ToInt32();
            t_ysdetail.ERPNote = dbFactory.ToModelValue(reader, "ERPNote").ToDBString();

            t_ysdetail.FromBatchNo = dbFactory.ToModelValue(reader, "FromBatchNo").ToDBString();
            t_ysdetail.FromErpAreaNo = dbFactory.ToModelValue(reader, "FromErpAreaNo").ToDBString();
            t_ysdetail.FromErpWarehouse = dbFactory.ToModelValue(reader, "FromErpWarehouse").ToDBString();
            t_ysdetail.ToBatchNo = dbFactory.ToModelValue(reader, "ToBatchNo").ToDBString();
            t_ysdetail.ToErpAreaNo = dbFactory.ToModelValue(reader, "ToErpAreaNo").ToDBString();
            t_ysdetail.ToErpWarehouse = dbFactory.ToModelValue(reader, "ToErpWarehouse").ToDBString();
            t_ysdetail.IsSpcBatch = dbFactory.ToModelValue(reader, "IsSpcBatch").ToDBString();
            t_ysdetail.ADVRECEIVEQTY = dbFactory.ToModelValue(reader, "ADVRECEIVEQTY").ToInt32();

            t_ysdetail.QcCode = dbFactory.ToModelValue(reader, "QcCode").ToDBString();
            t_ysdetail.QcDesc = dbFactory.ToModelValue(reader, "QcDesc").ToDBString();
            t_ysdetail.ERPVoucherType = dbFactory.ToModelValue(reader, "ErpVoucherType").ToDBString();
            t_ysdetail.EDate = dbFactory.ToModelValue(reader, "EDate").ToDateTime();
            t_ysdetail.InvoiceNo = dbFactory.ToModelValue(reader, "InvoiceNo").ToDBString();

            t_ysdetail.TracNo = dbFactory.ToModelValue(reader, "TracNo").ToDBString();
            t_ysdetail.ProjectNo = dbFactory.ToModelValue(reader, "ProjectNo").ToDBString();
            t_ysdetail.iarrsid = dbFactory.ToModelValue(reader, "iarrsid").ToDBString();
            return t_ysdetail;
        }

        protected override string GetViewName()
        {
            return "V_YSDETAIL";
        }

        protected override string GetTableName()
        {
            return "T_YSDETAIL";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override List<string> GetDeleteSql(UserModel user, T_YSDetailInfo model)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            strSql = "delete t_ysdetail where id = '" + model.ID + "'";

            lstSql.Add(strSql);

            return lstSql;
        }



        /// <summary>
        /// 导入EXCEL，获取单据明细
        /// </summary>
        /// <param name="VoucherNo"></param>
        /// <returns></returns>
        public List<T_YSDetailInfo> GetDetailByVoucherNo(string VoucherNo, int VoucherType)
        {
            string strSql = "select * from v_ysdetail a where  Erpvoucherno = '" + VoucherNo + "' and vouchertype='" + VoucherType + "'";

            return base.GetModelListBySql(strSql);
        }

        protected override string GetFilterSql(UserModel user, T_YSDetailInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";
            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " erpvoucherno like '%" + model.ErpVoucherNo + "%' ";
            }
            if (!string.IsNullOrEmpty(model.MaterialNo))
            {
                strSql += strAnd;
                strSql += " MaterialNo like '%" + model.MaterialNo + "%' ";
            }
            strSql += strAnd;
            strSql += " VoucherType != 39 ";

            return strSql; //+ " order by id desc";
        }

        protected override string GetOrderNoFieldName()
        {
            return "taskno";
        }


        public List<string> GetSaveSqlAll(UserModel user, List<T_YSDetailInfo> models)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            for (int i = 0; i < models.Count; i++)
            {
               string strSql11 = "update t_YSDetail  set   Receiveqty = (isnull( Receiveqty,0) + '" + models[i].ScanQty + "') ," +
        " OPERATORUSERNO = '" + user.UserNo + "', Operatordatetime = getdate()  , materialnoid = '" + models[i].MaterialNoID + "'  where id  ='" + models[i].ID + "'";
                lstSql.Add(strSql11);// remainqty = (case when (isnull( remainqty,0) - '" + item.ScanQty + "') <= 0 then 0 else isnull( remainqty,0) - '" + item.ScanQty + "' end),

                string strSql21 = "update t_YSDetail  set  Linestatus =  (case when isnull( Receiveqty,0)< isnull( Instockqty,0) and isnull( Receiveqty,0)<>0 then 2" +
                          "when isnull( Receiveqty,0) >= isnull( Instockqty,0)  then 3 end ) where id = '" + models[i].ID + "'";
                lstSql.Add(strSql21);


                //for (int j = 0; j < models[i].lstBarCode.Count; j++)
                //{
                    //新增
                    if (models[i].RemainQty>0)
                    {
                        foreach (var itemBarCode in models[i].lstBarCode)
                        {
                            string strSql1 = "insert into t_stock(serialno,Materialno,materialdesc,qty,status,isdel,Creater,Createtime,batchno,unit,unitname,Palletno," +
                                      "islimitstock,materialnoid,warehouseid,houseid,areaid,Receivestatus,barcode,STRONGHOLDCODE,STRONGHOLDNAME,COMPANYCODE,EDATE,SUPCODE,SUPNAME," +
                                     "SUPPRDBATCH,Isquality,Stocktype,ean,BARCODETYPE,projectNo,TracNo)" +
                                     "values ('" + itemBarCode.SerialNo + "','" + itemBarCode.MaterialNo + "','" + itemBarCode.MaterialDesc + "','" + itemBarCode.Qty + "','0','1'" +
                                     ",'" + user.UserNo + "',getdate(),'" + itemBarCode.BatchNo + "','" + itemBarCode.Unit + "',''" +
                                     ",'','1','" + itemBarCode.MaterialNoID + "'" +
                                     ", '" + user.WarehouseID + "','" + user.ReceiveHouseID + "','" + user.ReceiveAreaID + "','1','" + itemBarCode.BarCode + "','" + itemBarCode.StrongHoldCode + "', " +
                                     "  '" + itemBarCode.StrongHoldName + "','','','',''," +
                                     "'','3' ,'1','','" + itemBarCode.BarcodeType + "' ,'" + (itemBarCode.ProjectNo == null ? "" : itemBarCode.ProjectNo) + "','" + (itemBarCode.TracNo == null ? "" : itemBarCode.TracNo) + "')";

                            lstSql.Add(strSql1);

                            int TaskTransID = base.GetTableIDBySqlServerTaskTrans("t_tasktrans");
                            string strSql2 = "SET IDENTITY_INSERT t_tasktrans on ;insert into t_tasktrans(id, Serialno,towarehouseid,Tohouseid, Toareaid, Materialno, Materialdesc, Supcuscode, " +
                                        "Supcusname, Qty, Tasktype, Vouchertype, Creater, Createtime,TaskdetailsId, Unit, Unitname,materialnoid," +
                                        "erpvoucherno,voucherno,barcode,STRONGHOLDCODE,STRONGHOLDNAME,COMPANYCODE,SUPPRDBATCH,EDATE,TASKNO,batchno,ToWarehouseNo,ToHouseNo,ToAreaNo,ToWarehouseName)" +
                                    " values ('" + TaskTransID + "','" + itemBarCode.SerialNo + "','" + user.WarehouseID + "','" + user.ReceiveHouseID + "'," +
                                    "'" + user.ReceiveAreaID + "','" + itemBarCode.MaterialNo + "','" + itemBarCode.MaterialDesc + "','',''," +
                                    " '" + itemBarCode.Qty + "','205',45 ,'" + user.UserName + "',getdate(),'" + models[i].ID + "'," +
                                    "'" + itemBarCode.Unit + "','','" + itemBarCode.MaterialNoID + "','" + models[i].ErpVoucherNo + "','" + models[i].VoucherNo + "','" + itemBarCode.BarCode + "'," +
                                    "'" + itemBarCode.StrongHoldCode + "','" + itemBarCode.StrongHoldName + "','" + models[i].CompanyCode + "','" + itemBarCode.SupPrdBatch + "'" +
                                    " ,null,'','" + itemBarCode.BatchNo + "','" + user.ReceiveWareHouseNo + "','" + user.ReceiveHouseNo + "','" + user.ReceiveAreaNo + "','" + user.ReceiveWareHouseName + "') SET IDENTITY_INSERT t_tasktrans off";
                            lstSql.Add(strSql2);

                        }
                    }
                    else
                    {
                        //删除
                        foreach (var itemBarCode in models[i].lstBarCode)
                        {
                            lstSql.Add("delete from t_stock  where Serialno = '" + itemBarCode.SerialNo + "'");
                            string strSql3 = "delete t_Palletdetail where BARCODE = '" + itemBarCode.BarCode + "'";
                            lstSql.Add(strSql3);
                            string strSql4 = "delete t_Pallet where palletno = '" + itemBarCode.PalletNo + "' and (select count(1) from t_Palletdetail where palletno = '" + itemBarCode.PalletNo + "')=0";
                            lstSql.Add(strSql4);

                            lstSql.Add(GetTaskTransSqlList(user, itemBarCode, models[i]));
                        }

                       
                    }
                //}
            }

            string strSql32 = "update t_YS  set  Status = 2 where id = '" + models[0].HeaderID + "'";
            lstSql.Add(strSql32);

            string strSql34 = " update t_YS  set  Status = 3 where " +
                      " id in(select b.Headerid from t_YSDetail b  group by b.Headerid having(max(isnull(linestatus,1)) = 3 and min(isnull(linestatus,1))=3) and b.Headerid = '" + models[0].HeaderID + "')" +
                      "and id = '" + models[0].HeaderID + "'";
            lstSql.Add(strSql34);

            return lstSql;
        }



        private string GetTaskTransSqlList(UserModel user, T_OutBarCodeInfo model, T_YSDetailInfo detailModel)
        {
            int id = base.GetTableIDBySqlServerTaskTrans("t_tasktrans");
            string strSql = "SET IDENTITY_INSERT t_tasktrans on ;insert into t_tasktrans(id, Serialno, Materialno, Materialdesc, Supcuscode, " +
            "Supcusname, Qty, Tasktype, Vouchertype, Creater, Createtime,TaskdetailsId, Unit, Unitname,partno,materialnoid,erpvoucherno,voucherno," +
            "Strongholdcode,Strongholdname,Companycode,Supprdbatch,taskno,batchno,barcode,status,materialdoc,houseprop,ean,FromWarehouseNo,FromWarehouseName,FromHouseNo,FromAreaNo,ToWarehouseNo,ToWarehouseName,ToHouseNo,ToAreaNo,PalletNo)" +
            " values ('" + id + "' , '" + model.SerialNo + "'," +
            " '" + model.MaterialNo + "','" + model.MaterialDesc + "','','','" + model.Qty + "','206'," +
            " 45,'" + user.UserName + "',getdate(),'" + model.ID + "', " +
            "'" + detailModel.Unit + "','" + detailModel.UnitName + "','','" + detailModel.MaterialNoID + "','" + detailModel.ErpVoucherNo + "'," +
            "  '" + detailModel.VoucherNo + "','" + detailModel.StrongHoldCode + "','" + detailModel.StrongHoldName + "','" + detailModel.CompanyCode + "'," +
            "  '" + model.SupPrdBatch + "','" + detailModel.TaskNo + "'," +
            " '" + model.BatchNo + "' ,'" + model.BarCode + "','" + model.Status + "','" + detailModel.MaterialDoc + "','',''," +
            "  (select WAREHOUSENO from T_WAREHOUSE where id ='" + model.WareHouseID + "')," +
            " (select WAREHOUSENAME from T_WAREHOUSE where id ='" + model.WareHouseID + "'), " +
            " (select HOUSENO from T_HOUSE where id='" + model.HouseID + "')," +
            " (select AREANO from T_AREA where id ='" + model.AreaID + "')," +
            " '',''," +
            " ''," +
            " '','" + model.PalletNo + "' ) SET IDENTITY_INSERT t_tasktrans off ";//,(select  ID from v_Area a where  warehouseno = '" + model.ToErpWarehouse + "' and  AREANO = '" + model.ToErpAreaNo + "'),'" + model.AreaID + "','" + model.WareHouseID + "','" + model.HouseID + "'
            
            return strSql;
        }

        public bool YSPost(UserModel user, List<T_YSDetailInfo> models, ref string strError)
        {
            try
            {
                List<string> lstSql = GetSaveSqlAll(user, models);
                return base.SaveModelListBySqlToDB(lstSql, ref strError);
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

       
    }
}
