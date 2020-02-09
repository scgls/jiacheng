using BILWeb.InStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BILWeb.Stock;
using BILWeb.InStockTask;
using BILWeb.OutStockTask;
using BILWeb.Material;
using BILWeb.Login.User;
using BILWeb.Pallet;
using BILWeb.Area;
//using BILWeb.Query;
using BILWeb.Login;
using System.ServiceModel.Activation;
using BILWeb.OutBarCode;
using BILWeb.Quality;
//using BILWeb.Print;
using BILWeb.OutStock;
using BILWeb.AdvInStock;
using BILWeb.Move;
using BILWeb.TransportSupplier;
using BILWeb.MoveStockTask;
using BILWeb.LandMark;
using BILWeb.Query;
using BILWeb.Box;

namespace SCCGAndroidService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]

    public class AndroidService : IAndroidService
    {

        #region 用户登录模块
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="UserJson"></param>
        /// <returns></returns>
        public string UserLoginADF(string UserJson)
        {
            User_Func tfunc = new User_Func();
            return tfunc.UserLoginADF(UserJson);
        }
        #endregion

        #region 预收货
        public string GetT_MaterialPackADF(string UserJson, string ModelJson)
        {
            MaterialPack_Func func = new MaterialPack_Func();
            return func.GetModelListADF(UserJson, ModelJson);

        }

        public string Post_SaveAdvInStock(string UserJson, string advInStock)
        {
            T_AdvInStock_Func func = new T_AdvInStock_Func();
            // return func.SaveModelListSqlToDBADF(UserJson, advInStock);
            return func.GetT_MaterialPackADF(advInStock);
        }

        public string Get_AdvInParameter(string groupname)
        {
            T_AdvInStock_Func func = new T_AdvInStock_Func();
            return func.Get_AdvInParameter(groupname);
        }

        #endregion

        #region 根据用户编码获取仓库信息

        /// <summary>
        /// 根据用户编码获取仓库列表
        /// </summary>
        /// <param name="UserNo"></param>
        /// <returns></returns>
        public string GetWareHouseByUserADF(string UserNo)
        {
            User_Func tfun = new User_Func();
            return tfun.GetWareHouseByUserADF(UserNo);
        }

        #endregion

        #region 托盘和装箱模块

        /// <summary>
        /// 根据托盘编号或者条码获取托盘明细
        /// </summary>
        /// <param name="PalletNo"></param>
        /// <returns></returns>
        public string GetT_PalletDetailByNoADF(string BarCode, string PalletModel)
        {
            T_PalletDetail_Func tfunc = new T_PalletDetail_Func();
            return tfunc.GetPalletDetailByPalletNo(BarCode, PalletModel);
        }

        /// <summary>
        /// 获取托盘明细
        /// </summary>
        /// <param name="VoucherNo"></param>
        /// <returns></returns>
        public string Get_PalletDetailByVoucherNo(String VoucherNo)
        {
            T_PalletDetail_Func tfunc = new T_PalletDetail_Func();
            return tfunc.GetPalletDetailByVoucherNo(VoucherNo);
        }

        /// <summary>
        /// 删除托盘或者托盘序列号
        /// </summary>
        /// <param name="PalletNo"></param>
        /// <param name="SerialNo"></param>
        /// <returns></returns>
        public string Del_PalletOrSerialNo(String PalletNo, String SerialNo)
        {
            T_PalletDetail_Func tfunc = new T_PalletDetail_Func();
            return tfunc.Del_PalletOrSerialNo(PalletNo, SerialNo);
        }

        /// <summary>
        /// 根据ERP单据号删除托盘数据
        /// </summary>
        /// <param name="ErpVoucherNo"></param>
        /// <param name="PalletType"></param>
        /// <returns></returns>
        public string DeletePalletByErpVoucherNo(string ErpVoucherNo, string PalletType)
        {
            T_PalletDetail_Func tfunc = new T_PalletDetail_Func();
            return tfunc.DeletePalletByErpVoucherNo(ErpVoucherNo, PalletType);
        }

        /// <summary>
        /// 提交托盘信息
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="ModelJson"></param>
        /// <returns></returns>
        public string SaveT_PalletDetailADF(string UserJson, string ModelJson)
        {
            T_PalletDetail_Func tfunc = new T_PalletDetail_Func();
            return tfunc.SaveModelListSqlToDBADF(UserJson, ModelJson);
        }

        /// <summary>
        /// 提交装箱或者拆箱数据
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="strOldBarCode"></param>
        /// <param name="strNewBarCode"></param>
        /// <param name="PrintFlag">打印标记1-打印 2-不打印</param>
        /// <returns></returns>
        public string SaveT_BarCodeToStockADF(string UserJson, string strOldBarCode, string strNewBarCode, string PrintFlag)
        {
            T_PalletDetail_Func tfunc = new T_PalletDetail_Func();
            return tfunc.SaveBarCodeToStock(UserJson, strOldBarCode, strNewBarCode, PrintFlag);
        }

        /// <summary>
        /// 删除整托或者单个托上的箱条码
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="PalletDetailJson"></param>
        /// <returns></returns>
        public string Delete_PalletORBarCodeADF(string UserJson, string PalletDetailJson)
        {
            T_PalletDetail_Func tfunc = new T_PalletDetail_Func();
            return tfunc.DeletePalletORBarCode(UserJson, PalletDetailJson);
        }

        public string GetMessageForPrint(string filter, string flag)
        {
            T_Box_Func tfunc = new T_Box_Func();
            return tfunc.GetMessageForPrint( filter,  flag);
        }

        /// <summary>
        /// 获取整箱库存信息
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public string GetT_OutBarCodeInfoByBoxADF(string BarCode)
        {
            T_PalletDetail_Func tfunc = new T_PalletDetail_Func();
            return tfunc.GetOutBarCodeInfoByBox(BarCode);
        }

        #endregion

        #region 收货模块

        /// <summary>
        /// 获取入库单据，构造状态1
        /// </summary>
        /// <param name="UserJosn"></param>
        /// <param name="ModelJson"></param>
        /// <returns></returns>
        public string GetT_InStockListADF(string UserJson, string ModelJson)
        {
            T_InStock_Func tfunc = new T_InStock_Func();
            return tfunc.GetModelListADF(UserJson, ModelJson);
        }

        /// <summary>
        /// 收货获取条码信息(不支持托盘，只是单个条码信息)
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <returns></returns>
        public string GetT_SerialNoADF(string SerialNo)
        {
            //T_SerialNo_Func tfun = new T_SerialNo_Func();
            //return tfun.CheckSerialNo(SerialNo);
            T_OutBarCode_Func tfunc = new T_OutBarCode_Func();
            return tfunc.GetOutBarCodeInfo(SerialNo);
        }

        /// <summary>
        /// 检查条码（包材接收）
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <returns></returns>
        public string GetT_SerialNobyymhADF(string SerialNo)
        {
            //T_SerialNo_Func tfun = new T_SerialNo_Func();
            //return tfun.CheckSerialNo(SerialNo);
            T_OutBarCode_Func tfunc = new T_OutBarCode_Func();
            return tfunc.GetOutBarCodeInfobyymh(SerialNo);
        }


        /// <summary>
        /// 收货获取条码信息，支持扫描物料条码或者托盘条码
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <returns></returns>
        public string GetT_PalletDetailByBarCodeADF(string UserJson, string BarCode)
        {
            T_InStockDetail_Func tfunc = new T_InStockDetail_Func();
            return tfunc.GetPalletDetailByBarCode(UserJson, BarCode);
        }


        /// <summary>
        /// 根据收货单据ID获取收货明细
        /// </summary>
        /// <param name="ModelDetailJson"></param>
        /// <returns></returns>
        public string GetT_InStockDetailListByHeaderIDADF(string ModelDetailJson)
        {
            T_InStockDetail_Func tfunc = new T_InStockDetail_Func();
            return tfunc.GetModelListByHeaderIDADF(ModelDetailJson);
        }

        /// <summary>
        /// 提交收货明细
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="ModelJson"></param>
        /// <returns></returns>
        public string SaveT_InStockDetailADF(string UserJson, string ModelJson)
        {
            T_InStockDetail_Func tfunc = new T_InStockDetail_Func();

            return tfunc.SaveModelListSqlToDBADF(UserJson, ModelJson);

        }


        #endregion

        #region 移库
        /// <summary>
        /// 获取货位库存条码列表
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="ModelJson"></param>
        /// <returns></returns>
        public string GetT_StockInfoADF(string UserJson, string ModelJson)
        {
            T_Stock_Func func = new T_Stock_Func();
            return func.GetModelListADF(UserJson, ModelJson);
        }
        /// <summary>
        /// 获取库位信息
        /// </summary>
        /// <param name="areaNO"></param>
        /// <returns></returns>
        public string GetT_AreaNOInfoADF(string areaNO)
        {
            T_Area_Func func = new T_Area_Func();
            return func.GetAreaModelBySql(areaNO);
        }
        /// <summary>
        /// 获取指定仓库货位信息
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="areaNO"></param>
        /// <returns></returns>
        public string GetT_AreaInfoADF(string UserJson, string areaNO)
        {
            T_Area_Func func = new T_Area_Func();
            return func.GetAreaModelBySqlADF(UserJson, areaNO);
        }

        public string Save_MoveInfoADF(string UserJson, string StockJson, string AreaJson)
        {
            T_Stock_Func func = new T_Stock_Func();
            return func.saveMoveBarcode(UserJson, StockJson, AreaJson);
        }


        #endregion

        #region 上架模块

        /// <summary>
        /// 上架获取序列号信息,此方法暂时未用
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <returns></returns>
        public string GetT_SerialNoInStockADF(string SerialNo)
        {
            T_SerialNo_Func tfun = new T_SerialNo_Func();
            return tfun.CheckSerialNoInStock(SerialNo);
        }

        /// <summary>
        /// 获取上架任务,需要构造status = 1 
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="ModelJson"></param>
        /// <returns></returns>
        public string GetT_InTaskListADF(string UserJson, string ModelJson)
        {
            T_InStockTask_Func tfunc = new T_InStockTask_Func();
            return tfunc.GetModelListADF(UserJson, ModelJson);
        }


        /// <summary>
        /// 上架扫描序列号，有托盘返回托盘关联物料总数，没有托盘返回单个库存对象
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <returns></returns>
        public string GetInStockModelADF(string SerialNo, string VoucherNo, string TaskNo)
        {
            T_Stock_Func tfunc = new T_Stock_Func();
            return tfunc.GetInStockModelADF(SerialNo, VoucherNo, TaskNo);
        }

        /// <summary>
        /// 上架扫描条码或者托盘条码
        /// </summary>
        /// <param name="BarCode"></param>
        /// <param name="ERPVoucherNo"></param>
        /// <param name="TaskNo"></param>
        /// <param name="AreaNo"></param>
        /// <returns></returns>
        public string GetT_ScanInStockModelADF(string BarCode, string ERPVoucherNo, string TaskNo, string AreaNo, int WareHouseID)
        {
            T_Stock_Func tfunc = new T_Stock_Func();
            return tfunc.GetScanInStockModelADF(BarCode, ERPVoucherNo, TaskNo, AreaNo, WareHouseID);
        }


        /// <summary>
        /// 根据选择的任务号ID获取上架任务明细
        /// </summary>
        /// <param name="ModelDetailJson"></param>
        /// <returns></returns>
        public string GetT_InTaskDetailListByHeaderIDADF(string ModelDetailJson)
        {
            T_InTaskDetails_Func tfunc = new T_InTaskDetails_Func();
            return tfunc.GetModelListByHeaderIDADF(ModelDetailJson);
        }

        /// <summary>
        /// 上架扫描库位
        /// </summary>
        /// <param name="AreaNo"></param>
        /// <returns></returns>
        public string GetAreaModelADF(string UserJson, string AreaNo)
        {
            T_Area_Func tfunc = new T_Area_Func();
            return tfunc.GetAreaModelBySqlADF(UserJson, AreaNo);
        }

        /// <summary>
        /// 选择上架行的时候锁定操作人
        /// </summary>
        /// <param name="TaskDetailsJson"></param>
        /// <param name="UserJson"></param>
        /// <returns></returns>
        public string LockTaskOperUserADF(string TaskDetailsJson, string UserJson)
        {
            T_InTaskDetails_Func tfunc = new T_InTaskDetails_Func();
            return tfunc.LockTaskOperUser(TaskDetailsJson, UserJson);
        }

        /// <summary>
        /// 退出上架界面的时候解锁上架人
        /// </summary>
        /// <param name="TaskDetailsJson"></param>
        /// <param name="UserJson"></param>
        /// <returns></returns>
        public string UnLockTaskOperUserADF(string TaskDetailsJson, string UserJson)
        {
            T_InTaskDetails_Func tfunc = new T_InTaskDetails_Func();
            return tfunc.UnLockTaskOperUser(TaskDetailsJson, UserJson);
        }


        /// <summary>
        /// 保存上架扫描数据
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="ModelJson"></param>
        /// <returns></returns>
        public string SaveT_InStockTaskDetailADF(string UserJson, string ModelJson)
        {
            T_InTaskDetails_Func tfunc = new T_InTaskDetails_Func();
            return tfunc.SaveModelListSqlToDBADF(UserJson, ModelJson);
        }

        #endregion

        #region 补货
        public string GetT_MoveTaskInfo(string UserJson, string ModelJson)
        {
            T_MoveDetail_Func func = new T_MoveDetail_Func();
            return func.GetModelListADF(UserJson, ModelJson);
        }

        public string Save_MoveTask(string UserJson, string ModelJson)
        {
            T_MoveDetail_Func func = new T_MoveDetail_Func();
            return func.SaveModelListSqlToDBADF(UserJson, ModelJson);
        }


        #endregion

        #region 拣货单补货

        public string GetT_MoveTaskScatInfo(string UserJson, string ModelJson)
        {
            MoveStockTaskDetail_Func tfunc = new MoveStockTaskDetail_Func();
            return tfunc.GetModelListADF(UserJson, ModelJson);
        }

        public string Save_MoveTaskScat(string UserJson, string ModelJson)
        {
            MoveStockTaskDetail_Func func = new MoveStockTaskDetail_Func();
            return func.SaveModelListSqlToDBADF(UserJson, ModelJson);
        }

        #endregion

        #region 拣货模块

        /// <summary>
        /// 获取下架任务列表
        /// </summary>
        /// <param name="UserJosn"></param>
        /// <param name="ModelJson"></param>
        /// <returns></returns>
        public string GetT_OutTaskListADF(string UserJson, string ModelJson)
        {
            T_OutStockTask_Func tfunc = new T_OutStockTask_Func();
            return tfunc.GetModelListADF(UserJson, ModelJson);
        }

        /// <summary>
        /// 根据ID获取下架物料
        /// </summary>
        /// <param name="ModelDetailJson"></param>
        /// <returns></returns>
        public string GetT_OutTaskDetailListByHeaderIDADF(string ModelDetailJson)
        {
            T_OutTaskDetails_Func tfunc = new T_OutTaskDetails_Func();
            return tfunc.GetOutTaskDetailListByHeaderIDADF(ModelDetailJson);
        }

        /// <summary>
        /// 扫描序列号或者ENA条码
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <returns></returns>
        public string GetStockModelADF(string ModelStockJson)
        {
            T_Stock_Func tfunc = new T_Stock_Func();
            return tfunc.GetStockModelBySql(ModelStockJson);
        }

        /// <summary>
        /// 保存下架扫描数据
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="ModelJson"></param>
        /// <returns></returns>
        public string SaveT_OutStockTaskDetailADF(string UserJson, string ModelJson)
        {
            T_OutTaskDetails_Func tfunc = new T_OutTaskDetails_Func();
            return tfunc.SaveModelListSqlToDBADF(UserJson, ModelJson);
        }

        /// <summary>
        /// 仓库内移库提交数据
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="ModelJson"></param>
        /// <returns></returns>
        public string SaveT_StockADF(string UserJson, string ModelJson)
        {
            T_Stock_Func tfunc = new T_Stock_Func();
            return tfunc.SaveModelListSqlToDBADF(UserJson, ModelJson);
        }

        /// <summary>
        /// 扫描绑定货车条码
        /// </summary>
        /// <param name="CarNo"></param>
        /// <param name="TaskNo"></param>
        /// <returns></returns>
        public string GetCarModelADF(string CarNo, string TaskNo, string strUserNo)
        {
            T_OutTaskDetails_Func tfunc = new T_OutTaskDetails_Func();
            return tfunc.GetCarModelADF(CarNo, TaskNo, strUserNo);
        }

        public string SaveBoxListADF(string UserJson, string ModelJson) 
        {
            T_OutTaskDetails_Func tfunc = new T_OutTaskDetails_Func();
            return tfunc.SaveBoxListADF(UserJson, ModelJson);
        }

        public string SaveCobBoxListADF(string UserJson, string ModelJson)
        {
            T_Box_Func tfunc = new T_Box_Func();
            return tfunc.SaveCobBoxListADF(UserJson, ModelJson);
        }

        public string ScanBoxSerial(string strSerialNo) 
        {
            T_Box_Func tfunc = new T_Box_Func();
            return tfunc.ScanBoxSerial(strSerialNo);
        }

        #endregion

        #region 拣货锁定和解锁任务

        public string LockTaskOperUser(string TaskOutStockModelJson, string UserJson) 
        {
            T_OutStockTask_Func tfunc = new T_OutStockTask_Func();
            return tfunc.LockTaskOperUser(TaskOutStockModelJson, UserJson);
        }

        public string UnLockTaskOperUser(string TaskOutStockModelJson, string UserJson) 
        {
            T_OutStockTask_Func tfunc = new T_OutStockTask_Func();
            return tfunc.UnLockTaskOperUser(TaskOutStockModelJson, UserJson);
        }

        #endregion

        #region 复核模块
        public string ScanOutStockReviewByBarCodeADF(string BarCode)
        {
            T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
            return tfunc.ScanOutStockReviewByBarCode(BarCode);
        }


        public string GetT_OutStockReviewListADF(string UserJson, string ModelJson)
        {
            T_OutStock_Func tfunc = new T_OutStock_Func();
            return tfunc.GetModelListADF(UserJson, ModelJson);
        }

        public string GetT_OutStockReviewDetailListByHeaderIDADF(string ModelDetailJson)
        {
            T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
            return tfunc.GetT_OutStockReviewDetailListByHeaderIDADF(ModelDetailJson);
            //return tfunc.GetModelListByHeaderIDADF(ModelDetailJson);
        }

        /// <summary>
        /// 复核扫描条码
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public string GetReviewStockModelADF(string ModelStockJson)
        {
            T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
            return tfunc.GetReviewStockModelADF(ModelStockJson);
        }

        /// <summary>
        /// 保存复核数据
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="ModelJson"></param>
        /// <returns></returns>
        public string SaveT_OutStockReviewDetailADF(string UserJson, string ModelJson)
        {
            T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
            return tfunc.SaveT_OutStockReviewDetailADF(UserJson, ModelJson);
        }

        /// <summary>
        /// 复核过账
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="ErpVoucherNo"></param>
        /// <returns></returns>
        public string PostT_OutStockReviewDetailADF(string UserJson, string ErpVoucherNo) 
        {
            T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
            return tfunc.PostT_OutStockReviewDetailADF(UserJson, ErpVoucherNo);
        }

        #endregion

        #region 盘点
        public string GetCheck()
        {
            Check_DB db = new Check_DB();
            return db.GetCheck();
        }

        public string GetScanInfo(string barcode)
        {
            Check_DB db = new Check_DB();
            return db.GetScanInfo(barcode);
        }
        public string CheckSerialno(string EAN, string areaid, string batchno, string materialno)
        {
            T_Stock_Func stock_Func = new T_Stock_Func();
            return stock_Func.CheckSerialno(EAN, areaid, batchno, materialno);
        }

        public string CheckGetBatchnoAndMaterialno(string EAN, string areaid)
        {
            T_Stock_Func stock_Func = new T_Stock_Func();
            return stock_Func.CheckGetBatchnoAndMaterialno(EAN, areaid);
        }

        //public string GetCheckMing()
        //{
        //    Check_DB db = new Check_DB();
        //    return db.GetCheckMing();
        //}

        //public string GetMinDetail(string checkno)
        //{
        //    Check_DB db = new Check_DB();
        //    return db.GetMinDetail(checkno);
        //}

        //public string GetMinBarocde(string barcode, string checkno)
        //{
        //    Check_DB db = new Check_DB();
        //    return db.GetMinBarocde(barcode, checkno);
        //}

        public string GetMinSerialno(int id)
        {
            Check_DB db = new Check_DB();
            return db.GetMinSerialno(id);
        }
        public string DeleteCheckDetail(string checkno, string json)
        {
            Check_DB db = new Check_DB();
            return db.DeleteCheckDetail(checkno, json);
        }

        //public string SummitMin(string checkno)
        //{
        //    Check_DB db = new Check_DB();
        //    return db.SummitMin(checkno);
        //}

        public string GetAreanobyCheckno(string checkno)
        {
            Check_DB db = new Check_DB();
            return db.GetAreanobyCheckno(checkno);
        }

        public string GetAreanobyCheckno2(string checkno, string areano)
        {
            Check_DB db = new Check_DB();
            return db.GetAreanobyCheckno2(checkno, areano);
        }

        public string InsertCheckDetail(string json)
        {
            Check_DB db = new Check_DB();
            return db.InsertCheckDetail(json);
        }

        public string GetCheckDetail(string checkno)
        {
            Check_DB db = new Check_DB();
            return db.GetCheckDetail(checkno);
        }
        //库存调整
        public string SaveInfo(string json, string man)
        {
            Check_DB db = new Check_DB();
            return db.SaveInfo(json, man);
        }
        //库存调整
        public string GetInfoBySerialymh(string barcode)
        {
            Check_DB db = new Check_DB();
            return db.GetInfoBySerial(barcode);
        }
        #endregion

        #region PDA查询
        /// <summary>
        /// 根据物料编号获取库存信息
        /// 根据scantype查询不同条件的库存
        /// </summary>
        /// <param name="MaterialNo"></param>
        /// <returns></returns>
        public string GetStockByMaterialNoADF(string UserJson, string MaterialNo, string ScanType)
        {
            T_Stock_Func tfunc = new T_Stock_Func();
            return tfunc.GetStockInfoByScanType(UserJson,MaterialNo, ScanType);
        }
        #endregion

        public string GetInfoBySerial(string EAN, string areaid, string batchno, string materialno)
        {
            T_Stock_Func func = new T_Stock_Func();
            return func.OffSerialno(EAN, areaid, batchno, materialno);
        }

        public string GetWareHouse()
        {
            Check_DB db = new Check_DB();
            return db.GetWareHouse();
        }

        #region ymh装车卸车

        /// <summary>
        /// 根据托盘条码获取托盘明细
        /// </summary>
        /// <returns></returns>
        public string GetPalletInfoByPalletNo(string PalletNo)
        {
            T_PalletDetail_Func func = new T_PalletDetail_Func();
            return func.GetPalletInfoByPalletNo(PalletNo);
        }

        //public string GetTransportSupplierListADF()
        //{
        //    T_TransportSupplier_Func func = new T_TransportSupplier_Func();
        //    return func.GetTransportSupplierList();
        //}

        public string GetTransportSupplierDetailListADF(string PalletNo)
        {
            T_TransportSupDetail_Func func = new T_TransportSupDetail_Func();
            return func.GetTransportSupplierDetailList(PalletNo);
        }

        public string SaveTransportSupplierListADF(string ModelJson)
        {
            T_TransportSupDetail_Func tfunc = new T_TransportSupDetail_Func();
            return tfunc.SaveTransportSupplierListADF(ModelJson);
        }
        #endregion

        #region 地标模块

        /// <summary>
        /// 获取地表信息
        /// </summary>
        /// <returns></returns>
        public string GetLandmark(string ModelJson, string UserJson)
        {
            //查询
            T_LandMark_Func func = new T_LandMark_Func();
            return func.GetLandmark( ModelJson,  UserJson);
        }

        /// <summary>
        /// 获取地表信息
        /// </summary>
        /// <returns></returns>
        public string GetTaskForLandmark(string ModelJson, string UserJson)
        {
            //查询
            T_LandMarkWithTask_Func func = new T_LandMarkWithTask_Func();
            return func.GetTaskForLandmark(ModelJson, UserJson);
            //释放
            //delete from t_landmarkwithtask where erpvoucherno = ''
        }

        /// <summary>
        /// 插入地标信息
        /// </summary>
        /// <returns></returns>
        public string SaveTaskwithandmark(string ModelJson, string UserJson)
        {
            //插入前判断地标没有其他单据占用
            //select erpvoucherno from t_landmarkwithtask where landmarkid=  group by taskno 
            T_LandMarkWithTask_Func func = new T_LandMarkWithTask_Func();
            return func.SaveTaskwithandmark(ModelJson, UserJson);
            //插入

        }

        #endregion



    }
}
