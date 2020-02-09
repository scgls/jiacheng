//************************************************************
//******************************作者：方颖*********************
//******************************时间：2019/8/15 20:50:36*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using BILBasic.User;
using System.Data;

namespace BILWeb.Move
{
    public partial class T_MoveDetail_DB : BILBasic.Basing.Factory.Base_DB<T_MoveDetailInfo>
    {

        /// <summary>
        /// 添加t_movedetail
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_MoveDetailInfo t_movedetail)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_MoveDetailInfo t_movedetail)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_MoveDetailInfo ToModel(IDataReader reader)
        {
            T_MoveDetailInfo t_movedetail = new T_MoveDetailInfo();


            t_movedetail.MaterialNoID = Convert.ToInt32(dbFactory.ToModelValue(reader, "MATERIALNOID"));
            t_movedetail.MaterialNo = (string)dbFactory.ToModelValue(reader, "MATERIALNO");
            t_movedetail.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MATERIALDESC");
            t_movedetail.Unit = (string)dbFactory.ToModelValue(reader, "UNIT");
            t_movedetail.MoveQty = (decimal?)dbFactory.ToModelValue(reader, "MOVEQTY");
            t_movedetail.RemainQty = (decimal?)dbFactory.ToModelValue(reader, "MINIQTY");//最低库存量
            t_movedetail.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "STRONGHOLDCODE");
            t_movedetail.FromErpWarehouse = dbFactory.ToModelValue(reader, "warehouseno").ToString();
            t_movedetail.ToErpWarehouse = dbFactory.ToModelValue(reader, "warehouseid").ToString();
            t_movedetail.VoucherType = 1;
            t_movedetail.ERPVoucherType = "Move";

            // t_movedetail.ToErpWarehouse = (string)dbFactory.ToModelValue(reader, "houseno"); //零拣库区
            //  t_movedetail.StrongHoldName = (string)dbFactory.ToModelValue(reader, "STRONGHOLDNAME");
            //t_movedetail.ID = (int)dbFactory.ToModelValue(reader, "ID");
            //t_movedetail.HeaderID = (int)dbFactory.ToModelValue(reader, "HEADERID");

            //t_movedetail.UnitName = (string)dbFactory.ToModelValue(reader, "UNITNAME");
            //t_movedetail.RemainQty = (decimal?)dbFactory.ToModelValue(reader, "REMAINQTY");
            //t_movedetail.FromStorageLoc = (string)dbFactory.ToModelValue(reader, "FROMSTORAGELOC");
            //t_movedetail.CloseOweUser = (string)dbFactory.ToModelValue(reader, "CLOSEOWEUSER");
            //t_movedetail.CloseOweDate = (DateTime?)dbFactory.ToModelValue(reader, "CLOSEOWEDATE");
            //t_movedetail.CloseOweRemark = (string)dbFactory.ToModelValue(reader, "CLOSEOWEREMARK");
            //t_movedetail.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            //t_movedetail.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            //t_movedetail.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            //t_movedetail.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            //t_movedetail.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToDecimal();
            //t_movedetail.LineStatus = (int)dbFactory.ToModelValue(reader, "LINESTATUS");
            //t_movedetail.VoucherNo = (string)dbFactory.ToModelValue(reader, "VOUCHERNO");


            //t_movedetail.CompanyCode = (string)dbFactory.ToModelValue(reader, "COMPANYCODE");
            //t_movedetail.FromBatchNo = (string)dbFactory.ToModelValue(reader, "FROMBATCHNO");
            //t_movedetail.FromErpAreaNo = (string)dbFactory.ToModelValue(reader, "FROMERPAREANO");
            //t_movedetail.FromErpWarehouse = (string)dbFactory.ToModelValue(reader, "FROMERPWAREHOUSE");
            //t_movedetail.ToBatchNo = (string)dbFactory.ToModelValue(reader, "TOBATCHNO");
            //t_movedetail.ToErpAreaNo = (string)dbFactory.ToModelValue(reader, "TOERPAREANO");
            //t_movedetail.EAN = (string)dbFactory.ToModelValue(reader, "EAN");
            //t_movedetail.InScanQty = (decimal?)dbFactory.ToModelValue(reader, "InScanQty");
            //t_movedetail.OutScanQty = (decimal?)dbFactory.ToModelValue(reader, "OutScanQty");

            return t_movedetail;
        }

        public bool getMoveTaskList(T_MoveDetailInfo moveInfo, out List<T_MoveDetailInfo> listMoveDetail, out string errMsg)
        {
            listMoveDetail = null;
            errMsg = "";
            string sql = "select * from v_Movetask ";
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

        protected override string GetFilterSql(UserModel user, T_MoveDetailInfo model)
        {
            string strSql = " where 1=1 ";
            string strAnd = " and ";
            if (!string.IsNullOrEmpty(model.FromErpWarehouse))
            {
                strSql += strAnd;
                strSql += " WAREHOUSEID = '" + model.FromErpWarehouse + "' ";
            }
            return strSql;
        }

        protected override List<string> GetSaveModelListSql(UserModel user, List<T_MoveDetailInfo> modelList)
        {
            List<string> listSql = new List<string>();
            string strSql1 = "";

            int taskid = GetTableID("seq_task_id");
            string TaskNoID = base.GetTableID("seq_task_no").ToString();
            string TaskNo = "B" + System.DateTime.Now.ToString("yyyyMMdd") + TaskNoID.PadLeft(4, '0');
            string voucheno = "B" + System.DateTime.Now.ToString("yyyyMMdd") + TaskNoID.PadLeft(4, '0');
            strSql1 = "insert into t_task (id,Vouchertype,tasktype,Taskno,status,Taskissued,Receiveuserno,Createtime,Creater," +
                        "erpvoucherno,movetype,Taskissueduser,voucherno,STRONGHOLDCODE,STRONGHOLDNAME,COMPANYCODE,erpinvoucherno,WAREHOUSEID,erpvouchertype,houseprop)" +
                       " values (" + taskid + ",3,3,'" + TaskNo + "', 1,Sysdate,'" + user.UserNo + "',Sysdate,'" + user.UserNo + "','" + TaskNo + "','3','" + user.UserNo + "','" + voucheno + "','" + modelList[0].StrongHoldCode + "','" + modelList[0].StrongHoldName + "'" +
                       ",'" + modelList[0].CompanyCode + "','" + voucheno + "'," + user.WarehouseID + ",'MOVE',1)";

            listSql.Add(strSql1);
            int i = 0;
            foreach (var item in modelList)
            {
                item.TaskNo = TaskNo;
                i++;
                strSql1 = "insert into t_Taskdetails (id,headerid,Materialno,materialdesc,Taskqty,Remainqty,LineStatus,Creater,Createtime,Unit,Unitname,erpvoucherno,materialnoid,toareano,voucherno," +
                "STRONGHOLDCODE,STRONGHOLDNAME,COMPANYCODE,Productdate,Supprddate,Fromerpareano,Fromerpwarehouse,rowno,rownodel)" +
                   "values(seq_taskdetail_id.Nextval ,'" + taskid + "','" + item.MaterialNo + "','" + item.MaterialDesc + "','" + item.MoveQty + "','" + item.MoveQty + "'," +
                   "'1','" + user.UserNo + "',Sysdate,'" + item.Unit + "','" + item.UnitName + "','" + item.ErpVoucherNo + "','" + item.MaterialNoID + "','" + user.ReceiveAreaID + "','" + item.VoucherNo + "'," +
                   "'" + item.StrongHoldCode + "','" + item.StrongHoldName + "','" + item.CompanyCode + "',to_date('" + DateTime.Now.ToString() + "','YYYY-MM-DD hh24:mi:ss')," +
                "to_date('" + DateTime.Now.ToString() + "','YYYY-MM-DD hh24:mi:ss'),'" + user.ReceiveAreaNo + "','" + item.FromErpWarehouse + "'," + i + "," + i + ")";

                listSql.Add(strSql1);
            }
            return listSql;
        }
        public bool setMoveDetail(T_MoveDetailInfo moveDetail, out string errMsg)
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
            return "v_Movetask";
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
