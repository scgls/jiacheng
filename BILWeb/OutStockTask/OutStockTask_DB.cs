using BILBasic.DBA;
using BILBasic.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;
using Oracle.ManagedDataAccess.Client;
using BILWeb.Login;
using System.Data;
using BILWeb.Boxing;

namespace BILWeb.OutStockTask
{
    public partial class T_OutStockTask_DB : BILBasic.Basing.Factory.Base_DB<T_OutStockTaskInfo>
    {

        /// <summary>
        /// 添加t_task
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_OutStockTaskInfo t_task)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_OutStockTaskInfo t_task)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_OutStockTaskInfo ToModel(IDataReader reader)
        {
            T_OutStockTaskInfo t_task = new T_OutStockTaskInfo();

            t_task.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_task.VoucherType = dbFactory.ToModelValue(reader, "VOUCHERTYPE").ToInt32();
            t_task.TaskType = dbFactory.ToModelValue(reader, "TASKTYPE").ToDecimal();
            t_task.TaskNo = dbFactory.ToModelValue(reader, "TASKNO").ToDBString();
            t_task.SupcusName = dbFactory.ToModelValue(reader, "SUPCUSNAME").ToDBString();
            t_task.Status = dbFactory.ToModelValue(reader, "Status").ToInt32();
            t_task.AuditUserNo = dbFactory.ToModelValue(reader, "AUDITUSERNO").ToDBString();
            t_task.AuditDateTime = (DateTime?)dbFactory.ToModelValue(reader, "AUDITDATETIME");
            t_task.TaskIssued = (DateTime?)dbFactory.ToModelValue(reader, "TASKISSUED");
            t_task.ReceiveUserNo = dbFactory.ToModelValue(reader, "RECEIVEUSERNO").ToDBString();
            t_task.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_task.Remark = dbFactory.ToModelValue(reader, "REMARK").ToDBString();
            t_task.Reason = dbFactory.ToModelValue(reader, "REASON").ToDBString();
            t_task.SupcusCode = dbFactory.ToModelValue(reader, "SupcusCode").ToDBString();
            t_task.Creater = dbFactory.ToModelValue(reader, "CREATER").ToDBString();
            t_task.IsShelvePost = dbFactory.ToModelValue(reader, "ISSHELVEPOST").ToDecimal();
            //t_task.Receive_ID = (decimal?)dbFactory.ToModelValue(reader, "RECEIVE_ID");
            t_task.ErpVoucherNo = dbFactory.ToModelValue(reader, "ERPVoucherNo").ToDBString();
            t_task.IsQuality = dbFactory.ToModelValue(reader, "ISQUALITY").ToDecimal();
            t_task.IsReceivePost = dbFactory.ToModelValue(reader, "ISRECEIVEPOST").ToDecimal();
            t_task.Plant = dbFactory.ToModelValue(reader, "PLANT").ToDBString();
            t_task.PlanName = dbFactory.ToModelValue(reader, "PLANTNAME").ToDBString();
            t_task.PostStatus = dbFactory.ToModelValue(reader, "POSTSTATUS").ToDecimal();
            t_task.MoveType = dbFactory.ToModelValue(reader, "MOVETYPE").ToDBString();
            t_task.IsOutStockPost = dbFactory.ToModelValue(reader, "ISOUTSTOCKPOST").ToDecimal();
            t_task.IsUnderShelvePost = dbFactory.ToModelValue(reader, "ISUNDERSHELVEPOST").ToDecimal();
            t_task.DepartmentCode = dbFactory.ToModelValue(reader, "DEPARTMENTCODE").ToDBString();
            t_task.DepartmentName = dbFactory.ToModelValue(reader, "DEPARTMENTNAME").ToDBString();
            t_task.ReviewStatus = dbFactory.ToModelValue(reader, "REVIEWSTATUS").ToDecimal();
            t_task.MoveReasonCode = dbFactory.ToModelValue(reader, "MOVEREASONCODE").ToDBString();
            t_task.MoveReasonDesc = dbFactory.ToModelValue(reader, "MOVEREASONDESC").ToDBString();
            t_task.PrintQty = dbFactory.ToModelValue(reader, "PRINTQTY").ToDecimal();
            t_task.PrintTime = (DateTime?)dbFactory.ToModelValue(reader, "PRINTTIME");
            t_task.CloseDateTime = (DateTime?)dbFactory.ToModelValue(reader, "CLOSEDATETIME");
            t_task.CloseUserNo = dbFactory.ToModelValue(reader, "CLOSEUSERNO").ToDBString();
            t_task.CloseReason = dbFactory.ToModelValue(reader, "CLOSEREASON").ToDBString();
            t_task.IsOwe = dbFactory.ToModelValue(reader, "ISOWE").ToDecimal();
            t_task.IsUrgent = dbFactory.ToModelValue(reader, "ISURGENT").ToDecimal();
            t_task.OutStockDate = (DateTime?)dbFactory.ToModelValue(reader, "OUTSTOCKDATE");

            t_task.StrVoucherType = dbFactory.ToModelValue(reader, "StrVoucherType").ToDBString();
            t_task.StrStatus = dbFactory.ToModelValue(reader, "StrStatus").ToDBString();
            t_task.StrCreater = dbFactory.ToModelValue(reader, "StrCreater").ToDBString();
            t_task.FloorType = dbFactory.ToModelValue(reader, "FloorType").ToInt32();
            t_task.PickGroupNo = dbFactory.ToModelValue(reader, "PickGroupNo").ToDBString();
            t_task.PickLeaderUserNo = dbFactory.ToModelValue(reader, "PickLeaderUserNo").ToDBString();
            t_task.StrPickLeaderUserNo = dbFactory.ToModelValue(reader, "StrPickLeaderUserNo").ToDBString();
            t_task.CompanyCode = dbFactory.ToModelValue(reader, "CompanyCode").ToDBString();
            t_task.StrongHoldCode = dbFactory.ToModelValue(reader, "StrongHoldCode").ToDBString();
            t_task.StrongHoldName = dbFactory.ToModelValue(reader, "StrongHoldName").ToDBString();

            t_task.WareHouseID = dbFactory.ToModelValue(reader, "WareHouseID").ToInt32();
            t_task.WareHouseName = dbFactory.ToModelValue(reader, "WareHouseName").ToDBString();
            t_task.WareHouseNo = dbFactory.ToModelValue(reader, "WareHouseNo").ToDBString();

            t_task.IsEdate = dbFactory.ToModelValue(reader, "IsEdate").ToDBString();
            t_task.PickUserNo = dbFactory.ToModelValue(reader, "PickUserNo").ToDBString();

            BILWeb.Login.User.User_DB _db = new Login.User.User_DB();
            BILWeb.Login.User.UserInfo usermodel =  _db.GetModelByFilterByUserNo(t_task.PickUserNo);
            if (usermodel != null)
            {
                t_task.PickUserName = usermodel.UserName;
            }

            t_task.FloorName = dbFactory.ToModelValue(reader, "FloorName").ToDBString();
            t_task.HeightArea = dbFactory.ToModelValue(reader, "HeightArea").ToDBString();
            t_task.HeightAreaName = dbFactory.ToModelValue(reader, "HeightAreaName").ToDBString();
            t_task.VouUser = dbFactory.ToModelValue(reader, "VouUser").ToDBString();

            t_task.IssueType = dbFactory.ToModelValue(reader, "IssueType").ToDBString();
            t_task.StrHouseProp = dbFactory.ToModelValue(reader, "StrHouseProp").ToDBString();
            t_task.TaskCount = GetTaskCount(t_task.ErpVoucherNo);
            t_task.ERPNote = dbFactory.ToModelValue(reader, "ERPNote").ToDBString();

            return t_task;
        }

        public int GetTaskCount(string strErpVoucherNo) 
        {
            string strSql = "select count(1) from t_task where erpvoucherno = '" + strErpVoucherNo + "'";
            return base.GetScalarBySql(strSql).ToInt32();
        }


        protected override string GetFilterSql(UserModel user, T_OutStockTaskInfo model)
        {
            string strUserNo = string.Empty;
            string strSerialNo = string.Empty;

            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";

            if (model.Status > 0)
            {
                if (model.PcOrPda == 1)//PC请求
                {
                    strSql += strAnd;
                    strSql += "isnull(status,1)= '" + model.Status + "'";
                }
                else
                {
                    if (model.Status == 1 || model.Status == 2)
                    {
                        strSql += strAnd;
                        strSql += "( status=1 or status=2 )";

                    }
                    else
                    {
                        strSql += strAnd;
                        strSql += "status= '" + model.Status + "'";
                    }
                }
            }

            if (!string.IsNullOrEmpty(model.SupcusCode))
            {
                strSql += strAnd;
                strSql += " (SupcusCode Like '" + model.SupcusCode + "%'  or SUPCUSNAME Like '" + model.SupcusCode + "%' )";
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
                strSql += " (taskno Like '%" + model.TaskNo + "%'  )";
            }
                        

            if (model.VoucherType > 0)
            {
                strSql += strAnd;
                strSql += " VoucherType ='" + model.VoucherType + "'";
            }

            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " ErpVoucherNo like '" + model.ErpVoucherNo.Trim() + "%'";
            }

            //if (!string.IsNullOrEmpty(model.PickLeaderUserNo))
            //{                
            //    strSql += strAnd;
            //    strSql += " Pickleaderuserno like '" + GetUserNo(model.PickLeaderUserNo) + "%'";
            //}

            //if (!string.IsNullOrEmpty(model.PickUserNo))
            //{                
            //    strSql += strAnd;
            //    strSql += " ID in (select taskid from t_Taskpick where Pickuserno like '" + GetUserNo(model.PickUserNo) + "%')";
            //}

            if (!string.IsNullOrEmpty(model.StrongHoldCode))
            {
                strSql += strAnd;
                strSql += " (StrongHoldCode like '" + model.StrongHoldCode + "%' )";
            }

            if (!string.IsNullOrEmpty(model.StrongHoldName))
            {
                strSql += strAnd;
                strSql += " (StrongHoldName like '" + model.StrongHoldName + "%' )";
            }

            if (model.WareHouseID > 0) 
            {
                strSql += strAnd;
                strSql += " warehouseid = '" + model.WareHouseID + "' ";
            }

            if (!string.IsNullOrEmpty(model.MaterialNo))
            {
                strSql += strAnd;
                strSql += "  id in ( SELECT HEADERID FROM v_Outtaskdetail WHERE MATERIALNO LIKE '%" + model.MaterialNo + "%' ) ";
            }

            if (!string.IsNullOrEmpty(model.VouUser))
            {
                strSql += strAnd;
                strSql += "  VouUser like '" + model.VouUser + "%' ";
            }

            if (!string.IsNullOrEmpty(model.CarNo)) 
            {
                strSql += strAnd;
                strSql += " taskno = ( select taskno from t_pickcar where carno = '"+model.CarNo+"' ) ";
            }

            if (!string.IsNullOrEmpty(model.BarCode)) 
            {
                strSerialNo =  OutBarCode.OutBarCode_DeCode.GetEndSerialNo(model.BarCode);
                strSql += strAnd;
                strSql += " taskno = ( select a.Taskno from v_Outtaskdetail a where a.id = (select a.Taskdetailesid from t_stock a where a.Serialno = '" + strSerialNo + "') )";
            }

            if (model.PcOrPda != 1) 
            {
                strSql += strAnd;
                strSql += " id in (select headerid from t_Taskdetails a where a.Fromerpwarehouse = '" + user.WarehouseCode + "' or a.Fromerpwarehouse is null) ";
            }

            if (model.TaskType > 0)
            {
                if (model.TaskType == 4)
                {
                    strSql += strAnd;
                    strSql += " ( tasktype = 2 or tasktype =3 ) ";
                }
                else
                {
                    strSql += strAnd;
                    strSql += " tasktype = '" + model.TaskType.ToInt32() + "' ";
                }

            }

            //strSql += strAnd;
            //strSql += " tasktype = 2 ";            

            return strSql;//+ " order by id  ";
        }

        private string GetUserNo(string UserNo) 
        {
            string strUserNo = string.Empty;
            if (TOOL.RegexMatch.isExists(UserNo) == true)
            {
                strUserNo = UserNo.Substring(0, UserNo.Length - 1);
            }
            else
            {
                strUserNo = UserNo;
            }
            return strUserNo;
        }

        protected override string GetViewName()
        {
            return "V_OUTTASK";
        }

        protected override string GetTableName()
        {
            return "T_TASK";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }


        protected override List<string> GetUpdateSql(UserModel user, T_OutStockTaskInfo model)
        {
            List<string> lstSql = new List<string>();
            //关闭任务之前判断有没有开始拣货
            string strSql1 = "select status from t_task where id = '" + model.ID + "'";
            int iStatus = base.GetScalarBySql(strSql1).ToInt32();

            //部分拣货或者全部拣货，需要释放库存
            if (iStatus == 2 || iStatus == 3) 
            {
                strSql1 = "update t_stock set Taskdetailesid = 0 where Taskdetailesid IN ( " +
                          " select id from t_Taskdetails where headerid = '"+model.ID+"' )";
                lstSql.Add(strSql1);
            }

            string strSql = "update t_Task  set Status = '" + model.Status + "' where id = '" + model.ID + "'";

            lstSql.Add(strSql);

            return lstSql;
        }

        /// <summary>
        /// 一张拣货单可以分配给多个拣货人
        /// </summary>
        /// <param name="UserList"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public bool SavePickUserList(List<UserModel> UserList,List<T_OutStockTaskInfo> modelList,ref string strError) 
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            foreach (var itemModel in modelList) 
            {
                //拣货单再次分配，先清空之前分配的拣货单
                strSql = "delete t_Taskpick where taskid = '" + itemModel.ID + "'";
                lstSql.Add(strSql);

                foreach (var item in UserList) 
                {
                    strSql = "insert into t_Taskpick(Id, Taskdetailid, Pickuserno, Taskid) " +
                            " Select seq_taskpick_id.Nextval,a.id,'"+item.UserNo+"',a.Headerid from t_Taskdetails a  where headerid = '"+itemModel.ID+"'";
                    lstSql.Add(strSql);
                }
            }

            return base.SaveModelListBySqlToDB(lstSql, ref strError);
        }

        #region 查看任务被哪个账户锁定

        /// <summary>
        /// 查看物料被哪个用户锁定
        /// </summary>
        /// <param name="taskDetailsMdl"></param>
        /// <param name="userMdl"></param>
        /// <returns></returns>
        public string QueryUserNameByTaskOutDetails(T_OutStockTaskInfo taskDetailsMdl, UserModel user)
        {
            try
            {
                string strUserName = string.Empty;
                string strSql = "select b.UserName from t_task a left join t_user b on a.Lockuser = b.userno where a.id = '"+taskDetailsMdl.ID+"'"+
                                " and  a.Lockuser != '"+user.UserNo+"'";
                
                return base.GetScalarBySql(strSql).ToDBString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.TargetSite);
            }
        }

        #endregion

        #region 更新任务锁定人

        /// <summary>
        /// 锁定物料操作人
        /// </summary>
        /// <param name="taskDetailsMdl"></param>
        /// <param name="userMdl"></param>
        /// <returns></returns>
        public int LockTaskOperUser(UserModel user, T_OutStockTaskInfo OutStockModel)
        {
            try
            {
                string strSql = "update t_task  set Lockuser = '{0}' where id = '{1}'";
                strSql = string.Format(strSql, user.UserNo, OutStockModel.ID);
                return GetExecuteNonQuery(strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.TargetSite);
            }
        }

        #endregion

        #region 解锁任务人
        /// <summary>
        /// 解锁任务操作人
        /// </summary>
        /// <param name="taskDetailsMdl"></param>
        /// <param name="userMdl"></param>
        /// <returns></returns>
        public int UnLockTaskOperUser(UserModel user, T_OutStockTaskInfo OutStockModel)
        {
            try
            {
                string strSql = "update t_task  set Lockuser = '' where "+
                               " ( select sum(isnull(Unshelveqty,0)) from t_Taskdetails   where headerid = '" + OutStockModel.ID+ "' ) =0" +
                               " and t_task.id = '" + OutStockModel.ID + "'";
                strSql = string.Format(strSql, user.UserNo, OutStockModel.ID);
                return GetExecuteNonQuery(strSql);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.TargetSite);
            }
        }
        #endregion

        public bool SaveBoxList(UserModel user, List<T_BoxingInfo> modelList,ref string VoucherNo , ref string strError)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            VoucherNo = base.GetNewOrderNo("1", "t_Boxing", "");

            foreach (var itemModel in modelList)
            {
                strSql = "INSERT into t_Boxing (materialno,materialname,qty,serialno,taskno,creater,createtime,isdel,status,erpvoucherno,customerno,customername) " +
                         "   values('"+itemModel.MaterialNo+"','"+itemModel.MaterialName+"','"+itemModel.Qty+"','"+VoucherNo+"','"+itemModel.TaskNo+"','"+user.UserName+"',getdate(),1,1,'"+itemModel.ErpVoucherNo+"','"+itemModel.CustomerNo+"','"+itemModel.CustomerName+"')";
                lstSql.Add(strSql);
            }

            

            return base.SaveModelListBySqlToDB(lstSql, ref strError);

        }

        public bool SaveBoxPinList(UserModel user,List<T_BoxingInfo> modelList, ref string VoucherNo, ref string strError)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            VoucherNo = base.GetNewOrderNo("1", "t_Boxing", "");

            foreach (var itemModel in modelList)
            {
                strSql = "INSERT into t_Boxing (materialno,materialname,qty,serialno,taskno,creater,createtime,isdel,status,erpvoucherno,customerno,customername) " +
                         "   values('" + itemModel.MaterialNo + "','" + itemModel.MaterialName + "','" + itemModel.Qty + "','" + VoucherNo + "','" + itemModel.TaskNo + "','" + user.UserName + "',getdate(),1,1,'" + itemModel.ErpVoucherNo + "','" + itemModel.CustomerNo + "','" + itemModel.CustomerName + "')";
                lstSql.Add(strSql);

                strSql = "update t_Boxing SET fserialno = '" + VoucherNo + "',ispin = 2 WHERE serialno = '" + itemModel.SerialNo + "'";
                lstSql.Add(strSql);
            }

            //strSql = "update t_Boxing SET fserialno = '" + VoucherNo + "',ispin = 2 WHERE serialno = '" + strSerialNew + "'";
            //lstSql.Add(strSql);

            //strSql = "update t_Boxing SET fserialno = '" + VoucherNo + "', ispin = 2 WHERE serialno = '" + strSerialOld + "'";
            //lstSql.Add(strSql);

            return base.SaveModelListBySqlToDB(lstSql, ref strError);

        }




        protected override string GetOrderNoFieldName()
        {
            return "serialno";
        }
    }
}
