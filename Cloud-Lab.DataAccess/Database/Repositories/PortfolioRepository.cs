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
    public class PortfolioRepository
    {
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        public PortfolioRepository(IDbContextFactory<ApplicationContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<OperationResult<Guid>> CreatePortfolio(Guid userId)
        {
            try
            {
                var context = await _contextFactory.CreateDbContextAsync();

                var portfolio = new Portfolio
                {
                    Id = Guid.NewGuid(),
                    UserId = userId
                };

                context.Portfolios.Add(portfolio);
                await context.SaveChangesAsync();
                return new OperationResult<Guid>(portfolio.Id);
            }
            catch (Exception)
            {
                return new OperationResult<Guid>(HttpStatusCode.InternalServerError, "Try again later");
            }
        }

        public async Task<OperationResult<List<Guid>>> GetPortfolios(Guid userId)
        {
            try
            {
                var context = await _contextFactory.CreateDbContextAsync();
                var userPortfolios = await context.Portfolios.Where(portfolio => portfolio.UserId == userId)
                    .Select(elem => elem.Id)
                    .ToListAsync();

                return userPortfolios.Count == 0
                    ? new OperationResult<List<Guid>>(HttpStatusCode.NotFound,
                        "Not found portfolio for this user")
                    : new OperationResult<List<Guid>>(userPortfolios);
            }
            catch (Exception)
            {
                return new OperationResult<List<Guid>>(HttpStatusCode.InternalServerError, "Try again later");

            }
        }
    }
}