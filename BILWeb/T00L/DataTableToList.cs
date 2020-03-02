using System;
using System.Data;
using System.Configuration;
using System.Linq;

using System.Xml.Linq;
using System.Collections.Generic;
using System.Reflection;



namespace BILWeb.TOOL
{
    public class DataTableToList
    {

        public static List<T> DataSetToList<T>(DataTable p_DataTable)
        {
            if (p_DataTable == null || p_DataTable.Rows.Count < 0)
                return null;

            //DataTable p_Data = p_DataSet.Tables[p_TableIndex];
            // 返回值初始化 
            List<T> result = new List<T>();
            for (int j = 0; j < p_DataTable.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_DataTable.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.ToLower().Equals(p_DataTable.Columns[i].ColumnName.ToLower()))
                        {
                            object value = p_DataTable.Rows[j][i];
                            // 数据库NULL值单独处理 
                            if (value != DBNull.Value)
                            {
                                pi.SetValue(_t, value, null);
                            }
                            else
                            {
                                pi.SetValue(_t, null, null);
                            }
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }
        private static bool IsInt(string value)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"^[0-9]*$");
        }
    }
}
