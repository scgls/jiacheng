using BILWeb.Stock;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using BILBasic.Common;
using System.Configuration;
using BILBasic.DBA;
using BILBasic.Basing;

namespace BILWeb.Query
{
    public class Query_dll
    {
        public DbFactory dbFactory = new DbFactory(DbFactory.DbFactoryType.SQLSERVER);

        public static string HttpPost(string json)
        {
            UTF8Encoding encoding = new UTF8Encoding();


            byte[] data = encoding.GetBytes(json);

            // Prepare web request...  
            string url = ConfigurationManager.ConnectionStrings["url"].ConnectionString;
            HttpWebRequest myRequest =
             (HttpWebRequest)WebRequest.Create(url);

            myRequest.Method = "POST";
            myRequest.ContentType = "application/json; charset=utf-8";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();

            // Send the data.  
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            // Get response  
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();
            return content;
        }
        public bool GetErpStock(T_StockInfoEX model, ref List<CheckAnalyze> listerp, ref List<CheckAnalyze> listwms,ref string ErrMsg)
        {
            string json = GetErpJson(model);


            JObject jo = JObject.Parse(json);
            JToken jt = jo["payload"]["std_data"]["execution"];
            string code = jt["code"].ToString();
            string description = jt["description"].ToString();
            if (code != "0")
            {
                ErrMsg = "没有ERP数据";
                return false;
            }

            JToken jt2 = jo["payload"]["std_data"]["parameter"]["data"];
            
            //得到erp数据
            foreach (JToken jtn in jt2)
            {
                CheckAnalyze erprow = new CheckAnalyze();
                erprow.STRONGHOLDCODE = jtn["Head"]["site_id"].ToDBString();
                erprow.MATERIALNO = jtn["Head"]["item_no"].ToDBString();
                erprow.warehouseno = jtn["Head"]["inv_id"].ToDBString();
                erprow.AREANO = jtn["Head"]["location_id"].ToDBString();
                erprow.BatchNo = jtn["Head"]["batch_no"].ToDBString();
                erprow.status = jtn["Head"]["qc_code"].ToInt32();
                if (string.IsNullOrEmpty(jtn["Head"]["qc_code"].ToDBString())) 
                {
                    erprow.status = 1;
                }
                switch (erprow.status)
                { 
                    case 1:
                        erprow.statusname = "合格";
                        break;
                    case 0:
                        erprow.statusname = "待检";
                        break;
                    case 2:
                        erprow.statusname = "不合格";
                        break;
                }
                erprow.QTY = jtn["Head"]["inv_amount"].ToDecimal();
                //erprow.EDATE = jtn["Head"]["valid_date"].ToDateTime();
                listerp.Add(erprow);
            }

      
            //去重,去除因为edate而重复的行
            List<CheckAnalyze> listerp2 = listerp;
            listerp = listerp2.Where((x, i) => listerp2.FindIndex(z => z.STRONGHOLDCODE == x.STRONGHOLDCODE && z.MATERIALNO == x.MATERIALNO && z.warehouseno == x.warehouseno && z.AREANO == x.AREANO && z.BatchNo == x.BatchNo && z.statusname == x.statusname && z.QTY == x.QTY) == i).ToList();


            //-------------------------------------获取type=2时的去除库位的整合数据
            if (model.XH == "2")
            {    
                var le =from t in listerp
                        group t by new { t1 = t.STRONGHOLDCODE, t2 = t.MATERIALNO ,t3 = t.warehouseno,t5 = t.BatchNo,t6 = t.status,t7 = t.statusname} into m
                        select new CheckAnalyze
                        {
                            STRONGHOLDCODE = m.Key.t1,                          
                            MATERIALNO =m.Key.t2,
                            warehouseno = m.Key.t3,
                            BatchNo = m.Key.t5,
                            status = m.Key.t6,
                            statusname = m.Key.t7,
                            QTY = m.Sum(item => item.QTY),       
                        };
                listerp = le.ToList();
            }

            //-------------------------------------

            //获取wms数据，不带edate     ,s.EDATE   ,EDATE ,,and STRONGHOLDCODE = '" + model.StrongHoldCode + "'
            if(model.XH=="1")
            {
                string sql = "select s.STRONGHOLDCODE,s.MATERIALNO,v.warehouseno,v.AREANO,s.BatchNo,s.status,sum(s.QTY) as qty from t_stock s left join v_area v on s.areaid=v.id group by " +
                   "STRONGHOLDCODE,MATERIALNO,warehouseno,AREANO,BatchNo,status having 1=1  and warehouseno = '" + model.WarehouseNo + "'";
                if (model.MaterialNo != "" && !model.MaterialNo.Contains(','))
                    sql += " and MaterialNo = '" + model.MaterialNo + "'";
                else if (model.MaterialNo != "" && model.MaterialNo.Contains(','))
                {
                    string MaterialNo = model.MaterialNo;
                    Query_Func.ChangeQuery(ref MaterialNo);
                    sql += " and MaterialNo in(" + MaterialNo + ")";
                }

                if (model.AreaNo != "" && !model.AreaNo.Contains(','))
                    sql += " and AreaNo = '" + model.AreaNo + "'";
                else if (model.AreaNo != "" && model.AreaNo.Contains(','))
                {
                    string AreaNo = model.AreaNo;
                    Query_Func.ChangeQuery(ref AreaNo);
                    sql += " and AreaNo in(" + AreaNo + ")";
                }

                if (model.BatchNo != "" && !model.BatchNo.Contains(','))
                    sql += " and BatchNo = '" + model.BatchNo + "'";
                else if (model.BatchNo != "" && model.BatchNo.Contains(','))
                {
                    string BatchNo = model.BatchNo;
                    Query_Func.ChangeQuery(ref BatchNo);
                    sql += " and BatchNo in(" + BatchNo + ")";
                }

                if (model.StrongHoldCode != "" && !model.StrongHoldCode.Contains(','))
                    sql += " and StrongHoldCode = '" + model.StrongHoldCode + "'";
                else if (model.StrongHoldCode != "" && model.StrongHoldCode.Contains(','))
                {
                    string StrongHoldCode = model.StrongHoldCode;
                    Query_Func.ChangeQuery(ref StrongHoldCode);
                    sql += " and StrongHoldCode in(" + StrongHoldCode + ")";
                }

                //得到wms数据
                using (var dr = dbFactory.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        CheckAnalyze ca = new CheckAnalyze();
                        ca.SSTRONGHOLDCODE = dr["STRONGHOLDCODE"].ToDBString();
                        ca.SMATERIALNO = dr["MATERIALNO"].ToDBString();
                        ca.swarehouseno = dr["warehouseno"].ToDBString();
                        ca.SAREANO = dr["AREANO"].ToDBString();
                        ca.SBatchNo = dr["BatchNo"].ToDBString();
                        ca.sstatus = dr["status"].ToInt32();
                        switch (ca.sstatus)
                        {
                            case 1:
                                ca.sstatusname = "待检";
                                break;
                            case 2:
                                ca.sstatusname = "待检";
                                break;
                            case 3:
                                ca.sstatusname = "合格";
                                break;
                            case 4:
                                ca.sstatusname = "不合格";
                                break;
                        }
                        ca.SQTY = dr["QTY"].ToDecimal();
                        //ca.SEDATE = dr["EDATE"].ToDateTime();
                        listwms.Add(ca);
                    }
                }
            }
            //类型2，仓库对比
            else if (model.XH == "2")
            {
                string sql = "select s.STRONGHOLDCODE,s.MATERIALNO,v.warehouseno,s.BatchNo,s.status,sum(s.QTY) as qty from t_stock s left join v_area v on s.areaid=v.id group by " +
                       "STRONGHOLDCODE,MATERIALNO,warehouseno,BatchNo,status having 1=1  and warehouseno = '" + model.WarehouseNo + "'";
                if (model.MaterialNo != "" && !model.MaterialNo.Contains(','))
                    sql += " and MaterialNo = '" + model.MaterialNo + "'";
                else if (model.MaterialNo != "" && model.MaterialNo.Contains(','))
                {
                    string MaterialNo = model.MaterialNo;
                    Query_Func.ChangeQuery(ref MaterialNo);
                    sql += " and MaterialNo in(" + MaterialNo + ")";
                }

                //if (model.AreaNo != "" && !model.AreaNo.Contains(','))
                //    sql += " and AreaNo = '" + model.AreaNo + "'";
                //else if (model.AreaNo != "" && model.AreaNo.Contains(','))
                //{
                //    string AreaNo = model.AreaNo;
                //    Query_Func.ChangeQuery(ref AreaNo);
                //    sql += " and AreaNo in(" + AreaNo + ")";
                //}

                if (model.BatchNo != "" && !model.BatchNo.Contains(','))
                    sql += " and BatchNo = '" + model.BatchNo + "'";
                else if (model.BatchNo != "" && model.BatchNo.Contains(','))
                {
                    string BatchNo = model.BatchNo;
                    Query_Func.ChangeQuery(ref BatchNo);
                    sql += " and BatchNo in(" + BatchNo + ")";
                }

                if (model.StrongHoldCode != "" && !model.StrongHoldCode.Contains(','))
                    sql += " and StrongHoldCode = '" + model.StrongHoldCode + "'";
                else if (model.StrongHoldCode != "" && model.StrongHoldCode.Contains(','))
                {
                    string StrongHoldCode = model.StrongHoldCode;
                    Query_Func.ChangeQuery(ref StrongHoldCode);
                    sql += " and StrongHoldCode in(" + StrongHoldCode + ")";
                }

                //得到wms数据
                using (var dr = dbFactory.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        CheckAnalyze ca = new CheckAnalyze();
                        ca.SSTRONGHOLDCODE = dr["STRONGHOLDCODE"].ToDBString();
                        ca.SMATERIALNO = dr["MATERIALNO"].ToDBString();
                        ca.swarehouseno = dr["warehouseno"].ToDBString();
                        //ca.SAREANO = dr["AREANO"].ToDBString();
                        ca.SBatchNo = dr["BatchNo"].ToDBString();
                        ca.sstatus = dr["status"].ToInt32();
                        switch (ca.sstatus)
                        {
                            case 1:
                                ca.sstatusname = "待检";
                                break;
                            case 2:
                                ca.sstatusname = "待检";
                                break;
                            case 3:
                                ca.sstatusname = "合格";
                                break;
                            case 4:
                                ca.sstatusname = "不合格";
                                break;
                        }
                        ca.SQTY = dr["QTY"].ToDecimal();
                        //ca.SEDATE = dr["EDATE"].ToDateTime();
                        listwms.Add(ca);
                    }
                }
            }

           
           
