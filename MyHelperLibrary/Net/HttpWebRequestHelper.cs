﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
namespace MyHelper
{
    /// <summary>
    /// HttpWebRequest Helper
    /// </summary>
    public class HttpWebRequestHelper : NetBaseHelper
    {
        private static readonly string contentType = "application/x-www-form-urlencoded;multipart/form-data";
        private static readonly string encodeType = "UTF-8";
        private static readonly int timeout = 5000;
        private static CookieContainer mCookieContainer = new CookieContainer();

        /// <summary>
        /// Post Http请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="postData">传输数据</param>

        /// <returns>泛型集合</returns>
        public static ResponseContent Post(string url, string postData,bool withCookes = true)
        {
            ResponseContent responseContent = new ResponseContent();
            if (string.IsNullOrEmpty(url))
            {
                responseContent.Code = -1;
                responseContent.Msg = "请求的地址不能为空";
                responseContent.Data = "";
                return responseContent;
            }
            HttpWebResponse webResponse = null;
            Stream responseStream = null;
            Stream requestStream = null;
            StreamReader streamReader = null;
            try
            {
                string respstr = PostStreamReader(url, responseStream, requestStream, streamReader, webResponse, postData, contentType);
                ConsoleHelper.writeLine(respstr);
                responseContent = (ResponseContent)JsonHelper.JsonToObject(respstr, typeof(ResponseContent));                
            }
            catch (Exception e)
            {
                ConsoleHelper.writeLine(" 请求失败: " + e.Message);
                responseContent.Code = -1;
                responseContent.Msg = "请求失败：客户端口或者服务有误。";
                responseContent.Data = "";
                throw e;
            }
            finally
            {
                if (responseStream != null) responseStream.Dispose();
                if (webResponse != null) webResponse.Dispose();
                if (requestStream != null) requestStream.Dispose();
                if (streamReader != null) streamReader.Dispose();
            }
            return responseContent;
        }

        /// <summary>
        /// Post Http请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="postData">传输数据</param>

