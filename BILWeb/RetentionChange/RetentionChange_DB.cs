//************************************************************
//******************************作者：方颖*********************
//******************************时间：2017/10/19 15:40:07*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using BILBasic.Common;
using BILBasic.User;
using System.Data;


namespace BILWeb.RetentionChange
{
    public partial class T_RetentionChange_DB : BILBasic.Basing.Factory.Base_DB<T_RetentionChangeInfo>
    {

        /// <summary>
        /// 添加t_retentionchange
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_RetentionChangeInfo t_retentionchange)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref  T_RetentionChangeInfo model)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            //更新
            if (model.ID > 0)
            {
                strSql = string.Format("update t_Retentionchange a set a.Modifyer = '{0}' ,a.Modifytime = sysdate,a.Note  = '{1}',Retaintype = '{3}' where id = '{2}'",
                    user.UserNo, model.Note, model.ID,model.RetainType);
                lstSql.Add(strSql);
            }
            else //插入
            {
                int voucherID = base.GetTableID("Seq_Retentionchange_Id");

                model.ID = voucherID.ToInt32();

                string VoucherNoID = base.GetTableID("Seq_Retentionchange_NO").ToString();

                string VoucherNo = "R" + System.DateTime.Now.ToString("yyyyMMdd") + VoucherNoID.PadLeft(4, '0');

                strSql = string.Format("insert into t_Retentionchange(Id,  Voucherno,  Createtime, Creater,  Status, Isdel, Note,  Vouchertype,Retaintype) values ('{0}','{1}',Sysdate,'{2}','{3}','{4}','{5}','{6}','{7}')",
                    voucherID, VoucherNo, user.UserNo, model.Status, model.IsDel, model.Note, model.VoucherType,model.RetainType);

                lstSql.Add(strSql);
            }

            return lstSql;
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_RetentionChangeInfo ToModel(IDataReader reader)
        {
            T_RetentionChangeInfo t_retentionchange = new T_RetentionChangeInfo();

            t_retentionchange.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_retentionchange.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ERPVOUCHERNO");
            t_retentionchange.VoucherNo = (string)dbFactory.ToModelValue(reader, "VOUCHERNO");
            t_retentionchange.ResoneCode = (string)dbFactory.ToModelValue(reader, "RESONECODE");
            t_retentionchange.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_retentionchange.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_retentionchange.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_retentionchange.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_retentionchange.Status = dbFactory.ToModelValue(reader, "STATUS").ToInt32();
            t_retentionchange.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToDecimal();
            t_retentionchange.Note = (string)dbFactory.ToModelValue(reader, "NOTE");
            t_retentionchange.ERPStatus = (string)dbFactory.ToModelValue(reader, "ERPSTATUS");
            t_retentionchange.VoucherType = dbFactory.ToModelValue(reader, "VOUCHERTYPE").ToInt32();
            t_retentionchange.RetainType = (string)dbFactory.ToModelValue(reader, "RETAINTYPE");
            t_retentionchange.StrVoucherType = (string)dbFactory.ToModelValue(reader, "StrVoucherType");
            t_retentionchange.StrStatus = (string)dbFactory.ToModelValue(reader, "StrStatus");
            t_retentionchange.StrCreater = (string)dbFactory.ToModelValue(reader, "StrCreater");
            t_retentionchange.StrRetainType = (string)dbFactory.ToModelValue(reader, "StrRetainType");

            return t_retentionchange;
        }

        protected override string GetViewName()
        {
            return "V_RETENTIONCHANGE";
        }

        protected override string GetTableName()
        {
            return "T_RETENTIONCHANGE";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }


        protected override string GetFilterSql(UserModel user, T_RetentionChangeInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";

            if (model.Status > 0)
            {
                strSql += " Status = " + model.Status + " ";
                strSql += strAnd;
            }

            if (model.DateFrom != null)
            {
                strSql += strAnd;
                strSql += " CreateTime >= " + model.DateFrom.ToDateTime().Date.ToOracleTimeString() + " ";
            }

            if (model.DateTo != null)
            {
                strSql += strAnd;
                strSql += " CreateTime <= " + model.DateTo.ToDateTime().Date.AddDays(1).ToOracleTimeString() + " ";
            }

            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " erpvoucherno Like '" + model.ErpVoucherNo + "%'  ";
            }

            return strSql + "order by id desc";
        }

        protected override List<string> GetDeleteSql(UserModel user, T_RetentionChangeInfo model)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            strSql = "delete T_RETENTIONCHANGE where id = '" + model.ID + "'";

            lstSql.Add(strSql);

            return lstSql;
        }

    }
}
