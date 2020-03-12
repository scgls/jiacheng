using BTOBDLL.Common;
using BTOBDLL.XMLUtil;
using MESDLL.OutStock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EMSDLL
{
    public class EMSDLL
    {
        public string PostOutStockToEms(string PostJson) 
        {
            ReturnMessage returnMsg = new ReturnMessage();
            try
            {

                if (string.IsNullOrEmpty(PostJson))
                {
                    return returnMsg.FlaseReturnMessage("传入圆通接口JSON为空值！");
                }

                List<RequestOrder> PostErpModelList = new PostErp_Model().GetPostErpField(PostJson, "PostEMS").ToObject<List<RequestOrder>>();

                LogNet.LogInfo("数据回传EMS(PostOutStockToEms)字段转换数据:---" + JsonConvert.SerializeObject(PostErpModelList));

                if (PostErpModelList == null || PostErpModelList.Count == 0)
                {
                    return returnMsg.FlaseReturnMessage("解析传入数据为空值！");
                }

                RequestOrder model = new RequestOrder();
                model.parternId = XMLHelper.GetXmlAddress("parternId");
                model.clientID = XMLHelper.GetXmlAddress("clientID");
                model.logisticProviderID = XMLHelper.GetXmlAddress("logisticProviderID");
                model.customerId = XMLHelper.GetXmlAddress("clientID");
                model.txLogisticID = PostErpModelList[0].txLogisticID;
                model.tradeNo = ""; //PostErpModelList[0].tradeNo;
                model.totalServiceFee =PostErpModelList[0].totalServiceFee;
                //model.codSplitFee = PostErpModelList[0].codSplitFee;
                model.orderType = XMLHelper.GetXmlAddress("orderType");
                model.serviceType = XMLHelper.GetXmlAddress("serviceType");
                model.flag = XMLHelper.GetXmlAddress("flag");
                model.sendStartTime = PostErpModelList[0].sendStartTime; 
                model.sendEndTime = PostErpModelList[0].sendEndTime;
                model.goodsValue = PostErpModelList[0].goodsValue;
                model.itemsValue = PostErpModelList[0].itemsValue;
                model.insuranceValue = PostErpModelList[0].insuranceValue;
                //model.special = PostErpModelList[0].special;
                model.type = "0";//PostErpModelList[0].type;
                //model.totalValue = PostErpModelList[0].totalValue;
                model.itemsWeight = PostErpModelList[0].itemsWeight; 

                model.sender = new Sender();
                model.sender.name = XMLHelper.GetXmlAddress("sname");
                model.sender.postCode = XMLHelper.GetXmlAddress("postCode");
                model.sender.phone = XMLHelper.GetXmlAddress("phone");
                model.sender.mobile = XMLHelper.GetXmlAddress("mobile");
                model.sender.prov = XMLHelper.GetXmlAddress("prov");//"上海"; //XMLHelper.GetXmlAddress("prov");
                model.sender.city = XMLHelper.GetXmlAddress("city");//"上海,青浦区";//XMLHelper.GetXmlAddress("city");
                model.sender.address = XMLHelper.GetXmlAddress("address");//"上海市青浦区华徐公路民兴大道"; //XMLHelper.GetXmlAddress("address");

                model.receiver = new Receiver();
                model.receiver.name = PostErpModelList[0].namer;
                model.receiver.postCode = PostErpModelList[0].postCoder;
                model.receiver.phone = PostErpModelList[0].phoner;
                model.receiver.mobile = PostErpModelList[0].mobiler;
                model.receiver.prov = PostErpModelList[0].provr;
                model.receiver.area = PostErpModelList[0].arear;
                model.receiver.city = PostErpModelList[0].cityr + "," + model.receiver.area;

                model.receiver.address = PostErpModelList[0].addressr;

                //model.items = new List<item>();

                //foreach (var itemModel in PostErpModelList)
                //{
                //    item item = new item();
                //    item.itemName = "asdasdda"; //itemModel.itemName;
                //    item.itemValue = 1;//itemModel.itemsValue;
                //    model.items.Add(item);
                //}
                                

                string strResult = string.Empty;
                string strUrl = XMLHelper.GetXmlAddress("EMSSERVICE");
                

                LogNet.LogInfo("EMS数据提交(PostOutStockToEms)开始时间:---" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));

                MD5 md5Hasher = MD5.Create();

                string xmlBuilder = XmlUtil.Serializer(typeof(RequestOrder), model);

                string postData = "logistics_interface=" + System.Web.HttpUtility.UrlEncode(xmlBuilder, Encoding.UTF8)
                            + "&data_digest=" + System.Web.HttpUtility.UrlEncode(Convert.ToBase64String(md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(xmlBuilder + model.parternId))), Encoding.UTF8)
                            + "&clientId=" + System.Web.HttpUtility.UrlEncode(model.clientID, Encoding.UTF8);

                

                //调用圆通接口
                strResult = HTTPUtils.GetResultXML(strUrl, postData, null);

                LogNet.LogInfo("EMS数据提交(PostOutStockToEms)结束时间:---" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                LogNet.LogInfo("EMS数据提交(PostOutStockToEms)请求数据:---" +strUrl + "---" + postData);
                LogNet.LogInfo("EMS数据提交(PostOutStockToEms)返回数据:---" + strResult);

                Response resModel = (Response)XmlUtil.Deserialize(typeof(Response), strResult);

                if (resModel.success == "false")
                {
                    return returnMsg.FlaseReturnMessage("EMS数据提交失败:"+resModel.reason);
                }

                return returnMsg.TrueReturnMessage(resModel,model);
                

            }
            catch (Exception ex)
            {
                return returnMsg.FlaseReturnMessage("EMS数据提交(PostOutStockToEms)异常：" + ex.Message);
            }
        }
    }
}

#region 返回格式

//<Response>
//    <clientID>K21000119</clientID>
//    <code>200</code>
//    <distributeInfo>
//        <consigneeBranchCode>210016</consigneeBranchCode>
//        <packageCenterCode>210902</packageCenterCode>
//        <packageCenterName>区域件</packageCenterName>
//        <printKeyWord></printKeyWord>
//        <shortAddress>330-251-00-121</shortAddress>
//    </distributeInfo>
//    <effectType>1</effectType>
//    <estimatedTrrivalTime>2019-08-10</estimatedTrrivalTime>
//    <logisticProviderID>YTO</logisticProviderID>
//    <mailNo>YT2990005674414</mailNo>
//    <originateOrgCode>210045</originateOrgCode>
//    <pickUpTime>2019-08-09 18:00:00</pickUpTime>
//    <qrCode>{"mn":"YT2990005674414","pcn":"区域件","rbc":"210016","sbc":"210045","ssc":"330-251-00","tsc":"121"}</qrCode>
//    <success>true</success>
//    <txLogisticID>FY2-HH2-1907310001</txLogisticID>
//</Response>

#endregion
