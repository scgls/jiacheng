//************************************************************
//******************************作者：方颖*********************
//******************************时间：2016/10/20 11:01:37*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using BILBasic.Common;
using BILWeb.Login.User;
using BILBasic.User;
using Oracle.ManagedDataAccess.Client;
using BILWeb.Warehouse;
using System.Data;

namespace BILWeb.TransportSupplier
{
    public partial class T_TransportSupplier_DB : Base_DB<T_TransportSupplier>
    {


        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_TransportSupplier ToModel(IDataReader reader)
        {
            T_TransportSupplier t_transupplier = new T_TransportSupplier();
            t_transupplier.ID =dbFactory.ToModelValue(reader, "ID").ToInt32();
        //    t_transupplier.TDID = dbFactory.ToModelValue(reader, "TDID").ToInt32();
            t_transupplier.TransportSupplierID = dbFactory.ToModelValue(reader, "transportsupplierid").ToInt32();
            t_transupplier.TransportSupplierName = dbFactory.ToModelValue(reader, "transportsuppliername").ToDBString();
            t_transupplier.Remark = dbFactory.ToModelValue(reader, "remark").ToDBString();
         //   t_transupplier.ErpVoucherNo = dbFactory.ToModelValue(reader, "erpvoucherno").ToDBString();
         //   t_transupplier.PlateNumber = dbFactory.ToModelValue(reader, "platenumber").ToDBString();
         //   t_transupplier.Volume = dbFactory.ToModelValue(reader, "volume").ToDecimal();
         //   t_transupplier.Weight = dbFactory.ToModelValue(reader, "weight").ToDecimal();
         //   t_transupplier.CartonNum = dbFactory.ToModelValue(reader, "cartonnum").ToDecimal();
         //   t_transupplier.Feight = dbFactory.ToModelValue(reader, "feight").ToDecimal();
         //   t_transupplier.Destina = dbFactory.ToModelValue(reader, "destina").ToDBString();
            t_transupplier.CreateTime = dbFactory.ToModelValue(reader, "createtime").ToDateTime();
            t_transupplier.Creater = dbFactory.ToModelValue(reader, "creater").ToDBString();
            return t_transupplier;
        }



        protected override string GetViewName()
        {
            return "v_transportsupplier";
        }

        protected override string GetTableName()
        {
            return "T_TransportSupplier";
        }

        protected override string GetSaveProcedureName()
        {
            return "P_Save_T_Transportsupplier";
        }

        protected override string GetDeleteProcedureName()
        {
            return "P_DELETE_T_Transportsupplier";
        }

        protected override IDataParameter[] GetSaveModelIDataParameter(T_TransportSupplier model)
        {
            OracleParameter[] param = new OracleParameter[]{
               new OracleParameter("@bResult",OracleDbType.Int32),
               new OracleParameter("@ErrorMsg",OracleDbType.NVarchar2,1000),

               new OracleParameter("@v_ID", model.ID.ToOracleValue()),
               new OracleParameter("@v_TransportSupplierID", model.TransportSupplierID.ToOracleValue()),
               new OracleParameter("@v_TransportSupplierName", model.TransportSupplierName.ToOracleValue()),
               new OracleParameter("@v_Remark", model.Remark.ToOracleValue()),
               // new OracleParameter("@v_IsDel", model.IsDel.ToOracleValue()),
               new OracleParameter("@v_Creater", model.Creater.ToOracleValue()),
               new OracleParameter("@v_CreateTime", model.CreateTime.ToOracleValue()),
               new OracleParameter("@v_Modifyer", model.Modifyer.ToOracleValue()),
               new OracleParameter("@v_ModifyTime", model.ModifyTime.ToOracleValue())
              };

            param[0].Direction = System.Data.ParameterDirection.Output;
            param[1].Direction = System.Data.ParameterDirection.Output;
            param[2].Direction = System.Data.ParameterDirection.InputOutput;


            return param;
        }

        protected override string GetFilterSql(UserModel user, T_TransportSupplier model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";

            if (!Common_Func.IsNullOrEmpty(model.TransportSupplierID.ToString()) || !Common_Func.IsNullOrEmpty(model.TransportSupplierName.ToString()))
            {
                strSql += strAnd;
                strSql += " (Transportsupplierid LIKE '%" + model.TransportSupplierID + "%' OR TransportSupplierName Like '%" + model.TransportSupplierName + "%')  ";
            }
            return strSql;
        }

        internal List<T_TransportSupplier> GetModelListBySql(UserInfo user, bool IncludNoCheck)
        {
            string strSql = string.Empty;
            strSql = "select * from t_Transportsupplier T order by ID";
            return GetModelListBySql(strSql);
        }

        protected override List<string> GetSaveSql(UserModel user, ref T_TransportSupplier model)
        {
            throw new NotImplementedException();
        }
    }
}
