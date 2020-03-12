using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.LandMark
{
    public class T_LandMarkInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_LandMarkInfo() : base() { }


        //私有变量

        private string landmarkno;
        private string remark;
        private string remark2;

        private decimal? isdel;



        //公开属性

            [Display(Name ="地标号")]
        public string LandMarkNo
        {
            get
            {
                return landmarkno;
            }
            set
            {
                landmarkno = value;
            }
        }

        public string Remark
        {
            get
            {
                return remark;
            }
            set
            {
                remark = value;
            }
        }

        public string Remark2
        {
            get
            {
                return remark2;
            }
            set
            {
                remark2 = value;
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

    }
}
