using BILBasic.Common;
using BILWeb.BaseInfo;
using BILWeb.Login.User;
using System.Collections.Generic;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_Team_Func代码

        public bool SaveT_Team(UserInfo user, ref T_Team t_customer, ref string strError)
        {
            T_Team_Func tfunc = new T_Team_Func();
            return tfunc.SaveData(t_customer, ref strError);
        }

        public bool DeleteT_TeamByModel(UserInfo user, T_Team model, ref string strError)
        {
            T_Team_Func tfunc = new T_Team_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_TeamByID(ref T_Team model, ref string strError)
        {
            T_Team_Func tfunc = new T_Team_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_TeamListByPage(ref List<T_Team> modelList, UserInfo user, T_Team t_customer, ref DividPage page, ref string strError)
        {
            T_Team_Func tfunc = new T_Team_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_customer, ref page, ref strError);
        }


        public bool GetAllT_TeamByHeaderID(ref List<T_Team> modelList, int headerID, ref string strError)
        {
            T_Team_Func tfunc = new T_Team_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_TeamStatus(UserInfo user, ref T_Team t_customer, int NewStatus, ref string strError)
        {
            T_Team_Func tfunc = new T_Team_Func();
            return tfunc.UpdateModelStatus(user, ref t_customer, NewStatus, ref strError);
        }


        #endregion

    }
}