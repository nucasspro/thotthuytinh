using OMS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace OMS.ViewModel
{
    public class OrderManagementUCViewModel : BaseViewModel
    {
        #region command

        public ICommand SearchCommand { get; set; }
        public ICommand LoadCommand { get; set; }

        public ICommand SelectionChangedCommand { get; set; }
        public ObservableCollection<Orders> List { get; set; }
        public string SelectedValue { get; set; }

        #endregion command

        public OrderManagementUCViewModel()
        {
            List = new ObservableCollection<Orders>();
            // ReSharper disable once ComplexConditionExpression
            SelectionChangedCommand = new RelayCommand<ComboBox>(p => true, p =>
            {
                ComboBoxItem comboBox = (ComboBoxItem)p.SelectedItem;
                SelectedValue = comboBox.Content.ToString();
                LoadData(SelectedValue);
                // FilterData(listSource);
            });
            //LoadCommand = new RelayCommand<Orders>(p => true, p => { LoadData(SelectedItem); });
            //SearchCommand = new RelayCommand<Orders>(p => true, p => { LoadData(); });
        }

        public void FilterData(ObservableCollection<Orders> listSource)
        {
            List<Orders> b = listSource.Where(x => (x.OrderFrom == SelectedValue)).ToList();
            ObservableCollection<Orders> a = new ObservableCollection<Orders>(b);
            List = a;
        }

        public void LoadData(string SelectedValue)
        {
            DBConnect dbConnect = new DBConnect();
            //string query = @"select Orders.id, Customers.Name, datetime(Orders.CreatedTime, 'unixepoch','localtime') as CreatedTime, Orders.Status, Orders.OrderFrom from Orders inner join Customers where Orders.CustomerId = Customers.Id;";
            string query = @"select Orders.id, Customers.Name, datetime(Orders.CreatedTime, 'unixepoch','localtime') as CreatedTime, Orders.Status, Orders.OrderFrom from Orders inner join Customers where Orders.CustomerId = Customers.Id and Orders.OrderFrom = '" + SelectedValue + "';";
            DataTable dataTable = dbConnect.SelectQuery(query);
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
                    Status = (string)((DataRow)row).ItemArray[3],
                    OrderFrom = (string)((DataRow)row).ItemArray[4]
                };
                List.Add(order);
            }
        }
    }
}