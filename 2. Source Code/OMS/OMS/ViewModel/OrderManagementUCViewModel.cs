using OMS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OMS.ViewModel
{
    public class OrderManagementUCViewModel : BaseViewModel
    {
        #region command

        public ICommand CbxGetOrderFromCommand { get; set; }

        #endregion command

        public ObservableCollection<Orders> Ordercollection { get; set; }
        private Model.DBConnect dbConnect;

        //public List<Orders> Ordercollection { get; set; }

        public OrderManagementUCViewModel()
        {
            CbxGetOrderFromCommand = new RelayCommand<ComboBox>(
                (p) => { return true; },
                (p) =>
                {
                    MessageBox.Show(p.SelectedIndex.ToString());
                    dbConnect = new Model.DBConnect();
                    Ordercollection = new ObservableCollection<Orders>();
                    //orderlist = new List<Orders>();
                    string query = @"select Orders.id, Customers.Name, datetime(Orders.CreatedTime, 'unixepoch','localtime'), Orders.IsVerify from Orders inner join Customers where Orders.CustomerId = Customers.Id;";
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
                            IsVerify = (string)((DataRow)row).ItemArray[3]
                        };

                        Ordercollection.Add(order);
                    }
                    
                }
                );
        }
    }
}