namespace OMS.Model
{
    public class Orders
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string CreatedTime { get; set; }
        public string UpdatedTime { get; set; }
        public string SubPrice { get; set; }
        public string GrandPrice { get; set; }
        public Customers Customer { get; set; }
        public string Status { get; set; }
        public Accounts Account { get; set; }
        public string OrderFrom { get; set; }
        public string Type { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public string CallShip { get; set; }
        public string PackageWidth { get; set; }
        public string PackageHeight { get; set; }
        public string PackageWeight { get; set; }
    }
}