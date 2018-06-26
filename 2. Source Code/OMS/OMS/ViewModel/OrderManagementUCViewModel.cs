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
                CustomerPhone = SelectedItem.Customer.Phone;
                CallShip = SelectedItem.CallShip.Equals("Chưa gọi ship") ? 0 : 1;
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

        private string _CustomerPhone { get; set; }

        public string CustomerPhone
        {
            get => _CustomerPhone;
            set { _CustomerPhone = value; OnPropertyChanged(); }
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
                        default:
                            FindOrderByCustomerPhone();
                            break;
                    }
                }

            });

            CreateOrderCommand = new RelayCommand<object>(p => true, p =>
            {

                CreateOrder();
                LoadData(SelectedValue);

            });
            SaveOrderCommand = new RelayCommand<object>(p => true, p =>
            {

                UpdateOrder();
                LoadData(SelectedValue);

            });
        }

        public void LoadData(string SelectedValue)
        {
            DBConnect dbConnect = new DBConnect();
            //string query = @"select Orders.id, Customers.Name, datetime(Orders.CreatedTime, 'unixepoch','localtime') as CreatedTime, Orders.Status, Orders.OrderFrom from Orders inner join Customers where Orders.CustomerId = Customers.Id;";
            string query = @"select Orders.Id, Customers.Name,
                            datetime(Orders.CreatedTime, 'unixepoch','localtime') as CreatedTime,
                            Orders.GrandPrice, Orders.SubTotal,
                            Orders.Status, Orders.ShippingAddress, Orders.BillingAddress, Customers.Phone, Orders.CallShip,
                            Orders.PackageWidth, Orders.PackageWeight, Orders.PackageHeight
                            from Orders inner join Customers
                            where Orders.CustomerId = Customers.Id and Orders.OrderFrom = '" + SelectedValue + "';";
            DataTable dataTable = dbConnect.SelectQuery(query);
            foreach (var row in dataTable.Rows)
            {
                Orders order = new Orders
                {
                    Id = Convert.ToInt32(((DataRow)row).ItemArray[0]),
                    Customer = new Customers
                    {
                        Name = (string)((DataRow)row).ItemArray[1],
                        Phone = (string)((DataRow)row).ItemArray[8]
                    },
                    CreatedTime = (string)((DataRow)row).ItemArray[2],
                    GrandPrice = (string)((DataRow)row).ItemArray[3],
                    SubPrice = (string)((DataRow)row).ItemArray[4],
                    Status = (string)((DataRow)row).ItemArray[5],
                    ShippingAddress = (string)((DataRow)row).ItemArray[6],
                    BillingAddress = (string)((DataRow)row).ItemArray[7],
                    CallShip = (string)((DataRow)row).ItemArray[9],
                    PackageWidth = (string)((DataRow)row).ItemArray[10],
                    PackageWeight = (string)((DataRow)row).ItemArray[11],
                    PackageHeight = (string)((DataRow)row).ItemArray[12]
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
            String CustomerNameTemp;
            foreach (var item in ListTemp)
            {
                CustomerNameTemp = item.Customer.Name.ToUpper();
                if (CustomerNameTemp.Contains(SearchContent.ToUpper()))
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
        public void FindOrderByCustomerPhone()
        {
            int temp = 0;
            String CustomerPhoneTemp;
            foreach (var item in ListTemp)
            {
                CustomerPhoneTemp = item.Customer.Phone;
                if (CustomerPhoneTemp.Contains(SearchContent))
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
        public Boolean CheckCustomerExist()
        {
            DBConnect dB = new DBConnect();
            String Query = "select * from Customers where Name='" + CustomerName + "' and Phone= '" + CustomerPhone + "' limit 1;";
            if (dB.ExecuteQueryToGetIdAndCount(Query) == 0)
                return false;
            return true;
        }
        public int ReturnCustomerID(String CustomerName, String CustomerPhone)
        {
            DBConnect dB = new DBConnect();
            String Query = "select ID from Customers where Name='" + CustomerName + "' and Phone='" + CustomerPhone + "';";
            return dB.ExecuteQueryToGetIdAndCount(Query);
        }
        public void CreateOrder()
        {
            DateTime CreatedDate = DateTime.Now;
            DBConnect dB = new DBConnect();
            String Query1, Query2, CallShipTemp, OrderStatusTemp;

            //check field CustomerName, CustomerPhone, Shipping Adress, Billing Adress not null
            if (CustomerName == null || CustomerPhone == null || ShippingAddress == null || BillingAddress == null)
            {
                MessageBox.Show("Bạn phải nhập đầy đủ các trường *");
                return;
            }

            //set value  to CallShipTemp and OrderStatusTemp
            if (CallShip == 0)
                CallShipTemp = "Chưa gọi ship";
            else
                CallShipTemp = "Đã gọi ship";
            if (OrderStatus == 0)
                OrderStatusTemp = "Chưa duyệt";
            else
                OrderStatusTemp = "Đã duyệt";

            if (!CheckCustomerExist())
            {
                Query2 = $"insert into Customers(Name, Phone, Address, Type) " +
                       $"values ('{CustomerName}', '{CustomerPhone}', '{BillingAddress}', 'Khách hàng')";
                try
                {
                    dB.ExecuteQuery(Query2);

                }
                catch (Exception e)
                {
                    MessageBox.Show("Có lỗi phát sinh khi thêm khách hàng! Lỗi: " + e);
                }
            }
            Query1 = $"insert into Orders(OrderCode, CreatedTime, UpdatedTime, SubTotal, GrandPrice, CustomerID, Status, VerifyBy, OrderFrom, Type, ShippingAddress, BillingAddress, CallShip, PackageWidth, PackageHeight, PackageWeight) " +
                    $"values ('', '{CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")}', '{CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")}', '{SubTotal}', '{GrandPrice}', "+ReturnCustomerID(CustomerName,CustomerPhone)+", '" + OrderStatusTemp + "','','CreatedByEmployee'," +
                    $"'Bán cho khách','" + ShippingAddress + "','" + BillingAddress + "', '" + CallShipTemp + "','" + PackageWidth + "','" + PackageHeight + "','" + PackageWeight + "');";
            //MessageBox.Show(Query1);
            try
            {
                dB.ExecuteQuery(Query1);
                MessageBox.Show("Thêm hóa đơn thành công! ");
                List.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show("Có lỗi phát sinh khi thêm đơn hàng! Lỗi: " + e);
            }
            List.Clear();
        }
        public void UpdateOrder()
        {
            DateTime UpdatedDate = DateTime.Now;
            DBConnect dB = new DBConnect();
            String Query1, Query2, CallShipTemp, OrderStatusTemp;

            //check field CustomerName, CustomerPhone, Shipping Adress, Billing Adress not null
            if (CustomerName == null || CustomerPhone == null || ShippingAddress == null || BillingAddress == null)
            {
                MessageBox.Show("Bạn phải nhập đầy đủ các trường *");
                return;
            }

            //set value  to CallShipTemp and OrderStatusTemp
            if (CallShip == 0)
                CallShipTemp = "Chưa gọi ship";
            else
                CallShipTemp = "Đã gọi ship";
            if (OrderStatus == 0)
                OrderStatusTemp = "Chưa duyệt";
            else
                OrderStatusTemp = "Đã duyệt";

            if (!CheckCustomerExist())
            {
                Query2 = $"insert into Customers(Name, Phone, Address, Type) " +
                       $"values ('{CustomerName}', '{CustomerPhone}', '{BillingAddress}', 'Khách hàng')";
                try
                {
                    dB.ExecuteQuery(Query2);

                }
                catch (Exception e)
                {
                    MessageBox.Show("Có lỗi phát sinh khi thêm khách hàng! Lỗi: " + e);
                }
            }
            Query1 = $"update Orders " +
                        $"set UpdatedTime='{UpdatedDate.ToString("yyyy-MM-dd HH:mm:ss")}', SubTotal='{SubTotal}', " +
                        $"GrandPrice='{GrandPrice}', CustomerID=" + ReturnCustomerID(CustomerName, CustomerPhone) + ", " +
                        "Status='" + OrderStatusTemp + "', VerifyBy='', OrderFrom='" + SelectedValue + "', " +
                        "ShippingAddress='" + ShippingAddress + "', BillingAddress='" + BillingAddress + "', " +
                        "CallShip='" + CallShipTemp + "', PackageWidth='" + PackageWidth + "', PackageHeight='" + PackageHeight + "', " +
                        "PackageWeight='" + PackageWeight + "' " +
                        "where Id="+OrderID+"";
            MessageBox.Show(Query1);
            try
            {
                dB.ExecuteQuery(Query1);
                MessageBox.Show("Chỉnh sửa thành công! ");
                List.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show("Có lỗi phát sinh khi sửa đơn hàng! Lỗi: " + e);
            }
            List.Clear();
        }
        #endregion Method
    }
}