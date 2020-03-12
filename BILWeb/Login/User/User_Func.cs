using BILWeb.Login.User;
using System;
using System.Linq;
using System.Collections.Generic;
using BILWeb.Menu;
using BILWeb.UserGroup;
using BILWeb.Warehouse;
using BILBasic.Basing.Factory;

using BILWeb.Area;
using BILBasic.Common;
using BILBasic.User;
using Newtonsoft.Json;


namespace BILWeb.Login.User
{
    public class User_Func : TBase_Func<User_DB, UserInfo>, IUserService
    {

        protected override bool CheckModelBeforeSave(UserInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (model.UserStatus <= 0)
            {
                strError = "用户状态必须选择！";
                return false;
            }

            if (model.UserType <= 0)
            {
                strError = "用户类型必须选择！";
                return false;
            }


            if (BILBasic.Common.Common_Func.IsNullOrEmpty(model.UserNo))
            {
                strError = "登录名不能为空！";
                return false;
            }
            if (Common_Func.IsNullOrEmpty(model.UserName))
            {
                strError = "用户姓名不能为空！";
                return false;
            }
            if (Common_Func.IsNullOrEmpty(model.PassWord)|| Common_Func.IsNullOrEmpty(model.RePassword))
            {
                strError = "登陆密码和确认密码不能为空！";
                return false;
            }
            
            //if (Common_Func.IsNullOrEmpty(model.GroupCode))
            //{
            //    strError = "用户分组不能为空！";
            //    return false;
            //}
            if (!Common_Func.IsEqualString(model.PassWord, model.RePassword))
            {
                strError = "确认密码与登陆密码不一致！";
                return false;
            }
            
           
            return true;
        }

        protected override UserInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

        protected override string GetModelChineseName()
        {
            return "用户";
        }

        public bool UserLogin(ref UserInfo user, ref string strError)
        {
            try
            {
                UserInfo model;
                User_DB _db = new User_DB();
                DateTime CurrentTime;

                model = _db.GetModelBySql(user);
                
                if (model == null)
                {
                    int iSucc = _db.GetScalarBySql(user);
                    if (iSucc <= 0)
                    {
                        strError = "该用户不存在,请检查大小写是否输入正确!";
                        return false;
                    }
                    else
                    {
                        strError = "密码输入错误,忘记密码请联系管理员重置!";
                        return false;
                    }
                }

                CurrentTime = model.CurrentTime;

                if (model.UserStatus == 2)
                {
                    strError = string.Format("用户【{0}】已停用", model.UserName);
                    return false;
                }

                if (model.IsDel == 2)
                {
                    strError = string.Format("用户【{0}】已删除", model.UserName);
                    return false;
                }

                user = model;

                T_UserGroup_Func ugf = new T_UserGroup_Func();
                List<T_UserGroupInfo> lstGroup = new List<T_UserGroupInfo>();

                if (ugf.GetModelListBySql(user, ref lstGroup)) 
                {
                    user.lstUserGroup = lstGroup;
                }

                T_MENU_Func tmfun = new T_MENU_Func();
                List<T_MenuInfo> lstMenu = new List<T_MenuInfo>();

                if (tmfun.GetModelListBySql(user, ref lstMenu,true)) 
                {
                    //user.lstMenu = lstMenu.Where(t => t.MenuType <= 3).ToList();
                    lstMenu = lstMenu.Where(t => t.MenuType ==1 && t.MenuStatus==1 && t.IsChecked==true ).ToList();
                    var lstMenuParent = lstMenu.Where(t => t.ParentID == 0).OrderBy(t=>t.NodeSort).ToList();

                    foreach (var item in lstMenuParent)
                    {
                        item.lstSubMenu = lstMenu.FindAll(t => t.ParentID == item.ID);
                    }
                    user.lstMenu = lstMenuParent;         
                }

                T_WareHouse_Func twfun = new T_WareHouse_Func();
                List<T_WareHouseInfo> lstWarehouse = new List<T_WareHouseInfo>();
                if (twfun.GetModelListBySql(user, ref lstWarehouse)) 
                {
                    user.lstWarehouse = lstWarehouse;
                }

                return true;
                
            }
            catch (Exception ex)
            {
                strError = ex.Message+ex.StackTrace;
                return false;
            }
        }

