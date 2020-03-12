using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;

namespace BILBasic.DBA
{
    /// <summary>
    /// 数据库的通用访问代码
    /// 此类为抽象类，不允许实例化，在应用时直接调用即可
    /// </summary>
    public abstract class OracleDBHelper
    {

        //获取数据库连接字符串，其属于静态变量且只读，项目中所有文档可以直接使用，但不能修改
        public static readonly string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["ConnOracleWithAddress"] == null ? "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = 10.1.254.71)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME =  masawms)));Persist Security Info=True;User ID=mzwmsbarcode;Password=123456;" : ConfigurationManager.ConnectionStrings["ConnOracleWithAddress"].ConnectionString;//防止空引用异常 modify by gzw 181227
        //public static readonly string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["ConnOracle"].ConnectionString;
        //public static readonly string ConnectionStringLocalTransaction = "Password=123456;User ID=jxbarcode;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=jingxinwms)));";
        // 哈希表用来存储缓存的参数信息，哈希表可以存储任意类型的参数。
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        ///执行一个不需要返回值的OracleCommand命令，通过指定专用的连接字符串。
        /// 使用参数数组形式提供参数列表 
        /// </summary>
        /// <remarks>
        /// 使用示例：
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">OracleCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供OracleCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此OracleCommand命令执行后影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
                connectionString = ConnectionStringLocalTransaction;

            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                //通过PrePareCommand方法将参数逐个加入到OracleCommand的参数集合中
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();

