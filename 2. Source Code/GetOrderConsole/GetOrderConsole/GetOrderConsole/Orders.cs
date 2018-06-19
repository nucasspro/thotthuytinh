using System;

namespace GetOrderConsole
{
    public class Orders
    {
        public Orders()
        {
        }

        public Orders(string orderCode, DateTime createdTime, DateTime updatedTime, string totalPrice, int customerId, string isVerify, int verifyBy, string orderFrom, string type, string deliverCity, string deliverDistrict, string deliverAddress, string callShip, string packageWidth, string packageHeight, string packageWeight)
        {
            OrderCode = orderCode;
            CreatedTime = createdTime;
            UpdatedTime = updatedTime;
            TotalPrice = totalPrice;
            CustomerId = customerId;
            IsVerify = isVerify;
            VerifyBy = verifyBy;
            OrderFrom = orderFrom;
            Type = type;
            DeliverCity = deliverCity;
            DeliverDistrict = deliverDistrict;
            DeliverAddress = deliverAddress;
            CallShip = callShip;
            PackageWidth = packageWidth;
            PackageHeight = packageHeight;
            PackageWeight = packageWeight;
        }

        public string OrderCode { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public string IsVerify { get; set; }
        public int VerifyBy { get; set; }
        public string OrderFrom { get; set; }
        public string Type { get; set; }
        public string DeliverCity { get; set; }
        public string DeliverDistrict { get; set; }
        public string DeliverAddress { get; set; }
        public string CallShip { get; set; }
        public string PackageWidth { get; set; }
        public string PackageHeight { get; set; }
        public string PackageWeight { get; set; }
    }
}