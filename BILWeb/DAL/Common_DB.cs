using BILBasic.Basing.Factory;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;


namespace BILWeb.DAL
{

    [System.AttributeUsage(System.AttributeTargets.Property,
                       AllowMultiple = true)
    ]
    public class DBAttribute : System.Attribute
    {
        public bool NotDBField { get; set; }
    }



    [System.AttributeUsage(System.AttributeTargets.Property,
                       AllowMultiple = true)
    ]
    public class SAPAttribute : System.Attribute
    {
        public string SAPField { get; set; }
    }

    [System.AttributeUsage(System.AttributeTargets.Property,
                       AllowMultiple = true)
    ]
    public class QueryAttribute : System.Attribute
    {
        public string OP { get; set; }
        public string Dbname { get; set; }
    }


    public static class DBMothed
    {
        public static int ExeNullQuery(this string sql, OracleTransaction trans = null)
        {
            if (trans != null)
            {
                return OracleDBPoolHelper.ExecuteNonQuery(trans, System.Data.CommandType.Text, sql, null);

            }

            return OracleDBPoolHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, null);
        }



        public static int Save(this DBModel dbmodel, string tablename, string seq,OracleTransaction trans = null)
        {
            string sql = Common_DB2.GetInertSqlCache(dbmodel, tablename,seq);
             BILBasic.Basing.DbFactory dbFactory = new BILBasic.Basing.DbFactory(BILBasic.Basing.DbFactory.DbFactoryType.SQLSERVER);
            if (trans != null)
            {
                return dbFactory.ExecuteNonQuery(trans, System.Data.CommandType.Text, sql, null);
                //return OracleDBPoolHelper.ExecuteNonQuery(trans, System.Data.CommandType.Text, sql, null);

            }
            return dbFactory.ExecuteNonQuery(System.Data.CommandType.Text, sql, null);
            //return OracleDBPoolHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, null);
        }
        public static int Save(this IEnumerable<DBModel> list, string tablename,string seq, OracleTransaction trans = null)
        {
            int rows = 0;
            foreach (var item in list)
            {
                rows += item.Save(tablename,seq, trans);
            }
            return rows;
        }


