using BILBasic.Common;
using BILWeb.DepInterface;
using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_DepInterface_Func代码

        public bool SaveT_DepInterface(UserInfo user, ref T_DepInterfaceInfo t_depinterface, ref string strError)
        {
            T_DepInterface_Func tfunc = new T_DepInterface_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_depinterface, ref strError);
        }


        public bool DeleteT_DepInterfaceByModel(UserInfo user, T_DepInterfaceInfo model, ref string strError)
        {
            T_DepInterface_Func tfunc = new T_DepInterface_Func();
            return tfunc.DeleteModelByModelSql(user, model, ref strError);
        }




        public bool GetT_DepInterfaceByID(ref T_DepInterfaceInfo model, ref string strError)
        {
            T_DepInterface_Func tfunc = new T_DepInterface_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_DepInterfaceListByPage(ref List<T_DepInterfaceInfo> modelList, UserInfo user, T_DepInterfaceInfo t_depinterface, ref DividPage page, ref string strError)
        {
            T_DepInterface_Func tfunc = new T_DepInterface_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_depinterface, ref page, ref strError);
        }


        public bool GetAllT_DepInterfaceByHeaderID(ref List<T_DepInterfaceInfo> modelList, int headerID, ref string strError)
        {
            T_DepInterface_Func tfunc = new T_DepInterface_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_DepInterfaceStatus(UserInfo user, ref T_DepInterfaceInfo t_depinterface, int NewStatus, ref string strError)
        {
            T_DepInterface_Func tfunc = new T_DepInterface_Func();
            return tfunc.UpdateModelStatus(user, ref t_depinterface, NewStatus, ref strError);
        }


        #endregion
    }
}