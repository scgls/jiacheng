using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.ContactCompany
{
    public partial class T_ContactCompany_Func : TBase_Func<T_ContactCompany_DB, T_ContactCompanyInfo>
     {
   
        protected override bool CheckModelBeforeSave(T_ContactCompanyInfo model, ref string strError)
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
            return "往来单位";
        }
        
        protected override T_ContactCompanyInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }
   
     
 }
}
