using BILBasic.Basing.Factory;
using BILBasic.JSONUtil;
using BILWeb.OutBarCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILWeb.Stock;

namespace BILWeb.Pallet
{
    public partial class T_Pallet_Func : TBase_Func<T_Pallet_DB, T_PalletInfo>
    {

        protected override bool CheckModelBeforeSave(T_PalletInfo model, ref string strError)
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
            return "托盘表头";
        }

        protected override T_PalletInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

        
    }
}
