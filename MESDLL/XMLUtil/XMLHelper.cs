using BTOBDLL.XMLUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace EMSDLL
{
    public class XMLHelper
    {
        public static string GetXmlAddress(string title)
        {
            XmlDocument xmlDoc = GetXmlPath();
            XmlNode xmlNode1 = xmlDoc.SelectSingleNode("/setting/" + title);
            return xmlNode1.InnerText;
        }

        private static XmlDocument GetXmlPath()
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                //xmlDoc.Load(@"E:\WMS产品\Z正泰集团\SourceSAP\SCGWMS\SAPDLL\bin\Debug\apiconfig.xml");//测试服务器路径
                xmlDoc.Load(@"C:\mswmsWcf\MESDLL\apiconfig.xml");//正式服务器路径
                //xmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "bin\\dll\\apiconfig.xml");//xmlPath为xml文件路径
            }
            catch
            {
                xmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "apiconfig.xml");//xmlPath为xml文件路径

            }
            return xmlDoc;
        }

        /// <summary>
        /// 获取某个节点下的子节点，转换成对应关系的dictionary
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static List<XML_Model> GetXml(string title)
        {

            XmlDocument xmlDoc = GetXmlPath();
            XmlNode xmlNode1 = xmlDoc.SelectSingleNode("/setting/" + title);
            List<XML_Model> list = new List<XML_Model>();
            foreach (XmlNode node in xmlNode1.ChildNodes)
            {
                XML_Model model = new XML_Model();
                model.erp = node.Name;
                model.wms = node.Attributes["wms"].Value.Trim();
                model.def = node.Attributes["def"].Value.Trim();
                list.Add(model);
            }
            return list;
        }

    }
}
