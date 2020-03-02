using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Material
{
    public partial class T_Material_Batch_Func : TBase_Func<T_Material_Batch_DB, T_Material_BatchInfo>
     {
   
        protected override bool CheckModelBeforeSave(T_Material_BatchInfo model, ref string strError)
        {
            T_Material_Batch_DB mdb = new T_Material_Batch_DB();
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (mdb.CheckMaterialExist(model) > 0) 
            {
                strError = "批次号已经存在！";
                return false;
            }

            return true;
        }

        protected override string GetModelChineseName()
        {
            return "物料批次";
        }
        
        protected override T_Material_BatchInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }
   
     }
 
}
