using BILBasic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace SCGWcfService
{
    public partial interface IService
    {
        [OperationContract]
        List<ComboBoxItem> GetComboBoxItem(string strSql);

        [OperationContract]
        bool GetComboBoxItemByKey(string key, ref List<ComboBoxItem> comboxBoxItemList, ref string strError);

        [OperationContract]
        bool GetComboBoxItemByKeyExt(string key, ref List<ComboBoxItemExt> comboxBoxItemListExt, ref string strError);

        [OperationContract]
        bool GetComboBoxItemExtByCym(string key, ref List<ComboBoxItemExt> comboxBoxItemList, ref string strError);

        [OperationContract]
        List<ComboBoxItemExt> GetComboBoxItemExt(string strSql);
    }
}