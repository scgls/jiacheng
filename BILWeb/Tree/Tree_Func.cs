
using BILWeb.Tree;
using System;
using System.Collections.Generic;

namespace BILWeb.Basing
{
    public class Tree_Func
    {
        public bool GetTreeNo(ref Tree_Model model, ref string strError) 
        {
            Tree_DB _db = new Tree_DB();
            try
            {
                string menuNo = _db.GetTreeNo(model);
                if (string.IsNullOrEmpty(menuNo)) return false;
                model.TreeNo = menuNo;
                return true;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }
    }
}
