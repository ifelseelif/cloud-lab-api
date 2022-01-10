using System;

namespace Cloud_Lab.Entities.DTO
{
    public class Stock
    {
        public Guid Id { get; set; }
        public string Figi { get; set; }
        public string Ticker { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public string CountryOfRisk { get; set; }
        public string CountryOfRiskName { get; set; }
        public string Sector { get; set; }
        public DateTime LastUpdate { get; set; }
        public int Price { get; set; }
    }
}