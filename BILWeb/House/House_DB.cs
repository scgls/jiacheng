using BILBasic.DBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;
using BILBasic.User;
using BILWeb.Warehouse;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BILWeb.House
{
    public partial class T_House_DB : BILBasic.Basing.Factory.Base_DB<T_HouseInfo>
    {
        /// <summary>
        /// 添加T_HOUSE
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_HouseInfo model)
        {

            dbFactory.dbF.CreateParameters(22);
            dbFactory.dbF.AddParameters(0, "@bResult", SqlDbType.Int, 0);
            dbFactory.dbF.AddParameters(1, "@ErrorMsg", SqlDbType.NVarChar, 1000);
            dbFactory.dbF.AddParameters(2, "@v_ID", model.ID.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(3, "@v_HouseNo", model.HouseNo.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(4, "@v_HouseName", model.HouseName.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(5, "@v_HouseType", model.HouseType.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(6, "@v_ContactUser", model.ContactUser.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(7, "@v_ContactPhone", model.ContactPhone.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(8, "@v_AreaCount", model.AreaCount.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(9, "@v_AreaUsingCount", model.AreaUsingCount.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(10, "@v_Address", model.Address.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(11, "@v_LocationDesc", model.LocationDesc.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(12, "@v_HouseStatus", model.HouseStatus.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(13, "@v_WarehouseID", model.WarehouseID.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(14, "@v_IsDel", model.IsDel.ToOracleValue(), 0);            

            dbFactory.dbF.AddParameters(15, "@v_Creater", model.Creater.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(16, "@v_CreateTime", model.CreateTime.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(17, "@v_Modifyer", model.Modifyer.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(18, "@v_ModifyTime", model.ModifyTime.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(19, "@v_FloorType", model.FloorType.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(20, "@v_MaterialClassCode", model.MaterialClassCode.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(21, "@v_HousePropType", model.HouseProp.ToOracleValue(), 0);
            

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
            //   new OracleParameter("@v_HouseNo", model.HouseNo.ToOracleValue()),
            //   new OracleParameter("@v_HouseName", model.HouseName.ToOracleValue()),
            //   new OracleParameter("@v_HouseType", model.HouseType.ToOracleValue()),
            //   new OracleParameter("@v_ContactUser", model.ContactUser.ToOracleValue()),
            //   new OracleParameter("@v_ContactPhone", model.ContactPhone.ToOracleValue()),
            //   new OracleParameter("@v_HouseCount", model.AreaCount.ToOracleValue()),
            //   new OracleParameter("@v_HouseUsingCount", model.AreaUsingCount.ToOracleValue()),
            //   new OracleParameter("@v_Address", model.Address.ToOracleValue()),
            //   new OracleParameter("@v_LocationDesc", model.LocationDesc.ToOracleValue()),
            //   new OracleParameter("@v_HouseStatus", model.HouseStatus.ToOracleValue()),
            //   new OracleParameter("@v_WarehouseID", model.WarehouseID.ToOracleValue()),
            //   new OracleParameter("@v_IsDel", model.IsDel.ToOracleValue()),
            //   new OracleParameter("@v_Creater", model.Creater.ToOracleValue()),
            //   new OracleParameter("@v_CreateTime", model.CreateTime.ToOracleValue()),
            //   new OracleParameter("@v_Modifyer", model.Modifyer.ToOracleValue()),
            //   new OracleParameter("@v_ModifyTime", model.ModifyTime.ToOracleValue()),
            //   new OracleParameter("@v_FloorType", model.FloorType.ToOracleValue()),
            //   new OracleParameter("@v_MaterialClassCode", model.MaterialClassCode.ToOracleValue()),
            //   new OracleParameter("@v_HousePropType", model.HouseProp.ToOracleValue()),
            //};

            //param[0].Direction = System.Data.ParameterDirection.Output;
            //param[1].Direction = System.Data.ParameterDirection.Output;
            //param[2].Direction = System.Data.ParameterDirection.InputOutput;
           

            //return param;



        }

        protected override List<string> GetSaveSql(UserModel user,ref T_HouseInfo t_house)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_HouseInfo ToModel(IDataReader reader)
        {
            T_HouseInfo t_house = new T_HouseInfo();

            t_house.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_house.HouseNo = (string)dbFactory.ToModelValue(reader, "HOUSENO");
            t_house.HouseName = (string)dbFactory.ToModelValue(reader, "HOUSENAME");
            t_house.HouseType = dbFactory.ToModelValue(reader, "HOUSETYPE").ToInt32();
            t_house.ContactUser = (string)dbFactory.ToModelValue(reader, "CONTACTUSER");
            t_house.ContactPhone = (string)dbFactory.ToModelValue(reader, "CONTACTPHONE");
            t_house.AreaCount = dbFactory.ToModelValue(reader, "AREACOUNT").ToInt32();
            t_house.AreaUsingCount = dbFactory.ToModelValue(reader, "AREAUSINGCOUNT").ToInt32();
            t_house.Address = (string)dbFactory.ToModelValue(reader, "ADDRESS");
            t_house.LocationDesc = (string)dbFactory.ToModelValue(reader, "LOCATIONDESC");
            t_house.HouseStatus = dbFactory.ToModelValue(reader, "HOUSESTATUS").ToInt32();
            t_house.WarehouseID = dbFactory.ToModelValue(reader, "WAREHOUSEID").ToInt32();
            t_house.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToInt32();
            t_house.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_house.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_house.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_house.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");


            if (Common_Func.readerExists(reader, "WarehouseNo")) t_house.WarehouseNo = reader["WarehouseNo"].ToDBString();
            if (Common_Func.readerExists(reader, "WarehouseName")) t_house.WarehouseName = reader["WarehouseName"].ToDBString();
            if (Common_Func.readerExists(reader, "StrHouseStatus")) t_house.StrHouseStatus = reader["StrHouseStatus"].ToDBString();
            if (Common_Func.readerExists(reader, "StrHouseType")) t_house.StrHouseType = reader["StrHouseType"].ToDBString();
            if (Common_Func.readerExists(reader, "StrFloorType")) t_house.StrFloorType = reader["StrFloorType"].ToDBString();

            t_house.AreaRate = t_house.AreaCount >= 1 ? t_house.AreaUsingCount.ToDecimal() / t_house.AreaCount.ToDecimal() : 0;

            t_house.FloorType = dbFactory.ToModelValue(reader, "FloorType").ToInt32();
            t_house.MaterialClassCode = dbFactory.ToModelValue(reader, "MaterialClassCode").ToDBString();
            t_house.MaterialClassName = dbFactory.ToModelValue(reader, "MaterialClassName").ToDBString();
            t_house.HouseProp = dbFactory.ToModelValue(reader, "HouseProp").ToInt32();
            t_house.StrHouseProp = dbFactory.ToModelValue(reader, "StrHouseProp").ToDBString();

            return t_house;
        }

        protected override string GetViewName()
        {
            return "V_HOUSE";
        }

        protected override string GetTableName()
        {
            return "T_HOUSE";
        }

        protected override string GetSaveProcedureName()
        {
            return "P_SAVE_T_HOUSE";
        }

        protected override string GetDeleteProcedureName()
        {
            return "P_DELETE_T_HOUSE";
        }

        protected override string GetFilterSql(UserModel user, T_HouseInfo model)
        {
            string strSql = " where isnull(isDel,0) != 2";

            string strAnd = " and ";
            if (model.WarehouseID > 0) 
            {
                strSql += strAnd;
                strSql += " warehouseid = '"+model.WarehouseID+"'  ";
            }

            if (!Common_Func.IsNullOrEmpty(model.WarehouseNo))
            {
                strSql += strAnd;
                strSql += " (WarehouseNo LIKE '%" + model.WarehouseNo + "%' OR WarehouseName Like '%" + model.WarehouseNo + "%')  ";
            }

            if (!Common_Func.IsNullOrEmpty(model.HouseNo))
            {
                strSql += strAnd;
                //strSql += " ID In (Select WarehouseID From T_House Where HouseNo LIKE '%" + model.HouseNo + "%' OR HouseName Like '%" + model.HouseNo + "%') ";
                strSql += "(HouseNo LIKE '%" + model.HouseNo + "%' OR HouseName Like '%" + model.HouseNo + "%') ";
            }

            if (!Common_Func.IsNullOrEmpty(model.AreaNo))
            {
                strSql += strAnd;
                strSql += " ID In (Select houseid From t_area Where AreaNo LIKE '" + model.AreaNo + "%' OR AreaName Like '" + model.AreaNo + "%') ";
                
            }

            if (!Common_Func.IsNullOrEmpty(model.Creater))
            {
                strSql += strAnd;
                strSql += " Creater Like '%" + model.Creater + "%' ";
            }

            if (model.DateFrom != null)
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


        public int CheckHouseType(T_HouseInfo model)
        {
            string strSql = string.Format("select count(1) from t_House where housetype = '{0}' and isnull(isdel,1) = 1 and warehouseid ='{1}' and id <> '{2}' ", model.HouseType, model.WarehouseID,model.ID);
            return GetScalarBySql(strSql).ToInt32();
        }

        public int CheckHouseTypeArea(T_HouseInfo model)
        {
            string strSql = string.Format("select count(1) from t_area a where a.Houseid <> '{0}' and  isnull(a.Areatype,1) = '"+model.HouseType+"' and isnull(a.Isdel,1) = 1", model.ID);
            return GetScalarBySql(strSql).ToInt32();
        }
       
    }
}
