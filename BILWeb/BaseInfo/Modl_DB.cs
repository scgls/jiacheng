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
    public partial class T_Modl_DB : BILBasic.Basing.Factory.Base_DB<T_Modl>
    {

        /// <summary>
        /// 添加t_customer
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_Modl t_customer)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            OracleParameter[] param = new OracleParameter[]{
              new OracleParameter("@bResult",OracleDbType.Int32),
               new OracleParameter("@ErrorMsg",OracleDbType.NVarchar2,1000),
               
               new OracleParameter("@v_ID", dbFactory.ToDBValue(t_customer.ID)),
               new OracleParameter("@v_Sn", dbFactory.ToDBValue(t_customer.Sn).ToOracleValue()),
               new OracleParameter("@v_MouldCode", dbFactory.ToDBValue(t_customer.MouldCode).ToOracleValue()),
               new OracleParameter("@v_MouldName", t_customer.MouldName.ToOracleValue()),
               new OracleParameter("@v_MouldType", t_customer.MouldType.ToOracleValue()),
               new OracleParameter("@v_Category", t_customer.Category.ToOracleValue()),
               new OracleParameter("@v_MachineType", t_customer.MachineType.ToOracleValue()),
               new OracleParameter("@v_MouldStatus", t_customer.MouldStatus.ToOracleValue()),
               new OracleParameter("@v_Clear", t_customer.ClearStatus.ToOracleValue()),
               new OracleParameter("@v_Disinfection", t_customer.Disinfection.ToOracleValue()),
               new OracleParameter("@v_ULQty", t_customer.ULQty.ToOracleValue()),
               new OracleParameter("@v_HoleNumber", t_customer.Acupoints.ToOracleValue()),
               new OracleParameter("@v_ProductType", t_customer.ProductType.ToOracleValue()),
               new OracleParameter("@v_Texture", t_customer.Texture.ToOracleValue()),
               new OracleParameter("@v_Unit", t_customer.Unit.ToOracleValue()),
               new OracleParameter("@v_OutSize", t_customer.OutSize.ToOracleValue()),
               new OracleParameter("@v_OpenSize", t_customer.OpenSize.ToOracleValue()),
               new OracleParameter("@v_Remark", t_customer.Remark.ToOracleValue()),
               new OracleParameter("@v_Creater", t_customer.Creater.ToOracleValue()),
               new OracleParameter("@v_CreateTime", t_customer.CreateTime.ToOracleValue()),
               new OracleParameter("@v_Modifyer", t_customer.Modifyer.ToOracleValue()),
               new OracleParameter("@v_ModifyTime", t_customer.ModifyTime.ToOracleValue())
              
              };
            param[0].Direction = System.Data.ParameterDirection.Output;
            param[1].Direction = System.Data.ParameterDirection.Output;
            param[2].Direction = System.Data.ParameterDirection.InputOutput;
            return param;


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_Modl model)
        {
 	        throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_Modl ToModel(IDataReader reader)
        {
            T_Modl t_customer = new T_Modl();

            t_customer.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_customer.Sn = (string)dbFactory.ToModelValue(reader, "SN");
            t_customer.MouldCode = (string)dbFactory.ToModelValue(reader, "MOULDCODE");
            t_customer.MouldName = (string)dbFactory.ToModelValue(reader, "MOULDNAME");
            t_customer.MouldType = dbFactory.ToModelValue(reader, "MOULDTYPE").ToDBString();
            t_customer.Category = dbFactory.ToModelValue(reader, "Category").ToDBString();
            t_customer.MachineType = dbFactory.ToModelValue(reader, "MachineType").ToDBString();
            t_customer.MouldStatus = dbFactory.ToModelValue(reader, "MOULDTYPE").ToDBString();
            t_customer.ClearStatus = dbFactory.ToModelValue(reader, "ClearStatus").ToDBString();
            t_customer.Disinfection = dbFactory.ToModelValue(reader, "Disinfection").ToDBString();
            t_customer.ULQty = dbFactory.ToModelValue(reader, "ULQty").ToDBString();
            t_customer.Acupoints = dbFactory.ToModelValue(reader, "Acupoints").ToDBString();
            t_customer.ProductType = dbFactory.ToModelValue(reader, "ProductType").ToDBString();
            t_customer.Texture = dbFactory.ToModelValue(reader, "Texture").ToDBString();
            t_customer.OutSize = dbFactory.ToModelValue(reader, "OutSize").ToDBString();
            t_customer.Unit = dbFactory.ToModelValue(reader, "Unit").ToDBString();
            t_customer.OpenSize = dbFactory.ToModelValue(reader, "OpenSize").ToDBString();
            t_customer.Remark = (string)dbFactory.ToModelValue(reader, "REMARK");
            t_customer.ERPNote = (string)dbFactory.ToModelValue(reader, "SumQty");//模具总数量
            t_customer.TaskNo = (string)dbFactory.ToModelValue(reader, "DelQty");//模具报废数量
            t_customer.Creater = (string)dbFactory.ToModelValue(reader, "creater");
            t_customer.CreateTime = (DateTime)dbFactory.ToModelValue(reader, "createtime");
            
            return t_customer;
        }

        protected override string GetViewName()
        {
            return "v_mould";
        }

        protected override string GetTableName()
        {
            return "Mes_MOULD";
        }

        protected override string GetSaveProcedureName()
        {
            return "P_SAVE_T_MOULD";
        }

        protected override string GetFilterSql(UserModel user, T_Modl customer)
        {
            string strSql = " where nvl(isDel,0) != 2  ";
            string strAnd = " and ";

            if (!Common_Func.IsNullOrEmpty(customer.MouldCode))
            {
                strSql += strAnd;
                strSql += " (MouldCode like '%" + customer.MouldCode + "%')  ";
            }


            if (!string.IsNullOrEmpty(customer.MouldName))
            {
                strSql += strAnd;
                strSql += " MouldName like '%" + customer.MouldName + "%'";
            }

            if (!string.IsNullOrEmpty(customer.MouldType))
            {
                strSql += strAnd;
                strSql += " MouldType like '%" + customer.MouldType + "%'";
            }

            return strSql;
        }
        private int GetID()
        {
            object id = dbFactory.ExecuteScalar(System.Data.CommandType.Text, "SELECT MAX(ID) FROM Mes_Mould");

            if (id == DBNull.Value)
                return 1;
            else
                return Convert.ToInt32(id) + 1;
        }
        private bool CheckCode(T_Modl model)
        {
            object id = dbFactory.ExecuteScalar(System.Data.CommandType.Text, "SELECT COUNT(*) FROM Mes_Mould WHERE MouldCode='" + model.MouldCode + "'");

            return Convert.ToInt32(id) > 0;
        }
        public bool SaveData(T_Modl model, ref string ErrMsg)
        {
            try
            {
                string strSql = String.Empty;                

                if (model.ID == 0)
                {
                    int id = GetID();

                    if (CheckCode(model))
                    {
                        ErrMsg = "该模具编号已经存在！";
                        return false;
                    }

                    strSql = "insert into Mes_Mould (id, SN, MouldCode, MouldName, MouldType, Category, MachineType, MouldStatus, ClearStatus, Disinfection, ULQty, Acupoints, ProductType, Texture, OutSize, Unit, OpenSize, Remark,SumQty,DelQty,creater,createtime, IsDel)" +
                                "values ('" + id + "', '"
                                + model.Sn + "','"
                                + model.MouldCode + "','"
                                + model.MouldName + "','"
                                + model.MouldType + "','"
                                + model.Category + "','"
                                + model.MachineType + "','"
                                + model.MouldStatus + "','"
                                + model.ClearStatus + "','"
                                + model.Disinfection + "','"
                                + model.ULQty + "','"
                                + model.Acupoints + "','"
                                + model.ProductType + "','"
                                + model.Texture + "','"
                                + model.OutSize + "','"
                                + model.Unit + "','"
                                + model.OpenSize + "','"
                                + model.Remark + "','"
                                + model.ERPNote + "','"
                                + model.TaskNo + "','"
                                + model.Creater + "',sysdate,'"
                                + 1 + "')";
                 
                }
                else
                {
                    strSql = "update Mes_Mould a set a.SN = '" + model.Sn + "',a.MouldName =  '" + model.MouldName + "',a.MouldType= '" + model.MouldType + "',a.Category= '" + model.Category
                        + "',a.MachineType= '" + model.MachineType + "',a.ULQty= '" + model.ULQty + "',a.Acupoints= '" + model.Acupoints + "',a.ProductType= '" + model.ProductType
                        + "',a.Texture= '" + model.Texture + "',a.OutSize = '" + model.OutSize + "' ,a.Unit = '" + model.Unit + "',a.OpenSize= '" + model.OpenSize
                         + "',a.SumQty= '" + model.ERPNote + "',a.DelQty= '" + model.TaskNo + "',a.creater= '" + model.Creater + "',a.createtime= sysdate"
                        + ",a.Remark= '" + model.Remark + "' where a.Id = '" + model.ID + "'";
                    
                }
                int i = dbFactory.ExecuteNonQuery(System.Data.CommandType.Text, strSql);
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

        public bool DelData(T_Modl model, ref string ErrMsg)
        {
            try
            {
                string sql = "DELETE FROM Mes_Mould  WHERE ID=" + model.ID;
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

    }
}
