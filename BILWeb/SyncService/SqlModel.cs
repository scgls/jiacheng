using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.SyncService
{
    public class SqlModel
    {
        public static string InsertTitleSql = "set identity_insert {0} ON;IF NOT EXISTS (SELECT id FROM {0} WHERE {1} 1=1) INSERT INTO {0} ({2},ID,VOUCHERNO,VOUCHERTYPE) SELECT {3},'{4}','{5}','{6}';set identity_insert {0} off;";

        public static string InsertDetailSql = "IF NOT EXISTS (SELECT id FROM {0} WHERE {1} 1=1) INSERT INTO {0} ({2},HEADERID,VOUCHERNO) SELECT {3},'{4}','{5}'";

        public static string UpdateSql = "update {0} set {1} where {2} 1=1";

        public static string checkSQL = "select ID from {0} Where {1}  1=1";

        public static string DeleteSql= "DELETE from {0} where ID in ({1}) ";

        public static string GetmaterialIdsql = "select id from T_MATERIAL where {0}   1=1";

        public static string GetWmsVoucherNo = "select VoucherNo from {0} Where ID={1}";

        public static string GetWhareHouseID = "select WAREHOUSEID from t_customeraddress Where  ISNULL(isdefault,0) = 1 and CUSTOMERNO='{0}'";

        public static string GetWhareHouseIDByNo = "SELECT ID FROM T_WAREHOUSE WHERE WAREHOUSENO='{0}'";
        
        public static string GetWhareHouseNo = "select WAREHOUSENO from t_customeraddress Where ISNULL(isdefault,0) = 1 and CUSTOMERNO='{0}'";

        public static string GetHeadids = "select ID from {0} where headerid={1}";




        public static string InsertSAPTitleSql = "set identity_insert {0} ON;IF NOT EXISTS (SELECT id FROM {0} WHERE {1}) INSERT INTO {0} ({2},ID,VOUCHERNO,VOUCHERTYPE) SELECT {3},'{4}','{5}','{6}';set identity_insert {0} off;";

        public static string InsertSAPDetailSql = "IF NOT EXISTS (SELECT id FROM {0} WHERE {1}) INSERT INTO {0} ({2},HEADERID,VOUCHERNO,SUBIARRSID) SELECT {3},'{4}','{5}','{6}'";

        public static string InsertU8DetailSql = "IF NOT EXISTS (SELECT id FROM {0} WHERE {1}) INSERT INTO {0} ({2},HEADERID,VOUCHERNO) SELECT {3},'{4}','{5}'";

        public static string GetsAPmaterialIdsql = "select id from T_MATERIAL where MATERIALNO='{0}'";

        public static string UpdateSAPSql = "update {0} set {1} where {2}";

        public static string SelectDetailCount = "Select count(ID) as num,VOUCHERNO from {0}  where ErpvoucherNo='{1}' group BY VOUCHERNO";

    }
}
