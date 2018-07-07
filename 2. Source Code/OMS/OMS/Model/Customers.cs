using System;

namespace OMS.Model
{
    public class Customers
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }

        #region method

        public bool AddCustomer(string CustomerName, string CustomerPhone, string BillingAddress)
        {
            var dB = new DBConnect();
            string query2 = $"Insert into Customers(Name, Phone, Address, Type) values ('{CustomerName}', '{CustomerPhone}', '{BillingAddress}', 'Khách hàng')";
            try
            {
                dB.ExecuteQuery(query2);
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