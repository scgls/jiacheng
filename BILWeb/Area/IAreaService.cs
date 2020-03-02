using BILBasic.User;
namespace BILWeb.Area
{
    public interface IAreaService : IBaseService<T_AreaInfo>
    {
        bool DeleteModelByModel(UserModel user, T_AreaInfo model, ref string strError);
    }
}