        public static int UpdateSql(this DBModel dbmodel, string sql, OracleTransaction trans = null)
        {
            if (trans != null)
            {
                return OracleDBPoolHelper.ExecuteNonQuery(trans, System.Data.CommandType.Text, sql, null);

            }

            return OracleDBPoolHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, null);
        }



        public static int Update(this DBModel dbmodel, string tablename, string keyname, OracleTransaction trans = null)
        {
            string sql = Common_DB2.GetUpdateSqlCache(dbmodel, tablename, keyname);

            if (trans != null)
            {
                return OracleDBPoolHelper.ExecuteNonQuery(trans, System.Data.CommandType.Text, sql, null);

            }

            return OracleDBPoolHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, null);
        }

        public static int UpdateEx(this DBModel dbmodel, string tablename, string keyname, string exnames, OracleTransaction trans = null)
        {
            string sql = Common_DB2.GetUpdateSqlCache(dbmodel, tablename, keyname, exnames);

            if (trans != null)
            {
                return OracleDBPoolHelper.ExecuteNonQuery(trans, System.Data.CommandType.Text, sql, null);

            }

            return OracleDBPoolHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, null);
        }

        public static int UpdateIn(this DBModel dbmodel, string tablename, string keyname, string innames, OracleTransaction trans = null)
        {
            string sql = Common_DB2.GetUpdateSqlInCache(dbmodel, tablename, keyname, innames);

            if (trans != null)
            {
                return OracleDBPoolHelper.ExecuteNonQuery(trans, System.Data.CommandType.Text, sql, null);

            }

            return OracleDBPoolHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, null);
        }

        public static int UpdateIn(this IEnumerable<DBModel> list, string tablename, string keyname, string innames, OracleTransaction trans = null)
        {
            int rows = 0;
            foreach (var item in list)
            {
                rows += item.UpdateIn(tablename, keyname, innames, trans);
            }
            return rows;
        }




    }


    public static class Common_DB2
    {

        public static bool NotDBField(PropertyInfo info)
        {
            System.Attribute[] at = System.Attribute.GetCustomAttributes(info);
            if (at.Length == 0) return false;
            foreach (var item in at)
            {
                if (item is DBAttribute)
                    return ((DBAttribute)item).NotDBField;
            }
            return false;
        }


        public volatile static Dictionary<string, StringBuilder> cacheupdatesqls = new Dictionary<string, StringBuilder>();

        public static string GetUpdateSqlCache<T>(T t, string tablename, string keyname)
        {
            PropertyInfo[] propertys = t.GetType().GetProperties();
            StringBuilder sb = new StringBuilder();
            tablename = tablename.ToLower();
            keyname = keyname.ToLower();
            lock ("object")
            {
                if (!cacheupdatesqls.ContainsKey(tablename + keyname))
                {
                    bool havewhere = false;

                    sb.Append("　Update  ");
                    sb.Append(tablename);
                    sb.Append("  set  ");

                    string wheresql = "";
                    int i = 0;
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(t, null) == null ? "" : pi.GetValue(t, null);

                        if (NotDBField(pi))
                            continue;

                        if (pi.Name.ToLower() == keyname.ToLower())
                        {
                            havewhere = true;
                            //  wheresql = string.Format(wheresql, pi.Name, pi.GetValue(t, null));
                            wheresql = " where  " + pi.Name + "=" + "'{" + i.ToString() + "}'";

                        }
                        else
                        {
                            switch (obj.GetType().Name.ToLower())
                            {
                                case "int32":
                                    sb.Append(pi.Name);
                                    sb.Append("=");
                                    //sb.Append(pi.GetValue(t, null).ToString());
                                    sb.Append("{" + i.ToString() + "}");
                                    sb.Append(",");
                                    break;

                                case "decimal":
                                    sb.Append(pi.Name);
                                    sb.Append("=");
                                    //sb.Append(pi.GetValue(t, null).ToString());
                                    sb.Append("{" + i.ToString() + "}");
                                    sb.Append(",");
                                    break;

                                case "datetime":
                                    sb.Append(pi.Name);
                                    sb.Append("=");
                                    sb.Append(" TO_DATE('");
                                    //   sb.Append(pi.GetValue(t, null).ToString());
                                    sb.Append("{" + i.ToString() + "}");
                                    sb.Append("','yyyy/mm/dd hh24:mi:ss')");
                                    sb.Append(",");
                                    break;

                                default:
                                    sb.Append(pi.Name);
                                    sb.Append("=");
                                    sb.Append("'");
                                    //    sb.Append(pi.GetValue(t, null) == null ? "" : pi.GetValue(t, null).ToString());
                                    sb.Append("{" + i.ToString() + "}");
                                    sb.Append("'");
                                    sb.Append(",");
                                    break;
                            }
                        }
                        i++;

                    }
                    sb.Remove(sb.Length - 1, 1);

                    if (havewhere)
                    {
                        sb.Append(wheresql);
                    }
                    else
                    {
                        sb.Clear();
                    }
                    cacheupdatesqls[tablename + keyname] = sb;

                }
                else
                {
                    sb = cacheupdatesqls[tablename + keyname];
                }
            }

            if (sb.ToString() == "")
            {
                return "";
            }

            List<object> olist = new List<object>();
            foreach (PropertyInfo pi in propertys)
            {

                if (NotDBField(pi))
                    continue;
                string obj = pi.GetValue(t, null) == null ? "" : pi.GetValue(t, null).ToString();
                obj = obj.Replace("\'", "");
                olist.Add(obj);


            }
            return string.Format(sb.ToString(), olist.ToArray());



        }

        public static string GetUpdateSqlCache<T>(T t, string tablename, string keyname, string exnames)
        {
            PropertyInfo[] propertys = t.GetType().GetProperties();
            StringBuilder sb = new StringBuilder();
            tablename = tablename.ToLower();
            keyname = keyname.ToLower();
            exnames = exnames.ToLower();
            var exnamearray = exnames.Split('|');

            lock ("object")
            {
                if (!cacheupdatesqls.ContainsKey(tablename + keyname + exnames))
                {
                    bool havewhere = false;

                    sb.Append("　Update  ");
                    sb.Append(tablename);
                    sb.Append("  set  ");

                    string wheresql = "";


                    int i = 0;
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(t, null) == null ? "" : pi.GetValue(t, null);

                        if (NotDBField(pi))
                            continue;

                        ///排除字段
                        if (exnamearray.Contains(pi.Name.ToLower()))
                        {
                            continue;
                        }

                        if (pi.Name.ToLower() == keyname.ToLower())
                        {
                            havewhere = true;
                            //  wheresql = string.Format(wheresql, pi.Name, pi.GetValue(t, null));
                            wheresql = " where  " + pi.Name + "=" + "'{" + i.ToString() + "}'";

                        }
                        else
                        {






                            switch (obj.GetType().Name.ToLower())
                            {




                                case "int32":
                                    sb.Append(pi.Name);
                                    sb.Append("=");
                                    sb.Append("{" + i.ToString() + "}");
                                    sb.Append(",");
                                    break;

                                case "decimal":
                                    sb.Append(pi.Name);
                                    sb.Append("=");
                                    sb.Append("{" + i.ToString() + "}");
                                    sb.Append(",");
                                    break;

                                case "datetime":
                                    sb.Append(pi.Name);
                                    sb.Append("=");
                                    sb.Append(" TO_DATE('");
                                    sb.Append("{" + i.ToString() + "}");
                                    sb.Append("','yyyy/mm/dd hh24:mi:ss')");
                                    sb.Append(",");
                                    break;

                                default:
                                    sb.Append(pi.Name);
                                    sb.Append("=");
                                    sb.Append("'");
                                    sb.Append("{" + i.ToString() + "}");
                                    sb.Append("'");
                                    sb.Append(",");
                                    break;
                            }
                        }
                        i++;

                    }
                    sb.Remove(sb.Length - 1, 1);

                    if (havewhere)
                    {
                        sb.Append(wheresql);
                    }
                    else
                    {
                        sb.Clear();
                    }
                    cacheupdatesqls[tablename + keyname + exnames] = sb;

                }
                else
                {
                    sb = cacheupdatesqls[tablename + keyname + exnames];
                }
            }

            if (sb.ToString() == "")
            {
                return "";
            }

            List<object> olist = new List<object>();
            foreach (PropertyInfo pi in propertys)
            {

                if (NotDBField(pi))
                    continue;

                ///排除字段
                if (exnamearray.Contains(pi.Name.ToLower()))
                {
                    continue;
                }

                string obj = pi.GetValue(t, null) == null ? "" : pi.GetValue(t, null).ToString();
                obj = obj.Replace("\'", "");
                olist.Add(obj);


            }
            return string.Format(sb.ToString(), olist.ToArray());



        }


        public static string GetUpdateSqlInCache<T>(T t, string tablename, string keynames, string innames)
        {
            PropertyInfo[] propertys = t.GetType().GetProperties();
            StringBuilder sb = new StringBuilder();
            tablename = tablename.ToLower();
            keynames = keynames.ToLower();
            innames = innames.ToLower();
            var innamearray = innames.Split('|');
            var keyarray = keynames.Split('|');

            lock ("object")
            {
                if (!cacheupdatesqls.ContainsKey(tablename + keynames + innames))
                {
                    bool havewhere = false;

                    sb.Append("　Update  ");
                    sb.Append(tablename);
                    sb.Append("  set  ");

                    string wheresql = " where ";


                    int i = 0;
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(t, null) == null ? "" : pi.GetValue(t, null);

                        if (NotDBField(pi))
                            continue;



                        //    if (pi.Name.ToLower() == keynames.ToLower())
                        if (keyarray.Contains(pi.Name.ToLower()))
                        {
                            havewhere = true;
                            //  wheresql = string.Format(wheresql, pi.Name, pi.GetValue(t, null));
                            wheresql += pi.Name + "=" + "'{" + i.ToString() + "}' and ";

                        }
                        else
                        {


                            ///包含字段
                            if (!innamearray.Contains(pi.Name.ToLower()))
                            {
                                continue;
                            }



                            switch (obj.GetType().Name.ToLower())
                            {




                                case "int32":
                                    sb.Append(pi.Name);
                                    sb.Append("=");
                                    sb.Append("{" + i.ToString() + "}");
                                    sb.Append(",");
                                    break;
                                case "decimal":
                                    sb.Append(pi.Name);
                                    sb.Append("=");
                                    sb.Append("{" + i.ToString() + "}");
                                    sb.Append(",");
                                    break;

                                case "datetime":
                                    sb.Append(pi.Name);
                                    sb.Append("=");
                                    sb.Append(" TO_DATE('");
                                    sb.Append("{" + i.ToString() + "}");
                                    sb.Append("','yyyy/mm/dd hh24:mi:ss')");
                                    sb.Append(",");
                                    break;

                                default:
                                    sb.Append(pi.Name);
                                    sb.Append("=");
                                    sb.Append("'");
                                    sb.Append("{" + i.ToString() + "}");
                                    sb.Append("'");
                                    sb.Append(",");
                                    break;
                            }
                        }
                        i++;

                    }
                    sb.Remove(sb.Length - 1, 1);

                    if (havewhere)
                    {
                        sb.Append(wheresql.Remove(wheresql.Length - 4, 4));
                    }
                    else
                    {
                        sb.Clear();
                    }
                    cacheupdatesqls[tablename + keynames + innames] = sb;

                }
                else
                {
                    sb = cacheupdatesqls[tablename + keynames + innames];
                }
            }

            if (sb.ToString() == "")
            {
                return "";
            }

            List<object> olist = new List<object>();
            foreach (PropertyInfo pi in propertys)
            {

                if (NotDBField(pi))
                    continue;

                ///包含字段

                if (innamearray.Contains(pi.Name.ToLower()) || keyarray.Contains(pi.Name.ToLower()))
                {



                    string obj = pi.GetValue(t, null) == null ? "" : pi.GetValue(t, null).ToString();
                    obj = obj.Replace("\'", "");
                    olist.Add(obj);
                }


            }
            return string.Format(sb.ToString(), olist.ToArray());



        }


        //public volatile static Dictionary<string, StringBuilder> cacheinsertsqls = new Dictionary<string, StringBuilder>();

        public static string GetInertSqlCache<T>(T t, string tablename, string seq)
        {
            tablename = tablename.ToLower();
            StringBuilder sb = null;
            PropertyInfo[] propertys = t.GetType().GetProperties();
            lock ("object")
            {
               
                    sb = new StringBuilder();
                    sb.Append("　INSERT INTO  ");
                    sb.Append(tablename);
                    sb.Append("(id,");

                    foreach (PropertyInfo pi in propertys)
                    {
                        if (NotDBField(pi))
                            continue;

                        sb.Append(pi.Name);
                        sb.Append(",");

                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(")");
                    sb.Append("values");
                    sb.Append("("+seq+".nextval,");
                    int i = 0;
                    foreach (PropertyInfo pi in propertys)
                    {
                        string name = pi.Name;
                        if (NotDBField(pi))
                            continue;
                        object obj = pi.GetValue(t, null) == null ? "" : pi.GetValue(t, null);

                        switch (obj.GetType().Name.ToLower())
                        {
                            case "int32":
                                //  sb.Append(pi.GetValue(t, null).ToString());
                                sb.Append("{" + i.ToString() + "}");
                                sb.Append(",");
                                break;
                            case "decimal":
                                //  sb.Append(pi.GetValue(t, null).ToString());
                                sb.Append("{" + i.ToString() + "}");
                                sb.Append(",");
                                break;

                            case "datetime":
                            //sb.Append(" TO_DATE('");
                            sb.Append(pi.GetValue(t, null).ToString());
                            //sb.Append("{" + i.ToString() + "}");
                            //sb.Append("','yyyy/mm/dd hh24:mi:ss')");
                            //sb.Append(",");
                            break;

                            default:
                                sb.Append("'");
                                //sb.Append(pi.GetValue(t, null) == null ? "" : pi.GetValue(t, null).ToString());
                                sb.Append("{" + i.ToString() + "}");
                                sb.Append("'");
                                sb.Append(",");
                                break;
                        }
                        i++;

                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(")");

            }






            List<object> olist = new List<object>();
            foreach (PropertyInfo pi in propertys)
            {

                if (NotDBField(pi))
                    continue;
                

                string obj = pi.GetValue(t, null) == null ? "" : pi.GetValue(t, null).ToString();
             
                obj = obj.Replace("\'", "");
                olist.Add(obj);


            }
            return string.Format(sb.ToString(), olist.ToArray());



        }


    }
}
