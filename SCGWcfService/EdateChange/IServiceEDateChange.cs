using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using BILWeb.EdateChange;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_EDATECHANGE_Func代码
        [OperationContract]
        bool SaveT_EDateChange(UserInfo user, ref T_EDateChangeInfo t_edatechange, ref string strError);


        [OperationContract]
        bool DeleteT_EDateChangeByModel(UserInfo user, T_EDateChangeInfo model, ref string strError);

        [OperationContract]
        bool GetT_EDateChangeByID(ref T_EDateChangeInfo model, ref string strError);


        [OperationContract]
        bool GetT_EDateChangeListByPage(ref List<T_EDateChangeInfo> modelList, UserInfo user, T_EDateChangeInfo t_edatechange, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_EDateChangeByHeaderID(ref List<T_EDateChangeInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_EDateChangeStatus(UserInfo user, ref T_EDateChangeInfo t_edatechange, int NewStatus, ref string strError);



        #endregion

        #region 自动生成WCF接口方法T_EDATECHANGEDETAIL_Func代码
        [OperationContract]
        bool SaveT_EDateChangeDetail(UserInfo user, ref T_EDateChangeDetailInfo t_edatechangedetail, ref string strError);


        [OperationContract]
        bool DeleteT_EDateChangeDetailByModel(UserInfo user, T_EDateChangeDetailInfo model, ref string strError);

        [OperationContract]
        bool GetT_EDateChangeDetailByID(ref T_EDateChangeDetailInfo model, ref string strError);


        [OperationContract]
        bool GetT_EDateChangeDetailListByPage(ref List<T_EDateChangeDetailInfo> modelList, UserInfo user, T_EDateChangeDetailInfo t_edatechangedetail, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_EDateChangeDetailByHeaderID(ref List<T_EDateChangeDetailInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_EDateChangeDetailStatus(UserInfo user, ref T_EDateChangeDetailInfo t_edatechangedetail, int NewStatus, ref string strError);



        #endregion

        [OperationContract]
        bool PostEDateChange(UserInfo user, List<T_EDateChangeDetailInfo> modelList, ref string strErrMsg);
    }
}