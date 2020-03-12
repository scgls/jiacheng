using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.House
{
    public partial class T_House_Func : TBase_Func<T_House_DB, T_HouseInfo>,IHouseService
     {
   
        protected override bool CheckModelBeforeSave(T_HouseInfo model, ref string strError)
        {
            T_House_DB _db = new T_House_DB();
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (model.WarehouseID ==0)
            {
                strError = "请选择对应的仓库！";
                return false;
            }

            if (BILBasic.Common.Common_Func.IsNullOrEmpty(model.HouseNo))
            {
                strError = "库区编号不能为空！";
                return false;
            }

            if (BILBasic.Common.Common_Func.IsNullOrEmpty(model.HouseName))
            {
                strError = "库区名称不能为空！";
                return false;
            }

            if (model.HouseStatus == 0) 
            {
                strError = "请选择库区状态！";
                return false;
            }

            if (model.HouseType == 0) 
            {
                strError = "请选择库区类型！";
                return　false;
            }

            if (model.FloorType == 0)
            {
                strError = "请选择库区所属楼层！";
                return false;
            }

            //if (string.IsNullOrEmpty(model.MaterialClassCode))
            //{
            //    strError = "请选择库区存放物料类别！";
            //    return false;
            //}

            //if (_db.CheckHouseTypeArea(model) >= 1) 
            //{
            //    strError = "该库区存在检验库位或者待发库位，请先设置库位类型！";
            //    return false;
            //}

            //库区类型是收货库区只能设置一个
            if (model.HouseType == 2 ) 
            {
                if (_db.CheckHouseType(model) >= 1) 
                {
                    strError = "收货库区已经设置，只能设置一个收货库区！";
                    return false;
                }
            }

            //库区类型是拣货库区只能设置一个
            if (model.HouseType == 3 )
            {
                if (_db.CheckHouseType(model) >= 1)
                {
                    strError = "待发库区已经设置，只能设置一个待发库区！";
                    return false;
                }
            }
            
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "库区";
        }
        
        protected override T_HouseInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }
   
     }
 }

