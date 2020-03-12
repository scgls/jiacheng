using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.Supplier
{
    /// <summary>
    /// t_supplier的实体类
    /// 作者:方颖
    /// 日期：2017/2/18 12:41:14
    /// </summary>

    public class T_SupplierInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_SupplierInfo() : base() { }


        //私有变量
        
        private string supplierno;
        private string suppliername;
        private string suppliernameen;
        private string supplierabridge;
        private string note;
        private string contactperson;
        private string contacttel;
        private string mobile;
        private string fax;
        private string email;
        private decimal? isdel;
        
        private string address;



        //公开属性
        [Display(Name = "供应商编码")]
        public string SupplierNo
        {
            get
            {
                return supplierno;
            }
            set
            {
                supplierno = value;
            }
        }
        [Display(Name = "供应商")]
        public string SupplierName
        {
            get
            {
                return suppliername;
            }
            set
            {
                suppliername = value;
            }
        }

        public string SupplierNameEN
        {
            get
            {
                return suppliernameen;
            }
            set
            {
                suppliernameen = value;
            }
        }

        public string SupplierAbridge
        {
            get
            {
                return supplierabridge;
            }
            set
            {
                supplierabridge = value;
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
        [Display(Name = "手机号")]
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

        public string MailAddress { get; set; }

    }
}

