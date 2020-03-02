using BILBasic.Basing.Factory;
using BILBasic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Customer
{
    public partial class T_CustomerAddress_Func : TBase_Func<T_CustomerAddress_DB, T_CustomerAddressInfo>
    {

        protected override bool CheckModelBeforeSave(T_CustomerAddressInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.ContactPerson))
            {
                strError = "联系人不能为空！";
                return false;
            }
            if (Common_Func.IsNullOrEmpty(model.Address))
            {
                strError = "地址不能为空！";
                return false;
            }
            
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "客户地址";
        }

        protected override T_CustomerAddressInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

    }
}
