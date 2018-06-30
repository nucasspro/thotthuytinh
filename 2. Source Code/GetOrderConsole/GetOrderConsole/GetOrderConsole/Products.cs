namespace GetOrderConsole
{
    public class Products
    {
        public Products()
        {
        }

        public Products(string name, string description, string weight, string width, string height, string length, string price, string image1, string image2, string image3, int quantity, int createdBy, string status)
        {
            Name = name;
            Description = description;
            Weight = weight;
            Width = width;
            Height = height;
            Length = length;
            Price = price;
            Image1 = image1;
            Image2 = image2;
            Image3 = image3;
            Quantity = quantity;
            CreatedBy = createdBy;
            Status = status;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Weight { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Length { get; set; }
        public string Price { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public int Quantity { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}