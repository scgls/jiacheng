using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.SyncService
{
    class ParamaterTable_Model : BILBasic.Basing.Factory.Base_Model
    {

        

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
        public char ExcelTitleLangrage { get; set; }
    }
}
