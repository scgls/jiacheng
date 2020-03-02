using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.InStock;

namespace SCGWcfService.SerialNo
{
    public partial class ServiceWMS : IService
    {
        public string GetT_SerialNo(string SerialNo) 
        {
            T_SerialNo_Func tfun = new T_SerialNo_Func();
            return tfun.CheckSerialNo(SerialNo);
        }

        public string GetT_SerialNoInStock(string SerialNo)
        {
            T_SerialNo_Func tfun = new T_SerialNo_Func();
            return tfun.CheckSerialNoInStock(SerialNo);
        }
    }
}