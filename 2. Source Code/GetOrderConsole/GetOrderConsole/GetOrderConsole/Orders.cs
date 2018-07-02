namespace GetOrderConsole
{
    public class Orders
    {
        public Orders()
        {
        }

        public Orders(string orderCode, string createdTime, string updatedTime, string subTotal, string grandPrice, int customerId, string status, int verifyBy, string orderFrom, string type, string shippingAddress, string billingAddress, string callShip, string shipPrice, string packageWidth, string packageHeight, string packageLenght)
        {
            OrderCode = orderCode;
            CreatedTime = createdTime;
            UpdatedTime = updatedTime;
            SubTotal = subTotal;
            GrandPrice = grandPrice;
            CustomerId = customerId;
            Status = status;
            VerifyBy = verifyBy;
            OrderFrom = orderFrom;
            Type = type;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            CallShip = callShip;
            ShipPrice = ShipPrice;
            PackageWidth = packageWidth;
            PackageHeight = packageHeight;
            PackageLenght = packageLenght;
        }

        public string OrderCode { get; set; }
        public string CreatedTime { get; set; }
        public string UpdatedTime { get; set; }
        public string SubTotal { get; set; }
        public string GrandPrice { get; set; }
        public int CustomerId { get; set; }
        public string Status { get; set; }
        public int VerifyBy { get; set; }
        public string OrderFrom { get; set; }
        public string Type { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public string CallShip { get; set; }
        public string ShipPrice { get; set; }
        public string PackageWidth { get; set; }
        public string PackageHeight { get; set; }
        public string PackageLenght { get; set; }
    }
}