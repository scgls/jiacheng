using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using BILWeb.Area;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_AREA_Func代码
        [OperationContract]
        bool SaveT_Area(UserInfo user, ref T_AreaInfo t_area, ref string strError);


        [OperationContract]
        bool DeleteT_AreaByModel(UserInfo user, T_AreaInfo model, ref string strError);

        [OperationContract]
        bool GetT_AreaByID(ref T_AreaInfo model, ref string strError);


        [OperationContract]
        bool GetT_AreaListByPage(ref List<T_AreaInfo> modelList, UserInfo user, T_AreaInfo t_area, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_AreaByHeaderID(ref List<T_AreaInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_AreaStatus(UserInfo user, ref T_AreaInfo t_area, int NewStatus, ref string strError);

        //[OperationContract]
        //string GetAreaModelADF(string AreaNo);

        #endregion
    }
}