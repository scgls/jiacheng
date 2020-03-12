//using BILWeb.AppVersion;
using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace SCGWcfService
{
    
    /// <summary>
    /// 定义用户模块接口
    /// </summary>
    public partial interface IService
    {
        [OperationContract]
        bool UserLogin(ref UserInfo user, ref string strError);

        [OperationContract]
        string UserLoginADF(string UserJson);

        //[OperationContract]
        //bool VerifyVersion(string FileVersion, string FileName, string path);

        //[OperationContract]
        //bool VerifyAppVersion(ref AppVersionInfo appversion, ref string strError);

        [OperationContract]
        bool ChangeUserPassword(UserInfo user, ref string strError);
    }
}