using BILBasic.Common;
using BILWeb.Login.User;
using BILWeb.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_MATERIAL_Func代码
        [OperationContract]
        bool SaveT_Material(UserInfo user, ref T_MaterialInfo t_material, ref string strError);


        [OperationContract]
        bool DeleteT_MaterialByModel(UserInfo user, T_MaterialInfo model, ref string strError);

        [OperationContract]
        bool GetT_MaterialByID(ref T_MaterialInfo model, ref string strError);


        [OperationContract]
        bool GetT_MaterialListByPage(ref List<T_MaterialInfo> modelList, UserInfo user, T_MaterialInfo t_material, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_MaterialByHeaderID(ref List<T_MaterialInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_MaterialStatus(UserInfo user, ref T_MaterialInfo t_material, int NewStatus, ref string strError);

        [OperationContract]
        string GetMaterialModelADF(string MaterialNo);

        #endregion
    }
}