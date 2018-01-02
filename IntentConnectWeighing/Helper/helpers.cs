using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IntentConnectWeighing
{
    class helpers
    {
        private static readonly string template = "IntentConnectWeighing;component/{0}";

        /// <summary>
        /// 获取图片资源的路径
        /// 如：Themes/Img/Background/bd.png
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string getImageSourceParth(string path)
        {
            return string.Format(template, path);
        }

        /// <summary>
        /// / 获取图片资源
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BitmapImage getImageSource(string path) {
            return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
        }
    }
}
