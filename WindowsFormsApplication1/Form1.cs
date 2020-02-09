using BILBasic.User;
using BILWeb.OutStock;
using BILWeb.SyncService;
using BILWeb.TransportSupplier;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strJson = "[{\"lstSerialNo\":null,\"OutStockID\":0.0,\"MaterialNo\":\"10013417\",\"MaterialDesc\":\"21003玛丽佳人多用美目胶水-白胶12ml\",\"RowNo\":\"3\",\"Plant\":null,\"PlantName\":null,\"ToStorageLoc\":null,\"Unit\":\"PCS\",\"UnitName\":null,\"OutStockQty\":2.0,\"OldOutStockQty\":0.0,\"RemainQty\":0.0,\"Costcenter\":null,\"Wbselem\":null,\"FromStorageLoc\":null,\"ReviewStatus\":null,\"CloseOweUser\":null,\"CloseOweDate\":null,\"CloseOweRemark\":null,\"IsOweClose\":null,\"OweRemark\":null,\"OweRemarkUser\":null,\"OweRemarkDate\":null,\"OKSelect\":false,\"VoucherNo\":\"F201908023874\",\"BatchNo\":null,\"IsSerial\":0,\"PartNo\":null,\"RowNoDel\":\"1\",\"StockQty\":0.0,\"lstStock\":null,\"ScanQty\":0.0,\"CustomerCode\":null,\"CustomerName\":null,\"PalletNo\":null,\"FromBatchNo\":\"10002\",\"FromErpAreaNo\":\"1A0101\",\"FromErpWarehouse\":\"MS001\",\"ToBatchno\":\"\",\"ToErpAreaNo\":\"\",\"ToErpWarehouse\":\"\",\"PostUser\":null,\"SourceVoucherNo\":\"\",\"SourceRowNo\":\"\",\"IsSpcBatch\":\"N\",\"StrIsSpcBatch\":\"否\",\"SouStrongHoldCode\":null,\"GoodsValue\":0.0,\"ItemsWeight\":0.0,\"Province\":\"上海市\",\"City\":\"上海市\",\"Area\":\"奉贤区\",\"Address\":\"邬桥镇安东路30号\",\"Address1\":\"\",\"Contact\":\"叶盼盼\",\"Phone\":\"18969350230\",\"ID\":11433,\"HeaderID\":9430,\"OrderNumber\":\"1\",\"Status\":0,\"EditText\":\"编辑\",\"StrStatus\":null,\"DateFrom\":null,\"DateTo\":null,\"Creater\":\"admin\",\"CreateTime\":\"2019-08-02T17:26:22\",\"StrCreateTime\":null,\"Modifyer\":null,\"ModifyTime\":null,\"Auditor\":null,\"AuditorTime\":null,\"RowVersion\":null,\"TerminateReasonID\":null,\"TerminateReason\":null,\"LineStatus\":null,\"DisplayID\":null,\"DisplayName\":null,\"InitFlag\":null,\"StrModifyTime\":null,\"TimeStamp\":null,\"StrVoucherType\":null,\"StrCreater\":null,\"VoucherType\":24,\"TaskNo\":null,\"StrLineStatus\":null,\"MaterialNoID\":2,\"MaterialDoc\":null,\"DocDate\":null,\"PostDate\":null,\"StrongHoldCode\":\"FY2\",\"StrongHoldName\":\"义乌菲扬\",\"CompanyCode\":\"10\",\"ERPCreater\":null,\"VouDate\":null,\"VouUser\":null,\"DepartmentCode\":null,\"DepartmentName\":null,\"ERPStatus\":null,\"ERPStatusCode\":null,\"ERPNote\":null,\"ErpLineStatus\":0,\"EDate\":\"0001-01-01T00:00:00\",\"StrEDate\":null,\"StockType\":0,\"ErpVoucherNo\":\"FY2-HH2-1907310001\",\"PrintIPAdress\":null,\"ERPVoucherType\":\"HH2\",\"StrModifyer\":null,\"OperUserNo\":null,\"MainTypeCode\":null},{\"lstSerialNo\":null,\"OutStockID\":0.0,\"MaterialNo\":\"10013417\",\"MaterialDesc\":\"21003玛丽佳人多用美目胶水-白胶12ml\",\"RowNo\":\"2\",\"Plant\":null,\"PlantName\":null,\"ToStorageLoc\":null,\"Unit\":\"PCS\",\"UnitName\":null,\"OutStockQty\":12.0,\"OldOutStockQty\":0.0,\"RemainQty\":0.0,\"Costcenter\":null,\"Wbselem\":null,\"FromStorageLoc\":null,\"ReviewStatus\":null,\"CloseOweUser\":null,\"CloseOweDate\":null,\"CloseOweRemark\":null,\"IsOweClose\":null,\"OweRemark\":null,\"OweRemarkUser\":null,\"OweRemarkDate\":null,\"OKSelect\":false,\"VoucherNo\":\"F201908023874\",\"BatchNo\":null,\"IsSerial\":0,\"PartNo\":null,\"RowNoDel\":\"1\",\"StockQty\":0.0,\"lstStock\":null,\"ScanQty\":0.0,\"CustomerCode\":null,\"CustomerName\":null,\"PalletNo\":null,\"FromBatchNo\":\"201904260001\",\"FromErpAreaNo\":\"2C06010201\",\"FromErpWarehouse\":\"MS001\",\"ToBatchno\":\"\",\"ToErpAreaNo\":\"\",\"ToErpWarehouse\":\"\",\"PostUser\":null,\"SourceVoucherNo\":\"\",\"SourceRowNo\":\"\",\"IsSpcBatch\":\"N\",\"StrIsSpcBatch\":\"否\",\"SouStrongHoldCode\":null,\"GoodsValue\":0.0,\"ItemsWeight\":0.0,\"Province\":\"上海市\",\"City\":\"上海市\",\"Area\":\"奉贤区\",\"Address\":\"邬桥镇安东路30号\",\"Address1\":\"\",\"Contact\":\"叶盼盼\",\"Phone\":\"18969350230\",\"ID\":11432,\"HeaderID\":9430,\"OrderNumber\":\"2\",\"Status\":0,\"EditText\":\"编辑\",\"StrStatus\":null,\"DateFrom\":null,\"DateTo\":null,\"Creater\":\"admin\",\"CreateTime\":\"2019-08-02T17:26:22\",\"StrCreateTime\":null,\"Modifyer\":null,\"ModifyTime\":null,\"Auditor\":null,\"AuditorTime\":null,\"RowVersion\":null,\"TerminateReasonID\":null,\"TerminateReason\":null,\"LineStatus\":null,\"DisplayID\":null,\"DisplayName\":null,\"InitFlag\":null,\"StrModifyTime\":null,\"TimeStamp\":null,\"StrVoucherType\":null,\"StrCreater\":null,\"VoucherType\":24,\"TaskNo\":null,\"StrLineStatus\":null,\"MaterialNoID\":2,\"MaterialDoc\":null,\"DocDate\":null,\"PostDate\":null,\"StrongHoldCode\":\"FY2\",\"StrongHoldName\":\"义乌菲扬\",\"CompanyCode\":\"10\",\"ERPCreater\":null,\"VouDate\":null,\"VouUser\":null,\"DepartmentCode\":null,\"DepartmentName\":null,\"ERPStatus\":null,\"ERPStatusCode\":null,\"ERPNote\":null,\"ErpLineStatus\":0,\"EDate\":\"0001-01-01T00:00:00\",\"StrEDate\":null,\"StockType\":0,\"ErpVoucherNo\":\"FY2-HH2-1907310001\",\"PrintIPAdress\":null,\"ERPVoucherType\":\"HH2\",\"StrModifyer\":null,\"OperUserNo\":null,\"MainTypeCode\":null},{\"lstSerialNo\":null,\"OutStockID\":0.0,\"MaterialNo\":\"10013417\",\"MaterialDesc\":\"21003玛丽佳人多用美目胶水-白胶12ml\",\"RowNo\":\"1\",\"Plant\":null,\"PlantName\":null,\"ToStorageLoc\":null,\"Unit\":\"PCS\",\"UnitName\":null,\"OutStockQty\":3.0,\"OldOutStockQty\":0.0,\"RemainQty\":0.0,\"Costcenter\":null,\"Wbselem\":null,\"FromStorageLoc\":null,\"ReviewStatus\":null,\"CloseOweUser\":null,\"CloseOweDate\":null,\"CloseOweRemark\":null,\"IsOweClose\":null,\"OweRemark\":null,\"OweRemarkUser\":null,\"OweRemarkDate\":null,\"OKSelect\":false,\"VoucherNo\":\"F201908023874\",\"BatchNo\":null,\"IsSerial\":0,\"PartNo\":null,\"RowNoDel\":\"1\",\"StockQty\":0.0,\"lstStock\":null,\"ScanQty\":0.0,\"CustomerCode\":null,\"CustomerName\":null,\"PalletNo\":null,\"FromBatchNo\":\"201903210013\",\"FromErpAreaNo\":\"\",\"FromErpWarehouse\":\"MS001\",\"ToBatchno\":\"\",\"ToErpAreaNo\":\"\",\"ToErpWarehouse\":\"\",\"PostUser\":null,\"SourceVoucherNo\":\"\",\"SourceRowNo\":\"\",\"IsSpcBatch\":\"N\",\"StrIsSpcBatch\":\"否\",\"SouStrongHoldCode\":null,\"GoodsValue\":0.0,\"ItemsWeight\":0.0,\"Province\":\"上海市\",\"City\":\"上海市\",\"Area\":\"奉贤区\",\"Address\":\"邬桥镇安东路30号\",\"Address1\":\"\",\"Contact\":\"叶盼盼\",\"Phone\":\"18969350230\",\"ID\":11431,\"HeaderID\":9430,\"OrderNumber\":\"3\",\"Status\":0,\"EditText\":\"编辑\",\"StrStatus\":null,\"DateFrom\":null,\"DateTo\":null,\"Creater\":\"admin\",\"CreateTime\":\"2019-08-02T17:26:22\",\"StrCreateTime\":null,\"Modifyer\":null,\"ModifyTime\":null,\"Auditor\":null,\"AuditorTime\":null,\"RowVersion\":null,\"TerminateReasonID\":null,\"TerminateReason\":null,\"LineStatus\":null,\"DisplayID\":null,\"DisplayName\":null,\"InitFlag\":null,\"StrModifyTime\":null,\"TimeStamp\":null,\"StrVoucherType\":null,\"StrCreater\":null,\"VoucherType\":24,\"TaskNo\":null,\"StrLineStatus\":null,\"MaterialNoID\":2,\"MaterialDoc\":null,\"DocDate\":null,\"PostDate\":null,\"StrongHoldCode\":\"FY2\",\"StrongHoldName\":\"义乌菲扬\",\"CompanyCode\":\"10\",\"ERPCreater\":null,\"VouDate\":null,\"VouUser\":null,\"DepartmentCode\":null,\"DepartmentName\":null,\"ERPStatus\":null,\"ERPStatusCode\":null,\"ERPNote\":null,\"ErpLineStatus\":0,\"EDate\":\"0001-01-01T00:00:00\",\"StrEDate\":null,\"StockType\":0,\"ErpVoucherNo\":\"FY2-HH2-1907310001\",\"PrintIPAdress\":null,\"ERPVoucherType\":\"HH2\",\"StrModifyer\":null,\"OperUserNo\":null,\"MainTypeCode\":null},{\"lstSerialNo\":null,\"OutStockID\":0.0,\"MaterialNo\":\"10013418\",\"MaterialDesc\":\"21004玛丽佳人多用美目胶水-黑胶12ml\",\"RowNo\":\"4\",\"Plant\":null,\"PlantName\":null,\"ToStorageLoc\":null,\"Unit\":\"PCS\",\"UnitName\":null,\"OutStockQty\":50.0,\"OldOutStockQty\":0.0,\"RemainQty\":0.0,\"Costcenter\":null,\"Wbselem\":null,\"FromStorageLoc\":null,\"ReviewStatus\":null,\"CloseOweUser\":null,\"CloseOweDate\":null,\"CloseOweRemark\":null,\"IsOweClose\":null,\"OweRemark\":null,\"OweRemarkUser\":null,\"OweRemarkDate\":null,\"OKSelect\":false,\"VoucherNo\":\"F201908023874\",\"BatchNo\":null,\"IsSerial\":0,\"PartNo\":null,\"RowNoDel\":\"1\",\"StockQty\":0.0,\"lstStock\":null,\"ScanQty\":0.0,\"CustomerCode\":null,\"CustomerName\":null,\"PalletNo\":null,\"FromBatchNo\":\"201811300175\",\"FromErpAreaNo\":\"1A0301\",\"FromErpWarehouse\":\"MS001\",\"ToBatchno\":\"\",\"ToErpAreaNo\":\"\",\"ToErpWarehouse\":\"\",\"PostUser\":null,\"SourceVoucherNo\":\"\",\"SourceRowNo\":\"\",\"IsSpcBatch\":\"N\",\"StrIsSpcBatch\":\"否\",\"SouStrongHoldCode\":null,\"GoodsValue\":0.0,\"ItemsWeight\":0.0,\"Province\":\"上海市\",\"City\":\"上海市\",\"Area\":\"奉贤区\",\"Address\":\"邬桥镇安东路30号\",\"Address1\":\"\",\"Contact\":\"叶盼盼\",\"Phone\":\"18969350230\",\"ID\":11434,\"HeaderID\":9430,\"OrderNumber\":\"4\",\"Status\":0,\"EditText\":\"编辑\",\"StrStatus\":null,\"DateFrom\":null,\"DateTo\":null,\"Creater\":\"admin\",\"CreateTime\":\"2019-08-02T17:26:22\",\"StrCreateTime\":null,\"Modifyer\":null,\"ModifyTime\":null,\"Auditor\":null,\"AuditorTime\":null,\"RowVersion\":null,\"TerminateReasonID\":null,\"TerminateReason\":null,\"LineStatus\":null,\"DisplayID\":null,\"DisplayName\":null,\"InitFlag\":null,\"StrModifyTime\":null,\"TimeStamp\":null,\"StrVoucherType\":null,\"StrCreater\":null,\"VoucherType\":24,\"TaskNo\":null,\"StrLineStatus\":null,\"MaterialNoID\":3,\"MaterialDoc\":null,\"DocDate\":null,\"PostDate\":null,\"StrongHoldCode\":\"FY2\",\"StrongHoldName\":\"义乌菲扬\",\"CompanyCode\":\"10\",\"ERPCreater\":null,\"VouDate\":null,\"VouUser\":null,\"DepartmentCode\":null,\"DepartmentName\":null,\"ERPStatus\":null,\"ERPStatusCode\":null,\"ERPNote\":null,\"ErpLineStatus\":0,\"EDate\":\"0001-01-01T00:00:00\",\"StrEDate\":null,\"StockType\":0,\"ErpVoucherNo\":\"FY2-HH2-1907310001\",\"PrintIPAdress\":null,\"ERPVoucherType\":\"HH2\",\"StrModifyer\":null,\"OperUserNo\":null,\"MainTypeCode\":null}]";
            EMSDLL.EMSDLL emsdll = new EMSDLL.EMSDLL();
            string strResult = emsdll.PostOutStockToEms(strJson);
            MessageBox.Show(strResult);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ErrorMsg = string.Empty;
            BILWeb.SyncService.ParamaterField_Func tfunc = new ParamaterField_Func();
            tfunc.Sync(99, "2019-04-11", "106394002B", -1, ref ErrorMsg, "ERP", -1, null);
            
            //PFunc.Sync(99, "2019-08-11", string.Empty, -1, ref ErrorMsg, "ERP", -1, null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string ErrorMsg = string.Empty;
            BILWeb.SyncService.ParamaterField_Func tfunc = new ParamaterField_Func();
            tfunc.Sync(10, "2019-04-11", "FY2-CG1-1908160001", -1, ref ErrorMsg, "ERP", -1, null);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string ErrorMsg = string.Empty;
            BILWeb.SyncService.ParamaterField_Func tfunc = new ParamaterField_Func();
            tfunc.Sync(10, "2019-04-11", "FY2-XT3-1908190001", -1, ref ErrorMsg, "ERP", -1, null);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string ErrorMsg = string.Empty;
            BILWeb.SyncService.ParamaterField_Func tfunc = new ParamaterField_Func();
            tfunc.Sync(10, "2019-04-11", "FY2-ZSD-1908160001", -1, ref ErrorMsg, "ERP", -1, null);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string ErrorMsg = string.Empty;
            BILWeb.SyncService.ParamaterField_Func tfunc = new ParamaterField_Func();
            tfunc.Sync(20, "2019-04-11", "FY2-CT9-1908080001", -1, ref ErrorMsg, "ERP", -1, null);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string ErrorMsg = string.Empty;
            BILWeb.SyncService.ParamaterField_Func tfunc = new ParamaterField_Func();
            tfunc.Sync(20, "2019-04-11", "FY2-ZF9-1907310001", -1, ref ErrorMsg, "ERP", -1, null);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string ErrorMsg = string.Empty;
            BILWeb.SyncService.ParamaterField_Func tfunc = new ParamaterField_Func();
            tfunc.Sync(20, "2019-04-11", "HM1-HH2-1908310001", -1, ref ErrorMsg, "ERP", -1, null);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string ErrorMsg = string.Empty;
            BILWeb.SyncService.ParamaterField_Func tfunc = new ParamaterField_Func();
            tfunc.Sync(10, "2019-04-11", "FY2-DB6-1908160002", -1, ref ErrorMsg, "ERP", -1, null);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string ErrorMsg = string.Empty;
            BILWeb.SyncService.ParamaterField_Func tfunc = new ParamaterField_Func();
            tfunc.Sync(20, "2019-04-11", "FY2-DB6-1909010012", -1, ref ErrorMsg, "ERP", -1, null);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            T_TransportSupDetail_Func tfunc = new T_TransportSupDetail_Func();
            tfunc.SaveTransportSupplierListADF("[{\"BoxCount\":\"16\",\"Creater\":\"admin\",\"ErpVoucherNo\":\"FY2-HH2-1909010013\",\"Feight\":\"599\",\"ID\":141,\"IsDel\":0,\"PalletNo\":\"P201909091577\",\"PlateNumber\":\"123456\",\"TradingConditionsCode\":\"MS13\",\"Type\":\"2\",\"VoucherNo\":\"987654321\",\"GUID\":\"8a6c9535-0028-4480-bfd6-6f56bc0eae38\"}]");
        
        }

        private void button12_Click(object sender, EventArgs e)
        {
            UserModel user = new UserModel();
            user.UserNo="admin";
            user.UserName = "管理员";
            string strError = string.Empty;
            T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
            T_OutStockInfo model = new T_OutStockInfo();
            tfunc.CreateOutStockByEmsLabel("7f21b00f-f24f-4d95-bed4-eca1bc45d2ff", "FY2-HH2-1908260002", 1, 1.444M, user,ref model, ref strError);
        }
    }
}
