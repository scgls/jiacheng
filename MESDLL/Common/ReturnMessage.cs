
using MESDLL.OutStock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTOBDLL.Common
{
    public class ReturnMessage
    {
        public string FlaseReturnMessage(string strError) 
        {
            Model.JsonReturn jr = new Model.JsonReturn();
            jr.Result = "false";
            jr.resultValue = strError;
            jr.ErrMsg = strError;
            return JsonConvert.SerializeObject(jr);
        }

        public string TrueReturnMessage(Response model, RequestOrder reqModel)
        {

            Model.JsonReturn jr = new Model.JsonReturn();
            jr.Result = "true";
            jr.resultValue = string.Empty;
            jr.data = null;
            jr.MaterialDoc = model.mailNo;
            jr.ErrMsg = model.mailNo + "@" + model.distributeInfo.shortAddress + "@" + model.distributeInfo.consigneeBranchCode + "@" + model.qrCode +
                "@" + reqModel.sender.name + "@" + reqModel.sender.phone + "@" + reqModel.sender.postCode + "@" + reqModel.sender.prov + "@" +
                reqModel.sender.city + "@" + reqModel.sender.address;
            jr.shortAddress = model.distributeInfo.shortAddress;
            jr.consigneeBranchCode = model.distributeInfo.consigneeBranchCode;
            jr.qrCode = model.qrCode;
            
           
           
            return JsonConvert.SerializeObject(jr);
        }

        public string TrueReturnMessageForB2B(string data, MaterialDoc_Model MaterialDoc)
        {
            string newData = string.Empty;
            newData = data.Insert(0, "{\"data\":[");
            newData = newData.Insert(newData.Length - 1, "}]");
            return newData;  
        }


        public class MaterialDoc_Model
        {
            /// <summary>
            /// 物料凭证号
            /// </summary>   

            public string MaterialDoc { get; set; }

            /// <summary>
            /// 凭证年度
            /// </summary>

            public string MaterialDocDate { get; set; }

            /// <summary>
            /// 凭证记账日期
            /// </summary>

            public DateTime MaterialDocPost { get; set; }

            /// <summary>
            /// 凭证类型
            /// </summary>

            public int MaterialDocType { get; set; }

            public string GUID { get; set; }

            /// <summary>
            /// 检验批号
            /// </summary>
            public string QualityNo { get; set; }

            public List<string> lstQuality { get; set; }

        }

    }

}
