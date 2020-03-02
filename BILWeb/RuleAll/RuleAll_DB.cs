using BILBasic.DBA;
using BILBasic.User;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;
using System.Data;


namespace BILWeb.RuleAll
{
    public partial class t_RuleAll_DB : BILBasic.Basing.Factory.Base_DB<T_RuleAllInfo>
    {

        /// <summary>
        /// 添加t_ruleall
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_RuleAllInfo t_ruleall)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user,ref T_RuleAllInfo t_ruleall)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_RuleAllInfo ToModel(IDataReader reader)
        {
            T_RuleAllInfo t_ruleall = new T_RuleAllInfo();

            t_ruleall.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_ruleall.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToInt32();
            t_ruleall.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_ruleall.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_ruleall.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_ruleall.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_ruleall.ConItemID = dbFactory.ToModelValue(reader, "CONITEMID").ToInt32();
            t_ruleall.IsEnable = dbFactory.ToModelValue(reader, "ISENABLE").ToInt32();
            t_ruleall.strConItemID = (string)dbFactory.ToModelValue(reader, "strConItemID");
            t_ruleall.strIsEnable = (string)dbFactory.ToModelValue(reader, "strIsEnable");

            return t_ruleall;
        }

        protected override string GetViewName()
        {
            return "v_Ruleall";
        }

        protected override string GetTableName()
        {
            return "T_RULEALL";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override List<string> GetUpdateSql(UserModel user, T_RuleAllInfo model)
        {
            List<string> lstSql = new List<string>();

            string strSql = "update t_Ruleall set isenable = '"+model.IsEnable+"' where id = '"+model.ID+"'";

            lstSql.Add(strSql);

            return lstSql;
        }

        /// <summary>
        /// 重写更新方法
        /// 数据库保存成功，修改缓存
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public override bool UpdateModelByModelSql(UserModel user, T_RuleAllInfo model, ref string strError)
        {
            bool bSucc = base.UpdateModelByModelSql(user, model, ref strError);
            //if (bSucc == true) 
            //{
            //    object obj = ExampleCache.GetExampleCache().Get("ruleall");
            //    if (obj != null)
            //    {
            //        List<T_RuleAllInfo> modelList = obj as List<T_RuleAllInfo>;
            //        modelList.Find(t => t.ID == model.ID).IsEnable = model.IsEnable;
            //    }
            //}

            return bSucc;
        }

        /// <summary>
        /// 重写获取数据列表方法
        /// 先从缓存读取，读取异常或者失败从数据库读取
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public override List<T_RuleAllInfo> GetModelListByPage(UserModel user, T_RuleAllInfo model, ref DividPage page, string condition)
        {
            //object obj = ExampleCache.GetExampleCache().Get("ruleall");
            ////设置缓存
            //if (obj != null)
            //{
            //    return (List<T_RuleAllInfo>)obj;
            //}

            List<T_RuleAllInfo> modelList = base.GetModelListByPage(user, model, ref page, condition);

            //ExampleCache.GetExampleCache().Set("ruleall", modelList);
            return modelList;
        }

        protected override string GetFilterSql(UserModel user, T_RuleAllInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";

            

            if (model.ConItemID > 0 )
            {
                strSql += strAnd;
                strSql += " ConItemID = " + model.ConItemID + " ";
            }
            return strSql;

        }

        /// <summary>
        /// 提供给外部调用的获取规则的方法
        /// </summary>
        /// <param name="ConItemID"></param>
        /// <returns></returns>
        public List<T_RuleAllInfo> GetRuleListByPage(int ConItemID) 
        {
            RuleAll.T_RuleAllInfo rullModel = new RuleAll.T_RuleAllInfo();
            BILBasic.Common.DividPage page = null;
            if (ConItemID > 0) 
            {
                rullModel.ConItemID = ConItemID;
            }
            return  GetModelListByPage(null, rullModel, ref page,"");
        }


    }
}

