using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BILWeb.Login.User;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using BILBasic.DBA;

namespace BILWeb.Excel
{
    public class Excel_DB
    {
        public bool ImportSerialNo(string SerialXml, string strTaskNo,UserInfo user, ref string strErrMsg)
        {
            try
            {
                int iResult = 0;

                OracleParameter[] cmdParms = new OracleParameter[] 
            {
                new OracleParameter("strSerialXml", OracleDbType.NClob),      
                new OracleParameter("TaskNo", OracleDbType.NVarchar2),
                new OracleParameter("strUserNo", OracleDbType.NVarchar2),
                new OracleParameter("bResult", OracleDbType.Int32,ParameterDirection.Output),
                new OracleParameter("strErrMsg", OracleDbType.NVarchar2,200,strErrMsg,ParameterDirection.Output)
            };

                cmdParms[0].Value = SerialXml;
                cmdParms[1].Value = strTaskNo;
                cmdParms[2].Value = user.UserNo;

                dbFactory.ExecuteNonQuery3(dbFactory.ConnectionStringLocalTransaction, CommandType.StoredProcedure, "P_IMPORTSERIAL", cmdParms);
                iResult = Convert.ToInt32(cmdParms[3].Value.ToString());
                strErrMsg = cmdParms[4].Value.ToString();

                return iResult == 1 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
