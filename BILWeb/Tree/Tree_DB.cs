
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.DBA;
using System.Data;


namespace BILWeb.Tree
{
    class Tree_DB 
    {
        public string GetTreeNo(Tree_Model model)
        {
            string strSql = string.Empty;
            if (model.ID <= 0) strSql = "SELECT TO_CHAR(TO_NUMBER(isnull(MAX(MENUNO),10000000000000000000)) + 1) FROM T_MENU";
            else strSql = string.Format("SELECT menuno FROM T_Menu WHERE ID = {0}", model.ID);

            object o;
            o = BILBasic.DBA.OracleDBHelper.ExecuteScalar(CommandType.Text, strSql, null);
            return o.ToString();
        }

        
    }
}
