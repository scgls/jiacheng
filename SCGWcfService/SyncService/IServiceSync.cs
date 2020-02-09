using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SCGWcfService
{
    public partial interface IService
    {

        [OperationContract]
        bool DocumentSyncReceipt(string lastSyncTime,string ErpVoucherNo, ref string ErrorMsg, int WmsVoucherType=-1, string syncType = "ERP", int syncExcelVouType = -1, DataSet excelds = null);

        [OperationContract]
        bool DocumentSyncDelivery(string lastSyncTime,string ErpVoucherNo, ref string ErrorMsg, int WmsVoucherType = -1, string syncType = "ERP", int syncExcelVouType = -1, DataSet excelds = null);

        [OperationContract]
        bool DocumentSyncBase(string lastSyncTime,string ErpVoucherNo, ref string ErrorMsg, int WmsVoucherType = -1, string syncType = "ERP", int syncExcelVouType = -1, DataSet excelds = null);
        //[OperationContract]
        //bool DocumentSyncWoInfo(string lastSyncTime, string ErpVoucherNo, ref string ErrorMsg, int WmsVoucherType = 32);

        //[OperationContract]
        //bool DocumentSyncSubmitMesStatus(string StatusJson, ref string ErrMsg);

    }
}
