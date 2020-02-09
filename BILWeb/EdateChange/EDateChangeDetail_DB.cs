//************************************************************
//******************************作者：方颖*********************
//******************************时间：2017/9/7 13:50:30*******

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess;
using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using BILBasic.User;
using BILBasic.Common;
using System.Data;

namespace BILWeb.EdateChange
{
    public partial class T_EDateChangeDetail_DB : BILBasic.Basing.Factory.Base_DB<T_EDateChangeDetailInfo>
    {

        /// <summary>
        /// 添加t_edatechangedetail
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_EDateChangeDetailInfo t_edatechangedetail)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_EDateChangeDetailInfo model)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            if (model.ID <= 0)
            {
                int detailID = base.GetTableID("seq_edatechangedetailid");
                model.ID = detailID;
                strSql = string.Format("insert into t_Edatechangedetail(Id, Headerid, Materialno, Materialnoid, Materialdesc, Batchno, Aftedate, Befedate, Resonecode,  Note, Linestatus, Creater, Createtime,  Isdel, Voucherno,Strongholdcode,Strongholdname,Companycode)" +
                                       " values ('{11}','{0}','{1}','{2}','{3}','{4}',to_date('{5}','YYYY/MM/DD hh24:mi:ss'),to_date('{6}','YYYY/MM/DD hh24:mi:ss'),'{7}','{8}','1','{9}',Sysdate,'1','{10}','{12}','{13}','10')",
                                       model.HeaderID, model.MaterialNo, model.MaterialNoID, model.MaterialDesc, model.BatchNo, model.AftEDate.ToDateTime().Date, model.EDate, model.ResoneCode, model.Note, user.UserNo, model.VoucherNo, detailID,model.StrongHoldCode,model.StrongHoldName);

                lstSql.Add(strSql);
            }
            else
            {
                strSql = "update t_Edatechangedetail a set a.Materialnoid = '" + model.MaterialNoID + "', a.Materialno = '" + model.MaterialNo + "',a.Materialdesc = '" + model.MaterialDesc + "',a.Aftedate = to_date('" + model.AftEDate.ToDateTime().Date + "','YYYY/MM/DD hh24:mi:ss'),a.batchno = '" + model.BatchNo + "', " +
                        " a.Befedate =  to_date('" + model.EDate.ToDateTime().Date + "','YYYY/MM/DD hh24:mi:ss') , a.Note = '" + model.Note + "',a.Resonecode = '" + model.ResoneCode + "',a.Modifyer = '" + user.UserNo + "',a.Modifytime = Sysdate,a.Strongholdcode = '" + model.StrongHoldCode + "',a.StrongHoldName = '" + model.StrongHoldName + "',Companycode = 10 where id = '" + model.ID + "'";
                lstSql.Add(strSql);
            }
            return lstSql;
        }



        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_EDateChangeDetailInfo ToModel(IDataReader reader)
        {
            T_EDateChangeDetailInfo t_edatechangedetail = new T_EDateChangeDetailInfo();

            t_edatechangedetail.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_edatechangedetail.HeaderID = dbFactory.ToModelValue(reader, "HEADERID").ToInt32();
            t_edatechangedetail.RowNo = (string)dbFactory.ToModelValue(reader, "ROWNO");
            t_edatechangedetail.MaterialNo = (string)dbFactory.ToModelValue(reader, "MATERIALNO");
            t_edatechangedetail.MaterialNoID = dbFactory.ToModelValue(reader, "MATERIALNOID").ToInt32();
            t_edatechangedetail.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MATERIALDESC");
            t_edatechangedetail.BatchNo = (string)dbFactory.ToModelValue(reader, "BATCHNO");
            t_edatechangedetail.AftEDate = (DateTime?)dbFactory.ToModelValue(reader, "AFTEDATE");
            t_edatechangedetail.BefEDate = (DateTime)dbFactory.ToModelValue(reader, "BEFEDATE");
            t_edatechangedetail.ResoneCode = dbFactory.ToModelValue(reader, "RESONECODE").ToInt32();
            t_edatechangedetail.ResoneName = (string)dbFactory.ToModelValue(reader, "RESONENAME");
            t_edatechangedetail.Note = (string)dbFactory.ToModelValue(reader, "NOTE");
            t_edatechangedetail.LineStatus = dbFactory.ToModelValue(reader, "LINESTATUS").ToInt32();
            t_edatechangedetail.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_edatechangedetail.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_edatechangedetail.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_edatechangedetail.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            t_edatechangedetail.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ERPVOUCHERNO");
            t_edatechangedetail.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToDecimal();
            t_edatechangedetail.EDate = t_edatechangedetail.BefEDate;
            t_edatechangedetail.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "StrongHoldCode");
            t_edatechangedetail.StrongHoldName = (string)dbFactory.ToModelValue(reader, "StrongHoldName");

            return t_edatechangedetail;
        }

        protected override string GetViewName()
        {
            return "V_EDATECHANGEDETAIL";
        }

        protected override string GetTableName()
        {
            return "T_EDATECHANGEDETAIL";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override List<string> GetDeleteSql(UserModel user, T_EDateChangeDetailInfo model)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            strSql = "delete t_Edatechangedetail where id = '" + model.ID + "'";

            lstSql.Add(strSql);

            return lstSql;
        }

        public bool UpdateStockEdate(List<T_EDateChangeDetailInfo> modelList,ref string strError) 
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;


            foreach (var item in modelList) 
            {
                strSql = "update t_stock a set a.Edate = to_date('" + item.AftEDate.ToDateTime().Date + "','YYYY/MM/DD hh24:mi:ss') where a.Materialnoid = '"+item.MaterialNoID+"' and a.Batchno = '"+item.BatchNo+"'";

                lstSql.Add(strSql);

                strSql = "update t_Outbarcode a set a.Edate = to_date('" + item.AftEDate.ToDateTime().Date + "','YYYY/MM/DD hh24:mi:ss') where a.Materialnoid = '" + item.MaterialNoID + "' and a.Batchno = '" + item.BatchNo + "'";

                lstSql.Add(strSql);

                strSql = "update t_Edatechangedetail a set a.Erpvoucherno = '"+item.ErpVoucherNo+"' where id = '"+item.ID+"'";
                lstSql.Add(strSql);
            }

            strSql = "update t_Edatechange a set a.Erpvoucherno = '" + modelList[0].ErpVoucherNo + "' ,a.Status = 2 where id = '" + modelList[0].HeaderID + "'";
            lstSql.Add(strSql);

            return base.UpdateModelListStatusBySql(lstSql, ref strError);
            
        }

        public int CheckBatchIsExists(T_EDateChangeDetailInfo model) 
        {
            string strSql = "select count(1) from t_Edatechangedetail a where a.Materialnoid = '"+model.MaterialNoID+"' and a.Batchno ='"+model.BatchNo+"' and a.Headerid = '"+model.HeaderID+"' ";

            return base.GetScalarBySql(strSql).ToInt32();
        }
    }
}
