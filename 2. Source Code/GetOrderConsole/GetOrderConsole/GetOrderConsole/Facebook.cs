using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using xNet;

namespace GetOrderConsole
{
    public class Facebook
    {
        public void Init()
        {
            GetData();
        }

        private string GetAccessToken()
        {
            string username = "nucasspronewrap@gmail.com";
            string password = "Nucass2189401222";

            string address = "https://nghia.org/public/api/v1/buildLogin.php";
            string data = "u=" + username + "&p=" + password;
            WebClient client = new WebClient
            {
                Headers = { [HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded" }
            };
            string str4 = client.UploadString(address, data);
            string accessToken = Convert.ToString(JObject.Parse(new WebClient().DownloadString(str4))["access_token"]);
            return accessToken;
        }

        private string GetPageAccessToken()
        {
            string accessToken = GetAccessToken();
            string newaddress = "https://graph.facebook.com/v3.0/722487931126157?fields=access_token&access_token=" + accessToken;

            HttpRequest httpRequest = new HttpRequest();
            var json = JsonConvert.DeserializeObject(httpRequest.Get(newaddress).ToString());
            JToken jToken = JToken.FromObject(json);
            string pageAccessToken = jToken["access_token"].ToString();

            return pageAccessToken;
        }

        private void GetData()
        {
            string pageAccessToken = GetPageAccessToken();
            string pageID = "1790501110988348";

            string conversations =
                @"https://graph.facebook.com/v3.0/" + pageID + "?fields=conversations%7Bmessages%7Bmessage%2Cfrom%2Ccreated_time%7D%7D&access_token=" +
                pageAccessToken;
            HttpRequest httpRequest = new HttpRequest();
            var getList = httpRequest.Get(conversations).ToString();
            JObject splitList = JObject.Parse(getList);
            var jToken = splitList["conversations"]["data"];

            foreach (var data in jToken)
            {
                foreach (var messages in data)
                {
                    foreach (var data1 in messages)
                    {
                        Console.WriteLine(data1["message"] + data1["id"].ToString());
                    }
                }
            }
        }

        #region Orders

        private void GetOrders()
        {
        }

        private void InsertOrdersToDb()
        {
        }

        #endregion Orders

        #region OrderDetail

        private void GetOrderDetail()
        {
        }

        private void InsertOrderDetailToDb()
        {
        }

        #endregion OrderDetail

        #region Others

        public DateTime ConvertToDateTime(string time)
        {
            return DateTime.Parse(time);
        }

        #endregion Others
    }
}