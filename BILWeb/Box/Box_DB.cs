using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using BILBasic.Common;
using BILBasic.User;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using BILWeb.Boxing;
using BILWeb.OutStock;

namespace BILWeb.Box
{
    public partial class T_Box_DB : BILBasic.Basing.Factory.Base_DB<T_BoxInfo>
    {

        /// <summary>
        /// 添加T_AREA
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_BoxInfo model)
        {

            dbFactory.dbF.CreateParameters(31);
            dbFactory.dbF.AddParameters(0, "@bResult", SqlDbType.Int, 0);
            dbFactory.dbF.AddParameters(1, "@ErrorMsg", SqlDbType.NVarChar, 1000);
            dbFactory.dbF.AddParameters(2, "@v_ID", model.ID.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(12, "@v_IsDel", model.IsDel.ToOracleValue(), 0);

            dbFactory.dbF.AddParameters(13, "@v_Creater", model.Creater.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(14, "@v_CreateTime", model.CreateTime.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(15, "@v_Modifyer", model.Modifyer.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(16, "@v_ModifyTime", model.ModifyTime.ToOracleValue(), 0);
            

            dbFactory.dbF.Parameters[0].Direction = System.Data.ParameterDirection.Output;
            dbFactory.dbF.Parameters[1].Direction = System.Data.ParameterDirection.Output;
            dbFactory.dbF.Parameters[2].Direction = System.Data.ParameterDirection.InputOutput;
            //dbFactory.dbF.Parameters[11].Direction = System.Data.ParameterDirection.InputOutput;
            //dbFactory.dbF.Parameters[13].Direction = System.Data.ParameterDirection.InputOutput;

            return dbFactory.dbF.Parameters;

        }

        protected override List<string> GetSaveSql(UserModel user,ref T_BoxInfo t_area)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_BoxInfo ToModel(IDataReader reader)
        {
            T_BoxInfo t_area = new T_BoxInfo();
            T_BoxInfo boxWeight = new T_BoxInfo();

            t_area.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_area.SerialNo = (string)dbFactory.ToModelValue(reader, "SerialNo");
            t_area.DelNo = (string)dbFactory.ToModelValue(reader, "DelNo");
            t_area.HeaderName = (string)dbFactory.ToModelValue(reader, "HeaderName");
            t_area.CustomerName = (string)dbFactory.ToModelValue(reader, "CustomerName");
            t_area.Flag = (string)dbFactory.ToModelValue(reader, "Flag");
            t_area.Remark = (string)dbFactory.ToModelValue(reader, "Remark");
            t_area.Remark2 = (string)dbFactory.ToModelValue(reader, "Remark2");
            t_area.Remark1 = (string)dbFactory.ToModelValue(reader, "Remark1");
            
            t_area.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToInt32();
            t_area.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_area.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_area.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_area.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_area.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ErpVoucherNo");
            t_area.StrCreateTime = t_area.CreateTime.ToShowTime();
            t_area.StrModifyTime = t_area.ModifyTime.ToShowTime();

            boxWeight = GetWeightVolum(t_area);

            t_area.Weight = boxWeight.Weight;
            t_area.Volume = boxWeight.Volume;

            return t_area;

        }

        private T_BoxInfo GetWeightVolum(T_BoxInfo model)
        {
            T_BoxInfo boxmodel = new T_BoxInfo();
            string strSql = string.Empty;
            string strSpltSerial = null;
            if (model.Flag == "整")
            {
                strSql = "SELECT (ISNULL(B.WEIGHT,0) * A.QTY ) AS WEIGHT, ( ISNULL(B.VOLUME,0) * A.QTY ) AS VOLUME from T_TASKTRANS A " +
                         " LEFT JOIN T_MATERIAL B ON A.MATERIALNO = B.MATERIALNO " +
                         " where ERPVOUCHERNO = '" + model.ErpVoucherNo + "' and TASKTYPE = 12 AND ISNULL(ISAMOUNT,0) = 1 AND SERIALNO = '" + model.SerialNo + "'";
            }
            else 
            {
                foreach (var item in model.SerialNo.Split(',')) 
                {
                    strSpltSerial += "'" + item + "'" + ",";
                }
                strSpltSerial = strSpltSerial.TrimEnd(',');

                strSql = "SELECT SUM(WEIGHT) AS WEIGHT,SUM(VOLUME) AS VOLUME FROM ( " +
                              " SELECT (ISNULL(C.WEIGHT,0) * B.QTY ) AS WEIGHT, ( ISNULL(C.VOLUME,0) * B.QTY ) AS VOLUME FROM (SELECT A.materialno, SUM(A.qty) AS QTY FROM t_Boxing A " +
                              " WHERE A.SERIALNO IN (" + strSpltSerial + ")  GROUP BY A.materialno) B " +
                              " LEFT JOIN T_MATERIAL C ON B.materialno = C.MATERIALNO) D";
            }

            using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
            {
                while (dr.Read())
                {
                    boxmodel.Weight = dbFactory.ToModelValue(dr, "Weight").ToDecimal();
                    boxmodel.Volume = dbFactory.ToModelValue(dr, "Volume").ToDecimal();                    
                }
            }

            return boxmodel;
        }


        protected override string GetViewName()
        {
            return "V_Box";
        }

        protected override string GetTableName()
        {
            return "T_Box";
        }

        protected override string GetSaveProcedureName()
        {
            return "P_SAVE_T_Box";
        }

        protected override string GetDeleteProcedureName()
        {
            return "P_DELETE_T_Box";
        }

        protected override string GetFilterSql(UserModel user, T_BoxInfo model)
        {
            string strSql = " where isnull(isDel,0) != 2";

            string strAnd = " and ";

            if (!string.IsNullOrEmpty(model.DelNo))
            {
                strSql += strAnd;
                strSql += " DelNo = '" + model.DelNo + "'  ";
            }

            if (!string.IsNullOrEmpty(model.SerialNo))
            {
                strSql += strAnd;
                strSql += "  SerialNo like '%" + model.SerialNo + "%' ";
            }

            if (!string.IsNullOrEmpty(model.Flag))
            {
                strSql += strAnd;
                strSql += " Flag = '" + model.Flag + "' ";
            }

            if (!string.IsNullOrEmpty(model.HeaderName))
            {
                strSql += strAnd;
                strSql += " HeaderName like '%" + model.HeaderName + "%' ";
            }

            if (!string.IsNullOrEmpty(model.CustomerName))
            {
                strSql += strAnd;
                strSql += " CustomerName like '%" + model.CustomerName + "%' ";
            }

            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " ErpVoucherNo like '%" + model.ErpVoucherNo + "%' ";
            }
            //if (!Common_Func.IsNullOrEmpty(model.Creater))
            //{
            //    strSql += strAnd;
            //    strSql += " Creater Like '%" + model.Creater + "%' ";
            //}


            return strSql;
        }


        protected List<T_BoxInfo> GetModelsbyfilter(string filter,string flag)
        {
            List<T_BoxInfo> lstBoxInfo = new List<T_BoxInfo>();
            try
            {
                string strsql = "";
                if (flag == "1")
                {
                    strsql = "select * from t_box where serialno = (select id from t_box where id = " + filter + ")";
                }
                else
                {
                    strsql = "select* from t_box where id in (" + filter + ")";
                }

                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strsql))
                {
                    while (dr.Read())
                    {
                        T_BoxInfo boxmodel = new T_BoxInfo();
                        boxmodel.ID = dbFactory.ToModelValue(dr, "ID").ToInt32();
                        boxmodel.SerialNo = (string)dbFactory.ToModelValue(dr, "SerialNo");
                        boxmodel.DelNo = (string)dbFactory.ToModelValue(dr, "DelNo");
                        boxmodel.HeaderName = (string)dbFactory.ToModelValue(dr, "HeaderName");
                        boxmodel.CustomerName = (string)dbFactory.ToModelValue(dr, "CustomerName");
                        boxmodel.Flag = (string)dbFactory.ToModelValue(dr, "Flag");
                        boxmodel.Remark = (string)dbFactory.ToModelValue(dr, "Remark");
                        boxmodel.Remark2 = (string)dbFactory.ToModelValue(dr, "Remark2");
                        boxmodel.Remark1 = (string)dbFactory.ToModelValue(dr, "Remark1");
                        boxmodel.ErpVoucherNo = (string)dbFactory.ToModelValue(dr, "ErpVoucherNo");
                        lstBoxInfo.Add(boxmodel);
                    }
                }
                return lstBoxInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
           

        }

        public List<T_BoxInfo> GetPrintBoxInfo(string strErpVoucherNo) 
        {
            List<T_BoxInfo> lstBox = new List<T_BoxInfo>();
            string strSql = "SELECT * from ( " +
                            " SELECT SERIALNO,'整' as Flag,ISAMOUNT from T_TASKTRANS where ERPVOUCHERNO = '" + strErpVoucherNo + "' and TASKTYPE = 12 AND ISNULL(ISAMOUNT,0) = 1 " +
                            " UNION ALL " +
                            " SELECT serialno,'零' as  Flag,'2'  from t_Boxing where erpvoucherno = '" + strErpVoucherNo + "' and ISNULL(ispin,1) = 1 GROUP by serialno " +
                            " ) as c";
            using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
            {
                while (dr.Read())
                {
                    T_BoxInfo boxmodel = new T_BoxInfo();
                    boxmodel.SerialNo = (string)dbFactory.ToModelValue(dr, "SerialNo");                    
                    boxmodel.Flag = (string)dbFactory.ToModelValue(dr, "Flag");
                    boxmodel.IsAmount = dbFactory.ToModelValue(dr, "IsAmount").ToInt32();
                    //boxmodel.FserialNo = dbFactory.ToModelValue(dr, "IsAmount").ToDBString();
                    //boxmodel.IsPin = dbFactory.ToModelValue(dr, "IsPin").ToInt32();
                    lstBox.Add(boxmodel);
                }
            }
            return lstBox;
        }

        public List<T_BoxInfo> GetSerialNoByFserialNo(string strSerialNo)
        {
            List<T_BoxInfo> lstBox = new List<T_BoxInfo>();

            string strSql = "SELECT serialno from t_Boxing where fserialno = '"+strSerialNo+"'";

            using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
            {
                while (dr.Read())
                {
                    T_BoxInfo boxmodel = new T_BoxInfo();
                    boxmodel.SerialNo = (string)dbFactory.ToModelValue(dr, "SerialNo");                    
                    lstBox.Add(boxmodel);
                }
            }
            return lstBox;
        }

        public bool SaveBoxByModelList(UserModel user,List<T_BoxInfo> modelList, ref string strError) 
        {
            try 
            {
                string strSql = string.Empty;
                List<string> lstSql = new List<string>();

                foreach (var item in modelList)
                {
                    strSql = "INSERT INTO [T_Box]([SerialNo],[DelNo],[HeaderName] ,[CustomerName],[Flag] ,[Remark] ,[creater],[createtime],[isdel],[ErpVoucherNo])" +
                            " values ('" + item.SerialNo + "','" + item.DelNo + "','" + item.HeaderName + "','" + item.CustomerName + "','" + item.Flag + "','"+item.Remark+"','" + user.UserName + "',getdate(),'1','" + item.ErpVoucherNo + "')";
                    lstSql.Add(strSql);
                }

                return base.SaveModelListBySqlToDB(lstSql, ref strError);

            }
            catch(Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        

        public List<T_BoxingInfo> GetMessageForPrint(string filter, string flag)
        {
            List<T_BoxingInfo> lstBoxInfo = new List<T_BoxingInfo>();
            try
            {
                string strsql = "";
                if (flag == "1")
                {
                    strsql = "select palletno from t_palletdetail where barcode = '" + filter + "'";
                }
                if (flag == "2")
                {
                    strsql = "select top 1 t_outbarcode.materialno,t_outbarcode.materialdesc,t_outbarcode.qty,t_outbarcode.serialno,v.* from t_outbarcode left join t_tasktrans on t_outbarcode.barcode=t_tasktrans.barcode left join (select CUSTOMERNAME,ERPVOUCHERNO,fromerpwarehousename,toerpwarehousename,erpnote from  v_outstockdetail) v on v.erpvoucherno=t_tasktrans.erpvoucherno where t_outbarcode.barcode='" + filter+"' and t_tasktrans.tasktype=2";
                }
                if (flag == "3")
                {
                    strsql = "select t_boxing.*,v.* from t_boxing left join (select top 1 CUSTOMERNAME,ERPVOUCHERNO,fromerpwarehousename,toerpwarehousename,erpnote from  v_outstockdetail where erpvoucherno = '"+ filter + "' ) v on v.erpvoucherno = t_boxing.erpvoucherno where t_boxing.erpvoucherno = '"+filter+"'";
                }

                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strsql))
                {
                    while (dr.Read())
                    {
                        if (flag == "1")
                        {
                            T_BoxingInfo boxmodel = new T_BoxingInfo();
                            boxmodel.SerialNo = (string)dbFactory.ToModelValue(dr, "palletno");
                            lstBoxInfo.Add(boxmodel);
                        }
                        if (flag == "2")
                        {
                            T_BoxingInfo boxmodel = new T_BoxingInfo();
                            boxmodel.MaterialNo = (string)dbFactory.ToModelValue(dr, "materialno");
                            boxmodel.MaterialName = (string)dbFactory.ToModelValue(dr, "materialdesc");
                            boxmodel.Qty =dbFactory.ToModelValue(dr, "qty").ToDecimal();
                            boxmodel.SerialNo = (string)dbFactory.ToModelValue(dr, "SerialNo");
                            boxmodel.Remark1 = (string)dbFactory.ToModelValue(dr, "ERPVOUCHERNO") + ";" + (string)dbFactory.ToModelValue(dr, "CUSTOMERNAME") + ";" + (string)dbFactory.ToModelValue(dr, "fromerpwarehousename") + ";" + (string)dbFactory.ToModelValue(dr, "toerpwarehousename") + ";" + (string)dbFactory.ToModelValue(dr, "erpnote");
                            lstBoxInfo.Add(boxmodel);
                        }
                        if (flag == "3")
                        {
                            T_BoxingInfo boxmodel = new T_BoxingInfo();
                            boxmodel.MaterialNo = (string)dbFactory.ToModelValue(dr, "MaterialNo");
                            boxmodel.MaterialName = (string)dbFactory.ToModelValue(dr, "MaterialName");
                            boxmodel.TaskNo = (string)dbFactory.ToModelValue(dr, "TaskNo");
                            boxmodel.ErpVoucherNo = (string)dbFactory.ToModelValue(dr, "ErpVoucherNo");
                            boxmodel.Remark = (string)dbFactory.ToModelValue(dr, "Remark");
                            boxmodel.SerialNo = (string)dbFactory.ToModelValue(dr, "SerialNo");
                            boxmodel.Qty = dbFactory.ToModelValue(dr, "qty").ToDecimal();
                            boxmodel.Remark1 = (string)dbFactory.ToModelValue(dr, "ERPVOUCHERNO") + ";" + (string)dbFactory.ToModelValue(dr, "CUSTOMERNAME") + ";" + (string)dbFactory.ToModelValue(dr, "fromerpwarehousename") + ";" + (string)dbFactory.ToModelValue(dr, "toerpwarehousename") + ";" + (string)dbFactory.ToModelValue(dr, "erpnote");
                            lstBoxInfo.Add(boxmodel);
                        }
                    }
                }
                return lstBoxInfo;
            }
            catch (Exception ex)
            {
                return null;
            }


        }

        public bool GetErpVoucherNoIsPrint(string strErpVoucherNo,ref string strError) 
        {
            try
            {
                bool bSucc = false;
                string strSql = "SELECT COUNT(1) from T_Box where erpvoucherno like '%"+strErpVoucherNo+"%'";
                int i =  base.GetScalarBySql(strSql).ToInt32();

                if (i > 0)
                {
                    strError = "订单:"+strErpVoucherNo+"已经生成物流标签，不能重复生成！";
                    bSucc = false;
                }
                else 
                {
                    bSucc = true;
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
        /// 根据散装箱码返回数据
        /// </summary>
        /// <param name="strSerialNo"></param>
        /// <returns></returns>
        public List<T_BoxingInfo> GetModelBySerial(string strSerialNo) 
        {
            try
            {
                T_BoxingInfo boxmodel = new T_BoxingInfo();
                List<T_BoxingInfo> modelList = new List<T_BoxingInfo>();
                string strsql = "SELECT * from t_Boxing where serialno = '" + strSerialNo + "'";
                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strsql))
                {
                    while (dr.Read())
                    {
                        boxmodel.ID = dbFactory.ToModelValue(dr, "ID").ToInt32();
                        boxmodel.MaterialNo = dbFactory.ToModelValue(dr, "MaterialNo").ToDBString();
                        boxmodel.MaterialName = dbFactory.ToModelValue(dr, "MaterialName").ToDBString();
                        boxmodel.Qty = dbFactory.ToModelValue(dr, "Qty").ToDecimal();
                        boxmodel.SerialNo = dbFactory.ToModelValue(dr, "SerialNo").ToDBString();
                        boxmodel.TaskNo = dbFactory.ToModelValue(dr, "TaskNo").ToDBString();
                        boxmodel.Status = dbFactory.ToModelValue(dr, "Status").ToInt32();
                        boxmodel.IsDel = dbFactory.ToModelValue(dr, "Creater").ToInt32();
                        boxmodel.ErpVoucherNo = dbFactory.ToModelValue(dr, "ErpVoucherNo").ToDBString();
                        boxmodel.FserialNo = dbFactory.ToModelValue(dr, "FserialNo").ToDBString();
                        boxmodel.CustomerNo = dbFactory.ToModelValue(dr, "CustomerNo").ToDBString();
                        boxmodel.CustomerName = dbFactory.ToModelValue(dr, "CustomerName").ToDBString();
                        modelList.Add(boxmodel);
                    }
                }

                return modelList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<T_BoxingInfo> GetFserialNoByErpvoucherNo(List<T_OutStockInfo> modelListOutStock) 
        {
            try
            {
                
                List<T_BoxingInfo> modelList = new List<T_BoxingInfo>();
                string strErpvoucherNo = string.Empty;

                foreach (var item in modelListOutStock) 
                {
                    strErpvoucherNo += "\'" + item.ErpVoucherNo + "\'" + ",";
                }

                strErpvoucherNo = strErpvoucherNo.TrimEnd(',');

                string strsql = "SELECT ISNULL(fserialno,'') as fserialno FROM t_Boxing WHERE erpvoucherno IN (" + strErpvoucherNo + ") GROUP BY fserialno";
                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strsql))
                {
                    while (dr.Read())
                    {
                        T_BoxingInfo boxmodel = new T_BoxingInfo();
                        boxmodel.FserialNo = dbFactory.ToModelValue(dr, "FserialNo").ToDBString();                        
                        modelList.Add(boxmodel);
                    }
                }

                if (modelList != null && modelList.Count > 0) 
                {
                    modelList = modelList.Where(t => t.FserialNo != "").ToList();
                }

                return modelList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public List<T_OutStockInfo> GetErpvoucherNoByFSerialNo(List<T_BoxingInfo> modelListFserialNo)
        {
            try
            {

                List<T_OutStockInfo> modelList = new List<T_OutStockInfo>();
                string strFserialno = string.Empty;

                foreach (var item in modelListFserialNo)
                {
                    strFserialno += "\'" + item.FserialNo + "\'" + ",";
                }

                strFserialno = strFserialno.TrimEnd(',');

                string strsql = "SELECT ISNULL(erpvoucherno,'') erpvoucherno from t_Boxing  where fserialno IN(" + strFserialno + ") GROUP BY erpvoucherno";
                using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strsql))
                {
                    while (dr.Read())
                    {
                        T_OutStockInfo boxmodel = new T_OutStockInfo();
                        boxmodel.ErpVoucherNo = dbFactory.ToModelValue(dr, "erpvoucherno").ToDBString();
                        modelList.Add(boxmodel);
                    }
                }

                return modelList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
