using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BILWeb.WorkCal
{
    public class Calendar_DB
    {
        public List<WorkingCalendar> GetCalList(int year)
        {
            List<WorkingCalendar> cal_list = new List<WorkingCalendar>();

            string query = "SELECT * FROM MES_WORKCALENDARTIME WHERE WORKDATE LIKE :WORKDATE||'%' ORDER BY WORKDATE,TEAMTYPE";
            
            List<OracleParameter> para_list = new List<OracleParameter>();

            para_list.Add(new OracleParameter(":WORKDATE", year));

            DataSet ds = dbFactory.ExecuteDataSet(System.Data.CommandType.Text, query, para_list.ToArray());

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                WorkingCalendar cal = new WorkingCalendar();

                if (row["ADDWORKTODATE"] != DBNull.Value)
                {
                    cal.Overtime_end = row["ADDWORKTODATE"].ToString();
                    cal.Overtime_start = row["ADDWORKFROMDATE"].ToString();
                }
                cal.Team_type = row["TEAMTYPE"].ToString();
                cal.Work_date = row["WORKDATE"].ToString();
                cal.Work_end = row["WORKTODATE"].ToString();
                cal.Work_person = Convert.ToInt32(row["QTY"]);
                cal.Work_start = row["WORKFROMDATE"].ToString();
                cal.Id = Convert.ToInt32(row["ID"]);

                cal_list.Add(cal);
            }

            return cal_list;
        }

        public bool UpdateCal(WorkingCalendar cal)
        {
            string query = "UPDATE MES_WORKCALENDARTIME SET WORKFROMDATE=:WORKFROMDATE,WORKTODATE=:WORKTODATE," +
                "QTY=:QTY,ADDWORKFROMDATE=:ADDWORKFROMDATE,ADDWORKTODATE=:ADDWORKTODATE WHERE WORKDATE='" + cal.Work_date + "' AND TEAMTYPE='"+ cal.Team_type+"'";


            List<OracleParameter> para_list = new List<OracleParameter>();

            para_list.Add(new OracleParameter(":WORKFROMDATE", cal.Work_start));
            para_list.Add(new OracleParameter(":WORKTODATE", cal.Work_end));
            para_list.Add(new OracleParameter(":QTY", cal.Work_person));
            para_list.Add(new OracleParameter(":ADDWORKFROMDATE", cal.Overtime_start));
            para_list.Add(new OracleParameter(":ADDWORKTODATE", cal.Overtime_end));

            return dbFactory.ExecuteNonQuery(CommandType.Text, query, para_list.ToArray()) > 0;
        }

        public bool CreateCal(WorkingCalendar cal)
        {
            string query = "INSERT INTO MES_WORKCALENDARTIME(ID,WORKDATE,TEAMTYPE,WORKFROMDATE,WORKTODATE,QTY,ADDWORKFROMDATE,ADDWORKTODATE)" +
                "VALUES(:ID,:WORKDATE,:TEAMTYPE,:WORKFROMDATE,:WORKTODATE,:QTY,:ADDWORKFROMDATE,:ADDWORKTODATE)";

            List<OracleParameter> para_list = new List<OracleParameter>();

            para_list.Add(new OracleParameter(":ID", this.GetID()));
            para_list.Add(new OracleParameter(":WORKDATE", cal.Work_date));
            para_list.Add(new OracleParameter(":TEAMTYPE", cal.Team_type));
            para_list.Add(new OracleParameter(":WORKFROMDATE", cal.Work_start));
            para_list.Add(new OracleParameter(":WORKTODATE", cal.Work_end));
            para_list.Add(new OracleParameter(":QTY", cal.Work_person));
            para_list.Add(new OracleParameter(":ADDWORKFROMDATE", cal.Overtime_start));
            para_list.Add(new OracleParameter(":ADDWORKTODATE", cal.Overtime_end));

            return dbFactory.ExecuteNonQuery(CommandType.Text, query, para_list.ToArray()) > 0;
        }

        public bool CreateCalSummary(DateTime date,double hours)
        {
            string query = "INSERT INTO MES_WORKCALENDAR(WORKYEAR,WORKMONTH,WORKDAY,WORKDATE,ISWORKDAY,WORKWEEKS,WORKHOURQTY)" +
                "VALUES(:WORKYEAR,:WORKMONTH,:WORKDAY,:WORKDATE,:ISWORKDAY,:WORKWEEKS,:WORKHOURQTY)";

            GregorianCalendar gc = new GregorianCalendar(GregorianCalendarTypes.Localized);

            List<OracleParameter> para_list = new List<OracleParameter>();

            para_list.Add(new OracleParameter(":WORKYEAR", date.Year));
            para_list.Add(new OracleParameter(":WORKMONTH", date.Month));
            para_list.Add(new OracleParameter(":WORKDAY",date.Day));
            para_list.Add(new OracleParameter(":WORKDATE",date.ToString("yyyy-MM-dd")));
            para_list.Add(new OracleParameter(":ISWORKDAY", hours > 0 ? "1" : "0"));
            para_list.Add(new OracleParameter(":WORKWEEKS", date.DayOfWeek.ToString()));
            para_list.Add(new OracleParameter(":WORKHOURQTY", hours));

            return dbFactory.ExecuteNonQuery(CommandType.Text, query, para_list.ToArray()) > 0;
        }

        public bool UpdateCalSummary(DateTime date, double hours)
        {
            string query = "Update MES_WORKCALENDAR SET ISWORKDAY=:ISWORKDAY,WORKHOURQTY=:WORKHOURQTY WHERE " +
                "WORKDATE='"+ date.ToString("yyyy-MM-dd")+"'";

            GregorianCalendar gc = new GregorianCalendar(GregorianCalendarTypes.Localized);

            List<OracleParameter> para_list = new List<OracleParameter>();

            para_list.Add(new OracleParameter(":ISWORKDAY", hours > 0 ? "1" : "0"));
            para_list.Add(new OracleParameter(":WORKHOURQTY", hours));

            return dbFactory.ExecuteNonQuery(CommandType.Text, query, para_list.ToArray()) > 0;
        }

        private int GetID()
        {
            object id = dbFactory.ExecuteScalar(System.Data.CommandType.Text, "SELECT MAX(ID) FROM MES_WORKCALENDARTIME");

            if (id == DBNull.Value)
                return 1;
            else
                return Convert.ToInt32(id) + 1;
        }
    }
}
