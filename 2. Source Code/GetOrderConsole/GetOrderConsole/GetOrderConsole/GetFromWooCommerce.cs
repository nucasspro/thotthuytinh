using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using xNet;

namespace ConsoleGetOrder
{
    public class GetFromWooCommerce
    {
        private static string HOST_URL = @"https://localhost/thotthuytinh";
        private static string CONSUMER_KEY = "ck_6dccdb287a7ac41beacafc58c9680117ba2871dc";
        private static string CONSUMER_SECRET = "cs_30fe451dad1d1394ce716e0a73e87d68573e9ba8";

        private static string API_URL = @"/wp-json/wc/v2/";
        private HttpRequest httpRequest;

        public GetFromWooCommerce()
        {
        }

        public void getproduct()
        {
            httpRequest = new HttpRequest();
            httpRequest.Cookies = new CookieDictionary();
            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
            RequestParams para = new RequestParams
            {
                ["consumer_key"] = CONSUMER_KEY,
                ["consumer_secret"] = CONSUMER_SECRET
            };

            string address = HOST_URL + API_URL + "orders/";

            string html = httpRequest.Get(address, para).ToString();
            var json = JsonConvert.DeserializeObject(html);
            JToken jToken = JToken.FromObject(json);
            foreach (var VARIABLE in jToken)
            {
                Console.WriteLine(VARIABLE["id"].ToString());
                Console.WriteLine(VARIABLE["billing"]["first_name"].ToString());
            }
        }
    }
}