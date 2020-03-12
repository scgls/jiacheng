using BILWeb.Login.User;
using BILWeb.Menu;
using BILWeb.OutStock;
using BILWeb.Pallet;
using BILWeb.Query;
using BILWeb.TransportSupplier;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Web.WMS.Common;

namespace Web.WMS.Controllers
{
    public class LoginController : Controller
    {
        //CommonDao com = new CommonDao();
        //LoginServices loginService = new LoginServices();
        Common.Commom Custom = new Common.Commom();
        //LoginDao loginDao = new LoginDao();
        /// <summary>
        /// 用户登录界面
        /// 
        /// </summary>
        /// <returns></returns>

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            //清空cookie
            HttpCookie hc = new HttpCookie("userinfo");
            hc.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(hc);
            return View();
        }


        //搜索用户名给出的提示框
        //public ActionResult userSearch(string username)
        //{
        //   DataTable dt = loginDao.getUsername("");
        //   string[] userNames = null;
        //   if (dt == null || dt.Rows.Count == 0)
        //   {
        //       userNames = null;
        //   }
        //   else
        //   {
        //       userNames = new string[dt.Rows.Count];
        //       for (int i = 0; i < dt.Rows.Count; i++)
        //       {
        //           userNames[i] = dt.Rows[i][0].ToString();
        //       }
        //   }

        //   return Json(userNames, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult ErrorMsg(string strMsg)
        {
            ViewData["strMsg"] = strMsg;
            return View("_ViewErrorMsg");
        }

        //提交用户名和密码进行登录判断
        [HttpPost]
        public ActionResult Login(string userName, string password, string UserSelect, string remember_me)
        {
            UserInfo user = new UserInfo();
            user.UserNo = userName;
            user.PassWord = Common.Basic_Func.JiaMi(password);
            string strError = "";
            User_Func func = new User_Func();
            if (func.UserLogin(ref user, ref strError))
            {
                HttpCookie hc = new HttpCookie("userinfo");
                hc["UserNo"] = user.UserNo;
                hc.Expires = DateTime.Now.AddDays(2);//设置过期时间
                Response.Cookies.Add(hc);//保存到客户端          

                //// 登录成功
                //this.TempData["lstMenu"] = user.lstMenu;
                return RedirectToAction("Home_Page", "HomePage_Main");
            }
            else
            {
                // 登录失败
                //Custom.MessageWrite(Response, "用户名和密码不匹配，登录失败！");
                ViewData["Msg"] = "用户名和密码不匹配，登录失败！";
                return View();
            }

            ////记住密码
            //loginService.RemberMe(Response, remember_me,UserSelect);

            ////从用户名Txt文档中取出历史记录显示在页面上
            //string[] Users = loginService.UsersOut(Response);
            //ViewData["Users"] = Users;

            //string MdPassword = com.md5(password);
            ////后台判断用户名和密码是否为空
            //if (String.IsNullOrEmpty(userName))
            //{
            //    Custom.MessageWrite(Response, "用户名不能为空!");
            //    return View();
            //}
            //if (String.IsNullOrEmpty(password))
            //{
            //    Custom.MessageWrite(Response, "密码不能为空!");
            //    return View();
            //}
            ////密码和用户名都输入了，数据库判断是否符合要求
            //if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
            //{
            //    if (loginService.checkUser(userName, MdPassword) == 1)
            //    {
            //        // 登录成功
            //        return RedirectToAction("Home_Page", "HomePage_Main");
            //    }
            //    else
            //    {
            //        if (loginService.checkUser(userName, MdPassword) == 2)
            //        {
            //            //用户名已经被注销,登录失败
            //            Custom.MessageWrite(Response, "该用户名已经被注销，请重新登录！");
            //        }
            //        else
            //        {
            //            // 登录失败
            //            Custom.MessageWrite(Response, "用户名和密码不匹配，登录失败！");
            //            return View();

            //        }
            //    }
            //}
            //return View();
        }

        #region 批量导入基站
        public ActionResult StationImport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult StationImport(HttpPostedFileBase filebase)
        {
            HttpPostedFileBase file = Request.Files["files"];
            string FileName;
            string savePath;
            if (file == null || file.ContentLength <= 0)
            {
                ViewBag.error = "文件不能为空";
                return View();
            }
            else
            {
                string filename = Path.GetFileName(file.FileName);
                int filesize = file.ContentLength;//获取上传文件的大小单位为字节byte
                string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名
                string NoFileName = System.IO.Path.GetFileNameWithoutExtension(filename);//获取无扩展名的文件名
                int Maxsize = 4000 * 1024;//定义上传文件的最大空间大小为4M
                string FileType = ".xls,.xlsx";//定义上传文件的类型字符串

                FileName = NoFileName + DateTime.Now.ToString("yyyyMMddhhmmss") + fileEx;
                if (!FileType.Contains(fileEx))
                {
                    ViewBag.error = "文件类型不对，只能导入xls和xlsx格式的文件";
                    return View();
                }
                if (filesize >= Maxsize)
                {
                    ViewBag.error = "上传文件超过4M，不能上传";
                    return View();
                }
                string path = AppDomain.CurrentDomain.BaseDirectory + "uploads/excel/";
                savePath = Path.Combine(path, FileName);
                file.SaveAs(savePath);
            }

            OperationExcel ec = new OperationExcel();
            DataTable dt = ec.ExcelToTableForXLSX(savePath);

            //string result = string.Empty;
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + savePath + ";" + "Extended Properties=Excel 8.0";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            OleDbDataAdapter myCommand = new OleDbDataAdapter("select * from [Sheet1$]", strConn);
            DataSet myDataSet = new DataSet();
            try
            {
                myCommand.Fill(myDataSet, "ExcelInfo");
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
            DataTable table = myDataSet.Tables["ExcelInfo"].DefaultView.ToTable();

            //引用事务机制，出错时，事物回滚
            //using (TransactionScope transaction = new TransactionScope())
            //{
            //    for (int i = 0; i < table.Rows.Count; i++)
            //    {
            //        //获取地区名称
            //        string _areaName = table.Rows[i][0].ToString();
            //        //判断地区是否存在
            //        if (!_areaRepository.CheckAreaExist(_areaName))
            //        {
            //            ViewBag.error = "导入的文件中：" + _areaName + "地区不存在，请先添加该地区";
            //            return View();
            //        }
            //        else
            //        {
            //            Station station = new Station();
            //            station.AreaID = _areaRepository.GetIdByAreaName(_areaName).AreaID;
            //            station.StationName = table.Rows[i][1].ToString();
            //            station.TerminaAddress = table.Rows[i][2].ToString();
            //            station.CapacityGrade = table.Rows[i][3].ToString();
            //            station.OilEngineCapacity = decimal.Parse(table.Rows[i][4].ToString());
            //            _stationRepository.AddStation(station);
            //        }
            //    }
            //    transaction.Complete();
            //}
            ViewBag.error = "导入成功";
            System.Threading.Thread.Sleep(2000);
            return RedirectToAction("Index");
        }
        #endregion

    }
}
