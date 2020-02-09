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
using System.Data;

namespace BILWeb.TransportSupplier
{
    public partial class T_SaveTransportSupplier_DB : Base_DB<T_TransportSupplier>
    {
        protected override IDataParameter[] GetSaveModelIDataParameter(T_TransportSupplier model)
        {
            throw new NotImplementedException();
        }

        protected override string GetSaveProcedureName()
        {
            throw new NotImplementedException();
        }

        protected override List<string> GetSaveSql(UserModel user, ref T_TransportSupplier model)
        {
            throw new NotImplementedException();
        }

        protected override string GetTableName()
        {
            throw new NotImplementedException();
        }

        protected override string GetViewName()
        {
            throw new NotImplementedException();
        }

        protected override T_TransportSupplier ToModel(IDataReader reader)
        {
            throw new NotImplementedException();
        }

        
    }
}
