using BILWeb.SyncService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.SyncService.SqlSugar
{
    public class SyncTimeManager:DbContext<SyncTime_Model>
    {
        private static SyncTimeManager instance = null;

        public SyncTimeManager()
        {

        }

        public static SyncTimeManager GetInstance()
        {
            //if (instance == null)
            {
                instance = new SyncTimeManager();
            }
            return instance;
        }

        public SyncTime_Model GetLastSyncTime(string wmsType)
        {
            var synctime = Db.Queryable<SyncTime_Model>().Where(it => it.WmsType == wmsType).ToList() ;
           return synctime.Count()==0?null:synctime[0];
        }

        public void InsertOrUpdateSyncTime(SyncTime_Model syncTime)
        {
            if (syncTime.ID == 0)
            {
                Insert(syncTime);
            }else
            {
                Update(syncTime);
            }
        }
    }
}
