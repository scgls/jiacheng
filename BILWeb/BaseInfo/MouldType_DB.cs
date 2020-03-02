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
    public partial class T_MouldType_DB : BILBasic.Basing.Factory.Base_DB<T_MouldType>
    {
        protected override List<string> GetSaveSql(UserModel user, ref T_MouldType model)
        {
            throw new System.NotImplementedException();
        }


        private bool CheckCode(T_MouldType model)
        {
            object id = dbFactory.ExecuteScalar(System.Data.CommandType.Text, "SELECT COUNT(*) FROM Mes_ModelType WHERE ID='" + model.MouldTypeCode + "'");

            return Convert.ToInt32(id) > 0;
        }

        public bool SaveData(T_MouldType model, ref string ErrMsg)
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

                    strSql = "insert into Mes_ModelType (ID, Name, machineType, qty)" +
                                "values ('" + model.MouldTypeCode + "', '"
                                + model.MouldTypeName + "', '" + model.MachineType + "', '" + model.Qty + "')";
                    
                }
                else
                {
                    strSql = "update Mes_ModelType a set a.Name = '" + model.MouldTypeName + "',machineType='" + model.MachineType + "', qty= " + model.Qty + " where a.ID = '" + model.MouldTypeCode + "'";
                    
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

        public bool DelData(T_MouldType model, ref string ErrMsg)
        {
            try
            {
                string sql = "DELETE FROM Mes_ModelType WHERE ID='" + model.MouldTypeCode + "'";
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
        protected override T_MouldType ToModel(IDataReader reader)
        {
            T_MouldType t_customer = new T_MouldType();

            t_customer.MouldTypeCode = dbFactory.ToModelValue(reader, "ID").ToDBString();
            t_customer.MouldTypeName = dbFactory.ToModelValue(reader, "NAME").ToDBString();
            t_customer.MachineType = dbFactory.ToModelValue(reader, "machineType").ToDBString();
            t_customer.Qty = dbFactory.ToModelValue(reader, "qty").ToInt32();

            return t_customer;
        }

        protected override string GetViewName()
        {
            return "V_MouldType";
        }

        protected override string GetTableName()
        {
            return "Mes_ModelType";
        }

        protected override string GetSaveProcedureName()
        {
            return "P_SAVE_T_MouldType";
        }


        protected override string GetFilterSql(UserModel user, T_MouldType customer)
        {
            string strSql = " where 1=1  ";
            string strAnd = " and ";

            if (!Common_Func.IsNullOrEmpty(customer.MouldTypeCode))
            {
                strSql += strAnd;
                strSql += " (ID like '%" + customer.MouldTypeCode + "%')  ";
            }


            if (!string.IsNullOrEmpty(customer.MouldTypeName))
            {
                strSql += strAnd;
                strSql += " Name like '%" + customer.MouldTypeName + "%'";
            }

            return strSql;
        }

        protected override IDataParameter[] GetSaveModelIDataParameter(T_MouldType model)
        {
            throw new NotImplementedException();
        }
    }
}
