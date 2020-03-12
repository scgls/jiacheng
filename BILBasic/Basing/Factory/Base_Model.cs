using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILBasic.Basing.Factory
{

    
    public class Base_Model
    {
        public Base_Model()
        {
            _ID = 0;
            _HeaderID = 0;

        }
        
        private int _ID;
        
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private int _HeaderID;
        public int HeaderID
        {
            get { return _HeaderID; }
            set { _HeaderID = value; }
        }

        private string _OrderNumber;
        [System.Web.Script.Serialization.ScriptIgnore]
        public string OrderNumber
        {
            get { return _OrderNumber; }
            set { _OrderNumber = value; }
        }

        private int _Status = 0;
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private string _EditText = "编辑";
        [System.Web.Script.Serialization.ScriptIgnore]
        public string EditText
        {
            get
            {
                return _EditText;
            }
            set
            {
                _EditText = value;
            }
        }
        [Display(Name = "状态")]
        public string StrStatus { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        public DateTime? DateFrom { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        public DateTime? DateTo { get; set; }

        [Display(Name = "创建人")]
        public string Creater { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        [Display(Name = "创建时间")]
        public DateTime? CreateTime { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        [Display(Name = "创建时间")]
        public string StrCreateTime { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        [Display(Name = "修改人")]
        public string Modifyer { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        public DateTime? ModifyTime { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        public string Auditor { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        public DateTime? AuditorTime { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        public DateTime? RowVersion { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        public int? TerminateReasonID { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        public string TerminateReason { get; set; }

        /// <summary>
        /// 行状态
        /// </summary>
        public int? LineStatus { get; set; }

        private string _DisplayID;
        /// <summary>
        /// 显示ID
        /// </summary>
        [System.Web.Script.Serialization.ScriptIgnore]
        public string DisplayID
        {
            get { return _DisplayID; }
            set { _DisplayID = value; }
        }

        private string _DisplayName;
        /// <summary>
        /// 显示名称
        /// </summary>
        [System.Web.Script.Serialization.ScriptIgnore]
        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value; }
        }
        [System.Web.Script.Serialization.ScriptIgnore]
        public string InitFlag { get; set; }

       [System.Web.Script.Serialization.ScriptIgnore]
        public string StrModifyTime { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        public DateTime? TimeStamp { get; set; }

        [Display(Name = "单据类型")]
        public string StrVoucherType { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        [Display(Name = "创建人")]
        public string StrCreater { get; set; }
        [Display(Name = "单据类型")]
        public int VoucherType { get; set; }
        [Display(Name = "任务号")]
        public string TaskNo { get; set; }

        public string StrLineStatus { get; set; }
        [Display(Name = "物料ID")]
        public int MaterialNoID { get; set; }
        [Display(Name = "物料描述")]
        public string MaterialDoc { get; set; }

        public DateTime? DocDate { get; set; }

        [Display(Name = "提交时间")]
        public DateTime? PostDate { get; set; }

        /// <summary>
        /// 据点编号
        /// </summary>
        [Display(Name = "据点编号")]
        public string StrongHoldCode { get; set; }

        /// <summary>
        /// 据点名称
        /// </summary>
        [Display(Name = "据点名")]
        public string StrongHoldName { get; set; }

        /// <summary>
        /// 企业编号
        /// </summary>
        [Display(Name = "企业编号")]
        public string CompanyCode { get; set; }

        public string ERPCreater { get; set; }

        public DateTime? VouDate { get; set; }
        [Display(Name = "制单人")]
        public string VouUser { get; set; }
        [Display(Name = "部门编号")]
        public string DepartmentCode { get; set; }
        [Display(Name = "部门名")]
        public string DepartmentName { get; set; }

        public string ERPStatus { get; set; }

        public string ERPStatusCode { get; set; }
        [Display(Name = "ERP备注")]
        public string ERPNote { get; set; }

        public int ErpLineStatus { get; set; }


        [Display(Name = "效期")]
        public DateTime EDate { get; set; }

        public string StrEDate { get; set; }

        public int StockType { get; set; }
        [Display(Name = "ERP单号")]
        public string ErpVoucherNo { get; set; }

        /// <summary>
        /// 打印机IP
        /// </summary>
        public string PrintIPAdress { get; set; }

        public string ERPVoucherType { get; set; }

        public string StrModifyer { get; set; }

        public string OperUserNo { get; set; }

        /// <summary>
        /// 物料类别
        /// </summary>
        public string MainTypeCode { get; set; }

        public string GUID { get; set; }

    }


    

    public class BaseMessage_Model<T>
    {

        /// <summary>
        /// 状态 S成功 E 失败
        /// </summary>
        public string HeaderStatus { get; set; }


        /// <summary>
        /// 失败消息
        /// </summary>
        public string Message { get; set; }

        public string MaterialDoc { get; set; }

        public string TaskNo { get; set; }

        public T ModelJson { get; set; }
        
    }
}
