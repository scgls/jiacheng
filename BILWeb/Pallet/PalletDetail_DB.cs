using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;
using BILBasic.User;
using BILWeb.OutBarCode;
using System.Data;
using BILBasic.XMLUtil;
using BILWeb.Stock;
using BILWeb.Quality;
using BILWeb.OutStock;

namespace BILWeb.Pallet
{
    public partial class T_PalletDetail_DB : BILBasic.Basing.Factory.Base_DB<T_PalletDetailInfo>
    {

        /// <summary>
        /// 添加t_palletdetail
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_PalletDetailInfo t_palletdetail)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();
        }

        protected override List<string> GetSaveSql(UserModel user, ref T_PalletDetailInfo t_palletdetail)
        {
            throw new NotImplementedException();
        }


        protected override List<string> GetSaveModelListSql(UserModel user, List<T_PalletDetailInfo> modelList)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            //查看modellist是否有托盘号，有托盘号说明是再次组托
            if (!string.IsNullOrEmpty(modelList[0].PalletNo))
            {
                modelList.ForEach(t => t.TaskNo = modelList[0].PalletNo);
                //再次组托，确定是收货组托还是在库组托
                if (modelList[0].lstBarCode[0].AreaID > 0)
                {
                    lstSql = GetUpdatePalletStockSql(modelList);
                }
                else
                {
                    //再次组托，只是收货组托
                    lstSql = GetInsertPalletNewSerialNoSql(user, modelList);
                }
                strSql = " update t_stock set palletno = '" + modelList[0].PalletNo + "' where barcode in (select barcode from T_PALLETDETAIL where PALLETNO='" + modelList[0].PalletNo + "');";
                lstSql.Add(strSql);
            }
            else
            {
                //不带托盘号，就是新组托,如果是已经入库的托盘需要插入托盘表和库存表
                //int ID = base.GetTableID("Seq_Pallet_Id");
                int ID = base.GetTableIDBySqlServer("t_Pallet");
                int detailID = 0;

                //string VoucherNoID = base.GetTableID("Seq_Pallet_No").ToString();

                string VoucherNo = "4" + System.DateTime.Now.ToString("yyMMdd") + ID.ToString().PadLeft(6, '0');

                modelList.ForEach(t => t.TaskNo = VoucherNo);

                strSql = string.Format("SET IDENTITY_INSERT t_Pallet on ;insert into t_Pallet(Id, Palletno, Creater, Createtime,Strongholdcode,Strongholdname,Companycode,Pallettype,Supplierno,Suppliername,ERPVOUCHERNO)" +
                                " values ('{0}','{1}','{2}',GETDATE(),'{3}','{4}','{5}','{6}','{7}','{8}','{9}');SET IDENTITY_INSERT t_Pallet off ;", ID, VoucherNo, user.UserNo, modelList[0].StrongHoldCode, modelList[0].StrongHoldName, modelList[0].CompanyCode, '1', modelList[0].SuppliernNo, modelList[0].SupplierName, modelList[0].ErpVoucherNo);

                lstSql.Add(strSql);

                foreach (var item in modelList)
                {
                    item.PalletNo = VoucherNo;

                    foreach (var itemSerial in item.lstBarCode)
                    {
                        //detailID = base.GetTableID("SEQ_PALLET_DETAIL_ID");
                        detailID = base.GetTableIDBySqlServer("t_Palletdetail");
                        strSql = string.Format("SET IDENTITY_INSERT t_Palletdetail on ;insert into t_Palletdetail(Id, Headerid, Palletno, Materialno, Materialdesc, Serialno,Creater," +
                        "Createtime,RowNo,VOUCHERNO,ERPVOUCHERNO,materialnoid,qty,BARCODE,StrongHoldCode,StrongHoldName,CompanyCode,pallettype," +
                        "batchno,rownodel,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,PRODUCTBATCH,EDATE,Supplierno,Suppliername,Unit)" +
                        "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',GETDATE(),'{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','1'," +
                        "'{16}','{17}',null,'{19}',null,'null','{22}','{23}','{24}','{25}');SET IDENTITY_INSERT t_Palletdetail off ;", detailID, ID, VoucherNo, itemSerial.MaterialNo, itemSerial.MaterialDesc, itemSerial.SerialNo, user.UserNo,
                        itemSerial.RowNo, itemSerial.VoucherNo, itemSerial.ErpVoucherNo, itemSerial.MaterialNoID, itemSerial.Qty, itemSerial.BarCode,
                        itemSerial.StrongHoldCode, itemSerial.StrongHoldName, itemSerial.CompanyCode, itemSerial.BatchNo, itemSerial.RowNoDel,
                        itemSerial.ProductDate, itemSerial.SupPrdBatch, itemSerial.SupPrdDate, itemSerial.ProductBatch, itemSerial.EDate, itemSerial.SupCode, itemSerial.SupName, itemSerial.Unit);

                        lstSql.Add(strSql);
                        //库存组托
                        if (itemSerial.AreaID > 0)
                        {
                            strSql = "update t_stock set palletno = '" + VoucherNo + "' where serialno = '" + itemSerial.SerialNo + "'";
                            lstSql.Add(strSql);
                        }
                    }
                }

                strSql = " update t_stock set palletno = '" + VoucherNo + "' where barcode in (select barcode from T_PALLETDETAIL where PALLETNO='" + VoucherNo + "');";
                lstSql.Add(strSql);
            }

            return lstSql;
        }

        /// <summary>
        /// 已经存在库存的组托,把2个托盘合并，之前先做好移库
        /// </summary>
        /// <param name="modelList"></param>
        /// <returns></returns>
        private List<string> GetUpdatePalletStockSql(List<T_PalletDetailInfo> modelList)
        {
            string strSql = string.Empty;
            string strSql1 = string.Empty;
            List<string> lstSql = new List<string>();

            foreach (var item in modelList)
            {
                foreach (var itemSerial in item.lstBarCode)
                {
                    strSql = "update t_stock a set a.Palletno = '' where a.Serialno = '" + itemSerial.SerialNo + "'";
                    lstSql.Add(strSql);
                    strSql1 = "update t_Palletdetail a set a.Headerid = (select id from t_Pallet b where b.palletno = '" + itemSerial.PalletNo + "' )," +
                              "a.Palletno = '" + itemSerial.PalletNo + "' where a.Serialno = '" + itemSerial.SerialNo + "';";
                    lstSql.Add(strSql1);
                }
            }
            return lstSql;
        }


        private List<string> GetInsertPalletNewSerialNoSql(UserModel user, List<T_PalletDetailInfo> modelList)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            foreach (var item in modelList)
            {
                foreach (var itemSerial in item.lstBarCode)
                {
                    strSql = "insert into t_Palletdetail (Headerid, Palletno, Materialno, Materialdesc, Serialno, Creater, Createtime, Unit," +
                            "Rowno, voucherno, Erpvoucherno,  Materialnoid, Qty, Barcode, Strongholdcode, Strongholdname, Companycode, Batchno, Sn, " +
                            "Rownodel, Pallettype, Productdate, Supprdbatch, Supprddate, Productbatch, Edate,supplierno,suppliername) " +
                            "select " +
                            "(Select id from t_Pallet where palletno = '" + item.PalletNo + "'),'" + item.PalletNo+ "','" + itemSerial.MaterialNo + "'," +
                            "'" + itemSerial.MaterialDesc + "','" + itemSerial.SerialNo + "',  '"+user.UserNo+"'," +
                            "getdate(),'" + itemSerial.Unit + "','" + itemSerial.RowNo + "','" + itemSerial.VoucherNo + "',  '" + itemSerial.ErpVoucherNo + "','" + itemSerial.MaterialNoID + "','" + itemSerial.Qty + "'," +
                            "'" + itemSerial.BarCode + "', '" + itemSerial.StrongHoldCode + "','" + itemSerial.StrongHoldName + "','" + itemSerial.CompanyCode + "','" + itemSerial.BatchNo + "', '" + itemSerial.SN + "','" + itemSerial.RowNoDel + "'," +
                            "'1',null,'" + itemSerial.SupPrdBatch + "',null,'" + itemSerial.ProductBatch + "','" + itemSerial.EDate + "','"+itemSerial.SupCode+"','"+itemSerial.SupName+"'" +
                            "";

                    lstSql.Add(strSql);
                }
            }
            return lstSql;
        }

        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_PalletDetailInfo ToModel(IDataReader reader)
        {
            T_PalletDetailInfo t_palletdetail = new T_PalletDetailInfo();

            t_palletdetail.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_palletdetail.HeaderID = dbFactory.ToModelValue(reader, "HEADERID").ToInt32();
            t_palletdetail.PalletNo = dbFactory.ToModelValue(reader, "PALLETNO").ToDBString();
            t_palletdetail.MaterialNo = dbFactory.ToModelValue(reader, "MATERIALNO").ToDBString();
            t_palletdetail.MaterialDesc = dbFactory.ToModelValue(reader, "MATERIALDESC").ToDBString();
            t_palletdetail.SerialNo = dbFactory.ToModelValue(reader, "SERIALNO").ToDBString();
            t_palletdetail.MaterialNoID = dbFactory.ToModelValue(reader, "MATERIALNOID").ToInt32();
            t_palletdetail.ErpVoucherNo = dbFactory.ToModelValue(reader, "ERPVoucherNo").ToDBString();
            t_palletdetail.BarCode = dbFactory.ToModelValue(reader, "BarCode").ToDBString();
            t_palletdetail.StrongHoldCode = dbFactory.ToModelValue(reader, "StrongHoldCode").ToDBString();
            t_palletdetail.StrongHoldName = dbFactory.ToModelValue(reader, "StrongHoldName").ToDBString();
            t_palletdetail.CompanyCode = dbFactory.ToModelValue(reader, "CompanyCode").ToDBString();
            t_palletdetail.PalletType = dbFactory.ToModelValue(reader, "pallettype").ToInt32();
            t_palletdetail.Qty = (decimal?)dbFactory.ToModelValue(reader, "Qty");
            t_palletdetail.BatchNo = dbFactory.ToModelValue(reader, "BatchNo").ToDBString();
            t_palletdetail.RowNo = dbFactory.ToModelValue(reader, "RowNo").ToDBString();
            t_palletdetail.RowNoDel = dbFactory.ToModelValue(reader, "RowNoDel").ToDBString();
            t_palletdetail.ProductBatch = dbFactory.ToModelValue(reader, "ProductBatch").ToDBString();
            t_palletdetail.ProductDate = dbFactory.ToModelValue(reader, "ProductDate").ToDateTime();
            t_palletdetail.SupPrdBatch = dbFactory.ToModelValue(reader, "SupPrdBatch").ToDBString();
            t_palletdetail.SupPrdDate = dbFactory.ToModelValue(reader, "SupPrdDate").ToDateTime();

            t_palletdetail.SuppliernNo = dbFactory.ToModelValue(reader, "SupplierNo").ToDBString();
            t_palletdetail.SupplierName = dbFactory.ToModelValue(reader, "SupplierName").ToDBString();
            t_palletdetail.Unit = dbFactory.ToModelValue(reader, "Unit").ToDBString();

            //add by cym 2019-3-26 完工入库的时候需要传效期日！！！
            t_palletdetail.EDate = dbFactory.ToModelValue(reader, "EDate").ToDateTime();
            //t_palletdetail.EAN = dbFactory.ToModelValue(reader, "EAN").ToDBString();


            return t_palletdetail;
        }

        protected override string GetViewName()
        {
            return "v_palletdetail";
        }

        protected override string GetTableName()
        {
            return "T_PALLETDETAIL";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }


        public List<T_PalletDetailInfo> GetPalletDetailByVoucherNo(string VoucherNo)
        {
            try
            {
                List<T_PalletDetailInfo> lstModel = new List<T_PalletDetailInfo>();

                string strSql = "SELECT a.Palletno,a.Serialno,a.Erpvoucherno,b.Materialno,b.Materialdesc,a.rowno,a.barcode,a.Unit FROM t_Palletdetail a left join t_Material b " +
                                " on a.Materialnoid = b.Id where a.Erpvoucherno = '" + VoucherNo + "'";

                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        T_PalletDetailInfo model = new T_PalletDetailInfo();
                        model.PalletNo = dr["PalletNo"].ToDBString();
                        model.SerialNo = dr["SerialNo"].ToDBString();
                        model.ErpVoucherNo = dr["Erpvoucherno"].ToDBString();
                        model.MaterialNo = dr["MaterialNo"].ToDBString();
                        model.MaterialDesc = dr["MaterialDesc"].ToDBString();
                        model.RowNo = dr["RowNo"].ToDBString();
                        model.BarCode = dr["BarCode"].ToDBString();
                        model.Unit = dr["Unit"].ToDBString();
                        lstModel.Add(model);
                    }
                }
                return lstModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Del_PalletOrSerialNo(string palletNo, string serialNo, ref string errMsg)
        {
            List<string> sqlList = new List<string>();
            if (!String.IsNullOrEmpty(serialNo))
            {
                string sql = "delete FROM T_PALLETDETAIL WHERE PALLETTYPE=1 and  PALLETNO='" + palletNo + "' and serialNo='" + serialNo + "'";
                sqlList.Add(sql);
            }
            else
            {
                string sql = "delete FROM T_PALLETDETAIL WHERE  PALLETTYPE=1 and PALLETNO='" + palletNo + "'";
                sqlList.Add(sql);
                sql = "delete FROM T_PALLET  WHERE  PALLETTYPE=1 and PALLETNO = '" + palletNo + "'";
                sqlList.Add(sql);
            }
            if (sqlList.Count != 0)
            {
                bool result = base.SaveModelListBySqlToDB(sqlList, ref errMsg);

                if (result && !string.IsNullOrEmpty(serialNo))
                {
                    string sql = "select count(*) From T_PALLETDETAIL  where PALLETTYPE=1 and  PALLETNO='" + palletNo + "'";
                    if (Convert.ToInt32(base.GetScalarBySql(sql).ToString()) == 0)
                    {
                        sql = "delete FROM T_PALLET  WHERE PALLETNO = '" + palletNo + "' and PALLETTYPE=1";
                        sqlList.Add(sql);
                        return base.SaveModelListBySqlToDB(sqlList, ref errMsg);
                    }
                }

                return result;
            }
            return false;
        }


        public bool DeletePalletORBarCode(List<T_PalletDetailInfo> modelList, ref string strErrMsg)
        {
            string strSql = string.Empty;
            string strSql1 = string.Empty;

            List<string> lstSql = new List<string>();

            foreach (var item in modelList)
            {
                foreach (var itemSerialNo in item.lstBarCode)
                {
                    strSql = "delete t_Palletdetail where  PALLETTYPE=1 and serialno = '" + itemSerialNo.SerialNo + "'";
                    lstSql.Add(strSql);
                }
            }

            strSql1 = "delete t_Pallet where  PALLETTYPE=1 and palletno = '" + modelList[0].PalletNo + "' and (select count(1) from t_Palletdetail where  PALLETTYPE=1 and palletno = '" + modelList[0].PalletNo + "')=0";
            lstSql.Add(strSql1);

            return base.SaveModelListBySqlToDB(lstSql, ref strErrMsg);
        }

        public bool CheckDeletePalletDetailBefore(List<T_PalletDetailInfo> modelList, ref string strError)
        {
            try
            {
                int iResult = 0;
                DataSet ds;

                string strOutStockTaskXml = XmlUtil.Serializer(typeof(List<T_PalletDetailInfo>), modelList);

                OracleParameter[] cmdParms = new OracleParameter[]
            {
                new OracleParameter("strPalletDetailXml", OracleDbType.NClob),
                new OracleParameter("bResult", OracleDbType.Int32,ParameterDirection.Output),
                new OracleParameter("ErrString", OracleDbType.NVarchar2,200,strError,ParameterDirection.Output)
            };

                cmdParms[0].Value = strOutStockTaskXml;

                cmdParms[1].Value = ParameterDirection.Output;
                cmdParms[2].Value = ParameterDirection.Output;

                dbFactory.ExecuteNonQuery2(dbFactory.ConnectionStringLocalTransaction, CommandType.StoredProcedure, "p_checkpalletisdelete", cmdParms);
                iResult = Convert.ToInt32(cmdParms[1].Value.ToString());
                strError = cmdParms[2].Value.ToString();
                return iResult == 1 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveBarCodeToStock(UserModel userModel, T_StockInfo OldModel, T_StockInfo NewModel, ref string NewSerialNo, ref string strErrMsg)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            //newmodel没有serialno需要重新生成新的条码，标示拆箱操作
            //装箱
            if (NewModel != null)
            {
                lstSql = GetBoxAmountSql(userModel, OldModel, NewModel);
            }
            else
            {
                //拆箱
                lstSql = GetUnBoxAmountSql(OldModel, userModel, ref NewSerialNo);
            }

            return base.SaveModelListBySqlToDB(lstSql, ref strErrMsg);
        }

        private List<string> GetUnBoxAmountSql(T_StockInfo model, UserModel userModel, ref string NewSerialNo)
        {
            T_QualityDetail_DB _db = new T_QualityDetail_DB();
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            //strSql = _db.GetAmountQtySql(model, ref NewSerialNo);//生成条码表拆零条码
            lstSql.AddRange(_db.GetAmountQtySql(model, ref NewSerialNo));

            strSql = _db.GetAmountQtyInsertStockSql(model, userModel, NewSerialNo);
            lstSql.Add(strSql);

            strSql = "update t_stock  set Qty = Qty - '" + model.AmountQty + "'  where serialno = '" + model.SerialNo + "'";
            lstSql.Add(strSql);

            strSql = "delete t_stock where serialno = '" + model.SerialNo + "' and qty =0";
            lstSql.Add(strSql);

            //拆零的条码需要把托盘拆掉
            if (!string.IsNullOrEmpty(model.PalletNo))
            {
                //strSql = _db.GetAmountQtyInsertPalletSql(model, userModel, NewSerialNo, model.PalletNo);
                //lstSql.Add(strSql);

                strSql = "update t_Palletdetail  set Qty = qty - '" + model.AmountQty + "' where barcode = '" + model.Barcode + "'";
                lstSql.Add(strSql);

                strSql = "delete t_Palletdetail  where barcode = '" + model.Barcode + "' and Qty = 0";
                lstSql.Add(strSql);

                strSql = "delete t_Pallet where palletno = '" + model.PalletNo + "' and (select count(1) from t_Palletdetail where palletno = '" + model.PalletNo + "')=0";
                lstSql.Add(strSql);
            }

            return lstSql;
        }

        //装箱SQL
        private List<string> GetBoxAmountSql(UserModel userModel, T_StockInfo OldModel, T_StockInfo NewModel)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;
            strSql = "update t_stock  set Qty = Qty + '" + NewModel.Qty + "' where serialno = '" + OldModel.SerialNo + "'";
            lstSql.Add(strSql);

            strSql = GetStockTransSql(userModel, NewModel);
            lstSql.Add(strSql);

            strSql = "delete t_stock where serialno = '" + NewModel.SerialNo + "'";
            lstSql.Add(strSql);

            if (!string.IsNullOrEmpty(OldModel.PalletNo))
            {

                strSql = "update t_Palletdetail  set Qty = qty + '" + NewModel.Qty + "' where serialno = '" + OldModel.SerialNo + "'";
                lstSql.Add(strSql);

                strSql = "delete t_Palletdetail  where Serialno = '" + NewModel.SerialNo + "' and Qty = 0";
                lstSql.Add(strSql);
            }

            return lstSql;
        }

        private string GetStockTransSql(UserModel user, T_StockInfo model)
        {
            string strSql = "INSERT INTO t_Stocktrans ( Barcode, Serialno, Materialno, Materialdesc, Warehouseid, Houseid, Areaid, Qty,  Status, Isdel, Creater, Createtime, " +
                             "Batchno, Sn,  Oldstockid, Taskdetailesid, Checkid, Transferdetailsid,  Unit, Unitname, Palletno, Receivestatus,  Islimitstock,  " +
                             "Materialnoid, Strongholdcode, Strongholdname, Companycode, Edate, Supcode, Supname, Productdate, Supprdbatch, Supprddate, Isquality)" +
                            "SELECT A.Barcode,A.Serialno,A.Materialno,A.Materialdesc,A.Warehouseid,A.Houseid,A.Areaid,A.Qty, " +
                            "A.Status,A.Isdel,'" + user.UserName + "',A.Createtime,A.Batchno,A.Sn,a.Oldstockid,A.Taskdetailesid,A.Checkid,A.Transferdetailsid,A.Unit,A.Unitname,A.Palletno,A.Receivestatus," +
                            "A.Islimitstock,A.Materialnoid,A.Strongholdcode,A.Strongholdname,A.Companycode,A.Edate,A.Supcode,A.Supname,A.Productdate,A.Supprdbatch," +
                            "A.Supprddate,A.Isquality FROM T_STOCK A WHERE A.Serialno = '" + model.SerialNo + "'";
            return strSql;
        }

        /// <summary>
        /// 新建组托，需要验证条码是否已经组托
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <returns></returns>
        public int CheckSerialNoIsInPallet(string SerialNo)
        {
            string strSql = "select count(1) from t_Palletdetail where ( serialno = '" + SerialNo + "' or palletno = '" + SerialNo + "')  ";//and pallettype=1

            return base.GetScalarBySql(strSql).ToInt32();
        }


        public bool GetBarCodeByPalletDetail(List<T_PalletDetailInfo> modelList, ref List<T_OutBarCodeInfo> lstBarCode, ref string strError)
        {
            try
            {
                int iResult = 0;
                DataSet ds;

                string strOutStockTaskXml = XmlUtil.Serializer(typeof(List<T_PalletDetailInfo>), modelList);

                OracleParameter[] cmdParms = new OracleParameter[] 
            {
                new OracleParameter("strPalletDetailXml", OracleDbType.NClob),    
                new OracleParameter("BarCodeCur", OracleDbType.RefCursor,ParameterDirection.Output),
                new OracleParameter("bResult", OracleDbType.Int32,ParameterDirection.Output),
                new OracleParameter("ErrString", OracleDbType.NVarchar2,200,strError,ParameterDirection.Output)
            };

                cmdParms[0].Value = strOutStockTaskXml;

                cmdParms[1].Value = ParameterDirection.Output;
                cmdParms[2].Value = ParameterDirection.Output;
                cmdParms[3].Value = ParameterDirection.Output;

                ds = dbFactory.ExecuteDataSetForCursor(ref iResult, ref strError, dbFactory.ConnectionStringLocalTransaction, CommandType.StoredProcedure, "p_getbarcodebypalletdetail", cmdParms);

                if (iResult == 1)
                {
                    lstBarCode = TOOL.DataTableToList.DataSetToList<T_OutBarCodeInfo>(ds.Tables[0]);
                    strError = string.Empty;
                }

                return iResult == 1 ? true : false;

            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool GetBarCodeByPalletDetailnew(List<T_PalletDetailInfo> modelList, ref List<T_OutBarCodeInfo> lstBarCode, ref string strError)
        {
            try
            {
                int iResult = 0;
                DataSet ds;

                string strOutStockTaskXml = XmlUtil.Serializer(typeof(List<T_PalletDetailInfo>), modelList);

                OracleParameter[] cmdParms = new OracleParameter[]
            {
                new OracleParameter("strPalletDetailXml", OracleDbType.NClob),
                new OracleParameter("BarCodeCur", OracleDbType.RefCursor,ParameterDirection.Output),
                new OracleParameter("bResult", OracleDbType.Int32,ParameterDirection.Output),
                new OracleParameter("ErrString", OracleDbType.NVarchar2,200,strError,ParameterDirection.Output)
            };

                cmdParms[0].Value = strOutStockTaskXml;

                cmdParms[1].Value = ParameterDirection.Output;
                cmdParms[2].Value = ParameterDirection.Output;
                cmdParms[3].Value = ParameterDirection.Output;

                ds = dbFactory.ExecuteDataSetForCursor(ref iResult, ref strError, dbFactory.ConnectionStringLocalTransaction, CommandType.StoredProcedure, "p_getbarcodedetails", cmdParms);
                DataTable dt = ds.Tables[0];


                if (iResult == 1)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        T_OutBarCodeInfo model = new T_OutBarCodeInfo();
                        DataRow dr = dt.Rows[i];

                        model.Qty = dr["Qty"].ToDecimalNull();
                        model.Unit = dr["Unit"].ToString();
                        model.SupCode = dr["SupCode"].ToDBString();
                        model.SupName = dr["SupName"].ToDBString();
                        model.SerialNo = dr["SerialNo"].ToDBString();
                        model.BatchNo = dr["Batchno"].ToDBString();

                        model.ErpVoucherNo = dr["ErpVoucherNo"].ToDBString();
                        model.MaterialNo = dr["MaterialNo"].ToDBString();
                        model.MaterialDesc = dr["MaterialDesc"].ToDBString();
                        model.OutPackQty = dr["OutPackQty"].ToDecimalNull();
                        model.PrintQty = dr["PrintQty"].ToDecimalNull();
                        model.BarCode = dr["BarCode"].ToDBString();

                        model.Creater = dr["Creater"].ToDBString();
                        model.CreateTime = dr["CreateTime"].ToDateTime();
                        model.MaterialNoID = dr["Materialnoid"].ToInt32();
                        model.StrongHoldCode = dr["Strongholdcode"].ToDBString();
                        model.StrongHoldName = dr["Strongholdname"].ToDBString();
                        model.CompanyCode = dr["Companycode"].ToDBString();
                        model.ProductDate = dr["Productdate"].ToDateTime();
                        model.EDate = dr["EDate"].ToDateTime();
                        lstBarCode.Add(model);
                    }
                    //lstBarCode = TOOL.DataTableToList.DataSetToList<T_OutBarCodeInfo>(dt);
                    strError = string.Empty;
                }

                return iResult == 1 ? true : false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetBarCodeByPalletDetailnewByCym(List<T_PalletDetailInfo> modelList, ref List<T_OutBarCodeInfo> lstBarCode, ref string strError)
        {
            try
            {
                string strSql = "";
                List<string> lstSql = new List<string>();

                string strKey = "";
                foreach (var item in modelList)
                {
                    if (string.IsNullOrEmpty(strKey))
                    {
                        strKey = "N'" + item.SerialNo + "'";
                    }
                    else
                        strKey += (",N'" + item.SerialNo + "'");
                }

                lstBarCode = new List<T_OutBarCodeInfo>();

                strSql = "select * from t_outbarcode t where t.SerialNo in (" + strKey + ")";

                using (IDataReader dr = dbFactory.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        T_OutBarCodeInfo model = new T_OutBarCodeInfo();

                        model.Qty = dr["Qty"].ToDecimalNull();
                        model.Unit = dr["Unit"].ToString();
                        model.SupCode = dr["SupCode"].ToDBString();
                        model.SupName = dr["SupName"].ToDBString();
                        model.SerialNo = dr["SerialNo"].ToDBString();
                        model.BatchNo = dr["Batchno"].ToDBString();

                        model.ErpVoucherNo = dr["ErpVoucherNo"].ToDBString();
                        model.MaterialNo = dr["MaterialNo"].ToDBString();
                        model.MaterialDesc = dr["MaterialDesc"].ToDBString();
                        model.OutPackQty = dr["OutPackQty"].ToDecimalNull();
                        model.PrintQty = dr["PrintQty"].ToDecimalNull();
                        model.BarCode = dr["BarCode"].ToDBString();

                        model.Creater = dr["Creater"].ToDBString();
                        model.CreateTime = dr["CreateTime"].ToDateTime();
                        model.MaterialNoID = dr["Materialnoid"].ToInt32();
                        model.StrongHoldCode = dr["Strongholdcode"].ToDBString();
                        model.StrongHoldName = dr["Strongholdname"].ToDBString();
                        model.CompanyCode = dr["Companycode"].ToDBString();
                        model.ProductDate = dr["Productdate"].ToDateTime();
                        model.EDate = dr["EDate"].ToDateTime();

                        //model.ZXBarcode = dr["ZXBarcode"].ToDBString();

                        lstBarCode.Add(model);
                    }
                }

                if (lstBarCode.Count > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }



        /// <summary>
        /// 根据ERP单据删除托盘
        /// </summary>
        /// <param name="ErpVoucherNo"></param>
        /// <param name="PalletType"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool DeletePalletByErpVoucherNo(string ErpVoucherNo, string PalletType, ref string strError)
        {
            try
            {
                string strSql = string.Empty;
                List<string> lstSql = new List<string>();
                strSql = "delete t_Palletdetail a where a.Erpvoucherno = '" + ErpVoucherNo + "' and a.Pallettype = '" + PalletType + "'";
                lstSql.Add(strSql);

                strSql = "Delete t_Pallet where Erpvoucherno = '" + ErpVoucherNo + "'";
                lstSql.Add(strSql);

                return base.SaveModelListBySqlToDB(lstSql, ref strError);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<T_PalletDetailInfo> GetPalletDetailMessage(string erpvoucherno)
        {
            try
            {
                List<T_PalletDetailInfo> lstModel = new List<T_PalletDetailInfo>();

                string strSql = "select * from t_palletdetail where palletno=(select palletno from (select * from t_pallet order by createtime desc) where erpvoucherno='" + erpvoucherno + "' and rownum<=1)";

                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        T_PalletDetailInfo model = new T_PalletDetailInfo();
                        model.PalletNo = dr["PalletNo"].ToDBString();
                        model.SerialNo = dr["SerialNo"].ToDBString();
                        model.ErpVoucherNo = dr["Erpvoucherno"].ToDBString();
                        model.MaterialNo = dr["MaterialNo"].ToDBString();
                        model.MaterialDesc = dr["MaterialDesc"].ToDBString();
                        model.RowNo = dr["RowNo"].ToDBString();
                        model.BarCode = dr["BarCode"].ToDBString();
                        model.Unit = dr["Unit"].ToDBString();
                        lstModel.Add(model);
                    }
                }
                return lstModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public string CheckSerialnoInPallet(string SerialNo)
        {
            try
            {
                string strSql = "select palletno from t_Palletdetail  where (serialno = '" + SerialNo + "' or palletno = '" + SerialNo + "' or barcode = '" + SerialNo + "' ) and rownum<= 1";

                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        return dr["palletno"].ToDBString();
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public bool CheckSerialNoIsInStock(string json)
        {
            try
            {
                string strSql = "select * from t_stock where serialno='" + json + "' or barcode='" + json + "' or palletno='" + json + "'";

                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckSerialNoIsTasktransForWGRK(string json)
        {
            try
            {
                string strSql = "select serialno from t_tasktrans where serialno='" + json + "' and Tasktype='5'";

                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
                {
                    if (dr.Read())
                    {
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool CheckSerialNoIsMore(string json)
        {
            try
            {
                if (json.Substring(0, 2) == "2@")
                {
                    //获取外箱标签
                    string strSql1 = "select serialno from t_Palletdetail where palletno = '" + json + "'";
                    using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql1))
                    {
                        while (dr.Read())
                        {
                            json = dr["serialno"].ToDBString();
                            break;
                        }
                    }

                }


                //判断外箱标签
                string strSql2 = "select count(*) aa from t_Palletdetail where serialno = '" + json + "' or barcode='" + json + "'";
                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql2))
                {
                    while (dr.Read())
                    {
                        if (dr["aa"].ToInt32() > 1)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }




        public List<T_PalletDetailInfo> GetCPInstockMes(string json)
        {
            try
            {
                if (json.Substring(0, 2) == "2@")
                {
                    //2@来头为托盘二维码
                    json = json.Substring(json.Length - 13, 13);
                }
                List<T_PalletDetailInfo> lstModel = new List<T_PalletDetailInfo>();
                string strSql = "select * from t_palletdetail where palletno=(select palletno from t_palletdetail where serialno='" + json + "' or barcode='" + json + "' or palletno='" + json + "' and ROWNUM = 1)";

                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        T_PalletDetailInfo model = new T_PalletDetailInfo();
                        model.PalletNo = dr["PalletNo"].ToDBString();
                        model.SerialNo = dr["SerialNo"].ToDBString();
                        model.ErpVoucherNo = dr["Erpvoucherno"].ToDBString();
                        model.MaterialNo = dr["MaterialNo"].ToDBString();
                        model.MaterialDesc = dr["MaterialDesc"].ToDBString();
                        model.RowNo = dr["RowNo"].ToDBString();
                        model.BarCode = dr["BarCode"].ToDBString();
                        model.Unit = dr["Unit"].ToDBString();

                        //add by cym 2019-3-25 完工入库的时候需要传效期日！！！
                        model.EDate = dr["EDate"].ToDateTime().Date;

                        lstModel.Add(model);
                    }
                    return lstModel;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// PC端打印装箱生成托盘
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="modelListSum"></param>
        /// <param name="user"></param>
        /// <param name="strPalletNo"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool CreateOutStockBox(List<T_PalletDetailInfo> modelList, List<T_PalletDetailInfo> modelListSum, UserModel user, ref string strPalletNo, ref string strError)
        {
            try
            {
                string strSql = string.Empty;
                List<string> lstSql = new List<string>();

                int ID = base.GetTableID("Seq_Pallet_Id");

                int detailID = 0;

                string VoucherNoID = base.GetTableID("Seq_Pallet_No").ToString();

                string VoucherNo = "P" + System.DateTime.Now.ToString("yyyyMMdd") + VoucherNoID.PadLeft(4, '0');

                strPalletNo = VoucherNo;

                modelList.ForEach(t => t.TaskNo = VoucherNo);

                strSql = string.Format("insert into t_Pallet(Id, Palletno, Creater, Createtime,Strongholdcode,Strongholdname,Companycode,Pallettype,Supplierno,Suppliername,ERPVOUCHERNO)" +
                                " values ('{0}','{1}','{2}',Sysdate,'{3}','{4}','{5}','{6}','{7}','{8}','{9}')", ID, VoucherNo, user.UserNo, modelList[0].StrongHoldCode, modelList[0].StrongHoldName, modelList[0].CompanyCode, '3', modelList[0].SuppliernNo, modelList[0].SupplierName, modelList[0].ErpVoucherNo);

                lstSql.Add(strSql);

                foreach (var item in modelListSum)
                {
                    item.PalletNo = VoucherNo;

                    detailID = base.GetTableID("SEQ_PALLET_DETAIL_ID");
                    strSql = string.Format("insert into t_Palletdetail(Id, Headerid, Palletno, Materialno, Materialdesc, Serialno,Creater," +
                    "Createtime,RowNo,VOUCHERNO,ERPVOUCHERNO,materialnoid,qty,BARCODE,StrongHoldCode,StrongHoldName,CompanyCode,pallettype," +
                    "batchno,rownodel,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,PRODUCTBATCH,EDATE,Supplierno,Suppliername,Unit,ean)" +
                    "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',Sysdate,'{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','3'," +
                    "'{16}','{17}',to_date('{18}','yyyy-mm-dd hh24:mi:ss'),'{19}',to_date('{20}','yyyy-mm-dd hh24:mi:ss'),'{21}',to_date('{22}','yyyy-mm-dd hh24:mi:ss'),'{23}','{24}','{25}','{26}')", detailID, ID, VoucherNo, item.MaterialNo, item.MaterialDesc, item.SerialNo, user.UserNo,
                    item.RowNo, item.VoucherNo, item.ErpVoucherNo, item.MaterialNoID, item.Qty, item.BarCode,
                    item.StrongHoldCode, item.StrongHoldName, item.CompanyCode, item.BatchNo, item.RowNoDel,
                    item.ProductDate, item.SupPrdBatch, item.SupPrdDate, item.ProductBatch, item.EDate, "", "", item.Unit, item.EAN);

                    lstSql.Add(strSql);
                }

                modelList = modelList.Where(t => t.PalletNo == string.Empty).ToList();

                foreach (var item in modelList)
                {
                    strSql = "update t_Tasktrans set palletno = '" + VoucherNo + "' where id = '" + item.ID + "'";
                    lstSql.Add(strSql);
                }

                return base.SaveModelListBySqlToDB(lstSql, ref strError);

            }

            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="modelListSum"></param>
        /// <param name="user"></param>
        /// <param name="strPalletNo"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool CreatePalletByEmsLabel(List<T_OutStockDetailInfo> modelList, int PrintBoxQty, UserModel user, ref string strPalletNo, ref string strError)
        {
            try
            {
                string strSql = string.Empty;
                List<string> lstSql = new List<string>();

                int ID = base.GetTableID("Seq_Pallet_Id");

                int detailID = 0;

                string VoucherNoID = base.GetTableID("Seq_Pallet_No").ToString();

                string VoucherNo = "P" + System.DateTime.Now.ToString("yyyyMMdd") + VoucherNoID.PadLeft(4, '0');

                strPalletNo = VoucherNo;

                modelList.ForEach(t => t.TaskNo = VoucherNo);

                strSql = string.Format("insert into t_Pallet(Id, Palletno, Creater, Createtime,Strongholdcode,Strongholdname,Companycode,Pallettype,Supplierno,Suppliername,ERPVOUCHERNO,BOXCOUNT,CUSTOMERCODE,CUSTOMERNAME)" +
                                " values ('{0}','{1}','{2}',Sysdate,'{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')", ID, VoucherNo, user.UserNo, modelList[0].StrongHoldCode, modelList[0].StrongHoldName, modelList[0].CompanyCode, '4', modelList[0].CustomerCode, modelList[0].CustomerName, modelList[0].ErpVoucherNo, PrintBoxQty, modelList[0].CustomerCode, modelList[0].CustomerName);

                lstSql.Add(strSql);

                foreach (var item in modelList)
                {
                    item.PalletNo = VoucherNo;

                    detailID = base.GetTableID("SEQ_PALLET_DETAIL_ID");
                    strSql = string.Format("insert into t_Palletdetail(Id, Headerid, Palletno, Materialno, Materialdesc, Serialno,Creater," +
                    "Createtime,RowNo,VOUCHERNO,ERPVOUCHERNO,materialnoid,qty,BARCODE,StrongHoldCode,StrongHoldName,CompanyCode,pallettype," +
                    "batchno,rownodel,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,PRODUCTBATCH,EDATE,Supplierno,Suppliername,Unit,ean)" +
                    "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',Sysdate,'{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','1'," +
                    "'{16}','{17}',to_date('{18}','yyyy-mm-dd hh24:mi:ss'),'{19}',to_date('{20}','yyyy-mm-dd hh24:mi:ss'),'{21}',to_date('{22}','yyyy-mm-dd hh24:mi:ss'),'{23}','{24}','{25}','{26}')",
                    detailID, ID, VoucherNo, item.MaterialNo, item.MaterialDesc, item.EAN, user.UserNo,
                    item.RowNo, item.VoucherNo, item.ErpVoucherNo, item.MaterialNoID, item.ReviewQty, item.EAN,
                    item.StrongHoldCode, item.StrongHoldName, item.CompanyCode, item.BatchNo, item.RowNoDel,
                    item.EDate, item.FromBatchNo, item.EDate, item.FromBatchNo, item.EDate, "", "", item.Unit, item.EAN);

                    lstSql.Add(strSql);
                }

                strSql = "update t_Outstock set boxcount = '' where id = '" + modelList[0].HeaderID + "'";
                lstSql.Add(strSql);

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
