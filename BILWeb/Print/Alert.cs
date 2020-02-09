using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.Print
{
    public class Alert
    {
        public int ID{get;set;}		
        public string MESSAGETYPE{get;set;}

        public string MESSAGESUBTYPE { get; set; }			
        public string MESSAGEDESC{get;set;}			
        public DateTime CREATETIME{get;set;}

        public string SCREATETIME { get; set; }
        //public DateTime SENDTIME { get; set; }			
        public int ISRETURN	{get;set;}			
        public string REMARK	{get;set;}		
        public string LINENO    {get;set;}

        public bool isChecked { get; set; }

        public string USERNO { get; set; }

        public DateTime USINGTIME { get; set; }

        public string SUSINGTIME { get; set; }

    }
}
