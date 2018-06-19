using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using xNet;

namespace GetOrderConsole
{
    public class WooCommerce
    {
        private const string HostUrl = @"https://localhost/thotthuytinh";
        private const string ConsumerKey = "ck_6dccdb287a7ac41beacafc58c9680117ba2871dc";
        private const string ConsumerSecret = "cs_30fe451dad1d1394ce716e0a73e87d68573e9ba8";
        private static string ApiUrl = @"/wp-json/wc/v2/";
        private HttpRequest _httpRequest;
        private DbConnect _dbConnect;
        private DateTime _time1, _time2, _time3;

        public WooCommerce()
        {
        }

        public void Init(DateTime time1, DateTime time2, DateTime time3)
        {
            _dbConnect = new DbConnect();
            _httpRequest = new HttpRequest
            {
                Cookies = new CookieDictionary(),
                UserAgent =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36"
            };
            _time1 = time1;
            _time2 = time2;
            _time3 = time3;
        }

        public void GetData(int time)
        {
            string address = HostUrl + ApiUrl + "orders/";
            RequestParams parameters = new RequestParams
            {
                ["consumer_key"] = ConsumerKey,
                ["consumer_secret"] = ConsumerSecret
            };
            string html = _httpRequest.Get(address, parameters).ToString();
            var json = JsonConvert.DeserializeObject(html);
            JToken jToken = JToken.FromObject(json);

            GetCustomers(jToken);
            GetOrdersAndOrderDetail(jToken, time);
        }

        private void GetCustomers(JToken jToken)
        {
            try
            {
                foreach (var item in jToken)
                {
                    if (CheckCustomerExists((string)item["billing"]["phone"]) >= 1)
                    {
                        continue;
                    }
                    Customers customers = new Customers();
                    customers.Name = (string)item["billing"]["first_name"] + " " + (string)item["billing"]["last_name"];
                    customers.Phone = (string)item["billing"]["phone"];
                    customers.Address = (string)item["billing"]["address_1"] + " - " + (string)item["billing"]["city"];
                    customers.NumberOfPurchasedpe = 0;
                    customers.QuantityPurchased = 0;
                    customers.Type = "Khách hàng";
                    InsertCustomersToDb(customers);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetCustomers that bai" + e);
                throw;
            }
        }

        private int GetCustomerIdFromDb(string phone)
        {
            string query = $"select Id from Customers where Customers.Phone = '{phone}' limit 1;";
            return _dbConnect.GetIdAndCountId(query);
        }

        private void GetOrdersAndOrderDetail(JToken jToken, int time)
        {
            try
            {
                foreach (var item in jToken)
                {
                    if (GetOrderIdFromDb((string)item["order_key"]) != 0)
                    {
                        continue;
                    }

                    //DateTime createdTime = ConvertToTimeSpan((string)item["date_created"]);
                    //DateTime updatedTime = ConvertToTimeSpan((string)item["date_modified"]);
                    //if (Check(time, createdTime) == 1)
                    //{
                    //    continue;
                    //}


                    Orders orders = new Orders
                    {
                        OrderCode = (string)item["order_key"],
                        CreatedTime = ConvertToTimeSpan((string)item["date_created"]),
                        UpdatedTime = ConvertToTimeSpan((string)item["date_modified"]),
                        TotalPrice = item["total"].ToString().Replace(".00", ""),
                        CustomerId = GetCustomerIdFromDb((string)item["billing"]["phone"]),
                        IsVerify = "Chưa duyệt",
                        VerifyBy = 1,
                        OrderFrom = "WooCommerce",
                        Type = "Bán cho khách",
                        DeliverCity = (string)item["billing"]["city"],
                        DeliverDistrict = (string)item["billing"]["address_1"],
                        DeliverAddress = (string)item["billing"]["address_1"],
                        CallShip = "Chưa gọi ship",
                        PackageWidth = "0",
                        PackageHeight = "0",
                        PackageWeight = "0",
                    };
                    InsertOrdersToDb(orders);

                    int orderId = GetOrderIdFromDb((string)item["order_key"]);
                    //string productId = GetProductIdFromDb((string)item["sku"]);
                    foreach (var subItem in item["line_items"])
                    {
                        OrderDetail orderDetail = new OrderDetail
                        {
                            OrderId = orderId,
                            ProductId = (string)subItem["sku"],
                            Quantity = (int)subItem["quantity"]
                        };
                        InsertOrderDetailToDb(orderDetail);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetOrders that bai" + e);
                throw;
            }
        }

        private string GetProductIdFromDb(string productId)
        {
            string query = $"select Products.Id from Products where Products.Id = '{productId}' limit 1;";
            return _dbConnect.GetData(query);
        }

        private int GetOrderIdFromDb(string orderCode)
        {
            string query = $"select Orders.Id from Orders where Orders.OrderCode = '{orderCode}' limit 1;";
            return _dbConnect.GetIdAndCountId(query);
        }

        private int CheckCustomerExists(string phone)
        {
            string query = $"select count(id) from Customers where Customers.Phone = '{phone}';";
            return _dbConnect.GetIdAndCountId(query);
        }
        private void InsertCustomersToDb(Customers customer)
        {
            try
            {
                string query = "insert into Customers (Name, Phone, Address, NumberOfPurchased, QuantityPurchased, Type) " +
                               $"VALUES('{customer.Name}', '{customer.Phone}', '{customer.Address}', '{customer.NumberOfPurchasedpe}', '{customer.QuantityPurchased}', '{customer.Type}')";
                _dbConnect.ExecuteQuery(query);
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi khi insert customers vao db" + e);
            }
        }
        private void InsertOrdersToDb(Orders orders)
        {
            try
            {
                string query = "insert into Orders (OrderCode, CreatedTime, UpdatedTime, TotalPrice, CustomerId, IsVerify, VerifyBy, OrderFrom, Type, DeliverCity, DeliverDistrict, DeliverAddress, CallShip, PackageWidth, PackageHeight, PackageWeight) " +
                               $"VALUES('{orders.OrderCode}', '{orders.CreatedTime}', '{orders.UpdatedTime}', '{orders.TotalPrice}', '{orders.CustomerId}', '{orders.IsVerify}', '{orders.VerifyBy}', '{orders.OrderFrom}', '{orders.Type}', '{orders.DeliverCity}', '{orders.DeliverDistrict}', '{orders.DeliverAddress}', '{orders.CallShip}', '{orders.PackageWidth}', '{orders.PackageHeight}', '{orders.PackageWeight}');";
                _dbConnect.ExecuteQuery(query);
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi khi insert orders vao db" + e);
            }
        }

        private void InsertOrderDetailToDb(OrderDetail orderDetail)
        {
            try
            {
                string query = "INSERT INTO OrderDetail (OrderId, ProductId, Quantity) " +
                               $"VALUES('{orderDetail.OrderId}','{orderDetail.ProductId}', '{orderDetail.Quantity}');";
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
            return DateTime.Parse(time).ToLocalTime();
        }

        public string ConvertToTimeSpan(string time)
        {
            DateTime dateTime = DateTime.Parse(time).ToLocalTime();
            var dateTimeOffset = new DateTimeOffset(dateTime);
            return dateTimeOffset.ToUnixTimeSeconds().ToString();
        }
    }
}