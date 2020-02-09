using BILBasic.Basing.Factory;
using BILBasic.JSONUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Material
{
    public partial class MaterialPack_Func : TBase_Func<MaterialPack_DB, MaterialPack_Model>, IMaterialService
    {
        protected override bool CheckModelBeforeSave(MaterialPack_Model model, ref string strError)
        {
            throw new NotImplementedException();
        }

        protected override string GetModelChineseName()
        {
            throw new NotImplementedException();
        }

        public bool GetModelListByPage(ref List<T_MaterialInfo> gmodelList, BILBasic.User.UserModel userInfo, T_MaterialInfo model, ref BILBasic.Common.DividPage page, ref string strMsg)
        {
            throw new NotImplementedException();
        }

        public bool GetModelByID(ref T_MaterialInfo model, ref string strMsg)
        {
            throw new NotImplementedException();
        }

        public bool SaveModelBySqlToDB(BILBasic.User.UserModel userInfo, ref T_MaterialInfo model, ref string strMsg)
        {
            throw new NotImplementedException();
        }

        public bool DeleteModelByModelSql(BILBasic.User.UserModel userInfo, T_MaterialInfo model, ref string strMsg)
        {
            throw new NotImplementedException();
        }

        public bool SaveModelToDB(BILBasic.User.UserModel userInfo, ref T_MaterialInfo model, ref string strMsg)
        {
            throw new NotImplementedException();
        }

        protected override MaterialPack_Model GetModelByJson(string ModelJson)
        {
            return JSONHelper.JsonToObject<MaterialPack_Model>(ModelJson);
        }

        public string GetQUALITYDAY(string materialno, ref string strError)
        {
            MaterialPack_DB db = new MaterialPack_DB();
            return db.GetQUALITYDAY(materialno, ref strError);
        }
    }
}
