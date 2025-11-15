using System.Security.Claims;
using AutoZone.DTOs.Car;
using AutoZone.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        // ✅ Get all cars
        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var response = await _carService.GetAllCarsAsync();
            if (!response.Success)
                return NotFound(new { message = response.Message });

            return Ok(response.Data);
        }

        // ✅ Get car by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var response = await _carService.GetCarByIdAsync(id);
            if (!response.Success)
                return NotFound(new { message = response.Message });

            return Ok(response.Data);
        }

        // ✅ Create new car
        [Authorize] // المستخدم لازم يكون مسجل
        [HttpPost]
        public async Task<IActionResult> CreateCar([FromBody] CreateCarDTO dto)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var response = await _carService.CreateCarAsync(dto, userId);
            if (!response.Success)
                return BadRequest(new { message = response.Message });

            return CreatedAtAction(nameof(GetCarById), new { id = response.Data.Id }, response.Data);
        }

        // ✅ Update car
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] UpdateCarDTO dto)
        {
            var response = await _carService.UpdateCarAsync(id, dto);
            if (!response.Success)
                return NotFound(new { message = response.Message });

            return NoContent();
        }

        // ✅ Delete car
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var response = await _carService.DeleteCarAsync(id);
            if (!response.Success)
                return NotFound(new { message = response.Message });

            return NoContent();
        }
    }
}
