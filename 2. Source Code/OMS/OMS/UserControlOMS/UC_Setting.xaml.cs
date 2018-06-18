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
            dbConnect.Init(TextBoxDatabasePath.Text);
            string query = @"select * from Accounts;";
            DataTable dataTable = dbConnect.SelectQuery(query);
            List<Accounts> list = new List<Accounts>();
            foreach (var row in dataTable.Rows)
            {
                Accounts a = new Accounts
                {
                    Id = Convert.ToInt32(((DataRow)row).ItemArray[0]),
                    Username = (string)((DataRow)row).ItemArray[1],
                    Password = (string)((DataRow)row).ItemArray[2],
                    Type = (string)((DataRow)row).ItemArray[3]
                };
                list.Add(a);
            }
            ListViewOrder.ItemsSource = list;
        }
    }
}