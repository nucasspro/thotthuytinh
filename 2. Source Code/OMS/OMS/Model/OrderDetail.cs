namespace OMS.Model
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string DeliverCity { get; set; }
        public string DeliverDistrict { get; set; }
        public string DeliverAddress { get; set; }

        public virtual Orders OrderId { get; set; }
        public virtual Products ProductId { get; set; }
    }
}