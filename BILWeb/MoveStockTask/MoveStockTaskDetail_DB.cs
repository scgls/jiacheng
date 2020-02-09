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

namespace BILWeb.MoveStockTask
{
    public class MoveStockTaskDetail_DB : BILBasic.Basing.Factory.Base_DB<T_MoveTaskDetailInfo>
    {

        /// <summary>
        /// 添加t_movedetail
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_MoveTaskDetailInfo t_movedetail)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_MoveTaskDetailInfo t_movedetail)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_MoveTaskDetailInfo ToModel(IDataReader reader)
        {
            T_MoveTaskDetailInfo t_movedetail = new T_MoveTaskDetailInfo();



            t_movedetail.MaterialNo = (string)dbFactory.ToModelValue(reader, "MATERIALNO");
            t_movedetail.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MATERIALDESC");
            t_movedetail.MaterialNoID = dbFactory.ToModelValue(reader, "MaterialNoID").ToInt32();
            //t_movedetail.Unit = (string)dbFactory.ToModelValue(reader, "UNIT");
            t_movedetail.MoveQty = (decimal?)dbFactory.ToModelValue(reader, "MOVEQTY");
            t_movedetail.RemainQty = t_movedetail.MoveQty;//补库数量
            t_movedetail.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "STRONGHOLDCODE");
            t_movedetail.StrongHoldName = (string)dbFactory.ToModelValue(reader, "StrongHoldName");
            t_movedetail.CompanyCode = (string)dbFactory.ToModelValue(reader, "CompanyCode");
            t_movedetail.FromErpWarehouse = dbFactory.ToModelValue(reader, "warehouseno").ToString();
            //t_movedetail.ToErpWarehouse = dbFactory.ToModelValue(reader, "warehouseid").ToString();
            t_movedetail.VoucherType = 3;
            t_movedetail.ERPVoucherType = "MoveTask";            

            return t_movedetail;
        }

        public bool getMoveTaskList(T_MoveTaskDetailInfo moveInfo, out List<T_MoveTaskDetailInfo> listMoveDetail, out string errMsg)
        {
            listMoveDetail = null;
            errMsg = "";
            string sql = "select * from v_movetaskscat ";
            try
            {
                listMoveDetail = GetModelListBySql(sql);
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.ToString();
                return false;
            }

        }

        protected override string GetFilterSql(UserModel user, T_MoveTaskDetailInfo model)
        {
            string strSql = " where 1=1 ";
            string strAnd = " and ";
            if (!string.IsNullOrEmpty(model.FromErpWarehouse))
            {
                strSql += strAnd;
                strSql += " WAREHOUSENO = '" + user.WarehouseCode + "' ";
            }
            return strSql;
        }

        protected override List<string> GetSaveModelListSql(UserModel user, List<T_MoveTaskDetailInfo> modelList)
        {
            List<string> listSql = new List<string>();
            string strSql1 = "";

            int taskid = GetTableID("seq_task_id");
            string TaskNoID = base.GetTableID("seq_task_no").ToString();
            string TaskNo = "B" + System.DateTime.Now.ToString("yyyyMMdd") + TaskNoID.PadLeft(4, '0');
            string voucheno = "B" + System.DateTime.Now.ToString("yyyyMMdd") + TaskNoID.PadLeft(4, '0');
            strSql1 = "insert into t_task (id,Vouchertype,tasktype,Taskno,status,Taskissued,Receiveuserno,Createtime,Creater," +
                        "erpvoucherno,movetype,Taskissueduser,voucherno,STRONGHOLDCODE,STRONGHOLDNAME,COMPANYCODE,erpinvoucherno,WAREHOUSEID,erpvouchertype,HOUSEPROP)" +
                       " values (" + taskid + ",3,3,'" + TaskNo + "', 1,Sysdate,'" + user.UserNo + "',Sysdate,'" + user.UserNo + "','" + TaskNo + "','3','" + user.UserNo + "','" + voucheno + "','" + modelList[0].StrongHoldCode + "','" + modelList[0].StrongHoldName + "'" +
                       ",'" + modelList[0].CompanyCode + "','" + voucheno + "'," + user.WarehouseID + ",'MOVETASK','1')";

            listSql.Add(strSql1);
            int i = 0;
            foreach (var item in modelList)
            {
                item.TaskNo = TaskNo;
                i++;
                strSql1 = "insert into t_Taskdetails (id,headerid,Materialno,materialdesc,Taskqty,Remainqty,LineStatus,Creater,Createtime,Unit,Unitname,erpvoucherno,materialnoid,toareano,voucherno," +
                "STRONGHOLDCODE,STRONGHOLDNAME,COMPANYCODE,Productdate,Supprddate,Fromerpareano,Fromerpwarehouse,rowno,rownodel)" +
                   "values(seq_taskdetail_id.Nextval ,'" + taskid + "','" + item.MaterialNo + "','" + item.MaterialDesc + "','" + item.MoveQty + "','" + item.MoveQty + "'," +
                   "'1','" + user.UserNo + "',Sysdate,'" + item.Unit + "','" + item.UnitName + "','" + voucheno + "','" + item.MaterialNoID + "','" + user.ReceiveAreaID + "','" + voucheno + "'," +
                   "'" + item.StrongHoldCode + "','" + item.StrongHoldName + "','" + item.CompanyCode + "',to_date('" + DateTime.Now.ToString() + "','YYYY-MM-DD hh24:mi:ss')," +
                "to_date('" + DateTime.Now.ToString() + "','YYYY-MM-DD hh24:mi:ss'),'" + user.ReceiveAreaNo + "','" + item.FromErpWarehouse + "'," + i + "," + i + ")";

                listSql.Add(strSql1);
            }
            return listSql;
        }
        public bool setMoveDetail(T_MoveTaskDetailInfo moveDetail, out string errMsg)
        {
            errMsg = "";
            try
            {
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected override string GetViewName()
        {
            return "v_movetaskscat";
        }

        protected override string GetTableName()
        {
            return "T_MOVEDETAIL";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }


    }
}
