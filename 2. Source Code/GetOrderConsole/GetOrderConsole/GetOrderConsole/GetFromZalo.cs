using GetOrderConsole;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using ZaloCSharpSDK;

namespace ConsoleGetOrder
{
    public class GetFromZalo
    {
        private static readonly long _oaId = 1364144657533100885;
        private static readonly string _secretKey = "hGq0eOPke9SvuMmGfiKx";
        private static readonly string _secretKeyApp = "17GTbC5Taph5FCo1XGTS";
        private static readonly string _callbackUrl = @"https://tatatatatata.herokuapp.com/";
        private static readonly long _appId = 2491981268401425748;
        private static readonly string _accessToken = "UUVXaTppuYr_uAd6pS3PF5hSgRUUySK89jQEfV2_s2badCYazOkyMdwGzBZOju4u59hgXfRFdY1LcFYfmRZg8L2ZfypGaiuNVuEigyAVyYHjZOYdvEM2E4VqeLQkMN68BqW8S";
        private ZaloOaInfo _zaloOaInfo;
        private ZaloStoreClient _storeClient;
        private DbConnect _dbConnect;

        public GetFromZalo()
        {
        }

        public void Init()
        {
            _zaloOaInfo = new ZaloOaInfo(_oaId, _secretKey);
            _storeClient = new ZaloStoreClient(_zaloOaInfo);
            _dbConnect = new DbConnect();
        }

        public void GetOrderAndOrderDetail()
        {
            var getList = JObject.FromObject(GetOrderList(_storeClient)).ToString();
            JObject splitList = JObject.Parse(getList);
            var jToken = splitList["data"]["orders"];

            List<Customers> listCustomers = GetCustomers(jToken);
            InsertCustomersToDb(listCustomers);

            List<Orders> listOrders = GetOrders(jToken);
            InsertOrdersToDb(listOrders);

            List<OrderDetail> listOrderDetail = GetOrderDetail(jToken);
            InsertOrderDetailToDb(listOrderDetail);
        }

        #region Customers
        /*
         * Input: JToken
         * Output: List<Customers>
         * Get data from JToken, add Customers one by one to list and return list
         */
        public List<Customers> GetCustomers(JToken jToken)
        {
            List<Customers> list = new List<Customers>();
            foreach (var item in jToken)
            {
                int id = GetCustomerIdFromDb((string)item["customerPhone"]);
                if (id < 1)
                {
                    Customers customers = new Customers
                    {
                        Name = (string)item["customerName"],
                        Phone = (string)item["customerPhone"],
                        Adress = (string)item["deliverAddress"] + " - " + (string)item["deliverDistrict"] + " - " + (string)item["deliverCity"],
                        NumberOfPurchasedpe = 0,
                        QuantityPurchased = 0,
                        Type = "Khách hàng"
                    };
                    list.Add(customers);
                }
            }
            return list;
        }

        private void InsertCustomersToDb(List<Customers> list)
        {
            try
            {
                foreach (var item in list)
                {
                    string query =
                        "insert into Customers (Name, Phone, Adress, NumberOfPurchased, QuantityPurchased, Type)" +
                        $"VALUES('{item.Name}', '{item.Phone}', '{item.Adress}', '{item.NumberOfPurchasedpe}', '{item.QuantityPurchased}', '{item.Type}')";
                    _dbConnect.ExecuteQuery(query);
                    Console.WriteLine("Insert thanh cong");
                }
                Console.WriteLine("END!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi khi insert customers vao db" + e);
            }
        }

        private int GetCustomerIdFromDb(string phone)
        {
            string query = $"select Customers.Id from Customers where Customers.Phone = '{phone}' limit 1;";
            return _dbConnect.ExecuteQueryToGetId(query);
        }

        #endregion Customers

        #region Orders

        private List<Orders> GetOrders(JToken jToken)
        {
            List<Orders> list = new List<Orders>();
            foreach (var item in jToken)
            {
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

                list.Add(orders);
            }
            return list;
        }

        private void InsertOrdersToDb(List<Orders> list)
        {
            try
            {
                foreach (var item in list)
                {
                    string query =
                        "INSERT INTO Orders (OrderCode, CreatedTime, UpdatedTime, ShipId, TotalPrice, CustomerId, VerifyBy, OrderFrom, Type)" +
                        $"VALUES('{item.OrderCode}', '{item.CreatedTime}', '{item.UpdatedTime}', '{item.ShipId}', '{item.TotalPrice}', '{item.CustomerId}','{item.VerifyBy}','{item.OrderFrom}','{item.Type}')";
                    _dbConnect.ExecuteQuery(query);
                    Console.WriteLine("Insert thanh cong");
                }
                Console.WriteLine("END!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi khi insert orders vao db" + e);
            }
        }

        #endregion Orders

        #region OrderDetail

        private List<OrderDetail> GetOrderDetail(JToken jToken)
        {
            List<OrderDetail> list = new List<OrderDetail>();
            foreach (var item in jToken)
            {
                OrderDetail orderDetail = new OrderDetail();

                int orderId = GetOrderIdFromDb((string)item["orderCode"]);
                if (orderId < 1)
                {
                    continue;
                }
                orderDetail.OrderId = orderId;
                orderDetail.Quantity = ((int)item["numItem"]);
                orderDetail.DeliverCity = (string)item["deliverCity"];
                orderDetail.DeliverDistrict = (string)item["deliverDistrict"];
                orderDetail.DeliverAddress = (string)item["deliverAddress"];
                orderDetail.ProductId = 0;
                list.Add(orderDetail);
            }
            return list;
        }

        private int GetOrderIdFromDb(string orderCode)
        {
            string query = $"select Orders.Id from Orders where Orders.OrderCode = '{orderCode}' limit 1;";
            return _dbConnect.ExecuteQueryToGetId(query);
        }

        private void InsertOrderDetailToDb(List<OrderDetail> list)
        {
            try
            {
                foreach (var item in list)
                {
                    string query =
                        "INSERT INTO OrderDetail (OrderId, Quantity, DeliverCity, DeliverDistrict, DeliverAddress, ProductId)" +
                        $"VALUES('{item.OrderId}', '{item.Quantity}', '{item.DeliverCity}', '{item.DeliverDistrict}', '{item.DeliverAddress}', '{item.ProductId}')";
                    _dbConnect.ExecuteQuery(query);
                    Console.WriteLine("Insert thanh cong");
                }
                Console.WriteLine("END!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi khi insert orderdetail vao db" + e);
            }
        }

        #endregion OrderDetail

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

        #region Others

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

        private static object GetProduct(string productId, ZaloStoreClient storeClient)
        {
            JObject getProduct = storeClient.getProduct(productId);
            return getProduct;
        }

        private static object GetProductList(ZaloStoreClient storeClient)
        {
            JObject getProductOfOa = storeClient.getProductOfOa(0, 10);
            return getProductOfOa;
        }

        private static object GetOrderInfo(string orderId, ZaloStoreClient storeClient)
        {
            JObject getOrder = storeClient.getOrder(orderId);
            return getOrder;
        }

        public void a()
        {
            Console.WriteLine(GetCustomerIdFromDb("84963209769"));
        }

        #endregion Others
    }
}