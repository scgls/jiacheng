using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILWeb.Login.User;

namespace BILWeb.Move
{

    public partial class T_Move_Func : TBase_Func<T_Move_DB, T_MoveInfo>
    {

        protected override bool CheckModelBeforeSave(T_MoveInfo model, ref string strError)
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
            return "移库单表头";
        }

        protected override T_MoveInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

    }
}