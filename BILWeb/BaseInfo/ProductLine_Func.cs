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
    public partial class T_ProductLine_Func : TBase_Func<T_ProductLine_DB, T_ProductLine>
    {
        public bool SaveData(T_ProductLine model, ref string strError)
        {
            T_ProductLine_DB db = new T_ProductLine_DB();
            return db.SaveData(model, ref strError);
        }

        public bool DeleteProductLineByID(T_ProductLine model, ref string strError)
        {
            T_ProductLine_DB db = new T_ProductLine_DB();
            return db.DelData(model, ref strError);
        }

        protected override bool CheckModelBeforeSave(T_ProductLine model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.Sn))
            {
                strError = "产线编号不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.MachineLineName))
            {
                strError = "产线名称不能为空！";
                return false;
            }
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "产线";
        }

        protected override T_ProductLine GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

    }
}
