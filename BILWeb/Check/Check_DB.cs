using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILBasic.DBA;
using BILWeb.Query;
using BILWeb.Check;
using BILWeb.Stock;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILWeb.OutBarCode;
using BILWeb.Print;
using BILWeb.Warehouse;
using BILWeb.OutStock;
using BILBasic.User;
using BILBasic.Basing;

namespace BILWeb.Query
{



    public class Check_DB
    {
        //查询盈亏分析,查询时候盘点表不要关联物料主数据获取信息，因为可能会有转过料的东西而带出不一样的据点信息

        public DbFactory dbFactory = new DbFactory(DbFactory.DbFactoryType.SQLSERVER);

        public bool GetCheckAnalyze(CheckAnalyze taskmo, ref DividPage dividpage, ref List<CheckAnalyze> lsttask, ref string strErrMsg)
        {
            if (dividpage == null) dividpage = new DividPage();
            lsttask = new List<CheckAnalyze>();
            try
            {

                string sql = "select  top 100 percent ROW_NUMBER() OVER(Order by INSERTTIME) AS PageRowNumber ,a.creater,a.status,a.materialno,a.materialdesc,a.strongholdcode,a.INSERTTIME,a.CHECKNO,a.AREANO,a.QTY,a.SERIALNO," +
                            "b.STRONGHOLDCODE as SSTRONGHOLDCODE,b.AREANO as SAREANO,b.MATERIALNO as SMATERIALNO,b.QTY as SQTY,b.MATERIALDESC as SMATERIALDESC ,b.SERIALNO as SSERIALNO," +
                            "case when isnull(a.QTY,0)<isnull(b.QTY,0) then '亏' when isnull(a.QTY,0)>isnull(b.QTY,0)  then '赢' else '平'" +
                            " end as remark,case when isnull(a.QTY,0)>isnull(b.QTY,0) then convert(nvarchar(50),isnull(a.QTY,0)-isnull(b.QTY,0)) else '' end as YQTY,case when isnull(a.QTY,0)<isnull(b.QTY,0) then convert(nvarchar(50),isnull(b.QTY,0)-isnull(a.QTY,0)) else '' end as KQTY "
                            + "from (select c.*,a.AREANO,m.materialno,m.materialdesc,m.strongholdcode from T_CHECKDETAILS c left join t_area a on c.areaid = a.id left join t_material m on m.id = c.materialid where CHECKNO = '" + taskmo.CHECKNO + "'";
                if (!string.IsNullOrEmpty(taskmo.AREANO))
                    sql += " and a.AREANO like '%" + taskmo.AREANO + "%'";
                if (!string.IsNullOrEmpty(taskmo.MATERIALNO))
                    sql += " and m.materialno like '%" + taskmo.MATERIALNO + "%'";
                sql += ") a full outer join " +

                    "(select s.areaid,a.AREANO,m.MATERIALNO,m.MATERIALDESC,s.SERIALNO,s.QTY,s.STRONGHOLDCODE" +
                      " from T_STOCK s left join t_area a on s.areaid = a.id left join t_material m on m.id = s.MATERIALNOID  where s.AREAID in(select AREAID from t_checkref where checkno = '" + taskmo.CHECKNO + "')";
                if (!string.IsNullOrEmpty(taskmo.AREANO))
                    sql += " and a.AREANO like '%" + taskmo.AREANO + "%'";
                if (!string.IsNullOrEmpty(taskmo.MATERIALNO))
                    sql += " and m.materialno like '%" + taskmo.MATERIALNO + "%'";
                sql += " )b" +
                        " on a.AREAID = b.AREAID " +
                        " and a.SERIALNO = b.SERIALNO where 1=1";

                if (taskmo.remark != "全部")
                    sql += " and case when isnull(a.QTY,0)<isnull(b.QTY,0) then '亏' when isnull(a.QTY,0)>isnull(b.QTY,0)  then '赢' else '平' end ='" + taskmo.remark + "' order by a.materialno";
                Common_FactoryDB factorydb = new Common_FactoryDB();
                using (IDataReader dr = factorydb.QueryByDividPage2(ref dividpage, sql))
                {
                    while (dr.Read())
                    {
                        lsttask.Add(GetCheckAnalyze_GetModelFromDataReader(dr));
                    }
                    dividpage.CurrentPageRecordCounts = lsttask.Count;
                }
                if (lsttask == null || lsttask.Count == 0)
                {
                    strErrMsg = "没有信息！";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }

        public CheckAnalyze GetCheckAnalyze_GetModelFromDataReader(IDataReader dr)
        {
            CheckAnalyze TMM = new CheckAnalyze();

            TMM.CHECKNO = (dr["CHECKNO"] ?? "").ToString();
            TMM.AREANO = (dr["AREANO"] ?? "").ToString();
            TMM.MATERIALDESC = (dr["MATERIALDESC"] ?? "").ToString();
            TMM.MATERIALNO = (dr["MATERIALNO"] ?? "").ToString();
            TMM.remark = (dr["remark"] ?? "").ToString();
            TMM.SAREANO = (dr["SAREANO"] ?? "").ToString();
            TMM.SERIALNO = (dr["SERIALNO"] ?? "").ToString();
            TMM.SMATERIALNO = (dr["SMATERIALNO"] ?? "").ToString();
            TMM.SSERIALNO = (dr["SSERIALNO"] ?? "").ToString();
            TMM.SMATERIALDESC = (dr["SMATERIALDESC"] ?? "").ToString();
            TMM.QTY = dr["QTY"].ToDecimalNull();
            TMM.SQTY = dr["SQTY"].ToDecimalNull();
            TMM.YQTY = (dr["YQTY"] ?? "").ToString();
            TMM.KQTY = (dr["KQTY"] ?? "").ToString();
            TMM.STRONGHOLDCODE = dr["STRONGHOLDCODE"].ToDBString();
            TMM.SSTRONGHOLDCODE = dr["SSTRONGHOLDCODE"].ToDBString();
            TMM.status = dr["status"].ToInt32();
            TMM.Creater = dr["Creater"].ToDBString();
            return TMM;
        }





        public bool GetCheckAnalyze2(CheckAnalyze taskmo, ref DividPage dividpage, ref List<CheckAnalyze> lsttask, ref string strErrMsg)
        {
            if (dividpage == null) dividpage = new DividPage();
            lsttask = new List<CheckAnalyze>();
            try
            {

                string sql = "select  top 100 percent ROW_NUMBER() OVER(Order by INSERTTIME) AS PageRowNumber,a.materialno,a.materialdesc,a.strongholdcode,a.AREANO,a.QTY," +
                            "b.STRONGHOLDCODE as SSTRONGHOLDCODE,b.AREANO as SAREANO,b.MATERIALNO as SMATERIALNO,b.QTY as SQTY,b.MATERIALDESC as SMATERIALDESC ," +
                            "(case when isnull(a.QTY,0)<isnull(b.QTY,0) then '亏' when  isnull(a.QTY,0)>isnull(b.QTY,0)then '赢' else '平' end)" +
                            " as remark,case when (case when isnull(a.QTY,0)<isnull(b.QTY,0) then '亏' when  isnull(a.QTY,0)>isnull(b.QTY,0)then '赢' else '平' end) ='赢' then  to_char((isnull(a.QTY,0)-isnull(b.QTY,0))) else '' end as YQTY," +
                            "case when (case when isnull(a.QTY,0)<isnull(b.QTY,0) then '亏' when  isnull(a.QTY,0)>isnull(b.QTY,0)then '赢' else '平' end) ='亏' then to_char((isnull(b.QTY,0)-isnull(a.QTY,0))) else '' end as KQTY from " +
                            "(select c.materialid,c.AREAID,sum(c.QTY) as qty,min(c.inserttime) as inserttime,a.AREANO,m.materialno,m.materialdesc,m.strongholdcode from T_CHECKDETAILS c left join t_area a on c.areaid = a.id left join t_material m on m.id = c.materialid where CHECKNO = '" + taskmo.CHECKNO + "'";
                if (taskmo.AREANO != "")
                    sql += " and a.AREANO like '%" + taskmo.AREANO + "%'";
                if (taskmo.MATERIALNO != "")
                    sql += " and m.materialno like '%" + taskmo.MATERIALNO + "%'";
                sql += " group by c.materialid,c.AREAID,a.AREANO,m.materialno,m.materialdesc,m.strongholdcode) a full outer join (select s.materialnoid,s.AREAID,a.AREANO,m.MATERIALNO,m.MATERIALDESC,s.STRONGHOLDCODE,sum(s.QTY) as qty" +
                      " from T_STOCK s left join t_area a on s.areaid = a.id left join t_material m on m.id = s.MATERIALNOID  where s.AREAID in(select AREAID from t_checkref where checkno = '" + taskmo.CHECKNO + "')";
                if (taskmo.AREANO != "")
                    sql += " and a.AREANO like '%" + taskmo.AREANO + "%'";
                if (taskmo.MATERIALNO != "")
                    sql += " and m.materialno like '%" + taskmo.MATERIALNO + "%'";
                sql += " group by s.materialnoid,s.AREAID,a.AREANO,m.MATERIALNO,m.MATERIALDESC,s.STRONGHOLDCODE)b" +
                        " on a.AREAID = b.AREAID and a.materialid =b.materialnoid " +
                        " where 1=1";

                if (taskmo.remark != "全部")
                    sql += " and case when isnull(a.QTY,0)<isnull(b.QTY,0) then '亏' when  isnull(a.QTY,0)>isnull(b.QTY,0)then '赢' else '平' end ='" + taskmo.remark + "'";
                using (IDataReader dr = Common_DB.QueryByDividPage2(ref dividpage, sql))
                {
                    while (dr.Read())
                    {
                        lsttask.Add(GetCheckAnalyze_GetModelFromDataReader2(dr));
                    }
                    dividpage.CurrentPageRecordCounts = lsttask.Count;
                }
                if (lsttask == null || lsttask.Count == 0)
                {
                    strErrMsg = "没有信息！";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }

        public CheckAnalyze GetCheckAnalyze_GetModelFromDataReader2(IDataReader dr)
        {
            CheckAnalyze TMM = new CheckAnalyze();

            //TMM.CHECKNO = (dr["CHECKNO"] ?? "").ToString();
            TMM.AREANO = (dr["AREANO"] ?? "").ToString();
            TMM.MATERIALDESC = (dr["MATERIALDESC"] ?? "").ToString();
            TMM.MATERIALNO = (dr["MATERIALNO"] ?? "").ToString();
            TMM.remark = (dr["remark"] ?? "").ToString();
            TMM.SAREANO = (dr["SAREANO"] ?? "").ToString();
            //TMM.SERIALNO = (dr["SERIALNO"] ?? "").ToString();
            TMM.SMATERIALNO = (dr["SMATERIALNO"] ?? "").ToString();
            //TMM.SSERIALNO = (dr["SSERIALNO"] ?? "").ToString();
            TMM.SMATERIALDESC = (dr["SMATERIALDESC"] ?? "").ToString();
            TMM.QTY = dr["QTY"].ToDecimalNull();
            TMM.SQTY = dr["SQTY"].ToDecimalNull();
            TMM.YQTY = (dr["YQTY"] ?? "").ToString();
            TMM.KQTY = (dr["KQTY"] ?? "").ToString();
            TMM.STRONGHOLDCODE = dr["STRONGHOLDCODE"].ToDBString();
            TMM.SSTRONGHOLDCODE = dr["SSTRONGHOLDCODE"].ToDBString();
            return TMM;
        }




        //查询盘点单表头
        public bool GetCheckInfo(Check_Model taskmo, ref DividPage dividpage, ref List<Check_Model> lsttask, ref string strErrMsg)
        {
            if (dividpage == null) dividpage = new DividPage();
            lsttask = new List<Check_Model>();
            try
            {
                Common_FactoryDB commonfunc = new Common_FactoryDB();
                using (IDataReader dr = commonfunc.QueryByDividPage(ref dividpage, "T_CHECK", CHECK_GetFilterSql(taskmo), " * ", "Order by ID Desc"))
                {
                    while (dr.Read())
                    {
                        lsttask.Add(CHECK_GetModelFromDataReader(dr));
                    }
                    dividpage.CurrentPageRecordCounts = lsttask.Count;
                }
                if (lsttask == null || lsttask.Count == 0)
                {
                    strErrMsg = "没有信息！";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }
        private string CHECK_GetFilterSql(Check_Model mo)
        {
            string sql = "where ISDEL = 1 ";



            if (!string.IsNullOrEmpty(mo.CHECKTYPE) && mo.CHECKTYPE != "全部")
            {
                sql += " and CHECKTYPE ='" + mo.CHECKTYPE + "'";
            }

            if (!string.IsNullOrEmpty(mo.CREATER))
                sql += " and CREATER like '%" + mo.CREATER + "%'";

            if (!string.IsNullOrEmpty(mo.REMARKS))
            {
                sql += " and REMARKS like '%" + mo.REMARKS + "%'";
            }

            if (!string.IsNullOrEmpty(mo.CHECKSTATUS) && mo.CHECKSTATUS != "全部")
            {
                sql += " and CHECKSTATUS ='" + mo.CHECKSTATUS + "'";
            }

            if (mo.begintime != null)
            {
                sql += "and CREATETIME>='" + mo.begintime + "' ";
            }
            if (mo.endtime != null)
            {
                sql += "and CREATETIME<='" + mo.endtime + "' ";
            }

            return sql;

        }
        public Check_Model CHECK_GetModelFromDataReader(IDataReader dr)
        {
            Check_Model TMM = new Check_Model();
            if (dr["CBEGINTIME"] is DBNull)
                TMM.CBEGINTIME = null;
            else
                TMM.CBEGINTIME = Convert.ToDateTime(dr["BEGINTIME"]);
            TMM.ID = Convert.ToInt32(dr["ID"]);
            TMM.CHECKDESC = (dr["CHECKDESC"] ?? "").ToString();
            TMM.ischeck = false;
            TMM.CHECKNO = (dr["CHECKNO"] ?? "").ToString();
            TMM.CHECKSTATUS = (dr["CHECKSTATUS"] ?? "").ToString();
            TMM.CHECKTYPE = (dr["CHECKTYPE"] ?? "").ToString();
            TMM.CREATER = (dr["CREATER"] ?? "").ToString();

            if (dr["CREATETIME"] is DBNull)
                TMM.CREATETIME = null;
            else
                TMM.CREATETIME = Convert.ToDateTime(dr["CREATETIME"]);

            if (dr["CDONETIME"] is DBNull)
                TMM.CDONETIME = null;
            else
                TMM.CDONETIME = Convert.ToDateTime(dr["DONETIME"]);

            TMM.REMARKS = (dr["REMARKS"] ?? "").ToString();
            return TMM;
        }

        //查询库位信息,只查没有被锁住的库位，多张盘点单不能使用重复库位，不然解锁是会把别人的库位解除
        public bool GetCheckArea(int hl, string areano, string houseno, string warehouseno, ref List<CheckArea_Model> list)
        {
            string sql = "select a.*,v.houseno,v.warehouseno from t_area a left join v_area v on v.id = a.id  where  a.AREASTATUS=1 and a.ISDEL = 1 ";

            if (!string.IsNullOrEmpty(areano))
            {
                if (areano.Contains(","))
                {
                    string[] areanos = areano.Split(',');
                    sql += " and (";
                    foreach (string a in areanos)
                    {
                        sql += " v.areano like '%" + a + "%' or";
                    }
                    sql = sql.Substring(0, sql.Length - 2);
                    sql += ")";
                }
                else
                {
                    sql += " and v.areano like '%" + areano + "%'";
                }

            }

            if (hl != 0)
            {
                sql += " and a.heightarea =" + hl + "";
            }


            if (houseno != "" && !houseno.Contains(','))
                sql += " and v.houseno ='" + houseno + "'";
            else if (houseno != "" && houseno.Contains(','))
            {
                Query_Func.ChangeQuery(ref houseno);
                sql += " and v.houseno in(" + houseno + ")";
            }


            if (!string.IsNullOrEmpty(warehouseno))
            {
                sql += " and v.warehouseno ='" + warehouseno + "'";
            }

            sql += "order by a.areano";
            DataTable dt = dbFactory.ExecuteDataSet(System.Data.CommandType.Text, sql).Tables[0];
            list = ModelConvertHelper<CheckArea_Model>.ConvertToModel(dt);
            if (list.Count == 0)
                return false;
            else
                return true;
        }

        public string SaveCheckAndroid(string json1, string json2)
        {
            BaseMessage_Model<string> bm = new BaseMessage_Model<string>();
            string j = "";
            string ErrMsg = "";
            Check_Model model = Check_Func.DeserializeJsonToObject<Check_Model>(json1);
            model.CHECKTYPE = "货位";
            model.CHECKSTATUS = "新建";
            List<CheckArea_Model> list = Check_Func.DeserializeJsonToList<CheckArea_Model>(json2);
            //去重复
            list = list.Where((x, i) => list.FindIndex(z => z.ID == x.ID) == i).ToList();
            if (list.Count == 0)
            {
                bm.HeaderStatus = "E";
                bm.Message = "没有扫描库位";
                j = Check_Func.SerializeObject(bm);
                return j;
            }
            if (SaveCheck(model, list, ref ErrMsg))
            {
                bm.HeaderStatus = "S";
                bm.Message = "保存成功";
                j = Check_Func.SerializeObject(bm);
                return j;
            }
            else
            {
                bm.HeaderStatus = "E";
                bm.Message = ErrMsg;
                j = Check_Func.SerializeObject(bm);
                return j;
            }
        }


        //保存盘点单
        public bool SaveCheck(Check_Model model, List<CheckArea_Model> list, ref string ErrMsg)
        {
            try
            {
                //int id = Check_Func.GetSeqID("SEQ_CHECK_ID");
                int id = GetTableIDBySqlServer("T_CHECK");
                List<string> sqls = new List<string>();
                string sqlhead = "SET IDENTITY_INSERT T_CHECK on ;insert into T_CHECK(ID,CHECKNO,CHECKTYPE,CHECKDESC,CHECKSTATUS,REMARKS,ISDEL,CREATER,CREATETIME) VALUES" +
                    "(" + id + ",'" + model.CHECKNO + "','" + model.CHECKTYPE + "','" + model.CHECKDESC + "','" + model.CHECKSTATUS + "','" + model.REMARKS + "',1,'" + model.CREATER + "',getdate()) SET IDENTITY_INSERT T_CHECK off ";
                sqls.Add(sqlhead);
                string sql = "";
                foreach (var item in list)
                {
                    sql = "insert into T_checkref (CHECKID,AREANO,CHECKNO,AREAID) values(" + id + ",'" + item.AREANO + "','" + model.CHECKNO + "'," + item.ID + ")";
                    sqls.Add(sql);
                    //锁库位
                    sql = "update t_area set areastatus = 2 where id = " + item.ID + "";
                    sqls.Add(sql);
                }


                int i = dbFactory.ExecuteNonQueryList(sqls);
                if (i == -2)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }

        }

        public int GetTableIDBySqlServer(string strSeq)
        {
            try
            {
                string strSql = "SELECT IDENT_CURRENT('" + strSeq + "')";
                int ID = dbFactory.ExecuteScalar(CommandType.Text, strSql).ToInt32();
                strSql = "DBCC   CHECKIDENT   ('" + strSeq + "',   RESEED, " + (ID + 1) + ")";
                dbFactory.ExecuteNonQuery2(CommandType.Text, strSql).ToInt32();
                return ID;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }


        public bool SaveCheck2(Check_Model model, List<T_StockInfoEX> list, ref string ErrMsg)
        {
            try
            {
                Check_Func CF = new Check_Func();
                string checkno = CF.GetTableID("SEQ_CHECK_NO");
                int id = Check_Func.GetSeqID("SEQ_CHECK_ID");
                List<string> sqls = new List<string>();
                string sqlhead = "insert into T_CHECK(ID,CHECKNO,CHECKTYPE,CHECKDESC,CHECKSTATUS,REMARKS,ISDEL,CREATER,CREATETIME) VALUES" +
                    "(" + id + ",'" + checkno + "','" + "明盘" + "','" + model.CHECKDESC + "','" + model.CHECKSTATUS + "','" + model.REMARKS + "',1,'" + model.CREATER + "',getdate())";
                sqls.Add(sqlhead);
                string sql = "";
                //合并重复项
                list = list
                   .GroupBy(x => new { x.MaterialNo, x.WarehouseNo, x.HouseNo, x.AreaNo, x.StrongHoldCode, x.BatchNo })
                   .Select(group => new T_StockInfoEX
                   {
                       MaterialNo = group.Key.MaterialNo,
                       WarehouseNo = group.Key.WarehouseNo,
                       HouseNo = group.Key.HouseNo,
                       AreaNo = group.Key.AreaNo,
                       StrongHoldCode = group.Key.StrongHoldCode,
                       BatchNo = group.Key.BatchNo,
                       Qty = group.Sum(p => p.Qty),
                   }).ToList();
                foreach (var item in list)
                {
                    sql = "insert into T_checkrefstock (voucherno,MaterialNo,WAREHOUSENO,HouseNo,AreaNo,qty,STRONGHOLDCODE,BATCHNO,sqty) " +
                        "values('" + checkno + "','" + item.MaterialNo + "','" + item.WarehouseNo + "','" + item.HouseNo + "','" + item.AreaNo + "'," + item.Qty + ",'" + item.StrongHoldCode + "','" + item.BatchNo + "',0)";
                    sqls.Add(sql);
                }
                int i = dbFactory.ExecuteNonQueryList(sqls);
                if (i == -2)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }

        }

        //创建复盘单
        public bool ReCheck(string checkno, string peo, ref string ErrMsg)
        {
            try
            {
                List<Check_Model> heads = new List<Check_Model>();
                List<CheckArea_Model> bodys = new List<CheckArea_Model>();
                string sql = "select * from T_CHECK where CHECKNO ='" + checkno + "'";
                DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                heads = ModelConvertHelper<Check_Model>.ConvertToModel(dt);
                Check_Model head = heads[0];
                string cno = head.CHECKNO;
                if (!cno.Contains('R'))
                {
                    head.CHECKNO = head.CHECKNO + "R1";
                }
                else
                {
                    string last = cno.Split('R')[1];
                    int nlast = Convert.ToInt32(last) + 1;
                    head.CHECKNO = cno.Split('R')[0] + "R" + nlast;
                }
                head.CHECKDESC = cno + "的复盘单据";
                head.CREATER = peo;
                head.CHECKSTATUS = "新建";

                //创建库位
                //insert into T_checkref (CHECKID,AREANO,CHECKNO,AREAID) values(" + id + ",'" + item.AREANO + "','" + model.CHECKNO + "',"+item.ID+")";
                sql = "select * from T_checkref where CHECKNO ='" + checkno + "'";
                using (var dr = dbFactory.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        CheckArea_Model b = new CheckArea_Model();
                        b.AREANO = dr["AREANO"].ToDBString();
                        b.ID = dr["AREAID"].ToInt32();
                        bodys.Add(b);
                    }
                }
                bool res = SaveCheck(head, bodys, ref ErrMsg);
                if (ErrMsg.Contains("违反唯一约束"))
                {
                    ErrMsg = "该盘点单已经复盘，请选择该盘点的复盘单再复盘";
                }
                if (res)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }

        }

        //查询盘点单依据
        public bool GetCheckRefInfo(CheckRef_Model model, ref DividPage dividpage, ref List<CheckRef_Model> lsttask, ref string strErrMsg)
        {
            if (dividpage == null) dividpage = new DividPage();
            lsttask = new List<CheckRef_Model>();
            try
            {
                using (IDataReader dr = Common_DB.QueryByDividPage(ref dividpage, "V_CHECKREF", CHECKREF_GetFilterSql(model), " * ", "order by CHECKID"))
                {
                    while (dr.Read())
                    {
                        lsttask.Add(CHECKREF_GetModelFromDataReader(dr));
                    }
                    dividpage.CurrentPageRecordCounts = lsttask.Count;
                }
                if (lsttask == null || lsttask.Count == 0)
                {
                    strErrMsg = "没有信息！";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }
        private string CHECKREF_GetFilterSql(CheckRef_Model mo)
        {
            string sql = "where 1 = 1 and CHECKNO = '" + mo.CHECKNO + "' ";


            if (mo.AREANO != "")
            {
                sql += " and AREANO like '%" + mo.AREANO + "%' ";
            }
            return sql;

        }
        public CheckRef_Model CHECKREF_GetModelFromDataReader(IDataReader dr)
        {
            CheckRef_Model TMM = new CheckRef_Model();
            TMM.CHECKNO = (dr["CHECKNO"] ?? "").ToString();
            TMM.AREANO = (dr["AREANO"] ?? "").ToString();
            TMM.areaid = dr["areaid"].ToInt32();
            TMM.houseno = dr["houseno"].ToDBString();
            TMM.warehouseno = dr["warehouseno"].ToDBString();
            return TMM;
        }

        //type=0删除 type=1终止 type=2完成
        public bool DelCloCheck(string checkno, int type, ref string ErrMsg, string username = "")
        {
            try
            {
                List<string> sqls = new List<string>();
                string sql = "";
                if (type == 0)
                {
                    //判断如果是明盘的话，有没有扫描过，没有就可以删除
                    sql = "select count(1) from T_CHECKREFSERIAL where voucherno = '" + checkno + "'";
                    int cn = dbFactory.ExecuteScalar(CommandType.Text, sql).ToInt32();
                    if (cn != 0)
                    {
                        ErrMsg = "明盘单据已经开始扫描，无法删除，可以终止";
                        return false;
                    }
                    sql = "update t_check set isdel = 2 where checkno = '" + checkno + "'";
                    sqls.Add(sql);
                }
                if (type == 1)
                {
                    sql = "update t_check set CHECKSTATUS = '终止' where checkno = '" + checkno + "'";
                    sqls.Add(sql);
                }
                if (type == 2)
                {
                    //判断是否扫描过
                    //string stringres = GetCheckDetail(checkno);
                    //BaseMessage_Model<List<CheckDet_Model>> model = Check_Func.DeserializeJsonToObject<BaseMessage_Model<List<CheckDet_Model>>>(stringres);
                    //if (model.HeaderStatus == "E")
                    //{
                    //    ErrMsg = "没有盘点信息";
                    //    return false;
                    //}

                    //盘点明细表链接库位视图和托盘表，当完全盘盈时插入要使用这些字段
                    sql = "select a.creater,a.CHECKNO,a.AREAID,a.MATERIALID,a.inserttime,a.status,x.areano,x.houseid,x.warehouseid,a.SERIALNO,a.qty," +
                                "o.BARCODE,o.MATERIALNO,o.MATERIALDESC,o.BATCHNO,o.unit,o.STRONGHOLDCODE,o.STRONGHOLDNAME,o.EDATE,o.SUPCODE,o.SUPNAME,o.PRODUCTDATE,o.SUPPRDBATCH,o.SUPPRDDATE,o.ean,p.PALLETNO," +
                                "b.qty as sqty,b.STRONGHOLDCODE,b.AREAID as SAREAID,b.MATERIALNOID as SMATERIALNOID,b.SERIALNO as SSERIALNO,b.status as sstatus ,b.ean as sean ," +
                                "case when isnull(a.QTY,0)<isnull(b.QTY,0) then '亏' when isnull(a.QTY,0)>isnull(b.QTY,0)  then '赢' else '平'" +
                                " end as remark from (select * from T_CHECKDETAILS where CHECKNO = '" + checkno + "') a full outer join (select AREAID,MATERIALNOID,SERIALNO,STRONGHOLDCODE,qty,status,ean " +
                                " from T_STOCK where AREAID in(select AREAID from t_checkref where checkno = '" + checkno + "'))b" +
                            " on a.AREAID = b.AREAID " +
                            " and a.SERIALNO = b.SERIALNO left join (select * from v_AREA where ISDEL = 1) x on a.areaid = x.id" +
                            " left join (select * from T_OUTBARCODE where  BARCODETYPE = 3 or BARCODETYPE = 5 or BARCODETYPE = 6) o on a.SERIALNO = o.SERIALNO " +
                            " left join T_PALLETDETAIL p on p.SERIALNO = a.SERIALNO and p.PALLETTYPE = 1  order by remark desc";
                    DataTable dt = dbFactory.ExecuteDataSet(System.Data.CommandType.Text, sql).Tables[0];
                    List<CheckAnalyze> listshow = ModelConvertHelper<CheckAnalyze>.ConvertToModel(dt);
                    if (listshow.Count == 0)
                    {
                        ErrMsg = "没有获取盈亏处理信息";
                        return false;
                    }
                    //处理结果
                    foreach (var item in listshow)
                    {
                        //把操作人改成扫描人
                        username = item.Creater;
                        if (item.remark == "平")
                        {
                            //修改质检状态
                            if (item.status == item.sstatus)
                                continue;
                            else
                            {
                                sql = OutTrans(username, sql, item, 101, item.SSERIALNO);
                                sqls.Add(sql);
                                sql = "update T_stock set MODIFYER = '" + username + "',MODIFYTIME = getdate(),status = 3 where AREAID = " + item.SAREAID + "  and SERIALNO = '" + item.SSERIALNO + "'";
                                sqls.Add(sql);
                                sql = InTrans(username, sql, item, 101, item.SSERIALNO);
                                sqls.Add(sql);
                            }
                        }

                        if (item.remark == "亏")
                        {
                            //如果完全亏
                            if (String.IsNullOrEmpty(item.SERIALNO))
                            {
                                sql = OutTrans(username, sql, item, 101, item.SSERIALNO);
                                sqls.Add(sql);
                                sql = "delete from T_stock where AREAID = " + item.SAREAID + " and SERIALNO = '" + item.SSERIALNO + "'";
                                sqls.Add(sql);
                            }
                            //数量亏
                            else
                            {
                                sql = OutTrans(username, sql, item, 101, item.SSERIALNO);
                                sqls.Add(sql);
                                sql = "update T_stock set MODIFYER = '" + username + "',MODIFYTIME = getdate(),qty = " + item.QTY + ",status = " + item.status + " where AREAID = " + item.SAREAID + "  and SERIALNO = '" + item.SSERIALNO + "'";
                                sqls.Add(sql);
                                sql = InTrans(username, sql, item, 101, item.SSERIALNO);
                                sqls.Add(sql);
                            }
                        }
                        if (item.remark == "赢")
                        {
                            //完全赢
                            if (String.IsNullOrEmpty(item.SSERIALNO))
                            {
                                //如果当前序列号在其他库位里存在，则把其他库位的序列号和库位修改为当前盘点的
                                sql = "select count(1) from T_stock where SERIALNO = '" + item.SERIALNO + "'";
                                int o = Convert.ToInt32(dbFactory.ExecuteScalar(CommandType.Text, sql));
                                if (o > 0)
                                {
                                    sql = OutTrans(username, sql, item, 101, item.SERIALNO);
                                    sqls.Add(sql);
                                    sql = "update T_stock set MODIFYER = '" + username + "',MODIFYTIME = getdate(),qty = " + item.QTY + ",AREAID = " + item.AREAID + ",WAREHOUSEID =" + item.warehouseid + ",HOUSEID =" + item.houseid + ",  status = 3 where SERIALNO = '" + item.SERIALNO + "'";
                                    sqls.Add(sql);
                                    sql = InTrans(username, sql, item, 101, item.SERIALNO);
                                    sqls.Add(sql);
                                }
                                else
                                {
                                    DateTime date1 = Convert.ToDateTime("2119-10-10 12:40:59"); 
                                   
                                    //库存中没有，全部从条码表中拉数据，再从托盘表中拉托盘
                                    sql = "insert into T_STOCK (BARCODE,SERIALNO,MATERIALNO,MATERIALDESC,MATERIALNOID,WAREHOUSEID," +
                                       "HOUSEID,AREAID,QTY,STATUS,ISDEL,CREATER,CREATETIME,BATCHNO,UNIT,PALLETNO,STRONGHOLDCODE,STRONGHOLDNAME" +
                                       ",COMPANYCODE,EDATE,SUPCODE,SUPNAME,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,RECEIVESTATUS,ISLIMITSTOCK,EAN)" +
                                       "VALUES('" + item.BARCODE + "','" + item.SERIALNO + "','" + item.MATERIALNO + "','" + item.MATERIALDESC + "'" +
                                       "," + item.MATERIALID + "," + item.warehouseid + "," + item.houseid + "," + item.AREAID +
                                       "," + item.QTY + ",3,1,'" + username + "',getdate(),'" + item.BatchNo + "','" + item.unit + "','" + item.PALLETNO + "','" + item.STRONGHOLDCODE + "','" + item.STRONGHOLDNAME + "'" +
                                       ",'10',"+ (item.EDATE < date1 ? "'2119-10-10'" : ("'"+ item.EDATE.ToString("yyyy-MM-dd")) + "'") + ",'" + item.SUPCODE + "','" + item.SUPNAME + "'" +
                                       ",null,null,null,2,2,'" + item.ean + "')";
                                    sqls.Add(sql);
                                    sql = InTrans(username, sql, item, 101, item.SERIALNO);
                                    sqls.Add(sql);
                                }
                            }
                            //数量赢
                            else
                            {
                                sql = OutTrans(username, sql, item, 101, item.SSERIALNO);
                                sqls.Add(sql);
                                sql = "update T_stock set MODIFYER = '" + username + "',MODIFYTIME = getdate(),qty = " + item.QTY + ",status = 3 where AREAID = " + item.SAREAID + "  and SERIALNO = '" + item.SSERIALNO + "'";
                                sqls.Add(sql);
                                sql = InTrans(username, sql, item, 101, item.SSERIALNO);
                                sqls.Add(sql);
                            }
                        }
                    }
                    //修改盘点单状态
                    sql = "update t_check set CHECKSTATUS = '完成' where checkno = '" + checkno + "'";
                    sqls.Add(sql);
                }
                //解锁库位
                List<CheckArea_Model> listarea = new List<CheckArea_Model>();
                bool res = GetAreanobyCheckno(ref listarea, checkno);
                if (res)
                {
                    foreach (var item in listarea)
                    {
                        sql = "update t_area set AREASTATUS = 1 where id = " + item.ID + "";
                        sqls.Add(sql);
                    }
                }
                sqls.Add("update T_stock set T_stock.barcodetype = t_outbarcode.barcodetype from t_outbarcode, T_stock where t_outbarcode.serialno = T_stock.serialno and T_stock.barcodetype is null");
                
                //执行
                int i = dbFactory.ExecuteNonQueryList(sqls, ref ErrMsg);
                if (i == -2)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }

        }
        //type=0删除 type=1终止 type=2完成
        public bool DelCloCheck_MsTest(string checkno, int type, ref string ErrMsg, string username = "")
        {
            try
            {
                string strPost = "";
                List<string> sqls = new List<string>();
                string sql = "";
                //ymh 比较整个库存 赢：调入盘盈仓 亏：调拨单
                //ymh汇总的数据
                string sqlall = "select '" + checkno + "' as checkno,ss as SERIALNO, materialno,MATERIALDESC,MATERIALID,areaid,areano,batchno,sum(yqty)-sum(kqty) as qty from(  " +
                                "select isnull(a.sserialno, a.serialno) as ss, isnull(a.yqty, 0) as yqty, isnull(a.kqty, 0) as kqty, isnull(a.smaterialno, a.materialno) as materialno,isnull(a.SMATERIALDESC, a.SMATERIALDESC) as MATERIALDESC,isnull(a.SMATERIALNOid, a.SMATERIALNOid) as MATERIALID,areaid,SAREANO as areano,batchno from( " +
                                "select ROW_NUMBER() OVER(Order by INSERTTIME) AS PageRowNumber, a.creater, a.status, a.materialno, a.materialdesc, a.strongholdcode, a.INSERTTIME, a.CHECKNO, a.AREANO, a.QTY, a.SERIALNO,  " +
                                "b.STRONGHOLDCODE as SSTRONGHOLDCODE, b.AREANO as SAREANO, b.MATERIALNO as SMATERIALNO,b.id as SMATERIALNOid,b.areaid,b.batchno, b.QTY as SQTY, b.MATERIALDESC as SMATERIALDESC, b.SERIALNO as SSERIALNO, " +
                                "case when isnull(a.QTY, 0) < isnull(b.QTY, 0) then '亏' when isnull(a.QTY, 0) > isnull(b.QTY, 0)  then '赢' else '平' end as remark,case when isnull(a.QTY,0)> isnull(b.QTY, 0) then to_char(isnull(a.QTY,0)-isnull(b.QTY, 0)) else '' end as YQTY,case when isnull(a.QTY,0)< isnull(b.QTY, 0) then to_char(isnull(b.QTY,0)-isnull(a.QTY, 0)) else '' end as KQTY " +
                                "from(select c.*, a.AREANO, m.materialno, m.materialdesc, m.strongholdcode from T_CHECKDETAILS c left join t_area a on c.areaid = a.id left join t_material m on m.id = c.materialid where CHECKNO = '" + checkno + "') a " +
                                "full outer join(select s.areaid, a.AREANO, m.id, m.MATERIALNO, m.MATERIALDESC, s.SERIALNO, s.QTY, s.STRONGHOLDCODE , s.batchno " +
                                "from T_STOCK s left join t_area a on s.areaid = a.id " +
                                "left join t_material m on m.id = s.MATERIALNOID " +
                                "where s.AREAID in (select AREAID from t_checkref where checkno = '"+ checkno + "') )b on a.AREAID = b.AREAID  and a.SERIALNO = b.SERIALNO where 1 = 1 ) a " +
                                ") b group by ss, materialno, MATERIALDESC, MATERIALID, areaid, areano, batchno having sum(yqty)-sum(kqty) != 0";
                DataTable dtAll = dbFactory.ExecuteDataSet(System.Data.CommandType.Text, sqlall).Tables[0];
                List<CheckAnalyze> listshowall = ModelConvertHelper<CheckAnalyze>.ConvertToModel(dtAll);
                List<CheckAnalyze> listYing = listshowall.FindAll(t => t.QTY > 0);
                List<CheckAnalyze> listKui = listshowall.FindAll(t => t.QTY < 0);
                foreach (var item in listYing)
                {
                    sql = OutTransCheck(username, sql, item.QTY.ToString(), 101, item.SERIALNO);
                    sqls.Add(sql);
                }
                List<CheckDet_Model> models = new List<CheckDet_Model>();
                foreach (var item in listKui)
                {
                    CheckDet_Model model = new CheckDet_Model()
                    {
                        CHECKNO = item.CHECKNO,
                        AREANO = item.AREANO,
                        AREAID = item.AREAID,
                        MATERIALID = item.MATERIALID,
                        MATERIALNO = item.MATERIALNO,
                        MATERIALDESC = item.MATERIALDESC,
                        SERIALNO = item.SERIALNO,
                        BATCHNO = item.BatchNo,
                        QTY = 0-item.QTY.ToDecimal()
                    };
                    models.Add(model);
                }
                if (models.Count > 0)
                {
                    T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
                    UserModel userModel = new UserModel();
                    //盘点生成差异
                    //if (tfunc.SaveT_ChangeMaterial(userModel, models, ref strPost))
                    //{

                    //}

                }
                //执行
                int i = dbFactory.ExecuteNonQueryList(sqls, ref ErrMsg);
                ErrMsg = ErrMsg + strPost;
                if (i == -2)
                    return false;
                else
                    return true;

            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }

        }

        //type=0删除 type=1终止 type=2完成
        public bool DelCloCheck_Ms(string checkno, int type, ref string ErrMsg, string username = "")
        {
            try
            {
                List<string> sqls = new List<string>();
                string sql = "";
                if (type == 0)
                {
                    //判断如果是明盘的话，有没有扫描过，没有就可以删除
                    sql = "select count(1) from T_CHECKREFSERIAL where voucherno = '" + checkno + "'";
                    int cn = dbFactory.ExecuteScalar(CommandType.Text, sql).ToInt32();
                    if (cn != 0)
                    {
                        ErrMsg = "明盘单据已经开始扫描，无法删除，可以终止";
                        return false;
                    }
                    sql = "update t_check set isdel = 2 where checkno = '" + checkno + "'";
                    sqls.Add(sql);
                }
                if (type == 1)
                {
                    sql = "update t_check set CHECKSTATUS = '终止' where checkno = '" + checkno + "'";
                    sqls.Add(sql);
                }
                if (type == 2)
                {
                    //ymh 比较整个库存 赢：调入盘盈仓 亏：调拨单
                    //ymh汇总的数据
                    string sqlall = "select ss as SERIALNO, materialno, sum(yqty)-sum(kqty) as qty from( " +
                    "select isnull(a.sserialno, a.serialno) as ss, isnull(a.yqty, 0) as yqty, isnull(a.kqty, 0) as kqty, isnull(a.smaterialno, a.materialno) as materialno from( " +
                    "select ROW_NUMBER() OVER(Order by INSERTTIME) AS PageRowNumber, a.creater, a.status, a.materialno, a.materialdesc, a.strongholdcode, a.INSERTTIME, a.CHECKNO, a.AREANO, a.QTY, a.SERIALNO, " +
                    "b.STRONGHOLDCODE as SSTRONGHOLDCODE, b.AREANO as SAREANO, b.MATERIALNO as SMATERIALNO, b.QTY as SQTY, b.MATERIALDESC as SMATERIALDESC, b.SERIALNO as SSERIALNO," +
                    "case when isnull(a.QTY, 0) < isnull(b.QTY, 0) then '亏' when isnull(a.QTY, 0) > isnull(b.QTY, 0)  then '赢' else '平' end as remark,case when isnull(a.QTY,0)> isnull(b.QTY, 0) then to_char(isnull(a.QTY,0)-isnull(b.QTY, 0)) else '' end as YQTY,case when isnull(a.QTY,0)< isnull(b.QTY, 0) then to_char(isnull(b.QTY,0)-isnull(a.QTY, 0)) else '' end as KQTY " +
                    "from(select c.*, a.AREANO, m.materialno, m.materialdesc, m.strongholdcode from T_CHECKDETAILS c left join t_area a on c.areaid = a.id left join t_material m on m.id = c.materialid where CHECKNO = '" + checkno + "') a full outer join(select s.areaid, a.AREANO, m.MATERIALNO, m.MATERIALDESC, s.SERIALNO, s.QTY, s.STRONGHOLDCODE " +
                    "from T_STOCK s left join t_area a on s.areaid = a.id left join t_material m on m.id = s.MATERIALNOID  where s.AREAID in (select AREAID from t_checkref where checkno = '" + checkno + "') )b on a.AREAID = b.AREAID  and a.SERIALNO = b.SERIALNO where 1 = 1 " +
                    ") a " +
                    ") b group by ss, materialno having sum(yqty)-sum(kqty) != 0 ";
                    DataTable dtAll = dbFactory.ExecuteDataSet(System.Data.CommandType.Text, sqlall).Tables[0];
                    List<CheckAnalyze> listshowall = ModelConvertHelper<CheckAnalyze>.ConvertToModel(dtAll);
                    List<CheckAnalyze> listYing = listshowall.FindAll(t => t.QTY > 0);
                    List<CheckAnalyze> listKui = listshowall.FindAll(t => t.QTY < 0);
                    foreach (var item in listKui)
                    {
                        sql = OutTransCheck(username, sql, item.QTY.ToString(), 101, item.SSERIALNO);
                        sqls.Add(sql);
                    }
                    List<CheckDet_Model> models = new List<CheckDet_Model>();
                    foreach (var item in listYing)
                    {
                        CheckDet_Model model = new CheckDet_Model()
                        {
                            CHECKNO = item.CHECKNO,
                            AREANO = item.AREANO,
                            AREAID = item.AREAID,
                            MATERIALID = item.MATERIALID,
                            MATERIALNO = item.MATERIALNO,
                            MATERIALDESC = item.SMATERIALDESC,
                            SERIALNO = item.SERIALNO,
                            BATCHNO = item.BatchNo,
                            QTY = item.QTY.ToDecimal(),
                            VoucherType = 101
                        };
                        models.Add(model);
                    }
                    if (models.Count > 0)
                    {
                        T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
                        UserModel userModel = new UserModel();
                        string strError = "";
                        //盘点生成差异
                        //if (tfunc.SaveT_ChangeMaterial(userModel, models, ref strError))
                        //{

                        //}
                        //public bool SaveT_ChangeMaterial(UserModel userModel, List<CheckDet_Model> modelList, ref string strError)盘点生成差异

                    }



                    //盘点明细表链接库位视图和托盘表，当完全盘盈时插入要使用这些字段
                    sql = "select a.creater,a.CHECKNO,a.AREAID,a.MATERIALID,a.inserttime,a.status,x.areano,x.houseid,x.warehouseid,a.SERIALNO,a.qty," +
                                "o.BARCODE,o.MATERIALNO,o.MATERIALDESC,o.BATCHNO,o.unit,o.STRONGHOLDCODE,o.STRONGHOLDNAME,o.EDATE,o.SUPCODE,o.SUPNAME,o.PRODUCTDATE,o.SUPPRDBATCH,o.SUPPRDDATE,o.ean,p.PALLETNO," +
                                "b.qty as sqty,b.STRONGHOLDCODE,b.AREAID as SAREAID,b.MATERIALNOID as SMATERIALNOID,b.SERIALNO as SSERIALNO,b.status as sstatus,b.ean as sean ," +
                                "case when isnull(a.QTY,0)<isnull(b.QTY,0) then '亏' when isnull(a.QTY,0)>isnull(b.QTY,0)  then '赢' else '平'" +
                                " end as remark from (select * from T_CHECKDETAILS where CHECKNO = '" + checkno + "') a full outer join (select AREAID,MATERIALNOID,SERIALNO,STRONGHOLDCODE,qty,status,ean" +
                                " from T_STOCK where AREAID in(select AREAID from t_checkref where checkno = '" + checkno + "'))b" +
                            " on a.AREAID = b.AREAID " +
                            " and a.SERIALNO = b.SERIALNO left join (select * from v_AREA where ISDEL = 1) x on a.areaid = x.id" +
                            " left join T_OUTBARCODE o on a.SERIALNO = o.SERIALNO and o.BARCODETYPE = 1 " +
                            " left join T_PALLETDETAIL p on p.SERIALNO = a.SERIALNO and p.PALLETTYPE = 1  order by remark desc";
                    DataTable dt = dbFactory.ExecuteDataSet(System.Data.CommandType.Text, sql).Tables[0];
                    List<CheckAnalyze> listshow = ModelConvertHelper<CheckAnalyze>.ConvertToModel(dt);
                    if (listshow.Count == 0)
                    {
                        ErrMsg = "没有获取盈亏处理信息";
                        return false;
                    }
                    //处理结果
                    foreach (var item in listshow)
                    {
                        //把操作人改成扫描人
                        username = item.Creater;
                        if (item.remark == "平")
                        {
                            //修改质检状态
                            if (item.status == item.sstatus)
                                continue;
                            else
                            {
                                sql = OutTrans(username, sql, item, 101, item.SSERIALNO);
                                sqls.Add(sql);
                                sql = "update T_stock set MODIFYER = '" + username + "',MODIFYTIME = getdate(),status = " + item.status + " where AREAID = " + item.SAREAID + "  and SERIALNO = '" + item.SSERIALNO + "'";
                                sqls.Add(sql);
                                sql = InTrans(username, sql, item, 101, item.SSERIALNO);
                                sqls.Add(sql);
                            }
                        }

                        if (item.remark == "亏")
                        {
                            //如果完全亏
                            if (String.IsNullOrEmpty(item.SERIALNO))
                            {
                                sql = OutTrans(username, sql, item, 101, item.SSERIALNO);
                                sqls.Add(sql);
                                sql = "delete from T_stock where AREAID = " + item.SAREAID + " and SERIALNO = '" + item.SSERIALNO + "'";
                                sqls.Add(sql);
                            }
                            //数量亏
                            else
                            {
                                sql = OutTrans(username, sql, item, 101, item.SSERIALNO);
                                sqls.Add(sql);
                                sql = "update T_stock set MODIFYER = '" + username + "',MODIFYTIME = getdate(),qty = " + item.QTY + ",status = " + item.status + " where AREAID = " + item.SAREAID + "  and SERIALNO = '" + item.SSERIALNO + "'";
                                sqls.Add(sql);
                                sql = InTrans(username, sql, item, 101, item.SSERIALNO);
                                sqls.Add(sql);
                            }
                        }
                        if (item.remark == "赢")
                        {
                            //完全赢
                            if (String.IsNullOrEmpty(item.SSERIALNO))
                            {
                                //如果当前序列号在其他库位里存在，则把其他库位的序列号和库位修改为当前盘点的
                                sql = "select count(1) from T_stock where SERIALNO = '" + item.SERIALNO + "'";
                                int o = Convert.ToInt32(dbFactory.ExecuteScalar(CommandType.Text, sql));
                                if (o > 0)
                                {
                                    sql = OutTrans(username, sql, item, 101, item.SERIALNO);
                                    sqls.Add(sql);
                                    sql = "update T_stock set MODIFYER = '" + username + "',MODIFYTIME = getdate(),qty = " + item.QTY + ",AREAID = " + item.AREAID + ",WAREHOUSEID =" + item.warehouseid + ",HOUSEID =" + item.houseid + ",  status = " + item.status + " where SERIALNO = '" + item.SERIALNO + "'";
                                    sqls.Add(sql);
                                    sql = InTrans(username, sql, item, 101, item.SERIALNO);
                                    sqls.Add(sql);
                                }
                                else
                                {
                                    //库存中没有，全部从条码表中拉数据，再从托盘表中拉托盘
                                    sql = "insert into T_STOCK (BARCODE,SERIALNO,MATERIALNO,MATERIALDESC,MATERIALNOID,WAREHOUSEID," +
                                       "HOUSEID,AREAID,QTY,STATUS,ISDEL,CREATER,CREATETIME,BATCHNO,UNIT,PALLETNO,STRONGHOLDCODE,STRONGHOLDNAME" +
                                       ",COMPANYCODE,EDATE,SUPCODE,SUPNAME,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,RECEIVESTATUS,ISLIMITSTOCK,EAN)" +
                                       "VALUES('" + item.BARCODE + "','" + item.SERIALNO + "','" + item.MATERIALNO + "','" + item.MATERIALDESC + "'" +
                                       "," + item.MATERIALID + "," + item.warehouseid + "," + item.houseid + "," + item.AREAID +
                                       "," + item.QTY + "," + item.status + ",1,'" + username + "',getdate(),'" + item.BatchNo + "','" + item.unit + "','" + item.PALLETNO + "','" + item.STRONGHOLDCODE + "','" + item.STRONGHOLDNAME + "'" +
                                       ",'10','" + item.EDATE.ToString("yyyy-MM-dd") + "','" + item.SUPCODE + "','" + item.SUPNAME + "'" +
                                       ",'" + item.PRODUCTDATE.ToString("yyyy-MM-dd") + "','" + item.SUPPRDBATCH + "','" + item.SUPPRDDATE.ToString("yyyy-MM-dd") + "',2,2,'" + item.ean + "')";
                                    sqls.Add(sql);
                                    sql = InTrans(username, sql, item, 101, item.SERIALNO);
                                    sqls.Add(sql);
                                }
                            }
                            //数量赢
                            else
                            {
                                sql = OutTrans(username, sql, item, 101, item.SSERIALNO);
                                sqls.Add(sql);
                                sql = "update T_stock set MODIFYER = '" + username + "',MODIFYTIME = getdate(),qty = " + item.QTY + ",status = " + item.status + " where AREAID = " + item.SAREAID + "  and SERIALNO = '" + item.SSERIALNO + "'";
                                sqls.Add(sql);
                                sql = InTrans(username, sql, item, 101, item.SSERIALNO);
                                sqls.Add(sql);
                            }
                        }
                    }
                    //修改盘点单状态
                    sql = "update t_check set CHECKSTATUS = '完成' where checkno = '" + checkno + "'";
                    sqls.Add(sql);
                }
                //解锁库位
                List<CheckArea_Model> listarea = new List<CheckArea_Model>();
                bool res = GetAreanobyCheckno(ref listarea, checkno);
                if (res)
                {
                    foreach (var item in listarea)
                    {
                        sql = "update t_area set AREASTATUS = 1 where id = " + item.ID + "";
                        sqls.Add(sql);
                    }
                }

                //执行
                int i = dbFactory.ExecuteNonQueryList(sqls, ref ErrMsg);
                if (i == -2)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }

        }

        ////type=0删除 type=1终止 type=2完成
        //public bool DelCloCheck_Ms(string checkno, int type, ref string ErrMsg, string username = "")
        //{
        //    try
        //    {
        //        List<string> sqls = new List<string>();
        //        string sql = "";
        //        if (type == 0)
        //        {
        //            //判断如果是明盘的话，有没有扫描过，没有就可以删除
        //            sql = "select count(1) from T_CHECKREFSERIAL where voucherno = '" + checkno + "'";
        //            int cn = dbFactory.ExecuteScalar(CommandType.Text, sql).ToInt32();
        //            if (cn != 0)
        //            {
        //                ErrMsg = "明盘单据已经开始扫描，无法删除，可以终止";
        //                return false;
        //            }
        //            sql = "update t_check set isdel = 2 where checkno = '" + checkno + "'";
        //            sqls.Add(sql);
        //        }
        //        if (type == 1)
        //        {
        //            sql = "update t_check set CHECKSTATUS = '终止' where checkno = '" + checkno + "'";
        //            sqls.Add(sql);
        //        }
        //        if (type == 2)
        //        {
        //            //判断是否扫描过
        //            //string stringres = GetCheckDetail(checkno);
        //            //BaseMessage_Model<List<CheckDet_Model>> model = Check_Func.DeserializeJsonToObject<BaseMessage_Model<List<CheckDet_Model>>>(stringres);
        //            //if (model.HeaderStatus == "E")
        //            //{
        //            //    ErrMsg = "没有盘点信息";
        //            //    return false;
        //            //}
        //            //ymh 比较整个库存 赢：调入盘盈仓 亏：调拨单
        //            //ymh汇总的数据
        //            string sqlall = "select ss as SERIALNO, materialno, sum(yqty)-sum(kqty) as qty from( " +
        //            "select isnull(a.sserialno, a.serialno) as ss, isnull(a.yqty, 0) as yqty, isnull(a.kqty, 0) as kqty, isnull(a.smaterialno, a.materialno) as materialno from( " +
        //            "select ROW_NUMBER() OVER(Order by INSERTTIME) AS PageRowNumber, a.creater, a.status, a.materialno, a.materialdesc, a.strongholdcode, a.INSERTTIME, a.CHECKNO, a.AREANO, a.QTY, a.SERIALNO, " +
        //            "b.STRONGHOLDCODE as SSTRONGHOLDCODE, b.AREANO as SAREANO, b.MATERIALNO as SMATERIALNO, b.QTY as SQTY, b.MATERIALDESC as SMATERIALDESC, b.SERIALNO as SSERIALNO," +
        //            "case when isnull(a.QTY, 0) < isnull(b.QTY, 0) then '亏' when isnull(a.QTY, 0) > isnull(b.QTY, 0)  then '赢' else '平' end as remark,case when isnull(a.QTY,0)> isnull(b.QTY, 0) then to_char(isnull(a.QTY,0)-isnull(b.QTY, 0)) else '' end as YQTY,case when isnull(a.QTY,0)< isnull(b.QTY, 0) then to_char(isnull(b.QTY,0)-isnull(a.QTY, 0)) else '' end as KQTY " +
        //            "from(select c.*, a.AREANO, m.materialno, m.materialdesc, m.strongholdcode from T_CHECKDETAILS c left join t_area a on c.areaid = a.id left join t_material m on m.id = c.materialid where CHECKNO = '" + checkno + "') a full outer join(select s.areaid, a.AREANO, m.MATERIALNO, m.MATERIALDESC, s.SERIALNO, s.QTY, s.STRONGHOLDCODE " +
        //            "from T_STOCK s left join t_area a on s.areaid = a.id left join t_material m on m.id = s.MATERIALNOID  where s.AREAID in (select AREAID from t_checkref where checkno = '" + checkno + "') )b on a.AREAID = b.AREAID  and a.SERIALNO = b.SERIALNO where 1 = 1 " +
        //            ") a " +
        //            ") b group by ss, materialno having sum(yqty)-sum(kqty) != 0 ";
        //            DataTable dtAll = dbFactory.ExecuteDataSet(System.Data.CommandType.Text, sqlall).Tables[0];
        //            List<CheckAnalyze> listshowall = ModelConvertHelper<CheckAnalyze>.ConvertToModel(dtAll);

        //            //盘点明细表链接库位视图和托盘表，当完全盘盈时插入要使用这些字段
        //            sql = "select a.creater,a.CHECKNO,a.AREAID,a.MATERIALID,a.inserttime,a.status,x.areano,x.houseid,x.warehouseid,a.SERIALNO,a.qty," +
        //                        "o.BARCODE,o.MATERIALNO,o.MATERIALDESC,o.BATCHNO,o.unit,o.STRONGHOLDCODE,o.STRONGHOLDNAME,o.EDATE,o.SUPCODE,o.SUPNAME,o.PRODUCTDATE,o.SUPPRDBATCH,o.SUPPRDDATE,p.PALLETNO," +
        //                        "b.qty as sqty,b.STRONGHOLDCODE,b.AREAID as SAREAID,b.MATERIALNOID as SMATERIALNOID,b.SERIALNO as SSERIALNO,b.status as sstatus," +
        //                        "case when isnull(a.QTY,0)<isnull(b.QTY,0) then '亏' when isnull(a.QTY,0)>isnull(b.QTY,0)  then '赢' else '平'" +
        //                        " end as remark from (select * from T_CHECKDETAILS where CHECKNO = '" + checkno + "') a full outer join (select AREAID,MATERIALNOID,SERIALNO,STRONGHOLDCODE,qty,status" +
        //                        " from T_STOCK where AREAID in(select AREAID from t_checkref where checkno = '" + checkno + "'))b" +
        //                    " on a.AREAID = b.AREAID " +
        //                    " and a.SERIALNO = b.SERIALNO left join (select * from v_AREA where ISDEL = 1) x on a.areaid = x.id" +
        //                    " left join T_OUTBARCODE o on a.SERIALNO = o.SERIALNO and o.BARCODETYPE = 1 " +
        //                    " left join T_PALLETDETAIL p on p.SERIALNO = a.SERIALNO and p.PALLETTYPE = 1  order by remark desc";
        //            DataTable dt = dbFactory.ExecuteDataSet(System.Data.CommandType.Text, sql).Tables[0];
        //            List<CheckAnalyze> listshow = ModelConvertHelper<CheckAnalyze>.ConvertToModel(dt);
        //            if (listshow.Count == 0)
        //            {
        //                ErrMsg = "没有获取盈亏处理信息";
        //                return false;
        //            }
        //            //处理结果
        //            foreach (var item in listshow)
        //            {
        //                //把操作人改成扫描人
        //                username = item.Creater;
        //                if (item.remark == "平")
        //                {
        //                    //修改质检状态
        //                    if (item.status == item.sstatus)
        //                        continue;
        //                    else
        //                    {
        //                        sql = OutTrans(username, sql, item, 101, item.SSERIALNO);
        //                        sqls.Add(sql);
        //                        sql = "update T_stock set MODIFYER = '" + username + "',MODIFYTIME = SYSDATE,status = " + item.status + " where AREAID = " + item.SAREAID + "  and SERIALNO = '" + item.SSERIALNO + "'";
        //                        sqls.Add(sql);
        //                        sql = InTrans(username, sql, item, 101, item.SSERIALNO);
        //                        sqls.Add(sql);
        //                    }
        //                }

        //                if (item.remark == "亏")
        //                {
        //                    //ymh判断条码是否在listshowall中
        //                    decimal opterationQty = 0;
        //                    getOpterationQty(item.SSERIALNO, Convert.ToDecimal(item.QTY), listshowall, item.remark, ref opterationQty);
        //                    if (item.QTY==opterationQty)
        //                    {
        //                        sql = OutTrans(username, sql, item, 101, item.SSERIALNO);
        //                        sqls.Add(sql);
        //                        sql = "delete from T_stock where AREAID = " + item.SAREAID + " and SERIALNO = '" + item.SSERIALNO + "'";
        //                        sqls.Add(sql);
        //                    }
        //                    else
        //                    {
        //                        sql = OutTrans(username, sql, item, 101, item.SSERIALNO);
        //                        sqls.Add(sql);
        //                        sql = "update T_stock set MODIFYER = '" + username + "',MODIFYTIME = SYSDATE,qty = " + opterationQty + ",status = " + item.status + " where AREAID = " + item.SAREAID + "  and SERIALNO = '" + item.SSERIALNO + "'";
        //                        sqls.Add(sql);
        //                        sql = InTrans(username, sql, item, 101, item.SSERIALNO);
        //                        sqls.Add(sql);
        //                    }

        //                    ////如果完全亏
        //                    //if (String.IsNullOrEmpty(item.SERIALNO))
        //                    //{
        //                    //    sql = OutTrans(username, sql, item, 101, item.SSERIALNO);
        //                    //    sqls.Add(sql);
        //                    //    sql = "delete from T_stock where AREAID = " + item.SAREAID + " and SERIALNO = '" + item.SSERIALNO + "'";
        //                    //    sqls.Add(sql);
        //                    //}
        //                    ////数量亏
        //                    //else
        //                    //{
        //                    //    sql = OutTrans(username, sql, item, 101, item.SSERIALNO);
        //                    //    sqls.Add(sql);
        //                    //    sql = "update T_stock set MODIFYER = '" + username + "',MODIFYTIME = SYSDATE,qty = " + item.QTY + ",status = " + item.status + " where AREAID = " + item.SAREAID + "  and SERIALNO = '" + item.SSERIALNO + "'";
        //                    //    sqls.Add(sql);
        //                    //    sql = InTrans(username, sql, item, 101, item.SSERIALNO);
        //                    //    sqls.Add(sql);
        //                    //}
        //                }
        //                if (item.remark == "赢")
        //                {
        //                    //ymh判断条码是否在listshowall中
        //                    decimal opterationQty = 0;
        //                    getOpterationQty(item.SSERIALNO, Convert.ToDecimal(item.QTY), listshowall, item.remark, ref opterationQty);
        //                    if (item.QTY == opterationQty)
        //                    {
        //                        //库存中没有，全部从条码表中拉数据，再从托盘表中拉托盘
        //                        sql = "insert into T_STOCK (ID,BARCODE,SERIALNO,MATERIALNO,MATERIALDESC,MATERIALNOID,WAREHOUSEID," +
        //                           "HOUSEID,AREAID,QTY,STATUS,ISDEL,CREATER,CREATETIME,BATCHNO,UNIT,PALLETNO,STRONGHOLDCODE,STRONGHOLDNAME" +
        //                           ",COMPANYCODE,EDATE,SUPCODE,SUPNAME,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,RECEIVESTATUS,ISLIMITSTOCK)" +
        //                           "VALUES(seq_stock_id.nextval,'" + item.BARCODE + "','" + item.SERIALNO + "','" + item.MATERIALNO + "','" + item.MATERIALDESC + "'" +
        //                           "," + item.MATERIALID + "," + item.warehouseid + "," + item.houseid + "," + item.AREAID +
        //                           "," + opterationQty + "," + item.status + ",1,'" + username + "',SYSDATE,'" + item.BatchNo + "','" + item.unit + "','" + item.PALLETNO + "','" + item.STRONGHOLDCODE + "','" + item.STRONGHOLDNAME + "'" +
        //                           ",'10',to_date('" + item.EDATE.ToString("yyyy-MM-dd") + "','YYYY/MM/DD'),'" + item.SUPCODE + "','" + item.SUPNAME + "'" +
        //                           ",to_date('" + item.PRODUCTDATE.ToString("yyyy-MM-dd") + "','YYYY/MM/DD'),'" + item.SUPPRDBATCH + "',to_date('" + item.SUPPRDDATE.ToString("yyyy-MM-dd") + "','YYYY/MM/DD'),2,2)";
        //                        sqls.Add(sql);
        //                        sql = InTrans(username, sql, item, 101, item.SERIALNO);
        //                        sqls.Add(sql);
        //                    }
        //                    else
        //                    {
        //                        sql = OutTrans(username, sql, item, 101, item.SSERIALNO);
        //                        sqls.Add(sql);
        //                        sql = "update T_stock set MODIFYER = '" + username + "',MODIFYTIME = SYSDATE,qty = " + item.QTY + ",status = " + item.status + " where AREAID = " + item.SAREAID + "  and SERIALNO = '" + item.SSERIALNO + "'";
        //                        sqls.Add(sql);
        //                        sql = InTrans(username, sql, item, 101, item.SSERIALNO);
        //                        sqls.Add(sql);
        //                    }




        //                    ////完全赢
        //                    //if (String.IsNullOrEmpty(item.SSERIALNO))
        //                    //{
        //                    //    //如果当前序列号在其他库位里存在，则把其他库位的序列号和库位修改为当前盘点的
        //                    //    sql = "select count(1) from T_stock where SERIALNO = '" + item.SERIALNO + "'";
        //                    //    int o = Convert.ToInt32(dbFactory.ExecuteScalar(CommandType.Text, sql));
        //                    //    if (o > 0)
        //                    //    {
        //                    //        sql = OutTrans(username, sql, item, 101, item.SERIALNO);
        //                    //        sqls.Add(sql);
        //                    //        sql = "update T_stock set MODIFYER = '" + username + "',MODIFYTIME = SYSDATE,qty = " + item.QTY + ",AREAID = " + item.AREAID + ",WAREHOUSEID =" + item.warehouseid + ",HOUSEID =" + item.houseid + ",  status = " + item.status + " where SERIALNO = '" + item.SERIALNO + "'";
        //                    //        sqls.Add(sql);
        //                    //        sql = InTrans(username, sql, item, 101, item.SERIALNO);
        //                    //        sqls.Add(sql);
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        //库存中没有，全部从条码表中拉数据，再从托盘表中拉托盘
        //                    //        sql = "insert into T_STOCK (ID,BARCODE,SERIALNO,MATERIALNO,MATERIALDESC,MATERIALNOID,WAREHOUSEID," +
        //                    //           "HOUSEID,AREAID,QTY,STATUS,ISDEL,CREATER,CREATETIME,BATCHNO,UNIT,PALLETNO,STRONGHOLDCODE,STRONGHOLDNAME" +
        //                    //           ",COMPANYCODE,EDATE,SUPCODE,SUPNAME,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,RECEIVESTATUS,ISLIMITSTOCK)" +
        //                    //           "VALUES(seq_stock_id.nextval,'" + item.BARCODE + "','" + item.SERIALNO + "','" + item.MATERIALNO + "','" + item.MATERIALDESC + "'" +
        //                    //           "," + item.MATERIALID + "," + item.warehouseid + "," + item.houseid + "," + item.AREAID +
        //                    //           "," + item.QTY + "," + item.status + ",1,'" + username + "',SYSDATE,'" + item.BatchNo + "','" + item.unit + "','" + item.PALLETNO + "','" + item.STRONGHOLDCODE + "','" + item.STRONGHOLDNAME + "'" +
        //                    //           ",'10',to_date('" + item.EDATE.ToString("yyyy-MM-dd") + "','YYYY/MM/DD'),'" + item.SUPCODE + "','" + item.SUPNAME + "'" +
        //                    //           ",to_date('" + item.PRODUCTDATE.ToString("yyyy-MM-dd") + "','YYYY/MM/DD'),'" + item.SUPPRDBATCH + "',to_date('" + item.SUPPRDDATE.ToString("yyyy-MM-dd") + "','YYYY/MM/DD'),2,2)";
        //                    //        sqls.Add(sql);
        //                    //        sql = InTrans(username, sql, item, 101, item.SERIALNO);
        //                    //        sqls.Add(sql);
        //                    //    }
        //                    //}
        //                    ////数量赢
        //                    //else
        //                    //{
        //                    //    sql = OutTrans(username, sql, item, 101, item.SSERIALNO);
        //                    //    sqls.Add(sql);
        //                    //    sql = "update T_stock set MODIFYER = '" + username + "',MODIFYTIME = SYSDATE,qty = " + item.QTY + ",status = " + item.status + " where AREAID = " + item.SAREAID + "  and SERIALNO = '" + item.SSERIALNO + "'";
        //                    //    sqls.Add(sql);
        //                    //    sql = InTrans(username, sql, item, 101, item.SSERIALNO);
        //                    //    sqls.Add(sql);
        //                    //}
        //                }
        //            }
        //            //修改盘点单状态
        //            sql = "update t_check set CHECKSTATUS = '完成' where checkno = '" + checkno + "'";
        //            sqls.Add(sql);
        //        }
        //        //解锁库位
        //        List<CheckArea_Model> listarea = new List<CheckArea_Model>();
        //        bool res = GetAreanobyCheckno(ref listarea, checkno);
        //        if (res)
        //        {
        //            foreach (var item in listarea)
        //            {
        //                sql = "update t_area set AREASTATUS = 1 where id = " + item.ID + "";
        //                sqls.Add(sql);
        //            }
        //        }

        //        //执行
        //        int i = dbFactory.ExecuteNonQueryList(sqls, ref ErrMsg);
        //        if (i == -2)
        //            return false;
        //        else
        //            return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrMsg = ex.ToString();
        //        return false;
        //    }

        //}

        public void getOpterationQty(string serialno, decimal qty, List<CheckAnalyze> listshowAll, string strtype, ref decimal opterationQty)
        {
            for (int i = 0; i < listshowAll.Count; i++)
            {
                if (listshowAll[i].SERIALNO == serialno)
                {
                    decimal analyzeQty = Convert.ToDecimal(listshowAll[i].QTY);
                    if (strtype == "亏")
                    {
                        opterationQty = analyzeQty > 0 ? qty : qty + analyzeQty;
                    }
                    if (strtype == "赢")
                    {
                        opterationQty = analyzeQty < 0 ? qty : qty - analyzeQty;
                    }
                }
            }
            opterationQty = qty;
        }


        private static string OutTrans(string username, string sql, CheckAnalyze item, int vouchertype, string serialno)
        {
            sql = "insert into T_TASKTRANS (VOUCHERTYPE,MATERIALNO,MATERIALDESC,CREATETIME,UNIT,CREATER,MATERIALNOID,STRONGHOLDNAME,COMPANYCODE" +
                " ,STRONGHOLDCODE,BATCHNO,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,SERIALNO" +
                " ,TOWAREHOUSEID ,TOHOUSEID ,TOAREAID " +
                "  , BARCODE ,TASKTYPE ,QTY ,EDATE ,FROMWAREHOUSEID , FROMHOUSEID ,FROMAREAID,status)" +
                " select " + vouchertype + ",f.materialno,f.materialdesc,getdate(),f.unit,'" + username + "',f.materialnoid,f.strongholdname,10, " +
                "  f.strongholdcode,f.batchno,f.productdate,f.supprdbatch,f.supprddate,f.serialno" +
                " ,'','','',f.barcode,2,f.qty,f.edate,f.warehouseid,f.houseid,f.areaid,f.status from t_stock f where f.serialno = '" + serialno + "'";
            return sql;
        }

        private static string InTrans(string username, string sql, CheckAnalyze item, int vouchertype, string serialno)
        {
            sql = "insert into T_TASKTRANS (VOUCHERTYPE,MATERIALNO,MATERIALDESC,CREATETIME,UNIT,CREATER,MATERIALNOID,STRONGHOLDNAME,COMPANYCODE" +
                           " ,STRONGHOLDCODE,BATCHNO,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,SERIALNO" +
                            " ,TOWAREHOUSEID ,TOHOUSEID ,TOAREAID " +
                          "  , BARCODE ,TASKTYPE ,QTY ,EDATE ,FROMWAREHOUSEID ,FROMHOUSEID ,FROMAREAID,status)" +
                            "select " + vouchertype + ",f.materialno,f.materialdesc,getdate(),f.unit,'" + username + "',f.materialnoid,f.strongholdname,10, " +
                          "  f.strongholdcode,f.batchno,f.productdate,f.supprdbatch,f.supprddate,f.serialno" +
                           " ,f.warehouseid,f.houseid,f.areaid,f.barcode,1,f.qty,f.edate,'','','',f.status from t_stock f where  f.serialno = '" + serialno + "'";
            return sql;
        }


        private static string OutTransCheck(string username, string sql, string QTY, int vouchertype, string serialno)
        {
            sql = "insert into T_TASKTRANS (VOUCHERTYPE,MATERIALNO,MATERIALDESC,CREATETIME,UNIT,CREATER,MATERIALNOID,STRONGHOLDNAME,COMPANYCODE" +
                " ,STRONGHOLDCODE,BATCHNO,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,SERIALNO" +
                " ,TOWAREHOUSEID ,TOHOUSEID ,TOAREAID " +
                "  , BARCODE ,TASKTYPE ,QTY ,EDATE ,FROMWAREHOUSEID , FROMHOUSEID ,FROMAREAID,status)" +
                " select " + vouchertype + ",f.materialno,f.materialdesc,getdate(),f.unit,'" + username + "',f.materialnoid,f.strongholdname,10, " +
                "  f.strongholdcode,f.batchno,f.productdate,f.supprdbatch,f.supprddate,f.serialno" +
                " ,'','','',f.barcode,22," + QTY + ",f.edate,f.warehouseid,f.houseid,f.areaid,f.status from t_stock f where f.serialno = '" + serialno + "'";
            return sql;
        }


        public bool GetAreanobyCheckno(ref List<CheckArea_Model> list, string checkno)
        {
            list = new List<CheckArea_Model>();
            string sql = "select * from t_checkref where checkno = '" + checkno + "'";
            using (IDataReader dr = dbFactory.ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    CheckArea_Model m = new CheckArea_Model();
                    m.AREANO = dr["AREANO"].ToString();
                    m.ID = dr["areaid"].ToInt32();
                    list.Add(m);
                }
            }
            if (list.Count == 0)
                return false;
            else
                return true;
        }


        //查看盘点前库存内容

        public bool GetCheckStock(string checkno, ref DividPage dividpage, ref List<CheckAnalyze> lsttask, ref string strErrMsg)
        {
            if (dividpage == null) dividpage = new DividPage();
            lsttask = new List<CheckAnalyze>();
            try
            {

                string sql = "select ROW_NUMBER() OVER(Order by a.areano) AS PageRowNumber ,a.areano,a.checkno,b.STRONGHOLDCODE,b.BATCHNO,b.serialno,b.qty,c.materialno,c.materialdesc from t_checkref a left join t_stock b on a.areaid = b.areaid inner join t_material c on b.materialnoid = c.id  where a.checkno = '" + checkno + "'";

                using (IDataReader dr = Common_DB.QueryByDividPage2(ref dividpage, sql))
                {
                    while (dr.Read())
                    {
                        lsttask.Add(GetCheckStock_GetModelFromDataReader(dr));
                    }
                    dividpage.CurrentPageRecordCounts = lsttask.Count;
                }
                if (lsttask == null || lsttask.Count == 0)
                {
                    strErrMsg = "没有信息！";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }

        public CheckAnalyze GetCheckStock_GetModelFromDataReader(IDataReader dr)
        {
            CheckAnalyze TMM = new CheckAnalyze();
            TMM.CHECKNO = (dr["CHECKNO"] ?? "").ToString();
            TMM.AREANO = (dr["AREANO"] ?? "").ToString();
            TMM.MATERIALDESC = (dr["MATERIALDESC"] ?? "").ToString();
            TMM.MATERIALNO = (dr["MATERIALNO"] ?? "").ToString();
            TMM.SERIALNO = (dr["SERIALNO"] ?? "").ToString();
            TMM.QTY = dr["QTY"].ToDecimalNull();
            //TMM.partno = (dr["partno"] ?? "").ToString();
            TMM.STRONGHOLDCODE = dr["STRONGHOLDCODE"].ToDBString();
            TMM.BatchNo = dr["BatchNo"].ToDBString();
            return TMM;
        }


        //找到盘点单关联的库位(Android)
        //public string GetAreanobyCheckno(string checkno)
        //{
        //    //修改表头状态为开始
        //    string sql = "update t_check set CHECKSTATUS = '开始' where checkno = '" + checkno + "'";
        //    dbFactory.ExecuteNonQuery(CommandType.Text, sql);

        //    List<CheckArea_Model>  list = new List<CheckArea_Model>();
        //    sql = "select * from t_checkref where checkno = '" + checkno + "'";
        //    using (IDataReader dr = dbFactory.ExecuteReader(sql))
        //    {
        //        while (dr.Read())
        //        {
        //            CheckArea_Model m = new CheckArea_Model();
        //            m.AREANO = dr["AREANO"].ToString();
        //            m.ID = Convert.ToInt32(dr["AREAID"]);
        //            list.Add(m);
        //        }
        //    }
        //    if (list.Count == 0)
        //    {
        //        BaseMessage_Model<List<CheckArea_Model>> bm = new BaseMessage_Model<List<CheckArea_Model>>();
        //        bm.HeaderStatus = "E";
        //        bm.Message = "没有库位信息";
        //        string json = Check_Func.SerializeObject(bm);
        //        return json;
        //    }
        //    else
        //    {
        //        BaseMessage_Model<List<CheckArea_Model>> bm = new BaseMessage_Model<List<CheckArea_Model>>();
        //        bm.HeaderStatus = "S";
        //        bm.Message = "";
        //        bm.ModelJson = list;
        //        string json = Check_Func.SerializeObject(bm);
        //        return json;
        //    }   
        //}

        //修改盘点单状态(Android)
        public string GetAreanobyCheckno(string checkno)
        {
            //修改表头状态为开始
            string sql = "update t_check set CHECKSTATUS = '开始' where checkno = '" + checkno + "'";
            dbFactory.ExecuteNonQuery(CommandType.Text, sql);

            BaseMessage_Model<List<CheckArea_Model>> bm = new BaseMessage_Model<List<CheckArea_Model>>();
            bm.HeaderStatus = "S";
            bm.Message = "";
            string json = Check_Func.SerializeObject(bm);
            return json;

        }

        //检测库存是否在盘点单内，并且返回areaid(Android)
        public string GetAreanobyCheckno2(string checkno, string areano)
        {
            List<CheckArea_Model> list = new List<CheckArea_Model>();
            string sql = "select * from t_checkref where checkno = '" + checkno + "' and areano = '" + areano + "'";
            using (IDataReader dr = dbFactory.ExecuteReader(sql))
            {
                if (dr.Read())
                {
                    CheckArea_Model m = new CheckArea_Model();
                    m.AREANO = dr["AREANO"].ToString();
                    m.ID = Convert.ToInt32(dr["AREAID"]);
                    list.Add(m);
                }
            }
            if (list.Count == 0)
            {
                BaseMessage_Model<List<CheckArea_Model>> bm = new BaseMessage_Model<List<CheckArea_Model>>();
                bm.HeaderStatus = "E";
                bm.Message = "该库位不在盘点单内";
                string json = Check_Func.SerializeObject(bm);
                return json;
            }
            else
            {
                BaseMessage_Model<List<CheckArea_Model>> bm = new BaseMessage_Model<List<CheckArea_Model>>();
                bm.HeaderStatus = "S";
                bm.Message = "";
                bm.ModelJson = list;
                string json = Check_Func.SerializeObject(bm);
                return json;
            }
        }

        //查询盘点表头（Android）
        public string GetCheck()
        {
            List<Check_Model> list = new List<Check_Model>();
            string sql = "select * from t_check where (CHECKSTATUS = '新建' or CHECKSTATUS = '开始') and checktype = '货位' and ISDEL = 1 order by CREATETIME desc";
            using (IDataReader dr = dbFactory.ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    Check_Model m = new Check_Model();
                    m.CHECKNO = dr["CHECKNO"].ToString();
                    m.CHECKDESC = dr["CHECKDESC"].ToString();
                    m.CHECKSTATUS = dr["CHECKSTATUS"].ToString();
                    m.REMARKS = dr["REMARKS"].ToString();
                    list.Add(m);
                }
            }
            if (list.Count == 0)
            {
                BaseMessage_Model<List<Check_Model>> bm = new BaseMessage_Model<List<Check_Model>>();
                bm.HeaderStatus = "E";
                bm.Message = "没有单据信息";
                string json = Check_Func.SerializeObject(bm);
                return json;
            }
            else
            {
                BaseMessage_Model<List<Check_Model>> bm = new BaseMessage_Model<List<Check_Model>>();
                bm.HeaderStatus = "S";
                bm.Message = "";
                bm.ModelJson = list;
                string json = Check_Func.SerializeObject(bm);
                return json;
            }

        }

        //查询明盘表头（Android）
        public string GetCheckMing()
        {
            List<Check_Model> list = new List<Check_Model>();
            string sql = "select * from t_check where (CHECKSTATUS = '新建' or CHECKSTATUS = '开始') and checktype = '明盘' and ISDEL = 1 order by CREATETIME desc";
            using (IDataReader dr = dbFactory.ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    Check_Model m = new Check_Model();
                    m.CHECKNO = dr["CHECKNO"].ToString();
                    m.CHECKDESC = dr["CHECKDESC"].ToString();
                    m.CHECKSTATUS = dr["CHECKSTATUS"].ToString();
                    m.REMARKS = dr["REMARKS"].ToString();
                    list.Add(m);
                }
            }
            if (list.Count == 0)
            {
                BaseMessage_Model<List<Check_Model>> bm = new BaseMessage_Model<List<Check_Model>>();
                bm.HeaderStatus = "E";
                bm.Message = "没有单据信息";
                string json = Check_Func.SerializeObject(bm);
                return json;
            }
            else
            {
                BaseMessage_Model<List<Check_Model>> bm = new BaseMessage_Model<List<Check_Model>>();
                bm.HeaderStatus = "S";
                bm.Message = "";
                bm.ModelJson = list;
                string json = Check_Func.SerializeObject(bm);
                return json;
            }

        }

        //返回给徐鑫明盘集合信息
        public string GetMinDetail(string checkno)
        {
            List<Barcode_Model> list = new List<Barcode_Model>();
            list = GetCheckRefStock(checkno, list);

            if (list.Count == 0)
            {
                BaseMessage_Model<string> bm = new BaseMessage_Model<string>();
                bm.HeaderStatus = "E";
                bm.Message = "没有获取到数据";
                string j = Check_Func.SerializeObject(bm);
                return j;
            }
            else
            {
                BaseMessage_Model<List<Barcode_Model>> bm = new BaseMessage_Model<List<Barcode_Model>>();
                bm.HeaderStatus = "S";
                bm.Message = "";
                bm.ModelJson = list;
                string j = Check_Func.SerializeObject(bm);
                return j;
            }
        }

        private static List<Barcode_Model> GetCheckRefStock(string checkno, List<Barcode_Model> list)
        {
            DbFactory dbFactory = new DbFactory(DbFactory.DbFactoryType.SQLSERVER);
            string sql = "select t.*,m.materialdesc from T_CHECKREFSTOCK t left join t_material m on t.materialno = m.materialno and t.strongholdcode = m.strongholdcode where t.voucherno = '" + checkno + "' order by t.areano,t.materialno";
            DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, sql).Tables[0];
            list = ModelConvertHelper<Barcode_Model>.ConvertToModel(dt);
            return list;
        }

        //接收徐鑫明盘外箱条码
        public string GetMinBarocde(string barcode, string checkno)
        {
            //验证条码正确性
            T_OutBarCode_Func tf = new T_OutBarCode_Func();
            string SerialNo = "";
            string BarCodeType = "";
            string strError = "";
            string sql = "";
            if (!tf.GetSerialNoByBarCode(barcode, ref SerialNo, ref BarCodeType, ref strError))
            {
                BaseMessage_Model<List<CheckDet_Model>> bm = new BaseMessage_Model<List<CheckDet_Model>>();
                bm.HeaderStatus = "E";
                bm.Message = strError;
                string j = Check_Func.SerializeObject(bm);
                return j;
            }

            List<Barcode_Model> minlist = new List<Barcode_Model>();

            //外箱
            if (BarCodeType == "1" || (BarCodeType == "3" && !SerialNo.Contains('P')))
            {
                //获取盘点内容列
                minlist = GetCheckRefStock(checkno, minlist);
                Barcode_Model sb = new Barcode_Model();
                //获取序列号库存信息
                sql = "select s.materialno,s.qty,v.warehouseno,v.houseno,v.AREANO,s.strongholdcode,s.batchno from t_stock s left join v_area v on s.areaid = v.ID where s.serialno ='" + SerialNo + "' ";
                using (var dr = dbFactory.ExecuteReader(sql))
                {
                    if (dr.Read())
                    {
                        sb.MaterialNo = dr["materialno"].ToDBString();
                        sb.Qty = dr["qty"].ToDecimal();
                        sb.warehouseno = dr["warehouseno"].ToDBString();
                        sb.houseno = dr["houseno"].ToDBString();
                        sb.areano = dr["areano"].ToDBString();
                        sb.StrongHoldCode = dr["StrongHoldCode"].ToDBString();
                        sb.BatchNo = dr["BatchNo"].ToDBString();
                    }
                    else
                    {
                        BaseMessage_Model<List<CheckDet_Model>> bm = new BaseMessage_Model<List<CheckDet_Model>>();
                        bm.HeaderStatus = "E";
                        bm.Message = "库存中没有该条码，该条码没有入库";
                        string j = Check_Func.SerializeObject(bm);
                        return j;
                    }
                }

                //判断条码是否在列表中
                bool res = minlist.Exists(p => p.MaterialNo == sb.MaterialNo && p.warehouseno == sb.warehouseno && p.areano == sb.areano && p.StrongHoldCode == sb.StrongHoldCode && p.BatchNo == sb.BatchNo);
                if (!res)
                {
                    BaseMessage_Model<List<CheckDet_Model>> bm = new BaseMessage_Model<List<CheckDet_Model>>();
                    bm.HeaderStatus = "E";
                    bm.Message = sb.MaterialNo + "," + sb.warehouseno + "," + sb.areano + "," + sb.StrongHoldCode + "," + sb.BatchNo + "该条码不在比对列表中";
                    string j = Check_Func.SerializeObject(bm);
                    return j;
                }

                //得到表头id
                int id = minlist.Find(p => p.MaterialNo == sb.MaterialNo && p.warehouseno == sb.warehouseno && p.areano == sb.areano && p.StrongHoldCode == sb.StrongHoldCode && p.BatchNo == sb.BatchNo).id;

                //判断该序列号是否已经扫描
                sql = "select count(1) from T_CHECKREFSERIAL where VOUCHERNO = '" + checkno + "' and SERIALNO = '" + SerialNo + "'";
                int c = Convert.ToInt32(dbFactory.ExecuteScalar(CommandType.Text, sql));
                if (c > 0)
                {
                    BaseMessage_Model<List<CheckDet_Model>> bm = new BaseMessage_Model<List<CheckDet_Model>>();
                    bm.HeaderStatus = "E";
                    bm.Message = "该条码已经扫描";
                    string j = Check_Func.SerializeObject(bm);
                    return j;
                }

                //插入序列号，修改数量
                List<string> sqls = new List<string>();
                sql = "insert into T_CHECKREFSERIAL (VOUCHERNO,MATERIALNO,WAREHOUSENO,HOUSENO,AREANO,QTY,STRONGHOLDCODE,BATCHNO,SERIALNO,HEADERID) values " +
                    "('" + checkno + "','" + sb.MaterialNo + "','" + sb.warehouseno + "','" + sb.houseno + "','" + sb.areano + "'," + sb.Qty + ",'" + sb.StrongHoldCode + "','" + sb.BatchNo + "','" + SerialNo + "'," + id + ")";
                sqls.Add(sql);
                sql = "update T_CHECKREFSTOCK set SQTY = SQTY+" + sb.Qty + " where ID=" + id;
                sqls.Add(sql);
                dbFactory.ExecuteNonQueryList(sqls);

                //返回徐鑫类
                BaseMessage_Model<Barcode_Model> bm2 = new BaseMessage_Model<Barcode_Model>();
                bm2.HeaderStatus = "S";
                bm2.Message = "";
                bm2.ModelJson = sb;
                string j2 = Check_Func.SerializeObject(bm2);
                return j2;
            }
            //托
            else if (BarCodeType == "2")
            {
                //获取盘点内容列
                minlist = GetCheckRefStock(checkno, minlist);
                List<Barcode_Model> lsb = new List<Barcode_Model>();
                //获取序列号库存信息
                sql = "select s.serialno,s.materialno,s.qty,v.warehouseno,v.houseno,v.AREANO,s.strongholdcode,s.batchno from t_stock s left join v_area v on s.areaid = v.ID where s.palletno ='" + SerialNo + "' ";
                using (var dr = dbFactory.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        Barcode_Model sb = new Barcode_Model();
                        sb.SerialNo = dr["serialno"].ToDBString();
                        sb.MaterialNo = dr["materialno"].ToDBString();
                        sb.Qty = dr["qty"].ToDecimal();
                        sb.warehouseno = dr["warehouseno"].ToDBString();
                        sb.houseno = dr["houseno"].ToDBString();
                        sb.areano = dr["areano"].ToDBString();
                        sb.StrongHoldCode = dr["StrongHoldCode"].ToDBString();
                        sb.BatchNo = dr["BatchNo"].ToDBString();
                        lsb.Add(sb);
                    }

                }
                if (lsb.Count == 0)
                {
                    BaseMessage_Model<List<CheckDet_Model>> bm = new BaseMessage_Model<List<CheckDet_Model>>();
                    bm.HeaderStatus = "E";
                    bm.Message = "库存中没有该托条码";
                    string j = Check_Func.SerializeObject(bm);
                    return j;
                }

                //判断条码是否是列表中的属性
                foreach (var sb in lsb)
                {
                    bool res = minlist.Exists(p => p.MaterialNo == sb.MaterialNo && p.warehouseno == sb.warehouseno && p.areano == sb.areano && p.StrongHoldCode == sb.StrongHoldCode && p.BatchNo == sb.BatchNo);
                    if (!res)
                    {
                        BaseMessage_Model<List<CheckDet_Model>> bm = new BaseMessage_Model<List<CheckDet_Model>>();
                        bm.HeaderStatus = "E";
                        bm.Message = sb.MaterialNo + "," + sb.warehouseno + "," + sb.areano + "," + sb.StrongHoldCode + "," + sb.BatchNo + "该条码不在比对列表中";
                        string j = Check_Func.SerializeObject(bm);
                        return j;
                    }
                }

                List<string> sqls = new List<string>();
                decimal qtyall = 0;
                foreach (var sb in lsb)
                {
                    //得到表头id
                    int id = minlist.Find(p => p.MaterialNo == sb.MaterialNo && p.warehouseno == sb.warehouseno && p.areano == sb.areano && p.StrongHoldCode == sb.StrongHoldCode && p.BatchNo == sb.BatchNo).id;

                    //判断该序列号是否已经扫描
                    sql = "select count(1) from T_CHECKREFSERIAL where VOUCHERNO = '" + checkno + "' and SERIALNO = '" + sb.SerialNo + "'";
                    int c = Convert.ToInt32(dbFactory.ExecuteScalar(CommandType.Text, sql));
                    if (c > 0)
                    {
                        BaseMessage_Model<List<CheckDet_Model>> bm = new BaseMessage_Model<List<CheckDet_Model>>();
                        bm.HeaderStatus = "E";
                        bm.Message = "该条码已经扫描";
                        string j = Check_Func.SerializeObject(bm);
                        return j;
                    }

                    //插入序列号，修改数量

                    sql = "insert into T_CHECKREFSERIAL (VOUCHERNO,MATERIALNO,WAREHOUSENO,HOUSENO,AREANO,QTY,STRONGHOLDCODE,BATCHNO,SERIALNO,HEADERID) values " +
                        "('" + checkno + "','" + sb.MaterialNo + "','" + sb.warehouseno + "','" + sb.houseno + "','" + sb.areano + "'," + sb.Qty + ",'" + sb.StrongHoldCode + "','" + sb.BatchNo + "','" + sb.SerialNo + "'," + id + ")";
                    sqls.Add(sql);
                    sql = "update T_CHECKREFSTOCK set SQTY = SQTY+" + sb.Qty + " where ID=" + id;
                    sqls.Add(sql);
                    qtyall += sb.Qty;
                }
                dbFactory.ExecuteNonQueryList(sqls);

                //返回徐鑫类
                BaseMessage_Model<Barcode_Model> bm2 = new BaseMessage_Model<Barcode_Model>();
                bm2.HeaderStatus = "S";
                bm2.Message = "";
                //合并数量
                lsb[0].Qty = qtyall;
                bm2.ModelJson = lsb[0];
                string j2 = Check_Func.SerializeObject(bm2);
                return j2;
            }
            else
            {
                BaseMessage_Model<List<CheckDet_Model>> bm6 = new BaseMessage_Model<List<CheckDet_Model>>();
                bm6.HeaderStatus = "E";
                bm6.Message = "扫描的条码有误";
                string j6 = Check_Func.SerializeObject(bm6);
                return j6;
            }
        }

        //查看明盘序列号表体
        public string GetMinSerialno(int id)
        {
            List<Barcode_Model> list = new List<Barcode_Model>();
            string sql = "select * from T_CHECKREFSERIAL where HEADERID = " + id;
            DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, sql).Tables[0];
            list = ModelConvertHelper<Barcode_Model>.ConvertToModel(dt);
            if (list.Count == 0)
            {
                BaseMessage_Model<List<CheckDet_Model>> bm = new BaseMessage_Model<List<CheckDet_Model>>();
                bm.HeaderStatus = "E";
                bm.Message = "没有数据";
                string j = Check_Func.SerializeObject(bm);
                return j;
            }
            BaseMessage_Model<List<Barcode_Model>> bm2 = new BaseMessage_Model<List<Barcode_Model>>();
            bm2.HeaderStatus = "S";
            bm2.Message = "";
            bm2.ModelJson = list;
            string j2 = Check_Func.SerializeObject(bm2);
            return j2;
        }

        //提交明盘
        public string SummitMin(string checkno)
        {
            string sql = "update t_check set CHECKSTATUS = '完成' where checkno = '" + checkno + "'";
            dbFactory.ExecuteNonQuery(CommandType.Text, sql);
            BaseMessage_Model<List<Barcode_Model>> bm2 = new BaseMessage_Model<List<Barcode_Model>>();
            bm2.HeaderStatus = "S";
            bm2.Message = "提交完成";
            string j2 = Check_Func.SerializeObject(bm2);
            return j2;
        }




        //插入徐鑫扫描的盘点数据（Android）
        public string InsertCheckDetail(string json)
        {
            try
            {
                List<string> sqls = new List<string>();
                List<Barcode_Model> list = Check_Func.DeserializeJsonToList<Barcode_Model>(json);
                foreach (Barcode_Model model in list)
                {
                    //判断是否已经扫过
                    string sql = "select count(1) from T_CHECKDETAILS where CHECKNO = '" + model.CHECKNO + "' and SERIALNO = '" + model.SerialNo + "' ";
                    int o = Convert.ToInt32(dbFactory.ExecuteScalar(CommandType.Text, sql));
                    if (o > 0)
                    {
                        //修改数据
                        sql = "update T_CHECKDETAILS set CREATER = '" + model.Creater + "', AREAID = " + model.AREAID + ",qty = " + model.Qty + ",INSERTTIME =  getdate(),status = " + model.STATUS + " where CHECKNO = '" + model.CHECKNO + "' and SERIALNO = '" + model.SerialNo + "' ";
                        sqls.Add(sql);
                    }
                    else
                    {
                        sql = "insert into T_CHECKDETAILS(CREATER,CHECKNO,AREAID,MATERIALID,SERIALNO,QTY,INSERTTIME,status) values('" + model.Creater + "','" + model.CHECKNO + "'," + model.AREAID + "," + model.MaterialNoID + ",'" + model.SerialNo + "'," + model.Qty + ",getdate()," + model.STATUS + ")";
                        sqls.Add(sql);
                    }
                }
                int val = dbFactory.ExecuteNonQueryList(sqls);
                if (val == -2)
                {
                    BaseMessage_Model<string> bm = new BaseMessage_Model<string>();
                    bm.HeaderStatus = "E";
                    bm.Message = "插入失败";
                    string j = Check_Func.SerializeObject(bm);
                    return j;
                }
                else
                {
                    BaseMessage_Model<string> bm = new BaseMessage_Model<string>();
                    bm.HeaderStatus = "S";
                    bm.Message = "";
                    string j = Check_Func.SerializeObject(bm);
                    return j;
                }
            }
            catch (Exception ex)
            {
                BaseMessage_Model<string> bm = new BaseMessage_Model<string>();
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                string j = Check_Func.SerializeObject(bm);
                return j;
            }

        }

        //查询扫描合并记录（Android）
        public string GetCheckDetail(string checkno)
        {
            List<Barcode_Model> list = new List<Barcode_Model>();
            string sql = "select max(c.inserttime) as inserttime,c.areaid,c.materialid,a.AREANO,m.MATERIALNO,m.MATERIALDESC,m.StrongHoldCode,sum(QTY) as QTY from T_CHECKDETAILS c" +
            " left join t_area a on c.areaid = a.id left join T_MATERIAL m on m.id = c.MATERIALID" +
            " where CHECKNO = '" + checkno + "'  group by c.areaid,c.materialid,a.AREANO,m.MATERIALNO,m.MATERIALDESC,m.StrongHoldCode order by inserttime desc";
            using (IDataReader dr = dbFactory.ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    Barcode_Model m = new Barcode_Model();
                    m.AREAID = dr["areaid"].ToInt32();
                    m.MaterialNoID = dr["materialid"].ToInt32();
                    m.areano = dr["AREANO"].ToString();
                    m.MaterialNo = dr["MATERIALNO"].ToString();
                    m.MaterialDesc = dr["MATERIALDESC"].ToString();
                    m.Qty = Convert.ToDecimal(dr["QTY"]);
                    m.StrongHoldCode = dr["StrongHoldCode"].ToString();
                    list.Add(m);
                }
            }
            if (list.Count == 0)
            {
                BaseMessage_Model<List<Barcode_Model>> bm = new BaseMessage_Model<List<Barcode_Model>>();
                bm.HeaderStatus = "E";
                bm.Message = "没有扫描";
                string j = Check_Func.SerializeObject(bm);
                return j;
            }
            else
            {
                BaseMessage_Model<List<Barcode_Model>> bm = new BaseMessage_Model<List<Barcode_Model>>();
                bm.HeaderStatus = "S";
                bm.Message = "";
                bm.ModelJson = list;
                string j = Check_Func.SerializeObject(bm);
                return j;
            }
        }

        public string DeleteCheckDetail(string checkno, string json)
        {
            Barcode_Model bm = Check_Func.DeserializeJsonToObject<Barcode_Model>(json);
            string sql = "delete from T_CHECKDETAILS where checkno = '" + checkno + "' and materialid = " + bm.MaterialNoID + " and areaid = " + bm.AREAID;
            dbFactory.ExecuteNonQuery(CommandType.Text, sql);
            BaseMessage_Model<string> bm2 = new BaseMessage_Model<string>();
            bm2.HeaderStatus = "S";
            bm2.Message = "删除成功！";
            string j = Check_Func.SerializeObject(bm2);
            return j;
        }

        //盘点扫描条码获取内容

        public string GetScanInfo(string barcode)
        {
            //验证条码正确性
            T_OutBarCode_Func tf = new T_OutBarCode_Func();
            string SerialNo = "";
            string BarCodeType = "";
            string strError = "";
            string sql = "";
            if (!tf.GetSerialNoByBarCode(barcode, ref SerialNo, ref BarCodeType, ref strError))
            {
                BaseMessage_Model<List<CheckDet_Model>> bm = new BaseMessage_Model<List<CheckDet_Model>>();
                bm.HeaderStatus = "E";
                bm.Message = strError;
                string j = Check_Func.SerializeObject(bm);
                return j;
            }
            //if (BarCodeType == "3")
            //{
            //    BaseMessage_Model<List<CheckDet_Model>> bm = new BaseMessage_Model<List<CheckDet_Model>>();
            //    bm.HeaderStatus = "E";
            //    bm.Message = "请扫描二维码，盘点不支持手动输入";
            //    string j = Check_Func.SerializeObject(bm);
            //    return j;
            //}

            List<Barcode_Model> returnlist = new List<Barcode_Model>();

            //外箱
            if (BarCodeType == "1" || (BarCodeType == "3" && !SerialNo.Contains('P')))
            {

                //从查看库存里有没有
                sql = "select * from t_stock where SERIALNO = '" + SerialNo + "'";
                using (var dr = dbFactory.ExecuteReader(sql))
                {
                    if (dr.Read())
                    {
                        Barcode_Model bm = MakeQInfo(dr);
                        //bm.PALLETNO = dr["PALLETNO"].ToDBString();
                        returnlist.Add(bm);
                    }
                }
                if (returnlist.Count == 0)
                {
                    sql = "select * from t_outbarcode where SERIALNO = '" + SerialNo + "'";
                    using (var dr = dbFactory.ExecuteReader(sql))
                    {
                        if (dr.Read())
                        {
                            Barcode_Model bm = MakeQInfo(dr);
                            returnlist.Add(bm);
                        }
                    }
                }

                if (returnlist.Count == 0)
                {
                    BaseMessage_Model<List<CheckDet_Model>> bm = new BaseMessage_Model<List<CheckDet_Model>>();
                    bm.HeaderStatus = "E";
                    bm.Message = "没有找到箱条码";
                    string j = Check_Func.SerializeObject(bm);
                    return j;
                }
                //返回单箱信息
                BaseMessage_Model<List<Barcode_Model>> bm2 = new BaseMessage_Model<List<Barcode_Model>>();
                bm2.HeaderStatus = "S";
                bm2.Message = "";
                bm2.ModelJson = returnlist;
                string j2 = Check_Func.SerializeObject(bm2);
                return j2;

            }

            //托盘  ，就查条码表
            if (BarCodeType == "2" || (BarCodeType == "3" && SerialNo.Contains('P')))
            {

                //-----------------------原本托盘点-------------------
                sql = "select barcode from T_PALLETDETAIL where pallettype = 1 and palletno = '" + SerialNo + "'";
                sql = "select o.*,s.qty as sqty,s.MaterialNoID as sMaterialNoID,s.StrongHoldCode  as SStrongHoldCode,s.StrongHoldName as SStrongHoldName  from t_outbarcode o left join t_stock s on o.barcode = s.barcode  where o.barcode in (" + sql + ")";
                using (var dr = dbFactory.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        Barcode_Model bm = MakeQInfo2(dr);
                        returnlist.Add(bm);
                    }

                }
                if (returnlist.Count == 0)
                {
                    BaseMessage_Model<List<CheckDet_Model>> bm = new BaseMessage_Model<List<CheckDet_Model>>();
                    bm.HeaderStatus = "E";
                    bm.Message = "没有找到托盘条码对应的箱条码";
                    string j = Check_Func.SerializeObject(bm);
                    return j;
                }
                //返回单箱信息
                BaseMessage_Model<List<Barcode_Model>> bm2 = new BaseMessage_Model<List<Barcode_Model>>();
                bm2.HeaderStatus = "S";
                bm2.Message = "";
                bm2.ModelJson = returnlist;
                string j2 = Check_Func.SerializeObject(bm2);
                return j2;
                //-----------------------原本托盘点-------------------

                //BaseMessage_Model<List<CheckDet_Model>> bm = new BaseMessage_Model<List<CheckDet_Model>>();
                //bm.HeaderStatus = "E";
                //bm.Message = "盘点不支持托盘，请扫描箱条码";
                //string j = Check_Func.SerializeObject(bm);
                //return j;

            }

            BaseMessage_Model<List<CheckDet_Model>> bm3 = new BaseMessage_Model<List<CheckDet_Model>>();
            bm3.HeaderStatus = "E";
            bm3.Message = "错误";
            string j3 = Check_Func.SerializeObject(bm3);
            return j3;


        }

        private static Barcode_Model MakeQInfo(IDataReader dr)
        {
            Barcode_Model bm = new Barcode_Model();
            bm.SerialNo = dr["SerialNo"].ToDBString();
            bm.BarCode = dr["BarCode"].ToDBString();
            bm.MaterialNo = dr["MaterialNo"].ToDBString();
            bm.MaterialDesc = dr["MaterialDesc"].ToDBString();
            bm.Qty = dr["Qty"].ToDecimal();
            bm.BatchNo = dr["BatchNo"].ToDBString();
            bm.Unit = dr["Unit"].ToDBString();
            bm.MaterialNoID = dr["MATERIALNOID"].ToInt32();
            bm.StrongHoldCode = dr["StrongHoldCode"].ToDBString();
            bm.StrongHoldName = dr["StrongHoldName"].ToString();
            return bm;
        }

        private static Barcode_Model MakeQInfo2(IDataReader dr)
        {
            Barcode_Model bm = new Barcode_Model();
            bm.SerialNo = dr["SerialNo"].ToDBString();
            bm.BarCode = dr["BarCode"].ToDBString();
            bm.MaterialNo = dr["MaterialNo"].ToDBString();
            bm.MaterialDesc = dr["MaterialDesc"].ToDBString();
            if (dr["sqty"] is DBNull)
                bm.Qty = dr["Qty"].ToDecimal();
            else
                bm.Qty = dr["sQty"].ToDecimal();
            bm.BatchNo = dr["BatchNo"].ToDBString();
            bm.Unit = dr["Unit"].ToDBString();
            if (dr["sMATERIALNOID"] is DBNull)
                bm.MaterialNoID = dr["MATERIALNOID"].ToInt32();
            else
                bm.MaterialNoID = dr["sMATERIALNOID"].ToInt32();
            if (dr["sStrongHoldCode"] is DBNull)
                bm.StrongHoldCode = dr["StrongHoldCode"].ToDBString();
            else
                bm.StrongHoldCode = dr["sStrongHoldCode"].ToDBString();
            if (dr["sStrongHoldName"] is DBNull)
                bm.StrongHoldName = dr["StrongHoldName"].ToString();
            else
                bm.StrongHoldName = dr["sStrongHoldName"].ToString();
            return bm;
        }


        private static Barcode_Model MakeQInfo3(IDataReader dr)
        {
            Barcode_Model bm = new Barcode_Model();
            bm.areano = dr["areano"].ToDBString();
            bm.EDate = dr["EDate"].ToDateTime();
            bm.Eds = dr["EDate"].ToDateTime().ToString("yyyy-MM-dd");
            bm.SerialNo = dr["SerialNo"].ToDBString();
            bm.BarCode = dr["BarCode"].ToDBString();
            bm.MaterialNo = dr["MaterialNo"].ToDBString();
            bm.MaterialDesc = dr["MaterialDesc"].ToDBString();
            bm.Qty = dr["Qty"].ToDecimal();
            bm.BatchNo = dr["BatchNo"].ToDBString();
            //bm.Unit = dr["Unit"].ToDBString();
            //bm.MaterialNoID = dr["MATERIALNOID"].ToInt32();
            bm.StrongHoldCode = dr["StrongHoldCode"].ToDBString();
            bm.StrongHoldName = dr["StrongHoldName"].ToString();
            bm.warehousename = dr["warehousename"].ToDBString();
            bm.warehouseno = dr["warehouseno"].ToDBString();
            bm.STATUS = dr["STATUS"].ToInt32();
            bm.EAN = dr["EAN"].ToDBString();
            //用来判断是在库存还是条码表1库存，0条码
            bm.AllIn = "1";
            return bm;
        }

        private static Barcode_Model MakeQInfo4(IDataReader dr)
        {
            Barcode_Model bm = new Barcode_Model();
            //bm.areano = dr["areano"].ToDBString();
            bm.SerialNo = dr["SerialNo"].ToDBString();
            bm.BarCode = dr["BarCode"].ToDBString();
            bm.MaterialNo = dr["MaterialNo"].ToDBString();
            bm.MaterialDesc = dr["MaterialDesc"].ToDBString();
            bm.Qty = dr["Qty"].ToDecimal();
            bm.BatchNo = dr["BatchNo"].ToDBString();
            bm.EDate = dr["EDate"].ToDateTime();
            bm.Eds = dr["EDate"].ToDateTime().ToString("yyyy-MM-dd");
            //bm.Unit = dr["Unit"].ToDBString();
            //bm.MaterialNoID = dr["MATERIALNOID"].ToInt32();
            bm.StrongHoldCode = dr["StrongHoldCode"].ToDBString();
            bm.StrongHoldName = dr["StrongHoldName"].ToString();
            bm.EAN = dr["ean"].ToString();
            //bm.warehousename = dr["warehousename"].ToDBString();
            //bm.warehouseno = dr["warehouseno"].ToDBString();
            //bm.STATUS = dr["STATUS"].ToInt32();
            //用来判断是在库存还是条码表1库存，0条码
            bm.AllIn = "0";
            return bm;
        }




        //检查库位是否锁定，并返回库位id,徐鑫pda创建盘点单用
        public string TakeAID(string json)
        {
            CheckArea_Model para = new CheckArea_Model();
            para = Check_Func.DeserializeJsonToObject<CheckArea_Model>(json);
            BaseMessage_Model<CheckArea_Model> bm = new BaseMessage_Model<CheckArea_Model>();
            string j = "";
            CheckArea_Model cm = new CheckArea_Model();
            string sql = "select a.* from t_area a left join v_area v on a.id = v.id where a.AREANO = '" + para.AREANO + "' and v.warehouseno = '" + para.warehouseno + "' and a.AREASTATUS=1 and a.ISDEL = 1 ";
            using (var dr = dbFactory.ExecuteReader(sql))
            {
                if (dr.Read())
                {
                    cm.ID = dr["id"].ToInt32();
                    cm.AREANO = dr["AREANO"].ToDBString();
                }
                else
                {
                    sql = "select c.checkno from T_CHECK c left join t_checkref f on c.id = f.checkid left join v_area v on f.areaid = v.id where c.isdel = 1 and （c.checkstatus='新建' or c.checkstatus='开始') and f.areano='" + para.AREANO + "' and v.warehouseno = '" + para.warehouseno + "'";
                    object o = dbFactory.ExecuteScalar(CommandType.Text, sql);
                    if (o == null || o.ToString() == "")
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "没有该库位";
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }
                    else
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "该库位已经被" + o.ToString() + "锁定";
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }
                }
            }
            bm.HeaderStatus = "S";
            bm.Message = "";
            bm.ModelJson = cm;
            j = Check_Func.SerializeObject(bm);
            return j;
        }

