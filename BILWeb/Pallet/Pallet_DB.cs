using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;
using BILBasic.User;
using System.Data;

namespace BILWeb.Pallet
{
    public partial class T_Pallet_DB : BILBasic.Basing.Factory.Base_DB<T_PalletInfo>
    {

        /// <summary>
        /// 添加t_pallet
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_PalletInfo t_pallet)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_PalletInfo t_pallet)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_PalletInfo ToModel(IDataReader reader)
        {
            T_PalletInfo t_pallet = new T_PalletInfo();

            t_pallet.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_pallet.PalletNo = (string)dbFactory.ToModelValue(reader, "PALLETNO");
            t_pallet.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_pallet.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_pallet.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_pallet.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            return t_pallet;
        }

        protected override string GetViewName()
        {
            return "";
        }

        protected override string GetTableName()
        {
            return "T_PALLET";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }


    }
}
