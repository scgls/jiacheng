using BILBasic.Common;
using BILWeb.Customer;
using BILWeb.LandMark;
using BILWeb.OutStockTask;
using BILWeb.Stock;
using BILWeb.TransportSupplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.WMS.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            //PDA查询
            //T_OutStockTask_Func tfunc = new T_OutStockTask_Func();
            //tfunc.GetT_OutTaskListADF("","10016700", "1");

            //T_LandMark_DB db = new T_LandMark_DB();
            //db.Getlandmark("003");
            //T_LandMarkWithTaskInfo model = new T_LandMarkWithTaskInfo();
            //T_LandMarkWithTask_DB dbb = new T_LandMarkWithTask_DB();
            //string strmsg = "";
            //dbb.GetTaskForLandmark("1@FY2@10011931@8008970000831@2024-04-23@20240423@6@20190906001297", ref model, ref strmsg);


            //T_Stock_Func tfunc = new T_Stock_Func();
            //tfunc.GetStockModelBySql("{\"AreaID\":4947,\"AreaType\":0,\"Barcode\":\"6938826811521\",\"FromAreaID\":0,\"FromHouseID\":0,\"FromWareHouseID\":0,\"HouseID\":0,\"HouseProp\":2,\"IsLimitStock\":0,\"OutstockDetailID\":0,\"OutstockHeaderID\":0,\"PickModel\":0,\"ScanType\":2,\"StockBarCodeStatus\":0,\"WareHouseID\":1162,\"ErpLineStatus\":0,\"HeaderID\":0,\"ID\":0,\"LineStatus\":0,\"MaterialNoID\":0,\"Status\":0,\"StockType\":0,\"TerminateReasonID\":0,\"VoucherType\":0}");


            //T_LandMarkWithTask_Func func = new T_LandMarkWithTask_Func();
            //func.SaveTaskwithandmark("{\"CarNo\":\"002\",\"Landmarkid\":1.0,\"TaskNo\":\"D201909010485\",\"landmarkno\":\"001\",\"EDate\":\"Dec 31, 3938 7:00:00 PM\",\"ErpLineStatus\":0,\"ErpVoucherNo\":\"FY2-DB6-1909010001\",\"HeaderID\":0,\"ID\":0,\"LineStatus\":0,\"MaterialNoID\":0,\"Status\":0,\"StockType\":0,\"TerminateReasonID\":0,\"VoucherType\":0}", "{\"BIsAdmin\":true,\"BIsOnline\":false,\"IsOnline\":1,\"PickAreaNo\":\"DFK001\",\"PickWareHouseNo\":\"MS006\",\"QuanUserName\":\"\",\"QuanUserNo\":\"\",\"ReceiveAreaNo\":\"DSK001\",\"ReceiveWareHouseNo\":\"MS006\",\"StrIsAdmin\":\"管理员\",\"StrSex\":\"男\",\"StrUserStatus\":\"正常\",\"StrUserType\":\"管理员\",\"WarehouseName\":\"储备主仓\",\"lstMenu\":[{\"BHaveParameter\":false,\"BIsChecked\":false,\"BIsDefault\":false,\"Description\":\"预收货\",\"IsDel\":1.0,\"MenuStatus\":1,\"MenuStyle\":0.0,\"MenuType\":4,\"NodeUrl\":\"0\",\"ProjectName\":\"预收货\",\"SafeLevel\":1.0},{\"BHaveParameter\":false,\"BIsChecked\":false,\"BIsDefault\":false,\"Description\":\"收货\",\"IsDel\":1.0,\"MenuStatus\":1,\"MenuStyle\":0.0,\"MenuType\":4,\"NodeUrl\":\"2\",\"ProjectName\":\"收货\",\"SafeLevel\":1.0},{\"BHaveParameter\":false,\"BIsChecked\":false,\"BIsDefault\":false,\"Description\":\"上架\",\"IsDel\":1.0,\"MenuStatus\":1,\"MenuStyle\":0.0,\"MenuType\":4,\"NodeUrl\":\"3\",\"ProjectName\":\"上架\",\"SafeLevel\":1.0},{\"BHaveParameter\":false,\"BIsChecked\":false,\"BIsDefault\":false,\"Description\":\"发货复核\",\"IsDel\":1.0,\"MenuStatus\":1,\"MenuStyle\":0.0,\"MenuType\":4,\"NodeUrl\":\"5\",\"ProjectName\":\"发货复核\",\"SafeLevel\":1.0},{\"BHaveParameter\":false,\"BIsChecked\":false,\"BIsDefault\":false,\"Description\":\"下架\",\"IsDel\":1.0,\"MenuStatus\":1,\"MenuStyle\":0.0,\"MenuType\":4,\"NodeUrl\":\"4\",\"ProjectName\":\"下架\",\"SafeLevel\":1.0},{\"BHaveParameter\":false,\"BIsChecked\":false,\"BIsDefault\":false,\"Description\":\"盘点\",\"IsDel\":1.0,\"MenuStatus\":1,\"MenuStyle\":0.0,\"MenuType\":4,\"NodeUrl\":\"7\",\"ProjectName\":\"盘点\",\"SafeLevel\":1.0},{\"BHaveParameter\":false,\"BIsChecked\":false,\"BIsDefault\":false,\"Description\":\"移库\",\"IsDel\":1.0,\"MenuStatus\":1,\"MenuStyle\":0.0,\"MenuType\":4,\"NodeUrl\":\"6\",\"ProjectName\":\"移库\",\"SafeLevel\":1.0},{\"BHaveParameter\":false,\"BIsChecked\":false,\"BIsDefault\":false,\"Description\":\"PDA菜单\",\"IsDel\":1.0,\"MenuStatus\":1,\"MenuStyle\":0.0,\"MenuType\":4,\"NodeUrl\":\"14\",\"ProjectName\":\"库存调整\",\"SafeLevel\":1.0},{\"BHaveParameter\":false,\"BIsChecked\":false,\"BIsDefault\":false,\"Description\":\"装车扫描\",\"IsDel\":1.0,\"MenuStatus\":1,\"MenuStyle\":0.0,\"MenuType\":4,\"NodeUrl\":\"20\",\"ProjectName\":\"装车扫描\",\"SafeLevel\":1.0},{\"BHaveParameter\":false,\"BIsChecked\":false,\"BIsDefault\":false,\"Description\":\"查询\",\"IsDel\":1.0,\"MenuStatus\":1,\"MenuStyle\":0.0,\"MenuType\":4,\"NodeUrl\":\"8\",\"ProjectName\":\"查询\",\"SafeLevel\":1.0},{\"BHaveParameter\":false,\"BIsChecked\":false,\"BIsDefault\":false,\"Description\":\"物流扫描\",\"IsDel\":1.0,\"MenuStatus\":1,\"MenuStyle\":0.0,\"MenuType\":4,\"NodeUrl\":\"34\",\"ProjectName\":\"物流扫描\",\"SafeLevel\":1.0},{\"BHaveParameter\":false,\"BIsChecked\":false,\"BIsDefault\":false,\"Description\":\"补货扫描\",\"IsDel\":1.0,\"MenuStatus\":1,\"MenuStyle\":0.0,\"MenuType\":4,\"NodeUrl\":\"35\",\"ProjectName\":\"补货扫描\",\"SafeLevel\":1.0},{\"BHaveParameter\":false,\"BIsChecked\":false,\"BIsDefault\":false,\"Description\":\"地标扫描\",\"IsDel\":1.0,\"MenuStatus\":1,\"MenuStyle\":0.0,\"MenuType\":4,\"NodeUrl\":\"36\",\"ProjectName\":\"地标扫描\",\"SafeLevel\":1.0}],\"lstUserGroup\":[{\"BIsChecked\":true,\"IsDel\":1.0,\"UserGroupAbbname\":\"管理员组\",\"UserGroupName\":\"管理员组\",\"UserGroupNo\":\"1\",\"UserGroupStatus\":1,\"UserGroupType\":2}],\"lstWarehouse\":[{\"AreaCount\":0,\"AreaRate\":0.0,\"AreaUsingCount\":0,\"BIsChecked\":true,\"BIsLock\":false,\"HouseCount\":10,\"HouseRate\":0.0,\"HouseUsingCount\":0,\"ID\":16,\"IsDel\":1.0,\"PickRule\":0,\"WareHouseName\":\"电子商务仓\",\"WareHouseNo\":\"MS005\",\"WareHouseStatus\":1,\"WareHouseType\":0},{\"AreaCount\":0,\"AreaRate\":0.0,\"AreaUsingCount\":0,\"BIsChecked\":true,\"BIsLock\":false,\"HouseCount\":24,\"HouseRate\":0.0,\"HouseUsingCount\":0,\"ID\":1120,\"IsDel\":1.0,\"LocationDesc\":\"储备主仓\",\"PickRule\":0,\"WareHouseName\":\"储备主仓\",\"WareHouseNo\":\"MS006\",\"WareHouseStatus\":1,\"WareHouseType\":0},{\"AreaCount\":0,\"AreaRate\":0.0,\"AreaUsingCount\":0,\"BIsChecked\":true,\"BIsLock\":false,\"HouseCount\":3,\"HouseRate\":0.0,\"HouseUsingCount\":0,\"ID\":1140,\"IsDel\":1.0,\"PickRule\":0,\"WareHouseName\":\"特批采购仓\",\"WareHouseNo\":\"MS002\",\"WareHouseStatus\":1,\"WareHouseType\":0},{\"AreaCount\":0,\"AreaRate\":0.0,\"AreaUsingCount\":0,\"BIsChecked\":true,\"BIsLock\":false,\"HouseCount\":3,\"HouseRate\":0.0,\"HouseUsingCount\":0,\"ID\":1141,\"IsDel\":1.0,\"PickRule\":0,\"WareHouseName\":\"待处理仓\",\"WareHouseNo\":\"MS501\",\"WareHouseStatus\":1,\"WareHouseType\":0},{\"AreaCount\":0,\"AreaRate\":0.0,\"AreaUsingCount\":0,\"BIsChecked\":true,\"BIsLock\":false,\"HouseCount\":3,\"HouseRate\":0.0,\"HouseUsingCount\":0,\"ID\":1142,\"IsDel\":1.0,\"PickRule\":0,\"WareHouseName\":\"坏品仓\",\"WareHouseNo\":\"MS502\",\"WareHouseStatus\":1,\"WareHouseType\":0},{\"AreaCount\":0,\"AreaRate\":0.0,\"AreaUsingCount\":0,\"BIsChecked\":true,\"BIsLock\":false,\"HouseCount\":3,\"HouseRate\":0.0,\"HouseUsingCount\":0,\"ID\":1143,\"IsDel\":1.0,\"PickRule\":0,\"WareHouseName\":\"储位异常仓\",\"WareHouseNo\":\"MS504\",\"WareHouseStatus\":1,\"WareHouseType\":0},{\"AreaCount\":0,\"AreaRate\":0.0,\"AreaUsingCount\":0,\"BIsChecked\":true,\"BIsLock\":false,\"HouseCount\":2,\"HouseRate\":0.0,\"HouseUsingCount\":0,\"ID\":1144,\"IsDel\":1.0,\"PickRule\":0,\"WareHouseName\":\"耗材仓库\",\"WareHouseNo\":\"MS701\",\"WareHouseStatus\":1,\"WareHouseType\":0},{\"AreaCount\":0,\"AreaRate\":0.0,\"AreaUsingCount\":0,\"BIsChecked\":true,\"BIsLock\":false,\"HouseCount\":3,\"HouseRate\":0.0,\"HouseUsingCount\":0,\"ID\":1145,\"IsDel\":1.0,\"PickRule\":0,\"WareHouseName\":\"平台代发专用仓\",\"WareHouseNo\":\"MS702\",\"WareHouseStatus\":1,\"WareHouseType\":0},{\"AreaCount\":0,\"AreaRate\":0.0,\"AreaUsingCount\":0,\"BIsChecked\":true,\"BIsLock\":false,\"HouseCount\":2,\"HouseRate\":0.0,\"HouseUsingCount\":0,\"ID\":1146,\"IsDel\":1.0,\"PickRule\":0,\"WareHouseName\":\"拼多多专用仓\",\"WareHouseNo\":\"MS703\",\"WareHouseStatus\":1,\"WareHouseType\":0},{\"AreaCount\":0,\"AreaRate\":0.0,\"AreaUsingCount\":0,\"BIsChecked\":true,\"BIsLock\":false,\"HouseCount\":2,\"HouseRate\":0.0,\"HouseUsingCount\":0,\"ID\":1147,\"IsDel\":1.0,\"PickRule\":0,\"WareHouseName\":\"C01\",\"WareHouseNo\":\"C01\",\"WareHouseStatus\":1,\"WareHouseType\":0},{\"AreaCount\":0,\"AreaRate\":0.0,\"AreaUsingCount\":0,\"BIsChecked\":true,\"BIsLock\":false,\"HouseCount\":0,\"HouseRate\":0.0,\"HouseUsingCount\":0,\"ID\":1160,\"IsDel\":1.0,\"PickRule\":0,\"WareHouseName\":\"仓库003\",\"WareHouseNo\":\"ck003\",\"WareHouseStatus\":1,\"WareHouseType\":0},{\"AreaCount\":0,\"AreaRate\":0.0,\"AreaUsingCount\":0,\"BIsChecked\":true,\"BIsLock\":false,\"HouseCount\":23,\"HouseRate\":0.0,\"HouseUsingCount\":0,\"ID\":1180,\"IsDel\":1.0,\"PickRule\":0,\"WareHouseName\":\"MS007测试仓\",\"WareHouseNo\":\"MS007\",\"WareHouseStatus\":1,\"WareHouseType\":0}],\"Email\":\"2008050001\",\"GroupCode\":\"1\",\"GroupName\":\"管理员组\",\"ID\":175382,\"IsDel\":1.0,\"IsPick\":0,\"IsPickLeader\":1,\"IsQuality\":0,\"IsReceive\":0,\"Mobile\":\"18969350230\",\"PDAPrintIP\":\"1.1.1.1\",\"PassWord\":\"123\",\"PickAreaID\":30221,\"PickHouseID\":1188,\"PickLeader\":false,\"PickWareHouseID\":1120,\"ReceiveAreaID\":30220,\"ReceiveHouseID\":1189,\"Sex\":1,\"StrIsPick\":\"不拣货\",\"StrIsPickLeader\":\"否\",\"StrIsReceive\":\"不收货\",\"UserName\":\"叶盼盼\",\"UserNo\":\"2008050001\",\"UserStatus\":1,\"UserType\":1,\"WarehouseCode\":\"MS006\",\"WarehouseID\":1120}");

            //T_TransportSupDetail_Func func = new T_TransportSupDetail_Func();
            //func.GetTransportSupplierDetailList("P201909091577");

            //T_Stock_Func tfunc = new T_Stock_Func();
            //tfunc.GetStockModelBySql("{\"AreaID\":690,\"AreaType\":0,\"Barcode\":\"6937803921260\",\"FromAreaID\":0,\"FromHouseID\":0,\"FromWareHouseID\":0,\"HouseID\":0,\"HouseProp\":2,\"IsLimitStock\":0,\"OutstockDetailID\":0,\"OutstockHeaderID\":0,\"PickModel\":0,\"ScanType\":2,\"StockBarCodeStatus\":0,\"WareHouseID\":1167,\"ErpLineStatus\":0,\"HeaderID\":0,\"ID\":0,\"LineStatus\":0,\"MaterialNoID\":0,\"Status\":0,\"StockType\":0,\"TerminateReasonID\":0,\"VoucherType\":0}");

            T_Stock_Func tfunc = new T_Stock_Func();
            tfunc.GetStockInfoByScanType("", "10016173", "1");

            return View();
        }

        public ActionResult Index1()
        {
            return View();
        }

        public JsonResult table(string aaa,string bbb, string page,string limit)
        {
            List<bbb> b = new List<bbb>();
            for (int i = 0; i < 20; i++)
            {
                bbb bb = new bbb
                {
                    ID = i.ToString(),
                    productName = page +"ye"+ i.ToString(),
                    productState = "",
                    effectTime = i.ToString(),
                    invalidTime = i.ToString(),
                    productCost = i.ToString(),
                    poperation = i.ToString()
                };
                b.Add(bb);
            }

            AAA a = new AAA
            {
                code = "0",
                msg = "",
                count = "200",
                data = b
            };
            return Json(a, JsonRequestBehavior.AllowGet);


            //DividPage page = new DividPage { CurrentPageShowCounts = 10000 };

            //T_Customer_Func func = new T_Customer_Func();
            //List<T_CustomerInfo> modelList = new List<T_CustomerInfo>();
            //T_CustomerInfo model = new T_CustomerInfo();
            //string strError = "";
            //func.GetModelListByPage(ref modelList, Common.Commom.currentUser, model, ref page, ref strError);
            //return Json(new { state = false, obj = modelList }, JsonRequestBehavior.AllowGet);

        }

        public class AAA
        {
            public string code { get; set; }
            public string msg { get; set; }
            public string count { get; set; }
            public List<bbb> data { get; set; }
        }
        public class bbb
        {
            public string ID { get; set; }
            public string productName { get; set; }
            public string productState { get; set; }
            public string effectTime { get; set; }
            public string invalidTime { get; set; }
            public string productCost { get; set; }
            public string poperation { get; set; }
        }

    }
}
