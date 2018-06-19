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
                Customers customers = new Customers();
                customers.Name = (string)item["customerName"];
                customers.Phone = "0" + (string)item["customerPhone"];
                customers.Address = (string)item["deliverAddress"] + " - " + (string)item["deliverDistrict"] + " - " + (string)item["deliverCity"];
                customers.NumberOfPurchasedpe = 0;
                customers.QuantityPurchased = 0;
                customers.Type = "Khách hàng";
                InsertCustomersToDb(customers);
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

                //if (Check(time, createdTime) == 1)
                //{
                //    continue;
                //}

                Orders orders = new Orders();
                orders.OrderCode = (string)item["orderCode"];
                orders.CreatedTime = ((string)item["createdTime"]).Remove(10, 3);
                orders.UpdatedTime = ((string)item["updatedTime"]).Remove(10, 3); ;
                orders.TotalPrice = ((float)item["price"] * (float)item["numItem"]).ToString(CultureInfo.InvariantCulture);
                orders.CustomerId = GetCustomerIdFromDb("0" + (string)item["customerPhone"]);
                orders.IsVerify = "Chưa duyệt";
                orders.VerifyBy = 1;
                orders.OrderFrom = "Zalo";
                orders.Type = "Bán cho khách";
                orders.DeliverCity = (string)item["deliverCity"];
                orders.DeliverDistrict = (string)item["deliverDistrict"];
                orders.DeliverAddress = (string)item["deliverAddress"];
                orders.CallShip = "Chưa gọi ship";
                orders.PackageWidth = "0";
                orders.PackageHeight = "0";
                orders.PackageWeight = "0";
                InsertOrdersToDb(orders);


                OrderDetail orderDetail = new OrderDetail
                {
                    OrderId = GetOrderIdFromDb((string)item["orderCode"]),
                    ProductId = (string)item["productCode"],
                    Quantity = (int)item["numItem"]
                };
                InsertOrderDetailToDb(orderDetail);
            }
        }

        private int GetCustomerIdFromDb(string phone)
        {
            string query = $"select Id from Customers where Customers.Phone = '{phone}' limit 1;";
            return _dbConnect.GetIdAndCountId(query);
        }

        private int GetOrderIdFromDb(string orderCode)
        {
            string query = $"select Orders.Id from Orders where Orders.OrderCode = '{orderCode}' limit 1;";
            return _dbConnect.GetIdAndCountId(query);
        }

        private static object GetOrderList(ZaloStoreClient storeClient)
        {
            JObject getOrderOfOa = storeClient.getOrderOfOa(0, 10, 0);
            return getOrderOfOa;
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
                               $"VALUES('{customer.Name}', '{customer.Phone}', '{customer.Address}', '{customer.NumberOfPurchasedpe}', '{customer.QuantityPurchased}', '{customer.Type}');";
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

        private static DateTime UnixTimestampToDateTime(double unixTime)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds(unixTime).ToLocalTime();
            return dateTime;
        }
    }
}