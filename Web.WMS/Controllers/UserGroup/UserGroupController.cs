using BILWeb.Menu;
using BILWeb.UserGroup;
using System.Collections.Generic;
using System.Web.Mvc;
using WMS.Factory;

using System.Linq;
using System.Web.Helpers;
using Newtonsoft.Json;
using BILBasic.Common;
using BILWeb.Tree;
using BILWeb.Basing;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.UserGroup
{
    [RoleActionFilter(Message = "Basic/UserGroup")]
    public class UserGroupController : BaseController<T_UserGroupInfo>
    {
        private IUserGroupService userGroupService;
        public UserGroupController()
        {
            userGroupService = (IUserGroupService)ServiceFactory.CreateObject("UserGroup.T_UserGroup_Func");
            baseservice = userGroupService;
        }
        T_MENU_Func funcmenu = new T_MENU_Func();

        [HttpPost]
        //点击按钮 改变权限
        public JsonResult saveMenu(int UserGroupID, int MenuId, bool isTrue)
        {
            string strError = "";
            T_MenuInfo model = new T_MenuInfo();
            model.ID = MenuId;
            model.IsChecked = isTrue;

            bool isSuccess = funcmenu.SaveUserGroupMenuToDB(currentUser, model, UserGroupID, ref strError);
            return Json(new { state = isSuccess, obj = strError }, JsonRequestBehavior.AllowGet);
        }
        //点击组权限，初始化菜单
        public JsonResult InitMenu(int UserGroupID)
        {
            string strError = "";
            T_MenuInfo model = new T_MenuInfo();
            List<T_MenuInfo> modelList = new List<T_MenuInfo>();
            bool isSuccess = funcmenu.GetMenuListByUserGroupID(ref modelList, ref strError, UserGroupID);
            List<Allnodes> allnodes = GetTree(modelList);
            return Json(new { state = isSuccess, obj = allnodes }, JsonRequestBehavior.AllowGet);
        }

        //根据ID 获取菜单明细
        [HttpPost]
        public JsonResult GetModelByID(int ID)
        {
            string strError = "";
            T_MenuInfo model = new T_MenuInfo();
            model.ID = ID;
            bool isSuccess = funcmenu.GetModelByID(ref model, ref strError);
            return Json(new { state = isSuccess, obj = model }, JsonRequestBehavior.AllowGet);
        }

        //修改菜单
        [HttpPost]
        public JsonResult SaveNode(string json)
        {
            T_MenuInfo model = JsonConvert.DeserializeObject<T_MenuInfo>(json);
            T_MenuInfo oldmodel = new T_MenuInfo();
            oldmodel.ID = model.ID;
            string strError = "";
            funcmenu.GetModelByID(ref oldmodel, ref strError);
            oldmodel.MenuNo = model.MenuNo;
            oldmodel.MenuName = model.MenuName;
            oldmodel.MemuAbbName= model.MemuAbbName;
            oldmodel.ProjectName = model.ProjectName;
            oldmodel.ParentName = model.ParentName;
            oldmodel.Description = model.Description;
            oldmodel.MenuType = model.MenuType;
            oldmodel.NodeUrl = model.NodeUrl;
            oldmodel.MenuStatus = model.MenuStatus;
            bool isSuccess = funcmenu.SaveModelToDB(currentUser, ref oldmodel, ref strError);
            return Json(new { state = isSuccess, obj = strError }, JsonRequestBehavior.AllowGet);
        }

        //新增菜单
        [HttpPost]
        public JsonResult InsertNode(string json)
        {
            string strError = "";
            T_MenuInfo model = JsonConvert.DeserializeObject<T_MenuInfo>(json);
            model = SetModelDefaultValue(model);
            bool isSuccess = funcmenu.SaveModelToDB(currentUser, ref model, ref strError);
            return Json(new { state = isSuccess, obj = strError }, JsonRequestBehavior.AllowGet);
        }


        public T_MenuInfo SetModelDefaultValue(T_MenuInfo model)
        {
            model.MenuType = 1;
            model.MenuStatus = 1;
            model.IsDel = 1;
            model.SafeLevel = 1;
            string strError = string.Empty;
            Tree_Model treeModel = model as Tree_Model;
            Tree_Func treefunc = new Tree_Func();
            if (treefunc.GetTreeNo(ref treeModel, ref strError))
            {
                treeModel.MenuNo = treeModel.TreeNo;
                model = treeModel as T_MenuInfo;
            }
            return model;
        }




        //T_UserGroup_Func func = new T_UserGroup_Func();

        //public ActionResult GetModelList(T_UserGroupInfo model)
        //{
        //    string strMsg = "";
        //    DividPage page = new DividPage { PagesCount = 1000 };
        //    List<T_UserGroupInfo> gmodelList = new List<T_UserGroupInfo>();
        //    func.GetModelListByPage(ref gmodelList, Commom.currentUser, model, ref page, ref strMsg);
        //    ViewData["userGroupList"] = gmodelList;
        //    return View("UserGroup");
        //}

        #region 构造树形结构
        public List<Allnodes> GetTree(List<T_MenuInfo> menus)
        {
            List<T_MenuInfo> lstMenuParent = menus.Where(t => t.ParentID == 0).OrderBy(t => t.NodeSort).ToList();
            foreach (var item in lstMenuParent)
            {
                item.lstSubMenu = menus.FindAll(t => t.ParentID == item.ID);
                foreach (var item2 in item.lstSubMenu)
                {
                    item2.lstSubMenu = menus.FindAll(t => t.ParentID == item2.ID);

                }
            }
            return NewMethod(lstMenuParent);
        }

        private static List<Allnodes> NewMethod(List<T_MenuInfo> lstMenuParent)
        {

            List<Allnodes> retallnodes = new List<Allnodes>();
            for (int i = 0; i < lstMenuParent.Count; i++)
            {
                Allnodes allnodes = new Allnodes();
                allnodes.text = lstMenuParent[i].MenuName;

                allnodes.tags = new NodeItem { id = lstMenuParent[i].ID.ToString(), level = lstMenuParent[i].NodeLevel,parentId=lstMenuParent[i].ParentID };
                //checked 选中
                if (lstMenuParent[i].IsChecked == true)
                {
                    allnodes.state = new CheckClass { @checked = true };

                }
                if (lstMenuParent[i].lstSubMenu != null)
                {
                    if (lstMenuParent[i].lstSubMenu.Count > 0) {
                    allnodes.nodes = new List<Allnodes>();
                    List<Allnodes> Allnodesub = NewMethod(lstMenuParent[i].lstSubMenu);
                    allnodes.nodes.AddRange(Allnodesub);
                    }
                }

                retallnodes.Add(allnodes);
            };
            return retallnodes;
        }
        #endregion
    }

    #region 数据模型
    public class Allnodes
    {
        public string text { get; set; }
        public NodeItem tags { get; set; }
        public CheckClass state { get; set; }
        public List<Allnodes> nodes { get; set; }
    }

    public class CheckClass
    {
        public bool @checked { get; set; }
    }
    public class NodeItem {
        public string id { get; set; }
        public int level { get; set; }
        public int parentId { get; set; }
    }

    #endregion
}