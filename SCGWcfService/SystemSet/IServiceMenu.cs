using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.Menu;
using BILWeb.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_MENU_Func代码
        [OperationContract]
        bool SaveT_Menu(UserInfo user, ref T_MenuInfo t_menu, ref string strError);


        [OperationContract]
        bool DeleteT_MenuByModel(UserInfo user, T_MenuInfo model, ref string strError);

        [OperationContract]
        bool GetT_MenuByID(ref T_MenuInfo model, ref string strError);


        [OperationContract]
        bool GetT_MenuListByPage(ref List<T_MenuInfo> modelList, UserInfo user, T_MenuInfo t_menu, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_MenuByHeaderID(ref List<T_MenuInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_MenuStatus(UserInfo user, ref T_MenuInfo t_menu, int NewStatus, ref string strError);



        #endregion

        #region 非自动生成的代码

        [OperationContract]
        bool GetMenuListByUserGroupID(ref List<T_MenuInfo> modelList, ref string strError, int UserGroupID, bool IncludNoCheck = true);

        [OperationContract]
        bool GetTreeNo( ref Tree_Model model, ref string strError);

        [OperationContract]
        bool SaveT_UserGroupMenu(UserInfo user, T_MenuInfo t_Menu, int UserGroupID, ref string strError);
        
        #endregion
    }
}