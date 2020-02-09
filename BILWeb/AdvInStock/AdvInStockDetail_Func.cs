using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILBasic.JSONUtil;

namespace BILWeb.AdvInStock
{

    public partial class T_AdvInStockDetail_Func : TBase_Func<T_AdvInStockDetail_DB, T_AdvInStockDetailInfo>, IAdvInStockDetailService
    {

        protected override bool CheckModelBeforeSave(T_AdvInStockDetailInfo model, ref string strError)
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
            return "预到货单表体";
        }

        protected override T_AdvInStockDetailInfo GetModelByJson(string ModelJson)
        {
            return JSONHelper.JsonToObject<T_AdvInStockDetailInfo>(ModelJson);
        }

    }
}