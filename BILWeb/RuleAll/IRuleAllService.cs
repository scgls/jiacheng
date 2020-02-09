using BILBasic.User;
using BILWeb.RuleAll;

namespace BILWeb.RuleAll
{
    public interface IRuleAllService : IBaseService<T_RuleAllInfo>
    {
        bool UpadteModelByModelSql(UserModel userInfo, T_RuleAllInfo t_RuleAllInfo, ref string strError);
    }
}