using BILWeb.StrategeRuleAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILWeb.Stock;
using BILBasic.Language;

namespace BILWeb.Stock
{
    public class Stock_SerialUnEnableRule : StrategeRuleAll<T_StockInfo>
    {
        //不启用序列号根据库位+条码去查找
        public override bool GetStockByBarCode(T_StockInfo model,ref List<T_StockInfo> modelList,ref string strError)
        {
            try
            {
                if (string.IsNullOrEmpty(model.WarehouseNo) || string.IsNullOrEmpty(model.Barcode))
                {
                    strError = Language_CHS.DataIsEmpty;
                    return false;
                }

                T_Stock_DB db = new T_Stock_DB();
                List<T_StockInfo> newModelList = new List<T_StockInfo>();
                newModelList = db.GetStockByWHBarCode(model);
                if (newModelList == null) 
                {
                    strError = Language_CHS.StockIsEmpty;
                    return false;
                }

                modelList.AddRange(newModelList);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
