using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Cloud_Lab.DataAccess.Database.Repositories;
using Cloud_Lab.Entities;
using Cloud_Lab.Entities.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("portfolio")]
    public class PortfolioController : Controller
    {
        private readonly PortfolioRepository _portfolioRepository;
        private readonly PortfolioStocksRepository _portfolioStocksRepository;
        private readonly StockRepository _stockRepository;
        private readonly IMapper _mapper;

        public PortfolioController(PortfolioRepository portfolioRepository,
            PortfolioStocksRepository portfolioStocksRepository, StockRepository stockRepository, IMapper mapper)
        {
            _portfolioRepository = portfolioRepository;
            _portfolioStocksRepository = portfolioStocksRepository;
            _stockRepository = stockRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePortfolio()
        {
            return (await _portfolioRepository.CreatePortfolio(GetUserId()))
                .ToResponseMessage();
        }

        [HttpPost]
        [Route("stock")]
        public async Task<IActionResult> AddStockToPortfolio([FromQuery] Guid portfolioId, [FromQuery] Guid stockId,
            [FromQuery] int count)
        {
            return (await _portfolioStocksRepository.AddStock(portfolioId, stockId, count))
                .ToResponseMessage();
        }

        [HttpGet]
        [Route("recommendation")]
        public async Task<IActionResult> GetRecommendation([FromQuery] Guid portfolioId)
        {
            var portfolioStocks = await _stockRepository.GetAllStocks(portfolioId);
            if (!portfolioStocks.IsSuccess())
                return portfolioStocks.ToResponseMessage();

            var sumPortfolio = portfolioStocks.Value.Sum(e => e.Price * e.Count);
            var countInstrument = portfolioStocks.Value.Count;
            var normal = sumPortfolio / countInstrument;

            var neededToBalanced = portfolioStocks.Value
                .Where(e => e.Count * e.Price < normal)
                .Distinct()
                .ToList();

            neededToBalanced.ForEach(e =>
                e.Count = (normal / e.Price) - e.Count > 0 ? (normal / e.Price) - e.Count : 1);

            return new OperationResult<List<PortfolioStocks>>(neededToBalanced).ToResponseMessage();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPortfolios()
        {
            return (await _portfolioRepository.GetPortfolios(GetUserId()))
                .ToResponseMessage();
        }

        private Guid GetUserId()
        {
            var userIdString = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdString ?? string.Empty);
        }
    }
}