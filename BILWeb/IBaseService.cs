using BILBasic.Common;
using BILBasic.User;
using System.Collections.Generic;

namespace BILWeb
{
    public interface IBaseService<T>
    {
        bool GetModelListByPage(ref List<T> gmodelList, UserModel userInfo, T model, ref DividPage page, ref string strMsg);

        bool GetModelByID(ref T model, ref string strMsg);

        bool SaveModelBySqlToDB(UserModel userInfo, ref T model, ref string strMsg);

        bool DeleteModelByModelSql(UserModel userInfo, T model, ref string strMsg);

        bool DeleteModelByID(UserModel userInfo, int ID, ref string strMsg);

        bool SaveModelToDB(UserModel userInfo, ref T model, ref string strMsg);

    }
}
