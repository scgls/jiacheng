using BILBasic.Common;

using BILWeb.Customer;
using BILWeb.Login.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using BILWeb.BaseInfo;
using BILWeb.ProductGY;
using BILWeb.WorkCal;

namespace SCGWcfService
{
    public partial interface IService
    {
        #region 自动生成WCF接口方法T_CUSTOMER_Func代码
        [OperationContract]
        void GetCalendarList(int year, ref List<WorkingCalendar> list);
        [OperationContract]
        bool UpdateCal(WorkingCalendar cal);
        [OperationContract]
        bool CreateCal(WorkingCalendar cal);
        #endregion
    }
}