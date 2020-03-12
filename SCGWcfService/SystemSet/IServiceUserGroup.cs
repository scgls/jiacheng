
using BILBasic.Common;
using BILWeb.UserGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using BILWeb.Login.User;
using BILWeb.Menu;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_USERGROUP_Func代码
        [OperationContract]
        bool SaveT_UserGroup(UserInfo user, ref T_UserGroupInfo t_usergroup, ref string strError);


        [OperationContract]
        bool DeleteT_UserGroupByModel(UserInfo user, T_UserGroupInfo model, ref string strError);

        [OperationContract]
        bool GetT_UserGroupByID(ref T_UserGroupInfo model, ref string strError);


        [OperationContract]
        bool GetT_UserGroupListByPage(ref List<T_UserGroupInfo> modelList, UserInfo user, T_UserGroupInfo t_usergroup, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_UserGroupByHeaderID(ref List<T_UserGroupInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_UserGroupStatus(UserInfo user, ref T_UserGroupInfo t_usergroup, int NewStatus, ref string strError);

      
        #endregion
        
    }
}