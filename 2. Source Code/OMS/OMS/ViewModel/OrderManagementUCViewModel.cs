using OMS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
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

        private Orders _SelectedItem { get; set; }

        public Orders SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem == null)
                    return;
                OrderID = SelectedItem.Id.ToString();
                SelectedItemStatus = SelectedItem.Status.Equals("Chưa duyệt") ? 0 : 1;

                //MessageBox.Show(SelectedItem.Id.ToString());
            }
        }

        private string _OrderID { get; set; }

        public string OrderID
        {
            get => _OrderID;
            set
            {
                _OrderID = value;
                OnPropertyChanged();
            }
        }


        private int _SelectedItemStatus { get; set; }

        public int SelectedItemStatus
        {
            get => _SelectedItemStatus;
            set
            {
                _SelectedItemStatus = value;
                OnPropertyChanged();
            }
        }

        #endregion command

        public OrderManagementUCViewModel()
        {
            List = new ObservableCollection<Orders>();
            // ReSharper disable once ComplexConditionExpression
            SelectionChangedCommand = new RelayCommand<ComboBox>(p => true, p =>
            {
                if (List.Count != 0)
                {
                    List.Clear();
                }
                ComboBoxItem comboBox = (ComboBoxItem)p.SelectedItem;
                SelectedValue = comboBox.Content.ToString();
                LoadData(SelectedValue);
            });
            //LoadCommand = new RelayCommand<Orders>(p => true, p => { LoadData(SelectedItem); });
            //SearchCommand = new RelayCommand<Orders>(p => true, p => { LoadData(); });
        }

        public ObservableCollection<Orders> FilterData(ObservableCollection<Orders> listSource)
        {
            return (ObservableCollection<Orders>)listSource.Where(x => x.OrderFrom == SelectedValue);
        }

        public void LoadData(string SelectedValue)
        {
            DBConnect dbConnect = new DBConnect();
            //string query = @"select Orders.id, Customers.Name, datetime(Orders.CreatedTime, 'unixepoch','localtime') as CreatedTime, Orders.Status, Orders.OrderFrom from Orders inner join Customers where Orders.CustomerId = Customers.Id;";
            string query = @"select Orders.Id, Customers.Name,
                            datetime(Orders.CreatedTime, 'unixepoch','localtime') as CreatedTime,
                            Orders.GrandPrice,
                            Orders.Status, Orders.ShippingAddress, Orders.BillingAddress, Orders.CallShip,
                            Orders.PackageWidth, Orders.PackageWeight, Orders.PackageHeight
                            from Orders inner join Customers
                            where Orders.CustomerId = Customers.Id and Orders.OrderFrom = '" + SelectedValue + "';";
            DataTable dataTable = dbConnect.SelectQuery(query);
            foreach (var row in dataTable.Rows)
            {
                Orders order = new Orders
                {
                    Id = Convert.ToInt32(((DataRow)row).ItemArray[0]),
                    Customer = new Customers { Name = (string)((DataRow)row).ItemArray[1] },
                    CreatedTime = (string)((DataRow)row).ItemArray[2],
                    GrandPrice = (string)((DataRow)row).ItemArray[3],
                    Status = (string)((DataRow)row).ItemArray[4],
                    ShippingAddress = (string)((DataRow)row).ItemArray[5],
                    BillingAddress = (string)((DataRow)row).ItemArray[6],
                    CallShip = (string)((DataRow)row).ItemArray[7],
                    PackageWidth = (string)((DataRow)row).ItemArray[8],
                    PackageWeight = (string)((DataRow)row).ItemArray[9],
                    PackageHeight = (string)((DataRow)row).ItemArray[10]
                };
                List.Add(order);
            }
        }
    }
}