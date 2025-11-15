using AutoZone.DTOs;
using AutoZone.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoZone.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
       
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var response = await _accountService.RegisterAsync(registerDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("login")]
       
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var response = await _accountService.LoginAsync(loginDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

    }
}
