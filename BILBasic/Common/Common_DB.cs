using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection;

namespace BILBasic.Common
{
    public class Common_DB
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="RecordCounts"></param>
        /// <param name="Tables"></param>
        /// <param name="Filter"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageNumber"></param>
        /// <param name="Fields"></param>
        /// <param name="Sort"></param>
        /// <returns></returns>
        public static OracleDataReader QueryByDividPage(ref Common.DividPage page, string Tables, string Filter = "", string Fields = "*", string Sort = "Order by ID Desc")
        {


            if (Fields.Trim() == "*")
            {
                if (Tables.Contains(")N"))
                {
                    string temtable = Tables.Substring(Tables.Length - 1, 1);
                    if (Fields.Trim() == "*") Fields = temtable + '.' + Fields;
                }
                else
                    Fields = Tables + '.' + Fields;
            } 

            
            if (page == null) page = new Common.DividPage();
            if (page.CurrentPageNumber == 0) page.CurrentPageNumber = 1;
            int RecordCounts = 0;

            int TopNumber = page.CurrentPageShowCounts * page.CurrentPageNumber;
            int WhereNumber = (page.CurrentPageNumber - 1) * page.CurrentPageShowCounts;

            string strSqlRecordCounts = "select count(*) as recordcounts  from " + Tables + "  " + Filter;
            using (OracleDataReader dr = OracleDBHelper.ExecuteReader(strSqlRecordCounts))
            {
                if (dr.Read())
                {
                    RecordCounts = int.Parse(dr["recordcounts"].ToString());
                }
            }



            string strSql = "Select * From (Select ROW_NUMBER() OVER(@Sort) AS PageRowNumber , @Fields  From  @Tables  @Filter ) Where PageRowNumber <= @TopNumber And PageRowNumber > @WhereNumber ";

            strSql = strSql.Replace("@TopNumber", TopNumber.ToString());
            strSql = strSql.Replace("@Sort", Sort.ToString());
            strSql = strSql.Replace("@Fields", Fields.ToString());
            strSql = strSql.Replace("@Tables", Tables.ToString());
            strSql = strSql.Replace("@Filter", Filter.ToString());
            strSql = strSql.Replace("@WhereNumber", WhereNumber.ToString());

            OracleDataReader dR = OracleDBHelper.ExecuteReader(strSql);

            page.RecordCounts = RecordCounts;
            if (page.RecordCounts > 0)
            {
                page.PagesCount = (RecordCounts + page.CurrentPageShowCounts - 1) / page.CurrentPageShowCounts;

            }
            else
            {
                page.PagesCount = 0;
                page.CurrentPageRecordCounts = 0;
            }

            return dR;
        }

        public static OracleDataReader QueryByDividPage2(ref Common.DividPage page, string Tables)
        {

            if (page == null) page = new Common.DividPage();
            if (page.CurrentPageNumber == 0) page.CurrentPageNumber = 1;
            int RecordCounts = 0;

            int TopNumber = page.CurrentPageShowCounts * page.CurrentPageNumber;
            int WhereNumber = (page.CurrentPageNumber - 1) * page.CurrentPageShowCounts;

            string strSqlRecordCounts = "select count(*) as recordcounts  from (" + Tables + ")";
            using (OracleDataReader dr = OracleDBHelper.ExecuteReader(strSqlRecordCounts))
            {
                if (dr.Read())
                {
                    RecordCounts = int.Parse(dr["recordcounts"].ToString());
                }
            }
            //string strSql = "Select * From (Select ROW_NUMBER() OVER(@Sort) AS PageRowNumber , *  From  (@Tables) ) Where PageRowNumber <= @TopNumber And PageRowNumber > @WhereNumber ";
            string strSql = "Select * From (@Tables) Where PageRowNumber <= @TopNumber And PageRowNumber > @WhereNumber ";

            strSql = strSql.Replace("@TopNumber", TopNumber.ToString());
            //strSql = strSql.Replace("@Sort", Sort.ToString());
            //strSql = strSql.Replace("@Fields", Fields.ToString());
            strSql = strSql.Replace("@Tables", Tables.ToString());
            //strSql = strSql.Replace("@Filter", Filter.ToString());
            strSql = strSql.Replace("@WhereNumber", WhereNumber.ToString());

            OracleDataReader dR = OracleDBHelper.ExecuteReader(strSql);

            page.RecordCounts = RecordCounts;
            if (page.RecordCounts > 0)
            {
                page.PagesCount = (RecordCounts + page.CurrentPageShowCounts - 1) / page.CurrentPageShowCounts;


            }
            else
            {
                page.PagesCount = 0;
                page.CurrentPageRecordCounts = 0;
            }

            return dR;
        }

