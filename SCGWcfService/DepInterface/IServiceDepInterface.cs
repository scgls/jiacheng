using BILBasic.Common;
using BILWeb.DepInterface;
using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_DEPINTERFACE_Func代码
        [OperationContract]
        bool SaveT_DepInterface(UserInfo user, ref T_DepInterfaceInfo t_depinterface, ref string strError);


        [OperationContract]
        bool DeleteT_DepInterfaceByModel(UserInfo user, T_DepInterfaceInfo model, ref string strError);

        [OperationContract]
        bool GetT_DepInterfaceByID(ref T_DepInterfaceInfo model, ref string strError);


        [OperationContract]
        bool GetT_DepInterfaceListByPage(ref List<T_DepInterfaceInfo> modelList, UserInfo user, T_DepInterfaceInfo t_depinterface, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_DepInterfaceByHeaderID(ref List<T_DepInterfaceInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_DepInterfaceStatus(UserInfo user, ref T_DepInterfaceInfo t_depinterface, int NewStatus, ref string strError);



        #endregion
    }
}