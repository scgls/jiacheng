using BILBasic.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILBasic.Basing.Factory
{
    public abstract class Base_DB<TBase_Model> where TBase_Model : Base_Model
    {
      //  public static  IDBFactory dbFactory.dbF;
        public  static  DbFactory dbFactory = new DbFactory(DbFactory.DbFactoryType.SQLSERVER);
        protected DateTime start_date = new DateTime(2000, 1, 1);
        Common.Common_FactoryDB common_FactoryDB = new Common_FactoryDB();
            public virtual TBase_Model GetModelByID(int ID)
            {
                try
                {

                   // string sql = "SELECT * FROM " + this.GetViewName() + "  WHERE ID = ID and IsDel != 2";
                string sql = "SELECT * FROM " + this.GetViewName() + "  WHERE ID = '" + ID + "' and IsDel != 2";
                //dbFactory.dbF.CreateParameters(1);
                //dbFactory.dbF.AddParameters(0, " ID", ID,0);
                    using (IDataReader reader = dbFactory.ExecuteReader(CommandType.Text, sql))
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

                    IDataParameter[] param = GetSaveModelIDataParameter(model);

                    int i = dbFactory.RunProcedure(this.GetSaveProcedureName(), param, out iOut);
                    if (i == -1)//(int)param[0].Value == -1
                    {
                        strError = param[1].Value.ToString();
                    }
                    else
                    {
                        succ = true;
                        int modelID = Convert.ToInt32(((DbParameter)param[2]).Value);
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

                    int i = dbFactory.ExecuteNonQueryList(this.GetSaveSql(user, ref model), ref strError);

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

                    int i = dbFactory.ExecuteNonQueryList(lstSql, ref strError);

                    if (i > 0)
                    {
                        return true;
                    }
                    else { return false; }
                }
                catch (Exception ex)
                {
                    LogNet.LogInfo("SaveModelListBySqlToDB:" + ex.Message);
                    strError = ex.Message;
                    return false;
                    //throw new Exception(ex.Message);
                }
            }

            public bool SaveModelListBySqlToDB(List<string> lstSql, ref string strError)
            {
                try
                {
                    int i = dbFactory.ExecuteNonQueryList(lstSql, ref strError);

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


            public bool TransferModelByProcedure(User.UserModel user, ref TBase_Model model, string ProcedureName, IDataParameter[] param, ref string strError)
            {
                try
                {
                    bool succ = false;
                    //int iRows = 0;
                    int iOut = 0;

                    //把creater、createTime、modifyer等信息填到model中去.
                    AddModelOperatorInfo(user, ref model);

                    int i = dbFactory.RunProcedure(ProcedureName, param, out iOut);
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
                    List<IDataParameter[]> ParameterList = new List<IDataParameter[]>();

                    //把creater、createTime、modifyer等信息填到model中去.

                    for (int i = 0; i < modelList.Count; i++)
                    {
                        TBase_Model model = modelList[i];
                        AddModelOperatorInfo(user, ref model);

                        IDataParameter[] param = GetSaveModelIDataParameter(model);
                        ParameterList.Add(param);
                    }

                    succ = dbFactory.RunProcedures(this.GetSaveProcedureName(), ParameterList);

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

            //        Dictionary<string, IDataParameter[]> ParameterList = new Dictionary<string, IDataParameter[]>();
            //        //把creater、createTime、modifyer等信息填到model中去.

            //        IDataParameter[] paramModel = GetSaveModelIDataParameter(model);
            //        ParameterList.Add(GetModelSqlPara(GetTableName()), paramModel);

            //        for (int i = 0; i < modelList.Count; i++)
            //        {
            //            TBase_DetailModel modelDetail = modelList[i];
            //            //AddModelOperatorInfo(user, ref model);

            //            IDataParameter[] param = GetSaveModelDetailsIDataParameter(modelDetail);
            //            ParameterList.Add(GetModelSqlPara(GetTableNameDetail()), param);
            //        }

            //        succ = dbFactory.RunSqls(ParameterList);

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


                    using (IDataReader reader = dbFactory.ExecuteReader(sql))
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

                    int i = dbFactory.ExecuteNonQueryList(lstSql, ref strError);

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
            public bool UpdateModelStatus(User.UserModel user, ref TBase_Model model, int NewStatus, ref string strError, bool NeedReturnModel)
            {
                try
                {
                    int iRows = 0;
                    int iOut = 0;
                dbFactory.dbF.CreateParameters(9);
                dbFactory.dbF.AddParameters(0, "@Rows", iRows,0);
                //   dbFactory.dbF.AddParameters(1, "ErrorMsg", OracleDbType.NVarchar2,100);
                dbFactory.dbF.AddParameters(2,"ID" ,dbFactory.ToDBValue(GetUpdateModelStatusID(user, model, NewStatus)),0);
                dbFactory.dbF.AddParameters(3, "@NewStatus", dbFactory.ToDBValue(NewStatus), 0);
                dbFactory.dbF.AddParameters(4, "@Auditor", dbFactory.ToDBValue(user.UserNo), 0);
                dbFactory.dbF.AddParameters(5, "@AuditorTime", dbFactory.ToDBValue(DateTime.Now), 0);
                dbFactory.dbF.AddParameters(6, "@TerminateReasonID", dbFactory.ToDBValue(model.TerminateReasonID),0);
                dbFactory.dbF.AddParameters(7, "@TerminateReason", dbFactory.ToDBValue(model.TerminateReason),0);
                dbFactory.dbF.AddParameters(8, "@RowVersion", dbFactory.ToDBValue(DateTime.Now), 0);

                dbFactory.dbF.Parameters[0].Direction = System.Data.ParameterDirection.Output;
                dbFactory.dbF.Parameters[1].Direction = System.Data.ParameterDirection.Output;

                    int i = dbFactory.RunProcedure(this.GetUpdateStatusProcedureName(), dbFactory.dbF.Parameters, out iOut);
                    strError = dbFactory.dbF.Parameters[1].Value.ToString();
                    if ((int)dbFactory.dbF.Parameters[0].Value == -1)
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


            public bool UpdateModelListStatusBySql(List<string> sqlList, ref string strError)
            {
                try
                {
                    List<string> lstSql = new List<string>();

                    int i = dbFactory.ExecuteNonQueryList(sqlList, ref strError);

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


                    using (IDataReader reader = dbFactory.ExecuteReader(sql))
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
                    using (IDataReader reader = dbFactory.ExecuteReader(strSql))
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
                    using (IDataReader reader = dbFactory.ExecuteReader(GetModelSql(mdoel)))
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

            public IDataReader GetRowBySql(string sql)
            {
                IDataReader reader = null;
                try
                {
                    reader = dbFactory.ExecuteReader(sql);

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
                    return dbFactory.ExecuteScalar(CommandType.Text, strSql);
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
                    return dbFactory.ExecuteNonQuery2(CommandType.Text, strSql).ToInt32();
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


                    using (IDataReader reader = dbFactory.ExecuteReader(strSql))
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

            public int GetTableIDBySqlServer(string strSeq)
            {
                try
                {
                    string strSql = "SELECT IDENT_CURRENT('" + strSeq + "')";


                    int ID = dbFactory.ExecuteScalar(CommandType.Text, strSql).ToInt32();
                    // ID = ID + 1;
                    strSql = "DBCC   CHECKIDENT   ('" + strSeq + "',   RESEED, " + (ID + 1) + ")";

                    GetExecuteNonQuery(strSql);

                    // ID = ID + 1;

                    return ID;
                    
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }


            }

            public string GetNewOrderNoSql(string strPrex, string tableName)
            {
                //var seed = Guid.NewGuid().GetHashCode();
                //string strB=new Random(seed).Next(0, 9999).ToString().PadLeft(4, '0');
                string strNewNo = strPrex + DateTime.Now.ToString("yyMMdd");

                string strSql = "select ( case isnull(max(" + GetOrderNoFieldName() + "),'') " +
                            "when '' then '" + strNewNo + "'+'00001' else '" + strNewNo + "' + RIGHT('00000'+CAST((CAST(SUBSTRING(isnull(max(" + GetOrderNoFieldName() + "),''),8,5) as int) + 1 ) as varchar(5)),5) end )" +
                            "from " + tableName + " where " + GetOrderNoFieldName() + " like '" + strNewNo + "%'";
                return strSql;
            }

            public int GetTableIDBySqlServerTaskTrans(string strSeq)
            {
                try
                {
                    string strSql = "SELECT IDENT_CURRENT('" + strSeq + "')";


                    int ID = dbFactory.ExecuteScalar(CommandType.Text, strSql).ToInt32();
                    // ID = ID + 1;
                    strSql = "DBCC   CHECKIDENT   ('" + strSeq + "',   RESEED, " + (ID + 1) + ")";

                    GetExecuteNonQuery(strSql);

                    //修改t_tasktrsans 取ID有问题的BUG，报ID重复
                    ID = ID + 1;

                    return ID;

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }


            }


            public string GetNewOrderNo(string strPrex, string tableName, string wmsHeadVourcherNo)
            {

                string strNewNo =  strPrex + DateTime.Now.ToString("yyMMdd");
                string strNo = wmsHeadVourcherNo;
                try
                {
                    if (String.IsNullOrEmpty(wmsHeadVourcherNo))
                    {
                        string strSql = "select isnull(max(" + GetOrderNoFieldName() + "),'') as no from " + tableName + " where " + GetOrderNoFieldName() + " like '" + strNewNo + "%'";
                        using (IDataReader reader = dbFactory.ExecuteReader(strSql))
                        {
                            if (reader.Read())
                            {
                                strNo = reader["no"].ToString();
                                if (strNo == "")
                                {
                                    strNo = strNewNo + "00000";
                                }
                            }
                            else
                            {
                                throw new Exception("取单据号出错！" + strNewNo);
                            }
                        }
                    }
                    int iNumber = int.Parse(strNo.Substring(strNewNo.Length));
                    iNumber++;
                    strNo = iNumber.ToString().PadLeft(5, '0');
                    strNewNo += strNo;
                    return strNewNo;
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

                    using (IDataReader reader = dbFactory.ExecuteReader(strSql))
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


                    using (IDataReader reader = dbFactory.ExecuteReader(strSql))
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


                    using (IDataReader reader = dbFactory.ExecuteReader(strSql))
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
                dbFactory.dbF.CreateParameters(4);
                dbFactory.dbF.AddParameters(0, "@bResult", bResult, 0);
                //   dbFactory.dbF.AddParameters(1, "@ErrorMsg", OracleDbType.NVarchar2, 100);
                dbFactory.dbF.AddParameters(2, "@ID", dbFactory.ToDBValue(model.ID),100);
                dbFactory.dbF.AddParameters(3, "@Deleter", dbFactory.ToDBValue(user.UserNo),0);

                dbFactory.dbF.Parameters[0].Direction = System.Data.ParameterDirection.Output;
                dbFactory.dbF.Parameters[1].Direction = System.Data.ParameterDirection.Output;

                    int i = dbFactory.RunProcedure(this.GetDeleteProcedureName(), dbFactory.dbF.Parameters, out iOut);
                    strError = dbFactory.dbF.Parameters[1].Value.ToString();
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

                    int i = dbFactory.ExecuteNonQueryList(this.GetDeleteSql(user, model), ref strError);

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

                    int i = dbFactory.ExecuteNonQueryList(this.GetUpdateSql(user, model), ref strError);

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
                    using (IDataReader reader = dbFactory.ExecuteReader(sql))
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
                    using (IDataReader reader = dbFactory.ExecuteReader(sql))
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
            protected List<TBase_Model> ToModels(IDataReader reader)
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
            /// 分页获取modellist的记录  condition:多条件的情况下
            /// </summary>
            public virtual List<TBase_Model> GetModelListByPage(User.UserModel user, TBase_Model model, ref Common.DividPage page,string condition="")
            {
                try
                {
                    CanQuery(model);

                using (IDataReader reader = common_FactoryDB.QueryByDividPage(ref page, GetViewName(), GetFilterSql(user, model), GetFieldsSql(), GetOrderBySql(),condition))
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

                    using (IDataReader reader = dbFactory.ExecuteReader(sql))
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

                    using (IDataReader reader = dbFactory.ExecuteReader(sql))
                    {
                        return ToModels(reader);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
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
                return "VoucherNo";
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
            /// 得到保存的IDataParameter数组
            /// </summary>
            /// <param name="model"></param>
            /// <returns></returns>
            protected abstract IDataParameter[] GetSaveModelIDataParameter(TBase_Model model);

            //protected abstract IDataParameter[] GetSaveModelDetailsIDataParameter(TBase_DetailModel model);

            /// <summary>
            /// 把IDataReader转为实体类
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected abstract TBase_Model ToModel(IDataReader reader);

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

