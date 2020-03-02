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
    public partial class T_System_Func
    {
        public bool SaveData(T_System model, ref string strError)
        {
            T_System_DB db = new T_System_DB();
            return db.SaveData(model, ref strError);
        }

        public List<T_System> GetModel()
        {
            T_System_DB db = new T_System_DB();
            return db.GetModel();
        }

    }
}
