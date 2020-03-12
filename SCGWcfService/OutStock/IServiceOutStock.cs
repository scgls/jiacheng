using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using BILWeb.OutStock;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_OUTSTOCK_Func代码
        [OperationContract]
        bool SaveT_OutStock(UserInfo user, ref T_OutStockInfo t_outstock, ref string strError);


        [OperationContract]
        bool DeleteT_OutStockByModel(UserInfo user, T_OutStockInfo model, ref string strError);

        [OperationContract]
        bool GetT_OutStockByID(ref T_OutStockInfo model, ref string strError);


        [OperationContract]
        bool GetT_OutStockListByPage(ref List<T_OutStockInfo> modelList, UserInfo user, T_OutStockInfo t_outstock, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_OutStockByHeaderID(ref List<T_OutStockInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_OutStockStatus(UserInfo user, ref T_OutStockInfo t_outstock,  ref string strError);



        #endregion

        [OperationContract]
        bool GetOutStockAndDetailsModelByNo(string erpNo, ref BILWeb.OutStockTask.T_OutStockTaskInfo head, ref List<BILWeb.OutStockTask.T_OutStockTaskDetailsInfo> lstDetail, ref string ErrMsg);
    }
}