using BILBasic.User;
namespace BILWeb.Warehouse
{
    public interface IWarehouseService : IBaseService<T_WareHouseInfo>
    {
         bool DeleteModelByModel(UserModel user, T_WareHouseInfo model, ref string strError);
    }
}