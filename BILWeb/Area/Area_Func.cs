using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.JSONUtil;
using BILWeb.Stock;
using BILWeb.Login.User;
using BILBasic.User;

namespace BILWeb.Area
{
    public partial class T_Area_Func : TBase_Func<T_Area_DB, T_AreaInfo>,IAreaService
     {
   
        protected override bool CheckModelBeforeSave(T_AreaInfo model, ref string strError)
        {
            T_Area_DB _db = new T_Area_DB();
            int houseType = 0;
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！！！！！！！！！！！！！！！！！！";
                return false;
            }

            if (model.HouseID == 0)
            {
                strError = "请选择对应的库区！";
                return false;
            }

            if (BILBasic.Common.Common_Func.IsNullOrEmpty(model.AreaNo))
            {
                strError = "库位编号不能为空！";
                return false;
            }

            if (BILBasic.Common.Common_Func.IsNullOrEmpty(model.AreaName))
            {
                strError = "库位名称不能为空！";
                return false;
            }

            if (model.AreaStatus <= 0) 
            {
                strError = "请选择库位状态！";
                return false;
            }

            if (model.AreaType <= 0)
            {
                strError = "请选择库位类型！";
                return false;
            }

            if (model.HeightArea <= 0)
            {
                strError = "请选择高低货位类型！";
                return false;
            }

            houseType = _db.GetHouseTypeByAreaNo(model);

            if (houseType == 2 && model.AreaType == 1) 
            {
                strError = "检验库区不能设置正式库位！";
                return false;
            }

            if (houseType == 2 && model.AreaType == 3)
            {
                strError = "检验库区不能设置待发库位！";
                return false;
            }

            if (houseType == 3 && model.AreaType == 1)
            {
                strError = "待发库区不能设置正式库位！";
                return false;
            }            

            if (houseType == 3 && model.AreaType == 2)
            {
                strError = "待发库区不能设置检验库位！";
                return false;
            }

            if (houseType == 1 && model.AreaType == 2)
            {
                strError = "正式库区不能设置检验库位！";
                return false;
            }

            if (houseType == 1 && model.AreaType == 3)
            {
                strError = "正式库区不能设置待发库位！";
                return false;
            }



            //库位类型是收货库区只能设置一个
            if (model.AreaType == 2)
            {
                if (_db.CheckAreaType(model) >= 1)
                {
                    strError = "检验库位已经设置，只能设置一个检验库位！";
                    return false;
                }
            }

            //库位类型是拣货库区只能设置一个
            if (model.AreaType == 3)
            {
                if (_db.CheckAreaType(model) >= 1)
                {
                    strError = "待发库位已经设置，只能设置一个待发库位！";
                    return false;
                }
            }
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "库位";
        }
        
        protected override T_AreaInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

        public string GetAreaModelBySqlADF(string UserJson,string AreaNo) 
        {
            BaseMessage_Model<T_AreaInfo> messageModel = new BaseMessage_Model<T_AreaInfo>();
            try
            {
                string strError = string.Empty;
                T_AreaInfo model = new T_AreaInfo();

                if (string.IsNullOrEmpty(UserJson)) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来用户JSON为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
                }

                if (string.IsNullOrEmpty(AreaNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来库位编码为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
                }

                UserModel userModel = JSONHelper.JsonToObject<UserModel>(UserJson);

                model.WarehouseID = userModel.WarehouseID;
                model.AreaNo = AreaNo;
                bool bSucc = base.GetModelBySql(ref model, ref strError);

                if (bSucc == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
                }

                if (model.AreaStatus == 2)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "该库位已被锁定";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = model;

                return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
            }
            catch (Exception ex) 
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
            }
            
        }


        public string GetAreaModelBySql( string AreaNo)
        {
            BaseMessage_Model<T_AreaInfo> messageModel = new BaseMessage_Model<T_AreaInfo>();
            try
            {
                string strError = string.Empty;
                T_AreaInfo model = new T_AreaInfo();

               

                if (string.IsNullOrEmpty(AreaNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来库位编码为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
                }


                //model.WarehouseID = userModel.WarehouseID;
                model.AreaNo = AreaNo;
                bool bSucc = base.GetModelBySql(ref model, ref strError);

                if (bSucc == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
                }

                if (model.AreaStatus == 2)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "该库位已被锁定";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = model;

                return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
            }

        }



        public string GetAreaModelByMoveStockADF(int WareHouseID,string AreaNo, string ModelJson)
        {
            BaseMessage_Model<T_AreaInfo> messageModel = new BaseMessage_Model<T_AreaInfo>();
            try
            {
                int StockStatus = 0;

                if (string.IsNullOrEmpty(ModelJson)) 
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "客户端传来库存JSON为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
                }

                string strError = string.Empty;
                T_AreaInfo model = new T_AreaInfo();
                model.AreaNo = AreaNo;
                model.WarehouseID = WareHouseID;
                bool bSucc = base.GetModelBySql(ref model, ref strError);

                if (bSucc == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
                }

                if (model.AreaStatus == 2)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "该库位已被锁定";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
                }
                
                List<T_StockInfo> lstStock = new List<T_StockInfo>();
                lstStock = JSONHelper.JsonToObject<List<T_StockInfo>>(ModelJson);
                T_Stock_DB tdb = new T_Stock_DB();
                StockStatus = tdb.GetMaterialStatusByAreaID(WareHouseID,AreaNo, lstStock[0].MaterialNoID, lstStock[0].BatchNo);

                if (lstStock[0].AreaType == 4)
                {
                    if (StockStatus == 0)
                    {
                        messageModel.HeaderStatus = "E";
                        messageModel.Message = "该库位不存在扫描的条码！";
                        return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
                    }
                }
                else 
                {
                    if (StockStatus > 0)
                    {
                        if (lstStock[0].Status != StockStatus)
                        {
                            messageModel.HeaderStatus = "E";
                            messageModel.Message = "条码当前库位存在不同质检状态，不能移库！";
                            return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
                        }
                    }
                }

                if (StockStatus == 0)
                {
                    model.IsQuality = lstStock[0].Status;
                }
                else 
                {
                    model.IsQuality = StockStatus;
                }
                
                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = model;

                return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_AreaInfo>>(messageModel);
            }

        }



        public bool GetAreaModelBySql(int WareHouseID, string AreaNo,ref T_AreaInfo model,ref string strError)
        {            
            try
            {

                model.AreaNo = AreaNo;
                model.WarehouseID = WareHouseID;

                bool bSucc = base.GetModelBySql(ref model, ref strError);

                if (bSucc == false)
                {
                    return false;
                }

                if (model.AreaStatus == 2)
                {                    
                    strError = "该库位已被锁定";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }

        }

        public bool GetModelListBySql(int  WareHouseID, ref List<T_AreaInfo> lstArea)
        {
            try
            {
                T_Area_DB tdb = new T_Area_DB();
                lstArea = tdb.GetAreaModelList(WareHouseID);
                if (lstArea == null) { return false; }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

   
     }
 
}
