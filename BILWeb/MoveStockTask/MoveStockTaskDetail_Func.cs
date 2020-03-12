using BILBasic.Basing.Factory;
using BILBasic.JSONUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.MoveStockTask
{
    public class MoveStockTaskDetail_Func : TBase_Func<MoveStockTaskDetail_DB, T_MoveTaskDetailInfo>
    {

        protected override bool CheckModelBeforeSave(T_MoveTaskDetailInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            return true;
        }
        protected override bool CheckModelBeforeSave(List<T_MoveTaskDetailInfo> model, ref string strError)
        {
            if (model == null || model.Count == 0)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }
            object value = db.GetScalarBySql("select count(*) from t_task where tasktype =3 and (status=1 or status=2 ) and WAREHOUSEID=" + model[0].ToErpWarehouse + "");//ToErpWarehouse传登录人的仓库ID
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

        protected override T_MoveTaskDetailInfo GetModelByJson(string strJson)
        {
            return JSONHelper.JsonToObject<T_MoveTaskDetailInfo>(strJson);
        }



        protected override List<T_MoveTaskDetailInfo> GetModelListByJson(string UserJson, string ModelListJson)
        {
            return JSONHelper.JsonToObject<List<T_MoveTaskDetailInfo>>(ModelListJson);
        }

    }
}