            //if (listwms.Count == 0)
            //{
            //    ErrMsg = "没有WMS数据";
            //    return false;
            //}






            //合并数据
              //erprow.STRONGHOLDCODE = jtn["Head"]["site_id"].ToDBString();
              //  erprow.MATERIALNO = jtn["Head"]["item_no"].ToDBString();
              //  erprow.warehouseno = jtn["Head"]["inv_id"].ToDBString();
              //  erprow.AREANO = jtn["Head"]["location_id"].ToDBString();
              //  erprow.BatchNo = jtn["Head"]["batch_no"].ToDBString();
              //  erprow.status = jtn["Head"]["qc_code"].ToInt32();
              //  erprow.QTY = jtn["Head"]["inv_amount"].ToDecimal();
            List<CheckAnalyze> newlist = new List<CheckAnalyze>();
            decimal? count = 0;decimal? count2 = 0;
            if (model.XH == "1")
            {
                int k = 1;
                foreach (CheckAnalyze erp in listerp)
                {
                    //关联时要通过状态链接,通过据点链接
                    //&&p.sstatusname==erp.statusname
                    var wms = listwms.Find(p => p.SMATERIALNO == erp.MATERIALNO && p.SAREANO == erp.AREANO && p.SBatchNo == erp.BatchNo && p.sstatusname == erp.statusname && p.SSTRONGHOLDCODE == erp.STRONGHOLDCODE);
                    if (wms == null)
                    {
                        erp.partno = k + "";
                        erp.remark = "赢";
                        newlist.Add(erp);
                        k++;
                    }
                    else
                    {
                        erp.partno = k + "";
                        erp.SMATERIALNO = wms.SMATERIALNO;
                        erp.SAREANO = wms.SAREANO;
                        erp.SSTRONGHOLDCODE = wms.SSTRONGHOLDCODE;
                        erp.SBatchNo = wms.SBatchNo;
                        erp.sstatusname = wms.sstatusname;
                        erp.SQTY = wms.SQTY;
                        if (erp.QTY > erp.SQTY)
                            erp.remark = "赢";
                        else if (erp.QTY == erp.SQTY)
                            erp.remark = "平";
                        else
                            erp.remark = "亏";
                        ////判断合格不同状态
                        //if (erp.statusname != erp.sstatusname)
                        //    erp.remark += "|质检差异";
                        ////判断合格不同状态
                        //if (erp.STRONGHOLDCODE != erp.SSTRONGHOLDCODE)
                        //    erp.remark += "|据点差异";
                        newlist.Add(erp);
                        k++;
                    }
                }
                foreach (CheckAnalyze wms in listwms)
                {
                    //&&p.sstatusname==wms.sstatusname
                    var w = newlist.Find(p => p.SMATERIALNO == wms.SMATERIALNO && p.SAREANO == wms.SAREANO && p.SBatchNo == wms.SBatchNo && p.sstatusname == wms.sstatusname && p.SSTRONGHOLDCODE == wms.SSTRONGHOLDCODE);
                    if (w == null)
                    {
                        wms.partno = k + "";
                        wms.remark = "亏";
                        //wms.STRONGHOLDCODE = wms.SSTRONGHOLDCODE;
                        wms.warehouseno = wms.swarehouseno;
                        newlist.Add(wms);
                        k++;
                    }
                }
            }
            else if (model.XH == "2")
            {
                int k = 1;
                foreach (CheckAnalyze erp in listerp)
                {
                    //关联时要通过状态链接,通过据点链接
                    //&&p.sstatusname==erp.statusname
                    var wms = listwms.Find(p => p.SMATERIALNO == erp.MATERIALNO && p.SBatchNo == erp.BatchNo && p.sstatusname == erp.statusname && p.SSTRONGHOLDCODE == erp.STRONGHOLDCODE);
                    if (wms == null)
                    {
                        erp.partno = k + "";
                        erp.remark = "赢";
                        newlist.Add(erp);
                        k++;
                    }
                    else
                    {
                        erp.partno = k + "";
                        erp.SMATERIALNO = wms.SMATERIALNO;
                        //erp.SAREANO = wms.SAREANO;
                        erp.SSTRONGHOLDCODE = wms.SSTRONGHOLDCODE;
                        erp.SBatchNo = wms.SBatchNo;
                        erp.sstatusname = wms.sstatusname;
                        erp.SQTY = wms.SQTY;
                        if (erp.QTY > erp.SQTY)
                            erp.remark = "赢";
                        else if (erp.QTY == erp.SQTY)
                            erp.remark = "平";
                        else
                            erp.remark = "亏";
                        ////判断合格不同状态
                        //if (erp.statusname != erp.sstatusname)
                        //    erp.remark += "|质检差异";
                        ////判断合格不同状态
                        //if (erp.STRONGHOLDCODE != erp.SSTRONGHOLDCODE)
                        //    erp.remark += "|据点差异";
                        newlist.Add(erp);
                        k++;
                    }
                }
                foreach (CheckAnalyze wms in listwms)
                {
                    //&&p.sstatusname==wms.sstatusname
                    var w = newlist.Find(p => p.SMATERIALNO == wms.SMATERIALNO && p.SBatchNo == wms.SBatchNo && p.sstatusname == wms.sstatusname && p.SSTRONGHOLDCODE == wms.SSTRONGHOLDCODE);
                    if (w == null)
                    {
                        wms.partno = k + "";
                        wms.remark = "亏";
                        //wms.STRONGHOLDCODE = wms.SSTRONGHOLDCODE;
                        wms.warehouseno = wms.swarehouseno;
                        newlist.Add(wms);
                        k++;
                    }
                }
            }
           


