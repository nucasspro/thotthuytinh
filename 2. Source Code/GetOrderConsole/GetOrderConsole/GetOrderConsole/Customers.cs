namespace GetOrderConsole
{
    public class Customers
    {
        public Customers()
        {
        }

        public Customers(string name, string phone, string address, int numberOfPurchasedpe, int quantityPurchased, string type)
        {
            Name = name;
            Phone = phone;
            Address = address;
            NumberOfPurchasedpe = numberOfPurchasedpe;
            QuantityPurchased = quantityPurchased;
            Type = type;
        }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int NumberOfPurchasedpe { get; set; }
        public int QuantityPurchased { get; set; }
        public string Type { get; set; }
    }
}