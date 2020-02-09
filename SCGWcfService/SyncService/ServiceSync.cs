using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {

        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="lastSyncTime">最后同步时间</param>
        /// <param name="ErpVoucherNo">ERP单据号，为空：按照最后同步时间查询，有：</param>
        /// <param name="errorMsg"></param>
        /// <param name="syncType"></param>
        /// <param name="syncExcelVouType"></param>
        /// <param name="excelds"></param>
        /// <returns></returns>
        public bool DocumentSyncReceipt(string lastSyncTime, string ErpVoucherNo, ref string errorMsg, int WmsVoucherType=-1, string syncType = "ERP", int syncExcelVouType = -1, DataSet excelds = null)
        {
            BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
            return PFunc.Sync((int)GolabelModel.StockType.IN, lastSyncTime, ErpVoucherNo, WmsVoucherType, ref errorMsg, syncType, syncExcelVouType, excelds);
        }

        public bool DocumentSyncDelivery(string lastSyncTime, string ErpVoucherNo, ref string ErrorMsg, int WmsVoucherType = -1, string syncType = "ERP", int syncExcelVouType = -1, DataSet excelds = null)
        {
            BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
            return PFunc.Sync((int)GolabelModel.StockType.OUT, lastSyncTime, ErpVoucherNo, WmsVoucherType, ref ErrorMsg, syncType, syncExcelVouType, excelds);
        }


        //public bool DocumentSyncWoInfo(string lastSyncTime, string ErpVoucherNo, ref string ErrorMsg, int WmsVoucherType =32)
        //{
        //    BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
        //    return PFunc.SyncWO(lastSyncTime, ErpVoucherNo, ref ErrorMsg, (int)GolabelModel.StockType.WO, WmsVoucherType);
        //}

        //public bool DocumentSyncSubmitMesStatus(string StatusJson, ref string ErrMsg)
        //{
        //    BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
        //    return PFunc.SubmitMesStatus(StatusJson, ref ErrMsg);
        //}
    

    public bool DocumentSyncBase(string lastSyncTime,string ErpVoucherNo, ref string ErrorMsg, int WmsVoucherType = -1, string syncType = "ERP", int syncExcelVouType = -1, DataSet excelds = null)
        {
            BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
            return PFunc.Sync((int)GolabelModel.StockType.BASE, lastSyncTime, ErpVoucherNo, WmsVoucherType, ref ErrorMsg, syncType, syncExcelVouType, excelds);
        }
    }
}