using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.Customer
{
    /// <summary>
    /// t_customeraddress的实体类
    /// 作者:方颖
    /// 日期：2017/2/21 10:35:35
    /// </summary>

    public class T_CustomerAddressInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_CustomerAddressInfo() : base() { }


        //私有变量        
        
        private string note;
        private string contactperson;
        private string contacttel;
        private string mobile;
        private string fax;
        private string email;
        private decimal? isdel;
       
        private decimal? isdefault;
        private string address;



        //公开属性
       

        public string Note
        {
            get
            {
                return note;
            }
            set
            {
                note = value;
            }
        }

        public string ContactPerson
        {
            get
            {
                return contactperson;
            }
            set
            {
                contactperson = value;
            }
        }

        public string ContactTel
        {
            get
            {
                return contacttel;
            }
            set
            {
                contacttel = value;
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

        public decimal? IsDel
        {
            get
            {
                return isdel;
            }
            set
            {
                isdel = value;
            }
        }

        

        public decimal? IsDefault
        {
            get
            {
                return isdefault;
            }
            set
            {
                isdefault = value;
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

