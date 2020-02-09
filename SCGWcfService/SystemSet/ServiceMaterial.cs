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
        #region 自动生成WCF调用T_Material_Func代码

        public bool SaveT_Material(UserInfo user, ref T_MaterialInfo t_material, ref string strError)
        {
            T_Material_Func tfunc = new T_Material_Func();
            return tfunc.SaveModelBySqlToDB(user, ref t_material, ref strError);
        }


        public bool DeleteT_MaterialByModel(UserInfo user, T_MaterialInfo model, ref string strError)
        {
            T_Material_Func tfunc = new T_Material_Func();
            return tfunc.DeleteModelByModelSql(user, model, ref strError);
        }




        public bool GetT_MaterialByID(ref T_MaterialInfo model, ref string strError)
        {
            T_Material_Func tfunc = new T_Material_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_MaterialListByPage(ref List<T_MaterialInfo> modelList, UserInfo user, T_MaterialInfo t_material, ref DividPage page, ref string strError)
        {
            T_Material_Func tfunc = new T_Material_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_material, ref page, ref strError);
        }


        public bool GetAllT_MaterialByHeaderID(ref List<T_MaterialInfo> modelList, int headerID, ref string strError)
        {
            T_Material_Func tfunc = new T_Material_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_MaterialStatus(UserInfo user, ref T_MaterialInfo t_material, int NewStatus, ref string strError)
        {
            T_Material_Func tfunc = new T_Material_Func();
            return tfunc.UpdateModelStatus(user, ref t_material, NewStatus, ref strError);
        }

        public string GetMaterialModelADF(string MaterialNo)
        {
            T_Material_Func tfunc = new T_Material_Func();
            return tfunc.GetMaterialModelBySqlADF(MaterialNo);
        }

        #endregion
    }
}