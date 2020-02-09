using BILBasic.Common;
using BILWeb.BaseInfo;
using BILWeb.Customer;
using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_Mould_Func代码
        [OperationContract]
        bool SaveT_Modl(UserInfo user, ref T_Modl t_customer, ref string strError);


        [OperationContract]
        bool DeleteT_ModlByModel(UserInfo user, T_Modl model, ref string strError);

        [OperationContract]
        bool GetT_ModlByID(ref T_Modl model, ref string strError);


        [OperationContract]
        bool GetT_ModlListByPage(ref List<T_Modl> modelList, UserInfo user, T_Modl t_customer, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_ModlByHeaderID(ref List<T_Modl> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_ModlStatus(UserInfo user, ref T_Modl t_customer, int NewStatus, ref string strError);



        #endregion

        #region 自动生成WCF接口方法T_MouldType_Func代码
        [OperationContract]
        bool SaveT_MouldType(UserInfo user, ref T_MouldType t_customer, ref string strError);


        [OperationContract]
        bool DeleteT_MouldTypeByModel(UserInfo user, T_MouldType model, ref string strError);

        [OperationContract]
        bool GetT_MouldTypeByID(ref T_MouldType model, ref string strError);


        [OperationContract]
        bool GetT_MouldTypeListByPage(ref List<T_MouldType> modelList, UserInfo user, T_MouldType t_customer, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_MouldTypeByHeaderID(ref List<T_MouldType> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_MouldTypeStatus(UserInfo user, ref T_MouldType t_customer, int NewStatus, ref string strError);



        #endregion
    }
}