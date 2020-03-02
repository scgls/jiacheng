using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILBasic.JSONUtil;
using BILWeb.Stock;
using BILBasic.User;
using BILWeb.Login.User;

namespace BILWeb.Quality
{

    public partial class T_QualityDetail_Func : TBase_Func<T_QualityDetail_DB, T_QualityDetailInfo>
    {
        T_Stock_Func stockFunc = new T_Stock_Func();

        protected override bool CheckModelBeforeSave(T_QualityDetailInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }
            
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "检验单行";
        }


        protected override T_QualityDetailInfo GetModelByJson(string strJson)
        {
            string errorMsg = string.Empty;

            T_QualityDetailInfo model = JSONHelper.JsonToObject<T_QualityDetailInfo>(strJson);

            return model;
        }
        

        protected override List<T_QualityDetailInfo> GetModelListByJson(string UserJson, string ModelListJson)
        {
            UserModel userModel = JSONHelper.JsonToObject<UserModel>(UserJson); 
            List<T_QualityDetailInfo> modelList = JSONHelper.JsonToObject<List<T_QualityDetailInfo>>(ModelListJson);

            string strUserNo = string.Empty;
            string strPostUser = string.Empty;

            //if (TOOL.RegexMatch.isExists(userModel.UserNo) == true)
            //{
            //    strUserNo = userModel.UserNo.Substring(0, userModel.UserNo.Length - 1);
            //}
            //else
            //{
            //    strUserNo = userModel.UserNo;
            //}

            //User_DB _db = new User_DB();
            //strPostUser = _db.GetPostAccountByUserNo(strUserNo, modelList[0].StrongHoldCode);

            foreach (var item in modelList)
            {
                item.FromErpWarehouse = item.lstStock.FirstOrDefault().WarehouseNo;
                item.FromErpAreaNo = item.lstStock.FirstOrDefault().AreaNo;
                item.FromBatchNo = item.lstStock.FirstOrDefault().BatchNo;
                item.ToErpWarehouse = userModel.ToSampWareHouseNo;
                item.ToErpAreaNo = userModel.ToSampAreaNo;
                item.PostUser = userModel.UserNo;// strPostUser;
            }

            LogNet.LogInfo("UpadteT_QualityUserADF-----" + JSONHelper.ObjectToJson<List<T_QualityDetailInfo>>(modelList));

            return modelList;  
          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErpVoucherNo"></param>
        /// <returns></returns>
        public bool GetQualityUpadteStock(string ErpVoucherNo,ref string strError) 
        {
            T_QualityDetail_DB _db = new T_QualityDetail_DB();
            List<T_QualityDetailInfo> modelList = new List<T_QualityDetailInfo>();
            modelList = _db.GetQualityUpadteStock(ErpVoucherNo);

            if (modelList == null || modelList.Count==0) 
            {
                strError = "未获取到检验单信息！";
                return false;
            }

            return stockFunc.UpdateStockByQuality(modelList,ref  strError);
        }
    }
}