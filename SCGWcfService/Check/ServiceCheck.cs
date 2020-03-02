using BILBasic.Common;
using BILWeb.Query;
using BILWeb.Check;
using BILWeb.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {

        public bool GetCheckInfo(Check_Model taskmo, ref DividPage dividpage, ref List<Check_Model> lsttask, ref string strErrMsg)
        {
            Check_DB db = new Check_DB();
            return db.GetCheckInfo(taskmo,ref dividpage,ref lsttask,ref strErrMsg);
        }

        public bool GetCheckArea(int hl, string areano, string houseno, string warehouseno, ref List<CheckArea_Model> list)
        {
            Check_DB db = new Check_DB();
            return db.GetCheckArea(hl,areano, houseno, warehouseno, ref list);
        }

        public string GetTableID(string strSeq)
        {
            Check_Func db = new Check_Func();
            return db.GetTableID(strSeq);
        }

        public bool SaveCheck(Check_Model model, List<CheckArea_Model> list, ref string ErrMsg)
        { 
            Check_DB db = new Check_DB();
            return db.SaveCheck(model,list,ref ErrMsg);
        }

        public bool SaveCheck2(Check_Model model, List<T_StockInfoEX> list, ref string ErrMsg)
        {
            Check_DB db = new Check_DB();
            return db.SaveCheck2(model, list, ref ErrMsg);
        }

        public bool GetCheckRefInfo(CheckRef_Model model, ref DividPage dividpage, ref List<CheckRef_Model> lsttask, ref string strErrMsg)
        {
            Check_DB db = new Check_DB();
            return db.GetCheckRefInfo(model,ref dividpage, ref lsttask, ref strErrMsg);
        }

        public bool DelCloCheck(string checkno, int type, ref string ErrMsg, string username = "")
        {
            Check_DB db = new Check_DB();
            return db.DelCloCheck(checkno, type, ref ErrMsg, username);
        }

        public bool GetCheckStock(string checkno, ref DividPage dividpage, ref List<CheckAnalyze> lsttask, ref string strErrMsg)
        {
            Check_DB db = new Check_DB();
            return db.GetCheckStock(checkno, ref dividpage, ref lsttask, ref strErrMsg);
        }

        public bool GetCheckAnalyze(CheckAnalyze taskmo, ref DividPage dividpage, ref List<CheckAnalyze> lsttask, ref string strErrMsg)
        {
            Check_DB db = new Check_DB();
            return db.GetCheckAnalyze(taskmo, ref dividpage, ref lsttask, ref strErrMsg);
        }

        public bool GetCheckAnalyze2(CheckAnalyze taskmo, ref DividPage dividpage, ref List<CheckAnalyze> lsttask, ref string strErrMsg)
        {
            Check_DB db = new Check_DB();
            return db.GetCheckAnalyze2(taskmo, ref dividpage, ref lsttask, ref strErrMsg);
        }

        public string GetScanInfo(string barcode)
        {
            Check_DB db = new Check_DB();
            return db.GetScanInfo(barcode);
        }

        public bool ReCheck(string checkno, string peo, ref string ErrMsg)
        {
            Check_DB db = new Check_DB();
            return db.ReCheck(checkno, peo, ref ErrMsg);
        }

        public void dbTest()
        {
            Check_DB db = new Check_DB();
            db.dbTest();
        }

        public bool GetCheckStock2(string checkno, ref List<T_StockInfoEX> list)
        {
            Check_DB db = new Check_DB();
            return db.GetCheckStock2(checkno, ref list);
        }
    }
}