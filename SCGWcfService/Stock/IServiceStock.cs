using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using BILWeb.Stock;
using BILBasic.Common;

namespace SCGWcfService.Stock
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_STOCK_Func代码
        [OperationContract]
        bool SaveT_Stock(UserInfo user, ref T_StockInfo t_stock, ref string strError);


        [OperationContract]
        bool DeleteT_StockByModel(UserInfo user, T_StockInfo model, ref string strError);

        [OperationContract]
        bool GetT_StockByID(ref T_StockInfo model, ref string strError);


        [OperationContract]
        bool GetT_StockListByPage(ref List<T_StockInfo> modelList, UserInfo user, T_StockInfo t_stock, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_StockByHeaderID(ref List<T_StockInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_StockStatus(UserInfo user, ref T_StockInfo t_stock, int NewStatus, ref string strError);

        //[OperationContract]
        //string GetStockModelADF(string SerialNo);

        #endregion
    }
}