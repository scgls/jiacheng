using BILWeb.OutBarCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using BILBasic.Common;
using BILWeb.Stock;

namespace BILWeb.Print
{
    public partial class Print_DB
    {
        public static string glasses = "~DGR:glasses,891,11,000000000000000000000000000000000000000000000000000000000000000000000000001FFF800000000000000001FFFFF8000000000000000FFFFFFF000000000000003FFFFFFFC0000000000000FFFFFFFFF0000000000003FFFFFFFFFC000000000007FFFE07FFFE00000000001FFFE000FFFF80000000003FFF00001FFFC0000000007FFC000007FFE000000000FFF0000003FFF000000001FFE0000000FFF800000003FFC00000007FFC00000007FF800000003FFE0000000FFF000000001FFE0000000FFF0000000007FF0000001FFE0000000003FF8000003FFC0000000003FF8000003FF80000000001FFC000007FF80000000001FFE000007FF00000000000FFE00000FFF00000000FC0FFF00000FFF00000003FE0FFF00001FFE00000001FE0FFF80001FFE03FE000FFF0FFF80001FFE0FFFFFFFFF0FFF80003FFE0FFFFFFFFF0FFFC0003FFF1FFFFFFFFF0FFFC0003FFF1FFFFFFFFF0FFFC0003FFF1FFFFFFFFF0FFFC0007FFF1FFFFFFFFF1FFFC0007FFF1FFFFFFFFF1FFFC0007FFF9FFFFFFFFE3FFFE0007FFF9FC0FFC07E3FFFE0007FFF18000000031FFFE0007FFF10000000010FFFE0007FFE000000000087FFE0007FFC000000000087FFE0007FFC200000000083FFE0007FFC60000E0000E3FFE0007FFCE0001E0001E3FFE0007FFC70000E0001E7FFE0007FFE70001E0001C7FFC0003FFE70001F0003C7FFC0003FFE38003F80038FFFC0003FFF3C00FFC0070FFFC0003FFF1FFFFFFFFE1FFFC0001FFF87FFFFFFFC1FFF80001FFFC3FFFFFFFC7FFF80001FFFE1FFFFFFF8FFFF80000FFFF8FFFFFFF9FFFF00000FFFF8FFFFFFF1FFFF00000FFFF8FFFFFFF1FFFE000007FFFCFFFFFFF3FFFE000003FFFC7FFFFFE3FFFC000003FFFC7FFFFFC3FFFC000001FFFE3FFFFFC7FFF8000001FFFE3FFFFF87FFF8000000FFFE1FFFFF8FFFF00000007FFF1FFFFF1FFFE00000007FFF8FFFFE1FFFC00000001FFFC3FFFC3FFF800000000FFFC1FFF87FFF000000000FFFF0FFF0FFFE0000000003FFF80001FFFC0000000001FFFC0003FFF80000000000FFFE0007FFF000000000007FFFFFFFFFC000000000001FFFFFFFFF80000000000007FFFFFFFE00000000000001FFFFFFF8000000000000007FFFFFC0000000000000000FFFFE0000000000000000003F800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
        public static string mask = "~DGR:mask,790,10,00000000000000000000000000007FFF0000000000000003FFFFF00000000000001FFFFFFC000000000000FFFFFFFF800000000001FFFFFFFFE00000000007FFFFFFFFF0000000000FFFFFFFFFFC000000003FFFFFE3FFFF000000007FFFF0001FFF80000000FFFF000007FFC0000001FFFE003C01FFE0000003FFFC1FFFE0FFF0000007FFF87FFFF83FF800000FFFF8FFFFFC3FF800001FFFF1FFFFFE1FFC00003FFFE1FFFFFF0FFE00003FFFC3FFFFFE07FF00007FFF87FFFFFC07FF00007FFF0FFFFFF043FF8000FFFF1FFFFFE0E3FF8000FFFF3FFFFFC3E3FFC001FFFE3FFFFF07E1FFC001FFFE3FFFFE0FF1FFE003FFFE3FFFFC1FF1FFE003FFFE3FFFF03FF1FFF003FFFE3FFFE0FFF1FFF007FFFE3FFFC1FFF1FFF007FFFC3FFF03FFF1FFF807FFF81FFE0FFFF9FFF807FFF003FC1FFFF1FFF80FFFF000E83FFFF1FFF80FFFE00024FFFFF1FFF80FFFC00011FFFFF1FFF80FFFC0000BFFFFE3FFF80FFFC00007FFFFC3FFFC0FFFC00007FFFF87FFFC0FFFC00007FFFF0FFFFC0FFFC00007FFFE1FFFFC0FFFE00003FFFE3FFFFC0FFFE00003FFFC7FFFFC0FFFE00003FFF87FFFF80FFFE00003FFF8FFFFF80FFFE000025FF8FFFFF80FFFE000020031FFFFF807FFE000000001FFFFF807FFE000000001FFFFF807FFF00003FFE3FFFFF007FFF00007FFE3FFFFF003FFF8000FFFE3FFFFF003FFFC003FFFE3FFFFE003FFFC003FFFE3FFFFE001FFFE001FFFE3FFFFE001FFFE000FFFE3FFFFC000FFFF078FFFE3FFFFC000FFFFFF8FFFE3FFFF80007FFFFF8FFFC1FFFF80007FFFFF8FFC01FFFF00003FFFFF8FF003FFFE00001FFFFF8F803FFFFE00000FFFFF8C01FFFFFC000007FFFF8007FFFFF8000007FFFF003FFFFFF0000003FFFF00FFFFFFE0000001FFFF07FFFFFFC0000000FFFF1FFFFFFF800000003FFF3FFFFFFF000000001FFFFFFFFFFC000000000FFFFFFFFFF80000000003FFFFFFFFE00000000000FFFFFFFFC000000000003FFFFFFF0000000000000FFFFFF800000000000000FFFFC00000000000000007F00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
        public static string cloth = "~DGR:cloth,800,10,00000000000000000000000000000000000000000000000000000000000000000000FFF8000000000000000FFFFF800000000000007FFFFFF0000000000001FFF87FFE000000000007FFE01FFF80000000001FFFC70FFFC0000000003FFF8FCFFFF000000000FFFF9FE7FFF800000001FFFF1FE7FFFC00000003FFFF3FE7FFFF00000007FFFF1FE7FFFF8000000FFFFF9FE7FFFF8000001FFFFF9FC7FFFFE000003FFFFF8787FFFFE000003FFFFF8007FFFFF000007FFFFF800FFFFFF80000FFFFFF800FFFFFFC0001FFFFFFC00FFFFFFC0001FFFFFFC00FFFFFFE0003FFFFFC0001FFFFFE0007FFFFE020003FFFFF0007FFFFC000000FFFFF0007FFFF8000000FFFFF800FFFFF80000007FFFF800FFFFF00100007FFFFC00FFFFF03F07F07FFFFC01FFFFF00800C07FFFFC01FFFFF00000003FFFFE01FFFFF00000003FFFFE03FFFFE00000003FFFFE03FFFFE00000001FFFFE03FFFFE00000001FFFFE03FFFFC00000001FFFFE03FFFFC00000001FFFFF03FFFFC08000080FFFFF03FFFF808000080FFFFF03FFFF8080000C0FFFFF03FFFF81C0000C07FFFF03FFFF8180000C07FFFF03FFFF0180000E07FFFF03FFFF01C0000E07FFFF03FFFF0380000E03FFFE03FFFF0380000F03FFFE03FFFE0380000F03FFFE01FFFE0780000F03FFFE01FFFE0780000F81FFFE01FFFE0F80000F81FFFC01FFFC0F80000F81FFFC00FFFC0F80000FC1FFFC00FFFC1F80000FC1FFF800FFFE1F80000FE1FFF8007FFFFF80000FFFFFF8007FFFFF80000FFFFFF0003FFFFF80000FFFFFF0003FFFFF80000FFFFFE0001FFFFF80000FFFFFE0001FFFFF80000FFFFFC0000FFFFF80000FFFFF800007FFFF80000FFFFF800003FFFF80200FFFFF000003FFFF80200FFFFE000001FFFF80200FFFFC000000FFFF80200FFFF80000007FFF80200FFFF00000003FFF80300FFFE00000001FFF80300FFFC000000007FF80300FFF8000000003FF80300FFE0000000000FF80300FFC00000000007FF0303FF000000000000FFFFFFFC0000000000003FFFFFF000000000000007FFFF80000000000000003FE000000000000000000000000000000000000000000000000000000000000000000000";
        public static string glove = "~DGR:glove,790,10,00000000FFFC0000000000000007FFFFC00000000000003FFFFFF8000000000001FFFFFFFF000000000007FFFFFFFF80000000000FFFFFFFFFE0000000003FFFFFFFFFF8000000007FFFFFFEFFFC00000000FFFFFF183FFE00000001FFFFC40001FF00000003FFFF00C380FF80000007FFFF11E39CFFC000000FFFFF39E79CFFE000001FFFFE39E79CFFF000003FFFFE71E73CFFF800007FFFFE73E73CFFFC0000FFFFFE73C73CFFFE0000FFFFFFF3CF38FFFE0001FFFFFFF3CF39FFFF0001FFFFFFFFFF79FFFF0003FFFFFF8FFFF9FFFF8003FFFFF107FFF9FFFF8007FFFFE107FFF9FFFF8007FFFFA007FFF9FFFFC007FFFE1083FC19FFFFC00FFFFE0003F819FFFFE00FFFFE0001F389FFFFE00FFFF20841E3CBFFFFE01FFFE00000C79BFFFFE01FFFE000208F93FFFFF01FFFE00020DF33FFFFF01FFFE000007E33FFFFF01FFFE000107E73FFFFF01FFFF002007C7BFFFFF01FFFF0020038F9FFFFF01FFFF0000031F9FFFFF01FFFF0000011F9FFFFF01FFFF0000013F9FFFFF01FFFF000001FF9FFFFF01FFFF800000FF9FFFFF01FFFF800000FFCFFFFF01FFFF8000007FCFFFFF01FFFF8000007FCFFFFF01FFFFC000003FE7FFFF01FFFFC000003FE7FFFF00FFFFC000001FE7FFFE00FFFFE000001FF3FFFE00FFFFE000000FFBFFFE00FFFFF000000EA9FFFE007FFFF000000000FFFC007FFFF8000007FFFFFC003FFFF8000003FFFFF8003FFFFC000003FFFFF8003FFFFC000001FFFFF0001FFFFE000001FFFFF0000FFFFE000000FFFFF0000FFFFF0000007FFFE00007FFFF0000007FFFC00003FFFF0000003FFF800003FFFF0000001FFF800001FFFF8000003FFF000000FFFF80003FFFFE0000007FFF8003FFFFFC0000003FFF803FFFFFF80000001FFF81FFFFFFF00000000FFFCFFFFFFFE000000007FFFFFFFFFFC000000001FFFFFFFFFF0000000000FFFFFFFFFC00000000003FFFFFFFF800000000000FFFFFFFE0000000000001FFFFFF000000000000003FFFF80000000000000001FF0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";

     
       
    
        public static string 产品编号 = "产品编号Pro.Code";
        public static string 产品名称 = "产品名称Pro.Name";
        public static string 订单号 = "订单号PO#";
        public static string 供应商 = "供应商Supplier";
        public static string 供应商批号 = "供应商批号QS Lot";
        public static string 生产日期 = "生产日期Pro.date";
        public static string 厂内批号 = "厂内批号Lot";
        public static string 到期日期 = "到期日期Exp.date";
        public static string 存储条件 = "存储条件conditon";
        public static string 防护措施 = "防护措施Precaution";
        public static string 特殊要求 = "特殊要求Attention";
        public static string 接收日期 = "接收日期Rec.date";
        public static string 接收人 = "接收人Recipient";
        public static string 数量 = "数量Quantity";
        public static string 装箱明细 = "Packing Detail:";
        public static string 箱码 = "箱码WMS Code:";
        public static string 第几箱总箱数 = "第几箱/总箱数SN/TTL:";
        public static string 净重 = "净重N.W";
        public static string 皮重 = "皮重G.W";
        public static string 相对比重 = "相对比重Gravity";
        public static string 混料日期 = "混料日期Mix.date";
        public static string 料体批次 = "料体批次Bulk Lot";
        public static string 工单号 = "工单号WO#";
        public static string 生产班组 = "生产班组Pro.group";
        public static string 退料班组 = "退料班组Pro.group";
        public static string 成品批号 = "成品批号FG Lot";
        public static string 操作人 = "操作人Operator";
        public static string 总件数 = "总件数TNP";
        public static string 第几件 = "第几件SN";
        public static string 包装方式 = "包装方式Packing";
        public static string 重量 = "重量Weight";
        public static string 退料日期 = "退料日期Ret.date";
        public static string 退料单位 = "退料单位Ret.unit";
        public static string 退料人 = "退料人Ret.staff";
        public static string 托盘明细 = "托盘明细Tray Detail：";
        public static string 总箱数 = "总箱数TTL CTN：";
        public static string 托盘号 = "托盘号Tray NO.：";


        public static string 取样人 = "取样人Pick.staff";
        public static string 取样数量 = "取样数量Pick.Qty";
        public static string 取样日期 = "取样日期Pick.date";

