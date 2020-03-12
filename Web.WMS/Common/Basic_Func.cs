using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Web.WMS.Common
{
    public class Basic_Func
    {
        private const string pukey = "XianDa00";
        private const string pvkey = "SCGWMS00";

        public static string JiaMi(string MingWen)
        {
            try
            {
                if (string.IsNullOrEmpty(MingWen))
                {
                    return string.Empty;
                }

                string strCipherText = "";
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray;
                inputByteArray = Encoding.Default.GetBytes(MingWen);
                des.Key = Encoding.Default.GetBytes(pukey);
                des.IV = Encoding.Default.GetBytes(pvkey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                strCipherText = ret.ToString();
                return strCipherText;
            }
            catch
            {
                return string.Empty;
            }

        }

        public static string JieMi(string MiWen)
        {
            try
            {
                if (string.IsNullOrEmpty(MiWen))
                {
                    return string.Empty;
                }

                if (string.IsNullOrEmpty(MiWen) || MiWen.Length <= 0)
                    return null;

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                int len;
                len = MiWen.Length / 2;
                byte[] inputByteArray = new byte[len];
                int x, i;
                for (x = 0; x < len; x++)
                {
                    i = Convert.ToInt32(MiWen.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }
                des.Key = ASCIIEncoding.ASCII.GetBytes(pukey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(pvkey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.Default.GetString(ms.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }


    }
}
