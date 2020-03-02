using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace SCGWcfService.SerialNo
{
    public partial interface IService
    {
        [OperationContract]
        string GetT_SerialNo(string SerialNo);

        [OperationContract]
        string GetT_SerialNoInStock(string SerialNo);
    }
}