        public static string 包材外 = "包材(外)PG";
        public static string 原料外 = "原料(外)RM";
        public static string 杂入外 = "杂入(外)OG";
        public static string 杂入托 = "杂入(托)OG";
        public static string 杂入内 = "杂入(内)OG";
        public static string 原料内 = "原料(内)RM";
        public static string 外散装外 = "外来半成品(外)IB";
        public static string 外散装内 = "外来半成品(内)IB";
        public static string 外半制外 = "外来半制品(外)IS";
        public static string 半制外 = "半制品(外)IS";
        public static string 散装外 = "半成品(外)BG";
        public static string 散装内 = "半成品(内)BG";
        public static string 成品外 = "成品(外)FG";
        public static string 成品托 = "成品(托)FG";
        public static string 包材退外 = "包材退PG";
        public static string 散装退外 = "半成品退BG";
        public static string 半制退外 = "半制品退SG";
        public static string 成品退外 = "成品退FG";
        public static string 包材托 = "包材(托)PG";
        public static string 原料托 = "原料(托)RM";
        public static string 包材退托 = "包材退(托)PG";
        public static string 成品退托 = "成品退(托)FG";
        public static string 报废料外 = "报废料(外)RM";

       
        private static void NewDel(Barcode_Model model, StringBuilder sbPrint)
        {
            if (model.ProtectWay == "1")
            {
                sbPrint.Append("^IDR:cloth^FS");
                sbPrint.Append("^IDR:glove^FS");
            }
            if (model.ProtectWay == "2")
            {
                sbPrint.Append("^IDR:cloth^FS");
                sbPrint.Append("^IDR:glove^FS");
                sbPrint.Append("^IDR:mask^FS");
            }
            if (model.ProtectWay == "3")
            {
                sbPrint.Append("^IDR:glasses^FS");
                sbPrint.Append("^IDR:mask^FS");
                sbPrint.Append("^IDR:cloth^FS");
                sbPrint.Append("^IDR:glove^FS");
            }
        }

        private static void NewPos(Barcode_Model model, StringBuilder sbPrint)
        {
            if (model.ProtectWay == "1")
            {
                sbPrint.Append("^FO305,15  ^XGcloth^FS");
                sbPrint.Append("^FO375,15 ^XGglove^FS");
            }
            if (model.ProtectWay == "2")
            {
                sbPrint.Append("^FO305,15  ^XGcloth^FS");
                sbPrint.Append("^FO375,15 ^XGglove^FS");
                sbPrint.Append("^FO445,15 ^XGmask^FS");
            }
            if (model.ProtectWay == "3")
            {
                sbPrint.Append("^FO305,15  ^XGcloth^FS");
                sbPrint.Append("^FO375,15 ^XGglove^FS");
                sbPrint.Append("^FO445,15 ^XGmask^FS");
                sbPrint.Append("^FO515,16 ^XGglasses^FS");
            }
        }

        private static void NewImage(Barcode_Model model, StringBuilder sbPrint)
        {
            if (model.ProtectWay == "1")
            {
                sbPrint.Append(cloth);
                sbPrint.Append(glove);
            }
            if (model.ProtectWay == "2")
            {
                sbPrint.Append(cloth);
                sbPrint.Append(glove);
                sbPrint.Append(mask);
            }
            if (model.ProtectWay == "3")
            {
                sbPrint.Append(cloth);
                sbPrint.Append(glove);
                sbPrint.Append(mask);
                sbPrint.Append(glasses);
            }
        }

