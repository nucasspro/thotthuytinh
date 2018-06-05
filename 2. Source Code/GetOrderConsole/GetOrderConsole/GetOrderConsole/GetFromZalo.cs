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

                x.price = (string)item["price"];
                x.orderCode = (string)item["orderCode"];

                string unixCreatedTime = ((string)item["createdTime"]).Remove(9, 3);
                DateTime createdTime = UnixTimestampToDateTime(Convert.ToDouble(unixCreatedTime));
                x.createdTime = createdTime;

                string unixUpdatedTime = ((string)item["updatedTime"]).Remove(9, 3);
                DateTime updatedTime = UnixTimestampToDateTime(Convert.ToDouble(unixUpdatedTime));
                x.updatedTime = updatedTime;

                x.productName = (string)item["productName"];
                x.productImage = (string)item["productImage"];
                x.numberItem = (int)item["numItem"];
                x.deliverCity = (string)item["deliverCity"];
                x.deliverDistrict = (string)item["deliverDistrict"];
                x.shippingInfo = (string)item["shippingInfo"];
                list.Add(x);
            }

            return list;
        }


        public List<OrderAfterGet> SortOrderByDay(List<OrderAfterGet> list)
        {
            List<OrderAfterGet> sortList = new List<OrderAfterGet>();
            foreach (var item in list)
            {
                if (item.createdTime == DateTime.Today)
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

        private static object GetOrderList(ZaloStoreClient storeClient)
        {
            JObject getOrderOfOa = storeClient.getOrderOfOa(0, 10, 0);
            return getOrderOfOa;
        }

        private static object GetOrderInfo(string orderId, ZaloStoreClient storeClient)
        {
            JObject getOrder = storeClient.getOrder(orderId);
            return getOrder;
        }

        public static DateTime UnixTimestampToDateTime(double unixTime)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds(unixTime).ToLocalTime();
            return dateTime;
        }
    }
}