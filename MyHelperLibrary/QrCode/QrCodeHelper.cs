using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ZXing;
using ZXing.QrCode.Internal;

namespace MyHelper.QrCode
{
    public class QrCodeHelper
    {
        /// <summary>
        /// 生成二维码
        /// 
        /// </summary>
        /// <param name="content">加密内容，不能为空</param>
        /// <param name="w">宽</param>
        /// <param name="h">高</param>
        /// <returns></returns>
        public static Bitmap GenerateQrCode(String content, int w = 80, int h = 80)
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
           Bitmap bitmap = null;
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            barcodeWriter.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            barcodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.M);
            barcodeWriter.Options.Width = w;
            barcodeWriter.Options.Height = h;
            barcodeWriter.Options.Margin =1;
            ZXing.Common.BitMatrix bitMatrix = barcodeWriter.Encode(content);
            bitmap = barcodeWriter.Write(bitMatrix);
            return bitmap;
        }
        /// <summary>
        /// 读取二维码内容
        /// </summary>
        /// <param name="imgPath"> 路径 </param>
        /// <returns></returns>
        public String ReadQrCode(String imgPath)
        {
            try
            {
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(imgPath);
                ZXing.BitmapLuminanceSource bitmapLuminanceSource = new BitmapLuminanceSource(bitmap);
                BinaryBitmap image = new BinaryBitmap(new ZXing.Common.HybridBinarizer(bitmapLuminanceSource));
                ZXing.QrCode.QRCodeReader qRCodeReader = new ZXing.QrCode.QRCodeReader();
                Result res = qRCodeReader.decode(image);
                return res.Text;
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 读取二维码内容
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public String ReadQrCode(Bitmap bitmap)
        {
            try
            {               
                ZXing.BitmapLuminanceSource bitmapLuminanceSource = new BitmapLuminanceSource(bitmap);
                BinaryBitmap image = new BinaryBitmap(new ZXing.Common.HybridBinarizer(bitmapLuminanceSource));
                ZXing.QrCode.QRCodeReader qRCodeReader = new ZXing.QrCode.QRCodeReader();
                Result res = qRCodeReader.decode(image);
                return res.Text;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 生成条码
        /// 
        /// </summary>
        /// <param name="content">加密内容，不能为空</param>
        /// <param name="w">宽</param>
        /// <param name="h">高</param>
        /// <returns></returns>
        public static Bitmap GenerateBarCode(String content, int w = 80, int h = 30)
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            Bitmap bitmap = null;
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.EAN_13;
            barcodeWriter.Options.Hints.Add(EncodeHintType.CHARACTER_SET,"UTF-8");
            barcodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.H);
            barcodeWriter.Options.Width = w;
            barcodeWriter.Options.Height = h;
            barcodeWriter.Options.Margin = 0;
            ZXing.Common.BitMatrix bitMatrix = barcodeWriter.Encode(content);
            bitmap = barcodeWriter.Write(bitMatrix);
            return bitmap;
        }

        /// <summary>
        /// 读取条码内容
        /// </summary>
        /// <param name="imgPath"> 路径 </param>
        /// <returns></returns>
        public String ReadBarCode(String imgPath)
        {
            try
            {
                Bitmap bitmap = new Bitmap(imgPath);
                BitmapLuminanceSource bitmapLuminanceSource = new BitmapLuminanceSource(bitmap);
                BinaryBitmap image = new BinaryBitmap(new ZXing.Common.HybridBinarizer(bitmapLuminanceSource));
                ZXing.QrCode.QRCodeReader qRCodeReader = new ZXing.QrCode.QRCodeReader();
                Result res = qRCodeReader.decode(image);
                return res.Text;
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 读取条码内容
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public String ReadBarCode(Bitmap bitmap)
        {
            try
            {
                ZXing.BitmapLuminanceSource bitmapLuminanceSource = new BitmapLuminanceSource(bitmap);
                BinaryBitmap image = new BinaryBitmap(new ZXing.Common.HybridBinarizer(bitmapLuminanceSource));
                ZXing.QrCode.QRCodeReader qRCodeReader = new ZXing.QrCode.QRCodeReader();
                Result res = qRCodeReader.decode(image);
                return res.Text;
            }
            catch
            {
                return string.Empty;
            }
        }
        
    }
}
