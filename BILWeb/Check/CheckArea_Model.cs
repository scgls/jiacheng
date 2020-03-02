using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Check
{
    public class CheckArea_Model
    {
        public int ID { get; set; }
        public  string      AREANO	        {get;set;}

        public string houseno { get; set; }

        public string warehouseno { get; set; }
        public   string     AREANAME	     {get;set;}
        public  string      CONTACTUSER	     {get;set;}
        public  string      CONTACTPHONE	 {get;set;}
        public  string      ADDRESS	         {get;set;}
        public  string      LOCATIONDESC	 {get;set;}
        public   string     CREATER	         {get;set;}
        public   DateTime     CREATETIME	 {get;set;}
        public  string      MODIFYER	     {get;set;}
        public    DateTime    MODIFYTIME	 {get;set;}

        private bool ischeck;
        public bool ISCheck { get { return ischeck;} set { ischeck = false; } }


    }
}
