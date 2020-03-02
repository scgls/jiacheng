using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.ContactCompany
{
    public class T_ContactCompany_AddressInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_ContactCompany_AddressInfo() : base() { }


        //私有变量
        
        private decimal? contactcompanyid;
        private string contactuser;
        private string tel;
        private string mobile;
        private string fax;
        private string netaddress;
        private string email;
        private string address;
       



        //公开属性
        

        public decimal? ContactCompanyID
        {
            get
            {
                return contactcompanyid;
            }
            set
            {
                contactcompanyid = value;
            }
        }

        public string ContactUser
        {
            get
            {
                return contactuser;
            }
            set
            {
                contactuser = value;
            }
        }

        public string Tel
        {
            get
            {
                return tel;
            }
            set
            {
                tel = value;
            }
        }

        public string Mobile
        {
            get
            {
                return mobile;
            }
            set
            {
                mobile = value;
            }
        }

        public string Fax
        {
            get
            {
                return fax;
            }
            set
            {
                fax = value;
            }
        }

        public string NetAddress
        {
            get
            {
                return netaddress;
            }
            set
            {
                netaddress = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }

        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
            }
        }
    }
}
