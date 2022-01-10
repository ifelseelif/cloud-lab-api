﻿using System;

namespace Cloud_Lab.Entities.DTO
{
    public class PortfolioStocks
    {
        public Guid Id { get; set; }
        public Guid PortfolioId { get; set; }
        public Guid StockId { get; set; }
        public int Percent { get; set; }

        public Portfolio Portfolio { get; set; }
        public Stock Stock { get; set; }
    }
}