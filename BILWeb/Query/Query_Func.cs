using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Query
{
    public class Query_Func
    {
        public static void ChangeQuery(ref string t)
        {
            string[] list = t.Split(',');
            for (int i = 0; i < list.Count(); i++)
            {
                list[i] = "'" + list[i] + "'";
            }
            string res = "";
            foreach (string j in list)
            {
                res += j + ",";
            }
            t = res.Substring(0, res.Length - 1);
        }
    }
}