        //徐鑫pda创建盘点单用
        public string GetWareHouse()
        {
            List<T_WareHouseInfo> list = new List<T_WareHouseInfo>();
            string sql = "select distinct warehouseno,warehousename from t_warehouse ";
            DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, sql).Tables[0];
            list = ModelConvertHelper<T_WareHouseInfo>.ConvertToModel(dt);
            if (list.Count == 0)
            {
                BaseMessage_Model<List<T_WareHouseInfo>> bm = new BaseMessage_Model<List<T_WareHouseInfo>>();
                bm.HeaderStatus = "E";
                bm.Message = "没有获取到列表";
                string j = Check_Func.SerializeObject(bm);
                return j;
            }
            else
            {
                BaseMessage_Model<List<T_WareHouseInfo>> bm = new BaseMessage_Model<List<T_WareHouseInfo>>();
                bm.HeaderStatus = "S";
                bm.Message = "";
                bm.ModelJson = list;
                string j = Check_Func.SerializeObject(bm);
                return j;
            }

        }



        //库存调整，获取条码
        public string GetInfoBySerial(string barcode)
        {
            //验证条码正确性
            T_OutBarCode_Func tf = new T_OutBarCode_Func();
            string SerialNo = "";
            string BarCodeType = "";
            string strError = "";
            string sql = "";
            if (!tf.GetSerialNoByBarCode(barcode, ref SerialNo, ref BarCodeType, ref strError))
            {
                BaseMessage_Model<List<CheckDet_Model>> bm = new BaseMessage_Model<List<CheckDet_Model>>();
                bm.HeaderStatus = "E";
                bm.Message = strError;
                string j = Check_Func.SerializeObject(bm);
                return j;
            }


            List<Barcode_Model> returnlist = new List<Barcode_Model>();

            //外箱
            if (BarCodeType == "1" || (BarCodeType == "3" && !SerialNo.Contains('P')))
            {

                //从查看库存里有没有
                sql = "select s.*,v.areano,v.warehouseno,v.warehousename from t_stock s left join v_area v on s.areaid = v.id where SERIALNO = '" + SerialNo + "'";
                using (var dr = dbFactory.ExecuteReader(sql))
                {
                    if (dr.Read())
                    {
                        Barcode_Model bm = MakeQInfo3(dr);
                        returnlist.Add(bm);
                    }
                }
                if (returnlist.Count == 0)
                {
                    sql = "select * from t_outbarcode where SERIALNO = '" + SerialNo + "' and barcodetype = 3";
                    using (var dr = dbFactory.ExecuteReader(sql))
                    {
                        if (dr.Read())
                        {
                            Barcode_Model bm = MakeQInfo4(dr);
                            returnlist.Add(bm);
                        }
                    }
                }

                if (returnlist.Count == 0)
                {
                    BaseMessage_Model<List<CheckDet_Model>> bm = new BaseMessage_Model<List<CheckDet_Model>>();
                    bm.HeaderStatus = "E";
                    bm.Message = "没有找到箱条码";
                    string j = Check_Func.SerializeObject(bm);
                    return j;
                }
                //返回单箱信息
                BaseMessage_Model<List<Barcode_Model>> bm2 = new BaseMessage_Model<List<Barcode_Model>>();
                bm2.HeaderStatus = "S";
                bm2.Message = "";
                bm2.ModelJson = returnlist;
                string j2 = Check_Func.SerializeObject(bm2);
                return j2;

            }


            BaseMessage_Model<List<CheckDet_Model>> bm3 = new BaseMessage_Model<List<CheckDet_Model>>();
            bm3.HeaderStatus = "E";
            bm3.Message = "只能扫描外箱";
            string j3 = Check_Func.SerializeObject(bm3);
            return j3;

        }

