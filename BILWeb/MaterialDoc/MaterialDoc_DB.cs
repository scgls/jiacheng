//************************************************************
//******************************作者：方颖*********************
//******************************时间：2017/3/29 13:51:01*******

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
using System.Data; 

namespace BILWeb.MaterialDoc
{
    public partial class T_Material_Doc_DB : BILBasic.Basing.Factory.Base_DB<T_MaterialDoc_Info>
    {
        /// <summary>
        /// 添加t_material_doc
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_MaterialDoc_Info t_material_doc)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();
        }

        protected override List<string> GetSaveSql(BILBasic.User.UserModel user, ref T_MaterialDoc_Info model)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_MaterialDoc_Info ToModel(IDataReader reader)
        {
            T_MaterialDoc_Info t_material_doc = new T_MaterialDoc_Info();

            t_material_doc.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_material_doc.MaterialDoc = (string)dbFactory.ToModelValue(reader, "MATERIALDOC");
            t_material_doc.DocDate = (DateTime?)dbFactory.ToModelValue(reader, "DOCDATE");
            t_material_doc.PostDate = (DateTime?)dbFactory.ToModelValue(reader, "POSTDATE");
            t_material_doc.InOutStockID = dbFactory.ToModelValue(reader, "INOUTSTOCKID").ToInt32();
            t_material_doc.TaskID = dbFactory.ToModelValue(reader, "TASKID").ToInt32();
            t_material_doc.MaterialDocType = dbFactory.ToModelValue(reader, "MATERIALDOCTYPE").ToInt32();
            t_material_doc.TaskType = dbFactory.ToModelValue(reader, "TASKTYPE").ToInt32();
            t_material_doc.TimeStampPost = (string)dbFactory.ToModelValue(reader, "TIMESTAMPPOST");
            t_material_doc.IsDel = dbFactory.ToModelValue(reader, "ISDEL").ToInt32();
            t_material_doc.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_material_doc.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_material_doc.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            t_material_doc.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");
            return t_material_doc;
        }

        protected override string GetViewName()
        {
            return "";
        }

        protected override string GetTableName()
        {
            return "T_MATERIAL_DOC";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }


    }
}