        //发货托盘标签
        public static string DeliveryTray(T_StockInfoEX model, List<T_StockInfoEX> list)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //内容
            fcb.GETFONTHEX("申请人：", false, "黑体", "a1", 28, 0, true, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.RecPeo, false, "黑体", "a2", 27, 0, false, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX("发货单号：", false, "黑体", "b1", 28, 0, true, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ErpVoucherNo, false, "黑体", "b2", 27, 0, false, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX("客户名称：", false, "黑体", "c1", 28, 0, true, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CompanyCode, true, "黑体", "c2", 26, 260, false, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX("送货地址：", false, "黑体", "d1", 28, 0, true, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.DelAddress, true, "黑体", "d2", 25, 260, false, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX("备注：", false, "黑体", "e1", 28, 0, true, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SN, true, "黑体", "e2", 25, 260, true, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());



            fcb.GETFONTHEX("发货人：", false, "黑体", "f1", 28, 0, true, true, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.PostPeo, false, "黑体", "f2", 27, 0, false, true, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX("复核时间：", false, "黑体", "g1", 28, 0, true, true, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CreateTime.ToDateTime().ToString("yyyy-MM-dd"), false, "黑体", "g2", 27, 0, false, true, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX("发货地址：", false, "黑体", "h1", 28, 0, true, true, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.WareAddress, false, "黑体", "h2", 27, 0, false, true, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX("物流公司：", false, "黑体", "p1", 28, 0, true, true, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.boxweight, false, "黑体", "p2", 27, 0, false, true, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX("流水号：", false, "黑体", "q1", 28, 0, true, true, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.BarcodeNo + "/" + model.PrintQty, false, "黑体", "q2", 33, 0, false, true, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());




            fcb.GETFONTHEX("项序", false, "黑体", "x1", 28, 0, true, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX("产品号", false, "黑体", "i1", 28, 0, true, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());
            //fcb.GETFONTHEX("描述", false, "黑体", "j1", 30, 0, true, false, sbReturn, true);
            //sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX("厂内批号", false, "黑体", "k1", 28, 0, true, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());
            //fcb.GETFONTHEX("箱码", false, "黑体", "l1", 30, 0, true, false, sbReturn, true);
            //sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX("数量", false, "黑体", "l1", 28, 0, true, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX("件数", false, "黑体", "m1", 28, 0, true, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX("单位", false, "黑体", "n1", 28, 0, true, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX("有效期", false, "黑体", "o1", 28, 0, true, false, sbReturn, true);
            sbPrint.Append(sbReturn.ToString());

            for (int i = 0; i < list.Count; i++)
            {
                fcb.GETFONTHEX((i+1)+"", false, "黑体", "xx" + i, 29, 0, true, false, sbReturn, true);
                sbPrint.Append(sbReturn.ToString());
                fcb.GETFONTHEX(list[i].MaterialNo, false, "黑体", "ii"+i, 29, 0, true, false, sbReturn, true);
                sbPrint.Append(sbReturn.ToString());
                //fcb.GETFONTHEX(list[i].MaterialDesc.StrCut(30), true, "黑体", "jj" + i, 25, 235, true, false, sbReturn, true);
                //sbPrint.Append(sbReturn.ToString());
                fcb.GETFONTHEX(list[i].BatchNo, false, "黑体", "kk" + i, 29, 0, true, false, sbReturn, true);
                sbPrint.Append(sbReturn.ToString());
                //fcb.GETFONTHEX(list[i].SerialNo, false, "黑体", "ll" + i, 25, 0, true, false, sbReturn, true);
                //sbPrint.Append(sbReturn.ToString());
                fcb.GETFONTHEX(list[i].Qty + "", false, "黑体", "ll" + i, 29, 0, true, false, sbReturn, true);
                sbPrint.Append(sbReturn.ToString());
                fcb.GETFONTHEX(list[i].ItemQty + "", false, "黑体", "mm" + i, 29, 0, true, false, sbReturn, true);
                sbPrint.Append(sbReturn.ToString());
                fcb.GETFONTHEX(list[i].Unit, false, "黑体", "nn" + i, 29, 0, true, false, sbReturn, true);
                sbPrint.Append(sbReturn.ToString());
                fcb.GETFONTHEX(list[i].EDate.ToString("yyyy-MM-dd"), false, "黑体", "oo" + i, 29, 0, true, false, sbReturn, true);
                sbPrint.Append(sbReturn.ToString());
            }


            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0800^LL1200^LS0");

            //横线
            sbPrint.Append("^FO0,315^GB800,0,3^FS");
            //竖线
            sbPrint.Append("^FO737,315^GB0,890,3^FS");

            sbPrint.Append("^FO339,0^GB0,310,3^FS");

            //体
            sbPrint.Append("^FO749,40 ^XGa1^FS");
            sbPrint.Append("^FO722,40 ^XGa2^FS");

            sbPrint.Append("^FO684,40  ^XGb1^FS");
            sbPrint.Append("^FO657,40  ^XGb2^FS");

            sbPrint.Append("^FO620,40 ^XGc1^FS");
            sbPrint.Append("^FO556,40 ^XGc2^FS");

            sbPrint.Append("^FO518,40 ^XGd1^FS");
            sbPrint.Append("^FO436,40 ^XGd2^FS");

            sbPrint.Append("^FO396,40 ^XGe1^FS");
            sbPrint.Append("^FO344,40 ^XGe2^FS");
            //-----------------------------------------------------339
            sbPrint.Append("^FO302,40 ^XGf1^FS");
            sbPrint.Append("^FO274,40 ^XGf2^FS");

            sbPrint.Append("^FO236,40 ^XGg1^FS");
            sbPrint.Append("^FO209,40 ^XGg2^FS");

            sbPrint.Append("^FO171,40 ^XGh1^FS");
            sbPrint.Append("^FO144,40 ^XGh2^FS");

            sbPrint.Append("^FO106,40 ^XGp1^FS");
            sbPrint.Append("^FO79,40 ^XGp2^FS");

            sbPrint.Append("^FO40,40 ^XGq1^FS");
            sbPrint.Append("^FO10,40 ^XGq2^FS");



            //表体
            sbPrint.Append("^FO740,335 ^XGx1^FS");
            sbPrint.Append("^FO740,415 ^XGi1^FS");
            sbPrint.Append("^FO740,615 ^XGk1^FS");
            sbPrint.Append("^FO740,745 ^XGl1^FS");
            sbPrint.Append("^FO740,855 ^XGm1^FS");
            sbPrint.Append("^FO740,935  ^XGn1^FS");
            sbPrint.Append("^FO740,1015 ^XGo1^FS");
            for (int i = 0; i < list.Count; i++)
            {
                //sbPrint.Append("^FO" + (690 - i * 30) + ",350 ^XGxx" + i + "^FS");
                //sbPrint.Append("^FO" + (690 - i * 30) + ",350 ^XGii" + i + "^FS");
                ////if (list[i].MaterialDesc.StrCut(30).StrLength()>16)
                ////    sbPrint.Append("^FO" + (690 - i * 60-30) + ",465 ^XGjj" + i + "^FS");
                ////else
                ////    sbPrint.Append("^FO" + (690 - i * 60) + ",465 ^XGjj" + i + "^FS");
                ////先通过打印测试出115只能放8个字符，然后下面写大于8就往下移动
                ////if (list[i].BatchNo.StrLength() > 8)
                ////    sbPrint.Append("^FO" + (690 - i * 60-30) + ",700 ^XGkk" + i + "^FS");
                ////else
                ////    sbPrint.Append("^FO" + (690 - i * 60) + ",700 ^XGkk" + i + "^FS");
                //sbPrint.Append("^FO" + (690 - i * 30) + ",600 ^XGkk" + i + "^FS");
                //sbPrint.Append("^FO" + (690 - i * 30) + ",780 ^XGll" + i + "^FS");
                //sbPrint.Append("^FO" + (690 - i * 30) + ",920 ^XGmm" + i + "^FS");
                //sbPrint.Append("^FO" + (690 - i * 30) + ",1040^XGnn" + i + "^FS");
                //sbPrint.Append("^FO" + (690 - i * 30) + ",1040^XGoo" + i + "^FS");



                sbPrint.Append("^FO" + (690 - i * 30) + ",335 ^XGxx" + i + "^FS");
                sbPrint.Append("^FO" + (690 - i * 30) + ",415 ^XGii" + i + "^FS");
                sbPrint.Append("^FO" + (690 - i * 30) + ",615 ^XGkk" + i + "^FS");
                sbPrint.Append("^FO" + (690 - i * 30) + ",745 ^XGll" + i + "^FS");
                sbPrint.Append("^FO" + (690 - i * 30) + ",855 ^XGmm" + i + "^FS");
                sbPrint.Append("^FO" + (690 - i * 30) + ",935 ^XGnn" + i + "^FS");
                sbPrint.Append("^FO" + (690 - i * 30) + ",1015^XGoo" + i + "^FS");
            }



            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:a1^FS");
            sbPrint.Append("^IDR:a2^FS");
            sbPrint.Append("^IDR:b1^FS");
            sbPrint.Append("^IDR:b2^FS");
            sbPrint.Append("^IDR:c1^FS");
            sbPrint.Append("^IDR:c2^FS");
            sbPrint.Append("^IDR:d1^FS");
            sbPrint.Append("^IDR:d2^FS");
            sbPrint.Append("^IDR:e1^FS");
            sbPrint.Append("^IDR:e2^FS");
            sbPrint.Append("^IDR:f1^FS");
            sbPrint.Append("^IDR:f2^FS");
            sbPrint.Append("^IDR:g1^FS");
            sbPrint.Append("^IDR:g2^FS");
            sbPrint.Append("^IDR:p1^FS");
            sbPrint.Append("^IDR:p2^FS");
            sbPrint.Append("^IDR:q1^FS");
            sbPrint.Append("^IDR:q2^FS");
            sbPrint.Append("^IDR:h1^FS");
            sbPrint.Append("^IDR:h2^FS");
            sbPrint.Append("^IDR:x1^FS");
            sbPrint.Append("^IDR:i1^FS");
            //sbPrint.Append("^IDR:j1^FS");
            sbPrint.Append("^IDR:k1^FS");
            sbPrint.Append("^IDR:l1^FS");
            sbPrint.Append("^IDR:m1^FS");
            sbPrint.Append("^IDR:n1^FS");
            sbPrint.Append("^IDR:o1^FS");
            for (int i = 0; i < list.Count; i++)
            {
                sbPrint.Append("^IDR:xx" + i + "^FS");
                sbPrint.Append("^IDR:ii" + i + "^FS");
                sbPrint.Append("^IDR:kk" + i + "^FS");
                sbPrint.Append("^IDR:ll" + i + "^FS");
                sbPrint.Append("^IDR:mm" + i + "^FS");
                sbPrint.Append("^IDR:nn" + i + "^FS");
                sbPrint.Append("^IDR:oo" + i + "^FS");
            }



            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        static int titlesize = 34;

        //包材外
        public static string OutBaoCai(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(包材外 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, true, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(订单号, false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ErpVoucherNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(供应商, false, "黑体", "supplyname", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupCode, false, "黑体", "supplyname2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(供应商批号, false, "黑体", "supbatch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupPrdBatch, false, "黑体", "supbatch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate==DateTime.MinValue?"": model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(存储条件, false, "黑体", "cctj", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.StoreCondition, false, "黑体", "cctj2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收日期, false, "黑体", "recdate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CreateTime.ToString("yyyy-MM-dd"), false, "黑体", "recdate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收人, false, "黑体", "recpeo", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "recpeo2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(数量, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //尾部
            fcb.GETFONTHEX(装箱明细, false, "黑体", "zxmx", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.BoxDetail, false, "黑体", "zxmx2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(第几箱总箱数, false, "黑体", "allin", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.BarcodeNo + "/" + model.PrintQty, false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());



            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");
            
            sbPrint.Append("^FO10,20^XGtitle^FS");


            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,415,3^FS");

            //体
            sbPrint.Append("^FO20,100 ^XGmateno^FS");
            sbPrint.Append("^FO25,130  ^XGmateno2^FS");
            sbPrint.Append("^FO325,100 ^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");

            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190  ^XGvoucherno^FS");
            sbPrint.Append("^FO25,220^XGvoucherno2^FS");
            sbPrint.Append("^FO325,190^XGsupplyname^FS");
            sbPrint.Append("^FO330,220^XGsupplyname2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255  ^XGsupbatch^FS");
            sbPrint.Append("^FO25,285 ^XGsupbatch2^FS");
            sbPrint.Append("^FO325,255^XGprodate^FS");
            sbPrint.Append("^FO330,285^XGprodate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGbatch^FS");
            sbPrint.Append("^FO25,350 ^XGbatch2^FS");
            sbPrint.Append("^FO325,320^XGenddate^FS");
            sbPrint.Append("^FO330,350^XGenddate2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385  ^XGcctj^FS");
            sbPrint.Append("^FO25,415^XGcctj2^FS");
            sbPrint.Append("^FO325,385^XGrecdate^FS");
            sbPrint.Append("^FO330,415^XGrecdate2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            sbPrint.Append("^FO20,450  ^XGrecpeo^FS");
            sbPrint.Append("^FO25,480 ^XGrecpeo2^FS");
            sbPrint.Append("^FO325,450^XGqty^FS");
            sbPrint.Append("^FO330,480^XGqty2^FS");

            sbPrint.Append("^FO0,510^GB610,0,3^FS");




            //sbPrint.Append("^FO0,555^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,530^XGzxmx^FS");
            sbPrint.Append("^FO20,560^XGzxmx2^FS");
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            sbPrint.Append("^FO20,670^XGallin^FS");
            sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,760^BQN,2,4^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
            sbPrint.Append("^IDR:voucherno^FS");
            sbPrint.Append("^IDR:voucherno2^FS");
            sbPrint.Append("^IDR:supplyname^FS");
            sbPrint.Append("^IDR:supplyname2^FS");
            sbPrint.Append("^IDR:supbatch^FS");
            sbPrint.Append("^IDR:supbatch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:cctj^FS");
            sbPrint.Append("^IDR:cctj2^FS");
            sbPrint.Append("^IDR:recdate^FS");
            sbPrint.Append("^IDR:recdate2^FS");
            sbPrint.Append("^IDR:recpeo^FS");
            sbPrint.Append("^IDR:recpeo2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:zxmx^FS");
            sbPrint.Append("^IDR:zxmx2^FS");
            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            sbPrint.Append("^IDR:allin^FS");
            sbPrint.Append("^IDR:allin2^FS");

            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //包材托
        public static string TOutBaoCai(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(包材托 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, true, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(订单号, false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ErpVoucherNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(供应商, false, "黑体", "supplyname", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupCode, false, "黑体", "supplyname2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(供应商批号, false, "黑体", "supbatch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupPrdBatch, false, "黑体", "supbatch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate == DateTime.MinValue ? "" : model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(存储条件, false, "黑体", "cctj", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.StoreCondition, false, "黑体", "cctj2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收日期, false, "黑体", "recdate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CreateTime.ToString("yyyy-MM-dd"), false, "黑体", "recdate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收人, false, "黑体", "recpeo", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "recpeo2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(数量, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //尾部
            fcb.GETFONTHEX(托盘明细, false, "黑体", "zxmx", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.PalletDetail, false, "黑体", "zxmx2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(托盘号, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.PalletNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(总箱数, false, "黑体", "allin", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.BoxCount + "", false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());



            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");

            sbPrint.Append("^FO10,20^XGtitle^FS");


            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,415,3^FS");

            //体
            sbPrint.Append("^FO20,100 ^XGmateno^FS");
            sbPrint.Append("^FO25,130  ^XGmateno2^FS");
            sbPrint.Append("^FO325,100 ^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");

            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190  ^XGvoucherno^FS");
            sbPrint.Append("^FO25,220^XGvoucherno2^FS");
            sbPrint.Append("^FO325,190^XGsupplyname^FS");
            sbPrint.Append("^FO330,220^XGsupplyname2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255  ^XGsupbatch^FS");
            sbPrint.Append("^FO25,285 ^XGsupbatch2^FS");
            sbPrint.Append("^FO325,255^XGprodate^FS");
            sbPrint.Append("^FO330,285^XGprodate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGbatch^FS");
            sbPrint.Append("^FO25,350 ^XGbatch2^FS");
            sbPrint.Append("^FO325,320^XGenddate^FS");
            sbPrint.Append("^FO330,350^XGenddate2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385  ^XGcctj^FS");
            sbPrint.Append("^FO25,415^XGcctj2^FS");
            sbPrint.Append("^FO325,385^XGrecdate^FS");
            sbPrint.Append("^FO330,415^XGrecdate2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            sbPrint.Append("^FO20,450  ^XGrecpeo^FS");
            sbPrint.Append("^FO25,480 ^XGrecpeo2^FS");
            sbPrint.Append("^FO325,450^XGqty^FS");
            sbPrint.Append("^FO330,480^XGqty2^FS");

            sbPrint.Append("^FO0,510^GB610,0,3^FS");




            //sbPrint.Append("^FO0,555^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,530^XGzxmx^FS");
            sbPrint.Append("^FO20,560^XGzxmx2^FS");
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            sbPrint.Append("^FO20,670^XGallin^FS");
            sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,760^BQN,2,4^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
            sbPrint.Append("^IDR:voucherno^FS");
            sbPrint.Append("^IDR:voucherno2^FS");
            sbPrint.Append("^IDR:supplyname^FS");
            sbPrint.Append("^IDR:supplyname2^FS");
            sbPrint.Append("^IDR:supbatch^FS");
            sbPrint.Append("^IDR:supbatch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:cctj^FS");
            sbPrint.Append("^IDR:cctj2^FS");
            sbPrint.Append("^IDR:recdate^FS");
            sbPrint.Append("^IDR:recdate2^FS");
            sbPrint.Append("^IDR:recpeo^FS");
            sbPrint.Append("^IDR:recpeo2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:zxmx^FS");
            sbPrint.Append("^IDR:zxmx2^FS");
            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            sbPrint.Append("^IDR:allin^FS");
            sbPrint.Append("^IDR:allin2^FS");

            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //包材内
        public static string InBaoCai(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX("包材(内)PG", false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, true, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(订单号, false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ErpVoucherNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(供应商, false, "黑体", "supplyname", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupCode, false, "黑体", "supplyname2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(供应商批号, false, "黑体", "supbatch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupPrdBatch, false, "黑体", "supbatch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate == DateTime.MinValue ? "" : model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(存储条件, false, "黑体", "cctj", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.StoreCondition, false, "黑体", "cctj2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收日期, false, "黑体", "recdate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CreateTime.ToString("yyyy-MM-dd"), false, "黑体", "recdate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收人, false, "黑体", "recpeo", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "recpeo2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(数量, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //尾部
            //fcb.GETFONTHEX(装箱明细, false, "黑体", "zxmx", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());
            //fcb.GETFONTHEX(model.BoxDetail, false, "黑体", "zxmx2", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //fcb.GETFONTHEX(第几箱总箱数, false, "黑体", "allin", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());
            //fcb.GETFONTHEX(model.BarcodeNo + "/" + model.PrintQty, false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());



            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");

            sbPrint.Append("^FO10,20^XGtitle^FS");


            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,415,3^FS");

            //体
            sbPrint.Append("^FO20,100 ^XGmateno^FS");
            sbPrint.Append("^FO25,130  ^XGmateno2^FS");
            sbPrint.Append("^FO325,100 ^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");

            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190  ^XGvoucherno^FS");
            sbPrint.Append("^FO25,220^XGvoucherno2^FS");
            sbPrint.Append("^FO325,190^XGsupplyname^FS");
            sbPrint.Append("^FO330,220^XGsupplyname2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255  ^XGsupbatch^FS");
            sbPrint.Append("^FO25,285 ^XGsupbatch2^FS");
            sbPrint.Append("^FO325,255^XGprodate^FS");
            sbPrint.Append("^FO330,285^XGprodate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGbatch^FS");
            sbPrint.Append("^FO25,350 ^XGbatch2^FS");
            sbPrint.Append("^FO325,320^XGenddate^FS");
            sbPrint.Append("^FO330,350^XGenddate2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385  ^XGcctj^FS");
            sbPrint.Append("^FO25,415^XGcctj2^FS");
            sbPrint.Append("^FO325,385^XGrecdate^FS");
            sbPrint.Append("^FO330,415^XGrecdate2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            sbPrint.Append("^FO20,450  ^XGrecpeo^FS");
            sbPrint.Append("^FO25,480 ^XGrecpeo2^FS");
            sbPrint.Append("^FO325,450^XGqty^FS");
            sbPrint.Append("^FO330,480^XGqty2^FS");

            sbPrint.Append("^FO0,510^GB610,0,3^FS");




            //sbPrint.Append("^FO0,555^GB610,0,3^FS");

            //尾
            //sbPrint.Append("^FO20,530^XGzxmx^FS");
            //sbPrint.Append("^FO20,560^XGzxmx2^FS");
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            //sbPrint.Append("^FO20,670^XGallin^FS");
            //sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,760^BQN,2,4^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
            sbPrint.Append("^IDR:voucherno^FS");
            sbPrint.Append("^IDR:voucherno2^FS");
            sbPrint.Append("^IDR:supplyname^FS");
            sbPrint.Append("^IDR:supplyname2^FS");
            sbPrint.Append("^IDR:supbatch^FS");
            sbPrint.Append("^IDR:supbatch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:cctj^FS");
            sbPrint.Append("^IDR:cctj2^FS");
            sbPrint.Append("^IDR:recdate^FS");
            sbPrint.Append("^IDR:recdate2^FS");
            sbPrint.Append("^IDR:recpeo^FS");
            sbPrint.Append("^IDR:recpeo2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            //sbPrint.Append("^IDR:zxmx^FS");
            //sbPrint.Append("^IDR:zxmx2^FS");
            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            //sbPrint.Append("^IDR:allin^FS");
            //sbPrint.Append("^IDR:allin2^FS");

            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //原料外
        public static string OutR(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(原料外 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, true, "黑体", "mateno2", 27, 295, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(供应商, true, "黑体", "supplyname", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupCode, true, "黑体", "supplyname2", 27, 295, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(订单号, false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ErpVoucherNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(供应商批号, false, "黑体", "supbatch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupPrdBatch, false, "黑体", "supbatch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate==DateTime.MinValue?"": model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(存储条件, false, "黑体", "cctj", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.StoreCondition, false, "黑体", "cctj2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(防护措施, false, "黑体", "protectway", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProtectWay, false, "黑体", "protectway2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收日期, false, "黑体", "recdate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CreateTime.ToString("yyyy-MM-dd"), false, "黑体", "recdate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(特殊要求, false, "黑体", "spereq", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SpecialRequire, false, "黑体", "spereq2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(净重, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(接收人, false, "黑体", "recpeo", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "recpeo2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //fcb.GETFONTHEX(皮重, false, "黑体", "boxweight", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());
            //fcb.GETFONTHEX(model.BoxWeight, false, "黑体", "boxweight2", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());

            //图片
            NewImage(model, sbPrint);


            //尾部


            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(第几箱总箱数, false, "黑体", "allin", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.BarcodeNo + "/" + model.PrintQty, false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            

            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");
            sbPrint.Append("^FO10,20^XGtitle^FS");

            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,480,3^FS");

            //图片
            NewPos(model, sbPrint);

            //体
            sbPrint.Append("^FO20,100  ^XGmateno^FS");
            sbPrint.Append("^FO25,130 ^XGmateno2^FS");
            sbPrint.Append("^FO325,100 ^XGsupplyname^FS");
            sbPrint.Append("^FO330,130^XGsupplyname2^FS");

            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190 ^XGvoucherno^FS");
            sbPrint.Append("^FO25,220 ^XGvoucherno2^FS");
            sbPrint.Append("^FO325,190^XGsupbatch^FS");
            sbPrint.Append("^FO330,220^XGsupbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255 ^XGbatch^FS");
            sbPrint.Append("^FO25,285 ^XGbatch2^FS");
            sbPrint.Append("^FO325,255^XGprodate^FS");
            sbPrint.Append("^FO330,285^XGprodate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGcctj^FS");
            sbPrint.Append("^FO25,350 ^XGcctj2^FS");
            sbPrint.Append("^FO325,320^XGenddate^FS");
            sbPrint.Append("^FO330,350^XGenddate2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385 ^XGprotectway^FS");
            sbPrint.Append("^FO25,415 ^XGprotectway2^FS");
            sbPrint.Append("^FO325,385^XGrecdate^FS");
            sbPrint.Append("^FO330,415^XGrecdate2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            sbPrint.Append("^FO20,450 ^XGspereq^FS");
            sbPrint.Append("^FO25,480 ^XGspereq2^FS");
            sbPrint.Append("^FO325,450^XGqty^FS");
            sbPrint.Append("^FO330,480^XGqty2^FS");

            sbPrint.Append("^FO0,510^GB610,0,3^FS");

            sbPrint.Append("^FO20,515 ^XGrecpeo^FS");
            sbPrint.Append("^FO25,545 ^XGrecpeo2^FS");
            //sbPrint.Append("^FO325,515^XGboxweight^FS");
            //sbPrint.Append("^FO330,545^XGboxweight2^FS");

            sbPrint.Append("^FO0,575^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            sbPrint.Append("^FO20,670^XGallin^FS");
            sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,760^BQN,2,4^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:supplyname^FS");
            sbPrint.Append("^IDR:supplyname2^FS");
            sbPrint.Append("^IDR:voucherno^FS");
            sbPrint.Append("^IDR:voucherno2^FS");
            sbPrint.Append("^IDR:supbatch^FS");
            sbPrint.Append("^IDR:supbatch2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:cctj^FS");
            sbPrint.Append("^IDR:cctj2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:protectway^FS");
            sbPrint.Append("^IDR:protectway2^FS");
            sbPrint.Append("^IDR:recdate^FS");
            sbPrint.Append("^IDR:recdate2^FS");
            sbPrint.Append("^IDR:spereq^FS");
            sbPrint.Append("^IDR:spereq2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:recpeo^FS");
            sbPrint.Append("^IDR:recpeo2^FS");
            //sbPrint.Append("^IDR:boxweight^FS");
            //sbPrint.Append("^IDR:boxweight2^FS");
            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            sbPrint.Append("^IDR:allin^FS");
            sbPrint.Append("^IDR:allin2^FS");

            //删除图片
            NewDel(model, sbPrint);


            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //杂入外（无参照）
        public static string NullRef(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(杂入外 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, false, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(操作人, true, "黑体", "supplyname", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "supplyname2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(供应商批号, false, "黑体", "supbatch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupPrdBatch, false, "黑体", "supbatch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate==DateTime.MinValue?"": model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(存储条件, false, "黑体", "cctj", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.StoreCondition, false, "黑体", "cctj2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(防护措施, false, "黑体", "protectway", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProtectWay, false, "黑体", "protectway2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收日期, false, "黑体", "recdate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CreateTime.ToString("yyyy-MM-dd"), false, "黑体", "recdate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(特殊要求, false, "黑体", "spereq", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SpecialRequire, false, "黑体", "spereq2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(数量, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //图片
            NewImage(model, sbPrint);

            //尾部


            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //fcb.GETFONTHEX(第几箱总箱数, false, "黑体", "allin", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());
            //fcb.GETFONTHEX(model.BarcodeNo + "/" + model.PrintQty, false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());



            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");

            sbPrint.Append("^FO10,20^XGtitle^FS");


            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,415,3^FS");

            //图片
            NewPos(model, sbPrint);

            //体
            sbPrint.Append("^FO20,100  ^XGmateno^FS");
            sbPrint.Append("^FO25,130  ^XGmateno2^FS");
            sbPrint.Append("^FO325,100^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");

            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190  ^XGsupplyname^FS");
            sbPrint.Append("^FO25,220 ^XGsupplyname2^FS");
            sbPrint.Append("^FO325,190^XGsupbatch^FS");
            sbPrint.Append("^FO330,220^XGsupbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255  ^XGbatch^FS");
            sbPrint.Append("^FO25,285 ^XGbatch2^FS");
            sbPrint.Append("^FO325,255^XGprodate^FS");
            sbPrint.Append("^FO330,285^XGprodate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGcctj^FS");
            sbPrint.Append("^FO25,350 ^XGcctj2^FS");
            sbPrint.Append("^FO325,320^XGenddate^FS");
            sbPrint.Append("^FO330,350^XGenddate2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385 ^XGprotectway^FS");
            sbPrint.Append("^FO25,415 ^XGprotectway2^FS");
            sbPrint.Append("^FO325,385^XGrecdate^FS");
            sbPrint.Append("^FO330,415^XGrecdate2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            sbPrint.Append("^FO20,450  ^XGspereq^FS");
            sbPrint.Append("^FO25,480 ^XGspereq2^FS");
            sbPrint.Append("^FO325,450^XGqty^FS");
            sbPrint.Append("^FO330,480^XGqty2^FS");

            sbPrint.Append("^FO0,510^GB610,0,3^FS");

            //
            //
            //
            //

            //sbPrint.Append("^FO0,575^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            //sbPrint.Append("^FO20,670^XGallin^FS");
            //sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,760^BQN,2,4^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
            sbPrint.Append("^IDR:supplyname^FS");
            sbPrint.Append("^IDR:supplyname2^FS");
            sbPrint.Append("^IDR:supbatch^FS");
            sbPrint.Append("^IDR:supbatch2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:cctj^FS");
            sbPrint.Append("^IDR:cctj2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:protectway^FS");
            sbPrint.Append("^IDR:protectway2^FS");
            sbPrint.Append("^IDR:recdate^FS");
            sbPrint.Append("^IDR:recdate2^FS");
            sbPrint.Append("^IDR:spereq^FS");
            sbPrint.Append("^IDR:spereq2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            //sbPrint.Append("^IDR:allin^FS");
            //sbPrint.Append("^IDR:allin2^FS");


            //删除图片
            NewDel(model, sbPrint);
            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //杂入托（无参照）
        public static string TNullRef(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(杂入托 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, false, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(操作人, true, "黑体", "supplyname", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "supplyname2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(供应商批号, false, "黑体", "supbatch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupPrdBatch, false, "黑体", "supbatch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate == DateTime.MinValue ? "" : model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(存储条件, false, "黑体", "cctj", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.StoreCondition, false, "黑体", "cctj2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(防护措施, false, "黑体", "protectway", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProtectWay, false, "黑体", "protectway2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收日期, false, "黑体", "recdate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CreateTime.ToString("yyyy-MM-dd"), false, "黑体", "recdate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(特殊要求, false, "黑体", "spereq", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SpecialRequire, false, "黑体", "spereq2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(数量, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //图片
            NewImage(model, sbPrint);

            //尾部


            fcb.GETFONTHEX(托盘号, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.PalletNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(总箱数, false, "黑体", "allin", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.BoxCount+"" , false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());



            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");

            sbPrint.Append("^FO10,20^XGtitle^FS");


            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,415,3^FS");

            //图片
            NewPos(model, sbPrint);

            //体
            sbPrint.Append("^FO20,100  ^XGmateno^FS");
            sbPrint.Append("^FO25,130  ^XGmateno2^FS");
            sbPrint.Append("^FO325,100^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");

            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190  ^XGsupplyname^FS");
            sbPrint.Append("^FO25,220 ^XGsupplyname2^FS");
            sbPrint.Append("^FO325,190^XGsupbatch^FS");
            sbPrint.Append("^FO330,220^XGsupbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255  ^XGbatch^FS");
            sbPrint.Append("^FO25,285 ^XGbatch2^FS");
            sbPrint.Append("^FO325,255^XGprodate^FS");
            sbPrint.Append("^FO330,285^XGprodate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGcctj^FS");
            sbPrint.Append("^FO25,350 ^XGcctj2^FS");
            sbPrint.Append("^FO325,320^XGenddate^FS");
            sbPrint.Append("^FO330,350^XGenddate2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385 ^XGprotectway^FS");
            sbPrint.Append("^FO25,415 ^XGprotectway2^FS");
            sbPrint.Append("^FO325,385^XGrecdate^FS");
            sbPrint.Append("^FO330,415^XGrecdate2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            sbPrint.Append("^FO20,450  ^XGspereq^FS");
            sbPrint.Append("^FO25,480 ^XGspereq2^FS");
            sbPrint.Append("^FO325,450^XGqty^FS");
            sbPrint.Append("^FO330,480^XGqty2^FS");

            sbPrint.Append("^FO0,510^GB610,0,3^FS");

            //
            //
            //
            //

            //sbPrint.Append("^FO0,575^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            sbPrint.Append("^FO20,670^XGallin^FS");
            sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,760^BQN,2,4^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
            sbPrint.Append("^IDR:supplyname^FS");
            sbPrint.Append("^IDR:supplyname2^FS");
            sbPrint.Append("^IDR:supbatch^FS");
            sbPrint.Append("^IDR:supbatch2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:cctj^FS");
            sbPrint.Append("^IDR:cctj2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:protectway^FS");
            sbPrint.Append("^IDR:protectway2^FS");
            sbPrint.Append("^IDR:recdate^FS");
            sbPrint.Append("^IDR:recdate2^FS");
            sbPrint.Append("^IDR:spereq^FS");
            sbPrint.Append("^IDR:spereq2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            sbPrint.Append("^IDR:allin^FS");
            sbPrint.Append("^IDR:allin2^FS");
            //图片
            NewPos(model, sbPrint);
            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //杂入内（无参照）
        public static string InNullRef(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(杂入内 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, false, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(操作人, true, "黑体", "supplyname", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "supplyname2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(供应商批号, false, "黑体", "supbatch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupPrdBatch, false, "黑体", "supbatch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate == DateTime.MinValue ? "" : model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(存储条件, false, "黑体", "cctj", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.StoreCondition, false, "黑体", "cctj2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(防护措施, false, "黑体", "protectway", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProtectWay, false, "黑体", "protectway2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收日期, false, "黑体", "recdate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CreateTime.ToString("yyyy-MM-dd"), false, "黑体", "recdate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(特殊要求, false, "黑体", "spereq", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SpecialRequire, false, "黑体", "spereq2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(数量, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //图片
            NewImage(model, sbPrint);

            //尾部


            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //fcb.GETFONTHEX(第几箱总箱数, false, "黑体", "allin", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());
            //fcb.GETFONTHEX(model.BarcodeNo + "/" + model.PrintQty, false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());



            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");

            sbPrint.Append("^FO10,20^XGtitle^FS");


            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,415,3^FS");
            //图片
            NewPos(model, sbPrint);

            //体
            sbPrint.Append("^FO20,100  ^XGmateno^FS");
            sbPrint.Append("^FO25,130  ^XGmateno2^FS");
            sbPrint.Append("^FO325,100^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");

            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190  ^XGsupplyname^FS");
            sbPrint.Append("^FO25,220 ^XGsupplyname2^FS");
            sbPrint.Append("^FO325,190^XGsupbatch^FS");
            sbPrint.Append("^FO330,220^XGsupbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255  ^XGbatch^FS");
            sbPrint.Append("^FO25,285 ^XGbatch2^FS");
            sbPrint.Append("^FO325,255^XGprodate^FS");
            sbPrint.Append("^FO330,285^XGprodate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGcctj^FS");
            sbPrint.Append("^FO25,350 ^XGcctj2^FS");
            sbPrint.Append("^FO325,320^XGenddate^FS");
            sbPrint.Append("^FO330,350^XGenddate2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385 ^XGprotectway^FS");
            sbPrint.Append("^FO25,415 ^XGprotectway2^FS");
            sbPrint.Append("^FO325,385^XGrecdate^FS");
            sbPrint.Append("^FO330,415^XGrecdate2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            sbPrint.Append("^FO20,450  ^XGspereq^FS");
            sbPrint.Append("^FO25,480 ^XGspereq2^FS");
            sbPrint.Append("^FO325,450^XGqty^FS");
            sbPrint.Append("^FO330,480^XGqty2^FS");

            sbPrint.Append("^FO0,510^GB610,0,3^FS");

            //
            //
            //
            //

            //sbPrint.Append("^FO0,575^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            //sbPrint.Append("^FO20,670^XGallin^FS");
            //sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,760^BQN,2,4^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
            sbPrint.Append("^IDR:supplyname^FS");
            sbPrint.Append("^IDR:supplyname2^FS");
            sbPrint.Append("^IDR:supbatch^FS");
            sbPrint.Append("^IDR:supbatch2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:cctj^FS");
            sbPrint.Append("^IDR:cctj2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:protectway^FS");
            sbPrint.Append("^IDR:protectway2^FS");
            sbPrint.Append("^IDR:recdate^FS");
            sbPrint.Append("^IDR:recdate2^FS");
            sbPrint.Append("^IDR:spereq^FS");
            sbPrint.Append("^IDR:spereq2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            //sbPrint.Append("^IDR:allin^FS");
            //sbPrint.Append("^IDR:allin2^FS");
            //删除图片
            NewDel(model, sbPrint);
            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }
        //原料内
        public static string InR(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(原料内 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, false, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(供应商, true, "黑体", "supplyname", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupCode, false, "黑体", "supplyname2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(订单号, false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ErpVoucherNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(供应商批号, false, "黑体", "supbatch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupPrdBatch, false, "黑体", "supbatch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate==DateTime.MinValue?"": model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(存储条件, false, "黑体", "cctj", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.StoreCondition, false, "黑体", "cctj2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(防护措施, false, "黑体", "protectway", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProtectWay, false, "黑体", "protectway2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收日期, false, "黑体", "recdate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CreateTime.ToString("yyyy-MM-dd"), false, "黑体", "recdate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(特殊要求, false, "黑体", "spereq", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SpecialRequire, false, "黑体", "spereq2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(净重, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(接收人, false, "黑体", "recpeo", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "recpeo2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            //图片
            NewImage(model, sbPrint);

            //尾部


            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());




            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");
            sbPrint.Append("^FO10,20^XGtitle^FS");

            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,480,3^FS");
            //图片
            NewImage(model, sbPrint);
            //体
            sbPrint.Append("^FO20,100  ^XGmateno^FS");
            sbPrint.Append("^FO25,130 ^XGmateno2^FS");
            sbPrint.Append("^FO325,100^XGsupplyname^FS");
            sbPrint.Append("^FO330,130^XGsupplyname2^FS");

            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190  ^XGvoucherno^FS");
            sbPrint.Append("^FO25,220 ^XGvoucherno2^FS");
            sbPrint.Append("^FO325,190^XGsupbatch^FS");
            sbPrint.Append("^FO330,220^XGsupbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255  ^XGbatch^FS");
            sbPrint.Append("^FO25,285 ^XGbatch2^FS");
            sbPrint.Append("^FO325,255^XGprodate^FS");
            sbPrint.Append("^FO330,285^XGprodate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320  ^XGcctj^FS");
            sbPrint.Append("^FO25,350 ^XGcctj2^FS");
            sbPrint.Append("^FO325,320^XGenddate^FS");
            sbPrint.Append("^FO330,350^XGenddate2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385  ^XGprotectway^FS");
            sbPrint.Append("^FO25,415 ^XGprotectway2^FS");
            sbPrint.Append("^FO325,385^XGrecdate^FS");
            sbPrint.Append("^FO330,415^XGrecdate2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            sbPrint.Append("^FO20,450  ^XGspereq^FS");
            sbPrint.Append("^FO25,480 ^XGspereq2^FS");
            sbPrint.Append("^FO325,450^XGqty^FS");
            sbPrint.Append("^FO330,480^XGqty2^FS");

            sbPrint.Append("^FO0,510^GB610,0,3^FS");

            sbPrint.Append("^FO20,515  ^XGrecpeo^FS");
            sbPrint.Append("^FO25,545 ^XGrecpeo2^FS");


            sbPrint.Append("^FO0,575^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
       


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,760^BQN,2,4^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:supplyname^FS");
            sbPrint.Append("^IDR:supplyname2^FS");
            sbPrint.Append("^IDR:voucherno^FS");
            sbPrint.Append("^IDR:voucherno2^FS");
            sbPrint.Append("^IDR:supbatch^FS");
            sbPrint.Append("^IDR:supbatch2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:cctj^FS");
            sbPrint.Append("^IDR:cctj2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:protectway^FS");
            sbPrint.Append("^IDR:protectway2^FS");
            sbPrint.Append("^IDR:recdate^FS");
            sbPrint.Append("^IDR:recdate2^FS");
            sbPrint.Append("^IDR:spereq^FS");
            sbPrint.Append("^IDR:spereq2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:recpeo^FS");
            sbPrint.Append("^IDR:recpeo2^FS");

            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");


            //删除图片
            NewDel(model, sbPrint);
            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //外来散装外
        public static string OutFromSanZhuang(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(外散装外+ model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, false, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(订单号, false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ErpVoucherNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(供应商, true, "黑体", "supplyname", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupCode, false, "黑体", "supplyname2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(供应商批号, false, "黑体", "supbatch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupPrdBatch, false, "黑体", "supbatch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(相对比重, false, "黑体", "xdbz", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.RelaWeight, false, "黑体", "xdbz2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate==DateTime.MinValue?"": model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(存储条件, false, "黑体", "cctj", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.StoreCondition, false, "黑体", "cctj2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(防护措施, false, "黑体", "protectway", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProtectWay, false, "黑体", "protectway2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收日期, false, "黑体", "recdate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CreateTime.ToString("yyyy-MM-dd"), false, "黑体", "recdate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收人, false, "黑体", "recpeo", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "recpeo2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(净重, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //图片
            NewImage(model, sbPrint);
            //尾部


            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(第几箱总箱数, false, "黑体", "allin", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.BarcodeNo + "/" + model.PrintQty, false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());



            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");
            sbPrint.Append("^FO10,20^XGtitle^FS");

            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,480,3^FS");
            //图片
            NewPos(model, sbPrint);
            //体
            sbPrint.Append("^FO20,100 ^XGmateno^FS");
            sbPrint.Append("^FO25,130 ^XGmateno2^FS");
            sbPrint.Append("^FO325,100^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");

            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190 ^XGvoucherno^FS");
            sbPrint.Append("^FO25,220 ^XGvoucherno2^FS");
            sbPrint.Append("^FO325,190^XGbatch^FS");
            sbPrint.Append("^FO330,220^XGbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255 ^XGsupplyname^FS");
            sbPrint.Append("^FO25,285 ^XGsupplyname2^FS");
            sbPrint.Append("^FO325,255^XGsupbatch^FS");
            sbPrint.Append("^FO330,285^XGsupbatch2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGxdbz^FS");
            sbPrint.Append("^FO25,350 ^XGxdbz2^FS");
            sbPrint.Append("^FO325,320^XGprodate^FS");
            sbPrint.Append("^FO330,350^XGprodate2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385 ^XGcctj^FS");
            sbPrint.Append("^FO25,415 ^XGcctj2^FS");
            sbPrint.Append("^FO325,385^XGenddate^FS");
            sbPrint.Append("^FO330,415^XGenddate2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            sbPrint.Append("^FO20,450 ^XGprotectway^FS");
            sbPrint.Append("^FO25,480 ^XGprotectway2^FS");
            sbPrint.Append("^FO325,450^XGrecdate^FS");
            sbPrint.Append("^FO330,480^XGrecdate2^FS");

            sbPrint.Append("^FO0,510^GB610,0,3^FS");

            sbPrint.Append("^FO20,515 ^XGrecpeo^FS");
            sbPrint.Append("^FO25,545 ^XGrecpeo2^FS");
            sbPrint.Append("^FO325,515^XGqty^FS");
            sbPrint.Append("^FO330,545^XGqty2^FS");

            sbPrint.Append("^FO0,575^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            sbPrint.Append("^FO20,670^XGallin^FS");
            sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,760^BQN,2,4^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
            sbPrint.Append("^IDR:supplyname^FS");
            sbPrint.Append("^IDR:supplyname2^FS");
            sbPrint.Append("^IDR:voucherno^FS");
            sbPrint.Append("^IDR:voucherno2^FS");
            sbPrint.Append("^IDR:supbatch^FS");
            sbPrint.Append("^IDR:supbatch2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:cctj^FS");
            sbPrint.Append("^IDR:cctj2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:protectway^FS");
            sbPrint.Append("^IDR:protectway2^FS");
            sbPrint.Append("^IDR:recdate^FS");
            sbPrint.Append("^IDR:recdate2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:recpeo^FS");
            sbPrint.Append("^IDR:recpeo2^FS");
            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            sbPrint.Append("^IDR:allin^FS");
            sbPrint.Append("^IDR:allin2^FS");
            sbPrint.Append("^IDR:xdbz^FS");
            sbPrint.Append("^IDR:xdbz2^FS");
            //删除图片
            NewDel(model, sbPrint);
            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //外来散装内
        public static string InFromSanZhuang(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(外散装内 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, false, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(订单号, false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ErpVoucherNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(供应商, true, "黑体", "supplyname", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupCode, false, "黑体", "supplyname2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(供应商批号, false, "黑体", "supbatch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupPrdBatch, false, "黑体", "supbatch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(相对比重, false, "黑体", "xdbz", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.RelaWeight, false, "黑体", "xdbz2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate==DateTime.MinValue?"": model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(存储条件, false, "黑体", "cctj", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.StoreCondition, false, "黑体", "cctj2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(防护措施, false, "黑体", "protectway", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProtectWay, false, "黑体", "protectway2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收日期, false, "黑体", "recdate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CreateTime.ToString("yyyy-MM-dd"), false, "黑体", "recdate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收人, false, "黑体", "recpeo", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "recpeo2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(净重, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            //图片
            NewImage(model, sbPrint);


            //尾部


            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());



            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");
            sbPrint.Append("^FO10,20^XGtitle^FS");

            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,480,3^FS");
            //图片
            NewPos(model, sbPrint);

            //体
            sbPrint.Append("^FO20,100 ^XGmateno^FS");
            sbPrint.Append("^FO25,130 ^XGmateno2^FS");
            sbPrint.Append("^FO325,100^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");

            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190 ^XGvoucherno^FS");
            sbPrint.Append("^FO25,220 ^XGvoucherno2^FS");
            sbPrint.Append("^FO325,190^XGbatch^FS");
            sbPrint.Append("^FO330,220^XGbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255 ^XGsupplyname^FS");
            sbPrint.Append("^FO25,285 ^XGsupplyname2^FS");
            sbPrint.Append("^FO325,255^XGsupbatch^FS");
            sbPrint.Append("^FO330,285^XGsupbatch2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGxdbz^FS");
            sbPrint.Append("^FO25,350 ^XGxdbz2^FS");
            sbPrint.Append("^FO325,320^XGprodate^FS");
            sbPrint.Append("^FO330,350^XGprodate2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385 ^XGcctj^FS");
            sbPrint.Append("^FO25,415 ^XGcctj2^FS");
            sbPrint.Append("^FO325,385^XGenddate^FS");
            sbPrint.Append("^FO330,415^XGenddate2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            sbPrint.Append("^FO20,450 ^XGprotectway^FS");
            sbPrint.Append("^FO25,480 ^XGprotectway2^FS");
            sbPrint.Append("^FO325,450^XGrecdate^FS");
            sbPrint.Append("^FO330,480^XGrecdate2^FS");

            sbPrint.Append("^FO0,510^GB610,0,3^FS");

            sbPrint.Append("^FO20,515 ^XGrecpeo^FS");
            sbPrint.Append("^FO25,545 ^XGrecpeo2^FS");
            sbPrint.Append("^FO325,515^XGqty^FS");
            sbPrint.Append("^FO330,545^XGqty2^FS");

            sbPrint.Append("^FO0,575^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
      


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,760^BQN,2,4^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
            sbPrint.Append("^IDR:supplyname^FS");
            sbPrint.Append("^IDR:supplyname2^FS");
            sbPrint.Append("^IDR:voucherno^FS");
            sbPrint.Append("^IDR:voucherno2^FS");
            sbPrint.Append("^IDR:supbatch^FS");
            sbPrint.Append("^IDR:supbatch2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:cctj^FS");
            sbPrint.Append("^IDR:cctj2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:protectway^FS");
            sbPrint.Append("^IDR:protectway2^FS");
            sbPrint.Append("^IDR:recdate^FS");
            sbPrint.Append("^IDR:recdate2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:recpeo^FS");
            sbPrint.Append("^IDR:recpeo2^FS");
            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            sbPrint.Append("^IDR:xdbz^FS");
            sbPrint.Append("^IDR:xdbz2^FS");
            //删除图片
            NewDel(model, sbPrint);
            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //外来半制外
        public static string OutFromBanZhi(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(外半制外 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, false, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(订单号, false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ErpVoucherNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(供应商, true, "黑体", "supplyname", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupCode, false, "黑体", "supplyname2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(供应商批号, false, "黑体", "supbatch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SupPrdBatch, false, "黑体", "supbatch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(混料日期, false, "黑体", "hlrq", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MixDate.ToString("yyyy-MM-dd"), false, "黑体", "hlrq2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate==DateTime.MinValue?"": model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(存储条件, false, "黑体", "cctj", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.StoreCondition, false, "黑体", "cctj2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(料体批次, false, "黑体", "ltpc", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MateBatch, false, "黑体", "ltpc2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收日期, false, "黑体", "recdate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CreateTime.ToString("yyyy-MM-dd"), false, "黑体", "recdate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(接收人, false, "黑体", "recpeo", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "recpeo2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(数量, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //尾部


            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(第几箱总箱数, false, "黑体", "allin", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.BarcodeNo + "/" + model.PrintQty, false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());



            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");
            sbPrint.Append("^FO10,20^XGtitle^FS");

            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,480,3^FS");

            //体
            sbPrint.Append("^FO20,100 ^XGmateno^FS");
            sbPrint.Append("^FO25,130 ^XGmateno2^FS");
            sbPrint.Append("^FO325,100^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");

            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190 ^XGvoucherno^FS");
            sbPrint.Append("^FO25,220 ^XGvoucherno2^FS");
            sbPrint.Append("^FO325,190^XGbatch^FS");
            sbPrint.Append("^FO330,220^XGbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255 ^XGsupplyname^FS");
            sbPrint.Append("^FO25,285 ^XGsupplyname2^FS");
            sbPrint.Append("^FO325,255^XGsupbatch^FS");
            sbPrint.Append("^FO330,285^XGsupbatch2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGhlrq^FS");
            sbPrint.Append("^FO25,350 ^XGhlrq2^FS");
            sbPrint.Append("^FO325,320^XGprodate^FS");
            sbPrint.Append("^FO330,350^XGprodate2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385 ^XGcctj^FS");
            sbPrint.Append("^FO25,415 ^XGcctj2^FS");
            sbPrint.Append("^FO325,385^XGenddate^FS");
            sbPrint.Append("^FO330,415^XGenddate2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            sbPrint.Append("^FO20,450 ^XGltpc^FS");
            sbPrint.Append("^FO25,480 ^XGltpc2^FS");
            sbPrint.Append("^FO325,450^XGrecdate^FS");
            sbPrint.Append("^FO330,480^XGrecdate2^FS");

            sbPrint.Append("^FO0,510^GB610,0,3^FS");

            sbPrint.Append("^FO20,515 ^XGrecpeo^FS");
            sbPrint.Append("^FO25,545 ^XGrecpeo2^FS");
            sbPrint.Append("^FO325,515^XGqty^FS");
            sbPrint.Append("^FO330,545^XGqty2^FS");

            sbPrint.Append("^FO0,575^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            sbPrint.Append("^FO20,670^XGallin^FS");
            sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,760^BQN,2,4^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
            sbPrint.Append("^IDR:supplyname^FS");
            sbPrint.Append("^IDR:supplyname2^FS");
            sbPrint.Append("^IDR:voucherno^FS");
            sbPrint.Append("^IDR:voucherno2^FS");
            sbPrint.Append("^IDR:supbatch^FS");
            sbPrint.Append("^IDR:supbatch2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:cctj^FS");
            sbPrint.Append("^IDR:cctj2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:ltpc^FS");
            sbPrint.Append("^IDR:ltpc2^FS");
            sbPrint.Append("^IDR:recdate^FS");
            sbPrint.Append("^IDR:recdate2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:recpeo^FS");
            sbPrint.Append("^IDR:recpeo2^FS");
            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            sbPrint.Append("^IDR:allin^FS");
            sbPrint.Append("^IDR:allin2^FS");
            sbPrint.Append("^IDR:hlrq^FS");
            sbPrint.Append("^IDR:hlrq2^FS");

            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //半制外
        public static string OutBanZhi(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(半制外 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, false, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(40), true, "黑体", "matename2", 22, 300, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(工单号, false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ErpVoucherNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate==DateTime.MinValue?"": model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(生产班组, false, "黑体", "scbz", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductClass, false, "黑体", "scbz2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(成品批号, false, "黑体", "probatch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductBatch, false, "黑体", "probatch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(数量, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(重量, false, "黑体", "weight", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ItemQty.ToString(), false, "黑体", "weight2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //尾部


            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(第几箱总箱数, false, "黑体", "allin", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.BarcodeNo + "/", false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());



            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");
            sbPrint.Append("^FO10,20^XGtitle^FS");

            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,350,3^FS");

            //体
            sbPrint.Append("^FO20,100 ^XGmateno^FS");
            sbPrint.Append("^FO25,130 ^XGmateno2^FS");
            sbPrint.Append("^FO325,100^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");




            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190 ^XGvoucherno^FS");
            sbPrint.Append("^FO25,220 ^XGvoucherno2^FS");
            sbPrint.Append("^FO325,190^XGbatch^FS");
            sbPrint.Append("^FO330,220^XGbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255 ^XGprodate^FS");
            sbPrint.Append("^FO25,285 ^XGprodate2^FS");
            sbPrint.Append("^FO325,255^XGenddate^FS");
            sbPrint.Append("^FO330,285^XGenddate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGscbz^FS");
            sbPrint.Append("^FO25,350 ^XGscbz2^FS");
            sbPrint.Append("^FO325,320^XGprobatch^FS");
            sbPrint.Append("^FO330,350^XGprobatch2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385^XGqty^FS");
            sbPrint.Append("^FO25,415^XGqty2^FS");
            sbPrint.Append("^FO325,385^XGweight^FS");
            sbPrint.Append("^FO330,415^XGweight2^FS");
            //
            //

            sbPrint.Append("^FO0,445^GB610,0,3^FS");
            //
            //
            //
            //

            //sbPrint.Append("^FO0,510^GB610,0,3^FS");

            //
            //
            //
            //
            //sbPrint.Append("^FO0,575^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            sbPrint.Append("^FO20,670^XGallin^FS");
            sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,740^BQN,2,3^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
            sbPrint.Append("^IDR:voucherno^FS");
            sbPrint.Append("^IDR:voucherno2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:scbz^FS");
            sbPrint.Append("^IDR:scbz2^FS");
            sbPrint.Append("^IDR:probatch^FS");
            sbPrint.Append("^IDR:probatch2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:weight^FS");
            sbPrint.Append("^IDR:weight2^FS");
            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            sbPrint.Append("^IDR:allin^FS");
            sbPrint.Append("^IDR:allin2^FS");
            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //散装外
        public static string OutSanZhuang(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(散装外+ model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, false, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(工单号, false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ErpVoucherNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(相对比重, false, "黑体", "xdbz", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.RelaWeight, false, "黑体", "xdbz2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate==DateTime.MinValue?"": model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(存储条件, false, "黑体", "cctj", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.StoreCondition, false, "黑体", "cctj2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(防护措施, false, "黑体", "fhcs", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProtectWay, false, "黑体", "fhcs2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(净重, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(操作人, false, "黑体", "creater", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "creater2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //尾部
            //图片
            NewImage(model, sbPrint);

            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //这里指大箱里面有多少小箱
            fcb.GETFONTHEX(总件数, false, "黑体", "allin", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.BoxCount+"", false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());



            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");
            sbPrint.Append("^FO10,20^XGtitle^FS");

            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,415,3^FS");

            //图片
            NewPos(model, sbPrint);
            //体
            sbPrint.Append("^FO20,100 ^XGmateno^FS");
            sbPrint.Append("^FO25,130 ^XGmateno2^FS");
            sbPrint.Append("^FO325,100^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");




            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190 ^XGvoucherno^FS");
            sbPrint.Append("^FO25,220 ^XGvoucherno2^FS");
            sbPrint.Append("^FO325,190^XGbatch^FS");
            sbPrint.Append("^FO330,220^XGbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255 ^XGprodate^FS");
            sbPrint.Append("^FO25,285 ^XGprodate2^FS");
            sbPrint.Append("^FO325,255^XGenddate^FS");
            sbPrint.Append("^FO330,285^XGenddate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGxdbz^FS");
            sbPrint.Append("^FO25,350 ^XGxdbz2^FS");
            sbPrint.Append("^FO325,320^XGcctj^FS");
            sbPrint.Append("^FO330,350^XGcctj2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385 ^XGfhcs^FS");
            sbPrint.Append("^FO25,415 ^XGfhcs2^FS");
            sbPrint.Append("^FO325,385^XGqty^FS");
            sbPrint.Append("^FO330,415^XGqty2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            sbPrint.Append("^FO20,450^XGcreater^FS");
            sbPrint.Append("^FO25,480^XGcreater2^FS");
            //
            //

            sbPrint.Append("^FO0,510^GB610,0,3^FS");

            //
            //
            //
            //
            //sbPrint.Append("^FO0,575^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            sbPrint.Append("^FO20,670^XGallin^FS");
            sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,740^BQN,2,3^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
            sbPrint.Append("^IDR:voucherno^FS");
            sbPrint.Append("^IDR:voucherno2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:xdbz^FS");
            sbPrint.Append("^IDR:xdbz2^FS");
            sbPrint.Append("^IDR:cctj^FS");
            sbPrint.Append("^IDR:cctj2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:fhcs^FS");
            sbPrint.Append("^IDR:fhcs2^FS");
            sbPrint.Append("^IDR:creater^FS");
            sbPrint.Append("^IDR:creater2^FS");
            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            sbPrint.Append("^IDR:allin^FS");
            sbPrint.Append("^IDR:allin2^FS");

            //删除图片
            NewDel(model, sbPrint);
            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //散装内
        public static string InSanZhuang(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(散装内 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, false, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(工单号, false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ErpVoucherNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(相对比重, false, "黑体", "xdbz", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.RelaWeight, false, "黑体", "xdbz2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate==DateTime.MinValue?"": model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(存储条件, false, "黑体", "cctj", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.StoreCondition, false, "黑体", "cctj2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(防护措施, false, "黑体", "fhcs", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProtectWay, false, "黑体", "fhcs2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(净重, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(操作人, false, "黑体", "creater", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "creater2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //尾部
            //图片
            NewImage(model, sbPrint);

            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //这里指大箱里面有多少小箱
            fcb.GETFONTHEX(第几件, false, "黑体", "allin", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.BarcodeNo + "", false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());



            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");
            sbPrint.Append("^FO10,20^XGtitle^FS");

            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,415,3^FS");
            //图片
            NewPos(model, sbPrint);
            //体
            sbPrint.Append("^FO20,100 ^XGmateno^FS");
            sbPrint.Append("^FO25,130 ^XGmateno2^FS");
            sbPrint.Append("^FO325,100^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");

           


            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190 ^XGvoucherno^FS");
            sbPrint.Append("^FO25,220 ^XGvoucherno2^FS");
            sbPrint.Append("^FO325,190^XGbatch^FS");
            sbPrint.Append("^FO330,220^XGbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255 ^XGprodate^FS");
            sbPrint.Append("^FO25,285 ^XGprodate2^FS");
            sbPrint.Append("^FO325,255^XGenddate^FS");
            sbPrint.Append("^FO330,285^XGenddate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGxdbz^FS");
            sbPrint.Append("^FO25,350 ^XGxdbz2^FS");
            sbPrint.Append("^FO325,320^XGcctj^FS");
            sbPrint.Append("^FO330,350^XGcctj2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385 ^XGfhcs^FS");
            sbPrint.Append("^FO25,415 ^XGfhcs2^FS");
            sbPrint.Append("^FO325,385^XGqty^FS");
            sbPrint.Append("^FO330,415^XGqty2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            sbPrint.Append("^FO20,450^XGcreater^FS");
            sbPrint.Append("^FO25,480^XGcreater2^FS");
            //
            //

            sbPrint.Append("^FO0,510^GB610,0,3^FS");

            //
            //
            //
            //
            //sbPrint.Append("^FO0,575^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            sbPrint.Append("^FO20,670^XGallin^FS");
            sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,740^BQN,2,3^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
            sbPrint.Append("^IDR:voucherno^FS");
            sbPrint.Append("^IDR:voucherno2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:xdbz^FS");
            sbPrint.Append("^IDR:xdbz2^FS");
            sbPrint.Append("^IDR:cctj^FS");
            sbPrint.Append("^IDR:cctj2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:fhcs^FS");
            sbPrint.Append("^IDR:fhcs2^FS");
            sbPrint.Append("^IDR:creater^FS");
            sbPrint.Append("^IDR:creater2^FS");
            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            sbPrint.Append("^IDR:allin^FS");
            sbPrint.Append("^IDR:allin2^FS");
            //删除图片
            NewDel(model, sbPrint);
            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //成品外
        public static string OutChengPin(Barcode_Model model)
        {


                StringBuilder sbPrint = new StringBuilder();
                StringBuilder sbReturn = new StringBuilder(10240);
                FontConvertBmp fcb = new FontConvertBmp();


                //标题
                fcb.GETFONTHEX(成品外 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());


                //内容
                fcb.GETFONTHEX("产品编号:Pro.Code", false, "黑体", "mateno", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());
                fcb.GETFONTHEX(model.MaterialNo, false, "黑体", "mateno2", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());

                fcb.GETFONTHEX("产品名称:Pro.Name", false, "黑体", "matename", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());
                fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());

                //利用warehouseno字段1代表外来成品
                if (model.warehouseno == "1")
                {
                    fcb.GETFONTHEX(订单号, false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
                    sbPrint.Append(sbReturn.ToString());
                    fcb.GETFONTHEX(model.ErpVoucherNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
                    sbPrint.Append(sbReturn.ToString());
                }
                else
                {
                    fcb.GETFONTHEX("工单号:WO#", false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
                    sbPrint.Append(sbReturn.ToString());
                    fcb.GETFONTHEX(model.ErpVoucherNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
                    sbPrint.Append(sbReturn.ToString());
                }


                fcb.GETFONTHEX("厂内批号:Lot", false, "黑体", "batch", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());
                fcb.GETFONTHEX(model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());

                fcb.GETFONTHEX("生产班组:Pro.group", false, "黑体", "scbz", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());
                fcb.GETFONTHEX(model.ProductClass, false, "黑体", "scbz2", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());

                fcb.GETFONTHEX("生产日期:Pro.date", false, "黑体", "prodate", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());
                fcb.GETFONTHEX(model.ProductDate == DateTime.MinValue ? "" : model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());


                fcb.GETFONTHEX("到期日期:Exp.date", false, "黑体", "enddate", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());
                fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());


                fcb.GETFONTHEX("客户:Customer", false, "黑体", "bzfs", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());
                fcb.GETFONTHEX(model.CusName, false, "黑体", "bzfs2", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());

                fcb.GETFONTHEX("数量:Quantity", false, "黑体", "sl", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());
                fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "sl2", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());

                if (model.warehouseno != "1")
                {
                    fcb.GETFONTHEX("重量:Weight", false, "黑体", "qty", 27, 0, true, false, sbReturn);
                    sbPrint.Append(sbReturn.ToString());
                    fcb.GETFONTHEX(model.ItemQty + "", false, "黑体", "qty2", 27, 0, true, false, sbReturn);
                    sbPrint.Append(sbReturn.ToString());
                }



                //尾部


                fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());
                fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
                sbPrint.Append(sbReturn.ToString());

                if (model.warehouseno != "3")
                {

                    fcb.GETFONTHEX("标准量/箱:", false, "黑体", "xxx", 27, 0, true, false, sbReturn);
                    sbPrint.Append(sbReturn.ToString());

                    if (model.standardbox1 == 0) model.standardbox1 = 1;
                    if (model.standardbox2 == 0) model.standardbox2 = 1;
                    if (model.standardbox3 == 0) model.standardbox3 = 1;
                    int all = model.standardbox1 * model.standardbox2 * model.standardbox3;

                    string text = "";
                    if (model.standardbox1 == 1 && model.standardbox2 == 1 && model.standardbox3 == 1)
                        text = "";
                    else if (model.standardbox1 == 1)
                        text = model.standardbox2 + "*" + model.standardbox3 + "=" + all;
                    else if (model.standardbox2 == 1)
                        text = model.standardbox1 + "*" + model.standardbox3 + "=" + all;
                    else if (model.standardbox3 == 1)
                        text = model.standardbox1 + "*" + model.standardbox2 + "=" + all;
                    else
                        text = model.standardbox1 + "*" + model.standardbox2 + "*" + model.standardbox3 + "=" + all;
                    fcb.GETFONTHEX(text, false, "黑体", "xxx2", 27, 0, true, false, sbReturn);
                    sbPrint.Append(sbReturn.ToString());

                }


                //老蔡外销用
                if (model.warehouseno == "3")
                {
                    fcb.GETFONTHEX(总箱数, false, "黑体", "BoxCount", 27, 0, true, false, sbReturn);
                    sbPrint.Append(sbReturn.ToString());
                    fcb.GETFONTHEX(model.BoxCount + "", false, "黑体", "BoxCount2", 27, 0, true, false, sbReturn);
                    sbPrint.Append(sbReturn.ToString());
                }


                //定位
                sbPrint.Append("^XA");
                sbPrint.Append("^PW0610^LL0730^LS0");
                sbPrint.Append("^FO10,20^XGtitle^FS");

                //横线
                sbPrint.Append("^FO0,95^GB610,0,3^FS");
                //竖线
                sbPrint.Append("^FO320,95^GB0,350,3^FS");

                //体
                sbPrint.Append("^FO20,100 ^XGmateno^FS");
                sbPrint.Append("^FO25,130 ^XGmateno2^FS");
                sbPrint.Append("^FO325,100^XGmatename^FS");
                sbPrint.Append("^FO330,130^XGmatename2^FS");




                sbPrint.Append("^FO0,185^GB610,0,3^FS");

                sbPrint.Append("^FO20,190 ^XGvoucherno^FS");
                sbPrint.Append("^FO25,220 ^XGvoucherno2^FS");
                sbPrint.Append("^FO325,190^XGbatch^FS");
                sbPrint.Append("^FO330,220^XGbatch2^FS");

                sbPrint.Append("^FO0,250^GB610,0,3^FS");

                sbPrint.Append("^FO20,255 ^XGprodate^FS");
                sbPrint.Append("^FO25,285 ^XGprodate2^FS");
                sbPrint.Append("^FO325,255^XGenddate^FS");
                sbPrint.Append("^FO330,285^XGenddate2^FS");

                sbPrint.Append("^FO0,315^GB610,0,3^FS");

                sbPrint.Append("^FO20,320 ^XGscbz^FS");
                sbPrint.Append("^FO25,350 ^XGscbz2^FS");
                sbPrint.Append("^FO325,320^XGbzfs^FS");
                sbPrint.Append("^FO330,350^XGbzfs2^FS");

                sbPrint.Append("^FO0,380^GB610,0,3^FS");

                sbPrint.Append("^FO20,385 ^XGsl^FS");
                sbPrint.Append("^FO25,415 ^XGsl2^FS");

                if (model.warehouseno != "1")
                {
                    sbPrint.Append("^FO325,385^XGqty^FS");
                    sbPrint.Append("^FO330,415^XGqty2^FS");
                }

                sbPrint.Append("^FO0,445^GB610,0,3^FS");

                //sbPrint.Append("^FO20,430^XGcreater^FS");
                //sbPrint.Append("^FO25,460^XGcreater2^FS");
                //
                //

                //sbPrint.Append("^FO0,510^GB610,0,3^FS");

                //
                //
                //
                //
                //sbPrint.Append("^FO0,575^GB610,0,3^FS");

                //尾
                sbPrint.Append("^FO20,600 ^XGserialno^FS");
                sbPrint.Append("^FO20,630^XGserialno2^FS");

                if (model.warehouseno != "3")
                {
                    sbPrint.Append("^FO20,660 ^XGxxx^FS");
                    sbPrint.Append("^FO20,690^XGxxx2^FS");
                }

                if (model.warehouseno == "3")
                {
                    sbPrint.Append("^FO20,670 ^XGBoxCount^FS");
                    sbPrint.Append("^FO20,700^XGBoxCount2^FS");
                }


                //FT45,180^BQN,2,3
                //BQ FT35,180^BQN,2,3
            //BYw,r,h
                sbPrint.Append("^FT380,740^BQN,2,3^FDQA," + model.BarCode + "^FS");
                sbPrint.Append("^BY3,2,30^FT60,490^BEN,,Y,N^FD" + model.ErpBarCode + "^FS");
                if (!string.IsNullOrEmpty(model.ZXBARCODE))
                    sbPrint.Append("^BY3,2,30^FT60,575^BCN,,N,N^FD" + model.ZXBARCODE + "^FS");



                sbPrint.Append("^XZ");

                //删除图片
                sbPrint.Append("^XA");
                sbPrint.Append("^IDR:title^FS");
                sbPrint.Append("^IDR:mateno^FS");
                sbPrint.Append("^IDR:mateno2^FS");
                sbPrint.Append("^IDR:matename^FS");
                sbPrint.Append("^IDR:matename2^FS");
                sbPrint.Append("^IDR:voucherno^FS");
                sbPrint.Append("^IDR:voucherno2^FS");
                sbPrint.Append("^IDR:batch^FS");
                sbPrint.Append("^IDR:batch2^FS");
                sbPrint.Append("^IDR:prodate^FS");
                sbPrint.Append("^IDR:prodate2^FS");
                sbPrint.Append("^IDR:enddate^FS");
                sbPrint.Append("^IDR:enddate2^FS");
                sbPrint.Append("^IDR:scbz^FS");
                sbPrint.Append("^IDR:scbz2^FS");
                sbPrint.Append("^IDR:bzfs^FS");
                sbPrint.Append("^IDR:bzfs2^FS");
                if (model.warehouseno != "1")
                {
                    sbPrint.Append("^IDR:qty^FS");
                    sbPrint.Append("^IDR:qty2^FS");
                }
                sbPrint.Append("^IDR:sl^FS");
                sbPrint.Append("^IDR:sl2^FS");

                sbPrint.Append("^IDR:serialno^FS");
                sbPrint.Append("^IDR:serialno2^FS");
                if (model.warehouseno != "3")
                {
                    sbPrint.Append("^IDR:xxx^FS");
                    sbPrint.Append("^IDR:xxx2^FS");
                }
                if (model.warehouseno == "3")
                {
                    sbPrint.Append("^IDR:BoxCount^FS");
                    sbPrint.Append("^IDR:BoxCount2^FS");
                }


                //sbPrint.Append("^IDR:allin^FS");
                //sbPrint.Append("^IDR:allin2^FS");
                sbPrint.Append("^XZ");
                return sbPrint.ToString();
            }

        //成品托
        public static string TOutChengPin(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(成品托 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, false, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(工单号, false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ErpVoucherNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(生产班组, false, "黑体", "scbz", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductClass, false, "黑体", "scbz2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate == DateTime.MinValue ? "" : model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(包装方式, false, "黑体", "bzfs", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.BoxWeight, false, "黑体", "bzfs2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(数量, false, "黑体", "sl", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "sl2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //fcb.GETFONTHEX(重量, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());
            //fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());


            //尾部


            fcb.GETFONTHEX(托盘号, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.PalletNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(总箱数, false, "黑体", "BoxCount", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.BoxCount + "", false, "黑体", "BoxCount2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());





            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");
            sbPrint.Append("^FO10,20^XGtitle^FS");

            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,350,3^FS");

            //体
            sbPrint.Append("^FO20,100 ^XGmateno^FS");
            sbPrint.Append("^FO25,130 ^XGmateno2^FS");
            sbPrint.Append("^FO325,100^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");




            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190 ^XGvoucherno^FS");
            sbPrint.Append("^FO25,220 ^XGvoucherno2^FS");
            sbPrint.Append("^FO325,190^XGbatch^FS");
            sbPrint.Append("^FO330,220^XGbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255 ^XGprodate^FS");
            sbPrint.Append("^FO25,285 ^XGprodate2^FS");
            sbPrint.Append("^FO325,255^XGenddate^FS");
            sbPrint.Append("^FO330,285^XGenddate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGscbz^FS");
            sbPrint.Append("^FO25,350 ^XGscbz2^FS");
            sbPrint.Append("^FO325,320^XGbzfs^FS");
            sbPrint.Append("^FO330,350^XGbzfs2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385 ^XGsl^FS");
            sbPrint.Append("^FO25,415 ^XGsl2^FS");
            //sbPrint.Append("^FO325,385^XGqty^FS");
            //sbPrint.Append("^FO330,415^XGqty2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            //sbPrint.Append("^FO20,430^XGcreater^FS");
            //sbPrint.Append("^FO25,460^XGcreater2^FS");
            //
            //

            //sbPrint.Append("^FO0,510^GB610,0,3^FS");

            //
            //
            //
            //
            //sbPrint.Append("^FO0,575^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");

            sbPrint.Append("^FO20,670 ^XGBoxCount^FS");
            sbPrint.Append("^FO20,700^XGBoxCount2^FS");

            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,740^BQN,2,3^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
            sbPrint.Append("^IDR:voucherno^FS");
            sbPrint.Append("^IDR:voucherno2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:scbz^FS");
            sbPrint.Append("^IDR:scbz2^FS");
            sbPrint.Append("^IDR:bzfs^FS");
            sbPrint.Append("^IDR:bzfs2^FS");
            //sbPrint.Append("^IDR:qty^FS");
            //sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:sl^FS");
            sbPrint.Append("^IDR:sl2^FS");

            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            sbPrint.Append("^IDR:BoxCount^FS");
            sbPrint.Append("^IDR:BoxCount2^FS");
            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //包材退
        public static string OutBaoCaiTui(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(包材退外 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, true, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(工单号, false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.WorkNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(存储条件, false, "黑体", "cctj", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.StoreCondition, false, "黑体", "cctj2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(退料日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CreateTime.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(退料单位, false, "黑体", "tldw", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Unit, false, "黑体", "tldw2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(退料人, false, "黑体", "recpeo", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "recpeo2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(数量, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //尾部
            fcb.GETFONTHEX(装箱明细, false, "黑体", "zxmx", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.BoxDetail, false, "黑体", "zxmx2", 26, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //fcb.GETFONTHEX(第几箱总箱数, false, "黑体", "allin", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());
            //fcb.GETFONTHEX(model.BarcodeNo + "/" + model.PrintQty, false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());



            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");
            sbPrint.Append("^FO10,20^XGtitle^FS");


            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,350,3^FS");

            //体
            sbPrint.Append("^FO20,100 ^XGmateno^FS");
            sbPrint.Append("^FO25,130  ^XGmateno2^FS");
            sbPrint.Append("^FO325,100 ^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");

            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190  ^XGvoucherno^FS");
            sbPrint.Append("^FO25,220 ^XGvoucherno2^FS");
            sbPrint.Append("^FO325,190^XGbatch^FS");
            sbPrint.Append("^FO330,220^XGbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255  ^XGcctj^FS");
            sbPrint.Append("^FO25,285 ^XGcctj2^FS");
            sbPrint.Append("^FO325,255^XGprodate^FS");
            sbPrint.Append("^FO330,285^XGprodate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGtldw^FS");
            sbPrint.Append("^FO25,350 ^XGtldw2^FS");
            sbPrint.Append("^FO325,320^XGenddate^FS");
            sbPrint.Append("^FO330,350^XGenddate2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385 ^XGrecpeo^FS");
            sbPrint.Append("^FO25,415 ^XGrecpeo2^FS");
            sbPrint.Append("^FO325,385^XGqty^FS");
            sbPrint.Append("^FO330,415^XGqty2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            //sbPrint.Append("^FO20,430  ^XGrecpeo^FS");
            //sbPrint.Append("^FO25,460 ^XGrecpeo2^FS");
            //sbPrint.Append("^FO325,430^XGqty^FS");
            //sbPrint.Append("^FO330,460^XGqty2^FS");

            //sbPrint.Append("^FO0,490^GB610,0,3^FS");




            //sbPrint.Append("^FO0,555^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,530^XGzxmx^FS");
            sbPrint.Append("^FO20,560^XGzxmx2^FS");
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            //sbPrint.Append("^FO20,670^XGallin^FS");
            //sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,760^BQN,2,4^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
            sbPrint.Append("^IDR:voucherno^FS");
            sbPrint.Append("^IDR:voucherno2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:cctj^FS");
            sbPrint.Append("^IDR:cctj2^FS");

            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:tldw^FS");
            sbPrint.Append("^IDR:tldw2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:recpeo^FS");
            sbPrint.Append("^IDR:recpeo2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:zxmx^FS");
            sbPrint.Append("^IDR:zxmx2^FS");
            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            //sbPrint.Append("^IDR:allin^FS");
            //sbPrint.Append("^IDR:allin2^FS");

            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //散装退
        public static string OutSanZhuangTui(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(散装退外 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, false, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(工单号, false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.WorkNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(相对比重, false, "黑体", "xdbz", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.RelaWeight, false, "黑体", "xdbz2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(退料日期, false, "黑体", "recdate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CreateTime.ToString("yyyy-MM-dd"), false, "黑体", "recdate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(存储条件, false, "黑体", "cctj", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.StoreCondition, false, "黑体", "cctj2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(防护措施, false, "黑体", "fhcs", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProtectWay, false, "黑体", "fhcs2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(净重, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(退料人, false, "黑体", "creater", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "creater2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate==DateTime.MinValue?"": model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(退料单位, false, "黑体", "tldw", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Unit, false, "黑体", "tldw2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //尾部
            //图片
            NewImage(model, sbPrint);

            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //这里指大箱里面有多少小箱
            //fcb.GETFONTHEX("总件数：", false, "黑体", "allin", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());
            //fcb.GETFONTHEX(model.PrintQty + "", false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());



            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");
            sbPrint.Append("^FO10,20^XGtitle^FS");

            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,480,3^FS");
            //图片
            NewPos(model, sbPrint);
            //体
            sbPrint.Append("^FO20,100 ^XGmateno^FS");
            sbPrint.Append("^FO25,130 ^XGmateno2^FS");
            sbPrint.Append("^FO325,100^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");




            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190 ^XGvoucherno^FS");
            sbPrint.Append("^FO25,220 ^XGvoucherno2^FS");
            sbPrint.Append("^FO325,190^XGbatch^FS");
            sbPrint.Append("^FO330,220^XGbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255 ^XGrecdate^FS");
            sbPrint.Append("^FO25,285 ^XGrecdate2^FS");
            sbPrint.Append("^FO325,255^XGenddate^FS");
            sbPrint.Append("^FO330,285^XGenddate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGxdbz^FS");
            sbPrint.Append("^FO25,350 ^XGxdbz2^FS");
            sbPrint.Append("^FO325,320^XGcctj^FS");
            sbPrint.Append("^FO330,350^XGcctj2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385 ^XGfhcs^FS");
            sbPrint.Append("^FO25,415 ^XGfhcs2^FS");
            sbPrint.Append("^FO325,385^XGqty^FS");
            sbPrint.Append("^FO330,415^XGqty2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            sbPrint.Append("^FO20,450 ^XGcreater^FS");
            sbPrint.Append("^FO25,480 ^XGcreater2^FS");
            sbPrint.Append("^FO325,450^XGprodate^FS");
            sbPrint.Append("^FO330,480^XGprodate2^FS");

            sbPrint.Append("^FO0,510^GB610,0,3^FS");

            sbPrint.Append("^FO20,515 ^XGtldw^FS");
            sbPrint.Append("^FO25,545 ^XGtldw2^FS");
            //
            //
            sbPrint.Append("^FO0,575^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            //sbPrint.Append("^FO20,670^XGallin^FS");
            //sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,760^BQN,2,4^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
            sbPrint.Append("^IDR:voucherno^FS");
            sbPrint.Append("^IDR:voucherno2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:recdate^FS");
            sbPrint.Append("^IDR:recdate2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            sbPrint.Append("^IDR:xdbz^FS");
            sbPrint.Append("^IDR:xdbz2^FS");
            sbPrint.Append("^IDR:cctj^FS");
            sbPrint.Append("^IDR:cctj2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:fhcs^FS");
            sbPrint.Append("^IDR:fhcs2^FS");
            sbPrint.Append("^IDR:creater^FS");
            sbPrint.Append("^IDR:creater2^FS");
            sbPrint.Append("^IDR:tldw^FS");
            sbPrint.Append("^IDR:tldw2^FS");

            //删除图片
            NewDel(model, sbPrint);

            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            //sbPrint.Append("^IDR:allin^FS");
            //sbPrint.Append("^IDR:allin2^FS");
            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //半制退
        public static string OutBanZhiTui(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(半制退外 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, false, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(工单号, false, "黑体", "voucherno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.WorkNo, false, "黑体", "voucherno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(退料日期, false, "黑体", "recdate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CreateTime.ToString("yyyy-MM-dd"), false, "黑体", "recdate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(存储条件, false, "黑体", "cctj", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.StoreCondition, false, "黑体", "cctj2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(防护措施, false, "黑体", "fhcs", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProtectWay, false, "黑体", "fhcs2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(净重, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(退料人, false, "黑体", "creater", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "creater2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate==DateTime.MinValue?"": model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(退料单位, false, "黑体", "tldw", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Unit, false, "黑体", "tldw2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //尾部
            //图片
            NewImage(model, sbPrint);

            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            //这里指大箱里面有多少小箱
            //fcb.GETFONTHEX("总件数：", false, "黑体", "allin", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());
            //fcb.GETFONTHEX(model.PrintQty + "", false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());



            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");
            sbPrint.Append("^FO10,20^XGtitle^FS");

            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,415,3^FS");
            //图片
            NewImage(model, sbPrint);
            //体
            sbPrint.Append("^FO20,100 ^XGmateno^FS");
            sbPrint.Append("^FO25,130 ^XGmateno2^FS");
            sbPrint.Append("^FO325,100^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");




            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190 ^XGvoucherno^FS");
            sbPrint.Append("^FO25,220 ^XGvoucherno2^FS");
            sbPrint.Append("^FO325,190^XGbatch^FS");
            sbPrint.Append("^FO330,220^XGbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255 ^XGrecdate^FS");
            sbPrint.Append("^FO25,285 ^XGrecdate2^FS");
            sbPrint.Append("^FO325,255^XGenddate^FS");
            sbPrint.Append("^FO330,285^XGenddate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGtldw^FS");
            sbPrint.Append("^FO25,350 ^XGtldw2^FS");
            sbPrint.Append("^FO325,320^XGcctj^FS");
            sbPrint.Append("^FO330,350^XGcctj2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            sbPrint.Append("^FO20,385 ^XGfhcs^FS");
            sbPrint.Append("^FO25,415 ^XGfhcs2^FS");
            sbPrint.Append("^FO325,385^XGqty^FS");
            sbPrint.Append("^FO330,415^XGqty2^FS");

            sbPrint.Append("^FO0,445^GB610,0,3^FS");

            sbPrint.Append("^FO20,450 ^XGcreater^FS");
            sbPrint.Append("^FO25,480 ^XGcreater2^FS");
            sbPrint.Append("^FO325,450^XGprodate^FS");
            sbPrint.Append("^FO330,480^XGprodate2^FS");

            sbPrint.Append("^FO0,510^GB610,0,3^FS");

            //sbPrint.Append("^FO20,495^XGtldw^FS");
            //sbPrint.Append("^FO25,525^XGtldw2^FS");
            //
            //
            //sbPrint.Append("^FO0,575^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            //sbPrint.Append("^FO20,670^XGallin^FS");
            //sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,760^BQN,2,4^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
            sbPrint.Append("^IDR:voucherno^FS");
            sbPrint.Append("^IDR:voucherno2^FS");
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:recdate^FS");
            sbPrint.Append("^IDR:recdate2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");
            //sbPrint.Append("^IDR:xdbz^FS");
            //sbPrint.Append("^IDR:xdbz2^FS");
            sbPrint.Append("^IDR:cctj^FS");
            sbPrint.Append("^IDR:cctj2^FS");
            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:fhcs^FS");
            sbPrint.Append("^IDR:fhcs2^FS");
            sbPrint.Append("^IDR:creater^FS");
            sbPrint.Append("^IDR:creater2^FS");
            sbPrint.Append("^IDR:tldw^FS");
            sbPrint.Append("^IDR:tldw2^FS");

            //删除图片
            NewDel(model, sbPrint);


            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            //sbPrint.Append("^IDR:allin^FS");
            //sbPrint.Append("^IDR:allin2^FS");
            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //成品退
        public static string OutChengPinTui(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(成品退外+ model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, false, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(生产日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductDate==DateTime.MinValue?"": model.ProductDate.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(到期日期, false, "黑体", "enddate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.EDate.ToString("yyyy-MM-dd"), false, "黑体", "enddate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(数量, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //尾部
            fcb.GETFONTHEX(第几箱总箱数, false, "黑体", "allin", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.BarcodeNo + "/" + model.PrintQty, false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());




            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");
            sbPrint.Append("^FO10,20^XGtitle^FS");

            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,220,3^FS");

            //体
            sbPrint.Append("^FO20,100 ^XGmateno^FS");
            sbPrint.Append("^FO25,130 ^XGmateno2^FS");
            sbPrint.Append("^FO325,100^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");




            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190 ^XGprodate^FS");
            sbPrint.Append("^FO25,220 ^XGprodate2^FS");
            sbPrint.Append("^FO325,190^XGbatch^FS");
            sbPrint.Append("^FO330,220^XGbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255 ^XGqty^FS");
            sbPrint.Append("^FO25,285 ^XGqty2^FS");
            sbPrint.Append("^FO325,255^XGenddate^FS");
            sbPrint.Append("^FO330,285^XGenddate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            //sbPrint.Append("^FO20,320 ^XGscbz^FS");
            //sbPrint.Append("^FO25,350 ^XGscbz2^FS");
            //sbPrint.Append("^FO325,320^XGbzfs^FS");
            //sbPrint.Append("^FO330,350^XGbzfs2^FS");

            //sbPrint.Append("^FO0,380^GB610,0,3^FS");

            //sbPrint.Append("^FO20,365^XGsl^FS");
            //sbPrint.Append("^FO25,395^XGsl2^FS");
            //sbPrint.Append("^FO325,365^XGqty^FS");
            //sbPrint.Append("^FO330,395^XGqty2^FS");

            //sbPrint.Append("^FO0,445^GB610,0,3^FS");

            //sbPrint.Append("^FO20,430^XGcreater^FS");
            //sbPrint.Append("^FO25,460^XGcreater2^FS");
            //
            //

            //sbPrint.Append("^FO0,510^GB610,0,3^FS");

            //
            //
            //
            //
            //sbPrint.Append("^FO0,575^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            sbPrint.Append("^FO20,670^XGallin^FS");
            sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,760^BQN,2,4^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");
 
            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:enddate^FS");
            sbPrint.Append("^IDR:enddate2^FS");

            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
  
            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            sbPrint.Append("^IDR:allin^FS");
            sbPrint.Append("^IDR:allin2^FS");
            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //报废料
        public static string OutBaoFei(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX(报废料外 + model.areano, false, "黑体", "title", titlesize, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX(产品编号, false, "黑体", "mateno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialNo, false, "黑体", "mateno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(产品名称, false, "黑体", "matename", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.MaterialDesc.StrCut(42), true, "黑体", "matename2", 25, 290, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(工单号, false, "黑体", "workno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.WorkNo, false, "黑体", "workno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(厂内批号, false, "黑体", "batch", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX( model.BatchNo, false, "宋体", "batch2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            fcb.GETFONTHEX(退料班组, false, "黑体", "tlbz", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.ProductClass, false, "黑体", "tlbz2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(退料日期, false, "黑体", "prodate", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.CreateTime.ToString("yyyy-MM-dd"), false, "黑体", "prodate2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            
            fcb.GETFONTHEX(退料人, false, "黑体", "recpeo", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Creater, false, "黑体", "recpeo2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(数量, false, "黑体", "qty", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.Qty.ToString() + model.Unit, false, "黑体", "qty2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //尾部
            //fcb.GETFONTHEX(第几箱总箱数, false, "黑体", "allin", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());
            //fcb.GETFONTHEX(model.BarcodeNo + "/" + model.PrintQty, false, "黑体", "allin2", 27, 0, true, false, sbReturn);
            //sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX(箱码, false, "黑体", "serialno", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "serialno2", 27, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());




            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");
            sbPrint.Append("^FO10,20^XGtitle^FS");

            //横线
            sbPrint.Append("^FO0,95^GB610,0,3^FS");
            //竖线
            sbPrint.Append("^FO320,95^GB0,285,3^FS");

            //体
            sbPrint.Append("^FO20,100 ^XGmateno^FS");
            sbPrint.Append("^FO25,130 ^XGmateno2^FS");
            sbPrint.Append("^FO325,100^XGmatename^FS");
            sbPrint.Append("^FO330,130^XGmatename2^FS");




            sbPrint.Append("^FO0,185^GB610,0,3^FS");

            sbPrint.Append("^FO20,190 ^XGworkno^FS");
            sbPrint.Append("^FO25,220 ^XGworkno2^FS");
            sbPrint.Append("^FO325,190^XGbatch^FS");
            sbPrint.Append("^FO330,220^XGbatch2^FS");

            sbPrint.Append("^FO0,250^GB610,0,3^FS");

            sbPrint.Append("^FO20,255 ^XGtlbz^FS");
            sbPrint.Append("^FO25,285 ^XGtlbz2^FS");
            sbPrint.Append("^FO325,255^XGprodate^FS");
            sbPrint.Append("^FO330,285^XGprodate2^FS");

            sbPrint.Append("^FO0,315^GB610,0,3^FS");

            sbPrint.Append("^FO20,320 ^XGrecpeo^FS");
            sbPrint.Append("^FO25,350 ^XGrecpeo2^FS");
            sbPrint.Append("^FO325,320^XGqty^FS");
            sbPrint.Append("^FO330,350^XGqty2^FS");

            sbPrint.Append("^FO0,380^GB610,0,3^FS");

            //sbPrint.Append("^FO20,365^XGsl^FS");
            //sbPrint.Append("^FO25,395^XGsl2^FS");
            //sbPrint.Append("^FO325,365^XGqty^FS");
            //sbPrint.Append("^FO330,395^XGqty2^FS");

            //sbPrint.Append("^FO0,445^GB610,0,3^FS");

            //sbPrint.Append("^FO20,430^XGcreater^FS");
            //sbPrint.Append("^FO25,460^XGcreater2^FS");
            //
            //

            //sbPrint.Append("^FO0,510^GB610,0,3^FS");

            //
            //
            //
            //
            //sbPrint.Append("^FO0,575^GB610,0,3^FS");

            //尾
            sbPrint.Append("^FO20,600 ^XGserialno^FS");
            sbPrint.Append("^FO20,630^XGserialno2^FS");
            //sbPrint.Append("^FO20,670^XGallin^FS");
            //sbPrint.Append("^FO20,700^XGallin2^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT380,760^BQN,2,4^FDQA," + model.BarCode + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:mateno^FS");
            sbPrint.Append("^IDR:mateno2^FS");
            sbPrint.Append("^IDR:matename^FS");
            sbPrint.Append("^IDR:matename2^FS");

            sbPrint.Append("^IDR:batch^FS");
            sbPrint.Append("^IDR:batch2^FS");
            sbPrint.Append("^IDR:prodate^FS");
            sbPrint.Append("^IDR:prodate2^FS");
            sbPrint.Append("^IDR:workno^FS");
            sbPrint.Append("^IDR:workno2^FS");

            sbPrint.Append("^IDR:qty^FS");
            sbPrint.Append("^IDR:qty2^FS");
            sbPrint.Append("^IDR:tlbz^FS");
            sbPrint.Append("^IDR:tlbz2^FS");
            sbPrint.Append("^IDR:recpeo^FS");
            sbPrint.Append("^IDR:recpeo2^FS");

            sbPrint.Append("^IDR:serialno^FS");
            sbPrint.Append("^IDR:serialno2^FS");
            //sbPrint.Append("^IDR:allin^FS");
            //sbPrint.Append("^IDR:allin2^FS");
            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }

        //人员打印
        public static string Man(Barcode_Model model)
        {
            StringBuilder sbPrint = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder(10240);
            FontConvertBmp fcb = new FontConvertBmp();


            //标题
            fcb.GETFONTHEX("信息标签", false, "黑体", "title", 80, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //内容
            fcb.GETFONTHEX("名称", false, "黑体", "name", 50, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.RecPeo, false, "黑体", "name2", 60, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());

            fcb.GETFONTHEX("编号", false, "黑体", "no", 50, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());
            fcb.GETFONTHEX(model.SerialNo, false, "黑体", "no2", 60, 0, true, false, sbReturn);
            sbPrint.Append(sbReturn.ToString());


            //定位
            sbPrint.Append("^XA");
            sbPrint.Append("^PW0610^LL0730^LS0");

            sbPrint.Append("^FO50,60^XGtitle^FS");

            //横线
            sbPrint.Append("^FO0,150^GB610,0,3^FS");
           

            //体
            sbPrint.Append("^FO30,180 ^XGname^FS");
            sbPrint.Append("^FO30,250 ^XGname2^FS");
            sbPrint.Append("^FO30,340^XGno^FS");
            sbPrint.Append("^FO30,410^XGno2^FS");

            //横线
            sbPrint.Append("^FO0,490^GB610,0,3^FS");


            //FT45,180^BQN,2,3
            //BQ FT35,180^BQN,2,3
            sbPrint.Append("^FT230,750^BQN,2,9^FDQA," + model.SerialNo + "^FS");

            sbPrint.Append("^XZ");

            //删除图片
            sbPrint.Append("^XA");
            sbPrint.Append("^IDR:title^FS");
            sbPrint.Append("^IDR:name^FS");
            sbPrint.Append("^IDR:name2^FS");
            sbPrint.Append("^IDR:no^FS");
            sbPrint.Append("^IDR:no2^FS");

            sbPrint.Append("^XZ");
            return sbPrint.ToString();
        }
    }
}
