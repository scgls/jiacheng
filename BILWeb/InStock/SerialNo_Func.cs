using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.JSONUtil;

namespace BILWeb.InStock
{
    public partial class T_SerialNo_Func : TBase_Func<T_SerialNo_DB, T_SerialNoInfo>
    {

        protected override bool CheckModelBeforeSave(T_SerialNoInfo model, ref string strError)
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
            return "序列号";
        }

        protected override T_SerialNoInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }


        public string CheckSerialNo(string SerialNo) 
        {
            BaseMessage_Model<T_SerialNoInfo> model = new BaseMessage_Model<T_SerialNoInfo>();
            try
            {
                bool bSucc = false;
                string strError = string.Empty;
                
                T_SerialNo_DB db = new T_SerialNo_DB();
                bSucc = db.CheckSerialNo(SerialNo, ref strError);

                if (bSucc == true)
                {
                    model.HeaderStatus = "S";
                }
                else
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                }
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_SerialNoInfo>>(model);
            }
            catch (Exception ex) 
            {
                model.HeaderStatus = "E";
                model.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_SerialNoInfo>>(model);
            }
        }


        public string CheckSerialNoInStock(string SerialNo)
        {
            BaseMessage_Model<T_SerialNoInfo> model = new BaseMessage_Model<T_SerialNoInfo>();
            try
            {
                bool bSucc = false;
                string strError = string.Empty;

                T_SerialNo_DB db = new T_SerialNo_DB();
                bSucc = db.CheckSerialNoInStock(SerialNo, ref strError);

                if (bSucc == true)
                {
                    model.HeaderStatus = "S";
                }
                else
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                }
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_SerialNoInfo>>(model);
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = ex.Message;
                return JSONHelper.ObjectToJson<BaseMessage_Model<T_SerialNoInfo>>(model);
            }


        }

    }
}
