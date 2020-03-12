using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.RetentionChange
{
    /// <summary>
    /// t_retentionchange的实体类
    /// 作者:方颖
    /// 日期：2017/10/19 15:34:06
    /// </summary>

    public class T_RetentionChangeInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_RetentionChangeInfo() : base() { }


        //私有变量
        
        private string resonecode;        
        private decimal? isdel;
        private string note;
        private string retaintype;



       

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

        public string RetainType
        {
            get
            {
                return retaintype;
            }
            set
            {
                retaintype = value;
            }
        }

        public string VoucherNo { get; set; }

        public string StrRetainType { get; set; }

    }
}

