using System;
using System.Collections.ObjectModel;
using System.Data;

namespace OMS.Model
{
    public class Orders
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string CreatedTime { get; set; }
        public string UpdatedTime { get; set; }
        public string SubPrice { get; set; }
        public string GrandPrice { get; set; }
        public Customers Customer { get; set; }
        public string Status { get; set; }
        public Accounts Account { get; set; }
        public string OrderFrom { get; set; }
        public string Type { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public string CallShip { get; set; }
        public string ShipId { get; set; }
        public string ShipPrice { get; set; }
        public string PackageWidth { get; set; }
        public string PackageHeight { get; set; }
        public string PackageLenght { get; set; }

        #region method

        public DataTable CreateReport(DateTime start, DateTime end)
        {
            var dB = new DBConnect();
            string test = "Select OrderFrom, Id, datetime(UpdatedTime, 'unixepoch','localtime') as UpdatedTime, cast(GrandPrice as integer) as GrandPrice " +
                           "From Orders Where cast (UpdatedTime as integer) > cast (" + ConvertToTimeSpan(start.ToString()) + " as integer) " +
                           "and cast (UpdatedTime as integer) < cast (" + ConvertToTimeSpan(end.ToString()) + " as integer)" +
                           "and Status = 'Đã thanh toán';";
            return dB.SelectQuery(test);
        }

        public ObservableCollection<Orders> LoadData(string SelectedValue)
        {
            var dbConnect = new DBConnect();
            var temp = new ObservableCollection<Orders>();
            string query = @"select Orders.Id, Customers.Name,
                            datetime(Orders.CreatedTime, 'unixepoch','localtime') as CreatedTime,
                            Orders.GrandPrice, Orders.SubTotal,
                            Orders.Status, Orders.ShippingAddress, Orders.BillingAddress, Customers.Phone, Orders.CallShip, Orders.ShipId, Orders.ShipPrice,
                            Orders.PackageWidth, Orders.PackageLenght, Orders.PackageHeight
                            from Orders inner join Customers
                            where Orders.CustomerId = Customers.Id and Orders.OrderFrom = '" + SelectedValue + "';";

            var dataTable = dbConnect.SelectQuery(query);
            foreach (var row in dataTable.Rows)
            {
                var order = new Orders
                {
                    Id = Convert.ToInt32(((DataRow)row).ItemArray[0]),
                    CreatedTime = (string)((DataRow)row).ItemArray[2],
                    GrandPrice = (string)((DataRow)row).ItemArray[3],
                    SubPrice = (string)((DataRow)row).ItemArray[4],
                    Status = (string)((DataRow)row).ItemArray[5],
                    ShippingAddress = (string)((DataRow)row).ItemArray[6],
                    BillingAddress = (string)((DataRow)row).ItemArray[7],
                    CallShip = (string)((DataRow)row).ItemArray[9],
                    ShipId = (string)((DataRow)row).ItemArray[10],
                    ShipPrice = (string)((DataRow)row).ItemArray[11],
                    PackageWidth = (string)((DataRow)row).ItemArray[12],
                    PackageLenght = (string)((DataRow)row).ItemArray[13],
                    PackageHeight = (string)((DataRow)row).ItemArray[14]
                };
                var customer = new Customers
                {
                    Name = (string)((DataRow)row).ItemArray[1],
                    Phone = (string)((DataRow)row).ItemArray[8]
                };
                order.Customer = customer;
                temp.Add(order);
            }
            return temp;
        }

        public string ConvertToTimeSpan(string time)
        {
            var dateTime = DateTime.Parse(time).ToLocalTime();
            var dateTimeOffset = new DateTimeOffset(dateTime);
            return dateTimeOffset.ToUnixTimeSeconds().ToString();
        }

        public int ReturnCustomerID(string CustomerName, string CustomerPhone)
        {
            var dB = new DBConnect();
            string query = $"Select ID From Customers Where Name = '{CustomerName}' and Phone = '{CustomerPhone}';";
            return dB.ExecuteQueryToGetIdAndCount(query);
        }

        public bool CreateOrder(string CreatedDate, string SubTotal, string GrandPrice, string CustomerName, string CustomerPhone, string OrderStatusTemp, string ShippingAddress, string BillingAddress, string CallShipTemp, int ShipPrice, string PackageWidth, string PackageHeight, string PackageLenght, int isVerify)
        {
            var dB = new DBConnect();
            string query1 = $"Insert into Orders(OrderCode, CreatedTime, UpdatedTime, SubTotal, GrandPrice, CustomerID, Status, VerifyBy, OrderFrom, Type, ShippingAddress, BillingAddress, CallShip, ShipPrice, PackageWidth, PackageHeight, PackageLenght) " +
                            $"values ('', '{CreatedDate}', '{CreatedDate}', '{SubTotal}', '{GrandPrice}', {ReturnCustomerID(CustomerName, CustomerPhone)}, '{OrderStatusTemp}', '{isVerify}', 'CreatedByEmployee'," +
                            $"'Bán cho khách', '{ShippingAddress}', '{BillingAddress}', '{CallShipTemp}', '{ShipPrice}', '{PackageWidth}', '{PackageHeight}', '{PackageLenght}');";
            try
            {
                dB.ExecuteQuery(query1);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateOrder(string UpdatedDate, string SubTotal, string GrandPrice, string CustomerName, string CustomerPhone, string OrderStatusTemp, string ShippingAddress, string BillingAddress, string CallShipTemp, string ShipId, int ShipPrice, string PackageWidth, string PackageHeight, string PackageLenght, string SelectedValue, string OrderID, int isVerify)
        {
            var dB = new DBConnect();
            string query1 = $"Update Orders " +
                            $"Set UpdatedTime = '{UpdatedDate}', SubTotal = '{SubTotal}', " +
                            $"GrandPrice = '{GrandPrice}', CustomerID = {ReturnCustomerID(CustomerName, CustomerPhone)}, " +
                            $"Status = '{OrderStatusTemp}', VerifyBy = '{isVerify}', OrderFrom = '{SelectedValue}', " +
                            $"ShippingAddress = '{ShippingAddress}', BillingAddress = '{BillingAddress}', " +
                            $"CallShip = '{CallShipTemp}', ShipId = '{ShipId}', ShipPrice = '{ShipPrice}', PackageWidth = '{PackageWidth}', " +
                            $"PackageHeight = '{PackageHeight}', PackageLenght = '{PackageLenght}' " +
                            $"Where Id = {OrderID}";
            try
            {
                dB.ExecuteQuery(query1);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateShipId(string OrderID, string ShipId, string ShipPrice)
        {
            var dB = new DBConnect();
            string query = $"Update Orders Set ShipId = '{ShipId}', Status = 'Đã đóng gói', CallShip = 'Đã gọi ship', ShipPrice = '{ShipPrice}' Where Id = {OrderID}";
            try
            {
                dB.ExecuteQuery(query);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateCallShip(string OrderID)
        {
            var dB = new DBConnect();
            string query = "Update Orders Set Status = 'Chưa duyệt', CallShip = 'Chưa gọi ship', ShipId = '' Where Id = {OrderID}";
            try
            {
                dB.ExecuteQuery(query);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion method
    }
}