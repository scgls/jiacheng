
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
    public partial class Mes_ProductGYLine_DB : BILBasic.Basing.Factory.Base_DB<Mes_ProductGYLineInfo>
    {

        /// <summary>
        /// 添加t_outstock
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(Mes_ProductGYLineInfo t_outstock)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref Mes_ProductGYLineInfo model)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            //更新
            if (model.ID > 0)
            {
                //strSql = string.Format("update Mes_GY a set a.cptype = '{0}',a.machineType='{1}',a.gxSeq='{2}',a.Modifyer = '{3}',a.Modifytime = Sysdate,a.description = '{5}',a.imagePic = '{6}',a.videoUrl = '{7}',a.remark = '{8}' where a.Id = '{4}'",
                //    model.cptype, model.machineType, model.gxSeq, user.UserNo, model.ID, model.description, model.imagePic, model.videoUrl, model.remark);
                //lstSql.Add(strSql);
            }
            else //插入
            {
                int voucherID = base.GetTableID("SEQ_MES_GY");

                model.ID = voucherID.ToInt32();

                string VoucherNoID = base.GetTableID("seq_outstock_no").ToString();

                //string VoucherNo ="F"+ System.DateTime.Now.ToString("yyyyMMdd") + VoucherNoID.PadLeft(4, '0');
                string gyid = Guid.NewGuid().ToString();

                //strSql = string.Format("insert into Mes_GY (Id,  gxID, cptype, machineType, gxSeq,  gxName, isDel, description,imagePic,videoUrl,creater,createtime,remark)" +
                //    " values('{0}','{1}','{2}','{3}','{4}','{5}',1,'{6}','{7}','{8}','{9}',Sysdate,'{10}')",
                //    voucherID, gyid, model.cptype, model.machineType, model.gxSeq, model.gxName, model.description, model.imagePic, model.videoUrl, user.UserNo, model.remark);

                //lstSql.Add(strSql);
            }

            return lstSql;
        }

        protected override List<string> GetUpdateSql(UserModel user, Mes_ProductGYLineInfo model)
        {
            List<string> lstSql = new List<string>();

            string strSql = "update Mes_GYLine a set a.isDel = '" + model.isDel + "' where id = '" + model.ID + "'";

            lstSql.Add(strSql);

            return lstSql;
        }


        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override Mes_ProductGYLineInfo ToModel(IDataReader reader)
        {
            Mes_ProductGYLineInfo mes_productGY = new Mes_ProductGYLineInfo();

            mes_productGY.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            mes_productGY.cptype = (string)dbFactory.ToModelValue(reader, "cptype");
            mes_productGY.machineType = (string)dbFactory.ToModelValue(reader, "machineType");
            mes_productGY.Seq = dbFactory.ToModelValue(reader, "Seq").ToInt32();
            mes_productGY.gyLineName = (string)dbFactory.ToModelValue(reader, "gyLineName");
            mes_productGY.isDel = dbFactory.ToModelValue(reader, "isDel").ToInt32();
            mes_productGY.cpCode = (string)dbFactory.ToModelValue(reader, "cpCode");
            mes_productGY.cpName = (string)dbFactory.ToModelValue(reader, "cpName");
            mes_productGY.gybbID = (string)dbFactory.ToModelValue(reader, "gybbID");

            mes_productGY.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            mes_productGY.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            mes_productGY.Modifyer = (string)dbFactory.ToModelValue(reader, "Modifyer");
            mes_productGY.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "ModifyTime");

            mes_productGY.remark = (string)dbFactory.ToModelValue(reader, "remark");

            //todo:
            mes_productGY.StrStatus = (string)dbFactory.ToModelValue(reader, "StrStatus");
            mes_productGY.StrCreater = (string)dbFactory.ToModelValue(reader, "StrCreater");

            return mes_productGY;
        }

        protected override string GetViewName()
        {
            return "V_PRODUCT_GYLINE";
        }

        protected override string GetTableName()
        {
            return "V_PRODUCT_GYLINE";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }


        protected override string GetFilterSql(UserModel user, Mes_ProductGYLineInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";


            if (model.isMoban == 0)
            {
                strSql += strAnd;
                strSql += " isMoban= 0";
            }
            else
            {
                strSql += strAnd;
                strSql += " isMoban= 1";
            }

            if (model.isDel > 0)
            {
                strSql += strAnd;
                strSql += " isDel= 1";
            }

            if (!string.IsNullOrEmpty(model.gyLineName))
            {
                strSql += strAnd;
                strSql += " gyLineName Like '" + model.gyLineName + "%' ";
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

        public bool GetProductGYLineDataByID(ref Mes_ProductGYLineInfo model, ref string ErrMsg)
        {
            string strSql = string.Empty;

            strSql = "select t.* from Mes_GYLine t where t.gyLineID='" + model.gyLineID + "'";

            model = new Mes_ProductGYLineInfo();

            try
            {
                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    if (dr.Read())
                    {
                        model.ID = dr["ID"].ToInt32();
                        model.gyLineID = dr["gyLineID"].ToDBString();
                        model.cptype = dr["cptype"].ToDBString();
                        model.machineType = dr["machineType"].ToDBString();
                        model.Seq = dr["Seq"].ToInt32();
                        model.gyLineName = dr["gyLineName"].ToDBString();

                        model.isDel = dr["isDel"].ToInt32();
                        model.cpCode = dr["cpCode"].ToDBString();

                        model.cpName = dr["cpName"].ToDBString();
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

                return true;
            }
            catch(Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public int GetMaxGYbbID(string cptype)
        {
            object id = dbFactory.ExecuteScalar(System.Data.CommandType.Text, "SELECT Max(gybbID) FROM Mes_GYLine WHERE cptype='" + cptype + "'");

            return id.ToInt32() + 1;
        }

        public bool SaveProductGYLineData(string userNo, Mes_ProductGYLineInfo model, ref string ErrMsg)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            try
            {
                List<BILBasic.DBA.dbFactory.imageButtfes> buttfes = new List<BILBasic.DBA.dbFactory.imageButtfes>();
                
                //更新
                if (model.ID > 0)
                {
                    //delete GYLine ; delete GYD/GY/Limit
                    strSql = string.Format("delete Mes_GYLine where gyLineID = '{0}'", model.gyLineID);
                    lstSql.Add(strSql);
                    strSql = string.Format("delete Mes_GYLine_GYD where gyLineID = '{0}'", model.gyLineID);
                    lstSql.Add(strSql);
                    strSql = string.Format("delete Mes_GYLine_GY where gyLineID = '{0}'", model.gyLineID);
                    lstSql.Add(strSql);
                    strSql = string.Format("delete Mes_GYLine_Limit where gyLineID = '{0}'", model.gyLineID);
                    lstSql.Add(strSql);

                    //插入Mes_GYLine
                    int voucherID = base.GetTableID("SEQ_MES_GYLINE");

                    //工艺路线-表
                    lstSql.Add("insert into Mes_GYLine(Id, gyLineID,bbID,gybbID,cpCode,cpName,gyLineName,machineType,isDel,isNew,capacity,cptype,Qty,isMoban,remark,creater,createtime,SHENHE,YEWUSHENHE,SHENGCHANSHENHE,PINBAOSHENHE,PIZHUN)" +
                                        "values('" + voucherID + "','" + model.gyLineID + "','" + model.bbID + "','" + model.gybbID + "','"
                                        + model.cpCode + "','" + model.cpName.Replace("'", "’") + "','" + model.gyLineName + "','" + model.machineType + "','1','"
                                        + model.isNew + "','" + model.capacity.Value + "','" + model.cptype + "','" + model.Qty + "','" + model.isMoban + "','" + model.remark + "','"
                                        + model.Creater + "',Sysdate,'" + model.Auditor + "','" + model.ERPCreater + "','" + model.StrCreater + "','" + model.StrModifyer + "','" + model.VouUser + "') ");


                    //工艺路线明细-工艺路线对应工序段表
                    foreach (var gyld in model.lstGYLine_GYD)
                    {
                        //插入Mes_GYLine_GYD
                        voucherID = base.GetTableID("SEQ_MES_GYLINE_GYD");

                        strSql = string.Empty;

                        strSql = "insert into Mes_GYLine_GYD(Id,gxDuanCode,gyLineID,gxDuanName,MachineName,MachineType,MouldCode,gxSeq,Qty,gxDCode,MouldName)" +
                            "values('" + voucherID + "','" + gyld.gxDuanCode + "','" + gyld.gyLineID + "','" + gyld.gxDuanName + "','" + gyld.MachineName.Replace("'", "’") + "','" +
                            gyld.MachineType + "','" + gyld.MouldCode + "','" + gyld.gxSeq + "','" + gyld.Qty + "','" + gyld.gxDCode + "','" + gyld.MaterialDoc + "') ";
                        lstSql.Add(strSql);
                    }

                    //工艺操作-工序段对应工序操作表
                    foreach (var gyg in model.lstGYLine_GYD)
                    {
                        foreach (var gygEntity in gyg.lstGY)
                        {
                            strSql = string.Empty;

                            strSql = "insert into Mes_GYLine_GY(gyDetailID,gyLineID,cpCode,gxDuanCode,gxID,gxName,Seq,description,imagePic,imagePic2,imagePic3,videoUrl,Remark)" +
                                "values('" + gygEntity.gyDetailID + "','" + gygEntity.gyLineID + "','" + gygEntity.cpCode + "','" + gygEntity.gxDuanCode + "','" +
                                gygEntity.gxID + "','" + gygEntity.gxName + "','" + gygEntity.Seq + "','" + gygEntity.description.Replace("'","’") + "',:imaBuffers,:imaBuffers2,:imaBuffers3,'" +
                                gygEntity.videoUrl + "','" + gygEntity.remark + "') ";
                            lstSql.Add(strSql);

                            buttfes.Add(new BILBasic.DBA.dbFactory.imageButtfes()
                            {
                                imagePic1 = gygEntity.imagePic,
                                imagePic2 = gygEntity.imagePic2,
                                imagePic3 = gygEntity.imagePic3
                            });
                        }
                    }
                    //工艺操作对应限值-模板表
                    foreach (var gyl in model.lstGYLine_GYD)
                    {
                        foreach (var gylEntity in gyl.lstGY)
                        {
                            if (gylEntity.lstLimit != null && gylEntity.lstLimit.Count > 0)
                            {
                                foreach (var gygg in gylEntity.lstLimit)
                                {
                                    //插入Mes_GYLine_Limit
                                    voucherID = base.GetTableID("SEQ_MES_GYLINE_LIMIT");

                                    strSql = string.Empty;

                                    strSql = "insert into Mes_GYLine_Limit(Id,gyDetailID,gyLineID,xzID,type,xzName,xzRoleMax,xzRoleMin,QTY,Unit)" +
                                        "values('" + voucherID + "','" + gygg.gyDetailID + "','" + gygg.gyLineID + "','" + gygg.xzID + "','" + gygg.type + "','" + gygg.xzName + "','" +
                                        gygg.xzRoleMax + "','" + gygg.xzRoleMin + "','" + gygg.QTY + "','" + gygg.Unit + "') ";
                                    lstSql.Add(strSql);
                                }
                            }
                        }
                    }
                }
                else //插入
                {
                    //插入Mes_GYLine
                    int voucherID = base.GetTableID("SEQ_MES_GYLINE");

                    model.ID = voucherID.ToInt32();

                    //string VoucherNoID = base.GetTableID("seq_outstock_no").ToString();

                    //string VoucherNo ="F"+ System.DateTime.Now.ToString("yyyyMMdd") + VoucherNoID.PadLeft(4, '0');
                    //string gyid = Guid.NewGuid().ToString();
                    
                    //工艺路线-表
                    lstSql.Add("insert into Mes_GYLine(Id, gyLineID,bbID,gybbID,cpCode,cpName,gyLineName,machineType,isDel,isNew,capacity,cptype,Qty,isMoban,remark,creater,createtime,SHENHE,YEWUSHENHE,SHENGCHANSHENHE,PINBAOSHENHE,PIZHUN)" +
                                        "values('" + voucherID + "','" + model.gyLineID + "','" + model.bbID + "','" + model.gybbID + "','"
                                        + model.cpCode + "','" + model.cpName.Replace("'", "’") + "','" + model.gyLineName + "','" + model.machineType + "','1','"
                                        + model.isNew + "','" + model.capacity.Value + "','" + model.cptype + "','" + model.Qty + "','" + model.isMoban + "','" + model.remark + "','"
                                        + userNo + "',Sysdate,'" + model.Auditor + "','" + model.ERPCreater + "','" + model.StrCreater + "','" + model.StrModifyer + "','" + model.VouUser + "') ");

                    int gydID = 1;
                    //工艺路线明细-工艺路线对应工序段表
                    foreach (var gyld in model.lstGYLine_GYD)
                    {
                        //插入Mes_GYLine_GYD
                        gydID = base.GetTableID("SEQ_MES_GYLINE_GYD");

                        strSql = string.Empty;

                        strSql = "insert into Mes_GYLine_GYD(Id,gxDuanCode,gyLineID,gxDuanName,MachineName,MachineType,MouldCode,gxSeq,Qty,gxDCode,MouldName)" +
                            "values('" + gydID + "','" + gyld.gxDuanCode + "','" + gyld.gyLineID + "','" + gyld.gxDuanName + "','" + gyld.MachineName.Replace("'", "’") + "','" +
                            gyld.MachineType + "','" + gyld.MouldCode + "','" + gyld.gxSeq + "','" + gyld.Qty + "','" + gyld.gxDCode + "','" + gyld.MaterialDoc + "') ";
                        lstSql.Add(strSql);
                    }

                    //工艺操作-工序段对应工序操作表
                    foreach (var gyg in model.lstGYLine_GYD)
                    {
                        foreach (var gygEntity in gyg.lstGY)
                        {
                            strSql = string.Empty;

                            strSql = "insert into Mes_GYLine_GY(gyDetailID,gyLineID,cpCode,gxDuanCode,gxID,gxName,Seq,description,imagePic,imagePic2,imagePic3,videoUrl,Remark)" +
                                "values('" + gygEntity.gyDetailID + "','" + gygEntity.gyLineID + "','" + gygEntity.cpCode + "','" + gygEntity.gxDuanCode + "','" +
                                gygEntity.gxID + "','" + gygEntity.gxName + "','" + gygEntity.Seq + "','" + gygEntity.description + "',:imaBuffers,:imaBuffers2,:imaBuffers3,'" + 
                                gygEntity.videoUrl + "','" + gygEntity.remark + "') ";
                            lstSql.Add(strSql);

                            buttfes.Add(new BILBasic.DBA.dbFactory.imageButtfes()
                            {
                                imagePic1 = gygEntity.imagePic,
                                imagePic2 = gygEntity.imagePic2,
                                imagePic3 = gygEntity.imagePic3
                            });
                        }
                    }
                    //工艺操作对应限值-模板表
                    foreach (var gyl in model.lstGYLine_GYD)
                    {
                        foreach (var gylEntity in gyl.lstGY)
                        {
                            if (gylEntity.lstLimit != null && gylEntity.lstLimit.Count > 0)
                            {
                                foreach (var gygg in gylEntity.lstLimit)
                                {
                                    //插入Mes_GYLine_Limit
                                    voucherID = base.GetTableID("SEQ_MES_GYLINE_LIMIT");

                                    strSql = string.Empty;

                                    strSql = "insert into Mes_GYLine_Limit(Id,gyDetailID,gyLineID,xzID,type,xzName,xzRoleMax,xzRoleMin,QTY,Unit)" +
                                        "values('" + voucherID + "','" + gygg.gyDetailID + "','" + gygg.gyLineID + "','" + gygg.xzID + "','" + gygg.type + "','" + gygg.xzName + "','" +
                                        gygg.xzRoleMax + "','" + gygg.xzRoleMin + "','" + gygg.QTY + "','" + gygg.Unit + "') ";
                                    lstSql.Add(strSql);
                                }
                            }
                        }
                    }

                }

                if (lstSql == null || lstSql.Count == 0)
                {
                    ErrMsg = "no data!!!";
                    return false;
                }

                int i = dbFactory.ExecuteNonQueryListByCymNewImages3(lstSql, buttfes, ref ErrMsg);

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

        public bool DeleteProductGYLineInfo(string gxLineID, int isdel, ref string ErrMsg)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            try
            {
                //更新
                strSql = "update Mes_GYLine set isDel = '" + isdel + "' where gyLineID = '" + gxLineID + "'";
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

        public bool GetProductGYLineListDataBygxLineID(string gxLineID, ref Mes_ProductGYLineInfo entityGYLine, ref List<Mes_GYLine_GYInfo> lstDisGY, ref string ErrMsg)
        {
            string strSql = string.Empty;

            strSql = "select * from Mes_GYLine t where t.gyLineID='" + gxLineID + "'";

            try
            {
                //Mes_GYLine
                entityGYLine = new Mes_ProductGYLineInfo();

                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    if (dr.Read())
                    {
                        entityGYLine.ID = dr["ID"].ToInt32();
                        entityGYLine.gyLineID = dr["gyLineID"].ToDBString();
                        entityGYLine.machineType = dr["machineType"].ToDBString();
                        entityGYLine.bbID = dr["bbID"].ToDBString();
                        entityGYLine.gybbID = dr["gybbID"].ToDBString();
                        entityGYLine.cpCode = dr["cpCode"].ToDBString();
                        entityGYLine.cpName = dr["cpName"].ToDBString();
                        entityGYLine.gyLineName = dr["gyLineName"].ToDBString();
                        entityGYLine.capacity = dr["capacity"].ToDecimalNull();
                        entityGYLine.cptype = dr["cptype"].ToDBString();
                        entityGYLine.Qty = dr["Qty"].ToInt32();
                        entityGYLine.isMoban = dr["isMoban"].ToInt32();
                        entityGYLine.isNew = dr["isNew"].ToInt32();
                        entityGYLine.remark = dr["remark"].ToDBString();

                        entityGYLine.Creater = dr["Creater"].ToDBString();
                        entityGYLine.CreateTime = dr["CreateTime"].ToDateTime();
                        entityGYLine.Modifyer = dr["Modifyer"].ToDBString();
                        entityGYLine.ModifyTime = dr["ModifyTime"].ToDateTimeNull();

                        //add by cym 2018-10-29 在新增工艺路线模板界面中，增加几个字段，这些字段变更时不走审批，编写人（可引用创建人）、审核人、业务审核人、生产审核、品保审核、批准人
                        entityGYLine.Auditor = dr["SHENHE"].ToDBString();
                        entityGYLine.ERPCreater = dr["YEWUSHENHE"].ToDBString();
                        entityGYLine.StrCreater = dr["SHENGCHANSHENHE"].ToDBString();
                        entityGYLine.StrModifyer = dr["PINBAOSHENHE"].ToDBString();
                        entityGYLine.VouUser = dr["PIZHUN"].ToDBString();

                        entityGYLine.isDel = dr["isDel"].ToInt32();

                        entityGYLine.lstGYLine_GYD = new List<Mes_ProductGYLine_GYDInfo>();
                    }
                    else
                    {
                        ErrMsg = "no Mes_GYLine data!";
                        return false;
                    }
                }

                //Mes_GYLine_GYD
                strSql = "select * from Mes_GYLine_GYD t where t.gyLineID='" + gxLineID + "'";

                List<Mes_ProductGYLine_GYDInfo> lst = new List<Mes_ProductGYLine_GYDInfo>();

                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        Mes_ProductGYLine_GYDInfo model = new Mes_ProductGYLine_GYDInfo();

                        model.ID = dr["ID"].ToInt32();
                        model.gxDuanCode = dr["gxDuanCode"].ToDBString();
                        model.gxDuanName = dr["gxDuanName"].ToDBString();
                        model.gyLineID = dr["gyLineID"].ToDBString();
                        model.MachineName = dr["MachineName"].ToDBString();
                        model.gxSeq = dr["gxSeq"].ToInt32();
                        model.MachineType = dr["MachineType"].ToDBString();

                        model.Qty = dr["Qty"].ToDecimal();
                        model.MouldCode = dr["MouldCode"].ToDBString();
                        model.MaterialDoc = dr["MouldName"].ToDBString();
                        model.gxDCode = dr["gxDCode"].ToDBString();

                        model.lstGY = new List<Mes_GYLine_GYInfo>();
                        
                        lst.Add(model);
                    }
                }

                if (lst.Count <= 0)
                {
                    ErrMsg = "no Mes_GYLine_GYD data!";
                    return false;
                }

                //Mes_GYLine_GY
                strSql = "select * from Mes_GYLine_GY t where t.gyLineID='" + gxLineID + "'";

                List<Mes_GYLine_GYInfo>  lstGY = new List<Mes_GYLine_GYInfo>();

                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        Mes_GYLine_GYInfo model = new Mes_GYLine_GYInfo();

                        model.gyDetailID = dr["gyDetailID"].ToDBString();
                        model.gyLineID = dr["gyLineID"].ToDBString();
                        model.gxDuanCode = dr["gxDuanCode"].ToDBString();
                        model.cpCode = dr["cpCode"].ToDBString();
                        model.gxID = dr["gxID"].ToDBString();
                        model.gxName = dr["gxName"].ToDBString();
                        model.Seq = dr["Seq"].ToInt32();

                        model.description = dr["description"].ToDBString();

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

                        model.lstLimit = new List<Mes_ProductGY_LimitInfo>();

                        lstGY.Add(model);
                    }
                }

                if (lstGY.Count <= 0)
                {
                    ErrMsg = "no Mes_GYLine_GY data!";
                    return false;
                }
                else
                {
                    lstDisGY = lstGY;
                }

                //Mes_GYLine_Limit
                strSql = "select * from Mes_GYLine_Limit t where t.gyLineID='" + gxLineID + "'";

                List<Mes_ProductGY_LimitInfo> lstLimit = new List<Mes_ProductGY_LimitInfo>();

                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        Mes_ProductGY_LimitInfo model = new Mes_ProductGY_LimitInfo();

                        model.ID = dr["ID"].ToInt32();
                        model.xzID = dr["xzID"].ToDBString();
                        model.gyDetailID = dr["gyDetailID"].ToDBString();
                        model.gyLineID = dr["gyLineID"].ToDBString();
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
                    lstDisGY.ForEach(delegate(Mes_GYLine_GYInfo entityGy)
                    {
                        var templst = lstLimit.FindAll(t => t.gyDetailID == entityGy.gyDetailID);

                        if (templst != null && templst.Count >0)
                        {
                            entityGy.lstLimit = templst;
                        }
                    });
                }

                lst.ForEach(delegate(Mes_ProductGYLine_GYDInfo entityGYD)
                {
                    var templst = lstGY.FindAll(t => t.gxDuanCode == entityGYD.gxDuanCode);
                    if (templst != null && templst.Count > 0)
                    {
                        templst.ForEach(t => t.HeaderID = entityGYD.gxSeq);
                        entityGYD.lstGY = templst;
                    }
                });

                //lstDisGY = lstGY;

                entityGYLine.lstGYLine_GYD = lst;

                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }


        #region 正式工艺路线

        public int GetMaxGYbbID_C(string cpCode, string machineType)
        {
            //update by cym 2019-1-2 把自动/半自动/手工 方式改成 用户自动输入的加工方式
            //object id = dbFactory.ExecuteScalar(System.Data.CommandType.Text, "SELECT Max(gybbID) FROM Mes_GYLine_C WHERE cpCode='" + cpCode + "'  and machineType='" + machineType + "'");

            object id = dbFactory.ExecuteScalar(System.Data.CommandType.Text, "SELECT Max(gybbID) FROM Mes_GYLine_C WHERE cpCode='" + cpCode + "'  and processMode='" + machineType + "'");

            return id.ToInt32() + 1;
        }

        public bool SaveProductGYLineData_C(string userNo, Mes_ProductGYLineInfo model, ref string ErrMsg)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            try
            {
                List<BILBasic.DBA.dbFactory.imageButtfes> buttfes = new List<BILBasic.DBA.dbFactory.imageButtfes>();

                //更新
                if (model.ID > 0)
                {
                    //delete GYLine ; delete GYD/GY/Limit
                    strSql = string.Format("delete Mes_GYLine_C where gyLineID = '{0}'", model.gyLineID);
                    lstSql.Add(strSql);
                    strSql = string.Format("delete Mes_GYLine_GYD_C where gyLineID = '{0}'", model.gyLineID);
                    lstSql.Add(strSql);
                    strSql = string.Format("delete Mes_GYLine_GY_C where gyLineID = '{0}'", model.gyLineID);
                    lstSql.Add(strSql);
                    strSql = string.Format("delete Mes_GYLine_Limit_C where gyLineID = '{0}'", model.gyLineID);
                    lstSql.Add(strSql);

                    //插入Mes_GYLine_C
                    int voucherID = base.GetTableID("SEQ_MES_GYLINE_C");

                    //工艺路线-表
                    lstSql.Add("insert into Mes_GYLine_C(Id, gyLineID,bbID,gybbID,processMode,lineType,cpCode,cpName,gyLineName,machineType,isDel,isNew,capacity,cptype,Qty,isMoban,remark,creater,createtime,Modifyer,ModifyTime,SHENHE,YEWUSHENHE,SHENGCHANSHENHE,PINBAOSHENHE,PIZHUN,filePath1,filePath2)" +
                                        "values('" + voucherID + "','" + model.gyLineID + "','" + model.bbID + "','" + model.gybbID + "','" + model.processMode + "','" + model.lineType + "','"
                                        + model.cpCode + "','" + model.cpName.Replace("'", "’") + "','" + model.gyLineName + "','" + model.machineType + "','1','"
                                        + model.isNew + "','" + model.capacity.Value + "','" + model.cptype + "','" + model.Qty + "','" + model.isMoban + "','" + model.remark + "','"
                                        + model.Creater + "',to_date('" + model.CreateTime + "','YYYY-MM-DD hh24:mi:ss'),'" + model.Modifyer + "',Sysdate,'" + model.Auditor + "','" 
                                        + model.ERPCreater + "','" + model.StrCreater + "','" + model.StrModifyer + "','" + model.VouUser
                                        + "','" + model.CompanyCode + "','" + model.DepartmentCode + "') ");


                    //工艺路线明细-工艺路线对应工序段表
                    foreach (var gyld in model.lstGYLine_GYD)
                    {
                        //插入Mes_GYLine_GYD
                        voucherID = base.GetTableID("SEQ_MES_GYLINE_GYD_C");

                        strSql = string.Empty;

                        strSql = "insert into Mes_GYLine_GYD_C(Id,gxDuanCode,gyLineID,gxDuanName,MachineName,MachineType,MouldCode,gxSeq,Qty,gxDCode,MouldName)" +
                            "values('" + voucherID + "','" + gyld.gxDuanCode + "','" + gyld.gyLineID + "','" + gyld.gxDuanName + "','" + gyld.MachineName.Replace("'", "’") + "','" +
                            gyld.MachineType + "','" + gyld.MouldCode + "','" + gyld.gxSeq + "','" + gyld.Qty + "','" + gyld.gxDCode + "','" + gyld.MaterialDoc + "') ";
                        lstSql.Add(strSql);
                    }

                    //工艺操作-工序段对应工序操作表
                    foreach (var gyg in model.lstGYLine_GYD)
                    {
                        foreach (var gygEntity in gyg.lstGY)
                        {
                            strSql = string.Empty;

                            strSql = "insert into Mes_GYLine_GY_C(gyDetailID,gyLineID,cpCode,gxDuanCode,gxID,gxName,Seq,description,imagePic,imagePic2,imagePic3,videoUrl,Remark)" +
                                "values('" + gygEntity.gyDetailID + "','" + gygEntity.gyLineID + "','" + gygEntity.cpCode + "','" + gygEntity.gxDuanCode + "','" +
                                gygEntity.gxID + "','" + gygEntity.gxName + "','" + gygEntity.Seq + "','" + gygEntity.description.Replace("'", "’") + "',:imaBuffers,:imaBuffers2,:imaBuffers3,'" +
                                gygEntity.videoUrl + "','" + gygEntity.remark + "') ";
                            lstSql.Add(strSql);

                            buttfes.Add(new BILBasic.DBA.dbFactory.imageButtfes()
                            {
                                imagePic1 = gygEntity.imagePic,
                                imagePic2 = gygEntity.imagePic2,
                                imagePic3 = gygEntity.imagePic3
                            });
                        }
                    }
                    //工艺操作对应限值-模板表
                    foreach (var gyl in model.lstGYLine_GYD)
                    {
                        foreach (var gylEntity in gyl.lstGY)
                        {
                            if (gylEntity.lstLimit != null && gylEntity.lstLimit.Count > 0)
                            {
                                foreach (var gygg in gylEntity.lstLimit)
                                {
                                    //插入Mes_GYLine_Limit
                                    voucherID = base.GetTableID("SEQ_MES_GYLINE_LIMIT_C");

                                    strSql = string.Empty;

                                    strSql = "insert into Mes_GYLine_Limit_C(Id,gyDetailID,gyLineID,xzID,type,xzName,xzRoleMax,xzRoleMin,QTY,Unit)" +
                                        "values('" + voucherID + "','" + gygg.gyDetailID + "','" + gygg.gyLineID + "','" + gygg.xzID + "','" + gygg.type + "','" + gygg.xzName + "','" +
                                        gygg.xzRoleMax + "','" + gygg.xzRoleMin + "','" + gygg.QTY + "','" + gygg.Unit + "') ";
                                    lstSql.Add(strSql);
                                }
                            }
                        }
                    }
                }
                else //插入
                {
                    //插入Mes_GYLine_C
                    int voucherID = base.GetTableID("SEQ_MES_GYLINE_C");

                    model.ID = voucherID.ToInt32();

                    //string VoucherNoID = base.GetTableID("seq_outstock_no").ToString();

                    //string VoucherNo ="F"+ System.DateTime.Now.ToString("yyyyMMdd") + VoucherNoID.PadLeft(4, '0');
                    //string gyid = Guid.NewGuid().ToString();

                    //工艺路线-表
                    lstSql.Add("insert into Mes_GYLine_C(Id, gyLineID,bbID,gybbID,processMode,lineType,cpCode,cpName,gyLineName,machineType,isDel,isNew,capacity,cptype,Qty,isMoban,remark,creater,createtime,SHENHE,YEWUSHENHE,SHENGCHANSHENHE,PINBAOSHENHE,PIZHUN,filePath1,filePath2)" +
                                        "values('" + voucherID + "','" + model.gyLineID + "','" + model.bbID + "','" + model.gybbID + "','" + model.processMode + "','" + model.lineType + "','"
                                        + model.cpCode + "','" + model.cpName.Replace("'", "’") + "','" + model.gyLineName + "','" + model.machineType + "','1','"
                                        + model.isNew + "','" + model.capacity.Value + "','" + model.cptype + "','" + model.Qty + "','" + model.isMoban + "','" + model.remark + "','"
                                        + userNo + "',Sysdate,'" + model.Auditor + "','" + model.ERPCreater + "','" + model.StrCreater + "','" + model.StrModifyer + "','" + model.VouUser
                                        + "','" + model.CompanyCode + "','" + model.DepartmentCode + "') ");

                    int gydID = 1;
                    //工艺路线明细-工艺路线对应工序段表
                    foreach (var gyld in model.lstGYLine_GYD)
                    {
                        //插入Mes_GYLine_GYD_C
                        gydID = base.GetTableID("SEQ_MES_GYLINE_GYD_C");

                        strSql = string.Empty;

                        strSql = "insert into Mes_GYLine_GYD_C(Id,gxDuanCode,gyLineID,gxDuanName,MachineName,MachineType,MouldCode,gxSeq,Qty,gxDCode,MouldName)" +
                            "values('" + gydID + "','" + gyld.gxDuanCode + "','" + gyld.gyLineID + "','" + gyld.gxDuanName + "','" + gyld.MachineName.Replace("'", "’") + "','" +
                            gyld.MachineType + "','" + gyld.MouldCode + "','" + gyld.gxSeq + "','" + gyld.Qty + "','" + gyld.gxDCode + "','" + gyld.MaterialDoc + "') ";
                        lstSql.Add(strSql);
                    }

                    //工艺操作-工序段对应工序操作表
                    foreach (var gyg in model.lstGYLine_GYD)
                    {
                        foreach (var gygEntity in gyg.lstGY)
                        {
                            strSql = string.Empty;

                            strSql = "insert into Mes_GYLine_GY_C(gyDetailID,gyLineID,cpCode,gxDuanCode,gxID,gxName,Seq,description,imagePic,imagePic2,imagePic3,videoUrl,Remark)" +
                                "values('" + gygEntity.gyDetailID + "','" + gygEntity.gyLineID + "','" + gygEntity.cpCode + "','" + gygEntity.gxDuanCode + "','" +
                                gygEntity.gxID + "','" + gygEntity.gxName + "','" + gygEntity.Seq + "','" + gygEntity.description + "',:imaBuffers,:imaBuffers2,:imaBuffers3,'" +
                                gygEntity.videoUrl + "','" + gygEntity.remark + "') ";
                            lstSql.Add(strSql);

                            buttfes.Add(new BILBasic.DBA.dbFactory.imageButtfes()
                            {
                                imagePic1 = gygEntity.imagePic,
                                imagePic2 = gygEntity.imagePic2,
                                imagePic3 = gygEntity.imagePic3
                            });
                        }
                    }
                    //工艺操作对应限值-模板表
                    foreach (var gyl in model.lstGYLine_GYD)
                    {
                        foreach (var gylEntity in gyl.lstGY)
                        {
                            if (gylEntity.lstLimit != null && gylEntity.lstLimit.Count > 0)
                            {
                                foreach (var gygg in gylEntity.lstLimit)
                                {
                                    //插入Mes_GYLine_Limit_C
                                    voucherID = base.GetTableID("SEQ_MES_GYLINE_LIMIT_C");

                                    strSql = string.Empty;

                                    strSql = "insert into Mes_GYLine_Limit_C(Id,gyDetailID,gyLineID,xzID,type,xzName,xzRoleMax,xzRoleMin,QTY,Unit)" +
                                        "values('" + voucherID + "','" + gygg.gyDetailID + "','" + gygg.gyLineID + "','" + gygg.xzID + "','" + gygg.type + "','" + gygg.xzName + "','" +
                                        gygg.xzRoleMax + "','" + gygg.xzRoleMin + "','" + gygg.QTY + "','" + gygg.Unit + "') ";
                                    lstSql.Add(strSql);
                                }
                            }
                        }
                    }

                }

                if (lstSql == null || lstSql.Count == 0)
                {
                    ErrMsg = "no data!!!";
                    return false;
                }

                int i = dbFactory.ExecuteNonQueryListByCymNewImages3(lstSql, buttfes, ref ErrMsg);

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

        public bool GetAllProductGYLineList_C(Mes_ProductGYLineInfo model, ref List<Mes_ProductGYLineInfo> lst, ref string ErrMsg)
        {
            string strSql = string.Empty;

            strSql = "select * from Mes_GYLine_C where 1=1 and rownum < 100 ";
            string strAnd = " and ";

            #region where

            strSql += strAnd;
            strSql += " isMoban = " + model.isMoban;

            if (model.isDel != 0)
            {
                strSql += strAnd;
                strSql += " isDel = " + model.isDel;
            }

            if (model.isNew != 9)
            {
                strSql += strAnd;
                strSql += " isNew = " + model.isNew;
            }

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

            strSql += " order by id desc ";
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

                        entityGYLine.Modifyer = dr["Modifyer"].ToDBString();
                        entityGYLine.ModifyTime = dr["ModifyTime"].ToDateTime();

                        entityGYLine.processMode = dr["processMode"].ToDBString();
                        entityGYLine.lineType = dr["lineType"].ToDBString();

                        entityGYLine.isDel = dr["isDel"].ToInt32();

                        //add by cym 2019-2-19
                        entityGYLine.ApprovalProduct = dr["ApprovalProduct"].ToDBString();
                        entityGYLine.ApprovalQC = dr["ApprovalQC"].ToDBString();
                        entityGYLine.ApprovalSOP = dr["ApprovalSOP"].ToDBString();
                        entityGYLine.returnRemark = dr["returnRemark"].ToDBString();

                        lst.Add(entityGYLine);
                    }
                }

                if (lst.Count <= 0)
                {
                    ErrMsg = "no Mes_GYLine_C data!";
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

        public bool GetProductGYLineListDataBygxLineID_C(string gxLineID, ref Mes_ProductGYLineInfo entityGYLine, ref List<Mes_GYLine_GYInfo> lstDisGY, ref string ErrMsg)
        {
            string strSql = string.Empty;

            strSql = "select t.*,m.materialdesc from Mes_GYLine_C t inner join t_material m on t.cpcode=m.materialno where t.gyLineID='" + gxLineID + "'";

            try
            {
                //Mes_GYLine_C
                entityGYLine = new Mes_ProductGYLineInfo();

                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    if (dr.Read())
                    {
                        entityGYLine.ID = dr["ID"].ToInt32();
                        entityGYLine.gyLineID = dr["gyLineID"].ToDBString();
                        entityGYLine.machineType = dr["machineType"].ToDBString();
                        entityGYLine.bbID = dr["bbID"].ToDBString();
                        entityGYLine.gybbID = dr["gybbID"].ToDBString();
                        entityGYLine.cpCode = dr["cpCode"].ToDBString();
                        //entityGYLine.cpName = dr["cpName"].ToDBString();
                        entityGYLine.cpName = dr["materialdesc"].ToDBString();
                        entityGYLine.gyLineName = dr["gyLineName"].ToDBString();
                        entityGYLine.capacity = dr["capacity"].ToDecimalNull();
                        entityGYLine.cptype = dr["cptype"].ToDBString();
                        entityGYLine.Qty = dr["Qty"].ToInt32();
                        entityGYLine.isMoban = dr["isMoban"].ToInt32();
                        entityGYLine.isNew = dr["isNew"].ToInt32();
                        entityGYLine.remark = dr["remark"].ToDBString();

                        entityGYLine.Creater = dr["Creater"].ToDBString();
                        entityGYLine.CreateTime = dr["CreateTime"].ToDateTime();
                        entityGYLine.Modifyer = dr["Modifyer"].ToDBString();
                        entityGYLine.ModifyTime = dr["ModifyTime"].ToDateTimeNull();

                        //add by cym 2018-10-29 在新增工艺路线模板界面中，增加几个字段，这些字段变更时不走审批，编写人（可引用创建人）、审核人、业务审核人、生产审核、品保审核、批准人
                        entityGYLine.Auditor = dr["SHENHE"].ToDBString();
                        entityGYLine.ERPCreater = dr["YEWUSHENHE"].ToDBString();
                        entityGYLine.StrCreater = dr["SHENGCHANSHENHE"].ToDBString();
                        entityGYLine.StrModifyer = dr["PINBAOSHENHE"].ToDBString();
                        entityGYLine.VouUser = dr["PIZHUN"].ToDBString();

                        //add by cym 2018-11-30
                        entityGYLine.CompanyCode = dr["filePath1"].ToDBString(); ;
                        entityGYLine.DepartmentCode = dr["filePath2"].ToDBString(); ;

                        //add by cym 2019-1-2
                        entityGYLine.processMode = dr["processMode"].ToDBString();
                        entityGYLine.lineType = dr["lineType"].ToDBString();

                        //add by cym 2019-2-19
                        entityGYLine.ApprovalProduct = dr["ApprovalProduct"].ToDBString();
                        entityGYLine.ApprovalQC = dr["ApprovalQC"].ToDBString();
                        entityGYLine.ApprovalSOP = dr["ApprovalSOP"].ToDBString();
                        entityGYLine.returnRemark = dr["returnRemark"].ToDBString();

                        entityGYLine.isDel = dr["isDel"].ToInt32();

                        entityGYLine.lstGYLine_GYD = new List<Mes_ProductGYLine_GYDInfo>();
                    }
                    else
                    {
                        ErrMsg = "no Mes_GYLine_C data!";
                        return false;
                    }
                }

                //Mes_GYLine_GYD_C
                strSql = "select * from Mes_GYLine_GYD_C t where t.gyLineID='" + gxLineID + "'";

                List<Mes_ProductGYLine_GYDInfo> lst = new List<Mes_ProductGYLine_GYDInfo>();

                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        Mes_ProductGYLine_GYDInfo model = new Mes_ProductGYLine_GYDInfo();

                        model.ID = dr["ID"].ToInt32();
                        model.gxDuanCode = dr["gxDuanCode"].ToDBString();
                        model.gxDuanName = dr["gxDuanName"].ToDBString();
                        model.gyLineID = dr["gyLineID"].ToDBString();
                        model.MachineName = dr["MachineName"].ToDBString();
                        model.gxSeq = dr["gxSeq"].ToInt32();
                        model.MachineType = dr["MachineType"].ToDBString();

                        model.Qty = dr["Qty"].ToDecimal();
                        model.MouldCode = dr["MouldCode"].ToDBString();
                        model.MaterialDoc = dr["MouldName"].ToDBString();
                        model.gxDCode = dr["gxDCode"].ToDBString();

                        model.lstGY = new List<Mes_GYLine_GYInfo>();

                        lst.Add(model);
                    }
                }

                if (lst.Count <= 0)
                {
                    ErrMsg = "no Mes_GYLine_GYD_C data!";
                    return false;
                }

                //Mes_GYLine_GY_C
                strSql = "select * from Mes_GYLine_GY_C t where t.gyLineID='" + gxLineID + "'";

                List<Mes_GYLine_GYInfo> lstGY = new List<Mes_GYLine_GYInfo>();

                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        Mes_GYLine_GYInfo model = new Mes_GYLine_GYInfo();

                        model.gyDetailID = dr["gyDetailID"].ToDBString();
                        model.gyLineID = dr["gyLineID"].ToDBString();
                        model.gxDuanCode = dr["gxDuanCode"].ToDBString();
                        model.cpCode = dr["cpCode"].ToDBString();
                        model.gxID = dr["gxID"].ToDBString();
                        model.gxName = dr["gxName"].ToDBString();
                        model.Seq = dr["Seq"].ToInt32();

                        model.description = dr["description"].ToDBString();

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

                        model.lstLimit = new List<Mes_ProductGY_LimitInfo>();

                        lstGY.Add(model);
                    }
                }

                if (lstGY.Count <= 0)
                {
                    ErrMsg = "no Mes_GYLine_GY_C data!";
                    return false;
                }
                else
                {
                    lstDisGY = lstGY;
                }

                //Mes_GYLine_Limit_C
                strSql = "select * from Mes_GYLine_Limit_C t where t.gyLineID='" + gxLineID + "'";

                List<Mes_ProductGY_LimitInfo> lstLimit = new List<Mes_ProductGY_LimitInfo>();

                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    while (dr.Read())
                    {
                        Mes_ProductGY_LimitInfo model = new Mes_ProductGY_LimitInfo();

                        model.ID = dr["ID"].ToInt32();
                        model.xzID = dr["xzID"].ToDBString();
                        model.gyDetailID = dr["gyDetailID"].ToDBString();
                        model.gyLineID = dr["gyLineID"].ToDBString();
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
                    lstDisGY.ForEach(delegate(Mes_GYLine_GYInfo entityGy)
                    {
                        var templst = lstLimit.FindAll(t => t.gyDetailID == entityGy.gyDetailID);

                        if (templst != null && templst.Count > 0)
                        {
                            entityGy.lstLimit = templst;
                        }
                    });
                }

                lst.ForEach(delegate(Mes_ProductGYLine_GYDInfo entityGYD)
                {
                    var templst = lstGY.FindAll(t => t.gxDuanCode == entityGYD.gxDuanCode);
                    if (templst != null && templst.Count > 0)
                    {
                        templst.ForEach(t => t.HeaderID = entityGYD.gxSeq);
                        entityGYD.lstGY = templst;
                    }
                });

                //lstDisGY = lstGY;

                entityGYLine.lstGYLine_GYD = lst;

                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public bool DeleteProductGYLineInfo_C(string gxLineID, int isdel, ref string ErrMsg)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            try
            {
                //更新
                //strSql = "update Mes_GYLine_C set isDel = '" + isdel + "' where gyLineID = '" + gxLineID + "'";
                strSql = string.Format("delete Mes_GYLine_C where gyLineID = '{0}'", gxLineID);
                lstSql.Add(strSql);
                strSql = string.Format("delete Mes_GYLine_GYD_C where gyLineID = '{0}'", gxLineID);
                lstSql.Add(strSql);
                strSql = string.Format("delete Mes_GYLine_GY_C where gyLineID = '{0}'", gxLineID);
                lstSql.Add(strSql);
                strSql = string.Format("delete Mes_GYLine_Limit_C where gyLineID = '{0}'", gxLineID);
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

        public bool UpdateProductGYLineIsNewTo2BygxLineID(string gxLineID, ref string ErrMsg)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            try
            {
                //更新
                strSql = string.Format("update Mes_GYLine_C set isnew=2 where gyLineID = '{0}'", gxLineID);
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

        /// <summary>
        /// 更新审批
        /// </summary>
        /// <param name="gxLineID"></param>
        /// <param name="isdel"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool UpdateProductGYLineStatus_C(string gxLineID, int isdel, ref string ErrMsg)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            try
            {
                //更新State
                strSql = "update Mes_GYLine_C set isDel = '" + isdel + "' where gyLineID = '" + gxLineID + "'";
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

        /// <summary>
        /// 更新审批-New
        /// </summary>
        /// <param name="gxLineID"></param>
        /// <param name="ApprovalSOP"></param>
        /// <param name="ApprovalProduct"></param>
        /// <param name="ApprovalQC"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool UpdateProductGYLineStatusNew_C(string gxLineID, string ApprovalSOP, string ApprovalProduct, string ApprovalQC, ref string ErrMsg)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();
            var rt = false;

            try
            {
                if (!string.IsNullOrWhiteSpace(ApprovalSOP))
                {
                    //更新生技审批者
                    strSql = "update Mes_GYLine_C set ApprovalSOP = '" + ApprovalSOP + "' where gyLineID = '" + gxLineID + "'";
                    lstSql.Add(strSql);
                }
                else if (!string.IsNullOrWhiteSpace(ApprovalProduct))
                {
                    //更新生产审批者
                    strSql = "update Mes_GYLine_C set ApprovalProduct = '" + ApprovalProduct + "' where gyLineID = '" + gxLineID + "'";
                    lstSql.Add(strSql);
                }
                else if (!string.IsNullOrWhiteSpace(ApprovalQC))
                {
                    //更新品保审批者
                    strSql = "update Mes_GYLine_C set ApprovalQC = '" + ApprovalQC + "' where gyLineID = '" + gxLineID + "'";
                    lstSql.Add(strSql);
                }

                int i = dbFactory.ExecuteNonQueryList(lstSql, ref ErrMsg);

                //check
                strSql = "select count(*) as countQty from Mes_GYLine_C where ApprovalSOP is not null and ApprovalProduct is not null and ApprovalQC is not null and gyLineID = '" + gxLineID + "'";

                using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
                {
                    if (dr.Read())
                    {
                        //更新State 
                        if (dr["countQty"].ToInt32() != 0)
                        {
                            //更新State
                            strSql = "update Mes_GYLine_C set isDel = 2 where gyLineID = '" + gxLineID + "'";
                            lstSql.Add(strSql);

                            i = dbFactory.ExecuteNonQueryList(lstSql, ref ErrMsg);

                            if (i > 0)
                            {
                                rt = true;
                            }
                            else { return false; }
                        }
                        else
                        {
                            return true;
                        }
                    }
                }

                return rt;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新驳回原因
        /// </summary>
        /// <param name="gxLineID"></param>
        /// <param name="isdel"></param>
        /// <param name="returnRemark"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool UpdateProductGYLineStatus1AndReturn_C(string gxLineID, int isdel, string returnRemark, ref string ErrMsg)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            try
            {
                //更新
                strSql = "update Mes_GYLine_C set isDel = '" + isdel + "', returnRemark='" + returnRemark + "' where gyLineID = '" + gxLineID + "'";
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

        #endregion

    }
}
