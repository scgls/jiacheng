using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILWeb.BaseInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.BaseInfo
{
    public partial class T_Modl_Func : TBase_Func<T_Modl_DB, T_Modl>
    {
        public bool SaveData(T_Modl model, ref string strError)
        {
            T_Modl_DB db = new T_Modl_DB();
            return db.SaveData(model, ref strError);
        }

        protected override bool CheckModelBeforeSave(T_Modl model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.Sn))
            {
                strError = "模具编号不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.MouldName))
            {
                strError = "模具名称不能为空！";
                return false;
            }
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "模具";
        }

        protected override T_Modl GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }
        public bool DeletModlByID(T_Modl model, ref string strError)
        {
            T_Modl_DB db = new T_Modl_DB();
            return db.DelData(model, ref strError);
        }
    }
}
