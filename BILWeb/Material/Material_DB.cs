using BILBasic.DBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;
using BILBasic.User;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BILWeb.Material
{
    public partial class T_Material_DB : BILBasic.Basing.Factory.Base_DB<T_MaterialInfo>
    {

        /// <summary>
        /// 添加t_material
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_MaterialInfo t_material)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();
        }

        protected override List<string> GetSaveSql(UserModel user,ref  T_MaterialInfo model)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            if (model.ID <= 0)
            {
                int voucherID = base.GetTableID("seq_material");

                model.ID = voucherID.ToInt32();

                strSql = "insert into T_MATERIAL (id, materialno, materialdesc, materialdescen, stackwarehouse, stackhouse, stackarea, length, wide, hight, volume, weight, netweight, packrule, stackrule, disrule, supplierno, suppliername, unit, UnitName, keeperno, keepername, isdangerous, isactivate, isbond, isquality, creater, createtime,isserial,partno)" +
                            "values ('" + voucherID + "', '" + model.MaterialNo + "','" + model.MaterialDesc + "','" + model.MaterialDescEN + "','" + model.StackWareHouse + "','" + model.StackHouse + "','" + model.StackArea + "'," +
                            "'" + model.Length + "','" + model.Wide + "','" + model.Hight + "', '" + model.Volume + "','" + model.Weight + "','" + model.NetWeight + "', '" + model.PackRule + "','" + model.StackRule + "','"+model.DisRule+"','" + model.SupplierNo + "'," +
                            "'" + model.SupplierName + "','" + model.Unit + "','" + model.UnitName + "','" + model.KeeperNo + "','" + model.KeeperName + "','" + model.IsDangerous + "','" + model.IsActivate + "','" + model.IsBond + "','" + model.IsQuality + "'," +
                            "'" + user.UserNo + "',sysdate,'"+model.IsSerial+"','"+model.PartNo+"')";
                lstSql.Add(strSql);
            }
            else 
            {
                 strSql = "update t_Material a set a.Materialno = '" + model.MaterialNo + "',a.Materialdesc =  '" + model.MaterialDesc + "',a.Materialdescen= '" + model.MaterialDescEN + "',a.Stackwarehouse= '" + model.StackWareHouse + "',a.Stackhouse= '" + model.StackHouse + "',a.Stackarea= '" + model.StackArea + "',a.Length= '" + model.Length + "',a.Wide= '" + model.Wide + "',a.Hight= '" + model.Hight + "'," +
                                "a.Volume = '" + model.Volume + "' ,a.Weight = '" + model.Weight + "' ,a.Netweight= '" + model.NetWeight + "',a.Packrule= '" + model.PackRule + "',a.Stackrule= '" + model.StackRule + "',a.Disrule= '" + model.DisRule + "',a.Supplierno= '" + model.SupplierNo + "',a.Suppliername= '" + model.SupplierName + "',a.Unit= '" + model.Unit + "',a.Unitname= '" + model.UnitName + "',a.Keeperno= '" + model.KeeperNo + "',a.Keepername= '" + model.KeeperName + "'," +
                                "a.Isdangerous= '" + model.IsDangerous + "',a.Isactivate= '" + model.IsActivate + "',a.Isquality= '" + model.IsQuality + "',a.Modifyer= '" + user.Modifyer + "',a.Modifytime=Sysdate,a.isserial = '"+model.IsSerial+"' ,a.partno = '"+model.PartNo+"' where a.Id = '" + model.ID + "'";
                 lstSql.Add(strSql);
            }
            
            return lstSql;
        }

        protected override List<string> GetDeleteSql(UserModel user, T_MaterialInfo model)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            strSql = "delete t_Material where id = '" + model.ID + "'";
            lstSql.Add(strSql);

            strSql = "delete t_Material_Batch where headerid = '"+model.ID+"'";
            lstSql.Add(strSql);

            return lstSql;
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_MaterialInfo ToModel(IDataReader reader)
        {
            T_MaterialInfo t_material = new T_MaterialInfo();

            t_material.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_material.MaterialNo = (string)dbFactory.ToModelValue(reader, "MATERIALNO");
            t_material.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MATERIALDESC");
            t_material.MaterialDescEN = (string)dbFactory.ToModelValue(reader, "MATERIALDESCEN");
            t_material.StackWareHouse = dbFactory.ToModelValue(reader, "STACKWAREHOUSE").ToInt32();
            t_material.StackHouse = dbFactory.ToModelValue(reader, "STACKHOUSE").ToInt32();
            t_material.StackArea = dbFactory.ToModelValue(reader, "STACKAREA").ToInt32();
            t_material.Length =  dbFactory.ToModelValue(reader, "LENGTH").ToDecimal();
            t_material.Wide = dbFactory.ToModelValue(reader, "WIDE").ToDecimal();
            t_material.Hight = dbFactory.ToModelValue(reader, "HIGHT").ToDecimal();
            t_material.Volume = dbFactory.ToModelValue(reader, "VOLUME").ToDecimal();
            t_material.Weight = dbFactory.ToModelValue(reader, "WEIGHT").ToDecimal();
            t_material.NetWeight = dbFactory.ToModelValue(reader, "NETWEIGHT").ToDecimal();
            t_material.PackRule = dbFactory.ToModelValue(reader, "PACKRULE").ToDecimal();
            t_material.StackRule = dbFactory.ToModelValue(reader, "STACKRULE").ToDecimal();
            t_material.DisRule = dbFactory.ToModelValue(reader, "DISRULE").ToDecimal();
            t_material.SupplierNo = (string)dbFactory.ToModelValue(reader, "SUPPLIERNO");
            t_material.SupplierName = (string)dbFactory.ToModelValue(reader, "SUPPLIERNAME");
            t_material.Unit = (string)dbFactory.ToModelValue(reader, "UNIT");
            t_material.UnitName = (string)dbFactory.ToModelValue(reader, "UnitName");
            t_material.KeeperNo = (string)dbFactory.ToModelValue(reader, "KEEPERNO");
            t_material.KeeperName = (string)dbFactory.ToModelValue(reader, "KEEPERNAME");
            t_material.IsDangerous = dbFactory.ToModelValue(reader, "ISDANGEROUS").ToDecimal();
            t_material.IsActivate = dbFactory.ToModelValue(reader, "ISACTIVATE").ToDecimal();
            t_material.IsBond = dbFactory.ToModelValue(reader, "ISBOND").ToDecimal();
            t_material.IsQuality = dbFactory.ToModelValue(reader, "ISQUALITY").ToDecimal();
            t_material.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_material.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_material.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_material.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_material.IsSerial = dbFactory.ToModelValue(reader, "IsSerial").ToInt32();
            t_material.PartNo = dbFactory.ToModelValue(reader, "PartNo").ToDBString();
            t_material.DisplayName = (string)dbFactory.ToModelValue(reader, "MATERIALDESC");
            t_material.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "StrongHoldCode");
            t_material.StrongHoldName = (string)dbFactory.ToModelValue(reader, "Strongholdname");
            t_material.CompanyCode = (string)dbFactory.ToModelValue(reader, "CompanyCode");
            t_material.MainTypeCode = (string)dbFactory.ToModelValue(reader, "MainTypeCode");
            t_material.MainTypeName = (string)dbFactory.ToModelValue(reader, "MainTypeName");
            t_material.PurchaseTypeCode = (string)dbFactory.ToModelValue(reader, "PurchaseTypeCode");
            t_material.PurchaseTypeName = (string)dbFactory.ToModelValue(reader, "PurchaseTypeName");
            t_material.ProductTypeCode = (string)dbFactory.ToModelValue(reader, "ProductTypeCode");
            t_material.ProductTypeName = (string)dbFactory.ToModelValue(reader, "ProductTypeName");
            t_material.QualityDay = dbFactory.ToModelValue(reader, "QualityDay").ToDecimal();
            t_material.QualityMon = dbFactory.ToModelValue(reader, "QualityMon").ToDecimal();
            t_material.Brand = (string)dbFactory.ToModelValue(reader, "Brand");
            t_material.PlaceArea = (string)dbFactory.ToModelValue(reader, "PlaceArea");
            t_material.LifeCycle = (string)dbFactory.ToModelValue(reader, "LifeCycle");
            t_material.PackQty = dbFactory.ToModelValue(reader, "PackQty").ToDecimal();
            t_material.PalletVolume = dbFactory.ToModelValue(reader, "PalletVolume").ToDecimal();
            t_material.PalletPackQty = dbFactory.ToModelValue(reader, "PalletPackQty").ToDecimal();
            t_material.PackVolume = dbFactory.ToModelValue(reader, "PackVolume").ToDecimal();
            t_material.Status = dbFactory.ToModelValue(reader, "Status").ToInt32();
            t_material.sku = (string)dbFactory.ToModelValue(reader, "sku"); 
            //t_material.WaterCode = (string)dbFactory.ToModelValue(reader, "WaterCode");//ena码

            //if (Common_Func.readerExists(reader, "Batchno")) t_material.BatchNo = reader["Batchno"].ToDBString();
            //if (Common_Func.readerExists(reader, "EDate")) t_material.EDate = reader["EDate"].ToDateTime();
            //t_material.WareHouseNo = dbFactory.ToModelValue(reader, "WareHouseNo").ToDBString();
            //t_material.AreaNo = dbFactory.ToModelValue(reader, "AreaNo").ToDBString();
            //t_material.StockQty = dbFactory.ToModelValue(reader, "StockQty").ToDecimal();
            //t_material.WareHouseID = dbFactory.ToModelValue(reader, "WareHouseID").ToInt32();
            //t_material.AreaID = dbFactory.ToModelValue(reader, "AreaID").ToInt32();

            return t_material;
        }

        protected override string GetViewName()
        {
            return "V_MATERIAL";
        }

        protected override string GetTableName()
        {
            return "T_MATERIAL";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }


        protected override string GetFilterSql(UserModel user, T_MaterialInfo model)
        {
            string strSql = string.Empty;
            string strAnd = " and ";
         
            strSql +=  base.GetFilterSql(user, model);

            if (!Common_Func.IsNullOrEmpty(model.MaterialNo))
            {
                strSql += strAnd;
                strSql += " (MaterialNo LIKE '" + model.MaterialNo + "%' )  ";
            }           

            if (!Common_Func.IsNullOrEmpty(model.MaterialDesc))
            {
                strSql += strAnd;
                strSql += " MaterialDesc Like '" + model.MaterialDesc + "%'";
            }

            if (!Common_Func.IsNullOrEmpty(model.SupplierNo))
            {
                strSql += strAnd;
                strSql += "( SupplierNo Like '" + model.SupplierNo + "%'  or SupplierName Like '" + model.SupplierNo + "%' )";
            }

            if (model.DateFrom != null)
            {
                strSql += strAnd;
                strSql += " CreateTime >= " + model.DateFrom.ToDateTime().Date.AddDays(-1).ToOracleTimeString() + "  ";
            }

            if (model.DateTo != null)
            {
                strSql += strAnd;
                strSql += " CreateTime <= " + model.DateTo.ToDateTime().Date.AddDays(1).ToOracleTimeString() + " ";
            }

            if (!string.IsNullOrEmpty(model.BatchNo))
            {
                strSql += strAnd;
                strSql += " Batchno = '"+model.BatchNo+"' ";
            }

            if (!string.IsNullOrEmpty(model.WareHouseNo))
            {
                strSql += strAnd;
                strSql += " WareHouseNo = '" + model.WareHouseNo + "' ";
            }

            if (!string.IsNullOrEmpty(model.AreaNo))
            {
                strSql += strAnd;
                strSql += " AreaNo = '" + model.AreaNo + "' ";
            }

            return strSql;
        }

        public int CheckMaterialExist(T_MaterialInfo model)
        {
            string strSql = string.Format("SELECT COUNT(1) FROM T_MATERIAL WHERE materialno = '{0}'and isnull(isdel,1) = 1  ", model.MaterialNo);
            return GetScalarBySql(strSql).ToInt32();
        }

        protected override string GetModelSql(T_MaterialInfo model)
        {
            return string.Format("select * from t_Material where MaterialNo  = '{0}' and isnull(isdel,1) = 1", model.MaterialNo);
        }

        public List<T_MaterialInfo> getListMaterial(T_MaterialInfo model)
        {
            string sql = string.Format("select t.*,tp.watercode from t_material t left join t_material_pack tp on  t.id =tp.headerid where tp.watercode ='{0}'", model.WaterCode);
            return GetModelListBySql(sql);
        }
        /// <summary>
        /// 获取物料标准包装量
        /// </summary>
        /// <param name="MaterialNoID"></param>
        /// <param name="Strongholdcode"></param>
        /// <returns></returns>
        public decimal GetMaterialPackQty(string MaterialNo,string Strongholdcode)
        {
            string strSql = string.Format("select a.Unitnum from t_Material_Pack a where a.Mateno='{0}' and unit='箱' and a.Strongholdcode='{1}'  ", MaterialNo,Strongholdcode);
            return GetScalarBySql(strSql).ToDecimal();
        }

        /// <summary>
        /// 根据物料获取EAN
        /// </summary>
        public string getEAN(string materialno)
        {
            string sql = string.Format("select watercode from t_material_pack where mateno='"+ materialno + "' and unit='PCS'");
            return GetScalarBySql(sql).ToString();
        }
    }
}
