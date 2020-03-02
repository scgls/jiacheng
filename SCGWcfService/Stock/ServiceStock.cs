using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.Login.User;
using BILWeb.Stock;
using BILBasic.Common;


namespace SCGWcfService.Stock
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_Stock_Func代码

        public bool SaveT_Stock(UserInfo user, ref T_StockInfo t_stock, ref string strError)
        {
            T_Stock_Func tfunc = new T_Stock_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_stock, ref strError);
        }


        public bool DeleteT_StockByModel(UserInfo user, T_StockInfo model, ref string strError)
        {
            T_Stock_Func tfunc = new T_Stock_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_StockByID(ref T_StockInfo model, ref string strError)
        {
            T_Stock_Func tfunc = new T_Stock_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_StockListByPage(ref List<T_StockInfo> modelList, UserInfo user, T_StockInfo t_stock, ref DividPage page, ref string strError)
        {
            T_Stock_Func tfunc = new T_Stock_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_stock, ref page, ref strError);
        }


        public bool GetAllT_StockByHeaderID(ref List<T_StockInfo> modelList, int headerID, ref string strError)
        {
            T_Stock_Func tfunc = new T_Stock_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_StockStatus(UserInfo user, ref T_StockInfo t_stock, int NewStatus, ref string strError)
        {
            T_Stock_Func tfunc = new T_Stock_Func();
            return tfunc.UpdateModelStatus(user, ref t_stock, NewStatus, ref strError);
        }


        //public string GetStockModelADF(string SerialNo)
        //{
        //    T_Stock_Func tfunc = new T_Stock_Func();
        //    return tfunc.GetStockModelBySqlADF(SerialNo);
        //}

        
       
        #endregion
    }
}