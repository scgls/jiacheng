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
        #region 自动生成WCF接口方法T_QUALITYDETAIL_Func代码
        [OperationContract]
        bool SaveT_QualityDetail(UserInfo user, ref T_QualityDetailInfo t_qualitydetail, ref string strError);


        [OperationContract]
        bool DeleteT_QualityDetailByModel(UserInfo user, T_QualityDetailInfo model, ref string strError);

        [OperationContract]
        bool GetT_QualityDetailByID(ref T_QualityDetailInfo model, ref string strError);


        [OperationContract]
        bool GetT_QualityDetailListByPage(ref List<T_QualityDetailInfo> modelList, UserInfo user, T_QualityDetailInfo t_qualitydetail, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_QualityDetailByHeaderID(ref List<T_QualityDetailInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_QualityDetailStatus(UserInfo user, ref T_QualityDetailInfo t_qualitydetail, int NewStatus, ref string strError);

        [OperationContract]
        bool UpdateStockByQuality(string ErpVoucherNo, ref string strError);

        #endregion
    }
}