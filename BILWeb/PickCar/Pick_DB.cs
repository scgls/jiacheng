//************************************************************
//******************************作者：方颖*********************
//******************************时间：2019/8/30 23:51:34*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using BILBasic.User;
using BILBasic.Common;
using System.Data;

namespace BILWeb.PickCar
{
    public partial class T_PickCar_DB : BILBasic.Basing.Factory.Base_DB<T_PickCarInfo>
    {

        /// <summary>
        /// 添加t_pickcar
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_PickCarInfo t_pickcar)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_PickCarInfo model)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            //更新
            if (model.ID > 0)
            {
                strSql = "update t_Pickcar a set a.Carno = '"+model.CarNo+"' where id = '"+model.CarNo+"'";
                lstSql.Add(strSql);
            }
            else //插入
            {
                int voucherID = base.GetTableID("SEQ_PICKCAR");

                model.ID = voucherID.ToInt32();

                strSql = "insert into t_Pickcar(id, Carno, Creater, Createtime)" +
                        " values ('" + voucherID + "','"+model.CarNo+"','"+user.UserNo+"',sysdate)";
                lstSql.Add(strSql);
            }

            return lstSql;
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_PickCarInfo ToModel(IDataReader reader)
        {
            T_PickCarInfo t_pickcar = new T_PickCarInfo();

            t_pickcar.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_pickcar.CarNo = (string)dbFactory.ToModelValue(reader, "CARNO");
            t_pickcar.TaskNo = (string)dbFactory.ToModelValue(reader, "TASKNO");
            t_pickcar.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_pickcar.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_pickcar.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_pickcar.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_pickcar.IsDel = dbFactory.ToModelValue(reader, "IsDel").ToInt32();
            return t_pickcar;
        }

        protected override string GetViewName()
        {
            return "v_pickcar";
        }

        protected override string GetTableName()
        {
            return "T_PICKCAR";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        public int CheckCarIsExists(string strCarNo) 
        {
            string strSql = "select count(1) from t_Pickcar where carno = '" + strCarNo + "'";
            return base.GetScalarBySql(strSql).ToInt32();
        }

        public int UpdateCar(string id)
        {
            string strSql = " update t_Pickcar set taskno=''  where ID = '" + id + "'";
            return base.GetExecuteNonQuery(strSql);
        }


        protected override string GetFilterSql(UserModel user, T_PickCarInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";

            if (model.CarNo != null)
            {
                strSql += strAnd;
                strSql += " CarNo = '" + model.CarNo+"'";
            }

            if (model.TaskNo != null)
            {
                strSql += strAnd;
                strSql += " TaskNo = '" + model.TaskNo + "'";
            }
            return strSql + " order by id desc ";
        }


    }
}
