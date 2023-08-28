namespace apiProducts.Models
{
    public class ProductsKeyboard
    {
        public int ProductID { get; set; }

        public string? ProductName { get; set; }

        public string? Description { get; set; }

        public string? Brand { get; set; }

        public decimal Discount { get; set; }

        public decimal Price { get; set; }

        public string? Image { get; set; }

        public string? KeyBoard { get; set; }

        public string? BaoHanh { get; set; }

        public string? Switch { get; set; }

        public string? MauSac { get; set; }

        public string? KieuKetNoi { get; set; }

        public string? DenLed { get; set; }

        public string? KeTay { get; set; }

        public string? KichThuoc { get; set; }

        public string? Type { get; set; }
        public DateTime NgayNhap { get; set; }
    }
}
