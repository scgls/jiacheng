using BILWeb.SyncService.Model;
using BILWeb.SyncService.SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BILWeb.SyncService
{
    public class Sync_Erp_Func
    {

        /// <summary>
        /// 获取SAP服务器时间
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        internal static string GetSAPServerTime(string para)
        {
            BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
            string json = "{\"VoucherType\":\"" + para + "\"}";
            return TIF.GetModelListByInterface(json);
           // return "2018-01-01";
        }


        internal static string GetSAPJson(int StockType,string erpVoucherNo, Sync_Model Type,ref SyncTime_Model syncTime)
        {

                syncTime = SyncTimeManager.GetInstance().GetLastSyncTime(Type.WmsType);
                if (syncTime==null)
                {
                    syncTime = new SyncTime_Model();
                    syncTime.SyncServerTime = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationManager.ConnectionStrings["SyncAddDate"].ConnectionString)).ToString("yyyyMMdd,HHmmss");
                    syncTime.WmsType = Type.WmsType;
                    syncTime.ID = 0;
                }
            
            BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
            string json = "{\"code\":\"" + erpVoucherNo + "\",\"VoucherType\":\"" + Type.WmsType.ToString() + "\",\"sync_time\":\"";
            json += syncTime.SyncServerTime;
            json += "\",\"erp_vourcher_type\":\"" + Type.ErpType + "\"}";
            LogNet.SyncInfo(json);
            return //"{\"result\":1,\"resultValue\":\"\",\"data\":[{\"head\":{\"cwhcode\":null,\"cwhname\":null,\"iquantity\":null,\"cinvcode\":\"710431000180890\",\"cinvname\":\"LED消防应急标志灯线下款06  双面安全出口\",\"cinvstd\":\"NEP-XBZ06-021\",\"cbarcode\":\"2222220002565\",\"cinvdefine6\":\"883675\",\"cumName\":\"只\"}},{\"head\":{\"cwhcode\":null,\"cwhname\":null,\"iquantity\":null,\"cinvcode\":\"710431000180906\",\"cinvname\":\"LED消防应急标志灯线下款08   嵌入式左向\",\"cinvstd\":\"NEP-XBZ08-012\",\"cbarcode\":\"2222220002581\",\"cinvdefine6\":\"883691\",\"cumName\":\"只\"}},{\"head\":{\"cwhcode\":null,\"cwhname\":null,\"iquantity\":null,\"cinvcode\":\"710431001120306\",\"cinvname\":\"吸顶吊灯24 铜本色 玻璃灯罩 标配E27螺纹灯头，负载单头最大功率40W，整灯240W,出货不配光源\",\"cinvstd\":\"NEP-XDD24240X4\",\"cbarcode\":\"6941461102961\",\"cinvdefine6\":\"209479\",\"cumName\":\"只\"}},{\"head\":{\"cwhcode\":null,\"cwhname\":null,\"iquantity\":null,\"cinvcode\":\"710431001120322\",\"cinvname\":\"吸顶灯218 108W  双色 白色 方形\",\"cinvstd\":\"NEP-XD21810881-F\",\"cbarcode\":\"6941461103272\",\"cinvdefine6\":\"209510\",\"cumName\":\"只\"}},{\"head\":{\"cwhcode\":null,\"cwhname\":null,\"iquantity\":null,\"cinvcode\":\"710431001730064\",\"cinvname\":\"吸顶灯124款 32W 双色温  粉色面罩带厚亚克力\",\"cinvstd\":\"NEP-XD12403284-Y\",\"cbarcode\":\"6941461106815\",\"cinvdefine6\":\"209864\",\"cumName\":\"只\"}},{\"head\":{\"cwhcode\":null,\"cwhname\":null,\"iquantity\":null,\"cinvcode\":\"710431002170341\",\"cinvname\":\"线下新型筒灯22 3.5W 银色（铝面环） 6000K\",\"cinvstd\":\"NEP-TD2200363A\",\"cbarcode\":\"6941461103265\",\"cinvdefine6\":\"209509\",\"cumName\":\"只\"}},{\"head\":{\"cwhcode\":null,\"cwhname\":null,\"iquantity\":null,\"cinvcode\":\"710431000260017\",\"cinvname\":\"LED筒灯27 18W 白色 3000K\",\"cinvstd\":\"NEP-TD2701831G\",\"cbarcode\":\"2222220002644\",\"cinvdefine6\":\"883754\",\"cumName\":\"只\"}}]}";
                //"{\"result\":1,\"resultValue\":\"\",\"data\":[{\"head\":{\"body\":[{\"cwhcode1\":\"013\",\"cbmemo\":\"\",\"boxQty\":10,\"rQty\":0,\"packQty\":1,\"cwhname\":\"\",\"cwhcode\":\"013\",\"cinvcode\":\"710431001120181\",\"cinvname\":\"吸顶灯188款，白色灯罩，12头组合，192W，3000K+5700K\",\"cinvstd\":\"NEP-XD18819281\",\"iquantity\":10.00,\"foutquantity\":10.0000000000,\"Qty\":0.0000000000,\"Qty1\":0.0000000000,\"itaxunitprice\":0.0,\"itaxrate\":0.0,\"invbatch\":null,\"irowno\":1,\"iunitprice\":0.0,\"itax\":0.0,\"cdlcode\":\"FH1811230008\",\"ccuscode\":\"10097701\",\"ccusname\":null,\"idlsid\":1002675166},{\"cwhcode1\":\"013\",\"cbmemo\":\"\",\"boxQty\":10,\"rQty\":0,\"packQty\":1,\"cwhname\":\"\",\"cwhcode\":\"013\",\"cinvcode\":\"710431001120181\",\"cinvname\":\"吸顶灯188款，白色灯罩，12头组合，192W，3000K+5700K\",\"cinvstd\":\"NEP-XD18819281\",\"iquantity\":10.00,\"foutquantity\":10.0000000000,\"Qty\":0.0000000000,\"Qty1\":0.0000000000,\"itaxunitprice\":0.0,\"itaxrate\":0.0,\"invbatch\":null,\"irowno\":2,\"iunitprice\":0.0,\"itax\":0.0,\"cdlcode\":\"FH1811230008\",\"ccuscode\":\"10097701\",\"ccusname\":null,\"idlsid\":1002675167}],\"ccusphone\":\"0551-64369575\",\"ccusperson\":\"王世珍\",\"DLID\":1000385412,\"cDLCode\":\"FH1811230008\",\"dDate\":\"2018-11-23T00:00:00\",\"cRdCode\":null,\"cDepCode\":\"030901\",\"cDepName\":\"营业\",\"cPersonCode\":null,\"cSOCode\":null,\"cCusCode\":\"10097701\",\"cCusAddress\":null,\"cShipAddress\":\"0551-64369575 15605699888 安徽省合肥市瑶海区瑶海工业园灵石路中段安徽百事兴电气有限公司 正泰仓库\",\"cexch_name\":\"\",\"iExchRate\":0.0,\"iTaxRate\":0.0,\"cMemo\":\"照明\",\"cDefine1\":null,\"cDefine2\":\"\",\"cDefine3\":\"\",\"cDefine4\":null,\"cDefine5\":null,\"cDefine6\":null,\"cDefine7\":null,\"cDefine8\":null,\"cDefine9\":\"\",\"cDefine10\":\"\",\"cDefine11\":\"\",\"cDefine12\":null,\"cDefine13\":\"\",\"cDefine14\":null,\"cDefine15\":null,\"cDefine16\":null,\"cMaker\":\"demo\",\"cCusName\":\"王世珍-安徽百事兴电气有限公司\",\"cBusType\":\"普通销售\"}}]}";
            TIF.GetModelListByInterface(json);
        }
    }
}
