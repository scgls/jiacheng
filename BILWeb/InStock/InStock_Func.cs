using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.JSONUtil;

namespace BILWeb.InStock
{
    public partial class T_InStock_Func : TBase_Func<T_InStock_DB, T_InStockInfo>,IInStockService
     {
   
        protected override bool CheckModelBeforeSave(T_InStockInfo model, ref string strError)
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
            return "收货";
        }
        
        //protected override T_InStockInfo GetModelByJson(string ModelJson)
        //{
        //    return JSONHelper.JsonToObject<T_InStockInfo>(ModelJson);
        //}

        protected override T_InStockInfo GetModelByJson(string strJson)
        {
            string errorMsg = string.Empty;
            T_InStockDetailInfo model = JSONHelper.JsonToObject<T_InStockDetailInfo>(strJson);

            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
                PFunc.Sync(10, string.Empty, model.ErpVoucherNo, -1, ref errorMsg, "ERP", -1, null);

            }
            return JSONHelper.JsonToObject<T_InStockInfo>(strJson);
        }

        public string GetInStockStatusByTaskNo(string strTaskNo) 
        {
            T_InStock_DB _db = new T_InStock_DB();
            return _db.GetInStockStatusByTaskNo(strTaskNo);
        }

        public int GetInStockVoucherType(string strTaskNo) 
        {
            T_InStock_DB _db = new T_InStock_DB();
            return _db.GetInStockVoucherType(strTaskNo);
        }
   
     }
 }

