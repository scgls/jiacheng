using BILBasic.DBA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BILWeb.DAL;
using BILWeb.InStock;
using BILBasic.Common;
using Oracle.ManagedDataAccess.Client;
using System.Reflection;
using BILWeb.Material;
using BILWeb.Query;
using BILBasic.Basing.Factory;
using BILWeb.Stock;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Net;
using BILBasic.JSONUtil;
using BILBasic.Basing;



namespace BILWeb.Print
{
    public partial class Print_DB
    {
        public DbFactory dbFactory = new DbFactory(DbFactory.DbFactoryType.SQLSERVER);

        #region 获取对照表
        public string GetParameterByName(string group, string name)
        {
            string sql = "select parameterid from T_PARAMETER where groupname = '" + group + "' and parametername ='" + name + "'";
            object o = dbFactory.ExecuteScalar(CommandType.Text, sql);
            if (o == null || o.ToString() == "")
            {
                if (group == "Company")
                    return "1";
                return "X";
            }

            else
                return o.ToString();
        }

        public string GetSupMan() 
        {
            string username = ConfigurationManager.ConnectionStrings["username"].ConnectionString;
            return username;
        }

        public string GetParameterById(string group, string id)
        {
            string sql = "select parametername from T_PARAMETER where groupname = '" + group + "' and parameterid =" + id + "";
            object o = dbFactory.ExecuteScalar(CommandType.Text, sql);
            if (o == null || o.ToString() == "")
                return "X";
            else
                return o.ToString();
        }


        public void SetParameterById(string group, int id,string name)
        {
            string sql = "update T_PARAMETER set parametername = '" + name + "' where  groupname = '" + group + "'and parameterid =" + id + "";
            dbFactory.ExecuteNonQuery(CommandType.Text, sql);
        }
        #endregion

        #region 获取条码流水



        //获取条码流水，和条码开头分类信息XXX
        public List<string> getSqNumAndbarcodeType(int num, string mateid, ref string bt, ref string date)
        {
            date = DateTime.Now.ToString("yyyyMMdd");


            List<string> sqls = new List<string>();
            for (int i = 0; i < num; i++)
            {
                sqls.Add("select SEQ_SERIAL_NO.nextval from dual");
            }
            List<string> sqs = new List<string>();
            dbFactory.ExecuteList(sqls, ref sqs);



            bt = GetBTSql(mateid, bt);


            return sqs;

        }

        private static string GetBTSql(string mateid, string bt)
        {
            DbFactory dbFactory = new DbFactory(DbFactory.DbFactoryType.SQLSERVER);
            string sql = "select materialno,maintypecode,purchasetypecode from T_MATERIAL where id = " + mateid;
            using (var dr = dbFactory.ExecuteReader(sql))
            {
                if (dr.Read())
                {
                    string a = dr["materialno"].ToDBString();
                    string b = dr["maintypecode"].ToDBString();
                    string c = dr["purchasetypecode"].ToDBString();
                    bt = GetBT(bt, a, b, c);
                }
                else
                    bt = "XXX";
            }
            return bt;
        }

        private static string GetBT(string bt, string a, string b, string c)
        {
            if (a[0] == 'K')
                bt = "K";
            else
                bt = "0";

            if (b == "")
                bt += "0";
            else
                bt += b;

            if (b == "4")
            {
                if (!string.IsNullOrEmpty(c))
                    bt += c.Substring(c.Length - 1, 1);
                else
                    //X代c为空
                    bt += "X";
            }
            else
                bt += "0";
            return bt;
        }


        public List<string> getSqNum(int num, ref string date)
        {
            date = DateTime.Now.ToString("yyyyMMdd");
            List<string> sqls = new List<string>();
            for (int i = 0; i < num; i++)
            {
                sqls.Add("select SEQ_SERIAL_NO.nextval from dual");
            }
            List<string> sqs = new List<string>();
            dbFactory.ExecuteList(sqls, ref sqs);
            return sqs;

        }

        //获取条码流水
        public void updateNum(string sid, int num)
        {
            string sql = "update T_NCOUNTSEQ set num = " + num + " where sid = '" + sid + "'";
            dbFactory.ExecuteNonQuery(CommandType.Text, sql);
        }
        #endregion

        #region 获取单据

        /// <summary>
        /// 获取单据
        /// </summary>
        /// <param name="vouchertype"></param>
        /// <param name="voucherno"></param>
        /// <param name="infos"></param>
        /// <returns></returns>
        public bool GetPrintVoucher(int vouchertype, string voucherno, ref List<T_InStockDetailInfo> infos, ref string ErrMsg)
        {

            string sql = "select a.CreateTime,a.VoucherNo,a.ErpVoucherNo,a.SupplierNo,a.SupplierName,a.VoucherType,a.ERPVOUCHERTYPE,a.ERPVOUCHERDESC,a.StrongHoldCode,a.StrongHoldName,a.status," +
                "b.id,b.headerid,b.Unit,b.receiveqty,b.remainqty,b.productno,b.ArrivalDate,b.arrstockdate,b.InStockQty,b.MaterialDesc,b.frombatchno,b.MaterialNo,b.RowNo,b.RowNoDel,b.PROROWNO,b.PROROWNODEL,b.hasprint,b.linestatus,c.QUALITYDAY,c.id as mid,c.maintypecode,c.STORECONDITION,c.SPECIALREQUIRE,c.PROTECTWAY" +
                " from t_instock a left join t_instockdetail b on a.id = b.headerid left join t_material c on b.MaterialNo = c.MATERIALNO and b.strongholdcode = c.strongholdcode where a.erpvoucherno = '" + voucherno + "' and a.vouchertype = " + vouchertype;
            infos = new List<T_InStockDetailInfo>();
            using (var dr = dbFactory.ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    if (!GetVoucherField(infos, dr, ref ErrMsg))
                        return false;
                }
            }
            if (infos.Count == 0)
            {
                ErrMsg = "没有获取订单信息";
                return false;
            }
            else
            {
                //修改单据状态，行为3的才要修改，已收0改为1，已收代收不为0，改为2，最后表头判断表体，都为1是1，都为3是3，其他都是2
                List<string> sqls = new List<string>();
                List<int> statuses = new List<int>();
                foreach (var item in infos)
                {
                    if (item.LineStatus == 3)
                    {
                        if (item.ReceiveQty == 0)
                        {
                            sql = "update T_INSTOCKDETAIL set linestatus = 1 where id = " + item.ID;
                            sqls.Add(sql);
                            statuses.Add(1);
                        }
                        else if (item.ReceiveQty != 0 && item.RemainQty != 0)
                        {
                            sql = "update T_INSTOCKDETAIL set linestatus = 2 where id = " + item.ID;
                            sqls.Add(sql);
                            statuses.Add(2);
                        }
                        else
                        {
                            statuses.Add(item.LineStatus.ToInt32());
                        }
                    }
                    else
                    {
                        statuses.Add(item.LineStatus.ToInt32());
                    }
                }
                //修改表头
                int count = statuses.Count;
                int count1 = statuses.FindAll(s => s==1).Count;
                int count3 = statuses.FindAll(s => s == 3).Count;
                if (count == count1)
                    sql = "update T_INSTOCK set status = 1 where id =" + infos[0].HeaderID;
                else if (count == count3)
                {
                    //sql = "update T_INSTOCK set status = 3 where id =" + infos[0].HeaderID;
                }
                else
                    sql = "update T_INSTOCK set status = 2 where id =" + infos[0].HeaderID;
                sqls.Add(sql);
                dbFactory.ExecuteNonQueryList(sqls);
                return true;
            }
                
        }


        public bool GetPrintVoucher2(int vouchertype, string voucherno, ref List<T_InStockDetailInfo> infos, ref string ErrMsg,ref CusSup cs)
        {

            string sql = "select a.CreateTime,a.VoucherNo,a.ErpVoucherNo,a.SupplierNo,a.SupplierName,a.VoucherType,a.ERPVOUCHERTYPE,a.ERPVOUCHERDESC,a.StrongHoldCode,a.StrongHoldName,a.status," +
                "b.id,b.headerid,b.Unit,b.receiveqty,b.remainqty,b.productno,b.ArrivalDate,b.arrstockdate,b.InStockQty,b.MaterialDesc,b.frombatchno,b.MaterialNo,b.RowNo,b.RowNoDel,b.PROROWNO,b.PROROWNODEL,b.hasprint,b.linestatus,c.QUALITYDAY,c.id as mid,c.maintypecode,c.STORECONDITION,c.SPECIALREQUIRE,c.PROTECTWAY" +
                " from t_instock a left join t_instockdetail b on a.id = b.headerid left join t_material c on b.MaterialNo = c.MATERIALNO and b.strongholdcode = c.strongholdcode where a.erpvoucherno = '" + voucherno + "' and a.vouchertype = " + vouchertype;
            infos = new List<T_InStockDetailInfo>();
            using (var dr = dbFactory.ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    if (!GetVoucherField(infos, dr, ref ErrMsg))
                        return false;
                }
            }
            if (infos.Count == 0)
            {
                ErrMsg = "没有获取订单信息";
                return false;
            }
            else
            {
                string sh = infos[0].StrongHoldCode;
                string supno = infos[0].SupplierNo;
                cs = new CusSup();
               //查询订单流水
                sql = "select num from T_SENDNUM where erpvoucherno = '" + voucherno + "'";
                object o = dbFactory.ExecuteScalar(CommandType.Text, sql);
                if (o == null || o.ToString() == "")
                {
                    cs.SendNo = "1";
                    sql = "insert into T_SENDNUM values('" + voucherno + "',1)";
                    dbFactory.ExecuteNonQuery(CommandType.Text, sql);
                }
                else
                    cs.SendNo = o.ToString();
                sql = "select  max(address) as address from t_supplieraddress where supplierno = '" + supno + "' and strongholdcode = '" + sh + "'";
                using (var dr = dbFactory.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        cs.address = dr["address"].ToDBString();
                    }
                }

