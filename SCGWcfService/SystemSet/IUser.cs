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
        [OperationContract]
        bool SaveT_User(UserInfo user, ref UserInfo t_user, ref string strError);


        [OperationContract]
        bool DeleteT_UserByModel(UserInfo user, UserInfo model, ref string strError);     

        [OperationContract]
        bool GetT_UserByID(ref UserInfo model, ref string strError);


        [OperationContract]
        bool GetT_UserListByPage(ref List<UserInfo> modelList, UserInfo user, UserInfo t_user, ref  BILBasic.Common.DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_UserByHeaderID(ref List<UserInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_UserStatus(UserInfo user, ref UserInfo t_user, int NewStatus, ref string strError);


    }
}