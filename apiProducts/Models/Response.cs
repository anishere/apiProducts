namespace apiProducts.Models
{
    public class Response
    {
        public int StatusCode { get; set; }

        public string? StatusMessage { get; set; }

        public List<Products>? listproducts { get; set; }
    }
}
