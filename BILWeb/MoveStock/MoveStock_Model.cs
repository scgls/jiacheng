using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.Move
{
    /// <summary>
    /// t_move的实体类
    /// 作者:方颖
    /// 日期：2019/8/15 20:28:16
    /// </summary>

    public class T_MoveInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_MoveInfo() : base() { }


        //私有变量
        private string voucherno;
        private string note;
       
        //公开属性       

        public int IsDel { get; set; }       

        public string Voucherno
        {
            get
            {
                return voucherno;
            }
            set
            {
                voucherno = value;
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

        

    }
}

