
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.User;
using BILBasic.Common;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BILWeb.DepInterface
{
    public partial class T_DepInterface_DB : BILBasic.Basing.Factory.Base_DB<T_DepInterfaceInfo>
    {

        /// <summary>
        /// 添加t_depinterface
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_DepInterfaceInfo t_depinterface)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user,ref  T_DepInterfaceInfo model)
        {
            string strSql = string.Empty;

            List<string> lstSql = new List<string>();

            

            if (model.ID <= 0)
            {

                int voucherID = base.GetTableID("seq_interface_id");

                model.ID = voucherID;

                strSql = string.Format("insert into t_Depinterface(Id, Vouchername, Vouchertype, Function, Dllname, Route, Functionname, Functionnote, Creater, Createtime, Classname, Isdel)" +
                             "values ('{9}','{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',Sysdate,'{8}','1')",
                             model.VoucherName, model.VoucherType, model.Function, model.DLLName, model.Route, model.FunctionName, model.FunctionNote, user.UserNo, model.ClassName, voucherID);

                lstSql.Add(strSql);
            }
            else 
            {
                strSql = string.Format("update t_Depinterface a set a.Vouchername = '{0}' ,a.Vouchertype = '{1}',a.Function = '{2}',a.Dllname = '{3}',a.Route='{4}',a.Functionname='{5}'," +
                "a.Functionnote = '{6}',a.Modifyer = '{7}',a.Modifytime = Sysdate,a.Classname='{8}' where id = '{9}'",
                model.VoucherName,model.VoucherType,model.Function,model.DLLName,model.Route,model.FunctionName,model.FunctionNote,user.UserNo,model.ClassName,model.ID);
                
                lstSql.Add(strSql);
            }
            

            return lstSql;
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_DepInterfaceInfo ToModel(IDataReader reader)
        {
            T_DepInterfaceInfo t_depinterface = new T_DepInterfaceInfo();

            t_depinterface.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_depinterface.VoucherName = dbFactory.ToModelValue(reader, "VOUCHERNAME").ToInt32();
            t_depinterface.VoucherType = dbFactory.ToModelValue(reader, "VOUCHERTYPE").ToInt32();
            t_depinterface.Function = dbFactory.ToModelValue(reader, "FUNCTION").ToInt32();
            t_depinterface.DLLName = (string)dbFactory.ToModelValue(reader, "DLLNAME");
            t_depinterface.ClassName = (string)dbFactory.ToModelValue(reader, "ClassName");
            t_depinterface.Route = (string)dbFactory.ToModelValue(reader, "ROUTE");
            t_depinterface.FunctionName = (string)dbFactory.ToModelValue(reader, "FUNCTIONNAME");
            t_depinterface.FunctionNote = (string)dbFactory.ToModelValue(reader, "FUNCTIONNOTE");
            t_depinterface.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_depinterface.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_depinterface.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_depinterface.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_depinterface.StrVoucherName = dbFactory.ToModelValue(reader, "STRVOUCHERNAME").ToDBString();
            t_depinterface.StrFunction = dbFactory.ToModelValue(reader, "strFunction").ToDBString();
            t_depinterface.StrVoucherType = dbFactory.ToModelValue(reader, "StrVoucherType").ToDBString();

            return t_depinterface;
        }

        protected override string GetViewName()
        {
            return "V_DEPINTERFACE";
        }

        protected override string GetTableName()
        {
            return "T_DEPINTERFACE";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }


        protected override List<string> GetDeleteSql(UserModel user, T_DepInterfaceInfo model)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            strSql = "delete t_Depinterface where id = '" + model.ID + "'";

            lstSql.Add(strSql);

            return lstSql;
        }

        protected override string GetFilterSql(UserModel user, T_DepInterfaceInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";

            

            

            if (!string.IsNullOrEmpty(model.StrVoucherName))
            {
                strSql += strAnd;
                strSql += " (StrVoucherName Like '" + model.StrVoucherName + "%' )";
            }

            if (!string.IsNullOrEmpty(model.StrVoucherType))
            {
                strSql += strAnd;
                strSql += " (StrVoucherType ='" + model.StrVoucherType + "' )";
            }

            if (!string.IsNullOrEmpty(model.StrFunction))
            {
                strSql += strAnd;
                strSql += " (StrFunction Like '" + model.StrFunction + "%' )";
            }

            if (model.DateFrom != null)
            {
                strSql += strAnd;
                strSql += " CreateTime >= " + model.DateFrom.ToDateTime().Date.AddDays(-1).ToOracleTimeString() + " ";
            }

            if (model.DateTo != null)
            {
                strSql += strAnd;
                strSql += " CreateTime <= " + model.DateTo.ToDateTime().Date.AddDays(1).ToOracleTimeString() + " ";
            }

            


            return strSql;
        }

    }
}
