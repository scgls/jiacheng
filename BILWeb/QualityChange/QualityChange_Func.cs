using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILWeb.Login.User;

namespace BILWeb.QualityChange
{

    public partial class T_QualityChange_Func : TBase_Func<T_QualityChange_DB, T_QualityChangeInfo>
    {

        protected override bool CheckModelBeforeSave(T_QualityChangeInfo model, ref string strError)
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
            return "质量变更单";
        }

        protected override T_QualityChangeInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

    }
}
