using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using AutoZone.DTOs;
using AutoZone.Models;
using AutoZone.Services.Interfaces;
using AutoZone.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AutoZone.Services
{
    public class AccountService:IAccountService
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> RegisterAsync(RegisterDTO registerDto)
        {
            // تحقق أن الإيميل مش متسجل قبل كدا
            var existingUser = await _unitOfWork.Users.GetUserByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                //  throw new Exception("Email already registered."); الكود هيتكسر
                return ServiceResponse<string>.FailureResponse("Email already registered.");
            }


            var user = _mapper.Map<User>(registerDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveAsync();

            var token = GenerateJwtToken(user);

            //return new UserResponseDTO
            //{
            //    Id = user.Id,
            //    Email = user.Email,
            //    Name = user.Name,
            //    Role = user.Role.ToString(),
            //    Token = token

            //};
            return ServiceResponse<string>.SuccessResponse(null, "Registration successful.");
        }


        public async Task<ServiceResponse<string>> LoginAsync(LoginDTO loginDto)
        {
            var user = await _unitOfWork.Users.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return ServiceResponse<string>.FailureResponse("Invalid email or password.");

            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return ServiceResponse<string>.FailureResponse("Invalid email or password.");


            var token = GenerateJwtToken(user);


            return ServiceResponse<string>.SuccessResponse(token, "Login successful.");

        }



        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())


            };

            
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? _configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey)) throw new Exception("JWT key not set. Set JWT_KEY env var or Jwt:Key in configuration.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}
