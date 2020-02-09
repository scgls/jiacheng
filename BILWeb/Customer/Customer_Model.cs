using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.Customer
{
    /// <summary>
    /// t_customer的实体类
    /// 作者:方颖
    /// 日期：2017/2/21 10:21:43
    /// </summary>

    public class T_CustomerInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_CustomerInfo() : base() { }


        //私有变量
        
        private string customerno;
        private string customername;
        private string englishname;
        private string customerabridge;
        private decimal? customerstyle;
        private int companyid;
        private string note;
        private string contactperson;
        private string contacttel;
        private string mobile;
        private string fax;
        private string email;
        private decimal? isdel;        
        private string mailadress;
        private string address;



        //公开属性

        [Display(Name = "客户编码")]
        public string CustomerNo
        {
            get
            {
                return customerno;
            }
            set
            {
                customerno = value;
            }
        }
        [Display(Name = "客户")]
        public string CustomerName
        {
            get
            {
                return customername;
            }
            set
            {
                customername = value;
            }
        }

        public string EnglishName
        {
            get
            {
                return englishname;
            }
            set
            {
                englishname = value;
            }
        }

        public string CustomerAbridge
        {
            get
            {
                return customerabridge;
            }
            set
            {
                customerabridge = value;
            }
        }

        public decimal? CustomerStyle
        {
            get
            {
                return customerstyle;
            }
            set
            {
                customerstyle = value;
            }
        }

        public int CompanyID
        {
            get
            {
                return companyid;
            }
            set
            {
                companyid = value;
            }
        }

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
        [Display(Name = "联系人")]
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
        [Display(Name = "联系电话")]
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
        [Display(Name = "手机")]
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

        

        public string MailAdress
        {
            get
            {
                return mailadress;
            }
            set
            {
                mailadress = value;
            }
        }
        [Display(Name = "地址")]
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

