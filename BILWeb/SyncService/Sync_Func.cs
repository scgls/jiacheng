using BILBasic.Basing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BILBasic.Basing.Factory;

namespace BILWeb.SyncService
{
    public class Sync_Func : TBase_Func<ParamaterFiled_DB, EmptyModel>
    {


        protected override bool CheckModelBeforeSave(EmptyModel model, ref string strError)
        {
            throw new NotImplementedException();
        }

        protected override string GetModelChineseName()
        {
            throw new NotImplementedException();
        }

        public bool SyncSAP(int StockType, string LastSyncTime, string ErpVoucherNo, int wmsVourcherType,string InJson, ref string ErrMsg)
        {
            return SyncErp.SyncSAPJsonFromErp(StockType, LastSyncTime, ErpVoucherNo, wmsVourcherType, InJson, ref ErrMsg);
        }
    }
}