                //清空OracleCommand中的参数列表
                cmd.Parameters.Clear();
                conn.Close();
                return val;
            }
        }

        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction))
            {
                //通过PrePareCommand方法将参数逐个加入到OracleCommand的参数集合中
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();

                //清空OracleCommand中的参数列表
                cmd.Parameters.Clear();
                conn.Close();
                return val;
            }
        }
        public static int ExecuteNonQuery2(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
                connectionString = ConnectionStringLocalTransaction;

            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                //通过PrePareCommand方法将参数逐个加入到OracleCommand的参数集合中
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();

                //清空OracleCommand中的参数列表
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
        public static int ExecuteNonQuery3(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {

            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                cmd.CommandTimeout = 25;
                //通过PrePareCommand方法将参数逐个加入到OracleCommand的参数集合中
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();

                //清空OracleCommand中的参数列表
                cmd.Parameters.Clear();
                conn.Close();
                return val;
            }
        }

        public static int ExecuteNonQuery3(ref string strTaskNo, string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {

            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                //通过PrePareCommand方法将参数逐个加入到OracleCommand的参数集合中
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                strTaskNo = ((Oracle.ManagedDataAccess.Types.OracleClob)cmd.Parameters["strTaskNo"].Value).Value.ToString();
                //清空OracleCommand中的参数列表
                cmd.Parameters.Clear();
                conn.Close();
                return val;
            }
        }

        public static int ExecuteNonQuery2(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {

            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction))
            {
                //通过PrePareCommand方法将参数逐个加入到OracleCommand的参数集合中
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();

                //清空OracleCommand中的参数列表
                cmd.Parameters.Clear();
                conn.Close();
                return val;
            }
        }

        /// <summary>
        ///执行一条不返回结果的OracleCommand，通过一个已经存在的数据库连接 
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">一个现有的数据库连接</param>
        /// <param name="commandType">OracleCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供OracleCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此OracleCommand命令执行后影响的行数</returns>
        public static int ExecuteNonQuery(OracleConnection connection, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {

            OracleCommand cmd = new OracleCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            connection.Close();
            return val;
        }



        /// <summary>
        /// 执行一条不返回结果的OracleCommand，通过一个已经存在的数据库事物处理 
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例： 
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">一个存在的 sql 事物处理</param>
        /// <param name="commandType">OracleCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供OracleCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此OracleCommand命令执行后影响的行数</returns>
        public static int ExecuteNonQuery(OracleTransaction trans, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }



        /// <summary>
        /// 执行一条返回结果集的OracleCommand命令，通过专用的连接字符串。
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  OracleDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">OracleCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供OracleCommand命令中用到的参数列表</param>
        /// <returns>返回一个包含结果的OracleDataReader</returns>
        public static OracleDataReader ExecuteReader(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction);

            // 在这里使用try/catch处理是因为如果方法出现异常，则OracleDataReader就不存在，
            //CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            //关闭数据库连接，并通过throw再次引发捕捉到的异常。
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                OracleDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
        public static OracleDataReader ExecuteReader(string strSQL)
        {
            OracleConnection connection = new OracleConnection(ConnectionStringLocalTransaction);
            OracleCommand cmd = new OracleCommand(strSQL, connection);
            cmd.CommandTimeout = 200;
            //Log.WriteLog(cmd.CommandText);
            try
            {
                connection.Open();
                OracleDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
        public static OracleDataReader ExecuteReader(string strSQL,string connectString)
        {
            if (string.IsNullOrEmpty(connectString))
                connectString = ConnectionStringLocalTransaction;
            OracleConnection connection = new OracleConnection(connectString);
            OracleCommand cmd = new OracleCommand(strSQL, connection);
            cmd.CommandTimeout = 200;
            //Log.WriteLog(cmd.CommandText);
            try
            {
                connection.Open();
                OracleDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw e;
            }

        }

        /// <summary>
        /// 执行一条返回结果集的OracleCommand命令，通过专用的连接字符串。
        /// 使用参数数组提供参数,通过out sys_refcursor参数返回结果集
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  OracleDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">OracleCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供OracleCommand命令中用到的参数列表</param>
        /// <returns>返回一个包含结果的OracleDataReader</returns>
        public static OracleDataReader ExecuteReaderForCursor(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction);

            // 在这里使用try/catch处理是因为如果方法出现异常，则OracleDataReader就不存在，
            //CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            //关闭数据库连接，并通过throw再次引发捕捉到的异常。
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                OracleDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
        /// 执行一条返回第一条记录第一列的OracleCommand命令，通过专用的连接字符串。 
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">OracleCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供OracleCommand命令中用到的参数列表</param>
        /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection connection = new OracleConnection(ConnectionStringLocalTransaction))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static object ExecuteScalar(string connectString,CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectString))
                connectString = ConnectionStringLocalTransaction;
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection connection = new OracleConnection(connectString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }


        //CREATE BY XUP 特殊情况
        public static object ExecuteScalarTB(OracleConnection conn, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
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
        /// 执行一条返回第一条记录第一列的OracleCommand命令，通过已经存在的数据库连接。
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例： 
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">一个已经存在的数据库连接</param>
        /// <param name="commandType">OracleCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供OracleCommand命令中用到的参数列表</param>
        /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
        public static object ExecuteScalar(OracleConnection connection, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {

            OracleCommand cmd = new OracleCommand();

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
        public static void CacheParameters(string cacheKey, params OracleParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// 获取被缓存的参数
        /// </summary>
        /// <param name="cacheKey">用于查找参数的KEY值</param>
        /// <returns>返回缓存的参数数组</returns>
        public static OracleParameter[] GetCachedParameters(string cacheKey)
        {
            OracleParameter[] cachedParms = (OracleParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            //新建一个参数的克隆列表
            OracleParameter[] clonedParms = new OracleParameter[cachedParms.Length];

            //通过循环为克隆参数列表赋值
            for (int i = 0, j = cachedParms.Length; i < j; i++)
                //使用clone方法复制参数列表中的参数
                clonedParms[i] = (OracleParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// 为执行命令准备参数
        /// </summary>
        /// <param name="cmd">OracleCommand 命令</param>
        /// <param name="conn">已经存在的数据库连接</param>
        /// <param name="trans">数据库事物处理</param>
        /// <param name="cmdType">OracleCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="cmdText">Command text，T-SQL语句 例如 Select * from Products</param>
        /// <param name="cmdParms">返回带参数的命令</param>
        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter[] cmdParms)
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
                foreach (OracleParameter parm in cmdParms)
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

        private static void PrepareCommand2(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter[] cmdParms)
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
                foreach (OracleParameter parm in cmdParms)
                {

                    if (parm.Value == null)
                    {
                        parm.Value = DBNull.Value;

                    }


                    cmd.Parameters.Add(parm);
                }
            }
        }

        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {

            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction))
            {
                //通过PrePareCommand方法将参数逐个加入到OracleCommand的参数集合中 
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                OracleDataAdapter sda = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                //清空OracleCommand中的参数列表 
                cmd.Parameters.Clear();
                conn.Close();
                return ds; //注意:这里改了
            }
        }
        public static DataSet ExecuteDataSet(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {

            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                //通过PrePareCommand方法将参数逐个加入到OracleCommand的参数集合中 
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                OracleDataAdapter sda = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                //清空OracleCommand中的参数列表 
                cmd.Parameters.Clear();
                conn.Close();

                return ds; //注意:这里改了
            }
        }

        public static DataSet ExecuteDataSetForCursor(ref int iResult, ref string strErrMsg, string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {

            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                //通过PrePareCommand方法将参数逐个加入到OracleCommand的参数集合中 
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                OracleDataAdapter sda = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                iResult = Int32.Parse(cmd.Parameters["bresult"].Value.ToString());
                strErrMsg = cmd.Parameters["ErrString"].Value.ToString();

                //清空OracleCommand中的参数列表 
                cmd.Parameters.Clear();
                conn.Close();

                return ds; //注意:这里改了
            }
        }

        public static DataSet ExecuteDataSetForCursor(ref int iResult, ref string strErrMsg, ref string strErrVoucherNo, string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {

            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                //通过PrePareCommand方法将参数逐个加入到OracleCommand的参数集合中 
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                OracleDataAdapter sda = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                iResult = Int32.Parse(cmd.Parameters["bresult"].Value.ToString());
                strErrMsg = cmd.Parameters["ErrString"].Value.ToString();
                strErrVoucherNo = cmd.Parameters["strErrVoucherNo"].Value.ToString();
                //清空OracleCommand中的参数列表 
                cmd.Parameters.Clear();
                conn.Close();

                return ds; //注意:这里改了
            }
        }
        public static DataSet ExecuteDataSetForCursor1(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {

            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                //通过PrePareCommand方法将参数逐个加入到OracleCommand的参数集合中 
                PrepareCommand2(cmd, conn, null, cmdType, cmdText, commandParameters);
                OracleDataAdapter sda = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                //iResult = string.IsNullOrEmpty(cmd.Parameters["IsResult"].Value.ToString()) ? 0 : Int32.Parse(cmd.Parameters["IsResult"].Value.ToString());
                //清空OracleCommand中的参数列表 
                cmd.Parameters.Clear();
                conn.Close();

                return ds; //注意:这里改了
            }
        }
        //Create By xp
        public static int ExecuteNonQueryList(List<string> cmdTexts)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction))
            {

                //判断数据库连接状态
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;

                OracleTransaction trans = conn.BeginTransaction();

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

        public static bool ExecuteList(List<string> cmdTexts,ref List<string> sqs)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction))
            {

                //判断数据库连接状态
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;

                OracleTransaction trans = conn.BeginTransaction();

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

        public static int ExecuteNonQueryListByCymImages3(List<string> cmdTexts, List<imageButtfes> buffers, ref string strError)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction))
            {
                //判断数据库连接状态
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;

                OracleTransaction trans = conn.BeginTransaction();

                //判断是否需要事物处理
                if (trans != null)
                    cmd.Transaction = trans;

                cmd.CommandType = System.Data.CommandType.Text;
                int num = 0;
                int val = -2;

                int number = 0;

                try
                {
                    if (cmdTexts != null)
                    {
                        val = 0;

                        cmd.Parameters.Add("Images", OracleDbType.Blob);
                        cmd.Parameters.Add("Images2", OracleDbType.Blob);
                        cmd.Parameters.Add("Images3", OracleDbType.Blob);

                        foreach (string c in cmdTexts)
                        {
                            cmd.CommandText = c;

                            if (c.Contains("Images"))
                            {
                                cmd.Parameters[0].Value = buffers[number].imagePic1;
                                cmd.Parameters[1].Value = buffers[number].imagePic2;
                                cmd.Parameters[2].Value = buffers[number].imagePic3;
                                number++;
                            }

                            val += cmd.ExecuteNonQuery();

                            num++;
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

        public static int ExecuteNonQueryListByCymNewImages3(List<string> cmdTexts, List<imageButtfes> buffers, ref string strError)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction))
            {
                //判断数据库连接状态
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;

                OracleTransaction trans = conn.BeginTransaction();

                //判断是否需要事物处理
                if (trans != null)
                    cmd.Transaction = trans;

                cmd.CommandType = System.Data.CommandType.Text;
                int num = 0;
                int val = -2;

                int number = 0;

                try
                {
                    if (cmdTexts != null)
                    {
                        val = 0;

                        cmd.Parameters.Add("imaBuffers", OracleDbType.Blob);
                        cmd.Parameters.Add("imaBuffers2", OracleDbType.Blob);
                        cmd.Parameters.Add("imaBuffers3", OracleDbType.Blob);

                        foreach (string c in cmdTexts)
                        {
                            cmd.CommandText = c;

                            if (c.Contains("imaBuffers"))
                            {
                                cmd.Parameters[0].Value = buffers[number].imagePic1;
                                cmd.Parameters[1].Value = buffers[number].imagePic2;
                                cmd.Parameters[2].Value = buffers[number].imagePic3;
                                number++;
                            }

                            val += cmd.ExecuteNonQuery();

                            num++;
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





        public static int ExecuteNonQueryListByCymNew(List<string> cmdTexts, List<byte[]> buffers, ref string strError)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction))
            {
                //判断数据库连接状态
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;

                OracleTransaction trans = conn.BeginTransaction();

                //判断是否需要事物处理
                if (trans != null)
                    cmd.Transaction = trans;

                cmd.CommandType = System.Data.CommandType.Text;
                int num = 0;
                int val = -2;

                int number = 0;

                try
                {
                    if (cmdTexts != null)
                    {
                        val = 0;

                        cmd.Parameters.Add("Images", OracleDbType.Blob);

                        foreach (string c in cmdTexts)
                        {
                            cmd.CommandText = c;

                            if (c.Contains("Images"))
                            {
                                cmd.Parameters[0].Value = buffers[number];
                                number++;
                            }

                            val += cmd.ExecuteNonQuery();

                            num++;
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

        public static int ExecuteNonQueryListByCymNew2(List<string> cmdTexts, List<byte[]> buffers, ref string strError)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction))
            {
                //判断数据库连接状态
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;

                OracleTransaction trans = conn.BeginTransaction();

                //判断是否需要事物处理
                if (trans != null)
                    cmd.Transaction = trans;

                cmd.CommandType = System.Data.CommandType.Text;
                int num = 0;
                int val = -2;

                int number = 0;

                try
                {
                    if (cmdTexts != null)
                    {
                        val = 0;

                        cmd.Parameters.Add("imaBuffers", OracleDbType.Blob);

                        foreach (string c in cmdTexts)
                        {
                            cmd.CommandText = c;

                            if (c.Contains("imaBuffers"))
                            {
                                cmd.Parameters[0].Value = buffers[number];
                                number++;
                            }

                            val += cmd.ExecuteNonQuery();

                            num++;
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

        public static int ExecuteNonQueryListByCym(string cmdTexts, byte[] buffer, ref string strError)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction))
            {
                //判断数据库连接状态
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;

                OracleTransaction trans = conn.BeginTransaction();

                //判断是否需要事物处理
                if (trans != null)
                    cmd.Transaction = trans;

                cmd.CommandType = System.Data.CommandType.Text;
                int num = 0;
                int val = -2;

                try
                {
                    if (!string.IsNullOrWhiteSpace(cmdTexts))
                    {
                        val = 0;

                        cmd.CommandText = cmdTexts;
                        cmd.Parameters.Add("Images", OracleDbType.Blob);
                        cmd.Parameters[0].Value = buffer;

                        val = cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    val = -2;
                    strError = string.Format("执行第{0}条语句发生错误!{1}SQL:{2};{3}{4}", num, Environment.NewLine, cmdTexts, Environment.NewLine, ex.Message);

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


        //Create By xp
        public static int ExecuteNonQueryList(List<string> cmdTexts, ref string strError)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction))
            {

                //判断数据库连接状态
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;

                OracleTransaction trans = conn.BeginTransaction();

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

        public static int ExecuteNonQuery3(string connectionString, CommandType cmdType, string cmdText, List<OracleParameter[]> cmdParmsList)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                //通过PrePareCommand方法将参数逐个加入到OracleCommand的参数集合中
                //判断数据库连接状态
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                OracleTransaction trans = conn.BeginTransaction();
                //判断是否需要事物处理
                if (trans != null)
                    cmd.Transaction = trans;
                cmd.CommandType = cmdType;
                int val = -2;
                if (cmdParmsList != null)
                {
                    try
                    {
                        foreach (OracleParameter[] item in cmdParmsList)
                        {
                            PreCommand(item, cmd);
                            val = cmd.ExecuteNonQuery();
                            //清空OracleCommand中的参数列表
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

        private static void PreCommand(OracleParameter[] cmdParms, OracleCommand cmd)
        {
            foreach (OracleParameter parm in cmdParms)
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


        public static Array GetOracleArray(string OracleList, ArrayList ObjList)
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
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (OracleConnection connection = new OracleConnection(ConnectionStringLocalTransaction))
            {
                int result;
                connection.Open();
                OracleCommand command = BuildIntCommand(connection, storedProcName, parameters);
                command.CommandTimeout = 50000;
                //Log.WriteLog(command.CommandText);
                rowsAffected = command.ExecuteNonQuery();
                result = Convert.ToInt32(command.Parameters[0].Value.ToString());
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
        private static OracleCommand BuildIntCommand(OracleConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            OracleCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            //command.Parameters.Add(new OracleParameter("ReturnValue",
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
        private static OracleCommand BuildQueryCommand(OracleConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            OracleCommand command = new OracleCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (OracleParameter parameter in parameters)
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
        public static bool RunProcedures(string storedProcName, List<OracleParameter[]> ParameterList)
        {
            using (OracleConnection connection = new OracleConnection(ConnectionStringLocalTransaction))
            {
                int result;
                string strError = string.Empty;
                connection.Open();
                using (OracleTransaction trans = connection.BeginTransaction())
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        foreach (OracleParameter[] parameters in ParameterList)
                        {
                            PrepareCommandEx(cmd, connection, trans, storedProcName, parameters);
                            cmd.ExecuteNonQuery();
                            result = (int)cmd.Parameters[0].Value;
                            if (result == -1)
                            {
                                trans.Rollback();
                                strError = (string)cmd.Parameters[1].Value;
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


        private static void PrepareCommandEx(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, string cmdText, OracleParameter[] cmdParms)
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


                foreach (OracleParameter parameter in cmdParms)
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

        public static object ToDBValue(object value)
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


        public static bool RunSqls(Dictionary<string, OracleParameter[]> ParameterList)
        {
            using (OracleConnection connection = new OracleConnection(ConnectionStringLocalTransaction))
            {
                int result;
                string strError = string.Empty;
                connection.Open();
                using (OracleTransaction trans = connection.BeginTransaction())
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        foreach (KeyValuePair<string, OracleParameter[]> item in ParameterList)
                        {
                            PrepareCommandEx1(cmd, connection, trans, item.Key, item.Value);

                            result = cmd.ExecuteNonQuery();

                            if (result == -1)
                            {
                                trans.Rollback();
                                strError = (string)cmd.Parameters[1].Value;
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


        private static void PrepareCommandEx1(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, string cmdText, OracleParameter[] cmdParms)
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


                foreach (OracleParameter parameter in cmdParms)
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



        public static object ToModelValue(OracleDataReader reader, string columnName)
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

    }
}

