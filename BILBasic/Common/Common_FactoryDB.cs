using BILBasic.Basing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BILBasic.Common
{
    public class Common_FactoryDB
    {
        DbFactory dbFactory = new DbFactory(DbFactory.DbFactoryType.SQLSERVER);
            /// <summary>
            /// 分页查询
            /// </summary>
            /// <param name="RecordCounts"></param>
            /// <param name="Tables"></param>
            /// <param name="Filter"></param>
            /// <param name="PageSize"></param>
            /// <param name="PageNumber"></param>
            /// <param name="Fields"></param>
            /// <param name="Sort"></param>
            /// <returns></returns>
            public  IDataReader QueryByDividPage(ref Common.DividPage page, string Tables, string Filter = "", string Fields = "*", string Sort="", string condition="")// string Sort = "Order by ID Desc")
            {


                if (Fields.Trim() == "*")
                {
                    if (Tables.Contains(")N"))
                    {
                        string temtable = Tables.Substring(Tables.Length - 1, 1);
                        if (Fields.Trim() == "*") Fields = temtable + '.' + Fields;
                    }
                    else
                        Fields = Tables + '.' + Fields;
                }


                if (page == null) page = new Common.DividPage();
                if (page.CurrentPageNumber == 0) page.CurrentPageNumber = 1;
                int RecordCounts = 0;

                int TopNumber = page.CurrentPageShowCounts * page.CurrentPageNumber;
                int WhereNumber = (page.CurrentPageNumber - 1) * page.CurrentPageShowCounts;

            string strSqlRecordCounts = "select count(*) as recordcounts  from " + Tables + "  " + Filter + "" + condition;
                using (IDataReader dr =dbFactory.ExecuteReader(strSqlRecordCounts))
                {
                    if (dr.Read())
                    {
                        RecordCounts = int.Parse(dr["recordcounts"].ToString());
                    }
                }



              //  string strSql = "Select * From (Select ROW_NUMBER() OVER(@Sort) AS PageRowNumber , @Fields  From  @Tables  @Filter ) Where PageRowNumber <= @TopNumber And PageRowNumber > @WhereNumber ";

            //string strSql = "select   *   from  (select top @TopNumber ROW_NUMBER()   OVER   (@Sort)   AS   ROWNUM , @Fields  from  @Tables @Filter @Condition  ) t  ";
            string strSql = "select   *   from  (select top @TopNumber ROW_NUMBER()   OVER   (@Sort)   AS   ROWNUM , @Fields  from  @Tables @Filter ) t  ";

            strSql += " where  ROWNUM > @WhereNumber";
            strSql = strSql.Replace("@TopNumber", TopNumber.ToString());
                strSql = strSql.Replace("@Sort", Sort.ToString());
                strSql = strSql.Replace("@Fields", Fields.ToString());
                strSql = strSql.Replace("@Tables", Tables.ToString());
                strSql = strSql.Replace("@Filter", Filter.ToString());
            strSql = strSql.Replace("@Condition", condition.ToString());
            strSql = strSql.Replace("@WhereNumber", WhereNumber.ToString());

                IDataReader dR = dbFactory.ExecuteReader(strSql);

                page.RecordCounts = RecordCounts;
                if (page.RecordCounts > 0)
                {
                    page.PagesCount = (RecordCounts + page.CurrentPageShowCounts - 1) / page.CurrentPageShowCounts;

                }
                else
                {
                    page.PagesCount = 0;
                    page.CurrentPageRecordCounts = 0;
                }

                return dR;
            }

            public  IDataReader QueryByDividPage2(ref Common.DividPage page, string Tables)
            {

                if (page == null) page = new Common.DividPage();
                if (page.CurrentPageNumber == 0) page.CurrentPageNumber = 1;
                int RecordCounts = 0;

                int TopNumber = page.CurrentPageShowCounts * page.CurrentPageNumber;
                int WhereNumber = (page.CurrentPageNumber - 1) * page.CurrentPageShowCounts;

                string strSqlRecordCounts = "select count(*) as recordcounts  from (" + Tables + ") as a";
                using (IDataReader dr = dbFactory.ExecuteReader(strSqlRecordCounts))
                {
                    if (dr.Read())
                    {
                        RecordCounts = int.Parse(dr["recordcounts"].ToString());
                    }
                }
                //string strSql = "Select * From (Select ROW_NUMBER() OVER(@Sort) AS PageRowNumber , *  From  (@Tables) ) Where PageRowNumber <= @TopNumber And PageRowNumber > @WhereNumber ";
                string strSql = "Select * From (@Tables) as a Where PageRowNumber <= @TopNumber And PageRowNumber > @WhereNumber ";

                strSql = strSql.Replace("@TopNumber", TopNumber.ToString());
                //strSql = strSql.Replace("@Sort", Sort.ToString());
                //strSql = strSql.Replace("@Fields", Fields.ToString());
                strSql = strSql.Replace("@Tables", Tables.ToString());
                //strSql = strSql.Replace("@Filter", Filter.ToString());
                strSql = strSql.Replace("@WhereNumber", WhereNumber.ToString());

                IDataReader dR = dbFactory.ExecuteReader(strSql);

                page.RecordCounts = RecordCounts;
                if (page.RecordCounts > 0)
                {
                    page.PagesCount = (RecordCounts + page.CurrentPageShowCounts - 1) / page.CurrentPageShowCounts;


                }
                else
                {
                    page.PagesCount = 0;
                    page.CurrentPageRecordCounts = 0;
                }

                return dR;
            }

            public  DataTable QueryByDividPage(ref Common.DividPage page, string sql)
            {

                if (page == null) page = new Common.DividPage();
                if (page.CurrentPageNumber == 0) page.CurrentPageNumber = 1;
                int RecordCounts = 0;

                int TopNumber = page.CurrentPageShowCounts * page.CurrentPageNumber;
                int WhereNumber = (page.CurrentPageNumber - 1) * page.CurrentPageShowCounts;

                string strSqlRecordCounts = "select count(*) as recordcounts  from (" + sql + ") as a";
                using (IDataReader dr = dbFactory.ExecuteReader(strSqlRecordCounts))
                {
                    if (dr.Read())
                    {
                        RecordCounts = int.Parse(dr["recordcounts"].ToString());
                    }
                }
                //string strSql = "Select * From (Select ROW_NUMBER() OVER(@Sort) AS PageRowNumber , *  From  (@Tables) ) Where PageRowNumber <= @TopNumber And PageRowNumber > @WhereNumber ";
                string strSql = "Select * From (@Tables) as a Where PageRowNumber <= @TopNumber And PageRowNumber > @WhereNumber ";

                strSql = strSql.Replace("@TopNumber", TopNumber.ToString());
                //strSql = strSql.Replace("@Sort", Sort.ToString());
                //strSql = strSql.Replace("@Fields", Fields.ToString());
                strSql = strSql.Replace("@Tables", sql.ToString());
                //strSql = strSql.Replace("@Filter", Filter.ToString());
                strSql = strSql.Replace("@WhereNumber", WhereNumber.ToString());

                DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, strSql).Tables[0];

                page.RecordCounts = RecordCounts;
                if (page.RecordCounts > 0)
                {
                    page.PagesCount = (RecordCounts + page.CurrentPageShowCounts - 1) / page.CurrentPageShowCounts;


                }
                else
                {
                    page.PagesCount = 0;
                    page.CurrentPageRecordCounts = 0;
                }

                return dt;
            }


            public  int CheckRepeatValue(string TableName, string ColumnName, object Value, int ID, string IDColumnName = "ID")
            {
                try
                {
                    if (Value == null) return 0;
                    //if (string.IsNullOrEmpty(value.ToString())) return 0;

                    int iOut = 0;
                    string strError;
                    int RepeatNum = 0;
                dbFactory.dbF.CreateParameters(7);
                //  dBFactory.AddParameters(0, "@ErrorMsg", OracleDbType.NVarchar2, 100);
                dbFactory.dbF.AddParameters(1, "@RepeatNum", SqlDbType.Int,0);
                dbFactory.dbF.AddParameters(0, "@TableName", dbFactory.ToDBValue(TableName),0);
                dbFactory.dbF.AddParameters(0, "@ColumnName", dbFactory.ToDBValue(ColumnName), 0);
                dbFactory.dbF.AddParameters(0, "@Value", dbFactory.ToDBValue(Value), 0);
                dbFactory.dbF.AddParameters(0, "@IDColumnName", dbFactory.ToDBValue(IDColumnName), 0);
                dbFactory.dbF.AddParameters(0, "@ID", dbFactory.ToDBValue(ID), 0);



                dbFactory.dbF.Parameters[0].Direction = System.Data.ParameterDirection.Output;
                dbFactory.dbF.Parameters[1].Direction = System.Data.ParameterDirection.Output;

                    int i = dbFactory.RunProcedure("P_Check_RepeatValue", dbFactory.dbF.Parameters, out iOut);
                    strError = dbFactory.dbF.Parameters[0].Value.ToString();
                    RepeatNum = Convert.ToInt32(dbFactory.dbF.Parameters[1].Value);
                    return RepeatNum;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
        }


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

        public static string GetInertSqlCache<T>(T t, string tablename)
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
                    if(pi.Name.ToUpper().Equals("ID"))
                        continue;
                    sb.Append(pi.Name);
                    sb.Append(",");

                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(")");
                sb.Append("values");
                sb.Append("(");
                //sb.Append("(" + seq + ".nextval,");
                int i = 0;
                foreach (PropertyInfo pi in propertys)
                {
                    string name = pi.Name;
                    if (NotDBField(pi))
                        continue;
                    if (pi.Name.ToUpper().Equals("ID"))
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
