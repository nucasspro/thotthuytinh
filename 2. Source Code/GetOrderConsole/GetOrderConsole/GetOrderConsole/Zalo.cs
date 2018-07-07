using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using ZaloCSharpSDK;

namespace GetOrderConsole
{
    public class Zalo
    {
        private long _OaId = 0;
        private string _SecretKey = "";
        private ZaloOaInfo _zaloOaInfo;
        private ZaloStoreClient _storeClient;
        private DateTime _time1, _time2, _time3;

        public Zalo(DateTime time1, DateTime time2, DateTime time3, long Oaid, string SecretKey)
        {
            _time1 = time1;
            _time2 = time2;
            _time3 = time3;
            _OaId = Oaid;
            _SecretKey = SecretKey;
            _zaloOaInfo = new ZaloOaInfo(_OaId, SecretKey);
            _storeClient = new ZaloStoreClient(_zaloOaInfo);
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
            Customers tempCustomers = new Customers();
            foreach (var item in jToken)
            {
                if (tempCustomers.CheckCustomerExists("0" + (string)item["customerPhone"]) >= 1)
                {
                    continue;
                }
                Customers customers = new Customers();
                customers.Name = (string)item["customerName"];
                customers.Phone = "0" + (string)item["customerPhone"];
                customers.Address = (string)item["deliverAddress"] + " - " + (string)item["deliverDistrict"] + " - " + (string)item["deliverCity"];
                customers.Type = "Khách hàng";
                tempCustomers.InsertCustomersToDb(customers);
            }
        }

        private void GetOrdersAndOrderDetail(JToken jToken, int time)
        {
            Orders tempOrders = new Orders();
            OrderDetail tempOrderDetail = new OrderDetail();
            Customers tempCustomers = new Customers();
            foreach (var item in jToken)
            {
                if (tempOrders.GetOrderIdFromDb((string)item["orderCode"]) != 0)
                {
                    continue;
                }

                //if (Check(time, createdTime) == 1)
                //{
                //    continue;
                //}

                // ReSharper disable once ComplexConditionExpression
                Orders orders = new Orders
                {
                    OrderCode = (string)item["orderCode"],
                    CreatedTime = ((string)item["createdTime"]).Remove(10, 3),
                    UpdatedTime = ((string)item["updatedTime"]).Remove(10, 3),
                    SubTotal = ((float)item["price"] * (float)item["numItem"]).ToString(CultureInfo.InvariantCulture),
                    GrandPrice = ((float)item["price"] * (float)item["numItem"]).ToString(CultureInfo.InvariantCulture),
                    CustomerId = tempCustomers.GetCustomerIdFromDb("0" + (string)item["customerPhone"]),
                    Status = "Chưa duyệt",
                    VerifyBy = 1,
                    OrderFrom = "Zalo",
                    Type = "Bán cho khách",
                    ShippingAddress = (string)item["deliverAddress"] + " - " + (string)item["deliverDistrict"] + " - " + (string)item["deliverCity"],
                    BillingAddress = (string)item["deliverAddress"] + " - " + (string)item["deliverDistrict"] + " - " + (string)item["deliverCity"],
                    CallShip = "Chưa gọi ship",
                    ShipId = "",
                    ShipPrice = "0",
                    PackageWidth = "0",
                    PackageHeight = "0",
                    PackageLenght = "0"
                };
                tempOrders.InsertOrdersToDb(orders);

                OrderDetail orderDetail = new OrderDetail
                {
                    OrderId = tempOrders.GetOrderIdFromDb((string)item["orderCode"]),
                    ProductId = (string)item["productCode"],
                    Quantity = (int)item["numItem"]
                };
                tempOrderDetail.InsertOrderDetailToDb(orderDetail);
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