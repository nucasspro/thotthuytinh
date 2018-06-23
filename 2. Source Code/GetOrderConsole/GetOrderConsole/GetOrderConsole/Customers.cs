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
    }
}