            listerp = newlist;
            foreach (var item in listerp)
            {
                count += item.QTY.ToDecimal();
                count2 += item.SQTY.ToDecimal();
            }
            CheckAnalyze c = new CheckAnalyze();
            c.QTY = count;
            c.SQTY = count2;
            c.remark = "";
            listerp.Add(c);
            return true;
        }

        private static string GetErpJson(T_StockInfoEX model)
        {
            string basejson = "{\"key\":\"f5458f5c0f9022db743a7c0710145903\",\"type\":\"sync\",\"host\":{\"prod\":\"" + ConfigurationManager.ConnectionStrings["prod"].ConnectionString + "\",\"ip\":\"10.40.71.91\",\"lang\":\"zh_CN\",\"acct\":\"tiptop\",\"timestamp\":\"20151211123204361\"},\"service\":{\"prod\":\"T100\",\"name\":\"return.inventory.info.get\",\"ip\":\"10.1.254.26\",\"id\":\"topprd\"},\"datakey\":{\"EntId\":\"10\",\"CompanyId\":\"CY1\"},\"payload\":{\"std_data\":{\"parameter\":@}}}";
            string[] matenos = model.MaterialNo.Split(',');
            string[] areanos = model.AreaNo.Split(',');
            string[] batchs = model.BatchNo.Split(',');
            string[] judians = model.StrongHoldCode.Split(',');

            JObject jp = new JObject();
            JArray arr = new JArray();
            jp.Add(new JProperty("data", arr));

            foreach (string mate in matenos)
            {
                foreach (string area in areanos)
                {
                    foreach (string bat in batchs)
                    {
                        foreach (string ju in judians)
                        {
                            JObject content = new JObject();
                            content.Add(new JProperty("data_site", ju));
                            content.Add(new JProperty("data_item", mate));
                            content.Add(new JProperty("data_inv", model.WarehouseNo));
                            content.Add(new JProperty("data_location", area));
                            content.Add(new JProperty("data_batch", bat));
                            arr.Add(content);
                        }
                       
                    }
                }
            }

            basejson = basejson.Replace("@", jp.ToString());

            string json = HttpPost(basejson);
            return json;
        }
    }
}
