using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.TransportSupplier
{
    /// <summary>
    /// t_transportsupplierdetail的实体类
    /// 作者:方颖
    /// 日期：2019/9/9 16:39:21
    /// </summary>

    public class T_TransportSupDetailInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_TransportSupDetailInfo() : base() { }


        //私有变量        
        private string platenumber;
        private string feight;
       
        private decimal? isdel;
        private string palletno;
        private string boxcount;
        private string outboxcount;
        private string customername;
        private string remark;
        private string remark1;
        private string remark2;
        private string remark3;
       
        private string type;



        //公开属性

        [Display(Name = "车牌号")]

        public string PlateNumber
        {
            get
            {
                return platenumber;
            }
            set
            {
                platenumber = value;
            }
        }

        [Display(Name = "物流费用")]
        /// <summary>
        /// 物流费用
        /// </summary>
        public string Feight
        {
            get
            {
                return feight;
            }
            set
            {
                feight = value;
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
        [Display(Name = "条码")]
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
        [Display(Name = "箱数")]
        public string BoxCount
        {
            get
            {
                return boxcount;
            }
            set
            {
                boxcount = value;
            }
        }
        [Display(Name = "箱数")]
        public string OutBoxCount
        {
            get
            {
                return outboxcount;
            }
            set
            {
                outboxcount = value;
            }
        }
        [Display(Name = "客户")]
        public string CustomerName
        {
            get
            {
                return customername;
            }
            set
            {
                customername = value;
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


        [Display(Name = "类别")]
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        [Display(Name = "物流号")]
        /// <summary>
        /// 物流单号
        /// </summary>
        public string VoucherNo { get; set; }

        //public string guid { get; set; }
        /// <summary>
        /// 仓退单C,销货单A
        /// </summary>
        public string gtype { get; set; }

        [Display(Name = "类型")]
        public string strType { get; set; }

        [Display(Name = "交易状态")]
        public string TradingConditionsCode { get; set; }

        //收货地址
        public string Address { get; set; }

        //物流地址
        public string Address1 { get; set; }

        //联系人
        public string Contact { get; set; }

        //电话
        public string Phone { get; set; }

    }
}