        //库存调整
        public string SaveInfo(string json, string man)
        {
            try
            {

                //根据用户编号找到名称
                string sqlname = "select username from t_user where userno = '" + man + "'";
                man = dbFactory.ExecuteScalar(CommandType.Text, sqlname).ToDBString();
                List<string> sqls = new List<string>();
                string sql = "";
                List<Barcode_Model> list = Check_Func.DeserializeJsonToList<Barcode_Model>(json);
                string areaid, houseid, warehouseid, mid, palletno = "";
                if (list.Count == 0)
                {
                    BaseMessage_Model<Barcode_Model> bm = new BaseMessage_Model<Barcode_Model>();
                    bm.HeaderStatus = "E";
                    bm.Message = "修改失败，没有得到数据";
                    string j = Check_Func.SerializeObject(bm);
                    return j;
                }
                Barcode_Model bar = list[0];

                if (bar.EDate == DateTime.MinValue)
                {
                    BaseMessage_Model<Barcode_Model> bm = new BaseMessage_Model<Barcode_Model>();
                    bm.HeaderStatus = "E";
                    bm.Message = "有效期不能为空";
                    string j = Check_Func.SerializeObject(bm);
                    return j;
                }

                if (bar.STATUS == 0)
                {
                    BaseMessage_Model<Barcode_Model> bm = new BaseMessage_Model<Barcode_Model>();
                    bm.HeaderStatus = "E";
                    bm.Message = "质检状态必须填写";
                    string j = Check_Func.SerializeObject(bm);
                    return j;
                }
                //库存有，修改库存逻辑
                if (bar.AllIn == "1")
                {
                    //查看库位
                    sql = "select * from v_area where areano = '" + bar.areano + "' and warehouseno = '" + bar.warehouseno + "'";
                    using (var dr = dbFactory.ExecuteReader(CommandType.Text, sql))
                    {
                        if (dr.Read())
                        {
                            areaid = dr["id"].ToDBString();
                            houseid = dr["houseid"].ToDBString();
                            warehouseid = dr["warehouseid"].ToDBString();
                        }
                        else
                        {
                            BaseMessage_Model<Barcode_Model> bm = new BaseMessage_Model<Barcode_Model>();
                            bm.HeaderStatus = "E";
                            bm.Message = "没有对应的库位关系";
                            string j = Check_Func.SerializeObject(bm);
                            return j;
                        }
                    }

                    //查看物料id
                    sql = "select id from t_material t where t.materialno = '" + bar.MaterialNo + "' and t.strongholdcode='" + bar.StrongHoldCode + "'";
                    using (var dr = dbFactory.ExecuteReader(sql))
                    {
                        if (dr.Read())
                        {
                            mid = dr["id"].ToDBString();
                        }
                        else
                        {
                            BaseMessage_Model<Barcode_Model> bm = new BaseMessage_Model<Barcode_Model>();
                            bm.HeaderStatus = "E";
                            bm.Message = "该物料没有这个据点" + bar.StrongHoldCode;
                            string j = Check_Func.SerializeObject(bm);
                            return j;
                        }
                    }


                    sql = "insert into T_TASKTRANS (VOUCHERTYPE,MATERIALNO,MATERIALDESC,CREATETIME,UNIT,CREATER,MATERIALNOID,STRONGHOLDNAME,COMPANYCODE" +
                           " ,STRONGHOLDCODE,BATCHNO,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,SERIALNO" +
                            " ,TOWAREHOUSEID ,TOHOUSEID ,TOAREAID " +
                          "  , BARCODE ,TASKTYPE ,QTY ,EDATE ,FROMWAREHOUSEID , FROMHOUSEID ,FROMAREAID,status,EAN)" +
                            "select 100,f.materialno,f.materialdesc,GETDATE(),f.unit,'" + man + "',f.materialnoid,f.strongholdname,10, " +
                          "  f.strongholdcode,f.batchno,f.productdate,f.supprdbatch,f.supprddate,f.serialno" +
                           " ,'','','',f.barcode,2,f.qty,f.edate,f.warehouseid,f.houseid,f.areaid,f.status,f.EAN from t_stock f where f.barcode = '" + bar.BarCode + "'";
                    sqls.Add(sql);
                    //修改库存表
                    sql = "update t_stock set edate ='" + bar.EDate.ToString("yyyy/MM/dd") + "' ,materialnoid=" + mid + ",qty = " + bar.Qty + ",BatchNo ='" + bar.BatchNo + "',StrongHoldCode ='" + bar.StrongHoldCode + "',StrongHoldName ='" + bar.StrongHoldName + "',areaid = " + areaid + ",houseid = " + houseid + ",warehouseid = " + warehouseid + ",status=" + bar.STATUS + ",ean='" + bar.EAN + "' where serialno = '" + bar.SerialNo + "'";
                    sqls.Add(sql);
                    sql = "insert into T_TASKTRANS (VOUCHERTYPE,MATERIALNO,MATERIALDESC,CREATETIME,UNIT,CREATER,MATERIALNOID,STRONGHOLDNAME,COMPANYCODE" +
                        " ,STRONGHOLDCODE,BATCHNO,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,SERIALNO" +
                         " ,TOWAREHOUSEID ,TOHOUSEID ,TOAREAID " +
                       "  , BARCODE ,TASKTYPE ,QTY ,EDATE ,FROMWAREHOUSEID , FROMHOUSEID ,FROMAREAID,status,ean)" +
                         "select 100,f.materialno,f.materialdesc,GETDATE(),f.unit,'" + man + "',(select id from t_material t where t.materialno = '" + bar.MaterialNo + "' and t.strongholdcode='" + bar.StrongHoldCode + "'),'" + bar.StrongHoldName + "',10, " +
                       " '" + bar.StrongHoldCode + "','" + bar.BatchNo + "',f.productdate,f.supprdbatch,f.supprddate,f.serialno" +
                        " ," + warehouseid + "," + houseid + "," + areaid + ",f.barcode,1," + bar.Qty + ",f.edate,'','','',f.status,f.ean from t_stock f where f.barcode = '" + bar.BarCode + "'";
                    sqls.Add(sql);
                    sql = "update t_outbarcode set materialnoid=(select id from t_material t where t.materialno = '" + bar.MaterialNo + "' and t.strongholdcode='" + bar.StrongHoldCode + "'),edate ='" + bar.EDate.ToString("yyyy/MM/dd") + "',qty = " + bar.Qty + ",BatchNo ='" + bar.BatchNo + "',StrongHoldCode ='" + bar.StrongHoldCode + "',StrongHoldName ='" + bar.StrongHoldName + "' where serialno = '" + bar.SerialNo + "'";
                    sqls.Add(sql);
                    //插入流水


                }
                //条码有，做插入操作
                else if (bar.AllIn == "0")
                {

                    //查看
                    sql = "select * from v_area where areano = '" + bar.areano + "' and warehouseno = '" + bar.warehouseno + "'";
                    using (var dr = dbFactory.ExecuteReader(CommandType.Text, sql))
                    {
                        if (dr.Read())
                        {
                            areaid = dr["id"].ToDBString();
                            houseid = dr["houseid"].ToDBString();
                            warehouseid = dr["warehouseid"].ToDBString();
                        }
                        else
                        {
                            BaseMessage_Model<Barcode_Model> bm = new BaseMessage_Model<Barcode_Model>();
                            bm.HeaderStatus = "E";
                            bm.Message = "没有对应的库位关系";
                            string j = Check_Func.SerializeObject(bm);
                            return j;
                        }
                    }

                    sql = "select * from t_outbarcode where barcode = '" + bar.BarCode + "' and barcodetype = 3";
                    DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                    List<Barcode_Model> lbb = ModelConvertHelper<Barcode_Model>.ConvertToModel(dt);
                    Barcode_Model item = lbb[0];
                    //插入
                    sql = "insert into T_STOCK (BARCODE,SERIALNO,MATERIALNO,MATERIALDESC,MATERIALNOID,WAREHOUSEID," +
                                          "HOUSEID,AREAID,QTY,STATUS,ISDEL,CREATER,CREATETIME,BATCHNO,UNIT,PALLETNO,STRONGHOLDCODE,STRONGHOLDNAME" +
                                          ",COMPANYCODE,EDATE,SUPCODE,SUPNAME,SUPPRDBATCH,RECEIVESTATUS,ISLIMITSTOCK,ean,barcodetype)" +
                                          "VALUES('" + item.BarCode + "','" + item.SerialNo + "','" + item.MaterialNo + "','" + item.MaterialDesc + "'" +
                                          "," + item.MaterialNoID + "," + warehouseid + "," + houseid + "," + areaid +
                                          "," + item.Qty + "," + bar.STATUS + ",1,'" + man + "',GETDATE(),'" + item.BatchNo + "','" + item.Unit + "','" + item.PalletNo + "','" + item.StrongHoldCode + "','" + item.StrongHoldName + "'" +
                                          ",'10','" + item.EDate.ToString("yyyy-MM-dd") + "','" + item.SupCode + "','" + item.SupName + "'" +
                                          ",'" + item.SupPrdBatch + "',2,2,'" + item.EAN + "',3)";
                    sqls.Add(sql);
                    //插入流水
                    sql = "insert into T_TASKTRANS (VOUCHERTYPE,MATERIALNO,MATERIALDESC,CREATETIME,UNIT,CREATER,MATERIALNOID,STRONGHOLDNAME,COMPANYCODE" +
                          " ,STRONGHOLDCODE,BATCHNO,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,SERIALNO" +
                           " ,TOWAREHOUSEID ,TOHOUSEID ,TOAREAID " +
                         "  , BARCODE ,TASKTYPE ,QTY ,EDATE ,FROMWAREHOUSEID ,FROMHOUSEID ,FROMAREAID,status,ean)" +
                           "select 100,f.materialno,f.materialdesc,GETDATE(),f.unit,'" + man + "',f.materialnoid,f.strongholdname,10, " +
                         "  f.strongholdcode,f.batchno,f.productdate,f.supprdbatch,f.supprddate,f.serialno" +
                          " ,f.warehouseid,f.houseid,f.areaid,f.barcode,1,f.qty,f.edate,'','','',f.status,f.ean from t_stock f where f.barcode = '" + bar.BarCode + "'";
                    sqls.Add(sql);

                }
                //allin=2,删除操作
                else
                {
                    //判断箱有没有托盘
                    sql = "select max(palletno) from t_palletdetail where serialno = '" + bar.SerialNo + "'";
                    object o = dbFactory.ExecuteScalar(CommandType.Text, sql);
                    if (o == null || o.ToString() == "")
                    { }
                    else
                    {
                        palletno = o.ToString();
                    }


                    if (!string.IsNullOrEmpty(bar.areano))
                    {
                        sql = "insert into T_TASKTRANS (VOUCHERTYPE,MATERIALNO,MATERIALDESC,CREATETIME,UNIT,CREATER,MATERIALNOID,STRONGHOLDNAME,COMPANYCODE" +
                            " ,STRONGHOLDCODE,BATCHNO,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,SERIALNO" +
                             " ,TOWAREHOUSEID ,TOHOUSEID ,TOAREAID " +
                           "  , BARCODE ,TASKTYPE ,QTY ,EDATE ,FROMWAREHOUSEID , FROMHOUSEID ,FROMAREAID,status,ean)" +
                             "select 100,f.materialno,f.materialdesc,GETDATE(),f.unit,'" + man + "',f.materialnoid,f.strongholdname,10, " +
                           "  f.strongholdcode,f.batchno,f.productdate,f.supprdbatch,f.supprddate,f.serialno" +
                            " ,'','','',f.barcode,2,f.qty,f.edate,f.warehouseid,f.houseid,f.areaid,f.status,f.ean from t_stock f where f.barcode = '" + bar.BarCode + "'";
                        sqls.Add(sql);
                        //插入放颖库存备份表
                        sql = "INSERT INTO t_Stocktrans (Barcode, Serialno, Materialno, Materialdesc, Warehouseid, Houseid, Areaid, Qty,  Status, Isdel, Creater, Createtime, " +
                             "Batchno, Sn,  Oldstockid, Taskdetailesid, Checkid, Transferdetailsid,  Unit, Unitname, Palletno, Receivestatus,  Islimitstock,  " +
                             "Materialnoid, Strongholdcode, Strongholdname, Companycode, Edate, Supcode, Supname, Productdate, Supprdbatch, Supprddate, Isquality,ean)" +
                            "SELECT A.Barcode,A.Serialno,A.Materialno,A.Materialdesc,A.Warehouseid,A.Houseid,A.Areaid,A.Qty, " +
                            "A.Status,A.Isdel,'" + man + "',A.Createtime,A.Batchno,A.Sn,a.Oldstockid,A.Taskdetailesid,A.Checkid,A.Transferdetailsid,A.Unit,A.Unitname,A.Palletno,A.Receivestatus," +
                            "A.Islimitstock,A.Materialnoid,A.Strongholdcode,A.Strongholdname,A.Companycode,A.Edate,A.Supcode,A.Supname,A.Productdate,A.Supprdbatch," +
                            "A.Supprddate,A.Isquality,A.ean FROM T_STOCK A WHERE A.barcode = '" + bar.BarCode + "'";
                        sqls.Add(sql);
                        sql = "delete from t_stock where serialno = '" + bar.SerialNo + "'";
                        sqls.Add(sql);

                        //删除托盘
                        sql = "delete from t_palletdetail where serialno = '" + bar.SerialNo + "'";
                        sqls.Add(sql);
                    }

                }
                string errMsg = "";
                dbFactory.ExecuteNonQueryList(sqls, ref errMsg);


                //在所有逻辑处理完成后，删除空托盘信息
                if (!string.IsNullOrEmpty(palletno))
                {
                    sql = "select count(1) from t_palletdetail where palletno = '" + palletno + "'";
                    int i = dbFactory.ExecuteScalar(CommandType.Text, sql).ToInt32();
                    if (i == 0)
                    {
                        //没有托盘子信息，删除托盘头
                        sql = "delete from t_pallet where palletno = '" + palletno + "'";
                        dbFactory.ExecuteNonQuery(CommandType.Text, sql);
                    }
                }




                BaseMessage_Model<Barcode_Model> bm2 = new BaseMessage_Model<Barcode_Model>();
                bm2.HeaderStatus = "S";
                bm2.Message = "完成";
                string j2 = Check_Func.SerializeObject(bm2);
                return j2;
            }
            catch (Exception ex)
            {
                BaseMessage_Model<Barcode_Model> bm2 = new BaseMessage_Model<Barcode_Model>();
                bm2.HeaderStatus = "E";
                bm2.Message = ex.ToString();
                string j2 = Check_Func.SerializeObject(bm2);
                return j2;
            }
        }

        //查看明盘状态
        public bool GetCheckStock2(string checkno, ref List<T_StockInfoEX> list)
        {
            string sql = "select t.*, t.rowid from T_CHECKREFSTOCK t where voucherno = '" + checkno + "'";
            DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, sql).Tables[0];
            list = ModelConvertHelper<T_StockInfoEX>.ConvertToModel(dt);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].XH = (i + 1).ToString();
            }
            if (list.Count == 0)
                return false;
            else
                return true;
        }

        public void dbTest()
        {
            List<string> sqls = new List<string>();
            string sql = "delete from t_palletdetail where id = 2366";
            sqls.Add(sql);
            sql = "select id from t_palletdetail where id = 2366";
            object o = dbFactory.ExecuteScalar(CommandType.Text, sql);
            if (o == null || o.ToString() == "")
            {
                sql = "delete from t_palletdetail where id = 2367";
                sqls.Add(sql);
            }

            dbFactory.ExecuteNonQueryList(sqls);
        }



    }
}
