/*************************************
/* copyright (c) 2012 daniel dong
 * 
 * author：daniel dong
 * blog：  www.cnblogs.com/danielwise
 * email： guofoo@163.com
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;


public sealed class DBConnectionSingletion : ObjectPool
{
    private DBConnectionSingletion() { }

    public static readonly DBConnectionSingletion Instance =
        new DBConnectionSingletion();

    private static string connectionString =
        ConfigurationManager.ConnectionStrings["ConnOracleWithAddress"].ConnectionString;

    public static string ConnectionString
    {
        get
        {
            return connectionString;
        }
        set
        {
            connectionString = value;
        }
    }

    protected override object Create()
    {
        OracleConnection conn = new OracleConnection(connectionString);
        conn.Open();
        return conn;
    }

    protected override bool Validate(object o)
    {
        try
        {
            OracleConnection conn = (OracleConnection)o;
            return !conn.State.Equals(ConnectionState.Closed);
        }
        catch (OracleException)
        {
            return false;
        }
    }

    protected override void Expire(object o)
    {
        try
        {
            OracleConnection conn = (OracleConnection)o;
            conn.Close();
        }
        catch (OracleException) { }
    }

    public OracleConnection BorrowDBConnection()
    {
        try
        {
            return (OracleConnection)base.GetObjectFromPool();
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public void ReturnDBConnection(OracleConnection conn)
    {
        base.ReturnObjectToPool(conn);
    }
}
