using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using ZaloCSharpSDK;

namespace GetOrderConsole
{
    public class GetFromZalo
    {
        private const long OaId = 1364144657533100885;
        private const string SecretKey = "hGq0eOPke9SvuMmGfiKx";
        private ZaloOaInfo _zaloOaInfo;
        private ZaloStoreClient _storeClient;
        private DbConnect _dbConnect;

        public GetFromZalo()
        {
        }

        public void Init()
        {
            _zaloOaInfo = new ZaloOaInfo(OaId, SecretKey);
            _storeClient = new ZaloStoreClient(_zaloOaInfo);
            _dbConnect = new DbConnect();
        }

        public void GetData()
        {
            var getList = JObject.FromObject(GetOrderList(_storeClient)).ToString();
            JObject splitList = JObject.Parse(getList);
            var jToken = splitList["data"]["orders"];

            GetCustomers(jToken);
            GetOrdersAndOrderDetail(jToken);
        }

        public void GetCustomers(JToken jToken)
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

        private int CheckCustomerExists(string phone)
        {
            string query = $"select count(id) from Customers where Customers.Phone = '{phone}';";
            return _dbConnect.ExecuteQueryToGetIdAndCount(query);
        }

        private void GetOrdersAndOrderDetail(JToken jToken)
        {
            foreach (var item in jToken)
            {
                if (GetOrderIdFromDb((string)item["orderCode"]) != 0)
                {
                    continue;
                }
                Orders orders = new Orders();

                orders.OrderCode = (string)item["orderCode"];

                string unixCreatedTime = ((string)item["createdTime"]).Remove(9, 3);
                DateTime createdTime = UnixTimestampToDateTime(Convert.ToDouble(unixCreatedTime));
                orders.CreatedTime = createdTime;

                string unixUpdatedTime = ((string)item["updatedTime"]).Remove(9, 3);
                DateTime updatedTime = UnixTimestampToDateTime(Convert.ToDouble(unixUpdatedTime));
                orders.UpdatedTime = updatedTime;

                orders.ShipId = 0;
                orders.TotalPrice = ((float)item["price"] * (float)item["numItem"]).ToString();
                orders.CustomerId = 0;
                orders.VerifyBy = 1;
                orders.OrderFrom = "Zalo";
                orders.Type = "Bán cho khách";

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
                               $"VALUES(" +
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

        private static DateTime UnixTimestampToDateTime(double unixTime)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds(unixTime).ToLocalTime();
            return dateTime;
        }

        public List<Orders> SortOrderByDay(List<Orders> list)
        {
            List<Orders> sortList = new List<Orders>();
            foreach (var item in list)
            {
                if (item.CreatedTime == DateTime.Today)
                {
                    sortList.Add(item);
                }
            }
            return sortList;
        }
    }
}