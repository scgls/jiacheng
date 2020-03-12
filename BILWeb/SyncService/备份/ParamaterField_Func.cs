using BILBasic.Basing.Factory;
using BILWeb.SyncService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.SyncService
{
    public class ParamaterField_Func : TBase_Func<ParamaterFiled_DB, ParamaterField_Model>
    {

        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="StockType">类型 10：入库 20:出库  99:基础资料</param>
        /// <param name="LastSyncTime">最后更新时间</param>
        /// <param name="ErrMsg">返回错误信息</param>
        /// <param name="syncType">同步数据来源 ERP或者 EXCEL</param>
        /// <param name="syncVouType">Excel单据类型</param>
        /// <returns></returns>
        public bool Sync(int StockType, string LastSyncTime, string ErpVoucherNo,int wmsVourcherType, ref string ErrMsg, string syncType, int syncExcelVouType, DataSet ds)
        {
            bool ReturnFlag = false;
            //2.根据ERP单据类型，最后更新时间，从ERP获取单据（JSON格式）
            string DataJson = string.Empty;
            ReturnFlag = syncType.ToUpper() == "ERP" ?
                GetERPRecord(StockType, LastSyncTime, ErpVoucherNo, wmsVourcherType, DataJson, ref ErrMsg, syncType) :
                GetExcelRecord(StockType, ds, ref ErrMsg, syncType, wmsVourcherType);
            return ReturnFlag;
        }


        /// <summary>
        /// 同步工单
        /// </summary>
        /// <param name="type"></param>
        /// <param name="lastSyncTime"></param>
        /// <param name="ErpVoucherNo"></param>
        /// <param name="wmsVourcherType"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool SyncWO( string lastSyncTime, string ErpVoucherNo, ref string errMsg,int type=3, int wmsVourcherType=32)
        {
            ParamaterFiled_DB db = new ParamaterFiled_DB();
            string[] filter = ErpVoucherNo.Split('-');
            string CompanyNo = "";
            string ErpvourcherType ="";
            if (filter.Length == 3)
            {
                CompanyNo = filter[0];
                ErpvourcherType = filter[1];
            }

            List<ParamaterField_Model> pmList = new List<ParamaterField_Model>();
            bool result = GetPMList(type, ref pmList, ref errMsg, wmsVourcherType, CompanyNo, ErpvourcherType);
            if (!result)
            {
                errMsg = "未配置单据类型！";
                return false;
            }

            var TypeList = pmList.DistinctBy(s => new { s.ErpVourcherType, s.CompanyNo });
            string dataJson = string.Empty;
            bool returnValue = true;
            foreach (var Type in TypeList)
            {
                dataJson = String.Empty;
                string ErpvouType = Type.ErpVourcherType.ToString();
                string WmsvouType = Type.VoucherType.ToString();
                string companyNo = Type.CompanyNo.ToString();
                string json = GetERPInfo(type, companyNo, ErpvouType, WmsvouType, ErpVoucherNo, lastSyncTime);
                result = GetDataJson(json, ref dataJson, ref errMsg);
                if (result)
                {
                    if (pmList.Count > 0 && !String.IsNullOrEmpty(dataJson.TrimStart('[').TrimEnd(']')))
                    {

                        LogNet.DebugInfo("ERP同步工单号:" + ErpVoucherNo + "  \r\n ERP返回Json：" + dataJson);
                        List<ParamaterField_Model> pmListbyType = pmList.FindAll(p => p.VoucherType.ToString() == WmsvouType);

                        List<WOReturnModel> WOReturns = new List<WOReturnModel>();

                        result = db.WOComparerListAndCreateSQL(pmListbyType, dataJson, ErpvouType, Type.VoucherType, ref WOReturns,ref errMsg, "ERP");
                        if (!result) return result;
                        if (WOReturns.Count != 0)
                        {
                            string returnJson = JsonConvert.SerializeObject(WOReturns);
                            string returnErr = "";
                           if( !SubmitMesStatus(returnJson, ref returnErr))
                            {
                                returnValue = false;
                                errMsg += ErpVoucherNo+":"+returnErr + "\r\n";
                            }
                        }
                    }
                }
            }
           return returnValue;
        }

        public  bool SubmitMesStatus(string returnJson, ref string errMsg)
        {
            BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
            string result= TIF.PostModelListToInterface(returnJson);
            BaseMessage_Model<WOReturnModel> model = new BaseMessage_Model<WOReturnModel>();
            model = JsonConvert.DeserializeObject<BaseMessage_Model<WOReturnModel>>(result);
            if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
            {
                errMsg = model.Message;
                return false;
            }
            return true;
        }





        /// <summary>
        /// 获取数据Json
        /// </summary>
        private bool GetDataJson(string json, ref string dataJson, ref string errMsg)
        {
            bool result = false;
            JsonModel jsmodel = JsonConvert.DeserializeObject<JsonModel>(json);
            if (jsmodel.result.ToString() == "1")
            {
                result = true;
                if (jsmodel.data != null)
                {
                    dataJson = jsmodel.data.ToString().Trim();
                }
            }
            else
            {
                errMsg += jsmodel.resultValue + "\r\n";
            }
            return result;
            //bool result = false;
            //JsonModel jsmodel = JsonConvert.DeserializeObject<JsonModel>(Json);
            //if (jsmodel.payload.std_data.execution.code.Equals("0"))
            //{
            //    result = true;
            //    if (jsmodel.payload.std_data.parameter.data != null)
            //    {
            //        dataJson = jsmodel.payload.std_data.parameter.data.ToString().Trim();
            //  }
            //}
            //else
            //{
            //    errMsg += jsmodel.payload.std_data.execution.description + "|";
            //}
            //return result;






        }


        /// <summary>
        /// 获取ERP单据
        /// </summary>
        /// <param name="StockType">类型 10：入库 20:出库  99:基础资料</param>
        /// <param name="LastSyncTime">最后更新时间</param>
        /// <param name="dataJson">json数据</param>
        /// <param name="ErrMsg">返回错误信息</param>
        /// <returns>返回Json数据</returns>
        private bool GetERPRecord(int stockType, string lastSyncTime, string ErpVoucherNo, int wmsVourcherType, string dataJson, ref string errMsg, string syncType)
        {
            bool result = false;
            //根据stockType查询出入库类型（T_PARAMETER表-出入库类型）
            ParamaterFiled_DB db = new ParamaterFiled_DB();
            int type = db.GetStockType(stockType);
            string[] filter = ErpVoucherNo.Split('-');
            string CompanyNo = "";
            string ErpvourcherType = "";
            if (filter.Length == 3)
            {
                CompanyNo = filter[0];
                ErpvourcherType =  filter[1];
                if (ErpVoucherNo.ToUpper().Contains("NSH") || ErpVoucherNo.ToUpper().Contains("RK7"))
                    ErpvourcherType = "";
            }

            List<ParamaterField_Model> pmList = new List<ParamaterField_Model>();
            result = GetPMList(type, ref pmList, ref errMsg, wmsVourcherType, CompanyNo, ErpvourcherType);
            if (result)
            {
                try
                {
                    var TypeList = pmList.DistinctBy(s => new { s.ErpVourcherType, s.CompanyNo });

                    //按照单据类型循环
                    foreach (var Type in TypeList)
                    {
                        dataJson = String.Empty;
                        string ErpvouType = Type.ErpVourcherType.ToString();
                        string WmsvouType = Type.VoucherType.ToString();
                        string companyNo = Type.CompanyNo.ToString();

                        //判断ERP单据号是否包含单据类型
                        //if (!ErpVoucherNo.ToUpper().Contains("NSH") && !ErpVoucherNo.ToUpper().Contains("RK7"))
                        //{
                        //    if (ErpVoucherNo != "" && !ErpVoucherNo.Contains(ErpvouType))
                        //        continue;
                        //}

                        //根据ERP单据类型和最后同步时间，查询ERP单据数据

                        string json = "{\"result\":\"1\",\"resultValue\":\"\",\"data\":[{\"head\":{\"standard_box2\":null,\"standard_box3\":null,\"sto_condition\":null,\"spc_require\":null,\"protect_way\":null,\"EntId\":null,\"item_spec\":\"个\",\"item_unit\":null,\"group_code\":null,\"group_name\":null,\"classfiy_code\":null,\"classfiy_name\":null,\"purchase_group_code\":null,\"purchase_group_name\":null,\"main_supplier\":null,\"quality_month\":null,\"quality_day\":0,\"item_brand\":null,\"origin_place\":null,\"life_cycle\":null,\"pack_quantity\":0,\"item_size\":null,\"pallet_size\":null,\"pallet_amount\":null,\"all_size\":null,\"item_weight\":null,\"status\":null,\"standard_box1\":null,\"customer\":null,\"standard_box\":null,\"brand_intro\":null,\"bar_code\":\"\",\"Companyid\":null,\"item_name_us\":null,\"item_no\":\"14H22Q1\",\"item_name\":\"ALBION产品托盘\",\"body\":null}}],\"MaterialDoc\":null,\"MaterialYear\":null,\"QualityNo\":null,\"GUID\":null,\"DeliveryNo\":null}"; //GetERPInfo(stockType, companyNo, ErpvouType, WmsvouType, ErpVoucherNo, lastSyncTime);

                        result = GetDataJson(json, ref dataJson, ref errMsg);


                        //add by cym 2018-1-10
                        LogNet.LogInfo("ERP同步工单号:" + ErpVoucherNo + "  ERP返回Json：" + dataJson);

                        if (result)
                        {

                            errMsg += " \r\nerp类型：" + companyNo + "-" + ErpvouType + "\r\n" + dataJson + "\r\n";
                            //4.根据T_PARAMETERTABLE中ID于T_PARAMETERFIELD中的TableID对应关系，查询具体需要插入表字段
                            //3.将获取数据进行解析，比对获取的WMS字段名与ERP字段名，
                            //  dataJson = "[{\"DocEntry\":null,\"DocNum\":null,\"ObjType\":0,\"CardCode\":\"\",\"CardName\":\"\",\"SlpCode\":null,\"CreateDate\":\"0001-01-01T00:00:00\",\"UpdateDate\":\"0001-01-01T00:00:00\",\"U_ljlx\":\"1\",\"Filler\":\"PART,DRIP RAIL,MGL8300/8400\",\"Protype\":null,\"ItemCode\":\"M10262\",\"ProCode\":\"10-5395\",\"ItemName\":\"DLS MGL8300/8400接口盖板\",\"PlannedQty\":0.0,\"CmpltQty\":0.0,\"ISUPDATE\":\"1\",\"KEY\":\"ItemCode\"},{\"DocEntry\":null,\"DocNum\":null,\"ObjType\":0,\"CardCode\":\"\",\"CardName\":\"\",\"SlpCode\":null,\"CreateDate\":\"0001-01-01T00:00:00\",\"UpdateDate\":\"0001-01-01T00:00:00\",\"U_ljlx\":\"1\",\"Filler\":\"\",\"Protype\":null,\"ItemCode\":\"M10416\",\"ProCode\":\"VX3209-SW\",\"ItemName\":\"优派（ViewSonic）VX3209-SW 32英寸广视角 纤薄侧边颜值电脑显示器\",\"PlannedQty\":0.0,\"CmpltQty\":0.0,\"ISUPDATE\":\"1\",\"KEY\":\"ItemCode\"}]";
                            if (pmList.Count > 0 && !String.IsNullOrEmpty(dataJson.TrimStart('[').TrimEnd(']')))
                            {
                                List<ParamaterField_Model> pmListbyType = pmList.FindAll(p => p.VoucherType.ToString() == WmsvouType);

                                List<OrderReturnModel> orderReturns = new List<OrderReturnModel>();

                                result =  db.ComparerListAndCreateSQL(pmListbyType, dataJson, ErpvouType, Type.VoucherType, ref orderReturns, ref errMsg, syncType);
                                if (!result) return result;

                                if (orderReturns.Count != 0)
                                {
                                    string returnJson = JsonConvert.SerializeObject(orderReturns);
                                    string returnErr = "";
                                    if (!SubmitMesStatus(returnJson, ref returnErr))
                                    {
                                        result = false;
                                        errMsg += ErpVoucherNo + ":" + returnErr + "\r\n";
                                    }
                                }

                               
                            }
                        }
                       }
                    }
                catch (Exception ex)
                {
                    result = false;
                    errMsg += ex.Message + "|";
                }

            }
            return result;
        }


     

        /// <summary>
        /// 1.按出入库类型查询单据类型（T_PARAMETERVOU）
        ///3.根据WMS单据类型，查询需要插入/更新表名称（T_PARAMETERTABLE）
        /// </summary>
        /// <returns></returns>
        private bool GetPMList(int type, ref  List<ParamaterField_Model> pmList, ref string errMsg,int wmsVourcherType,string CompanyNo,string ErpVoucherType)
        {
            pmList = new List<ParamaterField_Model>();
            string filter = "InStockType="+ type+(wmsVourcherType == -1? "": " and VoucherType=" + wmsVourcherType);
            if (CompanyNo != "")
                filter += " and \"CompanyNo\"='" + CompanyNo + "'";
            if (ErpVoucherType != "")
                filter += " and \"ErpVourcherType\"='" + ErpVoucherType + "'";
            return base.GetModelListByFilter(ref pmList, ref errMsg, "", filter);
        }



        /// <summary>
        /// 获取ERP数据
        /// </summary>
        /// <param name="type">单据类型</param>
        /// <param name="lastSyncTime">最后同步时间</param>
        /// <returns></returns>
        private string GetERPInfo(int stockType,string companyNo,string erpvouType, string wmsvouType, string ErpVoucherNo, string lastSyncTime)
        {
            //ERP单据号不为空的情况下，查询该单据的最后同步时间
            string lastSyncErpVoucherNo = String.Empty;
            ParamaterFiled_DB db = new ParamaterFiled_DB();
            if (!String.IsNullOrEmpty(ErpVoucherNo))
            {
                lastSyncTime = db.GetLastSyncTime(stockType,ErpVoucherNo, companyNo, erpvouType);
            }
            else
            {
                if (stockType != 99)
                    lastSyncErpVoucherNo =db.getLastSyncErpVoucherNo(stockType, wmsvouType, companyNo, erpvouType);// companyNo + "-" + erpvouType + "-" + "1702150000";// 
            }

            if (wmsvouType == "24" || wmsvouType == "23")//|| wmsvouType == "23"
            {
                lastSyncErpVoucherNo = string.Empty;
                if (String.IsNullOrEmpty(lastSyncTime))
                    lastSyncTime = DateTime.Parse(lastSyncTime).AddDays(-10).ToString("yyyy-MM-dd");
                else
                    lastSyncTime = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd");

            }
            
            BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
            // lastSyncErpVoucherNo = "CY1-PO1-1607020001";
            //string json = "{\"ErpVoucherNo\":\"" + ErpVoucherNo + "\",\"lastSyncErpVoucherNo\":\"" + lastSyncErpVoucherNo + "\",\"VoucherType\":\"" + wmsvouType + "\",\"ErpVoucherType\":\"" + erpvouType + "\",\"datetime\":\"" + lastSyncTime + "\"}";

            string stringVoucherNo = ErpVoucherNo.ToUpper().Contains("NSH") || ErpVoucherNo.ToUpper().Contains("RK7") ? "\"data_no1\":\"" + ErpVoucherNo : "\"data_no\":\"" + ErpVoucherNo;
            string json = "{"+stringVoucherNo + "\",\"data_maxno\":\"" + lastSyncErpVoucherNo + "\",\"data_type\":\"" + erpvouType + "\",\"VoucherType\":\"" + wmsvouType + "\",\"companyNo\":\"" + companyNo + "\",\"edit_time\":\"" +DateTime.Parse(lastSyncTime).ToString("yyyy-MM-dd") + "\"}";
      //    string para = String.Format(json, type, lastSyncTime);
            return TIF.GetModelListByInterface(json);
        }


        /// <summary>
        /// 导入WMS-Excel单据
        /// </summary>
        /// <param name="stockType">类型 10：入库 20:出库 </param>
        /// <param name="ExcelJson">Excel数据JSON</param>
        /// <param name="syncType">同步类型：ERP</param>
        /// <param name="errMsg">返回值</param>
        /// <returns></returns>
        public bool GetExcelmport(int stockType, string ExcelJson, string syncType, ref string errMsg)
        {

            string dataJson = String.Empty;
            bool result = GetDataJson(ExcelJson, ref dataJson, ref errMsg);
            if (result && !String.IsNullOrEmpty(dataJson.TrimStart('[').TrimEnd(']')))
            {
                ParamaterFiled_DB db = new ParamaterFiled_DB();
                int type = db.GetStockType(stockType);

                List<ParamaterField_Model> pmList = new List<ParamaterField_Model>();
                result = GetPMList(type, ref pmList, ref errMsg, -1, "", "");
                if (result)
                {
                    result = db.ComparerListAndCreateSQL(pmList, stockType, dataJson, syncType, ref errMsg);
                    if (!result) return result;
                }
            }

            return result;
        }


        /// <summary>
        /// 同步EXCEL数据
        /// </summary>
        /// <param name="stockType">类型 10：入库 20:出库</param>
        /// <param name="syncVouType">Excel单据类型</param>
        /// <param name="ds">Excel数据</param>
        /// <param name="dataJson">返回json数据</param>
        /// <param name="errMsg">返回错误信息</param>
        /// <returns></returns>
        private bool GetExcelRecord(int stockType, DataSet ds, ref string errMsg, string syncType,int wmsVourcherType)
        {
            if (wmsVourcherType == 10000)
            {
                string dataJson = JsonConvert.SerializeObject(ds);
                dataJson = dataJson.Substring(dataJson.IndexOf("["));
                dataJson = dataJson.Substring(0, dataJson.Length - 1);
                    db.insertStock(dataJson, ref errMsg);
                return true;
            }

            int type = db.GetStockType(stockType);
            List<ParamaterField_Model> pmList = new List<ParamaterField_Model>();
            bool result = GetPMList(type, ref pmList, ref errMsg, wmsVourcherType,"","");
            if (result)
            {
                string dataJson = JsonConvert.SerializeObject(ds);
                dataJson = dataJson.Substring(dataJson.IndexOf("["));
                dataJson = dataJson.Substring(0, dataJson.Length - 1);
                if (pmList.Count > 0 && !String.IsNullOrEmpty(dataJson))
                {
                    result = db.CreateSQLByExcel(pmList, dataJson, wmsVourcherType, ref errMsg, syncType);
                }
            }
            return result;
        }

        protected override bool CheckModelBeforeSave(ParamaterField_Model model, ref string strError)
        {
            throw new NotImplementedException();
        }

        protected override ParamaterField_Model GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

        protected override string GetModelChineseName()
        {
            return "参数配置";
        }
    }
}
