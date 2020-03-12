using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILBasic.Basing
{
    /// <summary>
    /// 数据工厂类 
    /// </summary>
   public  class DbFactory
    {

        /// <summary>
        /// 枚举
        /// </summary>
        public enum DbFactoryType
        {
           SQLSERVER = 0,
            ROACLE = 1,
        }


        //数据库工厂接口 每个需要操作数据库的操作界面需要添加该对象 
       public   IDBFactory dbF;

        //server=180.166.173.214;database=ZTWMS;uid=sa;password=chinetek//server=192.168.45.2;database=SCG_ABH;uid=sa;password=SCG@123456;pooling=false
        //   public readonly string ConnectionStringLocalTransaction = "server=192.168.45.2;database=ZTWMS;uid=sa;password=SCG@123456";
        //server=192.168.100.86;database=ABH_SCG;uid=sa;password=chinetek
       
        public readonly string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["ConnOracleWithAddress"]==null? "server=192.168.100.86;database=ABH_SCG;uid=sa;password=chinetek" : ConfigurationManager.ConnectionStrings["ConnOracleWithAddress"].ToString();
        //   public readonly string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["ConnOracleWithAddress"] == null ? "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = 180.166.173.214)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = dswms)));Persist Security Info=True;User ID=c##dswms;Password=c##dswms;" : ConfigurationManager.ConnectionStrings["ConnOracleWithAddress"].ConnectionString;//防止空引用异常 modify by gzw 181227

        /// <summary>
        /// 数据库工厂构造函数
        /// </summary>
        /// <param name="dbtype">数据库枚举</param>
        public DbFactory(DbFactoryType dbtype)
        {
            switch (dbtype)
            {
                case DbFactoryType.SQLSERVER:
                    dbF = new SqlServerFactory
                    {
                        ConnStr = ConnectionStringLocalTransaction


                    };// 数据库连接字符
                    SqlConnection connection = (SqlConnection)dbF.CreateConnection();
                    //SqlBulkCopy
                    //connection.BulkCopy
                    break;
                case DbFactoryType.ROACLE:
                    dbF = new OracleFactory
                    {
                        ConnStr = ConnectionStringLocalTransaction
                    };
                    break;
            }
        }

        #region 批量添加
        /// <summary>
        /// 批量保存多表
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public  void SqlBatchCopy(Dictionary<DataTable,string> dt,ref string str)
        {
            try
            {
                
                using(IDbConnection conn = dbF.CreateConnection())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    IDbTransaction trans = conn.BeginTransaction();
                    foreach (var item in dt)
                    {
                        using (SqlBulkCopy copy = new SqlBulkCopy((SqlConnection)conn, SqlBulkCopyOptions.Default, (SqlTransaction)trans))
                        {
                            for (int i = 0; i < item.Key.Columns.Count; i++)
                            {
                                copy.ColumnMappings.Add(item.Key.Columns[i].ColumnName, item.Key.Columns[i].ColumnName);
                            }
                            copy.DestinationTableName = item.Value;
                            copy.WriteToServer(item.Key);
                        }

                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                str = "添加失败";
                LogNet.ErrorInfo(ex.ToString());
                throw;
            }
           
        }
        #endregion


        #region 抽象数据方法

        //获取数据库连接字符串，其属于静态变量且只读，项目中所有文档可以直接使用，但不能修改
        //public  readonly string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["ConnOracle"].ConnectionString;
        //public  readonly string ConnectionStringLocalTransaction = "Password=123456;User ID=jxbarcode;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=jingxinwms)));";
        // 哈希表用来存储缓存的参数信息，哈希表可以存储任意类型的参数。
        private Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        ///执行一个不需要返回值的IDbCommand命令，通过指定专用的连接字符串。
        /// 使用参数数组形式提供参数列表 
        /// </summary>
        /// <remarks>
        /// 使用示例：
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">IDbCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供IDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此IDbCommand命令执行后影响的行数</returns>
        public  int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
         
            if (string.IsNullOrEmpty(connectionString))
                connectionString = ConnectionStringLocalTransaction;
            IDbCommand cmd = dbF.CreateCommand();
            using(IDbConnection conn =dbF.CreateConnection())
            {
                //通过PrePareCommand方法将参数逐个加入到IDbCommand的参数集合中
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                //清空IDbCommand中的参数列表
                cmd.Parameters.Clear();
                conn.Close();
                return val;
            }
        }

        public  int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            IDbCommand cmd = dbF.CreateCommand();

            using (IDbConnection conn = dbF.CreateConnection())
            {
                //通过PrePareCommand方法将参数逐个加入到IDbCommand的参数集合中
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();

                //清空IDbCommand中的参数列表
                cmd.Parameters.Clear();
                conn.Close();
                return val;
            }
        }
        public  int ExecuteNonQuery2(string connectionString, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
                connectionString = ConnectionStringLocalTransaction;

             IDbCommand cmd = dbF.CreateCommand();

             using (IDbConnection conn = dbF.CreateConnection())
            {
                //通过PrePareCommand方法将参数逐个加入到IDbCommand的参数集合中
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();

                //清空IDbCommand中的参数列表
                cmd.Parameters.Clear();
                conn.Close();
                return val;
            }
        }


        /// <summary>
        /// 只用于下架提交数据
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public  int ExecuteNonQuery3(string connectionString, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {

             IDbCommand cmd = dbF.CreateCommand();

             using (IDbConnection conn = dbF.CreateConnection())
            {
                cmd.CommandTimeout = 25;
                //通过PrePareCommand方法将参数逐个加入到IDbCommand的参数集合中
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();

                //清空IDbCommand中的参数列表
                cmd.Parameters.Clear();
                conn.Close();
                return val;
            }
        }

        public  int ExecuteNonQuery3(ref string strTaskNo, string connectionString, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {

             IDbCommand cmd = dbF.CreateCommand();

             using (IDbConnection conn = dbF.CreateConnection())
            {
                //通过PrePareCommand方法将参数逐个加入到IDbCommand的参数集合中
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                strTaskNo = ((ICloneable)cmd.Parameters["strTaskNo"]).ToString();
                //清空IDbCommand中的参数列表
                cmd.Parameters.Clear();
                conn.Close();
                return val;
            }
        }

        public  int ExecuteNonQuery2(CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {

             IDbCommand cmd = dbF.CreateCommand();

          using(IDbConnection conn =dbF.CreateConnection())
            {
                //通过PrePareCommand方法将参数逐个加入到IDbCommand的参数集合中
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();

                //清空IDbCommand中的参数列表
                cmd.Parameters.Clear();
                conn.Close();
                return val;
            }
        }

        /// <summary>
        ///执行一条不返回结果的IDbCommand，通过一个已经存在的数据库连接 
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">一个现有的数据库连接</param>
        /// <param name="commandType">IDbCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供IDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此IDbCommand命令执行后影响的行数</returns>
        public  int ExecuteNonQuery(DbConnection connection, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {

             IDbCommand cmd = dbF.CreateCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            connection.Close();
            return val;
        }



        /// <summary>
        /// 执行一条不返回结果的IDbCommand，通过一个已经存在的数据库事物处理 
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例： 
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">一个存在的 sql 事物处理</param>
        /// <param name="commandType">IDbCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供IDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此IDbCommand命令执行后影响的行数</returns>
        public  int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
             IDbCommand cmd = dbF.CreateCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }



        /// <summary>
        /// 执行一条返回结果集的IDbCommand命令，通过专用的连接字符串。
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  OracleDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">IDbCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供IDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个包含结果的OracleDataReader</returns>
        public IDataReader ExecuteReader(CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
             IDbCommand cmd = dbF.CreateCommand();
              IDbConnection conn = dbF.CreateConnection();

            // 在这里使用try/catch处理是因为如果方法出现异常，则OracleDataReader就不存在，
            //CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            //关闭数据库连接，并通过throw再次引发捕捉到的异常。
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception ex)
            {
                conn.Close();
                Console.WriteLine(ex.Message);

                throw;
            }
        }


        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public IDataReader ExecuteReader(string strSQL)
        {
            IDbConnection connection = dbF.CreateConnection();
            IDbCommand cmd = dbF.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = strSQL;
            cmd.CommandTimeout = 200;
            //Log.WriteLog(cmd.CommandText);
            try
            {
                connection.Open();
                IDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
               // myReader.Close();
                return myReader;
              
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw e;
            }


        }

        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public  IDataReader ExecuteReader(string strSQL, string connectString)
        {
            if (string.IsNullOrEmpty(connectString))
                connectString = ConnectionStringLocalTransaction;
            IDbConnection connection = dbF.CreateConnection();
            IDbCommand cmd = dbF.CreateCommand();
            cmd.CommandText = strSQL;
            cmd.Connection = connection;
            cmd.CommandTimeout = 200;
            //Log.WriteLog(cmd.CommandText);
            try
            {
                connection.Open();
                IDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw e;
            }

        }

        /// <summary>
        /// 执行一条返回结果集的IDbCommand命令，通过专用的连接字符串。
        /// 使用参数数组提供参数,通过out sys_refcursor参数返回结果集
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  OracleDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">IDbCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供IDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个包含结果的OracleDataReader</returns>
        public  IDataReader ExecuteReaderForCursor(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
             IDbCommand cmd = dbF.CreateCommand();
            IDbConnection conn = dbF.CreateConnection();

            // 在这里使用try/catch处理是因为如果方法出现异常，则OracleDataReader就不存在，
            //CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            //关闭数据库连接，并通过throw再次引发捕捉到的异常。
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception ex)
            {
                conn.Close();
                Console.WriteLine(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// 执行一条返回第一条记录第一列的IDbCommand命令，通过专用的连接字符串。 
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">IDbCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供IDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
        public  object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
             IDbCommand cmd = dbF.CreateCommand();

            using (IDbConnection connection =dbF.CreateConnection())
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public  object ExecuteScalar(string connectString, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectString))
                connectString = ConnectionStringLocalTransaction;
             IDbCommand cmd = dbF.CreateCommand();

            using (IDbConnection connection = dbF.CreateConnection())
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }


        //CREATE BY XUP 特殊情况
        public  object ExecuteScalarTB(IDbConnection conn, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
             IDbCommand cmd = dbF.CreateCommand();
            //判断数据库连接状态
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;

        }

        /// <summary>
        /// 执行一条返回第一条记录第一列的IDbCommand命令，通过已经存在的数据库连接。
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例： 
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">一个已经存在的数据库连接</param>
        /// <param name="commandType">IDbCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供IDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
        public  object ExecuteScalar(IDbConnection connection, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {

             IDbCommand cmd = dbF.CreateCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            connection.Close();
            return val;
        }




        /// <summary>
        /// 缓存参数数组
        /// </summary>
        /// <param name="cacheKey">参数缓存的键值</param>
        /// <param name="cmdParms">被缓存的参数列表</param>
        public  void CacheParameters(string cacheKey, params DbParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// 获取被缓存的参数
        /// </summary>
        /// <param name="cacheKey">用于查找参数的KEY值</param>
        /// <returns>返回缓存的参数数组</returns>
        public  DbParameter[] GetCachedParameters(string cacheKey)
        {
            DbParameter[] cachedParms = (DbParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            //新建一个参数的克隆列表
            DbParameter[] clonedParms = new DbParameter[cachedParms.Length];

            //通过循环为克隆参数列表赋值
            for (int i = 0, j = cachedParms.Length; i < j; i++)
                //使用clone方法复制参数列表中的参数
                clonedParms[i] = (DbParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// 为执行命令准备参数
        /// </summary>
        /// <param name="cmd">IDbCommand 命令</param>
        /// <param name="conn">已经存在的数据库连接</param>
        /// <param name="trans">数据库事物处理</param>
        /// <param name="cmdType">IDbCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="cmdText">Command text，T-SQL语句 例如 Select * from Products</param>
        /// <param name="cmdParms">返回带参数的命令</param>
        private  void PrepareCommand(IDbCommand cmd, IDbConnection conn, DbTransaction trans, CommandType cmdType, string cmdText, IDbDataParameter[] cmdParms)
        {

            //判断数据库连接状态
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            //判断是否需要事物处理
            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (DbParameter parm in cmdParms)
                {

                    if (parm.Value == null)
                    {
                        parm.Value = DBNull.Value;

                    }

                    //switch (parm.OracleDbType)
                    //{


                    //    case OracleDbType.Date:
                    //        cmd.CommandText = cmd.CommandText.Replace(parm.ParameterName + " ", "TO_DATE('" + parm.Value.ToString() + "','yyyy-mm-dd hh24:mi:ss' )");
                    //        break;
                    //    case OracleDbType.Varchar2:
                    //    case OracleDbType.NVarchar2:
                    //        cmd.CommandText = cmd.CommandText.Replace(parm.ParameterName + " ", "'" + parm.Value + "'");
                    //        break;
                    //    default:
                    //        cmd.CommandText = cmd.CommandText.Replace(parm.ParameterName + " ", parm.Value.ToString());
                    //        break;
                    //}

                    cmd.Parameters.Add(parm);
                }
            }
        }

        private  void PrepareCommand2(IDbCommand cmd, IDbConnection conn, DbTransaction trans, CommandType cmdType, string cmdText, IDbDataParameter[] cmdParms)
        {

            //判断数据库连接状态
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            //判断是否需要事物处理
            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (DbParameter parm in cmdParms)
                {

                    if (parm.Value == null)
                    {
                        parm.Value = DBNull.Value;

                    }


                    cmd.Parameters.Add(parm);
                }
            }
        }

        public  DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {

             IDbCommand cmd = dbF.CreateCommand();

          using(IDbConnection conn =dbF.CreateConnection())
            {
                //通过PrePareCommand方法将参数逐个加入到IDbCommand的参数集合中 
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                IDbDataAdapter sda = dbF.CreateDataAdapter();
                sda.InsertCommand = cmd;
                sda.DeleteCommand = cmd;
                sda.SelectCommand = cmd;
                sda.UpdateCommand = cmd;
                DataSet ds = new DataSet();
                sda.Fill(ds);
                //清空IDbCommand中的参数列表 
                cmd.Parameters.Clear();
                conn.Close();
                return ds; //注意:这里改了
            }
        }
        public  DataSet ExecuteDataSet(string connectionString, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {

             IDbCommand cmd = dbF.CreateCommand();

             using (IDbConnection conn = dbF.CreateConnection())
            {
                //通过PrePareCommand方法将参数逐个加入到IDbCommand的参数集合中 
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                IDbDataAdapter sda = dbF.CreateDataAdapter();
                sda.InsertCommand = cmd;
                sda.DeleteCommand = cmd;
                sda.SelectCommand = cmd;
                sda.UpdateCommand = cmd;
                DataSet ds = new DataSet();
                sda.Fill(ds);

                //清空IDbCommand中的参数列表 
                cmd.Parameters.Clear();
                conn.Close();

                return ds; //注意:这里改了
            }
        }

        public DataSet ExecuteDataSetForCursor2(ref int iResult, ref string strErrMsg, string connectionString, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {

            IDbCommand cmd = dbF.CreateCommand();

            using (IDbConnection conn = dbF.CreateConnection())
            {
                //通过PrePareCommand方法将参数逐个加入到IDbCommand的参数集合中 
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                IDbDataAdapter sda = dbF.CreateDataAdapter();
                sda.InsertCommand = cmd;
                sda.DeleteCommand = cmd;
                sda.SelectCommand = cmd;
                sda.UpdateCommand = cmd;
                DataSet ds = new DataSet();
                sda.Fill(ds);
                //iResult = Int32.Parse(cmd.Parameters["bresult"].ToString());
                //strErrMsg = cmd.Parameters["ErrString"].ToString();

                //清空IDbCommand中的参数列表 
                cmd.Parameters.Clear();
                conn.Close();

                return ds; //注意:这里改了
            }
        }


        public  DataSet ExecuteDataSetForCursor(ref int iResult, ref string strErrMsg, string connectionString, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {

             IDbCommand cmd = dbF.CreateCommand();

             using (IDbConnection conn = dbF.CreateConnection())
            {
                //通过PrePareCommand方法将参数逐个加入到IDbCommand的参数集合中 
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                IDbDataAdapter sda = dbF.CreateDataAdapter();
                sda.InsertCommand = cmd;
                sda.DeleteCommand = cmd;
                sda.SelectCommand = cmd;
                sda.UpdateCommand = cmd;
                DataSet ds = new DataSet();
                sda.Fill(ds);
                iResult = Int32.Parse(cmd.Parameters["bresult"].ToString());
                strErrMsg = cmd.Parameters["ErrString"].ToString();

                //清空IDbCommand中的参数列表 
                cmd.Parameters.Clear();
                conn.Close();

                return ds; //注意:这里改了
            }
        }

        public  DataSet ExecuteDataSetForCursor(ref int iResult, ref string strErrMsg, ref string strErrVoucherNo, string connectionString, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {

             IDbCommand cmd = dbF.CreateCommand();

             using (IDbConnection conn = dbF.CreateConnection())
            {
                //通过PrePareCommand方法将参数逐个加入到IDbCommand的参数集合中 
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                IDbDataAdapter sda = dbF.CreateDataAdapter();
                sda.InsertCommand = cmd;
                sda.DeleteCommand = cmd;
                sda.SelectCommand = cmd;
                sda.UpdateCommand = cmd;
                DataSet ds = new DataSet();
                sda.Fill(ds);
                iResult = Int32.Parse(cmd.Parameters["bresult"].ToString());
                strErrMsg = cmd.Parameters["ErrString"].ToString();
                strErrVoucherNo = cmd.Parameters["strErrVoucherNo"].ToString();
                //清空IDbCommand中的参数列表 
                cmd.Parameters.Clear();
                conn.Close();

                return ds; //注意:这里改了
            }
        }
        public  DataSet ExecuteDataSetForCursor1(string connectionString, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {

             IDbCommand cmd = dbF.CreateCommand();

             using (IDbConnection conn = dbF.CreateConnection())
            {
                //通过PrePareCommand方法将参数逐个加入到IDbCommand的参数集合中 
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                IDbDataAdapter sda = dbF.CreateDataAdapter();
                sda.InsertCommand = cmd;
                sda.DeleteCommand = cmd;
                sda.SelectCommand = cmd;
                sda.UpdateCommand = cmd;
                DataSet ds = new DataSet();
                sda.Fill(ds);
                //iResult = string.IsNullOrEmpty(cmd.Parameters["IsResult"].Value.ToString()) ? 0 : Int32.Parse(cmd.Parameters["IsResult"].Value.ToString());
                //清空IDbCommand中的参数列表 
                cmd.Parameters.Clear();
                conn.Close();

                return ds; //注意:这里改了
            }
        }
        //Create By xp
        public  int ExecuteNonQueryList(List<string> cmdTexts)
        {
             IDbCommand cmd = dbF.CreateCommand();

          using(IDbConnection conn =dbF.CreateConnection())
            {

                //判断数据库连接状态
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;

                IDbTransaction trans = conn.BeginTransaction();

                //判断是否需要事物处理
                if (trans != null)
                    cmd.Transaction = trans;

                cmd.CommandType = System.Data.CommandType.Text;
                int val = -2;
                int line = 0;
                try
                {
                    if (cmdTexts != null)
                    {
                        foreach (string c in cmdTexts)
                        {
                            cmd.CommandText = c;
                            val = cmd.ExecuteNonQuery();
                            line++;
                        }
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    val = -2;
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    trans.Dispose();
                    conn.Close();
                }

                return val;

            }

        }

        public  bool ExecuteList(List<string> cmdTexts, ref List<string> sqs)
        {
             IDbCommand cmd = dbF.CreateCommand();

          using(IDbConnection conn =dbF.CreateConnection())
            {

                //判断数据库连接状态
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;

                IDbTransaction trans = conn.BeginTransaction();

                //判断是否需要事物处理
                if (trans != null)
                    cmd.Transaction = trans;

                cmd.CommandType = System.Data.CommandType.Text;
                try
                {
                    if (cmdTexts != null)
                    {
                        foreach (string c in cmdTexts)
                        {
                            cmd.CommandText = c;
                            sqs.Add(cmd.ExecuteScalar().ToString());
                        }
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    trans.Dispose();
                    conn.Close();
                }

                return true;

            }

        }


        public class imageButtfes
        {
            public byte[] imagePic1 { get; set; }
            public byte[] imagePic2 { get; set; }
            public byte[] imagePic3 { get; set; }
        }

        public  int ExecuteSqlTran(List<String> SQLStringList)
        {
            using (IDbConnection conn = dbF.CreateConnection())
            {
                conn.Open();
                //SqlCommand cmd = new SqlCommand();
                IDbCommand cmd = dbF.CreateCommand();
                cmd.Connection = conn;
                IDbTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    throw new Exception(ex.Message);
                    return 0;
                }
            }
        }


        //Create By xp
        public  int ExecuteNonQueryList(List<string> cmdTexts, ref string strError)
        {
             IDbCommand cmd = dbF.CreateCommand();

          using(IDbConnection conn =dbF.CreateConnection())
            {

                //判断数据库连接状态
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;

                IDbTransaction trans = conn.BeginTransaction();

                //判断是否需要事物处理
                if (trans != null)
                    cmd.Transaction = trans;

                cmd.CommandType = System.Data.CommandType.Text;
                int num = 0;
                int val = -2;
                try
                {
                    if (cmdTexts != null)
                    {
                        val = 0;
                        foreach (string c in cmdTexts)
                        {
                            num++;
                            cmd.CommandText = c;
                            val += cmd.ExecuteNonQuery();
                        }
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    val = -2;
                    strError = string.Format("执行第{0}条语句发生错误!{1}SQL:{2};{3}{4}", num, Environment.NewLine, cmdTexts[num - 1], Environment.NewLine, ex.Message);

                    trans.Rollback();
                    return val;
                }
                finally
                {
                    trans.Dispose();
                    conn.Close();
                }

                return val;

            }

        }

        public  int ExecuteNonQuery3(string connectionString, CommandType cmdType, string cmdText, List<DbParameter[]> cmdParmsList)
        {
             IDbCommand cmd = dbF.CreateCommand();

             using (IDbConnection conn = dbF.CreateConnection())
            {
                //通过PrePareCommand方法将参数逐个加入到IDbCommand的参数集合中
                //判断数据库连接状态
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                IDbTransaction trans = conn.BeginTransaction();
                //判断是否需要事物处理
                if (trans != null)
                    cmd.Transaction = trans;
                cmd.CommandType = cmdType;
                int val = -2;
                if (cmdParmsList != null)
                {
                    try
                    {
                        foreach (DbParameter[] item in cmdParmsList)
                        {
                            PreCommand(item, cmd);
                            val = cmd.ExecuteNonQuery();
                            //清空IDbCommand中的参数列表
                            cmd.Parameters.Clear();
                        }
                        //trans.Commit();

                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                    finally
                    {
                        trans.Dispose();
                        conn.Close();
                    }

                }
                return val;
            }

        }

        private  void PreCommand(DbParameter[] cmdParms, IDbCommand cmd)
        {
            foreach (DbParameter parm in cmdParms)
            {

                if (parm.Value == null)
                {
                    parm.Value = DBNull.Value;

                }

                //switch (parm.OracleType)
                //{
                //    case OracleType.DateTime:
                //        cmd.CommandText = cmd.CommandText.Replace(parm.ParameterName + " ", "TO_DATE('" + parm.Value.ToString() + "','yyyy-mm-dd hh24:mi:ss' )");
                //        break;
                //    case OracleType.VarChar:
                //        cmd.CommandText = cmd.CommandText.Replace(parm.ParameterName + " ", "'" + parm.Value + "'");
                //        break;
                //    case OracleType.NVarChar:
                //        cmd.CommandText = cmd.CommandText.Replace(parm.ParameterName + " ", "'" + parm.Value + "'");
                //        break;
                //    default:
                //        cmd.CommandText = cmd.CommandText.Replace(parm.ParameterName + " ", parm.Value.ToString());
                //        break;
                //}
            }
        }


        public  Array GetOracleArray(string OracleList, ArrayList ObjList)
        {
            Array list = null;
            if (ObjList == null || ObjList.Count <= 0)
            { }
            else
            {

            }

            return list;
        }


        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public  int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (IDbConnection connection = dbF.CreateConnection())
            {
                int result;
                connection.Open();
                IDbCommand command = BuildIntCommand(connection, storedProcName, parameters);
                command.CommandTimeout = 50000;
                //Log.WriteLog(command.CommandText);
                rowsAffected = command.ExecuteNonQuery();
                //result = Convert.ToInt32(((DbParameter)command.Parameters[0]).Value.ToString());
                result = Convert.ToInt32(((DbParameter)command.Parameters[0]).Value);
                //Connection.Close();
                return result;
            }
        }

        /// <summary>
        /// 创建 SqlCommand 对象实例(用来返回一个整数值)	
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand 对象实例</returns>
        private  IDbCommand BuildIntCommand(IDbConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            IDbCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            //command.Parameters.Add(new DbParameter("ReturnValue",
            //    OracleDbType.Int32, 4, ParameterDirection.ReturnValue,
            //    false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }

        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private  IDbCommand BuildQueryCommand(IDbConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            IDbCommand command = dbF.CreateCommand();
            command.CommandText = storedProcName;
            command.Connection = connection;
            //IDbCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (DbParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }


        /// <summary>
        /// 批量执行存储过程，存储多条数据		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public  bool RunProcedures(string storedProcName, List<IDataParameter[]> ParameterList)
        {
            using (IDbConnection connection = dbF.CreateConnection())
            {
                int result;
                string strError = string.Empty;
                connection.Open();
                using (IDbTransaction trans = connection.BeginTransaction())
                {
                    try
                    {
                         IDbCommand cmd = dbF.CreateCommand();
                        foreach (DbParameter[] parameters in ParameterList)
                        {
                            PrepareCommandEx(cmd, connection, trans, storedProcName, parameters);
                            cmd.ExecuteNonQuery();
                            result = (int)cmd.Parameters[0];
                            if (result == -1)
                            {
                                trans.Rollback();
                                strError = (string)cmd.Parameters[1];
                                throw new Exception(strError);
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (string.IsNullOrEmpty(strError))
                        {
                            trans.Rollback();
                        }

                        throw new Exception(ex.Message);
                    }

                }

                return true;
            }
        }


        private  void PrepareCommandEx(IDbCommand cmd,IDbConnection conn, IDbTransaction trans, string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;//cmdType;
            if (cmdParms != null)
            {


                foreach (DbParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        public  object ToDBValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }


        public  bool RunSqls(Dictionary<string, DbParameter[]> ParameterList)
        {
            using (IDbConnection connection = dbF.CreateConnection() )
            {
                int result;
                string strError = string.Empty;
                connection.Open();
                using (IDbTransaction trans = connection.BeginTransaction())
                {
                    try
                    {
                         IDbCommand cmd = dbF.CreateCommand();
                        foreach (KeyValuePair<string, DbParameter[]> item in ParameterList)
                        {
                            PrepareCommandEx1(cmd, connection, trans, item.Key, item.Value);

                            result = cmd.ExecuteNonQuery();

                            if (result == -1)
                            {
                                trans.Rollback();
                                strError = (string)cmd.Parameters[1];
                                throw new Exception(strError);
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (string.IsNullOrEmpty(strError))
                        {
                            trans.Rollback();
                        }

                        throw new Exception(ex.Message);
                    }

                }

                return true;
            }
        }


        private  void PrepareCommandEx1(IDbCommand cmd, IDbConnection conn, IDbTransaction trans, string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {


                foreach (DbParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }



        public  object ToModelValue(IDataReader reader, string columnName)
        {
            //if (!Common_Func.readerExists(reader, columnName)) return null;
            try
            {

                if (reader.IsDBNull(reader.GetOrdinal(columnName)))
                {
                    return null;
                }
                else
                {

                    return reader[columnName];
                }
            }
            catch
            {
                throw new Exception("视图中缺少" + columnName + "字段!");
            }
        }


        #endregion


    }
}
