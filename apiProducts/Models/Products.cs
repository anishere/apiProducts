namespace apiProducts.Models
{
    public class Products
    {
        public int ProductID { get; set; }

        public string? ProductName { get; set; }

        public string? Discription { get; set; }

        public string? Detail { get; set; }

        public string? Brand { get; set; }

        public decimal Discount { get; set; }

        public decimal Price { get; set; }

        public string? Image { get; set; }

        public string? Type { get; set; }

        public string? BaoHanh { get; set; }
    }
}
