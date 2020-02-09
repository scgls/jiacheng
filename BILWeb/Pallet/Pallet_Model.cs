using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Pallet
{
    public class T_PalletInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_PalletInfo() : base() { }


        //私有变量        
        private string palletno;
        



        //公开属性
        

        public string PalletNo
        {
            get
            {
                return palletno;
            }
            set
            {
                palletno = value;
            }
        }

        public int PalletType { get; set; }

    }
}
