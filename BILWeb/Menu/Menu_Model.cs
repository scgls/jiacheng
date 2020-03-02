using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.Menu
{
    /// <summary>
    /// T_MENU的实体类
    /// 作者:方颖
    /// 日期：2016/10/20 10:39:28
    /// </summary>
    
    public class T_MenuInfo : BILWeb.Tree.Tree_Model
    {
        //无参构造函数
        public T_MenuInfo() : base() { }


        //私有变量
        
        
        //private string menuname;
        private string menuabbname;
        private int menutype;
        private string projectname;
        //private string iconame;
        private decimal? safelevel;
        private decimal? isdefault;
        //private decimal? mnemonic;
        //private string mnemoniccode;
        
        private int menustatus;
        private string description;
        private decimal? isdel;        
        private decimal? menustyle;



        //公开属性
        

        

        //public string MENUNAME
        //{
        //    get
        //    {
        //        return menuname;
        //    }
        //    set
        //    {
        //        menuname = value;
        //    }
        //}

        public string MemuAbbName
        {
            get
            {
                return menuabbname;
            }
            set
            {
                menuabbname = value;
            }
        }

        public int MenuType
        {
            get
            {
                return menutype;
            }
            set
            {
                menutype = value;
            }
        }

        public string ProjectName
        {
            get
            {
                return projectname;
            }
            set
            {
                projectname = value;
            }
        }

        //public string ICONAME
        //{
        //    get
        //    {
        //        return iconame;
        //    }
        //    set
        //    {
        //        iconame = value;
        //    }
        //}

        public decimal? SafeLevel
        {
            get
            {
                return safelevel;
            }
            set
            {
                safelevel = value;
            }
        }

        public decimal? IsDefault
        {
            get
            {
                return isdefault;
            }
            set
            {
                isdefault = value;
            }
        }

        //public decimal? MNEMONIC
        //{
        //    get
        //    {
        //        return mnemonic;
        //    }
        //    set
        //    {
        //        mnemonic = value;
        //    }
        //}

        //public string MNEMONICCODE
        //{
        //    get
        //    {
        //        return mnemoniccode;
        //    }
        //    set
        //    {
        //        mnemoniccode = value;
        //    }
        //}

        //public string NODEURL
        //{
        //    get
        //    {
        //        return nodeurl;
        //    }
        //    set
        //    {
        //        nodeurl = value;
        //    }
        //}

        //public decimal NODELEVEL
        //{
        //    get
        //    {
        //        return nodelevel;
        //    }
        //    set
        //    {
        //        nodelevel = value;
        //    }
        //}

        //public decimal? NODESORT
        //{
        //    get
        //    {
        //        return nodesort;
        //    }
        //    set
        //    {
        //        nodesort = value;
        //    }
        //}

        //public decimal PARENTID
        //{
        //    get
        //    {
        //        return parentid;
        //    }
        //    set
        //    {
        //        parentid = value;
        //    }
        //}

        public int MenuStatus
        {
            get
            {
                return menustatus;
            }
            set
            {
                menustatus = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
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
        

        public decimal? MenuStyle
        {
            get
            {
                return menustyle;
            }
            set
            {
                menustyle = value;
            }
        }


        //辅助字段

        public string StrMenuType { get; set; }

        public string StrMenuStatus { get; set; }

        public bool BIsDefault { get; set; }

        public bool BIsChecked { get; set; }

        public bool BHaveParameter { get; set; }
        public string StrMenuStyle { get; set; }

        public List<T_MenuInfo> lstSubMenu {get;set;}
        
    }
}

