using BILBasic.DBA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BILWeb.DAL;
using BILWeb.InStock;
using BILBasic.Common;
using Oracle.ManagedDataAccess.Client;
using System.Reflection;
using BILWeb.Material;
using BILWeb.Query;
using BILBasic.Basing.Factory;
using BILWeb.Stock;
using BILWeb.Pallet;
using BILBasic.JSONUtil;
using BILBasic.User;

namespace BILWeb.Print
{
    public partial class Print_DB
    {
        #region 成品、半制品和散装品打印
        /// <summary>
        /// PDA成品、半制品和散装品打印
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="json">打印类型是成品托盘的时候，是T_PalletDetailInfo类；如果是其他的类型，则是Barcode_Model类</param>
        /// <param name="printtype">打印类型：1：托盘标签；0：其他标签；2：只打印托盘标签</param>
        /// <returns></returns>
        public string PrintForProductAndroid(string UserJson, string json, string printtype)
        {
            BaseMessage_Model<Barcode_Model> bm = new BaseMessage_Model<Barcode_Model>();
            string j = "";
            string ipport = "";
            string ErrMsg = "";

            try
            {
                List<Barcode_Model> list = new List<Barcode_Model>();
                if (printtype == "0")
                    list = Check_Func.DeserializeJsonToList<Barcode_Model>(json);
                else//成品托盘标签
                {
                    var templst = Check_Func.DeserializeJsonToList<T_PalletDetailInfo>(json);
                    if (templst.Count == 0)
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "数据不能为空";
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }

                    //生成托盘标签/或 更新托盘标签
                    var rtstring = SaveModelListSqlToDBADF(UserJson, json);

                    var rttemp = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_PalletDetailInfo>>>(rtstring);

                    if (rttemp.HeaderStatus == "E")
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "生成托盘标签失败";
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }

                    //palletNo
                    var rt = PrintZplPallet(rttemp.TaskNo, templst[0].PrintIPAdress, ref ErrMsg);

                    if (!rt)
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "打印托盘标签失败";
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }

                    bm.HeaderStatus = "S";
                    bm.Message = "成功";
                    j = Check_Func.SerializeObject(bm);
                    return j;

                }

                var tempUser = Check_Func.DeserializeJsonToObject<UserModel>(UserJson);
                string strUserName = "";
                if (!string.IsNullOrEmpty(tempUser.UserName))
                {
                    strUserName = tempUser.UserName;
                }

                if (list.Count == 0)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "数据不能为空";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

                ipport = list[0].IP;
                string data = "";
                List<string> squence = getSqNum(list.Count, ref data);
                string sq = "";
                int i = 0;

                bool rtIsOk = true;

                foreach (Barcode_Model model2 in list)
                {
                    List<T_MaterialInfo> lm = new List<T_MaterialInfo>();
                    if (!GetMaterial(0, model2.MaterialNo, model2.StrongHoldCode, ref lm, ref ErrMsg))
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = ErrMsg;
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }
                    T_MaterialInfo tm = lm[0];
                    sq = getSqu(squence[i]);

                    rtIsOk = ModelMakeAndroid(data, sq, model2, tm, strUserName);

