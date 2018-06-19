namespace GetOrderConsole
{
    public class Products
    {
        public Products()
        {
        }

        public Products(string name, string weight, string width, string height, string length, string price, int numberOfStocks, int createdBy)
        {
            Name = name;
            Weight = weight;
            Width = width;
            Height = height;
            Length = length;
            Price = price;
            NumberOfStocks = numberOfStocks;
            CreatedBy = createdBy;
        }

        public string Name { get; set; }
        public string Weight { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Length { get; set; }
        public string Price { get; set; }
        public int NumberOfStocks { get; set; }
        public int CreatedBy { get; set; }
    }
}