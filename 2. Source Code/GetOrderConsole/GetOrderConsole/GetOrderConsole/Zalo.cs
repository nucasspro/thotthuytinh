using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using ZaloCSharpSDK;

namespace GetOrderConsole
{
    public class Zalo
    {
        private const long OaId = 1364144657533100885;
        private const string SecretKey = "hGq0eOPke9SvuMmGfiKx";
        private ZaloOaInfo _zaloOaInfo;
        private ZaloStoreClient _storeClient;
        private DbConnect _dbConnect;
        private DateTime _time1, _time2, _time3;

        public Zalo()
        {
        }

        public void Init(DateTime time1, DateTime time2, DateTime time3)
        {
            _zaloOaInfo = new ZaloOaInfo(OaId, SecretKey);
            _storeClient = new ZaloStoreClient(_zaloOaInfo);
            _dbConnect = new DbConnect();
            _time1 = time1;
            _time2 = time2;
            _time3 = time3;
        }

        public void GetData(int time)
        {
            var getList = JObject.FromObject(GetOrderList(_storeClient)).ToString();
            JObject splitList = JObject.Parse(getList);
            var jToken = splitList["data"]["orders"];

            GetCustomers(jToken);
            GetOrdersAndOrderDetail(jToken, time);
        }

        private void GetCustomers(JToken jToken)
        {
            foreach (var item in jToken)
            {
                if (CheckCustomerExists("0" + (string)item["customerPhone"]) >= 1)
                {
                    continue;
                }
                Customers customers = new Customers
                {
                    Name = (string)item["customerName"],
                    Phone = "0" + (string)item["customerPhone"],
                    Adress = (string)item["deliverAddress"] + " - " + (string)item["deliverDistrict"] + " - " + (string)item["deliverCity"],
                    NumberOfPurchasedpe = 0,
                    QuantityPurchased = 0,
                    Type = "Khách hàng"
                };
                InsertCustomersToDb(customers);
            }
        }

        private int CheckCustomerExists(string phone)
        {
            string query = $"select count(id) from Customers where Customers.Phone = '{phone}';";
            return _dbConnect.ExecuteQueryToGetIdAndCount(query);
        }

        private int GetCustomerIdFromDb(string phone)
        {
            string query = $"select Id from Customers where Customers.Phone = '{phone}' limit 1;";
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
        private void GetOrdersAndOrderDetail(JToken jToken, int time)
        {
            foreach (var item in jToken)
            {
                if (GetOrderIdFromDb((string)item["orderCode"]) != 0)
                {
                    continue;
                }

                string unixCreatedTime = ((string)item["createdTime"]).Remove(10, 3);
                DateTime createdTime = UnixTimestampToDateTime(Convert.ToDouble(unixCreatedTime));
                //if (Check(time, createdTime) == 1)
                //{
                //    continue;
                //}
                string unixUpdatedTime = ((string)item["updatedTime"]).Remove(10, 3);
                DateTime updatedTime = UnixTimestampToDateTime(Convert.ToDouble(unixUpdatedTime));

                Orders orders = new Orders
                {
                    OrderCode = (string)item["orderCode"],
                    CreatedTime = createdTime,
                    UpdatedTime = updatedTime,
                    ShipId = 0,
                    TotalPrice =
                        ((float)item["price"] * (float)item["numItem"]).ToString(CultureInfo.InvariantCulture),
                    CustomerId = GetCustomerIdFromDb("0" + (string)item["customerPhone"]),
                    VerifyBy = 1,
                    OrderFrom = "Zalo",
                    Type = "Bán cho khách"
                };
                InsertOrdersToDb(orders);

                int orderId = GetOrderIdFromDb((string)item["orderCode"]);
                OrderDetail orderDetail = new OrderDetail
                {
                    OrderId = orderId,
                    Quantity = (int)item["numItem"],
                    DeliverCity = (string)item["deliverCity"],
                    DeliverDistrict = (string)item["deliverDistrict"],
                    DeliverAddress = (string)item["deliverAddress"],
                    ProductId = 0
                };
                InsertOrderDetailToDb(orderDetail);
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

        private static object GetOrderList(ZaloStoreClient storeClient)
        {
            JObject getOrderOfOa = storeClient.getOrderOfOa(0, 10, 0);
            return getOrderOfOa;
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

        private static DateTime UnixTimestampToDateTime(double unixTime)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds(unixTime).ToLocalTime();
            return dateTime;
        }
    }
}