        public string UserLoginADF(string UserJson)
        {
            BaseMessage_Model<UserInfo> messageModel = new BaseMessage_Model<UserInfo>();

            try
            {
                
                UserInfo model;
                User_DB _db = new User_DB();
                DateTime CurrentTime;
               
                LogNet.LogInfo("UserLoginADF---" + UserJson);
                UserInfo user = BILBasic.JSONUtil.JSONHelper.JsonToObject<UserInfo>(UserJson);
                //UserInfo user = JsonConvert.DeserializeObject<UserInfo>(UserJson);
           
                model = _db.GetModelBySql(user);

                if (model == null)
                {
                    int iSucc = _db.GetScalarBySql(user);
                    if (iSucc <= 0)
                    {
                        messageModel.Message = "该用户不存在,请检查大小写是否输入正确!";
                        messageModel.HeaderStatus = "E";
                        return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<UserInfo>>(messageModel);
                    }
                    else
                    {
                        messageModel.Message = "密码输入错误,忘记密码请联系管理员重置!";
                        messageModel.HeaderStatus = "E";
                        return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<UserInfo>>(messageModel);                        
                    }
                }

                CurrentTime = model.CurrentTime;

                if (user.WarehouseID != -100)
                {
                    if (!model.WarehouseCode.Contains(user.WarehouseID.ToString()))
                    {
                        messageModel.Message = "登录账户与所选仓库不一致！";
                        messageModel.HeaderStatus = "E";
                        return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<UserInfo>>(messageModel);
                    }
                }
                

                if (model.UserStatus == 2)
                {
                    messageModel.Message = string.Format("用户【{0}】已停用", model.UserName);
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<UserInfo>>(messageModel);                     
                }

                if (model.IsDel == 2)
                {
                    messageModel.Message = string.Format("用户【{0}】已删除", model.UserName);
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<UserInfo>>(messageModel);
                }

                model.WarehouseID = user.WarehouseID;
                user = model;

                T_UserGroup_Func ugf = new T_UserGroup_Func();
                List<T_UserGroupInfo> lstGroup = new List<T_UserGroupInfo>();

                if (ugf.GetModelListBySql(user, ref lstGroup))
                {
                    user.lstUserGroup = lstGroup;
                }

                //暂时屏蔽
                T_MENU_Func tmfun = new T_MENU_Func();
                List<T_MenuInfo> lstMenu = new List<T_MenuInfo>();

                if (tmfun.GetModelListBySql(user, ref lstMenu,false))
                {
                    user.lstMenu = lstMenu.Where(t => t.MenuType==4).ToList();
                }

                //根据用户配置的仓库ID，获取对应的收货，发货库位
                T_Area_Func tafun = new T_Area_Func();
                List<T_AreaInfo> lstArea = new List<T_AreaInfo>();

                if (tafun.GetModelListBySql(user.WarehouseID, ref lstArea))
                {
                    T_AreaInfo areaInfo = new T_AreaInfo();
                    //查找收货待检库位
                    areaInfo = lstArea.Find(t => t.AreaType == 2);
                    if (areaInfo != null)
                    {
                        user.ReceiveHouseID = areaInfo.HouseID;
                        user.ReceiveAreaID = areaInfo.ID;
                        user.ReceiveAreaNo = areaInfo.AreaNo;
                        user.ReceiveWareHouseNo = areaInfo.WarehouseNo;
                        user.ReceiveHouseNo = areaInfo.HouseNo;
                        user.ReceiveWareHouseName = areaInfo.WarehouseName;
                        user.QuanUserNo = areaInfo.QuanUserNo;
                        user.QuanUserName = areaInfo.QuanUserName;
                        //user.lstQuanUser = _db.GetQuanUser(user.QuanUserNo);
                    }

                    //查找拣货临时库位
                    areaInfo = lstArea.Find(t => t.AreaType == 3);
                    if (areaInfo != null)
                    {
                        user.PickHouseID = areaInfo.HouseID;
                        user.PickAreaID = areaInfo.ID;
                        user.PickWareHouseID = areaInfo.WarehouseID;
                        user.PickAreaNo = areaInfo.AreaNo;
                        user.PickWareHouseNo = areaInfo.WarehouseNo;
                        user.PickHouseNo = areaInfo.HouseNo;
                        user.PickWareHouseName = areaInfo.WarehouseName;
                    }

                    areaInfo = lstArea.Find(t => t.AreaType == 4);
                    if (areaInfo != null)
                    {
                        user.ToSampWareHouseNo = areaInfo.WarehouseNo;
                        user.ToSampAreaNo = areaInfo.AreaNo;
                    }

                }

                //T_WareHouse_Func twfun = new T_WareHouse_Func();
                //List<T_WareHouseInfo> lstWarehouse = new List<T_WareHouseInfo>();
                //if (twfun.GetModelListBySql(user, ref lstWarehouse))
                //{
                //    user.lstWarehouse = lstWarehouse;
                //}

                messageModel.Message = "登录成功！";
                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = user;
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<UserInfo>>(messageModel);


                //return JsonConvert.SerializeObject(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.Message = ex.Message;
                messageModel.HeaderStatus = "E";
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<UserInfo>>(messageModel);
            }
        }


