using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using BILWeb.Material;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_MATERIAL_BATCH_Func代码
        [OperationContract]
        bool SaveT_Material_Batch(UserInfo user, ref T_Material_BatchInfo t_material_batch, ref string strError);


        [OperationContract]
        bool DeleteT_Material_BatchByModel(UserInfo user, T_Material_BatchInfo model, ref string strError);

        [OperationContract]
        bool GetT_Material_BatchByID(ref T_Material_BatchInfo model, ref string strError);


        [OperationContract]
        bool GetT_Material_BatchListByPage(ref List<T_Material_BatchInfo> modelList, UserInfo user, T_Material_BatchInfo t_material_batch, ref DividPage page, ref string strError);


        [OperationContract]
        bool GetAllT_Material_BatchByHeaderID(ref List<T_Material_BatchInfo> modelList, int headerID, ref string strError);


        [OperationContract]
        bool UpdateT_Material_BatchStatus(UserInfo user, ref T_Material_BatchInfo t_material_batch, int NewStatus, ref string strError);



        #endregion
    }
}