using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILBasic.Basing
{

    /// <summary>
    /// sqlserver
    /// </summary>
   public class SqlServerFactory : IDBFactory
    {
        public string ConnStr { get; set; }

        public IDbConnection CreateConnection()
        {

            return new SqlConnection(ConnStr);
        }

        public IDbConnection CreateConnection(string strConn)
        {
            return new SqlConnection(strConn);
        }

        public IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        public IDbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }

        public IDbTransaction CreateTransaction(IDbConnection sqlConn)
        {
            return sqlConn.BeginTransaction();
        }

        public IDataReader CreateDataReader(IDbCommand sqlComm)
        {
            return sqlComm.ExecuteReader();
        }

        private IDbDataParameter[] _idbParameters;
        public IDBFactory dBFactory()
        {
            IDBFactory dBFactory = new SqlServerFactory();
            dBFactory.ConnStr = "";
            return dBFactory;
        }

        public IDbDataParameter[] dbParameter(int paramsCount)
        {
            IDbDataParameter[] idbParams = new IDbDataParameter[paramsCount];
            for (int i = 0; i < paramsCount; i++)
            {
                idbParams[i] = new SqlParameter();
            }
            return idbParams;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="index"></param>
        /// <param name="paramName"></param>
        /// <param name="objValue"></param>
        public void AddParameters(int index, string paramName, object objValue, int size)
        {
            if (index < Parameters.Length)
            {
                Parameters[index].ParameterName = paramName;
                Parameters[index].Value = objValue;
                if (size != 0)
                {
                    Parameters[index].Size = size;
                }
            }
        }

        public IDbDataParameter[] Parameters
        {
            get { return _idbParameters; }
            set { _idbParameters = value; }
        }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="paramsCount"></param>
        public void CreateParameters(int paramsCount)
        {
            Parameters = new IDbDataParameter[paramsCount];
            Parameters = dbParameter(paramsCount);
        }

    }
}
