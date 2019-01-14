using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper
{
    public class WebClientHelper
    {

        public static ResponseContent Get(String url, NameValueCollection queryString)
        {
            WebClient client = new WebClient();
            //var qs = new NameValueCollection
            //{
            //    { "table", table }, { "time", lastSyncTime} , { "stationid", stationid}
            //};
            client.QueryString = queryString;
            //client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //client.ResponseHeaders.Add("*/*");
            String res = client.DownloadString(url);
            ResponseContent response = (ResponseContent)MyHelper.JsonHelper.JsonToObject(res, typeof(ResponseContent));
            return response;
        }

        public static ResponseContent Post(String url, Object data, NameValueCollection queryString)
        {

            String josn = JsonHelper.ObjectToJson(data);
            WebClient client = new WebClient();
            //var qs = new NameValueCollection
            //{
            //    { "table", table }
            //};           
            client.QueryString = queryString;
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            String res = client.UploadString(url, "Post", josn);
            ResponseContent result = (ResponseContent)JsonHelper.JsonToObject(res, typeof(ResponseContent));
            return result;
        }
    }
}
