using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.PickCar
{
    /// <summary>
    /// t_pickcar的实体类
    /// 作者:方颖
    /// 日期：2019/8/30 23:48:45
    /// </summary>

    public class T_PickCarInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_PickCarInfo() : base() { }


        //私有变量
       
        private string carno;
        private string taskno;

        //公开属性
       

        [Display(Name ="拣货车号")]
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


        public int IsDel { get; set; }
       

    }
}

