namespace GetOrderConsole
{
    public class OrderDetail
    {
        public OrderDetail()
        {
        }

        public OrderDetail(int orderId, string productId, int quantity)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }

        public int OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}