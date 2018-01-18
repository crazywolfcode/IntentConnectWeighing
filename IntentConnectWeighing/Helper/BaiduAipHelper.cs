using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    class BaiduAipHelper
    {
        private static string baseUrl = "https://aip.baidubce.com/rest/2.0/ocr/v1/";
        public static string BaiduTokenUrl = "https://aip.baidubce.com/oauth/2.0/token";
        /// <summary>
        /// get baidu aip urls 
        /// https://aip.baidubce.com/rest/2.0/ocr/v1/ + url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string getRecognitonUrl(BaiduUrl url)
        {
            return baseUrl + url.ToString();
        }


        public static BaiduLicenseRecognition getLicenseRecognition(Newtonsoft.Json.Linq.JObject jObject)
        {
            if (jObject == null)
            {
                return null;
            }
            BaiduLicenseRecognition license = new BaiduLicenseRecognition();
            Newtonsoft.Json.Linq.JToken jtError = jObject.SelectToken("error_code");
            if (jtError != null)
            {
                license.errorCode = jtError.ToString();
                license.ErrorMsg = jObject.SelectToken("error_msg").ToString();
                return license;
            }
            Newtonsoft.Json.Linq.JToken wordsResult = jObject.SelectToken("words_result");
            if (wordsResult != null)
            {
                license.companyName = wordsResult.SelectToken("单位名称").SelectToken("words").ToString();
                license.expriseDate = wordsResult.SelectToken("有效期").SelectToken("words").ToString();
                license.licenseNumber = wordsResult.SelectToken("证件编号").SelectToken("words").ToString();
                license.creditNumber = wordsResult.SelectToken("社会信用代码").SelectToken("words").ToString();
                license.address = wordsResult.SelectToken("地址").SelectToken("words").ToString();
                license.logalPerson = wordsResult.SelectToken("法人").SelectToken("words").ToString();
            }
            return license;
        }

        public static baiduIDRecogniton getIDRecognition(Newtonsoft.Json.Linq.JObject jObject)
        {
            if (jObject == null)
            {
                return null;
            }
            baiduIDRecogniton id = new baiduIDRecogniton();
            Newtonsoft.Json.Linq.JToken jtError = jObject.SelectToken("error_code");
            if (jtError != null)
            {
                id.errorCode = jtError.ToString();
                id.ErrorMsg = jObject.SelectToken("error_msg").ToString();
                return id;
            }
            Newtonsoft.Json.Linq.JToken wordsResult = jObject.SelectToken("words_result");
            if (wordsResult != null)
            {
                id.name = wordsResult.SelectToken("姓名").SelectToken("words").ToString();
                id.address = wordsResult.SelectToken("住址").SelectToken("words").ToString();
                id.birthDate = wordsResult.SelectToken("出生").SelectToken("words").ToString();
                id.IdNumber = wordsResult.SelectToken("公民身份号码").SelectToken("words").ToString();
                id.sex = wordsResult.SelectToken("性别").SelectToken("words").ToString();
                id.nation = wordsResult.SelectToken("民族").SelectToken("words").ToString();
            }
            return id;
        }


        public static BaiduBandCardRecognition getBandCardRecognition(Newtonsoft.Json.Linq.JObject jObject)
        {
            if (jObject == null)
            {
                return null;
            }
            BaiduBandCardRecognition card = new BaiduBandCardRecognition();
            string result = MyHelper.StringHelper.jsonCamelCaseToDBnameing(jObject.SelectToken("result").ToString());
            if (string.IsNullOrEmpty(result))
            {
                card = MyHelper.JsonHelper.JsonToObject(MyHelper.StringHelper.jsonCamelCaseToDBnameing(jObject.ToString()), typeof(BaiduBandCardRecognition)) as BaiduBandCardRecognition;
            }
            else
            {
                card = MyHelper.JsonHelper.JsonToObject(result, typeof(BaiduBandCardRecognition)) as BaiduBandCardRecognition;
            }
            return card;
        }



        public static BaiduDriverLicenseRecognition getDriverRecognition(Newtonsoft.Json.Linq.JObject jObject)
        {
            if (jObject == null)
            {
                return null;
            }
            BaiduDriverLicenseRecognition driver = new BaiduDriverLicenseRecognition();
            Newtonsoft.Json.Linq.JToken jtError = jObject.SelectToken("error_code");
            if (jtError != null)
            {
                driver.errorCode = jtError.ToString();
                driver.ErrorMsg = jObject.SelectToken("error_msg").ToString();
                return driver;
            }
            MyHelper.ConsoleHelper.writeLine(jObject.ToString());
            Newtonsoft.Json.Linq.JToken wordsResult = jObject.SelectToken("words_result");
            if (wordsResult != null)
            {
                driver.name = wordsResult.SelectToken("姓名").SelectToken("words").ToString();
                driver.number = wordsResult.SelectToken("证号").SelectToken("words").ToString();  
                try
                {
                    driver.endTime = wordsResult.SelectToken("至").SelectToken("words").ToString();
                    driver.startTime = wordsResult.SelectToken("有效期限").SelectToken("words").ToString();
                }
                catch (Exception)
                {
                    driver.startTime = wordsResult.SelectToken("有效起始日期").SelectToken("words").ToString();
                    driver.expriseDate = wordsResult.SelectToken("有效期限").SelectToken("words").ToString();
                }
                driver.sex = wordsResult.SelectToken("性别").SelectToken("words").ToString();
                driver.birth = wordsResult.SelectToken("出生日期").SelectToken("words").ToString();
                driver.country = wordsResult.SelectToken("国籍").SelectToken("words").ToString();
                driver.type = wordsResult.SelectToken("准驾车型").SelectToken("words").ToString();
                driver.address = wordsResult.SelectToken("住址").SelectToken("words").ToString();
                driver.firstDate = wordsResult.SelectToken("初次领证日期").SelectToken("words").ToString();
            }
            return driver;
        }
        public static baiduVehicleRecognition getvehicleRecognition(Newtonsoft.Json.Linq.JObject jObject)
        {
            if (jObject == null)
            {
                return null;
            }
            baiduVehicleRecognition vehicle = new baiduVehicleRecognition();
            Newtonsoft.Json.Linq.JToken jtError = jObject.SelectToken("error_code");
            if (jtError != null)
            {
                vehicle.errorCode = jtError.ToString();
                vehicle.ErrorMsg = jObject.SelectToken("error_msg").ToString();
                return vehicle;
            }
            MyHelper.ConsoleHelper.writeLine(jObject.ToString());
            Newtonsoft.Json.Linq.JToken wordsResult = jObject.SelectToken("words_result");
            if (wordsResult != null)
            {
               vehicle.brand = wordsResult.SelectToken("品牌型号").SelectToken("words").ToString();
               vehicle.SendTime = wordsResult.SelectToken("发证日期").SelectToken("words").ToString(); 
               vehicle.useType = wordsResult.SelectToken("使用性质").SelectToken("words").ToString();
               vehicle.engineNumber = wordsResult.SelectToken("发动机号码").SelectToken("words").ToString();
               vehicle.carNumber = wordsResult.SelectToken("号牌号码").SelectToken("words").ToString();
               vehicle.owner = wordsResult.SelectToken("所有人").SelectToken("words").ToString();
               vehicle.address = wordsResult.SelectToken("住址").SelectToken("words").ToString();
               vehicle.registerDate = wordsResult.SelectToken("注册日期").SelectToken("words").ToString();
               vehicle.carRecongnitionCode = wordsResult.SelectToken("车辆识别代号").SelectToken("words").ToString();
               vehicle.carType = wordsResult.SelectToken("车辆类型").SelectToken("words").ToString();
            }
            return vehicle;
        }

        
    }
    public enum BaiduUrl
    {
        general_enhanced,
        accurate_basic,
        webimage,
        idcard,
        bankcard,
        driving_license,
        vehicle_license,
        business_license,
        license_plate,

    }
}
