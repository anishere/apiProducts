namespace apiProducts.Models
{
    public class Response
    {
        public int StatusCode { get; set; }

        public string? StatusMessage { get; set; }

        public List<ProductsPcLaptop>? listproducts { get; set; }

        public List<InformationCustomer>? listcustomers { get; set; }
    }
}
