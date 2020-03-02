using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILWeb.Login.User;

namespace BILWeb.Warehouse
{
    
    public partial class T_WareHouse_Func : TBase_Func<T_WareHouse_DB, T_WareHouseInfo>, IWarehouseService
    {

        protected override bool CheckModelBeforeSave(T_WareHouseInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (BILBasic.Common.Common_Func.IsNullOrEmpty(model.WareHouseNo))
            {
                strError = "仓库编号不能为空！";
                return false;
            }

            if (BILBasic.Common.Common_Func.IsNullOrEmpty(model.WareHouseName))
            {
                strError = "仓库名称不能为空！";
                return false;
            }

            //if (BILBasic.Common.Common_Func.IsNullOrEmpty(model.SamplerCode))
            //{
            //    strError = "取样人不能为空！";
            //    return false;
            //}
            
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "仓库";
        }

        protected override T_WareHouseInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }


        public bool GetModelListBySql(UserInfo user, ref List<T_WareHouseInfo> lstWarehouse) 
        {
            try
            {
                T_WareHouse_DB thd = new T_WareHouse_DB();
                lstWarehouse = thd.GetModelListBySql(user, false);
                if (lstWarehouse == null || lstWarehouse.Count <= 0) { return false; }
                else { return true; }
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

    }
}