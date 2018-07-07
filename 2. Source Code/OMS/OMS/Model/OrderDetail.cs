using System;
using System.Collections.ObjectModel;
using System.Data;

namespace OMS.Model
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public Orders OrderId { get; set; }
        public Products Product { get; set; }
        public int Quantity { get; set; }

        #region method

        public ObservableCollection<OrderDetail> LoadDataToOrderDetail(string SelectedValue, string OrderID)
        {
            var dbConnect = new DBConnect();
            var listtemp = new ObservableCollection<OrderDetail>();
            int temp = Convert.ToInt32(OrderID);

            string query = @"Select temp.Id, temp.Name, temp.Quantity, temp.Price
                            From (select OrderDetail.Id, Products.Name, Products.Price, OrderDetail.Quantity, OrderDetail.OrderId
		                    From OrderDetail inner join Products
		                    Where OrderDetail.ProductId=Products.id) as temp inner join Orders
                            Where temp.OrderID=Orders.Id
                            and Orders.OrderFrom = '" + SelectedValue + "' " +
                            "and Orders.Id=" + temp + ";";

            var dataTable = dbConnect.SelectQuery(query);
            foreach (var row in dataTable.Rows)
            {
                var orderDetail = new OrderDetail
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
            return listtemp;
        }

        public bool DeleteProductFromOrder(int OrderDetailID, string ProductID, int ProductQuantity, int ProductQuanlityStock)
        {
            var dB = new DBConnect();
            string query1 = $"Delete From OrderDetail where Id ={OrderDetailID}";
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
            var dB = new DBConnect();
            string query1 = $"Insert into OrderDetail (OrderID, ProductID, Quantity) values ({OrderID}, '{ProductID}', {ProductQuantity});";
            string query2 = $"Update Products Set Quantity = {ProductQuanlityStock - ProductQuantity} Where Id = '{ProductID}';";
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
            var dB = new DBConnect();
            string query1 = $"update OrderDetail Set ProductID = '{ProductID}', Quantity = {ProductQuantityAfter} Where Id = {OrderDetailID};";
            string query2 = $"Update Products Set Quantity = {ProductQuanlityStock - (ProductQuantityAfter - ProductQuantityBefore)} Where Id = '{ProductID}';";
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

        #endregion method
    }
}