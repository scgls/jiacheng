//************************************************************
//******************************作者：方颖*********************
//******************************时间：2019/9/9 17:03:31*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using BILBasic.User;
using BILBasic.Common;
using System.Data;


namespace BILWeb.MaterialMiniStock
{
    public partial class T_Material_MiniStock_DB : BILBasic.Basing.Factory.Base_DB<T_Material_MiniStockInfo>
    {

        /// <summary>
        /// 添加t_material_ministock
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_Material_MiniStockInfo t_material_ministock)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user ,ref T_Material_MiniStockInfo t_material_ministock)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_Material_MiniStockInfo ToModel(IDataReader reader)
        {
            T_Material_MiniStockInfo t_material_ministock = new T_Material_MiniStockInfo();

            t_material_ministock.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "StrongHoldCode");
            t_material_ministock.WarehouseID = (decimal?)dbFactory.ToModelValue(reader, "WAREHOUSEID");
            t_material_ministock.MaterialNo = (string)dbFactory.ToModelValue(reader, "MATERIALNO");
            t_material_ministock.MaterialNoID = dbFactory.ToModelValue(reader, "MATERIALNOID").ToInt32();
            t_material_ministock.MiniQty = (decimal?)dbFactory.ToModelValue(reader, "MINIQTY");
            t_material_ministock.StrongHoldName = (string)dbFactory.ToModelValue(reader, "STRONGHOLDNAME");
            t_material_ministock.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_material_ministock.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_material_ministock.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_material_ministock.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_material_ministock.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_material_ministock.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToInt32();
            return t_material_ministock;
        }

        protected override string GetViewName()
        {
            return "v_Material_Ministock";
        }

        protected override string GetTableName()
        {
            return "T_MATERIAL_MINISTOCK";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }


    }
}
