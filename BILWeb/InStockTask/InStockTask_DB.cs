
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;
using BILBasic.DBA;
using BILBasic.User;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BILWeb.InStockTask
{
    public partial class T_InStockTask_DB : BILBasic.Basing.Factory.Base_DB<T_InStockTaskInfo>
    {

        /// <summary>
        /// 添加t_task
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_InStockTaskInfo t_task)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user,ref  T_InStockTaskInfo t_task)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_InStockTaskInfo ToModel(IDataReader reader)
        {
            T_InStockTaskInfo t_task = new T_InStockTaskInfo();

            t_task.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_task.VoucherType = dbFactory.ToModelValue(reader, "VOUCHERTYPE").ToInt32();
            t_task.TaskType =  dbFactory.ToModelValue(reader, "TASKTYPE").ToInt32();
            t_task.TaskNo = (string)dbFactory.ToModelValue(reader, "TASKNO");
            t_task.SupcusName = (string)dbFactory.ToModelValue(reader, "SUPCUSNAME");
            t_task.Status = dbFactory.ToModelValue(reader, "Status").ToInt32();
            t_task.AuditUserNo = (string)dbFactory.ToModelValue(reader, "AUDITUSERNO");
            t_task.AuditDateTime = (DateTime?)dbFactory.ToModelValue(reader, "AUDITDATETIME");
            t_task.TaskIssued = (DateTime?)dbFactory.ToModelValue(reader, "TASKISSUED");
            t_task.ReceiveUserNo = (string)dbFactory.ToModelValue(reader, "RECEIVEUSERNO");
            t_task.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_task.Remark = (string)dbFactory.ToModelValue(reader, "REMARK");
            t_task.Reason = (string)dbFactory.ToModelValue(reader, "REASON");
            t_task.SupcusCode = (string)dbFactory.ToModelValue(reader, "SupcusCode");
            t_task.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_task.IsShelvePost = dbFactory.ToModelValue(reader, "ISSHELVEPOST").ToInt32();
            t_task.InStockID = dbFactory.ToModelValue(reader, "InStockID").ToInt32();
            t_task.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ErpVoucherNo");
            t_task.IsQuality = dbFactory.ToModelValue(reader, "ISQUALITY").ToInt32();
            t_task.IsReceivePost = dbFactory.ToModelValue(reader, "ISRECEIVEPOST").ToInt32();
            t_task.Plant = (string)dbFactory.ToModelValue(reader, "PLANT");
            t_task.PlanName = (string)dbFactory.ToModelValue(reader, "PLANTNAME");
            t_task.PostStatus = dbFactory.ToModelValue(reader, "POSTSTATUS").ToInt32();
            t_task.MoveType = (string)dbFactory.ToModelValue(reader, "MOVETYPE");
            t_task.IsOutStockPost = dbFactory.ToModelValue(reader, "ISOUTSTOCKPOST").ToInt32();
            t_task.IsUnderShelvePost = dbFactory.ToModelValue(reader, "ISUNDERSHELVEPOST").ToInt32();
            t_task.DepartmentCode = (string)dbFactory.ToModelValue(reader, "DEPARTMENTCODE");
            t_task.DepartmentName = (string)dbFactory.ToModelValue(reader, "DEPARTMENTNAME");
            t_task.ReviewStatus = dbFactory.ToModelValue(reader, "REVIEWSTATUS").ToInt32();
            t_task.MoveReasonCode = (string)dbFactory.ToModelValue(reader, "MOVEREASONCODE");
            t_task.MoveReasonDesc = (string)dbFactory.ToModelValue(reader, "MOVEREASONDESC");
            t_task.PrintQty = dbFactory.ToModelValue(reader, "PRINTQTY").ToInt32();
            t_task.PrintTime = (DateTime?)dbFactory.ToModelValue(reader, "PRINTTIME");
            t_task.CloseDateTime = (DateTime?)dbFactory.ToModelValue(reader, "CLOSEDATETIME");
            t_task.CloseUserNo = (string)dbFactory.ToModelValue(reader, "CLOSEUSERNO");
            t_task.CloseReason = (string)dbFactory.ToModelValue(reader, "CLOSEREASON");
            t_task.IsOwe = dbFactory.ToModelValue(reader, "ISOWE").ToInt32();
            t_task.IsUrgent = dbFactory.ToModelValue(reader, "ISURGENT").ToInt32();
            t_task.OutStockDate = (DateTime?)dbFactory.ToModelValue(reader, "OUTSTOCKDATE");
            t_task.TaskIsSuedUser = (string)dbFactory.ToModelValue(reader, "TASKISSUEDUSER");

            t_task.StrVoucherType = (string)dbFactory.ToModelValue(reader, "StrVoucherType");
            t_task.StrStatus = (string)dbFactory.ToModelValue(reader, "StrStatus");
            t_task.StrCreater = (string)dbFactory.ToModelValue(reader, "StrCreater");
            t_task.StrTaskIsSuedUser = (string)dbFactory.ToModelValue(reader, "StrTaskIsSuedUser");

            t_task.StrongHoldCode = dbFactory.ToModelValue(reader, "StrongHoldCode").ToDBString();
            t_task.StrongHoldName = dbFactory.ToModelValue(reader, "StrongHoldName").ToDBString();
            t_task.CompanyCode = dbFactory.ToModelValue(reader, "CompanyCode").ToDBString();
            t_task.ErpInVoucherNo = dbFactory.ToModelValue(reader, "ErpInVoucherNo").ToDBString();
            t_task.WareHouseNo = dbFactory.ToModelValue(reader, "WareHouseNo").ToDBString();
            t_task.WareHouseName = dbFactory.ToModelValue(reader, "WareHouseName").ToDBString();
            t_task.VouUser = dbFactory.ToModelValue(reader, "VouUser").ToDBString();

            return t_task;
        }


        protected override string GetFilterSql(UserModel user, T_InStockTaskInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";

            

            if (model.Status > 0) 
            {
                if (model.Status == 1 || model.Status == 2)
                {
                    strSql += strAnd;
                    strSql += "(isnull(status,1)=1 or isnull(status,1)=2) ";

                }
                else 
                {
                    strSql += strAnd;
                    strSql += "isnull(status,1)= '"+model.Status+"'";
                }
            }

            if (!string.IsNullOrEmpty(model.StrStatus))
            {
                string strStatus = string.Empty;
                string[] strSplit = model.StrStatus.Split('&');
                int sLen = strSplit.Length;

                foreach (var item in strSplit) 
                {
                    switch (item.Trim()) 
                    {
                        case "新建":
                            strStatus += "1" + (sLen > 1 ? "," : "");
                            break;
                        case "部分上架":
                            strStatus += "2" + (sLen > 1 ? "," : "");
                            break;
                        case "全部上架":
                            strStatus += "3" + (sLen > 1 ? "," : "");
                            break;
                        case "已关闭":
                            strStatus += "5" + (sLen > 1 ? "," : "");
                            break;
                        default:
                            strStatus += "0" + (sLen > 1 ? "," : "");
                            break;
                    }
                }
                strStatus = strStatus.TrimEnd(',');
                strSql += strAnd;
                strSql += "isnull(status,1) in (" + strStatus + ")";

            }

            if (!string.IsNullOrEmpty(model.SupcusCode))
            {
                strSql += strAnd;
                strSql += " (SUPCUSCODE Like '" + model.SupcusCode + "%'  or SUPCUSNAME Like '" + model.SupcusCode + "%' )";
            }

            if (model.DateFrom != null)
            {
                strSql += strAnd;
                strSql += " CreateTime >= " + model.DateFrom.ToDateTime().Date.ToOracleTimeString() + " ";
            }

            if (model.DateTo != null)
            {
                strSql += strAnd;
                strSql += " CreateTime <= " + model.DateTo.ToDateTime().Date.AddDays(1).ToOracleTimeString() + " ";
            }

            if (!string.IsNullOrEmpty(model.TaskNo))
            {
                strSql += strAnd;
                strSql += " (taskno Like '" + model.TaskNo + "%'  )";
            }

            if (!string.IsNullOrEmpty(model.ReceiveUserNo))
            {
                strSql += strAnd;
                strSql += " ReceiveUserNo like '"+model.ReceiveUserNo+"%'";
            }

            if (model.VoucherType > 0) 
            {
                strSql += strAnd;
                strSql += " VoucherType ='" + model.VoucherType + "'";
            }

            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " (erpvoucherno like '%" + model.ErpVoucherNo + "%' )";
            }

            //if (model.StrongHoldType == 1)
            //{
            //    strSql += strAnd;
            //    strSql += " StrongHoldCode ='CY1'";
            //}

            //if (model.StrongHoldType == 2)
            //{
            //    strSql += strAnd;
            //    strSql += " StrongHoldCode ='CX1'";
            //}

            if (!string.IsNullOrEmpty(model.MaterialNo))
            {
                strSql += strAnd;
                strSql += "  id in ( SELECT HEADERID FROM v_Taskdetail WHERE MATERIALNO LIKE '%" + model.MaterialNo + "%' ) ";
            }

            if (!string.IsNullOrEmpty(model.VouUser))
            {
                strSql += strAnd;
                strSql += "  VouUser like '" + model.VouUser + "%' ";
            }

            if (model.WareHouseID > 0 )
            {
                strSql += strAnd;
                strSql += "  WareHouseID  = '"+model.WareHouseID+"'";
            }


            if (!user.UserNo.Equals("admin"))
            {
                strSql += strAnd;
                strSql += " strongholdcode = '" + user.StrongHoldCode + "' and (fromerpwarehouse ='" + user.WarehouseCode + "' or isnull(fromerpwarehouse,'')='')";
            }

            //if (!string.IsNullOrEmpty(model.StrongHoldCode))
            //{
            //    strSql += strAnd;
            //    strSql += " (StrongHoldCode like '" + model.StrongHoldCode + "%' )";
            //}

            //if (!string.IsNullOrEmpty(model.StrongHoldName))
            //{
            //    strSql += strAnd;
            //    strSql += " (StrongHoldName like '" + model.StrongHoldName + "%' )";
            //}

            strSql += strAnd;
            strSql += " tasktype ='1' ";

            if (model.PcOrPda =="0")
            {
                strSql += strAnd;
                strSql += "  taskqty is not null ";
            }

            return strSql; //+ "order by id desc";
        }
    


        protected override string GetViewName()
        {
            return "V_TASK";
        }

        protected override string GetTableName()
        {
            return "T_TASK";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override List<string> GetUpdateSql(UserModel user, T_InStockTaskInfo model)
        {
            List<string> lstSql = new List<string>();

            string strSql = "update t_Task a set a.Status = '" + model.Status + "' where id = '" + model.ID + "'";

            lstSql.Add(strSql);

            return lstSql;
        }

        public string GetReceiveTaskNoBySerialNo(string SerialNo, ref string strError) 
        {
            string strSql = "select A.taskno from t_Tasktrans a where a.Serialno = '" + SerialNo + "' and a.Tasktype = '4'";

            return base.GetScalarBySql(strSql).ToDBString();
        }

        public override List<T_InStockTaskInfo> GetModelListADF(UserModel user, T_InStockTaskInfo model)
        {
            List<T_InStockTaskInfo> modelList = base.GetModelListADF(user, model);
            if (modelList != null && modelList.Count > 0)
            {
                modelList = modelList.Take(20).ToList();
            }

            return modelList;

        }
    }
}
