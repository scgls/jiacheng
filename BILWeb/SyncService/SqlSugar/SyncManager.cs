using BILBasic.Basing;
using BILBasic.DBA;
using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BILWeb.SyncService.SqlSugar
{
    public class SyncManager:DbContext<Sync_Model>
    {
        private static  SyncManager instance = null;
        public DbFactory dbFactory = new DbFactory(DbFactory.DbFactoryType.SQLSERVER);
    

        public SyncManager()
        {
           
        }

        public static SyncManager GetInstance()
        {
           // if (instance == null)
            {
                instance = new SyncManager();
            }
            return instance;
        }

        public  List<Sync_Model> GetSyncModelList(int stockType,string erpVoucherNo, int wmsVourcherType,int autoSync)
        {

            if (!String.IsNullOrEmpty(erpVoucherNo) && wmsVourcherType == -1)
            {
                string[] tables = stockType == 10 ? new string[] { "T_INSTOCK", "T_QUALITY" } : new string[] { "T_OUTSTOCK" };
                foreach(string table in tables)
                {
                    wmsVourcherType = Db.Ado.GetInt("SELECT VOUCHERTYPE FROM "+ table + " WHERE ERPVOUCHERNO='" + erpVoucherNo + "'");
                    if (wmsVourcherType != 0) break;
                }
                if (wmsVourcherType == 0) wmsVourcherType = -1;
              
            }
            return Db.Queryable<Sync_Model>()
                    .Where(it => it.InStockType == stockType)
                    .Where(it => it.AutoSync == autoSync)
                    .WhereIF(wmsVourcherType != -1, it => it.WmsType == wmsVourcherType.ToString())
                    .OrderBy(it=>it.WmsField).ToList();
    
        }

        public void GetDetailCountByErpVoucherNo(string TableName,string ErpVoucherNo,ref int detailCount,ref string wmsVoucherNo)
        {
            string sql = String.Format(SqlModel.SelectDetailCount, TableName, ErpVoucherNo);
            IDataReader dr = Db.Ado.GetDataReader(sql);
            try
            {
                if (dr.Read())
                { 
                    detailCount = dr["num"] != null ? Convert.ToInt32(dr["num"]) : 0;
                    wmsVoucherNo = dr["VOUCHERNO"] != null ? dr["VOUCHERNO"].ToString() : "";
                }
                dr.Close();

            }
            catch
            {
                dr.Close();
            }
        }


        public int CheckVoucherNoExit(int stockType,string WmsTableH, string whereString)
        {
            string checkVoucherNoExit = "select ID,status from {0} where {1}";
            checkVoucherNoExit = String.Format(checkVoucherNoExit, WmsTableH, whereString);
            IDataReader dr = Db.Ado.GetDataReader(checkVoucherNoExit);
            try
            {
                int id = 0;
                if (dr.Read()) {
                    // string[] headKeys = pmListbyType[0].WmsHeadKeys.Split(',');
                    id = dr["ID"] != null ? Convert.ToInt32(dr["ID"]) : 0;
                
                    int status = dr["status"] != null ? Convert.ToInt32(dr["status"]) : 0;
                    // if ((stockType == 10 && status == 3) || (stockType == 20 && status == 4))
                     if (status!=1)
                        id = -99; //单据WMS已操作，不需要更新
                }
                dr.Close();
                return id;
                    
            }
            catch
            {
                dr.Close();
                return -99;
            }

        }

        public string GetSAPwhereString(bool isHead, string[] keys, List<Sync_Model> pmListbyType, JToken jsonObject)
        {
            string whereString = String.Empty;
            foreach (string key in keys)
            {
                int index = pmListbyType.FindIndex(p => p.WmsField.ToUpper() == key.ToUpper() && p.FieldHD.ToUpper() == (isHead ? "H" : "D"));
                if (index != -1)
                {
                    whereString += pmListbyType[index].WmsField + "='" + jsonObject[pmListbyType[index].ErpField].ToString() + "' and ";
                }
            }

            return whereString + " 1=1";
        }

        public bool Executrans(List<string> SQList, ref string errMsg)
        {
            int ret = 0;
            try
            {
                //ret = SqlServerDBHelper.ExecuteSqlTran(SQList);
                ret = dbFactory.ExecuteSqlTran(SQList);
                //if (ret == 0)
                //{
                //    errMsg = "执行SQL语句失败！";
                //}
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ret!=0;
        }
    }
}
