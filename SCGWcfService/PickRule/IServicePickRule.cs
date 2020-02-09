using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.PickRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_PICKRULE_Func代码
        [OperationContract]
        bool SaveT_PickRule(UserInfo user, ref T_PickRuleInfo t_pickrule, ref string strError);


        [OperationContract]
        bool DeleteT_PickRuleByModel(UserInfo user, T_PickRuleInfo model, ref string strError);

        [OperationContract]
        bool GetT_PickRuleByID(ref T_PickRuleInfo model, ref string strError);


        [OperationContract]
        bool GetT_PickRuleListByPage(ref List<T_PickRuleInfo> modelList, UserInfo user, T_PickRuleInfo t_pickrule, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_PickRuleByHeaderID(ref List<T_PickRuleInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_PickRuleStatus(UserInfo user, ref T_PickRuleInfo t_pickrule, int NewStatus, ref string strError);



        #endregion
    }
}