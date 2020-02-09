using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;

namespace BILWeb.DepInterface
{
    public partial class T_DepInterface_Func : TBase_Func<T_DepInterface_DB, T_DepInterfaceInfo>,IDepInterfaceService
    {
   
        protected override bool CheckModelBeforeSave(T_DepInterfaceInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (model.VoucherName == 0)
            {
                strError = "请先选择单据名称！";
                return false;
            }

            if (model.VoucherType == 0)
            {
                strError = "请先选择单据类型！";
                return false;
            }

            if (model.Function == 0)
            {
                strError = "请先选择功能！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.Route)) 
            {
                strError = "请先输入DLL文件路径！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.ClassName))
            {
                strError = "请先输入类名！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.DLLName))
            {
                strError = "请先输入DLL文件名称！";
                return false;
            }

            if (Common_Func.IsNullOrEmpty(model.FunctionName))
            {
                strError = "请先输入函数名称！";
                return false;
            }

            return true;
        }

        protected override string GetModelChineseName()
        {
            return "二次开发接口";
        }
        
        protected override T_DepInterfaceInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }
   
     }
 
}
