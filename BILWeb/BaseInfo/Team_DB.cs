using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using BILBasic.User;
using BILBasic.Common;
using BILWeb.BaseInfo;

using System.Data;

namespace BILWeb.BaseInfo
{
    public partial class T_Team_DB : BILBasic.Basing.Factory.Base_DB<T_Team>
    {

        /// <summary>
        /// 添加T_Team
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_Team t_Team)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            OracleParameter[] param = new OracleParameter[]{
              //new OracleParameter("@bResult",OracleDbType.Int32),
              // new OracleParameter("@ErrorMsg",OracleDbType.NVarchar2,1000),
               
               new OracleParameter("@v_ID", dbFactory.ToDBValue(t_Team.ID)),
               new OracleParameter("@v_Seq", dbFactory.ToDBValue(t_Team.Seq).ToOracleValue()),
               new OracleParameter("@v_teamCode", dbFactory.ToDBValue(t_Team.teamCode).ToOracleValue()),
               new OracleParameter("@v_teamName", t_Team.teamName.ToOracleValue()),
               new OracleParameter("@v_LeaderCode", t_Team.LeaderCode.ToOracleValue()),
               new OracleParameter("@v_ShiftCode", t_Team.ShiftCode.ToOracleValue()),
               new OracleParameter("@v_ShiftName", t_Team.ShiftName.ToOracleValue()),
               new OracleParameter("@v_Position", t_Team.Position.ToOracleValue()),
               new OracleParameter("@v_Attribute", t_Team.Attribute.ToOracleValue()),
               new OracleParameter("@v_LineType", t_Team.LineType.ToOracleValue()),
               new OracleParameter("@v_Remark", t_Team.Remark.ToOracleValue()),
               new OracleParameter("@v_isDel", t_Team.isDel.ToOracleValue())
               //new OracleParameter("@v_Modifyer", t_Team.Modifyer.ToOracleValue()),
               //new OracleParameter("@v_ModifyTime", t_Team.ModifyTime.ToOracleValue())
              
              };
            //param[0].Direction = System.Data.ParameterDirection.Output;
            //param[1].Direction = System.Data.ParameterDirection.Output;
            //param[2].Direction = System.Data.ParameterDirection.InputOutput;
            return param;
        }

        private bool CheckCode(T_Team model)
        {
            object id = dbFactory.ExecuteScalar(System.Data.CommandType.Text, "SELECT COUNT(*) FROM Mes_Team WHERE teamCode='" + model.teamCode + "'");

            return Convert.ToInt32(id) > 0;
        }

        private int GetID()
        {
            object id = dbFactory.ExecuteScalar(System.Data.CommandType.Text, "SELECT MAX(ID) FROM Mes_Team");

            if (id == DBNull.Value)
                return 1;
            else
                return Convert.ToInt32(id) + 1;
        }

        public bool SaveData(T_Team model, ref string ErrMsg)
        {
            try
            {
                string sql = String.Empty;

                if (model.ID == 0)
                {
                    model.ID = GetID();

                    if (CheckCode(model))
                    {
                        ErrMsg = "该班组编号已经存在！";
                        return false;
                    }

                    sql = "insert into Mes_Team(ID,Seq,teamCode,teamName,LeaderCode,ShiftCode,ShiftName,Position,Attribute,LineType,Remark,isdel) VALUES" +
                        "('" + model.ID + "','" + model.Seq + "','" + model.teamCode + "','" + model.teamName + "','" + model.LeaderCode + "','" + model.ShiftCode +
                        "','" + model.ShiftName + "','" + model.Position + "','" + model.Attribute + "','" + model.LineType + "','" + model.Remark + "','" + model.isDel + "')";
                }
                else
                {
                    sql = "UPDATE Mes_Team SET Seq='" + model.Seq + "',teamCode='" + model.teamCode + "',teamName='" + model.teamName + "',LeaderCode='" + model.LeaderCode +
                        "',ShiftCode='" + model.ShiftCode + "',ShiftName='" + model.ShiftName + "',Position='" + model.Position + "',Attribute='" + model.Attribute +
                        "',LineType='" + model.LineType + "',Remark='" + model.Remark + "',isdel='" + model.isDel + "' where ID='" + model.ID + "'";
                }
                int i = dbFactory.ExecuteNonQuery(System.Data.CommandType.Text, sql);
                if (i == -2)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }
        }

        public bool DelData(T_Team model, ref string ErrMsg)
        {
            try
            {
                string sql = "DELETE FROM Mes_Team WHERE ID='" + model.ID + "'";
                int i = dbFactory.ExecuteNonQuery(System.Data.CommandType.Text, sql);
                if (i == -2)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }
        }

        protected override List<string> GetSaveSql(UserModel user, ref T_Team model)
        {
 	        throw new System.NotImplementedException();
        }

        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_Team ToModel(IDataReader reader)
        {
            T_Team t_customer = new T_Team();

            t_customer.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_customer.Seq = dbFactory.ToModelValue(reader, "Seq").ToString();
            t_customer.teamCode = dbFactory.ToModelValue(reader, "teamCode").ToDBString();
            t_customer.teamName = dbFactory.ToModelValue(reader, "teamName").ToDBString();
            t_customer.LeaderCode = dbFactory.ToModelValue(reader, "LeaderCode").ToDBString();
            t_customer.ShiftCode = dbFactory.ToModelValue(reader, "ShiftCode").ToDBString();
            t_customer.ShiftName = dbFactory.ToModelValue(reader, "ShiftName").ToDBString();
            t_customer.Position = dbFactory.ToModelValue(reader, "Position").ToDBString();
            t_customer.Attribute = dbFactory.ToModelValue(reader, "Attribute").ToDBString();
            t_customer.LineType = dbFactory.ToModelValue(reader, "LineType").ToDBString();
            t_customer.Remark = dbFactory.ToModelValue(reader, "Remark").ToDBString();
            t_customer.isDel = dbFactory.ToModelValue(reader, "isdel").ToInt32();

            //t_customer.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            //t_customer.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            //t_customer.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            //t_customer.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            return t_customer;
        }

        protected override string GetViewName()
        {
            return "Mes_Team";
        }

        protected override string GetTableName()
        {
            return "Mes_Team";
        }

        protected override string GetSaveProcedureName()
        {
            return "P_SAVE_Mes_Team";
        }

        //编号 名称 类型 产能
        protected override string GetFilterSql(UserModel user, T_Team customer)
        {
            string strSql = " where isnull(isDel,0) != 2  ";
            string strAnd = " AND ";

            if (!Common_Func.IsNullOrEmpty(customer.teamCode))
            {
                strSql += strAnd;
                strSql += " (teamCode like '%" + customer.teamCode + "%')  ";
            }


            if (!string.IsNullOrEmpty(customer.teamName))
            {
                strSql += strAnd;
                strSql += " teamName like '%" + customer.teamName + "%'";
            }

            if (!string.IsNullOrEmpty(customer.LeaderCode))
            {
                strSql += strAnd;
                strSql += " LeaderCode like '%" + customer.LeaderCode + "%'";
            }

            if (!string.IsNullOrEmpty(customer.Position))
            {
                strSql += strAnd;
                strSql += " Position like '%" + customer.Position + "%'";
            }

            return strSql;
        }

    }
}
