
using BILBasic.Basing;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
//using BILBasic.SystemSet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace BILBasic.Common
{
    public class Common_Func
    {
        public static DbFactory dbFactory = new DbFactory(DbFactory.DbFactoryType.SQLSERVER);

        private static Dictionary<string, string> _comboBoxSql = new Dictionary<string, string> 
        {
            {"cbxSex","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'User_Sex' ORDER BY ParameterID "},
            {"cbxUserType","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'User_UserType' ORDER BY ParameterID "},
            {"cbxUserStatus","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'User_UserStatus' ORDER BY ParameterID "},
            {"cbxMenuType","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Menu_MenuType' ORDER BY ParameterID "},
            {"cbxMenuStatus","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Menu_MenuStatus' ORDER BY ParameterID "},
            {"cbxUserGroupType","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'UserGroup_UserGroupType' ORDER BY ParameterID "},
            {"cbxUserGroupStatus","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'UserGroup_UserGroupStatus' ORDER BY ParameterID "},
            {"cbbIsOnline","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Online_Type' ORDER BY ParameterID "},
            
            {"cbxAreaType","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Area_AreaType' ORDER BY ParameterID "},
            {"cbxAreaStatus","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Area_AreaStatus' ORDER BY ParameterID "},
            {"cbxHouseType","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'House_HouseType' ORDER BY ParameterID "},
            {"cbxHouseStatus","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'House_HouseStatus' ORDER BY ParameterID "},
            {"cbxWarehouseType","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Warehouse_WarehouseType' ORDER BY ParameterID "},
            {"cbxWarehouseStatus","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Warehouse_WarehouseStatus' ORDER BY ParameterID "},
            {"colQualityType","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'QualityType' ORDER BY ParameterID "},
            {"cbbCheckType","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Check_CheckType' ORDER BY ParameterID "},
            {"cbbCheckStatus","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Check_CheckStatus' ORDER BY ParameterID "},
                        
            {"cbbParentMenu","SELECT 0 AS ID,TO_CHAR('根节点') AS NAME FROM DUAL UNION SELECT * FROM (SELECT ID,TO_CHAR(MENUNAME) AS NAME FROM T_MENU WHERE NODELEVEL <= 3 ORDER BY MENUTYPE, NODELEVEL, ID) "},
            {"cbbWarehouse","SELECT ID,WarehouseName AS NAME FROM T_Warehouse where ISDEL <> 2 AND WarehouseStatus <> 2 "},
                        
            {"cbbTransferType","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'MaterialTransfer_VoucherType' ORDER BY ParameterID "},
            
            {"cbbTransferStatus","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'MaterialTransfer_TransferStatus' ORDER BY ParameterID "},

            {"cbxType","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'MaterialRequest_VoucherType' ORDER BY ParameterID "},

            {"cbxStatus","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Stock_Status' ORDER BY ParameterID "},

            //收货查询界面单据名称
            {"cbxInStockVoucherName","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'VoucherRec_Name'  ORDER BY ParameterID "},
            
            //收货查询界面单据状态
            {"cbxInstockVoucherStatus","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'VoucherRec_Status' ORDER BY ParameterID "},
            
            {"cbxVoucherType","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Voucher_Type'  ORDER BY ParameterID "},

            {"cbxVoucherName","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Voucher_Name'  ORDER BY ParameterID "},
                        
            {"cbxFunction","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Voucher_Function'  ORDER BY ParameterID "},
            {"cbxWareHouse","select ID as ID,warehousename as NAME from t_Warehouse where isdel=1 and Warehousestatus=1 "},
             {"cbxHouse","select ID as ID,housename as NAME from t_House where isdel=1 and housestatus=1"},

            //上架任务界面任务状态
            {"cbxInTaskStatus","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'InStockTask_Status' ORDER BY ParameterID "},
            
            //出库单查询界面单据状态           
            {"cbxOutstockVoucherStatus","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'VoucherOut_Status' ORDER BY ParameterID "},
            //出库单查询界面单据名称
            {"cbxOutStockVoucherName","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'VoucherOut_Name'  ORDER BY ParameterID "},
            //下架任务界面任务状态
            {"cbxOutTaskStatus","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'OutStockTask_Status' ORDER BY ParameterID "},
                      
            //高低货位
            {"cbxHeightArea","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Area_HeightArea' ORDER BY ParameterID "},            
            //据点
            {"cbxStrongHold","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'StrongHold_Type' ORDER BY ParameterID "},
            //库存状态
            {"cbxStockStatus","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'MaterialStock_Status' ORDER BY ParameterID "},
           
            //拣货规则  
            {"cbxPickRule","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Pick_Rule' ORDER BY ParameterID "},
            //上架规则
            {"cbxRuleRec","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'RuleRec_Type' ORDER BY ParameterID "},
            
            //拣货单生成规则
            {"cbxRuleSplit","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'RuleSplit_Type' ORDER BY ParameterID "},
            //拣货单分配规则
            {"cbxRuleSlot","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'RuleSlot_Type' ORDER BY ParameterID "},
            //补货规则
            {"cbxRuleAddStock","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'RuleAddStock_Type' ORDER BY ParameterID "},
            //批次规则
            {"cbxRuleBatch","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'RuleBatch_Type' ORDER BY ParameterID "},
            
            //所属楼层  
            {"cbxFloor","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Floor_Type' ORDER BY ParameterID "},
            //单据是否质检  
            {"cbxIsQuality","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'Voucher_Quality' ORDER BY ParameterID "},
             
            //效期变更理由  
            {"cbxResoneCodeED","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'ResoneChangeCode' ORDER BY ParameterID "},
            //质量变更内容
            {"cbxResoneCodeQC","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'QResoneChangeCode' ORDER BY ParameterID "},
            {"chensComboBoxList1","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'InStockTask_Status' ORDER BY ParameterID "},
            //物料类别
            {"cbxMaterialClass","select Maintypecode as ID,Maintypename as NAME  from t_Material_Class where Strongholdcode = 'ABH'"},
            //库区分类
            {"cbxHouseProp","SELECT ParameterID AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'HouseProp_Type' ORDER BY ParameterID "},
            //仓库
            {"cbxFromErpWarehouse","select warehouseno AS ID,warehousename  AS NAME from t_warehouse ORDER BY warehouseno "},
            
        };

        //ymh 网页的获取下拉框方法
        public static List<SelectListItem> GetSelectListItem(string strSql)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (IDataReader dr = dbFactory.ExecuteReader(CommandType.Text, strSql))
            {
                while (dr.Read())
                {
                    SelectListItem item = new SelectListItem();
                    item.Value = dr["ID"].ToString();
                    item.Text = dr["Name"].ToString();
                    items.Add(item);
                }
            }

            return items;
        }

        //ymh 网页的获取下拉框方法
        public static bool GetSelectListItem(string key, ref List<SelectListItem> comboxBoxItemList, ref string strError)
        {
            if (_comboBoxSql.ContainsKey(key))
            {
                string strSql = _comboBoxSql[key];
                try
                {
                    comboxBoxItemList = GetSelectListItem(strSql);
                    return true;
                }
                catch (Exception ex)
                {
                    strError = "获取" + key + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                    return false;
                }
            }
            else
            {
                strError = "key  " + key + "的sql未找到!";
                return false;
            }
        }
        //ymh 网页的获取下拉框方法
        public static bool GetSelectListItemExt(string key, ref List<SelectListItem> comboxBoxItemList, ref string strError)
        {
            if (_comboBoxSqlExt.ContainsKey(key))
            {
                string strSql = _comboBoxSqlExt[key];
                try
                {
                    comboxBoxItemList = GetSelectListItemExt(strSql);
                    return true;
                }
                catch (Exception ex)
                {
                    strError = "获取" + key + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                    return false;
                }
            }
            else
            {
                strError = "key  " + key + "的sql未找到!";
                return false;
            }
        }
        //ymh 网页的获取下拉框方法
        public static List<SelectListItem> GetSelectListItemExt(string strSql)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (OracleDataReader dr = OracleDBHelper.ExecuteReader(CommandType.Text, strSql))
            {
                while (dr.Read())
                {
                    SelectListItem item = new SelectListItem();
                    item.Value = dr["ID"].ToDBString();
                    item.Text = dr["Name"].ToDBString();
                    items.Add(item);
                }
            }

            return items;
        }




        public static List<ComboBoxItem> GetComboBoxItem(string strSql)
        {
            List<ComboBoxItem> items = new List<ComboBoxItem>();
            using (OracleDataReader dr = OracleDBHelper.ExecuteReader(CommandType.Text, strSql))
            {
                while (dr.Read())
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.ID = int.Parse(dr["ID"].ToString());
                    item.Name = dr["Name"].ToString();
                    items.Add(item);
                }
            }

            return items;
        }

        public static bool GetComboBoxItem(string key, ref List<ComboBoxItem> comboxBoxItemList, ref string strError)
        {
            if (_comboBoxSql.ContainsKey(key))
            {
                string strSql = _comboBoxSql[key];
                try
                {
                    comboxBoxItemList = GetComboBoxItem(strSql);
                    return true;
                }
                catch (Exception ex)
                {
                    strError = "获取" + key + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                    return false;
                }
            }
            else
            {
                strError = "key  " + key + "的sql未找到!";
                return false;
            }
        }


        private static Dictionary<string, string> _comboBoxSqlExt = new Dictionary<string, string> 
        {            
            {"cbxWareHouse","select ID as ID,warehousename as NAME from t_Warehouse "},
            {"cbxPickLeader","select userno as ID,(userno || ';' || Username) as Name from t_user where nvl(ispickleader,1) = 2"},   //拣货组长 
             //物料类别
            {"cbxMaterialClass","select Maintypecode as ID,Maintypename as NAME  from t_Material_Class where Strongholdcode = 'CY1'"},
            //留置变更内容
            {"cbxRetenCodeR","SELECT ParameterIDN AS ID,ParameterName AS NAME FROM T_PARAMETER WHERE GROUPNAME = 'RResoneChangeCode' ORDER BY ParameterID "},
             
             //工艺路线设计界面======工序名称-------------add by cym 2018-10-8
            {"cmbGongXu","select gxDuanCode as ID,gxDuanName as NAME  from Mes_GYD where isDel = 1"},
            {"cmbMachineType","select ID, t.name as NAME from MES_MACHINETYPE t"},
            //工艺路线模板设计界面======模具
            {"cmbMouldType","select ID,ID || ' ' || t.name as NAME from MES_MODELTYPE t"},
            //工艺路线模板设计界面======单位
            {"cbxUnitType","select NAME as ID,NAME || ' ' || NAME_CN as NAME  from MES_UNIT "},
             //工艺路线模板设计界面======产品分类-------------add by cym 2018-10-18
            {"cbxCPType","select ID,NAME  from MES_CPTYPE "},
        
             //MES工控机======骏骏-------------add by cym 2019-3-18
            {"cob_main","select ID,NAME  from MES_ALERTTYPE"},
        };

        public static bool GetComboBoxItemExtByCym(string key, ref List<ComboBoxItemExt> comboxBoxItemList, ref string strError)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                //string strSql = "select gxDuanCode as ID,gxDuanName as NAME  from Mes_GYD where isDel = 1 and cptype = '" + key + "'";
                string strSql = key;
                try
                {
                    comboxBoxItemList = GetComboBoxItemExt(strSql);
                    return true;
                }
                catch (Exception ex)
                {
                    strError = "获取" + key + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                    return false;
                }
            }
            else
            {
                strError = "key  " + key + "的sql未找到!";
                return false;
            }
        }

        public static bool GetComboBoxItemExtByCymForGYLine(string key, ref List<ComboBoxItemExt> comboxBoxItemList, ref string strError)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                string strSql = "select gylineid as ID, (case when (machinetype = '01') then '自动' when (machinetype ='02') then '半自动' else '手工' end) || ' ' || gylinename as Name  from mes_gyline where isDel = 1 and ismoban='0' and cptype = '" + key + "'";
                try
                {
                    comboxBoxItemList = GetComboBoxItemExt(strSql);
                    return true;
                }
                catch (Exception ex)
                {
                    strError = "获取" + key + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                    return false;
                }
            }
            else
            {
                strError = "key  " + key + "的sql未找到!";
                return false;
            }
        }

        public static bool GetComboBoxItemExt(string key, ref List<ComboBoxItemExt> comboxBoxItemList, ref string strError)
        {
            if (_comboBoxSqlExt.ContainsKey(key))
            {
                string strSql = _comboBoxSqlExt[key];
                try
                {
                    comboxBoxItemList = GetComboBoxItemExt(strSql);
                    return true;
                }
                catch (Exception ex)
                {
                    strError = "获取" + key + "失败！" + ex.Message + "\r\n" + ex.TargetSite;
                    return false;
                }
            }
            else
            {
                strError = "key  " + key + "的sql未找到!";
                return false;
            }
        }
        
        public static List<ComboBoxItemExt> GetComboBoxItemExt(string strSql)
        {
            List<ComboBoxItemExt> items = new List<ComboBoxItemExt>();
            using (OracleDataReader dr = OracleDBHelper.ExecuteReader(CommandType.Text, strSql))
            {
                while (dr.Read())
                {
                    ComboBoxItemExt item = new ComboBoxItemExt();
                    item.ID = dr["ID"].ToDBString();
                    item.Name = dr["Name"].ToDBString();
                    items.Add(item);
                }
            }

            return items;
        }

        //public static List<ComboBoxItem> GetParentMenuByMenu(MenuInfo menu)
        //{
        //    List<ComboBoxItem> items = new List<ComboBoxItem>();
        //    Menu_DB _db = new Menu_DB();
        //    using (OracleDataReader dr = _db.GetParentSelectMenu(menu))
        //    {
        //        while (dr.Read())
        //        {
        //            ComboBoxItem item = new ComboBoxItem();
        //            item.ID = int.Parse(dr["ID"].ToString());
        //            item.Name = dr["MenuName"].ToString();
        //            items.Add(item);
        //        }
        //    }

        //    return items;
        //}

        public static string AddWhereAnd(string strSql, bool hadWhere)
        {
            string strReturn = strSql;
            if (hadWhere == false)
            {
                strSql += " Where ";
            }
            else
            {
                strSql += " And ";
            }

            return strSql;

        }

        public static string AddWhereOr(string strSql, bool hadWhere)
        {
            string strReturn = strSql;
            if (hadWhere == false)
            {
                strSql += " Where ";
            }
            else
            {
                strSql += " Or ";
            }

            return strSql;

        }
        public static bool readerExists(IDataReader dr, string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) return false;

            int count = dr.FieldCount;
            for (int i = 0; i < count; i++)
            {
                if (dr.GetName(i).ToUpper() == columnName.ToUpper()) return true;
            }
            return false;
        }

        //public static bool CheckImportTable(int iType, UserInfo user, ref string strError)
        //{
        //    switch (iType)
        //    {
        //        case 1:
        //            Basic.Area.Area_DB areadb = new Basic.Area.Area_DB();
        //            return areadb.CheckImportTable(ref strError);

        //        case 2:
        //            Stock.Stock_DB stockdb = new Stock.Stock_DB();
        //            return stockdb.CheckImportTable(ref strError);

        //        default:
        //            strError = "找不到对应的导入类型";
        //            return false;
        //    }
        //}

        public static bool UpLoadSql(List<string> lstSql, User.UserModel user, ref string strError)
        {
            if (lstSql == null || lstSql.Count <= 0)
            {
                strError = "上传数据不能为空";
                return false;
            }

            if (OracleDBHelper.ExecuteNonQueryList(lstSql, ref strError) >= 1)
            {
                return true;
            }
            else
            {
                if (string.IsNullOrEmpty(strError))
                {
                    try
                    {
                        OracleDBHelper.ExecuteNonQuery2(CommandType.Text, lstSql[0], null);
                        if (lstSql.Count >= 2) OracleDBHelper.ExecuteNonQuery2(CommandType.Text, lstSql[1], null);
                    }
                    catch (Exception ex)
                    {
                        strError = ex.Message;
                    }
                }
                return false;
            }
        }

        //public static bool DealImport(int iType, UserInfo user, ref string strError)
        //{
        //    switch (iType)
        //    {
        //        case 1:
        //            Basic.Area.Area_DB areadb = new Basic.Area.Area_DB();
        //            return areadb.DealImport(ref strError);

        //        case 2:
        //            Stock.Stock_DB stockdb = new Stock.Stock_DB();
        //            return stockdb.DealImport(ref strError);

        //        default:
        //            strError = "找不到对应的导入类型";
        //            return false;
        //    }
        //}

        public static bool IsOracleError(string ErrorMsg, ref string strError)
        {
            try
            {
                int index = ErrorMsg.ToUpper().IndexOf("ORA-");
                if (index >= 0)
                {
                    strError = string.Format("数据库连接错误,请重试!{0}{1}", Environment.NewLine, ErrorMsg.Substring(ErrorMsg.IndexOf(':', index) + 1).Trim());
                    return true;
                }
                else
                {
                    strError = ErrorMsg;
                    return false;
                }
            }
            catch
            {
                strError = ErrorMsg;
                return false;
            }
        }

        /// <summary>
        /// 获取128码(计算开始位,检验位,结束位) 
        /// (弃用)
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns>128B码：ChrW(204)</returns>
        public static string StrToCode128B(string barcode)
        {
            try
            {
                if (IsNumber(barcode))
                {
                    int checkB;
                    int ASCII;
                    checkB = 1; //开始位的码值为104 mod 103 = 1

                    for (int i = 0; i < barcode.Length; i++)
                    {
                        ASCII = barcode[0].ToInt32();    //不过滤无效字符,比如汉字

                        if (ASCII < 135)
                        {
                            ASCII = ASCII - 32;
                        }
                        else if (ASCII > 134)
                        {
                            ASCII = ASCII - 100;
                        }

                        checkB = (checkB + (i + 1) * ASCII) % 103;    //计算校验位
                    }

                    if (checkB > 0 && checkB < 95)    //有的资料直接求103的模,解说不充分,因为有的校验位超过127时,系统会"吃"掉它们(连带休止符).
                    {
                        checkB = checkB + 32;
                    }
                    else if (checkB > 94)              //字体设置时,字模被定义了2个值.观察字体文件时能发现.
                    {
                        checkB = checkB + 100;
                    }

                    //开始位 ＋ ［FNC1(为EAN128码时加)］ ＋ 数据位 ＋ 检验位 ＋ 结束位
                    return string.Format("{0}{1}{2}{3}", (char)204, barcode, checkB > 0 ? (char)checkB : (char)32, (char)206);
                }
                else
                {
                    return barcode;
                }
            }
            catch
            {
                return barcode;
            }
        }

        //public static string GetQRCodeImgStr(string barcode)
        //{
        //    if (Common_Var.QRCode == null)
        //    {
        //        Common_Var.QRCode = new GenerationQRCode();
        //    }

        //    return ConvertImageToString(Common_Var.QRCode.CreateQRCode(barcode));
        //}

        //public static string GetCode128ImgStr(string barcode)
        //{
        //    if (Common_Var.code128 == null)
        //    {
        //        Common_Var.code128 = new Code128Barcode();
        //        Common_Var.code128.Height = 60;
        //        Common_Var.code128.ValueFont = new System.Drawing.Font("Arial", 13, FontStyle.Bold);
        //    }

        //    //if (!File.Exists("C:\\Barcode\\" + barcode + ".bmp")) Common_Var.code128.GetCodeImage(barcode).Save("C:\\Barcode\\" + barcode + ".bmp", ImageFormat.Bmp);
        //    return ConvertImageToString(Common_Var.code128.GetCodeImage(barcode));
        //}

        /// <summary>
        /// 将图片Image转换成String
        /// </summary>
        /// <param name="imgOrderNo">image对象</param>
        /// <returns></returns>
        //public static string ConvertImageToString(Image image)
        //{
        //    byte[] BImage = ImageToBytes(image, ImageFormat.Bmp);
        //    return Convert.ToBase64String(BImage);
        //}

        ///// 将图片Image转换成Byte[]
        ///// </summary>
        ///// <param name="Image">image对象</param>
        ///// <param name="imageFormat">后缀名</param>
        ///// <returns></returns>
        //public static byte[] ImageToBytes(Image image, System.Drawing.Imaging.ImageFormat imageFormat)
        //{
        //    if (image == null) { return null; }
        //    byte[] data;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (Bitmap Bitmap = new Bitmap(image))
        //        {
        //            Bitmap.Save(ms, imageFormat);
        //            ms.Position = 0;
        //            data = new byte[ms.Length];
        //            ms.Read(data, 0, Convert.ToInt32(ms.Length));
        //            ms.Flush();
        //        }
        //    }
        //    return data;
        //}

        public static int SpiltString(string text, int length, ref string[] arr)
        {
            if (string.IsNullOrEmpty(text))
            {
                arr[0] = " ";
                return 1;
            }

            int count = 0;
            byte[] bytes = Encoding.GetEncoding(936).GetBytes(text);
            count = (int)Math.Ceiling(Convert.ToDouble(bytes.Length) / Convert.ToDouble(length));
            Array.Resize(ref arr, count);

            bool bBorrow = false;
            string strLine;
            string strTemp;
            char cEnd;

            if (count == 1)
            {
                arr[0] = text;
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    byte[] line = new byte[length + 1];
                    int cur = 0;
                    for (int j = 0; j < length; j++)
                    {
                        if (bBorrow && j == 0) continue;

                        cur = i * length + j;
                        if (cur >= bytes.Length) break;

                        line[j] = bytes[cur];
                    }

                    bBorrow = false;
                    strLine = Encoding.GetEncoding(936).GetString(line).Trim('\0');
                    if (!string.IsNullOrEmpty(strLine) && strLine.Length >= 2)
                    {
                        strTemp = strLine.Substring(0, strLine.Length - 1);
                        if (!string.IsNullOrEmpty(strTemp))
                        {
                            cEnd = text[text.IndexOf(strTemp) + strTemp.Length];

                            if (strLine[strLine.Length - 1] != cEnd)
                            {
                                strLine = strTemp + cEnd;
                                bBorrow = true;
                            }
                        }
                    }

                    arr[i] = strLine;
                }
            }

            return count;
        }

        public static bool IsAllZero(string text)
        {
            return Regex.IsMatch(text, @"^[0]*$");
        }

        public static bool IsNumber(string text)
        {
            return Regex.IsMatch(text, @"^-?\d+\.?\d*$");
        }

        public static bool IsNumber(char text)
        {
            return Regex.IsMatch(text.ToString(), @"^[0-9]*$");
        }

        public static bool IsNullOrEmpty(string str)
        {
            if (str == null || str == "" || str.Length <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsEqualString(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
            {
                return true;
            }
            else
            {
                return str1 == str2;
            }
        }
    }
}
