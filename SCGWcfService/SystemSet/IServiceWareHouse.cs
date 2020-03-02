using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using BILWeb.Warehouse;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_WAREHOUSE_Func代码
        [OperationContract]
        bool SaveT_WareHouse(UserInfo user, ref T_WareHouseInfo t_warehouse, ref string strError);


        [OperationContract]
        bool DeleteT_WareHouseByModel(UserInfo user, T_WareHouseInfo model, ref string strError);

        [OperationContract]
        bool GetT_WareHouseByID(ref T_WareHouseInfo model, ref string strError);


        [OperationContract]
        bool GetT_WareHouseListByPage(ref List<T_WareHouseInfo> modelList, UserInfo user, T_WareHouseInfo t_warehouse, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_WareHouseByHeaderID(ref List<T_WareHouseInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_WareHouseStatus(UserInfo user, ref T_WareHouseInfo t_warehouse, int NewStatus, ref string strError);



        #endregion
    }
}