using System;

namespace GetOrderConsole
{
    public class Customers
    {
        public Customers()
        {
        }

        public Customers(string name, string phone, string address, string type)
        {
            Name = name;
            Phone = phone;
            Address = address;
            Type = type;
        }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }

        public int CheckCustomerExists(string phone)
        {
            DbConnect dbConnect = new DbConnect();
            string query = $"select count(id) from Customers where Customers.Phone = '{phone}';";
            return dbConnect.GetIdAndCountId(query);
        }

        public int GetCustomerIdFromDb(string phone)
        {
            DbConnect dbConnect = new DbConnect();
            string query = $"select Id from Customers where Customers.Phone = '{phone}' limit 1;";
            return dbConnect.GetIdAndCountId(query);
        }

        public void InsertCustomersToDb(Customers customer)
        {
            if (CheckCustomerExists(customer.Phone) >= 1)
            {
                return;
            }
            try
            {
                DbConnect dbConnect = new DbConnect();
                string query = "insert into Customers (Name, Phone, Address, Type) " +
                               $"VALUES('{customer.Name}', '{customer.Phone}', '{customer.Address}', '{customer.Type}');";
                dbConnect.ExecuteQuery(query);
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi khi insert customers vao db" + e);
            }
        }
    }
}