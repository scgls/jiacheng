using BILBasic.DBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;
using BILBasic.User;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BILWeb.InStock
{
    public partial class T_SerialNo_DB : BILBasic.Basing.Factory.Base_DB<T_SerialNoInfo>
    {

        /// <summary>
        /// 添加t_serialno
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_SerialNoInfo t_serialno)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user,ref  T_SerialNoInfo t_serialno)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据条件获取单个对象
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <returns></returns>
        public T_SerialNoInfo GetSerialModel(string SerialNo) 
        {
            string Filter = "serialno = '"+SerialNo+"'";

            return base.GetModelByFilter(Filter);
        }

        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_SerialNoInfo ToModel(IDataReader reader)
        {
            T_SerialNoInfo t_serialno = new T_SerialNoInfo();

            t_serialno.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_serialno.FacMaterialNo = (string)dbFactory.ToModelValue(reader, "FACMATERIALNO");
            t_serialno.SerialNo = (string)dbFactory.ToModelValue(reader, "SERIALNO");
            t_serialno.ERPVoucherNo = (string)dbFactory.ToModelValue(reader, "ERPVOUCHERNO");
            t_serialno.TaskNo = dbFactory.ToModelValue(reader, "TaskNo").ToDBString();
            t_serialno.VoucherNo = dbFactory.ToModelValue(reader, "VoucherNo").ToDBString();

            return t_serialno;
        }

        protected override string GetViewName()
        {
            return "V_SERIALNO";
        }

        protected override string GetTableName()
        {
            return "T_SERIALNO";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        /// <summary>
        /// 收货序列号扫描验证
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool CheckSerialNo(string SerialNo, ref string strError) 
        {
            int i = 0;

            string strSql  =string.Empty;

            //strSql = string.Format("SELECT COUNT(1) FROM T_SERIALNO WHERE SERIALNO = '{0}'", SerialNo);
            //i= GetScalarBySql(strSql);

            //if (i > 0) 
            //{
            //    strError = "该序列号已经收货！";
            //    return false;
            //}

            strSql = string.Format("SELECT COUNT(1) FROM T_STOCK WHERE SERIALNO = '{0}'", SerialNo);
            i = GetScalarBySql(strSql).ToInt32();

            if (i > 0)
            {
                strError = "该序列号已经收货！";
                return false;
            }

            strSql = string.Format("SELECT COUNT(1) FROM t_Palletdetail WHERE SERIALNO = '{0}'", SerialNo);
            i = GetScalarBySql(strSql).ToInt32();

            if (i > 0)
            {
                strError = "该序列号已经拼托！";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 收货数据提交序列号验证
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool CheckSerialNoInStockCommit(string SerialNo, ref string strError)
        {
            int i = 0;

            string strSql = string.Empty;

            //strSql = string.Format("SELECT COUNT(1) FROM T_SERIALNO WHERE SERIALNO = '{0}'", SerialNo);
            //i= GetScalarBySql(strSql);

            //if (i > 0) 
            //{
            //    strError = "该序列号已经收货！";
            //    return false;
            //}

            strSql = string.Format("SELECT COUNT(1) FROM T_STOCK WHERE SERIALNO = '{0}'", SerialNo);
            i = GetScalarBySql(strSql).ToInt32();

            if (i > 0)
            {
                strError = "该序列号已经收货！";
                return false;
            }

            //strSql = string.Format("SELECT COUNT(1) FROM t_Palletdetail WHERE SERIALNO = '{0}'", SerialNo);
            //i = GetScalarBySql(strSql).ToInt32();

            //if (i > 0)
            //{
            //    strError = "该序列号已经拼托！";
            //    return false;
            //}

            return true;
        }

        public bool CheckSerialNoInStock(string SerialNo, ref string strError)
        {
            int i = 0;

            string strSql = string.Empty;

            strSql = string.Format("SELECT COUNT(1) FROM T_STOCK WHERE SERIALNO = '{0}'", SerialNo);
            i = GetScalarBySql(strSql).ToInt32();

            if (i <= 0)
            {
                strError = "该序列号不存在，请确认是否收货！";
                return false;
            }

            strSql = string.Format("SELECT COUNT(1) FROM T_STOCK WHERE SERIALNO = '{0}' and receivestatus = '2' ", SerialNo);
            i = GetScalarBySql(strSql).ToInt32();

            if (i > 0)
            {
                strError = "该序列号已经上架！";
                return false;
            }

            return true;
        }

    }
}
