//************************************************************
//******************************作者：方颖*********************
//******************************时间：2019/8/15 20:33:41*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using BILBasic.User;
using System.Data;

namespace BILWeb.Move
{
    public partial class T_Move_DB : BILBasic.Basing.Factory.Base_DB<T_MoveInfo>
    {

        /// <summary>
        /// 添加t_move
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_MoveInfo t_move)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_MoveInfo t_move)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_MoveInfo ToModel(IDataReader reader)
        {
            T_MoveInfo t_move = new T_MoveInfo();

            t_move.ID = (int)dbFactory.ToModelValue(reader, "ID");
            t_move.VoucherType = (int)dbFactory.ToModelValue(reader, "VOUCHERTYPE");
            t_move.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_move.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_move.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_move.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_move.IsDel = (int)dbFactory.ToModelValue(reader, "ISDEL");
            t_move.Status = (int)dbFactory.ToModelValue(reader, "STATUS");
            t_move.Voucherno = (string)dbFactory.ToModelValue(reader, "VOUCHERNO");
            t_move.Note = (string)dbFactory.ToModelValue(reader, "NOTE");
            t_move.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "STRONGHOLDCODE");
            t_move.StrongHoldName = (string)dbFactory.ToModelValue(reader, "STRONGHOLDNAME");
            t_move.CompanyCode = (string)dbFactory.ToModelValue(reader, "COMPANYCODE");
            return t_move;
        }

        protected override string GetViewName()
        {
            return "V_v_move";
        }

        protected override string GetTableName()
        {
            return "T_MOVE";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }


    }
}
