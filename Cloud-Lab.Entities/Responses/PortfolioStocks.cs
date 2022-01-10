using System;

namespace Cloud_Lab.Entities.Responses
{
    public class PortfolioStocks
    {
        public string Figi { get; set; }
        public string Ticker { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public string CountryOfRiskName { get; set; }
        public string Sector { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public int Percent { get; set; }
    }
}