using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Media;

namespace MyHelper
{
    /// <summary>
    /// HttpClinent Heper
    /// </summary>
    public class HttpClientHelper : NetBaseHelper
    {
        private static readonly HttpClient mHttpClient;
        private static readonly HttpClient fileHttpClient;
        public static readonly string baseAddress = "http://localhost/wheighting/public/index.php";
        private static readonly string headAddress = "/api";
        private static readonly int MaxResponseContentBufferSize = 25600;
        static HttpClientHelper()
        {

            #region 初始化和预热 httpClient
            mHttpClient = new HttpClient();
            mHttpClient.BaseAddress = new Uri(baseAddress);
            mHttpClient.Timeout = TimeSpan.FromMilliseconds(2000);
            //想Accept的数据格式
            mHttpClient.DefaultRequestHeaders.Add("Accept", "application/json"); //"application/xml"
            HttpRequestMessage hrm = new HttpRequestMessage();
            hrm.Method = new HttpMethod("HEAD");
            hrm.RequestUri = new Uri(baseAddress + headAddress);
            mHttpClient.SendAsync(hrm).Result.EnsureSuccessStatusCode();
            #endregion

            #region 初始化和预热 fileClient
            fileHttpClient = new HttpClient();
            fileHttpClient.BaseAddress = new Uri(baseAddress);
            fileHttpClient.MaxResponseContentBufferSize = MaxResponseContentBufferSize;
            #endregion
        }
        /// <summary>
        /// http get 请求
        /// </summary>
        /// <param name="url">url 地址</param>
        /// <param name="isKeepAlive">是否保持长连接</param>
        /// <returns>ResponseContent</returns>
        public static async Task<ResponseContent> GetAsync(string url, Boolean isKeepAlive = false)
        {
            try
            {
                if (isKeepAlive == true)
                {
                    if (!mHttpClient.DefaultRequestHeaders.Connection.Contains(keepAlive))
                        mHttpClient.DefaultRequestHeaders.Connection.Add(keepAlive);
                }
                else
                {
                    mHttpClient.DefaultRequestHeaders.Connection.Remove(keepAlive);
                }
                string obj = await mHttpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
                ConsoleHelper.writeLine(obj);
                return (ResponseContent)JsonHelper.JsonToObject(obj, typeof(ResponseContent));
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// http post request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="parametes">参数</param>
        /// <param name="isKeepAlive">是否保持长连接</param>
        /// <returns></returns>
        public static async Task<ResponseContent> PostAsync(string url, List<KeyValuePair<string, string>> parametes, Boolean isKeepAlive = false)
        {
            //_httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
            //  byte[] data = Encoding.UTF8.GetBytes(request.Data);
            try
            {
                FormUrlEncodedContent fc = new FormUrlEncodedContent(parametes);
                string json = await mHttpClient.PostAsync(url, fc).Result.Content.ReadAsStringAsync();
                return (ResponseContent)JsonHelper.JsonToObject(json, typeof(ResponseContent));
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<ResponseContent> PostFile(string url, string filePath, string postData = null)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(filePath))
            {
                throw new Exception("request url or  filePath is null !");
            }
            if (!FileHelper.Exists(filePath))
            {
                throw new Exception("file not exists 文件不存在!");
            }

            // 读文件流
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception e)
            {
                ConsoleHelper.writeLine(e.Message);
                throw;
            }
            finally
            {
                fileStream.Close();
            }
            //设置请求头
            // fileHttpClient.DefaultRequestHeaders.Add("user-agent", "User-Agent    Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; Touch; MALNJS; rv:11.0) like Gecko");
            //为文件流提供的HTTP容器
            HttpContent fileContent = new StreamContent(fileStream);
            //设置媒体类型
            fileContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("multipart/form-data");
            //创建用于可传递文件的容器
            MultipartFormDataContent dataContent = new MultipartFormDataContent();
            string fileName = filePath.Substring(filePath.LastIndexOf("/") + 1);
            dataContent.Add(fileContent, "form", fileName);
            HttpResponseMessage resMessage = await fileHttpClient.PostAsync(url, dataContent);
            resMessage.EnsureSuccessStatusCode();
            string result = await resMessage.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            return JsonHelper.JsonToObject(result, typeof(ResponseContent)) as ResponseContent;
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<byte[]> HttpDownloadData<T>(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new Exception(" url is null");
            var byteres = await fileHttpClient.GetByteArrayAsync(url);
            return byteres;
        }

    }
}
