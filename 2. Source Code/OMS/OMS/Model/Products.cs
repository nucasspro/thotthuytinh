namespace OMS.Model
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Weight { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Length { get; set; }
        public string Price { get; set; }
        public string Image { get; set; }
        public int NumberOfStocks { get; set; }
        public virtual Accounts CreatedBy { get; set; }
    }
}