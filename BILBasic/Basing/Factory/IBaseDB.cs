using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILBasic.Basing
{
   public interface  IBaseDB
    {
        IDbDataParameter[] Parameters
        {
            get;
            set;
        }
        
        void CreateParameters(int paramsCount);

        void AddParameters(int index, string paramName, object objValue, int size);


        IDataReader dataReader();

        IDBFactory dBFactory();
        //IDataReader CreateDataReader(IDbCommand sqlComm);
       
    }
}