                    i++;
                }

                //add by cym 2017-10-17
                if (!rtIsOk)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "到期日期不对，请确认！";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

              

                //传入类型为1，标识用lpk打印机打印
                bool res = SubBarcodes_C(list, ipport, 0, ref ErrMsg, 0);
                if (res)
                {
                    bm.HeaderStatus = "S";
                    bm.Message = "成功";
                    bm.MaterialDoc = list[0].SerialNo;
                    bm.ModelJson = list[0];

                    j = Check_Func.SerializeObject(bm);
                    return j;
                }
                else
                {
                    bm.HeaderStatus = "E";
                    bm.Message = ErrMsg;
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

            }
            catch (Exception ex)
            {
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                j = Check_Func.SerializeObject(bm);
                return j;
            }

        }


        public string PrintForProductAndroid2(string UserJson, string json, string printtype)
        {
            BaseMessage_Model<Barcode_Model> bm = new BaseMessage_Model<Barcode_Model>();
            string j = "";
            string ipport = "";
            string ErrMsg = "";

            try
            {
                List<Barcode_Model> list = new List<Barcode_Model>();
                if (printtype == "0")
                    list = Check_Func.DeserializeJsonToList<Barcode_Model>(json);
                else//成品托盘标签
                {
                    var templst = Check_Func.DeserializeJsonToList<T_PalletDetailInfo>(json);
                    if (templst.Count == 0)
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "数据不能为空";
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }

                    //生成托盘标签/或 更新托盘标签
                    var rtstring = SaveModelListSqlToDBADF(UserJson, json);

                    var rttemp = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_PalletDetailInfo>>>(rtstring);

                    if (rttemp.HeaderStatus == "E")
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "生成托盘标签失败";
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }

                    //palletNo
                    var rt = PrintZplPallet(rttemp.TaskNo, templst[0].PrintIPAdress, ref ErrMsg);

                    if (!rt)
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "打印托盘标签失败";
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }

                    bm.HeaderStatus = "S";
                    bm.Message = "成功";
                    j = Check_Func.SerializeObject(bm);
                    return j;

                }

                var tempUser = Check_Func.DeserializeJsonToObject<UserModel>(UserJson);
                string strUserName = "";
                if (!string.IsNullOrEmpty(tempUser.UserName))
                {
                    strUserName = tempUser.UserName;
                }

                if (list.Count == 0)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "数据不能为空";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

                ipport = list[0].IP;
                string data = "";
                List<string> squence = getSqNum(list.Count, ref data);
                string sq = "";
                int i = 0;

                bool rtIsOk = true;

                foreach (Barcode_Model model2 in list)
                {
                    List<T_MaterialInfo> lm = new List<T_MaterialInfo>();
                    if (!GetMaterial(0, model2.MaterialNo, model2.StrongHoldCode, ref lm, ref ErrMsg))
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = ErrMsg;
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }
                    T_MaterialInfo tm = lm[0];
                    sq = getSqu(squence[i]);

                    rtIsOk = ModelMakeAndroid(data, sq, model2, tm, strUserName);

                    i++;
                }

                //add by cym 2017-10-17
                if (!rtIsOk)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "到期日期不对，请确认！";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

                //================================================添加兆信标签,不要删================================================
                //================================================添加兆信标签,不要删================================================
                string sql = "";
                
                foreach (Barcode_Model model2 in list)
                {
                    //过滤掉客户兆信
                    //sql = "select count(1) from Mes_zx_paramert where customCode = '" + model2.CusCode + "'";
                    //int f = dbFactory.ExecuteScalar(CommandType.Text, sql).ToInt32();
                    //if (f > 0)
                    //{
                    //    break;
                    //}

                    if (!string.IsNullOrEmpty(model2.Eds))
                    {
                        if ((model2.LABELMARK == "OutChengPin" || model2.LABELMARK == "OutBanZhi") && model2.Eds == "Y")
                        {
                            //sql = "select barcode  from T_ZXBARCODE where id =(select min(id) from T_ZXBARCODE where erpvoucherno='" + model2.zyID + "' and materialno='" + model2.MaterialNo + "' and status=0)";
                            //update by cym 2019-2-28
                            sql = "select barcode  from T_ZXBARCODE where id =(select min(id) from T_ZXBARCODE where erpvoucherno='" + model2.zyID + "' and status=0)";
                            object o = dbFactory.ExecuteScalar(CommandType.Text, sql);
                            if (o == null || o.ToString() == "")
                            {
                                bm.HeaderStatus = "E";
                                bm.Message = "没有找到对应兆信条码";
                                j = Check_Func.SerializeObject(bm);
                                return j;
                            }
                            model2.ZXBARCODE = o.ToString();
                            sql = "update T_ZXBARCODE set status = 1 where barcode = '" + model2.ZXBARCODE + "'";
                            dbFactory.ExecuteNonQuery(CommandType.Text, sql);
                        }
                    }
                  
                }
                //=======================

                //传入类型为1，标识用lpk打印机打印
                bool res = SubBarcodes_C(list, ipport, 0, ref ErrMsg, 0, sql);
                if (res)
                {
                    bm.HeaderStatus = "S";
                    bm.Message = "成功";
                    bm.MaterialDoc = list[0].SerialNo;
                    bm.ModelJson = list[0];

                    j = Check_Func.SerializeObject(bm);
                    return j;
                }
                else
                {
                    bm.HeaderStatus = "E";
                    bm.Message = ErrMsg;
                    bm.MaterialDoc = list[0].SerialNo;
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

            }
            catch (Exception ex)
            {
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                j = Check_Func.SerializeObject(bm);
                return j;
            }

        }


        /// <summary>
        /// pda根据序列号重打
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="json"></param>
        /// <param name="printtype"></param>
        /// <returns></returns>
        public string ReprintAndroid(string json)
        {
            BaseMessage_Model<Barcode_Model> bm = new BaseMessage_Model<Barcode_Model>();
            string j = "";
            string ipport = "";
            string ErrMsg = "";

            try
            {
                List<Barcode_Model> list = new List<Barcode_Model>();
                list = Check_Func.DeserializeJsonToList<Barcode_Model>(json);
                ipport = list[0].IP;

                string sql2 = "select * from t_outbarcode b where serialno = '" + list[0].SerialNo + "'";
                DataTable dt2 = dbFactory.ExecuteDataSet(CommandType.Text, sql2).Tables[0];
                list = ConvertToModel<Barcode_Model>(dt2);
                if (list.Count == 0)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "没有打印数据";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }
                

                //打印标签
                bool res = PrintZPL(list, ipport, ref ErrMsg);
                if (res)
                {
                    bm.HeaderStatus = "S";
                    bm.Message = "成功";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }
                else
                {
                    bm.HeaderStatus = "E";
                    bm.Message = ErrMsg;
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

            }
            catch (Exception ex)
            {
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                j = Check_Func.SerializeObject(bm);
                return j;
            }

        }
        /// <summary>
        /// PDA成品、半制品和散装品打印
        /// 退料的时候，按照历史标签，原来是什么日期就是什么日期---------------add by cym 2018-11-2
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="json">打印类型是成品托盘的时候，是T_PalletDetailInfo类；如果是其他的类型，则是Barcode_Model类</param>
        /// <param name="printtype">打印类型：1：托盘标签；0：其他标签；2：只打印托盘标签</param>
        /// <returns></returns>
        public string PrintForProductAndroidForNoSunday(string UserJson, string json, string printtype)
        {
            BaseMessage_Model<Barcode_Model> bm = new BaseMessage_Model<Barcode_Model>();
            string j = "";
            string ipport = "";
            string ErrMsg = "";

            try
            {
                List<Barcode_Model> list = new List<Barcode_Model>();
                if (printtype == "0")
                    list = Check_Func.DeserializeJsonToList<Barcode_Model>(json);
                else//成品托盘标签
                {
                    var templst = Check_Func.DeserializeJsonToList<T_PalletDetailInfo>(json);
                    if (templst.Count == 0)
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "数据不能为空";
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }

                    //生成托盘标签/或 更新托盘标签
                    var rtstring = SaveModelListSqlToDBADF(UserJson, json);

                    var rttemp = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_PalletDetailInfo>>>(rtstring);

                    if (rttemp.HeaderStatus == "E")
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "生成托盘标签失败";
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }

                    //palletNo
                    var rt = PrintZplPallet(rttemp.TaskNo, templst[0].PrintIPAdress, ref ErrMsg);

                    if (!rt)
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "打印托盘标签失败";
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }

                    bm.HeaderStatus = "S";
                    bm.Message = "成功";
                    j = Check_Func.SerializeObject(bm);
                    return j;

                }

                var tempUser = Check_Func.DeserializeJsonToObject<UserModel>(UserJson);
                string strUserName = "";
                if (!string.IsNullOrEmpty(tempUser.UserName))
                {
                    strUserName = tempUser.UserName;
                }

                if (list.Count == 0)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "数据不能为空";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

                ipport = list[0].IP;
                string data = "";
                List<string> squence = getSqNum(list.Count, ref data);
                string sq = "";
                int i = 0;

                bool rtIsOk = true;

                foreach (Barcode_Model model2 in list)
                {
                    List<T_MaterialInfo> lm = new List<T_MaterialInfo>();
                    if (!GetMaterial(0, model2.MaterialNo, model2.StrongHoldCode, ref lm, ref ErrMsg))
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = ErrMsg;
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }
                    T_MaterialInfo tm = lm[0];
                    sq = getSqu(squence[i]);

                    rtIsOk = ModelMakeAndroidNoSunday(data, sq, model2, tm, strUserName);

                    i++;
                }

                //add by cym 2017-10-17
                if (!rtIsOk)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "到期日期不对，请确认！";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

                //传入类型为1，标识用lpk打印机打印
                bool res = SubBarcodes_C(list, ipport, 0, ref ErrMsg, 0);
                if (res)
                {
                    bm.HeaderStatus = "S";
                    bm.Message = "成功";
                    bm.MaterialDoc = list[0].SerialNo;
                    bm.ModelJson = list[0];

                    j = Check_Func.SerializeObject(bm);
                    return j;
                }
                else
                {
                    bm.HeaderStatus = "E";
                    bm.Message = ErrMsg;
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

            }
            catch (Exception ex)
            {
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                j = Check_Func.SerializeObject(bm);
                return j;
            }

        }

        public string PrintForProductAndroidSup(string UserJson, string json, string printtype)
        {
            BaseMessage_Model<Barcode_Model> bm = new BaseMessage_Model<Barcode_Model>();
            string j = "";
            string ipport = "";
            string ErrMsg = "";

            try
            {
                List<Barcode_Model> list = new List<Barcode_Model>();
                if (printtype == "0")
                    list = Check_Func.DeserializeJsonToList<Barcode_Model>(json);
                else//成品托盘标签
                {
                    var templst = Check_Func.DeserializeJsonToList<T_PalletDetailInfo>(json);
                    if (templst.Count == 0)
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "数据不能为空";
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }

                    //add by cym 2017-11-8 防止重复组托
                    if (CheckBarcodeIsSame(templst[0].SerialNo, ref ErrMsg) == false)
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "条码已组托！" + templst[0].SerialNo;
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }

                    //生成托盘标签/或 更新托盘标签
                    var rtstring = SaveModelListSqlToDBADF(UserJson, json);

                    var rttemp = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_PalletDetailInfo>>>(rtstring);

                    if (rttemp.HeaderStatus == "E")
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "生成托盘标签失败";
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }

                    //palletNo
                    //var rt = PrintZplPallet(rttemp.TaskNo, templst[0].PrintIPAdress, ref ErrMsg);

                    //if (!rt)
                    //{
                    //    bm.HeaderStatus = "E";
                    //    bm.Message = "打印托盘标签失败";
                    //    j = Check_Func.SerializeObject(bm);
                    //    return j;
                    //}

                    bm.HeaderStatus = "S";
                    bm.Message = "成功";
                    j = Check_Func.SerializeObject(bm);
                    return j;

                }

                var tempUser = Check_Func.DeserializeJsonToObject<UserModel>(UserJson);
                string strUserName = "";
                if (!string.IsNullOrEmpty(tempUser.UserName))
                {
                    strUserName = tempUser.UserName;
                }

                if (list.Count == 0)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "数据不能为空";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

                ipport = list[0].IP;
                string data = "";
                List<string> squence = getSqNum(list.Count, ref data);
                string sq = "";
                int i = 0;

                bool rtIsOk = true;

                foreach (Barcode_Model model2 in list)
                {
                    List<T_MaterialInfo> lm = new List<T_MaterialInfo>();
                    if (!GetMaterial(0, model2.MaterialNo, model2.StrongHoldCode, ref lm, ref ErrMsg))
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = ErrMsg;
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }
                    T_MaterialInfo tm = lm[0];
                    sq = getSqu(squence[i]);

                    rtIsOk = ModelMakeAndroid(data, sq, model2, tm, strUserName);

                    i++;
                }

                //add by cym 2017-10-17
                if (!rtIsOk)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "到期日期不对，请确认！";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

                //传入类型为1，标识用lpk打印机打印
                bool res = SubBarcodes_C(list, ipport, 0, ref ErrMsg, 0);
                if (res)
                {
                    bm.HeaderStatus = "S";
                    bm.Message = "成功";
                    bm.MaterialDoc = list[0].SerialNo;
                    bm.ModelJson = list[0];

                    j = Check_Func.SerializeObject(bm);
                    return j;
                }
                else
                {
                    bm.HeaderStatus = "E";
                    bm.Message = ErrMsg;
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

            }
            catch (Exception ex)
            {
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                j = Check_Func.SerializeObject(bm);
                return j;
            }

        }
        /// <summary>
        /// PDA成品、半制品和散装品组托
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="json">打印类型是托盘的时候，是T_PalletDetailInfo类</param>
        /// <returns></returns>
        public string SavePalletForProductAndroid(string UserJson, string json)
        {
            BaseMessage_Model<string> bm = new BaseMessage_Model<string>();
            string j = "";
            
            try
            {
                //托盘标签

                var templst = Check_Func.DeserializeJsonToList<T_PalletDetailInfo>(json);
                if (templst.Count == 0)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "数据不能为空";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

                string ErrMsg = "";
                //add by cym 2017-11-8 防止重复组托
                if (CheckBarcodeIsSame(templst[0].SerialNo, ref ErrMsg) == false)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "条码已组托！";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }
                
                //生成托盘标签/或 更新托盘标签
                var rtstring = SaveModelListSqlToDBADF(UserJson, json);

                var rttemp = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_PalletDetailInfo>>>(rtstring);

                if (rttemp.HeaderStatus == "E")
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "生成托盘标签失败";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

                bm.HeaderStatus = "S";
                bm.Message = "成功";
                bm.TaskNo = rttemp.TaskNo;
                j = Check_Func.SerializeObject(bm);
                return j;

            }
            catch (Exception ex)
            {
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                j = Check_Func.SerializeObject(bm);
                return j;
            }

        }

        /// <summary>
        /// 判断条码是否已经组托（false：已组托； true：没有）
        /// </summary>
        /// <param name="serialno"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool CheckBarcodeIsSame(string serialno, ref string ErrMsg)
        {
            try
            {
                ErrMsg = "";
                string strSql = "select count(*) sumC from t_palletdetail t where t.serialno ='" + serialno + "'";
                using (IDataReader dr = dbFactory.ExecuteReader(strSql))
                {
                    if (dr.Read())
                    {
                        if (dr["sumC"].ToInt32() > 0)
                            return false;
                        else
                            return true;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 拆托的时候，打印托盘标签
        /// </summary>
        /// <param name="PalletJson"></param>
        /// <returns></returns>
        public string PrintForChaiTuoProductAndroid(string PalletJson)
        {
            BaseMessage_Model<Barcode_Model> bm = new BaseMessage_Model<Barcode_Model>();
            string j = "";
            string ErrMsg = "";

            try
            {
                var templst = Check_Func.DeserializeJsonToList<T_PalletDetailInfo>(PalletJson);
                //palletNo
                var rt = PrintZplPallet(templst[0].TaskNo, templst[0].PrintIPAdress, ref ErrMsg);

                if (!rt)
                {
                    bm.HeaderStatus = "E";
                    bm.Message = "打印托盘标签失败";
                    j = Check_Func.SerializeObject(bm);
                    return j;
                }

                bm.HeaderStatus = "S";
                bm.Message = "成功";
                j = Check_Func.SerializeObject(bm);
                return j;
            }
            catch (Exception ex)
            {
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                j = Check_Func.SerializeObject(bm);
                return j;
            }
        }

        private static bool ModelMakeAndroid(string data, string sq, Barcode_Model model2, T_MaterialInfo tm, string strUserName)
        {
            var rt = true;
            model2.ZXBARCODE = "";
            model2.CompanyCode = "10";
            model2.IsDel = 1;
            model2.Unit = tm.Unit;
            model2.MaterialNoID = tm.ID;
            model2.ErpBarCode = tm.ErpBarCode;
            //model2.StrongHoldCode = selectItem.StrongHoldCode;

            //model2.StrongHoldName = tm.StrongHoldName;

            //model2.ErpVoucherNo = tm.ErpVoucherNo;
            //model2.VoucherType = selectItem.VoucherType.ToString();
            //model2.MaterialNo = selectItem.MaterialNo;
            if (!string.IsNullOrWhiteSpace(tm.MaterialDesc))
                model2.MaterialDesc = tm.MaterialDesc.Replace("'", "’");
            else
                model2.MaterialDesc = tm.MaterialDesc;
            //model2.SupCode = selectItem.SupplierNo;
            //model2.SupName = selectItem.SupplierName;
            //订单数量
            //外箱1，內盒0
            //model2.BarcodeType = 1;
            //model2.OutPackQty = model2.Qty;
            //model2.SupPrdDate = model2.ProductDate;
            //model2.BatchNo
            //model2.EDate
            if (tm.PackQty != 0)//有包装量的时候
            {
                model2.BoxWeight = tm.PackQty.ToString();
            }
            //存储条件
            model2.StoreCondition = tm.StoreCondition;
            //防护措施
            model2.ProtectWay = tm.ProtectWay;

            if (model2.ProductDate == DateTime.MinValue)
            {
                string weekstr = DateTime.Now.DayOfWeek.ToString();
                if (weekstr == "Sunday")
                {
                    model2.ProductDate = DateTime.Now.AddDays(1);
                }
                else
                {
                    model2.ProductDate = DateTime.Now;
                }
            }
            else
            {
                string weekstr = model2.ProductDate.DayOfWeek.ToString();
                if (weekstr == "Sunday")
                {
                    model2.ProductDate = model2.ProductDate.AddDays(1);
                }
            }
            //delete by cym 2017-9-29
            //if (!string.IsNullOrWhiteSpace(model2.SupName))
            //{
            //    if (model2.SupName == "1")//散装品 
            //    {
            //        string weekstr = DateTime.Now.DayOfWeek.ToString();
            //        if (weekstr == "Sunday")
            //        {
            //            model2.ProductDate = DateTime.Now.AddDays(1).Date;
            //        }
            //        else
            //        {
            //            model2.ProductDate = DateTime.Now.Date;
            //        }
            //    }
            //}
            //else
            //    model2.ProductDate = DateTime.Now.Date;

            model2.EDate = model2.EDate.Date;

            model2.Creater = strUserName;

            //add by cym 2018-7-16
            model2.CusName = tm.BrandIntRO;
            model2.standardbox1 = tm.standardbox1;
            model2.standardbox2 = tm.standardbox2;
            model2.standardbox3 = tm.standardbox3;

            #region 半制外
            //标题
            //model2.areano = "";
            //model2.ProductDate = DateTime.Now;
            ////生产班组
            //model2.ProductClass = "";
            ////成品批号
            //model2.ProductBatch = "";
            ////第几箱总箱数
            //model2.BarcodeNo = 1;
            #endregion

            #region 散装外
            //标题
            //model2.areano = "";
            ////相对比重
            //model2.RelaWeight = "";
            ////存储条件
            //model2.StoreCondition = "";
            ////防护措施
            //model2.ProtectWay = "";
            ////操作人
            //model2.Creater = "";
            ////这里指大箱里面有多少小箱
            //model2.BoxCount = 1;
            #endregion

            #region 成品外
            //标题
            //model2.areano = "";
            ////生产班组
            //model2.ProductClass = "";
            ////包装方式
            //model2.BoxWeight = "";
            ////数量
            //model2.ItemQty = 1;
            ////重量
            //model2.Qty = 1;
            #endregion

            #region 成品托
            //标题
            //model2.areano = "";
            ////生产班组
            //model2.ProductClass = "";
            ////包装方式
            //model2.BoxWeight = "";
            ////数量
            //model2.ItemQty = 1;
            ////托盘号
            //model2.PalletNo = "";
            ////总箱数
            //model2.BoxCount = 1;
            #endregion

            model2.StoreCondition = tm.StoreCondition;
            model2.SpecialRequire = tm.SpecialRequire;
            model2.ProtectWay = tm.ProtectWay;

            if (model2.CreateTime == DateTime.MinValue)
                model2.CreateTime = DateTime.Now;

            //add by cym 2017-10-17
            if (model2.EDate.Date == model2.CreateTime.Date)
            {
                return false;
            }

            //add by cym 2017-11-4
            if (model2.CreateTime >= model2.EDate)
            {
                return false;
            }

            model2.SerialNo = data + sq;

            model2.SupPrdDate = model2.ProductDate;

            //如果包材外，有效期自动加3年
            if (model2.LABELMARK == "OutBaoCai")
                model2.EDate = model2.CreateTime.AddYears(3);

            //add by cym 2019-2-27=================================后续需要用IsZXBarcode字段来替换赋值！！！！！！！===========================================
            model2.Eds = tm.PlaceArea;

            string bt = "";
            model2.BarcodeMType = GetBT(bt, model2.MaterialNo, tm.MainTypeCode, tm.PurchaseTypeCode);
            if (model2.BarcodeType == 1)
                model2.BarCode = "1@" + model2.BarcodeMType + "@" + model2.MaterialNo + "@" + model2.SupCode + "@" + model2.Qty + "@" + data + "@" + model2.SerialNo;
            else if (model2.BarcodeType == 0)
                model2.BarCode = "0@" + model2.BarcodeMType + "@" + model2.MaterialNo + "@" + model2.SupCode + "@" + model2.Qty + "@" + data + "@" + model2.SerialNo;

            return rt;
        }

        private static bool ModelMakeAndroidNoSunday(string data, string sq, Barcode_Model model2, T_MaterialInfo tm, string strUserName)
        {
            var rt = true;

            model2.CompanyCode = "10";
            model2.IsDel = 1;
            model2.Unit = tm.Unit;
            model2.MaterialNoID = tm.ID;
            model2.ErpBarCode = tm.ErpBarCode;

            if (!string.IsNullOrWhiteSpace(tm.MaterialDesc))
                model2.MaterialDesc = tm.MaterialDesc.Replace("'", "’");
            else
                model2.MaterialDesc = tm.MaterialDesc;

            if (tm.PackQty != 0)//有包装量的时候
            {
                model2.BoxWeight = tm.PackQty.ToString();
            }
            //存储条件
            model2.StoreCondition = tm.StoreCondition;
            //防护措施
            model2.ProtectWay = tm.ProtectWay;

            if (model2.ProductDate == DateTime.MinValue)
            {
                //string weekstr = DateTime.Now.DayOfWeek.ToString();
                //if (weekstr == "Sunday")
                //{
                //    model2.ProductDate = DateTime.Now.AddDays(1);
                //}
                //else
                //{
                    model2.ProductDate = DateTime.Now;
                //}
            }
            else
            {
                //string weekstr = model2.ProductDate.DayOfWeek.ToString();
                //if (weekstr == "Sunday")
                //{
                //    model2.ProductDate = model2.ProductDate.AddDays(1);
                //}
            }

            model2.EDate = model2.EDate.Date;
            model2.CreateTime = model2.CreateTime.Date;

            model2.Creater = strUserName;

            //add by cym 2018-7-16
            model2.CusName = tm.BrandIntRO;
            model2.standardbox1 = tm.standardbox1;
            model2.standardbox2 = tm.standardbox2;
            model2.standardbox3 = tm.standardbox3;
            
            model2.StoreCondition = tm.StoreCondition;
            model2.SpecialRequire = tm.SpecialRequire;
            model2.ProtectWay = tm.ProtectWay;

            if (model2.CreateTime == DateTime.MinValue)
                model2.CreateTime = DateTime.Now;

            //add by cym 2017-10-17
            if (model2.EDate.Date == model2.CreateTime.Date)
            {
                return false;
            }

            //add by cym 2017-11-4
            if (model2.CreateTime >= model2.EDate)
            {
                return false;
            }

            model2.SerialNo = data + sq;

            model2.SupPrdDate = model2.ProductDate;

            //如果包材外，有效期自动加3年
            //if (model2.LABELMARK == "OutBaoCai")
            //    model2.EDate = model2.ProductDate.AddYears(3);


            string bt = "";
            model2.BarcodeMType = GetBT(bt, model2.MaterialNo, tm.MainTypeCode, tm.PurchaseTypeCode);
            if (model2.BarcodeType == 1)
                model2.BarCode = "1@" + model2.BarcodeMType + "@" + model2.MaterialNo + "@" + model2.SupCode + "@" + model2.Qty + "@" + data + "@" + model2.SerialNo;
            else if (model2.BarcodeType == 0)
                model2.BarCode = "0@" + model2.BarcodeMType + "@" + model2.MaterialNo + "@" + model2.SupCode + "@" + model2.Qty + "@" + data + "@" + model2.SerialNo;

            return rt;
        }

        public bool SubBarcodes_C(List<Barcode_Model> list, string ipport, decimal hasprint, ref string ErrMsg, int type = 0,string psql = "")
        {
            //在条码中插入打印日期
            foreach (var item in list)
            {
                if (item.EDate == DateTime.MinValue || item.EDate.ToString("yyyy/MM/dd") == "0001/01/01")
                {
                    ErrMsg = "行号：" + item.RowNo + "，" + item.RowNoDel + "的有效期不能为最小值";
                    return false;
                }
                if (item.CreateTime == DateTime.MinValue)
                    item.CreateTime = DateTime.Now;
                try
                {
                    if (item.BarcodeMType[0] == 'K')
                    {
                        item.MTYPEF = "K";
                    }
                    else
                    {
                        item.MTYPEF = item.BarcodeMType[1].ToString();
                    }
                }
                catch { }

            }


            //保存
            try
            {
                List<string> lstSql = new List<string>();
                if(psql!="")
                    lstSql.Add(psql);
                string strSql = "";

                foreach (var itemBarCode in list)
                {
                    strSql = "insert into T_OUTBARCODE(id,VOUCHERNO ,ROWNO ,ERPVOUCHERNO ,VOUCHERTYPE ,MATERIALNO ,MATERIALDESC ,CUSCODE ,CUSNAME ,SUPCODE ,SUPNAME ,OUTPACKQTY ,INNERPACKQTY ," +
                             "VOUCHERQTY ,QTY ,NOPACK ,PRINTQTY ,BARCODE ,BARCODETYPE ,SERIALNO ,BARCODENO ,OUTCOUNT ,INNERCOUNT ,MANTISSAQTY ,ISROHS ,OUTBOX_ID," +
                            "INNER_ID,ABATCHQTY ,ISDEL ,CREATER ,CREATETIME ,MODIFYER ,MODIFYTIME ,MATERIALNOID ,STRONGHOLDCODE ,STRONGHOLDNAME ,COMPANYCODE ,PRODUCTDATE ,SUPPRDBATCH ,SUPPRDDATE ," +
                            "PRODUCTBATCH ,EDATE ,STORECONDITION ,SPECIALREQUIRE ,BATCHNO ,BARCODEMTYPE ,ROWNODEL ,PROTECTWAY ,BOXWEIGHT ,UNIT ,LABELMARK ,BOXDETAIL ,MATEBATCH ,MIXDATE ,RELAWEIGHT ," +
                            "PRODUCTCLASS ,ITEMQTY ,WORKNO ,MTYPEF ,PROROWNO ,PROROWNODEL ,BOXCOUNT,erpbarcode,zxbarcode,brandno,zyID )" +
                            "values (SEQ_OUTBARCODE_ID.Nextval,'" + itemBarCode.VoucherNo + "','" + itemBarCode.RowNo + "','" + itemBarCode.ErpVoucherNo + "','" + itemBarCode.VoucherType + "','" + itemBarCode.MaterialNo + "','" + itemBarCode.MaterialDesc + "','" +
                            itemBarCode.CusCode + "','" + itemBarCode.CusName + "','" + itemBarCode.SupCode + "','" + itemBarCode.SupName + "','" + itemBarCode.OutPackQty + "','" + itemBarCode.InnerPackQty + "','" +
                            itemBarCode.VoucherQty + "','" + itemBarCode.Qty + "','" + itemBarCode.NoPack + "','" + itemBarCode.PrintQty + "','" + itemBarCode.BarCode + "','" + itemBarCode.BarcodeType + "','" + itemBarCode.SerialNo + "','" +
                            itemBarCode.BarcodeNo + "','" + itemBarCode.OutCount + "','" + itemBarCode.InnerCount + "','" + itemBarCode.MantissaQty + "','" + itemBarCode.IsRohs + "','" + itemBarCode.OutBox_ID + "','" +
                            itemBarCode.Inner_ID + "','" + itemBarCode.ABatchQty + "','" + itemBarCode.IsDel + "','" + itemBarCode.Creater + "',Sysdate,'',Sysdate,'" + itemBarCode.MaterialNoID + "','" +
                            itemBarCode.StrongHoldCode + "', '" + itemBarCode.StrongHoldName + "','" + itemBarCode.CompanyCode + "',to_date('" + itemBarCode.ProductDate.ToString("yyyy/MM/dd") + "','YYYY-MM-DD'),'" + itemBarCode.SupPrdBatch + "',to_date('" + itemBarCode.SupPrdDate + "','YYYY-MM-DD hh24:mi:ss'),'" +
                            itemBarCode.ProductBatch + "',to_date('" + itemBarCode.EDate.ToString("yyyy/MM/dd") + "','YYYY-MM-DD'),'" + itemBarCode.StoreCondition + "','" + itemBarCode.SpecialRequire + "','" + itemBarCode.BatchNo + "','" +
                            itemBarCode.BarcodeMType + "','" + itemBarCode.RowNoDel + "','" + itemBarCode.ProtectWay + "','" + itemBarCode.BoxWeight + "','" + itemBarCode.Unit + "','" + itemBarCode.LABELMARK + "','" + itemBarCode.BoxDetail + "','" + itemBarCode.MateBatch +
                            "',to_date('" + itemBarCode.MixDate + "','YYYY-MM-DD hh24:mi:ss'),'" + itemBarCode.RelaWeight + "','" + itemBarCode.ProductClass + "','" + itemBarCode.ItemQty + "','" + itemBarCode.WorkNo + "','" + itemBarCode.MTYPEF + "','" + itemBarCode.PROROWNO + "','" +
                            itemBarCode.PROROWNODEL + "','" + itemBarCode.BoxCount + "','" + itemBarCode.ErpBarCode + "','" + itemBarCode.ZXBARCODE + "' ,'" + itemBarCode.brandno + "' ,'" + itemBarCode.zyID + "')";

                    lstSql.Add(strSql);                    
                }

                if (lstSql == null || lstSql.Count == 0)
                {
                    ErrMsg = "no data";
                    return false;
                }

                int i = dbFactory.ExecuteNonQueryList(lstSql, ref ErrMsg);

                if (i <= 0)
                { 
                    return false; 
                }

                if (type == 0)
                {
                    //打印标签
                    bool res = PrintZPL(list, ipport, ref ErrMsg);
                    if (!res)
                        return false;
                }
                else if (type == 1)
                {
                    //打印标签
                    bool res = PrintZPL2(list, ipport, ref ErrMsg);
                    if (!res)
                        return false;
                }

                ErrMsg = "保存成功";
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }

        }


        public string SaveModelListSqlToDBADF(string UserJson, string ModeJson)
        {
            BaseMessage_Model<List<T_PalletDetailInfo>> model = new BaseMessage_Model<List<T_PalletDetailInfo>>();
            try
            {
                bool bSucc = false;

                string strError = "";


                if (string.IsNullOrEmpty(UserJson))
                {
                    model.HeaderStatus = "E";
                    model.Message = "传入用户信息为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);
                
                List<T_PalletDetailInfo> modelList = JSONHelper.JsonToObject<List<T_PalletDetailInfo>>(ModeJson);

                if (modelList == null || modelList.Count == 0)
                {
                    strError = "客户端传来的实体类不能为空！";
                    model.HeaderStatus = "E";
                    model.Message = strError;

                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                if (String.IsNullOrEmpty(modelList[0].PrintIPAdress))
                {
                    strError = "请设置打印机IP地址！";
                    model.HeaderStatus = "E";
                    model.Message = strError;

                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }
                                
                var rtt = GetSaveModelListSql(user, modelList);

                T_Pallet_Func tf = new T_Pallet_Func();
                bSucc = tf.SaveModelListBySqlToDB(rtt, ref strError);

                if (bSucc == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                }
                else
                {
                    model.HeaderStatus = "S";
                    model.TaskNo = modelList[0].TaskNo;
                }

                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = "保存托盘标签失败！" + ex.Message + ex.TargetSite;

                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);

            }
        }
        
        private List<string> GetSaveModelListSql(UserModel user, List<T_PalletDetailInfo> modelList)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();
            
            //查看modellist是否有托盘号，有托盘号说明是再次组托
            if (!string.IsNullOrEmpty(modelList[0].PalletNo))
            {
                modelList.ForEach(t => t.TaskNo = modelList[0].PalletNo);
                
                //再次组托，只是收货组托
                lstSql = GetInsertPalletNewSerialNoSql(user, modelList);
            }
            else
            {
                //不带托盘号，就是新组托,如果是已经入库的托盘需要插入托盘表和库存表
                int ID = GetTableID("Seq_Pallet_Id");

                int detailID = 0;

                string VoucherNoID = GetTableID("Seq_Pallet_No").ToString();

                string VoucherNo = "P" + System.DateTime.Now.ToString("yyyyMMdd") + VoucherNoID.PadLeft(4, '0');

                modelList.ForEach(t => t.TaskNo = VoucherNo);

                strSql = string.Format("insert into t_Pallet(Id, Palletno, Creater, Createtime,Strongholdcode,Strongholdname,Companycode,Pallettype,Supplierno,Suppliername,ERPVOUCHERNO)" +
                                " values ('{0}','{1}','{2}',Sysdate,'{3}','{4}','{5}','{6}','{7}','{8}','{9}')", ID, VoucherNo, user.UserNo, modelList[0].StrongHoldCode, modelList[0].StrongHoldName, modelList[0].CompanyCode, '1', modelList[0].SuppliernNo, modelList[0].SupplierName, modelList[0].ErpVoucherNo);

                lstSql.Add(strSql);

                foreach (var item in modelList)
                {
                    item.PalletNo = VoucherNo;
                    item.ProductDate = DateTime.Now;

                    foreach (var itemSerial in item.lstBarCode)
                    {
                        detailID = GetTableID("SEQ_PALLET_DETAIL_ID");
                        strSql = string.Format("insert into t_Palletdetail(Id, Headerid, Palletno, Materialno, Materialdesc, Serialno,Creater," +
                        "Createtime,RowNo,VOUCHERNO,ERPVOUCHERNO,materialnoid,qty,BARCODE,StrongHoldCode,StrongHoldName,CompanyCode,pallettype," +
                        "batchno,rownodel,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,PRODUCTBATCH,EDATE,Supplierno,Suppliername)" +
                        "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',Sysdate,'{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','1'," +
                        "'{16}','{17}',to_date('{18}','yyyy-mm-dd hh24:mi:ss'),'{19}',to_date('{20}','yyyy-mm-dd hh24:mi:ss'),'{21}',to_date('{22}','yyyy-mm-dd hh24:mi:ss'),'{23}','{24}')", detailID, ID, VoucherNo, itemSerial.MaterialNo, itemSerial.MaterialDesc, itemSerial.SerialNo, user.UserNo,
                        itemSerial.RowNo, itemSerial.VoucherNo, itemSerial.ErpVoucherNo, itemSerial.MaterialNoID, itemSerial.Qty, itemSerial.BarCode,
                        itemSerial.StrongHoldCode, itemSerial.StrongHoldName, itemSerial.CompanyCode, itemSerial.BatchNo, itemSerial.RowNoDel,
                        itemSerial.ProductDate, itemSerial.SupPrdBatch, itemSerial.SupPrdDate, itemSerial.ProductBatch, itemSerial.EDate, itemSerial.SupCode, itemSerial.SupName);

                        lstSql.Add(strSql);
                        //库存组托
                        if (itemSerial.AreaID > 0)
                        {
                            strSql = "update t_stock set palletno = '" + VoucherNo + "' where serialno = '" + itemSerial.SerialNo + "'";
                            lstSql.Add(strSql);
                        }
                    }
                }
            }

            return lstSql;
        }

        private List<string> GetInsertPalletNewSerialNoSql(UserModel user, List<T_PalletDetailInfo> modelList)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            foreach (var item in modelList)
            {
                foreach (var itemSerial in item.lstBarCode)
                {
                    strSql = "insert into t_Palletdetail (Id, Headerid, Palletno, Materialno, Materialdesc, Serialno, Creater, Createtime," +
                            "Rowno, voucherno, Erpvoucherno,  Materialnoid, Qty, Barcode, Strongholdcode, Strongholdname, Companycode, Batchno, Sn, " +
                            "Rownodel, Pallettype, Productdate, Supprdbatch, Supprddate, Productbatch, Edate,supplierno,suppliername) " +
                            "select Seq_Pallet_Detail_Id.Nextval," +
                            "(Select id from t_Pallet where palletno = '" + item.PalletNo + "'),'" + item.PalletNo + "','" + itemSerial.MaterialNo + "'," +
                            "'" + itemSerial.MaterialDesc + "','" + itemSerial.SerialNo + "',  '" + user.UserNo + "'," +
                            "sysdate,'" + itemSerial.RowNo + "','" + itemSerial.VoucherNo + "',  '" + itemSerial.ErpVoucherNo + "','" + itemSerial.MaterialNoID + "','" + itemSerial.Qty + "'," +
                            "'" + itemSerial.BarCode + "', '" + itemSerial.StrongHoldCode + "','" + itemSerial.StrongHoldName + "','" + itemSerial.CompanyCode + "','" + itemSerial.BatchNo + "', '" + itemSerial.SN + "','" + itemSerial.RowNoDel + "'," +
                            "'1', to_date('" + itemSerial.ProductDate + "','yyyy-mm-dd hh24:mi:ss'),'" + itemSerial.SupPrdBatch + "',to_date('" + itemSerial.SupPrdDate + "','yyyy-mm-dd hh24:mi:ss'),'" + itemSerial.ProductBatch + "',to_date('" + itemSerial.EDate + "','yyyy-mm-dd hh24:mi:ss'),'" + itemSerial.SupCode + "','" + itemSerial.SupName + "'" +
                            "from dual where not Exists (select 1 from t_Palletdetail b where b.Serialno='" + itemSerial.SerialNo + "')";

                    lstSql.Add(strSql);
                }
            }
            return lstSql;
        }

        public int GetTableID(string strSeq)
        {
            try
            {
                string strSql = "select " + strSeq + ".Nextval  from dual";

                using (IDataReader reader = dbFactory.ExecuteReader(strSql))
                {
                    if (reader.Read())
                    {
                        int ID = reader["Nextval"].ToInt32();
                        return ID;
                    }
                    else
                    {
                        throw new Exception("取单据ID出错！");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion 成品、半制品和散装品打印


        //ymh
        /// <summary>
        /// PDA成品、半制品和散装品打印
        /// </summary>
        /// <param name="UserJson"></param>
        /// <param name="json">打印类型是成品托盘的时候，是T_PalletDetailInfo类</param>
        /// <returns></returns>
        public string GetCombinpalletno(string UserJson, string json)
        {
            BaseMessage_Model<string> bm = new BaseMessage_Model<string>();
            string j = "";
            try
            {
                    var templst = Check_Func.DeserializeJsonToList<T_PalletDetailInfo>(json);
                    if (templst.Count == 0)
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "数据不能为空";
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }

                    //生成托盘标签/或 更新托盘标签
                    var rtstring = SaveCombinModelListSqlToDBADF(UserJson, json);

                    var rttemp = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_PalletDetailInfo>>>(rtstring);

                    if (rttemp.HeaderStatus == "E")
                    {
                        bm.HeaderStatus = "E";
                        bm.Message = "生成托盘标签失败";
                        j = Check_Func.SerializeObject(bm);
                        return j;
                    }

                    bm.HeaderStatus = "S";
                    bm.Message = rttemp.TaskNo;
                    j = Check_Func.SerializeObject(bm);
                    return j;
                
            }
            catch (Exception ex)
            {
                bm.HeaderStatus = "E";
                bm.Message = ex.ToString();
                j = Check_Func.SerializeObject(bm);
                return j;
            }

        }


        //ymh
        public string SaveCombinModelListSqlToDBADF(string UserJson, string ModeJson)
        {
            BaseMessage_Model<List<T_PalletDetailInfo>> model = new BaseMessage_Model<List<T_PalletDetailInfo>>();
            try
            {
                bool bSucc = false;
                string strError = "";
                if (string.IsNullOrEmpty(UserJson))
                {
                    model.HeaderStatus = "E";
                    model.Message = "传入用户信息为空！";
                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }

                UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);

                List<T_PalletDetailInfo> modelList = JSONHelper.JsonToObject<List<T_PalletDetailInfo>>(ModeJson);

                if (modelList == null || modelList.Count == 0)
                {
                    strError = "客户端传来的实体类不能为空！";
                    model.HeaderStatus = "E";
                    model.Message = strError;

                    return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
                }


                var rtt = GetSaveModelListSql(user, modelList);

                T_Pallet_Func tf = new T_Pallet_Func();
                bSucc = tf.SaveModelListBySqlToDB(rtt, ref strError);

                if (bSucc == false)
                {
                    model.HeaderStatus = "E";
                    model.Message = strError;
                }
                else
                {
                    model.HeaderStatus = "S";
                    model.TaskNo = modelList[0].TaskNo;
                }

                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);
            }
            catch (Exception ex)
            {
                model.HeaderStatus = "E";
                model.Message = "保存托盘标签失败！" + ex.Message + ex.TargetSite;

                return JSONHelper.ObjectToJson<BaseMessage_Model<List<T_PalletDetailInfo>>>(model);

            }
        }

    }
}



