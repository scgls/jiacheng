using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Material
{
    public class T_Material_BatchInfo : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_Material_BatchInfo() : base() { }


        //私有变量       
        private int materialid;
        private string brand;
        private string batchno;        
        private decimal? pquantily;
        private decimal? dquantily;
        private decimal? rquantily;
        private DateTime? edate;
        private string wbs;
        private string projectno;
        private string customerno;
        private string customername;
        private decimal? iescrow;
        private string factorycode;
        private string factoryname;
        private string version;
        private string rohs;
        private string customernote;
        private string exceptionnote;
       



       

        public int MaterialID
        {
            get
            {
                return materialid;
            }
            set
            {
                materialid = value;
            }
        }

        public string Brand
        {
            get
            {
                return brand;
            }
            set
            {
                brand = value;
            }
        }

        public string BatchNo
        {
            get
            {
                return batchno;
            }
            set
            {
                batchno = value;
            }
        }

        

        public decimal? PQuantily
        {
            get
            {
                return pquantily;
            }
            set
            {
                pquantily = value;
            }
        }

        public decimal? DQuantily
        {
            get
            {
                return dquantily;
            }
            set
            {
                dquantily = value;
            }
        }

        public decimal? RQuantily
        {
            get
            {
                return rquantily;
            }
            set
            {
                rquantily = value;
            }
        }

        public DateTime? Edate
        {
            get
            {
                return edate;
            }
            set
            {
                edate = value;
            }
        }

        public string WBS
        {
            get
            {
                return wbs;
            }
            set
            {
                wbs = value;
            }
        }

        public string ProjectNo
        {
            get
            {
                return projectno;
            }
            set
            {
                projectno = value;
            }
        }

        public string CustomerNo
        {
            get
            {
                return customerno;
            }
            set
            {
                customerno = value;
            }
        }

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

        public decimal? IEscrow
        {
            get
            {
                return iescrow;
            }
            set
            {
                iescrow = value;
            }
        }

        public string FactoryCode
        {
            get
            {
                return factorycode;
            }
            set
            {
                factorycode = value;
            }
        }

        public string FactoryName
        {
            get
            {
                return factoryname;
            }
            set
            {
                factoryname = value;
            }
        }

        public string Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
            }
        }

        public string ROHS
        {
            get
            {
                return rohs;
            }
            set
            {
                rohs = value;
            }
        }

        public string CustomerNote
        {
            get
            {
                return customernote;
            }
            set
            {
                customernote = value;
            }
        }

        public string ExceptionNote
        {
            get
            {
                return exceptionnote;
            }
            set
            {
                exceptionnote = value;
            }
        }

        public string WareHouseNo { get; set; }

        public string HouseNo { get; set; }

        public string AreaNo { get; set; }

        public string SupCode { get; set; }

        public string SupName { get; set; }

        public decimal StockQty { get; set; }

        public DateTime SupPrdDate { get; set; }

        public string SupPrdBatch { get; set; }

        public DateTime ProductDate { get; set; }

        public string ProductBatch { get; set; }
    }
}
