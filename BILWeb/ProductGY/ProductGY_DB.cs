
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.User;
using BILBasic.DBA;
using BILBasic.Common;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BILWeb.ProductGY
{
    public partial class Mes_ProductGY_DB : BILBasic.Basing.Factory.Base_DB<Mes_ProductGYInfo>
    {

        /// <summary>
        /// 添加t_outstock
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(Mes_ProductGYInfo t_outstock)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref Mes_ProductGYInfo model)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            //更新
            if (model.ID > 0)
            {
                strSql = string.Format("update Mes_GY a set a.cptype = '{0}',a.machineType='{1}',a.gxSeq='{2}',a.Modifyer = '{3}',a.Modifytime = Sysdate,a.description = '{5}',a.imagePic = '{6}',a.videoUrl = '{7}',a.remark = '{8}' where a.Id = '{4}'",
                    model.cptype, model.machineType, model.gxSeq, user.UserNo, model.ID, model.description, model.imagePic, model.videoUrl, model.remark);
                lstSql.Add(strSql);
            }
            else //插入
            {
                int voucherID = base.GetTableID("SEQ_MES_GY");

                model.ID = voucherID.ToInt32();

                string VoucherNoID = base.GetTableID("seq_outstock_no").ToString();

                //string VoucherNo ="F"+ System.DateTime.Now.ToString("yyyyMMdd") + VoucherNoID.PadLeft(4, '0');
                string gyid = Guid.NewGuid().ToString();

                strSql = string.Format("insert into Mes_GY (Id,  gxID, cptype, machineType, gxSeq,  gxName, isDel, description,imagePic,videoUrl,creater,createtime,remark)" +
                    " values('{0}','{1}','{2}','{3}','{4}','{5}',1,'{6}','{7}','{8}','{9}',Sysdate,'{10}')",
                    voucherID, gyid, model.cptype, model.machineType, model.gxSeq, model.gxName, model.description, model.imagePic, model.videoUrl, user.UserNo, model.remark);

                lstSql.Add(strSql);
            }

            return lstSql;
        }

        protected override List<string> GetUpdateSql(UserModel user, Mes_ProductGYInfo model)
        {
            List<string> lstSql = new List<string>();

            string strSql = "update Mes_GY a set a.isDel = '" + model.isDel + "' where id = '" + model.ID + "'";

            lstSql.Add(strSql);

            return lstSql;
        }


        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override Mes_ProductGYInfo ToModel(IDataReader reader)
        {
            Mes_ProductGYInfo mes_productGY = new Mes_ProductGYInfo();

            mes_productGY.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            mes_productGY.gxID = (string)dbFactory.ToModelValue(reader, "gxID");
            mes_productGY.cptype = (string)dbFactory.ToModelValue(reader, "cptype");
            mes_productGY.machineType = (string)dbFactory.ToModelValue(reader, "machineType");
            mes_productGY.gxSeq = dbFactory.ToModelValue(reader, "gxSeq").ToInt32();
            mes_productGY.gxName = (string)dbFactory.ToModelValue(reader, "gxName");
            mes_productGY.isDel = dbFactory.ToModelValue(reader, "isDel").ToInt32();
            mes_productGY.description = (string)dbFactory.ToModelValue(reader, "description");
            mes_productGY.imagePic = (byte[])dbFactory.ToModelValue(reader, "imagePic");
            mes_productGY.videoUrl = (string)dbFactory.ToModelValue(reader, "videoUrl");

            mes_productGY.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            mes_productGY.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            mes_productGY.Modifyer = (string)dbFactory.ToModelValue(reader, "Modifyer");
            mes_productGY.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "ModifyTime");

            mes_productGY.remark = (string)dbFactory.ToModelValue(reader, "remark");

            //todo:
            //mes_productGY.StrStatus = (string)dbFactory.ToModelValue(reader, "StrStatus");
            //mes_productGY.StrCreater = (string)dbFactory.ToModelValue(reader, "StrCreater");

            return mes_productGY;
        }

        protected override string GetViewName()
        {
            return "V_PRODUCT_GY";
        }

        protected override string GetTableName()
        {
            return "V_PRODUCT_GY";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }


        protected override string GetFilterSql(UserModel user, Mes_ProductGYInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";


            if (model.isDel > 0)
            {
                strSql += strAnd;
                strSql += " isDel= 1";
            }

            if (!string.IsNullOrEmpty(model.gxName))
            {
                strSql += strAnd;
                strSql += " gxName Like '" + model.gxName + "%' ";
            }

            if (!string.IsNullOrEmpty(model.cptype))
            {
                strSql += strAnd;
                strSql += " cptype Like '" + model.cptype + "%' ";
            }

            if (!string.IsNullOrEmpty(model.machineType))
            {
                strSql += strAnd;
                strSql += " machineType Like '" + model.machineType + "%' ";
            }

            if (model.DateFrom.HasValue)
            {
                strSql += strAnd;
                strSql += " CreateTime >= " + model.DateFrom.Value.Date.ToOracleTimeString() + " ";
            }

            if (model.DateTo.HasValue)
            {
                strSql += strAnd;
                strSql += " CreateTime <= " + model.DateTo.Value.Date.AddDays(1).ToOracleTimeString() + " ";
            }

            if (!string.IsNullOrEmpty(model.Creater))
            {
                strSql += strAnd;
                strSql += " Creater Like '" + model.Creater + "%'  ";
            }

            return strSql + " order by id desc ";
        }

        public bool GetProductGYDataByID(ref Mes_ProductGYInfo model, ref string ErrMsg)
        {
            string strSql = string.Empty;

            strSql = "select t.imagePic,t.* from Mes_GY t where t.gxID='" + model.gxID + "'";

            model = new Mes_ProductGYInfo();

            try
            {
                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    if (dr.Read())
                    {
                        model.ID = dr["ID"].ToInt32();
                        model.gxID = dr["gxID"].ToDBString();
                        model.cptype = dr["cptype"].ToDBString();
                        model.machineType = dr["machineType"].ToDBString();
                        model.gxSeq = dr["gxSeq"].ToInt32();
                        model.gxName = dr["gxName"].ToDBString();

                        model.isDel = dr["isDel"].ToInt32();
                        model.description = dr["description"].ToDBString();

                        var blob = dr.GetOracleBlob(0);
                        byte[] bout = new byte[blob.Length];
                        blob.Read(bout, 0, bout.Length);

                        model.imagePic = bout;

                        model.videoUrl = dr["videoUrl"].ToDBString();
                        model.remark = dr["remark"].ToDBString();

                        model.Creater = dr["Creater"].ToDBString();
                        model.CreateTime = dr["CreateTime"].ToDateTime();
                        model.Modifyer = dr["Modifyer"].ToDBString();
                        model.ModifyTime = dr["ModifyTime"].ToDateTimeNull();

                    }
                    else
                    {
                        ErrMsg = "no data!";
                        return false;
                    }
                }

                //strSql = "select t.* from Mes_GY_Limit t where t.gxID='" + model.gxID + "'";

                //model.lstLimit = new List<Mes_ProductGY_LimitInfo>();

                //using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                //{
                //    while (dr.Read())
                //    {
                //        model.lstLimit.Add(new Mes_ProductGY_LimitInfo() 
                //        {
                //            xzID = dr["xzID"].ToDBString(),
                //            gxID = dr["gxID"].ToDBString(),
                //            type = dr["type"].ToDBString(),
                //            xzName = dr["xzName"].ToDBString(),
                //            xzRoleMax = dr["xzRoleMax"].ToDBString(),
                //            xzRoleMin = dr["xzRoleMin"].ToDBString(),
                //            QTY = dr["QTY"].ToDBString(),
                //            Unit = dr["Unit"].ToDBString()
                //        });
                //    }
                //}

                return true;
            }
            catch(Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public bool SaveProductGYData(string userNo, List<Mes_ProductGYInfo> lst, ref string ErrMsg)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            try
            {
                List<byte[]> buttfes = new List<byte[]>();

                foreach (var model in lst)
                {
                    //更新---------------todo:-------------?????
                    if (model.ID > 0)
                    {
                        strSql = string.Format("update Mes_GY a set a.cptype = '{0}',a.machineType='{1}',a.gxSeq='{2}',a.Modifyer = '{3}',a.Modifytime = Sysdate,a.description = '{5}',a.imagePic = '{6}',a.videoUrl = '{7}',a.remark = '{8}' where a.Id = '{4}'",
                            model.cptype, model.machineType, model.gxSeq, userNo, model.ID, model.description, model.imagePic, model.videoUrl, model.remark);
                        lstSql.Add(strSql);
                    }
                    else //插入
                    {
                        //插入Mes_GY
                        int voucherID = base.GetTableID("SEQ_MES_GY");

                        model.ID = voucherID.ToInt32();

                        //string VoucherNoID = base.GetTableID("seq_outstock_no").ToString();

                        //string VoucherNo ="F"+ System.DateTime.Now.ToString("yyyyMMdd") + VoucherNoID.PadLeft(4, '0');
                        string gyid = Guid.NewGuid().ToString();
                        //string gxDuanCode = gyid + voucherID.ToString();

                        //strSql = string.Format("insert into Mes_GY (Id,  gxID, cptype, machineType, gxSeq,  gxName, isDel, description,imagePic,videoUrl,creater,createtime,remark,bbNo,gxDuanCode,gxDuanName)" +
                        //    " values('{0}','{1}','{2}','{3}','{4}','{5}',1,'{6}','{7}','{8}','{9}',Sysdate,'{10}','{11}','{12}','{13}')",
                        //    voucherID, gyid, model.cptype, model.machineType, model.gxSeq, model.gxName, model.description, model.imagePic, model.videoUrl, userNo, model.remark, model.bbNo, gxDuanCode, model.gxDuanName);

                        strSql = string.Format("insert into Mes_GY (Id,  gxID, cptype, machineType, gxSeq,  gxName, isDel, description,imagePic,videoUrl,creater,createtime,remark,bbNo,gxDuanCode,gxDuanName)" +
                            " values('{0}','{1}','{2}','{3}','{4}','{5}',1,'{6}',:Images,'{7}','{8}',Sysdate,'{9}','{10}','{11}','{12}')",
                            voucherID, model.gxID, model.cptype, model.machineType, model.gxSeq, model.gxName, model.description, model.videoUrl, userNo, model.remark, model.bbNo, model.gxDuanCode, model.gxDuanName);

                        lstSql.Add(strSql);

                        buttfes.Add(model.imagePic);

                    }
                }

                if (lstSql == null || lstSql.Count == 0)
                {
                    ErrMsg = "no data!!!";
                    return false;
                }

                int i = dbFactory.ExecuteNonQueryListByCymNew(lstSql, buttfes, ref ErrMsg);
                //int i = dbFactory.ExecuteNonQueryList(lstSql, ref strError);

                if (i > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch(Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }


        #region GYLine
        public bool GetProductGYlistBygxDuanCode(string gxDuanCode, ref List<Mes_ProductGYInfo> lst, ref string machineName, ref string machineType, ref string ErrMsg)
        {
            lst = new List<Mes_ProductGYInfo>();
            try
            {
                ErrMsg = "";

                string strSql = "select MachineName,MachineType from Mes_GYD where gxDuanCode ='" + gxDuanCode + "' and isdel !=2 ";

                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    if (dr.Read())
                    {
                        machineName = dr["machineName"].ToDBString();
                        machineType = dr["machineType"].ToDBString();
                    }
                    else
                    {
                        machineName = machineType = "";
                    }
                }

                strSql = "select id,gxID,gxName,description,imagePic,imagePic2,imagePic3,videoUrl,gxSeq from Mes_GY where gxDuanCode ='" + gxDuanCode + "' and isdel !=2 and rownum < 100 ";

                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        Mes_ProductGYInfo model = new Mes_ProductGYInfo();

                        model.ID = dr["ID"].ToInt32();
                        model.gxID = dr["gxID"].ToDBString();
                        model.gxName = dr["gxName"].ToDBString();
                        model.description = dr["description"].ToDBString();
                        model.videoUrl = dr["videoUrl"].ToDBString();
                        model.gxSeq = dr["gxSeq"].ToInt32();
                        if (!string.IsNullOrWhiteSpace(dr["imagePic"].ToString()))
                            model.imagePic = (byte[])dr["imagePic"];
                        else
                            model.imagePic = null;
                        if (!string.IsNullOrWhiteSpace(dr["imagePic2"].ToString()))
                            model.imagePic2 = (byte[])dr["imagePic2"];
                        else
                            model.imagePic2 = null;
                        if (!string.IsNullOrWhiteSpace(dr["imagePic3"].ToString()))
                            model.imagePic3 = (byte[])dr["imagePic3"];
                        else
                            model.imagePic3 = null;

                        model.lstLimit = new List<Mes_ProductGY_LimitInfo>();

                        lst.Add(model);
                    }
                }

                //Mes_GY_Limit
                strSql = "select * from Mes_GY_Limit t where t.gxID in (select gxID from Mes_GY where gxDuanCode='" + gxDuanCode + "')";

                List<Mes_ProductGY_LimitInfo> lstLimit = new List<Mes_ProductGY_LimitInfo>();

                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        Mes_ProductGY_LimitInfo model = new Mes_ProductGY_LimitInfo();

                        model.ID = dr["ID"].ToInt32();
                        model.xzID = dr["xzID"].ToDBString();
                        model.gyDetailID = dr["gxID"].ToDBString();
                        //model.gyLineID = dr["gyLineID"].ToDBString();
                        model.type = dr["type"].ToDBString();
                        model.seqno = dr["seqno"].ToInt32();

                        model.xzName = dr["xzName"].ToDBString();
                        model.xzRoleMax = dr["xzRoleMax"].ToDBString();
                        model.xzRoleMin = dr["xzRoleMin"].ToDBString();
                        model.QTY = dr["QTY"].ToDBString();
                        model.Unit = dr["Unit"].ToDBString();

                        lstLimit.Add(model);
                    }
                }

                if (lstLimit.Count > 0)
                {
                    lst.ForEach(delegate(Mes_ProductGYInfo entityGy)
                    {
                        var templst = lstLimit.FindAll(t => t.gyDetailID == entityGy.gxID);

                        if (templst != null && templst.Count > 0)
                        {
                            entityGy.lstLimit = templst;
                        }
                    });
                }

            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }

            return true;
        }

        public bool GetMaterialInfoByNoForGYLine(string materialno, ref string materialname, ref string ErrMsg)
        {
            try
            {
                ErrMsg = "";

                string strSql = "select * from T_MATERIAL where materialno ='" + materialno + "' and isdel !=2 ";

                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    if (dr.Read())
                    {
                        materialname = dr["materialdesc"].ToDBString();
                        return true;
                    }
                    else
                    {
                        ErrMsg = "no data!";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public bool GetAllProductGYLineList(Mes_ProductGYLineInfo model, ref List<Mes_ProductGYLineInfo> lst, ref string ErrMsg)
        {
            string strSql = string.Empty;

            //strSql = "select * from Mes_GYLine where 1=1 and isdel=1 and rownum < 100 ";
            strSql = "select * from Mes_GYLine where 1=1 and isdel=1 ";
            string strAnd = " and ";

            #region where

            strSql += strAnd;
            strSql += " isMoban = " + model.isMoban + " ";

            if (!string.IsNullOrEmpty(model.gyLineID))
            {
                strSql += strAnd;
                strSql += " gyLineID Like '" + model.gyLineID + "%' ";
            }

            if (!string.IsNullOrEmpty(model.cpCode))
            {
                strSql += strAnd;
                strSql += " cpCode Like '" + model.cpCode + "%' ";
            }

            if (!string.IsNullOrEmpty(model.cpName))
            {
                strSql += strAnd;
                strSql += " cpName Like '" + model.cpName + "%' ";
            }

            if (model.DateFrom.HasValue)
            {
                strSql += strAnd;
                strSql += " CreateTime >= " + model.DateFrom.Value.Date.ToOracleTimeString() + " ";
            }

            if (model.DateTo.HasValue)
            {
                strSql += strAnd;
                strSql += " CreateTime <= " + model.DateTo.Value.Date.AddDays(1).ToOracleTimeString() + " ";
            }

            if (!string.IsNullOrEmpty(model.Creater))
            {
                strSql += strAnd;
                strSql += " Creater Like '" + model.Creater + "%'  ";
            }

            //strSql += " order by id desc ";
            strSql += " order by gyLineID ";
            #endregion

            try
            {
                lst = new List<Mes_ProductGYLineInfo>();
                //GYD
                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        Mes_ProductGYLineInfo entityGYLine = new Mes_ProductGYLineInfo();

                        entityGYLine.ID = dr["ID"].ToInt32();
                        entityGYLine.gyLineID = dr["gyLineID"].ToDBString();
                        entityGYLine.machineType = dr["machineType"].ToDBString();
                        entityGYLine.bbID = dr["bbID"].ToDBString();
                        entityGYLine.gybbID = dr["gybbID"].ToDBString();
                        entityGYLine.cpCode = dr["cpCode"].ToDBString();
                        entityGYLine.cpName = dr["cpName"].ToDBString();
                        entityGYLine.gyLineName = dr["gyLineName"].ToDBString();
                        entityGYLine.isNew = dr["isNew"].ToInt32();
                        entityGYLine.capacity = dr["capacity"].ToDecimal();
                        entityGYLine.cptype = dr["cptype"].ToDBString();
                        entityGYLine.Qty = dr["Qty"].ToInt32();
                        entityGYLine.isMoban = dr["isMoban"].ToInt32();
                        entityGYLine.remark = dr["remark"].ToDBString();

                        entityGYLine.Creater = dr["Creater"].ToDBString();
                        entityGYLine.CreateTime = dr["CreateTime"].ToDateTime();

                        entityGYLine.isDel = dr["isDel"].ToInt32();

                        lst.Add(entityGYLine);
                    }
                }

                if (lst.Count <= 0)
                {
                    ErrMsg = "no data!";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        #endregion
    }
}
