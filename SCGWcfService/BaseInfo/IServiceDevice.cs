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
        #region 自动生成WCF接口方法T_Machine_Func代码
        [OperationContract]
        bool SaveT_Machine(UserInfo user, ref T_Machine t_customer, ref string strError);


        [OperationContract]
        bool DeleteT_MachineByModel(UserInfo user, T_Machine model, ref string strError);

        [OperationContract]
        bool GetT_MachineByID(ref T_Machine model, ref string strError);


        [OperationContract]
        bool GetT_MachineListByPage(ref List<T_Machine> modelList, UserInfo user, T_Machine t_customer, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_MachineByHeaderID(ref List<T_Machine> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_MachineStatus(UserInfo user, ref T_Machine t_customer, int NewStatus, ref string strError);



        #endregion

        #region 自动生成WCF接口方法T_MachineType_Func代码
        [OperationContract]
        bool SaveT_MachineType(UserInfo user, ref T_MachineType t_customer, ref string strError);


        [OperationContract]
        bool DeleteT_MachineTypeByModel(UserInfo user, T_MachineType model, ref string strError);

        [OperationContract]
        bool GetT_MachineTypeByID(ref T_MachineType model, ref string strError);


        [OperationContract]
        bool GetT_MachineTypeListByPage(ref List<T_MachineType> modelList, UserInfo user, T_MachineType t_customer, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_MachineTypeByHeaderID(ref List<T_MachineType> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_MachineTypeStatus(UserInfo user, ref T_MachineType t_customer, int NewStatus, ref string strError);



        #endregion
    }
}