        /// <returns>泛型集合</returns>
        public static List<T> Post<T>(string url, string postData)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new Exception("请求的地址不能为空！");
            }
            HttpWebResponse webResponse = null;
            Stream responseStream = null;
            Stream requestStream = null;
            StreamReader streamReader = null;
            try
            {
                string respstr = PostStreamReader(url, responseStream, requestStream, streamReader, webResponse, postData, contentType);
                return (List<T>)JsonHelper.JsonToObject(respstr, typeof(List<T>));
            }
            catch
            {
                throw;
            }
            finally
            {
                if (responseStream != null) responseStream.Dispose();
                if (webResponse != null) webResponse.Dispose();
                if (requestStream != null) requestStream.Dispose();
                if (streamReader != null) streamReader.Dispose();
            }
        }

        /// <summary>
        /// 真正的请求开始
        /// </summary>
        /// <param name="url"></param>
        /// <param name="responseStream"></param>
        /// <param name="requestStream"></param>
        /// <param name="streamReader"></param>
        /// <param name="webResponse"></param>
        /// <param name="postData"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static string PostStreamReader(string url, Stream responseStream, Stream requestStream, StreamReader streamReader, HttpWebResponse webResponse, string postData, string contentType, bool withCookes = true)
        {
            byte[] data = Encoding.GetEncoding("UTF-8").GetBytes(postData);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "post";
            webRequest.ContentType = contentType + ";" + encodeType;
            webRequest.ContentLength = data.Length;
            webRequest.Timeout = timeout;
            if (withCookes) {
                webRequest.CookieContainer = mCookieContainer;
            }
            requestStream = webRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            webResponse = (HttpWebResponse)webRequest.GetResponse();
            responseStream = webResponse.GetResponseStream();
            if (responseStream == null)
            {
                return string.Empty;
            }
            streamReader = new StreamReader(responseStream, Encoding.UTF8);
            if (webRequest.CookieContainer != null) {
                mCookieContainer = webRequest.CookieContainer; 
            }
            return streamReader.ReadToEnd();
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="filePath">路径</param>
        /// <param name="postData">参数</param>
        /// <param name="contentType">媒体类型</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static ResponseContent postFile(string url, string filePath, string postData = null, string contentType = "application/octet-stream", string encode = "UTF-8")
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(filePath))
            {
                throw new Exception("request url or  filePath is null !");
            }
            if (!FileHelper.Exists(filePath))
            {
                throw new Exception("file not exists 文件不存在!");
            }
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader streamReader = null;
            FileStream fileStream = null;
            ResponseContent response = new ResponseContent();
            try
            {
                // 设置参数
                HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
                //SetAuth(webRequest);
                webRequest.AllowAutoRedirect = true;
                webRequest.Method = "POST";
                string boundary = DateTime.Now.Ticks.ToString("X");// 随机分隔线
                webRequest.ContentType = "multipart/form-data;charset=" + encode + ";boundary=" + boundary;

                byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");//消息开始
                byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");//消息结尾
                var fileName = filePath.Substring(filePath.LastIndexOf("/") + 1);
                //请求头部信息
                string postHeader = string.Format("Content-Disposition:form-data;name=\"media\";filename=\"{0}\"\r\nContent-Type:{1}\r\n\r\n", fileName, contentType);
                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);
                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] fileByteArr = new byte[fileStream.Length];
                fileStream.Read(fileByteArr, 0, fileByteArr.Length);
                fileStream.Close();
                requestStream = webRequest.GetRequestStream();
                requestStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                requestStream.Write(fileByteArr, 0, fileByteArr.Length);

                if (!string.IsNullOrEmpty(postData))
                {
                    byte[] data = Encoding.UTF8.GetBytes(postData);
                    requestStream.Write(data, 0, data.Length);
                }
                requestStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                requestStream.Close();
                //发送请求，得到响应流
                responseStream = webRequest.GetResponse().GetResponseStream();
                if (responseStream == null)
                {
                    response.Data = string.Empty;
                    response.Msg = "no respose 没有返回！";
                    response.Code = 0;
                }
                streamReader = new StreamReader(responseStream, Encoding.UTF8);
                response.Data = streamReader.ReadToEnd();
                response.Code = 1;
                return response;
            }
            catch (Exception e)
            {
                ConsoleHelper.writeLine("请求错误：" + e.Message);
                throw;
            }
            finally
            {
                if (requestStream != null) requestStream.Close();
                if (responseStream != null) responseStream.Close();
                if (streamReader != null) streamReader.Close();
                if (fileStream != null) fileStream.Close();
            }
        }


        /// <summary>
        ///  Get http请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns>响应流字符串</returns>
        public static ResponseContent Get(string url)
        {
            ResponseContent responseContent = new ResponseContent();
            if (!string.IsNullOrEmpty(url))
            {
                Stream responseStream = null;
                StreamReader streamReader = null;
                HttpWebResponse webResponse = null;
                try
                {
                    String res = GetRespStr(url, responseStream, streamReader, webResponse);
                    responseContent = (ResponseContent)JsonHelper.JsonToObject(res, typeof(ResponseContent));
                }
                catch (Exception ex)
                {
                    ConsoleHelper.writeLine(ex.Message + "location: HttpWebRequestHelper Get");
                    responseContent.Code = -1;
                    responseContent.Msg = "请求失败：客户端口或者服务有误。";
                    responseContent.Data = "";
                }
                finally
                {
                    if (responseStream != null) responseStream.Dispose();
                    if (streamReader != null) streamReader.Dispose();
                    if (webResponse != null) webResponse.Dispose();
                }
            }
            else
            {
                responseContent.Code = -1;
                responseContent.Msg = "请求地址错误！";
                responseContent.Data = "";
            }
            return responseContent;
        }

        private static string GetRespStr(string url, Stream responseStream, StreamReader streamReader, HttpWebResponse webResponse)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            webRequest.Timeout = timeout;       
            webRequest.CookieContainer = mCookieContainer;
            webResponse = (HttpWebResponse)webRequest.GetResponse();            
            responseStream = webResponse.GetResponseStream();           
            if (responseStream == null) { return ""; }
            streamReader = new StreamReader(responseStream, Encoding.UTF8);      
            return streamReader.ReadToEnd();
        }


        public static string BaiduRecognition(string url, string token, string filePath)
        {
            string requestUrl = url + "?access_token = " + token;
            Encoding encoding = Encoding.GetEncoding("UTF-8");
            string base64 = FileHelper.GetFileBase64(filePath);
            String str = "image=" + System.Web.HttpUtility.UrlEncode(base64);
            byte[] buffer = encoding.GetBytes(str);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Method = "post";
            request.KeepAlive = true;
            request.ContentType = NetBaseHelper.ContentTypeFormUrlEncoded;
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            return result;
        }

        public static string GetBaiduAcesessToken(string tokenUrl, string clientId, string clientSecret)
        {
            string token = string.Empty;
            List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", clientId));
            paraList.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
            HttpResponseMessage response = new HttpClient().PostAsync(tokenUrl, new FormUrlEncodedContent(paraList)).Result;
            token = response.Content.ReadAsStringAsync().Result;
            return token;
        }
    }
}