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
using BILWeb.Query;
using System.Data;

namespace BILWeb.BaseInfo
{
    public partial class T_ProductLine_DB : BILBasic.Basing.Factory.Base_DB<T_ProductLine>
    {

        /// <summary>
        /// 添加t_customer
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_ProductLine t_customer)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            OracleParameter[] param = new OracleParameter[]{
              //new OracleParameter("@bResult",OracleDbType.Int32),
              // new OracleParameter("@ErrorMsg",OracleDbType.NVarchar2,1000),
               
               new OracleParameter("@v_ID", dbFactory.ToDBValue(t_customer.ID)),
               new OracleParameter("@v_Seq", dbFactory.ToDBValue(t_customer.Seq).ToOracleValue()),
               new OracleParameter("@v_Sn", dbFactory.ToDBValue(t_customer.Sn).ToOracleValue()),
               new OracleParameter("@v_MachineLineName", t_customer.MachineLineName.ToOracleValue()),
               new OracleParameter("@v_Packaging", t_customer.Packaging.ToOracleValue()),
               new OracleParameter("@v_LineType", t_customer.LineType.ToOracleValue()),
               new OracleParameter("@v_FullType", t_customer.FullType.ToOracleValue()),
               new OracleParameter("@v_WorkroomCode", t_customer.WorkroomCode.ToOracleValue()),
               new OracleParameter("@v_CapacityUnit", t_customer.CapacityUnit.ToOracleValue()),
               new OracleParameter("@v_LineStatus", t_customer.Status.ToOracleValue())
               //new OracleParameter("@v_Creater", t_customer.Creater.ToOracleValue()),
               //new OracleParameter("@v_CreateTime", t_customer.CreateTime.ToOracleValue()),
               //new OracleParameter("@v_Modifyer", t_customer.Modifyer.ToOracleValue()),
               //new OracleParameter("@v_ModifyTime", t_customer.ModifyTime.ToOracleValue())
              
              };
            //param[0].Direction = System.Data.ParameterDirection.Output;
            //param[1].Direction = System.Data.ParameterDirection.Output;
            //param[2].Direction = System.Data.ParameterDirection.InputOutput;
            return param;
        }

        private bool CheckCode(T_ProductLine model)
        {
            object id = dbFactory.ExecuteScalar(System.Data.CommandType.Text, "SELECT COUNT(*) FROM Mes_ProductLine WHERE SN='" + model.Sn + "'");

            return Convert.ToInt32(id) > 0;
        }

        private int GetID()
        {
            object id = dbFactory.ExecuteScalar(System.Data.CommandType.Text, "SELECT MAX(ID) FROM Mes_ProductLine");

            if (id == DBNull.Value)
                return 1;
            else
                return Convert.ToInt32(id) + 1;
        }

        public bool SaveData(T_ProductLine model, ref string ErrMsg)
        {
            try
            {
                string sql = String.Empty;

                if (model.ID == 0)
                {
                    model.ID = GetID();

                    if (CheckCode(model))
                    {
                        ErrMsg = "该产线编号已经存在！";
                        return false;
                    }

                    sql = "insert into Mes_ProductLine(ID,Seq,SN,MachineLineName,Packaging,LineType,FulLine,WorkroomCode,CapacityUnit,Assemble,Status) VALUES" +
                        "('" + model.ID + "','" + model.Seq + "','" + model.Sn + "','" + model.MachineLineName + "','" + model.Packaging +
                        "','" + model.LineType + "','" + model.FullType + "','" + model.WorkroomCode + "','" + model.CapacityUnit + "','" + model.Assemble + "','" + model.Status + "')";
                }
                else
                {
                    sql = "UPDATE Mes_ProductLine SET Seq='" + model.Seq + "',SN='" + model.Sn + "',MachineLineName='" + model.MachineLineName +
                        "',Packaging='" + model.Packaging + "',LineType='" + model.LineType + "',FulLine='" + model.FullType + "',WorkroomCode='" + model.WorkroomCode +
                        "',CapacityUnit='" + model.CapacityUnit + "',Assemble='" + model.Assemble + "',Status='" + model.Status + "' where ID='" + model.ID + "'";
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

        public bool DelData(T_ProductLine model, ref string ErrMsg)
        {
            try
            {
                string sql = "DELETE FROM Mes_ProductLine WHERE ID='" + model.ID + "'";
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

        protected override List<string> GetSaveSql(UserModel user, ref T_ProductLine model)
        {
 	        throw new System.NotImplementedException();
        }

        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_ProductLine ToModel(IDataReader reader)
        {
            T_ProductLine t_customer = new T_ProductLine();

            t_customer.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_customer.Seq = dbFactory.ToModelValue(reader, "SEQ").ToInt32();
            t_customer.Sn = dbFactory.ToModelValue(reader, "SN").ToDBString();
            t_customer.MachineLineName = dbFactory.ToModelValue(reader, "MachineLineName").ToDBString();
            t_customer.Packaging = dbFactory.ToModelValue(reader, "PACKAGING").ToDBString();
            t_customer.LineType = dbFactory.ToModelValue(reader, "LINETYPE").ToDBString();
            t_customer.FullType = dbFactory.ToModelValue(reader, "Fulline").ToDBString();
            t_customer.Assemble = dbFactory.ToModelValue(reader, "Assemble").ToDBString();
            t_customer.WorkroomCode = dbFactory.ToModelValue(reader, "WORKROOMCODE").ToDBString();
            t_customer.CapacityUnit = dbFactory.ToModelValue(reader, "CAPACITYUNIT").ToDBString();
            t_customer.LineStatus = dbFactory.ToModelValue(reader, "Status").ToInt32();
            t_customer.StockErrHour = dbFactory.ToModelValue(reader, "StockErrHour").ToInt32();
            t_customer.FirstMixHour = dbFactory.ToModelValue(reader, "FirstMixHour").ToInt32();
            t_customer.SingleFeedHour = dbFactory.ToModelValue(reader, "SingleFeedHour").ToInt32();
            //t_customer.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            //t_customer.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            //t_customer.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            //t_customer.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            return t_customer;
        }

        protected override string GetViewName()
        {
            return "Mes_ProductLine";
        }

        protected override string GetTableName()
        {
            return "Mes_ProductLine";
        }

        protected override string GetSaveProcedureName()
        {
            return "P_SAVE_T_MACHINELINE";
        }

        //编号 名称 类型 产能
        protected override string GetFilterSql(UserModel user, T_ProductLine customer)
        {
            string strSql = " where nvl(isDel,0) != 2  ";
            string strAnd = " AND ";

            if (!Common_Func.IsNullOrEmpty(customer.Sn))
            {
                strSql += strAnd;
                strSql += " (SN like '%" + customer.Sn + "%')  ";
            }


            if (!string.IsNullOrEmpty(customer.MachineLineName))
            {
                strSql += strAnd;
                strSql += " MachineLineName like '%" + customer.MachineLineName + "%'";
            }

            if (!string.IsNullOrEmpty(customer.LineType))
            {
                strSql += strAnd;
                strSql += " LineType = '" + customer.LineType + "'";
            }

            return strSql;
        }

    }
}
