namespace GetOrderConsole
{
    public class OrderDetail
    {
        public OrderDetail()
        {
        }

        public OrderDetail(int orderId, int quantity, string deliverCity, string deliverDistrict, string deliverAddress, int productId)
        {
            OrderId = orderId;
            Quantity = quantity;
            DeliverCity = deliverCity;
            DeliverDistrict = deliverDistrict;
            DeliverAddress = deliverAddress;
            ProductId = productId;
            
        }

        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public string DeliverCity { get; set; }
        public string DeliverDistrict { get; set; }
        public string DeliverAddress { get; set; }
        public int ProductId { get; set; }
    }
}