using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BILWeb.SyncService
{
    public class SqlSugarBase
    {
        public static  SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = ConfigurationManager.ConnectionStrings["ConnOracleWithAddress"].ConnectionString, DbType = DbType.SqlServer, IsAutoCloseConnection = true });// "Data Source=192.168.100.86;Initial Catalog=ABH_SCG;Persist Security Info=True;User ID=sa;Password=chinetek;Persist Security Info=True;"
            return db;
        }
    }
}
