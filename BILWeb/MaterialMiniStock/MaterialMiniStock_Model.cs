using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.MaterialMiniStock
{
    /// <summary>
    /// t_material_ministock的实体类
    /// 作者:方颖
    /// 日期：2019/9/9 16:59:49
    /// </summary>

    public class T_Material_MiniStockInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_Material_MiniStockInfo() : base() { }


        //私有变量

        private decimal? warehouseid;

        private decimal? miniqty;



        public decimal? WarehouseID
        {
            get
            {
                return warehouseid;
            }
            set
            {
                warehouseid = value;
            }
        }




        public decimal? MiniQty
        {
            get
            {
                return miniqty;
            }
            set
            {
                miniqty = value;
            }
        }

        public string MaterialNo { get; set; }
        public int IsDel { get; set; }


    }
}
