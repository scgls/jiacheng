using BILBasic.Common;
using BILWeb.BaseInfo;
using BILWeb.Login.User;
using System.Collections.Generic;
using System.ServiceModel;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_Team_Func代码
        [OperationContract]
        bool SaveT_Team(UserInfo user, ref T_Team t_customer, ref string strError);


        [OperationContract]
        bool DeleteT_TeamByModel(UserInfo user, T_Team model, ref string strError);

        [OperationContract]
        bool GetT_TeamByID(ref T_Team model, ref string strError);


        [OperationContract]
        bool GetT_TeamListByPage(ref List<T_Team> modelList, UserInfo user, T_Team t_customer, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_TeamByHeaderID(ref List<T_Team> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_TeamStatus(UserInfo user, ref T_Team t_customer, int NewStatus, ref string strError);



        #endregion

    }
}