using Newtonsoft.Json.Linq;
using BTOBDLL.XMLUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMSDLL;

namespace BTOBDLL.Common
{
    public class PostErp_Model
    {
        /// <summary>
        /// 将配置XML文件中的WMS字段值转换成ERP需要过账的字段值
        /// </summary>
        /// <param name="PostJson"></param>
        /// <returns></returns>
        public JArray GetPostErpField(string PostJson,string PostType) 
        {
            JArray WmsPostJson = JArray.Parse(PostJson);
            JArray ErpPostJson = new JArray();
            List<XML_Model> listx = XMLHelper.GetXml(PostType);

            string value = "";

            for (int i = 0; i < WmsPostJson.Count; i++)
            {
                JObject Oerp = new JObject();
                foreach (XML_Model xmodel in listx)
                {
                    if (WmsPostJson[i][xmodel.wms] == null || WmsPostJson[i][xmodel.wms].ToString() == "")
                        value = xmodel.def;
                    else
                        value = WmsPostJson[i][xmodel.wms].ToString();
                    JProperty jp = new JProperty(xmodel.erp, value);
                    Oerp.Add(jp);
                }
                ErpPostJson.Add(Oerp);
            }

            return ErpPostJson;
        }
    }
}
