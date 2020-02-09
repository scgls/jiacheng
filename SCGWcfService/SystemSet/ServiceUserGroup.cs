using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.UserGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_USERGROUP_Func代码

        public bool SaveT_UserGroup(UserInfo user, ref T_UserGroupInfo t_usergroup, ref string strError)
        {
            T_UserGroup_Func tfunc = new T_UserGroup_Func();
            return tfunc.SaveModelToDB(user, ref t_usergroup, ref strError);
        }


        public bool DeleteT_UserGroupByModel(UserInfo user, T_UserGroupInfo model, ref string strError)
        {
            T_UserGroup_Func tfunc = new T_UserGroup_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_UserGroupByID(ref T_UserGroupInfo model, ref string strError)
        {
            T_UserGroup_Func tfunc = new T_UserGroup_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_UserGroupListByPage(ref List<T_UserGroupInfo> modelList, UserInfo user, T_UserGroupInfo t_usergroup, ref DividPage page, ref string strError)
        {
            T_UserGroup_Func tfunc = new T_UserGroup_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_usergroup, ref page, ref strError);
        }


        public bool GetAllT_UserGroupByHeaderID(ref List<T_UserGroupInfo> modelList, int headerID, ref string strError)
        {
            T_UserGroup_Func tfunc = new T_UserGroup_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_UserGroupStatus(UserInfo user, ref T_UserGroupInfo t_usergroup, int NewStatus, ref string strError)
        {
            T_UserGroup_Func tfunc = new T_UserGroup_Func();
            return tfunc.UpdateModelStatus(user, ref t_usergroup, NewStatus, ref strError);
        }


        #endregion
    }
}