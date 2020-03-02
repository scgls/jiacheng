using BILBasic.Basing;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILBasic.DBA;
using BILWeb.OutBarCode;
using BILWeb.Print;
using BILWeb.Stock;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.Query
{



    public class Query_DB
    {
        public DbFactory dbFactory = new DbFactory(DbFactory.DbFactoryType.SQLSERVER);
        //统计当前页数量总和
        decimal qtyall = 0;
        //统计序号
        int xh = 0;


        decimal sumslls = 0;


        //订单类型查询
        public List<VoucherType> GetVoucherType()
        {
            List<VoucherType> lsttask = new List<VoucherType>();
            try
            {
                string sql = "select * from T_PARAMETERVOU  where instocktype !=0 ";
                using (IDataReader dr = dbFactory.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        VoucherType v = new VoucherType();
                        v.vouchertype = Convert.ToInt32(dr["vouchertype"]);
                        v.vouchername = dr["vouchername"].ToString();
                        lsttask.Add(v);
                    }
                }
                lsttask.Insert(0, new VoucherType() { vouchertype = 0, vouchername = "全部" });
                return lsttask;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public string GetDeliveryInfoAndroid(string Erpvoucherno)
        {
            BaseMessage_Model<CusSup> bm = new BaseMessage_Model<CusSup>();
            string ipport = "";
            string j = "";
            string ErrMsg = "";
            CusSup cs = new CusSup();
            bool res = GetDeliveryInfo(Erpvoucherno, "", ref cs);
            if (res)
            {
                bm.HeaderStatus = "S";
                bm.Message = "成功";
                bm.ModelJson = cs;
                j = Check_Func.SerializeObject(bm);
                return j;
            }
            else
            {
                bm.HeaderStatus = "E";
                bm.Message = "失败";
                j = Check_Func.SerializeObject(bm);
                return j;
            }
        }

        public bool GetDeliveryInfo(string Erpvoucherno, string judian, ref CusSup cussup)
        {
            cussup = new CusSup();
            List<Address> la = new List<Address>();
            string sql = "select a.erpnote,a.erpvoucherno,a.strongholdcode,a.vouchertype,a.customercode,a.customername,a.supplierno,a.suppliername" +
                ",a.departmentcode,a.departmentname,a.vouuser,b.toerpwarehouse from t_outstock a left join t_outstockdetail b on a.id = b.headerid where a.erpvoucherno= '" + Erpvoucherno + "' order by b.toerpwarehouse ";
            using (var dr = dbFactory.ExecuteReader(sql))
            {
                if (dr.Read())
                {
                    cussup.SendNo = dr["erpnote"].ToDBString();
                    cussup.ErpVoucherNo = dr["erpvoucherno"].ToDBString();
                    cussup.strongholdcode = dr["strongholdcode"].ToDBString();
                    cussup.VoucherType = dr["vouchertype"].ToInt32();
                    cussup.customercode = dr["customercode"].ToDBString();
                    cussup.customername = dr["customername"].ToDBString();
                    cussup.supplierno = dr["supplierno"].ToDBString();
                    cussup.suppliername = dr["suppliername"].ToDBString();
                    cussup.departmentcode = dr["departmentcode"].ToDBString();
                    cussup.departmentname = dr["departmentname"].ToDBString();
                    cussup.vouuser = dr["vouuser"].ToDBString();
                    cussup.toerpwarehouse = dr["toerpwarehouse"].ToDBString();
                }
                else
                    return false;
            }
            //销售出库
            if (cussup.VoucherType == 24)
            {
                sql = "select  distinct address from t_customeraddress where customerno = '" + cussup.customercode + "' and strongholdcode = '" + cussup.strongholdcode + "'";
                using (var dr = dbFactory.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        Address cs = new Address();
                        cs.address = dr["address"].ToDBString();
                        la.Add(cs);
                    }
                    cussup.addresses = la;
                }
            }
            //仓退单
            else if (cussup.VoucherType == 21)
            {
                sql = "select  distinct address from t_supplieraddress where supplierno = '" + cussup.supplierno + "' and strongholdcode = '" + cussup.strongholdcode + "'";
                using (var dr = dbFactory.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        Address cs = new Address();
                        cs.address = dr["address"].ToDBString();
                        la.Add(cs);
                    }
                    cussup.addresses = la;
                }
            }
            return true;
        }


        //托盘查询
        public bool GetPalletInfo(T_StockInfoEX model, ref DividPage dividpage, ref List<T_StockInfoEX> lsttask, ref string strErrMsg)
        {
            if (dividpage == null) dividpage = new DividPage();
            lsttask = new List<T_StockInfoEX>();
            try
            {
                string sql = "select ROW_NUMBER() OVER(Order by ID desc) AS PageRowNumber ,b.* from T_PALLET b where  " + Pallet_GetFilterSql(model);
                DataTable dt = Common_DB.QueryByDividPage(ref dividpage, sql);
                dividpage.CurrentPageRecordCounts = dt.Rows.Count;
                lsttask = Print_DB.ConvertToModel<T_StockInfoEX>(dt);

                if (lsttask == null || lsttask.Count == 0)
                {
                    strErrMsg = "没有信息！";
                    return false;
                }
                else
                {

                    for (int i = 1; i <= lsttask.Count; i++)
                    {
                        lsttask[i - 1].XH = i.ToString();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }
        private string Pallet_GetFilterSql(T_StockInfoEX mo)
        {
            string sql = " ISDEL = 1 ";

            if (mo.PalletNo != "" && !mo.PalletNo.Contains(','))
                sql += " and PalletNo = '" + mo.PalletNo + "'";
            else if (mo.PalletNo != "" && mo.PalletNo.Contains(','))
            {
                string PalletNo = mo.PalletNo;
                Query_Func.ChangeQuery(ref PalletNo);
                sql += " and PalletNo in(" + PalletNo + ")";
            }
            if (mo.Creater != "")
                sql += " and Creater = '" + mo.Creater + "'";

            if (mo.ErpVoucherNo != "")
                sql += " and ErpVoucherNo = '" + mo.ErpVoucherNo + "'";

            if (!string.IsNullOrEmpty(mo.SupCode))
            {
                sql += " and SUPPLIERNO = '" + mo.SupCode + "'";
            }

            if (mo.Status != 0)
            {
                sql += " and pallettype =" + mo.Status;
            }

            if (mo.CreateTime != null)
            {
                sql += "and CREATETIME>=to_date('" + ((DateTime)mo.CreateTime).ToString("yyyy-MM-dd") + "','YYYY/MM/DD') ";
                sql += "and CREATETIME<=to_date('" + ((DateTime)mo.CreateTime).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY/MM/DD') ";
            }
            return sql;

        }

        //托盘明细查询
        public bool GetPalletDetInfo(T_StockInfoEX model, ref List<T_StockInfoEX> lsttask, ref string strErrMsg)
        {
            lsttask = new List<T_StockInfoEX>();
            try
            {
                string sql = "select b.*,o.unit from t_Palletdetail b left join t_outbarcode o on b.serialno = o.serialno where palletno = '" + model.PalletNo + "'";
                DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                lsttask = Print_DB.ConvertToModel<T_StockInfoEX>(dt);
                if (lsttask == null || lsttask.Count == 0)
                {
                    strErrMsg = "没有信息！";
                    return false;
                }
                else
                {

                    for (int i = 1; i <= lsttask.Count; i++)
                    {
                        lsttask[i - 1].XH = i.ToString();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }

        public bool GetPalletDetInfo2(T_StockInfoEX model, ref List<T_StockInfoEX> lsttask, ref string strErrMsg)
        {
            lsttask = new List<T_StockInfoEX>();
            try
            {
                string sql = "select b.edate,b.materialno,b.batchno,sum(b.qty) as qty,count(1) as ItemQty ,o.unit from t_Palletdetail b left join t_outbarcode o on b.serialno = o.serialno where palletno = '" + model.PalletNo + "' group by b.materialno,b.batchno,o.unit,b.edate";
                DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                lsttask = Print_DB.ConvertToModel<T_StockInfoEX>(dt);
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


        //开工明细

        //库存明细查询
        public bool GetStockDetInfo(T_StockInfoEX model, ref DividPage dividpage, ref List<T_StockInfoEX> lsttask, ref string strErrMsg)
        {
            if (dividpage == null) dividpage = new DividPage();
            lsttask = new List<T_StockInfoEX>();
            try
            {
                string sql = "select ROW_NUMBER() OVER(Order by ID desc) AS PageRowNumber ,b.* from V_StockDet b where  " + StockD_GetFilterSql(model);
                Common_FactoryDB fbd = new Common_FactoryDB();
                DataTable dt = fbd.QueryByDividPage(ref dividpage, sql);
                dividpage.CurrentPageRecordCounts = dt.Rows.Count;
                lsttask = Print_DB.ConvertToModel<T_StockInfoEX>(dt);

                if (lsttask == null || lsttask.Count == 0)
                {
                    strErrMsg = "没有信息！";
                    return false;
                }
                else
                {

                    for (int i = 1; i <= lsttask.Count; i++)
                    {
                        lsttask[i - 1].XH = i.ToString();
                        qtyall += lsttask[i - 1].Qty;
                        switch (lsttask[i - 1].Status)
                        {
                            case 1:
                                lsttask[i - 1].StatusName = "待检";
                                break;
                            case 2:
                                lsttask[i - 1].StatusName = "送检";
                                break;
                            case 3:
                                lsttask[i - 1].StatusName = "检验合格";
                                break;
                            case 4:
                                lsttask[i - 1].StatusName = "检验不合格";
                                break;
                        }
                        if (lsttask[i - 1].IsAmount == 2)
                        {
                            lsttask[i - 1].StrIsAmount = "拆";
                        }
                        else {
                            lsttask[i - 1].StrIsAmount = "原";
                        }

                    }
                    T_StockInfoEX TMM = new T_StockInfoEX();
                    TMM.Qty = qtyall;
                    lsttask.Add(TMM);
                    return true;
                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }
        private string StockD_GetFilterSql(T_StockInfoEX mo)
        {
            string sql = " ISDEL = 1 ";

            if (mo.MaterialNo != null)
            {
                if (mo.MaterialNo != "" && !mo.MaterialNo.Contains(','))
                    sql += " and MaterialNo like '" + mo.MaterialNo + "%'";
                else if (mo.MaterialNo != "" && mo.MaterialNo.Contains(','))
                {
                    string MaterialNo = mo.MaterialNo;
                    Query_Func.ChangeQuery(ref MaterialNo);
                    sql += " and MaterialNo in(" + MaterialNo + ")";
                }
            }

            if (mo.AreaNo != null)
            {
                if (mo.AreaNo != "" && !mo.AreaNo.Contains(','))
                    sql += " and AreaNo like '" + mo.AreaNo + "%'";
                else if (mo.AreaNo != "" && mo.AreaNo.Contains(','))
                {
                    string AreaNo = mo.AreaNo;
                    Query_Func.ChangeQuery(ref AreaNo);
                    sql += " and AreaNo in(" + AreaNo + ")";
                }
            }

            if (mo.BatchNo != null)
            {
                if (mo.BatchNo != "" && !mo.BatchNo.Contains(','))
                    sql += " and BatchNo like '" + mo.BatchNo + "%'";
                else if (mo.BatchNo != "" && mo.BatchNo.Contains(','))
                {
                    string BatchNo = mo.BatchNo;
                    Query_Func.ChangeQuery(ref BatchNo);
                    sql += " and BatchNo in(" + BatchNo + ")";
                }
            }



            if (!string.IsNullOrEmpty(mo.fserialno))
                sql += " and fserialno = '" + mo.fserialno + "'";

            if (!string.IsNullOrEmpty(mo.MaterialDesc))
                sql += " and MaterialDesc like '%" + mo.MaterialDesc + "%'";

            if (!string.IsNullOrEmpty(mo.StrongHoldCode))
                sql += " and StrongHoldCode = '" + mo.StrongHoldCode + "'";

            if (!string.IsNullOrEmpty(mo.SerialNo))
                sql += " and SerialNo = '" + mo.SerialNo + "'";

            if (!string.IsNullOrEmpty(mo.HouseNo))
                sql += " and HouseNo like '%" + mo.HouseNo + "%'";

            if (!string.IsNullOrEmpty(mo.WarehouseNo))
                sql += " and (WarehouseNo like '%" + mo.WarehouseNo + "%' or WarehouseName like '%" + mo.WarehouseNo + "%') ";



            if (mo.Status != 0)
            {
                sql += " and Status =" + mo.Status;
            }

            if (mo.IsAmount != 0)
            {
                if (mo.IsAmount==2) {
                    sql += " and IsAmount = 2 ";
                } else {
                    sql += " and IsAmount is null ";
                }  
            }
            

            if (!string.IsNullOrEmpty(mo.PalletNo))
                sql += " and PalletNo = '" + mo.PalletNo + "'";


            return sql;

        }





        //库存汇总查询
        private string getsqlinStockCombine(T_StockInfoEX mo)
        {
            string sql = " (select 1 as ID,  MaterialNo, MaterialDesc, WAREHOUSENAME, HouseNAME, AreaNAME, QTY, batchno, edate, EAN from  ( ";
            sql += " select max(s.ID) as ID,m.MaterialNo,m.MaterialDesc,a.WAREHOUSENAME,a.HouseNAME,a.AreaNAME,sum(s.qty) as QTY,s.batchno, ";
            sql += " CONVERT(varchar(12), s.edate, 111) as edate,s.EAN from T_STOCK s ";
            sql += " left join t_material m on s.materialnoid = m.id left join v_area a on s.areaid = a.id ";
            sql += " where s.ISDEL = 1  and s.batchno != '' ";
            sql += " group by  CONVERT(varchar(12), s.edate, 111),m.MaterialNo,m.MaterialDesc,a.WAREHOUSENAME,a.HouseNAME,a.AreaNAME,s.batchno,s.EAN ";
            sql += " union ";
            sql += " select max(s.ID) as ID, MaterialNo, MaterialDesc, a.WAREHOUSENAME, a.HouseNAME, a.AreaNAME, sum(s.qty) as QTY, s.batchno, ";
            sql += " CONVERT(varchar(12), s.edate, 111) as edate, s.EAN from( ";
            sql += " select T_OUTBARCODE.*, t_stock.areaid from T_OUTBARCODE ";
            sql += "  left join t_stock on T_OUTBARCODE.fserialno = t_stock.barcode ";
            sql += " where fserialno in (select barcode from T_STOCK where ISDEL = 1  and batchno = ''))  s ";
            sql += "   left join v_area a on s.areaid = a.id ";
            sql += " group by  CONVERT(varchar(12), s.edate, 111),MaterialNo,MaterialDesc,a.WAREHOUSENAME,a.HouseNAME,a.AreaNAME,s.batchno,s.EAN ";
            sql += " ) a group by MaterialNo, MaterialDesc, WAREHOUSENAME, HouseNAME, AreaNAME, QTY, batchno, edate, EAN)N";



            //string sql = "(select max(s.ID) as ID,m.MaterialNo,m.MaterialDesc,a.WAREHOUSENO,a.HouseNo,a.AreaNo,sum(s.qty)as QTY,s.strongholdcode,s.batchno,s.status,TO_CHAR( s.edate, 'YYYY-MM-DD') as edate,s.EAN from T_STOCK  s left join t_material m on s.materialnoid = m.id left join v_area a on s.areaid = a.id where s.ISDEL = 1 ";


            ////if (mo.Status != 0)
            ////{
            ////    sql += " and s.Status =" + mo.Status;
            ////}

            ////if (mo.ReceiveStatus != 0)
            ////{
            ////    sql += " and s.ReceiveStatus =" + mo.Status;
            ////}
            //sql += " group by TO_CHAR( s.edate, 'YYYY-MM-DD'),m.MaterialNo,s.status,m.MaterialDesc,a.WAREHOUSENO,a.HouseNo,a.AreaNo,s.strongholdcode,s.batchno,s.EAN)N";
            return sql;
        }
        public bool GetStockCombineInfo(T_StockInfoEX taskmo, ref DividPage dividpage, ref List<T_StockInfoEX> lsttask, ref string strErrMsg)
        {
            if (dividpage == null) dividpage = new DividPage();
            lsttask = new List<T_StockInfoEX>();
            try
            {
                Common_FactoryDB funcDB = new Common_FactoryDB();
                using (IDataReader dr = funcDB.QueryByDividPage(ref dividpage, getsqlinStockCombine(taskmo), StockC_GetFilterSql(taskmo), " * ", "Order by ID Desc"))
                {
                    while (dr.Read())
                    {
                        lsttask.Add(StockC_GetModelFromDataReader(dr));
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
                    //T_StockInfoEX TMM = new T_StockInfoEX();
                    //TMM.Qty = qtyall;
                    //lsttask.Add(TMM);
                    return true;
                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }

        public bool GetStockCombineInfo2(T_StockInfoEX taskmo, ref List<T_StockInfoEX> lsttask, ref string strErrMsg)
        {
            lsttask = new List<T_StockInfoEX>();
            string sql = "select * from " + getsqlinStockCombine(taskmo) + " " + StockC_GetFilterSql2(taskmo);
            try
            {
                using (IDataReader dr = dbFactory.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        lsttask.Add(StockC_GetModelFromDataReader(dr));
                    }
                }
                if (lsttask == null || lsttask.Count == 0)
                {
                    strErrMsg = "没有信息！";
                    return false;
                }
                else
                {
                    if (lsttask.Count > 1500)
                    {
                        strErrMsg = "明盘单据行不能大于1500行，请增加查询条件！";
                        lsttask = null;
                        return false;
                    }
                    //T_StockInfoEX TMM = new T_StockInfoEX();
                    //TMM.Qty = qtyall;
                    //lsttask.Add(TMM);
                    return true;
                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }
        private string StockC_GetFilterSql(T_StockInfoEX mo)
        {
            string sql = "where 1=1 ";

            if ( !string.IsNullOrEmpty(mo.MaterialNo) && !mo.MaterialNo.Contains(','))
                sql += " and MaterialNo like '" + mo.MaterialNo + "%'";
            else if (!string.IsNullOrEmpty(mo.MaterialNo)  && mo.MaterialNo.Contains(','))
            {
                string MaterialNo = mo.MaterialNo;
                Query_Func.ChangeQuery(ref MaterialNo);
                sql += " and MaterialNo in(" + MaterialNo + ")";
            }

            if (!string.IsNullOrEmpty(mo.AreaNo) && !mo.AreaNo.Contains(','))
                sql += " and AreaNo like '" + mo.AreaNo + "%'";
            else if (!string.IsNullOrEmpty(mo.AreaNo)&& mo.AreaNo.Contains(','))
            {
                string AreaNo = mo.AreaNo;
                Query_Func.ChangeQuery(ref AreaNo);
                sql += " and AreaNo in(" + AreaNo + ")";
            }

            if (!string.IsNullOrEmpty(mo.BatchNo)  && !mo.BatchNo.Contains(','))
                sql += " and BatchNo like '" + mo.BatchNo + "%'";
            else if (!string.IsNullOrEmpty(mo.BatchNo)  && mo.BatchNo.Contains(','))
            {
                string BatchNo = mo.BatchNo;
                Query_Func.ChangeQuery(ref BatchNo);
                sql += " and BatchNo in(" + BatchNo + ")";
            }
            if (!string.IsNullOrEmpty(mo.EAN) && mo.EAN != "")
                sql += " and EAN = '" + mo.EAN + "'";


            if (!string.IsNullOrEmpty(mo.MaterialDesc) && mo.MaterialDesc != "")
                sql += " and MaterialDesc like '%" + mo.MaterialDesc + "%'";


            if (!string.IsNullOrEmpty(mo.HouseNo) && mo.HouseNo != "")
                sql += " and HouseNo like '%" + mo.HouseNo + "%'";

            if (!string.IsNullOrEmpty(mo.WarehouseNo) && mo.WarehouseNo != "")
                sql += " and WarehouseNo like '%" + mo.WarehouseNo + "%'";





            if (!string.IsNullOrEmpty(mo.StrongHoldCode) && mo.StrongHoldCode != "")
                sql += " and StrongHoldCode = '" + mo.StrongHoldCode + "'";
            if (mo.Status != 0)
            {
                sql += " and Status =" + mo.Status;
            }
            return sql;

        }

        private string StockC_GetFilterSql2(T_StockInfoEX mo)
        {
            string sql = "where 1=1 ";

            if (mo.WarehouseNo != "")
                sql += " and WarehouseNo = '" + mo.WarehouseNo + "'";

            if (mo.MaterialNo != "" && !mo.MaterialNo.Contains(','))
                sql += " and MaterialNo like '" + mo.MaterialNo + "%'";
            else if (mo.MaterialNo != "" && mo.MaterialNo.Contains(','))
            {
                string MaterialNo = mo.MaterialNo;
                Query_Func.ChangeQuery(ref MaterialNo);
                sql += " and MaterialNo in(" + MaterialNo + ")";
            }

            if (mo.HouseNo != "" && !mo.HouseNo.Contains(','))
                sql += " and HouseNo = '" + mo.HouseNo + "'";
            else if (mo.HouseNo != "" && mo.HouseNo.Contains(','))
            {
                string HouseNo = mo.HouseNo;
                Query_Func.ChangeQuery(ref HouseNo);
                sql += " and HouseNo in(" + HouseNo + ")";
            }

            if (mo.AreaNo != "" && !mo.AreaNo.Contains(','))
                sql += " and AreaNo = '" + mo.AreaNo + "'";
            else if (mo.AreaNo != "" && mo.AreaNo.Contains(','))
            {
                string AreaNo = mo.AreaNo;
                Query_Func.ChangeQuery(ref AreaNo);
                sql += " and AreaNo in(" + AreaNo + ")";
            }


            if (mo.BatchNo != "" && !mo.BatchNo.Contains(','))
                sql += " and BatchNo = '" + mo.BatchNo + "'";
            else if (mo.BatchNo != "" && mo.BatchNo.Contains(','))
            {
                string BatchNo = mo.BatchNo;
                Query_Func.ChangeQuery(ref BatchNo);
                sql += " and BatchNo in(" + BatchNo + ")";
            }


            if (mo.StrongHoldCode != "" && !mo.StrongHoldCode.Contains(','))
                sql += " and StrongHoldCode = '" + mo.StrongHoldCode + "'";
            else if (mo.StrongHoldCode != "" && mo.StrongHoldCode.Contains(','))
            {
                string StrongHoldCode = mo.StrongHoldCode;
                Query_Func.ChangeQuery(ref StrongHoldCode);
                sql += " and StrongHoldCode in(" + StrongHoldCode + ")";
            }


            return sql;

        }
        public T_StockInfoEX StockC_GetModelFromDataReader(IDataReader dr)
        {
            T_StockInfoEX TMM = new T_StockInfoEX();
            TMM.XH = ++xh + "";
            TMM.BatchNo = (dr["BatchNo"] ?? "").ToString();

            TMM.MaterialNo = (dr["MaterialNo"] ?? "").ToString();
            TMM.EAN = (dr["EAN"] ?? "").ToString();
            TMM.MaterialDesc = (dr["MaterialDesc"] ?? "").ToString();
            TMM.WarehouseNo = (dr["WarehouseNAME"] ?? "").ToString();
            TMM.HouseNo = (dr["HouseNAME"] ?? "").ToString();
            TMM.AreaNo = (dr["AreaNAME"] ?? "").ToString();
            //TMM.StrongHoldCode = dr["StrongHoldCode"].ToDBString();
            TMM.Qty = (decimal)dr["Qty"];
            TMM.EDate = dr["EDate"].ToDateTime();
            //TMM.Status = dr["Status"].ToInt32();
            //switch (TMM.Status)
            //{
            //    case 1:
            //        TMM.StatusName = "待检";
            //        break;
            //    case 2:
            //        TMM.StatusName = "送检";
            //        break;
            //    case 3:
            //        TMM.StatusName = "检验合格";
            //        break;
            //    case 4:
            //        TMM.StatusName = "检验不合格";
            //        break;
            //}
            qtyall += (decimal)TMM.Qty;
            return TMM;
        }


        //流水表
        public bool GetTaskTransInfo(TaskTrans_Model taskmo, ref DividPage dividpage, ref List<TaskTrans_Model> lsttask, ref string strErrMsg)
        {
            List<VoucherType> lstvou = GetVoucherType();
            if (dividpage == null) dividpage = new DividPage();
            lsttask = new List<TaskTrans_Model>();
            try
            {
                Common_FactoryDB fbd = new Common_FactoryDB();
                using (IDataReader dr = fbd.QueryByDividPage(ref dividpage, "V_TaskTran", TASKTRANS_GetFilterSql(taskmo), " * ", "Order by ID Desc"))
                {
                    while (dr.Read())
                    {
                        lsttask.Add(TASKTRANS_GetModelFromDataReader(dr, lstvou));
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
                    TaskTrans_Model TMM = new TaskTrans_Model();
                    TMM.QTY = qtyall;
                    lsttask.Add(TMM);
                    return true;
                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }



        private string TASKTRANS_GetFilterSql(TaskTrans_Model mo)
        {
            string sql = "where 1=1 ";

            if (mo.MATERIALNO!=null)
            {
                if (mo.MATERIALNO != "" && !mo.MATERIALNO.Contains(','))
                    sql += " and MATERIALNO like '" + mo.MATERIALNO + "%'";
                else if (mo.MATERIALNO != "" && mo.MATERIALNO.Contains(','))
                {
                    string MaterialNo = mo.MATERIALNO;
                    Query_Func.ChangeQuery(ref MaterialNo);
                    sql += " and MATERIALNO in(" + MaterialNo + ")";
                }
            }

            if (!string.IsNullOrEmpty(mo.MATERIALDESC))
                sql += " and MATERIALDESC like '%" + mo.MATERIALDESC + "%'";

            if (!string.IsNullOrEmpty(mo.SERIALNO))
                sql += " and SERIALNO = '" + mo.SERIALNO + "'";

            //if (mo.PalletNo != "")
            //    sql += " and PalletNo like '%" + mo.PalletNo + "%'";

            if (!string.IsNullOrEmpty(mo.FROMHOUSENO))
                sql += " and (FROMHOUSENO like '%" + mo.FROMHOUSENO + "%' or TOHOUSENO like '%" + mo.FROMHOUSENO + "%')";

            if (!string.IsNullOrEmpty(mo.FROMAREANO))
                sql += " and (FROMAREANO like '%" + mo.FROMAREANO + "%' or TOAREANO like '%" + mo.FROMAREANO + "%')";

            if (!string.IsNullOrEmpty(mo.FROMWAREHOUSENO))
                sql += " and (FROMWAREHOUSENO like '%" + mo.FROMWAREHOUSENO + "%' or TOWAREHOUSENO like '%" + mo.FROMWAREHOUSENO + "%')";

            if (!string.IsNullOrEmpty(mo.BATCHNO))
                sql += " and BATCHNO = '" + mo.BATCHNO + "'";

            if (!string.IsNullOrEmpty(mo.CREATER))
                sql += " and CREATER = '" + mo.CREATER + "'";

            if (!string.IsNullOrEmpty(mo.ERPVOUCHERNO))
                sql += " and ERPVOUCHERNO like '%" + mo.ERPVOUCHERNO + "%'";

            if (mo.TASKTYPE != 0)
            {
                sql += " and TASKTYPE =" + mo.TASKTYPE;
            }

            if (mo.VOUCHERTYPE != 0)
            {
                sql += " and VOUCHERTYPE =" + mo.VOUCHERTYPE;
            }
            if (!string.IsNullOrEmpty(mo.SUPCUSCODE))
                sql += " and SUPCUSCODE like '%" + mo.SUPCUSCODE + "%'";

            if (!string.IsNullOrEmpty(mo.StrongHoldCode))
                sql += " and StrongHoldCode = '" + mo.StrongHoldCode + "'";
            //if (mo.SUPCUSNAME != "")
            //    sql += " and SUPCUSNAME like '%" + mo.SUPCUSNAME + "%'";

            if (mo.begintime != null)
            {
                sql += " and CREATETIME>='" + mo.begintime + "'";
            }
            if (mo.endtime != null)
            {
                sql += " and CREATETIME<='" + mo.endtime + "'";
            }

            //用于存放任务类型查找条件
            if (!string.IsNullOrEmpty(mo.StatusName))
            {
                string strTaskType = string.Empty;
                string[] strSplit = mo.StatusName.Split(',');
                int sLen = strSplit.Length;

                foreach (var item in strSplit)
                {
                    switch (item.Trim())
                    {
                        case "收货":
                            strTaskType += "4" + (sLen > 1 ? "," : "");
                            break;
                        case "上架":
                            strTaskType += "1" + (sLen > 1 ? "," : "");
                            break;
                        case "下架":
                            strTaskType += "2" + (sLen > 1 ? "," : "");
                            break;
                        case "移库":
                            strTaskType += "3" + (sLen > 1 ? "," : "");
                            break;
                        case "完工入库":
                            strTaskType += "5" + (sLen > 1 ? "," : "");
                            break;
                        case "退料入库":
                            strTaskType += "6" + (sLen > 1 ? "," : "");
                            break;
                        case "交接入库":
                            strTaskType += "7" + (sLen > 1 ? "," : "");
                            break;
                        case "齐套":
                            strTaskType += "8" + (sLen > 1 ? "," : "");
                            break;
                        case "领料出库":
                            strTaskType += "9" + (sLen > 1 ? "," : "");
                            break;
                        case "制成检":
                            strTaskType += "10" + (sLen > 1 ? "," : "");
                            break;
                        case "复核":
                            strTaskType += "12" + (sLen > 1 ? "," : "");
                            break;
                        case "包材接收":
                            strTaskType += "13" + (sLen > 1 ? "," : "");
                            break;
                        case "法规检":
                            strTaskType += "18" + (sLen > 1 ? "," : "");
                            break;
                        default:
                            strTaskType += "0" + (sLen > 1 ? "," : "");
                            break;
                    }
                }
                strTaskType = strTaskType.TrimEnd(',');
                sql += " and TASKTYPE in (" + strTaskType + ")";

            }
            return sql;

        }
        public TaskTrans_Model TASKTRANS_GetModelFromDataReader(IDataReader dr, List<VoucherType> lstvou)
        {
            TaskTrans_Model TMM = new TaskTrans_Model();
            TMM.Edate = dr["edate"].ToDateTime();
            TMM.XH = ++xh + "";
            TMM.FROMAREANO = (dr["FROMAREANO"] ?? "").ToString();
            TMM.FROMHOUSENO = dr["FROMHOUSENO"].ToDBString();
            TMM.FROMWAREHOUSENO = dr["FROMWAREHOUSENO"].ToDBString();
            TMM.ERPVOUCHERNO = (dr["ERPVOUCHERNO"] ?? "").ToString();
            TMM.TOAREANO = (dr["TOAREANO"] ?? "").ToString();
            TMM.TOHOUSENO = (dr["TOHOUSENO"] ?? "").ToString();
            TMM.TOWAREHOUSENO = (dr["TOWAREHOUSENO"] ?? "").ToString();
            TMM.Status = dr["Status"].ToInt32();
            switch (TMM.Status)
            {
                case 1:
                    TMM.StatusName = "待检";
                    break;
                case 2:
                    TMM.StatusName = "送检";
                    break;
                case 3:
                    TMM.StatusName = "合格";
                    break;
                case 4:
                    TMM.StatusName = "不合格";
                    break;
                default:
                    TMM.StatusName = "";
                    break;
            }
            TMM.SERIALNO = (dr["SERIALNO"] ?? "").ToString();
            TMM.MATERIALNO = (dr["MATERIALNO"] ?? "").ToString();
            TMM.MATERIALDESC = (dr["MATERIALDESC"] ?? "").ToString();
            TMM.StrongHoldCode = dr["StrongHoldCode"].ToDBString();
            TMM.StrongHoldName = dr["StrongHoldName"].ToDBString();

            TMM.CREATER = (dr["CREATER"] ?? "").ToString();
            TMM.TASKNO = (dr["TASKNO"] ?? "").ToString();
            TMM.QTY = dr["QTY"].ToDecimal();
            TMM.itemqty = dr["itemqty"].ToDecimal();
            TMM.SUPCUSCODE = (dr["SUPCUSCODE"] ?? "").ToString();
            TMM.CREATETIME = Convert.ToDateTime(dr["CREATETIME"]);
            TMM.ROWNO = (dr["ROWNO"] ?? "").ToString();
            TMM.ROWNODEL = dr["ROWNODEL"].ToDBString();
            TMM.BATCHNO = (dr["BATCHNO"] ?? "").ToString();
            TMM.ProductDate = dr["ProductDate"].ToDateTime();
            TMM.SupPrdBatch = dr["SupPrdBatch"].ToDBString();
            TMM.UNIT = dr["UNIT"].ToDBString();
            TMM.BARCODE = dr["BARCODE"].ToDBString();
            if (dr["VOUCHERTYPE"] is DBNull)
                TMM.vouchertypename = null;
            else
            {
                int a = (Convert.ToInt32(dr["VOUCHERTYPE"]));
                if (lstvou.Find(p => p.vouchertype == a) != null)
                    TMM.vouchertypename = lstvou.Find(p => p.vouchertype == a).vouchername;
                else
                    TMM.vouchertypename = null;
            }

            if (dr["TASKTYPE"] is DBNull)
                TMM.tasktypename = null;
            else
            {
                if (Convert.ToInt32(dr["TASKTYPE"]) == 1)
                {
                    TMM.tasktypename = "上架";
                }
                else if (Convert.ToInt32(dr["TASKTYPE"]) == 2)
                {
                    TMM.tasktypename = "下架";
                }
                else if (Convert.ToInt32(dr["TASKTYPE"]) == 3)
                {
                    TMM.tasktypename = "移库";
                }
                else if (Convert.ToInt32(dr["TASKTYPE"]) == 4)
                {
                    TMM.tasktypename = "收货";
                }
                else if (Convert.ToInt32(dr["TASKTYPE"]) == 5)
                {
                    TMM.tasktypename = "完工入库";
                }
                else if (Convert.ToInt32(dr["TASKTYPE"]) == 6)
                {
                    TMM.tasktypename = "退料入库";
                }
                else if (Convert.ToInt32(dr["TASKTYPE"]) == 7)
                {
                    TMM.tasktypename = "交接入库";
                }
                else if (Convert.ToInt32(dr["TASKTYPE"]) == 8)
                {
                    TMM.tasktypename = "齐套";
                }
                else if (Convert.ToInt32(dr["TASKTYPE"]) == 9)
                {
                    TMM.tasktypename = "领料出库";
                }
                else if (Convert.ToInt32(dr["TASKTYPE"]) == 12)
                {
                    TMM.tasktypename = "复核";
                }
            }
            qtyall += (decimal)TMM.QTY;
            return TMM;
        }


        //流水合并
        public bool GetTaskTransInfo2(TaskTrans_Model taskmo, ref DividPage dividpage, ref List<TaskTrans_Model> lsttask, ref string strErrMsg)
        {
            List<VoucherType> lstvou = GetVoucherType();
            if (dividpage == null) dividpage = new DividPage();
            lsttask = new List<TaskTrans_Model>();
            try
            {
                string sql = "select ROW_NUMBER() OVER(Order by t.id desc) AS PageRowNumber , a.AREANO as fromAREANO,a.houseno as fromhouseno,a.warehouseno as fromwarehouseno,b.AREANO as toAREANO,b.houseno as tohouseno,b.warehouseno as towarehouseno,t.* from " +
                        "(select max(t.id) as id,t.strongholdcode,t.materialno,t.materialdesc,sum(t.qty) as qty,t.unit,t.batchno,t.vouchertype,t.tasktype,t.creater ,t.fromareaid,t.toareaid" +
                        " from  t_tasktrans t " + TASKTRANS_GetFilterSql2(taskmo) +
                        " group by t.strongholdcode,t.materialno,t.materialdesc,t.unit,t.batchno,t.vouchertype,t.tasktype,t.creater,t.fromareaid,t.toareaid) t" +
                        " left join v_area a on t.fromareaid = a.id" +
                        " left join v_area b on t.toareaid = b.id " + TASKTRANS_GetFilterSql3(taskmo);
                DataTable dt = Common_DB.QueryByDividPage(ref dividpage, sql);
                dividpage.CurrentPageRecordCounts = dt.Rows.Count;
                lsttask = Print_DB.ConvertToModel<TaskTrans_Model>(dt);


                if (lsttask == null || lsttask.Count == 0)
                {
                    strErrMsg = "没有信息！";
                    return false;
                }
                else
                {
                    for (int i = 1; i <= lsttask.Count; i++)
                    {

                        lsttask[i - 1].XH = i.ToString();
                        qtyall += lsttask[i - 1].QTY;
                        int a = (Convert.ToInt32(lsttask[i - 1].VOUCHERTYPE));
                        var tr = lstvou.Find(p => p.vouchertype == a);
                        if (tr != null)
                            lsttask[i - 1].vouchertypename = tr.vouchername;
                        else
                            lsttask[i - 1].vouchertypename = null;
                        switch (lsttask[i - 1].TASKTYPE)
                        {
                            case 1:
                                lsttask[i - 1].tasktypename = "上架";
                                break;
                            case 2:
                                lsttask[i - 1].tasktypename = "下架";
                                break;
                            case 3:
                                lsttask[i - 1].tasktypename = "移库";
                                break;
                            case 4:
                                lsttask[i - 1].tasktypename = "收货";
                                break;
                            case 5:
                                lsttask[i - 1].tasktypename = "完工入库";
                                break;
                            case 6:
                                lsttask[i - 1].tasktypename = "退料入库";
                                break;
                            case 7:
                                lsttask[i - 1].tasktypename = "交接入库";
                                break;
                            case 8:
                                lsttask[i - 1].tasktypename = "齐套";
                                break;
                            case 9:
                                lsttask[i - 1].tasktypename = "领料出库";
                                break;
                            case 12:
                                lsttask[i - 1].tasktypename = "复核";
                                break;
                            default:
                                lsttask[i - 1].tasktypename = "";
                                break;
                        }

                    }
                    TaskTrans_Model TMM = new TaskTrans_Model();
                    TMM.QTY = qtyall;
                    lsttask.Add(TMM);
                    return true;
                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }
        private string TASKTRANS_GetFilterSql2(TaskTrans_Model mo)
        {
            string sql = "where 1=1 ";

            if (mo.MATERIALNO != "" && !mo.MATERIALNO.Contains(','))
                sql += " and T.MATERIALNO = '" + mo.MATERIALNO + "'";
            else if (mo.MATERIALNO != "" && mo.MATERIALNO.Contains(','))
            {
                string MaterialNo = mo.MATERIALNO;
                Query_Func.ChangeQuery(ref MaterialNo);
                sql += " and T.MATERIALNO in(" + MaterialNo + ")";
            }

            if (mo.MATERIALDESC != "")
                sql += " and T.MATERIALDESC like '%" + mo.MATERIALDESC + "%'";



            //if (mo.FROMHOUSENO != "")
            //    sql += " and (A.HOUSENO like '%" + mo.FROMHOUSENO + "%' or B.HOUSENO like '%" + mo.FROMHOUSENO + "%')";

            //if (mo.FROMAREANO != "")
            //    sql += " and (A.AREANO like '%" + mo.FROMAREANO + "%' or B.AREANO like '%" + mo.FROMAREANO + "%')";

            //if (mo.FROMWAREHOUSENO != "")
            //    sql += " and (A.WAREHOUSENO = '" + mo.FROMWAREHOUSENO + "' or B.WAREHOUSENO = '" + mo.FROMWAREHOUSENO + "')";

            if (mo.BATCHNO != "")
                sql += " and T.BATCHNO = '" + mo.BATCHNO + "'";

            if (mo.CREATER != "")
                sql += " and T.CREATER = '" + mo.CREATER + "'";

            if (mo.ERPVOUCHERNO != "")
                sql += " and T.ERPVOUCHERNO like '%" + mo.ERPVOUCHERNO + "%'";

            if (mo.TASKTYPE != 0)
            {
                sql += " and T.TASKTYPE =" + mo.TASKTYPE;
            }

            if (mo.VOUCHERTYPE != 0)
            {
                sql += " and T.VOUCHERTYPE =" + mo.VOUCHERTYPE;
            }
            //if (mo.SUPCUSCODE != "")
            //    sql += " and SUPCUSCODE like '%" + mo.SUPCUSCODE + "%'";

            if (mo.StrongHoldCode != "")
                sql += " and T.StrongHoldCode = '" + mo.StrongHoldCode + "'";
            //if (mo.SUPCUSNAME != "")
            //    sql += " and SUPCUSNAME like '%" + mo.SUPCUSNAME + "%'";

            if (mo.begintime != null)
            {
                sql += "and t.CREATETIME>=to_date('" + mo.begintime + "','YYYY/MM/DD') ";
            }
            if (mo.endtime != null)
            {
                sql += "and t.CREATETIME<=to_date('" + mo.endtime + "','YYYY/MM/DD') ";
            }
            return sql;

        }

        private string TASKTRANS_GetFilterSql3(TaskTrans_Model mo)
        {
            string sql = " where 1=1 ";

            if (mo.FROMHOUSENO != "")
                sql += " and (A.HOUSENO like '%" + mo.FROMHOUSENO + "%' or B.HOUSENO like '%" + mo.FROMHOUSENO + "%')";

            if (mo.FROMAREANO != "")
                sql += " and (A.AREANO like '%" + mo.FROMAREANO + "%' or B.AREANO like '%" + mo.FROMAREANO + "%')";

            if (mo.FROMWAREHOUSENO != "")
                sql += " and (A.WAREHOUSENO = '" + mo.FROMWAREHOUSENO + "' or B.WAREHOUSENO = '" + mo.FROMWAREHOUSENO + "')";
            return sql;

        }





        public List<string> GetWmsStock()
        {
            string sql = "select partno||substr(SERIALNO,instr(SERIALNO,'@',1)) as se from T_STOCK";
            List<string> list = new List<string>();
            using (var dr = dbFactory.ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    string a = dr["se"].ToString();
                    list.Add(a);
                }
            }
            return list;
        }





        public bool GetAllQitao(qitao_Model model, ref DividPage dividpage, ref List<qitao_Model> lsttask, ref string strErrMsg)
        {
            if (dividpage == null) dividpage = new DividPage();
            lsttask = new List<qitao_Model>();
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("select MATERIALNO,MATERIALDESC,CREATETIME,CREATER,ERPVOUCHERNO,BATCHNO,SERIALNO,QTY,LINEMANAGENO,ERPNO from t_QITAO where 1=1 ");

                if (model.SERIALNO != "")
                {
                    strb.Append(" and SERIALNO like '%" + model.SERIALNO + "%'");
                }
                if (model.ERPNO != "")
                {
                    strb.Append(" and ERPNO like '%" + model.ERPNO + "%'");
                }
                if (model.CREATER != "")
                {
                    strb.Append(" and CREATER = '" + model.CREATER + "'");
                }
                if (model.LINEMANAGENO != "")
                {
                    strb.Append(" and LINEMANAGENO='" + model.LINEMANAGENO + "'");
                }
                if (model.FROMTIME != DateTime.MinValue)
                {
                    strb.Append(" and CREATETIME>to_date('" + model.FROMTIME + "','YYYY-MM-DD hh24:mi:ss')");
                }
                if (model.TOTIME != DateTime.MinValue)
                {
                    strb.Append(" and CREATETIME<to_date('" + model.TOTIME + "','YYYY-MM-DD hh24:mi:ss')");
                }

                strb.Append(" order by CREATETIME");

                DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, strb.ToString()).Tables[0];

                dividpage.CurrentPageRecordCounts = dt.Rows.Count;
                lsttask = Print_DB.ConvertToModel<qitao_Model>(dt);

                if (lsttask == null || lsttask.Count == 0)
                {
                    strErrMsg = "没有信息！";
                    return false;
                }
                else
                {

                    for (int i = 1; i <= lsttask.Count; i++)
                    {
                        sumslls += lsttask[i - 1].QTY.ToDecimal();
                    }
                    qitao_Model TMM = new qitao_Model();
                    TMM.QTY = sumslls;
                    lsttask.Add(TMM);
                    return true;
                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                return false;
            }
        }

        public bool GetHistory(Linehistory model, ref DividPage dividpage, ref List<Linehistory> lsttask, ref string strErrMsg)
        {
            if (dividpage == null) dividpage = new DividPage();
            lsttask = new List<Linehistory>();
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("select * from ( ");
                strb.Append(" select p.*, q.Numuser from(select o.*, x.productqty, round((o.postnum / x.productqty) * 100, 1) || '%' perc,x.materialno materialdesc from(select ab.*, ac.postnum from(select erpvoucherno, productteamno, sum(sumtime) sumtime from( ");
                strb.Append(" select a.*, d.productteamno from t_pro_linemanagehistory a left ");
                strb.Append(" join t_linemanagemodel d  on a.linemanageno = d.id) dd  group by erpvoucherno, productteamno) ab left join(select erpvoucherno, productteamno, isnull(sum(postnum), 0) postnum from(select erpvoucherno, productteamno, postnum from(select a.*, d.productteamno from t_pro_linemanagehistory a left ");
                strb.Append(" join t_linemanagemodel d  on a.linemanageno = d.id) group by erpvoucherno, productteamno, Totime, postnum) group by erpvoucherno, productteamno ");
                strb.Append(" ) ac on ab.erpvoucherno = ac.erpvoucherno and ab.productteamno = ac.productteamno) o inner join t_woinfo x on o.erpvoucherno = x.erpvoucherno  and x.isdel=1 ");
                strb.Append(" ) p left join(select cc.erpvoucherno, cc.productteamno, count(cc.userno) Numuser from( ");
                strb.Append(" select DISTINCT(aa.userno), aa.erpvoucherno, bb.productteamno from t_pro_linemanageuser aa left ");
                strb.Append(" join t_linemanagemodel bb on aa.linemanageid = bb.id  where aa.status = 2 ) cc ");
                strb.Append(" group by erpvoucherno, productteamno) q on p.erpvoucherno = q.erpvoucherno and p.productteamno = q.productteamno) aaa where 1 = 1 ");

                if (model.ERPVOUCHERNO != "")
                {
                    strb.Append(" and ERPVOUCHERNO = '" + model.ERPVOUCHERNO + "'");
                }
                if (model.PRODUCTTEAMNO != "")
                {
                    strb.Append(" and PRODUCTTEAMNO = '" + model.PRODUCTTEAMNO + "'");
                }

                DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, strb.ToString()).Tables[0];

                dividpage.CurrentPageRecordCounts = dt.Rows.Count;
                lsttask = Print_DB.ConvertToModel<Linehistory>(dt);

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


        public bool GetHistoryForLook(ref List<Linehistory> lsttask, ref string strErrMsg)
        {
            lsttask = new List<Linehistory>();
            try
            {
                String aaa = @"select rownum,bbb.* from ( select *
                                    from (select p.*, q.Numuser
                                            from (select o.*,
                                                        x.productqty,
                                                        round((o.postnum / x.productqty) * 100, 1) || '%' perc,
                                                        x.materialno materialdesc
                                                    from (select ab.*, ac.postnum
                                                            from (select erpvoucherno,
                                                                        productteamno,
                                                                        sum(sumtime) sumtime
                                                                    from (select a.*, d.productteamno
                                                                            from t_pro_linemanagehistory a
                                                                            left join t_linemanagemodel d
                                                                            on a.linemanageno = d.id) dd
                                                                    group by erpvoucherno, productteamno) ab
                                                            left join (select erpvoucherno,
                                                                            productteamno,
                                                                            isnull(sum(postnum), 0) postnum
                                                                        from (select erpvoucherno,
                                                                                    productteamno,
                                                                                    postnum
                                                                                from (select a.*, d.productteamno
                                                                                        from t_pro_linemanagehistory a
                                                                                        left join t_linemanagemodel d
                                                                                        on a.linemanageno = d.id)
                                                                                group by erpvoucherno,
                                                                                        productteamno,
                                                                                        Totime,
                                                                                        postnum)
                                                                        group by erpvoucherno, productteamno) ac
                                                            on ab.erpvoucherno = ac.erpvoucherno
                                                            and ab.productteamno = ac.productteamno) o
                                                    inner join (select t.erpvoucherno,t.materialno,t.productqty
                                                                from t_woinfo t
                                                                inner join (select t.erpvoucherno
                                                                            from T_PRO_LINEMANAGEHISTORY t
                                                                            where t.fromtime >
                                                                                to_date(to_char(sysdate,
                                                                                        'yyyy-MM-dd'),'yyyy-MM-dd')
                                                                            group by t.erpvoucherno) bg
                                                                on t.erpvoucherno = bg.erpvoucherno
                                                                where t.isdel = '1') x
                                                    on o.erpvoucherno = x.erpvoucherno) p
                                            left join (select cc.erpvoucherno,
                                                            cc.productteamno,
                                                            count(cc.userno) Numuser
                                                        from (select DISTINCT (aa.userno),
                                                                            aa.erpvoucherno,
                                                                            bb.productteamno
                                                                from t_pro_linemanageuser aa
                                                                left join t_linemanagemodel bb
                                                                on aa.linemanageid = bb.id
                                                                where aa.status = 2) cc
                                                        group by erpvoucherno, productteamno) q
                                            on p.erpvoucherno = q.erpvoucherno
                                            and p.productteamno = q.productteamno) aaa
                                    where 1 = 1
                                    order by aaa.erpvoucherno) bbb";


                StringBuilder strb = new StringBuilder();
                //strb.Append("select * from ( ");
                //strb.Append(" select p.*, q.Numuser from(select o.*, x.productqty, round((o.postnum / x.productqty) * 100, 1) || '%' perc,x.materialno materialdesc from(select ab.*, ac.postnum from(select erpvoucherno, productteamno, sum(sumtime) sumtime from( ");
                //strb.Append(" select a.*, d.productteamno from t_pro_linemanagehistory a left ");
                //strb.Append(" join t_linemanagemodel d  on a.linemanageno = d.id) dd  group by erpvoucherno, productteamno) ab left join(select erpvoucherno, productteamno, isnull(sum(postnum), 0) postnum from(select erpvoucherno, productteamno, postnum from(select a.*, d.productteamno from t_pro_linemanagehistory a left ");
                //strb.Append(" join t_linemanagemodel d  on a.linemanageno = d.id) group by erpvoucherno, productteamno, Totime, postnum) group by erpvoucherno, productteamno ");
                //strb.Append(" ) ac on ab.erpvoucherno = ac.erpvoucherno and ab.productteamno = ac.productteamno) o inner join (select t.erpvoucherno,t.materialno,t.productqty from t_woinfo t ");
                //strb.Append(" inner join (select t.erpvoucherno from T_PRO_LINEMANAGEHISTORY t where t.fromtime > to_date(to_char(sysdate, 'yyyy-MM-dd'),'yyyy-MM-dd') group by t.erpvoucherno) bg ");
                //strb.Append(" on t.erpvoucherno = bg.erpvoucherno where t.isdel = '1') x on o.erpvoucherno = x.erpvoucherno  and x.isdel=1 ");
                //strb.Append(" ) p left join(select cc.erpvoucherno, cc.productteamno, count(cc.userno) Numuser from( ");
                //strb.Append(" select DISTINCT(aa.userno), aa.erpvoucherno, bb.productteamno from t_pro_linemanageuser aa left ");
                //strb.Append(" join t_linemanagemodel bb on aa.linemanageid = bb.id  where aa.status = 2 ) cc ");
                //strb.Append(" group by erpvoucherno, productteamno) q on p.erpvoucherno = q.erpvoucherno and p.productteamno = q.productteamno) aaa where 1 = 1 ");

                DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, aaa).Tables[0];

                lsttask = Print_DB.ConvertToModel<Linehistory>(dt);

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


        public List<Linehistory> GetHistoryForLooknew()
        {
            try
            {
                String aaa = @"select *
                                    from (select p.*, q.Numuser
                                            from (select o.*,
                                                        x.productqty,
                                                        round((o.postnum / x.productqty) * 100, 1) || '%' perc,
                                                        x.materialno materialdesc
                                                    from (select ab.*, ac.postnum
                                                            from (select erpvoucherno,
                                                                        productteamno,
                                                                        sum(sumtime) sumtime
                                                                    from (select a.*, d.productteamno
                                                                            from t_pro_linemanagehistory a
                                                                            left join t_linemanagemodel d
                                                                            on a.linemanageno = d.id) dd
                                                                    group by erpvoucherno, productteamno) ab
                                                            left join (select erpvoucherno,
                                                                            productteamno,
                                                                            isnull(sum(postnum), 0) postnum
                                                                        from (select erpvoucherno,
                                                                                    productteamno,
                                                                                    postnum
                                                                                from (select a.*, d.productteamno
                                                                                        from t_pro_linemanagehistory a
                                                                                        left join t_linemanagemodel d
                                                                                        on a.linemanageno = d.id)
                                                                                group by erpvoucherno,
                                                                                        productteamno,
                                                                                        Totime,
                                                                                        postnum)
                                                                        group by erpvoucherno, productteamno) ac
                                                            on ab.erpvoucherno = ac.erpvoucherno
                                                            and ab.productteamno = ac.productteamno) o
                                                    inner join (select t.erpvoucherno,t.materialno,t.productqty
                                                                from t_woinfo t
                                                                inner join (select t.erpvoucherno
                                                                            from T_PRO_LINEMANAGEHISTORY t
                                                                            where t.fromtime >
                                                                                to_date(to_char(sysdate,
                                                                                        'yyyy-MM-dd'),'yyyy-MM-dd')
                                                                            group by t.erpvoucherno) bg
                                                                on t.erpvoucherno = bg.erpvoucherno
                                                                where t.isdel = '1') x
                                                    on o.erpvoucherno = x.erpvoucherno) p
                                            left join (select cc.erpvoucherno,
                                                            cc.productteamno,
                                                            count(cc.userno) Numuser
                                                        from (select DISTINCT (aa.userno),
                                                                            aa.erpvoucherno,
                                                                            bb.productteamno
                                                                from t_pro_linemanageuser aa
                                                                left join t_linemanagemodel bb
                                                                on aa.linemanageid = bb.id
                                                                where aa.status = 2) cc
                                                        group by erpvoucherno, productteamno) q
                                            on p.erpvoucherno = q.erpvoucherno
                                            and p.productteamno = q.productteamno) aaa
                                    where 1 = 1
                                    order by aaa.erpvoucherno";
                StringBuilder strb = new StringBuilder();
                DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, aaa).Tables[0];

                List<Linehistory> lsttask = new List<Linehistory>();
                lsttask = Print_DB.ConvertToModel<Linehistory>(dt);
                return lsttask;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #region 代理商
        //插入出入库记录表 tasktype 13:收 14：发  EXCHNAME:标识符号
        public T_OutBarCodeInfo InsetTaskTrans(string UserName,string barcode,string tasktype,string EXCHNAME, ref string StrMsg)
        {
            try
            {
                T_OutBarCodeInfo model = GetBarCode(barcode);
                if (model == null)
                {
                    StrMsg = "未能获取条码信息！";
                    return null;
                }
                else {
                    List<string> lstSql = new List<string>();
                    string strSql = "insert into t_tasktrans(Serialno, Materialno, Materialdesc,Qty, Tasktype, Creater, Createtime,Unit,materialnoid,Strongholdcode,Strongholdname,Companycode,Edate,Batchno,Barcode,FromWarehouseNo,ToWarehouseName,EXCHNAME)" +
                            " values ('"+model.SerialNo+ "'Serialno,'" + model.MaterialNo + "','" + model.MaterialDesc + "'," + model.Qty + ", '"+ tasktype + "','" + model.SerialNo + 
                            "' ,'"+ UserName + "', GETDATE(),'" + model.Unit + "'," + model.MaterialNoID + ",'" + model.StrongHoldCode + "','" + model.StrongHoldName + "','" + model.CompanyCode + "','" + model.EDate + "','" + model.BatchNo + "','" + model.BarCode + "','','','" + EXCHNAME+ "')";
                    lstSql.Add(strSql);
                    int count = dbFactory.ExecuteNonQueryList(lstSql, ref StrMsg);
                    if (count<=0)
                        return null;
                    else
                        return model;
                }
            }
            catch (Exception ex)
            {
                StrMsg = ex.ToString();
                return null;
            }
            
        }

        //检查条码是否在本次扫描里面
        public bool CheckBarCode(string barcode, string EXCHNAME)
        {
            try
            {
                T_OutBarCodeInfo model = new T_OutBarCodeInfo();
                string strSql = "select * from  T_TASKTRANS  where EXCHNAME='" + EXCHNAME + "' and barcode='" + barcode + "'";
                using (IDataReader reader = dbFactory.ExecuteReader(strSql))
                {
                    if (reader.Read())
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //获取条码信息
        public T_OutBarCodeInfo GetBarCode(string barcode)
        {
            try
            {
                T_OutBarCodeInfo model = new T_OutBarCodeInfo();
                string strSql = "select Serialno,Materialno, Materialdesc,Qty,Unit,materialnoid,Strongholdcode,Strongholdname,Companycode,Edate,Batchno from  t_outbarcode  where barcode='"+barcode+"'";
                using (IDataReader reader = dbFactory.ExecuteReader(strSql))
                {
                    if (reader.Read())
                    {
                        model.SerialNo = reader["SerialNo"].ToDBString();
                        model.MaterialNo = reader["MaterialNo"].ToDBString();
                        model.MaterialDesc = reader["Materialdesc"].ToDBString();
                        model.BatchNo = reader["BatchNo"].ToDBString();
                        model.Qty = reader["Qty"].ToDecimal();
                        model.EDate = reader["EDate"].ToDateTime();
                        model.BatchNo = reader["BatchNo"].ToDBString();
                        model.Unit = reader["Unit"].ToDBString();
                        model.MaterialNoID = reader["materialnoid"].ToInt32();
                        model.StrongHoldCode = reader["Strongholdcode"].ToDBString();
                        model.StrongHoldName = reader["Strongholdname"].ToDBString();
                        model.CompanyCode = reader["Companycode"].ToDBString();
                        model.BarCode = barcode;
                    }
                    else
                    {
                        model = null;
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //获取本次的所有条码信息
        public List<TaskTransModel> GetScanMsg(string EXCHNAME, ref string StrMsg)
        {
            List<TaskTransModel> lsttask = new List<TaskTransModel>();
            try
            {
                string strb="select MATERIALNO,MATERIALDESC,BATCHNO,SUM(QTY) QTY from  T_TASKTRANS where EXCHNAME='"+ EXCHNAME + "' group by MATERIALNO,MATERIALDESC,BATCHNO";
                DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, strb).Tables[0];
                return Print_DB.ConvertToModel<TaskTransModel>(dt);
            }
            catch (Exception ex)
            {
                StrMsg = ex.ToString();
                return lsttask;
            }
        }

        //追溯条码的记录 
        public List<HistoryModel> GetBarcodeHistory(string BARCODE, ref string StrMsg)
        {
            List<HistoryModel> lsttask = new List<HistoryModel>();
            try
            {
                string strb = "select MATERIALNO,MATERIALDESC,BATCHNO,QTY,CREATER,CREATETIME,TASKTYPE from T_TASKTRANS where BARCODE='"+ BARCODE + "'";
                DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, strb).Tables[0];
                return Print_DB.ConvertToModel<HistoryModel>(dt);
            }
            catch (Exception ex)
            {
                StrMsg = ex.ToString();
                return lsttask;
            }
        }
        //数据模型
        public class TaskTransModel
        {
            public string materialno { get; set; }
            public string materialdesc { get; set; }
            public string batchno { get; set; }
            public string qty { get; set; }
        }

        public class HistoryModel
        {
            public string materialno { get; set; }
            public string materialdesc { get; set; }
            public string batchno { get; set; }
            public string qty { get; set; }
            public string creater { get; set; }
            public string createtime { get; set; }
            public string tasktype { get; set; }
        }

        #endregion


    }
}
