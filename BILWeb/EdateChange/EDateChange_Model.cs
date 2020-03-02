using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.EdateChange
{
    /// <summary>
    /// t_edatechange的实体类
    /// 作者:方颖
    /// 日期：2017/9/6 14:48:02
    /// </summary>

    public class T_EDateChangeInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_EDateChangeInfo() : base() { }


        //私有变量       
        private string resonecode;        
        private decimal? isdel;
        private string note;
        private string erpstatus;



        //公开属性
        public string ResoneCode
        {
            get
            {
                return resonecode;
            }
            set
            {
                resonecode = value;
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

        public string ErpStatus
        {
            get
            {
                return erpstatus;
            }
            set
            {
                erpstatus = value;
            }
        }

        public string VoucherNo { get; set; }


    }
}

