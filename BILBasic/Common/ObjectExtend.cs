using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILBasic.Common
{
    public static class ObjectExtend
    {
        public static decimal ToDecimal(this object o)
        {
            if (o == null || o == DBNull.Value) return 0;
            if (string.IsNullOrEmpty(o.ToString())) return 0;

            try
            {
                return Convert.ToDecimal(o);
            }
            catch
            {
                return 0;
            }
        }
        public static decimal? ToDecimalNull(this object o)
        {
            if (o == null || o == DBNull.Value) return null;
            if (string.IsNullOrEmpty(o.ToString())) return null;

            try
            {
                return Convert.ToDecimal(o);
            }
            catch
            {
                return null;
            }
        }

        public static DateTime ToDateTime(this object o)
        {
            if (o == null || o == DBNull.Value) return DateTime.MinValue;
            if (string.IsNullOrEmpty(o.ToString())) return DateTime.MinValue;

            try
            {
                return Convert.ToDateTime(o);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime? ToDateTimeNull(this object o)
        {
            if (o == null || o == DBNull.Value) return null;
            if (string.IsNullOrEmpty(o.ToString())) return null;

            try
            {
                return Convert.ToDateTime(o);
            }
            catch
            {
                return null;
            }
        }

        public static int ToInt32(this object o)
        {
            if (o == null || o == DBNull.Value) return 0;
            if (string.IsNullOrEmpty(o.ToString())) return 0;

            try
            {
                return Convert.ToInt32(o);
            }
            catch
            {
                return 0;
            }
        }

        public static long ToLong(this object o)
        {
            if (o == null || o == DBNull.Value) return 0;
            if (string.IsNullOrEmpty(o.ToString())) return 0;

            try
            {
                return long.Parse(o.ToString());
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 2为True
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static int ToInt32(this bool o)
        {
            try
            {
                return o ? 2 : 1;
            }
            catch
            {
                return 1;
            }
        }

        public static int? ToIntNull(this object o)
        {
            if (o == null || o == DBNull.Value) return null;
            if (string.IsNullOrEmpty(o.ToString())) return null;

            try
            {
                return Convert.ToInt32(o);
            }
            catch
            {
                return null;
            }
        }

        public static bool ToBoolean(this object o)
        {
            if (o == null || o == DBNull.Value) return false;
            if (string.IsNullOrEmpty(o.ToString())) return false;

            try
            {
                if (o.GetType() == typeof(int) || o.GetType() == typeof(decimal))
                {
                    return o.ToInt32().ToBoolean();
                }

                return Convert.ToBoolean(o);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 2为True
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool ToBoolean(this int o)
        {
            try
            {
                return o == 2;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 2为True
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool ToBoolean(this decimal o)
        {
            try
            {
                return o == 2;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 按照字节截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string StrCut(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            int len = 0;
            byte[] b;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < str.Length; i++)
            {
                b = Encoding.Default.GetBytes(str.Substring(i, 1));
                if (b.Length > 1)
                    len += 2;
                else
                    len++;

                if (len > length)
                    break;

                sb.Append(str[i]);
            }

            return sb.ToString();
        }

        public static int StrLength(this string str)
        {
            int len = 0;
            byte[] b;

            for (int i = 0; i < str.Length; i++)
            {
                b = Encoding.Default.GetBytes(str.Substring(i, 1));
                if (b.Length > 1)
                    len += 2;
                else
                    len++;
            }

            return len;
        }

        public static string ToDBString(this object o)
        {
            if (o == null || o == DBNull.Value)
            {
                return string.Empty;
            }
            else if (o.ToString().ToLower() == "null")
            {
                return string.Empty;
            }
            else
            {
                return o.ToString();
            }
        }

        public static string ToOracleTimeString(this object o)
        {
            if (o == null) return "NULL";

            return string.Format("TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss')", o.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public static string ToShowTime(this DateTime o)
        {
            if (o == o.Date)
            {
                return o.ToString("yyyy年MM月dd日");
            }
            else
            {
                return o.ToString("yyyy年MM月dd日 HH:mm:ss");
            }
        }

        public static string ToShowTime(this DateTime? o)
        {
            if (o == null)
            {
                return "";
            }
            else
            {
                return o.ToDateTime().ToShowTime();
            }
        }

        public static object ToOracleValue(this object o)
        {
            if (o == null)
            {
                return DBNull.Value;
            }
            else
            {
                if (o.GetType() == typeof(bool))
                {
                    return o.ToBoolean() ? 2 : 1;
                }
                else
                {
                    return o;
                }
            }
        }

        public static string TrimSapZero(this string o)
        {
            if (string.IsNullOrEmpty(o))
            {
                return string.Empty;
            }
            else
            {
                if (Common_Func.IsAllZero(o)) return string.Empty;
                if (o.StartsWith("0000")) return o.Substring(4);

                return o;
            }
        }

        /// <summary>
        /// 根据属性数据类型强制转换
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object GetPropertyValue(System.Reflection.PropertyInfo pi, object value)
        {
            if (pi.PropertyType.Equals(typeof(string)))
                return value.ToDBString();
            else if (pi.PropertyType.Equals(typeof(int)))
                return value.ToInt32();
            else if (pi.PropertyType.Equals(typeof(decimal)))
                return value.ToDecimal();
            else if (pi.PropertyType.Equals(typeof(DateTime)))
                return value.ToDateTime();
            else if (pi.PropertyType.Equals(typeof(Boolean)))
                return value.ToInt32() == 1;
            else
                return null;
        }
        //判断字段
        public static bool CheckStr(this object o)
        {
            if (o==null)
            {
                return false;
            }
            if (o.ToString() != "" && o.ToString().Contains(','))
            {
                return true;
            }
            return false;
        }
    }
}
