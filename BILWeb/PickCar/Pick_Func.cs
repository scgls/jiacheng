using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILWeb.Login.User;

namespace BILWeb.PickCar
{

    public partial class T_PickCar_Func : TBase_Func<T_PickCar_DB, T_PickCarInfo>, IPickCar
    {
        T_PickCar_DB _db = new T_PickCar_DB();
        protected override bool CheckModelBeforeSave(T_PickCarInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }
            if (BILBasic.Common.Common_Func.IsNullOrEmpty(model.CarNo))
            {
                strError = "小车编码不能为空！";
                return false;
            }

            if (_db.CheckCarIsExists(model.CarNo) > 0) 
            {
                strError = "小车编码已存在！";
                return false;
            }
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "拣货小车";
        }

        protected override T_PickCarInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

    }
}