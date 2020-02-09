using BILBasic.Basing.Factory;
using BILBasic.JSONUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.ProductGY
{
    public partial class Mes_ProductGYLine_Func : TBase_Func<Mes_ProductGYLine_DB, Mes_ProductGYLineInfo>
    {

        protected override bool CheckModelBeforeSave(Mes_ProductGYLineInfo model, ref string strError)
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
            return "工序";
        }

        protected override Mes_ProductGYLineInfo GetModelByJson(string strJson)
        {
            return JSONHelper.JsonToObject<Mes_ProductGYLineInfo>(strJson);
        }


        public bool GetProductGYLineDataByID(ref Mes_ProductGYLineInfo model, ref string ErrMsg)
        {
            Mes_ProductGYLine_DB mdb = new Mes_ProductGYLine_DB();
            return mdb.GetProductGYLineDataByID(ref model, ref ErrMsg);
        }

        public bool SaveProductGYLineData(string userNo, Mes_ProductGYLineInfo entity, ref string ErrMsg)
        {
            Mes_ProductGYLine_DB mdb = new Mes_ProductGYLine_DB();
            return mdb.SaveProductGYLineData(userNo, entity, ref ErrMsg);
        }

        public int GetMaxGYbbID(string cptype)
        {
            Mes_ProductGYLine_DB mdb = new Mes_ProductGYLine_DB();
            return mdb.GetMaxGYbbID(cptype);
        }

        public bool DeleteProductGYLineInfo(string gxLineID, int isdel, ref string ErrMsg)
        {
            Mes_ProductGYLine_DB mdb = new Mes_ProductGYLine_DB();
            return mdb.DeleteProductGYLineInfo(gxLineID, isdel, ref ErrMsg);
        }

        public bool GetProductGYLineListDataBygxLineID(string gxLineID, ref Mes_ProductGYLineInfo entityGYLine, ref List<Mes_GYLine_GYInfo> lstDisGY, ref string ErrMsg)
        {
            Mes_ProductGYLine_DB mdb = new Mes_ProductGYLine_DB();
            return mdb.GetProductGYLineListDataBygxLineID(gxLineID, ref entityGYLine, ref lstDisGY, ref ErrMsg);
        }

        #region 正式工艺路线
        public int GetMaxGYbbID_C(string cpCode, string machineType)
        {
            Mes_ProductGYLine_DB mdb = new Mes_ProductGYLine_DB();
            return mdb.GetMaxGYbbID_C(cpCode, machineType);
        }

        public bool SaveProductGYLineData_C(string userNo, Mes_ProductGYLineInfo entity, ref string ErrMsg)
        {
            Mes_ProductGYLine_DB mdb = new Mes_ProductGYLine_DB();
            return mdb.SaveProductGYLineData_C(userNo, entity, ref ErrMsg);
        }

        public bool GetAllProductGYLineList_C(Mes_ProductGYLineInfo model, ref List<Mes_ProductGYLineInfo> lst, ref string ErrMsg)
        {
            Mes_ProductGYLine_DB mdb = new Mes_ProductGYLine_DB();
            return mdb.GetAllProductGYLineList_C(model, ref lst, ref ErrMsg);
        }

        public bool GetProductGYLineListDataBygxLineID_C(string gxLineID, ref Mes_ProductGYLineInfo entityGYLine, ref List<Mes_GYLine_GYInfo> lstDisGY, ref string ErrMsg)
        {
            Mes_ProductGYLine_DB mdb = new Mes_ProductGYLine_DB();
            return mdb.GetProductGYLineListDataBygxLineID_C(gxLineID, ref entityGYLine, ref lstDisGY, ref ErrMsg);
        }

        public bool DeleteProductGYLineInfo_C(string gxLineID, int isdel, ref string ErrMsg)
        {
            Mes_ProductGYLine_DB mdb = new Mes_ProductGYLine_DB();
            return mdb.DeleteProductGYLineInfo_C(gxLineID, isdel, ref ErrMsg);
        }

        public bool UpdateProductGYLineIsNewTo2BygxLineID(string gxLineID, ref string ErrMsg)
        {
            Mes_ProductGYLine_DB mdb = new Mes_ProductGYLine_DB();
            return mdb.UpdateProductGYLineIsNewTo2BygxLineID(gxLineID, ref ErrMsg);
        }

        public bool UpdateProductGYLineStatus_C(string gxLineID, int isdel, ref string ErrMsg)
        {
            Mes_ProductGYLine_DB mdb = new Mes_ProductGYLine_DB();
            return mdb.UpdateProductGYLineStatus_C(gxLineID, isdel, ref ErrMsg);
        }

        public bool UpdateProductGYLineStatusNew_C(string gxLineID, string ApprovalSOP, string ApprovalProduct, string ApprovalQC, ref string ErrMsg)
        {
            Mes_ProductGYLine_DB mdb = new Mes_ProductGYLine_DB();
            return mdb.UpdateProductGYLineStatusNew_C(gxLineID, ApprovalSOP, ApprovalProduct, ApprovalQC, ref ErrMsg);
        }

        public bool UpdateProductGYLineStatus1AndReturn_C(string gxLineID, int isdel, string returnRemark, ref string ErrMsg)
        {
            Mes_ProductGYLine_DB mdb = new Mes_ProductGYLine_DB();
            return mdb.UpdateProductGYLineStatus1AndReturn_C(gxLineID, isdel, returnRemark, ref ErrMsg);
        }

        #endregion
    }
}
