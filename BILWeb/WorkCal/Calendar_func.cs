using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.WorkCal
{
    public class Calendar_func
    {
        Calendar_DB db = new Calendar_DB();
        public List<WorkingCalendar> GetCalList(int year)
        {
            return db.GetCalList(year);
        }

        public bool UpdateCal(WorkingCalendar cal)
        {
            if (!db.UpdateCal(cal))
                db.CreateCal(cal);
            if (!db.UpdateCalSummary(Convert.ToDateTime(cal.Work_date)
            , ((Convert.ToDateTime(cal.Overtime_end) - Convert.ToDateTime(cal.Overtime_start)).TotalMinutes +
            (Convert.ToDateTime(cal.Work_end) - Convert.ToDateTime(cal.Work_start)).TotalMinutes) / 60))
                db.CreateCalSummary(Convert.ToDateTime(cal.Work_date)
                , ((Convert.ToDateTime(cal.Overtime_end) - Convert.ToDateTime(cal.Overtime_start)).TotalMinutes +
                (Convert.ToDateTime(cal.Work_end) - Convert.ToDateTime(cal.Work_start)).TotalMinutes) / 60);

            return true;
        }

        public bool CreateCal(WorkingCalendar cal)
        {
            bool flag = db.CreateCal(cal);
            if (!db.UpdateCalSummary(Convert.ToDateTime(cal.Work_date)
            , ((Convert.ToDateTime(cal.Overtime_end) - Convert.ToDateTime(cal.Overtime_start)).TotalMinutes +
            (Convert.ToDateTime(cal.Work_end) - Convert.ToDateTime(cal.Work_start)).TotalMinutes) / 60))
                db.CreateCalSummary(Convert.ToDateTime(cal.Work_date)
                , ((Convert.ToDateTime(cal.Overtime_end) - Convert.ToDateTime(cal.Overtime_start)).TotalMinutes +
                (Convert.ToDateTime(cal.Work_end) - Convert.ToDateTime(cal.Work_start)).TotalMinutes) / 60);

            return flag;
        }
    }
}
