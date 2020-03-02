using BILBasic.Common;
using BILWeb.Query;
using BILWeb.Check;
using BILWeb.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.Print;
using BILWeb.InStock;
using BILWeb.Material;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        public void TestPrint()
        {
            //Print_Code db = new Print_Code();
         
        }

        public List<string> getSqNum( int num, ref string date)
        {
            Print_DB db = new Print_DB();
            return db.getSqNum( num,ref date);
        }

        public bool GetPrintVoucher(int vouchertype, string voucherno, ref List<T_InStockDetailInfo> infos, ref string ErrMsg)
        {
            Print_DB db = new Print_DB();
            return db.GetPrintVoucher(vouchertype, voucherno, ref infos,ref ErrMsg);
        }

        public bool GetPrintVoucher2(int vouchertype, string voucherno, ref List<T_InStockDetailInfo> infos, ref string ErrMsg, ref CusSup cs)
        {
            Print_DB db = new Print_DB();
            return db.GetPrintVoucher2(vouchertype, voucherno, ref infos, ref ErrMsg,ref cs);
        }

        public bool SaveSend(string voucherno, int num)
        {
            Print_DB db = new Print_DB();
            return db.SaveSend(voucherno, num);
        }

        public string GetBatch(string company, string mateno,string supcode)
        {
            Print_DB db = new Print_DB();
            return db.GetBatch(company, mateno,supcode);
        }

        public string GetBatch2(DateTime dt, string company, string matenoid, string supbatch)
        {
            Print_DB db = new Print_DB();
            return db.GetBatch2(dt, company, matenoid, supbatch);
        }

        public Dictionary<string, decimal> GetPack(string mateno)
        {
            Print_DB db = new Print_DB();
            return db.GetPack(mateno);
        }

        public bool SubBarcodes(List<Barcode_Model> list, string ipport,decimal hasprint,ref string ErrMsg)
        {
            Print_DB db = new Print_DB();
            return db.SubBarcodes(list,ipport,hasprint,ref ErrMsg,0);
        }

        public List<string> getSqNumAndbarcodeType( int num, string mateid, ref string bt, ref string date)
        {
            Print_DB db = new Print_DB();
            return db.getSqNumAndbarcodeType( num,mateid, ref bt,ref date);
        }

        public string GetParameterById(string group, string id)
        {
            Print_DB db = new Print_DB();
            return db.GetParameterById(group,id);
        }



        public string GetSupMan()
        {
            Print_DB db = new Print_DB();
            return db.GetSupMan();
        }

        public void SetParameterById(string group, int id, string name)
        {
            Print_DB db = new Print_DB();
            db.SetParameterById(group, id, name);
        }

        public string GetParameterByName(string group, string name)
        {
            Print_DB db = new Print_DB();
            return db.GetParameterByName(group, name);
        }

        public bool GetBarcode(Barcode_Model model, ref List<Barcode_Model> list, ref DividPage page, ref string ErrMsg)
        {
            Print_DB db = new Print_DB();
            return db.GetBarcode(model, ref list,ref page,ref ErrMsg);
        }

        public bool PrintZPL(List<Barcode_Model> list, string ipport, ref string ErrMsg)
        {
            Print_DB db = new Print_DB();
            return db.PrintZPL(list, ipport, ref ErrMsg);
        }

        public bool GetMaterial(int id, string mateno,string judian, ref List<T_MaterialInfo> list, ref string ErrMsg)
        {
            Print_DB db = new Print_DB();
            return db.GetMaterial(id, mateno,judian, ref list, ref ErrMsg);
        }

        public bool FirstPrint(ref List<Barcode_Model> list, string ipport, ref string ErrMsg)
        {
            Print_DB db = new Print_DB();
            return db.FirstPrint(ref list, ipport, ref ErrMsg);
        }

        public string PrintAndroid(string json)
        {
            Print_DB db = new Print_DB();
            return db.PrintAndroid(json);
        }

        public bool PrintLpkApart(string serialno, string ip, ref string ErrMsg)
        {
            Print_DB db = new Print_DB();
            return db.PrintLpkApart(serialno,ip,ref ErrMsg);
        }

        public bool PrintLpkPallet(string serialno, string ip, ref string ErrMsg)
        {
            Print_DB db = new Print_DB();
            return db.PrintLpkPallet(serialno, ip, ref ErrMsg);
        }

        public bool PrintDeliveryTray(T_StockInfoEX model, List<T_StockInfoEX> list, string ipport, ref string ErrMsg)
        {
            Print_DB db = new Print_DB();
            return db.PrintDeliveryTray(model, list, ipport, ref ErrMsg);
        }
        public string PrintQYAndroid(string json)
        {
            Print_DB db = new Print_DB();
            return db.PrintQYAndroid(json);
        }

        public string QYReprintAndroid(string json)
        {
            Print_DB db = new Print_DB();
            return db.QYReprintAndroid(json);
        }

        public bool PrintZplPallet2(List<string> serialnos, ref string ErrMsg, ref List<Barcode_Model> list2)
        {
            Print_DB db = new Print_DB();
            return db.PrintZplPallet2(serialnos, ref ErrMsg, ref list2);
        }

        public bool GetMesWoInfoBarcode(string barcode, string workno, string materialno, ref string ErrMsg)
        {
            Print_DB db = new Print_DB();
            return db.GetMesWoInfoBarcode(barcode, workno, materialno, ref ErrMsg);
        }

        public bool tuisong(ref string err) 
        {
            Print_DB db = new Print_DB();
            return db.tuisong(ref err);
        }
        public bool Mes_CheckWoInfoIsOK(string zyid, ref string ErrMsg)
        {
            Print_DB db = new Print_DB();
            return db.Mes_CheckWoInfoIsOK(zyid, ref ErrMsg);
        }
    }
}