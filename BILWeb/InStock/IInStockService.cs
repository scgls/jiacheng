using System.Collections.Generic;

namespace BILWeb.InStock
{
    public interface IInStockService : IBaseService<T_InStockInfo>
    {
        //bool GetModelListByHeaderID(ref List<T_InStockDetailInfo> modelList, int headerID, ref string strError);
    }
}