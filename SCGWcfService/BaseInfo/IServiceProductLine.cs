using BILBasic.Common;
using BILWeb.BaseInfo;
using BILWeb.Login.User;
using System.Collections.Generic;
using System.ServiceModel;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_ProductLine_Func代码
        [OperationContract]
        bool SaveT_ProductLine(UserInfo user, ref T_ProductLine t_customer, ref string strError);


        [OperationContract]
        bool DeleteT_ProductLineByModel(UserInfo user, T_ProductLine model, ref string strError);

        [OperationContract]
        bool GetT_ProductLineByID(ref T_ProductLine model, ref string strError);


        [OperationContract]
        bool GetT_ProductLineListByPage(ref List<T_ProductLine> modelList, UserInfo user, T_ProductLine t_customer, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_ProductLineByHeaderID(ref List<T_ProductLine> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_ProductLineStatus(UserInfo user, ref T_ProductLine t_customer, int NewStatus, ref string strError);



        #endregion

    }
}