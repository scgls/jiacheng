using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.Supplier
{
    /// <summary>
    /// t_supplieraddress的实体类
    /// 作者:方颖
    /// 日期：2017/2/18 12:54:58
    /// </summary>

    public class T_SupplierAddressInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_SupplierAddressInfo() : base() { }


        //私有变量
        
        private int supplierid;
        private string contactperson;
        private string contacttel;
        private string mobile;
        private string fax;
        private string email;
        private string address;
        private decimal? isdel;
        private decimal? isdefault;
      



        //公开属性
       

        public int SupplierID
        {
            get
            {
                return supplierid;
            }
            set
            {
                supplierid = value;
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

        

    }
}

