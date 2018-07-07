using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using xNet;

namespace GetOrderConsole
{
    public class Facebook
    {
        private const string GraphUrl = @"https://graph.facebook.com/v3.0/";
        private string _PageId = "";
        private string _username = "";
        private string _password = "";
        private HttpRequest _httpRequest;
        private string _pageAccessToken;
        private DateTime _time1, _time2, _time3;

        public Facebook(DateTime time1, DateTime time2, DateTime time3, string PageId, string UserName, string Password)
        {
            _time1 = time1;
            _time2 = time2;
            _time3 = time3;
            _PageId = PageId;
            _username = UserName;
            _password = Password;
            _httpRequest = new HttpRequest();
        }

        private string GetAccessToken()
        {
            const string username = "nucasspronewrap@gmail.com";
            const string password = "Nucasspro9696";

            const string address = "https://nghia.org/public/api/v1/buildLogin.php";
            const string data = "u=" + username + "&p=" + password;
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
            string newaddress = GraphUrl + _PageId + "?fields=access_token&access_token=" + accessToken;

            var json = JsonConvert.DeserializeObject(_httpRequest.Get(newaddress).ToString());
            JToken jToken = JToken.FromObject(json);
            string pageAccessToken = jToken["access_token"].ToString();

            return pageAccessToken;
        }

        //public void GetData(int time)
        //{
        //    _pageAccessToken = GetPageAccessToken();
        //    const string fields = @"?fields=conversations%7Bid%2Cupdated_time%7D&access_token=";
        //    string getListChatId = $"{GraphUrl}{PageId}{fields}{_pageAccessToken}";
        //    string getList = _httpRequest.Get(getListChatId).ToString();
        //    JObject splitList = JObject.Parse(getList);
        //    JToken jToken = splitList["conversations"]["data"];
        //    JToken jTokenPaging = splitList["conversations"]["paging"];

        //    if (jTokenPaging.ToString().Contains("\"next\": "))
        //    {
        //        var a = jTokenPaging["next"];
        //    }

        //    foreach (var data in jToken)
        //    {
        //        DateTime updatedTime = ConvertToDateTime((string)data["updated_time"]);
        //        //if (Check(time, updatedTime) == 1)
        //        //{
        //        //    continue;
        //        //}
        //        string id = (string)data["id"];
        //        JToken jToken2 = GetListMessageFromChatId(id)["messages"];
        //        GetOrders(GetMessage(jToken2));
        //    }
        //}

        public void GetData(int time)
        {
            _pageAccessToken = GetPageAccessToken();
            const string fields = @"?fields=conversations%7Bid%2Cupdated_time%7D&access_token=";
            string getListChatId = $"{GraphUrl}{_PageId}{fields}{_pageAccessToken}";
            string getList = _httpRequest.Get(getListChatId).ToString();
            JObject splitList = JObject.Parse(getList);
            JToken jToken = splitList["conversations"]["data"];

            foreach (var data in jToken)
            {
                DateTime updatedTime = ConvertToDateTime((string)data["updated_time"]);
                //if (Check(time, updatedTime) == 1)
                //{
                //    continue;
                //}
                string id = (string)data["id"];
                JToken jToken2 = GetListMessageFromChatId(id)["messages"];
                GetOrders(GetMessage(jToken2));
            }
            JToken jTokenPaging = splitList["conversations"]["paging"];

            if (jTokenPaging.ToString().Contains("\"next\": "))
            {
                string next = jTokenPaging["next"].ToString();
                while (GetJson(next) != null)
                {
                    next = GetJson(next);
                }
            }
        }

        public string GetJson(string url)
        {
            string getList = _httpRequest.Get(url).ToString();
            JObject splitList = JObject.Parse(getList);
            JToken jToken = splitList["data"];

            JToken jTokenPaging = splitList["paging"];
            foreach (var data in jToken)
            {
                DateTime updatedTime = ConvertToDateTime((string)data["updated_time"]);
                //if (Check(time, updatedTime) == 1)
                //{
                //    continue;
                //}
                string id = (string)data["id"];
                JToken jToken2 = GetListMessageFromChatId(id)["messages"];
                GetOrders(GetMessage(jToken2));
            }
            if (jTokenPaging.ToString().Contains("\"next\": "))
                return jTokenPaging["next"].ToString();
            return null;
        }

