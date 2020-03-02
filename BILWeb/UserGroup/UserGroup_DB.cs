//************************************************************
//******************************作者：方颖*********************
//******************************时间：2016/10/24 11:15:45*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using BILBasic.Common;
using BILWeb.Login.User;
using BILBasic.User;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BILWeb.UserGroup
{
    public partial class T_UserGroup_DB : Base_DB<T_UserGroupInfo>
    {
        //public DbFactory dbFactory = new DbFactory(DbFactory.DbFactoryType.SQLSERVER);
        /// <summary>
        /// 添加T_USERGROUP
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_UserGroupInfo model)
        {
            dbFactory.dbF.CreateParameters(16);
            dbFactory.dbF.AddParameters(0, "@bResult", SqlDbType.Int, 0);
            dbFactory.dbF.AddParameters(1, "@ErrorMsg", SqlDbType.NVarChar, 1000);
            dbFactory.dbF.AddParameters(2, "@v_ID", model.ID.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(3, "@v_UserGroupNo", model.UserGroupNo.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(4, "@v_UserGroupName", model.UserGroupName.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(5, "@v_UserGroupAbbName", model.UserGroupAbbname.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(6, "@v_UserGroupType", model.UserGroupType.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(7, "@v_UserGroupStatus", model.UserGroupStatus.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(8, "@v_Description", model.Description.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(9, "@v_IsDel", model.IsDel.ToOracleValue(), 0);
            
            dbFactory.dbF.AddParameters(10, "@v_Creater", model.Creater.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(11, "@v_CreateTime", model.CreateTime.ToOracleValue(), 0);            
            dbFactory.dbF.AddParameters(12, "@v_Modifyer", model.Modifyer.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(13, "@v_ModifyTime", model.ModifyTime.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(14, "@v_MainTypeCode", model.MainTypeCode.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(15, "@v_PickLeaderUserNo", model.PickLeaderUserNo.ToOracleValue(), 0);

            dbFactory.dbF.Parameters[0].Direction = System.Data.ParameterDirection.Output;
            dbFactory.dbF.Parameters[1].Direction = System.Data.ParameterDirection.Output;
            dbFactory.dbF.Parameters[2].Direction = System.Data.ParameterDirection.InputOutput;
            //dbFactory.dbF.Parameters[11].Direction = System.Data.ParameterDirection.InputOutput;
            //dbFactory.dbF.Parameters[13].Direction = System.Data.ParameterDirection.InputOutput;

            return dbFactory.dbF.Parameters;

            //throw new NotImplementedException();
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            //OracleParameter[] param = new OracleParameter[]{
            //    new OracleParameter("@bResult",OracleDbType.Int32),
            //    new OracleParameter("@ErrorMsg",OracleDbType.NVarchar2,1000),
               
               
            //   new OracleParameter("@v_ID", dbFactory.ToDBValue(Convert.ToDecimal(t_usergroup.ID))),
            //   new OracleParameter("@v_UserGroupNo", dbFactory.ToDBValue(t_usergroup.UserGroupNo)),
            //   new OracleParameter("@v_UserGroupName", dbFactory.ToDBValue(t_usergroup.UserGroupName)),
            //   new OracleParameter("@v_UserGroupAbbName", dbFactory.ToDBValue(t_usergroup.UserGroupAbbname)),
            //   new OracleParameter("@v_UserGroupType", dbFactory.ToDBValue(Convert.ToDecimal(t_usergroup.UserGroupType))),
            //   new OracleParameter("@v_UserGroupStatus", dbFactory.ToDBValue(Convert.ToDecimal(t_usergroup.UserGroupStatus))),
            //   new OracleParameter("@v_Description",dbFactory.ToDBValue( t_usergroup.Description)),
            //   new OracleParameter("@v_IsDel", dbFactory.ToDBValue(t_usergroup.IsDel)),
            //   new OracleParameter("@v_Creater",dbFactory.ToDBValue( t_usergroup.Creater)),
            //   new OracleParameter("@v_CreateTime",dbFactory.ToDBValue( t_usergroup.CreateTime)),
            //   new OracleParameter("@v_Modifyer", dbFactory.ToDBValue(t_usergroup.Modifyer)),
            //   new OracleParameter("@v_ModifyTime", dbFactory.ToDBValue(t_usergroup.ModifyTime)),
            //   new OracleParameter("@v_MainTypeCode", dbFactory.ToDBValue(t_usergroup.MainTypeCode)),
            //   new OracleParameter("@v_PickLeaderUserNo", dbFactory.ToDBValue(t_usergroup.PickLeaderUserNo)),
            //  };

            //param[0].Direction = System.Data.ParameterDirection.Output;
            //param[1].Direction = System.Data.ParameterDirection.Output;
            //param[2].Direction = System.Data.ParameterDirection.InputOutput;
            //param[11].Direction = System.Data.ParameterDirection.InputOutput;
            //param[13].Direction = System.Data.ParameterDirection.InputOutput;
            //return param;
        }

        protected override List<string> GetSaveSql(UserModel user,ref  T_UserGroupInfo t_usergroup)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_UserGroupInfo ToModel(IDataReader reader)
        {
            T_UserGroupInfo t_usergroup = new T_UserGroupInfo();

            t_usergroup.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_usergroup.UserGroupNo = (string)dbFactory.ToModelValue(reader, "USERGROUPNO");
            t_usergroup.UserGroupName = (string)dbFactory.ToModelValue(reader, "USERGROUPNAME");
            t_usergroup.UserGroupAbbname = (string)dbFactory.ToModelValue(reader, "USERGROUPABBNAME");
            t_usergroup.UserGroupType = dbFactory.ToModelValue(reader, "USERGROUPTYPE").ToInt32();
            t_usergroup.UserGroupStatus = dbFactory.ToModelValue(reader, "USERGROUPSTATUS").ToInt32();
            t_usergroup.Description = (string)dbFactory.ToModelValue(reader, "DESCRIPTION");
            t_usergroup.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToDecimal();
            t_usergroup.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_usergroup.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_usergroup.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_usergroup.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");

            if (Common_Func.readerExists(reader, "IsChecked")) t_usergroup.BIsChecked = reader["IsChecked"].ToBoolean();
            if (Common_Func.readerExists(reader, "StrUserGroupType")) t_usergroup.StrUserGroupType = reader["StrUserGroupType"].ToDBString();
            if (Common_Func.readerExists(reader, "StrUserGroupStatus")) t_usergroup.StrUserGroupStatus = reader["StrUserGroupStatus"].ToDBString();

            t_usergroup.StrCreateTime = t_usergroup.CreateTime.ToShowTime();
            t_usergroup.StrModifyTime = t_usergroup.ModifyTime.ToShowTime();

            t_usergroup.DisplayID = t_usergroup.ID.ToString();
            t_usergroup.DisplayName = t_usergroup.UserGroupName;

            t_usergroup.MainTypeCode = (string)dbFactory.ToModelValue(reader, "MainTypeCode");
            t_usergroup.PickLeaderUserNo = (string)dbFactory.ToModelValue(reader, "PickLeaderUserNo");

            return t_usergroup;
        }


        protected override string GetFilterSql(UserModel user, T_UserGroupInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";


            if (!Common_Func.IsNullOrEmpty(model.UserGroupNo))
            {
                strSql += strAnd;
                strSql += " (USERGROUPNO like '%" + model.UserGroupNo + "%')  ";
            }

            if (!Common_Func.IsNullOrEmpty(model.UserGroupName))
            {
                strSql += strAnd;
                strSql += " (USERGROUPNAME like '%" + model.UserGroupName + "%')  ";
            }

            if (model.UserGroupType >= 1)
            {
                strSql += strAnd;
                strSql += " USERGROUPTYPE = " + model.UserGroupType + "";
            }

            return strSql;
        }

        public List<T_UserGroupInfo> GetModelListBySql(UserInfo user, bool IncludNoCheck)
        {
            string strSql = string.Empty;
            if (user.ID >= 1)
            {
                strSql = string.Format("SELECT DISTINCT 2 AS IsChecked,T_UserGroup.* FROM T_UserGroup WHERE ISDEL <> 2 AND UserGroupStatus <> 2 AND ID IN (SELECT GroupID FROM T_UserWithGroup WHERE UserID = {0}) ", user.ID);
                if (IncludNoCheck) strSql += string.Format("UNION SELECT DISTINCT 1 AS IsChecked,T_UserGroup.* FROM T_UserGroup WHERE ISDEL <> 2 AND UserGroupStatus <> 2 AND ID NOT IN (SELECT GroupID FROM T_UserWithGroup WHERE UserID = {0}) ", user.ID);
                strSql = string.Format("SELECT * FROM ({0}) T ORDER BY ID ", strSql);
            }
            else
            {
                if (IncludNoCheck) strSql = "SELECT DISTINCT 1 AS IsChecked,T_UserGroup.* FROM T_UserGroup WHERE ISDEL <> 2 AND UserGroupStatus <> 2";
                else strSql = "SELECT DISTINCT 2 AS IsChecked,T_UserGroup.* FROM T_UserGroup WHERE 1 = 2";
            }

            return GetModelListBySql(strSql);

        }

        protected override string GetViewName()
        {
            return "V_USERGROUP";
        }

        protected override string GetTableName()
        {
            return "T_USERGROUP";
        }

        protected override string GetSaveProcedureName()
        {
            return "P_SAVE_T_USERGROUP";
        }

        protected override string GetDeleteProcedureName() 
        {
            return "P_DELETE_T_USERGROUP";
        }

        /// <summary>
        /// 获取物料分类对应主管的用户组编码
        /// </summary>
        /// <returns></returns>
        public List<T_UserGroupInfo> GetPickLaderForMaintypeCode() 
        {
            List<T_UserGroupInfo> modelList = new List<T_UserGroupInfo>();

            string strSql = "select a.Maintypecode,a.Pickleaderuserno,a.Usergroupno,Func_GetWHCodeByWHID(isnull(to_char(b.Warehousecode),0)) as warehouseno from t_Usergroup a left join t_User b on a.Pickleaderuserno = b.Userno";

            using (IDataReader reader = dbFactory.ExecuteReader(strSql))
            {
                while (reader.Read())
                {
                    T_UserGroupInfo model = new T_UserGroupInfo();
                    model.MainTypeCode = reader["MainTypeCode"].ToDBString();
                    model.PickLeaderUserNo = reader["PickLeaderUserNo"].ToDBString();
                    model.PickGroupNo = reader["UserGroupNo"].ToDBString();
                    model.WarehouseNo = reader["warehouseno"].ToDBString();
                    modelList.Add(model);
                }
            }

            return modelList;
    
        }

        
    }
}
