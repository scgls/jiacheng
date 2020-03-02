
using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCGWcfService
{
    public partial class ServiceWMS:IService
    {
        public bool UserLogin(ref UserInfo user, ref string strError) 
        {
            User_Func tfunc = new User_Func();
            return tfunc.UserLogin(ref user, ref strError);
        }

        public string UserLoginADF(string UserJson)
        {
            User_Func tfunc = new User_Func();
            return tfunc.UserLoginADF(UserJson);
        }

        //public bool VerifyVersion(string FileVersion, string FileName, string path)
        //{
        //    AppVersion_Func func = new AppVersion_Func();
        //    return !func.VerifyVersion(FileVersion, FileName, path);
        //}

        //public bool VerifyAppVersion(ref AppVersionInfo appversion, ref string strError)
        //{
        //    AppVersion_Func func = new AppVersion_Func();
        //    return func.VerifyVersion(ref appversion, ref strError);
        //}

        public bool ChangeUserPassword(UserInfo user,ref string strError)
        {
            User_Func func = new User_Func();
            return func.ChangeUserPassword(user, ref strError);
        }
    }
}