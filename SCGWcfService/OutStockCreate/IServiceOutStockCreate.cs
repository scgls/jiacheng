using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using BILWeb.OutStockCreate;
using BILWeb.Login.User;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_OUTSTOCKDETAILS_Func代码
        [OperationContract]
        bool SaveT_OutStockCreate(UserInfo user, ref T_OutStockCreateInfo t_outstockdetails, ref string strError);

        [OperationContract]
        bool SaveT_OutStockCreateList(UserInfo user,  List<T_OutStockCreateInfo> modelList, ref string strError);



        [OperationContract]
        bool DeleteT_OutStockCreateByModel(UserInfo user, T_OutStockCreateInfo model, ref string strError);

        [OperationContract]
        bool GetT_OutStockCreateByID(ref T_OutStockCreateInfo model, ref string strError);


        [OperationContract]
        bool GetT_OutStockCreateListByPage(ref List<T_OutStockCreateInfo> modelList, UserInfo user, T_OutStockCreateInfo t_outstockdetails, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_OutStockCreateByHeaderID(ref List<T_OutStockCreateInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_OutStockCreateStatus(UserInfo user, ref T_OutStockCreateInfo t_outstockdetails, int NewStatus, ref string strError);



        #endregion
    }
}