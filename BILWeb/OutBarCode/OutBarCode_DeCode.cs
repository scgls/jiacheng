using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BILWeb.OutBarCode
{
    public class OutBarCode_DeCode
    {
        private static int BarcodeType = 0;
        private static int SerialNo =5;
        private static int MaterialNo = 2;
        private static int EAN = 3;
        


        /// <summary>
        /// 检测条码是否为有效条码
        /// </summary>
        /// <param name="strBarcode"></param>
        /// <returns></returns>
        public static bool InvalidBarcode(string strBarcode)
        {
            string[] strSplit = strBarcode.Split('@');

            if (strSplit.Length == 6) return true;
            else return false;
        }

        /// <summary>
        /// 判断是否二维码
        /// </summary>
        /// <param name="strBarcode"></param>
        /// <returns></returns>
        public static bool InvalidTwoBarcode(string strBarcode)
        {
            string[] strSplit = strBarcode.Split('@');

            if (strSplit.Length == 2) return true;//二维码
            else return false;
        }

        /// <summary>
        /// 获取条码类型，1-外箱条码 0-内盒条码，2-托盘
        /// </summary>
        /// <param name="strBarcode"></param>
        /// <returns></returns>
        public static string GetBarcodeType(string strBarcode)
        {
            string[] strSplit = strBarcode.Split('@');
            return strSplit[BarcodeType];
        }

        public static string GetSubBarcodeType(string strBarcode)
        {
            string strSplit = strBarcode.Substring(0, 1);
            return strSplit;
        }

        public static string GetSubBarcodeSerialNo(string strBarcode)
        {
            string[] strSplit = strBarcode.Split('@');
            return strSplit[SerialNo];
        }
        

        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <param name="strBarcode"></param>
        /// <returns></returns>
        public static string GetSerialNo(string strBarcode)
        {
            string[] strSplit = strBarcode.Split('@');
            return strSplit[strSplit.Length - 1];
        }

        /// <summary>
        /// 获取最后一位流水号
        /// </summary>
        /// <param name="strBarcode"></param>
        /// <returns></returns>
        public static string GetEndSerialNo(string strBarcode)
        {
            string[] strSplit = strBarcode.Split('@');
            return strSplit[strSplit.Length-1];
        }

        public static string GetMaterialNo(string strBarcode)
        {
            string[] strSplit = strBarcode.Split('@');
            return strSplit[MaterialNo];
        }


        public static string GeEAN(string strBarcode)
        {
            string[] strSplit = strBarcode.Split('@');
            return strSplit[EAN];
        }
    }
}
