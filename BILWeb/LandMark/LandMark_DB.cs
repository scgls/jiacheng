//************************************************************
//******************************作者：方颖*********************
//******************************时间：2019/9/5 15:56:29*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using BILBasic.Common;
using BILBasic.User;
using System.Data;

namespace BILWeb.LandMark
{
    public partial class T_LandMark_DB : BILBasic.Basing.Factory.Base_DB<T_LandMarkInfo>
    {

        /// <summary>
        /// 添加t_landmark
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_LandMarkInfo t_landmark)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_LandMarkInfo t_landmark)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_LandMarkInfo ToModel(IDataReader reader)
        {
            T_LandMarkInfo t_landmark = new T_LandMarkInfo();

            t_landmark.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_landmark.LandMarkNo = (string)dbFactory.ToModelValue(reader, "LANDMARKNO");
            t_landmark.Remark = (string)dbFactory.ToModelValue(reader, "REMARK");
            t_landmark.Remark2 = (string)dbFactory.ToModelValue(reader, "REMARK2");
            t_landmark.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_landmark.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_landmark.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToDecimal();
            return t_landmark;
        }

        protected override string GetViewName()
        {
            return "V_LANDMARK";
        }

        protected override string GetTableName()
        {
            return "T_LANDMARK";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override string GetFilterSql(UserModel user, T_LandMarkInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";
            if (!string.IsNullOrEmpty(model.LandMarkNo))
            {
                strSql += strAnd;
                strSql += " LandMarkNo = '" + model.LandMarkNo.Trim() + "' ";
            }

            return strSql + " order by id desc";
        }


        public T_LandMarkInfo Getlandmark(string landmarkno)
        {
            string strFilter1 = "landmarkno = '" + landmarkno + "'";
            return base.GetModelByFilter(strFilter1);
        }

    }
}
