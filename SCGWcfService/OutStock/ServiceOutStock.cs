using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.OutStock;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_OUTSTOCK_Func代码

        public bool SaveT_OutStock(UserInfo user, ref T_OutStockInfo t_outstock, ref string strError)
        {
            T_OutStock_Func tfunc = new T_OutStock_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_outstock, ref strError);
        }


        public bool DeleteT_OutStockByModel(UserInfo user, T_OutStockInfo model, ref string strError)
        {
            T_OutStock_Func tfunc = new T_OutStock_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_OutStockByID(ref T_OutStockInfo model, ref string strError)
        {
            T_OutStock_Func tfunc = new T_OutStock_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_OutStockListByPage(ref List<T_OutStockInfo> modelList, UserInfo user, T_OutStockInfo t_outstock, ref DividPage page, ref string strError)
        {
            T_OutStock_Func tfunc = new T_OutStock_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_outstock, ref page, ref strError);
        }


        public bool GetAllT_OutStockByHeaderID(ref List<T_OutStockInfo> modelList, int headerID, ref string strError)
        {
            T_OutStock_Func tfunc = new T_OutStock_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_OutStockStatus(UserInfo user, ref T_OutStockInfo t_outstock,  ref string strError)
        {
            T_OutStock_Func tfunc = new T_OutStock_Func();
            return tfunc.UpadteModelByModelSql(user,  t_outstock, ref strError);
        }


        #endregion

        public bool GetOutStockAndDetailsModelByNo(string erpNo, ref BILWeb.OutStockTask.T_OutStockTaskInfo head, ref List<BILWeb.OutStockTask.T_OutStockTaskDetailsInfo> lstDetail, ref string ErrMsg)
        {
            T_OutStock_Func tfunc = new T_OutStock_Func();
            return tfunc.GetOutStockAndDetailsModelByNo(erpNo, ref head, ref lstDetail, ref ErrMsg);
        }
    }
}