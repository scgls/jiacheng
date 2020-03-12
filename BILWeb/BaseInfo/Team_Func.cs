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
    public partial class T_Team_Func : TBase_Func<T_Team_DB, T_Team>
    {
        public bool SaveData(T_Team model, ref string strError)
        {
            T_Team_DB db = new T_Team_DB();
            return db.SaveData(model, ref strError);
        }

        public bool DeleteProductLineByID(T_Team model, ref string strError)
        {
            T_Team_DB db = new T_Team_DB();
            return db.DelData(model, ref strError);
        }

        protected override bool CheckModelBeforeSave(T_Team model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.teamCode))
            {
                strError = "班组编号不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.teamName))
            {
                strError = "班组名称不能为空！";
                return false;
            }
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "班组";
        }

        protected override T_Team GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

    }
}
