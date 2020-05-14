using BILBasic.Common;
using BILWeb.Check;
using BILWeb.Login.User;
using BILWeb.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WMS.Web.Filter;

namespace Web.WMS.Controllers
{
    [RoleActionFilter(Message = "Check/Check")]
    public class CheckController : Controller
    {
         UserInfo currentUser = Common.Commom.ReadUserInfo();
        Check_DB tfun = new Check_DB();

        public ActionResult PageView(PageData<Check_Model> model)
        {
            ViewData["dividPage"] = model.dividPage;
            ViewData["url"] = model.link;
            return View("_ViewPage");
        }

        public ActionResult GetModelList(DividPage page, Check_Model model)
        {
            List<Check_Model> modelList = new List<Check_Model>();
            string strError = "";
            tfun.GetCheckInfo(model, ref page, ref modelList, ref strError);
            ViewData["PageData"] = new PageData<Check_Model> { data = modelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/Check/GetModelList") };
            return View("GetModelList", model);
        }


        public FileResult Excel(CheckAnalyze model)
        {
            string strErrMsg = string.Empty;
            List<CheckAnalyze> lstExport = new List<CheckAnalyze>();
            string strError = "";
            DividPage page = new DividPage
            {
                CurrentPageShowCounts = 10000000
            };
            tfun.GetCheckAnalyze(model, ref page, ref lstExport, ref strError);

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("盈亏情况");
            row1.CreateCell(1).SetCellValue("实盘据点");
            row1.CreateCell(2).SetCellValue("实盘物料号");
            row1.CreateCell(3).SetCellValue("实盘物料描述");
            row1.CreateCell(4).SetCellValue("实盘库位");
            row1.CreateCell(5).SetCellValue("实盘序列号");
            row1.CreateCell(6).SetCellValue("实盘数量");
            row1.CreateCell(7).SetCellValue("库存数量");
            row1.CreateCell(8).SetCellValue("库存序列号");
            row1.CreateCell(9).SetCellValue("库存库位");
            row1.CreateCell(10).SetCellValue("库存物料号");
            row1.CreateCell(11).SetCellValue("库存物料描述");
            row1.CreateCell(12).SetCellValue("库存据点");
            row1.CreateCell(13).SetCellValue("盘盈数量");
            row1.CreateCell(14).SetCellValue("盘亏数量");
            row1.CreateCell(15).SetCellValue("操作人");
            
            //将数据逐步写入seet1各个行
            for (int i = 0; i < lstExport.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);

                rowtemp.CreateCell(0).SetCellValue(lstExport[i].remark == null ? "" : lstExport[i].remark.ToString());
                rowtemp.CreateCell(1).SetCellValue(lstExport[i].STRONGHOLDCODE == null ? "" : lstExport[i].STRONGHOLDCODE.ToString());
                rowtemp.CreateCell(2).SetCellValue(lstExport[i].MATERIALNO == null ? "" : lstExport[i].MATERIALNO.ToString());
                rowtemp.CreateCell(3).SetCellValue(lstExport[i].MATERIALDESC == null ? "" : lstExport[i].MATERIALDESC.ToString());
                rowtemp.CreateCell(4).SetCellValue(lstExport[i].AREANO == null ? "" : lstExport[i].AREANO.ToString());
                rowtemp.CreateCell(5).SetCellValue(lstExport[i].SERIALNO == null ? "" : lstExport[i].SERIALNO.ToString());
                rowtemp.CreateCell(6).SetCellValue(lstExport[i].QTY == null ? "" : lstExport[i].QTY.ToString());
                rowtemp.CreateCell(7).SetCellValue(lstExport[i].SQTY == null ? "" : lstExport[i].SQTY.ToString());
                rowtemp.CreateCell(8).SetCellValue(lstExport[i].SSERIALNO.ToString());
                rowtemp.CreateCell(9).SetCellValue(lstExport[i].SAREANO.ToString());
                rowtemp.CreateCell(10).SetCellValue(lstExport[i].SMATERIALNO == null ? "" : lstExport[i].SMATERIALNO.ToString());
                rowtemp.CreateCell(11).SetCellValue(lstExport[i].SMATERIALDESC == null ? "" : lstExport[i].SMATERIALDESC.ToString());
                rowtemp.CreateCell(12).SetCellValue(lstExport[i].SSTRONGHOLDCODE == null ? "" : lstExport[i].SSTRONGHOLDCODE.ToString());
                rowtemp.CreateCell(13).SetCellValue(lstExport[i].YQTY == null ? "" : lstExport[i].YQTY.ToString());
                rowtemp.CreateCell(14).SetCellValue(lstExport[i].KQTY == null ? "" : lstExport[i].KQTY.ToString());
                rowtemp.CreateCell(15).SetCellValue(lstExport[i].Creater == null ? "" : lstExport[i].Creater.ToString());
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }



        [HttpPost]
        public ActionResult Add(Check_Model model,string strAll)
        {
            model.CHECKSTATUS = "新建";
            model.CHECKTYPE = "货位";
            model.CREATER = currentUser.UserName;
            List<CheckArea_Model> list = new List<CheckArea_Model>();
            string[] strsp = strAll.Split(';');
            foreach (string item in strsp)
            {
                if (item.Contains(','))
                {
                    CheckArea_Model m = new CheckArea_Model();
                    string[] strsp2 = item.Split(',');
                    m.AREANO = strsp2[1];
                    m.ID =Convert.ToInt32(strsp2[0]);
                    list.Add(m);
                }
            }
            string ErrMsg = "";
            bool bSucc = tfun.SaveCheck(model, list, ref ErrMsg);
            if (!bSucc)
            {
                ErrMsg="保存失败！" + ErrMsg;
            }
            else
            {
                ErrMsg = "保存成功！";
            }
            return Json(ErrMsg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult shanchu(string CHECKNO)
        {
            string ErrMsg = "";
            if (tfun.DelCloCheck(CHECKNO, 0, ref ErrMsg, ""))
            {
                ErrMsg = "删除成功！";
            }
            else
            {
                ErrMsg = "删除失败！" + ErrMsg;
            }
            return Json(ErrMsg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult zhongzhi(string CHECKNO)
        {
            string ErrMsg = "";
            if (tfun.DelCloCheck(CHECKNO, 1, ref ErrMsg, ""))
            {
                ErrMsg="终止成功！";
            }
            else
            {
                ErrMsg = "终止失败！"+ErrMsg;
            }
            return Json(ErrMsg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Delete(Check_Model model)
        {
            return RedirectToAction("GetModelList");
        }

        [HttpGet]
        public ActionResult GetModel (Check_Model model)
        {
            Check_Func db = new Check_Func();
            ViewData["Taskno"] = db.GetTableID("SEQ_CHECK_NO").ToString();
            return View();
        }

        
        public JsonResult GetDetail(int Svalue,string areano,string houseno,string warehouseno) {
            Thread.Sleep(5000);
            List<CheckArea_Model> lsttask = new List<CheckArea_Model>();
            tfun.GetCheckArea(Svalue, areano, houseno, warehouseno, ref lsttask);
            return Json(lsttask, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckAnalyze(DividPage page, CheckAnalyze model)
        {
            List<CheckAnalyze> modelList = new List<CheckAnalyze>();
            string strError = "";
            tfun.GetCheckAnalyze(model, ref page, ref modelList, ref strError);
            ViewData["PageData"] = new PageData<CheckAnalyze> { data = modelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/Check/CheckAnalyze") };
            return View("CheckAnalyze", model);
        }
        public ActionResult PageView1(PageData<CheckAnalyze> model)
        {
            ViewData["dividPage"] = model.dividPage;
            ViewData["url"] = model.link;
            return View("_ViewPage");
        }

        public JsonResult tiaozheng(string CHECKNO) {
            if (currentUser == null)
            {
                return Json("Cookie失效，重新登陆！" , JsonRequestBehavior.AllowGet);
            }
            string ErrMsg = "";
            Check_DB db = new Check_DB();
            if (db.DelCloCheck(CHECKNO, 2, ref ErrMsg, currentUser.UserNo))
            {
                return Json("调整成功！", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ErrMsg, JsonRequestBehavior.AllowGet);
            }
        }


        
        public JsonResult tiaozheng_ms(string CHECKNO)
        {
            if (currentUser == null)
            {
                return Json("Cookie失效，重新登陆！", JsonRequestBehavior.AllowGet);
            }
            string ErrMsg = "";
            Check_DB db = new Check_DB();
            if (db.DelCloCheck_MsTest(CHECKNO, 2, ref ErrMsg, currentUser.UserNo))
            {
                return Json("调整成功！"+ ErrMsg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ErrMsg, JsonRequestBehavior.AllowGet);
            }
        }
    }
}