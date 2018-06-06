using System;

namespace ConsoleGetOrder
{
    public class OrderAfterGet
    {
        public OrderAfterGet()
        {
        }

        public OrderAfterGet(string price, string orderCode, DateTime createdTime, DateTime updatedTime, string productName, string productImage, int numberItem, string deliverCity, string deliverDistrict, string shippingInfo, string orderFrom)
        {
            Price = price;
            OrderCode = orderCode;
            CreatedTime = createdTime;
            UpdatedTime = updatedTime;
            ProductName = productName;
            ProductImage = productImage;
            NumberItem = numberItem;
            DeliverCity = deliverCity;
            DeliverDistrict = deliverDistrict;
            ShippingInfo = shippingInfo;
            IsVerify = false;
            OrderFrom = orderFrom;
        }

        public string Price { get; set; }
        public string OrderCode { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int NumberItem { get; set; }
        public string DeliverCity { get; set; }
        public string DeliverDistrict { get; set; }
        public string ShippingInfo { get; set; }
        public bool IsVerify { get; set; }
        public string OrderFrom { get; set; }
    }
}