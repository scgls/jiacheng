using BILBasic.DBA;
using System;
using System.Collections.Generic;
using System.Data;
using BILBasic.Common;
using Oracle.ManagedDataAccess.Client;
using System.Reflection;
using System.Text;

namespace BILBasic.Basing
{
    public abstract class Base_DB<TBase_Model> where TBase_Model : Base_Model
    {
        protected DateTime start_date = new DateTime(2000, 1, 1);

        public virtual TBase_Model GetModelByID(int ID)
        {
            try
            {

                string sql = "SELECT * FROM " + this.GetViewName() + "  WHERE ID = :ID and IsDel != 2";

                OracleParameter[] param = new OracleParameter[] {
                    new OracleParameter(":ID", ID) 
                };
                using (OracleDataReader reader = OracleDBHelper.ExecuteReader(CommandType.Text, sql, param))
                {
                    if (reader.Read())
                    {
                        return ToModel(reader);
                    }
                    else
                    {
                        throw new Exception("在视图" + this.GetViewName() + "获取数据失败！");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GetDateFromFilter(TBase_Model model)
        {
            return " >=" + model.DateFrom.ToDateTime().Date.ToOracleTimeString() + "  ";
        }

        public string GetDateToFilter(TBase_Model model)
        {
            return " <= " + model.DateTo.ToDateTime().AddDays(1).Date.ToOracleTimeString() + " ";
        }



        public bool SaveModelToDB(User.UserModel user, ref TBase_Model model, ref string strError)
        {
            try
            {
                bool succ = false;
                //int iRows = 0;
                int iOut = 0;

                //把creater、createTime、modifyer等信息填到model中去.
                AddModelOperatorInfo(user, ref model);

                OracleParameter[] param = GetSaveModelOracleParameter(model);

                int i = OracleDBHelper.RunProcedure(this.GetSaveProcedureName(), param, out iOut);
                if (i == -1)//(int)param[0].Value == -1
                {
                    strError = param[1].Value.ToString();
                }
                else
                {
                    succ = true;
                    int modelID = Convert.ToInt32(param[2].Value);
                    //model.ID = (int)param[2].Value;
                    model = GetModelByID(modelID);
                    if (model == null)
                    {
                        throw new Exception("GetModelByID(modelID)出错，没有找到相符的记录，可能是视图有错误！");
                    }
                }
                return succ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public bool SaveModelBySqlToDB(User.UserModel user, ref TBase_Model model, ref string strError)
        {
            try
            {
                List<string> lstSql = new List<string>();

                int i = OracleDBHelper.ExecuteNonQueryList(this.GetSaveSql(user, ref model), ref strError);

                if (i > 0)
                {
                    model = GetModelByID(model.ID);
                    if (model == null)
                    {
                        strError = "GetModelByID(modelID)出错，没有找到相符的记录，可能是视图有错误！";
                        return false;
                    }
                    else { return true; }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
                //throw new Exception(ex.Message);
            }
        }


        public bool SaveModelBySqlToDB2(User.UserModel user, TBase_Model model, ref string strError)
        {
            try
            {
                List<string> lstSql = new List<string>();

                int i = OracleDBHelper.ExecuteNonQueryList(this.GetSaveSql(user, ref model), ref strError);

                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
                //throw new Exception(ex.Message);
            }
        }

        public bool SaveModelListBySqlToDB(User.UserModel user, ref List<TBase_Model> modelList, ref string strError)
        {
            try
            {
                List<string> lstSql = new List<string>();

                lstSql = this.GetSaveModelListSql(user, modelList);

                if (lstSql == null || lstSql.Count == 0)
                {
                    return true;
                }

                int i = OracleDBHelper.ExecuteNonQueryList(lstSql, ref strError);

                if (i > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
                //throw new Exception(ex.Message);
            }
        }

        public bool SaveModelListBySqlToDB(List<string> lstSql, ref string strError)
        {
            try
            {
                int i = OracleDBHelper.ExecuteNonQueryList(lstSql, ref strError);

                if (i > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
                //throw new Exception(ex.Message);
            }
        }


        public bool TransferModelByProcedure(User.UserModel user, ref TBase_Model model, string ProcedureName, OracleParameter[] param, ref string strError)
        {
            try
            {
                bool succ = false;
                //int iRows = 0;
                int iOut = 0;

                //把creater、createTime、modifyer等信息填到model中去.
                AddModelOperatorInfo(user, ref model);

                int i = OracleDBHelper.RunProcedure(ProcedureName, param, out iOut);
                if ((int)param[0].Value == -1)
                {
                    strError = param[1].Value.ToString();
                }
                else
                {
                    succ = true;
                    int modelID = (int)param[2].Value;
                    //model.ID = (int)param[2].Value;
                    model = GetModelByID(modelID);
                    if (model == null)
                    {
                        throw new Exception("GetModelByID(modelID)出错，没有找到相符的记录，可能是视图有错误！");
                    }
                }
                return succ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool SaveModelListToDB(User.UserModel user, List<TBase_Model> modelList, ref string strError)
        {
            try
            {
                bool succ = false;
                List<OracleParameter[]> ParameterList = new List<OracleParameter[]>();

                //把creater、createTime、modifyer等信息填到model中去.

                for (int i = 0; i < modelList.Count; i++)
                {
                    TBase_Model model = modelList[i];
                    AddModelOperatorInfo(user, ref model);

                    OracleParameter[] param = GetSaveModelOracleParameter(model);
                    ParameterList.Add(param);
                }

                succ = OracleDBHelper.RunProcedures(this.GetSaveProcedureName(), ParameterList);

                return succ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //public bool SaveModelAndDeatilListToDB(User.UserModel user, TBase_Model model, List<TBase_DetailModel> modelList, ref string strError)
        //{
        //    try
        //    {
        //        bool succ = false;

        //        Dictionary<string, OracleParameter[]> ParameterList = new Dictionary<string, OracleParameter[]>();
        //        //把creater、createTime、modifyer等信息填到model中去.

        //        OracleParameter[] paramModel = GetSaveModelOracleParameter(model);
        //        ParameterList.Add(GetModelSqlPara(GetTableName()), paramModel);

        //        for (int i = 0; i < modelList.Count; i++)
        //        {
        //            TBase_DetailModel modelDetail = modelList[i];
        //            //AddModelOperatorInfo(user, ref model);

        //            OracleParameter[] param = GetSaveModelDetailsOracleParameter(modelDetail);
        //            ParameterList.Add(GetModelSqlPara(GetTableNameDetail()), param);
        //        }

        //        succ = OracleDBHelper.RunSqls(ParameterList);

        //        return succ;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}


        private string GetModelSqlPara(string tableName)
        {
            try
            {
                List<string> lstSqlPara = new List<string>();
                string sqlPara = "insert into " + tableName;
                string fields = string.Empty;
                string values = string.Empty;

                string sql = "select column_name from user_tab_columns where table_name=upper('" + tableName + "')";


                using (OracleDataReader reader = OracleDBHelper.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        lstSqlPara.Add(reader["column_name"].ToString());
                    }
                }

                if (lstSqlPara == null)
                {
                    return string.Empty;
                }

                foreach (string item in lstSqlPara)
                {
                    if (!item.Equals("CREATEDATE") && !item.Equals("QUALITYDATE") && !item.Equals("PRINTTIME"))
                    {
                        fields += item + ",";
                        values += ":" + item + ",";
                    }

                }

                sqlPara += "(" + fields.TrimEnd(',') + ")" + " values " + "(" + values.TrimEnd(',') + ")";

                return sqlPara;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual int GetUpdateModelStatusID(User.UserModel user, TBase_Model model, int NewStatus)
        {
            return model.ID;
        }


        public bool UpdateModelListBySqlToDB(User.UserModel user, ref List<TBase_Model> modelList, ref string strError)
        {
            try
            {
                List<string> lstSql = new List<string>();

                lstSql = this.GetUpdateModelListSql(user, modelList);

                if (lstSql == null || lstSql.Count == 0)
                {
                    return true;
                }

                int i = OracleDBHelper.ExecuteNonQueryList(lstSql, ref strError);

                if (i > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
                //throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <param name="NewStatus"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool UpdateModelStatus(User.UserModel user, ref  TBase_Model model, int NewStatus, ref  string strError, bool NeedReturnModel)
        {
            try
            {

                int iRows = 0;
                int iOut = 0;

                OracleParameter[] param = new OracleParameter[]{
                    new OracleParameter("@Rows", iRows),
                    new OracleParameter("@ErrorMsg",OracleDbType.NVarchar2,100),
                    new OracleParameter("@ID", OracleDBHelper.ToDBValue(GetUpdateModelStatusID(user,model, NewStatus))),
               
                    new OracleParameter("@NewStatus", OracleDBHelper.ToDBValue(NewStatus)),
                    new OracleParameter("@Auditor", OracleDBHelper.ToDBValue(user.UserNo)),
                    new OracleParameter("@AuditorTime", OracleDBHelper.ToDBValue(DateTime.Now)),
                    new OracleParameter("@TerminateReasonID", OracleDBHelper.ToDBValue(model.TerminateReasonID)),
                    new OracleParameter("@TerminateReason", OracleDBHelper.ToDBValue(model.TerminateReason)),
                    new OracleParameter("@RowVersion", OracleDBHelper.ToDBValue(DateTime.Now))
                };

                param[0].Direction = System.Data.ParameterDirection.Output;
                param[1].Direction = System.Data.ParameterDirection.Output;

                int i = OracleDBHelper.RunProcedure(this.GetUpdateStatusProcedureName(), param, out iOut);
                strError = param[1].Value.ToString();
                if ((int)param[0].Value == -1)
                {
                    return false;
                }
                else
                {
                    if (NeedReturnModel)
                    {
                        model = GetModelByID(model.ID);
                        if (model == null)
                        {
                            throw new Exception("GetModelByID(modelID)出错，没有找到相符的记录，可能是视图有错误！");
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public bool UpdateModelListStatusBySql(List<string> sqlList, ref  string strError)
        {
            try
            {
                List<string> lstSql = new List<string>();

                int i = OracleDBHelper.ExecuteNonQueryList(sqlList, ref strError);

                if (i > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
                //throw new Exception(ex.Message);
            }
        }

        private void AddModelOperatorInfo(User.UserModel user, ref TBase_Model model)
        {
            model.RowVersion = DateTime.Now;
            if (model.ID <= 0)
            {
                model.Creater = user.UserNo;
                model.CreateTime = DateTime.Now;
            }
            else
            {
                model.Modifyer = user.UserNo;
                model.ModifyTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 通过过滤条件得到model
        /// </summary>
        /// <param name="strFilter"></param>
        /// <returns></returns>
        public TBase_Model GetModelByFilter(string strFilter)
        {
            try
            {

                string sql = "SELECT  * FROM " + this.GetViewName() + "  WHERE IsDel != 2 and  " + strFilter;


                using (OracleDataReader reader = OracleDBHelper.ExecuteReader(sql))
                {

                    if (reader.Read())
                    {
                        return ToModel(reader);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 通过SQL得到modelList
        /// </summary>
        /// <param name="strFilter"></param>
        /// <returns></returns>
        public List<TBase_Model> GetModelListBySql(string strSql)
        {
            try
            {
                using (OracleDataReader reader = OracleDBHelper.ExecuteReader(strSql))
                {
                    return ToModels(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 通过sql获取单个对象
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public TBase_Model GetModelBySql(TBase_Model mdoel)
        {
            try
            {
                using (OracleDataReader reader = OracleDBHelper.ExecuteReader(GetModelSql(mdoel)))
                {
                    if (reader.Read())
                    {
                        return ToModel(reader);
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public OracleDataReader GetRowBySql(string sql)
        {
            OracleDataReader reader = null;
            try
            {
                reader = OracleDBHelper.ExecuteReader(sql);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reader;
        }


        /// <summary>
        /// 执行SQL返回第一条记录的第一列
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public object GetScalarBySql(string strSql)
        {
            try
            {
                return OracleDBHelper.ExecuteScalar(CommandType.Text, strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public int GetExecuteNonQuery(string strSql)
        {
            try
            {
                return OracleDBHelper.ExecuteNonQuery2(CommandType.Text, strSql).ToInt32();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        /// <summary>
        /// 获取新的单据号
        /// </summary>
        /// <returns></returns>
        public string GetNewOrderNo(string strPrex)
        {
            string strNewNo = strPrex + DateTime.Now.ToString("yyMMdd");

            try
            {

                string strSql = "select isnull(max(" + GetOrderNoFieldName() + "),'') as no from " + GetTableName() + " where " + GetOrderNoFieldName() + " like '" + strNewNo + "%'";


                using (OracleDataReader reader = OracleDBHelper.ExecuteReader(strSql))
                {
                    if (reader.Read())
                    {
                        string strNo = reader["no"].ToString();
                        if (strNo == "")
                        {
                            strNo = "001";
                        }
                        else
                        {
                            int iNumber = int.Parse(strNo.Substring(strNewNo.Length));
                            iNumber++;
                            strNo = iNumber.ToString().PadLeft(3, '0');

                        }
                        strNewNo += strNo;
                        return strNewNo;
                    }
                    else
                    {
                        throw new Exception("取单据号出错！" + strNewNo);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }


        public int GetTableID(string strSeq)
        {
            try
            {
                string strSql = "select " + strSeq + ".Nextval  from dual";

                using (OracleDataReader reader = OracleDBHelper.ExecuteReader(strSql))
                {
                    if (reader.Read())
                    {
                        int ID = reader["Nextval"].ToInt32();
                        return ID;
                    }
                    else
                    {
                        throw new Exception("取单据ID出错！");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        /// <summary>
        /// 获取新的单据号
        /// </summary>
        /// <returns></returns>
        public string GetNewOrderNo(string strPrex, int iPadLeft)
        {
            string strNewNo = strPrex;

            try
            {

                string strSql = "select isnull(max(" + GetOrderNoFieldName() + "),'') as no from " + GetTableName() + " where " + GetOrderNoFieldName() + " like '" + strNewNo + "%'";


                using (OracleDataReader reader = OracleDBHelper.ExecuteReader(strSql))
                {
                    if (reader.Read())
                    {
                        string strNo = reader["no"].ToString();
                        if (strNo == "")
                        {
                            strNo = "1".PadLeft(iPadLeft, '0');
                        }
                        else
                        {
                            long iNumber = long.Parse(strNo.Substring(strNewNo.Length));
                            iNumber++;
                            strNo = iNumber.ToString().PadLeft(iPadLeft, '0');

                        }
                        strNewNo += strNo;
                        return strNewNo;
                    }
                    else
                    {
                        throw new Exception("取单据号出错！" + strNewNo);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        /// <summary>
        /// 获取新的树编号
        /// </summary>
        /// <returns></returns>
        public string GetNewTreeNo()
        {
            string strNewNo = "";

            try
            {

                string strSql = "select REPLICATE('0',10-LEN(ISNULL(MAX(ID),0)+1))+CONVERT(CHAR,ISNULL(MAX(ID),0)+1) as no from " + GetTableName() + "";


                using (OracleDataReader reader = OracleDBHelper.ExecuteReader(strSql))
                {
                    if (reader.Read())
                    {
                        strNewNo = reader["no"].ToString();
                        if (strNewNo == "")
                        {
                            strNewNo = "0000000001";
                        }
                        return strNewNo;
                    }
                    else
                    {
                        throw new Exception("取树编号出错！" + strNewNo);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        /// <param name="user"></param>
        /// <param name="iD"></param>
        /// <returns></returns>
        public bool DeleteModelByID(User.UserModel user, int iD)
        {
            return false;
        }

        public virtual bool DeleteModelByModel(User.UserModel user, TBase_Model model, ref string strError)
        {
            try
            {

                int bResult = 0;
                int iOut = 0;

                OracleParameter[] param = new OracleParameter[]{
                    new OracleParameter("@bResult", bResult),
                    new OracleParameter("@ErrorMsg", OracleDbType.NVarchar2,100),
                    new OracleParameter("@ID", OracleDBHelper.ToDBValue(model.ID)),
               
                    new OracleParameter("@Deleter", OracleDBHelper.ToDBValue(user.UserNo))
                };

                param[0].Direction = System.Data.ParameterDirection.Output;
                param[1].Direction = System.Data.ParameterDirection.Output;

                int i = OracleDBHelper.RunProcedure(this.GetDeleteProcedureName(), param, out iOut);
                strError = param[1].Value.ToString();
                if (i == -1)//(int)param[0].Value == -1
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public virtual bool DeleteModelByModelSql(User.UserModel user, TBase_Model model, ref string strError)
        {
            try
            {
                List<string> lstSql = new List<string>();

                int i = OracleDBHelper.ExecuteNonQueryList(this.GetDeleteSql(user, model), ref strError);

                if (i > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
                //throw new Exception(ex.Message);
            }
        }

        public virtual bool UpdateModelByModelSql(User.UserModel user, TBase_Model model, ref string strError)
        {
            try
            {
                List<string> lstSql = new List<string>();

                int i = OracleDBHelper.ExecuteNonQueryList(this.GetUpdateSql(user, model), ref strError);

                if (i > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
                //throw new Exception(ex.Message);
            }
        }

        public virtual bool CanDelModel(User.UserModel user, int ID, ref string strError)
        {
            return true;
        }




        /// <summary>
        /// 获取根据头表ID获取detail表的所有记录
        /// </summary>
        public virtual List<TBase_Model> GetModelListByHeaderID(int headerID)
        {
            try
            {
                string sql = "SELECT *  FROM " + GetViewName() + " where IsDel != 2 and ";
                sql += GetHeaderIDFieldName() + " = " + headerID.ToString() + "  " + GetDetailListOrderBySql();
                using (OracleDataReader reader = OracleDBHelper.ExecuteReader(sql))
                {
                    return ToModels(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取根据头表ID获取detail表的所有记录,没有合计行，因为上面的那个函数被子类重写了，只好加一个绝对没有合计行的函数，没写好。
        /// </summary>
        public virtual List<TBase_Model> GetModelListByHeaderIDNoSum(int headerID)
        {
            try
            {
                string sql = "SELECT *  FROM " + GetViewName() + " where IsDel != 2 and ";
                sql += GetHeaderIDFieldName() + " = " + headerID.ToString() + "  " + GetDetailListOrderBySql();
                using (OracleDataReader reader = OracleDBHelper.ExecuteReader(sql))
                {
                    return ToModels(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 将获取的多条数据转换成对象并添加到泛型集合返回
        /// </summary>
        protected List<TBase_Model> ToModels(OracleDataReader reader)
        {
            var list = new List<TBase_Model>();
            while (reader.Read())
            {
                list.Add(ToModel(reader));
            }

            for (int i = 0; i < list.Count; i++)
            {
                list[i].OrderNumber = (i + 1).ToString();
            }

            return list;
        }



        public virtual bool CanQuery(TBase_Model model)
        {
            if (model.DateFrom > model.DateTo)
            {
                throw new Exception("查询时开始时间不能大于结束时间！");
            }
            return true;
        }

        /// <summary>
        /// 分页获取modellist的记录
        /// </summary>
        public virtual List<TBase_Model> GetModelListByPage(User.UserModel user, TBase_Model model, ref Common.DividPage page)
        {
            try
            {
                CanQuery(model);

                using (OracleDataReader reader = Common.Common_DB.QueryByDividPage(ref  page, GetViewName(), GetFilterSql(user, model), GetFieldsSql(), GetOrderBySql()))
                {

                    return ToModels(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 通过查询条件得到部分或所有记录
        /// </summary>
        /// <returns></returns>
        public virtual List<TBase_Model> GetModelListByFilter(string OrderBy, string Filter, string Fields)
        {
            try
            {
                string sql = "SELECT " + Fields + "  FROM  " + GetViewName();

                if (Filter != "")
                {
                    sql += " where " + Filter;
                }

                if (OrderBy != "")
                {
                    sql += "  " + OrderBy;
                }

                using (OracleDataReader reader = OracleDBHelper.ExecuteReader(sql))
                {
                    return ToModels(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public virtual List<TBase_Model> GetModelListADF(User.UserModel user, TBase_Model model)
        {
            try
            {
                string sql = "SELECT *  FROM  " + GetViewName() + GetFilterSql(user, model);

                using (OracleDataReader reader = OracleDBHelper.ExecuteReader(sql))
                {
                    return ToModels(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool NotDBField(PropertyInfo info)
        {
            System.Attribute[] at = System.Attribute.GetCustomAttributes(info);
            if (at.Length == 0) return false;
            foreach (var item in at)
            {
                if (item is DBAttribute)
                    return ((DBAttribute)item).NotDBField;
            }
            return false;
        }
        public string GetInertSqlCache<T>(T t, string tablename)
        {
            Dictionary<string, StringBuilder> cacheinsertsqls = new Dictionary<string, StringBuilder>();
            tablename = tablename.ToLower();
            StringBuilder sb = null;
            PropertyInfo[] propertys = t.GetType().GetProperties();
            lock ("object")
            {
                if (!cacheinsertsqls.ContainsKey(tablename))
                {
                    sb = new StringBuilder();
                    sb.Append("　INSERT INTO  ");
                    sb.Append(tablename);
                    sb.Append("(");

                    foreach (PropertyInfo pi in propertys)
                    {
                        if (NotDBField(pi))
                            continue;

                        sb.Append(pi.Name);
                        sb.Append(",");

                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(")");
                    sb.Append("values");
                    sb.Append("(");
                    int i = 0;
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (NotDBField(pi))
                            continue;
                        object obj = pi.GetValue(t, null) == null ? "" : pi.GetValue(t, null);

                        switch (obj.GetType().Name.ToLower())
                        {
                            case "int32":
                                //  sb.Append(pi.GetValue(t, null).ToString());
                                sb.Append("{" + i.ToString() + "}");
                                sb.Append(",");
                                break;
                            case "decimal":
                                //  sb.Append(pi.GetValue(t, null).ToString());
                                sb.Append("{" + i.ToString() + "}");
                                sb.Append(",");
                                break;

                            case "datetime":
                                sb.Append(" TO_DATE('");
                                // sb.Append(pi.GetValue(t, null).ToString());
                                sb.Append("{" + i.ToString() + "}");
                                sb.Append("','yyyy/mm/dd hh24:mi:ss')");
                                sb.Append(",");
                                break;

                            default:
                                sb.Append("'");
                                //sb.Append(pi.GetValue(t, null) == null ? "" : pi.GetValue(t, null).ToString());
                                sb.Append("{" + i.ToString() + "}");
                                sb.Append("'");
                                sb.Append(",");
                                break;
                        }
                        i++;
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(")");
                    cacheinsertsqls[tablename] = sb;
                }
                else
                {
                    sb = cacheinsertsqls[tablename];
                }
            }
            List<object> olist = new List<object>();
            foreach (PropertyInfo pi in propertys)
            {
                if (NotDBField(pi))
                    continue;
                string obj = pi.GetValue(t, null) == null ? "" : pi.GetValue(t, null).ToString();
                obj = obj.Replace("\'", "");
                olist.Add(obj);
            }
            return string.Format(sb.ToString(), olist.ToArray());
        }


        protected virtual string GetFieldsSql()
        {
            return "*";
        }

        protected virtual string GetOrderBySql()
        {
            return "Order by ID Desc";
        }

        protected virtual string GetDetailListOrderBySql()
        {
            return "";
        }

        protected virtual string GetFilterSql(User.UserModel user, TBase_Model model)
        {
            return " where isDel != 2  ";
        }

        protected virtual string GetModelSql(TBase_Model model)
        {
            return "";
        }

        //protected virtual string GetFilterSql(string Parameter = "")
        //{
        //    return "";
        //}

        /// <summary>
        /// 得到ditail表中的HeaderID字段名
        /// </summary>
        /// <returns></returns>
        protected virtual string GetHeaderIDFieldName()
        {
            return "HeaderID";
        }

        protected virtual string GetOrderNoFieldName()
        {
            return "OrderNo";
        }

        protected virtual string GetUpdateStatusProcedureName()
        {
            return "P_";
        }

        protected virtual string GetDeleteProcedureName()
        {
            return "P_";
        }

        protected virtual List<string> GetDeleteSql(User.UserModel user, TBase_Model model)
        {
            return null;
        }

        protected virtual List<string> GetUpdateSql(User.UserModel user, TBase_Model model)
        {
            return null;
        }

        protected virtual List<string> GetUpdateModelListSql(User.UserModel user, List<TBase_Model> modelList)
        {
            return null;
        }

        protected virtual List<string> GetSaveModelListSql(User.UserModel user, List<TBase_Model> modelList)
        {
            return null;
        }

        #region 纯虚方法

        /// <summary>
        /// 得到保存的OracleParameter数组
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected abstract OracleParameter[] GetSaveModelOracleParameter(TBase_Model model);

        //protected abstract OracleParameter[] GetSaveModelDetailsOracleParameter(TBase_DetailModel model);

        /// <summary>
        /// 把OracleDataReader转为实体类
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected abstract TBase_Model ToModel(OracleDataReader reader);

        /// <summary>
        /// 得到查询的视图或表名
        /// </summary>
        /// <returns></returns>
        protected abstract string GetViewName();




        /// <summary>
        /// 得到表名
        /// </summary>
        /// <returns></returns>
        protected abstract string GetTableName();

        //protected abstract string GetTableNameDetail();


        /// <summary>
        /// 得到保存的存储过程名
        /// </summary>
        /// <returns></returns>
        protected abstract string GetSaveProcedureName();


        protected abstract List<string> GetSaveSql(User.UserModel user, ref TBase_Model model);

        #endregion

    }
}
