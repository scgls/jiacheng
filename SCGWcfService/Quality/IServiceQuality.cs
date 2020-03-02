using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using BILWeb.Quality;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_QUALITY_Func代码
        [OperationContract]
        bool SaveT_Quality(UserInfo user, ref T_QualityInfo t_quality, ref string strError);


        [OperationContract]
        bool DeleteT_QualityByModel(UserInfo user, T_QualityInfo model, ref string strError);

        [OperationContract]
        bool GetT_QualityByID(ref T_QualityInfo model, ref string strError);


        [OperationContract]
        bool GetT_QualityListByPage(ref List<T_QualityInfo> modelList, UserInfo user, T_QualityInfo t_quality, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_QualityByHeaderID(ref List<T_QualityInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_QualityStatus(UserInfo user, ref T_QualityInfo t_quality, int NewStatus, ref string strError);

        [OperationContract]
        bool GetT_AllQualityList(ref List<T_QualityInfo> modelList, ref string strError);

        #endregion
    }
}