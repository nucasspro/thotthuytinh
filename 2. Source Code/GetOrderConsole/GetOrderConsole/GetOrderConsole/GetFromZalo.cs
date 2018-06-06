using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using ZaloCSharpSDK;

namespace ConsoleGetOrder
{
    public class GetFromZalo
    {
        private static readonly long _oaId = 1364144657533100885;
        private static string _secretKey = "hGq0eOPke9SvuMmGfiKx";
        private static string _secretKeyApp = "17GTbC5Taph5FCo1XGTS";
        private static string _callbackUrl = @"https://tatatatatata.herokuapp.com/";
        private static long _appId = 2491981268401425748;
        private static string _accessToken = "UUVXaTppuYr_uAd6pS3PF5hSgRUUySK89jQEfV2_s2badCYazOkyMdwGzBZOju4u59hgXfRFdY1LcFYfmRZg8L2ZfypGaiuNVuEigyAVyYHjZOYdvEM2E4VqeLQkMN68BqW8S";
        private ZaloOaInfo _zaloOaInfo;
        private ZaloStoreClient _storeClient;

        public GetFromZalo()
        {
        }

        public void Init()
        {
            _zaloOaInfo = new ZaloOaInfo(_oaId, _secretKey);
            _storeClient = new ZaloStoreClient(_zaloOaInfo);
        }

        public List<OrderAfterGet> GetOrder()
        {
            var getList = JObject.FromObject(GetOrderList(_storeClient)).ToString();
            JObject splitList = JObject.Parse(getList);
            var jToken = splitList["data"]["orders"];

            List<OrderAfterGet> list = new List<OrderAfterGet>();

            foreach (var item in jToken)
            {
                OrderAfterGet x = new OrderAfterGet();

                x.Price = (string)item["price"];
                x.OrderCode = (string)item["orderCode"];

                string unixCreatedTime = ((string)item["createdTime"]).Remove(9, 3);
                DateTime createdTime = UnixTimestampToDateTime(Convert.ToDouble(unixCreatedTime));
                x.CreatedTime = createdTime;

                string unixUpdatedTime = ((string)item["updatedTime"]).Remove(9, 3);
                DateTime updatedTime = UnixTimestampToDateTime(Convert.ToDouble(unixUpdatedTime));
                x.UpdatedTime = updatedTime;

                x.ProductName = (string)item["productName"];
                x.ProductImage = (string)item["productImage"];
                x.NumberItem = (int)item["numItem"];
                x.DeliverCity = (string)item["deliverCity"];
                x.DeliverDistrict = (string)item["deliverDistrict"];
                x.ShippingInfo = (string)item["shippingInfo"];
                x.OrderFrom = "Zalo";
                list.Add(x);
            }

            return list;
        }

        public List<OrderAfterGet> SortOrderByDay(List<OrderAfterGet> list)
        {
            List<OrderAfterGet> sortList = new List<OrderAfterGet>();
            foreach (var item in list)
            {
                if (item.CreatedTime == DateTime.Today)
                {
                    sortList.Add(item);
                }
            }
            return sortList;
        }

        private static object GetOrderList(ZaloStoreClient storeClient)
        {
            JObject getOrderOfOa = storeClient.getOrderOfOa(0, 10, 0);
            return getOrderOfOa;
        }

        public static DateTime UnixTimestampToDateTime(double unixTime)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds(unixTime).ToLocalTime();
            return dateTime;
        }

        public bool InsertOrderToDb()
        {
            DbConnect dbConnect = new DbConnect();

            try
            {
                var listOrder = GetOrder();
                foreach (var item in listOrder)
                {
                    string query =
                        string.Format("INSERT INTO Orders (OrderCode, Price, CreatedTime, UpdatedTime, ProductName, ProductImage, NumberItem, DeliverCity, DeliverDistrict, ShippingInfo, IsVerify, OrderFrom)" +
                                      "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}','{7}','{8}','{9}','{10}', '{11}')",
                            item.OrderCode, item.Price, item.CreatedTime, item.UpdatedTime, item.ProductName, item.ProductImage, item.NumberItem, item.DeliverCity, item.DeliverDistrict, item.ShippingInfo, item.IsVerify, item.OrderFrom);
                    dbConnect.InsertData(query);
                    Console.WriteLine("Insert thanh cong");
                }

                Console.WriteLine("END!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi khi insert vao db" + e);
                return false;
            }

            return true;
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

        public string a()
        {
            return JObject.FromObject(GetOrderList(_storeClient)).ToString();
        }
    }
}