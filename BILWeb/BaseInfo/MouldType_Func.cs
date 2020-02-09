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
    public partial class T_MouldType_Func : TBase_Func<T_MouldType_DB, T_MouldType>
    {
        public bool SaveData(T_MouldType model, ref string strError)
        {
            T_MouldType_DB db = new T_MouldType_DB();
            return db.SaveData(model, ref strError);
        }

        protected override bool CheckModelBeforeSave(T_MouldType model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.MouldTypeCode))
            {
                strError = "模具型号编号不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.MouldTypeName))
            {
                strError = "模具型号名称不能为空！";
                return false;
            }
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "模具型号";
        }

        protected override T_MouldType GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

        public bool DeleteMouldTypeByID(T_MouldType model, ref string strError)
        {
            T_MouldType_DB db = new T_MouldType_DB();
            return db.DelData(model, ref strError);
        }

    }
}
