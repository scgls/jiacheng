using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using BILWeb.House;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_HOUSE_Func代码
        [OperationContract]
        bool SaveT_House(UserInfo user, ref T_HouseInfo t_house, ref string strError);


        [OperationContract]
        bool DeleteT_HouseByModel(UserInfo user, T_HouseInfo model, ref string strError);

        [OperationContract]
        bool GetT_HouseByID(ref T_HouseInfo model, ref string strError);


        [OperationContract]
        bool GetT_HouseListByPage(ref List<T_HouseInfo> modelList, UserInfo user, T_HouseInfo t_house, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_HouseByHeaderID(ref List<T_HouseInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_HouseStatus(UserInfo user, ref T_HouseInfo t_house, int NewStatus, ref string strError);



        #endregion
    }
}