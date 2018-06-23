namespace OMS.Model
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public Orders OrderId { get; set; }
        public Products ProductId { get; set; }
        public int Quantity { get; set; }
    }
}