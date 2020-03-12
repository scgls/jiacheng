using BILBasic.Common;
using BILWeb.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Web.WMS.Common;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Query
{
    [RoleActionFilter(Message = "Query/TaskTrans")]
    public class TaskTransController : Controller
    {
        Query_DB queryDB = new Query_DB();

        // GET: TaskTrans
        public ActionResult Index()
        {
            List<TaskTrans_Model> modelList = new List<TaskTrans_Model>();
            DividPage page = new DividPage();
            TaskTrans_Model model = new TaskTrans_Model();
            ViewData["PageData"] = new PageData<TaskTrans_Model> { data = modelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/TaskTrans/GetModelList") };
            return View("GetModelList", model);
        }

        public ActionResult PageView(PageData<TaskTrans_Model> model)
        {
            ViewData["dividPage"] = model.dividPage;
            ViewData["url"] = model.link;
            return View("_ViewPage");
        }

        public ActionResult GetModelList(DividPage page, TaskTrans_Model model)
        {
            //var selectItemList = new List<SelectListItem>();
            //SelectListItem selectListItem;

            //if (string.IsNullOrEmpty(model.tasktypename))
            //{
            //    selectListItem = new SelectListItem() { Value = "0", Text = "全部", Selected = true };
            //}
            //else
            //{
            //    selectListItem = new SelectListItem() { Value = model.TASKTYPE.ToString(), Text = model.tasktypename, Selected = true };
            //}
            //selectItemList.Add(selectListItem);
            //var selectList = new SelectList(Commom.TaskTypeList, "Id", "Name");
            //selectItemList.AddRange(selectList);
            //ViewBag.ComTaskType = selectItemList;

            List<TaskTrans_Model> modelList = new List<TaskTrans_Model>();
            string strError = "";
            queryDB.GetTaskTransInfo(model, ref page, ref modelList, ref strError);
            ViewData["PageData"] = new PageData<TaskTrans_Model> { data = modelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/TaskTrans/GetModelList") };
            return View("GetModelList", model);
        }




        public FileResult Excel(TaskTrans_Model model)
        {
            DividPage page = new DividPage();
            page.CurrentPageShowCounts = 1000000;
            List<TaskTrans_Model> list = new List<TaskTrans_Model>();
            string str = "";
            queryDB.GetTaskTransInfo(model, ref page, ref list, ref str);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("ERP单号");
            row1.CreateCell(2).SetCellValue("据点");
            row1.CreateCell(3).SetCellValue("物料号");
            row1.CreateCell(4).SetCellValue("物料名");
            row1.CreateCell(5).SetCellValue("序列号");
            row1.CreateCell(6).SetCellValue("数量");
            row1.CreateCell(7).SetCellValue("单位");
            row1.CreateCell(8).SetCellValue("条码");
            row1.CreateCell(9).SetCellValue("批次");
            row1.CreateCell(10).SetCellValue("来自仓库");
            row1.CreateCell(11).SetCellValue("来自库区");
            row1.CreateCell(12).SetCellValue("来自库位");
            row1.CreateCell(13).SetCellValue("到仓库");
            row1.CreateCell(14).SetCellValue("到库区");
            row1.CreateCell(15).SetCellValue("到库位");
            row1.CreateCell(16).SetCellValue("状态");
            row1.CreateCell(17).SetCellValue("任务号");
            row1.CreateCell(18).SetCellValue("任务类别");
            row1.CreateCell(19).SetCellValue("单据类别");
            row1.CreateCell(20).SetCellValue("创建人");
            row1.CreateCell(21).SetCellValue("创建时间");

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].XH==null?"": list[i].XH);
                rowtemp.CreateCell(1).SetCellValue(list[i].ERPVOUCHERNO);
                rowtemp.CreateCell(2).SetCellValue(list[i].StrongHoldCode);
                rowtemp.CreateCell(3).SetCellValue(list[i].MATERIALNO);
                rowtemp.CreateCell(4).SetCellValue(list[i].MATERIALDESC);
                rowtemp.CreateCell(5).SetCellValue(list[i].SERIALNO);
                rowtemp.CreateCell(6).SetCellValue(list[i].QTY.ToString());
                rowtemp.CreateCell(7).SetCellValue(list[i].UNIT);
                rowtemp.CreateCell(8).SetCellValue(list[i].BARCODE);
                rowtemp.CreateCell(9).SetCellValue(list[i].BATCHNO);
                rowtemp.CreateCell(10).SetCellValue(list[i].FROMWAREHOUSENO);
                rowtemp.CreateCell(11).SetCellValue(list[i].FROMHOUSENO);
                rowtemp.CreateCell(12).SetCellValue(list[i].FROMAREANO);
                rowtemp.CreateCell(13).SetCellValue(list[i].TOWAREHOUSENO);
                rowtemp.CreateCell(14).SetCellValue(list[i].TOHOUSENO);
                rowtemp.CreateCell(15).SetCellValue(list[i].TOAREANO);
                rowtemp.CreateCell(16).SetCellValue(list[i].StatusName);
                rowtemp.CreateCell(17).SetCellValue(list[i].TASKNO);
                rowtemp.CreateCell(18).SetCellValue(list[i].tasktypename);
                rowtemp.CreateCell(19).SetCellValue(list[i].VOUCHERTYPE);
                rowtemp.CreateCell(20).SetCellValue(list[i].CREATETIME);
                rowtemp.CreateCell(21).SetCellValue(list[i].CREATER);
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

        }





    }
}