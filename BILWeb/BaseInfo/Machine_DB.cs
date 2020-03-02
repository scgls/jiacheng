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
    public partial class T_Machine_DB : BILBasic.Basing.Factory.Base_DB<T_Machine>
    { 

        protected override List<string> GetSaveSql(UserModel user, ref T_Machine model)
        {
            throw new System.NotImplementedException();
        }

        private int GetID()
        {
            object id = dbFactory.ExecuteScalar(System.Data.CommandType.Text, "SELECT MAX(ID) FROM Mes_Machine");

            if (id == DBNull.Value)
                return 1;
            else
                return Convert.ToInt32(id) + 1;
        }

        private bool CheckCode(T_Machine model)
        {
            object id = dbFactory.ExecuteScalar(System.Data.CommandType.Text, "SELECT COUNT(*) FROM Mes_Machine WHERE MachineCode='" + model.MachineCode + "'");

            return Convert.ToInt32(id) > 0;
        }

        public bool SaveData(T_Machine model, ref string ErrMsg)
        {
            try
            {
                string strSql = String.Empty;               

                if (model.ID == 0)
                {
                    int id = GetID();

                    if (CheckCode(model))
                    {
                        ErrMsg = "该设备编号已经存在！";
                        return false;
                    }

                    strSql = "insert into Mes_Machine (id, SN, MachineCode, MachineName, MachineType, MACHINEPROPERTY, MACHINEEVERSION, Brand, ConMode, DeviceStatus, IPAddress, Port, TVBrand, TVConMode, Manufacturer, Capacity, Unit, Capacity2, EHSStatus, MaintainTime, Remark,AddressSite, IsDel)" +
                                "values ('" + id + "', '"
                                + model.Sn + "','" 
                                + model.MachineCode + "','" 
                                + model.MachineName + "','"
                                + model.MachineType + "','" 
                                + model.MachineProperty + "','" 
                                + model.MachineEversion + "','"
                                + model.Brand + "','"
                                + model.ConMode + "','"
                                + model.DeviceStatus + "','"
                                + model.IPAddress + "','"
                                + model.Port + "','"
                                + model.TVBrand + "','"
                                + model.TVConMode + "','"
                                + model.Manufacturer + "','"
                                + model.Capacity + "','" 
                                + model.Unit + "','"
                                + model.Capacity2 + "','"
                                + model.EHSStatus + "','"
                                + model.MaintainTime + "','"
                                + model.Remark + "','"
                                + model.AddressSite + "','" 
                                + 1 + "')";
                    
                }
                else
                {
                    strSql = "update Mes_Machine a set a.SN = '" + model.Sn + "',a.MachineName =  '" + model.MachineName + "',a.MachineType= '" + model.MachineType + "',a.MachineProperty= '" + model.MachineProperty
                        + "',a.MACHINEEVERSION= '" + model.MachineEversion + "',a.Brand= '" + model.Brand + "',a.ConMode= '" + model.ConMode + "',a.DeviceStatus= '" + model.DeviceStatus
                        + "',a.IPAddress= '" + model.IPAddress + "',a.Port = '" + model.Port + "' ,a.TVBrand = '" + model.TVBrand + "' ,a.TVConMode= '" + model.TVConMode + "',a.Manufacturer= '" + model.Manufacturer
                        + "',a.Capacity= '" + model.Capacity + "',a.Unit= '" + model.Unit + "',a.Capacity2= '" + model.Capacity2 + "',a.EHSStatus= '" + model.EHSStatus + "',a.MaintainTime= '" + model.MaintainTime
                        + "',a.Remark= '" + model.Remark + "',a.AddressSite='" + model.AddressSite + "' where a.Id = '" + model.ID + "'";
                    
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

        public bool DelData(T_Machine model, ref string ErrMsg)
        {
            try
            {
                string sql = "DELETE FROM Mes_Machine WHERE ID=" + model.ID;
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
        protected override T_Machine ToModel(IDataReader reader)
        {
            T_Machine t_customer = new T_Machine();

            t_customer.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_customer.Sn = dbFactory.ToModelValue(reader, "SN").ToDBString();
            t_customer.MachineCode = dbFactory.ToModelValue(reader, "MACHINECODE").ToDBString();
            t_customer.MachineName = dbFactory.ToModelValue(reader, "MACHINENAME").ToDBString();
            t_customer.MachineType = dbFactory.ToModelValue(reader, "MACHINETYPE").ToDBString();
            t_customer.MachineProperty = dbFactory.ToModelValue(reader, "MachineProperty").ToDBString();
            t_customer.MachineEversion = dbFactory.ToModelValue(reader, "MACHINEEVERSION").ToDBString();
            t_customer.Brand = dbFactory.ToModelValue(reader, "Brand").ToDBString();
            t_customer.ConMode = dbFactory.ToModelValue(reader, "ConMode").ToDBString();
            t_customer.DeviceStatus = dbFactory.ToModelValue(reader, "DeviceStatus").ToInt32();
            t_customer.IPAddress = dbFactory.ToModelValue(reader, "IPAddress").ToDBString();
            t_customer.Port = dbFactory.ToModelValue(reader, "Port").ToDBString();
            t_customer.TVBrand = dbFactory.ToModelValue(reader, "TVBrand").ToDBString();
            t_customer.TVConMode = dbFactory.ToModelValue(reader, "TVConMode").ToDBString();
            t_customer.Manufacturer = dbFactory.ToModelValue(reader, "Manufacturer").ToDBString();
            t_customer.Capacity = dbFactory.ToModelValue(reader, "Capacity").ToDecimal();
            t_customer.Unit = dbFactory.ToModelValue(reader, "Unit").ToDBString();
            t_customer.Capacity2 = dbFactory.ToModelValue(reader, "Capacity2").ToDecimal();
            t_customer.EHSStatus = dbFactory.ToModelValue(reader, "EHSStatus").ToInt32();
            t_customer.MaintainTime = dbFactory.ToModelValue(reader, "MaintainTime").ToInt32();
            t_customer.Remark = dbFactory.ToModelValue(reader, "REMARK").ToDBString();
            t_customer.AddressSite = dbFactory.ToModelValue(reader, "AddressSite").ToDBString();

            return t_customer;
        }

        protected override string GetViewName()
        {
            return "V_Machine";
        }

        protected override string GetTableName()
        {
            return "Mes_Machine";
        }

        protected override string GetSaveProcedureName()
        {
            return "P_SAVE_T_DEVICE";
        }


        protected override string GetFilterSql(UserModel user, T_Machine customer)
        {
            string strSql = " where nvl(isDel,0) != 2  ";
            string strAnd = " and ";

            if (!Common_Func.IsNullOrEmpty(customer.MachineCode))
            {
                strSql += strAnd;
                strSql += " (MachineCode like '%" + customer.MachineCode + "%')  ";
            }


            if (!string.IsNullOrEmpty(customer.MachineName))
            {
                strSql += strAnd;
                strSql += " MachineName like '%" + customer.MachineName + "%'";
            }

            if ((!string.IsNullOrEmpty(customer.MachineType)))
            {
                strSql += strAnd;
                strSql += " MachineType like '%" + customer.MachineType + "%'";
            }

            return strSql;
        }

        protected override IDataParameter[] GetSaveModelIDataParameter(T_Machine model)
        {
            throw new NotImplementedException();
        }
    }
}
