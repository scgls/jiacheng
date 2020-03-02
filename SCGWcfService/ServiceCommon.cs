using BILBasic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        public List<ComboBoxItem> GetComboBoxItem(string strSql)
        {
            return Common_Func.GetComboBoxItem(strSql);
        }

        public bool GetComboBoxItemByKey(string key, ref List<ComboBoxItem> comboxBoxItemList, ref string strError)
        {
            return Common_Func.GetComboBoxItem(key, ref comboxBoxItemList, ref strError);
        }

        public bool GetComboBoxItemByKeyExt(string key, ref List<ComboBoxItemExt> comboxBoxItemList, ref string strError)
        {
            return Common_Func.GetComboBoxItemExt(key, ref comboxBoxItemList, ref strError);
        }

        public bool GetComboBoxItemExtByCym(string key, ref List<ComboBoxItemExt> comboxBoxItemList, ref string strError)
        {
            return Common_Func.GetComboBoxItemExtByCym(key, ref comboxBoxItemList, ref strError);
        }

        public List<ComboBoxItemExt> GetComboBoxItemExt(string strSql)
        {
            return Common_Func.GetComboBoxItemExt(strSql);
        }
    }
}