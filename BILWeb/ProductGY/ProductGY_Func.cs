using BILBasic.Basing.Factory;
using BILBasic.JSONUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.ProductGY
{
    public partial class Mes_ProductGY_Func : TBase_Func<Mes_ProductGY_DB, Mes_ProductGYInfo>
    {

        protected override bool CheckModelBeforeSave(Mes_ProductGYInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }
            
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "工序";
        }

        protected override Mes_ProductGYInfo GetModelByJson(string strJson)
        {
            return JSONHelper.JsonToObject<Mes_ProductGYInfo>(strJson);
        }


        public bool GetProductGYDataByID(ref Mes_ProductGYInfo model, ref string ErrMsg)
        {
            Mes_ProductGY_DB mdb = new Mes_ProductGY_DB();
            return mdb.GetProductGYDataByID(ref model, ref ErrMsg);
        }

        public bool SaveProductGYData(string userNo, List<Mes_ProductGYInfo> lst, ref string strError)
        {
            Mes_ProductGY_DB mdb = new Mes_ProductGY_DB();
            return mdb.SaveProductGYData(userNo, lst, ref strError);
        }

        public bool GetProductGYlistBygxDuanCode(string gxDuanCode, ref List<Mes_ProductGYInfo> lst, ref string machineName, ref string machineType, ref string ErrMsg)
        {
            Mes_ProductGY_DB mdb = new Mes_ProductGY_DB();
            return mdb.GetProductGYlistBygxDuanCode(gxDuanCode, ref lst, ref machineName, ref machineType, ref ErrMsg);
        }

        public bool GetMaterialInfoByNoForGYLine(string materialno, ref string materialname, ref string ErrMsg)
        {
            Mes_ProductGY_DB mdb = new Mes_ProductGY_DB();
            return mdb.GetMaterialInfoByNoForGYLine(materialno, ref materialname, ref ErrMsg);
        }

        public bool GetAllProductGYLineList(Mes_ProductGYLineInfo model, ref List<Mes_ProductGYLineInfo> lst, ref string ErrMsg)
        {
            Mes_ProductGY_DB mdb = new Mes_ProductGY_DB();
            return mdb.GetAllProductGYLineList(model, ref lst, ref ErrMsg);
        }

    }
}
