using BILBasic.User;
using BILWeb.Boxing;

namespace BILWeb.Box
{
    public interface IBoxService : IBaseService<T_BoxInfo>
    {
        bool DeleteModelByModel(UserModel user, T_BoxInfo model, ref string strError);
    }
}