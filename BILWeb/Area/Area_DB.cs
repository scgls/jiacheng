//************************************************************
//******************************作者：方颖*********************
//******************************时间：2016/11/4 15:46:48*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using BILBasic.Common;
using BILBasic.User;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BILWeb.Area
{
    public partial class T_Area_DB : BILBasic.Basing.Factory.Base_DB<T_AreaInfo>
    {

        /// <summary>
        /// 添加T_AREA
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_AreaInfo model)
        {   

                //


            dbFactory.dbF.CreateParameters(31);
            dbFactory.dbF.AddParameters(0, "@bResult", SqlDbType.Int, 0);
            dbFactory.dbF.AddParameters(1, "@ErrorMsg", SqlDbType.NVarChar, 1000);
            dbFactory.dbF.AddParameters(2, "@v_ID", model.ID.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(3, "@v_AreaNo", model.AreaNo.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(4, "@v_AreaName", model.AreaName.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(5, "@v_AreaType", model.AreaType.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(6, "@v_ContactUser", model.ContactUser.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(7, "@v_ContactPhone", model.ContactPhone.ToOracleValue(), 0);
            
            dbFactory.dbF.AddParameters(8, "@v_Address", model.Address.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(9, "@v_LocationDesc", model.LocationDesc.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(10, "@v_AreaStatus", model.AreaStatus.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(11, "@v_HouseID", model.HouseID.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(12, "@v_IsDel", model.IsDel.ToOracleValue(), 0);

            dbFactory.dbF.AddParameters(13, "@v_Creater", model.Creater.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(14, "@v_CreateTime", model.CreateTime.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(15, "@v_Modifyer", model.Modifyer.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(16, "@v_ModifyTime", model.ModifyTime.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(17, "@v_Length", model.Length.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(18, "@v_Wide", model.Wide.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(19, "@v_Hight", model.Hight.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(20, "@v_WeightLimit", model.WeightLimit.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(21, "@v_VolumeLimit", model.VolumeLimit.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(22, "@v_QuantityLimit", model.QuantityLimit.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(23, "@v_PalletLimit", model.PalletLimit.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(24, "@v_TemperaturePry", model.TemperaturePry.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(25, "@v_CustomerNo", model.CustomerNo.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(26, "@v_CustomerName", model.CustomerName.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(27, "@v_ProjectNo", model.ProjectNo.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(28, "@v_IEscrow", model.IsDel.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(29, "@v_HeightArea", model.HeightArea.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(30, "@v_SortArea", model.SortArea.ToOracleValue(), 0);

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
            //   new OracleParameter("@v_AreaNo", model.AreaNo.ToOracleValue()),
            //   new OracleParameter("@v_AreaName", model.AreaName.ToOracleValue()),
            //   new OracleParameter("@v_AreaType", model.AreaType.ToOracleValue()),
            //   new OracleParameter("@v_ContactUser", model.ContactUser.ToOracleValue()),
            //   new OracleParameter("@v_ContactPhone", model.ContactPhone.ToOracleValue()),
            //   new OracleParameter("@v_Address", model.Address.ToOracleValue()),
            //   new OracleParameter("@v_LocationDesc", model.LocationDesc.ToOracleValue()),
            //   new OracleParameter("@v_AreaStatus", model.AreaStatus.ToOracleValue()),
            //   new OracleParameter("@v_HouseID", model.HouseID.ToOracleValue()),
            //   new OracleParameter("@v_IsDel", model.IsDel.ToOracleValue()),
            //   new OracleParameter("@v_Creater", model.Creater.ToOracleValue()),
            //   new OracleParameter("@v_CreateTime", model.CreateTime.ToOracleValue()),
            //   new OracleParameter("@v_Modifyer", model.Modifyer.ToOracleValue()),
            //   new OracleParameter("@v_ModifyTime", model.ModifyTime.ToOracleValue()),

            //   new OracleParameter("@v_Length", model.Length.ToOracleValue()),
            //   new OracleParameter("@v_Wide", model.Wide.ToOracleValue()),
            //   new OracleParameter("@v_Hight", model.Hight.ToOracleValue()),
            //   new OracleParameter("@v_WeightLimit", model.WeightLimit.ToOracleValue()),
            //   new OracleParameter("@v_VolumeLimit", model.VolumeLimit.ToOracleValue()),
            //   new OracleParameter("@v_QuantityLimit", model.QuantityLimit.ToOracleValue()),
            //   new OracleParameter("@v_PalletLimit", model.PalletLimit.ToOracleValue()),
            //   new OracleParameter("@v_TemperaturePry", model.TemperaturePry.ToOracleValue()),
            //   new OracleParameter("@v_CustomerNo", model.CustomerNo.ToOracleValue()),
            //   new OracleParameter("@v_CustomerName", model.CustomerName.ToOracleValue()),
            //   new OracleParameter("@v_ProjectNo", model.ProjectNo.ToOracleValue()),
            //   new OracleParameter("@v_IEscrow", model.IsDel.ToOracleValue()),
            //   new OracleParameter("@v_HeightArea", model.HeightArea.ToOracleValue()),
            //   new OracleParameter("@v_SortArea", model.SortArea.ToOracleValue()),
            //  };

            //param[0].Direction = System.Data.ParameterDirection.Output;
            //param[1].Direction = System.Data.ParameterDirection.Output;
            //param[2].Direction = System.Data.ParameterDirection.InputOutput;


            //return param;
        }

        protected override List<string> GetSaveSql(UserModel user,ref T_AreaInfo t_area)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_AreaInfo ToModel(IDataReader reader)
        {
            T_AreaInfo t_area = new T_AreaInfo();

            t_area.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_area.AreaNo = (string)dbFactory.ToModelValue(reader, "AREANO");
            t_area.AreaName = (string)dbFactory.ToModelValue(reader, "AREANAME");
            t_area.AreaType = dbFactory.ToModelValue(reader, "AREATYPE").ToInt32();
            t_area.ContactUser = (string)dbFactory.ToModelValue(reader, "CONTACTUSER");
            t_area.ContactPhone = (string)dbFactory.ToModelValue(reader, "CONTACTPHONE");
            t_area.Address = (string)dbFactory.ToModelValue(reader, "ADDRESS");
            t_area.LocationDesc = (string)dbFactory.ToModelValue(reader, "LOCATIONDESC");
            t_area.AreaStatus = dbFactory.ToModelValue(reader, "AREASTATUS").ToInt32();
            t_area.HouseID = dbFactory.ToModelValue(reader, "HOUSEID").ToInt32();
            t_area.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToInt32();
            t_area.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_area.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_area.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_area.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_area.CheckID = dbFactory.ToModelValue(reader, "CHECKID").ToInt32();

            t_area.Length = dbFactory.ToModelValue(reader, "Length").ToDecimal();
            t_area.Wide = dbFactory.ToModelValue(reader, "Wide").ToDecimal();
            t_area.Hight = dbFactory.ToModelValue(reader, "Hight").ToDecimal();
            t_area.WeightLimit = dbFactory.ToModelValue(reader, "WeightLimit").ToDecimal();
            t_area.VolumeLimit = dbFactory.ToModelValue(reader, "VolumeLimit").ToDecimal();
            t_area.QuantityLimit = dbFactory.ToModelValue(reader, "QuantityLimit").ToDecimal();
            t_area.PalletLimit = dbFactory.ToModelValue(reader, "PalletLimit").ToDecimal();
            t_area.TemperaturePry = dbFactory.ToModelValue(reader, "TemperaturePry").ToDBString();
            t_area.CustomerNo = dbFactory.ToModelValue(reader, "CustomerNo").ToDBString();
            t_area.CustomerName = dbFactory.ToModelValue(reader, "CustomerName").ToDBString();
            t_area.ProjectNo = dbFactory.ToModelValue(reader, "ProjectNo").ToDBString();
            t_area.IEscrow = dbFactory.ToModelValue(reader, "IEscrow").ToInt32();

            if (Common_Func.readerExists(reader, "HouseNo")) t_area.HouseNo = reader["HouseNo"].ToDBString();
            if (Common_Func.readerExists(reader, "HouseName")) t_area.HouseName = reader["HouseName"].ToDBString();
            if (Common_Func.readerExists(reader, "WarehouseNo")) t_area.WarehouseNo = reader["WarehouseNo"].ToDBString();
            if (Common_Func.readerExists(reader, "WarehouseName")) t_area.WarehouseName = reader["WarehouseName"].ToDBString();
            if (Common_Func.readerExists(reader, "StrAreaStatus")) t_area.StrAreaStatus = reader["StrAreaStatus"].ToDBString();
            if (Common_Func.readerExists(reader, "StrAreaType")) t_area.StrAreaStatus = reader["StrAreaType"].ToDBString();
            
            if (Common_Func.readerExists(reader, "CheckID")) t_area.CheckID = reader["CheckID"].ToInt32();

            t_area.StrCreateTime = t_area.CreateTime.ToShowTime();
            t_area.StrModifyTime = t_area.ModifyTime.ToShowTime();

            t_area.WarehouseID = dbFactory.ToModelValue(reader, "WarehouseID").ToInt32();
            t_area.HeightArea = dbFactory.ToModelValue(reader, "HeightArea").ToInt32();
            t_area.StrHeightArea = dbFactory.ToModelValue(reader, "StrHeightArea").ToDBString();
            t_area.QuanUserNo = dbFactory.ToModelValue(reader, "Samplercode").ToDBString();
            t_area.QuanUserName = dbFactory.ToModelValue(reader, "Samplername").ToDBString();
            t_area.SortArea = dbFactory.ToModelValue(reader, "SortArea").ToDBString();
            t_area.StrAreaStatus = dbFactory.ToModelValue(reader, "StrAreaStatus").ToDBString();
            t_area.StrAreaType = dbFactory.ToModelValue(reader, "StrAreaType").ToDBString();
            t_area.HouseProp = dbFactory.ToModelValue(reader, "HouseProp").ToDBString();

            return t_area;

        }

        protected override string GetViewName()
        {
            return "V_AREA";
        }

        protected override string GetTableName()
        {
            return "T_AREA";
        }

        protected override string GetSaveProcedureName()
        {
            return "P_SAVE_T_AREA";
        }

        protected override string GetDeleteProcedureName()
        {
            return "P_DELETE_T_AREA";
        }

        protected override string GetFilterSql(UserModel user, T_AreaInfo model)
        {
            string strSql = " where isnull(isDel,0) != 2";

            string strAnd = " and ";

            if (model.HouseID > 0)
            {
                strSql += strAnd;
                strSql += " HouseID = '" + model.HouseID + "'  ";
            }

            if (!string.IsNullOrEmpty(model.AreaNo))
            {
                strSql += strAnd;
                strSql += " areano like '" + model.AreaNo + "%'  ";
            }

            if (!Common_Func.IsNullOrEmpty(model.WarehouseNo))
            {
                strSql += strAnd;
                strSql += "  (WarehouseNo LIKE '%" + model.WarehouseNo + "%' OR WarehouseName Like '%" + model.WarehouseNo + "%')  ";
            }

            if (!Common_Func.IsNullOrEmpty(model.HouseNo))
            {
                strSql += strAnd;
                strSql += "  (HouseNo LIKE '%" + model.HouseNo + "%' OR HouseName Like '%" + model.HouseNo + "%') ";
            }

            //if (!Common_Func.IsNullOrEmpty(model.Creater))
            //{
            //    strSql += strAnd;
            //    strSql += " Creater Like '%" + model.Creater + "%' ";
            //}

            //if (model.DateFrom != null)
            //{
            //    strSql += strAnd;
            //    strSql += " CreateTime >= " + model.DateFrom.ToDateTime().Date.ToOracleTimeString() + "  ";
            //}

            //if (model.DateTo != null)
            //{
            //    strSql += strAnd;
            //    strSql += " CreateTime <= " + model.DateTo.ToDateTime().AddDays(1).Date.ToOracleTimeString() + " ";
            //}

            return strSql;
        }
      

        protected override string GetModelSql(T_AreaInfo model)
        {
            if (model.WarehouseID == null || model.WarehouseID ==0)
            {
                return string.Format("select * from v_area where areano = '{0}' and isdel = 1 ", model.AreaNo);
            }
            else
            {
                return string.Format("select * from v_area where areano = '{0}' and isdel = 1 and WAREHOUSEID = '{1}'", model.AreaNo, model.WarehouseID);
            }
          
        }



        /// <summary>
        /// 根据仓库ID获取收货库位和拣货清点货位
        /// </summary>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        public List<T_AreaInfo> GetAreaModelList(int WareHouseID)
        {
            try
            {
                string strSql = "select * from v_Area a where a.warehouseid = '" + WareHouseID + "' and  ( areatype = 2 or areatype = 3 or areatype = 4 )";
                List<T_AreaInfo> modelList = base.GetModelListBySql(strSql);
                if (modelList == null || modelList.Count == 0)
                {
                    return null;
                }
                else
                {
                    return modelList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public int CheckAreaType(T_AreaInfo model)
        {
            string strSql = string.Format("select * from v_area where areatype = '{0}' and isnull(isdel,1) = 1 and warehouseid = (select warehouseid from t_house where id='{1}') and v_area.ID <> '{2}' ", model.AreaType, model.HouseID, model.ID);
            return GetScalarBySql(strSql).ToInt32();
        }

        public int GetHouseTypeByAreaNo(T_AreaInfo mdoel) 
        {
            string strSql = string.Format("select housetype from t_house where id = '{0}' ", mdoel.HouseID);
            return GetScalarBySql(strSql).ToInt32();
        }

    }
}