        public static DataTable QueryByDividPage(ref Common.DividPage page, string sql)
        {

            if (page == null) page = new Common.DividPage();
            if (page.CurrentPageNumber == 0) page.CurrentPageNumber = 1;
            int RecordCounts = 0;

            int TopNumber = page.CurrentPageShowCounts * page.CurrentPageNumber;
            int WhereNumber = (page.CurrentPageNumber - 1) * page.CurrentPageShowCounts;

            string strSqlRecordCounts = "select count(*) as recordcounts  from (" + sql + ")";
            using (OracleDataReader dr = OracleDBHelper.ExecuteReader(strSqlRecordCounts))
            {
                if (dr.Read())
                {
                    RecordCounts = int.Parse(dr["recordcounts"].ToString());
                }
            }
            //string strSql = "Select * From (Select ROW_NUMBER() OVER(@Sort) AS PageRowNumber , *  From  (@Tables) ) Where PageRowNumber <= @TopNumber And PageRowNumber > @WhereNumber ";
            string strSql = "Select * From (@Tables) Where PageRowNumber <= @TopNumber And PageRowNumber > @WhereNumber ";

            strSql = strSql.Replace("@TopNumber", TopNumber.ToString());
            //strSql = strSql.Replace("@Sort", Sort.ToString());
            //strSql = strSql.Replace("@Fields", Fields.ToString());
            strSql = strSql.Replace("@Tables", sql.ToString());
            //strSql = strSql.Replace("@Filter", Filter.ToString());
            strSql = strSql.Replace("@WhereNumber", WhereNumber.ToString());

            DataTable dt =  OracleDBHelper.ExecuteDataSet(CommandType.Text, strSql).Tables[0];
          
            page.RecordCounts = RecordCounts;
            if (page.RecordCounts > 0)
            {
                page.PagesCount = (RecordCounts + page.CurrentPageShowCounts - 1) / page.CurrentPageShowCounts;


            }
            else
            {
                page.PagesCount = 0;
                page.CurrentPageRecordCounts = 0;
            }

            return dt;
        }


        public static int CheckRepeatValue(string TableName, string ColumnName, object Value, int ID, string IDColumnName = "ID")
        {
            try
            {

                if (Value == null) return 0;
                //if (string.IsNullOrEmpty(value.ToString())) return 0;

                int iOut = 0;
                string strError;
                int RepeatNum = 0;

                OracleParameter[] param = new OracleParameter[]{
                    new OracleParameter("@ErrorMsg",OracleDbType.NVarchar2,100),
                    new OracleParameter("@RepeatNum",System.Data.SqlDbType.Int),
                    new OracleParameter("@TableName", OracleDBHelper.ToDBValue(TableName)),
                    new OracleParameter("@ColumnName", OracleDBHelper.ToDBValue(ColumnName)),
                    new OracleParameter("@Value", OracleDBHelper.ToDBValue(Value)),    
                    new OracleParameter("@IDColumnName", OracleDBHelper.ToDBValue(IDColumnName)), 
                    new OracleParameter("@ID", OracleDBHelper.ToDBValue(ID)),                      
                };

                param[0].Direction = System.Data.ParameterDirection.Output;
                param[1].Direction = System.Data.ParameterDirection.Output;

                int i = OracleDBHelper.RunProcedure("P_Check_RepeatValue", param, out iOut);
                strError = param[0].Value.ToString();
                RepeatNum = Convert.ToInt32(param[1].Value);
                return RepeatNum;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
