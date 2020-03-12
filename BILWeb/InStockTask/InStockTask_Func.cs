using BILBasic.Basing.Factory;
using BILBasic.JSONUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.InStockTask
{
    public partial class T_InStockTask_Func : TBase_Func<T_InStockTask_DB, T_InStockTaskInfo>, IInStockTaskService
    {
   
        protected override bool CheckModelBeforeSave(T_InStockTaskInfo model, ref string strError)
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
            return "入库任务总览";
        }


        protected override T_InStockTaskInfo GetModelByJson(string ModelJson)
        {
            return JSONHelper.JsonToObject<T_InStockTaskInfo>(ModelJson);
        }

        public bool GetReceiveTaskNoBySerialNo(string SerialNo,ref string TaskNo, ref string strError) 
        {
            try
            {
                T_InStockTask_DB _db = new T_InStockTask_DB();
                TaskNo = _db.GetReceiveTaskNoBySerialNo(SerialNo, ref strError);                
                return true;
            }
            catch (Exception ex) 
            {
                strError = ex.Message;
                return false;

            }
        }
   
     
 }
}
