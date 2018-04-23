using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
namespace IntentConnectWeighing
{
    class Helpers
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
           
        public static Company getRecognitionCompany(string RecognitionJson) {
            if (string.IsNullOrEmpty(RecognitionJson))
            {
                return null;
            }
            Object obj = MyHelper.JsonHelper.JsonToObject(RecognitionJson, typeof(Object));
            
            Company company = new Company();
            return company;
        }

        public class BaiduLocation{
            public string left { get; set; }
            public string right { get; set; }
            public string width { get; set; }
            public string height { get; set; }
        }

        public class BaiduLicenseResponse {
            public string companyName { get; set; }
            public string legalPreson { get; set; }
            public string address { get; set; }
            public string expityDate { get; set; }

        }
    }
}
