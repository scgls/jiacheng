using BILBasic.Common;
using BILBasic.User;
using BILWeb.InStock;
using BILWeb.Login.User;
using BILWeb.OutStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_OutStockDetail_Func代码

        public bool SaveT_OutStockDetail(UserInfo user, ref T_OutStockDetailInfo t_outstockdetail, ref string strError)
        {
            T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_outstockdetail, ref strError);
        }


        public bool DeleteT_OutStockDetailByModel(UserInfo user, T_OutStockDetailInfo model, ref string strError)
        {
            T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
            return tfunc.DeleteModelByModelSql(user, model, ref strError);
        }




        public bool GetT_OutStockDetailByID(ref T_OutStockDetailInfo model, ref string strError)
        {
            T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_OutStockDetailListByPage(ref List<T_OutStockDetailInfo> modelList, UserInfo user, T_OutStockDetailInfo t_outstockdetail, ref DividPage page, ref string strError)
        {
            T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_outstockdetail, ref page, ref strError);
        }


        public bool GetAllT_OutStockDetailByHeaderID(ref List<T_OutStockDetailInfo> modelList, int headerID, ref string strError)
        {
            T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_OutStockDetailStatus(UserInfo user, ref T_OutStockDetailInfo t_outstockdetail, int NewStatus, ref string strError)
        {
            T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
            return tfunc.UpdateModelStatus(user, ref t_outstockdetail, NewStatus, ref strError);
        }

        public bool GetChangeMaterialForStock(ref T_OutStockDetailInfo model, ref string strError) 
        {
            T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
            return tfunc.GetChengeMaterialForStock(ref model, ref strError);
        }

        public bool UpdateChangeMaterial(ref T_OutStockDetailInfo model, ref string strError) 
        {
            T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
            return tfunc.UpdateChangeMaterial(ref model, ref strError);
        }

        public bool SaveT_ChangeMaterial(UserInfo userModel, List<T_InStockDetailInfo> InStockDetailList, List<T_OutStockDetailInfo> OutStockDetailList, ref string strError) 
        {
            T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
            return tfunc.SaveT_ChangeMaterial(userModel, InStockDetailList,OutStockDetailList, ref strError);
        }

        #endregion
    }
}