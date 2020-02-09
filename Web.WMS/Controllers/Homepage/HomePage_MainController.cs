using BILWeb.Login.User;
using BILWeb.Menu;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WMS.Web.Filter;

namespace Web.WMS.Controllers
{
    [RoleActionFilter(Message = "HomePage_MainController")]
    public class HomePage_MainController : Controller
    {
        //
        // GET: /HomePage_Main/
        

        public ActionResult Index()
        {
            return View("~/views/shared/nav.cshtml");
        }

        public ActionResult Index1()
        {
            return View("~/views/shared/_layout.cshtml");
        }

        public ActionResult Home_Page()
        {
             UserInfo user = Common.Commom.ReadUserInfo();
            T_MENU_Func tmfun = new T_MENU_Func();
            List<T_MenuInfo> lstMenu = new List<T_MenuInfo>();
            if (tmfun.GetModelListBySql(user, ref lstMenu, true))
            {
                lstMenu = lstMenu.Where(t => t.MenuType == 1 && t.MenuStatus == 1 && t.IsChecked == true).ToList();
                var lstMenuParent = lstMenu.Where(t => t.ParentID == 0).OrderBy(t => t.NodeSort).ToList();

                foreach (var item in lstMenuParent)
                {
                    item.lstSubMenu = lstMenu.FindAll(t => t.ParentID == item.ID);
                }
                ViewData["lstMenu"] = lstMenuParent;
            }

            return View();
        }

        public ActionResult Home_Page2()
        {
            int[] unmberAll = new int[5];
            unmberAll[0] = 0;
            unmberAll[1] = 0;
            unmberAll[2] = 0;
            unmberAll[3] = 0;
            try
            {
                User_DB userdb = new User_DB();
                unmberAll = userdb.sysshow();
            }
            catch (System.Exception)
            {
            }

            ViewData["number"] = unmberAll;
            return View();
        }
    }
}
