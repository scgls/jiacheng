using BILBasic.Common;
using BILWeb.Check;
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
        bool GetCheckInfo(Check_Model taskmo, ref DividPage dividpage, ref List<Check_Model> lsttask, ref string strErrMsg);

        [OperationContract]
        bool GetCheckArea(int hl, string areano, string houseno, string warehouseno, ref List<CheckArea_Model> list);

        [OperationContract]
        string GetTableID(string strSeq);

        [OperationContract]
        bool SaveCheck(Check_Model model, List<CheckArea_Model> list, ref string ErrMsg);

        [OperationContract]
        bool SaveCheck2(Check_Model model, List<T_StockInfoEX> list, ref string ErrMsg);

        [OperationContract]
        bool GetCheckRefInfo(CheckRef_Model model,ref DividPage dividpage, ref List<CheckRef_Model> lsttask, ref string strErrMsg);

        [OperationContract]
        bool DelCloCheck(string checkno, int type, ref string ErrMsg, string username = "");

        [OperationContract]
        bool GetCheckStock(string checkno, ref DividPage dividpage, ref List<CheckAnalyze> lsttask, ref string strErrMsg);

        [OperationContract]
        bool GetCheckAnalyze(CheckAnalyze taskmo, ref DividPage dividpage, ref List<CheckAnalyze> lsttask, ref string strErrMsg);

        [OperationContract]
        bool GetCheckAnalyze2(CheckAnalyze taskmo, ref DividPage dividpage, ref List<CheckAnalyze> lsttask, ref string strErrMsg);

        [OperationContract]
        string GetScanInfo(string barcode);

        [OperationContract]
        bool ReCheck(string checkno, string peo, ref string ErrMsg);

        [OperationContract]
        void dbTest();

        [OperationContract]
        bool GetCheckStock2(string checkno, ref List<T_StockInfoEX> list);
    }
}