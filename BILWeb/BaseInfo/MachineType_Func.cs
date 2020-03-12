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
    public partial class T_MachineType_Func : TBase_Func<T_MachineType_DB, T_MachineType>
    {
        public bool SaveData(T_MachineType model, ref string strError)
        {
            T_MachineType_DB db = new T_MachineType_DB();
            return db.SaveData(model, ref strError);
        }

        protected override bool CheckModelBeforeSave(T_MachineType model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.MachineTypeCode))
            {
                strError = "设备型号编号不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.MachineTypeName))
            {
                strError = "设备型号名称不能为空！";
                return false;
            }
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "设备型号";
        }

        protected override T_MachineType GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDeviceByID(T_MachineType model, ref string strError)
        {
            T_MachineType_DB db = new T_MachineType_DB();
            return db.DelData(model, ref strError);
        }

    }
}
