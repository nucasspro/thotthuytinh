using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using xNet;

namespace GetOrderConsole
{
    public class GetFromWooCommerce
    {
        private const string HostUrl = @"https://localhost/thotthuytinh";
        private const string ConsumerKey = "ck_6dccdb287a7ac41beacafc58c9680117ba2871dc";
        private const string ConsumerSecret = "cs_30fe451dad1d1394ce716e0a73e87d68573e9ba8";

        private static string ApiUrl = @"/wp-json/wc/v2/";
        private static HttpRequest _httpRequest;

        private DbConnect _dbConnect;

        public GetFromWooCommerce()
        {
        }

        public void Init()
        {
            _dbConnect = new DbConnect();
            _httpRequest = new HttpRequest
            {
                Cookies = new CookieDictionary(),
                UserAgent =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36"
            };
        }

        public void GetData()
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
            GetOrdersAndOrderDetail(jToken);
        }

        public void GetCustomers(JToken jToken)
        {
            try
            {
                foreach (var item in jToken)
                {
                    if (CheckCustomerExists((string)item["billing"]["phone"]) >= 1)
                    {
                        continue;
                    }
                    Customers customers = new Customers
                    {
                        Name = (string)item["billing"]["first_name"] + " " + (string)item["billing"]["last_name"],
                        Phone = (string)item["billing"]["phone"],
                        Adress = (string)item["billing"]["address_1"] + " - " + (string)item["billing"]["city"],
                        NumberOfPurchasedpe = 0,
                        QuantityPurchased = 0,
                        Type = "Khách hàng"
                    };
                    InsertCustomersToDb(customers);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetCustomers that bai" + e);
                throw;
            }
        }

        private int CheckCustomerExists(string phone)
        {
            string query = $"select count(id) from Customers where Customers.Phone = '{phone}';";
            return _dbConnect.ExecuteQueryToGetIdAndCount(query);
        }

        private void InsertCustomersToDb(Customers customer)
        {
            try
            {
                string query = "insert into Customers (" +
                               "Name, " +
                               "Phone, " +
                               "Adress, " +
                               "NumberOfPurchased, " +
                               "QuantityPurchased, " +
                               "Type)" +
                               $"VALUES(" +
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

        public void GetOrdersAndOrderDetail(JToken jToken)
        {
            try
            {
                foreach (var item in jToken)
                {
                    if (GetOrderIdFromDb((string)item["order_key"]) != 0)
                    {
                        continue;
                    }

                    DateTime createdTime = ConvertToDateTime((string)item["date_created"]);
                    DateTime updatedTime = ConvertToDateTime((string)item["date_modified"]);

                    Orders orders = new Orders
                    {
                        OrderCode = (string)item["order_key"],
                        CreatedTime = createdTime,
                        UpdatedTime = updatedTime,
                        ShipId = 0,
                        TotalPrice = item["total"].ToString().Replace(".00", ""),
                        CustomerId = 0,
                        VerifyBy = 1,
                        OrderFrom = "WooCommerce",
                        Type = "Bán cho khách"
                    };

                    InsertOrdersToDb(orders);

                    int orderId = GetOrderIdFromDb((string)item["order_key"]);

                    foreach (var subItem in item["line_items"])
                    {
                        OrderDetail orderDetail = new OrderDetail
                        {
                            OrderId = orderId,
                            Quantity = (int)subItem["quantity"],
                            DeliverCity = (string)item["billing"]["city"],
                            DeliverDistrict = (string)item["billing"]["address_1"],
                            DeliverAddress = (string)item["billing"]["address_1"],
                            ProductId = 0
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
                               $"VALUES(" +
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

        private void InsertOrderDetailToDb(OrderDetail orderDetail)
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
                               $"VALUES(" +
                               $"'{orderDetail.OrderId}', " +
                               $"'{orderDetail.Quantity}', " +
                               $"'{orderDetail.DeliverCity}', " +
                               $"'{orderDetail.DeliverDistrict}', " +
                               $"'{orderDetail.DeliverAddress}', " +
                               $"'{orderDetail.ProductId}')";
                _dbConnect.ExecuteQuery(query);
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi khi insert orderdetail vao db" + e);
            }
        }

        public DateTime ConvertToDateTime(string time)
        {
            return DateTime.Parse(time);
        }
    }
}