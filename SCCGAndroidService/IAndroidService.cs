using BILWeb.InStock;
using BILWeb.Login.User;
using BILWeb.OutBarCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SCCGAndroidService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IAndroidService
    {
        #region 用户登录模块

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UserLoginADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string UserLoginADF(string UserJson);

        #endregion

        #region 根据用户编码获取仓库信息

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetWareHouseByUserADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetWareHouseByUserADF(string UserNo);

        #endregion

        #region 预收货

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_MaterialPackADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_MaterialPackADF(string UserJson, string ModelJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Post_SaveAdvInStock", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string Post_SaveAdvInStock(string UserJson, string advInStock);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Get_AdvInParameter", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string Get_AdvInParameter(string groupname);
        #endregion

        #region 托盘和装箱模块

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_PalletDetailByNoADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_PalletDetailByNoADF(string Barcode, string PalletModel);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Get_PalletDetailByVoucherNo", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string Get_PalletDetailByVoucherNo(String VoucherNo);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Del_PalletOrSerialNo", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string Del_PalletOrSerialNo(String PalletNo, String SerialNo);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "DeletePalletByErpVoucherNo", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string DeletePalletByErpVoucherNo(string ErpVoucherNo, string PalletType);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SaveT_PalletDetailADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string SaveT_PalletDetailADF(string UserJson, string ModelJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SaveT_BarCodeToStockADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string SaveT_BarCodeToStockADF(string UserJson, string strOldBarCode, string strNewBarCode, string PrintFlag);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Delete_PalletORBarCodeADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string Delete_PalletORBarCodeADF(string UserJson, string PalletDetailJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_OutBarCodeInfoByBoxADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_OutBarCodeInfoByBoxADF(string BarCode);

        #endregion

        #region 收货模块

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_InStockListADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_InStockListADF(string UserJson, string ModelJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_SerialNoADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_SerialNoADF(string SerialNo);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_SerialNobyymhADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_SerialNobyymhADF(string SerialNo);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_PalletDetailByBarCodeADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_PalletDetailByBarCodeADF(string UserJson, string BarCode);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_InStockDetailListByHeaderIDADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_InStockDetailListByHeaderIDADF(string ModelDetailJson);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SaveT_InStockDetailADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string SaveT_InStockDetailADF(string UserJson, string ModelJson);

        #endregion

        #region 上架模块

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_SerialNoInStockADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_SerialNoInStockADF(string SerialNo);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_InTaskListADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_InTaskListADF(string UserJson, string ModelJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetInStockModelADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetInStockModelADF(string SerialNo, string VoucherNo, string TaskNo);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_ScanInStockModelADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_ScanInStockModelADF(string SerialNo, string ERPVoucherNo, string TaskNo, string AreaNo, int WareHouseID);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_InTaskDetailListByHeaderIDADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_InTaskDetailListByHeaderIDADF(string ModelDetailJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetAreaModelADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]

        string GetAreaModelADF(string UserJson, string AreaNo);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "LockTaskOperUserADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string LockTaskOperUserADF(string TaskDetailsJson, string UserJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UnLockTaskOperUserADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string UnLockTaskOperUserADF(string TaskDetailsJson, string UserJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SaveT_InStockTaskDetailADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string SaveT_InStockTaskDetailADF(string UserJson, string ModelJson);


        #endregion

        #region 补货
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_MoveTaskInfo", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_MoveTaskInfo(string UserJson, string ModelJson);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Save_MoveTask", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string Save_MoveTask(string UserJson, string ModelJson);

        #endregion

        #region 移库管理

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_StockInfoADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_StockInfoADF(string UserJson, string ModelJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_AreaNOInfoADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_AreaNOInfoADF(string areaNO);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_AreaInfoADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_AreaInfoADF(string UserJson, string areaNO);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Save_MoveInfoADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string Save_MoveInfoADF(string UserJson, string StockJson, string AreaJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SaveT_StockADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string SaveT_StockADF(string UserJson, string ModelJson);

        #endregion

        #region 拣货模块

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_OutTaskListADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_OutTaskListADF(string UserJson, string ModelJson);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_OutTaskDetailListByHeaderIDADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_OutTaskDetailListByHeaderIDADF(string ModelDetailJson);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetStockModelADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetStockModelADF(string ModelStockJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetBarcodeModelForJADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetBarcodeModelForJADF(string Serialno);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SaveT_OutStockTaskDetailADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string SaveT_OutStockTaskDetailADF(string UserJson, string ModelJson,string Guid);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetCarModelADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetCarModelADF(string CarNo, string TaskNo, string strUserNo);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SaveBoxListADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string SaveBoxListADF(string UserJson, string ModelJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SaveCobBoxListADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string SaveCobBoxListADF(string UserJson,string ModelJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ScanBoxSerial", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string ScanBoxSerial(string strSerialNo);

        #endregion

        #region 拣货锁定和解锁任务

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "LockTaskOperUser", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string LockTaskOperUser(string TaskOutStockModelJson, string UserJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UnLockTaskOperUser", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string UnLockTaskOperUser(string TaskOutStockModelJson, string UserJson);

        #endregion

        #region 拣货单补货

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_MoveTaskScatInfo", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_MoveTaskScatInfo(string UserJson, string ModelJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Save_MoveTaskScat", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string Save_MoveTaskScat(string UserJson, string ModelJson);

        #endregion

        #region 复核模块

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_OutStockReviewListADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_OutStockReviewListADF(string UserJson, string ModelJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_OutStockReviewDetailListByHeaderIDADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_OutStockReviewDetailListByHeaderIDADF(string ModelDetailJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetReviewStockModelADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetReviewStockModelADF(string ModelStockJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SaveT_OutStockReviewDetailADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string SaveT_OutStockReviewDetailADF(string UserJson, string ModelJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "PostT_OutStockReviewDetailADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string PostT_OutStockReviewDetailADF(string UserJson, string ErpVoucherNo);

        #endregion

        #region 盘点模块
        [WebInvoke(Method = "POST", UriTemplate = "GetCheck", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetCheck();

        [WebInvoke(Method = "POST", UriTemplate = "GetAreanobyCheckno", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetAreanobyCheckno(string checkno);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetScanInfo", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetScanInfo(string barcode);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "InsertCheckDetail", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string InsertCheckDetail(string json);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetAreanobyCheckno2", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetAreanobyCheckno2(string checkno, string areano);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetCheckDetail", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetCheckDetail(string checkno);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetMinSerialno", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetMinSerialno(int id);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "DeleteCheckDetail", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string DeleteCheckDetail(string checkno, string json);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "CheckSerialno", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string CheckSerialno(string EAN, string areaid, string batchno, string materialno);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "CheckGetBatchnoAndMaterialno", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string CheckGetBatchnoAndMaterialno(string EAN, string areaid);
        #endregion

        #region PDA查询
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetStockByMaterialNoADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetStockByMaterialNoADF(string UserJson, string MaterialNo, string ScanType);
        #endregion

        #region 库存调整
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetInfoBySerialymh", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetInfoBySerialymh(string barcode);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SaveInfo", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string SaveInfo(string json, string man);

        #endregion

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetInfoBySerial", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string  GetInfoBySerial(string EAN, string areaid, string batchno, string materialno);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetWareHouse", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetWareHouse();


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetPalletInfoByPalletNo", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetPalletInfoByPalletNo(string PalletNo);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "GetTransportSupplierListADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        //string GetTransportSupplierListADF();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SaveTransportSupplierListADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string SaveTransportSupplierListADF(string ModelJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetTransportSupplierDetailListADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetTransportSupplierDetailListADF(string PalletNo);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ScanOutStockReviewByBarCodeADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string ScanOutStockReviewByBarCodeADF(string BarCode);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetMessageForPrint", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetMessageForPrint(string filter, string flag);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetOutBarCodeForPrint", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetOutBarCodeForPrint(string BarCode);
        
        #region 地标模块
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetLandmark", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetLandmark(string ModelJson, string UserJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetTaskForLandmark", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetTaskForLandmark(string ModelJson, string UserJson);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SaveTaskwithandmark", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string SaveTaskwithandmark(string ModelJson, string UserJson);

        #endregion

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetErpVoucherNo", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetErpVoucherNo(string BarCode);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetOutBarCodeForYS", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetOutBarCodeForYS(string BarCode);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "YSPost", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string YSPost(string UserJson, string ModelJson);
        #region 预留释放
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetT_YSListADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetT_YSListADF(string UserJson, string ModelJson);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetTYSDetailListByHeaderIDADF", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetTYSDetailListByHeaderIDADF(string ModelDetailJson);
        #endregion

    }
}