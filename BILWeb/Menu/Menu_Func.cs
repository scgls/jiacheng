using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILWeb.Login.User;


namespace BILWeb.Menu
{

    public partial class T_MENU_Func : TBase_Func<T_MENU_DB, T_MenuInfo>
    {

        protected override bool CheckModelBeforeSave(T_MenuInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }
            if (model.ParentName=="PDA菜单")
            {
                model.MenuType = 4;
            }
            
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "菜单";
        }

        protected override T_MenuInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }


        public bool GetModelListBySql(UserInfo user, ref List<T_MenuInfo> lstMenu, bool IncludNoCheck) 
        {
            T_MENU_DB tmd = new T_MENU_DB();
            lstMenu = tmd.GetModelListBySql(user, IncludNoCheck);

            if (lstMenu == null || lstMenu.Count <= 0) { return false; }
            else { return true; }
        }


        public bool GetMenuListByUserGroupID(ref List<T_MenuInfo> modelList, ref string strError, int UserGroupID, bool IncludNoCheck = true)
        {
            try
            {
                T_MENU_DB tdb = new T_MENU_DB();
                modelList = tdb.GetMenuListByUserGroupID(UserGroupID, IncludNoCheck);
                return true;
            }
            catch (Exception ex)
            {
                strError = "获取" + GetModelChineseName() + "列表失败！" + ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }

        public bool SaveUserGroupMenuToDB(UserInfo user, T_MenuInfo model, int UserGroupID, ref string strError)
        {
            try
            {
                if (UserGroupID <= 0)
                {
                    strError = "用户组信息不正确";
                    return false;
                }
                return db.SaveUserGroupMenuToDB(user, model, UserGroupID, ref strError);
            }
            catch (Exception ex)
            {
                strError = "保存" + GetModelChineseName() + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                return false;
            }
        }
    }
}