//************************************************************
//******************************作者：方颖*********************
//******************************时间：2019/9/5 16:39:45*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using BILBasic.Common;
using BILBasic.User;
using System.Data;

namespace BILWeb.LandMark
{
    public partial class T_LandMarkWithTask_DB : BILBasic.Basing.Factory.Base_DB<T_LandMarkWithTaskInfo>
    {

        /// <summary>
        /// 添加t_landmarkwithtask
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_LandMarkWithTaskInfo t_landmarkwithtask)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_LandMarkWithTaskInfo t_landmarkwithtask)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_LandMarkWithTaskInfo ToModel(IDataReader reader)
        {
            T_LandMarkWithTaskInfo t_landmarkwithtask = new T_LandMarkWithTaskInfo();

            t_landmarkwithtask.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_landmarkwithtask.LandMarkID = (decimal?)dbFactory.ToModelValue(reader, "LANDMARKID");
            t_landmarkwithtask.CarNo = (string)dbFactory.ToModelValue(reader, "CARNO");
            t_landmarkwithtask.TaskNo = (string)dbFactory.ToModelValue(reader, "TASKNO");
            t_landmarkwithtask.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ERPVOUCHERNO");
            t_landmarkwithtask.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_landmarkwithtask.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_landmarkwithtask.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToDecimal();
            t_landmarkwithtask.Remark = (string)dbFactory.ToModelValue(reader, "REMARK");
            t_landmarkwithtask.Remark1 = (string)dbFactory.ToModelValue(reader, "REMARK1");
            t_landmarkwithtask.Remark2 = (string)dbFactory.ToModelValue(reader, "REMARK2");
            t_landmarkwithtask.Remark3 = (string)dbFactory.ToModelValue(reader, "REMARK3");
            t_landmarkwithtask.landmarkno = (string)dbFactory.ToModelValue(reader, "landmarkno");
            return t_landmarkwithtask;
        }

        protected override string GetViewName()
        {
            return "v_landmarkwithtask";
        }

        protected override string GetTableName()
        {
            return "T_LANDMARKWITHTASK";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override string GetFilterSql(UserModel user, T_LandMarkWithTaskInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";
            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " erpvoucherno like '%" + model.ErpVoucherNo.Trim() + "%' ";
            }
            if (!string.IsNullOrEmpty(model.TaskNo))
            {
                strSql += strAnd;
                strSql += " TaskNo like '%" + model.TaskNo.Trim() + "%' ";
            }
            if (!string.IsNullOrEmpty(model.CarNo))
            {
                strSql += strAnd;
                strSql += " CarNo like '%" + model.CarNo.Trim() + "%' ";
            }
            if (!string.IsNullOrEmpty(model.landmarkno))
            {
                strSql += strAnd;
                strSql += " landmarkno like '%" + model.landmarkno.Trim() + "%' ";
            }
            return strSql + " order by id desc";
        }


        public bool GetTaskForLandmark(string barcode,ref T_LandMarkWithTaskInfo model,ref string strmsg)
        {
            try
            {
                string sql = "";
                if (barcode.Contains("@"))
                {
                    //整箱
                    sql = "select t_tasktrans.taskno，t_tasktrans.erpvoucherno,t_landmark.landmarkno from t_tasktrans " +
                        "left join t_landmarkwithtask on t_tasktrans.taskno = t_landmarkwithtask.taskno " +
                         "left join t_landmark on t_landmark.id = t_landmarkwithtask.landmarkid " +

                        "where tasktype = 2 and barcode = '" + barcode + "'";
                    DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                    if (dt==null|| dt.Rows.Count==0)
                    {
                        strmsg = "没有找到所属的任务单号";
                        return false;
                    }
                    if (dt.Rows[0]["landmarkno"].ToString() != "")
                    {
                        strmsg = "扫描条码已经存在："+ dt.Rows[0]["landmarkno"].ToString();
                        return false;
                    }
                    else {
                        model.TaskNo = dt.Rows[0]["taskno"].ToString();
                        model.ErpVoucherNo = dt.Rows[0]["erpvoucherno"].ToString();
                    }
                }
                else
                {
                    //零头
                    sql = "select t_pickcar.carno,t_pickcar.taskno,t_task.erpvoucherno,t_landmark.landmarkno from  t_pickcar " +
                   "left join t_task on t_pickcar.taskno = t_task.taskno " +
                   "left join t_landmarkwithtask on t_pickcar.taskno = t_landmarkwithtask.taskno " +
                   "left join t_landmark on t_landmark.id = t_landmarkwithtask.landmarkid " +
                   "where t_pickcar.carno = '" + barcode + "' ";
                    DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        strmsg = "没有找到所属的任务单号";
                        return false;
                    }
                    if (dt.Rows[0]["landmarkno"].ToString() != "")
                    {
                        strmsg = "扫描小车已经存在：" + dt.Rows[0]["landmarkno"].ToString();
                        return false;
                    }
                    else
                    {
                        model.TaskNo = dt.Rows[0]["taskno"].ToString();
                        model.ErpVoucherNo = dt.Rows[0]["erpvoucherno"].ToString();
                        model.CarNo = dt.Rows[0]["carno"].ToString();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                strmsg = ex.ToString();
                return false;
            }

        }

        public bool SaveTaskwithandmark(T_LandMarkWithTaskInfo landMarkWithTaskInfo, UserModel user, ref string strError)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;
            int ID = base.GetTableID("SEQ_LANDMARK_ID");
            strSql = "insert into t_Landmarkwithtask(id,landmarkid,carno,taskno,erpvoucherno,Creater,Createtime,isdel,remark,remark1,remark2,remark3)" +
                     " values("+ID+ ",'{0}', '{1}', '{2}','{3}', '{4}',SYSDATE,1, '', '', '', '')";
            strSql = string.Format(strSql, landMarkWithTaskInfo.LandMarkID, landMarkWithTaskInfo.CarNo, landMarkWithTaskInfo.TaskNo, landMarkWithTaskInfo.ErpVoucherNo, user.UserName);
            lstSql.Add(strSql);
            return base.SaveModelListBySqlToDB(lstSql, ref strError);

        }


        public void DelTaskwithandmark(string erpvoucherno,ref string strmsg)
        {
            try
            {
                string strSql = string.Empty;
                strSql = "delete t_Landmarkwithtask where erpvoucherno='" + erpvoucherno + "'";
                base.GetExecuteNonQuery(strSql);
            }
            catch (Exception ex)
            {
                strmsg=ex.ToString();
            }


        }


    }
}
