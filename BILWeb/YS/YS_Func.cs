using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.JSONUtil;

namespace BILWeb.YS
{
    public partial class T_YS_Func : TBase_Func<T_YS_DB, T_YS>, IYSService
    {
   
        protected override bool CheckModelBeforeSave(T_YS model, ref string strError)
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
            return "预留释放";
        }
        

        protected override T_YS GetModelByJson(string strJson)
        {
            string errorMsg = string.Empty;
            T_YSDetailInfo model = JSONHelper.JsonToObject<T_YSDetailInfo>(strJson);

            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
                PFunc.Sync(10, string.Empty, model.ErpVoucherNo, -1, ref errorMsg, "ERP", -1, null);

            }
            return JSONHelper.JsonToObject<T_YS>(strJson);
        }

   
     }
 }

