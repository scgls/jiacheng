//************************************************************
//******************************作者：方颖*********************
//******************************时间：2016/10/20 10:43:54*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.DBA;
using BILBasic.Basing.Factory;
using BILWeb.Login.User;
using BILBasic.Common;
using BILWeb.UserGroup;
using BILBasic.User;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BILWeb.Menu
{
    public partial class T_MENU_DB : Base_DB<T_MenuInfo>
    {

        /// <summary>
        /// 添加T_MENU
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_MenuInfo model)
        {
            dbFactory.dbF.CreateParameters(23);
            dbFactory.dbF.AddParameters(0, "@bResult",SqlDbType.Int, 0);
            dbFactory.dbF.AddParameters(1, "@ErrorMsg", SqlDbType.NVarChar, 1000);
            dbFactory.dbF.AddParameters(2, "@v_ID", model.ID.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(3, "@v_MenuNo", model.MenuNo.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(4, "@v_MenuName", model.MenuName.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(5, "@v_MenuAbbName", model.MemuAbbName.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(6, "@v_MenuType", model.MenuType.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(7, "@v_ProjectName", model.ProjectName.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(8, "@v_IcoName", model.IcoName.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(9, "@v_SafeLevel", model.SafeLevel.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(10, "@v_IsDefault", model.IsDefault.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(11, "@v_NodeUrl", model.NodeUrl.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(12, "@v_NodeLevel", model.NodeLevel.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(13, "@v_NodeSort", model.NodeSort.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(14, "@v_ParentID", model.ParentID.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(15, "@v_MenuStatus", model.MenuStatus.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(16, "@v_Description", model.Description.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(17, "@v_IsDel", model.IsDel.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(18, "@v_Creater", model.Creater.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(19, "@v_CreateTime", model.CreateTime.ToOracleValue(), 0);            
            dbFactory.dbF.AddParameters(20, "@v_Modifyer", model.Modifyer.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(21, "@v_ModifyTime", model.ModifyTime.ToOracleValue(), 0);
            dbFactory.dbF.AddParameters(22, "@v_MenuStyle", model.MenuStyle.ToOracleValue(), 0);

            dbFactory.dbF.Parameters[0].Direction = System.Data.ParameterDirection.Output;
            dbFactory.dbF.Parameters[1].Direction = System.Data.ParameterDirection.Output;
            dbFactory.dbF.Parameters[2].Direction = System.Data.ParameterDirection.InputOutput;
            
            return dbFactory.dbF.Parameters;
            //throw new NotImplementedException();
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            //OracleParameter[] param = new OracleParameter[]{
            //    new OracleParameter("@bResult",OracleDbType.Int32),
            //   new OracleParameter("@ErrorMsg",OracleDbType.NVarchar2,1000),
               
            //   new OracleParameter("@v_ID", dbFactory.ToDBValue(t_menu.ID)),
            //   new OracleParameter("@v_MenuNo", dbFactory.ToDBValue(t_menu.MenuNo)),
            //   new OracleParameter("@v_MenuName", dbFactory.ToDBValue(t_menu.MenuName)),
            //   new OracleParameter("@v_MenuAbbName", t_menu.MemuAbbName.ToOracleValue()),
            //   new OracleParameter("@v_MenuType", t_menu.MenuType.ToOracleValue()),
            //   new OracleParameter("@v_ProjectName", t_menu.ProjectName.ToOracleValue()),
            //   new OracleParameter("@v_IcoName", t_menu.IcoName.ToOracleValue()),
            //   new OracleParameter("@v_SafeLevel", t_menu.SafeLevel.ToOracleValue()),
            //   new OracleParameter("@v_IsDefault", t_menu.IsDefault.ToOracleValue()),
            //   new OracleParameter("@v_NodeUrl", t_menu.NodeUrl.ToOracleValue()),
            //   new OracleParameter("@v_NodeLevel", t_menu.NodeLevel.ToOracleValue()),
            //   new OracleParameter("@v_NodeSort", t_menu.NodeSort.ToOracleValue()),
            //   new OracleParameter("@v_ParentID", t_menu.ParentID.ToOracleValue()),
            //   new OracleParameter("@v_MenuStatus", t_menu.MenuStatus.ToOracleValue()),
            //   new OracleParameter("@v_Description", t_menu.Description.ToOracleValue()),
            //   new OracleParameter("@v_IsDel", t_menu.IsDel.ToOracleValue()),
            //   new OracleParameter("@v_Creater", t_menu.Creater.ToOracleValue()),
            //   new OracleParameter("@v_CreateTime", t_menu.CreateTime.ToOracleValue()),
            //   new OracleParameter("@v_Modifyer", t_menu.Modifyer.ToOracleValue()),
            //   new OracleParameter("@v_ModifyTime", t_menu.ModifyTime.ToOracleValue()),
            //   new OracleParameter("@v_ModifyTime", t_menu.MenuStyle.ToOracleValue()),
            //  };

            //param[0].Direction = System.Data.ParameterDirection.Output;
            //param[1].Direction = System.Data.ParameterDirection.Output;
            //param[2].Direction = System.Data.ParameterDirection.InputOutput;
           
            //return param;


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_MenuInfo t_menu)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_MenuInfo ToModel(IDataReader reader)
        {
            T_MenuInfo t_menu = new T_MenuInfo();

            t_menu.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_menu.MenuNo = (string)dbFactory.ToModelValue(reader, "MENUNO");
            t_menu.MenuName = (string)dbFactory.ToModelValue(reader, "MENUNAME");
            t_menu.MemuAbbName = (string)dbFactory.ToModelValue(reader, "MENUABBNAME");
            t_menu.MenuType = dbFactory.ToModelValue(reader, "MENUTYPE").ToInt32();
            t_menu.ProjectName = (string)dbFactory.ToModelValue(reader, "PROJECTNAME");
            t_menu.IcoName = (string)dbFactory.ToModelValue(reader, "ICONAME");
            t_menu.SafeLevel = dbFactory.ToModelValue(reader, "SAFELEVEL").ToDecimal();
            t_menu.IsDefault = dbFactory.ToModelValue(reader, "ISDEFAULT").ToDecimal();
            t_menu.Mnemonic = dbFactory.ToModelValue(reader, "MNEMONIC").ToDecimal();
            t_menu.Mnemoniccode = (string)dbFactory.ToModelValue(reader, "MNEMONICCODE");
            t_menu.NodeUrl = (string)dbFactory.ToModelValue(reader, "NODEURL");
            t_menu.NodeLevel = dbFactory.ToModelValue(reader, "NODELEVEL").ToInt32();
            t_menu.NodeSort = dbFactory.ToModelValue(reader, "NODESORT").ToInt32();
            t_menu.ParentID = dbFactory.ToModelValue(reader, "PARENTID").ToInt32();
            t_menu.MenuStatus =dbFactory.ToModelValue(reader, "MENUSTATUS").ToInt32();
            t_menu.Description = (string)dbFactory.ToModelValue(reader, "DESCRIPTION");
            t_menu.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToDecimal();
            t_menu.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_menu.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_menu.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_menu.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_menu.MenuStyle = dbFactory.ToModelValue(reader, "MENUSTYLE").ToDecimal();

            if (Common_Func.readerExists(reader, "ParentName")) t_menu.ParentName = reader["ParentName"].ToDBString();
            if (Common_Func.readerExists(reader, "MenuStyle")) t_menu.MenuStyle = reader["MenuStyle"].ToInt32();

            if (Common_Func.readerExists(reader, "IsChecked")) t_menu.IsChecked = reader["IsChecked"].ToBoolean();
            if (Common_Func.readerExists(reader, "StrMenuType")) t_menu.StrMenuType = reader["StrMenuType"].ToDBString();
            if (Common_Func.readerExists(reader, "StrMenuStatus")) t_menu.StrMenuStatus = reader["StrMenuStatus"].ToDBString();
            if (Common_Func.readerExists(reader, "SonQty")) t_menu.SonQty = reader["SonQty"].ToInt32();
            if (Common_Func.readerExists(reader, "StrMenuStyle")) t_menu.StrMenuStyle = reader["StrMenuStyle"].ToDBString();


            t_menu.BIsDefault = t_menu.IsDefault.ToBoolean();
            t_menu.BHaveParameter = t_menu.MenuType == 2 && t_menu.ProjectName.IndexOf(':') >= 0;

            t_menu.StrCreateTime = t_menu.CreateTime.ToShowTime();
            t_menu.StrModifyTime = t_menu.ModifyTime.ToShowTime();

            return t_menu;
        }

        protected override string GetViewName()
        {
            return "V_MENU";
        }

        protected override string GetTableName()
        {
            return "T_MENU";
        }

        protected override string GetSaveProcedureName()
        {
            return "P_SAVE_T_MENU";
        }


        public List<T_MenuInfo> GetModelListBySql(UserInfo user, bool IncludNoCheck) 
        {
            string strSql = string.Empty;
            if (user.ID >= 1)
            {
                strSql = string.Format("SELECT DISTINCT 2 AS IsChecked,T_Menu.* from T_Menu WHERE ISDEL <> 2 AND MenuStatus <> 2 AND ID IN (SELECT MenuID FROM T_GroupWithMenu WHERE GroupID IN (SELECT T_USERGROUP.ID FROM T_UserWithGroup JOIN T_USERGROUP ON T_UserWithGroup.Groupid = T_USERGROUP.ID WHERE T_USERGROUP.ISDEL <> 2 AND T_USERGROUP.USERGROUPSTATUS <> 2 AND T_UserWithGroup.UserID = {0})) ", user.ID);
                if (IncludNoCheck) strSql += string.Format("UNION SELECT DISTINCT 1 AS IsChecked,T_Menu.* from T_Menu WHERE ISDEL <> 2 AND MenuStatus <> 2 AND ID NOT IN (SELECT MenuID FROM T_GroupWithMenu WHERE GroupID IN (SELECT T_USERGROUP.ID FROM T_UserWithGroup JOIN T_USERGROUP ON T_UserWithGroup.Groupid = T_USERGROUP.ID WHERE T_USERGROUP.ISDEL <> 2 AND T_USERGROUP.USERGROUPSTATUS <> 2 AND T_UserWithGroup.UserID = {0})) ", user.ID);
                if (user.UserType == 1) strSql += "UNION SELECT DISTINCT 2 AS IsChecked,T_Menu.* from T_Menu WHERE ISDEL <> 2 AND MenuStatus <> 2 AND SafeLevel <> 0 ";
                strSql = string.Format("SELECT * FROM ({0}) T ORDER BY NodeLevel, NodeSort, ID  ", strSql);
            }
            else
            {
                if (IncludNoCheck) strSql = "SELECT DISTINCT 1 AS IsChecked,T_Menu.* FROM T_Menu WHERE ISDEL <> 2 AND MenuStatus <> 2 ORDER BY NodeLevel, NodeSort, ID ";
                else strSql = "SELECT DISTINCT 2 AS IsChecked,T_Menu.* FROM T_Menu WHERE 1 = 2";
            }

            return GetModelListBySql(strSql);
        }


        internal List<T_MenuInfo> GetMenuListByUserGroupID(int UserGroupID, bool IncludNoCheck)
        {
            string strSql = string.Empty;
            if (UserGroupID >= 1)
            {
                strSql = string.Format("SELECT DISTINCT 2 AS IsChecked,T_Menu.* FROM T_Menu WHERE ISDEL <> 2 AND ID IN (SELECT MenuID FROM T_GroupWithMenu WHERE GroupID = {0}) ", UserGroupID);
                if (IncludNoCheck) strSql += string.Format("UNION SELECT DISTINCT 1 AS IsChecked,T_Menu.* FROM T_Menu WHERE ISDEL <> 2 AND ID NOT IN (SELECT MenuID FROM T_GroupWithMenu WHERE GroupID = {0}) ", UserGroupID);
                strSql = string.Format("SELECT * FROM ({0}) T ORDER BY NodeLevel, NodeSort, ID ", strSql);
            }
            else
            {
                if (IncludNoCheck) strSql = "SELECT DISTINCT 1 AS IsChecked,T_Menu.* FROM T_Menu WHERE ISDEL <> 2 ORDER BY NodeUrl, NodeSort, ID ";
                else strSql = "SELECT DISTINCT 2 AS IsChecked,T_Menu.* FROM T_Menu WHERE 1 = 2";
            }

            return GetModelListBySql(strSql);
        }

        internal bool SaveUserGroupMenuToDB(UserInfo user, T_MenuInfo model, int UserGroupID, ref string strError)
        {
            try
            {
                int isCheck = model.IsChecked==true?1:0;
                dbFactory.dbF.CreateParameters(4);
                dbFactory.dbF.AddParameters(0, "@ErrorMsg", SqlDbType.NVarChar, 1000);
                dbFactory.dbF.AddParameters(1, "@v_MenuID", model.ID, 0);
                dbFactory.dbF.AddParameters(2, "@v_UserGroupID", UserGroupID, 0);
                dbFactory.dbF.AddParameters(3, "@v_IsChecked", isCheck, 0);

                dbFactory.dbF.Parameters[0].Direction = System.Data.ParameterDirection.Output;


                dbFactory.ExecuteNonQuery2(dbFactory.ConnectionStringLocalTransaction, CommandType.StoredProcedure, "P_Save_T_GroupWithMenu", dbFactory.dbF.Parameters);
                

                //int iOut = 0;
                //int i = dbFactory.RunProcedure("P_Save_T_GroupWithMenu", dbFactory.dbF.Parameters, out iOut);
                //strError = dbFactory.dbF.Parameters[1].Value.ToString();

            //    if (i == -1)//(int)param[0].Value == -1
            //    {
            //        strError = param[1].Value.ToString();
            //    }
            //    else
            //    {
            //        succ = true;
            //        int modelID = Convert.ToInt32(((DbParameter)param[2]).Value);
            //        //model.ID = (int)param[2].Value;
            //        model = GetModelByID(modelID);
            //        if (model == null)
            //        {
            //            throw new Exception("GetModelByID(modelID)出错，没有找到相符的记录，可能是视图有错误！");
            //        }
            //    }
            //    return succ;
            //}



            ////    OracleParameter[] param = new OracleParameter[]{
            ////   new OracleParameter("@ErrorMsg",OracleDbType.NVarchar2,1000),
               
            ////   new OracleParameter("@v_MenuID", model.ID.ToOracleValue()),
            ////   new OracleParameter("@v_UserGroupID", UserGroupID.ToOracleValue()),
            ////   new OracleParameter("@v_IsChecked", model.IsChecked.ToOracleValue()),
            ////};
            //   // param[0].Direction = ParameterDirection.Output;

            //    dbFactory.ExecuteNonQuery2(CommandType.StoredProcedure, "P_Save_T_GroupWithMenu", param);

                string ErrorMsg = dbFactory.dbF.Parameters[0].Value.ToDBString();
                if (ErrorMsg.StartsWith("执行错误"))
                {
                    throw new Exception(ErrorMsg);
                }
                else
                {
                    return true;
                    //return string.IsNullOrEmpty(ErrorMsg);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
