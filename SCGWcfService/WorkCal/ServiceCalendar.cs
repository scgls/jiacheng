using BILBasic.Common;
using BILWeb.BaseInfo;
using BILWeb.Customer;
using BILWeb.Login.User;
using BILWeb.ProductGY;
using BILWeb.WorkCal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCGWcfService
{
    public partial class ServiceWMS : IService
    {
        public void GetCalendarList(int year,ref List<WorkingCalendar> list)
        {
            Calendar_func func = new Calendar_func();
            list=func.GetCalList(year);
        }

        public bool UpdateCal(WorkingCalendar cal)
        {
            Calendar_func func = new Calendar_func();
            return func.UpdateCal(cal);
        }

        public bool CreateCal(WorkingCalendar cal)
        {
            Calendar_func func = new Calendar_func();
            return func.CreateCal(cal);
        }
    }
}