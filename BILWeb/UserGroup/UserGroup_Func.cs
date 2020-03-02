using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILWeb.Login.User;


namespace BILWeb.UserGroup
{

    public partial class T_UserGroup_Func : TBase_Func<T_UserGroup_DB, T_UserGroupInfo>, IUserGroupService
    {

        protected override bool CheckModelBeforeSave(T_UserGroupInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (BILBasic.Common.Common_Func.IsNullOrEmpty(model.UserGroupNo)) 
            {
                strError = "分组编号不能为空！";
                return false;
            }

            if (BILBasic.Common.Common_Func.IsNullOrEmpty(model.UserGroupName))
            {
                strError = "分组名称不能为空！";
                return false;
            }

            if (model.UserGroupType <= 0)
            {
                strError = "请选择分组类型！";
                return false;
            }

            if (model.UserGroupStatus <= 0)
            {
                strError = "请选择分组状态！";
                return false;
            }

            return true;
        }

        protected override string GetModelChineseName()
        {
            return "用户组";
        }

        protected override T_UserGroupInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }


        public bool GetModelListBySql(UserInfo user, ref List<T_UserGroupInfo> modelList)
        {
            try
            {
                T_UserGroup_DB tdb = new T_UserGroup_DB();
                modelList = tdb.GetModelListBySql(user, false);

                if (modelList != null && modelList.Count > 0) { return true; }
                else { return false; }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
    }
}