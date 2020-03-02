using BILBasic.Common;
using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using BILWeb.InStock;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_INSTOCK_Func代码
        [OperationContract]
        bool SaveT_InStock(UserInfo user, ref T_InStockInfo t_instock, ref string strError);


        [OperationContract]
        bool DeleteT_InStockByModel(UserInfo user, T_InStockInfo model, ref string strError);

        [OperationContract]
        bool GetT_InStockByID(ref T_InStockInfo model, ref string strError);


        [OperationContract]
        bool GetT_InStockListByPage(ref List<T_InStockInfo> modelList, UserInfo user, T_InStockInfo t_instock, ref DividPage page, ref string strError);

        [OperationContract]
        string GetT_InStockListADF(string UserJosn, string ModelJson);

      
        [OperationContract]
        bool GetAllT_InStockByHeaderID(ref List<T_InStockInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_InStockStatus(UserInfo user, ref T_InStockInfo t_instock,  ref string strError);

        #endregion
    }
}