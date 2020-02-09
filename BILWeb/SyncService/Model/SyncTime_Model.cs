using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.SyncService.Model
{
    [SugarTable("T_SYNCTIME")]
    public class SyncTime_Model
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }

        public string WmsType { get; set; }

        public string SyncServerTime { get; set; }
    }
}
