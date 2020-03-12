using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILBasic.JSONUtil;

namespace BILWeb.Move
{

    public partial class T_MoveDetail_Func : TBase_Func<T_MoveDetail_DB, T_MoveDetailInfo>
    {

        protected override bool CheckModelBeforeSave(T_MoveDetailInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            return true;
        }
        protected override bool CheckModelBeforeSave(List<T_MoveDetailInfo> model, ref string strError)
        {
            if (model == null || model.Count == 0)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }
            object value = db.GetScalarBySql("select count(*) from t_task where tasktype =3 and (status=1 or status=2 ) and WAREHOUSEID=" + model[0].ToErpWarehouse + "");
            if (value.ToString() != "0")
            {
                strError = "该仓库补货任务已存在，请先完成或关闭后再生成！";
                return false;
            }

            return true;
        }

        protected override string GetModelChineseName()
        {
            return "移库单表体";
        }

        protected override T_MoveDetailInfo GetModelByJson(string strJson)
        {
            return JSONHelper.JsonToObject<T_MoveDetailInfo>(strJson);
        }



        protected override List<T_MoveDetailInfo> GetModelListByJson(string UserJson, string ModelListJson)
        {
            return JSONHelper.JsonToObject<List<T_MoveDetailInfo>>(ModelListJson);
        }

    }
}