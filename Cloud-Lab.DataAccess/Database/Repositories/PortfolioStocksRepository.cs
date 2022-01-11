using System;
using System.Linq;
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
                var elem = context.PortfolioStocks.FirstOrDefault(e =>
                    e.PortfolioId == portfolioId && e.StockId == stockId);
                if (elem != null)
                {
                    elem.Count += count;
                    context.PortfolioStocks.Update(elem);
                    await context.SaveChangesAsync();
                    return new OperationResult();
                }

                context.PortfolioStocks.Add(new PortfolioStocks
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