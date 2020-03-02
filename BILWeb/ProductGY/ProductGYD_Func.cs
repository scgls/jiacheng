using BILBasic.Basing.Factory;
using BILBasic.JSONUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.ProductGY
{
    public partial class Mes_ProductGYD_Func : TBase_Func<Mes_ProductGYD_DB, Mes_ProductGYDInfo>
    {

        protected override bool CheckModelBeforeSave(Mes_ProductGYDInfo model, ref string strError)
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

        protected override Mes_ProductGYDInfo GetModelByJson(string strJson)
        {
            return JSONHelper.JsonToObject<Mes_ProductGYDInfo>(strJson);
        }


        public bool SaveProductGYDData(string userNo, Mes_ProductGYDInfo entityGYD, ref string strError)
        {
            Mes_ProductGYD_DB mdb = new Mes_ProductGYD_DB();
            return mdb.SaveProductGYDData(userNo, entityGYD, ref strError);
        }

        public bool GetProductGYListDataBygxDuanCode(string gxDuanCode, ref Mes_ProductGYDInfo entityGYD, ref string ErrMsg)
        {
            Mes_ProductGYD_DB mdb = new Mes_ProductGYD_DB();
            return mdb.GetProductGYListDataBygxDuanCode(gxDuanCode, ref entityGYD, ref ErrMsg);
        }

        public bool GetProductGYDAllListData(Mes_ProductGYDInfo model, ref List<Mes_ProductGYDInfo> lst, ref string ErrMsg)
        {
            Mes_ProductGYD_DB mdb = new Mes_ProductGYD_DB();
            return mdb.GetProductGYDAllListData(model, ref lst, ref ErrMsg);
        }

        public bool DeleteProductGYDInfo(string gxDuanCode, int isdel, ref string ErrMsg)
        {
            Mes_ProductGYD_DB mdb = new Mes_ProductGYD_DB();
            return mdb.DeleteProductGYDInfo(gxDuanCode, isdel, ref ErrMsg);
        }

    }
}
