
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

namespace BILWeb.InStock
{
    public partial class T_InStockDetail_DB : BILBasic.Basing.Factory.Base_DB<T_InStockDetailInfo>
    {

        /// <summary>
        /// 添加t_instockdetail
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_InStockDetailInfo t_instockdetail)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref  T_InStockDetailInfo model)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            if (model.ID <= 0)
            {
                int detailID = base.GetTableID("Seq_Instockdetail_Id");
                model.ID = detailID;
                strSql = string.Format("insert into t_Instockdetail (Id, Headerid, Rowno, Materialno, Materialdesc, Instockqty, Unit, Unitname, Linestatus, Creater, Createtime,  Voucherno, Erpvoucherno, Isdel,Arrivaldate,partno,remainqty)" +
                    " values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','1','{8}',getdate(),'{9}','{10}','1','{11}','{12}','{13}')",
                    detailID, model.HeaderID, model.RowNo, model.MaterialNo, model.MaterialDesc, model.InStockQty, model.Unit, model.UnitName, user.UserNo, model.VoucherNo, model.ErpVoucherNo, model.ArrivalDate, model.PartNo, model.InStockQty);

                lstSql.Add(strSql);
            }
            else
            {
                strSql = "update t_Instockdetail a set  Materialno = '" + model.MaterialNo + "', Partno = '" + model.PartNo + "', Instockqty = '" + model.InStockQty + "', Materialdesc='" + model.MaterialDesc + "', Arrivaldate = '" + model.ArrivalDate + "', remainqty = '" + model.InStockQty + "'" +
                        "where  Id = '" + model.ID + "'";
                lstSql.Add(strSql);
            }
            return lstSql;
        }

        protected override List<string> GetSaveModelListSql(UserModel user, List<T_InStockDetailInfo> modelList)
        {
            string strSql1 = string.Empty;
            string strSql2 = string.Empty;
            string strSql3 = string.Empty;
            string strSql4 = string.Empty;
            string strSql5 = string.Empty;
            string strSql6 = string.Empty;
            string strSql7 = string.Empty;
            string strSql8 = string.Empty;
            string strSql9 = string.Empty;
            string strSql10 = string.Empty;
            string TaskNo = string.Empty;
            int TaskTransID = 0;

            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }

            if (modelList[0].VoucherType == 35)
            {
                return null;
            }

            int taskid = base.GetTableIDBySqlServer("t_task");//GetTableID("seq_task_id");

            List<string> lstSql = new List<string>();

            if (modelList.Count() > 0)
            {
                //string TaskNoID = base.GetTableID("seq_task_no").ToString();

                //TaskNo = "T" + System.DateTime.Now.ToString("yyyyMMdd") + TaskNoID.PadLeft(4, '0');

                //string TaskNoID = base.GetTableIDBySqlServer("t_task").ToString();

                TaskNo = base.GetNewOrderNo("D", "t_task", "");

                modelList.ForEach(t => t.TaskNo = TaskNo);
            }
            else
            {
                modelList.ForEach(t => t.TaskNo = "");
            }

            foreach (var item in modelList)
            {
                strSql1 = "update t_Instockdetail  set   Receiveqty = (isnull( Receiveqty,0) + '" + item.ScanQty + "') ," +
                        " OPERATORUSERNO = '" + user.UserNo + "', Operatordatetime = getdate()  , materialnoid = '" + item.MaterialNoID + "'  where id  ='" + item.ID + "'";
                lstSql.Add(strSql1);// remainqty = (case when (isnull( remainqty,0) - '" + item.ScanQty + "') <= 0 then 0 else isnull( remainqty,0) - '" + item.ScanQty + "' end),

                strSql2 = "update t_Instockdetail  set  Linestatus =  (case when isnull( Receiveqty,0)< isnull( Instockqty,0) and isnull( Receiveqty,0)<>0 then 2" +
                          "when isnull( Receiveqty,0) >= isnull( Instockqty,0)  then 3 end ) where id = '" + item.ID + "'";
                lstSql.Add(strSql2);

                foreach (var itemBarCode in item.lstBarCode)
                {
                    item.IsQuality = 3;//库存状态改为合格
                    strSql8 = "insert into t_stock(serialno,Materialno,materialdesc,qty,status,isdel,Creater,Createtime,batchno,unit,unitname,Palletno," +
                             "islimitstock,materialnoid,warehouseid,houseid,areaid,Receivestatus,barcode,STRONGHOLDCODE,STRONGHOLDNAME,COMPANYCODE,EDATE,SUPCODE,SUPNAME," +
                            "SUPPRDBATCH,Isquality,Stocktype,ean,BARCODETYPE,projectNo,TracNo)" +
                            "values ('" + itemBarCode.SerialNo + "','" + itemBarCode.MaterialNo + "','" + itemBarCode.MaterialDesc + "','" + itemBarCode.Qty + "','" + item.IsQuality + "','1'" +
                            ",'" + user.UserNo + "',getdate(),'" + itemBarCode.BatchNo + "','" + item.Unit + "','" + item.UnitName + "'" +
                            ",(select palletno from t_Palletdetail where serialno = '" + itemBarCode.SerialNo + "'),'1','" + itemBarCode.MaterialNoID + "'" +
                            ", '" + user.WarehouseID + "','" + user.ReceiveHouseID + "','" + user.ReceiveAreaID + "','1','" + itemBarCode.BarCode + "','" + item.StrongHoldCode + "', " +
                            "  '" + itemBarCode.StrongHoldName + "','" + itemBarCode.CompanyCode + "','" + itemBarCode.EDate + "','" + item.SupplierNo + "','" + item.SupplierName + "'," +
                            "'" + itemBarCode.SupPrdBatch + "','3' ,'1','" + itemBarCode.EAN + "','"+itemBarCode.BarcodeType+"','"+ (itemBarCode.ProjectNo==null?"": itemBarCode.ProjectNo) + "','" + (itemBarCode.TracNo==null?"": itemBarCode.TracNo) + "' )";

                    lstSql.Add(strSql8);

                    TaskTransID = base.GetTableIDBySqlServerTaskTrans("t_tasktrans");

                    strSql10 = "SET IDENTITY_INSERT t_tasktrans on ;insert into t_tasktrans(id, Serialno,towarehouseid,Tohouseid, Toareaid, Materialno, Materialdesc, Supcuscode, " +
                                "Supcusname, Qty, Tasktype, Vouchertype, Creater, Createtime,TaskdetailsId, Unit, Unitname,materialnoid," +
                                "erpvoucherno,voucherno,barcode,STRONGHOLDCODE,STRONGHOLDNAME,COMPANYCODE,SUPPRDBATCH,EDATE,TASKNO,batchno,ToWarehouseNo,ToHouseNo,ToAreaNo,ToWarehouseName)" +
                            " values ('" + TaskTransID + "','" + itemBarCode.SerialNo + "','" + user.WarehouseID + "','" + user.ReceiveHouseID + "'," +
                            "'" + user.ReceiveAreaID + "','" + itemBarCode.MaterialNo + "','" + itemBarCode.MaterialDesc + "','" + item.SupplierNo + "','" + item.SupplierName + "'," +
                            " '" + itemBarCode.Qty + "','4',(select vouchertype from t_Instock where voucherno = '" + item.VoucherNo + "') ,'" + user.UserName + "',getdate(),'" + item.ID + "'," +
                            "'" + item.Unit + "','" + item.UnitName + "','" + itemBarCode.MaterialNoID + "','" + item.ErpVoucherNo + "','" + item.VoucherNo + "','" + itemBarCode.BarCode + "'," +
                            "'" + item.StrongHoldCode + "','" + item.StrongHoldName + "','" + item.CompanyCode + "','" + itemBarCode.SupPrdBatch + "'" +
                            " ,'" + itemBarCode.EDate + "','" + TaskNo + "','" + itemBarCode.BatchNo + "','" + user.ReceiveWareHouseNo + "','" + user.ReceiveHouseNo + "','" + user.ReceiveAreaNo + "','" + user.ReceiveWareHouseName + "') SET IDENTITY_INSERT t_tasktrans off";
                    lstSql.Add(strSql10);

                }

            }

            strSql3 = "update t_Instock  set  Status = 2 where id = '" + modelList[0].HeaderID + "'";
            lstSql.Add(strSql3);

            strSql4 = " update t_Instock  set  Status = 3 where " +
                      " id in(select b.Headerid from t_Instockdetail b  group by b.Headerid having(max(isnull(linestatus,1)) = 3 and min(isnull(linestatus,1))=3) and b.Headerid = '" + modelList[0].HeaderID + "')" +
                      "and id = '" + modelList[0].HeaderID + "'";
            lstSql.Add(strSql4);


            List<T_InStockDetailInfo> NewModelList = GroupInstockDetailList(modelList);

            //ymh 查询单据类型
            T_InStock_Func func = new T_InStock_Func();
            T_InStockInfo InStockInfoModel = new T_InStockInfo() { ID = modelList[0].HeaderID };
            string strmsg = "";
            func.GetModelByID(ref InStockInfoModel,ref strmsg);

            //是否生成上架任务的配置
            if (!(user.ISVWAREHOUSE == 0 && InStockInfoModel.VoucherType != 39)) {
                //汇总生成上架任务不汇总收货数据
                foreach (var item in NewModelList)
                {
                    //taskqty》QualityQty
                    strSql6 = "insert into t_Taskdetails (headerid,Materialno,materialdesc,QualityQty,Remainqty,LineStatus,Creater,Createtime,Unit,Unitname,erpvoucherno,materialnoid,toareano,voucherno," +
                    "STRONGHOLDCODE,STRONGHOLDNAME,COMPANYCODE,batchno,Productbatch,Supprdbatch,Frombatchno,Fromerpareano,Fromerpwarehouse,isdel,iarrsid)" +
                       "values('" + taskid + "','" + item.MaterialNo + "','" + item.MaterialDesc + "','" + item.ScanQty + "','" + item.ScanQty + "'," +
                       "'1','" + user.UserNo + "',getdate(),'" + item.Unit + "','" + item.UnitName + "','" + item.ErpVoucherNo + "','" + item.MaterialNoID + "','" + user.ReceiveAreaID + "','" + item.VoucherNo + "'," +
                       "'" + item.StrongHoldCode + "','" + item.StrongHoldName + "','" + item.CompanyCode + "','" + item.BatchNo + "'," +
                    "'" + item.ProductBatch + "','" + item.SupPrdBatch + "'," +
                    "'" + item.BatchNo + "','" + user.ReceiveAreaNo + "','" + user.ReceiveWareHouseNo + "','1','" + modelList[0].iarrsid + "')";

                    lstSql.Add(strSql6);
                }

                if (NewModelList != null && NewModelList.Count() > 0)
                {
                    //ymh到货单特殊处理，其他单据不过帐：Vouchertype
                    strSql5 = "set IDENTITY_INSERT t_task on;insert into t_task (id,Vouchertype,tasktype,Taskno,Supcusname,status,Taskissued,Receiveuserno,Createtime,supcuscode,Creater,InStockID," +
                              "erpvoucherno,plant,plantname,movetype,Taskissueduser,voucherno,STRONGHOLDCODE,STRONGHOLDNAME,COMPANYCODE,ERPCREATER,VOUDATE,VOUUSER,ERPSTATUS,ERPNOTE,erpinvoucherno,WAREHOUSEID,erpvouchertype,isdel)" +
                              "select  '" + taskid + "', case Vouchertype when 39 then 39 else 888 end as Vouchertype,'1','" + TaskNo + "', Suppliername , '1',getdate(),'" + user.UserNo + "',getdate(), Supplierno,'" + user.UserNo + "', Id," +
                              " Erpvoucherno, Plant, Plantname, Movetype,'" + user.UserNo + "',voucherno,STRONGHOLDCODE,STRONGHOLDNAME,COMPANYCODE,ERPCREATER,VOUDATE,VOUUSER,ERPSTATUS,ERPNOTE,'" + NewModelList[0].MaterialDoc + "'" +
                              "  ,'" + user.WarehouseID + "','" + modelList[0].ERPVoucherType + "','1' from t_Instock a where id = '" + modelList[0].HeaderID + "' set IDENTITY_INSERT t_task off;";
                    lstSql.Add(strSql5);
                }
            }
            return lstSql;
        }


        /// <summary>
        /// 汇总收货数据
        /// </summary>
        /// <param name="modelList"></param>
        private List<T_InStockDetailInfo> GroupInstockDetailList(List<T_InStockDetailInfo> modelList)
        {
            var newModelList = from t in modelList
                               group t by new { t1 = t.MaterialNo} into m//, t2 = t.BatchNo 
                               select new T_InStockDetailInfo
                               {
                                   MaterialNo = m.Key.t1,
                                   ScanQty = m.Sum(item => item.ScanQty),
                                   MaterialDesc = m.FirstOrDefault().MaterialDesc,
                                   Unit = m.FirstOrDefault().Unit,
                                   UnitName = m.FirstOrDefault().UnitName,
                                   ErpVoucherNo = m.FirstOrDefault().ErpVoucherNo,
                                   MaterialNoID = m.FirstOrDefault().MaterialNoID,
                                   VoucherNo = m.FirstOrDefault().VoucherNo,
                                   StrongHoldCode = m.FirstOrDefault().StrongHoldCode,
                                   StrongHoldName = m.FirstOrDefault().StrongHoldName,
                                   CompanyCode = m.FirstOrDefault().CompanyCode,
                                   BatchNo = m.FirstOrDefault().BatchNo,
                                   ProductBatch = m.FirstOrDefault().ProductBatch,
                                   ProductDate = m.FirstOrDefault().ProductDate,
                                   SupPrdBatch = m.FirstOrDefault().SupPrdBatch,
                                   SupPrdDate = m.FirstOrDefault().SupPrdDate,
                                   MaterialDoc = m.FirstOrDefault().MaterialDoc

                               };

            return newModelList.ToList();
        }

        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_InStockDetailInfo ToModel(IDataReader reader)
        {
            T_InStockDetailInfo t_instockdetail = new T_InStockDetailInfo();

            t_instockdetail.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_instockdetail.HeaderID = dbFactory.ToModelValue(reader, "HeaderID").ToInt32();
            t_instockdetail.RowNo = dbFactory.ToModelValue(reader, "ROWNO").ToDBString();
            t_instockdetail.MaterialNo = (string)dbFactory.ToModelValue(reader, "MATERIALNO");
            t_instockdetail.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MATERIALDESC");
            t_instockdetail.InStockQty = dbFactory.ToModelValue(reader, "INSTOCKQTY").ToDecimal();
            t_instockdetail.ReceiveQty = dbFactory.ToModelValue(reader, "RECEIVEQTY").ToDecimal();
            t_instockdetail.Unit = (string)dbFactory.ToModelValue(reader, "UNIT");
            t_instockdetail.StorageLoc = (string)dbFactory.ToModelValue(reader, "STORAGELOC");
            t_instockdetail.Plant = (string)dbFactory.ToModelValue(reader, "PLANT");
            t_instockdetail.PlantName = (string)dbFactory.ToModelValue(reader, "PLANTNAME");
            t_instockdetail.QualityQty = dbFactory.ToModelValue(reader, "QUALITYQTY").ToDecimal();
            t_instockdetail.UnQualityQty = dbFactory.ToModelValue(reader, "UNQUALITYQTY").ToDecimal();
            t_instockdetail.QualityType = (string)dbFactory.ToModelValue(reader, "QUALITYTYPE");
            t_instockdetail.QualityUserNo = (string)dbFactory.ToModelValue(reader, "QUALITYUSERNO");
            t_instockdetail.QualityDate = (DateTime?)dbFactory.ToModelValue(reader, "QUALITYDATE");
            t_instockdetail.UnitName = (string)dbFactory.ToModelValue(reader, "UNITNAME");
            t_instockdetail.LineStatus = dbFactory.ToModelValue(reader, "LINESTATUS").ToInt32();
            t_instockdetail.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_instockdetail.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_instockdetail.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_instockdetail.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_instockdetail.TimeStamp = (DateTime?)dbFactory.ToModelValue(reader, "TIMESTAMP");
            t_instockdetail.RemainQty = dbFactory.ToModelValue(reader, "RemainQty").ToDecimal();
            t_instockdetail.ArrivalDate = (DateTime?)dbFactory.ToModelValue(reader, "ArrivalDate");
            t_instockdetail.VoucherType = dbFactory.ToModelValue(reader, "VoucherType").ToInt32();
            t_instockdetail.SupplierNo = dbFactory.ToModelValue(reader, "SupplierNo").ToDBString();
            t_instockdetail.SupplierName = dbFactory.ToModelValue(reader, "SupplierName").ToDBString();
            t_instockdetail.SaleCode = dbFactory.ToModelValue(reader, "SaleCode").ToDBString();
            t_instockdetail.SaleName = dbFactory.ToModelValue(reader, "SaleName").ToDBString();
            t_instockdetail.ErpVoucherNo = dbFactory.ToModelValue(reader, "ErpVoucherNo").ToDBString();

            t_instockdetail.MaterialNoID = dbFactory.ToModelValue(reader, "MateiralNoID").ToInt32();
            t_instockdetail.PartNo = dbFactory.ToModelValue(reader, "PartNo").ToDBString();

            t_instockdetail.VoucherNo = dbFactory.ToModelValue(reader, "VoucherNo").ToDBString();
            t_instockdetail.IsSerial = dbFactory.ToModelValue(reader, "IsSerial").ToInt32();


            t_instockdetail.StrongHoldCode = dbFactory.ToModelValue(reader, "StrongHoldCode").ToDBString();
            t_instockdetail.StrongHoldName = dbFactory.ToModelValue(reader, "StrongHoldName").ToDBString();
            t_instockdetail.CompanyCode = dbFactory.ToModelValue(reader, "CompanyCode").ToDBString();
            t_instockdetail.RowNoDel = dbFactory.ToModelValue(reader, "RowNoDel").ToDBString();
            t_instockdetail.DeliverySup = dbFactory.ToModelValue(reader, "DeliverySup").ToDBString();
            t_instockdetail.ShipmentDate = dbFactory.ToModelValue(reader, "ShipmentDate").ToDateTime();
            t_instockdetail.ArrStockDate = dbFactory.ToModelValue(reader, "ArrStockDate").ToDateTime();
            t_instockdetail.ErpLineStatus = dbFactory.ToModelValue(reader, "ErpLineStatus").ToInt32();
            t_instockdetail.ERPNote = dbFactory.ToModelValue(reader, "ERPNote").ToDBString();

            t_instockdetail.FromBatchNo = dbFactory.ToModelValue(reader, "FromBatchNo").ToDBString();
            t_instockdetail.FromErpAreaNo = dbFactory.ToModelValue(reader, "FromErpAreaNo").ToDBString();

            string FromErpWarehouse = dbFactory.ToModelValue(reader, "FromErpWarehouse").ToDBString();
            t_instockdetail.FromErpWarehouse = string.IsNullOrEmpty(FromErpWarehouse) ? "" : (FromErpWarehouse.Contains("-") ? FromErpWarehouse : (t_instockdetail.StrongHoldCode + "-" + FromErpWarehouse));

            string ToErpWarehouse = dbFactory.ToModelValue(reader, "ToErpWarehouse").ToDBString();
            t_instockdetail.ToErpWarehouse = string.IsNullOrEmpty(ToErpWarehouse)?"":(ToErpWarehouse.Contains("-") ? ToErpWarehouse : (t_instockdetail.StrongHoldCode + "-" + ToErpWarehouse));
            t_instockdetail.IsSpcBatch = dbFactory.ToModelValue(reader, "IsSpcBatch").ToDBString();
            t_instockdetail.ToBatchNo = dbFactory.ToModelValue(reader, "ToBatchNo").ToDBString();
            t_instockdetail.ToErpAreaNo = dbFactory.ToModelValue(reader, "ToErpAreaNo").ToDBString();
            t_instockdetail.ADVRECEIVEQTY = dbFactory.ToModelValue(reader, "ADVRECEIVEQTY").ToInt32();

            t_instockdetail.QcCode = dbFactory.ToModelValue(reader, "QcCode").ToDBString();
            t_instockdetail.QcDesc = dbFactory.ToModelValue(reader, "QcDesc").ToDBString();
            t_instockdetail.ERPVoucherType = dbFactory.ToModelValue(reader, "ErpVoucherType").ToDBString();
            t_instockdetail.EDate = dbFactory.ToModelValue(reader, "EDate").ToDateTime();
            t_instockdetail.InvoiceNo = dbFactory.ToModelValue(reader, "InvoiceNo").ToDBString();

            t_instockdetail.TracNo = dbFactory.ToModelValue(reader, "TracNo").ToDBString();
            t_instockdetail.ProjectNo = dbFactory.ToModelValue(reader, "ProjectNo").ToDBString();
            t_instockdetail.iarrsid = dbFactory.ToModelValue(reader, "iarrsid").ToDBString();
            t_instockdetail.spec = dbFactory.ToModelValue(reader, "spec").ToDBString();
            return t_instockdetail;
        }

        protected override string GetViewName()
        {
            return "V_INSTOCKDETAIL";
        }

        protected override string GetTableName()
        {
            return "T_INSTOCKDETAIL";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override List<string> GetDeleteSql(UserModel user, T_InStockDetailInfo model)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            strSql = "delete t_Instockdetail where id = '" + model.ID + "'";

            lstSql.Add(strSql);

            return lstSql;
        }


        public bool CheckSerialNo(List<T_InStockDetailInfo> modelList, ref string strError)
        {
            try
            {
                bool bSucc = false;
                T_SerialNo_DB db = new T_SerialNo_DB();

                foreach (var item in modelList)
                {
                    //序列号管理
                    if (item.IsSerial == 2)
                    {
                        foreach (var itemSerialNo in item.lstSerialNo)
                        {
                            if (db.CheckSerialNoInStockCommit(itemSerialNo.SerialNo, ref strError) == false)
                            {
                                bSucc = false;
                                break;
                            }
                            else { bSucc = true; }
                        }
                    }
                    else
                    {
                        bSucc = true;
                    }

                }

                return bSucc;

            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 导入序列号验证是否存在
        /// </summary>
        /// <param name="SerialXml"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        public bool CheckSerialListIsExist(string SerialXml, ref string strErrMsg)
        {
            try
            {
                int iResult = 0;

                OracleParameter[] cmdParms = new OracleParameter[] 
            {
                new OracleParameter("strSerialXml", OracleDbType.NClob),                
                new OracleParameter("bResult", OracleDbType.Int32,ParameterDirection.Output),
                new OracleParameter("strErrMsg", OracleDbType.NVarchar2,200,strErrMsg,ParameterDirection.Output)
            };

                cmdParms[0].Value = SerialXml;

                dbFactory.ExecuteNonQuery3(dbFactory.ConnectionStringLocalTransaction, CommandType.StoredProcedure, "P_CHECKSERIAL", cmdParms);
                iResult = Convert.ToInt32(cmdParms[1].Value.ToString());
                strErrMsg = cmdParms[2].Value.ToString();

                return iResult == 1 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 导入EXCEL，获取单据明细
        /// </summary>
        /// <param name="VoucherNo"></param>
        /// <returns></returns>
        public List<T_InStockDetailInfo> GetDetailByVoucherNo(string VoucherNo, int VoucherType)
        {
            string strSql = "select * from v_Instockdetail a where  Erpvoucherno = '" + VoucherNo + "' and vouchertype='" + VoucherType + "'";

            return base.GetModelListBySql(strSql);
        }

        protected override string GetFilterSql(UserModel user, T_InStockDetailInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";
            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " erpvoucherno like '%" + model.ErpVoucherNo+ "%' ";
            }
            if (!string.IsNullOrEmpty(model.MaterialNo))
            {
                strSql += strAnd;
                strSql += " MaterialNo like '%" + model.MaterialNo + "%' ";
            }
            if (!string.IsNullOrEmpty(model.ProjectNo))
            {
                strSql += strAnd;
                strSql += " ProjectNo like '%" + model.ProjectNo + "%' ";
            }
            if (!string.IsNullOrEmpty(model.TracNo))
            {
                strSql += strAnd;
                strSql += " TracNo like '%" + model.TracNo + "%' ";
            }
            if (!string.IsNullOrEmpty(model.spec))
            {
                strSql += strAnd;
                strSql += " spec like '%" + model.spec + "%' ";
            }
            strSql += strAnd;
            strSql += " VoucherType != 39 ";

            return strSql; //+ " order by id desc";
        }

        protected override string GetOrderNoFieldName()
        {
            return "taskno";
        }


        }
}