                sql = "select  max(mobile) as mobile from t_supplier where supplierno = '" + supno + "' and strongholdcode = '" + sh + "'";
                using (var dr = dbFactory.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        cs.Mobile = dr["mobile"].ToDBString();
                    }
                }
                return true;
            }

        }

        public bool SaveSend(string voucherno, int num) 
        {
            string sql = "update T_SENDNUM set num = " + num + " where erpvoucherno = '" + voucherno + "'";
            dbFactory.ExecuteNonQuery(CommandType.Text, sql);
            return true;
        }

        private static bool GetVoucherField(List<T_InStockDetailInfo> infos, IDataReader dr, ref string ErrMsg)
        {
            T_InStockDetailInfo info = new T_InStockDetailInfo();
            //info.Creater = dr["Creater"].ToDBString();
            info.ischeck = false;
            info.Status = dr["Status"].ToInt32();
            info.LineStatus = dr["LineStatus"].ToInt32();
            info.CreateTime = dr["CreateTime"].ToDateTimeNull();
            info.ErpVoucherNo = dr["ErpVoucherNo"].ToDBString();
            info.VoucherNo = dr["VoucherNo"].ToDBString();
            info.SupplierNo = dr["SupplierNo"].ToDBString();
            info.SupplierName = dr["SupplierName"].ToDBString();
            info.ERPVoucherType = dr["ERPVOUCHERTYPE"].ToDBString();
            info.ERPVoucherDesc = dr["ERPVOUCHERDESC"].ToDBString();
            info.StrongHoldCode = dr["StrongHoldCode"].ToDBString();
            info.StrongHoldName = dr["StrongHoldName"].ToDBString();
            if (string.IsNullOrEmpty(info.StrongHoldName))
            {
                ErrMsg = dr["MaterialNo"].ToDBString()+"据点名称为空，请在物料数据中维护";
                return false;
            }
            info.FromBatchNo = dr["FromBatchNo"].ToDBString();
            info.Unit = dr["Unit"].ToDBString();
            info.ArrivalDate = dr["ArrivalDate"].ToDateTimeNull();
            info.ArrStockDate = dr["ArrStockDate"].ToDateTime();
            info.InStockQty = dr["InStockQty"].ToDecimal();
            info.MaterialDesc = dr["MaterialDesc"].ToDBString();
            info.MaterialNo = dr["MaterialNo"].ToDBString();
            info.RowNo = dr["RowNo"].ToDBString();
            info.RowNoDel = dr["RowNoDel"].ToDBString();
            info.lasttime = dr["QUALITYDAY"].ToInt32();
            info.VoucherType = dr["VoucherType"].ToInt32();
            info.MaterialNoID = dr["mid"].ToInt32();
            info.MainTypeCode = dr["MainTypeCode"].ToDBString();
            info.Hasprint = dr["Hasprint"].ToDecimal();
            info.StoreCondition = dr["StoreCondition"].ToDBString();
            info.productno = dr["productno"].ToDBString();
            info.SpecialRequire = dr["SpecialRequire"].ToDBString();
            info.ProtectWay = dr["ProtectWay"].ToDBString();
            info.ReceiveQty = dr["ReceiveQty"].ToDecimal();
            info.RemainQty = dr["RemainQty"].ToDecimal();
            info.ProRowNo = dr["ProRowNo"].ToDBString();
            info.ProRowNoDel = dr["ProRowNoDel"].ToDBString();
            info.ID = dr["id"].ToInt32();
            info.HeaderID = dr["HeaderID"].ToInt32();
            infos.Add(info);
            return true;
        }


        //获取物料主数据
        public bool GetMaterial(int id, string mateno, string judian, ref List<T_MaterialInfo> list, ref string ErrMsg)
        {
            string sql = "";
            if (id != 0)
                sql = "select * from t_material where STRONGHOLDCODE = '" + judian + "' and id = " + id;
            else
                sql = "select * from t_material where STRONGHOLDCODE = '" + judian + "' and MATERIALNO = '" + mateno + "'";
            DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, sql).Tables[0];
            list = ConvertToModel<T_MaterialInfo>(dt);
            if (list.Count == 0)
            {
                ErrMsg = "没有获取物料数据:" + mateno;
                return false;
            }
            return true;
        }
        #endregion

        #region 获取厂内批号

        //获取厂内批号
        public string GetBatch(string company, string matenoid, string supbatch)
        {
            //获取公司代号
            string one = GetParameterByName("Company", company);
            string two = GetBarchYear();
            string three = GetBarchMonth();
            string four = DateTime.Now.ToString("dd");
            string five = GetMaxBatchSeq(two + three + four, company, matenoid, supbatch);
            return one + two + three + four + five;
        }


        public string GetBatch2(DateTime dt ,string company, string matenoid, string supbatch)
        {
            //获取公司代号
            string one = GetParameterByName("Company", company);
            string two = GetBarchYear2(dt);
            string three = GetBarchMonth2(dt);
            string four = dt.ToString("dd");
            string five = GetMaxBatchSeq(two + three + four, company, matenoid, supbatch);
            return one + two + three + four + five;
        }

        private void InsertBarchreg(string materialno, string line ,int sn) 
        {
            string sql = "insert into T_BATCHINS values('" + materialno + "','" + line + "'," + DateTime.Now.Year + ",," + DateTime.Now.Month + ",," + DateTime.Now.Day + ",SYSDATE," + sn + ");";
            dbFactory.ExecuteNonQuery(CommandType.Text,sql);
        }

        public string GetBatchSC(string json)
        {
            BaseMessage_Model<Barcode_Model> bm = new BaseMessage_Model<Barcode_Model>();
            //返回结果中包含数字字符的起始位置和终结位置
            Barcode_Model model = Check_Func.DeserializeJsonToObject<Barcode_Model>(json);
            string result = "";

            string customerno = model.CusCode;
            string lineno = model.RowNo;
            string materialno = model.MaterialNo;
            string sql = "select * from t_batchreg where customerno = '" + customerno + "'  order by idex";
            using (IDataReader dr = dbFactory.ExecuteReader(sql))
            {
                if (dr.Read())
                {
                    string plant = dr["plant"].ToDBString();
                    string subcode = dr["subcode"].ToDBString();
                    string combine = dr["combine"].ToDBString();
                    int datetype = dr["datetype"].ToInt32();

                    DateTime laocaidate=DateTime.Now ;
                    int sn = 0;
                    if(datetype<=1)
                    {
                        //欧莱雅
                        if (datetype == 1)
                        {
                            //todo老蔡方法获取laocaidate和sn
                            if (sn == 0)
                            {
                                bm.HeaderStatus = "E";
                                bm.Message = "没有获取到序列号";
                                return Check_Func.SerializeObject(bm);
                            }
                        }
                        string[] comb = combine.Split('+');
                        for (int i = 0; i < comb.Length; i++)
                        {
                            switch (comb[i])
                            {
                                case "P":
                                    result += plant;
                                   
                                    break;
                                case "S":
                                    result += subcode;
                                  
                                    break;
                                case "Y":
                                    //0 当前，1灌装日期
                                    if (datetype == 0)
                                        result += GetBarchYear2(DateTime.Now);
                                    else if (datetype == 1)
                                    {
                                        result += GetBarchYear2(laocaidate);
                                    }
                                    break;
                                case "M":
                                    //0 当前，1灌装日期
                                    if (datetype == 0)
                                        result += GetBarchMonth2(DateTime.Now);
                                    else if (datetype == 1)
                                    {
                                        result += GetBarchMonth2(laocaidate);
                                    }
                                    break;
                                case "N":
                                    //宜昌天美
                                    if (datetype == 0)
                                    { 
                                        //从记录表中获取数据
                                        sql = "select * from T_BATCHINS where materialno = '" + materialno + "' and year = " + DateTime.Now.Year + " and month = " + DateTime.Now.Month ;
                                        List<Barcode_Model> l = new List<Barcode_Model>() ;
                                        using (IDataReader dr1 = dbFactory.ExecuteReader(sql))
                                        {
                                            
                                            while (dr1.Read())
                                            {
                                                Barcode_Model m = new Barcode_Model();
                                                m.MaterialNo = dr1["materialno"].ToDBString();
                                                m.RowNo = dr1["line"].ToDBString();
                                                m.year = dr1["year"].ToInt32();
                                                m.month = dr1["month"].ToInt32();
                                                m.day = dr1["day"].ToInt32();
                                                m.STATUS = dr1["sn"].ToInt32();
                                                l.Add(m);
                                            }
                                        }
                                        if (l.Count == 0)
                                        {
                                            //新建并且插入
                                            result += GetSN(0);
                                            InsertBarchreg(materialno, lineno, 0);
                                        }
                                        else 
                                        {
                                            //如果存在完全匹配的直接返回
                                            int index = l.FindIndex(p => p.MaterialNo == materialno && p.RowNo == lineno && p.year == DateTime.Now.Year && p.month == DateTime.Now.Month && p.day == DateTime.Now.Day);
                                            if (index < 0)
                                            {
                                                //没有找到匹配项，加1插入
                                                int max = l.Max(p => p.STATUS);
                                                result += GetSN(max + 1);
                                                InsertBarchreg(materialno, lineno, max + 1);
                                            }
                                            else
                                            {
                                                //找到匹配的直接返回
                                                result += GetSN(l[index].STATUS);
                                            }
                                        }
                                    }
                                    //欧莱雅
                                    else if (datetype == 1)
                                    {
                                        //获取老蔡的序列号
                                        result += GetSN(sn);
                                    }
                                    break;
                            }
                        }
                    }
                    //Coty
                    else if (datetype == 2)
                    { 
                        string y = DateTime.Now.Year.ToString();
                        y = y.Substring(y.Length-1,1);
                        string days = DateTime.Now.DayOfYear.ToString().PadLeft(3,'0');
                        result = y + days + "00";
                    }
                    //HCT-ULTA
                    else if (datetype == 3)
                    {
                        string y = DateTime.Now.Year.ToString();
                        y = y.Substring(y.Length - 1, 1);
                        string days = DateTime.Now.DayOfYear.ToString().PadLeft(3, '0');
                        string snn = "";
                        //todo laocai  snn
                        if (snn == "")
                        {
                            bm.HeaderStatus = "E";
                            bm.Message = "没有获取到序列号";
                            return Check_Func.SerializeObject(bm);
                        }
                        result = "B" + y + days + snn;
                    }
                    
                    Barcode_Model m1 = new Barcode_Model();
                    m1.BatchNo = result;
                    bm.HeaderStatus = "S";
                    bm.Message = "";
                    bm.ModelJson = m1;
                    return Check_Func.SerializeObject(bm);
                }
                else
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "没有自动获取到批次信息";
                    return Check_Func.SerializeObject(bm);
                }
            }
        }


        private string GetMaxBatchSeq(string ymd, string company, string matenoid, string supbatch)
        {
            //string sql = "select Max(BATCHNO) from t_outbarcode where STRONGHOLDCODE = '" + company + "' and MATERIALNOID = " + matenoid + " and batchno like '%" + ymd + "%' and SUPCODE='" + supcode + "' and barcodetype=1";
            //object o = dbFactory.ExecuteScalar(CommandType.Text, sql);
            //if (o == null || o.ToString() == "")
            //    return "01";
            //return (Convert.ToInt32(o.ToString().Substring(5, 2)) + 1).ToString().PadLeft(2, '0');
            string sql ="";
            string[] items = supbatch.Split('+');
            if (items.Length > 1)
            {
                string voucherno = items[items.Length - 1];
                supbatch = items[0];
                sql = "select max(BATCHNO) from t_outbarcode where STRONGHOLDCODE = '" + company + "' and erpvoucherno = '" + voucherno + "' and  MATERIALNOID = " + matenoid + " and batchno like '%" + ymd + "%' and SupPrdBatch = '" + supbatch + "' and barcodetype=1";
            }
            else
                sql = "select max(BATCHNO) from t_outbarcode where STRONGHOLDCODE = '" + company + "'  and  MATERIALNOID = " + matenoid + " and batchno like '%" + ymd + "%' and SupPrdBatch = '" + supbatch + "' and barcodetype=1";
         

            object o = dbFactory.ExecuteScalar(CommandType.Text, sql);
            if (o == null || o.ToString() == "")
            {
                sql = "select Max(BATCHNO) from t_outbarcode where STRONGHOLDCODE = '" + company + "' and MATERIALNOID = " + matenoid + " and batchno like '%" + ymd + "%' and barcodetype=1";
                o = dbFactory.ExecuteScalar(CommandType.Text, sql);
                if (o == null || o.ToString() == "")
                    return "01";
                return (Convert.ToInt32(o.ToString().Substring(5, 2)) + 1).ToString().PadLeft(2, '0');
            }
            else
            {
                return o.ToString().Substring(5, 2);
            }
            
        }

        private string GetBarchYear()
        {
            string year = DateTime.Now.ToString("yyyy");
            switch (year)
            {
                case "2017":
                    return "7";
                case "2018":
                    return "8";
                case "2019":
                    return "9";
                case "2020":
                    return "A";
                case "2021":
                    return "B";
                case "2022":
                    return "C";
                case "2023":
                    return "D";
                case "2024":
                    return "E";
                case "2025":
                    return "F";
                case "2026":
                    return "G";
                case "2027":
                    return "H";
                case "2028":
                    return "I";
                case "2029":
                    return "J";
                case "2030":
                    return "K";
                case "2031":
                    return "L";
                case "2032":
                    return "M";
                case "2033":
                    return "N";
                case "2034":
                    return "O";
                case "2035":
                    return "P";
                case "2036":
                    return "Q";
                case "2037":
                    return "R";
                case "2038":
                    return "S";
                case "2039":
                    return "T";
                default:
                    return "X";

            }
        }
        private string GetBarchYear2(DateTime dt)
        {
            string year = dt.ToString("yyyy");
            switch (year)
            {
                case "2017":
                    return "7";
                case "2018":
                    return "8";
                case "2019":
                    return "9";
                case "2020":
                    return "A";
                case "2021":
                    return "B";
                case "2022":
                    return "C";
                case "2023":
                    return "D";
                case "2024":
                    return "E";
                case "2025":
                    return "F";
                case "2026":
                    return "G";
                case "2027":
                    return "H";
                case "2028":
                    return "I";
                case "2029":
                    return "J";
                case "2030":
                    return "K";
                case "2031":
                    return "L";
                case "2032":
                    return "M";
                case "2033":
                    return "N";
                case "2034":
                    return "O";
                case "2035":
                    return "P";
                case "2036":
                    return "Q";
                case "2037":
                    return "R";
                case "2038":
                    return "S";
                case "2039":
                    return "T";
                default:
                    return "X";

            }
        }
        private string GetBarchMonth()
        {
            string month = DateTime.Now.ToString("MM");
            switch (month)
            {
                case "01":
                    return "A";
                case "02":
                    return "B";
                case "03":
                    return "C";
                case "04":
                    return "D";
                case "05":
                    return "E";
                case "06":
                    return "F";
                case "07":
                    return "G";
                case "08":
                    return "H";
                case "09":
                    return "I";
                case "10":
                    return "J";
                case "11":
                    return "K";
                case "12":
                    return "L";
                default:
                    return "X";

            }
        }
        private string GetBarchMonth2(DateTime dt)
        {
            string month = dt.ToString("MM");
            switch (month)
            {
                case "01":
                    return "A";
                case "02":
                    return "B";
                case "03":
                    return "C";
                case "04":
                    return "D";
                case "05":
                    return "E";
                case "06":
                    return "F";
                case "07":
                    return "G";
                case "08":
                    return "H";
                case "09":
                    return "I";
                case "10":
                    return "J";
                case "11":
                    return "K";
                case "12":
                    return "L";
                default:
                    return "X";

            }
        }

        private string GetSN(int i)
        {
            switch (i)
            {
                case 1:
                    return "0";
                case 2:
                    return "1";
                case 3:
                    return "2";
                case 4:
                    return "3";
                case 5:
                    return "4";
                case 6:
                    return "5";
                case 7:
                    return "6";
                case 8:
                    return "7";
                case 9:
                    return "8";
                case 10:
                    return "9";
                case 11:
                    return "A";
                case 12:
                    return "B";
                case 13:
                    return "C";
                case 14:
                    return "D";
                case 15:
                    return "E";
                case 16:
                    return "F";
                case 17:
                    return "G";
                case 18:
                    return "H";
                case 19:
                    return "J";
                case 20:
                    return "K";
                case 21:
                    return "L";
                case 22:
                    return "M";
                case 23:
                    return "N";
                case 24:
                    return "P";
                case 25:
                    return "R";
                case 26:
                    return "S";
                case 27:
                    return "T";
                case 28:
                    return "U";
                case 29:
                    return "V";
                case 30:
                    return "W";
                case 31:
                    return "X";
                default:
                    return "X";

            }
        }

        #endregion

        #region 获取包装量
        public Dictionary<string, decimal> GetPack(string matenoid)
        {
            string sql = "select STANDARD,QTY from t_material_pack where HEADERID = " + matenoid + "";
            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();
            using (var dr = dbFactory.ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    if (!(dr["STANDARD"] is DBNull))
                    {
                        dic.Add(dr["STANDARD"].ToDBString(), dr["QTY"].ToDecimal());
                    }
                }
            }
            return dic;
        }
        #endregion

        #region 提交条码列表
        public bool PrintDeliveryTray(T_StockInfoEX model, List<T_StockInfoEX> list, string ipport, ref string ErrMsg)
        {
            //取12行
            //list = list.GetRange(0, 12);
            string zpl = DeliveryTray(model, list);
            List<string> strlist = new List<string>();
            strlist.Add(zpl);
            int port = 0;
            try
            {
                bool res = int.TryParse(ipport.Split(':')[1], out port);
                if (!res)
                {
                    ErrMsg = "端口格式错误";
                    return false;
                }
            }
            catch
            {
                ErrMsg = "端口格式错误";
                return false;
            }
           
            //打印
            SocketHelper sh = new SocketHelper(ipport.Split(':')[0], port);
            if (!sh.Send(strlist, ref ErrMsg))
            {
                return false;
            }
            return true;
        }


        public bool PrintZPL(List<Barcode_Model> list, string ipport, ref string ErrMsg)
        {
            List<string> strlist = new List<string>();
            foreach (Barcode_Model bm in list)
            {
                string pt = bm.LABELMARK;
                switch (pt)
                {
                    case "OutBaoCai":
                        strlist.Add(OutBaoCai(bm));
                        break;
                    case "TOutBaoCai":
                        strlist.Add(TOutBaoCai(bm));
                        break;
                    case "InBaoCai":
                        strlist.Add(InBaoCai(bm));
                        break;
                    case "OutR":
                        strlist.Add(OutR(bm));
                        break;
                    case "InR":
                        strlist.Add(InR(bm));
                        break;
                    case "OutFromBanZhi":
                        strlist.Add(OutFromBanZhi(bm));
                        break;
                    case "OutFromSanZhuang":
                        strlist.Add(OutFromSanZhuang(bm));
                        break;
                    case "InFromSanZhuang":
                        strlist.Add(InFromSanZhuang(bm));
                        break;
                    case "OutBanZhi":
                        strlist.Add(OutBanZhi(bm));
                        break;
                    case "OutChengPin":
                        strlist.Add(OutChengPin(bm));
                        break;
                    case "TOutChengPin":
                        strlist.Add(TOutChengPin(bm));
                        break;
                    case "OutSanZhuang":
                        strlist.Add(OutSanZhuang(bm));
                        break;
                    case "InSanZhuang":
                        strlist.Add(InSanZhuang(bm));
                        break;
                    case "OutBaoCaiTui":
                        strlist.Add(OutBaoCaiTui(bm));
                        break;
                    case "OutSanZhuangTui":
                        strlist.Add(OutSanZhuangTui(bm));
                        break;
                    case "OutBanZhiTui":
                        strlist.Add(OutBanZhiTui(bm));
                        break;
                    case "OutChengPinTui":
                        strlist.Add(OutChengPinTui(bm));
                        break;
                    case "OutBaoFei":
                        strlist.Add(OutBaoFei(bm));
                        break;
                    case "NullRef":
                        strlist.Add(NullRef(bm));
                        break;
                    case "TNullRef":
                        strlist.Add(TNullRef(bm));
                        break;
                    case "InNullRef":
                        strlist.Add(InNullRef(bm));
                        break;
                    case "Man":
                        strlist.Add(Man(bm));
                        break;
                    default:
                        strlist.Add(NullRef(bm));
                        break;
                }
            }
            int port = 0;
            try
            {
                bool res = int.TryParse(ipport.Split(':')[1], out port);
                if (!res)
                {
                    ErrMsg = "端口格式错误";
                    return false;
                }
            }
            catch {
                ErrMsg = "端口格式错误";
                return false;
            }
            
           
            //打印
            SocketHelper sh = new SocketHelper(ipport.Split(':')[0], port);
            if (!sh.Send(strlist, ref ErrMsg))
            {
                return false;
            }
            return true;
        }

        public bool PrintZPL2(List<Barcode_Model> list, string ipport, ref string ErrMsg)
        {
            Print_CodeLpk code = null;
            try
            {
                code = new Print_CodeLpk();
                //打印
                if (!code.Connect(ipport, ref ErrMsg))
                {
                    code.Close();
                    return false;
                }

                foreach (Barcode_Model bm in list)
                {
                    string pt = bm.LABELMARK;
                    switch (pt)
                    {
                        case "QuYang":
                            code.QuYang(bm);
                            break;
                        case "QuYang2":
                            code.QuYang2(bm);
                            break;
                        case "OutBaoCai":
                            code.OutBaoCai(bm);
                            break;
                        case "TOutBaoCai":
                            code.TOutBaoCai(bm);
                            break;
                        case "OutR":
                            code.OutR(bm);
                            break;
                        case "TOutR":
                            code.TOutR(bm);
                            break;
                        case "TNullRef":
                            code.TNullRef(bm);
                            break;
                        case "OutFromBanZhi":
                            code.OutFromBanZhi(bm);
                            break;
                        case "OutFromSanZhuang":
                            code.OutFromSanZhuang(bm);
                            break;
                        case "OutBanZhi":
                            code.OutBanZhi(bm);
                            break;
                        case "OutSanZhuang":
                            code.OutSanZhuang(bm);
                            break;
                        case "OutChengPin":
                            code.OutChengPin(bm);
                            break;
                        case "TOutChengPin":
                            code.TOutChengPin(bm);
                            break;
                        case "OutBaoCaiTui":
                            code.OutBaoCaiTui(bm);
                            break;
                        case "TOutBaoCaiTui":
                            code.TOutBaoCaiTui(bm);
                            break;
                        case "OutSanZhuangTui":
                            code.OutSanZhuangTui(bm);
                            break;
                        case "OutBanZhiTui":
                            code.OutBanZhiTui(bm);
                            break;
                        case "OutChengPinTui":
                            code.OutChengPinTui(bm);
                            break;
                        case "TOutChengPinTui":
                            code.TOutChengPinTui(bm);
                            break;
                        case "OutBaoFei":
                            code.OutBaoFei(bm);
                            break;
                        case "NullRef":
                            code.NullRef(bm);
                            break;
                        default:
                            code.NullRef(bm);
                            break;
                    }
                }
                code.Close();
                return true;

            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }
            finally
            {
                code.Close();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ipport"></param>
        /// <param name="hasprint"></param>
        /// <param name="ErrMsg"></param>
        /// <param name="type">默认为0代表zpl指令打印，如果为1代表pda打印</param>
        /// <returns></returns>
        public bool SubBarcodes(List<Barcode_Model> list, string ipport, decimal hasprint, ref string ErrMsg, int type = 0)
        {
            //在条码中插入打印日期
            foreach (var item in list)
            {
                if (item.EDate == DateTime.MinValue || item.EDate.ToString("yyyy/MM/dd") == "0001/01/01")
                {
                    ErrMsg = "行号："+item.RowNo+"，"+item.RowNoDel+"的有效期不能为最小值";
                    return false;
                }
                if (item.CreateTime == DateTime.MinValue)
                    item.CreateTime = DateTime.Now;
            }

            //保存
            using (var tran = OracleDBPoolHelper.GetTransaction())
            {
                try
                {
                    list.Save("t_outbarcode", "SEQ_OUTBARCODE_ID", tran);

                    if (hasprint != 0)
                    {
                        //修改已经打印数量
                        string sql = "update t_instockdetail t set t.hasprint =(case when t.hasprint is null then " + hasprint + " else t.hasprint+" + hasprint + " end) where t.rowno = '" + list[0].RowNo + "' and (t.rownodel='" + list[0].RowNoDel + "' or t.rownodel is null) and t.headerid =(select max(id) from t_instock i where i.ErpVoucherNo = '" + list[0].ErpVoucherNo + "' )";
                        sql.ExeNullQuery(tran);
                    }

                    OracleDBPoolHelper.CommitTransaction(tran);
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.ToString();
                    OracleDBPoolHelper.RollbackTransaction(tran);
                    return false;
                }
            }
            if (ipport != "sup")
            {
                if (type == 0)
                {
                    //打印标签
                    bool res = PrintZPL(list, ipport, ref ErrMsg);
                    if (!res)
                        return false;
                }
                else if (type == 1)
                {
                    //打印标签
                    bool res = PrintZPL2(list, ipport, ref ErrMsg);
                    if (!res)
                        return false;
                }
            }
           

            ErrMsg = "保存成功";
            return true;
        }

        public bool SubBarcodesNoPrint(List<Barcode_Model> list, ref string ErrMsg)
        {
            //在条码中插入打印日期
            foreach (var item in list)
            {
                if (item.EDate == DateTime.MinValue || item.EDate.ToString("yyyy/MM/dd") == "0001/01/01")
                {
                    ErrMsg = "行号：" + item.RowNo + "，" + item.RowNoDel + "的有效期不能为最小值";
                    return false;
                }
                if (item.CreateTime == DateTime.MinValue)
                    item.CreateTime = DateTime.Now;
            }

            List<string> sqls = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                string strsql = GetInsertSQL(list[i]);
                sqls.Add(strsql);
            }
            sqls.Add("update  t_outbarcode  set t_outbarcode.materialnoid=T_MATERIAL.id from t_outbarcode,T_MATERIAL where t_outbarcode.materialno = T_MATERIAL.materialno and t_outbarcode.materialnoid is null");
            dbFactory.ExecuteNonQueryList(sqls);

            ////保存
            //using (var tran = OracleDBPoolHelper.GetTransaction())
            //{
            //    try
            //    {

            //        list.Save("t_outbarcode", "SEQ_OUTBARCODE_ID", tran);
            //        //批量设置物料编号id
            //        OracleDBPoolHelper.ExecuteNonQuery(tran, System.Data.CommandType.Text, "UPDATE t_outbarcode SET t_outbarcode.materialnoid = (select t_material.ID from t_material where t_material.materialno = t_outbarcode.materialno and t_material.strongholdcode = t_outbarcode.strongholdcode)", null);
            //        OracleDBPoolHelper.CommitTransaction(tran);
            //    }
            //    catch (Exception ex)
            //    {
            //        ErrMsg = ex.ToString();
            //        OracleDBPoolHelper.RollbackTransaction(tran);
            //        return false;
            //    }
            //}
            ErrMsg = "保存成功";
            return true;
        }

        //期初条码生成
        public string GetInsertSQL(Barcode_Model model) {
            return "insert into t_outbarcode (materialno,materialdesc,qty,barcode,barcodetype,serialno,batchno,ean,edate,status,receivetime,erpvoucherno,strongholdcode,strongholdname,companycode) values ('"+ model.MaterialNo + "','" + model.MaterialDesc + "'," + model.Qty + ",'" + model.BarCode + "'," + model.BarcodeType + ",'" + model.SerialNo + "','" + model.BatchNo + "','" + model.EAN + "','" + model.EDate + "',0,'"+model.ReceiveTime+"','"+ model.ErpVoucherNo+ "','" + model.StrongHoldCode + "','" + model.StrongHoldName + "','" + model.CompanyCode + "')";
        }
        #endregion

        #region 转换方法
        public static List<T> ConvertToModel<T>(DataTable dt) where T : new()
        {
            // 定义集合    
            List<T> ts = new List<T>();

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    

                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            string name = pi.PropertyType.Name.ToLower();
                            if (name == "int32")
                                value = Convert.ToInt32(value);
                            if (name == "decimal")
                                value = Convert.ToDecimal(value);
                            if (name == "datetime")
                                value = Convert.ToDateTime(value);
                            pi.SetValue(t, value, null);
                        }

                    }
                }
                ts.Add(t);
            }
            return ts;
        }
        #endregion

        #region 标签重打
        public bool GetBarcode(Barcode_Model model, ref List<Barcode_Model> list, ref DividPage page, ref string ErrMsg)
        {
            if (page == null)
            {
                string sql2 = "select * from t_outbarcode b where barcode = '"+model.BarCode+"'";
                DataTable dt2 = dbFactory.ExecuteDataSet(CommandType.Text, sql2).Tables[0];
                list = ConvertToModel<Barcode_Model>(dt2);
                if (list.Count == 0)
                {
                    ErrMsg = "没有数据";
                    return false;
                }
                return true;
            }

            string sql = "select ROW_NUMBER() OVER(Order by ID desc) AS PageRowNumber ,b.* from t_outbarcode b where  " + GetBarcodeQuery(model);
            DataTable dt = Common_DB.QueryByDividPage(ref page, sql);
           
            list = ConvertToModel<Barcode_Model>(dt);
            page.CurrentPageRecordCounts = dt.Rows.Count;
            if (list.Count == 0)
            {
                ErrMsg = "没有数据";
                return false;
            }
            return true;
        }

        private string GetBarcodeQuery(Barcode_Model model)
        {
            string query = "1=1 and isdel = 1 and BarcodeType=" + model.BarcodeType;
            if (model.ErpVoucherNo != "")
            {
                query += " and ErpVoucherNo = '" + model.ErpVoucherNo + "'";
            }
            if (model.MaterialNo != "")
            {
                query += " and MaterialNo = '" + model.MaterialNo + "'";
            }

            if (model.RowNo != "")
            {
                query += " and RowNo = '" + model.RowNo + "'";
            }

            if (model.RowNoDel != "")
            {
                query += " and RowNoDel = '" + model.RowNoDel + "'";
            }

            if (model.SerialNo != "")
            {
                query += " and SerialNo like '%" + model.SerialNo + "%'";
            }

            if (model.BatchNo != "")
            {
                query += " and BatchNo = '" + model.BatchNo + "'";
            }
            if (model.begin != null)
            {
                query += " and CREATETIME>=to_date('" + model.begin + "','YYYY/MM/DD') ";
            }
            if (model.end != null)
            {
                query += " and CREATETIME<=to_date('" + model.end + "','YYYY/MM/DD') ";
            }


            return query;
        }
        #endregion

        #region 期初打印

      

        //PDA期初打印

        public string PrintAndroid(string json)
        {
            BaseMessage_Model<string> bm = new BaseMessage_Model<string>();
            string j = "";
            string ipport = "";
            try
            {
                List<Barcode_Model> list = Check_Func.DeserializeJsonToList<Barcode_Model>(json);
                if (list.Count == 0)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "数据不能为空";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

                ipport = list[0].IP;
                string data = "";
                List<string> squence = getSqNum(list.Count, ref data);
                string sq = "";
                int i = 0;
                string ErrMsg = "";
                foreach (Barcode_Model model2 in list)
                {
                    List<T_MaterialInfo> lm = new List<T_MaterialInfo>();
                    if (!GetMaterial(0, model2.MaterialNo, model2.StrongHoldCode, ref lm, ref ErrMsg))
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = ErrMsg;
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }
                    T_MaterialInfo tm = lm[0];
                    sq = getSqu(squence[i]);

                    FirstModelMakeAndroid(data, sq, model2, tm);

                    i++;
                }
                //传入类型为1，标识用lpk打印机打印
                bool res = SubBarcodes(list, ipport, 0, ref ErrMsg, 1);
                if (res)
                {
                    bm.HeaderStatus = "S";
                    bm.Message = "成功";
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
            catch (Exception ex)
            {
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                j = Check_Func.SerializeObject(bm);
                return j;
            }

        }

        public bool FirstPrint(ref List<Barcode_Model> list, string ipport, ref string ErrMsg)
        {
            string data = "";
            List<string> squence = getSqNum(list.Count, ref data);
            string sq = "";
            int i = 0;
            foreach (Barcode_Model model2 in list)
            {
                List<T_MaterialInfo> lm = new List<T_MaterialInfo>();
                if (!GetMaterial(0, model2.MaterialNo, model2.StrongHoldCode, ref lm, ref ErrMsg))
                {
                    return false;
                }
                T_MaterialInfo tm = lm[0];
                sq = getSqu(squence[i]);

                FirstModelMake(data, sq, model2, tm);

                i++;
            }

            bool res = SubBarcodes(list, ipport, 0, ref ErrMsg);
            return res;
        }

        private static void FirstModelMake(string data, string sq, Barcode_Model model2, T_MaterialInfo tm)
        {
            model2.CompanyCode = "10";
            model2.IsDel = 1;
            model2.Unit = tm.Unit;
            model2.MaterialNoID = tm.ID;
            model2.ErpBarCode = tm.ErpBarCode;
            //model2.StrongHoldCode = selectItem.StrongHoldCode;
            //model2.StrongHoldName = selectItem.StrongHoldName;
            //model2.ErpVoucherNo = tm.ErpVoucherNo;
            //model2.VoucherType = selectItem.VoucherType.ToString();
            //model2.MaterialNo = selectItem.MaterialNo;
            model2.MaterialDesc = tm.MaterialDesc;
            //model2.SupCode = selectItem.SupplierNo;
            //model2.SupName = selectItem.SupplierName;
            //订单数量
            //model2.VoucherQty = selectItem.InStockQty;
            //model2.SupCode = "0";
            model2.OutPackQty = model2.Qty;

            model2.StoreCondition = tm.StoreCondition;
            model2.SpecialRequire = tm.SpecialRequire;
            model2.ProtectWay = tm.ProtectWay;

            //model2.StoreCondition = "<15℃";
            //model2.SpecialRequire = "无";
            //model2.ProtectWay = "3";
            //model2.RelaWeight = "5kg";
            //model2.ProductClass = "班组FA";

            //model2.CreateTime = model2.ProductDate;

            //外箱1，內盒0
            model2.BarcodeType = 1;
            model2.SerialNo = data + sq;


            //if ((DateTime.Now - model2.ProductDate).Days > 3650)
            //{
            //    ErrMsg = "生产日期不能早于10年前";
            //    return false;
            //}
            if (model2.ProductDate == DateTime.MinValue)
                model2.ProductDate = model2.CreateTime;
            model2.SupPrdDate = model2.ProductDate;

            //model2.EDate = model2.ProductDate.AddDays((double)tm.QualityDay);
            //如果包材外，有效期自动加3年
            if (model2.LABELMARK == "OutBaoCai")
                model2.EDate = model2.CreateTime.AddYears(3);


            string bt = "";
            model2.BarcodeMType = GetBT(bt, model2.MaterialNo, tm.MainTypeCode, tm.PurchaseTypeCode);

            model2.BarCode = "1@" + model2.BarcodeMType + "@" + model2.MaterialNo + "@" + model2.SupCode + "@" + model2.Qty + "@" + data + "@" + model2.SerialNo;
        }
        private static void FirstModelMakeAndroid(string data, string sq, Barcode_Model model2, T_MaterialInfo tm)
        {
            model2.CompanyCode = "10";
            model2.IsDel = 1;
            model2.Unit = tm.Unit;
            model2.MaterialNoID = tm.ID;
            //model2.StrongHoldCode = selectItem.StrongHoldCode;
            model2.StrongHoldName = tm.StrongHoldName;
            //model2.ErpVoucherNo = tm.ErpVoucherNo;
            //model2.VoucherType = selectItem.VoucherType.ToString();
            //model2.MaterialNo = selectItem.MaterialNo;
            model2.MaterialDesc = tm.MaterialDesc;
            //model2.SupCode = selectItem.SupplierNo;
            //model2.SupName = selectItem.SupplierName;
            //订单数量
            //外箱1，內盒0
            //model2.BarcodeType = 1;
            //model2.OutPackQty = model2.Qty;
            //model2.SupPrdDate = model2.ProductDate;
            //model2.BatchNo
            //model2.EDate
            model2.StoreCondition = tm.StoreCondition;
            model2.SpecialRequire = tm.SpecialRequire;
            model2.ProtectWay = tm.ProtectWay;

            if (model2.CreateTime == DateTime.MinValue)
                model2.CreateTime = DateTime.Now;


            model2.SerialNo = data + sq;

            model2.SupPrdDate = model2.ProductDate;

            //如果包材外，有效期自动加3年
            if (model2.LABELMARK == "OutBaoCai")
                model2.EDate = model2.CreateTime.AddYears(3);


            string bt = "";
            model2.BarcodeMType = GetBT(bt, model2.MaterialNo, tm.MainTypeCode, tm.PurchaseTypeCode);
            if (model2.BarcodeType == 1)
                model2.BarCode = "1@" + model2.BarcodeMType + "@" + model2.MaterialNo + "@" + model2.SupCode + "@" + model2.Qty + "@" + data + "@" + model2.SerialNo;
            else if (model2.BarcodeType == 0)
                model2.BarCode = "0@" + model2.BarcodeMType + "@" + model2.MaterialNo + "@" + model2.SupCode + "@" + model2.Qty + "@" + data + "@" + model2.SerialNo;
        }

        public string getSqu(string ss)
        {
            if (ss.Length >= 6)
                ss = ss.Substring(ss.Length - 6, 6);
            else
            {
                ss = "000000" + ss;
                ss = ss.Substring(ss.Length - 6, 6);
            }
            return ss;
        }
        #endregion 期初打印

        #region 拆零标签
        public string PrintLpkApartAndroid(string json)
        {
            BaseMessage_Model<string> bm = new BaseMessage_Model<string>();
            string ipport = "";
            string j = "";
            string ErrMsg = "";
            try
            {
                List<Barcode_Model> list = Check_Func.DeserializeJsonToList<Barcode_Model>(json);
                ipport = list[0].IP;
                if (!Print_DB.isConnected(ipport))
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "打印机连接失败！";
                    return Check_Func.SerializeObject(bm);
                }

                if (list.Count == 0)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "没有传入数据";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }
                foreach (var item in list)
                {
                    bool res = PrintLpkApart(item.SerialNo, ipport, ref ErrMsg);
                    if (!res)
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = ErrMsg;
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }
                }
                bm.HeaderStatus = "S";
                bm.Message = "打印成功";
                j = Check_Func.SerializeObject(bm);
                return j;
            }
            catch (Exception ex)
            {
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                j = Check_Func.SerializeObject(bm);
                return j;
            }
        }
        /// <summary>
        /// 库存拆零外箱打印，只有库存有才能打印，也能用于库存pda补打
        /// </summary>
        /// <param name="serialno"></param>
        /// <param name="ip"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool PrintLpkApart(string serialno, string ip, ref string ErrMsg)
        {
            try
            {
                string sql = "select b.* from V_StockDet b where serialno ='" + serialno + "' ";
                DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    ErrMsg = "没有该条记录";
                    return false;
                }
                List<Barcode_Model> list = Print_DB.ConvertToModel<Barcode_Model>(dt);
                foreach (var item in list)
                {
                    item.Creater = item.PrintCreater == null ? "" : item.PrintCreater;
                    item.CreateTime = item.PrintCreateTime;
                    item.areano = "";
                }
                return PrintZPL2(list, ip, ref ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }

        }

        /// <summary>
        /// 条码拆零外箱打印------也可以是不在库条码
        /// </summary>
        /// <param name="serialno"></param>
        /// <param name="ip"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool PrintLpkApart_ForProduct(string serialno, string ip, ref string ErrMsg)
        {
            try
            {
                string sql = "select b.* from t_outbarcode b where serialno ='" + serialno + "' ";
                DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    ErrMsg = "没有该条记录";
                    return false;
                }
                List<Barcode_Model> list = Print_DB.ConvertToModel<Barcode_Model>(dt);
                foreach (var item in list)
                {
                    item.Creater = item.PrintCreater == null ? "" : item.PrintCreater;
                    item.CreateTime = item.PrintCreateTime;
                    item.areano = "";
                }
                return PrintZPL2(list, ip, ref ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }

        }
        #endregion

        #region 托盘标签打印

        public string PrintLpkPalletAndroid(string json)
        {
            BaseMessage_Model<string> bm = new BaseMessage_Model<string>();
            string ipport = "";
            string j = "";
            string ErrMsg = "";
            try
            {
                List<Barcode_Model> list = Check_Func.DeserializeJsonToList<Barcode_Model>(json);
                ipport = list[0].IP;
                if (list.Count == 0)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "没有传入数据";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }
                foreach(var item in list)
                {
                    bool res = PrintLpkPallet(item.SerialNo, ipport,ref ErrMsg);
                    if (!res)
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = ErrMsg;
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }
                }
                bm.HeaderStatus = "S";
                bm.Message = "打印成功";
                j = Check_Func.SerializeObject(bm);
                return j;
            }
            catch (Exception ex)
            {
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                j = Check_Func.SerializeObject(bm);
                return j;
            }
        }

        /// <summary>
        /// 打印托盘标签，只要托盘表有就能打印
        /// </summary>
        /// <param name="serialno"></param>
        /// <param name="ip"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool PrintLpkPallet(string serialno, string ip, ref string ErrMsg)
        {
            try
            {
                decimal count;
                decimal sumqty;
                string sql;
                GetPalletCount(serialno, out count, out sumqty, out sql);
                if (count == 0)
                {
                    ErrMsg = "没有该条记录";
                    return false;
                }

                DataTable dt = GetPalletDet(serialno, ref sql);
                if (dt.Rows.Count < 1)
                {
                    ErrMsg = "没有该条记录";
                    return false;
                }
                List<Barcode_Model> list2 = GetPalletField(count, sumqty, dt); 
                return PrintZPL2(list2, ip, ref ErrMsg);

            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }
        }

        //pda调用大打印机
        public bool PrintZplPallet(string serialno, string ip, ref string ErrMsg)
        {
            try
            {
                decimal count;
                decimal sumqty;
                string sql;
                GetPalletCount(serialno, out count, out sumqty, out sql);
                if (count == 0)
                {
                    ErrMsg = "没有该条记录";
                    return false;
                }

                DataTable dt = GetPalletDet(serialno, ref sql);
                if (dt.Rows.Count < 1)
                {
                    ErrMsg = "没有该条记录";
                    return false;
                }
                List<Barcode_Model> list2 = GetPalletField(count, sumqty, dt); 

                ip = ip + ":9100";

                return PrintZPL(list2, ip, ref ErrMsg);

            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }
        }

        //pc调用大打印机打托盘
        public bool PrintZplPallet2(List<string> serialnos, ref string ErrMsg, ref List<Barcode_Model> list2)
        {
            try
            {
                foreach (string serialno in serialnos)
                {
                    decimal count;
                    decimal sumqty;
                    string sql;
                    GetPalletCount(serialno, out count, out sumqty, out sql);
                    if (count == 0)
                    {
                        ErrMsg = "没有该条记录" + serialno;
                        return false;
                    }

                    DataTable dt = GetPalletDet(serialno, ref sql);
                    if (dt.Rows.Count < 1)
                    {
                        ErrMsg = "没有该条记录" + serialno;
                        return false;
                    }
                    List<Barcode_Model> list3 = GetPalletField(count, sumqty, dt);
                    list2.Add(list3[0]);
                }
                return true;

            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }
        }

        private static List<Barcode_Model> GetPalletField(decimal count, decimal sumqty, DataTable dt)
        {
            List<Barcode_Model> list = Print_DB.ConvertToModel<Barcode_Model>(dt);
            List<Barcode_Model> list2 = new List<Barcode_Model>();
            list2.Add(list[0]);
            string bt = "";
            bt = GetBTSql(list[0].MaterialNoID + "", bt);
            list[0].Qty = sumqty;
            list[0].BoxCount = count;
            if (string.IsNullOrEmpty(list[0].LABELMARK))
                list[0].LABELMARK = "NullRef";
            //if (list[0].LABELMARK.Contains("Tui"))
            //    list[0].LABELMARK = list[0].LABELMARK.Substring(0, list[0].LABELMARK.Length - 3);
            list[0].LABELMARK = "T" + list[0].LABELMARK;
            list[0].BarCode = "2@" + bt + "@" + list[0].MaterialNo + "@" + (string.IsNullOrEmpty(list[0].SupCode) ? "0" : list[0].SupCode) + "@" + sumqty + "@" + list[0].PalletNo.Substring(1, 8) + "@" + list[0].PalletNo;

            return list2;
        }

        private static DataTable GetPalletDet(string serialno, ref string sql)
        {
            DbFactory dbFactory = new DbFactory(DbFactory.DbFactoryType.SQLSERVER);
            sql = "select p.palletno,p.palletdetail,o.* from t_pallet p left join t_palletdetail d on p.id = d.headerid left join t_outbarcode o on o.serialno = d.serialno" +
                         " where p.isdel = 1 and p.pallettype = 1 and p.palletno = '" + serialno + "'";
            DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, sql).Tables[0];
            return dt;
        }

        private static void GetPalletCount(string serialno, out decimal count, out decimal sumqty, out string sql)
        {
            count = 0;
            sumqty = 0;
            DbFactory dbFactory = new DbFactory(DbFactory.DbFactoryType.SQLSERVER);
            sql = "select count(1) as count,sum(qty) as qty from t_palletdetail where palletno = '" + serialno + "'";
            using (var dr = dbFactory.ExecuteReader(sql))
            {
                if (dr.Read())
                {
                    count = dr["count"].ToDecimal();
                    sumqty = dr["qty"].ToDecimal();
                }
            }
        }
        #endregion

        #region 取样标签
        ///原来的取样，新标签
        //public string PrintQYAndroid(string json)
        //{
        //    BaseMessage_Model<string> bm = new BaseMessage_Model<string>();
        //    string ipport = "";
        //    string j = "";
        //    string ErrMsg = "";
        //    try
        //    {
        //        List<Barcode_Model> list = Check_Func.DeserializeJsonToList<Barcode_Model>(json);
                
        //        if (list.Count == 0)
        //        {
        //            bm.HeaderStatus = "E";
        //            bm.Message = "没有传入数据";
        //            j = Check_Func.SerializeObject(bm);
        //            return j;
        //        }
        //        ipport = list[0].IP;
               
        //        //拼接取样标签
        //        //1@04X@4100001H11QA@CY1LD12@5@20170623@2017062300083
        //        string pick = list[0].BarCode;
        //        string[] picks = pick.Split('@');
        //        picks[0] = "5";
        //        picks[5] = DateTime.Now.ToString("yyyyMMdd");
        //        picks[6] = picks[5] + new Check_Func().GetTableID("seq_pickbarcode").Replace("PD","");
        //        pick = picks[0] + "@" + picks[1] + "@" + picks[2] + "@" + "0" + "@" + "0" + "@" + "A" + "@" + picks[6];

        //        List<string> sqls = new List<string>(); 
        //        foreach (var item in list)
        //        {
        //            string sql = "insert into T_PICKRELBARCODE (PICKBARCODE,SERIALNO,CREATETIME,MATERIALNO,CREATER,QTY) " +
        //                "values('" + pick + "','" + item.SerialNo + "',SYSDATE,'"+item.MaterialNo+"','"+item.Creater+"',"+item.Qty+")";
        //            sqls.Add(sql);
        //            item.LABELMARK = "QuYang";
        //        }
        //        dbFactory.ExecuteNonQueryList(sqls);

        //        //取出一条记录作为打印的取样标签
        //        //徐鑫传来的数据都是经过汇总的数量，只需要把取样条码赋值给barcode就可以
        //        Barcode_Model printb = list[0];
        //        printb.BarCode = pick;
        //        printb.CreateTime = DateTime.Now;
        //        List<Barcode_Model> printlist = new List<Barcode_Model>();
        //        printlist.Add(printb);

        //        //传入类型为1，标识用lpk打印机打印
        //        bool res = PrintZPL2(printlist, ipport, ref ErrMsg);
        //        if (res)
        //        {
        //            bm.HeaderStatus = "S";
        //            bm.Message = "打印成功";
        //            j = Check_Func.SerializeObject(bm);
        //            return j;
        //        }
        //        else
        //        {
        //            bm.HeaderStatus = "E";
        //            bm.Message = ErrMsg;
        //            j = Check_Func.SerializeObject(bm);
        //            return j;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bm.HeaderStatus = "E";
        //        bm.Message = ex.ToString();
        //        j = Check_Func.SerializeObject(bm);
        //        return j;
        //    }

        //}

        public string PrintQYAndroid(string json)
        {
            BaseMessage_Model<string> bm = new BaseMessage_Model<string>();
            string ipport = "";
            string j = "";
            string ErrMsg = "";
            try
            {
                List<Barcode_Model> list = Check_Func.DeserializeJsonToList<Barcode_Model>(json);

                if (list.Count == 0)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "没有传入数据";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

                ipport = list[0].IP;
                if (!Print_DB.isConnected(ipport))
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "打印机连接失败！";
                    return Check_Func.SerializeObject(bm);
                }


                //记录取样标签

                List<string> sqls = new List<string>();
                foreach (var item in list)
                {
                    string sql = "insert into T_PICKRELBARCODE (PICKBARCODE,CREATETIME,MATERIALNO,CREATER,QTY,BATCHNO) " +
                        "values('" + item.BarCode + "',SYSDATE,'" + item.MaterialNo + "','" + item.Creater + "'," + item.Qty + ",'"+item.BatchNo+"')";
                    sqls.Add(sql);
                    item.CreateTime = DateTime.Now;
                    item.LABELMARK = "QuYang";
                }
                dbFactory.ExecuteNonQueryList(sqls);



                Barcode_Model bm2 = new Barcode_Model();
                bm2.LABELMARK = "QuYang2";
                bm2.MaterialNo = list[0].MaterialNo;
                bm2.Creater = list[0].Creater;
                bm2.Qty = list[0].Qty;
                bm2.CreateTime = list[0].CreateTime;
                bm2.BatchNo = list[0].BatchNo;
                list.Add(bm2);

                bool res = PrintZPL2(list, ipport, ref ErrMsg);
                if (res)
                {
                    bm.HeaderStatus = "S";
                    bm.Message = "打印成功";
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
            catch (Exception ex)
            {
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                j = Check_Func.SerializeObject(bm);
                return j;
            }

        }

        //取样标签补打
        public string QYReprintAndroid(string json)
        {
            BaseMessage_Model<string> bm = new BaseMessage_Model<string>();
            string ipport = "";
            string j = "";
            string ErrMsg = "";

            try
            {
                List<Barcode_Model> list = Check_Func.DeserializeJsonToList<Barcode_Model>(json);
                if (list.Count == 0)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "没有传入数据";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }
                string serialno = list[0].SerialNo;
                ipport = list[0].IP;

                string sql = "select * from T_PICKRELBARCODE where PICKBARCODE like '%" + serialno + "%' order by CREATETIME desc";
                using (var dr = dbFactory.ExecuteReader(sql))
                {
                    if (dr.Read())
                    {
                        //PICKBARCODE,SERIALNO,CREATETIME,MATERIALNO,CREATER,QTY
                        list[0].LABELMARK = "QuYang";
                        list[0].BarCode = dr["PICKBARCODE"].ToDBString();
                        list[0].MaterialNo = dr["MaterialNo"].ToDBString();
                        list[0].Creater = dr["Creater"].ToDBString();
                        list[0].Qty = dr["qty"].ToDecimal();
                        list[0].CreateTime = dr["CreateTime"].ToDateTime();
                        list[0].BatchNo = dr["BatchNo"].ToDBString();
                    }
                    else
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "没有查询到取样信息";
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }
                }
                List<Barcode_Model> plist = new List<Barcode_Model>();
                plist.Add(list[0]);
                bool res = PrintZPL2(plist, ipport, ref ErrMsg);
                if (res)
                {
                    bm.HeaderStatus = "S";
                    bm.Message = "打印成功";
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
            catch(Exception ex)
            {
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                j = Check_Func.SerializeObject(bm);
                return j;
            }
            

        }


        #endregion

        public static bool isConnected(string ipaddress)
        {
            Print_CodeLpk code = new Print_CodeLpk();
            return code.isConnect(ipaddress);
        }

        public static bool isConnectedPrint(string ipaddress, ref string ErrMsg)
        {
            ErrMsg = "";
            Print_CodeLpk code = new Print_CodeLpk();
            return code.Connect(ipaddress, ref ErrMsg);
        }

        //-------------------------------------------------兆信------------------------
        public bool Mes_CheckWoInfoIsOK(string zyid, ref string ErrMsg)
        {
            if (string.IsNullOrEmpty(zyid))
            {
                ErrMsg = "工单不能为空！";
                return false;
            }

            try
            {
                //string sql = "select count(1) from mes_product where zyid='" + zyid + "'";

                //int j = Convert.ToInt32(dbFactory.ExecuteScalar(CommandType.Text, sql));
                //if (j <= 0)
                //{
                //    ErrMsg = "该小工单不存在，请核实！";
                //    return false;
                //}

                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }
        }

        public bool GetMesWoInfoBarcode(string barcode, string workno, string materialno, ref string ErrMsg)
        {
            if (string.IsNullOrEmpty(barcode) || string.IsNullOrEmpty(workno) || string.IsNullOrEmpty(materialno))
            {
                ErrMsg = "条码，工单，物料不能为空！";
                return false;
            }
            //保存
            try
            {
                string sql = "";

                sql = "insert into T_ZXBARCODE values(seq_zx.nextval,'" + barcode + "','" + materialno + "','" + workno + "',0,SYSDATE)";
                int i = dbFactory.ExecuteNonQuery(CommandType.Text, sql);
                if (i > 0)
                    return true;
                else
                {
                    ErrMsg = "插入失败！";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }
          
        }

        public string GetAlert(string type, string subtype)
        {
            List<Alert> list = new List<Alert>();
            string sql ="";
            if(subtype==""||subtype=="全部")
                sql = "select * from mes_alert where  CREATETIME>=to_date('" + DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd") + "','YYYY/MM/DD') and MESSAGETYPE='" + type + "' and ISRETURN <>0 order by ISRETURN desc,CREATETIME desc";
            else
                sql = "select * from mes_alert where  CREATETIME>=to_date('" + DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd") + "','YYYY/MM/DD') and MESSAGETYPE='" + type + "' and MESSAGESUBTYPE='" + subtype + "' and ISRETURN <>0 order by ISRETURN desc,CREATETIME desc";
            DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, sql).Tables[0];
            list = ModelConvertHelper<Alert>.ConvertToModel(dt);
            if (list.Count == 0)
            {
                BaseMessage_Model<List<Alert>> bm = new BaseMessage_Model<List<Alert>>();
                bm.HeaderStatus = "E";
                bm.Message = "没有获取到列表";
                string j = Check_Func.SerializeObject(bm);
                return j;
            }
            else
            {
                foreach (var item in list)
                {
                    item.SCREATETIME = item.CREATETIME.ToString("yyyy-MM-dd HH:mm:ss");
                    if (item.USINGTIME == DateTime.MinValue)
                    {
                        item.SUSINGTIME = "";
                    }
                    else
                        item.SUSINGTIME = item.USINGTIME.ToString("yyyy-MM-dd HH:mm:ss");
                }
                BaseMessage_Model<List<Alert>> bm = new BaseMessage_Model<List<Alert>>();
                bm.HeaderStatus = "S";
                bm.Message = "";
                bm.ModelJson = list;
                string j = Check_Func.SerializeObject(bm);
                return j;
            }
        }

        //level=1 or 2
        public string UpdateAlert(string json,int level)
        {
            BaseMessage_Model<string> bm = new BaseMessage_Model<string>();
            try
            {
                Alert model = Check_Func.DeserializeJsonToObject<Alert>(json);
                string sql = "update mes_alert set ISRETURN=" + level + " , USINGTIME =sysdate,userno =userno||'" + model.USERNO + "-' where id= " + model.ID;
               
                dbFactory.ExecuteNonQuery(CommandType.Text, sql);
                bm.HeaderStatus = "S";
                bm.Message = "成功";
                string j = Check_Func.SerializeObject(bm);
                return j;
            }
            catch (Exception ex)
            {
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                string j = Check_Func.SerializeObject(bm);
                return j;
            }

        }

        public string GetAlertType()
        {
            BaseMessage_Model<List<string>> bm = new BaseMessage_Model<List<string>>();
            try
            {
                string sql = "select t.*, t.rowid from MES_ALERTTYPE t";
                List<string> typs = new List<string>();
                using (var read = dbFactory.ExecuteReader(CommandType.Text, sql))
                {
                    while (read.Read())
                    {
                        typs.Add(read["name"].ToString());
                    }
                }
                bm.HeaderStatus = "S";
                bm.Message = "成功";
                bm.ModelJson = typs;
                string j = Check_Func.SerializeObject(bm);
                return j;
            }
            catch (Exception ex)
            {
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                string j = Check_Func.SerializeObject(bm);
                return j;
            }

        }

        public string GetSubAlertType(string name)
        {
            BaseMessage_Model<List<string>> bm = new BaseMessage_Model<List<string>>();
            try
            {
                string sql = "select b.subname from MES_ALERTTYPE t inner join MES_SUBALERTTYPE b on t.id = b.pid where t.name='"+name+"'";
                List<string> typs = new List<string>();
                using (var read = dbFactory.ExecuteReader(CommandType.Text, sql))
                {
                    while (read.Read())
                    {
                        typs.Add(read["subname"].ToString());
                    }
                }
                bm.HeaderStatus = "S";
                bm.Message = "成功";
                bm.ModelJson = typs;
                string j = Check_Func.SerializeObject(bm);
                return j;
            }
            catch (Exception ex)
            {
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                string j = Check_Func.SerializeObject(bm);
                return j;
            }

        }


        public bool tuisong(ref string err)
        {
            //没推送是0，推送过是3，一级人员回复1，二级人员回复2
            string sql = "select * from mes_alert where isreturn =0 or isreturn =3";
            DataTable dt = dbFactory.ExecuteDataSet(CommandType.Text, sql).Tables[0];
            List<Alert> list = ConvertToModel<Alert>(dt);
            if (list.Count == 0)
            {
                err = "没有需要推送的数据";
                return false;
            }
            string time = ConfigurationManager.ConnectionStrings["alerttime"].ConnectionString;
            string url = ConfigurationManager.ConnectionStrings["alerturl"].ConnectionString;
            string para = "";
            StringBuilder ps = new StringBuilder();
            List<string> sqls = new List<string>();
            try
            {
                foreach (var item in list)
                {
                    //第一种，状态为0全部推送给一级
                    if (item.ISRETURN == 0)
                    {
                        para = MakeAlertJson(para, item);
                        ps.Append(para + "\r\n");
                        //推送代码
                        CreateGetHttpResponse(url + para, 5000, "", null);
                        sql = "update mes_alert set ISRETURN = 3 where id =" + item.ID;
                        sqls.Add(sql);
                    }
                    //第二种，状态为3时（已经推送），查看当前时间和创建时间是否超过半个小时，超过就推送二级
                    else if (item.ISRETURN == 3)
                    {
                        TimeSpan timeSpan = DateTime.Now - item.CREATETIME;
                        if (timeSpan.TotalMinutes >= int.Parse(time))
                        {
                            para = MakeAlertJson(para, item);
                            ps.Append(para + "\r\n");
                            //推送代码
                            CreateGetHttpResponse(url + para, 5000, "", null);
                            sql = "update mes_alert set ISRETURN = 4 where id =" + item.ID;
                            sqls.Add(sql);
                        }
                        else
                        {
                            err = "没有需要推送的数据,没有到时间推送给领导";
                            continue;
                        }
                    }
                }
            }
            catch(Exception ex) {
                err = ex.ToString();
                return false;
            }
            try
            {
                dbFactory.ExecuteNonQueryList(sqls);
            }
            catch (Exception ex)
            {
                err = "推送成功，修改数据失败 "+ex.ToString();
                return false;
            }
            err = ps.ToString();
            return true;
        }

        private static string MakeAlertJson(string para, Alert item)
        {
            para = "";
            para += "MESSAGETYPE:" + item.MESSAGETYPE+"@";
            para += "MESSAGEDESC:" + item.MESSAGEDESC + "@";
            para += "ISRETURN:" + item.ISRETURN + "@";
            para += "LINENO:" + item.LINENO+ "@";
            para += "CREATETIME:" + item.CREATETIME.ToString("yyyy-MM-dd_HH:MM:ss") + "@";
            para += "REMARK:" + item.REMARK + "@";
            para += "MESSAGESUBTYPE:" + item.MESSAGESUBTYPE;
           
           
            return para;
        }

        public string CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            //request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string responseText = myreader.ReadToEnd();
            myreader.Close();
            return responseText;

        }
       


    }
}



