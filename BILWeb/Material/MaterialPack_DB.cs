using BILBasic.DBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;
using BILBasic.User;
using System.Data;

namespace BILWeb.Material
{
    public partial class MaterialPack_DB : BILBasic.Basing.Factory.Base_DB<MaterialPack_Model>
    {
        protected override IDataParameter[] GetSaveModelIDataParameter(MaterialPack_Model model)
        {
            throw new NotImplementedException();
        }

        protected override MaterialPack_Model ToModel(IDataReader reader)
        {
            MaterialPack_Model materialPack = new MaterialPack_Model();

            materialPack.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            materialPack.HeaderID = dbFactory.ToModelValue(reader, "HeaderID").ToInt32();
            materialPack.MATERIALNO = (string)dbFactory.ToModelValue(reader, "MATERIALNO");
            materialPack.MATERIALDESC = (string)dbFactory.ToModelValue(reader, "MATERIALDESC");
            materialPack.QUALITYDAY = dbFactory.ToModelValue(reader, "QUALITYDAY").ToInt32();
            materialPack.QUALITYMON = dbFactory.ToModelValue(reader, "QUALITYMON").ToInt32();
            materialPack.QTY = dbFactory.ToModelValue(reader, "QTY").ToInt32();
            materialPack.UNIT = (string)dbFactory.ToModelValue(reader, "UNIT");
            materialPack.WATERCODE = (string)dbFactory.ToModelValue(reader, "WATERCODE");
            materialPack.ISDEL = dbFactory.ToModelValue(reader, "ISDEL").ToInt32();
            materialPack.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "STRONGHOLDNAME");
            materialPack.StrongHoldName = (string)dbFactory.ToModelValue(reader, "STRONGHOLDCODE");
            materialPack.CompanyCode = (string)dbFactory.ToModelValue(reader, "COMPANYCODE");
            materialPack.UNITNUM = Convert.ToDecimal(dbFactory.ToModelValue(reader, "UNITNUM"));
            return materialPack;

        }

        protected override string GetFilterSql(UserModel user, MaterialPack_Model model)
        {

            string strSql = string.Empty;
            string strAnd = " and ";
            strSql += base.GetFilterSql(user, model);
            if (!Common_Func.IsNullOrEmpty(model.WATERCODE))
            {
                strSql += strAnd;
                strSql += " (WATERCODE = '" + model.WATERCODE + "' ) ";
            }

            if (!Common_Func.IsNullOrEmpty(model.StrongHoldCode))
            {
                strSql += strAnd;
                strSql += " (StrongHoldCode = '" + model.StrongHoldCode + "' ) ";
            }
            return strSql;
        }
        protected override string GetViewName()
        {
            return "V_MaterialPack";
        }

        protected override string GetTableName()
        {
            return "t_material_pack";
        }

        protected override string GetSaveProcedureName()
        {
            throw new NotImplementedException();
        }

        protected override List<string> GetSaveSql(BILBasic.User.UserModel user, ref MaterialPack_Model model)
        {
            throw new NotImplementedException();
        }


        public string GetQUALITYDAY(string materialno,ref string strError)
        {
            strError = "";
            try
            {
                string strSql = "select QUALITYDAY from V_MaterialPack where materialno='"+ materialno + "'";
                return GetScalarBySql(strSql).ToString();
            }
            catch (Exception ex)
            {
                strError = ex.Message + ex.TargetSite;
                return "";
            }
        }

    }
}
