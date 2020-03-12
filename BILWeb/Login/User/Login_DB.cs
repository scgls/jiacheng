using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;


namespace BILWeb.Login.User
{
    public class Login_DB
    {
        //private static int LoginIPLength;
        //private static int LoginDeviceLength;

        //public bool UserLogin(ref UserInfo user, ref string strError)
        //{
        //    if (string.IsNullOrEmpty(user.LoginDevice)) user.LoginDevice = user.LoginIP;
        //    string LoginIP = user.LoginIP;
        //    DateTime CurrentTime;

        //    string strSql = string.Empty;
        //    strSql = string.Format("SELECT sysdate CurrentTime,V_User.* FROM V_User WHERE UserNo = '{0}' AND Password = '{1}' ", user.UserCode, user.Password);
        //    //strSql = string.Format("SELECT sysdate CurrentTime,V_User.* FROM V_User WHERE UserNo = '{0}'", user.UserNo);

        //    UserInfo model;
        //    using (IDataReader odr = dbFactory.ExecuteReader(CommandType.Text, strSql))
        //    {
        //        if (odr.Read())
        //        {
        //            User_Func func = new User_Func();
        //            model = func.GetModelFromDataReader(odr);
        //            CurrentTime = odr["CurrentTime"].ToDateTime();

        //            if (model == null)
        //            {
        //                strError = "用户实例化失败";
        //                return false;
        //            }
        //            else if (model.UserStatus == 2)
        //            {
        //                strError = string.Format("用户【{0}】已停用", model.UserName);
        //                return false;
        //            }
        //            else if (model.IsDel == 2)
        //            {
        //                strError = string.Format("用户【{0}】已删除", model.UserName);
        //                return false;
        //            }

        //            if (model.BIsOnline)
        //            {
        //                if (model.UserType == 1)
        //                {
        //                    GetLoginInfo();

        //                    if (!string.IsNullOrEmpty(user.LoginIP) && model.LoginIP.Length + user.LoginIP.Length >= LoginIPLength && model.LoginIP.IndexOf(user.LoginIP) <= -1)
        //                    {
        //                        strError = string.Format("超级管理员用户【{0}】已超过登录次数上限{1}目前共【{2}】处登录{3}请先登出或联系管理员清除后重试", model.UserName, Environment.NewLine, model.LoginIP.Split(';').Length, Environment.NewLine);
        //                        return false;
        //                    }
        //                    else if (!string.IsNullOrEmpty(user.LoginDevice) && model.LoginDevice.Length + user.LoginDevice.Length >= LoginDeviceLength && model.LoginDevice.IndexOf(user.LoginDevice) <= -1)
        //                    {
        //                        strError = string.Format("超级管理员用户【{0}】已超过登录次数上限{1}目前共【{2}】处登录{3}请先登出或联系管理员清除后重试", model.UserName, Environment.NewLine, model.LoginDevice.Split(';').Length, Environment.NewLine);
        //                        return false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (model.LoginIP != user.LoginIP)
        //                    {
        //                        string LoginAddress = string.IsNullOrEmpty(model.LoginDevice) ? model.LoginIP : model.LoginDevice;
        //                        if (!model.LoginIP.StartsWith("PC"))
        //                        {
        //                            strError = string.Format("用户【{0}】已于【{1}】在【{2}】处登录{3}请先登出或联系管理员清除后重试", model.UserName, model.LoginTime, LoginAddress, Environment.NewLine);
        //                            return false;
        //                        }
        //                        else if ((CurrentTime - model.LoginTime.ToDateTime()).TotalMilliseconds < 1500000)
        //                        {
        //                            strError = string.Format("用户【{0}】正在【{1}】处使用{2}请先登出或联系管理员清除后重试", model.UserName, LoginAddress, Environment.NewLine);
        //                            return false;
        //                        }
        //                    }
        //                }
        //            }

        //            model.LoginTime = CurrentTime;
        //            model.LoginIP = user.LoginIP;
        //            model.LoginDevice = user.LoginDevice;

        //            user = model;
        //        }
        //        else
        //        {
        //            strSql = string.Format("SELECT COUNT(1) FROM V_User WHERE UserNo = '{0}' ", user.UserNo);
        //            int i = dbFactory.ExecuteScalar(CommandType.Text, strSql).ToInt32();
        //            if (i <= 0)
        //            {
        //                strError = "该用户不存在,请检查大小写是否输入正确!";
        //                return false;
        //            }
        //            else
        //            {
        //                strError = "密码输入错误,忘记密码请联系管理员重置!";
        //                return false;
        //            }
        //        }
        //    }

        //    return true;
        //}

        //    private void GetLoginInfo()
        //    {
        //        string strSql = string.Empty;

