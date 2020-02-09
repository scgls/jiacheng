using BILBasic.User;
namespace BILWeb.House
{
    public interface IHouseService : IBaseService<T_HouseInfo>
    {
        bool DeleteModelByModel(UserModel user, T_HouseInfo model, ref string strError);
    }
}