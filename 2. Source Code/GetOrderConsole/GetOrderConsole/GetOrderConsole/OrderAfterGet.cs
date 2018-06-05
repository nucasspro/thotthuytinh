using System;

namespace ConsoleGetOrder
{
    public class OrderAfterGet
    {
        public OrderAfterGet() { }
        public OrderAfterGet(string price, string orderCode, DateTime createdTime, DateTime updatedTime, string productName, string productImage, int numberItem, string deliverCity, string deliverDistrict, string shippingInfo)
        {
            this.price = price;
            this.orderCode = orderCode;
            this.createdTime = createdTime;
            this.updatedTime = updatedTime;
            this.productName = productName;
            this.productImage = productImage;
            this.numberItem = numberItem;
            this.deliverCity = deliverCity;
            this.deliverDistrict = deliverDistrict;
            this.shippingInfo = shippingInfo;
        }

        public string price { get; set; }
        public string orderCode { get; set; }
        public DateTime createdTime { get; set; }
        public DateTime updatedTime { get; set; }
        public string productName { get; set; }
        public string productImage { get; set; }
        public int numberItem { get; set; }
        public string deliverCity { get; set; }
        public string deliverDistrict { get; set; }
        public string shippingInfo { get; set; }
    }
}