        public string GetWareHouseByUserADF(string UserNo)
        {
            BaseMessage_Model<UserInfo> messageModel = new BaseMessage_Model<UserInfo>();

            try
            {

                UserInfo model;
                User_DB _db = new User_DB();               

                if (string.IsNullOrEmpty(UserNo)) 
                {
                    messageModel.Message = "传入用户编号为空！";
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<UserInfo>>(messageModel);
                }

                model = _db.GetModelByFilterByUserNo(UserNo);

                if (model == null)
                {
                    messageModel.Message = "该用户不存在,请检查大小写是否输入正确!";
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<UserInfo>>(messageModel);
                }
                

                if (model.UserStatus == 2)
                {
                    messageModel.Message = string.Format("用户【{0}】已停用", model.UserName);
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<UserInfo>>(messageModel);
                }

                if (model.IsDel == 2)
                {
                    messageModel.Message = string.Format("用户【{0}】已删除", model.UserName);
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<UserInfo>>(messageModel);
                }

                

                
                T_WareHouse_Func twfun = new T_WareHouse_Func();
                List<T_WareHouseInfo> lstWarehouse = new List<T_WareHouseInfo>();
                if (twfun.GetModelListBySql(model, ref lstWarehouse))
                {
                    model.lstWarehouse = lstWarehouse;
                }

                messageModel.Message = "仓库获取成功！";
                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = model;
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<UserInfo>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.Message = ex.Message;
                messageModel.HeaderStatus = "E";
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<UserInfo>>(messageModel);
            }
        }


        /// <summary>
        /// 获取拣货组人员列表
        /// </summary>
        /// <param name="UserJson"></param>
        /// <returns></returns>
        public string GetPickUserListByUser(string UserJson) 
        {
            BaseMessage_Model<List<UserModel>> messageModel = new BaseMessage_Model<List<UserModel>>();

            try
            {
                if (string.IsNullOrEmpty(UserJson))
                {
                    messageModel.Message = "客户端传入用户为空！";
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<UserModel>>>(messageModel);
                }

                UserModel model = BILBasic.JSONUtil.JSONHelper.JsonToObject<UserModel>(UserJson);

                User_DB _db = new User_DB();
                List<UserModel> modelList = _db.GetUserGroupByUser(model);

                if (modelList == null || modelList.Count == 0) 
                {
                    messageModel.Message = "获取拣货人员列表为空！请先配置拣货组人员";
                    messageModel.HeaderStatus = "E";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<UserModel>>>(messageModel);
                }

                messageModel.HeaderStatus = "S";
                messageModel.ModelJson = modelList;

                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<UserModel>>>(messageModel);

            }
            catch (Exception ex) 
            {
                messageModel.Message = ex.Message;
                messageModel.HeaderStatus = "E";
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<UserModel>>>(messageModel);
            }
        }

        public bool ChangeUserPassword(UserInfo user, ref string strError)
        {
            if (user == null || user.ID == 0)
            {
                strError = "用户信息获取失败!";
                return false;
            }

            if (string.IsNullOrEmpty(user.PassWord))
            {
                strError = "新登录密码不能为空!";
                return false;
            }

            if (string.IsNullOrEmpty(user.RePassword)) 
            {
                strError = "确认新密码不能为空!";
                return false;
            }

            if (!user.PassWord.Equals(user.RePassword)) 
            {
                strError = "确认密码与登陆密码不一致！";
                return false;
            }

            User_DB _db = new User_DB();
            return _db.ChangeUserPassword(user, ref strError);
        }

    }
}
