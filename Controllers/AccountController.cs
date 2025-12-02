using AutoZone.DTOs;
using AutoZone.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="registerDto">The registration details</param>
        /// <returns>A success message if registration is successful</returns>
        /// <response code="200">User registered successfully</response>
        /// <response code="400">Invalid registration data</response>
        [SwaggerOperation(
            Summary = "Register a new user",
            Description = "Creates a new user account with the provided registration details."
        )]
        [SwaggerResponse(200, "User registered successfully")]
        [SwaggerResponse(400, "Invalid registration data")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var response = await _accountService.RegisterAsync(registerDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="loginDto">The login credentials</param>
        /// <returns>A JWT token if authentication is successful</returns>
        /// <response code="200">Login successful</response>
        /// <response code="400">Invalid credentials</response>
        [SwaggerOperation(
            Summary = "Login user",
            Description = "Authenticates a user and returns a JWT token if credentials are correct."
        )]
        [SwaggerResponse(200, "Login successful")]
        [SwaggerResponse(400, "Invalid credentials")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var response = await _accountService.LoginAsync(loginDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

    }
}
