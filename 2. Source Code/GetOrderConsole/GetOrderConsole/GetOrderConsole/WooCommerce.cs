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
            //string html = _httpRequest.Get(address, parameters).ToString();
            //var json = JsonConvert.DeserializeObject(html);
            //JToken jToken = JToken.FromObject(json);

            string a = "[{\"id\": 18,\"parent_id\": 0,\"number\": \"18\",\"order_key\": \"wc_order_5b190c6dbbe1Don1\",\"created_via\": \"checkout\",\"version\": \"3.4.0\",\"status\": \"processing\",\"currency\": \"VND\",\"date_created\": \"2018-06-11T04:43:57\",\"date_created_gmt\": \"2018-06-07T10:43:57\",\"date_modified\": \"2018-06-07T10:44:02\",\"date_modified_gmt\": \"2018-06-07T10:44:02\",\"discount_total\": \"0.00\",\"discount_tax\": \"0.00\",\"shipping_total\": \"0.00\",\"shipping_tax\": \"0.00\",\"cart_tax\": \"0.00\",\"total\": \"2770000.00\",\"total_tax\": \"0.00\",\"prices_include_tax\": false,\"customer_id\": 1,\"customer_ip_address\": \"::1\",\"customer_user_agent\": \"mozilla/5.0 (windows nt 10.0; wow64) applewebkit/537.36 (khtml, like gecko) chrome/66.0.3359.181 safari/537.36\",\"customer_note\": \"\",\"billing\": {\"first_name\": \"Đặng Nhật\",\"last_name\": \"Long\",\"company\": \"\",\"address_1\": \"KTX Khu B - TP.HCM\",\"address_2\": \"\",\"city\": \"Hồ Chí Minh\",\"state\": \"\",\"postcode\": \"70000\",\"country\": \"VN\",\"email\": \"hailongdang.hailong@gmail.com\",\"phone\": \"0963209769\"},\"shipping\": {\"first_name\": \"Đặng Nhật\",\"last_name\": \"Long\",\"company\": \"\",\"address_1\": \"KTX Khu B - TP.HCM\",\"address_2\": \"\",\"city\": \"Hồ Chí Minh\",\"state\": \"\",\"postcode\": \"70000\",\"country\": \"VN\"},\"payment_method\": \"cod\",\"payment_method_title\": \"Cash on delivery\",\"transaction_id\": \"\",\"date_paid\": null,\"date_paid_gmt\": null,\"date_completed\": null,\"date_completed_gmt\": null,\"cart_hash\": \"6fc7061ff05281e6f4abdc95d4a4d9ad\",\"meta_data\": [],\"line_items\": [{    \"id\": 3,    \"name\": \"product 2\",    \"product_id\": 16,    \"variation_id\": 0,    \"quantity\": 3,    \"tax_class\": \"\",    \"subtotal\": \"1020000.00\",    \"subtotal_tax\": \"0.00\",    \"total\": \"1020000.00\",    \"total_tax\": \"0.00\",    \"taxes\": [],    \"meta_data\": [],    \"sku\": \"\",    \"price\": 340000},{    \"id\": 4,    \"name\": \"Thớt kinh cường lực mẫu TKCL001\",    \"product_id\": 12,    \"variation_id\": 0,    \"quantity\": 5,    \"tax_class\": \"\",    \"subtotal\": \"1750000.00\",    \"subtotal_tax\": \"0.00\",    \"total\": \"1750000.00\",    \"total_tax\": \"0.00\",    \"taxes\": [],    \"meta_data\": [],    \"sku\": \"\",    \"price\": 350000}],},{\"id\": 118,\"parent_id\": 0,\"number\": \"118\",\"order_key\": \"wc_order_5b190c6dbbe1Don2\",\"created_via\": \"checkout\",\"version\": \"3.4.0\",\"status\": \"processing\",\"currency\": \"VND\",\"date_created\": \"2018-06-11T10:43:57\",\"date_created_gmt\": \"2018-06-07T10:43:57\",\"date_modified\": \"2018-06-07T10:44:02\",\"date_modified_gmt\": \"2018-06-07T10:44:02\",\"discount_total\": \"0.00\",\"discount_tax\": \"0.00\",\"shipping_total\": \"0.00\",\"shipping_tax\": \"0.00\",\"cart_tax\": \"0.00\",\"total\": \"2770000.00\",\"total_tax\": \"0.00\",\"prices_include_tax\": false,\"customer_id\": 1,\"customer_ip_address\": \"::1\",\"customer_user_agent\": \"mozilla/5.0 (windows nt 10.0; wow64) applewebkit/537.36 (khtml, like gecko) chrome/66.0.3359.181 safari/537.36\",\"customer_note\": \"\",\"billing\": {\"first_name\": \"Đặng Nhật\",\"last_name\": \"Long\",\"company\": \"\",\"address_1\": \"KTX Khu B - TP.HCM\",\"address_2\": \"\",\"city\": \"Hồ Chí Minh\",\"state\": \"\",\"postcode\": \"70000\",\"country\": \"VN\",\"email\": \"hailongdang.hailong@gmail.com\",\"phone\": \"0963209769\"},\"shipping\": {\"first_name\": \"Đặng Nhật\",\"last_name\": \"Long\",\"company\": \"\",\"address_1\": \"KTX Khu B - TP.HCM\",\"address_2\": \"\",\"city\": \"Hồ Chí Minh\",\"state\": \"\",\"postcode\": \"70000\",\"country\": \"VN\"},\"payment_method\": \"cod\",\"payment_method_title\": \"Cash on delivery\",\"transaction_id\": \"\",\"date_paid\": null,\"date_paid_gmt\": null,\"date_completed\": null,\"date_completed_gmt\": null,\"cart_hash\": \"6fc7061ff05281e6f4abdc95d4a4d9ad\",\"meta_data\": [],\"line_items\": [{    \"id\": 3,    \"name\": \"product 2\",    \"product_id\": 16,    \"variation_id\": 0,    \"quantity\": 3,    \"tax_class\": \"\",    \"subtotal\": \"1020000.00\",    \"subtotal_tax\": \"0.00\",    \"total\": \"1020000.00\",    \"total_tax\": \"0.00\",    \"taxes\": [],    \"meta_data\": [],    \"sku\": \"\",    \"price\": 340000},{    \"id\": 4,    \"name\": \"Thớt kinh cường lực mẫu TKCL001\",    \"product_id\": 12,    \"variation_id\": 0,    \"quantity\": 5,    \"tax_class\": \"\",    \"subtotal\": \"1750000.00\",    \"subtotal_tax\": \"0.00\",    \"total\": \"1750000.00\",    \"total_tax\": \"0.00\",    \"taxes\": [],    \"meta_data\": [],    \"sku\": \"\",    \"price\": 350000}],},]";

            var json = JsonConvert.DeserializeObject(a);

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

                    DateTime createdTime = ConvertToDateTime((string)item["date_created"]);
                    if (Check(time, createdTime) == 1)
                    {
                        continue;
                    }
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
                               "VALUES(" +
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
            return DateTime.Parse(time).ToLocalTime();
        }
    }
}