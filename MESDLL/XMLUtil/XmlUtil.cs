using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace EMSDLL
{
    public class XmlUtil : Dictionary<string, string>  
    {
        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(Type type, object obj)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            //去除xml声明
            settings.OmitXmlDeclaration = true;
            settings.Encoding = Encoding.Default;
            System.IO.MemoryStream mem = new MemoryStream();
            using (XmlWriter writer = XmlWriter.Create(mem, settings))
            {
                //去除默认命名空间xmlns:xsd和xmlns:xsi
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlSerializer formatter = new XmlSerializer(obj.GetType());
                formatter.Serialize(writer, obj, ns);
            }
            return Encoding.Default.GetString(mem.ToArray());
            //MemoryStream Stream = new MemoryStream();
            //XmlSerializer xml = new XmlSerializer(type);
            //XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            //ns.Add("", "");//把命名空间设置为空，这样就没有命名空间了
            //XmlWriterSettings settings = new XmlWriterSettings();
            ////去除xml声明
            //settings.OmitXmlDeclaration = true;
            //settings.Encoding = Encoding.Default;
            //try
            //{
            //    ns.Add("", "");
            //    //序列化对象
            //    xml.Serialize(Stream, obj,ns);
            //}
            //catch (InvalidOperationException)
            //{
            //    throw;
            //}
            //Stream.Position = 0;
            //StreamReader sr = new StreamReader(Stream);
            //string str = sr.ReadToEnd();

            //sr.Dispose();
            //Stream.Dispose();

            //return str;
        }

        #endregion


    }
}
