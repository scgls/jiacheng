using BILBasic.Common;
using BILWeb.Basing;
using BILWeb.Login.User;
using BILWeb.Menu;
using BILWeb.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_MENU_Func代码

        public bool SaveT_Menu(UserInfo user, ref T_MenuInfo t_menu, ref string strError)
        {
            T_MENU_Func tfunc = new T_MENU_Func();
            return tfunc.SaveModelToDB(user, ref t_menu, ref strError);
        }


        public bool DeleteT_MenuByModel(UserInfo user, T_MenuInfo model, ref string strError)
        {
            T_MENU_Func tfunc = new T_MENU_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_MenuByID(ref T_MenuInfo model, ref string strError)
        {
            T_MENU_Func tfunc = new T_MENU_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_MenuListByPage(ref List<T_MenuInfo> modelList, UserInfo user, T_MenuInfo t_menu, ref DividPage page, ref string strError)
        {
            T_MENU_Func tfunc = new T_MENU_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_menu, ref page, ref strError);
        }


        public bool GetAllT_MenuByHeaderID(ref List<T_MenuInfo> modelList, int headerID, ref string strError)
        {
            T_MENU_Func tfunc = new T_MENU_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_MenuStatus(UserInfo user, ref T_MenuInfo t_menu, int NewStatus, ref string strError)
        {
            T_MENU_Func tfunc = new T_MENU_Func();
            return tfunc.UpdateModelStatus(user, ref t_menu, NewStatus, ref strError);
        }


        #endregion


        #region 非自动生成的代码
        public bool GetMenuListByUserGroupID(ref List<T_MenuInfo> modelList, ref string strError, int UserGroupID, bool IncludNoCheck = true) 
        {
            T_MENU_Func tfunc = new T_MENU_Func();
            return tfunc.GetMenuListByUserGroupID(ref modelList, ref strError, UserGroupID, IncludNoCheck);
        }

        public bool GetTreeNo( ref Tree_Model model, ref string strError)
        {
            Tree_Func tfunc = new Tree_Func();
            return tfunc.GetTreeNo(ref model, ref strError);

        }

        public bool SaveT_UserGroupMenu(UserInfo user, T_MenuInfo t_Menu, int UserGroupID, ref string strError) 
        {
            T_MENU_Func tfunc = new T_MENU_Func();
            return tfunc.SaveUserGroupMenuToDB(user, t_Menu, UserGroupID, ref strError);
        }
        #endregion
    }
}