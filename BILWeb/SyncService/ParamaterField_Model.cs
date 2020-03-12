using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.SyncService
{
    [SugarTable("V_PARAMETERTABLE")]
    public class ParamaterField_Model 
    {

        /// <summary>
        /// 1-入库 2-出库....
        /// </summary>
        public int InStockType { get; set; }


        /// <summary>
        /// erp单据类型
        /// </summary>
        public string ErpVouType { get; set; }//ErpVourcherType

        /// <summary>
        /// erp单据类型
        /// </summary>
        public string ErpVourcherType { get; set; }//ErpVourcherType

        /// <summary>
        /// WMS单据类型
        /// </summary>
        public int VoucherType { get; set; }

        /// <summary>
        /// WMS表头名称
        /// </summary>
        public string WMSTableNameH { get; set; }

        /// <summary>
        /// WMS表体名称
        /// </summary>
        public string WMSTableNameD { get; set; }

        /// <summary>
        /// Excel抬头语言  E：英文  C：中文
        /// </summary>
        public string ExcelTitleLangrage { get; set; }

        /// <summary>
        /// WMS表头  ID 字段对应seq值
        /// </summary>
        public string HEADID { get; set; }

        /// <summary>
        /// WMS表体  ID 字段对应seq值
        /// </summary>
        public string DETAILID { get; set; }

        /// <summary>
        /// 表头WMS单据号生成规则
        /// </summary>
        public string VOUCHERNO{ get; set; }

        /// <summary>
        /// 表头主键
        /// </summary>
        public string HEADKEYS { get; set; }

        /// <summary>
        /// 表体主键
        /// </summary>
        public string DETAILKEYS { get; set; }

        /// <summary>
        /// 查询MaterialNOID主键
        /// </summary>
        public string MATERIALNOKEYS { get; set; }

        /// <summary>
        /// 区分是表头字段还是表体字段  H:表头  D:表体
        /// </summary>
        public string FieldHD { get; set; }

        /// <summary>
        /// EXCEL英文名称
        /// </summary>
        public string XlsNameEN { get; set; }

        /// <summary>
        /// XLSNAMECN
        /// </summary>
        public string XlsNameCN { get; set; }

        /// <summary>
        /// WMS字段
        /// </summary>
        public string WMSField { get; set; }

        /// <summary>
        /// ERP字段
        /// </summary>
        public string ERPField { get; set; }

        /// <summary>
        /// DEFAULTVALUE
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// DefaultType 0:默认  1:时间
        /// </summary>
        public int DefaultType{ get; set; }

    }

    public class deleteModel
    {
        public string ERPVOUCHERNO { get; set; }

        public List<RowDelModel> ROW { get; set; }
    }

    public class RowDelModel
    {
        public string ROWNO { get; set; }
        public List<string> RowDel { get; set; }
    }
}