        //        if (LoginIPLength < 100)
        //        {
        //            strSql = "select char_Length from user_tab_columns where table_name = 'T_USER' and column_name = 'LOGINIP'";
        //            LoginIPLength = dbFactory.ExecuteScalar(CommandType.Text, strSql, null).ToInt32();
        //        }

        //        if (LoginDeviceLength < 200)
        //        {
        //            strSql = "select char_Length from user_tab_columns where table_name = 'T_USER' and column_name = 'LOGINDEVICE'";
        //            LoginDeviceLength = dbFactory.ExecuteScalar(CommandType.Text, strSql, null).ToInt32();
        //        }
        //    }

        //    public bool UpdateLoginTime(UserInfo user)
        //    {
        //        string strSql;
        //        user.LoginTime = DateTime.Now;
        //        UserInfo model = new UserInfo();
        //        if (user.ID >= 1 && user.UserType == 1)
        //        {
        //            strSql = string.Format("SELECT LoginIP, LoginDevice FROM T_User WHERE ID = {0}", user.ID);
        //            using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
        //            {
        //                if (dr.Read())
        //                {
        //                    model.LoginIP = dr["LoginIP"].ToDBString();
        //                    model.LoginDevice = dr["LoginDevice"].ToDBString();
        //                    if (!string.IsNullOrEmpty(model.LoginIP))
        //                    {
        //                        if (!string.IsNullOrEmpty(user.LoginIP) && model.LoginIP.IndexOf(user.LoginIP) <= -1)
        //                        {
        //                            model.LoginIP = string.Format("{0};{1}", model.LoginIP, user.LoginIP).Trim(';');
        //                            model.LoginDevice = string.Format("{0};{1}", model.LoginDevice, user.LoginDevice).Trim(';');
        //                        }
        //                        else
        //                        {
        //                            model.LoginIP = string.Format("{0};{1}", model.LoginIP.Replace(user.LoginIP, ""), user.LoginIP).Trim(';');
        //                            model.LoginDevice = string.Format("{0};{1}", model.LoginDevice.Replace(user.LoginDevice, ""), user.LoginDevice).Trim(';');
        //                        }
        //                    }
        //                    else
        //                    {
        //                        model.LoginIP = user.LoginIP;
        //                        model.LoginDevice = user.LoginDevice;
        //                    }
        //                }
        //                else
        //                {
        //                    model.LoginIP = user.LoginIP;
        //                    model.LoginDevice = user.LoginDevice;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            model.LoginIP = user.LoginIP;
        //            model.LoginDevice = user.LoginDevice;
        //        }
        //        strSql = string.Format("UPDATE T_User SET LoginIP = '{0}', LoginTime = sysdate, LoginDevice = '{1}' WHERE ID = {2} ", model.LoginIP, model.LoginDevice, user.ID);
        //        int i = dbFactory.ExecuteNonQuery2(CommandType.Text, strSql);
        //        return i >= 1;
        //    }

        //    internal bool ChangeUserPassword(UserInfo user, ref string strError)
        //    {
        //        string strSql;
        //        strSql = string.Format("UPDATE T_User SET Password = '{0}', Modifyer = '{1}', ModifyTime = sysdate WHERE ID = {2} ", user.Password, user.Modifyer, user.ID);
        //        int i = dbFactory.ExecuteNonQuery2(CommandType.Text, strSql);
        //        return i >= 1;
        //    }

        //    internal bool ClearLoginTime(UserInfo user, ref string strError)
        //    {
        //        string strSql;
        //        strSql = "UPDATE T_User SET LoginIP = null, LoginTime = null, LoginDevice = null WHERE (SUBSTR(LOGINIP,0,2) = 'PC'AND (LOGINTIME + (30/60/24)) <= SYSDATE) OR (USERTYPE <> 1 AND LOGINTIME IS NULL) ";
        //        dbFactory.ExecuteNonQuery2(CommandType.Text, strSql);
        //        if (user.ID >= 1 && user.UserType == 1)
        //        {
        //            strSql = string.Format("UPDATE T_User SET LoginIP = ltrim(rtrim(replace(LoginIP,'{1}',''),';'),';'), LoginTime = null, LoginDevice = ltrim(rtrim(replace(LoginDevice,'{2}',''),';'),';') WHERE ID = {0} ", user.ID, user.LoginIP, user.LoginDevice);
        //        }
        //        else
        //        {
        //            strSql = string.Format("UPDATE T_User SET LoginIP = null, LoginTime = null, LoginDevice = null WHERE ID = {0} ", user.ID);
        //        }
        //        int i = dbFactory.ExecuteNonQuery2(CommandType.Text, strSql);
        //        strSql = "UPDATE T_User SET LoginIP = replace(LoginIP,';;',';'), LoginDevice = replace(LoginDevice,';;',';')";
        //        dbFactory.ExecuteNonQuery2(CommandType.Text, strSql);
        //        return i >= 1;
        //    }
        //}
    }
}
