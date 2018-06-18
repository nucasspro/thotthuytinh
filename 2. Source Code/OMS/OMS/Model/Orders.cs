namespace OMS.Model
{
    public class Orders
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public int CreatedTime { get; set; }
        public int UpdatedTime { get; set; }
        public int ShipId { get; set; }
        public string TotalPrice { get; set; }
        public string IsVerify { get; set; }
        public string OrderFrom { get; set; }
        public string Type { get; set; }

        public virtual Customers CustomerId { get; set; }
        public virtual Accounts VerifyBy { get; set; }
    }
}