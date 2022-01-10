using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cloud_Lab.DataAccess.Database.Repositories;
using Cloud_Lab.Entities.DTO;
using Cloud_Lab.Entities.Options;
using Cloud_Lab.Entities.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IOptions<TokenOptions> _tokenOption;
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;

        public AuthController(IOptions<TokenOptions> tokenOption, UserRepository userRepository, IMapper mapper)
        {
            _tokenOption = tokenOption;
            _userRepository = userRepository;
            _mapper = mapper;

        }

        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Registration(UserCredential request)
        {
            var user = _mapper.Map<User>(request);
            var result = await _userRepository.CreateUser(user);
            return result.ToResponseMessage();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserCredential request)
        {
            var user = _mapper.Map<User>(request);
            var result = await _userRepository.GetUser(user);
            if (!result.IsSuccess())
            {
                return result.ToResponseMessage();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, result.Value.Username),
                new(ClaimTypes.NameIdentifier, result.Value.Id.ToString())
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOption.Value.SecretKey)),
                    SecurityAlgorithms.HmacSha256)), new JwtPayload(claims));

            var response = new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return Ok(response);
        }
    }
}