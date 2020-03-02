using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.ContactCompany
{
    public partial class T_ContactCompany_Address_Func : TBase_Func<T_ContactCompany_Address_DB, T_ContactCompany_AddressInfo>
     {
   
        protected override bool CheckModelBeforeSave(T_ContactCompany_AddressInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }
            
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "往来单位地址";
        }
        
        protected override T_ContactCompany_AddressInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }
   
     }
 
}
