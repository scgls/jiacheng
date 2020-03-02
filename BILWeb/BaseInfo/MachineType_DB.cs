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
    public partial class T_MachineType_DB : BILBasic.Basing.Factory.Base_DB<T_MachineType>
    {
        protected override List<string> GetSaveSql(UserModel user, ref T_MachineType model)
        {
            throw new System.NotImplementedException();
        }


        private bool CheckCode(T_MachineType model)
        {
            object id = dbFactory.ExecuteScalar(System.Data.CommandType.Text, "SELECT COUNT(*) FROM Mes_MachineType WHERE ID='" + model.MachineTypeCode + "'");

            return Convert.ToInt32(id) > 0;
        }

        public bool SaveData(T_MachineType model, ref string ErrMsg)
        {
            try
            {
                string strSql = String.Empty;               

                if (model.ID == 0)
                {
                    if (CheckCode(model))
                    {
                        ErrMsg = "该设备编号已经存在！";
                        return false;
                    }

                    strSql = "insert into Mes_MachineType (ID, Name)" +
                                "values ('" + model.MachineTypeCode + "', '"
                                + model.MachineTypeName + "')";
                    
                }
                else
                {
                    strSql = "update Mes_MachineType a set a.Name = '" + model.MachineTypeName + "' where a.ID = '" + model.MachineTypeCode + "'";
                    
                }

                int i = dbFactory.ExecuteNonQuery(System.Data.CommandType.Text, strSql);
                if (i == -2)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public bool DelData(T_MachineType model, ref string ErrMsg)
        {
            try
            {
                string sql = "DELETE FROM Mes_MachineType WHERE ID='" + model.MachineTypeCode + "'";
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

        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_MachineType ToModel(IDataReader reader)
        {
            T_MachineType t_customer = new T_MachineType();

            t_customer.MachineTypeCode = dbFactory.ToModelValue(reader, "ID").ToDBString();
            t_customer.MachineTypeName = dbFactory.ToModelValue(reader, "NAME").ToDBString();

            return t_customer;
        }

        protected override string GetViewName()
        {
            return "V_MachineType";
        }

        protected override string GetTableName()
        {
            return "Mes_MachineType";
        }

        protected override string GetSaveProcedureName()
        {
            return "P_SAVE_T_MachineType";
        }


        protected override string GetFilterSql(UserModel user, T_MachineType customer)
        {
            string strSql = " where 1=1  ";
            string strAnd = " and ";

            if (!Common_Func.IsNullOrEmpty(customer.MachineTypeCode))
            {
                strSql += strAnd;
                strSql += " (ID like '%" + customer.MachineTypeCode + "%')  ";
            }


            if (!string.IsNullOrEmpty(customer.MachineTypeName))
            {
                strSql += strAnd;
                strSql += " Name like '%" + customer.MachineTypeName + "%'";
            }

            return strSql;
        }

        protected override IDataParameter[] GetSaveModelIDataParameter(T_MachineType model)
        {
            throw new NotImplementedException();
        }
    }
}
