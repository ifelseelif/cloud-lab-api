using System;
using System.Net;
using System.Threading.Tasks;
using Cloud_Lab.Entities;
using Cloud_Lab.Entities.DTO;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Lab.DataAccess.Database.Repositories
{
    public class PortfolioStocksRepository
    {
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        public PortfolioStocksRepository(IDbContextFactory<ApplicationContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<OperationResult> AddStock(Guid portfolioId, Guid stockId, int count)
        {
            try
            {
                var context = await _contextFactory.CreateDbContextAsync();
                context.Add(new PortfolioStocks
                {
                    Id = Guid.NewGuid(),
                    StockId = stockId,
                    PortfolioId = portfolioId,
                    Count = count
                });
                await context.SaveChangesAsync();
                return new OperationResult();
            }
            catch (Exception e)
            {
                return new OperationResult(HttpStatusCode.InternalServerError, "Try again later");
            }
        }
    }
}