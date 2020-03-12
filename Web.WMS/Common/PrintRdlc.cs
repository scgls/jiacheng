using System;
using System.IO;
using System.Text;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;

namespace Web.WMS.Common
{
    public class PrintRdlc
    {

        public static string ConvertImageToString(System.Drawing.Image imgOrderNo)
        {
            byte[] BImage = ImageToBytes(imgOrderNo, System.Drawing.Imaging.ImageFormat.Bmp);
            return Convert.ToBase64String(BImage);
        }

        public static byte[] ImageToBytes(System.Drawing.Image Image, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            if (Image == null) { return null; }
            byte[] data;
            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap Bitmap = new Bitmap(Image))
                {
                    Bitmap.Save(ms, imageFormat);
                    ms.Position = 0;
                    data = new byte[ms.Length];
                    ms.Read(data, 0, Convert.ToInt32(ms.Length));
                    ms.Flush();
                }
            }
            return data;
        }

        public static System.Drawing.Image GetQRImg(string barcode)
        {
            QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//编码方式(注意：BYTE能支持中文，ALPHA_NUMERIC扫描出来的都是数字)
            encoder.QRCodeScale = 4;//大小(值越大生成的二维码图片像素越高)
            encoder.QRCodeVersion = 0;//版本(注意：设置为0主要是防止编码的字符串太长时发生错误)
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;//错误效验、错误更正(有4个等级)
            String qrdata = barcode;
            System.Drawing.Bitmap bp = encoder.Encode(qrdata.ToString(), Encoding.Default);
            return bp;
        }

    }
}