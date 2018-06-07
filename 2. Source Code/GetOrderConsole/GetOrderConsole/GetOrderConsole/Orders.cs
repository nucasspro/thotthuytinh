using System;

namespace GetOrderConsole
{
    public class Orders
    {
        public Orders()
        {
        }

        public Orders(string orderCode, DateTime createdTime, DateTime updatedTime, int shipId, string totalPrice, int customerId, int verifyBy, string orderFrom, string type)
        {
            OrderCode = orderCode;
            CreatedTime = createdTime;
            UpdatedTime = updatedTime;
            ShipId = shipId;
            TotalPrice = totalPrice;
            CustomerId = customerId;
            IsVerify = "Chưa duyệt";
            VerifyBy = verifyBy;
            OrderFrom = orderFrom;
            Type = type;
        }

        public string OrderCode { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int ShipId { get; set; }
        public string TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public string IsVerify { get; set; }
        public int VerifyBy { get; set; }
        public string OrderFrom { get; set; }
        public string Type { get; set; }
    }
}