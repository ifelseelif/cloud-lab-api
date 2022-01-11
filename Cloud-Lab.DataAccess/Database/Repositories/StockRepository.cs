using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cloud_Lab.Entities;
using Cloud_Lab.Entities.DTO;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Lab.DataAccess.Database.Repositories
{
    public class StockRepository
    {
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        public StockRepository(IDbContextFactory<ApplicationContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<OperationResult<List<Stock>>> GetAllStocks()
        {
            try
            {
                var context = _contextFactory.CreateDbContext();
                var stocks = await context.Stocks.Where(e => e.Currency == "rub").ToListAsync();
                return stocks.Count == 0
                    ? new OperationResult<List<Stock>>(HttpStatusCode.NotFound, "Not found any stocks")
                    : new OperationResult<List<Stock>>(stocks);

            }
            catch (Exception)
            {
                return new OperationResult<List<Stock>>(HttpStatusCode.InternalServerError, "try again later");
            }
        }

        public async Task<OperationResult<List<Stock>>> GetAllStocks(Guid portfolioId)
        {
            try
            {
                var context = await _contextFactory.CreateDbContextAsync();
                var portfolioStocks = context.PortfolioStocks.Where(e => e.PortfolioId == portfolioId);

                var stocks = await context.Stocks
                    .Join(portfolioStocks, stock => stock.Id,
                        portfolio => portfolio.StockId,
                        (stock, _) => stock).Where(e => e.Currency == "rub").ToListAsync();
                return stocks.Count == 0
                    ? new OperationResult<List<Stock>>(HttpStatusCode.NotFound, "Not found any stocks")
                    : new OperationResult<List<Stock>>(stocks);
            }
            catch (Exception)
            {
                return new OperationResult<List<Stock>>(HttpStatusCode.InternalServerError, "try again later");
            }
        }
    }
}