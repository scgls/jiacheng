using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.Pallet;

namespace SCGWcfService.Pallet
{
    public partial class ServiceWMS : IService
    {
        public string SaveT_PalletDetailADF(string UserJson, string ModelJson)
        {
            T_PalletDetail_Func tfunc = new T_PalletDetail_Func();
            return tfunc.SaveModelListSqlToDBADF(UserJson, ModelJson);
        }

    }
}