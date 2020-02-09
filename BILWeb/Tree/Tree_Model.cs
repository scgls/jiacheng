using BILBasic.Basing.Factory;
using BILWeb.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.Tree
{
    [KnownType(typeof(T_MenuInfo))]
    public class Tree_Model : Base_Model
    {

        public Tree_Model()
        { 
        }

        private string nodeurl;
        private int nodelevel;
        private int nodesort;
        private int parentid;
        private decimal? mnemonic;
        private string mnemoniccode;

        private string _TreeNo;

        public string TreeNo
        {
            get { return _TreeNo; }
            set { _TreeNo = value; }
        }

        private int _IsEnd;

        public int IsEnd
        {
            get { return _IsEnd; }
            set { _IsEnd = value; }
        }

        private string _IcoName;

        public string IcoName
        {
            get { return _IcoName; }
            set { _IcoName = value; }
        }

        private string _ToolTipText;

        public string ToolTipText
        {
            get { return _ToolTipText; }
            set { _ToolTipText = value; }
        }

       
       

        internal virtual void SetDisplayName()
        {
            this.DisplayName = "请为DisplayName属性赋值";
        }

        private bool _IsChecked;

        public bool IsChecked
        {
            get { return _IsChecked; }
            set { _IsChecked = value; }
        }

        private int _SonQty;

        public int SonQty
        {
            get { return _SonQty; }
            set { _SonQty = value; }
        }


        public decimal? Mnemonic
        {
            get
            {
                return mnemonic;
            }
            set
            {
                mnemonic = value;
            }
        }

        public string Mnemoniccode
        {
            get
            {
                return mnemoniccode;
            }
            set
            {
                mnemoniccode = value;
            }
        }

        public string NodeUrl
        {
            get
            {
                return nodeurl;
            }
            set
            {
                nodeurl = value;
            }
        }

        public int NodeLevel
        {
            get
            {
                return nodelevel;
            }
            set
            {
                nodelevel = value;
            }
        }

        public int NodeSort
        {
            get
            {
                return nodesort;
            }
            set
            {
                nodesort = value;
            }
        }

        public int ParentID
        {
            get
            {
                return parentid;
            }
            set
            {
                parentid = value;
            }
        }

        public string ParentName { get; set; }

        private string menuname;

        public string MenuName
        {
            get
            {
                return menuname;
            }
            set
            {
                menuname = value;
            }
        }

        private string menuno;

        public string MenuNo
        {
            get
            {
                return menuno;
            }
            set
            {
                menuno = value;
            }
        }

        public string MnemonicCode { get; set;}
    }

}
