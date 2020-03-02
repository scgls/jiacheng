using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using BILWeb.InStock;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_INSTOCKDETAIL_Func代码
        [OperationContract]
        bool SaveT_InStockDetail(UserInfo user, ref T_InStockDetailInfo t_instockdetail, ref string strError);


        [OperationContract]
        bool DeleteT_InStockDetailByModel(UserInfo user, T_InStockDetailInfo model, ref string strError);

        [OperationContract]
        bool GetT_InStockDetailByID(ref T_InStockDetailInfo model, ref string strError);


        [OperationContract]
        bool GetT_InStockDetailListByPage(ref List<T_InStockDetailInfo> modelList, UserInfo user, T_InStockDetailInfo t_instockdetail, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_InStockDetailByHeaderID(ref List<T_InStockDetailInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_InStockDetailStatus(UserInfo user, ref T_InStockDetailInfo t_instockdetail, int NewStatus, ref string strError);



        #endregion

        [OperationContract]
        string GetT_InStockDetailListByHeaderIDADF(string ModelDetailJson);

        [OperationContract]
        string SaveT_InStockDetailADF(string UserJson,string ModelJson);
    }
}