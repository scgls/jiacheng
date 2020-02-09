using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.OutStockCreate;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_OutStockDetail_Func代码

        public bool SaveT_OutStockCreate(UserInfo user, ref T_OutStockCreateInfo t_outstockdetail, ref string strError)
        {
            T_OutStockCreate_Func tfunc = new T_OutStockCreate_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_outstockdetail, ref strError);
        }

        public bool SaveT_OutStockCreateList(UserInfo user,  List<T_OutStockCreateInfo> modelList, ref string strError) 
        {
            T_OutStockCreate_Func tfunc = new T_OutStockCreate_Func();
            return tfunc.SaveModelListBySqlToDB(user, modelList, ref strError);
        }

        public bool DeleteT_OutStockCreateByModel(UserInfo user, T_OutStockCreateInfo model, ref string strError)
        {
            T_OutStockCreate_Func tfunc = new T_OutStockCreate_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_OutStockCreateByID(ref T_OutStockCreateInfo model, ref string strError)
        {
            T_OutStockCreate_Func tfunc = new T_OutStockCreate_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_OutStockCreateListByPage(ref List<T_OutStockCreateInfo> modelList, UserInfo user, T_OutStockCreateInfo t_outstockdetail, ref DividPage page, ref string strError)
        {
            T_OutStockCreate_Func tfunc = new T_OutStockCreate_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_outstockdetail, ref page, ref strError);
        }


        public bool GetAllT_OutStockCreateByHeaderID(ref List<T_OutStockCreateInfo> modelList, int headerID, ref string strError)
        {
            T_OutStockCreate_Func tfunc = new T_OutStockCreate_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_OutStockCreateStatus(UserInfo user, ref T_OutStockCreateInfo t_outstockdetail, int NewStatus, ref string strError)
        {
            T_OutStockCreate_Func tfunc = new T_OutStockCreate_Func();
            return tfunc.UpdateModelStatus(user, ref t_outstockdetail, NewStatus, ref strError);
        }


        #endregion
    }
}