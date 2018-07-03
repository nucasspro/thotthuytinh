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
        private DateTime _time1, _time2, _time3;

        public Zalo(DateTime time1, DateTime time2, DateTime time3)
        {
            _zaloOaInfo = new ZaloOaInfo(OaId, SecretKey);
            _storeClient = new ZaloStoreClient(_zaloOaInfo);
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

                Orders orders = new Orders();
                orders.OrderCode = (string)item["orderCode"];
                orders.CreatedTime = ((string)item["createdTime"]).Remove(10, 3);
                orders.UpdatedTime = ((string)item["updatedTime"]).Remove(10, 3);
                orders.SubTotal = ((float)item["price"] * (float)item["numItem"]).ToString(CultureInfo.InvariantCulture);
                orders.GrandPrice = ((float)item["price"] * (float)item["numItem"]).ToString(CultureInfo.InvariantCulture);
                orders.CustomerId = tempCustomers.GetCustomerIdFromDb("0" + (string)item["customerPhone"]);
                orders.Status = "Chưa duyệt";
                orders.VerifyBy = 1;
                orders.OrderFrom = "Zalo";
                orders.Type = "Bán cho khách";
                orders.BillingAddress = orders.ShippingAddress = (string)item["deliverAddress"] + " - " + (string)item["deliverDistrict"] + " - " + (string)item["deliverCity"];
                orders.CallShip = "Chưa gọi ship";
                orders.ShipPrice = "0";
                orders.PackageWidth = "0";
                orders.PackageHeight = "0";
                orders.PackageLenght = "0";
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