using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BILWeb.Quality;
using BILBasic.Common;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        #region 自动生成WCF调用T_QualityDetail_Func代码

        public bool SaveT_QualityDetail(UserInfo user, ref T_QualityDetailInfo t_qualitydetail, ref string strError)
        {
            T_QualityDetail_Func tfunc = new T_QualityDetail_Func();
            return tfunc.SaveModelToDB(user, ref t_qualitydetail, ref strError);
        }


        public bool DeleteT_QualityDetailByModel(UserInfo user, T_QualityDetailInfo model, ref string strError)
        {
            T_QualityDetail_Func tfunc = new T_QualityDetail_Func();
            return tfunc.DeleteModelByModel(user, model, ref strError);
        }




        public bool GetT_QualityDetailByID(ref T_QualityDetailInfo model, ref string strError)
        {
            T_QualityDetail_Func tfunc = new T_QualityDetail_Func();
            return tfunc.GetModelByID(ref model, ref strError);
        }


        public bool GetT_QualityDetailListByPage(ref List<T_QualityDetailInfo> modelList, UserInfo user, T_QualityDetailInfo t_qualitydetail, ref DividPage page, ref string strError)
        {
            T_QualityDetail_Func tfunc = new T_QualityDetail_Func();
            return tfunc.GetModelListByPage(ref modelList, user, t_qualitydetail, ref page, ref strError);
        }


        public bool GetAllT_QualityDetailByHeaderID(ref List<T_QualityDetailInfo> modelList, int headerID, ref string strError)
        {
            T_QualityDetail_Func tfunc = new T_QualityDetail_Func();
            return tfunc.GetModelListByHeaderID(ref modelList, headerID, ref strError);
        }


        public bool UpdateT_QualityDetailStatus(UserInfo user, ref T_QualityDetailInfo t_qualitydetail, int NewStatus, ref string strError)
        {
            T_QualityDetail_Func tfunc = new T_QualityDetail_Func();
            return tfunc.UpdateModelStatus(user, ref t_qualitydetail, NewStatus, ref strError);
        }

        /// <summary>
        /// 更新库存检验结果
        /// </summary>
        /// <param name="ErpVoucherNo"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool UpdateStockByQuality(string ErpVoucherNo, ref string strError)
        {
            T_QualityDetail_Func tfunc = new T_QualityDetail_Func();
            return tfunc.GetQualityUpadteStock(ErpVoucherNo, ref strError);

        }



        #endregion
    }
}