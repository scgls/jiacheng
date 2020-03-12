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
    public partial class T_Machine_Func : TBase_Func<T_Machine_DB, T_Machine>
    {
        public bool SaveData(T_Machine model, ref string strError)
        {
            T_Machine_DB db = new T_Machine_DB();
            return db.SaveData(model, ref strError);
        }

        protected override bool CheckModelBeforeSave(T_Machine model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.Sn))
            {
                strError = "设备编号不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.MachineName))
            {
                strError = "设备名称不能为空！";
                return false;
            }
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "设备";
        }

        protected override T_Machine GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDeviceByID(T_Machine model, ref string strError)
        {
            T_Machine_DB db = new T_Machine_DB();
            return db.DelData(model, ref strError);
        }

    }
}
