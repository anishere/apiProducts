namespace apiProducts.Models
{
    public class ProductsTaiNghe
    {
        public int ProductID { get; set; }

        public string? ProductName { get; set; }

        public string? Description { get; set; }

        public string? Brand { get; set; }

        public decimal Discount { get; set; }

        public decimal Price { get; set; }

        public string? Image { get; set; }

        public string? Type { get; set; }

        public string? BaoHanh { get; set; }

        public string? TanSo { get; set; }

        public string? KetNoi { get; set; } 

        public string? KieuKetNoi { get; set; }

        public string? MauSac { get; set; }

        public string? DenLed { get; set; }

        public string? Microphone { get; set; }

        public string? KhoiLuong { get; set ; }

        public DateTime NgayNhap { get; set; }
    }
}
