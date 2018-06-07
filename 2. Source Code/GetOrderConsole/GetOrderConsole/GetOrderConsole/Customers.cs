namespace GetOrderConsole
{
    public class Customers
    {
        public Customers()
        {
        }

        public Customers(string name, string phone, string adress, int numberOfPurchasedpe, int quantityPurchased, string type)
        {
            Name = name;
            Phone = phone;
            Adress = adress;
            NumberOfPurchasedpe = numberOfPurchasedpe;
            QuantityPurchased = quantityPurchased;
            Type = type;
        }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public int NumberOfPurchasedpe { get; set; }
        public int QuantityPurchased { get; set; }
        public string Type { get; set; }
    }
}