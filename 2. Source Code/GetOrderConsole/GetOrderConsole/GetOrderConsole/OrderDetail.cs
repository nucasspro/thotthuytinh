using System;

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

        public void InsertOrderDetailToDb(OrderDetail orderDetail)
        {
            try
            {
                DbConnect dbConnect = new DbConnect();
                string query = "INSERT INTO OrderDetail (OrderId, ProductId, Quantity) " +
                               $"VALUES('{orderDetail.OrderId}','{orderDetail.ProductId}', '{orderDetail.Quantity}');";
                dbConnect.ExecuteQuery(query);
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi khi insert orderdetail vao db" + e);
            }
        }
    }
}