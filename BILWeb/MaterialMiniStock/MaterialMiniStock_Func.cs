using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;

namespace BILWeb.MaterialMiniStock
{

    public partial class T_Material_MiniStock_Func : TBase_Func<T_Material_MiniStock_DB, T_Material_MiniStockInfo>
    {

        protected override bool CheckModelBeforeSave(T_Material_MiniStockInfo model, ref string strError)
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
            return "库存下限";
        }

        protected override T_Material_MiniStockInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

    }
}