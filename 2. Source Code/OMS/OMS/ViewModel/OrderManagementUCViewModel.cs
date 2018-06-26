using OMS.Model;
using System;
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

        public ICommand ButtonSearchCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand CreateOrderCommand { get; set; }
        public ICommand SaveOrderCommand { get; set; }

        #endregion command

        #region Variable

        public ObservableCollection<Orders> List { get; set; }
        public ObservableCollection<OrderDetail> ListOrderDetail { get; set; }
        public ObservableCollection<Orders> ListTemp { get; set; }
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
                CreatedDate = SelectedItem.CreatedTime;
                SubTotal = SelectedItem.SubPrice;
                ShippingAddress = SelectedItem.ShippingAddress;
                BillingAddress = SelectedItem.BillingAddress;
                CallShip = SelectedItem.CallShip.Equals("Chưa đặt") ? 0 : 1;
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
        private string _SubTotal { get; set; }

        public string SubTotal
        {
            get => _SubTotal;
            set { _SubTotal = value; OnPropertyChanged(); }
        }
        private string _CreatedDate { get; set; }

        public string CreatedDate
        {
            get => _CreatedDate;
            set { _CreatedDate = value; OnPropertyChanged(); }
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
        private string _SearchContent { get; set; }

        public string SearchContent
        {
            get => _SearchContent;
            set { _SearchContent = value; OnPropertyChanged(); }
        }
        #endregion Variable

        #region Method

        public OrderManagementUCViewModel()
        {
            List = new ObservableCollection<Orders>();
            ListTemp = new ObservableCollection<Orders>();
            ListOrderDetail = new ObservableCollection<OrderDetail>();

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

            ButtonSearchCommand = new RelayCommand<ComboBox>(p => true, p =>
            {
                int SelectedIndex = p.SelectedIndex;
                if (List != null)
                    List.Clear();

                if (SearchContent == null)
                {
                    foreach (var item in ListTemp)
                    {
                        List.Add(item);
                    }
                }
                else
                {
                    switch (SelectedIndex)
                    {
                        case 0:
                            try
                            {
                                int id = Convert.ToInt32(SearchContent);
                                FindOrderByID(id);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Mã hóa đơn chỉ bao gồm số!!");
                            }

                            break;
                        case 1:
                            FindOrderByCustomerName();
                            break;
                    }
                }

            });

            CreateOrderCommand = new RelayCommand<object>(p => true, p =>
            {
                //if (List.Count != 0)
                //{
                //    List.Clear();
                //}
                MessageBox.Show(CallShip.ToString() + OrderStatus.ToString());
            });
        }

        public void LoadData(string SelectedValue)
        {
            DBConnect dbConnect = new DBConnect();
            //string query = @"select Orders.id, Customers.Name, datetime(Orders.CreatedTime, 'unixepoch','localtime') as CreatedTime, Orders.Status, Orders.OrderFrom from Orders inner join Customers where Orders.CustomerId = Customers.Id;";
            string query = @"select Orders.Id, Customers.Name,
                            datetime(Orders.CreatedTime, 'unixepoch','localtime') as CreatedTime,
                            Orders.GrandPrice, Orders.SubTotal,
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
                    SubPrice = (string)((DataRow)row).ItemArray[4],
                    Status = (string)((DataRow)row).ItemArray[5],
                    ShippingAddress = (string)((DataRow)row).ItemArray[6],
                    BillingAddress = (string)((DataRow)row).ItemArray[7],
                    CallShip = (string)((DataRow)row).ItemArray[8],
                    PackageWidth = (string)((DataRow)row).ItemArray[9],
                    PackageWeight = (string)((DataRow)row).ItemArray[10],
                    PackageHeight = (string)((DataRow)row).ItemArray[11]
                };
                List.Add(order);
                ListTemp.Add(order);
            }
        }

        public void LoadDataToOrderDetail(string SelectedValue, int OrderID)
        {
            DBConnect dbConnect = new DBConnect();
            string query = @"select temp.id, temp.Name, temp.Quantity
                            from (select OrderDetail.Id, Products.Name, OrderDetail.Quantity, OrderDetail.OrderId
		                    from OrderDetail inner join Products
		                    where OrderDetail.ProductId=Products.id) as temp inner join Orders
                            where temp.OrderID=Orders.Id
                            and Orders.OrderFrom = '" + SelectedValue + "';";
            DataTable dataTable = dbConnect.SelectQuery(query);
            foreach (var row in dataTable.Rows)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    Id = Convert.ToInt32(((DataRow)row).ItemArray[0]),
                    ProductId = new Products { Name = (string)((DataRow)row).ItemArray[1] },
                    Quantity = Convert.ToInt32(((DataRow)row).ItemArray[2])

                };
                ListOrderDetail.Add(orderDetail);
            }
        }

        public void FindOrderByID(int id)
        {
            int temp = 0;

            foreach (var item in ListTemp)
            {
                if (item.Id == id)
                {
                    List.Add(item);
                    temp++;
                }
            }
            if (temp == 0)
            {
                foreach (var item in ListTemp)
                {
                    List.Add(item);
                }
                MessageBox.Show("Không có kết quả cần tìm!");
            }

        }
        public void FindOrderByCustomerName()
        {
            int temp = 0;
            String CustomerName;
            foreach (var item in ListTemp)
            {
                CustomerName = item.Customer.Name.ToUpper();
                if (CustomerName.Contains(SearchContent.ToUpper()))
                {
                    List.Add(item);
                    temp++;
                }
            }
            if (temp == 0)
            {
                foreach (var item in ListTemp)
                {
                    List.Add(item);
                }
                MessageBox.Show("Không có kết quả cần tìm!");
            }
        }
        #endregion Method
    }
}