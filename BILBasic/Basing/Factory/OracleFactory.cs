using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILBasic.Basing
{
    class OracleFactory : IDBFactory
    {
        public string ConnStr { get; set; }

        public IDbCommand CreateCommand()
        {
            return new OracleCommand();
        }

        public IDbConnection CreateConnection()
        {
            return new OracleConnection(ConnStr);
        }

        public IDbConnection CreateConnection(string strConn)
        {
            return new OracleConnection(strConn);
        }

        public IDbDataAdapter CreateDataAdapter()
        {
            return new OracleDataAdapter();
        }

        public IDataReader CreateDataReader(IDbCommand roacleComm)
        {
            return roacleComm.ExecuteReader();
        }

        public IDbTransaction CreateTransaction(IDbConnection oracleConn)
        {
            return oracleConn.BeginTransaction();
        }

        private IDbDataParameter[] _idbParameters;

        public IDBFactory dBFactory()
        {
            IDBFactory dBFactory = new OracleFactory();
            dBFactory.ConnStr = "";
            return dBFactory;
        }
        public IDbDataParameter[] dbParameter(int paramsCount)
        {
            IDbDataParameter[] idbParams = new IDbDataParameter[paramsCount];
            for (int i = 0; i < paramsCount; i++)
            {
                idbParams[i] = new OracleParameter();
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
