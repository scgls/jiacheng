using BILBasic.Common;
using BILBasic.User;
using BILWeb.InStock;
using BILWeb.Login.User;
using BILWeb.OutStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_OUTSTOCKDETAILS_Func代码
        [OperationContract]
        bool SaveT_OutStockDetail(UserInfo user, ref T_OutStockDetailInfo t_outstockdetails, ref string strError);


        [OperationContract]
        bool DeleteT_OutStockDetailByModel(UserInfo user, T_OutStockDetailInfo model, ref string strError);

        [OperationContract]
        bool GetT_OutStockDetailByID(ref T_OutStockDetailInfo model, ref string strError);


        [OperationContract]
        bool GetT_OutStockDetailListByPage(ref List<T_OutStockDetailInfo> modelList, UserInfo user, T_OutStockDetailInfo t_outstockdetails, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_OutStockDetailByHeaderID(ref List<T_OutStockDetailInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_OutStockDetailStatus(UserInfo user, ref T_OutStockDetailInfo t_outstockdetails, int NewStatus, ref string strError);

        [OperationContract]
        bool GetChangeMaterialForStock(ref T_OutStockDetailInfo model, ref string strError);

        [OperationContract]
        bool UpdateChangeMaterial(ref T_OutStockDetailInfo model, ref string strError);

        [OperationContract]
        bool SaveT_ChangeMaterial(UserInfo userModel, List<T_InStockDetailInfo> InStockDetailList, List<T_OutStockDetailInfo> OutStockDetailList, ref string strError);
        
        #endregion
    }
}