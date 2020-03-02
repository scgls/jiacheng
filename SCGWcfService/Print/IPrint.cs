using BILBasic.Common;
using BILWeb.Check;
using BILWeb.InStock;
using BILWeb.Material;
using BILWeb.Print;
using BILWeb.Query;
using BILWeb.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace SCGWcfService
{
    public partial interface IService
    {
        [OperationContract]
        void TestPrint();

        [OperationContract]
        List<string> getSqNum(int num, ref string data);

        [OperationContract]
        bool GetPrintVoucher(int vouchertype, string voucherno, ref List<T_InStockDetailInfo> infos, ref string ErrMsg);

        [OperationContract]
        bool GetPrintVoucher2(int vouchertype, string voucherno, ref List<T_InStockDetailInfo> infos, ref string ErrMsg, ref CusSup cs);

        [OperationContract]
        bool SaveSend(string voucherno, int num);

        [OperationContract]
        string GetBatch(string company, string mateno, string supcode);

        [OperationContract]
        string GetBatch2(DateTime dt, string company, string matenoid, string supbatch);

        [OperationContract]
        Dictionary<string, decimal> GetPack(string mateno);

        [OperationContract]
        bool SubBarcodes(List<Barcode_Model> list,string ipport,decimal hasprint, ref string ErrMsg);

        [OperationContract]
        List<string> getSqNumAndbarcodeType( int num, string mateid, ref string bt, ref string date);

        [OperationContract]
        string GetParameterById(string group, string id);

        [OperationContract]
        string GetSupMan();

        [OperationContract]
        void SetParameterById(string group, int id, string name);

        [OperationContract]
        string GetParameterByName(string group, string name);

        [OperationContract]
        bool GetBarcode(Barcode_Model model, ref List<Barcode_Model> list, ref DividPage page, ref string ErrMsg);

        [OperationContract]
        bool PrintZPL(List<Barcode_Model> list, string ipport, ref string ErrMsg);

        [OperationContract]
        bool GetMaterial(int id, string mateno,string judian, ref List<T_MaterialInfo> list, ref string ErrMsg);

        [OperationContract]
        bool FirstPrint(ref List<Barcode_Model> list, string ipport, ref string ErrMsg);

        [OperationContract]
        string PrintAndroid(string json);

        [OperationContract]
        bool PrintLpkApart(string serialno, string ip, ref string ErrMsg);

        [OperationContract]
        bool PrintLpkPallet(string serialno, string ip, ref string ErrMsg);

        [OperationContract]
        bool PrintDeliveryTray(T_StockInfoEX model, List<T_StockInfoEX> list, string ipport, ref string ErrMsg);

        [OperationContract]
        string PrintQYAndroid(string json);

        [OperationContract]
        string QYReprintAndroid(string json);

        [OperationContract]
        bool PrintZplPallet2(List<string> serialnos, ref string ErrMsg, ref List<Barcode_Model> list2);

        [OperationContract]
        bool GetMesWoInfoBarcode(string barcode, string workno, string materialno, ref string ErrMsg);

        [OperationContract]
        bool tuisong(ref string err);

        [OperationContract]
        bool Mes_CheckWoInfoIsOK(string zyid, ref string ErrMsg);
    }
}