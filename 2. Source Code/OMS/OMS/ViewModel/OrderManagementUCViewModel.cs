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

        public ICommand ButtonSearchCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand CreateOrderCommand { get; set; }
        public ICommand SaveOrderCommand { get; set; }

        public ICommand OrderDetailCommand { get; set; }
        public ICommand DeleteProductFromOrderCommand { get; set; }
        public ICommand AddProductToOrderCommand { get; set; }
        public ICommand UpdateProductToOrderCommand { get; set; }
        public ICommand SelectionChangedCallShipCommand { get; set; }

        #endregion command

        #region Variable

        public ObservableCollection<Orders> List { get; set; }
        public ObservableCollection<OrderDetail> ListOrderDetail { get; set; }
        public ObservableCollection<Orders> ListTemp { get; set; }
        public ObservableCollection<Products> ListProduct { get; set; }

        public List<String> ListProductName { get; set; }
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

        private bool _isEnabledCallShip { get; set; }

        public bool isEnabledCallShip
        {
            get => _isEnabledCallShip;
            set { _isEnabledCallShip = value; OnPropertyChanged(); }
        }

        private bool _isEnabledCancelShip { get; set; }

        public bool isEnabledCancelShip
        {
            get => _isEnabledCancelShip;
            set { _isEnabledCancelShip = value; OnPropertyChanged(); }
        }

        private bool _AddOrderDetailButtonEnabled { get; set; }

        public bool AddOrderDetailButtonEnabled
        {
            get => _AddOrderDetailButtonEnabled;
            set { _AddOrderDetailButtonEnabled = value; OnPropertyChanged(); }
        }

        private bool _UpdateOrderDetailButtonEnabled { get; set; }

        public bool UpdateOrderDetailButtonEnabled
        {
            get => _UpdateOrderDetailButtonEnabled;
            set { _UpdateOrderDetailButtonEnabled = value; OnPropertyChanged(); }
        }

        private String _ProductQuantity { get; set; }

        public String ProductQuantity
        {
            get => _ProductQuantity;
            set { _ProductQuantity = value; OnPropertyChanged(); }
        }

        private String _ComboboxProductListSelectedValue { get; set; }

        public String ComboboxProductListSelectedValue
        {
            get => _ComboboxProductListSelectedValue;
            set { _ComboboxProductListSelectedValue = value; OnPropertyChanged(); }
        }

        private int _OrderDetailId { get; set; }

        public int OrderDetailId
        {
            get => _OrderDetailId;
            set { _OrderDetailId = value; OnPropertyChanged(); }
        }

        public OrderDetail _OrderDtailSelectedItem { get; set; }

        public OrderDetail OrderDtailSelectedItem
        {
            get => _OrderDtailSelectedItem;
            set
            {
                _OrderDtailSelectedItem = value;
                OnPropertyChanged();
                try
                {
                    OrderDetailId = OrderDtailSelectedItem.Id;
                    ComboboxProductListSelectedValue = OrderDtailSelectedItem.Product.Name;
                    ProductQuantity = OrderDtailSelectedItem.Quantity.ToString();
                }
                catch (Exception e)
                {
                }
            }
        }

        #endregion Variable

        #region Method

        public OrderManagementUCViewModel()
        {
            List = new ObservableCollection<Orders>();
            ListTemp = new ObservableCollection<Orders>();
            ListOrderDetail = new ObservableCollection<OrderDetail>();
            ListProduct = new ObservableCollection<Products>();

            // ReSharper disable once ComplexConditionExpression
            SelectionChangedCommand = new RelayCommand<ComboBox>(p => true, p =>
            {
                if (List.Count != 0)
                {
                    List.Clear();
                    ListProduct.Clear();
                }
                ComboBoxItem comboBox = (ComboBoxItem)p.SelectedItem;
                SelectedValue = comboBox.Content.ToString();
                LoadData(SelectedValue);
                LoadProduct();
            });

            // ReSharper disable once ComplexConditionExpression
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
                if (MessageBox.Show("Bạn có muốn lưu?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    UpdateOrder();
                    LoadData(SelectedValue);
                }
            });

            // ReSharper disable once ComplexConditionExpression
            SelectionChangedCallShipCommand = new RelayCommand<ComboBox>(p => true, p =>
            {
                int temp = p.SelectedIndex;

                if (temp == 0)
                {
                    isEnabledCallShip = true;
                    isEnabledCancelShip = false;
                }
                else
                {
                    isEnabledCallShip = false;
                    isEnabledCancelShip = true;
                }
            });

            // ReSharper disable once ComplexConditionExpression
            OrderDetailCommand = new RelayCommand<object>(p => true, p =>
            {
                if (ListOrderDetail != null)
                    ListOrderDetail.Clear();
                LoadDataToOrderDetail(SelectedValue, OrderID);

                AddOrderDetailButtonEnabled = true;
                UpdateOrderDetailButtonEnabled = true;

                if (CallShip == 0)
                {
                    isEnabledCallShip = true;
                    isEnabledCancelShip = false;
                }
                else
                {
                    isEnabledCallShip = false;
                    isEnabledCancelShip = true;
                }
            });

            // ReSharper disable once ComplexConditionExpression
            AddProductToOrderCommand = new RelayCommand<object>(p => true, p =>
            {
                DBConnect dB = new DBConnect();
                string ProductIDTemp = null;
                int ProductQuantityTemp = 0;
                int ProductQuantityStock = 0;
                int OrderIDTemp = Convert.ToInt32(OrderID);
                string query1, query2;
                if (ComboboxProductListSelectedValue == null || ProductQuantity == null)
                {
                    MessageBox.Show("Các trường * không được bỏ trống!");
                }
                else
                {
                    try
                    {
                        ProductQuantityTemp = Convert.ToInt32(ProductQuantity);
                        //Kiểm tra sản phẩm đã tồn tại trong hóa đơn chưa
                        foreach (var item in ListOrderDetail)
                        {
                            if (item.Product.Name.Contains(ComboboxProductListSelectedValue))
                            {
                                MessageBox.Show("Sản phẩm đã tồn tại trong hóa đơn!");
                                return;
                            }
                        }
                        //Lấy id sản phẩm
                        foreach (var item in ListProduct)
                        {
                            if (item.Name.Contains(ComboboxProductListSelectedValue))
                            {
                                ProductIDTemp = item.Id;
                                ProductQuantityStock = item.Quantity;
                            }
                        }
                        //Kiểm tra số lượng bán có nhiều hơn số lượng tồn không?
                        if (CheckProductQuantity(ProductQuantityTemp, ProductQuantityStock))
                        {
                            query1 = $"insert into OrderDetail (OrderID, ProductID, Quantity)" +
                                     $" values (" + OrderIDTemp + ",'" + ProductIDTemp + "'," + ProductQuantityTemp + ");";
                            query2 = $"Update Products " +
                                     $"Set Quantity = {ProductQuantityStock - ProductQuantityTemp} " +
                                    $"where Id = '{ProductIDTemp}';";
                            MessageBox.Show(query1);
                            MessageBox.Show(query2);
                            try
                            {
                                dB.ExecuteQuery(query1);
                                dB.ExecuteQuery(query2);
                                MessageBox.Show("Đã thêm thành công!");
                                ListOrderDetail.Clear();
                                LoadDataToOrderDetail(SelectedValue, OrderID);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Xảy ra lỗi khi thêm sản phẩm vào hóa đơn! Lỗi: " + e);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Quantity phải là số!");
                        return;
                    }
                }
            });

            // ReSharper disable once ComplexConditionExpression
            DeleteProductFromOrderCommand = new RelayCommand<object>(p => true, p =>
            {
                DBConnect dB = new DBConnect();
                string ProductIDTemp = null;
                int ProductQuantityTemp = 0;
                int ProductQuantityStock = 0;
                if (OrderDetailId == 0)
                {
                    MessageBox.Show("Bạn hãy chọn sản phẩm cần xóa!");
                    return;
                }
                ProductQuantityTemp = Convert.ToInt32(ProductQuantity);
                //Lấy id sản phẩm
                foreach (var item in ListProduct)
                {
                    if (item.Name.Contains(ComboboxProductListSelectedValue))
                    {
                        ProductIDTemp = item.Id;
                        ProductQuantityStock = item.Quantity;
                    }
                }
                string query1 = $"delete from OrderDetail where Id =" + OrderDetailId + "";
                string query2 = $"Update Products " +
                                $"Set Quantity = {ProductQuantityStock + ProductQuantityTemp} " +
                                $"where Id = '{ProductIDTemp}';";
                try
                {
                    if (MessageBox.Show("Bạn có muốn xóa chi tiết có Id = " + OrderDetailId + " ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        dB.ExecuteQuery(query1);
                        dB.ExecuteQuery(query2);
                        MessageBox.Show("Đã xóa thành công!");
                        ListOrderDetail.Clear();
                        LoadDataToOrderDetail(SelectedValue, OrderID);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi xóa chi tiết có Id = " + OrderDetailId + "! Lỗi: " + e);
                }
            });

            // ReSharper disable once ComplexConditionExpression
            UpdateProductToOrderCommand = new RelayCommand<object>(p => true, p =>
            {
                DBConnect dB = new DBConnect();
                string ProductIDTemp = null;
                int ProductQuantityAfter = 0;
                int ProductQuantityBefore = 0;
                int ProductQuantityStock = 0;
                int temp = 0;
                int OrderIDTemp = Convert.ToInt32(OrderID);
                string query1, query2;
                if (ComboboxProductListSelectedValue == null || ProductQuantity == null)
                {
                    MessageBox.Show("Các trường * không được bỏ trống!");
                }
                else
                {
                    try
                    {
                        //Kiểm tra sản phẩm đã tồn tại trong hóa đơn chưa
                        foreach (var item in ListOrderDetail)
                        {
                            if (item.Product.Name.Contains(ComboboxProductListSelectedValue))
                            {
                                temp++;
                            }
                            if (temp > 1)
                            {
                                MessageBox.Show("Sản phẩm đã tồn tại trong hóa đơn!");
                                return;
                            }
                        }
                        //Lấy số lượng ban đầu trước khi sửa
                        foreach (var item in ListOrderDetail)
                        {
                            if (item.Product.Name.Contains(ComboboxProductListSelectedValue))
                            {
                                ProductQuantityBefore = item.Quantity;
                            }
                        }
                        //Lấy id sản phẩm
                        foreach (var item in ListProduct)
                        {
                            if (item.Name.Contains(ComboboxProductListSelectedValue))
                            {
                                ProductIDTemp = item.Id;
                                ProductQuantityStock = item.Quantity;
                                continue;
                            }
                        }
                        //Lấy số lượng sau khi đã chỉnh sửa.
                        ProductQuantityAfter = Convert.ToInt32(ProductQuantity);
                        //Kiểm tra số lượng bán có nhiều hơn số lượng tồn không?
                        if (CheckProductQuantity(ProductQuantityAfter, ProductQuantityStock))
                        {
                            query1 = $"update OrderDetail" +
                                    $" set ProductID = '" + ProductIDTemp + "',Quantity= " + ProductQuantityAfter + "" +
                                    $" where Id =" + OrderDetailId + ";";
                            query2 = $"Update Products " +
                                    $"Set Quantity = {ProductQuantityStock - (ProductQuantityAfter - ProductQuantityBefore)} " +
                                    $"where Id = '{ProductIDTemp}';";
                            //MessageBox.Show(Query2);
                            try
                            {
                                if (MessageBox.Show("Bạn có muốn lưu?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                {
                                    dB.ExecuteQuery(query1);
                                    dB.ExecuteQuery(query2);
                                    MessageBox.Show("Đã lưu thành công!");
                                    ListOrderDetail.Clear();
                                    LoadDataToOrderDetail(SelectedValue, OrderID);
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Xảy ra lỗi khi thêm sản phẩm vào hóa đơn! Lỗi: " + e);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Quantity phải là số!");
                        return;
                    }
                }
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
                Customers customer = new Customers
                {
                    Name = (string)((DataRow)row).ItemArray[1],
                    Phone = (string)((DataRow)row).ItemArray[8]
                };
                order.Customer = customer;
                List.Add(order);
                ListTemp.Add(order);
            }
        }

        public void LoadDataToOrderDetail(string SelectedValue, string OrderID)
        {
            int temp = Convert.ToInt32(OrderID);
            DBConnect dbConnect = new DBConnect();
            string query = @"select temp.Id, temp.Name, temp.Quantity
                            from (select OrderDetail.Id, Products.Name, OrderDetail.Quantity, OrderDetail.OrderId
		                    from OrderDetail inner join Products
		                    where OrderDetail.ProductId=Products.id) as temp inner join Orders
                            where temp.OrderID=Orders.Id
                            and Orders.OrderFrom = '" + SelectedValue + "' " +
                            "and Orders.Id=" + temp + ";";
            DataTable dataTable = dbConnect.SelectQuery(query);
            foreach (var row in dataTable.Rows)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    Id = Convert.ToInt32(((DataRow)row).ItemArray[0]),
                    Product = new Products { Name = (string)((DataRow)row).ItemArray[1] },
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
                if (item.Id != id)
                    continue;
                List.Add(item);
                temp++;
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
            string CustomerNameTemp;
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
            string CustomerPhoneTemp;
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
            string query = "select * from Customers where Name='" + CustomerName + "' and Phone= '" + CustomerPhone + "' limit 1;";
            if (dB.ExecuteQueryToGetIdAndCount(query) == 0)
                return false;
            return true;
        }

        public int ReturnCustomerID(String CustomerName, String CustomerPhone)
        {
            DBConnect dB = new DBConnect();
            string query = "select ID from Customers where Name='" + CustomerName + "' and Phone='" + CustomerPhone + "';";
            return dB.ExecuteQueryToGetIdAndCount(query);
        }

        public void CreateOrder()
        {
            string CreatedDate = ConvertToTimeSpan(DateTime.Now.ToLocalTime().ToString());
            DBConnect dB = new DBConnect();
            string query1, query2, CallShipTemp, OrderStatusTemp;

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
                query2 = $"insert into Customers(Name, Phone, Address, Type) " +
                       $"values ('{CustomerName}', '{CustomerPhone}', '{BillingAddress}', 'Khách hàng')";
                try
                {
                    dB.ExecuteQuery(query2);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Có lỗi phát sinh khi thêm khách hàng! Lỗi: " + e);
                }
            }
            query1 = $"insert into Orders(OrderCode, CreatedTime, UpdatedTime, SubTotal, GrandPrice, CustomerID, Status, VerifyBy, OrderFrom, Type, ShippingAddress, BillingAddress, CallShip, PackageWidth, PackageHeight, PackageWeight) " +
                    $"values ('', '{CreatedDate}', '{CreatedDate}', '{SubTotal}', '{GrandPrice}', " + ReturnCustomerID(CustomerName, CustomerPhone) + ", '" + OrderStatusTemp + "','','CreatedByEmployee'," +
                    $"'Bán cho khách','" + ShippingAddress + "','" + BillingAddress + "', '" + CallShipTemp + "','" + PackageWidth + "','" + PackageHeight + "','" + PackageWeight + "');";
            //MessageBox.Show(Query1);
            try
            {
                dB.ExecuteQuery(query1);
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
            string UpdatedDate = ConvertToTimeSpan(DateTime.Now.ToLocalTime().ToString());
            DBConnect dB = new DBConnect();
            string query1, query2, CallShipTemp, OrderStatusTemp;

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
                query2 = $"insert into Customers(Name, Phone, Address, Type) " +
                       $"values ('{CustomerName}', '{CustomerPhone}', '{BillingAddress}', 'Khách hàng')";
                try
                {
                    dB.ExecuteQuery(query2);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Có lỗi phát sinh khi thêm khách hàng! Lỗi: " + e);
                }
            }
            query1 = $"update Orders " +
                        $"set UpdatedTime='{UpdatedDate}', SubTotal='{SubTotal}', " +
                        $"GrandPrice='{GrandPrice}', CustomerID=" + ReturnCustomerID(CustomerName, CustomerPhone) + ", " +
                        "Status='" + OrderStatusTemp + "', VerifyBy='', OrderFrom='" + SelectedValue + "', " +
                        "ShippingAddress='" + ShippingAddress + "', BillingAddress='" + BillingAddress + "', " +
                        "CallShip='" + CallShipTemp + "', PackageWidth='" + PackageWidth + "', PackageHeight='" + PackageHeight + "', " +
                        "PackageWeight='" + PackageWeight + "' " +
                        "where Id=" + OrderID + "";
            try
            {
                dB.ExecuteQuery(query1);
                MessageBox.Show("Chỉnh sửa thành công! ");
                List.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show("Có lỗi phát sinh khi sửa đơn hàng! Lỗi: " + e);
            }
            List.Clear();
        }

        public string ConvertToTimeSpan(string time)
        {
            DateTime dateTime = DateTime.Parse(time).ToLocalTime();
            var dateTimeOffset = new DateTimeOffset(dateTime);
            return dateTimeOffset.ToUnixTimeSeconds().ToString();
        }

        public void LoadProduct()
        {
            DBConnect dbConnect = new DBConnect();
            const string query = @"select * from Products;";
            DataTable dataTable = dbConnect.SelectQuery(query);
            foreach (var row in dataTable.Rows)
            {
                Products product = new Products
                {
                    Id = (string)((DataRow)row).ItemArray[0],
                    Name = (string)((DataRow)row).ItemArray[1],
                    Weight = (string)((DataRow)row).ItemArray[2],
                    Width = (string)((DataRow)row).ItemArray[3],
                    Height = (string)((DataRow)row).ItemArray[4],
                    Length = (string)((DataRow)row).ItemArray[5],
                    Price = (string)((DataRow)row).ItemArray[6],
                    Image1 = (string)((DataRow)row).ItemArray[7],
                    Image2 = (string)((DataRow)row).ItemArray[8],
                    Image3 = (string)((DataRow)row).ItemArray[9],
                    Quantity = Convert.ToInt32(((DataRow)row).ItemArray[10]),
                    CreatedBy = new Accounts { Id = Convert.ToInt32(((DataRow)row).ItemArray[11]) }
                };
                ListProduct.Add(product);
            }
        }

        public bool CheckProductQuantity(int temp, int stock)
        {
            if (temp > stock)
            {
                MessageBox.Show("Số lượng không đủ!");
                return false;
            }
            return true;
        }

        #endregion Method
    }
}