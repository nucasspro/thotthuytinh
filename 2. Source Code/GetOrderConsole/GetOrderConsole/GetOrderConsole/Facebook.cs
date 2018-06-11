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
        private const string PageId = "1790501110988348";
        private const string GraphUrl = @"https://graph.facebook.com/v3.0/";
        private HttpRequest _httpRequest;
        private DateTime _time1, _time2, _time3;
        private string _pageAccessToken;
        private DbConnect _dbConnect;

        public Facebook()
        {
        }

        public void Init(DateTime time1, DateTime time2, DateTime time3)
        {
            _time1 = time1;
            _time2 = time2;
            _time3 = time3;
            _dbConnect = new DbConnect();
            _httpRequest = new HttpRequest();
        }

        public void GetData(int time)
        {
            _pageAccessToken = GetPageAccessToken();
            string fields = "?fields=conversations%7Bid%2Cupdated_time%7D&access_token=";
            string getListChatId = GraphUrl + PageId + fields + _pageAccessToken;
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
            string newaddress = GraphUrl + PageId + "?fields=access_token&access_token=" + accessToken;

            var json = JsonConvert.DeserializeObject(_httpRequest.Get(newaddress).ToString());
            JToken jToken = JToken.FromObject(json);
            string pageAccessToken = jToken["access_token"].ToString();

            return pageAccessToken;
        }

        public JObject GetListMessageFromChatId(string chatId)
        {
            string getListChatId = GraphUrl + chatId + "?fields=messages.limit(25)%7Bcreated_time%2Cmessage%7D&access_token=" +
                                   _pageAccessToken;
            var getList = _httpRequest.Get(getListChatId).ToString();
            JObject splitList = JObject.Parse(getList);
            return splitList;
        }

        public List<string> GetMessage(JToken jToken)
        {
            jToken = jToken["data"];
            List<string> list = jToken.Select(item => item["message"].ToString()).ToList();
            list.Reverse();
            return list;
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
                    if (!list[j].Equals("Cảm ơn bạn đã đặt hàng tại Shop. " +
                                        "Đơn hàng của bạn đã được gửi tới cho nhân viên. " +
                                        "Việc xác nhận đơn hàng là trong 24h, sẽ có nhân viên liên hệ để xác nhận đơn hàng. " +
                                        "Xin cảm ơn!"))
                        continue;
                    newList.Add(list[j]);
                    break;
                }
            }

            if (newList.Count == 0)
                return;

            Orders orders = new Orders();
            Customers customers = new Customers();
            OrderDetail orderDetail = new OrderDetail();

            for (int i = 0; i < newList.Count - 3; i++)
            {
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
                    customers.Adress = orderDetail.DeliverCity = orderDetail.DeliverDistrict = orderDetail.DeliverAddress = newList[i + 1];
                }
            }
            customers.NumberOfPurchasedpe = 0;
            customers.QuantityPurchased = 0;
            customers.Type = "Khách hàng";
            InsertCustomersToDb(customers);

            orders.OrderCode = GenerateOrderCode();
            orders.CreatedTime = _time1;
            orders.UpdatedTime = _time1;
            orders.ShipId = 0;
            orders.TotalPrice = (350000 * orderDetail.Quantity).ToString();
            orders.CustomerId = CheckCustomerExists(customers.Phone);
            orders.VerifyBy = 1;
            orders.OrderFrom = "Facebook";
            orders.Type = "Bán cho khách";
            InsertOrdersToDb(orders);

            orderDetail.OrderId = GetOrderIdFromDb(orders.OrderCode);
            orderDetail.ProductId = 0;
            InsertOrderDetailToDb(orderDetail);
        }

        private int CheckCustomerExists(string phone)
        {
            string query = $"select count(id) from Customers where Customers.Phone = '{phone}';";
            return _dbConnect.ExecuteQueryToGetIdAndCount(query);
        }

        private void InsertCustomersToDb(Customers customer)
        {
            if (CheckCustomerExists(customer.Phone) >= 1)
            {
                return;
            }
            try
            {
                string query = "insert into Customers (" +
                               "Name, " +
                               "Phone, " +
                               "Adress, " +
                               "NumberOfPurchased, " +
                               "QuantityPurchased, " +
                               "Type)" +
                               "VALUES(" +
                               $"'{customer.Name}', " +
                               $"'{customer.Phone}', " +
                               $"'{customer.Adress}', " +
                               $"'{customer.NumberOfPurchasedpe}', " +
                               $"'{customer.QuantityPurchased}', " +
                               $"'{customer.Type}')";
                _dbConnect.ExecuteQuery(query);
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi khi insert customers vao db" + e);
            }
        }

        private int GetOrderIdFromDb(string orderCode)
        {
            string query = $"select Orders.Id from Orders where Orders.OrderCode = '{orderCode}' limit 1;";
            return _dbConnect.ExecuteQueryToGetIdAndCount(query);
        }

        private void InsertOrdersToDb(Orders orders)
        {
            try
            {
                string query = "INSERT INTO Orders (" +
                               "OrderCode, " +
                               "CreatedTime, " +
                               "UpdatedTime, " +
                               "ShipId, " +
                               "TotalPrice, " +
                               "CustomerId, " +
                               "VerifyBy, " +
                               "OrderFrom, " +
                               "Type)" +
                               "VALUES(" +
                               $"'{orders.OrderCode}', " +
                               $"'{orders.CreatedTime}', " +
                               $"'{orders.UpdatedTime}', " +
                               $"'{orders.ShipId}', " +
                               $"'{orders.TotalPrice}', " +
                               $"'{orders.CustomerId}'," +
                               $"'{orders.VerifyBy}'," +
                               $"'{orders.OrderFrom}'," +
                               $"'{orders.Type}')";
                _dbConnect.ExecuteQuery(query);
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi khi insert orders vao db" + e);
            }
        }

        private void InsertOrderDetailToDb(OrderDetail order)
        {
            try
            {
                string query = "INSERT INTO OrderDetail (" +
                               "OrderId, " +
                               "Quantity, " +
                               "DeliverCity, " +
                               "DeliverDistrict, " +
                               "DeliverAddress, " +
                               "ProductId)" +
                               "VALUES(" +
                               $"'{order.OrderId}', " +
                               $"'{order.Quantity}', " +
                               $"'{order.DeliverCity}', " +
                               $"'{order.DeliverDistrict}', " +
                               $"'{order.DeliverAddress}', " +
                               $"'{order.ProductId}')";
                _dbConnect.ExecuteQuery(query);
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi khi insert orderdetail vao db" + e);
            }
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

        public DateTime ConvertToDateTime(string time)
        {
            return DateTime.Parse(time);
        }

        public string GenerateOrderCode()
        {
            return "fb" + DateTime.Now.ToFileTimeUtc();
        }
    }
}