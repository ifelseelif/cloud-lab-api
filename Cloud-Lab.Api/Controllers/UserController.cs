using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Cloud_Lab.DataAccess.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("users")]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(UserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser([FromQuery] string username)
        {
            return (await _userRepository.GetUser(username)).ToResponseMessage();
        }
    }
}