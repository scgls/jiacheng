using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.QualityChange
{
    /// <summary>
    /// t_qualitychange的实体类
    /// 作者:方颖
    /// 日期：2017/9/20 15:22:26
    /// </summary>

    public class T_QualityChangeInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_QualityChangeInfo() : base() { }


        //私有变量        
       
        private string qresonecode;        
        private decimal? isdel;
        private string note;
        
        



        //公开属性
        

        public string QresoneCode
        {
            get
            {
                return qresonecode;
            }
            set
            {
                qresonecode = value;
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

        public string VoucherNo { get; set; }
        

    }
}

