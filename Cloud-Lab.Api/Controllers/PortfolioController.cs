using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Cloud_Lab.DataAccess.Database.Repositories;
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
        private readonly IMapper _mapper;

        public PortfolioController(PortfolioRepository portfolioRepository,
            PortfolioStocksRepository portfolioStocksRepository, IMapper mapper)
        {
            _portfolioRepository = portfolioRepository;
            _portfolioStocksRepository = portfolioStocksRepository;
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
            [FromQuery] int count, [FromQuery] int percent)
        {
            return (await _portfolioStocksRepository.AddStock(portfolioId, stockId, count, percent))
                .ToResponseMessage();
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