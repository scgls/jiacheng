using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.MoveStockTask
{
   
    /// <summary>
    /// t_movedetail的实体类
    /// 作者:方颖
    /// 日期：2019/8/15 20:44:17
    /// </summary>

    public class T_MoveTaskDetailInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_MoveTaskDetailInfo() : base() { }


        //私有变量

        private decimal? moveqty;
        private decimal? remainqty;
        private string fromstorageloc;
        private string closeoweuser;
        private DateTime? closeowedate;
        private string closeoweremark;
        private decimal? isdel;
        private string voucherno;

        private string frombatchno;
        private string fromerpareano;
        private string fromerpwarehouse;
        private string tobatchno;
        private string toerpareano;
        private string toerpwarehouse;

        private decimal? inScanQty;
        private decimal? outScanQty;
        public decimal? InScanQty
        {
            get { return inScanQty; }
            set { inScanQty = value; }
        }


        public decimal? OutScanQty
        {
            get { return outScanQty; }
            set { outScanQty = value; }
        }


        //公开属性


        public decimal? MoveQty
        {
            get
            {
                return moveqty;
            }
            set
            {
                moveqty = value;
            }
        }

        public decimal? RemainQty
        {
            get
            {
                return remainqty;
            }
            set
            {
                remainqty = value;
            }
        }

        public string FromStorageLoc
        {
            get
            {
                return fromstorageloc;
            }
            set
            {
                fromstorageloc = value;
            }
        }

        public string CloseOweUser
        {
            get
            {
                return closeoweuser;
            }
            set
            {
                closeoweuser = value;
            }
        }

        public DateTime? CloseOweDate
        {
            get
            {
                return closeowedate;
            }
            set
            {
                closeowedate = value;
            }
        }

        public string CloseOweRemark
        {
            get
            {
                return closeoweremark;
            }
            set
            {
                closeoweremark = value;
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



        public string VoucherNo
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



        public string FromBatchNo
        {
            get
            {
                return frombatchno;
            }
            set
            {
                frombatchno = value;
            }
        }

        public string FromErpAreaNo
        {
            get
            {
                return fromerpareano;
            }
            set
            {
                fromerpareano = value;
            }
        }

        public string FromErpWarehouse
        {
            get
            {
                return fromerpwarehouse;
            }
            set
            {
                fromerpwarehouse = value;
            }
        }

        public string ToBatchNo
        {
            get
            {
                return tobatchno;
            }
            set
            {
                tobatchno = value;
            }
        }

        public string ToErpAreaNo
        {
            get
            {
                return toerpareano;
            }
            set
            {
                toerpareano = value;
            }
        }

        public string ToErpWarehouse
        {
            get
            {
                return toerpwarehouse;
            }
            set
            {
                toerpwarehouse = value;
            }
        }

        public string MaterialNo { get; set; }

        public string MaterialDesc { get; set; }

        public string Unit { get; set; }

        public string UnitName { get; set; }

        public string EAN { get; set; }
    }
    }

