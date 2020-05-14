using BILBasic.Common;
using BILWeb.BaseInfo;
using BILWeb.Login.User;
using BILWeb.Menu;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Web.WMS.Common
{
    public class Commom : System.Web.UI.Page
    {
        public static List<T_MenuInfo> lstMenu = new List<T_MenuInfo>();
        //public static UserInfo currentUser = new UserInfo();

        //public static List<But> SysButs = new List<But>();

        public void MessageWrite(HttpResponseBase Response, string message)
        {
            Response.Write("<script type='text/javascript'>alert('" + message + "');</script>");
        }

        public static T_System getSystem()
        {
            T_System_Func funcsystem = new T_System_Func();
            T_System model = new T_System();
            List<T_System> tsystem = funcsystem.GetModel();
            if (tsystem != null && tsystem.Count > 0)
            {
                model = tsystem[0];
            }
            return model;
        }

        public static List<SelectListItem> TaskTypeList = new List<SelectListItem>
        {
                new SelectListItem(){Value="0",Text="全部"},
                new SelectListItem(){Value="1",Text="上架"},
                new SelectListItem(){Value="2",Text="下架"},
                new SelectListItem(){Value="3",Text="移库"},
                new SelectListItem(){Value="4",Text="收货"},
                new SelectListItem(){Value="200",Text="拆零"},
                new SelectListItem(){Value="201",Text="调整上"},
                new SelectListItem(){Value="202",Text="调整下"},
                //new SelectListItem(){Value="5",Text="完工入库"},
                //new SelectListItem(){Value="6",Text="退料入库"},
                //new SelectListItem(){Value="7",Text="交接入库"},
                //new SelectListItem(){Value="8",Text="齐套"},
                //new SelectListItem(){Value="9",Text="领料出库"},
                //new SelectListItem(){Value="10",Text="制成检"},
                //new SelectListItem(){Value="12",Text="复核"},
                //new SelectListItem(){Value="13",Text="包材接收"},
                //new SelectListItem(){Value="18",Text="法规检"},
        };

        public static List<SelectListItem> WarehouseStateList = new List<SelectListItem>
        {
            new SelectListItem { Text = "正常", Value = "1" },
            new SelectListItem { Text = "停用", Value = "2" }
        };


        public static List<SelectListItem> ISVWAREHOUSE = new List<SelectListItem>
        {
            new SelectListItem { Text = "否", Value = "0" },
            new SelectListItem { Text = "是", Value = "1" }
        };
        

        public static List<SelectListItem> CheckAnalyzeStateList = new List<SelectListItem>
        {
            new SelectListItem { Text = "全部", Value = "全部" },
            new SelectListItem { Text = "赢", Value = "赢" },
            new SelectListItem { Text = "亏", Value = "亏" },
            new SelectListItem { Text = "平", Value = "平" }
        };

        public static List<SelectListItem> SockTypeSelectList = new List<SelectListItem>
        {
            new SelectListItem { Text = "全部", Value = "0" },
            new SelectListItem { Text = "待检", Value = "1" },
            new SelectListItem { Text = "送检", Value = "2" },
            new SelectListItem { Text = "合格", Value = "3" },
            new SelectListItem { Text = "不合格", Value = "4" }
        };

        //MS01    托运，到付，费用客户/供应商承担
        //MS02    送货上门，到付，费用客户/供应商承担
        //MS03    普通快递，到付，费用客户/供应商承担
        //MS04    客户到仓库自提
        //MS05    义乌周边送货上门
        //MS06    顺丰快递，到付，费用客户/供应商承担
        //MS07    配好货，送门店，客户去门店自提
        //MS08    德邦快递，到付，费用客户/供应商承担
        //MS11    托运，现付，费用公司承担
        //MS12    送货上门，现付，费用公司承担
        //MS13    普通快递，现付，费用公司承担
        //MS14    顺丰快递，现付，费用公司承担
        //MS15    德邦快递，现付，费用公司承担
        //MS21    托运，现付，费用客户/供应商承担
        //MS22    送货上门，现付，费用客户/供应商承担
        //MS23    普通快递，现付，费用客户/供应商承担
        //MS24    顺丰快递，现付，费用客户/供应商承担
        //MS25    德邦快递，现付，费用客户/供应商承担
        public static List<SelectListItem> GetTradingConditionsNameList = new List<SelectListItem>
        {
            new SelectListItem { Text = "托运，到付，费用客户/供应商承担", Value = "托运，到付，费用客户/供应商承担" },
            new SelectListItem { Text = "送货上门，到付，费用客户/供应商承担", Value = "送货上门，到付，费用客户/供应商承担" },
            new SelectListItem { Text = "普通快递，到付，费用客户/供应商承担", Value = "普通快递，到付，费用客户/供应商承担" },
            new SelectListItem { Text = "客户到仓库自提", Value = "客户到仓库自提" },
            new SelectListItem { Text = "义乌周边送货上门", Value = "义乌周边送货上门" },
            new SelectListItem { Text = "顺丰快递，到付，费用客户/供应商承担", Value = "顺丰快递，到付，费用客户/供应商承担" },
            new SelectListItem { Text = "配好货，送门店，客户去门店自提", Value = "配好货，送门店，客户去门店自提" },
            new SelectListItem { Text = "德邦快递，到付，费用客户/供应商承担", Value = "德邦快递，到付，费用客户/供应商承担" },
            new SelectListItem { Text = "托运，现付，费用公司承担", Value = "托运，现付，费用公司承担" },
            new SelectListItem { Text = "送货上门，现付，费用公司承担", Value = "送货上门，现付，费用公司承担" },
            new SelectListItem { Text = "普通快递，现付，费用公司承担", Value = "普通快递，现付，费用公司承担" },
            new SelectListItem { Text = "顺丰快递，现付，费用公司承担", Value = "顺丰快递，现付，费用公司承担" },
            new SelectListItem { Text = "德邦快递，现付，费用公司承担", Value = "德邦快递，现付，费用公司承担" },
            new SelectListItem { Text = "托运，现付，费用客户/供应商承担", Value = "托运，现付，费用客户/供应商承担" },
            new SelectListItem { Text = "送货上门，现付，费用客户/供应商承担", Value = "送货上门，现付，费用客户/供应商承担" },
            new SelectListItem { Text = "普通快递，现付，费用客户/供应商承担", Value = "普通快递，现付，费用客户/供应商承担" },
            new SelectListItem { Text = "顺丰快递，现付，费用客户/供应商承担", Value = "顺丰快递，现付，费用客户/供应商承担" },
            new SelectListItem { Text = "德邦快递，现付，费用客户/供应商承担", Value = "德邦快递，现付，费用客户/供应商承担" }
        };



        public static List<SelectListItem> GetList(string key)
        {
            List<SelectListItem> SelectListList = new List<SelectListItem>();
            try
            {
                string strError = "";
                Common_Func.GetSelectListItem(key, ref SelectListList, ref strError);
                return SelectListList;
            }
            catch (Exception)
            {
                return SelectListList;
            }

        }

        public static List<SelectListItem> GetWarehouseList(string key)
        {
            List<SelectListItem> SelectListListBack = new List<SelectListItem>() {
            new SelectListItem{ Value="0",Text="全部" } };
            List<SelectListItem> SelectListList = new List<SelectListItem>();
            try
            {
                string strError = "";
                Common_Func.GetSelectListItem(key, ref SelectListList, ref strError);
                SelectListListBack.AddRange(SelectListList);
                return SelectListListBack;
            }
            catch (Exception)
            {
                return SelectListList;
            }
        }



        public static List<SelectListItem> GetHeaderNameList = new List<SelectListItem>
        {
            new SelectListItem { Text = "奥碧虹", Value = "奥碧虹" },
            new SelectListItem { Text = "傲之美", Value = "傲之美" },
            new SelectListItem { Text = "天猫", Value = "天猫" }
        };

        public static List<SelectListItem> GetStrongHoldcodeList = new List<SelectListItem>
        {
            new SelectListItem { Text = "上海嘉成", Value = "SHJC" },
            new SelectListItem { Text = "江苏嘉成", Value = "JSJC" },
            new SelectListItem { Text = "上海三意", Value = "SHSY" }
        };

        public static UserInfo ReadUserInfo()
        {
            string userno = ReadCookie("userinfo");
            UserInfo model = new UserInfo();
            if (!string.IsNullOrEmpty(userno))
            {
                User_DB _db = new User_DB();
                model = _db.GetModelByFilterByUserNo(userno);
                return model;
            }
            else
            {
                return null;
            }

        }


        public static string ReadCookie(string cn)
        {
            string str = "";
            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[cn];
                str = cookie["UserNo"].ToString();
            }
            catch (Exception ex)
            {
            }
            return str;
        }



    }
}