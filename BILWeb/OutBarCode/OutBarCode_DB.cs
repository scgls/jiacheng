//************************************************************
//******************************作者：方颖*********************
//******************************时间：2017/3/23 16:04:56*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using BILBasic.User;
using BILBasic.Common;
using System.Data;
using BILBasic.XMLUtil;

namespace BILWeb.OutBarCode
{
    public partial class T_OutBarcode_DB : BILBasic.Basing.Factory.Base_DB<T_OutBarCodeInfo>
    {

        /// <summary>
        /// 添加t_outbarcode
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_OutBarCodeInfo t_outbarcode)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();
        }


        protected override List<string> GetSaveSql(UserModel user, ref T_OutBarCodeInfo model)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_OutBarCodeInfo ToModel(IDataReader reader)
        {
            T_OutBarCodeInfo t_outbarcode = new T_OutBarCodeInfo();

            t_outbarcode.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_outbarcode.VoucherNo = (string)dbFactory.ToModelValue(reader, "VOUCHERNO");
            t_outbarcode.RowNo = dbFactory.ToModelValue(reader, "ROWNO").ToDBString();
            t_outbarcode.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ERPVOUCHERNO");
            t_outbarcode.VoucherType = dbFactory.ToModelValue(reader, "VOUCHERTYPE").ToInt32();
            t_outbarcode.MaterialNo = (string)dbFactory.ToModelValue(reader, "MATERIALNO");
            t_outbarcode.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MATERIALDESC");
            t_outbarcode.CusCode = dbFactory.ToModelValue(reader, "CUSCODE").ToDBString();
            t_outbarcode.CusName = dbFactory.ToModelValue(reader, "CUSNAME").ToDBString();
            t_outbarcode.SupCode = dbFactory.ToModelValue(reader, "SUPCODE").ToDBString();
            t_outbarcode.SupName = dbFactory.ToModelValue(reader, "SUPNAME").ToDBString();
            t_outbarcode.OutPackQty = (decimal?)dbFactory.ToModelValue(reader, "OUTPACKQTY");
            t_outbarcode.InnerPackQty = (decimal?)dbFactory.ToModelValue(reader, "INNERPACKQTY");
            t_outbarcode.VoucherQty = (decimal?)dbFactory.ToModelValue(reader, "VOUCHERQTY");
            t_outbarcode.Qty = (decimal?)dbFactory.ToModelValue(reader, "QTY");
            t_outbarcode.NoPack = dbFactory.ToModelValue(reader, "NOPACK").ToInt32();
            t_outbarcode.PrintQty = (decimal?)dbFactory.ToModelValue(reader, "PRINTQTY");
            t_outbarcode.BarCode = dbFactory.ToModelValue(reader, "BARCODE").ToDBString();
            t_outbarcode.BarcodeType = dbFactory.ToModelValue(reader, "BARCODETYPE").ToInt32();
            t_outbarcode.SerialNo = dbFactory.ToModelValue(reader, "SERIALNO").ToDBString();
            t_outbarcode.BarcodeNo = dbFactory.ToModelValue(reader, "BARCODENO").ToInt32();
            t_outbarcode.OutCount = dbFactory.ToModelValue(reader, "OUTCOUNT").ToInt32();
            t_outbarcode.InnerCount = dbFactory.ToModelValue(reader, "INNERCOUNT").ToInt32();
            t_outbarcode.MantissaQty = (decimal?)dbFactory.ToModelValue(reader, "MANTISSAQTY");
            t_outbarcode.IsRohs = dbFactory.ToModelValue(reader, "ISROHS").ToInt32();
            t_outbarcode.OutBox_ID = dbFactory.ToModelValue(reader, "OUTBOX_ID").ToInt32();
            t_outbarcode.Inner_ID = dbFactory.ToModelValue(reader, "INNER_ID").ToInt32();            
            t_outbarcode.BatchNo = (string)dbFactory.ToModelValue(reader, "BATCHNO");
            //t_outbarcode.ABatchQty = (decimal?)dbFactory.ToModelValue(reader, "ABATCHQTY");
            t_outbarcode.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToInt32();
            t_outbarcode.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_outbarcode.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_outbarcode.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_outbarcode.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_outbarcode.MaterialNoID = dbFactory.ToModelValue(reader, "MATERIALNOID").ToInt32();

            t_outbarcode.StrongHoldCode = dbFactory.ToModelValue(reader, "StrongHoldCode").ToDBString();
            t_outbarcode.StrongHoldName = dbFactory.ToModelValue(reader, "StrongHoldName").ToDBString();
            t_outbarcode.CompanyCode = dbFactory.ToModelValue(reader, "CompanyCode").ToDBString();
            t_outbarcode.ProductBatch = dbFactory.ToModelValue(reader, "ProductBatch").ToDBString();
            t_outbarcode.SupPrdBatch = dbFactory.ToModelValue(reader, "SupPrdBatch").ToDBString();
            t_outbarcode.SupPrdDate = dbFactory.ToModelValue(reader, "SupPrdDate").ToDateTime();
            t_outbarcode.ProductDate = dbFactory.ToModelValue(reader, "ProductDate").ToDateTime();
            t_outbarcode.EDate = dbFactory.ToModelValue(reader, "EDate").ToDateTime();
            t_outbarcode.StoreCondition = dbFactory.ToModelValue(reader, "StoreconDition").ToDBString();
            t_outbarcode.SpecialRequire = dbFactory.ToModelValue(reader, "SpecialRequire").ToDBString();
            t_outbarcode.BarcodeMType = dbFactory.ToModelValue(reader, "BarcodeMType").ToDBString();

            t_outbarcode.RowNoDel = dbFactory.ToModelValue(reader, "RowNoDel").ToDBString();

            t_outbarcode.Unit = dbFactory.ToModelValue(reader, "Unit").ToDBString();
            t_outbarcode.LabelMark = dbFactory.ToModelValue(reader, "LABELMARK").ToDBString();

            t_outbarcode.EAN = dbFactory.ToModelValue(reader, "EAN").ToDBString();
            t_outbarcode.receivetime = dbFactory.ToModelValue(reader, "RECEIVETIME")==null?DateTime.MinValue: (DateTime)dbFactory.ToModelValue(reader, "RECEIVETIME");
            t_outbarcode.WorkNo = dbFactory.ToModelValue(reader, "WorkNo").ToDBString();
            t_outbarcode.ProductClass = dbFactory.ToModelValue(reader, "ProductClass").ToDBString();
            t_outbarcode.InvoiceNo = GetInvoiceNo(t_outbarcode.WorkNo).Trim();
            t_outbarcode.Status = dbFactory.ToModelValue(reader, "Status").ToInt32();
            t_outbarcode.fserialno = dbFactory.ToModelValue(reader, "fserialno").ToDBString();
            t_outbarcode.BarcodeType = dbFactory.ToModelValue(reader, "BarcodeType").ToInt32();
            

            return t_outbarcode;
        }

        public string GetInvoiceNo(string strWorkNo) 
        {
            string strSql = "select Remark from T_PurchaseWorkOrder where WorkNo = '" + strWorkNo + "'";
            return base.GetScalarBySql(strSql).ToDBString();
        }

        protected override string GetViewName()
        {
            return "V_OUTBARCODE";
        }

        protected override string GetTableName()
        {
            return "T_OUTBARCODE";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }


        /// <summary>
        /// 收货条码扫描验证,验证外箱条码或者托盘条码是否已经入库
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool CheckSerialNo(string SerialNo, ref string strError)
        {
            int i = 0;

            string strSql = string.Empty;

            strSql = string.Format("SELECT COUNT(1) FROM T_STOCK WHERE SERIALNO = '{0}' or Palletno = '{1}'", SerialNo,SerialNo);
            i = GetScalarBySql(strSql).ToInt32();

            if (i > 0)
            {
                strError = "该外箱条码或者托盘条码已经收货！";
                return false;
            }

            //strSql = string.Format("SELECT COUNT(1) FROM t_Palletdetail WHERE SERIALNO = '{0}'", SerialNo);
            //i = GetScalarBySql(strSql).ToInt32();

            //if (i > 0)
            //{
            //    strError = "该条码已经拼托！";
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// 检查条码（包材接收）
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool CheckSerialNobyymh(string SerialNo, ref string strError)
        {
            int i = 0;
            string strSql = string.Empty;
            strSql = string.Format("select * from t_tasktrans where tasktype=13 and (serialno='{0}' or barcode='{0}') ", SerialNo);
            i = GetScalarBySql(strSql).ToInt32();

            if (i > 0)
            {
                strError = "该外箱条码已经做过包材接收！";
                return false;
            }
            return true;
        }



        protected override string GetModelSql(T_OutBarCodeInfo model)
        {
            return string.Format("select a.StoreCondition,a.SpecialRequire ,a.Strongholdcode,a.Strongholdname,a.Companycode,a.Supprdbatch, a.Supprddate,a.Productdate,a.Edate,a.Barcodemtype,a.Id, a.Voucherno, a.Rowno, a.Erpvoucherno, a.Vouchertype, a.Cuscode, a.Cusname," +
                                 "a.Supcode, a.Supname, a.Outpackqty, a.Innerpackqty, a.Voucherqty, a.Qty, a.Nopack, a.Printqty, a.Barcode, a.Barcodetype, "+
                                 "a.Serialno, a.Barcodeno, a.Outcount, a.Innercount, a.Mantissaqty, a.Isrohs, a.Outbox_Id, a.Inner_Id, a.PRODUCTBATCH, " +
                                 "a.Batchno, a.Isdel, a.Creater, a.Createtime, a.Modifyer, a.Modifytime, a.Materialnoid,a.rownodel,a.Unit,a.LABELMARK,a.EAN,a.receivetime,a.materialno,a.materialdesc  ,a.productclass,a.workno,a.status,a.fserialno,a.BarCodeType " +
                                 "from t_Outbarcode a where serialno = '{0}'", model.SerialNo);
        }


        /// <summary>
        /// 批量验证收货时，条码在库存中是否已经存在
        /// </summary>
        /// <param name="BarCodeXml"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        public bool CheckBarCodeIsExists(string BarCodeXml, ref string strErrMsg)
        {
            try
            {
                int iResult = 0;
                
                dbFactory.dbF.CreateParameters(3);
                dbFactory.dbF.AddParameters(0, "@OutBarCodeXml", SqlDbType.Xml);
                dbFactory.dbF.AddParameters(1, "@bResult", SqlDbType.Int, 0);
                dbFactory.dbF.AddParameters(2, "@ErrString", SqlDbType.NVarChar, 200);

                dbFactory.dbF.Parameters[0].Value = BarCodeXml;
                dbFactory.dbF.Parameters[1].Direction = System.Data.ParameterDirection.Output;
                dbFactory.dbF.Parameters[2].Direction = System.Data.ParameterDirection.Output;

                dbFactory.ExecuteNonQuery2(dbFactory.ConnectionStringLocalTransaction, CommandType.StoredProcedure, "P_Check_BarCode", dbFactory.dbF.Parameters);
                iResult = Convert.ToInt32(dbFactory.dbF.Parameters[1].Value);
                strErrMsg = dbFactory.dbF.Parameters[2].Value.ToString();

                return iResult == 1 ? true : false;
            //    int iResult = 0;

            //    OracleParameter[] cmdParms = new OracleParameter[] 
            //{
            //    new OracleParameter("OutBarCodeXml", OracleDbType.NClob),
            //    new OracleParameter("bResult", OracleDbType.Int32,ParameterDirection.Output),
            //    new OracleParameter("strErrMsg", OracleDbType.NVarchar2,200,strErrMsg,ParameterDirection.Output)
            //};

            //    cmdParms[0].Value = BarCodeXml;

            //    dbFactory.ExecuteNonQuery3(dbFactory.ConnectionStringLocalTransaction, CommandType.StoredProcedure, "P_Check_BarCode", cmdParms);
            //    iResult = Convert.ToInt32(cmdParms[1].Value.ToString());
            //    strErrMsg = cmdParms[2].Value.ToString();

            //    return iResult == 1 ? true : false;
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据托盘号查找条码
        /// </summary>
        /// <param name="PalletNo"></param>
        /// <returns></returns>
        public List<T_OutBarCodeInfo> GetOutBarCodeByPalletNo(string PalletNo) 
        {
            string strSql = "select b.*,a.palletno from t_Palletdetail a left join t_Outbarcode b on a.barcode = b.barcode " +
                            "where a.Palletno = '"+PalletNo+"'";
            return base.GetModelListBySql(strSql);
        }

        public List<T_OutBarCodeInfo> GetOutBarCodeByPalletNoforCar(string PalletNo)
        {
            List<T_OutBarCodeInfo> modellist = new List<T_OutBarCodeInfo>();
            string strSql = "select pa.*,t_outstock.contact,t_outstock.address,t_outstock.address1,t_outstock.phone from (select c.erpvoucherno, c.suppliername, sum(c.qty) as qty, c.boxcount as count from(select a.palletno,a.boxcount, b.erpvoucherno, a.suppliername, b.qty from t_pallet a left join t_palletdetail b on a.palletno= b.palletno where a.palletno= '" + PalletNo+ "' and a.pallettype= 4) c group by c.erpvoucherno, c.suppliername,c.boxcount) pa left join t_outstock on pa.erpvoucherno=t_outstock.erpvoucherno";
            using (IDataReader reader = dbFactory.ExecuteReader(strSql))
            {
                while (reader.Read())
                {
                    T_OutBarCodeInfo model = new T_OutBarCodeInfo();
                    model.PalletNo = PalletNo;
                    model.ErpVoucherNo = reader["ErpVoucherNo"].ToDBString();
                    model.SupName = reader["suppliername"].ToDBString();
                    model.Qty = reader["qty"].ToInt32();
                    model.OutCount = reader["count"].ToInt32();

                    model.Contact = reader["contact"].ToDBString();
                    model.Address = reader["address"].ToDBString();
                    model.Address1 = reader["address1"].ToDBString();
                    model.Phone = reader["phone"].ToDBString();

                    modellist.Add(model);
                }
                return modellist;
            }
        }

        

        public T_OutBarCodeInfo GetErpBarCode(string strSerialNo) 
        {
            try 
            {
                T_OutBarCodeInfo model = new T_OutBarCodeInfo();
                string strSql = "select a.Barcode,a.Serialno,a.Materialno,a.Materialdesc,a.Batchno," +
                                " a.Edate,a.Qty,b.Erpbarcode from t_Outbarcode a left join t_Material b  on a.Materialnoid = b.Id" +
                                " where a.Serialno = '" + strSerialNo + "'";

                using (IDataReader reader = dbFactory.ExecuteReader(strSql))
                {
                    if (reader.Read())
                    {
                        model.BarCode = reader["BarCode"].ToDBString();
                        model.SerialNo = reader["SerialNo"].ToDBString();
                        model.MaterialNo = reader["MaterialNo"].ToDBString();
                        model.MaterialDesc = reader["Materialdesc"].ToDBString();
                        model.BatchNo = reader["BatchNo"].ToDBString();
                        model.Qty = reader["Qty"].ToDecimal();
                        model.EDate = reader["EDate"].ToDateTime();
                        model.ErpBarCode = reader["Erpbarcode"].ToDBString();

                    }
                    else
                    {
                        model = null;
                    }
                }
                return model;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public List<T_OutBarCodeInfo> GetBarCodeOutAll(string BarCode)
        {
            string strSql = "select * from t_outbarcode where  fserialno='" + BarCode + "'";
            return base.GetModelListBySql(strSql);
        }

        public int GetJBarCodeIsScan(string strJSerialNo) 
        {
            string strSql = "SELECT COUNT(1)  from T_TASKTRANSDETAIL where SERIALNO = '"+strJSerialNo+"'";
            return base.GetScalarBySql(strSql).ToInt32();
        }

    }
}
