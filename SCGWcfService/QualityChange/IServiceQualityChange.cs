using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.QualityChange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_QUALITYCHANGE_Func代码
        [OperationContract]
        bool SaveT_QualityChange(UserInfo user, ref T_QualityChangeInfo t_qualitychange, ref string strError);


        [OperationContract]
        bool DeleteT_QualityChangeByModel(UserInfo user, T_QualityChangeInfo model, ref string strError);

        [OperationContract]
        bool GetT_QualityChangeByID(ref T_QualityChangeInfo model, ref string strError);


        [OperationContract]
        bool GetT_QualityChangeListByPage(ref List<T_QualityChangeInfo> modelList, UserInfo user, T_QualityChangeInfo t_qualitychange, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_QualityChangeByHeaderID(ref List<T_QualityChangeInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_QualityChangeStatus(UserInfo user, ref T_QualityChangeInfo t_qualitychange, int NewStatus, ref string strError);



        #endregion

        #region 自动生成WCF接口方法T_QUALITYCHANGEDETAIL_Func代码
        [OperationContract]
        bool SaveT_QualityChangeDetail(UserInfo user, ref T_QualityChangeDetailInfo t_qualitychangedetail, ref string strError);


        [OperationContract]
        bool DeleteT_QualityChangeDetailByModel(UserInfo user, T_QualityChangeDetailInfo model, ref string strError);

        [OperationContract]
        bool GetT_QualityChangeDetailByID(ref T_QualityChangeDetailInfo model, ref string strError);


        [OperationContract]
        bool GetT_QualityChangeDetailListByPage(ref List<T_QualityChangeDetailInfo> modelList, UserInfo user, T_QualityChangeDetailInfo t_qualitychangedetail, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_QualityChangeDetailByHeaderID(ref List<T_QualityChangeDetailInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_QualityChangeDetailStatus(UserInfo user, ref T_QualityChangeDetailInfo t_qualitychangedetail, int NewStatus, ref string strError);



        #endregion

        [OperationContract]
        bool PostQualityChange(UserInfo user, List<T_QualityChangeDetailInfo> modelList, ref string strErrMsg);

    }
}