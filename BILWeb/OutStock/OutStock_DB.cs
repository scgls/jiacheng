
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.User;
using BILBasic.DBA;
using BILBasic.Common;
using Oracle.ManagedDataAccess.Client;
using BILWeb.OutStockTask;
using System.Data;

namespace BILWeb.OutStock
{
    public partial class T_OutStock_DB : BILBasic.Basing.Factory.Base_DB<T_OutStockInfo>
    {

        /// <summary>
        /// 添加t_outstock
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_OutStockInfo t_outstock)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user,ref T_OutStockInfo model)
        {
            string strSql = string.Empty;
            List<string> lstSql = new List<string>();

            //更新
            if (model.ID > 0)
            {
                strSql = string.Format("update t_Outstock a set a.Plant='{0}',a.Plantname='{1}',a.Modifyer = '{2}',a.Modifytime = getdate(),a.note = '{3}',a.Address = '{4}',a.Address1 = '{5}',a.Contact = '{6}',a.Phone = '{7}',a.ShipNFlg = '{8}',a.ShipWFlg = '{9}',a.TRAD_NAME = '{10}',a.ERPNote = '{11}',a.Province = '{12}',a.City = '{13}',a.Area = '{14}',a.ShipPFlg = '{15}',a.ShipDFlg = '{16}'  where a.Id = '{17}'", 
                     model.Plant, model.PlantName,  user.UserNo,model.Note, model.Address, model.Address1, model.Contact, model.Phone, model.ShipNFlg, model.ShipWFlg, model.TradingConditionsName, model.ERPNote, model.Province, model.City, model.Area, model.ShipPFlg, model.ShipDFlg, model.ID);
                lstSql.Add(strSql);
            }
            else //插入
            {
                int voucherID = base.GetTableID("seq_outstock_id");

                model.ID = voucherID.ToInt32();

                string VoucherNoID = base.GetTableID("seq_outstock_no").ToString();

                string VoucherNo ="F"+ System.DateTime.Now.ToString("yyyyMMdd") + VoucherNoID.PadLeft(4, '0');

                strSql = string.Format("insert into t_Outstock (Id,  Vouchertype, Customercode, Plant, Plantname,  Creater, Createtime, Isdel,voucherno,status,note,CustomerName)" +
                    " values('{0}','{1}','{2}','{3}','{4}','{5}',getdate(),1,'{6}','1','{7}','{8}')", voucherID, model.VoucherType, model.CustomerCode, model.Plant, model.PlantName, user.UserNo,VoucherNo,model.Note,model.CustomerName);

                lstSql.Add(strSql);
            }

            return lstSql;
        }

        //protected override List<string> GetUpdateSql(UserModel user, T_OutStockInfo model)
        //{
        //    List<string> lstSql = new List<string>();

        //    string strSql = "update t_Outstock a set a.Status = '" + model.Status + "' where id = '" + model.ID + "'";

        //    lstSql.Add(strSql);

        //    return lstSql;
        //}


        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_OutStockInfo ToModel(IDataReader reader)
        {
            T_OutStockInfo t_outstock = new T_OutStockInfo();
            T_OutStockInfo t_outstockwei = new T_OutStockInfo();

            t_outstock.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_outstock.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ERPVOUCHERNO");
            t_outstock.VoucherType = dbFactory.ToModelValue(reader, "VOUCHERTYPE").ToInt32();
            t_outstock.CustomerCode = (string)dbFactory.ToModelValue(reader, "CUSTOMERCODE");
            t_outstock.CustomerName = (string)dbFactory.ToModelValue(reader, "CUSTOMERNAME");
            t_outstock.IsOutStockPost = (decimal?)dbFactory.ToModelValue(reader, "ISOUTSTOCKPOST");
            t_outstock.IsUnderShelvePost = (decimal?)dbFactory.ToModelValue(reader, "ISUNDERSHELVEPOST");
            t_outstock.Plant = (string)dbFactory.ToModelValue(reader, "PLANT");
            t_outstock.PlantName = (string)dbFactory.ToModelValue(reader, "PLANTNAME");
            t_outstock.MoveType = (string)dbFactory.ToModelValue(reader, "MOVETYPE");
            t_outstock.SupCode = (string)dbFactory.ToModelValue(reader, "SUPPLIERNO");
            t_outstock.SupName = (string)dbFactory.ToModelValue(reader, "SUPPLIERNAME");
            t_outstock.DepartmentCode = (string)dbFactory.ToModelValue(reader, "DEPARTMENTCODE");
            t_outstock.DepartmentName = (string)dbFactory.ToModelValue(reader, "DEPARTMENTNAME");
            t_outstock.MoveReasonCode = (string)dbFactory.ToModelValue(reader, "MOVEREASONCODE");
            t_outstock.MoveReasonDesc = (string)dbFactory.ToModelValue(reader, "MOVEREASONDESC");
            t_outstock.ReviewStatus = dbFactory.ToModelValue(reader, "REVIEWSTATUS").ToInt32();
            t_outstock.OutStockDate = (DateTime?)dbFactory.ToModelValue(reader, "OUTSTOCKDATE");
            t_outstock.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            t_outstock.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            t_outstock.Modifyer = (string)dbFactory.ToModelValue(reader, "Modifyer");
            t_outstock.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "ModifyTime");
            t_outstock.VoucherNo = (string)dbFactory.ToModelValue(reader, "VoucherNo");
            t_outstock.StrVoucherType = (string)dbFactory.ToModelValue(reader, "StrVoucherType");
            t_outstock.StrStatus = (string)dbFactory.ToModelValue(reader, "StrStatus");
            t_outstock.StrCreater = (string)dbFactory.ToModelValue(reader, "StrCreater");
            t_outstock.Note = (string)dbFactory.ToModelValue(reader, "Note");
            t_outstock.Status = dbFactory.ToModelValue(reader, "Status").ToInt32();
            t_outstock.ERPStatus = dbFactory.ToModelValue(reader, "ERPStatus").ToDBString();

            t_outstock.FromShipmentDate = (DateTime?)dbFactory.ToModelValue(reader, "SHIPMENTDATE");
            t_outstock.StrongHoldCode = dbFactory.ToModelValue(reader, "StrongHoldCode").ToDBString();
            t_outstock.StrongHoldName = dbFactory.ToModelValue(reader, "StrongHoldName").ToDBString();
            t_outstock.CompanyCode = dbFactory.ToModelValue(reader, "CompanyCode").ToDBString();
            t_outstock.DepartmentCode = (string)dbFactory.ToModelValue(reader, "DepartmentCode");
            t_outstock.DepartmentName = (string)dbFactory.ToModelValue(reader, "DepartmentName");
            t_outstock.StockQty = (decimal?)dbFactory.ToModelValue(reader, "StockQty");
            t_outstock.OutStockQty = (decimal?)dbFactory.ToModelValue(reader, "OutStockQty");
            t_outstock.CompanyCode = (string)dbFactory.ToModelValue(reader, "CompanyCode");
            t_outstock.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "StrongHoldCode");
            t_outstock.StrongHoldName = (string)dbFactory.ToModelValue(reader, "StrongHoldName");

            t_outstock.Province = dbFactory.ToModelValue(reader, "Province").ToDBString();
            t_outstock.City = dbFactory.ToModelValue(reader, "City").ToDBString();
            t_outstock.Area = dbFactory.ToModelValue(reader, "Area").ToDBString();
            t_outstock.Address = dbFactory.ToModelValue(reader, "Address").ToDBString();
            t_outstock.Phone = dbFactory.ToModelValue(reader, "Phone").ToDBString();
            t_outstock.Contact = dbFactory.ToModelValue(reader, "Contact").ToDBString();
            t_outstock.Address1 = dbFactory.ToModelValue(reader, "Address1").ToDBString();

            //t_outstock.ShipNFlg = dbFactory.ToModelValue(reader, "ShipNFlg").ToDBString();
            //t_outstock.ShipDFlg = dbFactory.ToModelValue(reader, "ShipDFlg").ToDBString();
            //t_outstock.ShipWFlg = dbFactory.ToModelValue(reader, "ShipWFlg").ToDBString();
            //t_outstock.ShipPFlg = dbFactory.ToModelValue(reader, "ShipPFlg").ToDBString();
            
            //t_outstock.TradingConditions = dbFactory.ToModelValue(reader, "TradingConditions").ToDBString();
            //t_outstock.TradingConditionsName = dbFactory.ToModelValue(reader, "trad_name").ToDBString();
            t_outstock.strReviewUserNo = dbFactory.ToModelValue(reader, "strReviewUserNo").ToDBString();
            //t_outstock.fydocno = dbFactory.ToModelValue(reader, "fydocno").ToDBString();
            //t_outstock.hmdocno = dbFactory.ToModelValue(reader, "hmdocno").ToDBString();
            t_outstock.VouUser = dbFactory.ToModelValue(reader, "VouUser").ToDBString();
            t_outstock.ERPNote = dbFactory.ToModelValue(reader, "ERPNote").ToDBString();
            t_outstock.PostDate = (DateTime?)dbFactory.ToModelValue(reader, "PostDate");
            //t_outstock.FromErpWarehouse = dbFactory.ToModelValue(reader, "FromErpWarehouse").ToDBString();
            //t_outstock.TOTALAMT = dbFactory.ToModelValue(reader, "TOTALAMT").ToDBString();
            //t_outstock.TOTALNUM = dbFactory.ToModelValue(reader, "TOTALNUM").ToDBString();
            t_outstock.ERPVoucherType = dbFactory.ToModelValue(reader, "ERPVoucherType").ToDBString();

            //t_outstock.TOTALAMT1 = dbFactory.ToModelValue(reader, "TOTALAMT1").ToDBString();
            //t_outstock.TOTALNUM1 = dbFactory.ToModelValue(reader, "TOTALNUM1").ToDBString();
            //t_outstock.DISPRICE = dbFactory.ToModelValue(reader, "DISPRICE").ToDBString();
            //t_outstock.DISPRICE1 = dbFactory.ToModelValue(reader, "DISPRICE1").ToDBString();
            t_outstock.FromErpWarehouseName = dbFactory.ToModelValue(reader, "FromErpWareHouseName").ToDBString();
            t_outstock.ToErpWarehouseName = dbFactory.ToModelValue(reader, "ToErpWareHouseName").ToDBString();
            t_outstock.FromErpWarehouse = dbFactory.ToModelValue(reader, "FromErpWareHouse").ToDBString();
            t_outstock.ToErpWarehouse = dbFactory.ToModelValue(reader, "ToErpWarehouse").ToDBString();

            t_outstockwei = GetWeightVolum(t_outstock);
            t_outstock.Weight = t_outstockwei.Weight;
            t_outstock.Volume = t_outstockwei.Volume;

            return t_outstock;
        }


        private T_OutStockInfo GetWeightVolum(T_OutStockInfo model)
        {
            T_OutStockInfo boxmodel = new T_OutStockInfo();
            string strSql = "select (ISNULL(B.WEIGHT,0) * A.OUTSTOCKQTY ) AS WEIGHT, ( ISNULL(B.VOLUME,0) * A.OUTSTOCKQTY ) AS VOLUME from ( "+
                         "  select MATERIALNO,sum(OUTSTOCKQTY) as OUTSTOCKQTY from T_OUTSTOCKDETAIL where headerid = '"+model.ID+"' group by MATERIALNO "+
                          " ) a left join T_MATERIAL b on a.MATERIALNO = b.MATERIALNO";

            using (IDataReader dr = dbFactory.ExecuteReader(System.Data.CommandType.Text, strSql))
            {
                while (dr.Read())
                {
                    boxmodel.Weight = dbFactory.ToModelValue(dr, "Weight").ToDecimal();
                    boxmodel.Volume = dbFactory.ToModelValue(dr, "Volume").ToDecimal();
                }
            }

            return boxmodel;
        }

        protected override string GetViewName()
        {
            return "V_OUTSTOCK";
        }

        protected override string GetTableName()
        {
            return "T_OUTSTOCK";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }


        protected override string GetFilterSql(UserModel user, T_OutStockInfo model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";

            
            if (model.Status > 0)
            {
                if (model.PcOrPda == 1)//PC请求
                {
                    strSql += strAnd;
                    strSql += "isnull(status,1)= '" + model.Status + "'";
                }
                else 
                {
                    if (model.Status == 3 || model.Status == 4 || model.Status == 2)
                    {
                        strSql += strAnd;
                        strSql += " (isnull(status,1)=2 or isnull(status,1)=3 or isnull(status,1)=4) ";
                    }
                    else
                    {
                        strSql += strAnd;
                        strSql += "isnull(status,1)= '" + model.Status + "'";
                    }
                }                
            }

            if (model.FromErpWarehouse != null&&model.FromErpWarehouse != "0")
            {
                strSql += strAnd;
                strSql += " FromErpWarehouse = '" + model.FromErpWarehouse + "'";
            }

            if (model.FromShipmentDate != null)
            {
                strSql += strAnd;
                strSql += " ShipmentDate >= " + model.FromShipmentDate.ToDateTime().Date.ToOracleTimeString() + " ";
            }

            if (model.ToShipmentDate != null)
            {
                strSql += strAnd;
                strSql += " ShipmentDate <= " + model.ToShipmentDate.ToDateTime().Date.AddDays(1).ToOracleTimeString() + " ";
            }

            if (!string.IsNullOrEmpty(model.CustomerName))
            {
                strSql += strAnd;
                strSql += " (CustomerCode Like '" + model.CustomerName.Trim() + "%'  or CustomerName Like '" + model.CustomerName.Trim() + "%' )";
            }

            if (model.DateFrom != null)
            {
                strSql += strAnd;
                strSql += " CreateTime >= " + model.DateFrom.ToDateTime().Date.ToOracleTimeString() + " ";
            }

            if (model.DateTo != null)
            {
                strSql += strAnd;
                strSql += " CreateTime <= " + model.DateTo.ToDateTime().Date.AddDays(1).ToOracleTimeString() + " ";
            }

            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " erpvoucherno = '" + model.ErpVoucherNo.Trim() + "'  ";
            }

            if (!string.IsNullOrEmpty(model.VoucherNo))
            {
                strSql += strAnd;
                strSql += " voucherno like '%" + model.VoucherNo.Trim() + "%'  ";
            }

            if (model.VoucherType > 0)
            {
                strSql += strAnd;
                strSql += " vouchertype ='" + model.VoucherType + "'  ";
            }

            if (!string.IsNullOrEmpty(model.TradingConditionsName))
            {
                strSql += strAnd;
                strSql += " trad_name like '%" + model.TradingConditionsName + "%'  ";
            }

            if (model.FromErpWarehouse != null && model.FromErpWarehouse != "0")
            {
                strSql += strAnd;
                strSql += " FromErpWarehouse ='"+model.FromErpWarehouse+"'";
            }

            if (model.ToErpWarehouse != null && model.ToErpWarehouse != "0")
            {
                strSql += strAnd;
                strSql += " ToErpWarehouse ='" + model.ToErpWarehouse + "'";
            }

            if (model.ToErpWarehouseName != null && model.ToErpWarehouseName != "0")
            {
                strSql += strAnd;
                strSql += " ToErpWarehouseName like '%" + model.ToErpWarehouseName + "%'";
            }


            //if (model.StrongHoldType == 1)
            //{
            //    strSql += strAnd;
            //    strSql += " StrongHoldCode ='CY1'";
            //}

            //if (model.StrongHoldType == 2)
            //{
            //    strSql += strAnd;
            //    strSql += " StrongHoldCode ='CX1'";
            //}

            //if (model.MStockStatus == 1)
            //{
            //    strSql += strAnd;
            //    strSql += " stockqty = 0 ";
            //}

            //if (model.MStockStatus == 2)
            //{
            //    strSql += strAnd;
            //    strSql += " stockqty  > 0 and stockqty < OutStockQty ";
            //}

            //if (model.MStockStatus == 3)
            //{
            //    strSql += strAnd;
            //    strSql += " stockqty  > 0 and stockqty >= OutStockQty ";
            //}

            return strSql; //+ " order by id desc ";
        }

        #region 获取一张销售出库单和明细列表----------PC端----------------导出装箱单用！！！
        public bool GetOutStockAndDetailsModelByNo(string erpNo, ref BILWeb.OutStockTask.T_OutStockTaskInfo head, ref List<BILWeb.OutStockTask.T_OutStockTaskDetailsInfo> lstDetail, ref string ErrMsg)
        {
            try
            {
                string strSql = "";

                head = new BILWeb.OutStockTask.T_OutStockTaskInfo();

                strSql = "select m.erpbarcode,t.materialno,t.materialdesc,t.edate,t.batchno,t.erpvoucherno,sum(t.qty) as sumQty,count(t.serialno) as countQty from t_tasktrans t " +
                    " inner join t_material m on m.id = t.materialnoid " +
                    " where t.tasktype=12 and t.vouchertype=24 and t.erpvoucherno in (" + erpNo + ") " +
                    " group by t.erpvoucherno,m.erpbarcode,t.materialno,t.materialdesc,t.edate,t.batchno " +
                    " order by t.erpvoucherno,t.materialno,t.batchno ";

                lstDetail = new List<BILWeb.OutStockTask.T_OutStockTaskDetailsInfo>();

                using (IDataReader dr = dbFactory.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        lstDetail.Add(new BILWeb.OutStockTask.T_OutStockTaskDetailsInfo()
                        {
                            MaterialNo = dr["materialno"].ToDBString(),
                            MaterialDesc = dr["materialdesc"].ToDBString(),
                            TaskQty = dr["sumQty"].ToDecimal(),//合计数量
                            QualityQty = dr["countQty"].ToDecimal(),//合计件数
                            ErpDocNo = dr["erpvoucherno"].ToDBString(),//销货单号
                            BatchNo = dr["batchno"].ToDBString(),//批号
                            Remark = dr["erpbarcode"].ToDBString(),//条码
                            CompleteDateTime = dr["edate"].ToDateTime()//产品限用日期
                        });
                    }
                }

                if (lstDetail.Count <= 0)
                {
                    ErrMsg = "没有拣货单明细数据！";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }
        #endregion

        public bool GetModelListByCar(string strCarNo, ref T_OutStockInfo model ,ref string strError) 
        {
            string strFilter = "   erpvoucherno = ( select erpvoucherno from t_task where taskno = (select taskno from t_Pickcar where carno = '"+strCarNo+"') )";

            string strFilter1 = "erpvoucherno like '%" + strCarNo + "%'";

            //扫描批次标签
            if (strCarNo.Contains("@"))
            {
                string strSerialNo = OutBarCode.OutBarCode_DeCode.GetEndSerialNo(strCarNo);
                string strFilter2 = "   erpvoucherno = (select erpvoucherno from t_Taskdetails where id  = (select a.Taskdetailesid from t_stock a where a.Serialno = '" + strSerialNo + "'))";
                model = base.GetModelByFilter(strFilter2);
            }
            else 
            {
                //先查小车
                //model = base.GetModelByFilter(strFilter);

                //if (model == null)
                //{
                //    //再查ERP单号
                //    model = base.GetModelByFilter(strFilter1);
                //}
                //再查ERP单号
                model = base.GetModelByFilter(strFilter1);

            }

            if (model == null) 
            {
                strError = "该单据编号不存在或者小车编码未关联拣货单！" ;
                return false;
            }

            model.lstDetail = new List<T_OutStockDetailInfo>();

            T_OutTaskDetails_DB tdb = new T_OutTaskDetails_DB();
            List<T_OutStockTaskDetailsInfo> modelListTaskDetail = new List<T_OutStockTaskDetailsInfo>();

            if (tdb.GetOutTaskDetailByErpVoucherNo(model.ErpVoucherNo, ref modelListTaskDetail, ref strError) == false) 
            {
                return false;
            }

            T_OutStockDetail_DB odb = new T_OutStockDetail_DB();
            model.lstDetail = odb.CreateOutStockDetailByTaskDetail(modelListTaskDetail);
            model.ToErpWarehouse = model.lstDetail != null ? model.lstDetail[0].ToErpWarehouse : string.Empty;

            //T_OutStockDetail_DB _db = new T_OutStockDetail_DB();
            ////model.lstDetail = _db.GetModelListByHeaderIDForCar(model.ID);
            //T_OutStockDetail_Func tfunc = new T_OutStockDetail_Func();
            //model.lstDetail = tfunc.OutStockSameMaterialNoSumQty(_db.GetModelListByHeaderIDForCar(model.ID));

            return true;
        }

        public T_OutStockInfo  GetOutStockDetailForPrint(string strErpVoucherNo)
        {
            string strFilter1 = "erpvoucherno = '" + strErpVoucherNo + "'";
            return base.GetModelByFilter(strFilter1);
        }

        public int GetOutStockNoIsExists(string strErpVoucherNo) 
        {
            string strSql = "select count(1) from t_Outstock where erpvoucherno = '" + strErpVoucherNo + "'";
            return base.GetScalarBySql(strSql).ToInt32();
        }

        protected override List<string> GetUpdateSql(UserModel user, T_OutStockInfo model)
        {
            List<string> lstSql = new List<string>();
            //关闭任务之前判断有没有开始拣货
            string strSql1 = string.Empty;
            int iCount = 0;

            //已经生单，或者已经复核 需要关闭拣货任务
            if (model.Status == 2 || model.Status==3 || model.Status==4 ) 
            {
                strSql1 = "update t_task  set status = 5 where erpvoucherno = '"+model.ErpVoucherNo+"'";
                lstSql.Add(strSql1);
            }

            strSql1 = "select count(1) from t_task a where a.Erpvoucherno = '"+model.ErpVoucherNo+"' and (a.Status=2 or a.Status=3)";
            iCount = base.GetScalarBySql(strSql1).ToInt32();

            //存在开始拣货的拣货单，需要释放库存
            if (iCount > 0) 
            {
                strSql1="update t_stock set Taskdetailesid = 0 where Taskdetailesid IN ( "+
                        " select b.Id from t_task a left join t_Taskdetails b on a.Id = b.Headerid where a.Erpvoucherno='"+model.ErpVoucherNo+"' )";
                lstSql.Add(strSql1);

                strSql1 = "delete  T_TASKTRANSDETAIL where ERPVOUCHERNO = '" + model.ErpVoucherNo + "'";
                lstSql.Add(strSql1);
            }

            string strSql = "update t_Outstock  set Status='6',Closename = '"+user.UserName+"',Closedate=getdate() where id = '"+model.ID+"'";

            lstSql.Add(strSql);

            return lstSql;
        }

        //public int insertLog(string message)
        //{
        //    string strSql = "insert into t_log (message,createtime) value ('"+message+ "',GETDATE())";
        //    return base.GetScalarBySql(strSql).ToInt32();
        //}
    }
}
