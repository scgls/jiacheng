using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.InStock;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_InStock_Func代码

        public bool SaveT_InStock(UserInfo user, ref T_InStockInfo t_instock, ref string strError)
        {
            T_InStock_Func tfunc = new T_InStock_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_instock, ref strError);
        }


        public bool DeleteT_InStockByModel(UserInfo user, T_InStockInfo model, ref string strError)
        {
            T_InStock_Func tfunc = new T_InStock_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_InStockByID(ref T_InStockInfo model, ref string strError)
        {
            T_InStock_Func tfunc = new T_InStock_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_InStockListByPage(ref List<T_InStockInfo> modelList, UserInfo user, T_InStockInfo t_instock, ref DividPage page, ref string strError)
        {
            T_InStock_Func tfunc = new T_InStock_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_instock, ref page, ref strError);
        }

        public string GetT_InStockListADF(string UserJosn, string ModelJson) 
        {
            T_InStock_Func tfunc = new T_InStock_Func();
            return tfunc.GetModelListADF(UserJosn, ModelJson);
        }
        

        public bool GetAllT_InStockByHeaderID(ref List<T_InStockInfo> modelList, int headerID, ref string strError)
        {
            T_InStock_Func tfunc = new T_InStock_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_InStockStatus(UserInfo user, ref T_InStockInfo t_instock,  ref string strError)
        {
            T_InStock_Func tfunc = new T_InStock_Func();
            return tfunc.UpadteModelByModelSql(user,  t_instock, ref strError);
        }


        #endregion
    }
}