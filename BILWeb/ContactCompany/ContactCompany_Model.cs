using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.ContactCompany
{
    public class T_ContactCompanyInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_ContactCompanyInfo() : base() { }


        //私有变量
       
        
        private string companyname;
        private string companynameen;
        private decimal? companytype;
        private string country;
        private string province;
        private string city;
        



        //公开属性
        public string ContactUser { get; set; }

        public string Tel { get; set; }

        public string Note { get; set; }

       

        public string CompanyName
        {
            get
            {
                return companyname;
            }
            set
            {
                companyname = value;
            }
        }

        public string CompanyNameEN
        {
            get
            {
                return companynameen;
            }
            set
            {
                companynameen = value;
            }
        }

        public decimal? CompanyType
        {
            get
            {
                return companytype;
            }
            set
            {
                companytype = value;
            }
        }

        public string Country
        {
            get
            {
                return country;
            }
            set
            {
                country = value;
            }
        }

        public string Province
        {
            get
            {
                return province;
            }
            set
            {
                province = value;
            }
        }

        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }
    }
}
