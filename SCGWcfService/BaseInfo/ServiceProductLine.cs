using BILBasic.Common;
using BILWeb.BaseInfo;
using BILWeb.Login.User;
using System.Collections.Generic;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_ProductLine_Func代码

        public bool SaveT_ProductLine(UserInfo user, ref T_ProductLine t_customer, ref string strError)
        {
            T_ProductLine_Func tfunc = new T_ProductLine_Func();
            return tfunc.SaveData(t_customer, ref strError);
        }

        public bool DeleteT_ProductLineByModel(UserInfo user, T_ProductLine model, ref string strError)
        {
            T_ProductLine_Func tfunc = new T_ProductLine_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_ProductLineByID(ref T_ProductLine model, ref string strError)
        {
            T_ProductLine_Func tfunc = new T_ProductLine_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_ProductLineListByPage(ref List<T_ProductLine> modelList, UserInfo user, T_ProductLine t_customer, ref DividPage page, ref string strError)
        {
            T_ProductLine_Func tfunc = new T_ProductLine_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_customer, ref page, ref strError);
        }


        public bool GetAllT_ProductLineByHeaderID(ref List<T_ProductLine> modelList, int headerID, ref string strError)
        {
            T_ProductLine_Func tfunc = new T_ProductLine_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_ProductLineStatus(UserInfo user, ref T_ProductLine t_customer, int NewStatus, ref string strError)
        {
            T_ProductLine_Func tfunc = new T_ProductLine_Func();
            return tfunc.UpdateModelStatus(user, ref t_customer, NewStatus, ref strError);
        }


        #endregion

    }
}