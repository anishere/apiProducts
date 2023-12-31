﻿namespace apiProducts.Models
{
    public class InformationCustomer
    {
        public int ID { get; set;}
        public string? PhoneNumber { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? ListCart { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
