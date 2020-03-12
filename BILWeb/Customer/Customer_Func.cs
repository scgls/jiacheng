using BILBasic.Basing.Factory;
using BILBasic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Customer
{
    public partial class T_Customer_Func : TBase_Func<T_Customer_DB, T_CustomerInfo>, ICustomerService
    {

        protected override bool CheckModelBeforeSave(T_CustomerInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.CustomerNo))
            {
                strError = "客户编号不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.CustomerName))
            {
                strError = "客户名称不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.CustomerAbridge))
            {
                strError = "客户简称不能为空！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.EnglishName))
            {
                strError = "英文名不能为空!";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.ContactPerson))
            {
                strError = "联系人不能为空！";
                return false;
            }
           
            return true;
        }

        protected override string GetModelChineseName()
        {
            return "客户";
        }

        protected override T_CustomerInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

    }
}
