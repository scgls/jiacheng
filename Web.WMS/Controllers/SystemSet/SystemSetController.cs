using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.PickRule;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.SystemSet
{
    [RoleActionFilter(Message = "Basic/SystemSet")]
    /// <summary>
    /// 系统设置模块
    /// </summary>
    public class SystemSetController : BaseController<T_PickRuleInfo>
    {
        private UserInfo currentUser = Common.Commom.ReadUserInfo();

        private IPickRuleService pickRuleService;
        public SystemSetController()
        {
            pickRuleService = (IPickRuleService)ServiceFactory.CreateObject("PickRule.T_PickRule_Func");
            baseservice = pickRuleService;
        }

        #region 规格视图
        /// <summary>
        /// 拣货规则
        /// </summary>
        /// <returns></returns>
        public ActionResult PickRule() {

            ViewData["type"] = "pick";
            return Redirect("Index");
        }


        /// <summary>
        /// 批处理规则
        /// </summary>
        /// <returns></returns>
        public ActionResult BatchRule() {
            ViewData["type"] = "bath";
            return Redirect("Index");
        }

        /// <summary>
        /// 生单规则
        /// </summary>
        /// <returns></returns>
        public ActionResult RawOrderRule(){
            ViewData["type"] = "rawOrder";
            return Redirect("Index");

        }

        /// <summary>
        /// 分单规则
        /// </summary>
        /// <returns></returns>
        public ActionResult SeparateOrderRule() {
            ViewData["type"] = "separateOrder";
            return Redirect("Index");
        }

        /// <summary>
        ///补货规则 
        /// </summary>
        /// <returns></returns>
        public ActionResult SupplementRule() {
            ViewData["type"] = "supplement";
            return Redirect("Index");
        }

        /// <summary>
        /// 上架规则
        /// </summary>
        /// <returns></returns>
        public ActionResult UppershelfRule() {
            ViewData["type"] = "uppershelf";
            return Redirect("Index");

        }

        #endregion


        /// <summary>
        /// 系统规格界面
        /// </summary>
        /// <param name="type">1:上架，2：生单，3拣货，4分单，5补货，6批次</param>
        /// <returns></returns>
        public ActionResult Index(int type) {
            T_PickRuleInfo model = new T_PickRuleInfo();
            model.RuleType = type;
            List<T_PickRuleInfo> modelList = new List<T_PickRuleInfo>();
            DividPage page = new DividPage();
            string strError = "";
            baseservice.GetModelListByPage(ref modelList, currentUser, model, ref page, ref strError);
            ViewData["PageData"] = new PageData<T_PickRuleInfo> { data = modelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/" + System.Web.HttpContext.Current.Session["Message"].ToString() + "/GetModelList") };
            return View(model);
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add() {

            return View();
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit() {

            return View();
        }


        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteId(int id,int type) {
            T_PickRuleInfo info = new T_PickRuleInfo();
            info.ID = id;
            string str = "";
            baseservice.DeleteModelByModelSql(currentUser, info, ref str);
            return RedirectToAction("GetModelList", new { RuleType= type });
        }
        

        /// <summary>
        /// 新增修改保存
        /// </summary>
        /// <param name="modeladd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveModel(T_PickRuleInfo modeladd)
        {
            string strMsg = "";
            if (baseservice.SaveModelBySqlToDB(currentUser, ref modeladd, ref strMsg))
            {
                return RedirectToAction("GetModelList",new { RuleType = modeladd.RuleType});
            }
            else
            {
                this.TempData["modeladd"] = modeladd;
                return RedirectToAction("GetModel", new { model = modeladd, strMsg = strMsg });
            }
        }



        public ActionResult pageNav() {


            return View();
        }


        public ActionResult IndexView() {
            return View();
        }

    }
}