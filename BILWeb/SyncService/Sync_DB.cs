using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BILBasic.User;
using BILBasic.Basing.Factory;
using System.Data;

namespace BILWeb.SyncService
{
    public class Sync_DB : Base_DB<EmptyModel>
    {

        /// <summary>
        /// 根据wms类型获取同步字段
        /// </summary>
        /// <param name="wmsVourcherType"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>


        internal string GetLastSyncTime(string wmsType)
        {

            using (var db = SqlSugarBase.GetInstance())
            {
                db.Ado.GetString();
            }
        }



        //protected override SqlParameter[] GetSaveModelSqlParameter(EmptyModel model)
        //{
        //    throw new NotImplementedException();
        //}

        protected override IDataParameter[] GetSaveModelIDataParameter(EmptyModel model)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override string GetSaveProcedureName()
        {
            throw new NotImplementedException();
        }

        protected override List<string> GetSaveSql(UserModel user, ref EmptyModel model)
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

        //protected override EmptyModel ToModel(SqlDataReader reader)
        //{
        //    EmptyModel emptuModel = new EmptyModel();
        //    return emptuModel;
        //}
        protected override EmptyModel ToModel(IDataReader reader)
        {
            EmptyModel emptuModel = new EmptyModel();
            return emptuModel;
        }

    }
}
