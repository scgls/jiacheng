using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Supplier
{
    public partial class T_SupplierAddress_Func : TBase_Func<T_SupplierAddress_DB, T_SupplierAddressInfo>
    {

        protected override bool CheckModelBeforeSave(T_SupplierAddressInfo model, ref string strError)
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
            return "供应商";
        }

        protected override T_SupplierAddressInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

    }
}
