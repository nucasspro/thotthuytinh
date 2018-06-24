using OMS.Model;
using System;
using System.Collections.ObjectModel;
using System.Data;
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

        #endregion command

        #region Variable

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
                OrderStatus = SelectedItem.Status.Equals("Chưa duyệt") ? 0 : 1;
                CustomerName = SelectedItem.Customer.Name;
                GrandPrice = SelectedItem.GrandPrice;
                ShippingAddress = SelectedItem.ShippingAddress;
                BillingAddress = SelectedItem.BillingAddress;
                CallShip = SelectedItem.CallShip.Equals("Đã đặt") ? 0 : 1;
                PackageHeight = SelectedItem.PackageHeight;
                PackageWeight = SelectedItem.PackageWeight;
                PackageWidth = SelectedItem.PackageWidth;
            }
        }

        private string _OrderID { get; set; }

        public string OrderID
        {
            get => _OrderID;
            set { _OrderID = value; OnPropertyChanged(); }
        }

        private int _OrderStatus { get; set; }

        public int OrderStatus
        {
            get => _OrderStatus;
            set { _OrderStatus = value; OnPropertyChanged(); }
        }

        private string _CustomerName { get; set; }

        public string CustomerName
        {
            get => _CustomerName;
            set { _CustomerName = value; OnPropertyChanged(); }
        }

        private string _GrandPrice { get; set; }

        public string GrandPrice
        {
            get => _GrandPrice;
            set { _GrandPrice = value; OnPropertyChanged(); }
        }

        private string _BillingAddress { get; set; }

        public string BillingAddress
        {
            get => _BillingAddress;
            set { _BillingAddress = value; OnPropertyChanged(); }
        }

        private string _ShippingAddress { get; set; }

        public string ShippingAddress
        {
            get => _ShippingAddress;
            set { _ShippingAddress = value; OnPropertyChanged(); }
        }

        private int _CallShip { get; set; }

        public int CallShip
        {
            get => _CallShip;
            set { _CallShip = value; OnPropertyChanged(); }
        }

        private string _PackageWidth { get; set; }

        public string PackageWidth
        {
            get => _PackageWidth;
            set { _PackageWidth = value; OnPropertyChanged(); }
        }

        private string _PackageWeight { get; set; }

        public string PackageWeight
        {
            get => _PackageWeight;
            set { _PackageWeight = value; OnPropertyChanged(); }
        }

        private string _PackageHeight { get; set; }

        public string PackageHeight
        {
            get => _PackageHeight;
            set { _PackageHeight = value; OnPropertyChanged(); }
        }

        #endregion Variable

        #region Method

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

        #endregion Method
    }
}