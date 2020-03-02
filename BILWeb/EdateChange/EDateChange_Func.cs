using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILWeb.Login.User;

namespace BILWeb.EdateChange
{

    public partial class T_EDateChange_Func : TBase_Func<T_EDateChange_DB, T_EDateChangeInfo>
    {

        protected override bool CheckModelBeforeSave(T_EDateChangeInfo model, ref string strError)
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
            return "效期变更单表头";
        }

        protected override T_EDateChangeInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

    }
}