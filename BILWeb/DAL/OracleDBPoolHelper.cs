using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;

using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;

/// <summary>
/// 数据库的通用访问代码
/// 此类为抽象类，不允许实例化，在应用时直接调用即可
/// </summary>
public abstract class OracleDBPoolHelper
{

    //获取数据库连接字符串，其属于静态变量且只读，项目中所有文档可以直接使用，但不能修改
    public static readonly string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["ConnOracleWithAddress"].ConnectionString;


    public static DBConnectionSingletion pool = DBConnectionSingletion.Instance;

    public static OracleTransaction GetTransaction()
    {
        return pool.BorrowDBConnection().BeginTransaction();
    }

    public static void CommitTransaction(OracleTransaction trans)
    {
        trans.Commit();
        pool.ReturnDBConnection(trans.Connection);
    }
    public static void RollbackTransaction(OracleTransaction trans)
    {
        trans.Rollback();
        pool.ReturnDBConnection(trans.Connection);
    }




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
        OracleConnection conn = pool.BorrowDBConnection();
        //通过PrePareCommand方法将参数逐个加入到OracleCommand的参数集合中
        PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
        int val = cmd.ExecuteNonQuery();
        //清空OracleCommand中的参数列表
        cmd.Parameters.Clear();
        pool.ReturnDBConnection(conn);
        return val;
    }
    public static int ExecuteNonQuery2(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
    {

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
    ///  IDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="commandType">OracleCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
    /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
    /// <param name="commandParameters">以数组形式提供OracleCommand命令中用到的参数列表</param>
    /// <returns>返回一个包含结果的IDataReader</returns>
    public static IDataReader ExecuteReader(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
    {
        OracleCommand cmd = new OracleCommand();
        OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction);

        // 在这里使用try/catch处理是因为如果方法出现异常，则IDataReader就不存在，
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
        OracleConnection connection = pool.BorrowDBConnection();

        PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
        object val = cmd.ExecuteScalar();
        cmd.Parameters.Clear();
        pool.ReturnDBConnection(connection);
        return val;

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
        //YLBIL.Common.Logs.WriteLog(cmdText, "PrepareCommand_pool");
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

                //switch (parm.OracleType)
                //{


                //    case OracleType.DateTime:
                //        cmd.CommandText = cmd.CommandText.Replace(parm.ParameterName + " ", "TO_DATE('" + parm.Value.ToString() + "','yyyy-mm-dd hh24:mi:ss' )");
                //        break;
                //    case OracleType.VarChar:
                //        cmd.CommandText = cmd.CommandText.Replace(parm.ParameterName + " ", "'" + parm.Value + "'");
                //        break;
                //    default:
                //        cmd.CommandText = cmd.CommandText.Replace(parm.ParameterName + " ", parm.Value.ToString());
                //        break;
                //}
                // cmd.Parameters.Add(parm);
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
        OracleConnection conn = pool.BorrowDBConnection();

        //通过PrePareCommand方法将参数逐个加入到OracleCommand的参数集合中 
        PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
        OracleDataAdapter sda = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        //清空OracleCommand中的参数列表 
        cmd.Parameters.Clear();
        pool.ReturnDBConnection(conn);
        return ds; //注意:这里改了

    }
    public static DataSet ExecuteDataSet(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
    {

        OracleCommand cmd = new OracleCommand();

        using (OracleConnection conn = new OracleConnection(connectionString))
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

    //Create By xp
    public static int ExecuteNonQueryList(List<string> cmdTexts)
    {
        OracleCommand cmd = new OracleCommand();
        OracleConnection conn = pool.BorrowDBConnection();
        cmd.Connection = conn;

        OracleTransaction trans = conn.BeginTransaction();

        //判断是否需要事物处理
        if (trans != null)
            cmd.Transaction = trans;

        cmd.CommandType = System.Data.CommandType.Text;
        int val = -2;
        try
        {
            if (cmdTexts != null)
            {
                foreach (string c in cmdTexts)
                {
                    cmd.CommandText = c;
                    val = cmd.ExecuteNonQuery();
                }
            }
            trans.Commit();
        }
        catch (Exception)
        {
            trans.Rollback();
            throw;
        }
        finally
        {
            trans.Dispose();
            pool.ReturnDBConnection(conn);
        }

        return val;



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




}
