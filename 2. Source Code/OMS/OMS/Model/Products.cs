using System;
using System.Collections.ObjectModel;
using System.Data;

namespace OMS.Model
{
    public class Products
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Weight { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Length { get; set; }
        public string Price { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public int Quantity { get; set; }
        public Accounts CreatedBy { get; set; }
        public string Status { get; set; }

        #region method

        public ObservableCollection<Products> LoadProduct()
        {
            var dbConnect = new DBConnect();
            var temp = new ObservableCollection<Products>();
            const string query = @"Select * From Products Where status = 'Chưa xóa';";
            var dataTable = dbConnect.SelectQuery(query);
            foreach (var row in dataTable.Rows)
            {
                var product = new Products
                {
                    Id = (string)((DataRow)row).ItemArray[0],
                    Name = (string)((DataRow)row).ItemArray[1],
                    Description = (string)((DataRow)row).ItemArray[2],
                    Weight = (string)((DataRow)row).ItemArray[3],
                    Width = (string)((DataRow)row).ItemArray[4],
                    Height = (string)((DataRow)row).ItemArray[5],
                    Length = (string)((DataRow)row).ItemArray[6],
                    Price = (string)((DataRow)row).ItemArray[7],
                    Image1 = (string)((DataRow)row).ItemArray[8],
                    Image2 = (string)((DataRow)row).ItemArray[9],
                    Image3 = (string)((DataRow)row).ItemArray[10],
                    Quantity = Convert.ToInt32(((DataRow)row).ItemArray[11]),
                    CreatedBy = new Accounts { Id = Convert.ToInt32(((DataRow)row).ItemArray[12]) },
                    Status = (string)((DataRow)row).ItemArray[13]
                };
                temp.Add(product);
            }
            return temp;
        }

        public void CreateProduct(Products product)
        {
            var dbConnect = new DBConnect();
            string query = $"Insert into Products(Id, Name, Description, Weight, Width, Height, Length, Price, Image1, Image2, Image3, Quantity, CreatedBy, Status) " +
                           $"values ('{product.Id}', '{product.Name}', '{product.Description}', '{product.Weight}', '{product.Width}', '{product.Height}', '{product.Length}', " +
                           $"'{product.Price}', '{product.Image1}','{product.Image2}','{product.Image3}','{product.Quantity}', '{product.CreatedBy}', '{product.Status}')";
            dbConnect.ExecuteQuery(query);
        }

        public void UpdateProduct(Products product)
        {
            var dbConnect = new DBConnect();
            string query = $"Update Products " +
                           $"Set Name = '{product.Name}', Description = '{product.Description}', Weight = '{product.Weight}', Width = '{product.Width}', Height = '{product.Height}', Length = '{product.Length}', " +
                           $"Price = '{product.Price}', Image1 = '{product.Image1}', Image2 = '{product.Image2}', Image3 = '{product.Image3}', Quantity = {product.Quantity} " +
                           $"where Id = '{product.Id}';";
            dbConnect.ExecuteQuery(query);
        }

        public void DeleteProduct(string id)
        {
            var dbConnect = new DBConnect();
            string query = $"Update Products " +
                           $"Set Status = 'Đã xóa' " +
                           $"where Id = '{id}';";
            dbConnect.ExecuteQuery(query);
        }

        #endregion method
    }
}