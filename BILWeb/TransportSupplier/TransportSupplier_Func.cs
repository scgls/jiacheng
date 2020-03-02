using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILWeb.Login.User;
using BILWeb.TransportSupplier;

namespace BILWeb.TransportSupplier
{

    public partial class T_TransportSupplier_Func : TBase_Func<T_TransportSupplier_DB, T_TransportSupplier>
    {
        protected override bool CheckModelBeforeSave(T_TransportSupplier model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (BILBasic.Common.Common_Func.IsNullOrEmpty(model.TransportSupplierID.ToString()))
            {
                strError = "承运商编号不能为空！";
                return false;
            }

            if (BILBasic.Common.Common_Func.IsNullOrEmpty(model.TransportSupplierName))
            {
                strError = "承运商名称不能为空！";
                return false;
            }

            return true;
        }

        protected override string GetModelChineseName()
        {
            return "承运商";
        }

        
        
    }
}