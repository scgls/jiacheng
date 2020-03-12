using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.TransportSupplier
{
    
    public class T_TransportSupplier : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_TransportSupplier() : base() { }

        public int TDID { get; set; }

        /// <summary>
        /// wms单号
        /// </summary>
        public string VoucherNo { get; set; }

        /// <summary>
        /// 车牌
        /// </summary>
        public string PlateNumber { get; set; }


        public decimal? IsDel { get; set; }

        /// <summary>
        /// 体积
        /// </summary>
        public decimal? Volume { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// 件数
        /// </summary>
        public decimal? CartonNum { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal? Feight { get; set; }
        
        /// <summary>
        /// 承运商ID
        /// </summary>
        public int TransportSupplierID { get; set; }

        /// <summary>
        /// 承运商
        /// </summary>
        public string TransportSupplierName { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public string Destina { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}

