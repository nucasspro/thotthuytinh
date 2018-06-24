using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMS.Model;

namespace OMS.ViewModel
{
    public class ProductManagementUCViewModel:BaseViewModel
    {
        public ObservableCollection<Products> ListProduct { get; set; }

        public ProductManagementUCViewModel()
        {
            ListProduct = new ObservableCollection<Products>();
            LoadProduct();
        }

        public void LoadProduct()
        {
            DBConnect dbConnect = new DBConnect();
            const string query = @"select * from Products;";
            DataTable dataTable = dbConnect.SelectQuery(query);
            foreach (var row in dataTable.Rows)
            {
                Products product = new Products();
                product.Id = (string)((DataRow)row).ItemArray[0];
                product.Name = (string)((DataRow)row).ItemArray[1];
                product.Weight = (string)((DataRow)row).ItemArray[2];
                product.Width = (string)((DataRow)row).ItemArray[3];
                product.Height = (string)((DataRow)row).ItemArray[4];
                product.Length = (string)((DataRow)row).ItemArray[5];
                product.Price = (string)((DataRow)row).ItemArray[6];
                product.Image1 = (string)((DataRow)row).ItemArray[7];
                product.Image2 = (string)((DataRow)row).ItemArray[8];
                product.Image3 = (string)((DataRow)row).ItemArray[9];
                product.Quantity = Convert.ToInt32(((DataRow)row).ItemArray[10]);
                product.CreatedBy = new Accounts { Id = Convert.ToInt32(((DataRow)row).ItemArray[11]) };
                ListProduct.Add(product);
            }
        }
    }
}
