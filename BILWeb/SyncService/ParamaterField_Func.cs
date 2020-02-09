using BILBasic.Basing;
using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BILWeb.SyncService
{
    public class ParamaterField_Func : TBase_Func<ParamaterFiled_DB, EmptyModel>
    {

        /// <summary>
        ///  同步数据
        /// </summary>
        /// <param name="StockType">类型 10：入库 20:出库  99:基础资料</param>
        /// <param name="LastSyncTime">最后同步时间</param>
        /// <param name="ErpVoucherNo">ERP单号</param>
        /// <param name="wmsVourcherType">wms单据类型</param>
        /// <param name="ErrMsg">返回错误信息</param>
        /// <param name="syncType">同步数据来源 ERP或者 EXCEL</param>
        /// <param name="syncExcelVouType">Excel单据类型</param>
        /// <param name="ds">EXCEL单据数据</param>
        /// <returns>成功 true</returns>
        public bool Sync(int StockType, string LastSyncTime, string ErpVoucherNo, int wmsVourcherType, ref string ErrMsg, string syncType, int syncExcelVouType, DataSet ds)
        {
            return SyncErp.SyncJsonFromErp(StockType, LastSyncTime, ErpVoucherNo, wmsVourcherType, ref ErrMsg);
        }



        public bool GetExcelmport(int stockType, string ExcelJson, string syncType, ref string errMsg)
        {
            return false;
        }


        protected override bool CheckModelBeforeSave(EmptyModel model, ref string strError)
        {
            throw new NotImplementedException();
        }

        protected override string GetModelChineseName()
        {
            throw new NotImplementedException();
        }
    }
}
