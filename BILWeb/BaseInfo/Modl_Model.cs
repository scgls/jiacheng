using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.BaseInfo
{
    public class T_Modl: BILBasic.Basing.Factory.Base_Model
    {
        public T_Modl() : base() { }

        string sn;
        string mouldCode;
        string mouldName;
        string mouldType;
        string category;
        string machineType;
        string mouldStatus;
        string clear;
        string disinfection;
        string uLQty;
        string holeNumber;
        string productType;
        string texture;
        string outSize;
        string unit;
        string openSize;
        string remark;

        public string Sn { get { return sn; } set { sn = value; } }
        public string MouldCode { get { return mouldCode; } set { mouldCode = value; } }
        public string MouldName { get { return mouldName; } set { mouldName = value; } }
        public string MouldType { get { return mouldType; } set { mouldType = value; } }
        public string Category { get { return category; } set { category = value; } }
        public string MachineType { get { return machineType; } set { machineType = value; } }
        public string MouldStatus { get { return mouldStatus; } set { mouldStatus = value; } }
        public string ClearStatus { get { return clear; } set { clear = value; } }
        public string Disinfection { get { return disinfection; } set { disinfection = value; } }
        public string ULQty { get { return uLQty; } set { uLQty = value; } }
        public string Acupoints { get { return holeNumber; } set { holeNumber = value; } }
        public string ProductType { get { return productType; } set { productType = value; } }
        public string Texture { get { return texture; } set { texture = value; } }
        public string OutSize { get { return outSize; } set { outSize = value; } }
        public string Unit { get { return unit; } set { unit = value; } }
        public string OpenSize { get { return openSize; } set { openSize = value; } }
        public string Remark { get { return remark; } set { remark = value; } }
        /// <summary>
        /// 是否启用
        /// </summary>
        public int isDel { get; set; }
    }
}
