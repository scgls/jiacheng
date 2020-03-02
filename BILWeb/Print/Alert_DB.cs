using BILBasic.DBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BILBasic.Common;
using BILBasic.User;
using BILBasic.Basing;

namespace BILWeb.Print
{
    public class Alert_DB
    {
        public DbFactory dbFactory = new DbFactory(DbFactory.DbFactoryType.SQLSERVER);

        /// <summary>
        /// 添加推送数据
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="user"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool BatchAddData(List<Alert> lst, UserModel user, ref string msg)
        {
            List<string> lstSql = new List<string>();

            string sql = "";

            try
            {
                foreach (var model in lst)
                {
                    sql = GetInsertSql(model, ref msg);
                    lstSql.Add(sql);
                }

                int i = dbFactory.ExecuteNonQueryList(lstSql, ref msg);

                if (i > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
        private int GetID()
        {
            object id = dbFactory.ExecuteScalar(System.Data.CommandType.Text, "SELECT SEQ_ALERT.NEXTVAL FROM DUAL");
            return Convert.ToInt32(id);
        }

        //ID,MESSAGETYPE,MESSAGEDESC,CREATETIME,SENDTIME,ISRETURN,REMARK,LINENO,USERNO,USINGTIME
        public string GetInsertSql(Alert model, ref string msg)
        {
            int id = GetID();
            string strSql = String.Empty;
            try
            {
                strSql = "insert into MES_ALERT (ID,MESSAGETYPE,MESSAGEDESC,CREATETIME,SENDTIME,ISRETURN,REMARK,LINENO,USERNO,USINGTIME)" +
                                        "values (SEQ_ALERT.NEXTVAL,'"
                                        + model.MESSAGETYPE + "','"
                                        + model.MESSAGEDESC + "',"
                                        + model.CREATETIME.ToOracleTimeString() + ","
                                        + "NULL" + ",'"
                                        + model.ISRETURN + "','"
                                        + model.REMARK + "','"
                                        + model.LINENO + "',"
                                        + "NULL" + ","
                                        + "NULL" + ""
                                        + ")";
                return strSql;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return "";
            }
        }
        
    }
}
