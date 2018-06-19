namespace OMS.Model
{
    public class Orders
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string CreatedTime { get; set; }
        public string UpdatedTime { get; set; }
        public string TotalPrice { get; set; }
        public string IsVerify { get; set; }
        public string OrderFrom { get; set; }
        public string Type { get; set; }

        public  Customers Customer { get; set; }
        public  Accounts Account { get; set; }
    }
}