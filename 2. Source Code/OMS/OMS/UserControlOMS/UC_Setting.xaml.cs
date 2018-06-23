using OMS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace OMS.UserControlOMS
{
    /// <summary>
    /// Interaction logic for UC_Setting.xaml
    /// </summary>
    public partial class UC_Setting : UserControl
    {
        public UC_Setting()
        {
            InitializeComponent();
        }

        private void ButtonCheckConnect_OnClick(object sender, RoutedEventArgs e)
        {
            DBConnect dbConnect = new DBConnect();
            string query = @"select Orders.id, Customers.Name, datetime(Orders.CreatedTime, 'unixepoch','localtime'), Orders.IsVerify from Orders inner join Customers where Orders.CustomerId = Customers.Id;";
            DataTable dataTable = dbConnect.SelectQuery(query);
            List<Orders> list = new List<Orders>();
            foreach (var row in dataTable.Rows)
            {
                Customers customers = new Customers
                {
                    Name = (string)((DataRow)row).ItemArray[1]
                };

                Orders order = new Orders
                {
                    Customer = customers,
                    Id = Convert.ToInt32(((DataRow)row).ItemArray[0]),
                    CreatedTime = (string)((DataRow)row).ItemArray[2],
                    Status = (string)((DataRow)row).ItemArray[3]
                };
                list.Add(order);
            }
            ListaViewOrder.ItemsSource = list;

        }
    }
}