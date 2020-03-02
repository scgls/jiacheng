using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.Pallet;
using System.ServiceModel;

namespace SCGWcfService.Pallet
{
    public partial interface IService
    {
        [OperationContract]
        string SaveT_PalletDetailADF(string UserJson, string ModelJson);
    }
}