using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_Material_Batch_Func代码

        public bool SaveT_Material_Batch(UserInfo user, ref T_Material_BatchInfo t_material_batch, ref string strError)
        {
            T_Material_Batch_Func tfunc = new T_Material_Batch_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_material_batch, ref strError);
        }


        public bool DeleteT_Material_BatchByModel(UserInfo user, T_Material_BatchInfo model, ref string strError)
        {
            T_Material_Batch_Func tfunc = new T_Material_Batch_Func();
            return tfunc.DeleteModelByModelSql(user, model, ref strError);
        }




        public bool GetT_Material_BatchByID(ref T_Material_BatchInfo model, ref string strError)
        {
            T_Material_Batch_Func tfunc = new T_Material_Batch_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_Material_BatchListByPage(ref List<T_Material_BatchInfo> modelList, UserInfo user, T_Material_BatchInfo t_material_batch, ref DividPage page, ref string strError)
        {
            T_Material_Batch_Func tfunc = new T_Material_Batch_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_material_batch, ref page, ref strError);
        }


        public bool GetAllT_Material_BatchByHeaderID(ref List<T_Material_BatchInfo> modelList, int headerID, ref string strError)
        {
            T_Material_Batch_Func tfunc = new T_Material_Batch_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_Material_BatchStatus(UserInfo user, ref T_Material_BatchInfo t_material_batch, int NewStatus, ref string strError)
        {
            T_Material_Batch_Func tfunc = new T_Material_Batch_Func();
            return tfunc.UpdateModelStatus(user, ref t_material_batch, NewStatus, ref strError);
        }


        #endregion
    }
}