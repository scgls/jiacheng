using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.UserGroup;
using BILWeb.Warehouse;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.WMS.Common;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [RoleActionFilter(Message = "Basic/UserInfo")]
    public class UserInfoController : BaseController<UserInfo>
    {
        private IUserService userService;
        public UserInfoController()
        {
            userService = (IUserService)ServiceFactory.CreateObject("Login.User.User_Func");
            baseservice = userService;
        }



        User_Func tfunc = new User_Func();
        T_WareHouse_Func tfunc_warehouse = new T_WareHouse_Func();
        T_UserGroup_Func gfunc_warehouse = new T_UserGroup_Func();



        //public ActionResult DelListModelByModel(int id)
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Delete(UserInfo model)
        //{
        //    return RedirectToAction("GetModelList");
        //}

        [HttpGet]
        public ActionResult GetModel2(UserInfo model)
        {
            List<T_WareHouseInfo> wmodelList = new List<T_WareHouseInfo>();
            string strMsg = "";
            DividPage page = new DividPage { PagesCount = 1000 };
            T_WareHouseInfo Wmodel = new T_WareHouseInfo();
            tfunc_warehouse.GetModelListByPage(ref wmodelList, currentUser, Wmodel, ref page, ref strMsg);

            List<T_UserGroupInfo> gmodelList = new List<T_UserGroupInfo>();
            T_UserGroupInfo Gmodel = new T_UserGroupInfo();
            gfunc_warehouse.GetModelListByPage(ref gmodelList, currentUser, Gmodel, ref page, ref strMsg);

            ViewData["Warehouse"] = wmodelList;
            ViewData["serGroup"] = gmodelList;
            tfunc.GetModelByID(ref model, ref strMsg);
            return View("getmodel",model);
        }


        //public ActionResult GetModelList(DividPage page, UserInfo model)
        //{
        //    List<UserInfo> modelList = new List<UserInfo>();
        //    string strError = "";
        //    tfunc.GetModelListByPage(ref modelList, model, model, ref page, ref strError);
        //    ViewData["PageData"] = new PageData<UserInfo> { data= modelList,dividPage= page,link= Common.PageTag.ModelToUriParam(model, "/UserInfo/GetModelList")};
        //    return View("UserList", model);
        //}




        //public ActionResult Create(Achivement achieve, HttpPostedFileBase image)
        //{
        //    try
        //    {
        //        if (image != null && image.ContentLength > 0)
        //        {
        //            string fileName = DateTime.Now.ToString("yyyyMMdd") + "-" + Path.GetFileName(image.FileName);
        //            string filePath = Path.Combine(Server.MapPath("~/Images"), fileName);
        //            image.SaveAs(filePath);
        //            achieve.Pictures = "~/Images/" + fileName;
        //        }
        //        //m_achivementService.Create(achieve);
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        ///// <summary>
        ///// 取得符合要求的合约记录
        ///// </summary>
        ///// <param name="userName">用户名</param>
        ///// <param name="nickName">昵称</param>
        ///// <param name="role">角色</param>
        //private void getUserInfo(UserInfo user)
        //{



        //    string msg = "";
        //    DataTable dt = new DataTable();
        //    UserInfo user = new UserInfo();
        //    user.userName = userName.Trim();
        //    user.nickName = nickName.Trim();
        //    user.role = role.Trim();
        //    ViewData["User"] = user;
        //    dt = uServices.GetUser(userName.Trim(), nickName.Trim(), role.Trim(), out msg);
        //    Page page = new Page();
        //    page.totalCount = dt.Rows.Count;
        //    ViewData["page"] = page;
        //    DataTable dtUserInfo = new DataTable();
        //    dtUserInfo = uServices.GetUserOnePage(page.pageSize, page.pageNo, userName.Trim(), nickName.Trim(), role.Trim(), out msg);
        //    ViewData["errorMsg"] = msg;
        //    ViewData["UserTable"] = dtUserInfo;
        //}
        ///// <summary>
        ///// 翻页
        ///// </summary>
        ///// <param name="userName">用户名</param>
        ///// <param name="nickName">昵称</param>
        ///// <param name="role">角色</param>
        ///// <param name="totalCount">总条数</param>
        ///// <param name="pageSize">一页条数</param>
        ///// <param name="pageNo">当前页</param>
        ///// <returns></returns>
        //public ActionResult PageChange(string userName, string nickName, string role,
        //    string totalCount, string pageSize, string pageNo)
        //{
        //    string msg = "";
        //    Page page = new Page();
        //    page.totalCount = Convert.ToInt16(totalCount);
        //    page.pageNo = Convert.ToInt16(pageNo);
        //    page.pageSize = Convert.ToInt16(pageSize);
        //    ViewData["page"] = page;

        //    UserInfo user = new UserInfo();
        //    user.userName = userName.Trim();
        //    user.nickName = nickName.Trim();
        //    user.role = role.Trim();
        //    DataTable Data = new DataTable();
        //    Data = uServices.GetUserOnePage(page.pageSize, page.pageNo, userName.Trim(), nickName.Trim(), role.Trim(), out msg);
        //    ViewData["UserTable"] = Data;
        //    ViewData["User"] = user;
        //    return View("UserList");
        //}


        ///// <summary>
        ///// 添加一条记录返回所添加的那一行记录
        ///// </summary>
        ///// <param name="userName">用户名</param>
        ///// <param name="nickName">昵称</param>
        ///// <param name="role">角色</param>
        ///// <returns></returns>
        //public ActionResult UserAddGo(string userName, string nickName, string role)
        //{
        //    string msg = "";
        //    Page page = new Page();
        //    page.totalCount = 1;//总共的数据条数
        //    page.pageNo = 1;
        //    ViewData["page"] = page;

        //    //出版社的查询条件
        //    UserInfo user = new UserInfo();
        //    user.userName = userName.Trim();
        //    if (nickName == null || nickName == "")
        //    {
        //        user.nickName = nickName;
        //    }
        //    else
        //    {
        //        user.nickName = nickName.Trim();
        //    }
        //    if (role == null || role == "")
        //    {
        //        user.role = role;
        //    }
        //    else
        //        user.role = role.Trim();
        //    ViewData["User"] = user;
        //    DataTable dt = uServices.GetUser(userName.Trim(), user.nickName,user.role, out msg);
        //    ViewData["UserTable"] = dt;
        //    return View("UserList");
        //}

        ///// <summary>
        ///// 添加用户信息
        ///// </summary>
        ///// <param name="flg">标记是Insert还是Update</param>
        ///// <param name="userId">用户的ID号</param>
        ///// <param name="userName">用户名</param>
        ///// <param name="nickName">昵称</param>
        ///// <param name="password">密码</param>
        ///// <param name="role">角色</param>
        ///// <returns>当输入的信息正确时返回用户信息列表，当输入的信息有错误时返回错误信息并仍在添加界面</returns>
        //[HttpPost]
        //public ActionResult Add(string flg, string userId, string userName, string nickName, string password, string role)
        //{
        //    ViewData["flg"] = flg;
        //    UserInfo uInfo = new UserInfo();
        //    uInfo.userId = userId;
        //    uInfo.userName = userName.Trim();
        //    uInfo.nickName = nickName.Trim();
        //    uInfo.password = password.Trim();
        //    uInfo.role = role.Trim();
        //    string msg = "";
        //    uServices.InsertUpdateUserInfo(uInfo, flg, out msg);
        //    if (msg != "")
        //    {
        //        // 数据有误
        //        ViewData["errorMsg"] = msg;
        //        ViewData["uInfo"] = uInfo;
        //        return View("AddUser");
        //    }
        //    else
        //    {
        //        return RedirectToAction("UserAddGo", "UserInfo", new { userName = uInfo.userName });
        //    }
        //}

        ///// <summary>
        ///// 用户管理界面中的修改角色跳转到修改界面
        ///// </summary>
        ///// <param name="userName">用户名</param>
        ///// <returns></returns>
        //public ActionResult UpdateRole(string userName)
        //{
        //    ViewData["flg"] = "UpdateRole";
        //    ViewData["userName"] = userName;
        //    getUserInfoByName(userName);
        //    return View("AddUser");
        //}

        ///// <summary>
        ///// 用户管理界面中的修改密码跳转到修改界面
        ///// </summary>
        ///// <param name="userName">用户名</param>
        ///// <returns></returns>
        //public ActionResult UpdatePassword(string userName)
        //{
        //    ViewData["flg"] = "UpdatePassword";
        //    ViewData["userName"] = userName;
        //    getUserInfoByName(userName);
        //    return View("AddUser");
        //}

        ///// <summary>
        ///// 删除数据
        ///// </summary>
        ///// <param name="userId">用户Id号</param>
        ///// <param name="userName">用户名</param>
        ///// <param name="nickName">昵称</param>
        ///// <param name="role">角色</param>
        ///// <returns></returns>
        //public ActionResult Delete(string userId, string userName, string nickName, string role)
        //{
        //    string msg = "";
        //    uServices.DeleteUser(userId.Trim(), out msg);
        //    if (msg != "")
        //    {
        //        ViewData["errorMsg"] = msg;
        //        return View("UserList");
        //    }
        //    else
        //    {
        //        getUserInfo(userName.Trim(), nickName.Trim(), role.Trim());
        //        return View("UserList");
        //    }
        //}
        ///// <summary>
        ///// 取得用户名所对应的用户信息
        ///// </summary>
        ///// <param name="userName">用户名</param>
        //public void getUserInfoByName(string userName)
        //{
        //    string msg = "";
        //    DataTable dt = uServices.getUserInfoByName(userName.Trim(), out msg);
        //    UserInfo uInfo = new UserInfo();
        //    if (msg != "")
        //    {
        //        ViewData["errorMsg"] = msg;
        //    }
        //    if (dt.Rows.Count > 0)
        //    {
        //        uInfo.userName = dt.Rows[0]["userName"].ToString();
        //        uInfo.nickName = dt.Rows[0]["nickName"].ToString();
        //        uInfo.password = dt.Rows[0]["password"].ToString();
        //        uInfo.role = dt.Rows[0]["role"].ToString();
        //    }
        //    else
        //    {
        //        uInfo = new UserInfo();
        //    }
        //    ViewData["uInfo"] = uInfo;
        //}

        
        public ActionResult ModifyPassword()
        {
            return View();
        }


        /// <summary>
        /// 修改密码提交表单所执行的操作
        /// </summary>
        /// <param name="oldPassword">原始密码</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="confirmPassword">确认密码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Modify(string oldPassword, string newPassword, string confirmPassword)
        {
            UserInfo user = Commom.ReadUserInfo();
            string strError = string.Empty;
            user.PassWord = Common.Basic_Func.JiaMi(oldPassword);
            User_Func func = new User_Func();
            if (!func.UserLogin(ref user, ref strError))
            {
                ViewData["errorMsg"] = "原始密码不正确！";
                return View("ModifyPassword");
            }

            user.IsChangePwd = true;
            user.PassWord = Common.Basic_Func.JiaMi(newPassword); 
            user.RePassword = Common.Basic_Func.JiaMi(confirmPassword); 
            if (!func.ChangeUserPassword(user, ref strError))
            {
                // 数据有误
                ViewData["errorMsg"] =  "修改密码失败："+strError;
                return View("ModifyPassword");
            }
            else
            {
                // 数据有误
                ViewData["errorMsg"] = "修改成功退出重新登陆！";
                return View("ModifyPassword");
            }
        }

        ///// <summary>
        ///// 根据查询条件进行查询操作
        ///// </summary>
        ///// <param name="userName">用户名</param>
        ///// <param name="nickName">昵称</param>
        ///// <param name="role">角色</param>
        ///// <returns>如果有结果则会返回相应的结果信息列表，如果查询不到结果则会返回所有信息列表并提示用户查找不到符合条件的用户</returns>
        //[HttpPost]
        //public ActionResult Search(string userName, string nickName, string role)
        //{
        //    string msg = "";
        //    DataTable dt = uServices.GetUser(userName.Trim(), nickName.Trim(), role.Trim(), out msg);
        //    Page page = new Page();
        //    page.totalCount = dt.Rows.Count;
        //    ViewData["page"] = page;
        //    UserInfo user = new UserInfo();
        //    user.userName = userName.Trim();
        //    user.nickName = nickName.Trim();
        //    user.role = role.Trim();
        //    DataTable Data = new DataTable();
        //    Data = uServices.GetUserOnePage(page.pageSize, page.pageNo, userName.Trim(), nickName.Trim(), role.Trim(), out msg);
        //    if (msg != "")
        //    {
        //        ViewData["errorMsg"] = msg;
        //        ViewData["UserTable"] = Data;
        //        ViewData["User"] = user;
        //        return View("UserList");
        //    }
        //    ViewData["UserTable"] = Data;
        //    ViewData["User"] = user;
        //    return View("UserList");
        //}
    }

    //public class Achivement
    //{
    //    public string Pictures { get; set; }
    //}
}
