using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;

namespace OMS.Model

{
    public class OrderDetail
    {
        public int Id { get; set; }
        public Orders OrderId { get; set; }
        public Products Product { get; set; }
        public int Quantity { get; set; }

        public ObservableCollection<OrderDetail> LoadDataToOrderDetail(string SelectedValue, string OrderID)
        {
            DBConnect dbConnect = new DBConnect();
            ObservableCollection<OrderDetail> listtemp = new ObservableCollection<OrderDetail>();
            int temp = Convert.ToInt32(OrderID);

            string query = @"select temp.Id, temp.Name, temp.Quantity, temp.Price
                            from (select OrderDetail.Id, Products.Name, Products.Price, OrderDetail.Quantity, OrderDetail.OrderId
		                    from OrderDetail inner join Products
		                    where OrderDetail.ProductId=Products.id) as temp inner join Orders
                            where temp.OrderID=Orders.Id
                            and Orders.OrderFrom = '" + SelectedValue + "' " +
                            "and Orders.Id=" + temp + ";";
            DataTable dataTable = dbConnect.SelectQuery(query);
            foreach (var row in dataTable.Rows)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    Id = Convert.ToInt32(((DataRow)row).ItemArray[0]),
                    Product = new Products
                    {
                        Name = (string)((DataRow)row).ItemArray[1],
                        Price = (string)((DataRow)row).ItemArray[3]
                    },
                    Quantity = Convert.ToInt32(((DataRow)row).ItemArray[2])
                };
                listtemp.Add(orderDetail);
            }
            ////auto fill subtotal and grand total
            //SubTotal = CalculateSubTotal();
            //if(Convert.ToInt32(GrandPrice)< Convert.ToInt32(SubTotal))
            //    GrandPrice = SubTotal;
            //AutoUpdatePackageDimension();
            return listtemp;
        }

        public bool DeleteProductFromOrder( int OrderDetailID, string ProductID, int ProductQuantity, int ProductQuanlityStock)
        {
            DBConnect dB = new DBConnect();
            string query1 = $"delete from OrderDetail where Id =" + OrderDetailID + "";
            string query2 = $"Update Products " +
                            $"Set Quantity = {ProductQuanlityStock + ProductQuantity} " +
                            $"where Id = '{ProductID}';";
            try
            {
                dB.ExecuteQuery(query1);
                dB.ExecuteQuery(query2);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddProductToOrder(int OrderID, string ProductID, int ProductQuantity, int ProductQuanlityStock)
        {
            DBConnect dB = new DBConnect();
            String query1 = $"insert into OrderDetail (OrderID, ProductID, Quantity)" +
                                     $" values (" + OrderID + ",'" + ProductID + "'," + ProductQuantity + ");";
            String query2 = $"Update Products " +
                     $"Set Quantity = {ProductQuanlityStock - ProductQuantity} " +
                    $"where Id = '{ProductID}';";

            try
            {
                dB.ExecuteQuery(query1);
                dB.ExecuteQuery(query2);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateProductOrder(int OrderDetailID, string ProductID, int ProductQuantityAfter, int ProductQuantityBefore, int ProductQuanlityStock)
        {
            DBConnect dB = new DBConnect();
            string query1 = $"update OrderDetail" +
                                   $" set ProductID = '" + ProductID + "',Quantity= " + ProductQuantityAfter + "" +
                                   $" where Id =" + OrderDetailID + ";";
            string query2 = $"Update Products " +
                    $"Set Quantity = {ProductQuanlityStock - (ProductQuantityAfter - ProductQuantityBefore)} " +
                    $"where Id = '{ProductID}';";
            try
            {
                dB.ExecuteQuery(query1);
                dB.ExecuteQuery(query2);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}