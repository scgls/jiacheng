using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.RetentionChange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_RETENTIONCHANGE_Func代码
        [OperationContract]
        bool SaveT_RetentionChange(UserInfo user, ref T_RetentionChangeInfo t_retentionchange, ref string strError);


        [OperationContract]
        bool DeleteT_RetentionChangeByModel(UserInfo user, T_RetentionChangeInfo model, ref string strError);

        [OperationContract]
        bool GetT_RetentionChangeByID(ref T_RetentionChangeInfo model, ref string strError);


        [OperationContract]
        bool GetT_RetentionChangeListByPage(ref List<T_RetentionChangeInfo> modelList, UserInfo user, T_RetentionChangeInfo t_retentionchange, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_RetentionChangeByHeaderID(ref List<T_RetentionChangeInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_RetentionChangeStatus(UserInfo user, ref T_RetentionChangeInfo t_retentionchange, int NewStatus, ref string strError);



        #endregion

        #region 自动生成WCF接口方法T_RETENTIONDETAILCHANGE_Func代码
        [OperationContract]
        bool SaveT_RetentionDetailChange(UserInfo user, ref T_RetentionDetailChangeInfo t_retentiondetailchange, ref string strError);


        [OperationContract]
        bool DeleteT_RetentionDetailChangeByModel(UserInfo user, T_RetentionDetailChangeInfo model, ref string strError);

        [OperationContract]
        bool GetT_RetentionDetailChangeByID(ref T_RetentionDetailChangeInfo model, ref string strError);


        [OperationContract]
        bool GetT_RetentionDetailChangeListByPage(ref List<T_RetentionDetailChangeInfo> modelList, UserInfo user, T_RetentionDetailChangeInfo t_retentiondetailchange, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_RetentionDetailChangeByHeaderID(ref List<T_RetentionDetailChangeInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_RetentionDetailChangeStatus(UserInfo user, ref T_RetentionDetailChangeInfo t_retentiondetailchange, int NewStatus, ref string strError);



        #endregion

        [OperationContract]
        bool PostRetentionChange(UserInfo user, List<T_RetentionDetailChangeInfo> modelList, ref string strErrMsg);
    }
}