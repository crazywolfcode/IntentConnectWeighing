using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
namespace IntentConnectWeighing
{
    class BitmapHelper
    {
        public static BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            if (bitmap == null) {
                return null;
            }
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            stream.Seek(0, SeekOrigin.Begin);
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }
    }
}
