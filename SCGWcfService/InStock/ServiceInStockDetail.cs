using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.InStock;
using BILWeb.Login.User;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {

        #region 自动生成WCF调用T_InStockDetail_Func代码

        public bool SaveT_InStockDetail(UserInfo user, ref T_InStockDetailInfo t_instockdetail, ref string strError)
        {
            T_InStockDetail_Func tfunc = new T_InStockDetail_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_instockdetail, ref strError);
        }


        public bool DeleteT_InStockDetailByModel(UserInfo user, T_InStockDetailInfo model, ref string strError)
        {
            T_InStockDetail_Func tfunc = new T_InStockDetail_Func();
            return tfunc.DeleteModelByModelSql(user, model, ref strError);
        }




        public bool GetT_InStockDetailByID(ref T_InStockDetailInfo model, ref string strError)
        {
            T_InStockDetail_Func tfunc = new T_InStockDetail_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_InStockDetailListByPage(ref List<T_InStockDetailInfo> modelList, UserInfo user, T_InStockDetailInfo t_instockdetail, ref DividPage page, ref string strError)
        {
            T_InStockDetail_Func tfunc = new T_InStockDetail_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_instockdetail, ref page, ref strError);
        }


        public bool GetAllT_InStockDetailByHeaderID(ref List<T_InStockDetailInfo> modelList, int headerID, ref string strError)
        {
            T_InStockDetail_Func tfunc = new T_InStockDetail_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_InStockDetailStatus(UserInfo user, ref T_InStockDetailInfo t_instockdetail, int NewStatus, ref string strError)
        {
            T_InStockDetail_Func tfunc = new T_InStockDetail_Func();
            return tfunc.UpdateModelStatus(user, ref t_instockdetail, NewStatus, ref strError);
        }


        #endregion

        public string GetT_InStockDetailListByHeaderIDADF(string ModelDetailJson)
        {
            T_InStockDetail_Func tfunc = new T_InStockDetail_Func();
            return tfunc.GetModelListByHeaderIDADF(ModelDetailJson);
        }

        public string SaveT_InStockDetailADF(string UserJson, string ModelJson) 
        {
            T_InStockDetail_Func tfunc = new T_InStockDetail_Func();
            return tfunc.SaveModelListSqlToDBADF(UserJson, ModelJson);
        }
    }
}