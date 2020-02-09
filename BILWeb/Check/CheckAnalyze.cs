using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Query
{
    public class CheckAnalyze
    {
        [Display(Name ="操作人")]
        public string Creater { get; set; }
        [Display(Name = "单位")]
        public string unit { get; set; }

        public DateTime EDATE { get; set; }

        public DateTime SEDATE { get; set; }
        public string SUPCODE { get; set; }
        public string SUPNAME { get; set; }
        public DateTime PRODUCTDATE { get; set; }
        public string SUPPRDBATCH { get; set; }
        public DateTime SUPPRDDATE { get; set; }
        public string PALLETNO { get; set; }

        public string BARCODE { get; set; }
        [Display(Name = "盘点单号")]
        public string  CHECKNO {get;set;}
        [Display(Name = "实盘货位")]
        public string  AREANO {get;set;}

        public int AREAID { get; set; }

        public int houseid { get; set; }

        public int warehouseid { get; set; }
        [Display(Name = "实盘货区")]
        public string houseno { get; set; }
        [Display(Name = "实盘仓库")]
        public string warehouseno { get; set; }
        [Display(Name = "库存仓库")]
        public string swarehouseno { get; set; }


        [Display(Name = "实盘物料号")]
        public string  MATERIALNO  {get;set;}

        public int MATERIALID { get; set; }
        [Display(Name = "实盘物料描述")]
        public string  MATERIALDESC {get;set;}
        [Display(Name = "实盘序列")]
        public string  SERIALNO{get;set;}
        [Display(Name = "库存库位")]
        public string  SAREANO{get;set;}

        public int SAREAID { get; set; }
        [Display(Name = "库存物料")]
        public string SMATERIALDESC { get; set; }
        [Display(Name = "库存物料号")]
        public string  SMATERIALNO  {get;set;}
       
        public int SMATERIALNOID { get; set; }
        [Display(Name = "库存序列")]
        public string  SSERIALNO {get;set;}
        [Display(Name = "盈亏情况")]
        public string  remark{get;set;}
        [Display(Name = "实盘数量")]
        public decimal? QTY { get; set; }
        [Display(Name = "库存数量")]
        public decimal? SQTY { get; set; }
        [Display(Name = "盘赢数量")]
        //盘赢数量
        public string YQTY { get; set; }
        [Display(Name = "盘亏数量")]
        //盘亏数量
        public string KQTY { get; set; }

        public string partno { get; set; }


        public string BatchNo { get; set; }

        public string SBatchNo { get; set; }
        [Display(Name = "实盘据点")]
        public string STRONGHOLDCODE { get; set; }


        public string STRONGHOLDNAME { get; set; }
        [Display(Name = "库存据点")]
        public string SSTRONGHOLDCODE { get; set; }

        public int status { get; set; }
        public string statusname { get; set; }

        public int sstatus { get; set; }
        public string sstatusname { get; set; }

        public string ean { get; set; }
        public string sean { get; set; }
    }
}
