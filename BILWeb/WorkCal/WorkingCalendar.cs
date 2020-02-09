using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.WorkCal
{
    public class WorkingCalendar
    {
        int _id;
        string _work_date;
        string _team_type;
        string _work_start;
        string _work_end;
        string _overtime_start;
        string _overtime_end;
        int _person;
        public int Id { get { return _id; } set { _id = value; } }
        public string Work_date { get { return _work_date; } set { _work_date = value; } }
        public string Team_type { get { return _team_type; } set { _team_type = value; } }
        public string Work_start { get { return _work_start; } set { _work_start = value; } }
        public string Work_end { get { return _work_end; } set { _work_end = value; } }
        public string Overtime_start { get { return _overtime_start; } set { _overtime_start = value; } }
        public string Overtime_end { get { return _overtime_end; } set { _overtime_end = value; } }
        public int Work_person { get { return _person; } set { _person = value; } }
    }
}
