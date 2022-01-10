using System;
using System.Threading.Tasks;
using AutoMapper;
using Cloud_Lab.DataAccess.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("stocks")]
    public class StockController : Controller
    {
        private readonly StockRepository _stockRepository;
        private readonly IMapper _mapper;

        public StockController(StockRepository stockRepository, IMapper mapper)
        {
            _stockRepository = stockRepository;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetStocks()
        {
            return (await _stockRepository.GetAllStocks()).ToResponseMessage();
        }
        
        [HttpGet]
        [Route("byPortfolio")]
        public async Task<IActionResult> GetStocksByPortfolio([FromQuery] Guid portfolioId)
        {
            return (await _stockRepository.GetAllStocks(portfolioId)).ToResponseMessage();
        }
    }
}