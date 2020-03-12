using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.SyncService
{
    [SugarTable("V_SYNC")]
    public class Sync_Model
    {
        public string MainSubject { get; set; }
        public string ErpType { get; set; }
        public string ErpName { get; set; }
        public string WmsType { get; set; }
        public string WmsName { get; set; }
        public int InStockType { get; set; }
        public string WmsTableH { get; set; }
        public string WmsHeadID { get; set; }
        public string WmsHeadKeys { get; set; }
        public string WmsTableD { get; set; }
        public string WmsDetailID { get; set; }
        public string WmsDetailKeys { get; set; }
        public string WmsVoucherNoRual { get; set; }
        public string WmsMaterialNoKeys { get; set; }
        public string WmsWarehouseKeys { get; set; }
        public string WmsCustomKeys { get; set; }

        /// <summary>
        /// 区分是表头字段还是表体字段  H:表头  D:表体
        /// </summary>
        public string FieldHD { get; set; }

        /// Wms字段
        /// </summary>
        public string WmsFieldName { get; set; }

        /// Wms字段
        /// </summary>
        public string WmsField { get; set; }

        /// <summary>
        /// ERP字段
        /// </summary>
        public string ErpField { get; set; }

        /// <summary>
        /// DEFAULTVALUE
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// DefaultType 0:默认  1:时间
        /// </summary>
        public int DefaultType { get; set; }

        /// <summary>
        /// 1：默认值 2：只用INSERT字段
        /// </summary>
        public int FUNCTIONTYPE { get; set; }

        /// <summary>
        /// 是否自动同步 1：自动 0：手动
        /// </summary>
        public int AutoSync { get; set; }
    }
}
