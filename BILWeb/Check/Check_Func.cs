using BILBasic.Basing;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Query
{
    public class Check_Func
    {
        public DbFactory dbFactory = new DbFactory(DbFactory.DbFactoryType.SQLSERVER);

        public static void ChangeQuery(ref string t)
        {
            string[] list = t.Split(',');
            for (int i = 0; i < list.Count(); i++)
            {
                list[i] = "'" + list[i] + "'";
            }
            string res = "";
            foreach (string j in list)
            {
                res += j + ",";
            }
            t = res.Substring(0, res.Length - 1);
        }

        public string GetTableID(string strSeq)
        {
            try
            {
                //sqlserver
                return "PD" + DateTime.Now.ToString("yyMMddHHmmssffff");

                //oracle取id方法
                //string strSql = "select " + strSeq + ".Nextval  from dual";
                //using (IDataReader reader = dbFactory.ExecuteReader(strSql))
                //{
                //    if (reader.Read())
                //    {
                //        int ID = Convert.ToInt32(reader["Nextval"]);
                //        string strid = getSqu(ID);
                //        return "PD" + DateTime.Now.ToString("yyyyMMdd") + strid;
                //    }
                //    else
                //    {
                //        throw new Exception("取单据ID出错！");
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //获取盘点单号
        public string GetPDNoAndroid()
        {
            string no = GetTableID("SEQ_CHECK_NO");
            BaseMessage_Model<string> bm = new BaseMessage_Model<string>();
            bm.HeaderStatus = "S";
            bm.Message = "";
            bm.ModelJson = no;
            string j = Check_Func.SerializeObject(bm);
            return j;
        }


        public static string getSqu(int s)
        {
            string ss = s.ToString();
            if (ss.Length >= 4)
                ss = ss.Substring(ss.Length - 4, 4);
            else
            {
                ss = "0000" + ss;
                ss = ss.Substring(ss.Length - 4, 4);
            }
            return ss;
        }

        public static int GetSeqID(string strSeq)
        {
            try
            {
                string strSql = "select " + strSeq + ".Nextval  from dual";
                DbFactory dbFactory = new DbFactory(DbFactory.DbFactoryType.SQLSERVER);
                using (IDataReader reader = dbFactory.ExecuteReader(strSql))
                {
                    if (reader.Read())
                    {
                        int ID = Convert.ToInt32(reader["Nextval"]);
                        return ID;
                    }
                    else
                    {
                        throw new Exception("取单据ID出错！");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }


        //json
        public static string SerializeObject(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }
        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }

    }

    //datatable 转 list
    public class ModelConvertHelper<T> where T : new()
    {
        public static List<T> ConvertToModel(DataTable dt)
        {
            // 定义集合    
            List<T> ts = new List<T>();

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    

                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            if (pi.PropertyType == typeof(int))
                                value = Convert.ToInt32(value);
                            pi.SetValue(t, value, null);
                        }
                            
                    }
                }
                ts.Add(t);
            }
            return ts;
        }
    } 


  




}
