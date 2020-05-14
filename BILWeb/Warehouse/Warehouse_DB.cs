//************************************************************
//******************************作者：方颖*********************
//******************************时间：2016/10/20 11:01:37*******

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

namespace BILWeb.Warehouse
{
    public partial class T_WareHouse_DB : Base_DB<T_WareHouseInfo>
    {

        /// <summary>
        /// 添加T_WAREHOUSE
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_WareHouseInfo model)
        {

            dbFactory.dbF.CreateParameters(21);
            dbFactory.dbF.AddParameters(0, "@bResult", SqlDbType.Int, 0);
            dbFactory.dbF.AddParameters(1, "@ErrorMsg", SqlDbType.NVarChar, 1000);
            dbFactory.dbF.AddParameters(2, "@v_ID", model.ID.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(3, "@v_WarehouseNo", model.WareHouseNo.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(4, "@v_WarehouseName", model.WareHouseName.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(5, "@v_WarehouseType", model.WareHouseType.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(6, "@v_ContactUser", model.ContactUser.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(7, "@v_ContactPhone", model.ContactPhone.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(8, "@v_HouseCount", model.HouseCount.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(9, "@v_HouseUsingCount", model.HouseUsingCount.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(10, "@v_Address", model.Address.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(11, "@v_LocationDesc", model.LocationDesc.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(12, "@v_WarehouseStatus", model.WareHouseStatus.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(13, "@v_IsDel", model.IsDel.ToOracleValue(), 0);            

            dbFactory.dbF.AddParameters(14, "@v_Creater", model.Creater.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(15, "@v_CreateTime", model.CreateTime.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(16, "@v_Modifyer", model.Modifyer.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(17, "@v_ModifyTime", model.ModifyTime.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(18, "@v_SamplerCode", model.SamplerCode.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(19, "@v_SamplerName", model.SamplerName.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(20, "@v_ISVWAREHOUSE", model.ISVWAREHOUSE.ToOracleValue(), 0);

            dbFactory.dbF.Parameters[0].Direction = System.Data.ParameterDirection.Output;
            dbFactory.dbF.Parameters[1].Direction = System.Data.ParameterDirection.Output;
            dbFactory.dbF.Parameters[2].Direction = System.Data.ParameterDirection.InputOutput;
            //dbFactory.dbF.Parameters[11].Direction = System.Data.ParameterDirection.InputOutput;
            //dbFactory.dbF.Parameters[13].Direction = System.Data.ParameterDirection.InputOutput;

            return dbFactory.dbF.Parameters;
         
            //OracleParameter[] param = new OracleParameter[]{
            //   new OracleParameter("@bResult",OracleDbType.Int32),
            //   new OracleParameter("@ErrorMsg",OracleDbType.NVarchar2,1000),
               
            //   new OracleParameter("@v_ID", model.ID.ToOracleValue()),
            //   new OracleParameter("@v_WarehouseNo", model.WareHouseNo.ToOracleValue()),
            //   new OracleParameter("@v_WarehouseName", model.WareHouseName.ToOracleValue()),
            //   new OracleParameter("@v_WarehouseType", model.WareHouseType.ToOracleValue()),
            //   new OracleParameter("@v_ContactUser", model.ContactUser.ToOracleValue()),
            //   new OracleParameter("@v_ContactPhone", model.ContactPhone.ToOracleValue()),
            //   new OracleParameter("@v_HouseCount", model.HouseCount.ToOracleValue()),
            //   new OracleParameter("@v_HouseUsingCount", model.HouseUsingCount.ToOracleValue()),
            //   new OracleParameter("@v_Address", model.Address.ToOracleValue()),
            //   new OracleParameter("@v_LocationDesc", model.LocationDesc.ToOracleValue()),
            //   new OracleParameter("@v_WarehouseStatus", model.WareHouseStatus.ToOracleValue()),
            //   new OracleParameter("@v_IsDel", model.IsDel.ToOracleValue()),
            //   new OracleParameter("@v_Creater", model.Creater.ToOracleValue()),
            //   new OracleParameter("@v_CreateTime", model.CreateTime.ToOracleValue()),
            //   new OracleParameter("@v_Modifyer", model.Modifyer.ToOracleValue()),
            //   new OracleParameter("@v_ModifyTime", model.ModifyTime.ToOracleValue()),
            //   new OracleParameter("@v_SamplerCode", model.SamplerCode.ToOracleValue()),
            //   new OracleParameter("@v_SamplerName", model.SamplerName.ToOracleValue()),
            //  };

            //param[0].Direction = System.Data.ParameterDirection.Output;
            //param[1].Direction = System.Data.ParameterDirection.Output;
            //param[2].Direction = System.Data.ParameterDirection.InputOutput;
           

            //return param;
        }

        protected override List<string> GetSaveSql(UserModel user, ref T_WareHouseInfo t_warehouse)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_WareHouseInfo ToModel(IDataReader reader)
        {
            T_WareHouseInfo t_warehouse = new T_WareHouseInfo();

            t_warehouse.ID =dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_warehouse.WareHouseNo = (string)dbFactory.ToModelValue(reader, "WAREHOUSENO");
            t_warehouse.WareHouseName = (string)dbFactory.ToModelValue(reader, "WAREHOUSENAME");
            t_warehouse.WareHouseType = dbFactory.ToModelValue(reader, "WAREHOUSETYPE").ToInt32();
            t_warehouse.ContactUser = (string)dbFactory.ToModelValue(reader, "CONTACTUSER");
            t_warehouse.ContactPhone = (string)dbFactory.ToModelValue(reader, "CONTACTPHONE");
            t_warehouse.HouseCount = dbFactory.ToModelValue(reader, "HOUSECOUNT").ToInt32();
            t_warehouse.HouseUsingCount = dbFactory.ToModelValue(reader, "HOUSEUSINGCOUNT").ToInt32();
            t_warehouse.Address = (string)dbFactory.ToModelValue(reader, "ADDRESS");
            t_warehouse.LocationDesc = (string)dbFactory.ToModelValue(reader, "LOCATIONDESC");
            t_warehouse.WareHouseStatus = dbFactory.ToModelValue(reader, "WAREHOUSESTATUS").ToInt32();
            t_warehouse.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToDecimal();
            t_warehouse.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_warehouse.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_warehouse.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_warehouse.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");

            if (Common_Func.readerExists(reader, "IsChecked")) t_warehouse.BIsChecked = reader["IsChecked"].ToBoolean();
            if (Common_Func.readerExists(reader, "StrWarehouseStatus")) t_warehouse.StrWarehouseStatus = reader["StrWarehouseStatus"].ToDBString();
            if (Common_Func.readerExists(reader, "AreaCount")) t_warehouse.AreaCount = reader["AreaCount"].ToInt32();
            if (Common_Func.readerExists(reader, "AreaUsingCount")) t_warehouse.AreaUsingCount = reader["AreaUsingCount"].ToInt32();

            t_warehouse.HouseRate = t_warehouse.HouseCount >= 1 ? t_warehouse.HouseUsingCount.ToDecimal() / t_warehouse.HouseCount.ToDecimal() : 0;
            t_warehouse.AreaRate = t_warehouse.AreaCount >= 1 ? t_warehouse.AreaUsingCount.ToDecimal() / t_warehouse.AreaCount.ToDecimal() : 0;

            t_warehouse.StrCreateTime = t_warehouse.CreateTime.ToShowTime();
            t_warehouse.StrModifyTime = t_warehouse.ModifyTime.ToShowTime();

            t_warehouse.SamplerCode = dbFactory.ToModelValue(reader, "Samplercode").ToDBString();
            t_warehouse.SamplerName = dbFactory.ToModelValue(reader, "Samplername").ToDBString();

            t_warehouse.DisplayID = t_warehouse.WareHouseNo;
            t_warehouse.DisplayName = t_warehouse.WareHouseName;
            t_warehouse.ISVWAREHOUSE = dbFactory.ToModelValue(reader, "ISVWAREHOUSE").ToInt32();

            return t_warehouse;
        }

        protected override string GetViewName()
        {
            return "V_WAREHOUSE";
        }

        protected override string GetTableName()
        {
            return "T_WAREHOUSE";
        }

        protected override string GetSaveProcedureName()
        {
            return "P_SAVE_T_WAREHOUSE";
        }

        protected override string GetDeleteProcedureName()
        {
            return "P_DELETE_T_WAREHOUSE";
        }

        protected override string GetFilterSql(UserModel user, T_WareHouseInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";


            if (!Common_Func.IsNullOrEmpty(model.WareHouseNo))
            {
                strSql += strAnd;
                strSql += " (WarehouseNo LIKE '" + model.WareHouseNo + "%' OR WarehouseName Like '" + model.WareHouseNo + "%')  ";
            }

            if (!Common_Func.IsNullOrEmpty(model.WareHouseName))
            {
                strSql += strAnd;
                strSql += "  WarehouseName Like '" + model.WareHouseName + "%'  ";
            }

            if (!string.IsNullOrEmpty(model.HouseNo))
            {
                strSql += strAnd;
                strSql += " ID In (Select WarehouseID From T_House Where HouseNo LIKE '%" + model.HouseNo + "%' OR HouseName Like '%" + model.HouseNo + "%') ";
               
            }

            if (!Common_Func.IsNullOrEmpty(model.Address))
            {
                strSql += strAnd;
                strSql += " Address Like '%" + model.Address + "%'";
            }

            if (!Common_Func.IsNullOrEmpty(model.Creater))
            {
                strSql += strAnd;
                strSql += " Creater Like '%" + model.Creater + "%' ";
            }

            if (model.DateFrom!=null)
            {
                strSql += strAnd;
                strSql += " CreateTime >= " + model.DateFrom.ToDateTime().Date.ToOracleTimeString() + "  ";
            }

            if (model.DateTo != null)
            {
                strSql += strAnd;
                strSql += " CreateTime <= " + model.DateTo.ToDateTime().AddDays(1).Date.ToOracleTimeString() + " ";
            }

            return strSql;
        }

        public List<T_WareHouseInfo> GetModelListBySql(UserInfo user, bool IncludNoCheck)
        {
            string strSql = string.Empty;
            if (user.ID >= 1)
            {
                strSql = string.Format("SELECT DISTINCT 2 AS IsChecked,T_Warehouse.* FROM T_Warehouse WHERE ISDEL <> 2 AND WarehouseStatus <> 2 AND ID IN (SELECT WarehouseID FROM T_UserWithWarehouse WHERE UserID = {0}) ", user.ID);
                if (IncludNoCheck) strSql += string.Format("UNION SELECT DISTINCT 1 AS IsChecked,T_Warehouse.* FROM T_Warehouse WHERE ISDEL <> 2 AND WarehouseStatus <> 2 AND ID NOT IN (SELECT WarehouseID FROM T_UserWithWarehouse WHERE UserID = {0}) ", user.ID);
                strSql = string.Format("SELECT * FROM ({0}) T ORDER BY ID ", strSql);
            }
            else
            {
                if (IncludNoCheck) strSql = "SELECT DISTINCT 1 AS IsChecked,T_Warehouse.* FROM T_Warehouse WHERE ISDEL <> 2 AND WarehouseStatus <> 2";
                else strSql = "SELECT DISTINCT 2 AS IsChecked,T_Warehouse.* FROM T_Warehouse WHERE 1 = 2";
            }

            return GetModelListBySql(strSql);
        }

    }
}
