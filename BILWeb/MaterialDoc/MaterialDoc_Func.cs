using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;

namespace BILWeb.MaterialDoc
{

    public partial class T_Material_Doc_Func : TBase_Func<T_Material_Doc_DB, T_MaterialDoc_Info>
    {

        protected override bool CheckModelBeforeSave(T_MaterialDoc_Info model, ref string strError)
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
            return "物料凭证";
        }

        protected override T_MaterialDoc_Info GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

    }
}