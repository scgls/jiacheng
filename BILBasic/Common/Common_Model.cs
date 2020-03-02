using BILBasic.Basing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILBasic.Common
{
    public class ComboBoxItem
    {
        public int ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }

    public class ComboBoxItemExt
    {
        public string ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }

    public class DividPage
    {
        public DividPage()
        {
            _RecordCounts = 0;
            _CurrentPageNumber = 1;
            _PagesCount = 2;
        }

        private int _RecordCounts = 0;
        /// <summary>
        /// 记录总数
        /// </summary>
        public int RecordCounts
        {
            get
            {
                return _RecordCounts;
            }
            set
            {
                _RecordCounts = value;
            }
        }


        private int _CurrentPageRecordCounts;
        /// <summary>
        /// 当前页记录数
        /// </summary>
        public int CurrentPageRecordCounts
        {
            get
            {
                return _CurrentPageRecordCounts;
            }

            set
            {
                _CurrentPageRecordCounts = value;
            }
        }

        private int _CurrentPageShowCounts = 20;
        /// <summary>
        /// 当前页显示行数
        /// </summary>
        public int CurrentPageShowCounts
        {
            get
            {
                return _CurrentPageShowCounts;
            }

            set
            {
                _CurrentPageShowCounts = value;
            }
        }

        private int _CurrentPageNumber;
        /// <summary>
        /// 当前页数
        /// </summary>
        public int CurrentPageNumber
        {
            get
            {
                return _CurrentPageNumber;
            }

            set
            {
                _CurrentPageNumber = value;
            }
        }

        private int _PagesCount;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PagesCount
        {
            get
            {
                return _PagesCount;
            }

            set
            {
                _PagesCount = value;
            }
        }
    }

    public class QueryModel
    {
        /// <summary>
        /// 要查询的字段
        /// </summary>
        public string Fields { get; set; }

        /// <summary>
        /// 要查询的表名或视图名
        /// </summary>
        public string Tables { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public string PK { get; set; }

        /// <summary>
        /// 查询条件 where
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 排序 order by
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 分组 group by
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 分页对象
        /// </summary>
        public DividPage page { get; set; }

        /// <summary>
        /// 得到查询当前页的SQL
        /// </summary>
        /// <returns></returns>
        public string GetPageRecordsSql()
        {
            return "";
        }

        public string GetRecordsCountSql()
        {
            return "";
        }


    }

    public class CompanyInfo
    {
        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string CompanyTel { get; set; }

        public string CompanyFax { get; set; }
    }


    public class PageData<Tmodel>
    {
        public DividPage dividPage { get; set; }
        public List<Tmodel> data { get; set; }
        public string link { get; set; }
    }
}
