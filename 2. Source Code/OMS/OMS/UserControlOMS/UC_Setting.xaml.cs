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
            //DbConnect dbConnect = new DbConnect();
            //dbConnect.Init(TextBoxDatabasePath.Text);
            //string query = @"select * from Orders;";
            //DataTable dataTable = dbConnect.SelectQuery(query);
            //List<Orders> list = new List<Orders>();
            //foreach (var row in dataTable.Rows)
            //{
            //    Orders a = new Orders
            //    {
            //        OrderCode = (string)((DataRow)row).ItemArray[1],
            //        CreatedTime = DateTime.Now,
            //        UpdatedTime = DateTime.Now,
            //        ShipId = 0,
            //        TotalPrice = (string)((DataRow)row).ItemArray[5],
            //        CustomerId = 0,
            //        VerifyBy = 0,
            //        OrderFrom = (string)((DataRow)row).ItemArray[9],
            //        Type = (string)((DataRow)row).ItemArray[10]
            //    };
            //    list.Add(a);
            //}
            //ListViewOrder.ItemsSource = list;
        }
    }
}