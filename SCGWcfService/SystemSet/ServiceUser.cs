using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.Login.User;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        public bool SaveT_User(UserInfo user, ref UserInfo t_user, ref string strError)
        {
            User_Func tfunc = new User_Func();
            return tfunc.SaveModelToDB(user, ref t_user, ref strError);
        }


        public bool DeleteT_UserByModel(UserInfo user, UserInfo model, ref string strError)
        {
            User_Func tfunc = new User_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_UserByID(ref UserInfo model, ref string strError)
        {
            User_Func tfunc = new User_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_UserListByPage(ref List<UserInfo> modelList, UserInfo user, UserInfo t_user, ref  BILBasic.Common.DividPage page, ref string strError)
        {
            User_Func tfunc = new User_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_user, ref page, ref strError);
        }


        public bool GetAllT_UserByHeaderID(ref List<UserInfo> modelList, int headerID, ref string strError)
        {
            User_Func tfunc = new User_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_UserStatus(UserInfo user, ref UserInfo t_user, int NewStatus, ref string strError)
        {
            User_Func tfunc = new User_Func();
            return tfunc.UpdateModelStatus(user, ref t_user, NewStatus, ref strError);
        }

        


    }
}