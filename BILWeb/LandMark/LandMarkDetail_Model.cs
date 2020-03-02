using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.LandMark
{
    /// <summary>
    /// t_landmarkwithtask的实体类
    /// 作者:方颖
    /// 日期：2019/9/5 16:37:49
    /// </summary>

    public class T_LandMarkWithTaskInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_LandMarkWithTaskInfo() : base() { }


        //私有变量
        
        private decimal? landmarkid;
        private string carno;
        private string taskno;               
        private decimal? isdel;
        private string remark;
        private string remark1;
        private string remark2;
        private string remark3;



        [Display(Name = "地标id")]
        public decimal? LandMarkID
        {
            get
            {
                return landmarkid;
            }
            set
            {
                landmarkid = value;
            }
        }
        [Display(Name = "小车号")]
        public string CarNo
        {
            get
            {
                return carno;
            }
            set
            {
                carno = value;
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

        public string Remark1
        {
            get
            {
                return remark1;
            }
            set
            {
                remark1 = value;
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

        public string Remark3
        {
            get
            {
                return remark3;
            }
            set
            {
                remark3 = value;
            }
        }
        [Display(Name = "地标号")]
        public string landmarkno { get; set; }

    }
}

