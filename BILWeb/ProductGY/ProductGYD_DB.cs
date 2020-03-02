
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
    public partial class Mes_ProductGYD_DB : BILBasic.Basing.Factory.Base_DB<Mes_ProductGYDInfo>
    {

        /// <summary>
        /// 添加t_outstock
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(Mes_ProductGYDInfo t_outstock)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();

        }

        protected override List<string> GetSaveSql(UserModel user, ref Mes_ProductGYDInfo model)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            return lstSql;
        }

        protected override List<string> GetUpdateSql(UserModel user, Mes_ProductGYDInfo model)
        {
            List<string> lstSql = new List<string>();

            //string strSql = "update Mes_GY a set a.isDel = '" + model.isDel + "' where id = '" + model.ID + "'";

            //lstSql.Add(strSql);

            return lstSql;
        }


        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override Mes_ProductGYDInfo ToModel(IDataReader reader)
        {
            Mes_ProductGYDInfo mes_productGYD = new Mes_ProductGYDInfo();

            mes_productGYD.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            mes_productGYD.gxDuanCode = (string)dbFactory.ToModelValue(reader, "gxDuanCode");
            mes_productGYD.gxDuanName = (string)dbFactory.ToModelValue(reader, "gxDuanName");
            mes_productGYD.bbNo = (string)dbFactory.ToModelValue(reader, "bbNo");
            mes_productGYD.gxSeq = dbFactory.ToModelValue(reader, "gxSeq").ToInt32();
            mes_productGYD.isDel = dbFactory.ToModelValue(reader, "isDel").ToInt32();
            mes_productGYD.MachineName = (string)dbFactory.ToModelValue(reader, "MachineName");
            mes_productGYD.MachineType = (string)dbFactory.ToModelValue(reader, "MachineType");
            mes_productGYD.MouldCode = (string)dbFactory.ToModelValue(reader, "MouldCode");

            mes_productGYD.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            mes_productGYD.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            mes_productGYD.Modifyer = (string)dbFactory.ToModelValue(reader, "Modifyer");
            mes_productGYD.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "ModifyTime");

            mes_productGYD.Qty = dbFactory.ToModelValue(reader, "Qty").ToInt32();
            
            return mes_productGYD;
        }

        protected override string GetViewName()
        {
            return "V_PRODUCT_GYD";
        }

        protected override string GetTableName()
        {
            return "V_PRODUCT_GYD";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override string GetFilterSql(UserModel user, Mes_ProductGYDInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";


            if (model.isDel > 0)
            {
                strSql += strAnd;
                strSql += " isDel= 1";
            }

            if (!string.IsNullOrEmpty(model.gxDuanCode))
            {
                strSql += strAnd;
                strSql += " gxDuanCode Like '" + model.gxDuanCode + "%' ";
            }

            if (!string.IsNullOrEmpty(model.gxDuanName))
            {
                strSql += strAnd;
                strSql += " gxDuanName Like '" + model.gxDuanName + "%' ";
            }

            if (!string.IsNullOrEmpty(model.bbNo))
            {
                strSql += strAnd;
                strSql += " bbNo Like '" + model.bbNo + "%' ";
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



        public bool GetProductGYDAllListData(Mes_ProductGYDInfo model, ref List<Mes_ProductGYDInfo> lst, ref string ErrMsg)
        {
            string strSql = string.Empty;

            //strSql = "select * from Mes_GYD where 1=1 and isDel = 1 and rownum < 100 ";
            strSql = "select * from Mes_GYD where 1=1 and isDel = 1 ";
            string strAnd = " and ";

            #region where

            if (!string.IsNullOrEmpty(model.gxDuanCode))
            {
                strSql += strAnd;
                strSql += " gxDuanCode Like '" + model.gxDuanCode + "%' ";
            }

            if (!string.IsNullOrEmpty(model.gxDuanName))
            {
                strSql += strAnd;
                strSql += " gxDuanName Like '" + model.gxDuanName + "%' ";
            }

            if (!string.IsNullOrEmpty(model.bbNo))
            {
                strSql += strAnd;
                strSql += " bbNo Like '" + model.bbNo + "%' ";
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
            strSql += " order by gxDuanCode ";
            #endregion

            try
            {
                lst = new List<Mes_ProductGYDInfo>();
                //GYD
                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        Mes_ProductGYDInfo entityGYD = new Mes_ProductGYDInfo();

                        entityGYD.ID = dr["ID"].ToInt32();
                        entityGYD.bbNo = dr["bbNo"].ToDBString();
                        entityGYD.gxDuanCode = dr["gxDuanCode"].ToDBString();
                        entityGYD.gxDuanName = dr["gxDuanName"].ToDBString();
                        //add by cym 2018-10-29
                        entityGYD.gxSeq = dr["seqno"].ToInt32();
                        entityGYD.cptype = dr["cptype"].ToString();
                        //add by cym 2018-11-7
                        entityGYD.workHours = dr["workHours"].ToDecimal();

                        entityGYD.Creater = dr["Creater"].ToDBString();
                        entityGYD.CreateTime = dr["CreateTime"].ToDateTime();

                        entityGYD.isDel = dr["isDel"].ToInt32();

                        lst.Add(entityGYD);
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

        public bool GetProductGYListDataBygxDuanCode(string gxDuanCode, ref Mes_ProductGYDInfo entityGYD, ref string ErrMsg)
        {
            string strSql = string.Empty;

            strSql = "select * from Mes_GYD t where t.gxDuanCode='" + gxDuanCode + "'";

            try
            {
                //GYD
                entityGYD = new Mes_ProductGYDInfo();
                
                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    if (dr.Read())
                    {
                        entityGYD.ID = dr["ID"].ToInt32();
                        entityGYD.bbNo = dr["bbNo"].ToDBString();
                        entityGYD.gxDuanCode = dr["gxDuanCode"].ToDBString();
                        entityGYD.gxDuanName = dr["gxDuanName"].ToDBString();

                        //add by cym 2018-10-29
                        entityGYD.gxSeq = dr["seqno"].ToInt32();
                        entityGYD.cptype = dr["cptype"].ToDBString();
                        //add by cym 2018-11-7
                        entityGYD.workHours = dr["workHours"].ToDecimal();
                        //add by cym 2018-11-27
                        entityGYD.MachineType = dr["MachineType"].ToDBString();
                        entityGYD.MachineName = dr["MachineName"].ToDBString();

                        entityGYD.Creater = dr["Creater"].ToDBString();
                        entityGYD.CreateTime = dr["CreateTime"].ToDateTime();

                        entityGYD.isDel = dr["isDel"].ToInt32();

                        entityGYD.lstGY = new List<Mes_ProductGYInfo>();
                    }
                    else
                    {
                        ErrMsg = "no Mes_GYD data!";
                        return false;
                    }
                }

                //GY
                strSql = "select t.*,cp.name from Mes_GY t inner join mes_cptype cp on t.cptype=cp.id where t.gxDuanCode='" + gxDuanCode + "'";

                List<Mes_ProductGYInfo> lst = new List<Mes_ProductGYInfo>();

                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        Mes_ProductGYInfo model = new Mes_ProductGYInfo();

                        model.ID = dr["ID"].ToInt32();
                        model.gxID = dr["gxID"].ToDBString();
                        model.bbNo = dr["bbNo"].ToDBString();
                        model.gxDuanCode = dr["gxDuanCode"].ToDBString();
                        model.gxDuanName = dr["gxDuanName"].ToDBString();
                        model.cptype = dr["cptype"].ToDBString();
                        model.machineType = dr["machineType"].ToDBString();
                        model.gxSeq = dr["gxSeq"].ToInt32();
                        model.gxName = dr["gxName"].ToDBString();

                        model.isDel = dr["isDel"].ToInt32();
                        model.description = dr["description"].ToDBString();

                        model.CompanyCode = dr["name"].ToDBString();

                        //var blob = dr.GetOracleBlob(0);
                        //byte[] bout = new byte[blob.Length];
                        //blob.Read(bout, 0, bout.Length);

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

                        model.videoUrl = dr["videoUrl"].ToDBString();
                        model.remark = dr["remark"].ToDBString();

                        model.Creater = dr["Creater"].ToDBString();
                        model.CreateTime = dr["CreateTime"].ToDateTimeNull();
                        model.Modifyer = dr["Modifyer"].ToDBString();
                        model.ModifyTime = dr["ModifyTime"].ToDateTimeNull();

                        model.lstLimit = new List<Mes_ProductGY_LimitInfo>();

                        lst.Add(model);
                    }
                }

                if (lst.Count <= 0)
                {
                    ErrMsg = "no Mes_GY data!";
                    return false;
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

                entityGYD.lstGY = lst;

                return true;
            }
            catch(Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public int GetCountGXDcode(string gxDuanCode)
        {
            object id = dbFactory.ExecuteScalar(System.Data.CommandType.Text, "SELECT count(gxDuanCode) FROM Mes_GYD WHERE gxDuanCode='" + gxDuanCode + "'");

            return id.ToInt32();
        }


        public bool SaveProductGYDData(string userNo, Mes_ProductGYDInfo entityGYD, ref string ErrMsg)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            try
            {
                List<BILBasic.DBA.dbFactory.imageButtfes> buttfes = new List<BILBasic.DBA.dbFactory.imageButtfes>();

                //更新
                if (entityGYD.ID > 0)
                {
                    //delete GYD ; delete GY ; delete GY_Limit
                    strSql = string.Format("delete Mes_GYD where gxDuanCode = '{0}'", entityGYD.gxDuanCode);
                    lstSql.Add(strSql);
                    strSql = string.Format("delete Mes_GY where gxDuanCode = '{0}'", entityGYD.gxDuanCode);
                    lstSql.Add(strSql);
                    strSql = string.Format("delete Mes_GY_Limit where gxDuanCode = '{0}'", entityGYD.gxDuanCode);
                    lstSql.Add(strSql);

                    //add GYD
                    strSql = string.Format("insert into Mes_GYD (Id, bbNo, gxDuanCode, gxDuanName, isDel, creater, createtime, MachineName,MachineType,cptype,seqno,workHours)" +
                        " values('{0}','{1}','{2}','{3}','1','{4}',Sysdate,'{5}','{6}','{7}','{8}','{9}')",
                        entityGYD.ID, entityGYD.bbNo, entityGYD.gxDuanCode, entityGYD.gxDuanName, userNo, entityGYD.MachineName, entityGYD.MachineType, entityGYD.cptype, entityGYD.gxSeq, entityGYD.workHours);

                    lstSql.Add(strSql);

                    int voucherID = 0;
                    foreach (var model in entityGYD.lstGY)
                    {
                        //插入Mes_GY

                        voucherID = base.GetTableID("SEQ_MES_GY");
                        //string VoucherNoID = base.GetTableID("seq_outstock_no").ToString();

                        //string VoucherNo ="F"+ System.DateTime.Now.ToString("yyyyMMdd") + VoucherNoID.PadLeft(4, '0');

                        strSql = string.Format("insert into Mes_GY (Id,  gxID, cptype, machineType, gxSeq,  gxName, isDel, description,imagePic,imagePic2,imagePic3,videoUrl,creater,createtime,remark,bbNo,gxDuanCode,gxDuanName)" +
                            " values('{0}','{1}','{2}','{3}','{4}','{5}',1,'{6}',:Images,:Images2,:Images3,'{7}','{8}',Sysdate,'{9}','{10}','{11}','{12}')",
                            voucherID, model.gxID, model.cptype, model.machineType, model.gxSeq, model.gxName, model.description, model.videoUrl, userNo, model.remark, model.bbNo, entityGYD.gxDuanCode, model.gxDuanName);

                        lstSql.Add(strSql);

                        buttfes.Add(new BILBasic.DBA.dbFactory.imageButtfes()
                        {
                            imagePic1 = model.imagePic,
                            imagePic2 = model.imagePic2,
                            imagePic3 = model.imagePic3
                        });

                    }

                    //插入Mes_GY_limit
                    foreach (var model in entityGYD.lstGY)
                    {
                        if (model.lstLimit != null && model.lstLimit.Count > 0)
                        {
                            foreach (var gyxz in model.lstLimit)
                            {
                                //插入Mes_GY_Limit
                                voucherID = base.GetTableID("SEQ_MES_GY_LIMIT");

                                strSql = string.Empty;

                                strSql = "insert into Mes_GY_Limit(Id,gxID,xzID,type,xzName,xzRoleMax,xzRoleMin,QTY,Unit,gxDuanCode)" +
                                    "values('" + voucherID + "','" + gyxz.gyDetailID + "','" + gyxz.xzID + "','" + gyxz.type + "','" + gyxz.xzName + "','" +
                                    gyxz.xzRoleMax + "','" + gyxz.xzRoleMin + "','" + gyxz.QTY + "','" + gyxz.Unit + "','" + entityGYD.gxDuanCode + "') ";
                                lstSql.Add(strSql);
                            }
                        }
                    }
                }
                else //插入
                {
                    //插入Mes_GYD
                    int voucherID = base.GetTableID("SEQ_MES_GYD");

                    int seqno = 0;
                    //GetMaxGYInfoByCptype(entityGYD.cptype, ref seqno, ref ErrMsg);
                    seqno = GetCountGXDcode(entityGYD.gxDuanCode);
                    if (seqno > 0)
                    {
                        ErrMsg = "该工序编码：" + entityGYD.gxDuanCode + "已经存在！请重新输入新的工序编码！";
                        return false;
                    }

                    //string gxDuanCode = entityGYD.cptype + "-GX-" + seqno.ToString().PadLeft(3, '0');

                    strSql = string.Format("insert into Mes_GYD (Id, bbNo, gxDuanCode, gxDuanName, isDel, creater, createtime, MachineName,MachineType,cptype,seqno,workHours)" +
                        " values('{0}','{1}','{2}','{3}','1','{4}',Sysdate,'{5}','{6}','{7}','{8}','{9}')",
                        voucherID, entityGYD.bbNo, entityGYD.gxDuanCode, entityGYD.gxDuanName, userNo, entityGYD.MachineName, entityGYD.MachineType, entityGYD.cptype, seqno, entityGYD.workHours);

                    lstSql.Add(strSql);

                    //插入Mes_GY
                    foreach (var model in entityGYD.lstGY)
                    {
                        //插入Mes_GY
                        voucherID = base.GetTableID("SEQ_MES_GY");

                        //string VoucherNoID = base.GetTableID("seq_outstock_no").ToString();

                        //string VoucherNo ="F"+ System.DateTime.Now.ToString("yyyyMMdd") + VoucherNoID.PadLeft(4, '0');

                        strSql = string.Format("insert into Mes_GY (Id,  gxID, cptype, machineType, gxSeq,  gxName, isDel, description,imagePic,imagePic2,imagePic3,videoUrl,creater,createtime,remark,bbNo,gxDuanCode,gxDuanName)" +
                            " values('{0}','{1}','{2}','{3}','{4}','{5}',1,'{6}',:Images,:Images2,:Images3,'{7}','{8}',Sysdate,'{9}','{10}','{11}','{12}')",
                            voucherID, model.gxID, model.cptype, model.machineType, model.gxSeq, model.gxName, model.description, model.videoUrl, userNo, model.remark, model.bbNo, entityGYD.gxDuanCode, model.gxDuanName);

                        lstSql.Add(strSql);

                        buttfes.Add(new BILBasic.DBA.dbFactory.imageButtfes()
                        {
                            imagePic1 = model.imagePic,
                            imagePic2 = model.imagePic2,
                            imagePic3 = model.imagePic3
                        });

                    }

                    //插入Mes_GY_limit
                    foreach (var model in entityGYD.lstGY)
                    {
                        if (model.lstLimit != null && model.lstLimit.Count > 0)
                        {
                            foreach (var gyxz in model.lstLimit)
                            {
                                //插入Mes_GY_Limit
                                voucherID = base.GetTableID("SEQ_MES_GY_LIMIT");

                                strSql = string.Empty;

                                strSql = "insert into Mes_GY_Limit(Id,gxID,xzID,type,xzName,xzRoleMax,xzRoleMin,QTY,Unit,gxDuanCode)" +
                                    "values('" + voucherID + "','" + gyxz.gyDetailID + "','" + gyxz.xzID + "','" + gyxz.type + "','" + gyxz.xzName + "','" +
                                    gyxz.xzRoleMax + "','" + gyxz.xzRoleMin + "','" + gyxz.QTY + "','" + gyxz.Unit + "','" + entityGYD.gxDuanCode + "') ";
                                lstSql.Add(strSql);
                            }
                        }
                    }
                }

                if (lstSql == null || lstSql.Count == 0)
                {
                    ErrMsg = "no data!!!";
                    return false;
                }

                int i = dbFactory.ExecuteNonQueryListByCymImages3(lstSql, buttfes, ref ErrMsg);

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

        public bool DeleteProductGYDInfo(string gxDuanCode, int isdel, ref string ErrMsg)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            try
            {
                //更新
                strSql = "update Mes_GYD set isDel = '" + isdel + "' where gxDuanCode = '" + gxDuanCode + "'";
                lstSql.Add(strSql);
                
                int i = dbFactory.ExecuteNonQueryList(lstSql, ref ErrMsg);

                if (i > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }


        public bool GetMaxGYInfoByCptype(string cptype, ref int intNum, ref string ErrMsg)
        {
            string strSql = string.Empty;

            try
            {

                //GYD
                strSql = "select max(seqno) as maxSeq from Mes_GYD t where t.cptype='" + cptype + "'";

                intNum = 0;

                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    if (dr.Read())
                    {
                        intNum = dr["maxSeq"].ToInt32() + 1;
                    }
                    else
                    {
                        intNum = 1;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

    }
}