        public void GetOrders(List<string> list)
        {
            List<string> newList = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].Equals("Tôi muốn đặt hàng"))
                    continue;
                for (int j = i; j < list.Count; j++)
                {
                    newList.Add(list[j]);
                    if (!list[j].Equals("Cảm ơn bạn đã đặt hàng tại Shop. Đơn hàng của bạn đã được gửi tới cho nhân viên. Việc xác nhận đơn hàng là trong 24h, sẽ có nhân viên liên hệ để xác nhận đơn hàng. Xin cảm ơn!"))
                        continue;
                    newList.Add(list[j]);
                    break;
                }
            }

            if (newList.Count == 0)
                return;

            Orders orders = new Orders();
            Orders tempOrders = new Orders();
            Customers customers = new Customers();
            Customers tempCustomers = new Customers();
            OrderDetail orderDetail = new OrderDetail();
            OrderDetail tempOrderDetail = new OrderDetail();

            for (int i = 0; i < newList.Count; i++)
            {
                if (newList[i].Contains("Chọn thớt TKCL"))
                {
                    orderDetail.ProductId = newList[i].Replace(@"Chọn thớt ", "");
                }
                if (newList[i].Equals("Bạn muốn mua mẫu này với số lượng là bao nhiêu?"))
                {
                    orderDetail.Quantity = int.Parse(newList[i + 1]);
                }
                if (newList[i].Equals("Cho mình xin họ và tên, cả họ và tên nhé:"))
                {
                    customers.Name = newList[i + 1];
                }
                if (newList[i].Equals("Cho mình xin số điện thoại để mình bên mình sẽ liên lạc xác nhận đơn hàng:"))
                {
                    customers.Phone = newList[i + 1];
                }
                if (newList[i].Equals("Bạn muốn nhận hàng tại đâu? Phiền bạn ghi chi tiết địa chỉ hộ Shop"))
                {
                    customers.Address = newList[i + 1];
                    orders.BillingAddress = newList[i + 1];
                    orders.ShippingAddress = newList[i + 1];
                }
            }
            customers.Type = "Khách hàng";
            tempCustomers.InsertCustomersToDb(customers);

            orders.OrderCode = GenerateOrderCode();
            //chua fix time
            orders.CreatedTime = ConvertToTimeSpan(_time1.ToString());
            orders.UpdatedTime = ConvertToTimeSpan(_time1.ToString());
            orders.SubTotal = (350000 * orderDetail.Quantity).ToString();
            orders.GrandPrice = (350000 * orderDetail.Quantity).ToString();
            orders.CustomerId = tempCustomers.CheckCustomerExists(customers.Phone);
            orders.Status = "Chưa duyệt";
            orders.VerifyBy = 1;
            orders.OrderFrom = "Facebook";
            orders.Type = "Bán cho khách";
            orders.CallShip = "Chưa gọi ship";
            orders.ShipId = "";
            orders.ShipPrice = "0";
            orders.PackageWidth = "0";
            orders.PackageHeight = "0";
            orders.PackageLenght = "0";
            tempOrders.InsertOrdersToDb(orders);

            orderDetail.OrderId = tempOrders.GetOrderIdFromDb(orders.OrderCode);
            tempOrderDetail.InsertOrderDetailToDb(orderDetail);
        }

        public List<string> GetMessage(JToken jToken)
        {
            jToken = jToken["data"];
            List<string> list = jToken.Select(item => item["message"].ToString()).ToList();
            list.Reverse();
            return list;
        }

        public string GenerateOrderCode()
        {
            return "fb" + DateTime.Now.ToFileTimeUtc();
        }

        public DateTime ConvertToDateTime(string time)
        {
            return DateTime.Parse(time);
        }

        public JObject GetListMessageFromChatId(string chatId)
        {
            string getListChatId = $"{GraphUrl}{chatId}?fields=messages.limit(25)%7Bcreated_time%2Cmessage%7D&access_token={_pageAccessToken}";
            var getList = _httpRequest.Get(getListChatId).ToString();
            JObject splitList = JObject.Parse(getList);
            return splitList;
        }

        private int Check(int time, DateTime createdTime)
        {
            switch (time)
            {
                case 1:
                    if (createdTime.ToShortDateString() == _time1.ToShortDateString() && _time1 <= createdTime && createdTime < _time2)
                        return 0;
                    break;

                case 2:
                    if (createdTime.ToShortDateString() == _time2.ToShortDateString() && _time2 <= createdTime && createdTime < _time3)
                        return 0;
                    break;

                case 3:
                    if (createdTime.ToShortDateString() == _time3.ToShortDateString() && _time3 <= createdTime)
                        return 0;
                    break;
            }

            return 1;
        }

        public string ConvertToTimeSpan(string time)
        {
            DateTime dateTime = DateTime.Parse(time).ToLocalTime();
            var dateTimeOffset = new DateTimeOffset(dateTime);
            return dateTimeOffset.ToUnixTimeSeconds().ToString();
        }
    }
}