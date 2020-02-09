using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.RetentionChange
{
    public partial class T_RetentionChange_Func : TBase_Func<T_RetentionChange_DB, T_RetentionChangeInfo>
    {

        protected override bool CheckModelBeforeSave(T_RetentionChangeInfo model, ref string strError)
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
            return "库存留置单";
        }

        protected override T_RetentionChangeInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

    }
}
