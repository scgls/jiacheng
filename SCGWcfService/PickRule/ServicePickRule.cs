using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using BILWeb.PickRule;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_PickRule_Func代码

        public bool SaveT_PickRule(UserInfo user, ref T_PickRuleInfo t_pickrule, ref string strError)
        {
            T_PickRule_Func tfunc = new T_PickRule_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_pickrule, ref strError);
        }


        public bool DeleteT_PickRuleByModel(UserInfo user, T_PickRuleInfo model, ref string strError)
        {
            T_PickRule_Func tfunc = new T_PickRule_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_PickRuleByID(ref T_PickRuleInfo model, ref string strError)
        {
            T_PickRule_Func tfunc = new T_PickRule_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_PickRuleListByPage(ref List<T_PickRuleInfo> modelList, UserInfo user, T_PickRuleInfo t_pickrule, ref DividPage page, ref string strError)
        {
            T_PickRule_Func tfunc = new T_PickRule_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_pickrule, ref page, ref strError);
        }


        public bool GetAllT_PickRuleByHeaderID(ref List<T_PickRuleInfo> modelList, int headerID, ref string strError)
        {
            T_PickRule_Func tfunc = new T_PickRule_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_PickRuleStatus(UserInfo user, ref T_PickRuleInfo t_pickrule, int NewStatus, ref string strError)
        {
            T_PickRule_Func tfunc = new T_PickRule_Func();
            return tfunc.UpdateModelStatus(user, ref t_pickrule, NewStatus, ref strError);
        }


        #endregion
    }
}